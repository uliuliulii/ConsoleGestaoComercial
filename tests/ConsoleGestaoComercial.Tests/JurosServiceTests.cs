using ConsoleGestaoComercial.Services;
using Xunit;

namespace ConsoleGestaoComercial.Tests;

public sealed class JurosServiceTests
{
    private readonly JurosService _service = new();

    [Fact]
    public void Calcular_ComTresDiasDeAtraso_AplicaDoisEMeioPorCentoAoDia()
    {
        var resultado = _service.Calcular(
            1000m,
            new DateOnly(2026, 10, 1),
            new DateOnly(2026, 10, 4));

        Assert.Equal(3, resultado.DiasAtraso);
        Assert.Equal(75m, resultado.ValorJuros);
        Assert.Equal(1075m, resultado.ValorTotal);
    }

    [Fact]
    public void Calcular_SemAtraso_NaoCobraJuros()
    {
        var resultado = _service.Calcular(
            1000m,
            new DateOnly(2026, 10, 10),
            new DateOnly(2026, 10, 4));

        Assert.Equal(0, resultado.DiasAtraso);
        Assert.Equal(0m, resultado.ValorJuros);
        Assert.Equal(1000m, resultado.ValorTotal);
    }
}
