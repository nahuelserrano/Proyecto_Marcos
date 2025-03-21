using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Proyecto_camiones.Presentacion.Utils
{

    public class ValidadorCamion
    {
        private readonly Camion _camion;
        private List<string> _errores;
        private int pesominimo = 1;

        public ValidadorCamion(Camion camion)
        {
            _camion = camion;
            _errores = new List<string>();
        }

        // Método para iniciar la validación - verifica si el objeto es nulo
        public ValidadorCamion Validar()
        {
            _errores.Clear();

            if (_camion == null)
            {
                _errores.Add(MensajeError.objetoNulo(nameof(_camion)));
            }

            return this; // Para permitir encadenamiento
        }

        public ValidadorCamion ValidarPesos()
        {
            if (_camion == null) return this; // Evitamos NullException

            if (_camion.Tara < this.pesominimo)
                _errores.Add(MensajeError.PesoIncorrecto(_camion.Tara));

            if (_camion.CapacidadMax < this.pesominimo)
                _errores.Add(MensajeError.PesoIncorrecto(_camion.CapacidadMax));

            if (_camion.Tara > _camion.CapacidadMax)
                _errores.Add(MensajeError.PesoIncorrecto(_camion.Tara));

            return this;
        }
        public ValidadorCamion ValidarPatente ()
        {
            if (_camion == null) return this; // Evitamos NullException

            if (_camion.Patente==null)
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(_camion.Patente)));

            
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
                .ValidarPesos()
                .ValidarPatente()
   
                .ObtenerResultado();
        }

        // Método auxiliar para formatear los errores
        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}

