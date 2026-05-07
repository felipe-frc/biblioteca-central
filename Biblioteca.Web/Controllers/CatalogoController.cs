using Biblioteca.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Web.Controllers
{
    [AllowAnonymous]
    public class CatalogoController : Controller
    {
        private readonly BibliotecaDbContext _context;
        private readonly ILogger<CatalogoController> _logger;

        public CatalogoController(BibliotecaDbContext context, ILogger<CatalogoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index(string? busca = null, string disponibilidade = "todos")
        {
            try
            {
                busca = busca?.Trim();

                disponibilidade = string.IsNullOrWhiteSpace(disponibilidade)
                    ? "todos"
                    : disponibilidade.Trim().ToLowerInvariant();

                var query = _context.Livros
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(busca))
                {
                    query = query.Where(l =>
                        l.Titulo.Contains(busca) ||
                        l.Autor.Contains(busca) ||
                        l.Editora.Contains(busca));
                }

                query = disponibilidade switch
                {
                    "disponiveis" => query.Where(l => l.Disponivel),
                    "emprestados" => query.Where(l => !l.Disponivel),
                    _ => query
                };

                var livros = query
                    .OrderBy(l => l.Titulo)
                    .ToList();

                ViewBag.TotalLivros = livros.Count;
                ViewBag.TotalDisponiveis = livros.Count(l => l.Disponivel);
                ViewBag.TotalEmprestados = livros.Count(l => !l.Disponivel);

                ViewBag.Busca = busca ?? string.Empty;
                ViewBag.Disponibilidade = disponibilidade;

                return View(livros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar catálogo público.");
                TempData["Erro"] = "Erro ao carregar o catálogo público. Tente novamente.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}