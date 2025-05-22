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

        public async Task<Result<int>> CrearAsync(string nombre_chofer, DateOnly pagoDesde, DateOnly pagoHasta, DateOnly? fechaPago, string? patente_camion)
        {
            if (await this.testearConexion())
            {
                Console.WriteLine("hola view model hay conexion");
                return await this.sueldoService.CrearAsync(nombre_chofer, pagoDesde, pagoHasta,fechaPago, patente_camion);

            }
            return Result<int>.Failure("La conexión no pude establecerse");
        }

        public async Task<Result<SueldoDTO>> marcarPago(int idSueldo, DateOnly? fecha_pagado)
        {
            if (await this.testearConexion())
            {
                return await this.sueldoService.marcarPagado(idSueldo, fecha_pagado);
            }
            return Result<SueldoDTO>.Failure("No se pudo establecer la conexión");
        }
        public async Task<Result<List<SueldoDTO>>> ObtenerTodosAsync(string? patente,string? nombreChofer) {
            if (await this.testearConexion())
            {
                Console.WriteLine("hola view model");
                Result<List<SueldoDTO>> sueldos = await this.sueldoService.ObtenerTodosAsync(patente, nombreChofer);
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
