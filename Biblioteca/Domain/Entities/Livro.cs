namespace Biblioteca.Domain.Entities
{
    /// <summary>
    /// Entidade que representa um livro na biblioteca.
    /// </summary>
    public class Livro
    {
        public const int TituloMaxLength = 150;
        public const int AutorMaxLength = 120;

        private const int AnoPublicacaoMinimo = 1450;

        public int Id { get; private set; }

        public string Titulo { get; private set; } = default!;

        public string Autor { get; private set; } = default!;

        public int AnoPublicacao { get; private set; }

        public bool Disponivel { get; private set; }

        private Livro()
        {
        }

        /// <summary>
        /// Construtor para criação de novo livro.
        /// </summary>
        public Livro(string titulo, string autor, int anoPublicacao)
        {
            ValidarDados(titulo, autor, anoPublicacao);

            Titulo = titulo.Trim();
            Autor = autor.Trim();
            AnoPublicacao = anoPublicacao;
            Disponivel = true;
        }

        /// <summary>
        /// Construtor para criação de livro com ID, usado em testes.
        /// </summary>
        public Livro(int id, string titulo, string autor, int anoPublicacao)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "O ID deve ser maior que zero.");

            ValidarDados(titulo, autor, anoPublicacao);

            Id = id;
            Titulo = titulo.Trim();
            Autor = autor.Trim();
            AnoPublicacao = anoPublicacao;
            Disponivel = true;
        }

        /// <summary>
        /// Atualiza os dados do livro.
        /// </summary>
        public void AtualizarDados(string titulo, string autor, int anoPublicacao)
        {
            ValidarDados(titulo, autor, anoPublicacao);

            Titulo = titulo.Trim();
            Autor = autor.Trim();
            AnoPublicacao = anoPublicacao;
        }

        /// <summary>
        /// Marca o livro como emprestado.
        /// </summary>
        public void MarcarComoEmprestado()
        {
            if (!Disponivel)
                throw new InvalidOperationException($"O livro '{Titulo}' já está emprestado.");

            Disponivel = false;
        }

        /// <summary>
        /// Marca o livro como devolvido.
        /// </summary>
        public void MarcarComoDevolvido()
        {
            if (Disponivel)
                throw new InvalidOperationException($"O livro '{Titulo}' já está disponível para empréstimo.");

            Disponivel = true;
        }

        /// <summary>
        /// Valida os dados do livro.
        /// </summary>
        private static void ValidarDados(string titulo, string autor, int anoPublicacao)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("O título não pode ser vazio.", nameof(titulo));

            if (string.IsNullOrWhiteSpace(autor))
                throw new ArgumentException("O autor não pode ser vazio.", nameof(autor));

            string tituloTratado = titulo.Trim();
            string autorTratado = autor.Trim();

            if (tituloTratado.Length > TituloMaxLength)
                throw new ArgumentException($"O título deve ter no máximo {TituloMaxLength} caracteres.", nameof(titulo));

            if (autorTratado.Length > AutorMaxLength)
                throw new ArgumentException($"O autor deve ter no máximo {AutorMaxLength} caracteres.", nameof(autor));

            int anoAtual = DateTime.Now.Year;

            if (anoPublicacao < AnoPublicacaoMinimo || anoPublicacao > anoAtual)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(anoPublicacao),
                    $"O ano de publicação deve estar entre {AnoPublicacaoMinimo} e {anoAtual}.");
            }
        }
    }
}