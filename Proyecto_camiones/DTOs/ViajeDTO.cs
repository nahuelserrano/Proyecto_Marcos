﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class ViajeDTO
    {
        public int Id { get; set; } // ID del viaje, útil para futuras referencias en el frontend
        public DateOnly FechaInicio { get; set; }
        public string LugarPartida { get; set; }
        public string Destino { get; set; }
        public int Remito { get; set; }
        public float Kg { get; set; }
        public string Carga { get; set; }
        public string NombreCliente { get; set; } // Obtenido por join con la entidad Cliente
        public string NombreChofer { get; set; } // Obtenido por join con la entidad Chofer
        public float Km { get; set; }
        public float Tarifa { get; set; }
        public float PorcentajeChofer { get; set; } // Porcentaje por defecto del chofer

        public string PatenteCamion { get; set; }
        public float Total => Tarifa * Kg;
        public float GananciaChofer => Total * PorcentajeChofer / 100;

        public int Camion { get; internal set; }

        public ViajeDTO(int id, DateOnly fechaInicio, string lugarPartida,
            string destino, int remito, float kg, string carga, string nombreCliente,
            string nombreChofer, float km, float tarifa, float porcentajeChofer, string patenteCamion)
        {
            Id = id;
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
            PatenteCamion = patenteCamion;
        }
        public ViajeDTO(DateOnly fechaInicio, string lugarPartida,
            string destino, int remito, float kg, string carga, string nombreCliente,
            string nombreChofer, float km, float tarifa, float porcentajeChofer, string patenteCamion)
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
            PatenteCamion = patenteCamion;
        }

        public ViajeDTO(DateOnly fechaInicio, string lugarPartida, string destino, int remito, float kg, string carga, string nombreCliente, string nombreChofer, float km, float tarifa, float porcentajeChofer, string patenteCamion, int camion) : this(fechaInicio, lugarPartida, destino, remito, kg, carga, nombreCliente, nombreChofer, km, tarifa, porcentajeChofer)
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
            PorcentajeChofer = 18.0f; // Porcentaje por defecto del chofer
            PatenteCamion = "default";
        }

        public ViajeDTO(DateOnly fechaInicio, string lugarPartida, string destino, int remito, float kg, string carga, string nombreCliente, string nombreChofer, float km, float tarifa, float porcentajeChofer)
        {
            FechaInicio = fechaInicio;
            LugarPartida = lugarPartida;
            Destino = destino;
            Remito = remito;
            Kg = kg;
            Carga = carga;
            NombreCliente = nombreCliente;
            NombreChofer = nombreChofer;
            Km = km;
            Tarifa = tarifa;
            PorcentajeChofer = porcentajeChofer;
        }

        override
            public string ToString()
        {
            return $"Total: {Total}";
        }
    }
}

