using Org.BouncyCastle.Crypto.Macs;
using System;
using Proyecto_camiones.DTOs;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Viaje
    {

        public int Id { get; set; }
        public DateOnly FechaInicio { get; set; }
        public string LugarPartida { get; set; }
        public string Destino { get; set; }
        public int Remito { get; set; }
        public float Kg { get; set; }
        public string Carga { get; set; }
        public int Cliente { get; set; }
        public int Camion { get; set; }
        public float Km { get; set; }
        public float Tarifa { get; set; }
        public string NombreChofer { get; set; }
        public double PorcentajeChofer { get; set; }
        public float Total => Tarifa * Kg * 1000;

        // Propiedades de navegación
        public Cliente ClienteNavigation { get; set; } // La entidad Cliente relacionada
        public Camion CamionNavigation { get; set; } // La entidad Camión relacionada

        public Viaje(DateOnly fechaInicio, string lugarPartida, string destino, int remito, float kg,
            string carga, int cliente, int camion, float km, float tarifa, string nombreChofer, double porcentajeChofer)
        {
            FechaInicio = fechaInicio;
            LugarPartida = lugarPartida;
            Destino = destino;
            Remito = remito;
            Kg = kg;
            Carga = carga;
            Cliente = cliente;
            Camion = camion;
            Km = km;
            Tarifa = tarifa;
            NombreChofer = nombreChofer;
            PorcentajeChofer = porcentajeChofer;
        }

        public Viaje(DateOnly fechaInicio, string lugarPartida, string destino, int remito, float kg,
            string carga, int cliente, int camion, float km,  float tarifa)
        {
            FechaInicio = fechaInicio;
            LugarPartida = lugarPartida;
            Destino = destino;
            Remito = remito;
            Kg = kg;
            Carga = carga;
            Cliente = cliente;
            Camion = camion;
            Km = km;
            Tarifa = tarifa;
        }

        public ViajeDTO toDTO(string nombreChofer, string nombreCliente)
        {
            return new ViajeDTO(FechaInicio, LugarPartida, Destino, Remito, Kg, Carga, 
                nombreCliente, nombreChofer, Km, Tarifa, PorcentajeChofer);
        }

        public ViajeDTO toDTO(string nombreCliente)
        {
            return new ViajeDTO(FechaInicio, LugarPartida, Destino, Remito, Kg, Carga,
                nombreCliente, NombreChofer, Km, Tarifa, PorcentajeChofer);
        }
    }
}
