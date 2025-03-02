using System;

namespace Proyecto_Marcos.Presentacion.Utils
{
   public static class Validador
   {

        public static Result<bool> ValidarId(int id, string nombreDeClase)//el tipo T reemplaza a cualquier tipo de dato por ejemplo valirdarId <camion>
        {
            if (id <= 0)
            {
                return Result<bool>.Failure(MensajeError.idInvalido(nombreDeClase));
            }
            return Result<bool>.Success(true);
        }

        public static Result<bool> ValidarNumeroPositivo(decimal valor, string atributo)//el tipo T reemplaza a cualquier tipo de dato por ejemplo valirdarId <camion>
        {
            if (valor <= 0)
            {
                return Result<bool>.Failure(MensajeError.valorInvalido(atributo));
            }
            return Result<bool>.Success(true);
        }

        public static Result<bool> ValidarNoNull(object objeto, string nombreDeClase = null)
        {
            if (nombreDeClase == null)
                nombreDeClase = typeof(object).Name;
            
            if (objeto == null)
                return Result<bool>.Failure(MensajeError.objetoNulo(nombreDeClase));

            return Result<bool>.Success(true);
        }
    }
}
