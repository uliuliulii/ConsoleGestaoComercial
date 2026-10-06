using ConsoleGestaoComercial.Models;
using ConsoleGestaoComercial.Services;
using Xunit;

namespace ConsoleGestaoComercial.Tests;

public sealed class ComissaoServiceTests
{
    private readonly ComissaoService _service = new();

    [Theory]
    [InlineData(99.99, 0)]
    [InlineData(100, 1)]
    [InlineData(499.99, 5.00)]
    [InlineData(500, 25)]
    [InlineData(1200.50, 60.03)]
    public void CalcularComissao_AplicaFaixaCorreta(
        decimal valor,
        decimal esperado)
    {
        Assert.Equal(esperado, _service.CalcularComissao(valor));
    }

    [Theory]
    [InlineData(99.99, 0)]
    [InlineData(100, 0.01)]
    [InlineData(499.99, 0.01)]
    [InlineData(500, 0.05)]
    public void ObterPercentual_RetornaPercentualCorreto(
        decimal valor,
        decimal percentualEsperado)
    {
        Assert.Equal(
            percentualEsperado,
            _service.ObterPercentual(valor));
    }

    [Fact]
    public void CalcularDetalhe_DeveRetornarDadosDaVendaEComissao()
    {
        var venda = new Venda
        {
            Vendedor = "Ana",
            Valor = 500m
        };

        var resultado = _service.CalcularDetalhe(venda);

        Assert.Equal("Ana", resultado.Vendedor);
        Assert.Equal(500m, resultado.ValorVenda);
        Assert.Equal(0.05m, resultado.Percentual);
        Assert.Equal(25m, resultado.ValorComissao);
    }

    [Fact]
    public void CalcularPorVendedor_AgrupaESoma()
    {
        var vendas = new[]
        {
            new Venda { Vendedor = "Ana", Valor = 500m },
            new Venda { Vendedor = "Ana", Valor = 100m },
            new Venda { Vendedor = "Bruno", Valor = 99m }
        };

        var resultado = _service.CalcularPorVendedor(vendas);

        var ana = Assert.Single(
            resultado,
            x => x.Vendedor == "Ana");

        var bruno = Assert.Single(
            resultado,
            x => x.Vendedor == "Bruno");

        Assert.Equal(26m, ana.TotalComissao);
        Assert.Equal(0m, bruno.TotalComissao);
    }
}
