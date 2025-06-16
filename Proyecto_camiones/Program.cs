using Proyecto_camiones.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_camiones.Tests;
using MySqlX.XDevAPI.Common;
using Proyecto_camiones.Presentacion.Utils;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Front.Viaje()); // Ejecuta el formulario principal

            //await CamionTest.ProbarEliminacionCamionConPagosPendientes();
            //await CamionTest.EjecutarPruebasEliminacionConPagos();
            //await ViajeTest.ProbarFuzzyMatchingClientes();
            //await ClienteTests.ProbarEliminarCliente(7);
        }
    }
}

