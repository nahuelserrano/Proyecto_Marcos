using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.Presentacion.Models;

namespace Proyecto_camiones.Presentacion.Repositories
{
    public class ChoferRepository
    {
        private List<Chofer> _choferes;
        private int _siguienteId;

        public ChoferRepository()
        {
            _choferes = new List<Chofer>();
            _siguienteId = 1;

            // Agregamos algunos choferes de prueba
            _choferes.Add(new Chofer("Juan", "Pérez"));
            _choferes.Add(new Chofer("María", "Gómez"));
        }

        public async Task<Chofer> ObtenerPorId(int id)
        {
            // Devolvemos el primer chofer para simular la búsqueda por id
            return _choferes.Count > 0 ? _choferes[0] : null;
        }

        public async Task<List<Chofer>> ObtenerTodos()
        {
            // Devolvemos una copia de la lista
            return new List<Chofer>(_choferes);
        }

        public async Task<List<Chofer>> ObtenerDisponibles(DateTime fecha)
        {
            // Simulamos que todos están disponibles
            return new List<Chofer>(_choferes);
        }

        public async Task<int> Insertar(Chofer chofer)
        {
            // Agregamos a la lista
            _choferes.Add(chofer);
            return _siguienteId++;
        }

        public async Task Actualizar(Chofer chofer)
        {
            // En una maqueta simple no necesitamos implementar la actualización real
            // Solo simulamos que funciona correctamente
        }

        public async Task Eliminar(int id)
        {
            // Simulamos eliminación (en una implementación real buscaríamos por ID)
            if (_choferes.Count > 0)
            {
                _choferes.RemoveAt(0);
            }
        }
    }
}