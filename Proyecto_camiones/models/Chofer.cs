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
    public class Chofer : Empleado
    {
        public float porcentaje_cobro { get; set; }

        public Chofer(string nombre, string apellido) : base(nombre, apellido, "chofer")
        {
            this.porcentaje_cobro = 18;
        }

        public ChoferDTO toDTO()
        {
            ChoferDTO choferDTO = new ChoferDTO(nombre, apellido);
            return choferDTO;
        }
    }
}
