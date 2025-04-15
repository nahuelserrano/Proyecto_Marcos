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
        public string nombreCliente { get; set; }
        public string nombreEmpleado { get; set; }
        public float Km { get; set; }
        public float Tarifa { get; set; }
        public DateOnly FechaFacturacion { get; set; }
        public float toneladas { get; set; }
        public float PrecioViaje { get { return (Tarifa * toneladas) * 1000; } }

        public ViajeDTO(DateOnly fechaInicio, string lugarPartida,
                        string destino, int remito, float kg, string carga, string nombreCliente, 
                        string nombreEmpleado, float km, float tarifa)
        {
            this.FechaInicio = fechaInicio;
            this.LugarPartida = lugarPartida;
            this.Destino = destino;
            this.Carga = carga;
            this.Kg = kg;
            this.Km = km;
            this.Remito = remito;
            this.nombreEmpleado = nombreEmpleado;
            this.nombreCliente = nombreCliente;
            //this.Tarifa = tarifa;
            this.toneladas = toneladas;
        }

        public ViajeDTO()
        {
            FechaInicio = DateOnly.MinValue;
            LugarPartida = "default";
            Destino = "default";
            Remito = 0;
            Kg = 0.0f;
            Carga = "default";
            nombreCliente = "default";
            nombreEmpleado = "default";
            Km = 0;
            Tarifa = 1200;
            toneladas = 12;
        }
    }
    }

