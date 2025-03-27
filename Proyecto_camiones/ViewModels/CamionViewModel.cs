using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.ViewModels
{
    public class CamionViewModel
    {
        private CamionService _camionService;

        public CamionViewModel()
        {
            var dbContext = General.obtenerInstancia();
            var camionRepository = new CamionRepository(dbContext);
            this._camionService = new CamionService(camionRepository);
        }

        public async Task<bool> testearConexion()
        {
            return await this._camionService.ProbarConexionAsync();
        }

        public async Task<int> InsertarCamion(float peso_max, float tara, string patente)
        {
            if (this.testearConexion().Result)
            {
                Console.WriteLine("omg entré!!");
                var resultado = await this._camionService.CrearCamionAsync(150, 100, "WWW123");

                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    int idCamion = resultado.Value;
                    Console.WriteLine($"Camión creado con ID: {idCamion}");
                    return resultado.Value;
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al crear el camión: {resultado.Error}");
                }
            }
            return -1; //provisorio, averiguar que tipo de respuesta da, un simil response entity de java
        }

        public async Task<List<CamionDTO>> ObtenerTodos() 
        {
            if (this.testearConexion().Result)
            {
                var camiones = await this._camionService.ObtenerCamionesAsync();
                Console.WriteLine("no rompió ante la llamada");
                return camiones;
            }
            return new List<CamionDTO>();
        }

        public async Task<CamionDTO> Actualizar(int id, float? peso_max, float? tara, string? patente)
        {
            if (this.testearConexion().Result)
            {
                CamionDTO camion = await this._camionService.Actualizar(id, peso_max, tara, patente);
                if(camion != null)
                {
                    return camion;
                }
            }
            return null;
        }


    }
}
