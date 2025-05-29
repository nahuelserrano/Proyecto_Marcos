using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.ViewModels;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Models;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System.Runtime.CompilerServices;
using MySqlX.XDevAPI.Common;
using System.Data;
using MySql.Data.MySqlClient;
using Proyecto_camiones.Presentacion.Models;
using System.Collections.Generic;
using Proyecto_camiones.Tests;
using System.Collections.Generic;
using System.Linq;
using Proyecto_camiones.Front;
using Mysqlx.Cursor;

namespace Proyecto_camiones
{
    static class Program
    {


        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static async Task Main(string[] args)

        {
            //// Llamada a Windows Forms para inicializar la aplicación
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Front.Viaje()); // Ejecuta el formulario principal


            await SueldoTest.EjecutarTodasLasPruebas();
        }
    }
}

