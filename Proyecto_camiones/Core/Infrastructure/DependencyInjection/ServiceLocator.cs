using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Proyecto_camiones.Core.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Clase estática para acceso global al contenedor de dependencias
    /// </summary>
    public static class ServiceLocator
    {
        private static IServiceProvider? _serviceProvider;

        /// <summary>
        /// Inicializa el localizador de servicios
        /// DEBE llamarse al inicio de la aplicación
        /// </summary>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Obtiene un servicio del contenedor
        /// </summary>
        public static T GetService<T>() where T : class
        {
            if (_serviceProvider == null)
                throw new InvalidOperationException("¡PÁNICO! ServiceLocator no ha sido inicializado. " +
                                                    "Llama a ServiceLocator.Initialize() al inicio de tu aplicación.");

            return _serviceProvider.GetService<T>();
        }

        /// <summary>
        /// Obtiene un servicio requerido del contenedor
        /// </summary>
        public static T GetRequiredService<T>() where T : class
        {
            if (_serviceProvider == null)
                throw new InvalidOperationException("¡PÁNICO TOTAL! ServiceLocator no ha sido inicializado.");

            var service = _serviceProvider.GetService<T>();

            if (service == null)
                throw new InvalidOperationException($"¡ERROR CRÍTICO! El servicio {typeof(T).Name} no está registrado.");

            return service;
        }

        /// <summary>
        /// Crear un nuevo scope para operaciones específicas
        /// </summary>
        public static IServiceScope CreateScope()
        {
            if (_serviceProvider == null)
                throw new InvalidOperationException("ServiceLocator no inicializado.");

            return _serviceProvider.CreateScope();
        }
    }
}
