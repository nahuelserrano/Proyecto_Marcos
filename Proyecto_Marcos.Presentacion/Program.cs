using Proyecto_Marcos.Presentacion.Repositories;
using Proyecto_Marcos.Presentacion.Services;
using System;
using Proyecto_Marcos.Presentacion.Models;


namespace Proyecto_Marcos.Presentacion
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)

        {
            ViajeRepository ReposViaje = new Repositories.ViajeRepository();
            CamionRepository ReposCamion = new Repositories.CamionRepository();
            ChoferRepository ReposChofer = new Repositories.ChoferRepository();
            CamionService ServCamion = new Services.CamionService(ReposCamion);
            ChoferService ServChofer = new Services.ChoferService(ReposChofer);
            ViajeService ServViaje = new Services.ViajeService(ReposViaje, ServCamion, ServChofer);
            Camion camion = new Camion(10000, 200, "AAX2000");
            Chofer chofer = new Chofer("nahuel","serrano");
            Cliente cliente = new Cliente("manteca", "mantecoso", 12345678);
            DateTime fecha = new DateTime(2025, 03, 20);
            DateTime fechaEntrega = new DateTime(2025, 04, 20);


            ViajeService viajeService = new ViajeService(ReposViaje,ServCamion,ServChofer);
            Viaje viaje = new Viaje("olava", "tandil", 3, 334, 233, 344, chofer, cliente, camion, fecha, fechaEntrega, 88, camion.Id);
        

            System.Console.WriteLine(viajeService.CrearViaje(viaje).Result.Value);
        }
    }
}
