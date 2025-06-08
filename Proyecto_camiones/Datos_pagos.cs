

namespace Proyecto_camiones.Presentacion
{
    public class Datos_pagos
    {
        //public DataTable Listado_pagos(String cTexto)
        //{
        //    MySqlDataReader Resultado;
        //    DataTable tabla = new DataTable();
        //    MySqlConnection SqlCon = new MySqlConnection();

        //    try
        //    {
        //        SqlCon = Conexion.getInstancia().CrearConexion();
        //        String sql_tarea = "SELECT * FROM pago where Pagado like '"+cTexto+"' ";
        //        MySqlCommand comando = new MySqlCommand(sql_tarea,SqlCon);
        //        comando.CommandTimeout = 60;
        //        SqlCon.Open();
        //        Resultado = comando.ExecuteReader();
        //        tabla.Load(Resultado);
        //        Console.WriteLine(cTexto);
        //        return tabla;


        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }

        //    finally
        //    {
        //        if (SqlCon.State==ConnectionState.Open)  SqlCon.Close(); 
        //    }
        //}
    }
}
