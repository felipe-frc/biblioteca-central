[![CI (.NET)](https://github.com/felipe-frc/biblioteca-central/actions/workflows/dotnet.yml/badge.svg)](https://github.com/felipe-frc/biblioteca-central/actions/workflows/dotnet.yml)

# 📚 Biblioteca Central

Sistema web de gerenciamento de biblioteca desenvolvido com **ASP.NET Core MVC**, **Entity Framework Core** e **SQLite**, com foco em **arquitetura em camadas**, **regras de negócio**, **testes automatizados** e **boas práticas de Engenharia de Software**.

O projeto permite controlar livros, usuários e empréstimos, aplicando validações importantes como indisponibilidade de livros emprestados, bloqueio de exclusão quando há histórico vinculado e prevenção de devoluções duplicadas.

---

## 🌐 Acesse o Projeto

Este projeto ainda não possui deploy público, pois utiliza banco **SQLite local** e foi desenvolvido com foco em execução local, arquitetura, regras de negócio e testes automatizados.

Para executar a aplicação, siga as instruções da seção **Como Executar**.

📂 **Repositório:** [github.com/felipe-frc/biblioteca-central](https://github.com/felipe-frc/biblioteca-central)

---

## 📌 Objetivo do Projeto

Este projeto foi desenvolvido com o objetivo de praticar e demonstrar conhecimentos em:

- Desenvolvimento web com **ASP.NET Core MVC**;
- Persistência de dados com **Entity Framework Core** e **SQLite**;
- Organização em camadas e separação de responsabilidades;
- Criação de regras de negócio para um domínio real;
- Testes automatizados com **xUnit**;
- Integração contínua com **GitHub Actions**;
- Documentação técnica para portfólio profissional.

---

## 🚀 Funcionalidades

### Livros

- Cadastro, listagem, edição e exclusão de livros;
- Controle automático de disponibilidade;
- Livro fica **indisponível** ao ser emprestado;
- Livro volta a ficar **disponível** após devolução;
- Bloqueio de exclusão quando existe histórico de empréstimos vinculado.

### Usuários

- Cadastro, listagem, edição e exclusão de usuários;
- Validação de dados cadastrais;
- Bloqueio de exclusão quando existe histórico de empréstimos vinculado.

### Empréstimos

- Criação e listagem de empréstimos;
- Registro de devoluções;
- Validação contra data retroativa;
- Validação contra empréstimo de livro indisponível;
- Validação contra devolução duplicada;
- Mensagens de sucesso e erro com **Bootstrap Alerts**.

---

## 🛠️ Tecnologias

| Camada | Tecnologia |
|---|---|
| Linguagem | C# / .NET 8 |
| Framework Web | ASP.NET Core MVC |
| ORM | Entity Framework Core |
| Banco de Dados | SQLite |
| Testes | xUnit |
| CI/CD | GitHub Actions |
| Front-end | Bootstrap 5 + Razor Views |

---

## 🏗️ Arquitetura

O projeto utiliza uma organização em camadas para separar responsabilidades e facilitar manutenção, testes e evolução.

```text
biblioteca-central/
│
├── Biblioteca/               # Domínio — entidades, contratos e regras de negócio
│   ├── Models/               # Livro, Usuario, Emprestimo
│   ├── Interfaces/           # Contratos dos repositórios
│   └── Services/             # Serviços de domínio e validações
│
├── Biblioteca.Web/           # Aplicação web MVC
│   ├── Controllers/          # Controllers da aplicação
│   ├── Views/                # Razor Views
│   ├── Data/                 # DbContext e migrations do EF Core
│   └── Program.cs            # Configuração da aplicação e injeção de dependência
│
├── Biblioteca.Tests/         # Testes automatizados com xUnit
│   └── ...                   # Testes de regras de negócio e fluxos principais
│
├── docs/images/              # Imagens utilizadas na documentação
│
├── .github/workflows/        # Pipeline de integração contínua
│   └── dotnet.yml            # Build e testes automatizados
│
└── Biblioteca.sln            # Solution do projeto
```

---

## 📸 Interface do Sistema

### 🏠 Home

![Tela inicial do sistema Biblioteca Central](docs/images/home.png)

### 📋 Listagem de Livros

![Tela de listagem de livros](docs/images/livros.png)

### ➕ Cadastro de Livro

![Tela de cadastro de livro](docs/images/cadastro-livros.png)

### 👨🏻‍💻 Listagem de Usuários

![Tela de listagem de usuários](docs/images/usuarios.png)

### ➕ Cadastro de Usuário

![Tela de cadastro de usuário](docs/images/cadastro-usuario.png)

### 🔄 Controle de Empréstimos

![Tela de controle de empréstimos](docs/images/emprestimos.png)

---

## ⚙️ Como Executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022+ ou VS Code com extensão C#
- Git instalado na máquina

### 1. Clone o repositório

```bash
git clone https://github.com/felipe-frc/biblioteca-central.git
cd biblioteca-central
```

### 2. Restaure as dependências

```bash
dotnet restore
```

### 3. Aplique as migrations do banco de dados

```bash
cd Biblioteca.Web
dotnet ef database update
```

> No Visual Studio, também é possível executar pelo **Console do Gerenciador de Pacotes** com o comando `Update-Database`.

### 4. Execute a aplicação

```bash
dotnet run
```

Ou, a partir da raiz do repositório:

```bash
dotnet run --project Biblioteca.Web
```

### 5. Execute os testes

```bash
dotnet test
```

---

## ✅ Qualidade e Testes

O projeto possui testes automatizados com **xUnit** para validar regras importantes do domínio, como:

- Bloqueio de empréstimo para livro indisponível;
- Validação de devolução de empréstimos;
- Prevenção de operações inválidas;
- Regras relacionadas ao histórico de empréstimos.

Além disso, a pipeline de **GitHub Actions** executa build e testes automaticamente a cada alteração enviada para a branch `main`.

---

## 🧠 Decisões de Desenvolvimento

### Arquitetura em camadas

A separação entre `Biblioteca`, `Biblioteca.Web` e `Biblioteca.Tests` foi adotada para manter o projeto mais organizado, desacoplado e testável. As regras de negócio ficam concentradas no domínio, evitando dependência direta da camada web.

### Entity Framework Core + SQLite

O SQLite foi escolhido por ser simples de configurar, não exigir servidor externo e funcionar bem em projetos de portfólio e execução local. O Entity Framework Core facilita o mapeamento das entidades e o versionamento do banco por meio de migrations.

### Testes com xUnit

O xUnit foi escolhido por sua integração com o ecossistema .NET e por permitir testar regras de negócio de forma objetiva. Os testes ajudam a proteger o sistema contra regressões durante futuras alterações.

### CI/CD com GitHub Actions

A integração contínua automatiza o processo de build e testes, aumentando a confiabilidade do repositório e demonstrando cuidado com qualidade de software.

### Bootstrap + Razor Views

O Bootstrap foi utilizado para acelerar a construção da interface e manter o foco principal do projeto em arquitetura, regras de negócio e persistência de dados.

---

## 📈 Melhorias Futuras

- Deploy em nuvem com Railway, Render ou Azure;
- Autenticação e autorização com ASP.NET Core Identity;
- Busca avançada por título, autor e categoria;
- Dashboard com indicadores de empréstimos e disponibilidade;
- API REST para consumo por aplicações externas;
- Testes de integração com banco em memória;
- Paginação e filtros avançados nas listagens;
- Melhorias de responsividade e acessibilidade.

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE.txt](LICENSE.txt) para mais detalhes.

---

## 👨‍💻 Autor

**Marcos Felipe França**  
[LinkedIn](https://www.linkedin.com/in/marcosfelipefrc) · [GitHub](https://github.com/felipe-frc)
