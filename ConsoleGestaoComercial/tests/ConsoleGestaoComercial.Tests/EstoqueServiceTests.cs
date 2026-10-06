using ConsoleGestaoComercial.Models;
using ConsoleGestaoComercial.Services;
using Xunit;

namespace ConsoleGestaoComercial.Tests;

public sealed class EstoqueServiceTests
{
    private static EstoqueService CriarService() =>
        new([
            new ProdutoEstoque
            {
                CodigoProduto = 101,
                DescricaoProduto = "Caneta Azul",
                Estoque = 10
            }
        ]);

    [Fact]
    public void Entrada_AumentaEstoque()
    {
        var service = CriarService();

        var mov = service.Movimentar(
            101,
            TipoMovimentacao.Entrada,
            5,
            "Reposição");

        Assert.Equal(15, mov.EstoqueFinal);
        Assert.NotEqual(Guid.Empty, mov.Id);
    }

    [Fact]
    public void Saida_DiminuiEstoque()
    {
        var service = CriarService();

        var mov = service.Movimentar(
            101,
            TipoMovimentacao.Saida,
            4,
            "Venda");

        Assert.Equal(6, mov.EstoqueFinal);
    }

    [Fact]
    public void SaidaMaiorQueEstoque_LancaErro()
    {
        var service = CriarService();

        Assert.Throws<InvalidOperationException>(() =>
            service.Movimentar(
                101,
                TipoMovimentacao.Saida,
                11,
                "Venda"));
    }
}
