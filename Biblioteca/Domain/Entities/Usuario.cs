
namespace Biblioteca.Domain.Entities
{
    public class Usuario
    {
       
        public int Id { get; private set; }
        public string Nome { get; private set; } = default!;
        public string Email { get; private set; } = default!;

        public Usuario(int id, string nome, string email)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "O Id deve ser maior do que zero");

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome não pode ser vazio", nameof(nome));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O email não pode ser vazio", nameof(email));

            if (!email.Contains("@") || !email.Contains(".")
                || email.StartsWith("@") || email.EndsWith("@"))
                throw new ArgumentException("Email inválido", nameof(email));

            Id = id;
            Nome = nome;
            Email = email;
        }

        private Usuario() { }

        public Usuario(string nome, string email)
        {
            // validações
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio.", nameof(nome));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio.", nameof(email));

            if (!email.Contains("@") || !email.Contains(".") || email.StartsWith("@") || email.EndsWith("@"))
                throw new ArgumentException("Email inválido", nameof(email));

            Nome = nome.Trim();
            Email = email.Trim();
        }

        public void AtualizarDados(string nome, string email)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome não pode ser vazio.", nameof(nome));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O email não pode ser vazio.", nameof(email));

            if (!email.Contains("@") || !email.Contains(".") || email.StartsWith("@") || email.EndsWith("@"))
                throw new ArgumentException("Email inválido", nameof(email));

            Nome = nome.Trim();
            Email = email.Trim();
        }




    }
}
