﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Proyecto_Marcos.Presentacion.models


{
    class Chofer
    {
        public String nombre { get; set; }
        public String apellido { get; set; }
        public List<Pagos> pagos;
        
        public Chofer (String nombre, String apellido)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.pagos = new List<Pagos>();
        }
       

        public void addPagos(Pagos p)
        {
            pagos.Add(p);
        }
   
    }
}
