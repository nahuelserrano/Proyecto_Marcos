using Proyecto_Marcos.Presentacion.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CamionRepository
{
    private List<Camion> _camiones = new List<Camion>();
    private int _nextId = 1;
    internal async Task<Camion> getById(int id)
    {
        throw new NotImplementedException();
    }
}