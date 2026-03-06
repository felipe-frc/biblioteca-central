using Biblioteca.Domain.Enums;


namespace Biblioteca.Domain.Entities
{
    internal class Emprestimo
    {
        public int Id { get; private set; }
        public Livro Livro { get; private set; }
        public Usuario Usuario { get; private set; }
        public DateTime DataEmprestimo { get; private set; }
        public DateTime DataPrevistaDevolucao { get; private set; }
        public DateTime? DataDevolucao { get; private set; }
        public StatusEmprestimo Status { get; private set; }

        public Emprestimo(int id, Livro livro, Usuario usuario, DateTime dataPrevistaDevolucao)
        {

            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id deve ser maior que zero.");

            if (livro is null)
                throw new ArgumentNullException(nameof(livro));

            if (usuario is null)
                throw new ArgumentNullException(nameof(usuario));

            if (dataPrevistaDevolucao.Date < DateTime.Today)
                throw new ArgumentException("A data da devolução não pode ser no passado.", nameof(dataPrevistaDevolucao));

            livro.MarcarComoEmprestado();


            Id = id;
            Livro = livro;
            Usuario = usuario;
            DataEmprestimo = DateTime.Now;
            DataPrevistaDevolucao = dataPrevistaDevolucao.Date;
            Status = StatusEmprestimo.Ativo;


        }

        public void Devolver()
        {
            if (DataDevolucao is not null)
                throw new InvalidOperationException("Empréstimo já foi devolvido.");

            DataDevolucao = DateTime.Now;
            Livro.MarcarComoDevolvido();

            if (DataDevolucao.Value.Date > DataPrevistaDevolucao.Date)
                Status = StatusEmprestimo.Atrasado;
            else
                Status = StatusEmprestimo.Devolvido;

        }

        public bool EstaAtrasado(DateTime dataReferencia)
        {
            DateTime dataComparacao = DataDevolucao?.Date ?? dataReferencia.Date;
            return dataComparacao > DataPrevistaDevolucao.Date;
        }

        public void AtualizarStatus(DateTime dataReferencia)
        {
            if (DataDevolucao is not null)
            {
                Status = DataDevolucao.Value.Date > DataPrevistaDevolucao.Date
                    ? StatusEmprestimo.Atrasado
                    : StatusEmprestimo.Devolvido;
                return;
            }

            Status = dataReferencia.Date > DataPrevistaDevolucao.Date
                ? StatusEmprestimo.Atrasado
                : StatusEmprestimo.Ativo;
        }
    

    }


}
