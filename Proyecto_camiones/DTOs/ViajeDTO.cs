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
        public DateOnly FechaFacturacion { get; set; }
        public string LugarPartida { get; set; }
        public string Destino { get; set; }
        public string Carga { get; set; }
        public float Kg { get; set; }
        public int Km { get; set; }
        public float toneladas { get; set; }
        public double PrecioPorKilo { get; set; }
        public string Remito { get; set; }
        public string nombreEmpleado { get; set; }
        public string nombreCliente { get; set; }
        public float Tarifa { get; set; }
        public float PrecioViaje { get { return (Tarifa * toneladas) * 1000; } }

        public ViajeDTO(int id, DateOnly fechaInicio, DateOnly fechaEntrega, string lugarPartida,
                        string destino, String carga, int km, double precioPorKilo,
                        string remito, string nombreEmpleado, string nombreCliente,float kg,float tarifa)
        {
            Id = id;
            FechaInicio = fechaInicio;
           
            LugarPartida = lugarPartida;
            Destino = destino;
            //Carga = carga;
            //Kg = kg;
            Km = km;
            PrecioPorKilo = precioPorKilo;
            this.Remito = remito;
            this.nombreEmpleado = nombreEmpleado;
            this.nombreCliente = nombreCliente;
            //this.Tarifa = tarifa;
            this.toneladas = toneladas;
        }

        public ViajeDTO()
        {
            Id = 0;
            FechaInicio = DateTime.Now;
            FechaEntrega = DateTime.Now;
            LugarPartida = "default";
            Destino = "default";
            Carga = "default";
            Kg = 0.0f;
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

