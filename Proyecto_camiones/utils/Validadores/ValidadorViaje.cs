using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;

namespace Proyecto_camiones.Presentacion.Utils
{
    public class ValidadorViaje
    {
        private readonly DateOnly _fechaInicio;
        private readonly string _lugarPartida;
        private readonly string _destino;
        private readonly float _kg;
        private readonly int _remito;
        private readonly float _tarifa;
        private readonly int _cliente;
        private readonly int _camion;
        private readonly string _carga;
        private readonly float _km;
        private List<string> _errores;

        public ValidadorViaje(
            DateOnly fechaInicio,
            string lugarPartida,
            string destino,
            float kg,
            int remito,
            float tarifa,
            int cliente,
            int camion,
            string carga,
            float km)
        {
            _fechaInicio = fechaInicio;
            _lugarPartida = lugarPartida;
            _destino = destino;
            _kg = kg;
            _remito = remito;
            _tarifa = tarifa;
            _cliente = cliente;
            _camion = camion;
            _carga = carga;
            _km = km;
            _errores = new List<string>();
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
            if (_tarifa <= 0)
                _errores.Add(MensajeError.numeroNoValido(nameof(_tarifa)));

            if (_remito <= 0)
                _errores.Add(nameof(_remito));

            return this;
        }

        public ValidadorViaje ValidarExistencia(bool clienteExiste, bool camionExiste)
        {
            if (!camionExiste)
                _errores.Add($"No existe un camión con el ID {_camion}");
           
            if (!clienteExiste)
                    _errores.Add($"No existe un cliente con el ID {_cliente}");

            return this;
        }

        public ValidadorViaje ValidarIdPositivos()
        {
            if (_camion <= 0)
                _errores.Add(MensajeError.idInvalido(_camion));
            
            if (_cliente <= 0)
                _errores.Add(MensajeError.idInvalido(_cliente));
            
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
            return ValidarCarga()
                .ValidarRuta()
                .ValidarPrecioYRemito()
                .ValidarIdPositivos()
                .ObtenerResultado();
        }

        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}
