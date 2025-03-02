using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_Marcos.Presentacion.models;
using Proyecto_Marcos.Presentacion.Utils;

namespace Proyecto_Marcos.Presentacion.Utils
{

    public static class ValidadorViaje
    {
        public static string NOMBRE_CLASE = "Viaje";
        
        // Validaciones específicas para Viaje
        public static Result<bool> ValidarFechas(Viaje viaje)
        {
            if (viaje == null) return Result<bool>.Failure(MensajeError.objetoNulo(NOMBRE_CLASE));

            if (viaje.FechaEntrega < viaje.FechaInicio)
                return Result<bool>.Failure(MensajeError.fechaInvalida(nameof(viaje.FechaEntrega)));


            return Result<bool>.Success(true);
        }

        public Result<bool> ValidarCliente(Viaje viaje)
        {
            if (viaje.Cliente == null)
                return Result<bool>.Failure(MensajeError.objetoNulo(CLIENTE));

            return Result<bool>.Success(true);
        }

        public ViajeValidator ValidarCarga()
        {
            if (_viaje == null) return thisaaawssd;

            if (_viaje.KilosCarga <= 0)
                AddError("Los kilos de carga deben ser mayores a cero");

            return this;
        }

        // Método para validación personalizada con predicado
        public ViajeValidator Validate(Func<Viaje, bool> predicate, string errorMessage)
        {
            if (_viaje != null && !predicate(_viaje))
                AddError(errorMessage);

            return this;
        }

        // Validación completa en un solo método
        public ViajeValidator ValidarCompleto()
        {
            return NotNull()
                .ValidarFechas()
                .ValidarCliente()
                .ValidarCarga()
                .Validate(v => v.Tarifa > 0, "La tarifa debe ser mayor a cero")
                .Validate(v => v.ChoferId > 0, "Debe seleccionar un chofer válido")
                .Validate(v => v.CamionId > 0, "Debe seleccionar un camión válido");
        }
    }
}

