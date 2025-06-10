﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.ViewModels;

namespace Proyecto_camiones.Presentacion.Repositories
{
    public class CamionRepository
    {
        private ApplicationDbContext _context;

        public CamionRepository()
        {
            _context = General.obtenerInstancia();
        }

        public async Task<bool> ProbarConexionAsync()
        {
            //MessageBox.Show("testeando conexión");
            try
            {
                this._context = General.obtenerInstancia();
                // Intentar comprobar si la conexión a la base de datos es exitosa
                bool puedeConectar = await _context.Database.CanConnectAsync();
                return puedeConectar;
            }
            catch (Exception ex)
            {
                // Si ocurre un error (por ejemplo, si la base de datos no está disponible)
                return false;
            }
        }

        //agrego el signo de pregunta luego de Camion para decir que el result puede ser null
        public async Task<Camion?> InsertarAsync(string patente, string nombre)
        {
            try
            {
                this._context = General.obtenerInstancia();
                if (!await _context.Database.CanConnectAsync())
                {
                    return null;
                }

                var camion = new Camion(patente, nombre);

                // Agregar el camión a la base de datos (esto solo marca el objeto para insertar)
                _context.Camiones.Add(camion);

                // Guardar los cambios en la base de datos (aquí se genera el SQL real y se ejecuta)
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    return camion;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<CamionDTO>?> ObtenerTodosAsync()
        {
            try
            {
                var camiones = await _context.Camiones.Select(c => new CamionDTO
                {
                    Id = c.Id,
                    Patente = c.Patente,
                    Nombre_Chofer = c.nombre_chofer
                }).OrderByDescending(c=> c.Id)
                    .ToListAsync();

                return camiones;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<bool> ActualizarAsync(int id, string? patente, string? nombre)
        {
            try
            {
                this._context = General.obtenerInstancia();
                var camion = await _context.Camiones.FindAsync(id);
                if (camion == null) return false;


                if (!string.IsNullOrEmpty(patente))  // Mejor verificación para strings
                {
                    camion.Patente = patente;
                }

                if (!string.IsNullOrEmpty(nombre))  // Mejor verificación para strings
                {
                    camion.nombre_chofer = nombre;
                }


                await _context.SaveChangesAsync();
                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal async Task<CamionDTO?> ObtenerPorIdAsync(int id)
        {
            try
            {
                Camion camion = await _context.Camiones.FindAsync(id);
                if (camion != null)
                {
                    CamionDTO nuevo = new CamionDTO(camion.Id, camion.Patente, camion.nombre_chofer);
                    return nuevo;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                this._context = General.obtenerInstancia();
                var camion = await _context.Camiones.FindAsync(id);

                if (camion == null)
                    return false;

                _context.Camiones.Remove(camion);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Camion> ObtenerPorPatenteAsync(string patente)
        {
            try
            {
                var camion = await _context.Camiones.FirstOrDefaultAsync(c => c.Patente == patente);
                return camion;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}