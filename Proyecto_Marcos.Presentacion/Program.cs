using Proyecto_Marcos.Presentacion.Repositories;
using Proyecto_Marcos.Presentacion.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


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


            ViajeService viajeService = new ViajeService(ReposViaje,ServCamion,ServChofer);
        }
    }
}
