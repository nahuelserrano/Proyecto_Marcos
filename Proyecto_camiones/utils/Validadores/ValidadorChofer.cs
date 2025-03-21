
using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Proyecto_camiones.Presentacion.Utils
{

    public class ValidadorChofer
    {
        private readonly Chofer _chofer;
        private List<string> _errores;


        public ValidadorChofer(Chofer chofer)
        {
            _chofer = chofer;
            _errores = new List<string>();
        }

        // Método para iniciar la validación - verifica si el objeto es nulo
        public ValidadorChofer Validar()
        {
            _errores.Clear();

            if (_chofer == null)
            {
                _errores.Add(MensajeError.objetoNulo(nameof(Chofer)));
            }

            return this; // Para permitir encadenamiento
        }

        public ValidadorChofer ValidarDatos ()
        {
            if (_chofer == null) return this; // Evitamos NullException

            if (string.IsNullOrWhiteSpace(_chofer.nombre))
               _errores.Add(MensajeError.ausenciaDeDatos(nameof(_chofer.nombre)));

            if (string.IsNullOrWhiteSpace(_chofer.apellido))
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(_chofer.apellido)));

            

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

