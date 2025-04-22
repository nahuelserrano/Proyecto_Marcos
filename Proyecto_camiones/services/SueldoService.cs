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
        //    private SueldoRepository _sueldoRepository;
        //    private ViajeService _viajeService;
        //    private Viaje _viajeDTO;

        //    public SueldoService(SueldoRepository pagosR)
        //    {
        //        this._sueldoRepository = pagosR ?? throw new ArgumentNullException(nameof(pagosR));
        //    }

        //    public async Task<Result<SueldoDTO>> ObtenerPorId(int id)
        //    {
        //        if (id <= 0)
        //            return Result<SueldoDTO>.Failure(MensajeError.idInvalido(id));

        //        SueldoDTO Pago = await _sueldoRepository.ObtenerPorId(id);

        //        if (Pago == null)
        //            return Result<SueldoDTO>.Failure(MensajeError.objetoNulo(nameof(Pago)));

        //        return Result<SueldoDTO>.Success(Pago);
        //    }

        //    internal async Task<Result<bool>> EliminarAsync(int pagoId)
        //    {
        //        if (pagoId <= 0) return Result<bool>.Failure(MensajeError.idInvalido(pagoId));

        //        SueldoDTO pago = await this._sueldoRepository.ObtenerPorId(pagoId);

        //        if (pago == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(pago)));

        //        await _sueldoRepository.Eliminar(pagoId); 

        //        return Result<bool>.Success(true);





        //    }

        //    public async Task<Result<int>> CrearAsync(int Id_Chofer, DateOnly pagodesde, DateOnly pagoHasta, DateOnly FechaPago)
        //    {
        //         float monto = this.calculadorSueldo(Id_Chofer);


        //        ValidadorSueldo validador = new ValidadorSueldo(33,Id_Chofer, FechaPago, pagodesde, pagoHasta);

        //        Result<bool> resultadoValidacion = validador.ValidarCompleto();

        //        if (!resultadoValidacion.IsSuccess)
        //            return Result<int>.Failure(resultadoValidacion.Error);

        //        try
        //        {

        //            int idPago = await _sueldoRepository.Insertar(2,Id_Chofer, FechaPago, pagodesde, FechaPago);
        //            return Result<int>.Success(idPago);
        //        }
        //        catch (Exception ex)
        //        {
        //            return Result<int>.Failure("Hubo un error al crear el cheque");
        //        }
        //    }



        //    private double calculadorSueldo(int id_chofer, double porcentajePago, DateOnly calcularDesde, DateOnly calcularHasta)
        //    {
        //        List<ViajeDTO> viajes = new List<ViajeDTO>();

        //        ViajeDTO viajeDTO = new ViajeDTO();
        //        ViajeDTO viajeDTO2 = new ViajeDTO();
        //        ViajeDTO viajeDTO3 = new ViajeDTO();

        //        viajes.Add(viajeDTO);
        //        viajes.Add(viajeDTO2);
        //        viajes.Add(viajeDTO3);

        //        float sueldo = 0;
        //        foreach (var viaje in viajes)
        //        {
        //            if (viaje.FechaInicio >= calcularDesde && viaje.FechaInicio <= calcularHasta)
        //            {
        //                sueldo += viaje.PrecioViaje;

        //            }
        //        }

        //        return sueldo * porcentajePago;

        //    }

        //    public async Task<Result<SueldoDTO>> ActualizarAsync(int id,float? monto=null, int? Id_Chofer=null, DateOnly? pagadoDesde=null, DateOnly? pagadoHasta=null, DateOnly? FechaPago=null)
        //    {
        //        if (id <= 0)
        //            return Result<SueldoDTO>.Failure(MensajeError.idInvalido(id));

        //        var pagoExistente = await _sueldoRepository.ObtenerPorId(id);

        //        if (pagoExistente == null)
        //            return Result<SueldoDTO>.Failure("No se encontró el pago con el ID proporcionado.");

        //        if (monto == null && Id_Chofer == null && pagadoDesde == null && pagadoHasta == null && FechaPago == null)
        //            return Result<SueldoDTO>.Failure("No se proporcionó ningún dato para actualizar.");

        //        // Actualizaciones individuales
        //        if (monto != null)
        //            pagoExistente.Monto_Pagado = monto.Value;

        //        if (Id_Chofer != null)
        //            pagoExistente.Id_Chofer = Id_Chofer.Value;

        //        if (pagadoDesde != null)
        //            pagoExistente.pagadoDesde = pagadoDesde.Value;
        //        if (pagadoHasta != null)
        //            pagoExistente.pagadoHasta = pagadoHasta.Value;

        //        if (FechaPago != null)
        //            pagoExistente.FechaDePago = FechaPago.Value;

        //         bool success = await this._sueldoRepository.Actualizar(id,pagoExistente.Id_Chofer,pagoExistente.pagadoDesde, pagoExistente.pagadoHasta,pagoExistente.Monto_Pagado,pagoExistente.FechaDePago);
        //        if (success)
        //        {
        //            SueldoDTO result = await _sueldoRepository.ObtenerPorId(id);
        //            return Result<SueldoDTO>.Success(result);
        //        }
        //        return Result<SueldoDTO>.Failure("No se pudo realizar la actualización");
        //    }

        //}

    }
}

