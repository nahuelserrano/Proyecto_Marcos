﻿using MySqlX.XDevAPI.Common;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_camiones.ViewModels
{
    public class FleteViewModel
    {

        public FleteService fleteService;

        public FleteViewModel()
        {
            var repo = new FleteRepository();
            fleteService = new FleteService(repo);
        }

        public async Task<bool> TestearConexion()
        {
            return await this.fleteService.TestearConexion();
        }

        public async Task<Result<int>> InsertarAsync(string nombre)
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this.fleteService.InsertarFletero(nombre);
            }
            return Result<int>.Failure("No se pudo establecer la conexion con la base de datos");
        }

        public async Task<Result<Flete>> ObtenerFletePorNombreAsync(string nombre)
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this.fleteService.ObtenerPorNombreAsync(nombre);
            }
            return Result<Flete>.Failure("No se pudo establecer la conexion con la base de datos");
        }

        public async Task<Result<List<Flete>>> ObtenerTodosAsync()
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this.fleteService.ObtenerTodosAsync();
            }
            return Result<List<Flete>>.Failure("No se pudo establecer la conexión con la base de datos");
        }

        public async Task<Result<Flete>> ObtenerPorIdAsync(int id)
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this.fleteService.ObtenerPorIdAsync(id);
            }
            return Result<Flete>.Failure("No se pudo establecer la conexión con la base de datos");
        }

        public async Task<Result<bool>> EliminarAsync(int id)
        {
            MessageBox.Show("conexión");
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                MessageBox.Show("conexión");
                return await this.fleteService.EliminarAsync(id);
            }
            return Result<bool>.Failure("No se pudo establecer la conexión con la base de datos");
        }

        public async Task<Result<Flete>> ActualizarAsync(int id, string? nombre)
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this.fleteService.ActualizarAsync(id, nombre);
            }
            return Result<Flete>.Failure("No se pudo establecer la conexión con la base de datos");
        }
    }
}
