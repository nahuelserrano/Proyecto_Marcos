using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Proyecto_camiones.Presentacion
{
    class Conexion
    {
        private string Base;
        private string Servidor;
        private string Puerto;
        private string Usuario;
        private string Clave;

        protected static Conexion Con = null;

        public Conexion()
        {
            this.Base = "truck_manager_project";
            this.Servidor = "localhost";
            this.Puerto = "3306";
            this.Usuario = "root";
            this.Clave = "";
        }


        //definimos datos de conexión con la base
        public MySqlConnection CrearConexion()
        {
            MySqlConnection cadena = new MySqlConnection();
            try
            {
                cadena.ConnectionString = "datasource=" + this.Servidor +
                                        ";port=" + this.Puerto +
                                        ";username=" + this.Usuario +
                                        ";password=" + this.Clave +
                                        ";Database=" + this.Base;
            }
            catch (Exception e)
            {
                cadena = null;
                throw e;
            }
            return cadena;
        }

        //obtenemos la instancia de la conexión con mysql
        public static Conexion getInstancia()
        {
            if (Con == null)
            {
                Con = new Conexion();
            }
            return Con;
        }


        //testeamos que se haga bien la conexión
        public bool TestConexion()
        {
            try
            {
                using (MySqlConnection conexion = CrearConexion())
                {
                    conexion.Open();  // Intenta abrir la conexión
                    Console.WriteLine("Conexión exitosa.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error de conexión: " + ex.Message);
                return false;
            }
        }


    }
}
