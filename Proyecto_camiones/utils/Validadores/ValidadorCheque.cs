
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
        private readonly DateTime FechaIngresoCheque;
        private readonly string NumeroCheque;
        private readonly float Monto;
        private readonly string Banco;
        private readonly DateTime FechaCobro;



        public ValidadorCheque(int id_Cliente, DateTime FechaIngresoCheque, string NumeroCheque, float Monto, string Banco, DateTime FechaCobro)
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

                if (this.NumeroCheque!=null)
                    _errores.Add(MensajeError.ausenciaDeDatos(nameof(this.Banco)));

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

