using HelpDeskMvc.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpDeskMvc.Data
{
    public class ChamadoRepository : IChamadoRepository
    {
        private readonly string _connectionString;

        public ChamadoRepository(IConfiguration configuration)
        {
            // Busca a connection string configurada no appsettings.json.
            _connectionString =
                configuration.GetConnectionString("HelpDeskConnection")
                ?? throw new InvalidOperationException(
                    "A connection string HelpDeskConnection não foi encontrada.");
        }
        public async Task<IEnumerable<Chamado>> ObterTodosAsync()
        {
            // A lista receberá os objetos lidos no banco.
            var chamados = new List<Chamado>();

            const string sql = """
        SELECT
            Id,
            Titulo,
            Descricao,
            Solicitante,
            Prioridade,
            Status,
            DataAbertura
        FROM Chamados
        ORDER BY DataAbertura DESC;
        """;

            // SqlConnection representa uma conexão com o SQL Server.
            // await using garante que a conexão será descartada corretamente.
            await using var connection = new SqlConnection(_connectionString);

            // SqlCommand representa o comando SQL que será executado.
            await using var command = new SqlCommand(sql, connection);

            // Abre a conexão de maneira assíncrona.
            await connection.OpenAsync();

            // ExecuteReaderAsync é usado quando esperamos várias linhas.
            await using var reader = await command.ExecuteReaderAsync();

            // Cada iteração representa uma linha retornada pelo SELECT.
            while (await reader.ReadAsync())
            {
                var chamado = new Chamado
                {
                    // GetInt32, GetString e GetDateTime convertem os valores
                    // do banco para os tipos correspondentes em C#.
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                    Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                    Solicitante = reader.GetString(reader.GetOrdinal("Solicitante")),
                    Prioridade = reader.GetString(reader.GetOrdinal("Prioridade")),
                    Status = reader.GetString(reader.GetOrdinal("Status")),
                    DataAbertura =
                        reader.GetDateTime(reader.GetOrdinal("DataAbertura"))
                };

                chamados.Add(chamado);
            }

            return chamados;
        }
        public async Task<Chamado?> ObterPorIdAsync(int id)
        {
            const string sql = """
        SELECT
            Id,
            Titulo,
            Descricao,
            Solicitante,
            Prioridade,
            Status,
            DataAbertura
        FROM Chamados
        WHERE Id = @Id;
        """;

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);

            // Nunca concatenamos o ID diretamente no SQL.
            // O parâmetro protege a consulta contra SQL Injection.
            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            // Se nenhuma linha for encontrada, retornamos null.
            if (!await reader.ReadAsync())
            {
                return null;
            }

            return new Chamado
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                Solicitante = reader.GetString(reader.GetOrdinal("Solicitante")),
                Prioridade = reader.GetString(reader.GetOrdinal("Prioridade")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                DataAbertura =
                    reader.GetDateTime(reader.GetOrdinal("DataAbertura"))
            };
        }
        public async Task<int> CriarAsync(Chamado chamado)
        {
            const string sql = """
        INSERT INTO Chamados
        (
            Titulo,
            Descricao,
            Solicitante,
            Prioridade,
            Status,
            DataAbertura
        )
        OUTPUT INSERTED.Id
        VALUES
        (
            @Titulo,
            @Descricao,
            @Solicitante,
            @Prioridade,
            @Status,
            @DataAbertura
        );
        """;

            // Define a data no servidor da aplicação.
            chamado.DataAbertura = DateTime.Now;

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);

            // Declarar explicitamente os tipos ajuda a evitar conversões
            // inesperadas entre C# e SQL Server.
            command.Parameters.Add("@Titulo", SqlDbType.NVarChar, 150)
                .Value = chamado.Titulo;

            command.Parameters.Add("@Descricao", SqlDbType.NVarChar, 1000)
                .Value = chamado.Descricao;

            command.Parameters.Add("@Solicitante", SqlDbType.NVarChar, 120)
                .Value = chamado.Solicitante;

            command.Parameters.Add("@Prioridade", SqlDbType.NVarChar, 20)
                .Value = chamado.Prioridade;

            command.Parameters.Add("@Status", SqlDbType.NVarChar, 30)
                .Value = chamado.Status;

            command.Parameters.Add("@DataAbertura", SqlDbType.DateTime2)
                .Value = chamado.DataAbertura;

            await connection.OpenAsync();

            // ExecuteScalar retorna a primeira coluna da primeira linha.
            // Nesse caso, OUTPUT INSERTED.Id retorna o ID recém-criado.
            var resultado = await command.ExecuteScalarAsync();

            return Convert.ToInt32(resultado);
        }
        public async Task<bool> AtualizarAsync(Chamado chamado)
        {
            const string sql = """
        UPDATE Chamados
        SET
            Titulo = @Titulo,
            Descricao = @Descricao,
            Solicitante = @Solicitante,
            Prioridade = @Prioridade,
            Status = @Status
        WHERE Id = @Id;
        """;

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int)
                .Value = chamado.Id;

            command.Parameters.Add("@Titulo", SqlDbType.NVarChar, 150)
                .Value = chamado.Titulo;

            command.Parameters.Add("@Descricao", SqlDbType.NVarChar, 1000)
                .Value = chamado.Descricao;

            command.Parameters.Add("@Solicitante", SqlDbType.NVarChar, 120)
                .Value = chamado.Solicitante;

            command.Parameters.Add("@Prioridade", SqlDbType.NVarChar, 20)
                .Value = chamado.Prioridade;

            command.Parameters.Add("@Status", SqlDbType.NVarChar, 30)
                .Value = chamado.Status;

            await connection.OpenAsync();

            // ExecuteNonQuery é utilizado para INSERT, UPDATE ou DELETE.
            // Ele retorna a quantidade de linhas afetadas.
            var linhasAfetadas = await command.ExecuteNonQueryAsync();

            return linhasAfetadas > 0;
        }
        public async Task<bool> ExcluirAsync(int id)
        {
            const string sql = """
        DELETE FROM Chamados
        WHERE Id = @Id;
        """;

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();

            var linhasAfetadas = await command.ExecuteNonQueryAsync();

            return linhasAfetadas > 0;
        }
    }
}
