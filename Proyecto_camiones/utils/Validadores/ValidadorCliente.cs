
using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Proyecto_camiones.Presentacion.Utils
{

    public class ValidadorCliente
    {
        private readonly Cliente _cliente;
        private List<string> _errores;
   

        public ValidadorCliente(Cliente cliente)
        {
            _cliente = cliente;
            _errores = new List<string>();
        }

        // Método para iniciar la validación - verifica si el objeto es nulo
        public ValidadorCliente Validar()
        {
            _errores.Clear();

            if (_cliente == null)
            {
                _errores.Add(MensajeError.objetoNulo(nameof(Cliente)));
            }

            return this; // Para permitir encadenamiento
        }

        public ValidadorCliente ValidarDatos()
        {

            if (_cliente == null) return this; // Evitamos NullException

            if (string.IsNullOrWhiteSpace(_cliente.nombre))
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(_cliente.nombre)));

            if (string.IsNullOrWhiteSpace(_cliente.apellido))
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(_cliente.apellido)));



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

