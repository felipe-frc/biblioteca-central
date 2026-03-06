
namespace Biblioteca.Domain.Entities
{
    internal class Usuario
    {
       
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }

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




    }
}
