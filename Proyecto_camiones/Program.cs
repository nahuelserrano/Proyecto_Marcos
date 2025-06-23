using Proyecto_camiones.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_camiones.Tests;
using MySqlX.XDevAPI.Common;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Presentacion.Repositories;

namespace Proyecto_camiones
{
    static class Program
    {


        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// 


        [STAThread]
        static async Task Main(string[] args)

        {
            // Llamada a Windows Forms para inicializar la aplicación
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Front.Viaje()); // Ejecuta el formulario principal

            //await CamionTest.ProbarEliminacionCamionConPagosPendientes();
            //await CamionTest.EjecutarPruebasEliminacionConPagos();
            //await ViajeTest.ProbarFuzzyMatchingClientes();
            //await ClienteTests.ProbarEliminarCliente(7);

            ViajeViewModel vm = new ViajeViewModel();
            var result = await vm.ActualizarAsync(4, null, "ituzaingó", "chacarita", 98000, null, 900, "cooperativa", null, 89.2F, null, 15);
            if (result.IsSuccess)
            {
                Console.WriteLine(result.Value);
            }
            else
            {
                Console.WriteLine(result.Error);
            }

             
        }
    }
}

