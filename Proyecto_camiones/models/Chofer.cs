using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Proyecto_camiones.DTOs;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Chofer
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
       
        public Chofer (string nombre, string apellido)
        {
            this.nombre = nombre;
            this.apellido = apellido;
        }

        public ChoferDTO toDTO()
        {
            ChoferDTO choferDTO = new ChoferDTO(nombre, apellido);
            return choferDTO;
        }
    }
}
