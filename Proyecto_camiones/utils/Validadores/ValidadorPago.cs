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
        private readonly int Id_Chofer;
        private readonly DateOnly MesCorrespondiente;
        private readonly DateOnly FechaPago;
        private readonly DateOnly FechaDesde;
        private readonly DateOnly FechaHasta;
        private List<string> Errores;


        
        public ValidadorPago(float monto, int Id_Chofer, DateOnly pagoDesde,DateOnly pagoHasta)
        {
            this.Monto = monto;
            this.Id_Chofer = Id_Chofer;
            this.FechaDesde = pagoDesde;
            this.FechaHasta = pagoHasta;

            Errores = new List<string>();
        }

        public ValidadorPago(double monto ,int id_Chofer, DateOnly pagoDesde, DateOnly pagoHasta, DateOnly fechaPago)
        {
            this.Id_Chofer = id_Chofer;
            this.FechaPago = fechaPago;
            this.Monto = (float)monto;
            this.FechaDesde = pagoDesde;
            this.FechaHasta = pagoHasta;
            this.Errores = new List<string>();
        }

        public ValidadorPago ValidarDatos()
        {
          
            if (this.Monto < 0)
                Errores.Add(MensajeError.valorInvalido(nameof(this.Monto)));

            if (this.Id_Chofer <= 0)
                Errores.Add(MensajeError.valorInvalido(nameof(this.Id_Chofer)));

            return this;
        }

        public ValidadorPago ValidarFecha()
        {
            if (FechaPago != null) { 
                if (this.FechaPago < MesCorrespondiente) { 
           
                    Errores.Add(MensajeError.fechaInvalida(nameof(FechaPago)));
                }
            }

          
                if (this.FechaDesde > this.FechaHasta)
                {
                    Errores.Add(MensajeError.fechaInvalida(nameof(FechaDesde)));
                }
            if (this.FechaHasta < this.FechaDesde)
            {
                Errores.Add(MensajeError.fechaInvalida(nameof(FechaHasta)));
            }


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
            return ValidarFecha()
                  .ValidarDatos()
                  .ObtenerResultado();
        }

        // Método auxiliar para formatear los errores
        private string ObtenerMensajeError()
        {
            return string.Join(Environment.NewLine, Errores);
        }
    }
}

