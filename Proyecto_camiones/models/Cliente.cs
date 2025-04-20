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

        public Cliente(String nombre)
        {
            this.Nombre = nombre;
        }

        // Navegación inversa (colección)
        public ICollection<Viaje> Viajes { get; set; }

        override
        public String ToString()
        {
            return "Id: " + this.Id + "Nombre: "+this.Nombre;
        }
    }
}
