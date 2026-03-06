# Biblioteca Central (Console)

Sistema de biblioteca em C# (.NET 8) com menu no console para cadastro de livros/usuários e controle de empréstimos.

## Funcionalidades
- Cadastrar livro
- Cadastrar usuário
- Realizar empréstimo
- Devolver empréstimo
- Listar livros disponíveis
- Listar empréstimos do usuário
- Listar empréstimos atrasados (por data de referência)
- Remover livro (bloqueia se houver empréstimo em aberto)
- Remover usuário (bloqueia se houver empréstimo em aberto)

## Como executar
### Visual Studio
1. Abra a solution `Biblioteca.sln`
2. Rode com **F5** ou **Ctrl+F5**

### Terminal
```bash
dotnet run --project Biblioteca/Biblioteca.csproj