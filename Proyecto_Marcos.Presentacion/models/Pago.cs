﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Marcos.Presentacion.Models
{
    public class Pago
    {
        public int id { get; set; }
        public int Monto { get; set; }
        public bool Pagado { get; set; }
    }
}