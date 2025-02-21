using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Marcos.Presentacion.models
{
    class Camion
    {
        private float _capaciadaMax { get; set; }
       
        private float _tara { get ; set; }

        
        
       public Camion(float capMax, float tara)
        {
            this._capaciadaMax = capMax;
            this._tara = tara;
        }

        public bool chequeo_peso_maximo(float peso)
        {
            if (this._tara+peso>this._capaciadaMax)
            {
                return false;
            }
            return true;
        }
        
        
    }
}
