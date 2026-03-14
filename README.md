[![CI (.NET)](https://github.com/felipe-frc/biblioteca-central-console/actions/workflows/dotnet.yml/badge.svg)](https://github.com/felipe-frc/biblioteca-central-console/actions/workflows/dotnet.yml)

# Biblioteca Central

Sistema de biblioteca em **C# / .NET** com **CRUD de Livros e Usuários** e **controle de Empréstimos** (criar, listar e devolver).

## Funcionalidades
- Livros:
  - Cadastrar, listar, editar e excluir
  - Livro fica **indisponível** quando emprestado e volta a ficar **disponível** ao devolver
  - Exclusão bloqueada se existir **histórico de empréstimos**
- Usuários:
  - Cadastrar, listar, editar e excluir
  - Exclusão bloqueada se existir **histórico de empréstimos**
- Empréstimos:
  - Criar, listar e devolver
  - Validações: data no passado, livro indisponível, devolução repetida
- UX:
  - Mensagens de sucesso/erro (Bootstrap Alerts)

## Tecnologias
- C#
- .NET (Console + MVC Web, conforme estrutura do projeto)
- Entity Framework Core + SQLite (na versão Web)
- Bootstrap (na versão Web)

## Como executar (Visual Studio)
1. Clone o repositório
2. Abra a solução (`.sln`) no Visual Studio
3. Defina o projeto de inicialização (Console ou Web)
4. Rode com `F5`

## Banco de dados (SQLite + EF Core)
Se estiver usando o projeto Web:
1. Abra o **Console do Gerenciador de Pacotes**
2. Execute:
   - `Update-Database`

> Se precisar zerar em desenvolvimento: apague `biblioteca.db` e rode `Update-Database` novamente.

## Estrutura (alto nível)
- `Biblioteca` / `Domain`: entidades e regras de negócio (Livro, Usuário, Empréstimo)
- `Biblioteca.Web`: interface MVC (CRUD + empréstimos)
- `Migrations`: versionamento do banco (EF Core)

## Autor
Felipe França
