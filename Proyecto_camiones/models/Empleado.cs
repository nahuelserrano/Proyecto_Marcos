using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string tipo_empleado { get; set; }

        public Empleado(string nombre, string apellido, string tipo_empleado)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.tipo_empleado = tipo_empleado;
        }

    }
}
