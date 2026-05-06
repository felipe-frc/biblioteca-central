using System.ComponentModel.DataAnnotations;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Web.ViewModels
{
    public class LivroFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(Livro.TituloMaxLength, ErrorMessage = "O título deve ter no máximo {1} caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O autor é obrigatório.")]
        [StringLength(Livro.AutorMaxLength, ErrorMessage = "O autor deve ter no máximo {1} caracteres.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "A editora é obrigatória.")]
        [StringLength(Livro.EditoraMaxLength, ErrorMessage = "A editora deve ter no máximo {1} caracteres.")]
        public string Editora { get; set; } = string.Empty;

        [Required(ErrorMessage = "A edição é obrigatória.")]
        [StringLength(Livro.EdicaoMaxLength, ErrorMessage = "A edição deve ter no máximo {1} caracteres.")]
        public string Edicao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de publicação é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Publicação")]
        public DateTime? DataPublicacao { get; set; }

        [Required(ErrorMessage = "O número de páginas é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O número de páginas deve ser maior que zero.")]
        [Display(Name = "Número de Páginas")]
        public int? NumeroPaginas { get; set; }
    }
}
