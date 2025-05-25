using System;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Cheque
    {
        public int Id { get; set; }
        public DateOnly FechaIngresoCheque { get; set; }
        public int NumeroCheque { get; set; }
        public float Monto { get; set; }
        public string Banco { get; set; }
        public DateOnly FechaCobro { get; set; }
        public string Nombre { get; set; }
        public int? NumeroPersonalizado { get; set; }
        public DateOnly? FechaVencimiento { get; set; }
        public string? EntregadoA { get; set; } = null; // NUEVO CAMPO - Como mi esperanza de conseguir novia

        // Constructor completo - Ahora con más parámetros que un anime harem
        public Cheque(DateOnly fechaIngresoCheque, int numeroCheque,
            float monto, string banco, DateOnly fechaCobro, string nombre = "", 
            int? numeroPersonalizado = null, DateOnly? fechaVencimiento = null, string entregadoA = null)
        {
            this.FechaIngresoCheque = fechaIngresoCheque;
            this.NumeroCheque = numeroCheque;
            this.Monto = monto;
            this.Banco = banco;
            this.FechaCobro = fechaCobro;
            this.Nombre = nombre;
            this.NumeroPersonalizado = numeroPersonalizado;
            this.FechaVencimiento = fechaCobro; // Por defecto igual a la fecha de cobro
            this.EntregadoA = entregadoA; // Nuevo campo inicializado
        }

        // Constructor sin parámetros - Para cuando no sabes ni qué estás haciendo
        public Cheque()
        {
            this.Banco = "bna";
            this.Nombre = "";
            this.EntregadoA = ""; // Inicializar el nuevo campo
            this.FechaVencimiento = new DateOnly(1, 1, 1);
        }
    }
}