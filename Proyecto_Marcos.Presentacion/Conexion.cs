using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace Proyecto_Marcos.Presentacion
{
    public class Conexion
    {
        private string Base;
        private String Servidor;
        private String Puerto;
        private String Usuario;
        private String Clave;
        private static Conexion con = null;


        private Conexion()
        {
            this.Base = "proyecto_marcos";
            this.Servidor = "localhost";
            this.Puerto = "3306";
            this.Usuario = "root";
            this.Clave = "";
       
        }

        public MySqlConnection CrearConexion() {
            MySqlConnection cadena = new MySqlConnection();

            try
            {
                cadena.ConnectionString = "datasource=" + this.Servidor +
                                           "; port=" + this.Puerto +
                                           ";username=" + this.Usuario +
                                           ";password=" + this.Clave +
                                           ";Database=" + this.Base;
             }
            catch (Exception ex) {
                cadena = null;
                throw ex;
            }
            return cadena;
        }


        public static Conexion getInstancia()
        {
            if(con == null)
            {
                con = new Conexion();
            }
            return con;
        }
    }
}
