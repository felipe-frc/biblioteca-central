using Biblioteca.Domain.Entities;
using Biblioteca.Web.Data;
using Biblioteca.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Web.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão de livros.
    /// </summary>
    public class LivrosController : Controller
    {
        private readonly BibliotecaDbContext _context;
        private readonly ILogger<LivrosController> _logger;

        public LivrosController(BibliotecaDbContext context, ILogger<LivrosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Lista livros com paginação.
        /// </summary>
        public IActionResult Index(int page = 1)
        {
            const int pageSize = 6;

            try
            {
                var query = _context.Livros
                    .AsNoTracking()
                    .OrderBy(l => l.Id);

                var totalLivros = query.Count();
                var totalDisponiveis = query.Count(l => l.Disponivel);
                var anoMaisRecente = totalLivros > 0 ? query.Max(l => l.AnoPublicacao).ToString() : "-";

                var totalPages = (int)Math.Ceiling(totalLivros / (double)pageSize);
                if (totalPages == 0) totalPages = 1;

                if (page < 1) page = 1;
                if (page > totalPages) page = totalPages;

                var livros = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.HasPreviousPage = page > 1;
                ViewBag.HasNextPage = page < totalPages;

                ViewBag.TotalLivros = totalLivros;
                ViewBag.TotalDisponiveis = totalDisponiveis;
                ViewBag.AnoMaisRecente = anoMaisRecente;

                _logger.LogInformation("Página de livros acessada. Página: {Page}, Total: {Total}", page, totalLivros);
                return View(livros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar livros na página {Page}", page);
                TempData["Erro"] = "Erro ao carregar livros. Tente novamente.";
                return RedirectToAction(nameof(Index), new { page = 1 });
            }
        }

        /// <summary>
        /// Exibe formulário para criar novo livro.
        /// </summary>
        public IActionResult Create()
        {
            return View(new LivroFormViewModel());
        }

        /// <summary>
        /// Cria novo livro.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LivroFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                _logger.LogInformation("Criando novo livro: {Titulo} por {Autor}", model.Titulo, model.Autor);

                var livro = new Livro(model.Titulo, model.Autor, model.AnoPublicacao!.Value);
                _context.Livros.Add(livro);
                _context.SaveChanges();

                _logger.LogInformation("Livro criado com sucesso. ID: {LivroId}, Título: {Titulo}", livro.Id, livro.Titulo);
                TempData["Sucesso"] = "Livro cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao criar livro: {Titulo}", model.Titulo);

                if (ex.ParamName == "anoPublicacao")
                    ModelState.AddModelError(nameof(model.AnoPublicacao), "Informe um ano de publicação válido.");
                else
                    ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro de banco de dados ao criar livro: {Titulo}", model.Titulo);
                ModelState.AddModelError(string.Empty, "Erro ao salvar o livro. Verifique se os dados já existem.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar livro: {Titulo}", model.Titulo);
                ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao salvar o livro.");
                return View(model);
            }
        }

        /// <summary>
        /// Exibe formulário para editar livro.
        /// </summary>
        public IActionResult Edit(int id)
        {
            try
            {
                var livro = _context.Livros.FirstOrDefault(l => l.Id == id);
                if (livro is null)
                {
                    _logger.LogWarning("Tentativa de editar livro inexistente. ID: {LivroId}", id);
                    return NotFound();
                }

                var model = new LivroFormViewModel
                {
                    Id = livro.Id,
                    Titulo = livro.Titulo,
                    Autor = livro.Autor,
                    AnoPublicacao = livro.AnoPublicacao
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar formulário de edição. ID: {LivroId}", id);
                return NotFound();
            }
        }

        /// <summary>
        /// Atualiza livro existente.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LivroFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var livro = _context.Livros.FirstOrDefault(l => l.Id == model.Id);
                if (livro is null)
                {
                    _logger.LogWarning("Tentativa de atualizar livro inexistente. ID: {LivroId}", model.Id);
                    return NotFound();
                }

                _logger.LogInformation("Atualizando livro. ID: {LivroId}, Novo título: {Titulo}", model.Id, model.Titulo);

                livro.AtualizarDados(model.Titulo, model.Autor, model.AnoPublicacao!.Value);
                _context.SaveChanges();

                _logger.LogInformation("Livro atualizado com sucesso. ID: {LivroId}", model.Id);
                TempData["Sucesso"] = "Livro atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao atualizar livro. ID: {LivroId}", model.Id);

                if (ex.ParamName == "anoPublicacao")
                    ModelState.AddModelError(nameof(model.AnoPublicacao), "Informe um ano de publicação válido.");
                else
                    ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro de banco de dados ao atualizar livro. ID: {LivroId}", model.Id);
                ModelState.AddModelError(string.Empty, "Erro ao atualizar o livro.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar livro. ID: {LivroId}", model.Id);
                ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao atualizar o livro.");
                return View(model);
            }
        }

        /// <summary>
        /// Exibe confirmação de exclusão.
        /// </summary>
        public IActionResult Delete(int id)
        {
            try
            {
                var livro = _context.Livros.AsNoTracking().FirstOrDefault(l => l.Id == id);
                if (livro is null)
                {
                    _logger.LogWarning("Tentativa de deletar livro inexistente. ID: {LivroId}", id);
                    return NotFound();
                }

                bool temEmprestimoRelacionado = _context.Emprestimos.Any(e => e.LivroId == id);
                ViewBag.TemEmprestimoRelacionado = temEmprestimoRelacionado;

                return View(livro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar página de exclusão. ID: {LivroId}", id);
                return NotFound();
            }
        }

        /// <summary>
        /// Confirma e executa exclusão do livro.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var livro = _context.Livros.FirstOrDefault(l => l.Id == id);
                if (livro is null)
                {
                    _logger.LogWarning("Tentativa de deletar livro inexistente. ID: {LivroId}", id);
                    return NotFound();
                }

                bool temEmprestimoRelacionado = _context.Emprestimos.Any(e => e.LivroId == id);
                if (temEmprestimoRelacionado)
                {
                    _logger.LogWarning("Tentativa de deletar livro com empréstimos. ID: {LivroId}", id);
                    TempData["Erro"] = "Não é possível excluir este livro porque ele já possui empréstimos registrados.";
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogInformation("Deletando livro. ID: {LivroId}, Título: {Titulo}", id, livro.Titulo);

                _context.Livros.Remove(livro);
                _context.SaveChanges();

                _logger.LogInformation("Livro deletado com sucesso. ID: {LivroId}", id);
                TempData["Sucesso"] = "Livro excluído com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro de banco de dados ao deletar livro. ID: {LivroId}", id);
                TempData["Erro"] = "Erro ao excluir o livro. Tente novamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar livro. ID: {LivroId}", id);
                TempData["Erro"] = "Ocorreu um erro inesperado ao excluir o livro.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}