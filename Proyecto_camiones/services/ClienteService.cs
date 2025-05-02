﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.ViewModels;


namespace Proyecto_camiones.Presentacion.Services
{
    public class ClienteService
    {
        private ClienteRepository _clienteRepository;
        private ViajeFleteRepository _viajeFleteRepository;
        private ViajeRepository _viajeRepository;

        public ClienteService(ClienteRepository clienteRepository)
        {
            this._clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(_clienteRepository));
            this._viajeFleteRepository = new ViajeFleteRepository();
            this._viajeRepository = new ViajeRepository(General.obtenerInstancia());
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._clienteRepository.ProbarConexionAsync();
            return result;
        }

        public async Task<Result<Cliente>> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                return Result<Cliente>.Failure(MensajeError.idInvalido(id));

            Cliente? cliente = await this._clienteRepository.ObtenerPorIdAsync(id);

            if (cliente == null)
                return Result<Cliente>.Failure(MensajeError.objetoNulo(nameof(cliente)));

            return Result<Cliente>.Success(cliente);
        }


        public async Task<Result<bool>> EliminarAsync(int clienteId)
        {
            if (clienteId <= 0) return Result<bool>.Failure(MensajeError.idInvalido(clienteId));

            bool result = await this._clienteRepository.EliminarAsync(clienteId);

            if(result) return Result<bool>.Success(true);
            return Result<bool>.Failure("No se pudo eliminar el cliente o no existe un cliente con ese id");

        }

        public async Task<Result<int>> InsertarAsync(string nombre)
        {
            ValidadorCliente validador = new ValidadorCliente(nombre);
            Result<bool> resultadoValidacion = validador.ValidarCompleto();

        //    if (!resultadoValidacion.IsSuccess)
        //        return Result<int>.Failure(resultadoValidacion.Error);

            Cliente cliente = await _clienteRepository.InsertarAsync(nombre);
            if(cliente != null)return Result<int>.Success(cliente.Id);
            return Result<int>.Failure("Error al insertar el cliente");
        }



        public async Task<Result<Cliente>> ActualizarAsync(int id, String nombre, String apellido)
        {
            //lo dejo en para que lo chequeen, si mandamos el id para corregir a la funcion del
            //validador tambien nos hace mandarlo en crear. por eso propongo dejar el chequeo aca
            if (id < 0)
                return Result<Cliente>.Failure("El id es inválido");

            Cliente cliente = await _clienteRepository.ActualizarAsync(id, nombre);
            if (cliente != null) return Result<Cliente>.Success(cliente);

            return Result<Cliente>.Failure("No existe un cliente con ese id");
        }

        
        internal async Task<Result<List<ViajeMixtoDTO>>> ObtenerViajesDeUnClienteAsync(string cliente)
        {
            Cliente? c = _clienteRepository.ObtenerPorNombreAsync(cliente).Result;
            if(c != null)
            {
                List<ViajeMixtoDTO> viajesFlete = await this._viajeFleteRepository.ObtenerViajesDeUnClienteAsync(c.Id);
                List<ViajeMixtoDTO> viajes = await this._viajeRepository.ObtenerViajeMixtoPorClienteAsync(c.Id);
                if(viajesFlete != null && viajes != null)
                {
                    viajesFlete.AddRange(viajes);
                    return Result<List<ViajeMixtoDTO>>.Success(viajesFlete);
                }
                return Result<List<ViajeMixtoDTO>>.Failure("No se pudieron obtener los viajes");
            }
            return Result<List<ViajeMixtoDTO>>.Failure("No existe un cliente con ese nombre");
        }

        public async Task<Result<List<Cliente>>> ObtenerTodosAsync()
        {
            List<Cliente> clientes = await this._clienteRepository.ObtenerTodosAsync();
            if(clientes != null)
            {
                return Result<List<Cliente>>.Success(clientes);
            }
            else
            {
                return Result<List<Cliente>>.Failure("No se pudieron obtener los clientes");
            }
        }
    }
}
