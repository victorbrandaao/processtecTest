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
            try
            {
                // Carrega as variáveis de ambiente do arquivo .env
                DotNetEnv.Env.Load();

                var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

                var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                Log.Information("Conexão com o banco de dados estabelecida com sucesso.");
                return connection;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao estabelecer conexão com o banco de dados");
                throw;
            }
        }
    }
}