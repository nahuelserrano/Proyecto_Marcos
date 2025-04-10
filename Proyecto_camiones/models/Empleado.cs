using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;

namespace Proyecto_camiones.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        
        public Empleado(string nombre)
        {
            this.nombre = nombre;
        }

        public Empleado()
        {
            this.nombre = "default";
            // Constructor vacío
        }

        public EmpleadoDTO toDTO()
        {
            EmpleadoDTO empleadoDTO = new EmpleadoDTO();
            empleadoDTO.Nombre = this.nombre;
            return empleadoDTO;
        }

    }
}
