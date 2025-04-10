using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class EmpleadoDTO
    {
        public string Nombre;
        public EmpleadoDTO(string nombre)
        {
            Nombre = nombre;
        }
        public EmpleadoDTO()
        {
            Nombre = "default";
            // Constructor vacío
        }
    }
}
