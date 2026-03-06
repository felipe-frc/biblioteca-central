using Biblioteca.Domain.Entities;
using Biblioteca.Services;


var service = new BibliotecaService();

// Seed (dados iniciais)
service.CadastrarLivro(new Livro(1, "Clean Code", "Robert C. Martin", 2008));
service.CadastrarLivro(new Livro(2, "The Pragmatic Programmer", "Andrew Hunt", 1999));
service.CadastrarUsuario(new Usuario(1, "Felipe", "felipe@email.com"));

while (true)
{
    Console.Clear();

    Console.WriteLine("----- Biblioteca Central -----");
    Console.WriteLine();
    Console.WriteLine("Olá, seja bem-vindo(a) a nossa Biblioteca Central, confira o nosso menu abaixo e selecione uma das opções: ");
    Console.WriteLine();
    Console.WriteLine("1 - Cadastrar Livro");
    Console.WriteLine("2 - Cadastrar Usuário");
    Console.WriteLine("3 - Realizar Empréstimo");
    Console.WriteLine("4 - Listar Livros Disponíveis");
    Console.WriteLine("5 - Devolver Empréstimo");
    Console.WriteLine("6 - Listar Empréstimos do Usuário");
    Console.WriteLine("7 - Listar Empréstimos Atrasados");
    Console.WriteLine("8 - Remover Livro");
    Console.WriteLine("9 - Remover Usuário");
    Console.WriteLine("0 - Sair");
    Console.WriteLine();

    string? entrada = Console.ReadLine();

    if (!int.TryParse(entrada, out int opcao))
    {
        Console.WriteLine("Opção inválida. Digite um número.");
        Console.WriteLine("Pressione uma tecla para continuar...");
        Console.ReadKey();
        continue;
    }

    try
    {
        switch (opcao)
        {
            
            case 1:
                Console.WriteLine("Cadastro de Livro");
                Console.WriteLine();

                Console.Write("Id: ");
                if (!int.TryParse(Console.ReadLine(), out int idLivro))
                    throw new ArgumentException("Id inválido.");

                Console.Write("Título: ");
                string? titulo = Console.ReadLine();

                Console.Write("Autor: ");
                string? autor = Console.ReadLine();

                Console.Write("Ano de publicação: ");
                if (!int.TryParse(Console.ReadLine(), out int anoPublicacao))
                    throw new ArgumentException("Ano inválido.");

                var livro = new Livro(idLivro, titulo!, autor!, anoPublicacao);
                service.CadastrarLivro(livro);

                Console.WriteLine();
                Console.WriteLine("Livro cadastrado com sucesso!");
                break;
                

            
            case 2:
                Console.WriteLine("Cadastro de Usuário");
                Console.WriteLine();

                Console.Write("Id: ");
                if (!int.TryParse(Console.ReadLine(), out int idUsuario))
                    throw new ArgumentException("Id inválido.");

                Console.Write("Nome: ");
                string? nome = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nome))
                    throw new ArgumentException("Nome não pode ser vazio.");

                Console.Write("Email: ");
                string? email = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Email não pode ser vazio.");

                var usuario = new Usuario(idUsuario, nome.Trim(), email.Trim());
                service.CadastrarUsuario(usuario);

                Console.WriteLine();
                Console.WriteLine("Usuário cadastrado com sucesso!");
                break;

            case 3:
                Console.WriteLine("Realizar Empréstimo");
                Console.WriteLine();

                Console.Write("Id do Livro: ");
                if (!int.TryParse(Console.ReadLine(), out int idLivroEmp))
                    throw new ArgumentException("Id do livro inválido.");

                Console.Write("Id do Usuário: ");
                if (!int.TryParse(Console.ReadLine(), out int idUsuarioEmp))
                    throw new ArgumentException("Id do usuário inválido.");

                Console.Write("Data prevista de devolução (dd/MM/yyyy): ");
                string? dataTexto = Console.ReadLine();

                if (!DateTime.TryParseExact(
                        dataTexto,
                        "dd/MM/yyyy",
                        System.Globalization.CultureInfo.GetCultureInfo("pt-BR"),
                        System.Globalization.DateTimeStyles.None,
                        out DateTime dataPrevista))
                {
                    throw new ArgumentException("Data inválida. Use o formato dd/MM/yyyy.");
                }

                var emprestimo = service.RealizarEmprestimo(idLivroEmp, idUsuarioEmp, dataPrevista);

                Console.WriteLine();
                Console.WriteLine("Empréstimo realizado com sucesso!");
                Console.WriteLine($"Id: {emprestimo.Id}");
                Console.WriteLine($"Livro: {emprestimo.Livro.Titulo}");
                Console.WriteLine($"Usuário: {emprestimo.Usuario.Nome}");
                Console.WriteLine($"Prevista: {emprestimo.DataPrevistaDevolucao:dd/MM/yyyy}");
                Console.WriteLine($"Status: {emprestimo.Status}");
                break;

            case 4:
                var disponiveis = service.ListarLivrosDisponiveis();

                if (disponiveis.Count == 0)
                {
                    Console.WriteLine("Nenhum livro disponível no momento.");
                }
                else
                {
                    Console.WriteLine("Livros disponíveis:");
                    foreach (var livroDisponivel in disponiveis)
                    {
                        Console.WriteLine($"{livroDisponivel.Id} | {livroDisponivel.Titulo} | {livroDisponivel.Autor} | {livroDisponivel.AnoPublicacao}");
                    }
                }
                break;

            case 5:
                Console.WriteLine("Devolver Empréstimo");
                Console.WriteLine();

                Console.Write("Id do Empréstimo: ");
                if (!int.TryParse(Console.ReadLine(), out int idEmprestimo))
                    throw new ArgumentException("Id do empréstimo inválido.");

                service.DevolverEmprestimo(idEmprestimo);

                Console.WriteLine();
                Console.WriteLine("Empréstimo devolvido com sucesso!");
                break;

            case 6:
                Console.WriteLine("Listar Empréstimos do Usuário");
                Console.WriteLine();

                Console.Write("Id do Usuário: ");
                if (!int.TryParse(Console.ReadLine(), out int idUsuarioLista))
                    throw new ArgumentException("Id do usuário inválido.");

                var emprestimosUsuario = service.ListarEmprestimosDoUsuario(idUsuarioLista);

                Console.WriteLine();
                if (emprestimosUsuario.Count == 0)
                {
                    Console.WriteLine("Esse usuário não possui empréstimos.");
                }
                else
                {
                    Console.WriteLine("Empréstimos:");
                    foreach (var e in emprestimosUsuario)
                    {
                        string devolucao = e.DataDevolucao is null ? "-" : e.DataDevolucao.Value.ToString("dd/MM/yyyy");
                        Console.WriteLine($"{e.Id} | Livro: {e.Livro.Titulo} | Prevista: {e.DataPrevistaDevolucao:dd/MM/yyyy} | Devolução: {devolucao} | Status: {e.Status}");
                    }
                }
                break;

            case 7:
                Console.WriteLine("Listar Empréstimos Atrasados");
                Console.WriteLine();

                Console.Write("Data de referência (dd/MM/yyyy): ");
                string? dataRefTexto = Console.ReadLine();

                if (!DateTime.TryParseExact(
                        dataRefTexto,
                        "dd/MM/yyyy",
                        System.Globalization.CultureInfo.GetCultureInfo("pt-BR"),
                        System.Globalization.DateTimeStyles.None,
                        out DateTime dataReferencia))
                {
                    throw new ArgumentException("Data inválida. Use o formato dd/MM/yyyy.");
                }

                var atrasados = service.ListarEmprestimosAtrasados(dataReferencia);

                Console.WriteLine();
                if (atrasados.Count == 0)
                {
                    Console.WriteLine("Nenhum empréstimo atrasado nessa data.");
                }
                else
                {
                    Console.WriteLine("Empréstimos atrasados:");
                    foreach (var e in atrasados)
                    {
                        Console.WriteLine($"{e.Id} | Livro: {e.Livro.Titulo} | Usuário: {e.Usuario.Nome} | Prevista: {e.DataPrevistaDevolucao:dd/MM/yyyy} | Status: {e.Status}");
                    }
                }
                break;

            case 8:
                Console.WriteLine("Remover Livro");
                Console.WriteLine();

                Console.Write("Id do Livro: ");
                if (!int.TryParse(Console.ReadLine(), out int idLivroRemover))
                    throw new ArgumentException("Id do livro inválido.");

                service.RemoverLivro(idLivroRemover);

                Console.WriteLine();
                Console.WriteLine("Livro removido com sucesso!");
                break;

            case 9:
                Console.WriteLine("Remover Usuário");
                Console.WriteLine();

                Console.Write("Id do Usuário: ");
                if (!int.TryParse(Console.ReadLine(), out int idUsuarioRemover))
                    throw new ArgumentException("Id do usuário inválido.");

                service.RemoverUsuario(idUsuarioRemover);

                Console.WriteLine();
                Console.WriteLine("Usuário removido com sucesso!");
                break;

            case 0:
                return;

            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }

    Console.WriteLine();
    Console.WriteLine("Pressione uma tecla para continuar...");
    Console.ReadKey();
}