using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Services;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.Presentacion.Services
{
    class SueldoService
    {
        private SueldoRepository _sueldoRepository;
        private PagoService _pagoService;
        private ViajeService _viajeService;
        private CamionService _camionService;
        private ChoferService _choferService;
        private Viaje _viajeDTO;


        public SueldoService(SueldoRepository pagosR, PagoService pagoS)
        {
            this._sueldoRepository = pagosR ?? throw new ArgumentNullException(nameof(pagosR));
            this._pagoService = pagoS ?? throw new ArgumentNullException(nameof(pagoS));
            CamionRepository cr = new CamionRepository();
            this._camionService = new CamionService(cr);
            ViajeRepository vr = new ViajeRepository();
            ClienteRepository clr = new ClienteRepository();
            ClienteService cs = new ClienteService(clr);
            ChoferRepository chr = new ChoferRepository();
            this._choferService = new ChoferService(chr);
            this._viajeService = new ViajeService(vr, _camionService, cs, _choferService, pagoS);
        }


        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._sueldoRepository.ProbarConexionAsync();
            return result;
        }

        public async Task<Result<List<SueldoDTO>>> ObtenerTodosAsync(string? patenteCamion, string? nombreChofer)
        {
            if (string.IsNullOrEmpty(patenteCamion) && string.IsNullOrEmpty(nombreChofer))
                return Result<List<SueldoDTO>>.Failure("No se proporcionó la patente del camión ni un chofer");

            int idCamion = -1;
            if (patenteCamion != null)
            {
                var camion = await this._camionService.ObtenerPorPatenteAsync(patenteCamion.ToUpper());

                if (!camion.IsSuccess)
                    return Result<List<SueldoDTO>>.Failure("No se encontró el camión con la patente proporcionada.");
                Console.WriteLine("sobrevivimos a obtener camion");
                idCamion = camion.Value.Id;
            }

            int? idChofer = -1;

            if (nombreChofer != null)
            {
                var chofer = await this._choferService.ObtenerPorNombreAsync(nombreChofer.ToUpper());
                if (!chofer.IsSuccess)
                    return Result<List<SueldoDTO>>.Failure("No se encontró el chofer con el nombre proporcionado.");
                idChofer = chofer.Value.Id;
            }
            List<SueldoDTO>? sueldos = await this._sueldoRepository.ObtenerTodosAsync(idCamion, idChofer);


            if (sueldos != null)
                return Result<List<SueldoDTO>>.Success(sueldos);
            return Result<List<SueldoDTO>>.Failure("Hubo un problema al obtener los sueldos");
        }

        internal async Task<Result<bool>> EliminarAsync(int sueldoId)
        {
            if (sueldoId < 0) return Result<bool>.Failure(MensajeError.IdInvalido(sueldoId));

            SueldoDTO? sueldo = await _sueldoRepository.ObtenerPorId(sueldoId);

            if (sueldo == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(sueldoId)));
            if (sueldo.Id_Chofer == null)
            {
                return Result<bool>.Failure("No se pudo eliminar el pago ya que no se encontró el id del pago/chofer");
            }
            int id_chofer = (int)sueldo.Id_Chofer;

            await _pagoService.ModificarEstado(id_chofer, sueldo.PagadoDesde, sueldo.PagadoHasta, null, false);

            bool response = await _sueldoRepository.EliminarAsync(sueldoId);
            if (response)
            {
                return Result<bool>.Success(response);
            }
            return Result<bool>.Failure("Hubo un problema al eliminar el sueldo");
        }

        public async Task<Result<int>> CrearAsync(string nombre_chofer, DateOnly pagoDesde, DateOnly pagoHasta, DateOnly? fecha_pagado, string patenteCamion)
        {
            Result<Chofer?> c = await this._choferService.ObtenerPorNombreAsync(nombre_chofer);
            if (!c.IsSuccess)
            {
                return Result<int>.Failure("Hubo un problema al obtener el chofer con ese nombre, chequee los datos ingresados");
            }

            if (c.Value == null)
            {
                return Result<int>.Failure("error, no se pudo insertar el sueldo ya que no se encontró al chofer");
            }
            float monto = await calculadorSueldo(c.Value.Id, pagoDesde, pagoHasta);

            if (monto <= 0)
            {
                return Result<int>.Failure("No se pudo calcular el sueldo ya que no hay pagos pendientes");
            }

            ValidadorSueldo validador = new ValidadorSueldo(monto, c.Value.Id, pagoDesde, pagoHasta, pagoHasta);

            Result<bool> resultadoValidacion = validador.ValidarCompleto();

            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);

           

          
                Result<Camion> ca = await this._camionService.ObtenerPorPatenteAsync(patenteCamion);
                if (!ca.IsSuccess)
                {
                    return Result<int>.Failure("No existe camión con esa patente, revise los datos ingresados");
                }
                int idCamion = ca.Value.Id;
            

            try
            {

                int idSueldo = await _sueldoRepository.InsertarAsync(monto, c.Value.Id, pagoDesde, pagoHasta, fecha_pagado, idCamion);
                if (idSueldo < 0)
                    return Result<int>.Failure("No se pudo crear el sueldo en services");
                await _pagoService.ModificarEstado(c.Value.Id, pagoDesde, pagoHasta, idSueldo);
                return Result<int>.Success(idSueldo);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Hubo un error al crear el cheque");
            }
        }

        private async Task<float> calculadorSueldo(int id_chofer, DateOnly calcularDesde, DateOnly calcularHasta)
        {

            float sueldo = await _pagoService.ObtenerSueldoCalculado(id_chofer, calcularDesde, calcularHasta);

            return sueldo;
        }

        public async Task<Result<SueldoDTO>> marcarPagado(int id)
        {
            if (id < 0)
                return Result<SueldoDTO>.Failure("id del sueldo deseado invalido");

            SueldoDTO? sueldo = await _sueldoRepository.ObtenerPorId(id);
            if (sueldo == null)
                return Result<SueldoDTO>.Failure("No se encontró el sueldo con el ID proporcionado.");
            if (sueldo.Pagado)
            {
                return Result<SueldoDTO>.Failure("el sueldo que se desea marcar como pago ya esta pagado");
            }

            SueldoDTO? success = await this._sueldoRepository.PagarSueldo(id);
            if (success != null)
            {
                Console.WriteLine("se pagó el sueldo correctamente");
                return Result<SueldoDTO>.Success(success);
            }
            return Result<SueldoDTO>.Failure("Hubo un problema al actualizar el sueldo");
        }
        public async Task<Result<SueldoDTO>> ActualizarAsync(int id, float? monto = null, int? Id_Chofer = null, DateOnly? pagadoDesde = null, DateOnly? pagadoHasta = null, DateOnly? FechaPago = null)
        {
            if (id <= 0)
                return Result<SueldoDTO>.Failure(MensajeError.IdInvalido(id));


            var pagoExistente = await _sueldoRepository.ObtenerPorId(id);

            if (pagoExistente == null)
                return Result<SueldoDTO>.Failure("No se encontró el pago con el ID proporcionado.");

            if (monto == null && Id_Chofer == null && pagadoDesde == null && pagadoHasta == null && FechaPago == null)
                return Result<SueldoDTO>.Failure("No se proporcionó ningún dato para actualizar.");

            if (monto != null)
                pagoExistente.Monto_Pagado = monto.Value;
            if (Id_Chofer != null)
                pagoExistente.Id_Chofer = Id_Chofer.Value;

            if (pagadoDesde != null)
                pagoExistente.PagadoDesde = pagadoDesde.Value;
            if (pagadoHasta != null)
                pagoExistente.PagadoHasta = pagadoHasta.Value;

            if (FechaPago != null)
                pagoExistente.FechaDePago = FechaPago.Value;
            bool success = await this._sueldoRepository.ActualizarAsync(id, (int)pagoExistente.Id_Chofer, pagoExistente.PagadoDesde, pagoExistente.PagadoHasta, pagoExistente.Monto_Pagado, pagoExistente.FechaDePago);

            if (success)
            {
                SueldoDTO result = await _sueldoRepository.ObtenerPorId(id);
                return Result<SueldoDTO>.Success(result);
            }
            return Result<SueldoDTO>.Failure("No se pudo realizar la actualización");
        }
    }
}
