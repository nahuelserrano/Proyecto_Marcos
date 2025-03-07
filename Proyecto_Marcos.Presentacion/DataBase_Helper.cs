using Microsoft.Data.Sqlite;


namespace Proyecto_Marcos.Presentacion
{

    public class DataBase_Helper
    {
        private string connectionString = "Data Source=truck_manager_db.db"; 

        public void Connect()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Conexión exitosa!");
            }
        }
    }
}
