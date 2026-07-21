using HelpDeskMvc.Data;
using HelpDeskMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskMvc.Controllers
{
    public class ChamadosController:Controller
    {
        private readonly IChamadoRepository _repository;

        // O ASP.NET Core fornece automaticamente o Repository
        // que registramos no Program.cs.
        public ChamadosController(IChamadoRepository repository)
        {
            _repository = repository;
        }

        // GET: /Chamados
        // Exibe a lista de chamados.
        public async Task<IActionResult> Index()
        {
            var chamados = await _repository.ObterTodosAsync();

            return View(chamados);
        }

        // GET: /Chamados/Details/5
        // Exibe os detalhes de um chamado.
        public async Task<IActionResult> Details(int id)
        {
            var chamado = await _repository.ObterPorIdAsync(id);

            if (chamado is null)
            {
                return NotFound();
            }

            return View(chamado);
        }

        // GET: /Chamados/Create
        // Apenas abre o formulário.
        public IActionResult Create()
        {
            var chamado = new Chamado
            {
                Prioridade = "Média",
                Status = "Aberto"
            };

            return View(chamado);
        }

        // POST: /Chamados/Create
        // Recebe os dados preenchidos pelo usuário.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Chamado chamado)
        {
            // ModelState.IsValid verifica as regras definidas no Model.
            if (!ModelState.IsValid)
            {
                return View(chamado);
            }

            var novoId = await _repository.CriarAsync(chamado);

            // TempData permanece disponível durante o próximo redirecionamento.
            TempData["MensagemSucesso"] =
                $"Chamado #{novoId} cadastrado com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Chamados/Edit/5
        // Busca o chamado e abre o formulário preenchido.
        public async Task<IActionResult> Edit(int id)
        {
            var chamado = await _repository.ObterPorIdAsync(id);

            if (chamado is null)
            {
                return NotFound();
            }

            return View(chamado);
        }

        // POST: /Chamados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Chamado chamado)
        {
            // Evita atualizar um registro diferente do informado na URL.
            if (id != chamado.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(chamado);
            }

            var atualizado = await _repository.AtualizarAsync(chamado);

            if (!atualizado)
            {
                return NotFound();
            }

            TempData["MensagemSucesso"] =
                $"Chamado #{chamado.Id} atualizado com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Chamados/Delete/5
        // Mostra a tela de confirmação.
        public async Task<IActionResult> Delete(int id)
        {
            var chamado = await _repository.ObterPorIdAsync(id);

            if (chamado is null)
            {
                return NotFound();
            }

            return View(chamado);
        }

        // POST: /Chamados/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            var excluido = await _repository.ExcluirAsync(id);

            if (!excluido)
            {
                return NotFound();
            }

            TempData["MensagemSucesso"] =
                $"Chamado #{id} excluído com sucesso.";

            return RedirectToAction(nameof(Index));
        }
    }
}
