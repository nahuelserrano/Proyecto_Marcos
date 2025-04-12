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
        public double PrecioPorKilo { get; set; }
        public string Remito { get; set; }
        public string nombreEmpleado { get; set; }
        public string nombreCliente { get; set; }

        public ViajeDTO(int id, DateOnly fechaInicio, DateOnly fechaFacturacion, string lugarPartida,
                        string destino, string carga, float kg, int km, double precioPorKilo,
                        string remito, string nombreEmpleado, string nombreCliente)
        {
            Id = id;
            FechaInicio = fechaInicio;
            FechaFacturacion = fechaFacturacion;
            LugarPartida = lugarPartida;
            Destino = destino;
            Carga = carga;
            Kg = kg;
            Km = km;
            PrecioPorKilo = precioPorKilo;
            Remito = remito;
            this.nombreEmpleado = nombreEmpleado;
            this.nombreCliente = nombreCliente;
        }

        public ViajeDTO()
        {
            Id = 0;
            FechaInicio = DateOnly.FromDateTime(DateTime.Now);
            FechaFacturacion = DateOnly.FromDateTime(DateTime.Now);
            LugarPartida = "default";
            Destino = "default";
            Carga = "default";
            Kg = 0.0f;
            Km = 0;
            PrecioPorKilo = 0.0;
            Remito = "default";
            nombreEmpleado = "default";
            nombreCliente = "default";
        }
    }
}
