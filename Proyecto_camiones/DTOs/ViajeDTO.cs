using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class ViajeDTO
    {
        public DateOnly FechaInicio { get; set; }
        public string LugarPartida { get; set; }
        public string Destino { get; set; }
        public int Remito { get; set; }
        public float Kg { get; set; }
        public string Carga { get; set; }
        public string NombreCliente { get; set; }
        public string NombreChofer { get; set; }
        public float Km { get; set; }
        public float Tarifa { get; set; }
        public float PorcentajeChofer { get; set; } // Porcentaje por defecto del chofer
        public float Total => Tarifa * Kg;
        public float GananciaChofer => Total * PorcentajeChofer;

        public int Camion { get; internal set; }

        public ViajeDTO(DateOnly fechaInicio, string lugarPartida,
            string destino, int remito, float kg, string carga, string nombreCliente,
            string nombreChofer, float km, float tarifa, float porcentajeChofer)
        {
            FechaInicio = fechaInicio;
            LugarPartida = lugarPartida;
            Destino = destino;
            Carga = carga;
            Kg = kg;
            Km = km;
            Remito = remito;
            NombreChofer = nombreChofer;
            NombreCliente = nombreCliente;
            Tarifa = tarifa;
            PorcentajeChofer = porcentajeChofer;
        }

        //// También arreglamos la propiedad para que sea consistente

        public ViajeDTO()
        {
            //FechaInicio = DateTime.Now;
            //FechaEntrega = DateTime.Now;
            LugarPartida = "default";
            Destino = "default";
            Remito = 0;
            Kg = 0.0f;
            Carga = "default";
            NombreCliente = "default";
            NombreChofer = "default";
            Km = 0;
            Tarifa = 1200;
            PorcentajeChofer = 0.18f; // Porcentaje por defecto del chofer
        }

        override
            public string ToString()
        {
            return $"Total: {Total}";
        }
    }
}

