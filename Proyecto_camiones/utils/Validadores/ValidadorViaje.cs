using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;

namespace Proyecto_camiones.Presentacion.Utils
{
    public class ValidadorViaje
    {
        private readonly string _destino;
        private readonly string _lugarPartida;
        private readonly float _kg;
        private readonly int _remito;
        private readonly float _precioPorKilo;
        private readonly int _chofer;
        private readonly int _cliente;
        private readonly int _camion;
        private readonly DateOnly _fechaInicio;
        private readonly DateOnly? _fechaFacturacion;
        private readonly string _carga;
        private readonly float _km;
        private List<string> _errores;

        public ValidadorViaje(
            string destino,
            string lugarPartida,
            float kg,
            int remito,
            float precioPorKilo,
            int chofer,
            int cliente,
            int camion,
            DateOnly fechaInicio,
            string carga,
            float km,
            DateOnly? fechaFacturacion = null)
        {
            _destino = destino;
            _lugarPartida = lugarPartida;
            _kg = kg;
            _remito = remito;
            _precioPorKilo = precioPorKilo;
            _chofer = chofer;
            _cliente = cliente;
            _camion = camion;
            _fechaInicio = fechaInicio;
            _fechaFacturacion = fechaFacturacion;
            _carga = carga;
            _km = km;
            _errores = new List<string>();
        }


        public ValidadorViaje ValidarFechas()
        {

            if (_fechaFacturacion < _fechaInicio)
                _errores.Add(MensajeError.fechaInvalida(nameof(_fechaFacturacion)));

            return this;
        }

        public ValidadorViaje ValidarCarga()
        {
            if (_kg <= 0)
                _errores.Add(MensajeError.numeroNoValido(nameof(_kg)));

            // Nota: Para validar la capacidad del camión necesitaríamos
            // consultar la información del camión o recibir su capacidad

            return this;
        }

        public ValidadorViaje ValidarRuta()
        {
            if (string.IsNullOrWhiteSpace(_lugarPartida))
                _errores.Add("El lugar de partida es requerido");

            if (string.IsNullOrWhiteSpace(_destino))
                _errores.Add("El destino es requerido");

            if (_lugarPartida == _destino)
                _errores.Add("El origen y destino no pueden ser iguales");

            return this;
        }

        public ValidadorViaje ValidarPrecioYRemito()
        {
            if (_precioPorKilo <= 0)
                _errores.Add(MensajeError.numeroNoValido(nameof(_precioPorKilo)));

            if (_remito <= 0)
                _errores.Add(nameof(_remito));

            return this;
        }

        public Result<bool> ObtenerResultado()
        {
            return _errores.Count == 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(ObtenerMensajeError());
        }

        public Result<bool> ValidarCompleto()
        {
            return ValidarFechas()
                .ValidarCarga()
                .ValidarRuta()
                .ValidarPrecioYRemito()
                .ObtenerResultado();
        }

        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}
