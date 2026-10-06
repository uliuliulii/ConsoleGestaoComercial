namespace ConsoleGestaoComercial.Models;

public sealed record MovimentacaoEstoque(
    Guid Id,
    DateTime DataHora,
    int CodigoProduto,
    string Produto,
    TipoMovimentacao Tipo,
    int Quantidade,
    string Descricao,
    int EstoqueFinal
);
