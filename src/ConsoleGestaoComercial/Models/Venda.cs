namespace ConsoleGestaoComercial.Models;

public sealed class Venda
{
    public string Vendedor { get; init; } = string.Empty;
    public decimal Valor { get; init; }
}
