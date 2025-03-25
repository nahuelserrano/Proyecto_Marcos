using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;

namespace Proyecto_camiones.Presentacion.Utils
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

        public ValidadorViaje Validar()
        {
            _errores.Clear();

            if (_viaje == null)
                _errores.Add(MensajeError.objetoNulo(nameof(Viaje)));

            return this;
        }

        public ValidadorViaje ValidarFechas()
        {
            if (_viaje == null) return this;

            if (_viaje.FechaEntrega < DateTime.Now)
                _errores.Add("La fecha de partida no puede ser en el pasado");

            if (_viaje.FechaEntrega < _viaje.FechaInicio)
                _errores.Add("La fecha de entrega no puede ser anterior a la fecha de inicio");

            return this;
        }

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

            //if (_viaje.KilosCarga <= 0)
            //    _errores.Add("El peso de la carga debe ser mayor que cero");

            //if (_viaje.KilosCarga + _viaje.Camion.Tara > _viaje.Camion.CapacidadMax)
            //    _errores.Add($"La carga supera la capacidad máxima del camión ({_viaje.Camion.CapacidadMax}kg)");

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

        public ValidadorViaje ValidarPrecioYRemito()
        {
            if (_viaje == null) return this;

            if (_viaje.PrecioPorKilo <= 0)
                _errores.Add("El precio por kilo debe ser mayor a cero");

            if (_viaje.Remito <= 0)
                _errores.Add("El número de remito debe ser válido y mayor a cero");

            return this;
        }

        public ValidadorViaje ValidarEstado()
        {
            if (_viaje == null) return this;

            var estadosValidos = new HashSet<string> { "Pendiente", "En tránsito", "Finalizado" };

            if (!estadosValidos.Contains(_viaje.Estado))
                _errores.Add($"Estado inválido: {_viaje.Estado}. Debe ser 'Pendiente', 'En tránsito' o 'Finalizado'.");

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
            return Validar()
                .ValidarFechas()
                .ValidarEntidadesRelacionadas()
                .ValidarCarga()
                .ValidarRuta()
                .ValidarPrecioYRemito()
                .ValidarEstado()
                .ObtenerResultado();
        }

        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}
