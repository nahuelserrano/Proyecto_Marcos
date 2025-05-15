using System;

namespace Proyecto_camiones.DTOs
{
    public class ChequeDTO
    {
        public int Id { get; set; } // Añadido para operaciones CRUD
        public DateOnly FechaIngresoCheque { get; set; }
        public int NumeroCheque { get; set; } // CAMBIADO a int, para coincidir con la BD
        public float Monto { get; set; }
        public string Banco { get; set; }
        public DateOnly FechaCobro { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int? NumeroPersonalizado { get; set; }

        public ChequeDTO(
            DateOnly fechaIngreso,
            int numeroCheque,
            float monto,
            string banco,
            DateOnly fechaCobro,
            string nombre = "",
            int? numeroPersonalizado = null,
            DateOnly? fechaVencimiento = null)
        {
            FechaIngresoCheque = fechaIngreso;
            NumeroCheque = numeroCheque;
            Monto = monto;
            Banco = banco;
            FechaCobro = fechaCobro;
            Nombre = nombre;
            NumeroPersonalizado = numeroPersonalizado;
        }
        public ChequeDTO(int id_Cliente, DateOnly FechaIngresoCheque, int NumeroCheque, float Monto, string Banco, DateOnly FechaCobro)
        {
            this.FechaIngresoCheque = FechaIngresoCheque;
            this.NumeroCheque = NumeroCheque;
            this.Monto = Monto;
            this.Banco = Banco;
            this.FechaCobro = FechaCobro;

        }
        public ChequeDTO()
        {
            this.Banco = default;
            this.FechaCobro = default;
            this.FechaIngresoCheque = default;
            this.Monto = default;
            this.NumeroCheque = default;
            // Constructor vacío

        }
    }
}
