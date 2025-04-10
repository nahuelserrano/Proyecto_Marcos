using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Proyecto_camiones.Presentacion.Utils
{

    public class ValidadorCamion
    {
        private readonly float peso;
        private readonly float tara;
        private readonly string patente;
        private readonly string nombre;
        private List<string> _errores;
        private int pesominimo = 1;

        public ValidadorCamion(float peso, float tara, string patente, string nombre)
        {
            this.peso = peso;
            this.tara = tara;
            this.patente = patente;
            this.nombre = nombre;
            _errores = new List<string>();
        }

        // Método para iniciar la validación - verifica si el objeto es nulo

        public ValidadorCamion ValidarPesos()
        {
            //no nos acordamo cuanto es el peso minimo de un viaje, debemos chequearlo
            if (this.pesominimo==1000)
                _errores.Add(MensajeError.PesoIncorrecto(this.tara));

            if (this.peso < this.pesominimo)
                _errores.Add(MensajeError.PesoIncorrecto(this.peso));

            if (this.tara > this.peso)
                _errores.Add(MensajeError.PesoIncorrecto(this.tara));

            return this;
        }
        public ValidadorCamion ValidarPatente ()
        {
            if (this.patente == null)
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(this.patente)));

            return this;
        }

        public ValidadorCamion ValidarChofer()
        {
            if (this.nombre == null)
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(this.nombre)));

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
            return ValidarPesos()
                .ValidarPatente()
                .ValidarChofer()
   
                .ObtenerResultado();
        }

        // Método auxiliar para formatear los errores
        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}

