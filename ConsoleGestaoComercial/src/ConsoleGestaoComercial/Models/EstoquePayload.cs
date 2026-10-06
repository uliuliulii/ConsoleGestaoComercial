namespace ConsoleGestaoComercial.Models;

public sealed class EstoquePayload
{
    public List<ProdutoEstoque> Estoque { get; init; } = [];
}
