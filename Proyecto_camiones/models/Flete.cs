using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Models
{
    public class Flete : Empleado
    {
        public float porcentaje_cobro { get; set; }

        public Flete(string nombre, string apellido) :base(nombre, apellido, "flete")
        {
            this.porcentaje_cobro = 100;
        }

    }
}
