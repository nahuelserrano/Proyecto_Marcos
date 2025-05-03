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
                Console.WriteLine("rompió acá no??");
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

        public async Task<int> Insertar(int id_Cliente, DateOnly FechaIngresoCheque, string NumeroCheque, float Monto, string Banco, DateOnly FechaCobro)
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

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    return 1;
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
        public async Task<List<ChequeDTO>?> ObtenerTodos()
        {
            var cheques = await _context.Cheques.Select(c => new ChequeDTO
            {
                Id_cliente = c.id_Cliente,
                FechaIngresoCheque = c.FechaIngresoCheque,
                NumeroCheque = c.NumeroCheque,
                Monto = c.Monto,
                Banco = c.Banco,
                FechaCobro = c.FechaCobro
            }).ToListAsync();

            return cheques;
        }
        public async Task<bool> Actualizar(
                                            int id,
                                            int? id_Cliente = null,
                                            DateOnly? FechaIngresoCheque = null,
                                            string? NumeroCheque = null,
                                            string? Banco = null,
                                            float? Monto = null,
                                            DateOnly? FechaCobro = null
                                        )

        {

            try
            {
                var cheque = await _context.Cheques.FindAsync(id);

                // Actualizar solo los campos proporcionados
                if (id_Cliente.HasValue)
                    cheque.id_Cliente = id_Cliente.Value;

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

        public async Task<ChequeDTO?> ObtenerPorId(int id)
        {
            Cheque cheque = await _context.Cheques.FindAsync(id);
            if (cheque != null)
            {
                ChequeDTO nuevo = new ChequeDTO(cheque.id_Cliente, cheque.FechaIngresoCheque, cheque.NumeroCheque, cheque.Monto, cheque.Banco, cheque.FechaCobro);
                return nuevo;
            }
            return null;
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var cheque = await _context.Cheques.FindAsync(id);

                if (cheque == null)
                    return false;

                _context.Cheques.Remove(cheque);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}