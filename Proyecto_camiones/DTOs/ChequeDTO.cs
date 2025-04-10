using System;

namespace Proyecto_camiones.DTOs
{
    public class ChequeDTO
    {
        public int Id_cliente;
        public DateTime FechaIngresoCheque;
        public string NumeroCheque;
        public float Monto;
        public string Banco;
        public DateTime FechaCobro;


        public ChequeDTO(int id_Cliente, DateTime FechaIngresoCheque, string NumeroCheque, float Monto, String Banco, DateTime FechaCobro) {
            Id_cliente = id_Cliente;
            this.FechaIngresoCheque = FechaIngresoCheque;
            this.NumeroCheque = NumeroCheque;
            this.Monto = Monto;
            this.Banco = Banco;
            this.FechaCobro = FechaCobro;

        }
        public ChequeDTO()
        {
            this.Id_cliente = default;
            this.Banco = default;
            this.FechaCobro = default;
            this.FechaIngresoCheque = default;
            this.Monto = default;
            this.NumeroCheque = default;
            // Constructor vacío

        }
    }
}
