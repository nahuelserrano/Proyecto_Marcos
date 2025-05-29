using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.Services;
using Proyecto_camiones.Repositories;

namespace Proyecto_camiones.ViewModels
{
    class PagoViewModel
    {
        private PagoService pagoService;

        public PagoViewModel()
        {
            var PagoRepo = new PagoRepository();
            this.pagoService = new PagoService(PagoRepo);
        }

        public async Task<bool> testearConexion()
        {
            return await this.pagoService.ProbarConexionAsync();
        }

        public async Task<Result<int>> CrearAsync(int id_chofer, int id_viaje, float monto_pagado)
        {
            if (this.testearConexion().Result)
            {
                Console.WriteLine("omg entré!!");
                var resultado = await this.pagoService.CrearAsync(id_chofer, id_viaje, monto_pagado);


                Console.WriteLine(resultado + "resultado de viewmodel");
                // Ahora puedes acceder al resultado
                if (resultado > 0)
                {
                    // La operación fue exitosa
                    int idPago = resultado;
                    Console.WriteLine($"pago creado con ID: {idPago}");
                    return Result<int>.Success(resultado);
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al crear el pago: {Result<int>.Failure(nameof(resultado))}");
                    return Result<int>.Failure(nameof(resultado));
                }
            }
            return Result<int>.Failure("La conexión no pude establecerse");
        }





        public async Task<float> ObtenerSueldoCalculado(int id_chofer, DateOnly calcularDesde, DateOnly calcularHasta)
        {
            if (await this.testearConexion())
            {
                return await pagoService.ObtenerSueldoCalculado(id_chofer, calcularDesde, calcularHasta);
            }
            return -1;

        }
    }
}