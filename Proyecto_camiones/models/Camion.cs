
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
        public float tara { get; set; }
        public String Patente { get; set; }

        public string nombre_chofer { get; set; }

        public Camion(float peso_max, float tara, string Patente, string nombre)
        {
            this.peso_max = peso_max;
            this.tara = tara;
            this.Patente = Patente;
            this.nombre_chofer = nombre;
        }

        public Camion()
        {
            this.Patente = "x";
            this.nombre_chofer = "undefined";
        }

        public ICollection<Viaje> Viajes { get; set; }

        public static implicit operator Task<object>(Camion v)
        {
            throw new NotImplementedException();
        }

    }
}
