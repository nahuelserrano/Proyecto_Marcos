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
        public string Apellido;
        public ChoferDTO(string nombre, string apellido)
        {
            Nombre = nombre;
            Apellido = apellido;
        }
        public ChoferDTO()
        {
            Nombre = "default";
            Apellido = "default";
            // Constructor vacío
        }
    }
}
