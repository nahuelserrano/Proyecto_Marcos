using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Marcos.Presentacion.models
{
    class Cliente
    {
        public Viaje viaje { get; set; }
        public String _nombre { get; set; }
        public String _apellido { get; set; }
        public cheque _cheque { get; set; }

        public Cliente(Viaje viaje, String nombre, String apellido, cheque cheque)
        {
            this.viaje = viaje;
            this._nombre = nombre;
            this._apellido = apellido;
            this._cheque = cheque;
        }
    }
}
