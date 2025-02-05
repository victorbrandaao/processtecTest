using System;
using System.Threading.Tasks;
using Npgsql;

namespace processtecTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Iniciando a verificação das tabelas...");
                await VerificarTabelas();
                Console.WriteLine("Verificação das tabelas concluída.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar tabelas: {ex.Message}");
            }
        }

        public static async Task VerificarTabelas()
        {
            try
            {
                await using var connection = await DbConnection.GetConnection();

                var query = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public';";
                await using var command = new NpgsqlCommand(query, connection);
                await using var reader = await command.ExecuteReaderAsync();

                Console.WriteLine("Tabelas no banco de dados:");
                while (await reader.ReadAsync())
                {
                    var tableName = reader.GetString(0);
                    Console.WriteLine($"Tabela: {tableName}");

                    // Obter e exibir informações detalhadas da tabela
                    await ExibirInformacoesTabela(connection, tableName);
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Erro ao executar a consulta no banco de dados: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar tabelas: {ex.Message}");
            }
        }

        private static async Task ExibirInformacoesTabela(NpgsqlConnection connection, string tableName)
        {
            try
            {
                // Exibir colunas da tabela
                var colunasQuery = $"SELECT column_name, data_type FROM information_schema.columns WHERE table_name = '{tableName}';";
                await using var colunasCommand = new NpgsqlCommand(colunasQuery, connection);
                await using var colunasReader = await colunasCommand.ExecuteReaderAsync();

                Console.WriteLine($"Colunas da tabela {tableName}:");
                while (await colunasReader.ReadAsync())
                {
                    Console.WriteLine($"Coluna: {colunasReader.GetString(0)}, Tipo: {colunasReader.GetString(1)}");
                }
                colunasReader.Close();

                // Exibir índices da tabela
                var indicesQuery = $"SELECT indexname, indexdef FROM pg_indexes WHERE tablename = '{tableName}';";
                await using var indicesCommand = new NpgsqlCommand(indicesQuery, connection);
                await using var indicesReader = await indicesCommand.ExecuteReaderAsync();

                Console.WriteLine($"Índices da tabela {tableName}:");
                while (await indicesReader.ReadAsync())
                {
                    Console.WriteLine($"Índice: {indicesReader.GetString(0)}, Definição: {indicesReader.GetString(1)}");
                }
                indicesReader.Close();

                // Exibir conteúdo da tabela
                await ExibirConteudoTabela(connection, tableName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir informações da tabela {tableName}: {ex.Message}");
            }
        }

        private static async Task ExibirConteudoTabela(NpgsqlConnection connection, string tableName)
        {
            try
            {
                var query = $"SELECT * FROM {tableName};";
                await using var command = new NpgsqlCommand(query, connection);
                await using var reader = await command.ExecuteReaderAsync();

                Console.WriteLine($"Conteúdo da tabela {tableName}:");
                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}: {reader.GetValue(i)}\t");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir conteúdo da tabela {tableName}: {ex.Message}");
            }
        }
    }
}