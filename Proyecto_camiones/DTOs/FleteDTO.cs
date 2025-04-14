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
        public FleteDTO(string nombre)
        {
            Nombre = nombre;
        }
        public FleteDTO()
        {
            Nombre = "default";
            // Constructor vacío
        }

        public float getSaldo()
        {
            return 0.0f; //provisorio
        }
    }
}
