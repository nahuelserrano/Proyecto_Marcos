
using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Proyecto_camiones.Presentacion.Utils
{

    public class ValidadorCheque
    {
        private readonly Cheque _cheque;
        private List<string> _errores;


        public ValidadorCheque(Cheque cheque)
        {
            _cheque = cheque;
            _errores = new List<string>();
        }

        // Método para iniciar la validación - verifica si el objeto es nulo
        public ValidadorCheque Validar()
        {
            _errores.Clear();

            if (_cheque == null)
            {
                _errores.Add(MensajeError.objetoNulo(nameof(Cheque)));
            }

            return this; // Para permitir encadenamiento
        }

        public ValidadorCheque ValidarDatos()
        {
                if (_cheque.Monto < 0)
                     _errores.Add(MensajeError.valorInvalido(nameof(_cheque.Monto)));
            if (_cheque.Banco == null)
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(_cheque.Banco)));

            return this;
        }
        public ValidadorCheque ValidarFechas()
        {

            if (_cheque.FechaIngresoCheque > _cheque.FechaCobro) return this; // Evitamos NullException
            _errores.Add(MensajeError.fechaInvalida(nameof(Cheque)));



            return this;
        }
        public ValidadorCheque ValidarEntidadesRelacionadas()
        {
            if (_cheque == null) return this;

            if (_cheque.Cliente_Dueño_Cheque == null)
                _errores.Add(MensajeError.objetoNulo(nameof(Cheque)));

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
            return Validar()
                .ValidarDatos()
                .ValidarFechas()
                .ValidarEntidadesRelacionadas()
                .ObtenerResultado();
        }

        // Método auxiliar para formatear los errores
        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}

