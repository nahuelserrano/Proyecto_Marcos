using System;
using System.Collections.Generic;
using Proyecto_Marcos.Presentacion.Models;

namespace Proyecto_Marcos.Presentacion.Utils
{

    public class ValidadorViaje 
    {
        private readonly Viaje _viaje;
        private List<string> _errores;

        public ValidadorViaje(Viaje viaje)
        {
            _viaje = viaje;
            _errores = new List<string>();
        }

        // Método para iniciar la validación - verifica si el objeto es nulo
        public ValidadorViaje Validar()
        {
            _errores.Clear();

            if (_viaje == null)
            {
                _errores.Add(MensajeError.objetoNulo(nameof(Viaje)));
            }

            return this; // Para permitir encadenamiento
        }

        public ValidadorViaje ValidarFechas()
        {
            if (_viaje == null) return this; // Evitamos NullException

            if (_viaje.FechaEntrega < _viaje.FechaInicio)
                _errores.Add(MensajeError.fechaInvalida(nameof(_viaje.FechaEntrega)));

            return this;
        }

        // Validación específica para entidades relacionadas
        public ValidadorViaje ValidarEntidadesRelacionadas()
        {
            if (_viaje == null) return this;

            if (_viaje.Cliente == null)
                _errores.Add(MensajeError.objetoNulo(nameof(Cliente)));

            if (_viaje.Camion == null)
                _errores.Add(MensajeError.objetoNulo(nameof(Camion)));

            if (_viaje.Chofer == null)
                _errores.Add(MensajeError.objetoNulo(nameof(Chofer)));

            return this;
        }

        public ValidadorViaje ValidarCarga()
        {
            if (_viaje == null || _viaje.Camion == null) return this;

            if (_viaje.KilosCarga <= 0)
                _errores.Add("El peso de la carga debe ser mayor que cero");

            // Verificamos que el camión pueda soportar la carga
            if (!_viaje.Camion.chequeo_peso_maximo(_viaje.KilosCarga))
                _errores.Add($"La carga supera la capacidad máxima del camión ({_viaje.Camion.CapacidadMax}kg)");

            return this;
        }

        public ValidadorViaje ValidarRuta()
        {
            if (_viaje == null) return this; 

            if (string.IsNullOrWhiteSpace(_viaje.LugarPartida))
                _errores.Add("El lugar de partida es requerido");

            if (string.IsNullOrWhiteSpace(_viaje.Destino))
                _errores.Add("El destino es requerido");

            if (_viaje.LugarPartida == _viaje.Destino)
                _errores.Add("El origen y destino no pueden ser iguales");

            return this;
        }

        // Obtener el resultado final
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
                .ValidarFechas()
                .ValidarEntidadesRelacionadas()
                .ValidarCarga()
                .ValidarRuta()
                .ObtenerResultado();
        }

        // Método auxiliar para formatear los errores
        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}

