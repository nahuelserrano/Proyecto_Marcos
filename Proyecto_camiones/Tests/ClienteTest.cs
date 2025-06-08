using System;
using System.Threading.Tasks;
using Proyecto_camiones.ViewModels;

namespace Proyecto_camiones.Tests
{
    public static class ClienteTests
    {
        /// <summary>
        /// Ejecuta todas las pruebas de Cliente en secuencia
        /// </summary>
        public static async Task EjecutarTodasLasPruebas()
        {
            Console.WriteLine("\n======= INICIANDO PRUEBAS COMPLETAS DE CLIENTE =======\n");

            try
            {
                // 1. Probamos la conexión primero
                await ProbarConexionCliente();

                // 2. Creamos algunos clientes para las pruebas
                int idCliente1 = await ProbarInsertarCliente("TEST_CAMIONES_SRL");
                int idCliente2 = await ProbarInsertarCliente("TEST_DISTRIBUIDORA");

                // 3. Obtenemos por ID
                await ProbarObtenerClientePorId(idCliente1);

                // 4. Probamos obtener todos los clientes
                //await ProbarObtenerTodosClientes();

                // 5. Probamos obtener cliente por nombre
                //await ProbarObtenerClientePorNombre("TEST_CAMIONES_SRL");

                // 6. Probamos actualizar un cliente
                //await ProbarActualizarCliente(idCliente1, "TEST_CAMIONES_ACTUALIZADO");

                // 7. Probamos buscar viajes de un cliente
                //await ProbarBuscarViajesDeCliente("TEST_CAMIONES_ACTUALIZADO");

                // 8. Eliminamos el segundo cliente de prueba
                await ProbarEliminarCliente(idCliente2);

                // 9. Verificamos si realmente se eliminó
                await ProbarObtenerClientePorId(idCliente2);

                Console.WriteLine("\n======= FINALIZADAS TODAS LAS PRUEBAS DE CLIENTE =======\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR FATAL] Error en las pruebas de cliente: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Prueba si se puede establecer la conexión con la base de datos desde ClienteViewModel
        /// </summary>
        public static async Task ProbarConexionCliente()
        {
            Console.WriteLine("\n=== PROBANDO CONEXIÓN DE CLIENTE ===");

            try
            {
                var clienteViewModel = new ClienteViewModel();
                bool conexionExitosa = await clienteViewModel.testearConexion();

                if (conexionExitosa)
                {
                    Console.WriteLine("[ÉXITO] Conexión a la base de datos establecida correctamente");
                }
                else
                {
                    Console.WriteLine("[ERROR] No se pudo establecer conexión a la base de datos");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al probar la conexión de Cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Prueba la inserción de un nuevo cliente
        /// </summary>
        public static async Task<int> ProbarInsertarCliente(string nombre)
        {
            Console.WriteLine($"\n=== INSERTANDO CLIENTE: {nombre} ===");

            try
            {
                var clienteViewModel = new ClienteViewModel();
                var resultado = await clienteViewModel.InsertarCliente(nombre);

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Cliente insertado con ID: {resultado.Value}");
                    return resultado.Value;
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo insertar el cliente: {resultado.Error}");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al insertar cliente: {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Prueba la obtención de un cliente por su ID
        /// </summary>
        public static async Task ProbarObtenerClientePorId(int id)
        {
            Console.WriteLine($"\n=== OBTENIENDO CLIENTE POR ID: {id} ===");

            try
            {
                var clienteViewModel = new ClienteViewModel();
                var resultado = await clienteViewModel.ObtenerById(id);

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Cliente encontrado: {resultado.Value.Id} - {resultado.Value.Nombre}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se encontró el cliente: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al obtener cliente por ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Prueba la obtención de todos los clientes
        /// </summary>
        //public static async Task ProbarObtenerTodosClientes()
        //{
        //}

        /// <summary>
        /// Prueba la obtención de un cliente por nombre (método que debe ser implementado)
        /// </summary>
        //public static async Task ProbarObtenerClientePorNombre(string nombre)
        //{
        //}

        /// <summary>
        /// Prueba la actualización de un cliente
        /// </summary>
        //public static async Task ProbarActualizarCliente(int id, string nuevoNombre)
        //{
        //}

        /// <summary>
        /// Prueba la eliminación de un cliente
        /// </summary>
        public static async Task ProbarEliminarCliente(int id)
        {
            Console.WriteLine($"\n=== ELIMINANDO CLIENTE ID: {id} ===");

            try
            {
                var clienteViewModel = new ClienteViewModel();
                var resultado = await clienteViewModel.Eliminar(id);

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Cliente eliminado correctamente: {resultado.Value}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo eliminar el cliente: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al eliminar cliente: {ex.Message}");
            }
        }
    }
}
