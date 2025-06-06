﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class SueldoDTO
    {
        public int idSueldo { get; set; }
        public int? Id_Chofer { get; set; }
        public DateOnly? FechaDePago { get; set; }
        public float Monto_Pagado { get; set; }
        public DateOnly PagadoDesde { get; set; }
        public DateOnly PagadoHasta { get; set; }
        public bool Pagado { get; set; }
        public int? IdCamion { get; set; }
        public string? chofer { get; set; }

        public SueldoDTO(int id, float monto, int? Id_Chofer, DateOnly pagadoDesde, DateOnly pagadoHasta, DateOnly? FechaPago, bool pagado, int? IdCamion, string? chofer)
        {
            this.idSueldo = id;
            this.Id_Chofer = Id_Chofer;  // Sin casting
            this.FechaDePago = FechaPago;
            this.Monto_Pagado = monto;
            this.PagadoDesde = pagadoDesde;
            this.PagadoHasta = pagadoHasta;
            this.Pagado = pagado;
            this.IdCamion = IdCamion;  // Sin casting
            this.chofer = chofer;
        }

        public SueldoDTO()
        {
            // Constructor vacío
            this.PagadoDesde = default;
            this.PagadoHasta = default;
            this.Id_Chofer = 0;
            this.FechaDePago = default;
            this.Monto_Pagado = 0;
            this.IdCamion = 0;
            this.chofer = default;
        }

        override
            public String ToString()
        {
            return "Id: " + this.idSueldo + ", idChofer: " + this.Id_Chofer + ", idCamion: " + this.IdCamion;
        }
    }
}
