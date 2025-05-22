using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Services;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.Models;
using Proyecto_camiones.ViewModels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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
       

        public SueldoService(SueldoRepository pagosR,PagoService pagoS)
        {
            this._sueldoRepository = pagosR ?? throw new ArgumentNullException(nameof(pagosR));
            this._pagoService = pagoS ?? throw new ArgumentNullException(nameof(pagoS));
            CamionRepository cr = new CamionRepository();
            this._camionService = new CamionService(cr);
            ViajeRepository vr = new ViajeRepository(General.obtenerInstancia());
            ClienteRepository clr = new ClienteRepository(General.obtenerInstancia());
            ClienteService cs = new ClienteService(clr);
            ChoferRepository chr = new ChoferRepository(General.obtenerInstancia());
            this._choferService = new ChoferService(chr);
            this._viajeService = new ViajeService(vr, _camionService, cs, _choferService, pagoS);
        }


        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._sueldoRepository.ProbarConexionAsync();
            return result;
        }

        public async Task<Result<List<SueldoDTO>>> ObtenerTodosAsync(string? patenteCamion,string? nombreChofer)
        {
                if (string.IsNullOrEmpty(patenteCamion) && string.IsNullOrEmpty(nombreChofer))
                    return Result<List<SueldoDTO>>.Failure("No se proporcionó la patente del camión ni un chofer");

                int idCamion = -1;
                if (patenteCamion != null)
                        {
                    Console.WriteLine("hola if de patente");
                        var camion = await this._camionService.ObtenerPorPatenteAsync(patenteCamion);

                        if (camion == null)
                            return Result<List<SueldoDTO>>.Failure("No se encontró el camión con la patente proporcionada.");
                Console.WriteLine("sobrevivimos a obtener csmion");
                idCamion = camion.Value.Id;
                        }
            
                int idChofer = -1;

                if (nombreChofer != null)
                {
                Console.WriteLine("hola if de chofer");
                    var chofer = await this._choferService.ObtenerPorNombreAsync(nombreChofer);
                    if (chofer == null)
                        return Result<List<SueldoDTO>>.Failure("No se encontró el chofer con el nombre proporcionado.");
                Console.WriteLine("sobrevivimos a obtener chofer");
                    idChofer = chofer.Value.Id;
                }

            Console.WriteLine("llegamos tan lejos?");
                List<SueldoDTO>? sueldos = await this._sueldoRepository.ObtenerTodosAsync(idCamion,idChofer);
                
                
                if (sueldos != null)
                    return Result<List<SueldoDTO>>.Success(sueldos);
            return Result<List<SueldoDTO>>.Failure("Hubo un problema al obtener los sueldos");
        }

        internal async Task<Result<bool>> EliminarAsync(int sueldoId)
        {
            if (sueldoId <= 0) return Result<bool>.Failure(MensajeError.IdInvalido(sueldoId));
           
            SueldoDTO sueldo = await _sueldoRepository.ObtenerPorId(sueldoId);

            if (sueldo == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(sueldoId)));

            await _pagoService.ModificarEstado((int)sueldo.Id_Chofer, sueldo.PagadoDesde, sueldo.PagadoHasta,null,false);

            await _sueldoRepository.EliminarAsync(sueldoId);

            return Result<bool>.Success(true);
        }

        public async Task<Result<int>> CrearAsync(int Id_Chofer, DateOnly pagoDesde, DateOnly pagoHasta,int idCamion)
        {
            float monto = await calculadorSueldo(Id_Chofer,pagoDesde,pagoHasta);

            if(monto <= 0)
                return Result<int>.Failure("No se pudo calcular el sueldo ya que no hay pagos pendientes");

            ValidadorSueldo validador = new ValidadorSueldo(monto, Id_Chofer, pagoDesde, pagoHasta, pagoHasta);
         
            Result<bool> resultadoValidacion = validador.ValidarCompleto();

            Console.WriteLine("resultadoValidacion: " + resultadoValidacion.IsSuccess);

            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);

            try
            {

                int idSueldo = await _sueldoRepository.InsertarAsync(monto, Id_Chofer, pagoDesde, pagoHasta,idCamion);
                if (idSueldo<0)
                    return Result<int>.Failure("No se pudo crear el sueldo en services");
                 await _pagoService.ModificarEstado(Id_Chofer, pagoDesde, pagoHasta,idSueldo);
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

        public async Task<Result<SueldoDTO>> marcarPagado(int id, DateOnly? fecha_pagado) {
            if (id < 0) 
                return Result<SueldoDTO>.Failure("id del sueldo deseado invalido");

            SueldoDTO? sueldo = await _sueldoRepository.ObtenerPorId(id);
            if (sueldo == null)
                return Result<SueldoDTO>.Failure("No se encontró el sueldo con el ID proporcionado.");
            if (sueldo.Pagado) {
                return Result<SueldoDTO>.Failure("el sueldo que se desea marcar como pago ya esta pagado");
            }
            
            SueldoDTO? success = await this._sueldoRepository.PagarSueldo(id, fecha_pagado);
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
 