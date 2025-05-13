
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Proyecto_camiones.Presentacion.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Proyecto_camiones.Presentacion.Utils
{

    public class ValidadorCheque
    {
       
        private List<string> _errores;
        private readonly int Id_cliente;
        private readonly DateOnly FechaIngresoCheque;
        private readonly String NumeroCheque;
        private readonly float Monto;
        private readonly string Banco;
        private readonly DateOnly FechaCobro;



        public ValidadorCheque(int id_Cliente, DateOnly FechaIngresoCheque, String NumeroCheque, float Monto, string Banco, DateOnly FechaCobro)
        {
            this.Banco = Banco;
            this.Id_cliente = id_Cliente;
            this.FechaIngresoCheque = FechaIngresoCheque;
            this.NumeroCheque = NumeroCheque;
            this.Monto = Monto;
            this.FechaCobro = FechaCobro;
            _errores = new List<string>();  

        }

           
        public ValidadorCheque ValidarDatos()
        {
                if (this.Monto < 0)
                     _errores.Add(MensajeError.valorInvalido(nameof(this.Monto)));
                if (this.Banco == null)
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(this.Banco)));

                if ( String.IsNullOrEmpty(this.NumeroCheque))
                    _errores.Add(MensajeError.ausenciaDeDatos(nameof(this.NumeroCheque)));

            return this;
        }
        public ValidadorCheque ValidarFechas()
        {

            if (this.FechaIngresoCheque > this.FechaCobro) return this; // Evitamos NullException
            _errores.Add(MensajeError.fechaInvalida(nameof(Cheque)));



            return this;
        }
        


        public Result<bool> ObtenerResultado()
        {
            return _errores.Count == 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(ObtenerMensajeError());
        }

        // Esta función ayuda a mantener todas las validaciones en un solo llamado
        public Result<bool> ValidarCompleto()
        {
            return
                 ValidarDatos()
                .ValidarFechas()
                .ObtenerResultado();
        }

        // Método auxiliar para formatear los errores
        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}

