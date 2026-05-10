using System.Diagnostics;
using Biblioteca.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers
{
    /// <summary>
    /// Controller responsável pelas páginas públicas gerais da aplicação.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Exibe a página inicial pública da Biblioteca Áurea.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Exibe a página de erro da aplicação.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}