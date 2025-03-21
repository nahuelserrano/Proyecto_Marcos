using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Proyecto_camiones.Presentacion.Utils
{

    public class ValidadorPago
    {
        private readonly Pago _pago;
        private List<string> _errores;


        public ValidadorPago(Pago pago)
        {
            _pago = pago;
            _errores = new List<string>();
        }

        // Método para iniciar la validación - verifica si el objeto es nulo
        public ValidadorPago Validar()
        {
            _errores.Clear();

            if (_pago == null)
            {
                _errores.Add(MensajeError.objetoNulo(nameof(Pago)));
            }

            return this; // Para permitir encadenamiento
        }

        public ValidadorPago ValidarDatos()
        {
            if (_pago == null) return this; // Evitamos NullException

            if (_pago.Monto < 0)
                _errores.Add(MensajeError.valorInvalido(nameof(_pago.Monto)));

           


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
                .ObtenerResultado();
        }

        // Método auxiliar para formatear los errores
        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}

