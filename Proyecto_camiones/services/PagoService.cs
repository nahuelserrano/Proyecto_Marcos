using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.DTOs;
using System.Windows.Forms;


namespace Proyecto_camiones.Services
{
    public class PagoService
    {
        private PagoRepository _pagoRepository;
        public PagoService(PagoRepository pagoRepository)
        {
            this._pagoRepository = pagoRepository;
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await _pagoRepository.ProbarConexionAsync();
            return result;
        }
        public async Task<int> CrearAsync(int id_chofer, int id_viaje, float monto_pagado)
        {


            try
            {

                int idPago = await _pagoRepository.InsertarAsync(id_chofer, id_viaje, monto_pagado);
                if (idPago > 0)
                {
                    return idPago;
                }
                else
                {
                    Console.WriteLine(idPago + "pago services");
                    return -2;
                }
            }
            catch
            {
                return -1;
            }

        }

        public async Task<Result<bool>> ActualizarAsync(int id_chofer, int id_viaje, float monto_pagado) {
            if (id_chofer <= 0 || id_viaje <= 0) return Result<bool>.Failure(MensajeError.IdInvalido(id_chofer));
            if (monto_pagado <= 0) return Result<bool>.Failure(MensajeError.valorInvalido(nameof(monto_pagado)));
            try
            {
                bool actualizado = await _pagoRepository.ActualizarAsync(id_chofer, id_viaje, monto_pagado);
                if (actualizado)
                {
                    return Result<bool>.Success(actualizado);
                }
                return Result<bool>.Failure("No se pudo actualizar el pago ya que el mismo ya se encuentra pagado");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Error al actualizar el pago");
                Console.WriteLine($"Error al actualizar el pago: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ModificarEstado(int id_chofer, DateOnly desde, DateOnly hasta, int? id_Sueldo,bool pagado = true) {
            try
            {
                bool actualizado=await _pagoRepository.modificarEstado(id_chofer, desde, hasta, id_Sueldo,pagado);
                if (actualizado) {
                    return Result<bool>.Success(actualizado);
                }
                return Result<bool>.Failure("No se pudo marcar los pagos");

            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("error al marcar como pagados los pagos correspondientes al sueldo");
                Console.WriteLine($"Error al marcar los pagos: {ex.Message}");
            }

        }


        public async Task<float> ObtenerSueldoCalculado(int id_chofer, DateOnly calcularDesde, DateOnly calcularHasta)
        {
            Console.WriteLine("id chofer: " + id_chofer);
            List<Pago> pagos = await _pagoRepository.ObtenerPagosAsync(id_chofer, calcularDesde, calcularHasta);
            Console.WriteLine("lenght de pagos a pagar: " + pagos.Count);

            float totalPagar = 0;

            foreach (var pago in pagos)
            {
                MessageBox.Show(pago.Monto_Pagado + " " + pagos.Count);
                totalPagar += pago.Monto_Pagado;
            }
            Console.WriteLine("total a pagar: " + totalPagar);
            return totalPagar;






        }




    }
}