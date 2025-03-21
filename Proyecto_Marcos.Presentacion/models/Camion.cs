﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Marcos.Presentacion.Models
{
    public class Camion
    {
        public float CapacidadMax { get; set; }
        public float Tara { get ; set; }
        public int Id { get; set; }
        public String Patente { get; set; }

        public Camion(float capMax, float tara, String patente)
        {
            this.CapacidadMax = capMax;
            this.Tara = tara;
            this.Patente = patente;
        }

        public bool chequeo_peso_maximo(float peso)
        {
            return peso + this.Tara <= this.CapacidadMax;
        }
    }
}
