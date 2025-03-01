using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Marcos.Presentacion.models
{
    public class Camion
    {
        internal int CapacidadMaxima;

        public float CapaciadaMax { get; set; }

        public float Tara { get ; set; }
        public int Id { get; set; }
        public String patente { get; set; }



        public Camion(float capMax, float tara, String patente)
        {
            this.CapaciadaMax = capMax;
            this.Tara = tara;
       
            this.patente = patente;



        }

        public bool chequeo_peso_maximo(float peso)
        {
            if (this.Tara + peso>this.CapaciadaMax)
            {
                return false;
            }
            return true;
        }
        
        
    }
}
