﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.Presentacion.Services
{
    public class CamionService
    {
        private CamionRepository _camionRepository;
        private ChoferRepository _choferRepository;
        private ViajeRepository _viajeRepo;


        public CamionService(CamionRepository camionR)
        {
            this._camionRepository = camionR ?? throw new ArgumentNullException(nameof(camionR));
            this._choferRepository = new ChoferRepository();
            this._viajeRepo = new ViajeRepository();
        }

        //PROBAR CONEXION
        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._camionRepository.ProbarConexionAsync();
            return result;
        }


        //OBTENER TODOS LOS CAMIONES
        public async Task<List<CamionDTO>> ObtenerTodosAsync()
        {
            List<CamionDTO> camiones = await _camionRepository.ObtenerTodosAsync();
            if(camiones != null)
            {
                return camiones;
            }
            return new List<CamionDTO>();
        }


        //CREAR CAMION
        public async Task<Result<int>> CrearAsync(string patente, string nombre)
        {

            try
            {
                // Intentar insertar en la base de datos
                Camion response = await _camionRepository.InsertarAsync(patente, nombre);
                if (response != null)
                {
                    int id = await this._choferRepository.InsertarAsync(nombre);

                    if (id != -1)
                    {
                        return Result<int>.Success(response.Id);
                    }

                    return Result<int>.Failure(MensajeError.ErrorCreacion("camión"));
                }
               return Result<int>.Failure(MensajeError.ErrorCreacion("camión"));
            }
            catch (Exception ex)
            {
                // Si algo sale mal, registrar el error y devolver un mensaje amigable
                //Logger.LogError($"Error al crear camion: {ex.Message}"); 
                return Result<int>.Failure(MensajeError.ErrorCreacion("camión"));
            }
        }

        public async Task<CamionDTO> ObtenerPorIdAsync(int id)
        {
            try
            {
                CamionDTO camion = await _camionRepository.ObtenerPorIdAsync(id);
                return camion;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<Camion>> ObtenerPorPatenteAsync(string patente)
        {
            try
            {
                Camion? camion = await _camionRepository.ObtenerPorPatenteAsync(patente);

                if (camion == null)
                    return Result<Camion>.Failure(MensajeError.EntidadNoEncontrada(nameof(Camion), 0));

                return Result<Camion>.Success(camion);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        internal async Task<Result<CamionDTO>> ActualizarAsync(int id,  string? patente, string? nombre)
        {
            if (id <= 0)
                return Result<CamionDTO>.Failure(MensajeError.IdInvalido(id));
            if (patente == null && nombre == null)
                return Result<CamionDTO>.Failure(MensajeError.ErrorActualizacion("camión"));
            var camionExistente = await _camionRepository.ObtenerPorIdAsync(id);

            if (camionExistente == null) { 
            return Result<CamionDTO>.Failure(MensajeError.objetoNulo(nameof(camionExistente)));
            }

            bool success = await this._camionRepository.ActualizarAsync(id, patente, nombre);
            if (success)
            {
                CamionDTO result = await this._camionRepository.ObtenerPorIdAsync(id);
                return Result<CamionDTO>.Success(result);
            }
            return Result<CamionDTO>.Failure(MensajeError.objetoNulo(nameof(camionExistente)));
        }

        public async Task<Result<string>> EliminarAsync(int id)
        {
            CamionDTO? c = await this._camionRepository.ObtenerPorIdAsync(id);
            if(c == null)
            {
                return Result<string>.Failure("No se encontró un camión con ese id");
            }

            Chofer? ch = await this._choferRepository.ObtenerPorNombreAsync(c.Nombre_Chofer);
            if(ch != null)
            {
                await this._choferRepository.EliminarAsync(ch.Id);
            }

            bool success = await this._camionRepository.EliminarAsync(id);
            
            Console.WriteLine("rompimos acá?");
            if (success)
            {
                Console.WriteLine("holu ya se eliminó el camión");
                return Result<string>.Success("el camion con el id " + id + " fue eliminado correctamente");
            }
            return Result<string>.Failure("el camion con el id " + id + " no pudo ser eliminado");
        }

        internal async Task<Result<String>> ObtenerChofer(string patente)
        {
            Camion camion = await this._camionRepository.ObtenerPorPatenteAsync(patente);
            if(camion != null)
            {
                return Result<String>.Success(camion.nombre_chofer);
            }
            else
            {
                return Result<String>.Failure("No existe camión con esa patente");
            }
        }
    }
}
