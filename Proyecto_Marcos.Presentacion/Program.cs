using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Proyecto_Marcos.Presentacion
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        
        {

            Console.WriteLine("estoy acá?");

            // Obtener la instancia de la conexión
            Conexion conexion = Conexion.getInstancia();

            // Probar la conexión
            bool conexionExitosa = conexion.TestConexion();

            if (conexionExitosa)
            {
                Console.WriteLine("La conexión a la base de datos fue exitosa.");
            }
            else
            {
                Console.WriteLine("No se pudo conectar a la base de datos.");
            }
        
        //Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Formulario_Viajes());

        //    Datos_pagos datos = new Datos_pagos();
        //    datos.Listado_pagos("1");
        }
    }
}
