namespace ConsoleGestaoComercial.Models;

public sealed class ProdutoEstoque
{
    public int CodigoProduto { get; init; }
    public string DescricaoProduto { get; init; } = string.Empty;
    public int Estoque { get; set; }
}
