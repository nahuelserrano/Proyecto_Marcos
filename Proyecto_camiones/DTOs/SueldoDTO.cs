using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class SueldoDTO
    {
        public int Id_Chofer { get; set; }    
        public DateOnly FechaDePago { get; set; }
        public float Monto_Pagado { get; set; }
        public DateOnly pagadoDesde { get;  set; }
        public DateOnly pagadoHasta { get;  set; }

        public SueldoDTO(float monto, int Id_Chofer, DateOnly pagadoDesde,DateOnly pagadoHasta, DateOnly FechaPago)
        {
            this.Id_Chofer = Id_Chofer;
            this.FechaDePago = FechaPago;
            this.Monto_Pagado = monto;
            this.pagadoDesde = pagadoDesde;
            this.pagadoHasta = pagadoHasta;
        }

        public SueldoDTO()
        {
            // Constructor vacío
            this.pagadoDesde = default;
            this.pagadoHasta = default;
            this.Id_Chofer = 0;
            this.FechaDePago = default;
            this.Monto_Pagado = 0.0f;
        }

    }
}
