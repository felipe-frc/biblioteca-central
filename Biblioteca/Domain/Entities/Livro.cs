using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Domain.Entities
{
    public class Livro
    {
        public const int TituloMaxLength = 150;
        public const int AutorMaxLength = 120;
        public const int EditoraMaxLength = 120;
        public const int EdicaoMaxLength = 50;

        public int Id { get; private set; }

        public string Titulo { get; private set; } = string.Empty;

        public string Autor { get; private set; } = string.Empty;

        public string Editora { get; private set; } = string.Empty;

        public string Edicao { get; private set; } = string.Empty;

        public DateTime DataPublicacao { get; private set; }

        public int NumeroPaginas { get; private set; }

        public bool Disponivel { get; private set; }

        [NotMapped]
        public int AnoPublicacao => DataPublicacao.Year;

        protected Livro()
        {
        }

        public Livro(
            string titulo,
            string autor,
            string editora,
            string edicao,
            DateTime dataPublicacao,
            int numeroPaginas)
        {
            ValidarDados(titulo, autor, editora, edicao, dataPublicacao, numeroPaginas);

            Titulo = titulo.Trim();
            Autor = autor.Trim();
            Editora = editora.Trim();
            Edicao = edicao.Trim();
            DataPublicacao = dataPublicacao.Date;
            NumeroPaginas = numeroPaginas;
            Disponivel = true;
        }

        public Livro(
            int id,
            string titulo,
            string autor,
            string editora,
            string edicao,
            DateTime dataPublicacao,
            int numeroPaginas)
            : this(titulo, autor, editora, edicao, dataPublicacao, numeroPaginas)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "O ID deve ser maior que zero.");

            Id = id;
        }

        public Livro(string titulo, string autor, int anoPublicacao)
            : this(titulo, autor, "Não informada", "1ª edição", CriarDataPorAno(anoPublicacao), 1)
        {
        }

        public Livro(int id, string titulo, string autor, int anoPublicacao)
            : this(id, titulo, autor, "Não informada", "1ª edição", CriarDataPorAno(anoPublicacao), 1)
        {
        }

        public Livro(string titulo, string autor, string editora, string edicao)
            : this(titulo, autor, editora, edicao, DateTime.Today, 1)
        {
        }

        public Livro(int id, string titulo, string autor, string editora, string edicao)
            : this(id, titulo, autor, editora, edicao, DateTime.Today, 1)
        {
        }

        public void AtualizarDados(
            string titulo,
            string autor,
            string editora,
            string edicao,
            DateTime dataPublicacao,
            int numeroPaginas)
        {
            ValidarDados(titulo, autor, editora, edicao, dataPublicacao, numeroPaginas);

            Titulo = titulo.Trim();
            Autor = autor.Trim();
            Editora = editora.Trim();
            Edicao = edicao.Trim();
            DataPublicacao = dataPublicacao.Date;
            NumeroPaginas = numeroPaginas;
        }

        public void AtualizarDados(string titulo, string autor, int anoPublicacao)
        {
            AtualizarDados(
                titulo,
                autor,
                string.IsNullOrWhiteSpace(Editora) ? "Não informada" : Editora,
                string.IsNullOrWhiteSpace(Edicao) ? "1ª edição" : Edicao,
                CriarDataPorAno(anoPublicacao),
                NumeroPaginas <= 0 ? 1 : NumeroPaginas);
        }

        public void MarcarComoEmprestado()
        {
            if (!Disponivel)
                throw new InvalidOperationException("O livro já está emprestado.");

            Disponivel = false;
        }

        public void MarcarComoDisponivel()
        {
            if (Disponivel)
                throw new InvalidOperationException("O livro já está disponível.");

            Disponivel = true;
        }

        public void MarcarComoDevolvido()
        {
            MarcarComoDisponivel();
        }

        private static DateTime CriarDataPorAno(int anoPublicacao)
        {
            if (anoPublicacao <= 0)
                throw new ArgumentException("O ano de publicação deve ser maior que zero.", nameof(anoPublicacao));

            if (anoPublicacao > DateTime.Today.Year)
                throw new ArgumentException("O ano de publicação não pode ser futuro.", nameof(anoPublicacao));

            return new DateTime(anoPublicacao, 1, 1);
        }

        private static void ValidarDados(
            string titulo,
            string autor,
            string editora,
            string edicao,
            DateTime dataPublicacao,
            int numeroPaginas)
        {
            ValidarTitulo(titulo);
            ValidarAutor(autor);
            ValidarEditora(editora);
            ValidarEdicao(edicao);
            ValidarDataPublicacao(dataPublicacao);
            ValidarNumeroPaginas(numeroPaginas);
        }

        private static void ValidarTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("O título do livro é obrigatório.", nameof(titulo));

            if (titulo.Trim().Length > TituloMaxLength)
                throw new ArgumentException($"O título deve ter no máximo {TituloMaxLength} caracteres.", nameof(titulo));
        }

        private static void ValidarAutor(string autor)
        {
            if (string.IsNullOrWhiteSpace(autor))
                throw new ArgumentException("O autor do livro é obrigatório.", nameof(autor));

            if (autor.Trim().Length > AutorMaxLength)
                throw new ArgumentException($"O autor deve ter no máximo {AutorMaxLength} caracteres.", nameof(autor));
        }

        private static void ValidarEditora(string editora)
        {
            if (string.IsNullOrWhiteSpace(editora))
                throw new ArgumentException("A editora do livro é obrigatória.", nameof(editora));

            if (editora.Trim().Length > EditoraMaxLength)
                throw new ArgumentException($"A editora deve ter no máximo {EditoraMaxLength} caracteres.", nameof(editora));
        }

        private static void ValidarEdicao(string edicao)
        {
            if (string.IsNullOrWhiteSpace(edicao))
                throw new ArgumentException("A edição do livro é obrigatória.", nameof(edicao));

            if (edicao.Trim().Length > EdicaoMaxLength)
                throw new ArgumentException($"A edição deve ter no máximo {EdicaoMaxLength} caracteres.", nameof(edicao));
        }

        private static void ValidarDataPublicacao(DateTime dataPublicacao)
        {
            if (dataPublicacao == default)
                throw new ArgumentException("A data de publicação é obrigatória.", nameof(dataPublicacao));

            if (dataPublicacao.Date > DateTime.Today)
                throw new ArgumentException("A data de publicação não pode ser futura.", nameof(dataPublicacao));
        }

        private static void ValidarNumeroPaginas(int numeroPaginas)
        {
            if (numeroPaginas <= 0)
                throw new ArgumentException("O número de páginas deve ser maior que zero.", nameof(numeroPaginas));
        }
    }
}
