using ConsoleGestaoComercial.Models;

namespace ConsoleGestaoComercial.Services;

public sealed class JurosService
{
    public const decimal TaxaDiaria = 0.025m;

    public ResultadoJuros Calcular(
        decimal valor,
        DateOnly dataVencimento,
        DateOnly dataCalculo)
    {
        if (valor < 0)
            throw new ArgumentOutOfRangeException(
                nameof(valor),
                "O valor não pode ser negativo.");

        var diasAtraso = Math.Max(
            0,
            dataCalculo.DayNumber - dataVencimento.DayNumber);

        var juros = decimal.Round(
            valor * TaxaDiaria * diasAtraso,
            2,
            MidpointRounding.AwayFromZero);

        return new ResultadoJuros(
            ValorOriginal: valor,
            DataVencimento: dataVencimento,
            DataCalculo: dataCalculo,
            DiasAtraso: diasAtraso,
            TaxaDiaria: TaxaDiaria,
            ValorJuros: juros,
            ValorTotal: valor + juros);
    }
}
