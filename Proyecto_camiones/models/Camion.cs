using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Camion
    {
        public int Id { get; set; }
        public float peso_max { get; set; }
        public float tara {get; set; }
        public String Patente { get; set; }

        public Camion(float peso_max, float tara, string Patente)
        {
            this.peso_max = peso_max;
            this.tara = tara;
            this.Patente = Patente;
        }

        public Camion()
        {
            this.Patente = "x";
        }


        public static implicit operator Task<object>(Camion v)
        {
            throw new NotImplementedException();
        }

    }
}
