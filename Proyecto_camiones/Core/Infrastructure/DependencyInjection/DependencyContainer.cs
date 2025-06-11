using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.Presentacion;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.ViewModels;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.Services;
using Proyecto_camiones.Core.Services;
using System;

namespace Proyecto_camiones.Core.Infrastructure.DependencyInjection
{
    public static class DependencyContainer
    {
        /// <summary>
        /// Configura todas las dependencias del sistema 
        /// </summary>
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // === CONFIGURACIÓN DE BASE DE DATOS ===
            var connectionString = "server=localhost;user=root;password=;database=truck_manager_project_db;";

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                       .EnableSensitiveDataLogging(false) // En producción, esto debe estar en false
                       .EnableDetailedErrors(false);      // Solo para desarrollo
            });

            // === REPOSITORIOS ===
            services.AddScoped<CamionRepository>();
            services.AddScoped<ChoferRepository>();
            services.AddScoped<ClienteRepository>();
            services.AddScoped<ViajeRepository>();
            services.AddScoped<PagoRepository>();
            services.AddScoped<ChequeRepository>();
            services.AddScoped<FleteRepository>();
            services.AddScoped<SueldoRepository>();
            services.AddScoped<CuentaCorrienteRepository>();
            services.AddScoped<ViajeFleteRepository>();

            // === SERVICIOS ===
            // Los verdaderos héroes que manejan la lógica de negocios
            services.AddScoped<ICamionService, CamionService>();
            services.AddScoped<IChoferService, ChoferService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IViajeService, ViajeService>();
            services.AddScoped<IPagoService, PagoService>();
            services.AddScoped<IChequeService, ChequeService>();
            services.AddScoped<IFleteService, FleteService>();
            services.AddScoped<ISueldoService, SueldoService>();
            services.AddScoped<ICuentaCorrienteService, CuentaCorrienteService>();
            services.AddScoped<IViajeFleteService, ViajeFleteService>();

            // === VIEWMODELS ===
            // Los intermediarios que manejan la UI
            services.AddTransient<CamionViewModel>();
            services.AddTransient<ChoferViewModel>();
            services.AddTransient<ClienteViewModel>();
            services.AddTransient<ViajeViewModel>();
            services.AddTransient<PagoViewModel>();
            services.AddTransient<ChequeViewModel>();
            services.AddTransient<FleteViewModel>();
            services.AddTransient<SueldoViewModel>();
            services.AddTransient<CuentaCorrienteViewModel>();
            services.AddTransient<ViajeFleteViewModel>();

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Método de extensión para obtener servicios 
        /// </summary>
        public static T GetService<T>(this IServiceProvider serviceProvider) where T : class
        {
            var service = serviceProvider.GetService<T>();

            if (service == null)
                throw new InvalidOperationException($"¡SANTO PATRÓN DE DISEÑO! No se pudo obtener el servicio {typeof(T).Name}. " +
                    "¿Está registrado en el contenedor de dependencias?");

            return service;
        }

        /// <summary>
        /// Obtiene una instancia del ApplicationDbContext configurado
        /// </summary>
        public static ApplicationDbContext GetDbContext(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<ApplicationDbContext>();
        }
    }
}