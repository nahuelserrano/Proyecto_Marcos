using Org.BouncyCastle.Crypto.Macs;
using System;
using Proyecto_camiones.DTOs;

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
      
        public int Cliente { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFacturacion { get; set; }
        public string Carga { get; set; }
        public float Km { get; set; }
        public float PrecioPorKilo { get; set; }
        public float Tarifa { get; set; }
        public float PrecioViaje { get { return (Tarifa * Kg)*1000; } }

        public float total { get => Kg * PrecioPorKilo; }


        public Viaje( string destino, string lugarPartida, float kg, int remito,
                     float precioPorKilo, int empleado, int cliente, int camion,
                     DateTime fechaInicio, DateTime fechaEntrega, string carga, float km)
        {
     
            Destino = destino;
            LugarPartida = lugarPartida;
            Kg = kg;
            Remito = remito;
            //Tarifa = tarifa;
            Cliente = cliente;
            Camion = camion;
            //FechaInicio = fechaInicio;
            //FechaFacturacion = fechaFacturacion;
            Carga = carga;
            PrecioPorKilo = precioPorKilo;
            Km = km;

        }

        public Viaje()
        {

        }

        //public ViajeDTO toDTO()
        //{
        //    //return new ViajeDTO(Id, FechaInicio, FechaFacturacion, LugarPartida,
        //    //                    Destino, Carga, Kg, (int)Km, PrecioPorKilo,
        //    //                    Remito.ToString(), "default", "default");
        //}
    }
}
