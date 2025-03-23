using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string contrasenia { get; set; }

        public Usuario(int id, string nombre, string apellido, string email, string contrasenia)
        {
            Id = id;
            this.nombre = nombre;
            this.apellido = apellido;
            this.email = email;
            this.contrasenia = contrasenia;
        }
    }
}
