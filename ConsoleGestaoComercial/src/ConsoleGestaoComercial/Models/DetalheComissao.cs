namespace ConsoleGestaoComercial.Models;

public sealed record DetalheComissao(
    string Vendedor,
    decimal ValorVenda,
    decimal Percentual,
    decimal ValorComissao
);
