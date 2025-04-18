using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Models
{
    public class Flete
    {
        public int Id;
        public string nombre;

        public Flete(string nombre)
        {
            this.nombre = nombre;
        }

        override
            public string ToString()
        {
            return "Nombre fletero: " + this.nombre;
        }
    }
}
