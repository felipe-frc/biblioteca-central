

namespace Biblioteca.Domain.Entities
{
    internal class Livro
    {

        public int Id { get; private set; }
        public string Titulo { get; private set; }
        public string Autor { get; private set; }
        public int AnoPublicacao { get; private set; }
        public bool Disponivel { get; private set; }
       
        public Livro(int id, string titulo, string autor, int anoPublicacao)
        {
            if (id <= 0)
               throw new ArgumentOutOfRangeException(nameof(id), "O ID deve ser maior do que zero.");


            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título não pode ser vazio.", nameof(titulo));

            if (string.IsNullOrWhiteSpace(autor))
                throw new ArgumentException("Autor não pode ser vazio.", nameof(autor));

            int anoAtual = DateTime.Now.Year;
            if (anoPublicacao < 1450 || anoPublicacao > anoAtual)
                throw new ArgumentOutOfRangeException(nameof(anoPublicacao), $"Ano de publicação deve estar entre 1450 e {anoAtual}.");

            Id = id;
            Titulo = titulo;
            Autor = autor;
            AnoPublicacao = anoPublicacao;
            Disponivel = true;
        }

        public void MarcarComoEmprestado()
        {

            if (!Disponivel)
            {

                throw new InvalidOperationException($"O livro: {Titulo} já está emprestado.");
            }

            Disponivel = false;
        }

        public void MarcarComoDevolvido()
        {
            if (Disponivel)
            {

                throw new InvalidOperationException($"O livro: {Titulo} está disponível para empréstimo.");
            }

            Disponivel = true;
        }





    }
}
