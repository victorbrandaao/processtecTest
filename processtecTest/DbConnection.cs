using System;
using System.Threading.Tasks;
using Npgsql;
using DotNetEnv;
using Serilog;

namespace processtecTest
{
    public class DbConnection
    {
        public static async Task<NpgsqlConnection> GetConnection()
        {
            string connectionString = DotNetEnv.Env.GetString("DATABASE_URL");
            var connection = new NpgsqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();
                Log.Information("Conexão com o banco de dados aberta com sucesso.");
                return connection;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao abrir a conexão com o banco de dados.");
                throw;
            }
        }
    }
}