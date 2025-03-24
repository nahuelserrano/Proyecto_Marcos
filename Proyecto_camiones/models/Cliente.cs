using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Cliente
    {

        public int Id { get; set; }
        public String nombre { get; set; }
        public String apellido { get; set; }
        public int dni { get; set; }

        public Cliente(String nombre, String apellido, int dni)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.dni = dni;
        }
    }
}
