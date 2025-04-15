using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class ChoferDTO
    {
        public string Nombre;
        public ChoferDTO(string nombre)
        {
            Nombre = nombre;
        }
        public ChoferDTO()
        {
            Nombre = "default";
            // Constructor vacío
        }
    }
}
