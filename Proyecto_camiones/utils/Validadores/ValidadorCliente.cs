
using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Proyecto_camiones.Presentacion.Utils
{

    public class ValidadorCliente
    {
        private readonly int Id_Cliente;
        private readonly String Nombre;
        private List<string> _errores;
   

        public ValidadorCliente( string Nombre)
        {
           
            this.Nombre = Nombre;
            _errores = new List<string>();
        }

       

        public ValidadorCliente ValidarDatos()
        {


            if (string.IsNullOrWhiteSpace(this.Nombre))
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(this.Nombre)));




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
            return 
                 ValidarDatos()
                .ObtenerResultado();
        }

        // Método auxiliar para formatear los errores
        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, _errores);
        }
    }
}

