using System;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Sueldo
    {
        public int Id { get; set; }
        public int? Id_Chofer { get; set; }
        public DateOnly pagadoDesde { get; set; }
        public DateOnly pagadoHasta { get; set; }
        public DateOnly? FechaPago { get; set; }
        public float Monto { get; internal set; }
        public bool Pagado { get; set; }
        public int? IdCamion { get; set; }


        public Sueldo(float monto, int? Id_Chofer, DateOnly pagadoDesde, DateOnly pagadoHasta, int? idCamion)
        {
            this.Id_Chofer = Id_Chofer;
            this.Monto = monto;
            this.pagadoDesde = pagadoDesde;
            this.pagadoHasta = pagadoHasta;
            this.IdCamion = idCamion;

        }
    }
}