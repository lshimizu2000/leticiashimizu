using HelpDeskMvc.Models;

namespace HelpDeskMvc.Data
{
    public interface IChamadoRepository
    {
        // Retorna todos os chamados cadastrados.
        Task<IEnumerable<Chamado>> ObterTodosAsync();

        // Retorna somente um chamado a partir do ID.
        Task<Chamado?> ObterPorIdAsync(int id);

        // Insere um novo chamado no banco.
        Task<int> CriarAsync(Chamado chamado);

        // Atualiza um chamado existente.
        Task<bool> AtualizarAsync(Chamado chamado);

        // Exclui um chamado a partir do ID.
        Task<bool> ExcluirAsync(int id);
    }
}
