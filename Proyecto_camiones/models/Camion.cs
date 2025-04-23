
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
        public String Patente { get; set; }

        public string? nombre_chofer { get; set; }

        public Camion( string Patente, string? chofer)
        {
            this.Patente = Patente;
            this.nombre_chofer = chofer;
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
