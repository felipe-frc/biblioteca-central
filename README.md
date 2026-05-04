[![CI (.NET)](https://github.com/felipe-frc/biblioteca-central/actions/workflows/dotnet.yml/badge.svg)](https://github.com/felipe-frc/biblioteca-central/actions/workflows/dotnet.yml)

# 📚 Biblioteca Central

Sistema web de gerenciamento de biblioteca desenvolvido com **ASP.NET Core MVC**, **Entity Framework Core** e **SQLite**. Implementa controle completo de livros, usuários e empréstimos, com arquitetura em camadas, testes automatizados com **xUnit** e pipeline de CI/CD via **GitHub Actions**.

---

## 🌐 Acesse o Projeto

> 🚧 Deploy em breve — acompanhe as releases para novidades.

📂 **Repositório:** [github.com/felipe-frc/biblioteca-central](https://github.com/felipe-frc/biblioteca-central)

---

## 📌 Funcionalidades

**Livros**
- Cadastrar, listar, editar e excluir
- Livro fica **indisponível** automaticamente ao ser emprestado e volta a ficar **disponível** na devolução
- Exclusão bloqueada quando existe histórico de empréstimos vinculado

**Usuários**
- Cadastrar, listar, editar e excluir
- Exclusão bloqueada quando existe histórico de empréstimos vinculado

**Empréstimos**
- Criar, listar e registrar devoluções
- Validações: data retroativa, livro indisponível, devolução duplicada
- Mensagens de sucesso e erro via Bootstrap Alerts

---

## 🛠️ Tecnologias

| Camada | Tecnologia |
|---|---|
| Linguagem | C# / .NET 8 |
| Framework Web | ASP.NET Core MVC |
| ORM | Entity Framework Core |
| Banco de dados | SQLite |
| Testes | xUnit |
| CI/CD | GitHub Actions |
| Front-end | Bootstrap 5 + Razor Views |

---

## 🏗️ Arquitetura

O projeto é organizado em três camadas com separação clara de responsabilidades:

```
biblioteca-central/
│
├── Biblioteca/               # Domínio — entidades e regras de negócio
│   ├── Models/               # Livro, Usuario, Emprestimo
│   ├── Interfaces/           # Contratos dos repositórios
│   └── Services/             # Lógica de negócio (validações, regras)
│
├── Biblioteca.Web/           # Interface MVC — controllers, views, configuração
│   ├── Controllers/          # LivroController, UsuarioController, EmprestimoController
│   ├── Views/                # Razor Views para cada entidade
│   ├── Data/                 # DbContext e Migrations (EF Core)
│   └── Program.cs            # Entry point e injeção de dependência
│
├── Biblioteca.Tests/         # Testes automatizados com xUnit
│   └── ...                   # Testes de serviços e regras de negócio
│
├── .github/workflows/        # Pipeline CI/CD
│   └── dotnet.yml            # Build + testes automáticos a cada push
│
└── Biblioteca.sln            # Solution file
```

---

## 📸 Interface do Sistema

### 🏠 Página Principal
![Página Principal](docs/images/)

### 📋 Listagem de Livros
<!-- Substitua pelo caminho real após subir as imagens -->
![Listagem de Livros](docs/images/livros.png)

### ➕ Cadastro de Livro
![Cadastro de Livro](docs/images/cadastro.png)

### 🔄 Controle de Empréstimos
![Empréstimos](docs/images/emprestimos.png)

---

## ⚙️ Como Executar

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022+ ou VS Code com extensão C#

### 1. Clone o repositório

```bash
git clone https://github.com/felipe-frc/biblioteca-central.git
cd biblioteca-central
```

### 2. Aplique as migrations (banco de dados)

```bash
cd Biblioteca.Web
dotnet ef database update
```

> Ou pelo Visual Studio: **Console do Gerenciador de Pacotes → `Update-Database`**

### 3. Execute o projeto

```bash
dotnet run --project Biblioteca.Web
```

> Ou pressione `F5` no Visual Studio com `Biblioteca.Web` definido como projeto de inicialização.

### 4. Execute os testes

```bash
dotnet test
```

---

## 🧠 Decisões de Desenvolvimento

**Arquitetura em camadas**
A separação entre `Biblioteca` (domínio), `Biblioteca.Web` (apresentação) e `Biblioteca.Tests` (testes) foi uma decisão consciente para manter o código desacoplado e testável de forma independente. As regras de negócio vivem no domínio, sem depender do framework web.

**Entity Framework Core + SQLite**
SQLite foi escolhido por simplicidade de setup e zero dependências externas — o banco é um único arquivo, ideal para portfólio e ambientes locais. A estrutura de migrations garante que o schema evolua de forma versionada.

**xUnit para testes**
Os testes cobrem as regras de negócio críticas: validação de empréstimo com livro indisponível, bloqueio de exclusão com histórico e regras de devolução. A escolha do xUnit se alinha com o ecossistema .NET e integra nativamente com o GitHub Actions.

**CI/CD com GitHub Actions**
A cada push na branch `main`, o pipeline executa o build e roda todos os testes automaticamente. Isso garante que nenhuma regressão passe despercebida e demonstra maturidade no processo de desenvolvimento.

**Bootstrap para a camada web**
Foco mantido na lógica back-end — Bootstrap foi usado para agilizar a interface sem desviar o esforço do que realmente importa no projeto: as regras de negócio e a arquitetura.

---

## 📈 Melhorias Futuras

- 🚀 Deploy na nuvem (Railway / Azure / Render)
- 🔐 Autenticação e autorização de usuários (ASP.NET Core Identity)
- 🔍 Filtros e busca avançada por título, autor e categoria
- 📊 Dashboard com relatórios de empréstimos e disponibilidade
- 🌐 API REST para consumo por aplicações externas
- 🧪 Testes de integração com banco em memória

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE.txt](LICENSE.txt) para mais detalhes.

---

## 👨‍💻 Autor

**Marcos Felipe França**  
[LinkedIn](https://www.linkedin.com/in/marcosfelipefrc) · [GitHub](https://github.com/felipe-frc)