using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Marcos.Presentacion.models
{
    public class Cliente
    {
  
        public String Nombre { get; set; }
        public String Apellido { get; set; }
        public int dni { get; set; }


        public Cliente(Camion viaje, String nombre, String apellido, int dni)
        {
         
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.dni = dni;

        }
    }
}
