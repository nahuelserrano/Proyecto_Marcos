using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransporteApp.Utils;

namespace Proyecto_Marcos.Presentacion.Utils
{
   public static class Validador
    {
        public static Result<bool> ValidarId<T>(int id, String nombreDeClase = null)//el tipo T reemplaza a cualquier tipo de dato por ejemplo valirdarId <camion>
        {
            String nombre = nombreDeClase;
            if (nombre == null)
            {
                nombre = typeof(T).Name;
            }
            if (id <= 0)
            {
                return Result<bool>.Failure($"El id de {nombre} no puede ser menor a 0");
            }
            return Result<bool>.Failure(true);
        }
    }
}
