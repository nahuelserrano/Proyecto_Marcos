
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
        private readonly string Dni;
        private readonly String Apellido;
        private List<string> _errores;
   

        public ValidadorCliente( String Nombre, String Apellido, string Dni)
        {
            //this.Id_Cliente = id;
            this.Nombre = Nombre;
            this.Apellido = Apellido;
            this.Dni = Dni;
            _errores = new List<string>();
        }

       

        public ValidadorCliente ValidarDatos()
        {

            //if (this.Id_Cliente <=0) return this; 

            if (string.IsNullOrWhiteSpace(this.Nombre))
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(this.Nombre)));

            if (string.IsNullOrWhiteSpace(this.Apellido))
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(this.Apellido)));

            if (string.IsNullOrWhiteSpace(this.Dni))
                _errores.Add(MensajeError.ausenciaDeDatos(nameof(this.Dni)));



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

