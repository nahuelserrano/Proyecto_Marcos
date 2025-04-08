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
        public String Nombre { get; set; }
        public String Apellido { get; set; }

        public Cliente(String nombre, String apellido)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
        }
    }
}
