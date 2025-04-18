using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.Presentacion.Services
{
    class PagosService
    {
        private PagoRepository _pagoRepository;
        private VIA _viajeService;
        private Viaje _viajeDTO;

        public PagosService(PagoRepository pagosR)
        {
            this._pagoRepository = pagosR ?? throw new ArgumentNullException(nameof(pagosR));
        }

        public async Task<Result<PagoDTO>> ObtenerPorId(int id)
        {
            if (id <= 0)
                return Result<PagoDTO>.Failure(MensajeError.idInvalido(id));

            PagoDTO Pago = await _pagoRepository.ObtenerPorId(id);

            if (Pago == null)
                return Result<PagoDTO>.Failure(MensajeError.objetoNulo(nameof(Pago)));

            return Result<PagoDTO>.Success(Pago);
        }

        internal async Task<Result<bool>> Eliminar(int pagoId)
        {
            if (pagoId <= 0) return Result<bool>.Failure(MensajeError.idInvalido(pagoId));

            PagoDTO pago = await this._pagoRepository.ObtenerPorId(pagoId);

            if (pago == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(pago)));

            await _pagoRepository.Eliminar(pagoId); 

            return Result<bool>.Success(true);



           

        }

        public async Task<Result<int>> Crear(int Id_Chofer, DateOnly pagodesde, DateOnly pagoHasta, DateOnly FechaPago)
        {
             float monto = this.calculadorSueldo(Id_Chofer);

           
            ValidadorPago validador = new ValidadorPago(33,Id_Chofer, FechaPago, pagodesde, pagoHasta);

            Result<bool> resultadoValidacion = validador.ValidarCompleto();

            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);

            try
            {
                
                int idPago = await _pagoRepository.Insertar(2,Id_Chofer, FechaPago, pagodesde, FechaPago);
                return Result<int>.Success(idPago);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Hubo un error al crear el cheque");
            }
        }



        private double calculadorSueldo(int id_chofer, double porcentajePago, DateOnly calcularDesde, DateOnly calcularHasta)
        {
            List<ViajeDTO> viajes = new List<ViajeDTO>();

            ViajeDTO viajeDTO = new ViajeDTO();
            ViajeDTO viajeDTO2 = new ViajeDTO();
            ViajeDTO viajeDTO3 = new ViajeDTO();

            viajes.Add(viajeDTO);
            viajes.Add(viajeDTO2);
            viajes.Add(viajeDTO3);

            float sueldo = 0;
            foreach (var viaje in viajes)
            {
                if (viaje.FechaInicio >= calcularDesde && viaje.FechaInicio <= calcularHasta)
                {
                    sueldo += viaje.PrecioViaje;

                }
            }
           
            return sueldo * porcentajePago;

        }

        public async Task<Result<PagoDTO>> Actualizar(int id,float? monto=null, int? Id_Chofer=null, DateOnly? pagadoDesde=null, DateOnly? pagadoHasta=null, DateOnly? FechaPago=null)
        {
            if (id <= 0)
                return Result<PagoDTO>.Failure(MensajeError.idInvalido(id));

            var pagoExistente = await _pagoRepository.ObtenerPorId(id);

            if (pagoExistente == null)
                return Result<PagoDTO>.Failure("No se encontró el pago con el ID proporcionado.");

            if (monto == null && Id_Chofer == null && pagadoDesde == null && pagadoHasta == null && FechaPago == null)
                return Result<PagoDTO>.Failure("No se proporcionó ningún dato para actualizar.");

            // Actualizaciones individuales
            if (monto != null)
                pagoExistente.Monto_Pagado = monto.Value;

            if (Id_Chofer != null)
                pagoExistente.Id_Chofer = Id_Chofer.Value;

            if (pagadoDesde != null)
                pagoExistente.pagadoDesde = pagadoDesde.Value;
            if (pagadoHasta != null)
                pagoExistente.pagadoHasta = pagadoHasta.Value;

            if (FechaPago != null)
                pagoExistente.FechaDePago = FechaPago.Value;

             bool success = await this._pagoRepository.Actualizar(id,pagoExistente.Id_Chofer,pagoExistente.pagadoDesde, pagoExistente.pagadoHasta,pagoExistente.Monto_Pagado,pagoExistente.FechaDePago);
            if (success)
            {
                PagoDTO result = await this._pagoRepository.ObtenerPorId(id);
                return Result<PagoDTO>.Success(result);
            }
            return Result<PagoDTO>.Failure("No se pudo realizar la actualización");
        }

    }

}

