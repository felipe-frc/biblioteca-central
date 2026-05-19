using Biblioteca.Domain.Entities;
using Xunit;

namespace Biblioteca.Tests;

public class UsuarioTests
{
    // =========================================================
    // Criação
    // =========================================================

    [Fact]
    public void Usuario_Valido_DeveSerCriadoComSucesso()
    {
        var usuario = new Usuario("Marcos Felipe", "marcos@email.com");

        Assert.Equal("Marcos Felipe", usuario.Nome);
        Assert.Equal("marcos@email.com", usuario.Email);
    }

    [Fact]
    public void Usuario_SemId_DeveSerCriadoComIdPadraoZero()
    {
        var usuario = new Usuario("Marcos Felipe", "marcos@email.com");

        Assert.Equal(0, usuario.Id);
    }

    [Fact]
    public void Usuario_ComEspacos_DeveNormalizarNomeEEmail()
    {
        var usuario = new Usuario("  Marcos Felipe  ", "  MARCOS@EMAIL.COM  ");

        Assert.Equal("Marcos Felipe", usuario.Nome);
        Assert.Equal("marcos@email.com", usuario.Email);
    }

    [Fact]
    public void Usuario_EmailDeveFicarEmMinusculo()
    {
        var usuario = new Usuario("Marcos Felipe", "MARCOS.FELIPE@EMAIL.COM");

        Assert.Equal("marcos.felipe@email.com", usuario.Email);
    }

    [Fact]
    public void Usuario_ComEmailComDominioMaiusculo_DeveNormalizarParaMinusculo()
    {
        var usuario = new Usuario("Marcos Felipe", "marcos@EMAIL.COM");

        Assert.Equal("marcos@email.com", usuario.Email);
    }

    [Fact]
    public void Usuario_ComIdValido_DeveSerCriadoComSucesso()
    {
        var usuario = new Usuario(1, "Marcos Felipe", "marcos@email.com");

        Assert.Equal(1, usuario.Id);
        Assert.Equal("Marcos Felipe", usuario.Nome);
        Assert.Equal("marcos@email.com", usuario.Email);
    }

    [Fact]
    public void Usuario_ComIdValidoENomeEmailComEspacos_DeveNormalizarDados()
    {
        var usuario = new Usuario(10, "  Marcos Felipe  ", "  MARCOS@EMAIL.COM  ");

        Assert.Equal(10, usuario.Id);
        Assert.Equal("Marcos Felipe", usuario.Nome);
        Assert.Equal("marcos@email.com", usuario.Email);
    }

    [Fact]
    public void Usuario_ComIdInvalido_DeveLancarExcecao()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Usuario(0, "Marcos Felipe", "marcos@email.com"));

        Assert.Equal("id", exception.ParamName);
        Assert.Contains("maior que zero", exception.Message);
    }

    [Fact]
    public void Usuario_ComIdNegativo_DeveLancarExcecao()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Usuario(-1, "Marcos Felipe", "marcos@email.com"));

        Assert.Equal("id", exception.ParamName);
        Assert.Contains("maior que zero", exception.Message);
    }

    // =========================================================
    // Validações de nome
    // =========================================================

    [Fact]
    public void Usuario_ComNomeVazio_DeveLancarExcecao()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Usuario("", "marcos@email.com"));

        Assert.Equal("nome", exception.ParamName);
        Assert.Contains("nome não pode ser vazio", exception.Message);
    }

    [Fact]
    public void Usuario_ComNomeApenasBrancos_DeveLancarExcecao()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Usuario("   ", "marcos@email.com"));

        Assert.Equal("nome", exception.ParamName);
        Assert.Contains("nome não pode ser vazio", exception.Message);
    }

    [Fact]
    public void Usuario_ComNomeNoTamanhoMaximo_DeveSerCriadoComSucesso()
    {
        var nome = new string('A', Usuario.NomeMaxLength);

        var usuario = new Usuario(nome, "marcos@email.com");

        Assert.Equal(nome, usuario.Nome);
        Assert.Equal(Usuario.NomeMaxLength, usuario.Nome.Length);
    }

    [Fact]
    public void Usuario_ComNomeAcimaDoTamanhoMaximo_DeveLancarExcecao()
    {
        var nome = new string('A', Usuario.NomeMaxLength + 1);

        var exception = Assert.Throws<ArgumentException>(() =>
            new Usuario(nome, "marcos@email.com"));

        Assert.Equal("nome", exception.ParamName);
        Assert.Contains($"máximo {Usuario.NomeMaxLength}", exception.Message);
    }

    [Fact]
    public void Usuario_ComNomeAcimaDoTamanhoMaximoAposTrim_DeveLancarExcecao()
    {
        var nome = $"  {new string('A', Usuario.NomeMaxLength + 1)}  ";

        var exception = Assert.Throws<ArgumentException>(() =>
            new Usuario(nome, "marcos@email.com"));

        Assert.Equal("nome", exception.ParamName);
        Assert.Contains($"máximo {Usuario.NomeMaxLength}", exception.Message);
    }

    [Fact]
    public void Usuario_ComNomeNoTamanhoMaximoMaisEspacos_DeveCriarNormalizado()
    {
        var nome = new string('A', Usuario.NomeMaxLength);

        var usuario = new Usuario($"  {nome}  ", "marcos@email.com");

        Assert.Equal(nome, usuario.Nome);
        Assert.Equal(Usuario.NomeMaxLength, usuario.Nome.Length);
    }

    // =========================================================
    // Validações de email
    // =========================================================

    [Fact]
    public void Usuario_ComEmailVazio_DeveLancarExcecao()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Usuario("Marcos Felipe", ""));

        Assert.Equal("email", exception.ParamName);
        Assert.Contains("email não pode ser vazio", exception.Message);
    }

    [Fact]
    public void Usuario_ComEmailApenasBrancos_DeveLancarExcecao()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Usuario("Marcos Felipe", "   "));

        Assert.Equal("email", exception.ParamName);
        Assert.Contains("email não pode ser vazio", exception.Message);
    }

    [Theory]
    [InlineData("emailsemarroba")]
    [InlineData("marcos@")]
    [InlineData("@email.com")]
    [InlineData("marcos@email")]
    [InlineData("marcos @email.com")]
    [InlineData("marcos@ email.com")]
    [InlineData("marcos@email .com")]
    [InlineData("marcos@@email.com")]
    public void Usuario_ComEmailInvalido_DeveLancarExcecao(string email)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Usuario("Marcos Felipe", email));

        Assert.Equal("email", exception.ParamName);
        Assert.Contains("Email inválido", exception.Message);
    }

    [Theory]
    [InlineData("marcos@email.com")]
    [InlineData("marcos.felipe@email.com")]
    [InlineData("marcos_felipe@email.com")]
    [InlineData("marcos-felipe@email.com")]
    [InlineData("marcos+teste@email.com")]
    public void Usuario_ComEmailValido_DeveSerCriadoComSucesso(string email)
    {
        var usuario = new Usuario("Marcos Felipe", email);

        Assert.Equal(email.ToLowerInvariant(), usuario.Email);
    }

    [Fact]
    public void Usuario_ComEmailNoTamanhoMaximo_DeveSerCriadoComSucesso()
    {
        var dominio = "@email.com";
        var parteLocal = new string('a', Usuario.EmailMaxLength - dominio.Length);
        var email = $"{parteLocal}{dominio}";

        var usuario = new Usuario("Marcos Felipe", email);

        Assert.Equal(email, usuario.Email);
        Assert.Equal(Usuario.EmailMaxLength, usuario.Email.Length);
    }

    [Fact]
    public void Usuario_ComEmailAcimaDoTamanhoMaximo_DeveLancarExcecao()
    {
        var dominio = "@email.com";
        var parteLocal = new string('a', Usuario.EmailMaxLength - dominio.Length + 1);
        var email = $"{parteLocal}{dominio}";

        var exception = Assert.Throws<ArgumentException>(() =>
            new Usuario("Marcos Felipe", email));

        Assert.Equal("email", exception.ParamName);
        Assert.Contains($"máximo {Usuario.EmailMaxLength}", exception.Message);
    }

    [Fact]
    public void Usuario_ComEmailAcimaDoTamanhoMaximoAposTrim_DeveLancarExcecao()
    {
        var dominio = "@email.com";
        var parteLocal = new string('a', Usuario.EmailMaxLength - dominio.Length + 1);
        var email = $"  {parteLocal}{dominio}  ";

        var exception = Assert.Throws<ArgumentException>(() =>
            new Usuario("Marcos Felipe", email));

        Assert.Equal("email", exception.ParamName);
        Assert.Contains($"máximo {Usuario.EmailMaxLength}", exception.Message);
    }

    [Fact]
    public void Usuario_ComEmailNoTamanhoMaximoMaisEspacos_DeveCriarNormalizado()
    {
        var dominio = "@email.com";
        var parteLocal = new string('a', Usuario.EmailMaxLength - dominio.Length);
        var email = $"{parteLocal}{dominio}";

        var usuario = new Usuario("Marcos Felipe", $"  {email.ToUpperInvariant()}  ");

        Assert.Equal(email, usuario.Email);
        Assert.Equal(Usuario.EmailMaxLength, usuario.Email.Length);
    }

    // =========================================================
    // Atualização de dados
    // =========================================================

    [Fact]
    public void AtualizarDados_DeveAtualizarNomeEEmail()
    {
        var usuario = new Usuario("Marcos Felipe", "marcos@email.com");

        usuario.AtualizarDados("Felipe França", "felipe@email.com");

        Assert.Equal("Felipe França", usuario.Nome);
        Assert.Equal("felipe@email.com", usuario.Email);
    }

    [Fact]
    public void AtualizarDados_DeveNormalizarNomeEEmail()
    {
        var usuario = new Usuario("Marcos Felipe", "marcos@email.com");

        usuario.AtualizarDados("  Marcos Felipe França  ", "  MARCOS.FRANCA@EMAIL.COM  ");

        Assert.Equal("Marcos Felipe França", usuario.Nome);
        Assert.Equal("marcos.franca@email.com", usuario.Email);
    }

    [Fact]
    public void AtualizarDados_ComNomeVazio_DeveLancarExcecao()
    {
        var usuario = new Usuario("Marcos Felipe", "marcos@email.com");

        var exception = Assert.Throws<ArgumentException>(() =>
            usuario.AtualizarDados("", "marcos@email.com"));

        Assert.Equal("nome", exception.ParamName);
        Assert.Contains("nome não pode ser vazio", exception.Message);
    }

    [Fact]
    public void AtualizarDados_ComEmailVazio_DeveLancarExcecao()
    {
        var usuario = new Usuario("Marcos Felipe", "marcos@email.com");

        var exception = Assert.Throws<ArgumentException>(() =>
            usuario.AtualizarDados("Marcos Felipe", ""));

        Assert.Equal("email", exception.ParamName);
        Assert.Contains("email não pode ser vazio", exception.Message);
    }

    [Fact]
    public void AtualizarDados_ComEmailInvalido_DeveLancarExcecao()
    {
        var usuario = new Usuario("Marcos Felipe", "marcos@email.com");

        var exception = Assert.Throws<ArgumentException>(() =>
            usuario.AtualizarDados("Marcos Felipe", "invalido"));

        Assert.Equal("email", exception.ParamName);
        Assert.Contains("Email inválido", exception.Message);
    }

    [Fact]
    public void AtualizarDados_ComNomeAcimaDoTamanhoMaximo_DeveLancarExcecao()
    {
        var usuario = new Usuario("Marcos Felipe", "marcos@email.com");
        var nome = new string('A', Usuario.NomeMaxLength + 1);

        var exception = Assert.Throws<ArgumentException>(() =>
            usuario.AtualizarDados(nome, "marcos@email.com"));

        Assert.Equal("nome", exception.ParamName);
        Assert.Contains($"máximo {Usuario.NomeMaxLength}", exception.Message);
    }

    [Fact]
    public void AtualizarDados_ComEmailAcimaDoTamanhoMaximo_DeveLancarExcecao()
    {
        var usuario = new Usuario("Marcos Felipe", "marcos@email.com");
        var dominio = "@email.com";
        var parteLocal = new string('a', Usuario.EmailMaxLength - dominio.Length + 1);
        var email = $"{parteLocal}{dominio}";

        var exception = Assert.Throws<ArgumentException>(() =>
            usuario.AtualizarDados("Marcos Felipe", email));

        Assert.Equal("email", exception.ParamName);
        Assert.Contains($"máximo {Usuario.EmailMaxLength}", exception.Message);
    }

    [Fact]
    public void AtualizarDados_ComDadosInvalidos_NaoDeveAlterarEstadoAtual()
    {
        var usuario = new Usuario("Marcos Felipe", "marcos@email.com");

        Assert.Throws<ArgumentException>(() =>
            usuario.AtualizarDados("", "email-invalido"));

        Assert.Equal("Marcos Felipe", usuario.Nome);
        Assert.Equal("marcos@email.com", usuario.Email);
    }

    [Fact]
    public void AtualizarDados_ComEmailInvalido_NaoDeveAlterarEstadoAtual()
    {
        var usuario = new Usuario("Marcos Felipe", "marcos@email.com");

        Assert.Throws<ArgumentException>(() =>
            usuario.AtualizarDados("Felipe França", "invalido"));

        Assert.Equal("Marcos Felipe", usuario.Nome);
        Assert.Equal("marcos@email.com", usuario.Email);
    }
}