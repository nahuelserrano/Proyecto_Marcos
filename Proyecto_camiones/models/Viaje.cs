using System;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Viaje
    {
        private const int TONELADA = 1000;

        public Camion Camion { get; set; }

        public string LugarPartida { get; set; }
        public string Destino { get; set; }
        public float Peso { get; set; }
        public int Remito { get; set; }
        public float PrecioPorKilo { get; set; }
        public float Carga { get; set; }
        public Chofer Chofer { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int KilosCarga { get; set; }
        public int CamionId { get; set; }
        public string Estado { get; set; }

        public float Total => (Peso * PrecioPorKilo) * TONELADA;
        public float Presupuesto => Carga * PrecioPorKilo;

        public Viaje( string destino, string lugarPartida, float peso, int remito,
                     float precioPorKilo, float carga, Chofer chofer, Cliente cliente, Camion camion,
                     DateTime fechaInicio, DateTime fechaEntrega, int kilosCarga, int camionId, string estado = "Pendiente")
        {
     
            Destino = destino;
            LugarPartida = lugarPartida;
            Peso = peso;
            Remito = remito;
            PrecioPorKilo = precioPorKilo;
            Carga = carga;
            Chofer = chofer;
            Cliente = cliente;
            Camion = camion;
            FechaInicio = fechaInicio;
            FechaEntrega = fechaEntrega;
            KilosCarga = kilosCarga;
            CamionId = camionId;
            Estado = estado;
        }
    }
}
