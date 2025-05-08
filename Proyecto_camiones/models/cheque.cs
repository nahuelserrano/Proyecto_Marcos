using System;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Cheque
    {
        public int Id { get; set; }
        public int id_Cliente { get; set; }
        public DateOnly FechaIngresoCheque { get; set; }
        public string NumeroCheque { get; set; }
        public float Monto { get; set; }
        public string Banco { get; set; }
        public DateOnly FechaCobro { get; set; }
        public string Nombre { get; set; }
        public int? NumeroPersonalizado { get; set; }
        public DateOnly FechaVencimiento { get; set; }

        // Constructor completo
        public Cheque(int id_Cliente, DateOnly fechaIngresoCheque, string numeroCheque,
            float monto, string banco, DateOnly fechaCobro,
            string nombre = "", int? numeroPersonalizado = null)
        {
            this.id_Cliente = id_Cliente;
            this.FechaIngresoCheque = fechaIngresoCheque;
            this.NumeroCheque = numeroCheque;
            this.Monto = monto;
            this.Banco = banco;
            this.FechaCobro = fechaCobro;
            this.Nombre = nombre;
            this.NumeroPersonalizado = numeroPersonalizado;
            this.FechaVencimiento = fechaCobro; // Por defecto igual a la fecha de cobro
        }

        // Constructor sin parámetros
        public Cheque()
        {
            this.id_Cliente = 1;
            this.Banco = "bna";
            this.Nombre = "";
            this.FechaVencimiento = new DateOnly(1, 1, 1);
        }
    }
}