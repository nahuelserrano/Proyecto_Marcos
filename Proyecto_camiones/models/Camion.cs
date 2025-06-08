﻿
using System;
using System.Collections.Generic;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Camion // entidad camión con sus atributos propios
    {
        public int Id { get; set; }
        public String Patente { get; set; }

        public string? nombre_chofer { get; set; }

        public Camion( string Patente, string? chofer)
        {
            this.Patente = Patente;
            this.nombre_chofer = chofer;
        }

        public Camion()
        {
            this.Patente = "x";
            this.nombre_chofer = "undefined";
        }

        public ICollection<Viaje> Viajes { get; set; }

       
    }
}
