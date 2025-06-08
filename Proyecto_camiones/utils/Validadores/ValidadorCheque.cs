
using System;
using System.Collections.Generic;

namespace Proyecto_camiones.Presentacion.Utils
{

    public class ValidadorCheque
    {

        private readonly DateOnly _fechaIngresoCheque;
        private readonly int _numeroCheque;
        private readonly float _monto;
        private readonly string _banco;
        private readonly DateOnly _fechaCobro;
        private readonly string _nombre;
        private readonly int? _numeroPersonalizado;
        private readonly DateOnly _fechaVencimiento;
        private readonly string _entregadoA;
        private List<string> _errores;

        public ValidadorCheque(
            DateOnly fechaIngresoCheque,
            int numeroCheque,
            float monto,
            string banco,
            DateOnly fechaCobro,
            string nombre = "",
            int? numeroPersonalizado = null,
            DateOnly? fechaVencimiento = null,
            string entregadoA = null)
        {
            _fechaIngresoCheque = fechaIngresoCheque;
            _numeroCheque = numeroCheque;
            _monto = monto;
            _banco = banco;
            _fechaCobro = fechaCobro;
            _nombre = nombre ?? string.Empty;
            _numeroPersonalizado = numeroPersonalizado;
            _fechaVencimiento = fechaVencimiento ?? fechaCobro;
            _entregadoA = entregadoA;
            _errores = new List<string>();
        }

        public ValidadorCheque ValidarDatos()
        {
            if (_numeroCheque <= 0)
                _errores.Add($"El número de cheque debe ser mayor a 0. Valor recibido: {_numeroCheque}");

            if (_monto <= 0)
                _errores.Add($"El monto debe ser mayor a 0. Valor recibido: {_monto}");

            if (string.IsNullOrWhiteSpace(_banco))
                _errores.Add(MensajeError.atributoRequerido(nameof(_banco)));

            // Validar longitud (según tu schema)
            if (!string.IsNullOrEmpty(_banco) && _banco.Length > 45)
                _errores.Add($"El banco no puede tener más de 45 caracteres");

            if (!string.IsNullOrEmpty(_nombre) && _nombre.Length > 45)
                _errores.Add($"El nombre no puede tener más de 45 caracteres");

            return this;
        }

        public ValidadorCheque ValidarFechas()
        {
            // La fecha de ingreso no puede ser mayor a la fecha de cobro
            if (_fechaIngresoCheque > _fechaCobro)
                _errores.Add("La fecha de ingreso no puede ser posterior a la fecha de cobro");

            // La fecha de vencimiento no puede ser anterior a la fecha de cobro
            if (_fechaVencimiento < _fechaCobro)
                _errores.Add("La fecha de vencimiento no puede ser anterior a la fecha de cobro");

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
            return ValidarDatos()
                .ValidarFechas()
                .ObtenerResultado();
        }

        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}

