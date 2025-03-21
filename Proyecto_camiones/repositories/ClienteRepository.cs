//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Proyecto_camiones.Presentacion.Models;

//namespace Proyecto_camiones.Presentacion.Repositories
//{
//    public class ClienteRepository
//    {
//        private List<Cliente> _clientes;
//        private int _siguienteId;

//        public ClienteRepository()
//        {
//            _clientes = new List<Cliente>();
//            _siguienteId = 1;

//            // Agregamos clientes de prueba
//            _clientes.Add(new Cliente(null, "Empresa A", 11111111));
//            _clientes.Add(new Cliente(null, "Empresa B", 22222222));
//        }

//        public async Task<Cliente> ObtenerPorId(int id)
//        {
//            // Devolvemos el primer cliente para simplificar
//            return _clientes.Count > 0 ? _clientes[0] : null;
//        }

//        public async Task<List<Cliente>> ObtenerTodos()
//        {
//            // Devolvemos una copia de la lista
//            return new List<Cliente>(_clientes);
//        }

//        public async Task<int> Insertar(Cliente cliente)
//        {
//            // Agregamos a la lista
//            _clientes.Add(cliente);
//            return _siguienteId++;
//        }

//        public async Task Actualizar(Cliente cliente)
//        {
//            // Simulamos actualización (no hacemos nada real en la maqueta)
//        }

//        public async Task Eliminar(int id)
//        {
//            // Simulamos eliminación (en implementación real buscaríamos por ID)
//            if (_clientes.Count > 0)
//            {
//                _clientes.RemoveAt(0);
//            }
//        }

//        public async Task<List<Viaje>> ObtenerHistorialViajes(int clienteId)
//        {
//            // Devolvemos una lista vacía de viajes para simular
//            return new List<Viaje>();
//        }
//    }
//}
