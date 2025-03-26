using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class CamionDTO
    {
        public float? peso_max { get; set; }
        public float? tara { get; set; }
        public String Patente { get; set; }

        public CamionDTO(float peso_max, float tara, string patente)
        {
            this.peso_max = peso_max;
            this.tara = tara;
            Patente = patente;
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
