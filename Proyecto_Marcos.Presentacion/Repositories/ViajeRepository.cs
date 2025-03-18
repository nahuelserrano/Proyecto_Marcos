using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_Marcos.Presentacion.Models;

namespace Proyecto_Marcos.Presentacion.Repositories
{
    public class ViajeRepository
    {
        private List<Viaje> _viajes;
        private int _siguienteId;

        public ViajeRepository()
        {
            _viajes = new List<Viaje>();
            _siguienteId = 1;

            // Creamos un viaje de prueba
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = fechaInicio.AddDays(3);

            Chofer chofer = new Chofer("Carlos", "Rodríguez");
            Camion camion = new Camion(12000, 6000, "DEF456") { Id = 1 };
            Cliente cliente = new Cliente(null, "Empresa", 12345678);

            Viaje viaje = new Viaje(fechaInicio, "Buenos Aires", "Córdoba", 5000, 12345, 10, 8000, chofer, cliente)
            {
                FechaInicio = fechaInicio,
                FechaEntrega = fechaFin,
                KilosCarga = 8000,
                CamionId = camion.Id
            };

            _viajes.Add(viaje);
        }

        public async Task<Viaje> ObtenerPorId(int id)
        {
            // Devolvemos el primer viaje para simplificar
            return _viajes.Count > 0 ? _viajes[0] : null;
        }

        public async Task<List<Viaje>> ObtenerTodos()
        {
            // Devolvemos una copia de la lista
            return new List<Viaje>(_viajes);
        }

        public async Task<List<Viaje>> ObtenerPorFiltro()
        {
            // Simulamos una búsqueda filtrada
            return new List<Viaje>(_viajes);
        }

        public async Task<int> Insertar(Viaje viaje)
        {
            // Agregamos a la lista
            _viajes.Add(viaje);
            return _siguienteId++;
        }

        public async Task Actualizar(Viaje viaje)
        {
            // Simulamos actualización (no hacemos nada real en la maqueta)
        }

        public async Task Actualizar(int id, Viaje viaje)
        {
            // Simulamos actualización (no hacemos nada real en la maqueta)
        }

        public async Task Eliminar(int id)
        {
            // Simulamos eliminación (en implementación real buscaríamos por ID)
            if (_viajes.Count > 0)
            {
                _viajes.RemoveAt(0);
            }
        }
    }
}