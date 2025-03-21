//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Proyecto_camiones.Presentacion.Models;

//namespace Proyecto_camiones.Presentacion.Repositories
//{
//    public class CamionRepository
//    {
//        private List<Camion> _camiones; // Lista para pruebas
//        private int _siguienteId;

//        public CamionRepository()
//        {
//            _camiones = new List<Camion>();
//            _siguienteId = 1;

//            // Agregamos algunos camiones de prueba
//            _camiones.Add(new Camion(10000, 5000, "ABC123") { Id = _siguienteId++ });
//            _camiones.Add(new Camion(15000, 7000, "XYZ789") { Id = _siguienteId++ });
//        }

//        public async Task<Camion> ObtenerPorId(int id)
//        {
//            // Simulamos una búsqueda
//            return _camiones.Find(c => c.Id == id);
//        }

//        public async Task<List<Camion>> ObtenerTodos()
//        {
//            // Devolvemos una copia de la lista
//            return new List<Camion>(_camiones);
//        }

//        public async Task<List<Camion>> ObtenerDisponibles(DateTime fecha)
//        {
//            // Simulamos que todos están disponibles
//            return new List<Camion>(_camiones);
//        }

//        public async Task<int> Insertar(Camion camion)
//        {
//            // Asignamos un ID y agregamos a la lista
//            camion.Id = _siguienteId++;
//            _camiones.Add(camion);
//            return camion.Id;
//        }

//        public async Task Actualizar(Camion camion)
//        {
//            // Simulamos actualización
//            int indice = _camiones.FindIndex(c => c.Id == camion.Id);
//            if (indice >= 0)
//            {
//                _camiones[indice] = camion;
//            }
//        }

//        public async Task Eliminar(int id)
//        {
//            // Simulamos eliminación
//            _camiones.RemoveAll(c => c.Id == id);
//        }
//    }
//}