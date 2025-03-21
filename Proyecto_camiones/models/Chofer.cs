using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Chofer
    {
        public String nombre { get; set; }
        public String apellido { get; set; }
       
        public Chofer (String nombre, String apellido)
        {
            this.nombre = nombre;
            this.apellido = apellido;
           
        }
    }
}
