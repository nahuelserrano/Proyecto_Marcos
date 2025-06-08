using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Cliente
    {

        public int Id { get; set; }
        public string Nombre { get; set; }

        // Navegación inversa (colección)
        public ICollection<Viaje> Viajes { get; set; } = new List<Viaje>();

        public Cliente(){}
        public Cliente(string nombre)
        {
            this.Nombre = nombre;
        }


        override
        public string ToString()
        {
            return "Id: " + this.Id + "Nombre: "+this.Nombre;
        }
    }
}
