using Org.BouncyCastle.Crypto.Macs;
using System;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Viaje
    {

        public int Id { get; set; }
        public int Camion { get; set; }

        public string LugarPartida { get; set; }
        public string Destino { get; set; }
        public float Kg { get; set; }
        public int Remito { get; set; }
        public int Chofer { get; set; }
        public int Cliente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string Estado { get; set; }
        public string Carga { get; set; }
        public float Km { get; set; }
        public float PrecioPorKilo { get; set; }


        public Viaje( string destino, string lugarPartida, float kg, int remito,
                     float precioPorKilo, int chofer, int cliente, int camion,
                     DateTime fechaInicio, DateTime fechaEntrega, string carga, float km, string estado = "Pendiente")
        {
     
            Destino = destino;
            LugarPartida = lugarPartida;
            Kg = kg;
            Remito = remito;
            Chofer = chofer;
            Cliente = cliente;
            Camion = camion;
            FechaInicio = fechaInicio;
            FechaEntrega = fechaEntrega;
            Estado = estado;
            Carga = carga;
            PrecioPorKilo = precioPorKilo;
            Km = km;
        }
    }
}
