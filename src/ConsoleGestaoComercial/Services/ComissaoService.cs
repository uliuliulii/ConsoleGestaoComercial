using ConsoleGestaoComercial.Models;

namespace ConsoleGestaoComercial.Services;

public sealed class ComissaoService
{
    public decimal ObterPercentual(decimal valorVenda)
    {
        if (valorVenda < 0)
            throw new ArgumentOutOfRangeException(
                nameof(valorVenda),
                "O valor da venda não pode ser negativo.");

        return valorVenda switch
        {
            < 100m => 0m,
            < 500m => 0.01m,
            _ => 0.05m
        };
    }

    public decimal CalcularComissao(decimal valorVenda)
    {
        var percentual = ObterPercentual(valorVenda);

        return decimal.Round(
            valorVenda * percentual,
            2,
            MidpointRounding.AwayFromZero);
    }

    public DetalheComissao CalcularDetalhe(Venda venda)
    {
        ArgumentNullException.ThrowIfNull(venda);

        var percentual = ObterPercentual(venda.Valor);
        var valorComissao = CalcularComissao(venda.Valor);

        return new DetalheComissao(
            Vendedor: venda.Vendedor,
            ValorVenda: venda.Valor,
            Percentual: percentual,
            ValorComissao: valorComissao);
    }

    public IReadOnlyList<DetalheComissao> CalcularDetalhes(
        IEnumerable<Venda> vendas)
    {
        ArgumentNullException.ThrowIfNull(vendas);

        return vendas
            .Select(CalcularDetalhe)
            .ToList();
    }

    public IReadOnlyList<ResumoComissao> CalcularPorVendedor(
        IEnumerable<Venda> vendas)
    {
        ArgumentNullException.ThrowIfNull(vendas);

        return vendas
            .GroupBy(v => v.Vendedor)
            .Select(grupo => new ResumoComissao(
                Vendedor: grupo.Key,
                QuantidadeVendas: grupo.Count(),
                TotalVendido: grupo.Sum(v => v.Valor),
                TotalComissao: grupo.Sum(v => CalcularComissao(v.Valor))))
            .OrderBy(r => r.Vendedor)
            .ToList();
    }
}
