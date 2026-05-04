using Biblioteca.Domain.Entities;
using Biblioteca.Web.Data;
using Biblioteca.Web.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Biblioteca.Tests.Integration;

public class EmprestimoAppServiceIntegrationTests
{
    private static BibliotecaDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<BibliotecaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new BibliotecaDbContext(options);
    }

    [Fact]
    public void Realizar_DeveCriarEmprestimoEDeixarLivroIndisponivel()
    {
        using var context = CriarContexto();
        var livro = new Livro("Clean Code", "Robert C. Martin", 2008);
        var usuario = new Usuario("Felipe", "felipe@email.com");

        context.Livros.Add(livro);
        context.Usuarios.Add(usuario);
        context.SaveChanges();

        var service = new EmprestimoAppService(context);

        var emprestimo = service.Realizar(livro.Id, usuario.Id, DateTime.Today.AddDays(7));

        Assert.NotEqual(0, emprestimo.Id);
        Assert.False(emprestimo.Livro.Disponivel);
        Assert.Single(context.Emprestimos);
    }

    [Fact]
    public void Realizar_QuandoLivroNaoExiste_DeveLancarExcecao()
    {
        using var context = CriarContexto();
        var usuario = new Usuario("Felipe", "felipe@email.com");
        context.Usuarios.Add(usuario);
        context.SaveChanges();

        var service = new EmprestimoAppService(context);

        Assert.Throws<InvalidOperationException>(() =>
            service.Realizar(999, usuario.Id, DateTime.Today.AddDays(7)));
    }

    [Fact]
    public void Devolver_DeveAtualizarDataDevolucaoELivroDisponivel()
    {
        using var context = CriarContexto();
        var livro = new Livro("DDD", "Eric Evans", 2003);
        var usuario = new Usuario("Ana", "ana@email.com");
        context.Livros.Add(livro);
        context.Usuarios.Add(usuario);
        context.SaveChanges();

        var service = new EmprestimoAppService(context);
        var emprestimo = service.Realizar(livro.Id, usuario.Id, DateTime.Today.AddDays(1));

        service.Devolver(emprestimo.Id);

        var emprestimoAtualizado = context.Emprestimos
            .Include(e => e.Livro)
            .First(e => e.Id == emprestimo.Id);

        Assert.NotNull(emprestimoAtualizado.DataDevolucao);
        Assert.True(emprestimoAtualizado.Livro.Disponivel);
    }
}