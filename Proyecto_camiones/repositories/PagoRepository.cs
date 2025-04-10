using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.Presentacion.Models;

namespace Proyecto_camiones.Presentacion.Repositories
{
    public class PagoRepository
    {
        private List<Pago> _pagos;
        private int _siguienteId;

        public PagoRepository()
        {
            _pagos = new List<Pago>();
            _siguienteId = 1;

            // Creamos pagos de prueba
            //_pagos.Add(new Pago(1, 1000.0f, true));
            //_pagos.Add(new Pago(2, 2000.0f, false));
        }

        public async Task<Pago> ObtenerPorId(int id)
        {
            // Devolvemos el primer pago para simplificar
            return _pagos.Count > 0 ? _pagos[0] : null;
        }

        public async Task<List<Pago>> ObtenerTodos()
        {
            // Devolvemos una copia de la lista
            return new List<Pago>(_pagos);
        }

        //public async Task<int> Insertar(Pago pago)
        //{
        //    // Asignamos un ID y agregamos a la lista
        //    //pago.Id = _siguienteId++;
        //    //_pagos.Add(pago);
        //    //return pago.Id;
        //}

        public async Task Actualizar(Pago pago)
        {
            //// Simulamos actualización
            //int indice = _pagos.FindIndex(p => p.Id == pago.Id);
            //if (indice >= 0)
            //{
            //    _pagos[indice] = pago;
            //}
        }

        public async Task Eliminar(int id)
        {
            //// Simulamos eliminación
            //_pagos.RemoveAll(p => p.Id == id);
        }
    }
}