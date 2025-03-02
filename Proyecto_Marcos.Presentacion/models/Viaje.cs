using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Marcos.Presentacion.Models
{
    public class Viaje
    {
        const int tonelada = 1000;
        private Camion camion { get; set; }
        private DateTime _fechaPartida { get; set; }
        private String destino { get; set; }
        private String _lugarPartida { get; set; }
        private float _peso { get; set; }
        private int _remito { get; set; }
        private float _precio_kilo { get; set; }
        private float _total => (_peso * _precio_kilo)*tonelada;
        private float _carga;
        public float Carga
        {
            get { return _carga; }
            set
            {
                if (this.camion.chequeo_peso_maximo(_carga))
                {
                    _carga = value;
                }
                else
                {
                    throw new Exception("El peso de la carga supera la capacidad maxima del camion");
                }
            }
        }
        private Chofer _chofer { get; set; }
        private float presupuesto => _carga * _precio_kilo;
        private Cliente _cliente { get; set; }
        public DateTime FechaInicio { get; internal set; }
        public DateTime FechaEntrega { get; internal set; }
        public int KilosCarga { get; internal set; }
        public object CamionId { get; internal set; }

        public Viaje(DateTime fechaPartida, String destino, String lugarPartida, float _peso, int remito, float _precio_kilo, float carga, Chofer chofer, Cliente cliente)
        {
            this._fechaPartida = fechaPartida;
            this.destino = destino;
            this._lugarPartida = lugarPartida;
            this._peso = _peso;
            this._remito = remito;
            this._precio_kilo = _precio_kilo;
            this._carga = carga;
            this._chofer = chofer;
            this._cliente = cliente;
        }
    }
}