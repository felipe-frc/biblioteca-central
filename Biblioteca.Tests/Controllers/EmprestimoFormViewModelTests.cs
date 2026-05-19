using System.ComponentModel.DataAnnotations;
using Biblioteca.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xunit;

namespace Biblioteca.Tests.ViewModels;

public class EmprestimoFormViewModelTests
{
    [Fact]
    public void Validate_SemDataPrevista_NaoDeveRetornarErroCustomizado()
    {
        var model = new EmprestimoFormViewModel
        {
            UsuarioId = 1,
            LivroId = 1,
            DataPrevistaDevolucao = null
        };

        var resultados = ValidarCustomizado(model);

        Assert.Empty(resultados);
    }

    [Fact]
    public void Validate_ComDataHoje_DeveSerValido()
    {
        var model = new EmprestimoFormViewModel
        {
            UsuarioId = 1,
            LivroId = 1,
            DataPrevistaDevolucao = DateTime.Today
        };

        var resultados = ValidarCustomizado(model);

        Assert.Empty(resultados);
    }

    [Fact]
    public void Validate_ComDataFuturaDentroDoLimite_DeveSerValido()
    {
        var model = new EmprestimoFormViewModel
        {
            UsuarioId = 1,
            LivroId = 1,
            DataPrevistaDevolucao = DateTime.Today.AddDays(30)
        };

        var resultados = ValidarCustomizado(model);

        Assert.Empty(resultados);
    }

    [Fact]
    public void Validate_ComDataExatamente365Dias_DeveSerValido()
    {
        var model = new EmprestimoFormViewModel
        {
            UsuarioId = 1,
            LivroId = 1,
            DataPrevistaDevolucao = DateTime.Today.AddDays(365)
        };

        var resultados = ValidarCustomizado(model);

        Assert.Empty(resultados);
    }

    [Fact]
    public void Validate_ComDataAnteriorHoje_DeveRetornarErro()
    {
        var model = new EmprestimoFormViewModel
        {
            UsuarioId = 1,
            LivroId = 1,
            DataPrevistaDevolucao = DateTime.Today.AddDays(-1)
        };

        var resultados = ValidarCustomizado(model).ToList();

        Assert.Single(resultados);
        Assert.Equal(
            "A data prevista para devolução não pode ser anterior à data de hoje.",
            resultados[0].ErrorMessage);
        Assert.Contains(nameof(EmprestimoFormViewModel.DataPrevistaDevolucao), resultados[0].MemberNames);
    }

    [Fact]
    public void Validate_ComDataMaiorQue365Dias_DeveRetornarErro()
    {
        var model = new EmprestimoFormViewModel
        {
            UsuarioId = 1,
            LivroId = 1,
            DataPrevistaDevolucao = DateTime.Today.AddDays(366)
        };

        var resultados = ValidarCustomizado(model).ToList();

        Assert.Single(resultados);
        Assert.Equal(
            "A data prevista para devolução não pode ultrapassar 365 dias a partir de hoje.",
            resultados[0].ErrorMessage);
        Assert.Contains(nameof(EmprestimoFormViewModel.DataPrevistaDevolucao), resultados[0].MemberNames);
    }

    [Fact]
    public void ValidacaoComDataAnnotations_SemUsuario_DeveRetornarErroObrigatorio()
    {
        var model = new EmprestimoFormViewModel
        {
            UsuarioId = null,
            LivroId = 1,
            DataPrevistaDevolucao = DateTime.Today.AddDays(7)
        };

        var resultados = ValidarComDataAnnotations(model);

        Assert.Contains(resultados, r =>
            r.ErrorMessage == "Selecione um usuário." &&
            r.MemberNames.Contains(nameof(EmprestimoFormViewModel.UsuarioId)));
    }

    [Fact]
    public void ValidacaoComDataAnnotations_SemLivro_DeveRetornarErroObrigatorio()
    {
        var model = new EmprestimoFormViewModel
        {
            UsuarioId = 1,
            LivroId = null,
            DataPrevistaDevolucao = DateTime.Today.AddDays(7)
        };

        var resultados = ValidarComDataAnnotations(model);

        Assert.Contains(resultados, r =>
            r.ErrorMessage == "Selecione um livro." &&
            r.MemberNames.Contains(nameof(EmprestimoFormViewModel.LivroId)));
    }

    [Fact]
    public void ValidacaoComDataAnnotations_SemDataPrevista_DeveRetornarErroObrigatorio()
    {
        var model = new EmprestimoFormViewModel
        {
            UsuarioId = 1,
            LivroId = 1,
            DataPrevistaDevolucao = null
        };

        var resultados = ValidarComDataAnnotations(model);

        Assert.Contains(resultados, r =>
            r.ErrorMessage == "Informe a data prevista para devolução." &&
            r.MemberNames.Contains(nameof(EmprestimoFormViewModel.DataPrevistaDevolucao)));
    }

    [Fact]
    public void EmprestimoFormViewModel_DeveIniciarCombosVazios()
    {
        var model = new EmprestimoFormViewModel();

        Assert.NotNull(model.Usuarios);
        Assert.NotNull(model.Livros);
        Assert.Empty(model.Usuarios);
        Assert.Empty(model.Livros);
    }

    [Fact]
    public void EmprestimoFormViewModel_DevePermitirPreencherCombos()
    {
        var model = new EmprestimoFormViewModel
        {
            Usuarios =
            {
                new SelectListItem { Value = "1", Text = "Marcos Felipe" }
            },
            Livros =
            {
                new SelectListItem { Value = "1", Text = "Clean Code" }
            }
        };

        Assert.Single(model.Usuarios);
        Assert.Single(model.Livros);
        Assert.Equal("Marcos Felipe", model.Usuarios[0].Text);
        Assert.Equal("Clean Code", model.Livros[0].Text);
    }

    private static IEnumerable<ValidationResult> ValidarCustomizado(EmprestimoFormViewModel model)
    {
        var context = new ValidationContext(model);
        return model.Validate(context);
    }

    private static List<ValidationResult> ValidarComDataAnnotations(EmprestimoFormViewModel model)
    {
        var resultados = new List<ValidationResult>();
        var context = new ValidationContext(model);

        Validator.TryValidateObject(
            model,
            context,
            resultados,
            validateAllProperties: true);

        return resultados;
    }
}