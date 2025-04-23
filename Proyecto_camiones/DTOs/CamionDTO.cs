using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class CamionDTO
    {
        public String Patente { get; set; }
        public string Nombre_Chofer { get; set; }

        public CamionDTO(string patente, string nombre_Chofer)
        {
            Patente = patente;
            Nombre_Chofer = nombre_Chofer;
        }

        public CamionDTO()
        {
            this.Patente = "default";
            // Constructor vacío
        }

        override
            public String ToString()
        {
            return "Camion con la patente: " + this.Patente;
        }
    }
}
