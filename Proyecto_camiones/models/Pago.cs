using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int Id_Chofer { get; set; }
        public int Id_Viaje { get; set; }
        public int? Id_sueldo { get; set; }
        public bool Pagado { get; set; }
        public float Monto_Pagado { get; set; }
     
        public Pago(int id, int id_chofer, int id_viaje, float monto_pagado, int id_sueldo)
        {
            Id = id;
            Id_Chofer = id_chofer;
            Id_Viaje = id_viaje;
            Id_sueldo = id_sueldo;
            Pagado = false;
            Monto_Pagado = monto_pagado;
           
        }
        public Pago(int id, int id_chofer, int id_viaje, float monto_pagado)
        {
            Id = id;
            Id_Chofer = id_chofer;
            Id_Viaje = id_viaje;
            Id_sueldo = null;
            Pagado = false;
            Monto_Pagado = monto_pagado;
          
        }
        public Pago( int id_chofer, int id_viaje, float monto_pagado)
        {
            
            Id_Chofer = id_chofer;
            Id_Viaje = id_viaje;
            Id_sueldo = null;
            Pagado = false;
            Monto_Pagado = monto_pagado;
        }

        public Pago()
        {
        }

        public Pago()
        {

        }
    }
}
