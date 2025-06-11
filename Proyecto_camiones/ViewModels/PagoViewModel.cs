using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Threading.Tasks;
using Proyecto_camiones.Core.Services;

namespace Proyecto_camiones.ViewModels
{
    class PagoViewModel
    {
        private readonly IPagoService _pagoService;

        public PagoViewModel(IPagoService pagoService)
        {
            _pagoService = pagoService ?? throw new ArgumentNullException(nameof(pagoService));
        }

        public async Task<bool> testearConexion()
        {
            return await this._pagoService.ProbarConexionAsync();
        }

        public async Task<Result<int>> CrearAsync(int id_chofer, int id_viaje, float monto_pagado)
        {
            if (this.testearConexion().Result)
            {
                var resultado = await this._pagoService.CrearAsync(id_chofer, id_viaje, monto_pagado);

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
                return await _pagoService.ObtenerSueldoCalculado(id_chofer, calcularDesde, calcularHasta);
            }
            return -1;

        }
    }
}