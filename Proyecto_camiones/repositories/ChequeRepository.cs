using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Proyecto_camiones.Presentacion.Repositories
{

    public class ChequeRepository
    {
        private readonly ApplicationDbContext _context;


        public ChequeRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> ProbarConexionAsync()
        {
            try
            {
                // Intentar comprobar si la conexión a la base de datos es exitosa
                bool puedeConectar = await _context.Database.CanConnectAsync();
                if (puedeConectar)
                {
                    Console.WriteLine("Conexión exitosa a la base de datos.");
                }
                else
                {
                    Console.WriteLine("No se puede conectar a la base de datos.");
                }

                return puedeConectar;
            }
            catch (Exception ex)
            {
                // Si ocurre un error (por ejemplo, si la base de datos no está disponible)
                Console.WriteLine($"Error al intentar conectar: {ex.Message}");
                return false;
            }
        }

        public async Task<int> InsertarAsync(int id_Cliente, DateOnly FechaIngresoCheque, String NumeroCheque, float Monto, string Banco, DateOnly FechaCobro)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return -1;
                }


                var cheque = new Cheque(id_Cliente, FechaIngresoCheque, NumeroCheque, Monto, Banco, FechaCobro);


                // Agregar el cheque a la base de datos (esto solo marca el objeto para insertar)
                _context.Cheques.Add(cheque);

                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    return cheque.Id;
                }

                Console.WriteLine("No se insertó ningún registro");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar cheque: {ex.Message}");

                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return -1;
            }
        }
        public async Task<List<ChequeDTO>?> ObtenerTodosAsync()
        {
            try
            {
                var cheques = await _context.Cheques.Select(c => new ChequeDTO
                {
                    Id_cliente = c.Id_Cliente,
                    FechaIngresoCheque = c.FechaIngresoCheque,
                    NumeroCheque = c.NumeroCheque,
                    Monto = c.Monto,
                    Banco = c.Banco,
                    FechaCobro = c.FechaCobro
                }).ToListAsync();

                return cheques;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                throw;
            }
        }
        public async Task<bool> ActualizarAsync(
                                            int id,
                                            int? id_Cliente = null,
                                            DateOnly? FechaIngresoCheque = null,
                                            String? NumeroCheque = null,
                                            string? Banco = null,
                                            float? Monto = null,
                                            DateOnly? FechaCobro = null
                                        )

        {

            try
            {
                var cheque = await _context.Cheques.FindAsync(id);

                if (cheque == null)
                {
                    Console.WriteLine($"Cheque con ID {id} no encontrado.");
                    return false;
                }

                // Actualizar solo los campos proporcionados
                if (id_Cliente.HasValue)
                    cheque.Id_Cliente = id_Cliente.Value;

                if (FechaIngresoCheque.HasValue)
                    cheque.FechaIngresoCheque = FechaIngresoCheque.Value;

                if (!string.IsNullOrEmpty(NumeroCheque))
                    cheque.NumeroCheque = NumeroCheque;

                if (Monto.HasValue)
                    cheque.Monto = Monto.Value;

                if (!string.IsNullOrEmpty(Banco))
                    cheque.Banco = Banco;

                if (FechaCobro.HasValue)
                    cheque.FechaCobro = FechaCobro.Value;


                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar cheque: {ex.Message}");
                return false;
            }
        }

        public async Task<ChequeDTO?> ObtenerPorNumeroChequeAsync(string nroCheque)
        {
            try
            {
                Cheque? cheque = await _context.Cheques
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.NumeroCheque.Equals(nroCheque));

                Console.WriteLine($"Cheque");


                if (cheque != null)
                {
                    Console.WriteLine($"Cheque encontrado: nuemro cheque {cheque.NumeroCheque}, Monto: {cheque.Monto}");
                    ChequeDTO nuevo = new ChequeDTO(cheque.Id_Cliente, cheque.FechaIngresoCheque, cheque.NumeroCheque, cheque.Monto, cheque.Banco, cheque.FechaCobro);
                    return nuevo;
                }

                return null;    
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener cheque con numero de cheque:  {nroCheque}, exception: {ex}");
                return null;
            }
        }

        public async Task<bool> EliminarAsync(string nroCheque)
        {
            try
            {
                Cheque? cheque = await _context.Cheques
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.NumeroCheque.Equals(nroCheque));

                if (cheque == null)
                    return false;

                _context.Cheques.Remove(cheque);

                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados == 0)
                {
                    Console.WriteLine($"No hay registros afectados");
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Repository: Error al eliminar cheque con el numero de cheque: {nroCheque}");
                Console.WriteLine(e.InnerException);
                return false;
            }
        }
    }
}