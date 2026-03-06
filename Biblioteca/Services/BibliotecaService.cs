using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;


namespace Biblioteca.Services
{
    internal class BibliotecaService
    {
        private List<Livro> _livros = new();
        private List<Usuario> _usuarios = new();
        private List<Emprestimo> _emprestimos = new();

        public Emprestimo RealizarEmprestimo(int idLivro, int idUsuario, DateTime dataPrevistaDevolucao)
        {
            if (idLivro <= 0)
                throw new ArgumentOutOfRangeException(nameof(idLivro), "O id deve ser maior do que zero.");

            if (idUsuario <= 0)
                throw new ArgumentOutOfRangeException(nameof(idUsuario), "o id deve ser maior do que zero.");

            if (dataPrevistaDevolucao.Date < DateTime.Today)
                throw new ArgumentException("A data prevista para devolução não pode ser menor do que a data de hoje.");

            var livro = BuscarLivroPorId(idLivro);
            var usuario = BuscarUsuarioPorId(idUsuario);
            if (livro is null)
                throw new InvalidOperationException("Livro não encontrado.");
            if (usuario is null)
                throw new InvalidOperationException("Usuário não encontrado.");

            int novoId = _emprestimos.Count + 1;
            var emprestimo = new Emprestimo(novoId, livro, usuario, dataPrevistaDevolucao);

            _emprestimos.Add(emprestimo);
            return emprestimo;

        }

        public void CadastrarLivro(Livro livro)
        {
            if (livro is null)
                throw new ArgumentNullException(nameof(livro));

            if (_livros.Any(l => l.Id == livro.Id))
                throw new InvalidOperationException("Já existe um livro com esse Id.");

            _livros.Add(livro);
        }

        public void CadastrarUsuario(Usuario usuario)
        {
            if (usuario is null)
                throw new ArgumentNullException(nameof(usuario));

            if (_usuarios.Any(u => u.Id == usuario.Id))
                throw new InvalidOperationException("Já existe um usuário com esse Id.");

            string emailNormalizado = usuario.Email.Trim().ToLowerInvariant();

            if (_usuarios.Any(u => u.Email.Trim().ToLowerInvariant() == emailNormalizado))
                throw new InvalidOperationException("Já existe um usuário com esse e-mail.");

            _usuarios.Add(usuario);
        }

        private Livro? BuscarLivroPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id deve ser maior do que zero.");

            return _livros.FirstOrDefault(l => l.Id == id);
        }

        private Usuario? BuscarUsuarioPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id deve ser maior do que zero.");

            return _usuarios.FirstOrDefault(u => u.Id == id);
        }

        public void DevolverEmprestimo(int idEmprestimo)
        {
            if (idEmprestimo <= 0)
                throw new ArgumentOutOfRangeException(nameof(idEmprestimo), "Id do empréstimo deve ser maior que zero.");

            var emprestimo = _emprestimos.FirstOrDefault(e => e.Id == idEmprestimo);

            if (emprestimo is null)
                throw new InvalidOperationException("Empréstimo não encontrado.");

            emprestimo.Devolver();
        }

        public List<Emprestimo> ListarEmprestimosAtrasados(DateTime dataReferencia)
        {
            var atrasados = new List<Emprestimo>();

            foreach (var emprestimo in _emprestimos)
            {
                emprestimo.AtualizarStatus(dataReferencia);

                if (emprestimo.Status == StatusEmprestimo.Atrasado)
                    atrasados.Add(emprestimo);
            }

            return atrasados;
        }

        public List<Livro> ListarLivrosDisponiveis()
        {
            var disponiveis = new List<Livro>();

            foreach (Livro livro in _livros)
            {
                if (livro.Disponivel)
                {
                    disponiveis.Add(livro);
                }
            }

            return disponiveis;
        }

        public List<Emprestimo> ListarEmprestimosDoUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentOutOfRangeException(nameof(idUsuario), "Id do usuário deve ser maior que zero.");

            var usuario = BuscarUsuarioPorId(idUsuario);
            if (usuario is null)
                throw new InvalidOperationException("Usuário não encontrado.");

            var emprestimosDoUsuario = new List<Emprestimo>();

            foreach (var emprestimo in _emprestimos)
            {
                if (emprestimo.Usuario.Id == idUsuario)
                {
                    emprestimosDoUsuario.Add(emprestimo);
                }
            }

            return emprestimosDoUsuario;
        }

        public void RemoverLivro(int idLivro)
        {
            if (idLivro <= 0)
                throw new ArgumentOutOfRangeException(nameof(idLivro), "Id do livro deve ser maior que zero.");

            var livro = BuscarLivroPorId(idLivro);
            if (livro is null)
                throw new InvalidOperationException("Livro não encontrado.");

            bool existeEmprestimoEmAberto =
                _emprestimos.Any(e => e.Livro.Id == idLivro && e.DataDevolucao is null);

            if (existeEmprestimoEmAberto)
                throw new InvalidOperationException("Não é possível remover o livro pois existe empréstimo em aberto.");

            _livros.Remove(livro);
        }

        public void RemoverUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentOutOfRangeException(nameof(idUsuario), "Id do usuário deve ser maior que zero.");

            var usuario = BuscarUsuarioPorId(idUsuario);
            if (usuario is null)
                throw new InvalidOperationException("Usuário não encontrado.");

            bool existeEmprestimoEmAberto =
                _emprestimos.Any(e => e.Usuario.Id == idUsuario && e.DataDevolucao is null);

            if (existeEmprestimoEmAberto)
                throw new InvalidOperationException("Não é possível remover o usuário pois existe empréstimo em aberto.");

            _usuarios.Remove(usuario);
        }

        public List<Emprestimo> ListarEmprestimosAtivos()
        {
            var emprestimosAtivos = new List<Emprestimo>();

            foreach (var emprestimo in _emprestimos)
            {
                if (emprestimo.DataDevolucao is null)
                    emprestimosAtivos.Add(emprestimo);
            }

            return emprestimosAtivos;
        }
    }






}
