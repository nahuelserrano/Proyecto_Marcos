using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class ViajeDTO
    {
        public int Id { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string LugarPartida { get; set; }
        public string Destino { get; set; }
        public int Carga { get; set; }
        public int Km { get; set; }
        public float toneladas { get; set; }
        public double PrecioPorKilo { get; set; }
        public string Remito { get; set; }
        public string nombreEmpleado { get; set; }
        public string nombreCliente { get; set; }
        public float Tarifa { get; set; }
        public float PrecioViaje { get { return (Tarifa * toneladas) * 1000; } }

        public ViajeDTO(int id, DateOnly fechaInicio, DateTime fechaEntrega, string lugarPartida,
                        string destino, int carga, int km, double precioPorKilo,
                        string remito, string nombreEmpleado, string nombreCliente,float tarifa,float toneladas)
        {
            Id = id;
            FechaInicio = fechaInicio;
            FechaEntrega = fechaEntrega;
            LugarPartida = lugarPartida;
            Destino = destino;
            Carga = carga;
            Km = km;
            PrecioPorKilo = precioPorKilo;
            this.Remito = remito;
            this.nombreEmpleado = nombreEmpleado;
            this.nombreCliente = nombreCliente;
            this.Tarifa = tarifa;
            this.toneladas = toneladas;
        }

        public ViajeDTO()
        {
            Id = 0;
            FechaInicio = DateOnly.MinValue;
            FechaEntrega = DateTime.Now;
            LugarPartida = "default";
            Destino = "default";
            Carga = 0;
            Km = 0;
            PrecioPorKilo = 0.0;
            Remito = "default";
            nombreEmpleado = "default";
            nombreCliente = "default";
            Tarifa = 1200;
            toneladas = 12;
        }
    }
    }

