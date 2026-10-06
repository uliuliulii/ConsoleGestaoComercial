namespace ConsoleGestaoComercial.Models;

public sealed record ResumoComissao(
    string Vendedor,
    int QuantidadeVendas,
    decimal TotalVendido,
    decimal TotalComissao
);
