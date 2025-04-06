using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.Presentacion.Models;

namespace Proyecto_camiones.Presentacion.Repositories
{
  
    public class ChequeRepository
    {
        private List<Cheque> _cheques;
        private int _siguienteId;

        public ChequeRepository()
        {
            _cheques = new List<Cheque>();
            _siguienteId = 1;

            // Creamos un cheque de prueba
            Cliente cliente = new Cliente(null, "Empresa C", 33333333);
            Cheque cheque = new Cheque(
                1,
                DateTime.Now,
                123456,
                5000.0f,
                "Banco Nacional",
                DateTime.Now.AddDays(30)
            );

            _cheques.Add(cheque);
        }

        public async Task<Cheque> ObtenerPorId(int id)
        {
            // Devolvemos el primer cheque para simplificar
            return _cheques.Count > 0 ? _cheques[0] : null;
        }

        public async Task<List<Cheque>> ObtenerTodos()
        {
            // Devolvemos una copia de la lista
            return new List<Cheque>(_cheques);
        }

        public async Task<int> Insertar(Cheque cheque)
        {
            // Agregamos a la lista
            _cheques.Add(cheque);
            return _siguienteId++;
        }

        public async Task Actualizar(Cheque cheque)
        {
            // Simulamos actualización (no hacemos nada real en la maqueta)
        }

        public async Task Eliminar(int id)
        {
            // Simulamos eliminación (en implementación real buscaríamos por ID)
            if (_cheques.Count > 0)
            {
                _cheques.RemoveAt(0);
            }
        }
    }
}