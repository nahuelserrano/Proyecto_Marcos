using System;
using System.Collections.Generic;
using Proyecto_camiones.Presentacion.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Proyecto_camiones.Presentacion.Utils
{

    public class ValidadorPago
    {  
        private readonly float Monto;
        private readonly bool Pagado;
        private List<string> Errores;


        public ValidadorPago(float monto,bool pagado)
        {
            this.Monto = monto;
            this.Pagado = pagado;
            Errores = new List<string>();
        }

       
       

        public ValidadorPago ValidarDatos()
        {
           

            if (this.Monto < 0)
                Errores.Add(MensajeError.valorInvalido(nameof(this.Monto)));

           


            return this;
        }


        public Result<bool> ObtenerResultado()
        {
            return Errores.Count == 0
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
            return string.Join(Environment.NewLine, Errores);
        }
    }
}

