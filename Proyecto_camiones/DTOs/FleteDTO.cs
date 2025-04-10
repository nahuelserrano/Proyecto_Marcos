using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    class FleteDTO : EmpleadoDTO
    {
        public string Nombre;
        public string Apellido;
        public FleteDTO(string nombre, string apellido)
        {
            Nombre = nombre;
            Apellido = apellido;
        }
        public FleteDTO()
        {
            Nombre = "default";
            Apellido = "default";
            // Constructor vacío
        }

        public float getSaldo()
        {
            return 0.0f; //provisorio
        }
    }
}
