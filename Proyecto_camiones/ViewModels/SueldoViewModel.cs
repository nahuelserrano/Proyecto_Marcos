using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.ViewModels
{
    class SueldoViewModel
    {

        private SueldoService sueldoService;
        private PagoService pagoService;

        public SueldoViewModel()
        {
            var dbContext = General.obtenerInstancia();
            var sueldoRepo = new SueldoRepository();
            var pagoRepo = new PagoRepository(General.obtenerInstancia());
            this.pagoService = new PagoService(pagoRepo);
            this.sueldoService = new SueldoService(sueldoRepo,pagoService);
        }

        public async Task<bool> testearConexion()
        {
            return await this.sueldoService.ProbarConexionAsync();
        }

        public async Task<Result<int>> CrearAsync(int Id_Chofer, DateOnly pagoDesde, DateOnly pagoHasta,int idCamion)
        {
            if (await this.testearConexion())
            {
            
                var resultado = await this.sueldoService.CrearAsync(Id_Chofer, pagoDesde, pagoHasta,idCamion);

               
                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    int idSueldo = resultado.Value;
                    Console.WriteLine($"sueldo creado con ID: {idSueldo}");
                    return Result<int>.Success(resultado.Value);
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al crear el sueldo: "+resultado);
                    return Result<int>.Failure(nameof(resultado));
                }
            }
            return Result<int>.Failure("La conexión no pude establecerse");
        }

        public async Task<Result<bool>> marcarPago(int idSueldo)
        {
            if (await this.testearConexion())
            {
                return await this.sueldoService.marcarPagado(idSueldo);
            }
            return Result<bool>.Failure("No se pudo establecer la conexión");
        }
        public async Task<Result<List<SueldoDTO>>> ObtenerTodosAsync(string patente,string? nombreChofer=null) {
            if (await this.testearConexion())
            {
                Result<List<SueldoDTO>> sueldos = await this.sueldoService.ObtenerTodosAsync(patente,nombreChofer);
                if(sueldos!=null)
                return sueldos;
            }
            return Result<List<SueldoDTO>>.Failure("No se pudo establecer la conexión");
        }


        public async Task<Result<bool>> EliminarAsync(int id)   
        {
            if (this.testearConexion().Result)
            {
                return await this.sueldoService.EliminarAsync(id);
            }
            return Result<bool>.Failure("No se pudo establecer la conexión");
        }
    }
}
