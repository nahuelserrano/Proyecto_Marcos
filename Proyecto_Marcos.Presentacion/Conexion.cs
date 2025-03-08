using System;
using System.Data.SQLite;

class Conexion
{
    static void Main()
    {
        string connectionString = "Data Source=truck_manager_db.db;Version=3;";
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Crear la tabla 'users' si no existe
            string createTableSql = "CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY, name TEXT, age INTEGER)";
            using (SQLiteCommand createCommand = new SQLiteCommand(createTableSql, connection))
            {
                createCommand.ExecuteNonQuery();
            }

            Console.WriteLine("Base de datos y tabla 'users' creadas exitosamente.");

            // Listar todas las tablas en la base de datos
            string listTablesSql = "SELECT name FROM truck_manager_db.db WHERE type='table';";
            using (SQLiteCommand listCommand = new SQLiteCommand(listTablesSql, connection))
            {
                using (SQLiteDataReader reader = listCommand.ExecuteReader())
                {
                    Console.WriteLine("Tablitassss en la base de datos:");
                    while (reader.Read())
                    {
                        Console.WriteLine("- " + reader["name"]);
                    }
                }
            }
        }
    }
}

