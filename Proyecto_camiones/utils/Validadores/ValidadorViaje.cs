using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
        private readonly string _cliente;
        private readonly string _camion;
        private readonly string _carga;
        private readonly float _km;
        private readonly string _nombreChofer;
        private readonly float _porcentajeChofer;
        private List<string> _errores;

        public ValidadorViaje(
            DateOnly fechaInicio,
            string lugarPartida,
            string destino,
            float kg,
            int remito,
            float tarifa,
            string cliente,
            string camion,
            string carga,
            float km,
            string nombreChofer,
            float porcentajeChofer)
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
            _nombreChofer = nombreChofer;
            _porcentajeChofer = porcentajeChofer;
            _errores = new List<string>();
        }


        public ValidadorViaje ValidarCarga()
        {
            if (_kg <= 0)
                _errores.Add(MensajeError.numeroNoValido(nameof(_kg)));

            if (string.IsNullOrWhiteSpace(_carga))
                _errores.Add(MensajeError.atributoRequerido(nameof(_carga)));

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
            {
                MessageBox.Show("La tarifa debe ser mayor a 0");
                _errores.Add(MensajeError.numeroNoValido(nameof(_tarifa)));
            }

            if (_remito <= 0)
            {
                MessageBox.Show("El remimto debe ser mayor a 0");
                _errores.Add(nameof(_remito));
            }

            return this;
        }

        public ValidadorViaje ValidarExistencia(bool clienteExiste, bool camionExiste)
        {
            if (!camionExiste)
            {
                MessageBox.Show("no existe camión con esa patente");
                _errores.Add($"No existe un camión con la patente {_camion}");
            }

            if (!clienteExiste)
            {
                MessageBox.Show("no existe cliente con ese nombre");
                _errores.Add($"No existe un cliente con el nombre {_cliente}");
            }

            return this;
        }

        public ValidadorViaje ValidarDatosEntidadesRelacionadas()
        {
            if (string.IsNullOrWhiteSpace(_camion))
                _errores.Add("El campo de camion esta vacio");
            

            if (string.IsNullOrWhiteSpace(_cliente))
                _errores.Add("El campo de cliente esta vacio");

            if (string.IsNullOrWhiteSpace(_nombreChofer))
                _errores.Add("El campo de chofer esta vacio");

            if ( 0 > _porcentajeChofer || _porcentajeChofer  > 1)
                _errores.Add("El porcentaje del chofer no es valido, debe ser entre 0% y 100%");

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
                .ValidarDatosEntidadesRelacionadas()
                .ObtenerResultado();
        }

        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}
