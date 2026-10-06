namespace ConsoleGestaoComercial.Models;

public sealed record ResultadoJuros(
    decimal ValorOriginal,
    DateOnly DataVencimento,
    DateOnly DataCalculo,
    int DiasAtraso,
    decimal TaxaDiaria,
    decimal ValorJuros,
    decimal ValorTotal
);
