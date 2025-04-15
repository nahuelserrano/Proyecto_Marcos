using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;

namespace Proyecto_camiones.Presentacion.Utils
{

    public class ValidadorChofer
    {
        public readonly string Nombre;
        private List<string> _errores;


        public ValidadorChofer(string nombre)
        {
            this.Nombre = nombre;
            _errores = new List<string>();
        }

        public ValidadorChofer ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(Nombre)));

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
            return ValidarDatos()
                .ObtenerResultado();
        }

        // Método auxiliar para formatear los errores
        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}