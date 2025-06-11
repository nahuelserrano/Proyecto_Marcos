using Proyecto_camiones.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_camiones.Tests;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_camiones.Core.Infrastructure.DependencyInjection;
using Proyecto_camiones.Presentacion;

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

            try
            {
                // === INICIALIZACIÓN CORRECTA ===
                Console.WriteLine("🚀 Inicializando el contenedor de dependencias...");

                var serviceProvider = DependencyContainer.ConfigureServices();
                ServiceLocator.Initialize(serviceProvider);

                // === CONFIGURAR WINFORMS ===
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // === CREAR FORMULARIO PRINCIPAL CON DEPENDENCIAS ===
                // Aquí puedes crear tu formulario principal usando el ViewModelFactory
                var mainForm = new Front.Viaje(); // O el formulario que quieras como principal

                Console.WriteLine("✅ Aplicación inicializada correctamente");
                Application.Run(mainForm);

            }
            catch (Exception ex)
            {
                // Manejo de errores como un profesional
                Console.WriteLine($"💥 Error crítico durante la inicialización: {ex.Message}");
                MessageBox.Show(
                    $"Error crítico durante la inicialización:\n\n{ex.Message}\n\nLa aplicación se cerrará.",
                    "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Crea el formulario principal con todas las dependencias inyectadas
        /// </summary>
    }

    /// <summary>
    /// Clase helper para obtener ViewModels con inyección de dependencias desde formularios
    /// </summary>
    public static class ViewModelFactory
    {
        /// <summary>
        /// Obtiene una instancia de CamionViewModel con todas sus dependencias inyectadas
        /// </summary>
        public static CamionViewModel CreateCamionViewModel()
        {
            return ServiceLocator.GetRequiredService<CamionViewModel>();
        }

        /// <summary>
        /// Obtiene una instancia de ViajeViewModel con todas sus dependencias inyectadas
        /// </summary>
        public static ViajeViewModel CreateViajeViewModel()
        {
            return ServiceLocator.GetRequiredService<ViajeViewModel>();
        }

        /// <summary>
        /// Obtiene una instancia de ChoferViewModel con todas sus dependencias inyectadas
        /// </summary>
        public static ChoferViewModel CreateChoferViewModel()
        {
            return ServiceLocator.GetRequiredService<ChoferViewModel>();
        }

        /// <summary>
        /// Obtiene una instancia de ClienteViewModel con todas sus dependencias inyectadas
        /// </summary>
        public static ClienteViewModel CreateClienteViewModel()
        {
            return ServiceLocator.GetRequiredService<ClienteViewModel>();
        }

        /// <summary>
        /// Obtiene una instancia de ChequeViewModel con todas sus dependencias inyectadas
        /// </summary>
        public static ChequeViewModel CreateChequeViewModel()
        {
            return ServiceLocator.GetRequiredService<ChequeViewModel>();
        }

        public static SueldoViewModel CreateSueldoViewModel()
        {
            return ServiceLocator.GetRequiredService<SueldoViewModel>();
        }

        public static ViajeFleteViewModel CreateViajeFleteViewModel()
        {
            return ServiceLocator.GetRequiredService<ViajeFleteViewModel>();
        }

        public static FleteViewModel CreateFleteViewModel()
        {
            return ServiceLocator.GetRequiredService<FleteViewModel>();
        }

        public static CuentaCorrienteViewModel CreateCuentaCorrienteViewModel()
        {
            return ServiceLocator.GetRequiredService<CuentaCorrienteViewModel>();
        }

        /// <summary>
        /// Obtiene una instancia de cualquier ViewModel genérico
        /// </summary>
        public static T CreateViewModel<T>() where T : class
        {
            return ServiceLocator.GetRequiredService<T>();
        }
    }
}

