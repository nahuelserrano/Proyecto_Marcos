using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using NPOI.SS.UserModel;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Chofer
    {
        public int Id;
        public string Nombre { get; set; }

        public Chofer(string nombre)
        {
            this.Nombre = nombre;
        }

        public ChoferDTO toDTO()
        {
            ChoferDTO choferDTO = new ChoferDTO(this.Nombre);
            return choferDTO;
        }
    }
}
