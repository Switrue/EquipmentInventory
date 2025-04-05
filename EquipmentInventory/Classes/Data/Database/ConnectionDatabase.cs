using Npgsql;
using System;
using System.Data;

namespace EquipmentInventory.Classes.Data.Database;

public class ConnectionDatabase
{
    private static NpgsqlConnection GetConnection()
    {
        string connectionString = Environment.GetEnvironmentVariable("INVENTORY_APP_CONNECTION_STRING");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("Строка подключения не найдена в переменных окружения.");
        }

        return new NpgsqlConnection(connectionString);
    }

    public static DataTable ExecuteQuery(string query, params NpgsqlParameter[] parameters)
    {
        DataTable dt = new DataTable();

        using (var conn = GetConnection())
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);

                using (var adp = new NpgsqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
            }
        }

        return dt;
    }

    public static DataTable ExecuteQuery(string query)
    {
        return ExecuteQuery(query, new NpgsqlParameter[0]);
    }
}
