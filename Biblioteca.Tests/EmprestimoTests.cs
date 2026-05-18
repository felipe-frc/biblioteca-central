using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;
using Xunit;

namespace Biblioteca.Tests;

public class EmprestimoTests
{
    // =========================================================
    // Criação de empréstimo
    // =========================================================

    [Fact]
    public void Emprestimo_Valido_DeveSerCriadoComStatusAtivo()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var dataPrevista = DateTime.Today.AddDays(7);

        var emprestimo = new Emprestimo(livro, usuario, dataPrevista);

        Assert.Equal(livro, emprestimo.Livro);
        Assert.Equal(usuario, emprestimo.Usuario);
        Assert.Equal(livro.Id, emprestimo.LivroId);
        Assert.Equal(usuario.Id, emprestimo.UsuarioId);
        Assert.Equal(dataPrevista.Date, emprestimo.DataPrevistaDevolucao);
        Assert.Equal(StatusEmprestimo.Ativo, emprestimo.Status);
        Assert.Null(emprestimo.DataDevolucao);
        Assert.False(livro.Disponivel);
    }

    [Fact]
    public void Emprestimo_DeveRegistrarDataEmprestimoAoCriar()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var antesDaCriacao = DateTime.Now;

        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today.AddDays(7));

        var depoisDaCriacao = DateTime.Now;

        Assert.True(emprestimo.DataEmprestimo >= antesDaCriacao);
        Assert.True(emprestimo.DataEmprestimo <= depoisDaCriacao);
    }

    [Fact]
    public void Emprestimo_DeveNormalizarDataPrevistaParaDate()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var dataComHorario = DateTime.Today.AddDays(7).AddHours(15).AddMinutes(45);

        var emprestimo = new Emprestimo(livro, usuario, dataComHorario);

        Assert.Equal(DateTime.Today.AddDays(7), emprestimo.DataPrevistaDevolucao);
        Assert.Equal(TimeSpan.Zero, emprestimo.DataPrevistaDevolucao.TimeOfDay);
    }

    [Fact]
    public void Emprestimo_DeveMarcarLivroComoIndisponivelAoCriar()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();

        Assert.True(livro.Disponivel);

        _ = new Emprestimo(livro, usuario, DateTime.Today.AddDays(7));

        Assert.False(livro.Disponivel);
    }

    [Fact]
    public void Emprestimo_ComLivroIndisponivel_DeveLancarExcecao()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();

        livro.MarcarComoEmprestado();

        Assert.Throws<InvalidOperationException>(() =>
            new Emprestimo(livro, usuario, DateTime.Today.AddDays(7)));
    }

    [Fact]
    public void Emprestimo_ComDataNoPassado_DeveLancarExcecao()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();

        var exception = Assert.Throws<ArgumentException>(() =>
            new Emprestimo(livro, usuario, DateTime.Today.AddDays(-1)));

        Assert.Equal("dataPrevistaDevolucao", exception.ParamName);
        Assert.Contains("não pode ser anterior a hoje", exception.Message);
    }

    [Fact]
    public void Emprestimo_ComDataMaiorQue365Dias_DeveLancarExcecao()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();

        var exception = Assert.Throws<ArgumentException>(() =>
            new Emprestimo(livro, usuario, DateTime.Today.AddDays(366)));

        Assert.Equal("dataPrevistaDevolucao", exception.ParamName);
        Assert.Contains("não pode ultrapassar 365 dias", exception.Message);
    }

    [Fact]
    public void Emprestimo_ComDataExatamenteHoje_DeveSerValido()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();

        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today);

        Assert.Equal(DateTime.Today, emprestimo.DataPrevistaDevolucao);
        Assert.Equal(StatusEmprestimo.Ativo, emprestimo.Status);
    }

    [Fact]
    public void Emprestimo_ComDataExatamente365Dias_DeveSerValido()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var dataLimite = DateTime.Today.AddDays(365);

        var emprestimo = new Emprestimo(livro, usuario, dataLimite);

        Assert.Equal(dataLimite, emprestimo.DataPrevistaDevolucao);
        Assert.Equal(StatusEmprestimo.Ativo, emprestimo.Status);
    }

    [Fact]
    public void Emprestimo_ComLivroNulo_DeveLancarExcecao()
    {
        var usuario = CriarUsuario();

        var exception = Assert.Throws<ArgumentNullException>(() =>
            new Emprestimo(null!, usuario, DateTime.Today.AddDays(7)));

        Assert.Equal("livro", exception.ParamName);
    }

    [Fact]
    public void Emprestimo_ComUsuarioNulo_DeveLancarExcecao()
    {
        var livro = CriarLivro();

        var exception = Assert.Throws<ArgumentNullException>(() =>
            new Emprestimo(livro, null!, DateTime.Today.AddDays(7)));

        Assert.Equal("usuario", exception.ParamName);
    }

    // =========================================================
    // Devolução
    // =========================================================

    [Fact]
    public void Devolver_DeveRegistrarDataDevolucaoETornarLivroDisponivel()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today.AddDays(7));

        Assert.False(livro.Disponivel);

        emprestimo.Devolver();

        Assert.NotNull(emprestimo.DataDevolucao);
        Assert.True(livro.Disponivel);
    }

    [Fact]
    public void Devolver_DeveRegistrarDataDevolucaoAtual()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today.AddDays(7));
        var antesDaDevolucao = DateTime.Now;

        emprestimo.Devolver();

        var depoisDaDevolucao = DateTime.Now;

        Assert.NotNull(emprestimo.DataDevolucao);
        Assert.True(emprestimo.DataDevolucao >= antesDaDevolucao);
        Assert.True(emprestimo.DataDevolucao <= depoisDaDevolucao);
    }

    [Fact]
    public void Devolver_DentroDoPrazo_DeveAlterarStatusParaDevolvido()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today.AddDays(7));

        emprestimo.Devolver();

        Assert.Equal(StatusEmprestimo.Devolvido, emprestimo.Status);
    }

    [Fact]
    public void Devolver_NoDiaDoVencimento_DeveAlterarStatusParaDevolvido()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today);

        emprestimo.Devolver();

        Assert.Equal(StatusEmprestimo.Devolvido, emprestimo.Status);
        Assert.True(livro.Disponivel);
    }

    [Fact]
    public void Devolver_AposVencimento_DeveAlterarStatusParaDevolvidoComAtraso()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today);

        DefinirDataPrevistaDevolucao(emprestimo, DateTime.Today.AddDays(-1));

        emprestimo.Devolver();

        Assert.Equal(StatusEmprestimo.DevolvidoComAtraso, emprestimo.Status);
        Assert.True(livro.Disponivel);
        Assert.NotNull(emprestimo.DataDevolucao);
    }

    [Fact]
    public void Devolver_QuandoJaDevolvido_DeveLancarExcecao()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today.AddDays(7));

        emprestimo.Devolver();

        var exception = Assert.Throws<InvalidOperationException>(() => emprestimo.Devolver());

        Assert.Contains("já foi devolvido", exception.Message);
    }

    // =========================================================
    // Status
    // =========================================================

    [Fact]
    public void AtualizarStatus_AntesDoVencimento_DevePermanecerAtivo()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today.AddDays(5));

        emprestimo.AtualizarStatus(DateTime.Today);

        Assert.Equal(StatusEmprestimo.Ativo, emprestimo.Status);
    }

    [Fact]
    public void AtualizarStatus_NoDiaDoVencimento_DevePermanecerAtivo()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var dataPrevista = DateTime.Today.AddDays(5);
        var emprestimo = new Emprestimo(livro, usuario, dataPrevista);

        emprestimo.AtualizarStatus(dataPrevista);

        Assert.Equal(StatusEmprestimo.Ativo, emprestimo.Status);
    }

    [Fact]
    public void AtualizarStatus_ComDataAposVencimento_DeveFicarAtrasado()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today);

        emprestimo.AtualizarStatus(DateTime.Today.AddDays(1));

        Assert.Equal(StatusEmprestimo.Atrasado, emprestimo.Status);
    }

    [Fact]
    public void AtualizarStatus_QuandoEstavaAtrasadoMasReferenciaVoltaParaAntesDoPrazo_DeveFicarAtivo()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var dataPrevista = DateTime.Today.AddDays(5);
        var emprestimo = new Emprestimo(livro, usuario, dataPrevista);

        emprestimo.AtualizarStatus(dataPrevista.AddDays(1));
        Assert.Equal(StatusEmprestimo.Atrasado, emprestimo.Status);

        emprestimo.AtualizarStatus(dataPrevista);

        Assert.Equal(StatusEmprestimo.Ativo, emprestimo.Status);
    }

    [Fact]
    public void AtualizarStatus_QuandoDevolvidoDentroDoPrazo_DeveManterStatusDevolvido()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today.AddDays(7));

        emprestimo.Devolver();
        emprestimo.AtualizarStatus(DateTime.Today.AddDays(30));

        Assert.Equal(StatusEmprestimo.Devolvido, emprestimo.Status);
    }

    [Fact]
    public void AtualizarStatus_QuandoDevolvidoComAtraso_DeveManterStatusDevolvidoComAtraso()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today);

        DefinirDataPrevistaDevolucao(emprestimo, DateTime.Today.AddDays(-2));
        emprestimo.Devolver();

        emprestimo.AtualizarStatus(DateTime.Today.AddDays(30));

        Assert.Equal(StatusEmprestimo.DevolvidoComAtraso, emprestimo.Status);
    }

    // =========================================================
    // Atraso
    // =========================================================

    [Fact]
    public void EstaAtrasado_AntesDoVencimento_DeveRetornarFalso()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today.AddDays(5));

        var resultado = emprestimo.EstaAtrasado(DateTime.Today);

        Assert.False(resultado);
    }

    [Fact]
    public void EstaAtrasado_AposVencimento_DeveRetornarVerdadeiro()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today);

        var resultado = emprestimo.EstaAtrasado(DateTime.Today.AddDays(1));

        Assert.True(resultado);
    }

    [Fact]
    public void EstaAtrasado_NoExatoDiaDoVencimento_DeveRetornarFalso()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var dataPrevista = DateTime.Today.AddDays(3);
        var emprestimo = new Emprestimo(livro, usuario, dataPrevista);

        var resultado = emprestimo.EstaAtrasado(dataPrevista);

        Assert.False(resultado);
    }

    [Fact]
    public void EstaAtrasado_QuandoDevolvidoDentroDoPrazo_DeveRetornarFalsoMesmoComReferenciaFutura()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today.AddDays(7));

        emprestimo.Devolver();

        var resultado = emprestimo.EstaAtrasado(DateTime.Today.AddDays(30));

        Assert.False(resultado);
    }

    [Fact]
    public void EstaAtrasado_QuandoDevolvidoComAtraso_DeveRetornarVerdadeiroMesmoComReferenciaAnterior()
    {
        var livro = CriarLivro();
        var usuario = CriarUsuario();
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Today);

        DefinirDataPrevistaDevolucao(emprestimo, DateTime.Today.AddDays(-2));
        emprestimo.Devolver();

        var resultado = emprestimo.EstaAtrasado(DateTime.Today.AddDays(-10));

        Assert.True(resultado);
    }

    // =========================================================
    // Helpers
    // =========================================================

    private static Livro CriarLivro() =>
        new(
            1,
            "Clean Code",
            "Robert C. Martin",
            "Alta Books",
            "1ª edição",
            new DateTime(2008, 8, 1),
            425
        );

    private static Usuario CriarUsuario() =>
        new(1, "Marcos Felipe", "marcos@email.com");

    private static void DefinirDataPrevistaDevolucao(Emprestimo emprestimo, DateTime dataPrevistaDevolucao)
    {
        var propriedade = typeof(Emprestimo).GetProperty(nameof(Emprestimo.DataPrevistaDevolucao));

        if (propriedade is null)
            throw new InvalidOperationException("Propriedade DataPrevistaDevolucao não encontrada.");

        propriedade.SetValue(emprestimo, dataPrevistaDevolucao.Date);
    }
}