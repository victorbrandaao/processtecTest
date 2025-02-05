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
                Console.WriteLine("Olá, você está conectado ao banco de dados da processtec.");
                Console.WriteLine($"Horário atual: {DateTime.Now}");
                Console.WriteLine("Iniciando a verificação das tabelas...");

                await VerificarTabelas();

                Console.WriteLine("Verificação das tabelas concluída.");
                Console.WriteLine("Obrigado por utilizar nosso sistema!");
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
                    await ExibirInformacoesTabela(connection, tableName);
                }
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
                await ExibirColunas(connection, tableName);
                await ExibirIndices(connection, tableName);
                await ExibirConteudoTabela(connection, tableName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir informações da tabela '{tableName}': {ex.Message}");
            }
        }

        private static async Task ExibirColunas(NpgsqlConnection connection, string tableName)
        {
            try
            {
                var query = $"SELECT column_name, data_type FROM information_schema.columns WHERE table_name = '{tableName}';";
                await using var command = new NpgsqlCommand(query, connection);
                await using var reader = await command.ExecuteReaderAsync();

                Console.WriteLine($"Colunas da tabela {tableName}:");
                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"Coluna: {reader.GetString(0)}, Tipo: {reader.GetString(1)}");
                }

                using (var cmd = new NpgsqlCommand("SELECT column_name FROM information_schema.columns WHERE table_name = @tableName", connection))
                {
                    cmd.Parameters.AddWithValue("tableName", tableName);
                    using (var reader2 = cmd.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            Console.WriteLine($"  - {reader2["column_name"]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir colunas da tabela '{tableName}': {ex.Message}");
            }
        }

        private static async Task ExibirIndices(NpgsqlConnection connection, string tableName)
        {
            try
            {
                var query = $"SELECT indexname, indexdef FROM pg_indexes WHERE tablename = '{tableName}';";
                await using var command = new NpgsqlCommand(query, connection);
                await using var reader = await command.ExecuteReaderAsync();

                Console.WriteLine($"Índices da tabela {tableName}:");
                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"Índice: {reader.GetString(0)}, Definição: {reader.GetString(1)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exibir índices da tabela '{tableName}': {ex.Message}");
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
                Console.WriteLine($"Erro ao exibir conteúdo da tabela '{tableName}': {ex.Message}");
            }
        }
    }
}