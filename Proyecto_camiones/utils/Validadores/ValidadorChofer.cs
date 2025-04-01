
using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Proyecto_camiones.Presentacion.Utils
{

    public class ValidadorChofer
    {
        public readonly string Nombre;
        public readonly string Apellido;
        private List<string> _errores;


        public ValidadorChofer(string nombre, string apellido)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            _errores = new List<string>();
        }

        public ValidadorChofer ValidarDatos ()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
               _errores.Add(MensajeError.ausenciaDeDatos(nameof(Nombre)));

            if (string.IsNullOrWhiteSpace(Apellido))
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(Apellido)));

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

