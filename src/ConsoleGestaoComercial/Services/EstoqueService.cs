using ConsoleGestaoComercial.Models;

namespace ConsoleGestaoComercial.Services;

public sealed class EstoqueService
{
    private readonly Dictionary<int, ProdutoEstoque> _produtos;
    private readonly List<MovimentacaoEstoque> _movimentacoes = [];

    public EstoqueService(IEnumerable<ProdutoEstoque> produtos)
    {
        ArgumentNullException.ThrowIfNull(produtos);

        _produtos = produtos.ToDictionary(
            p => p.CodigoProduto,
            p => p);
    }

    public IReadOnlyCollection<ProdutoEstoque> Produtos =>
        _produtos.Values.OrderBy(p => p.CodigoProduto).ToList();

    public IReadOnlyList<MovimentacaoEstoque> Movimentacoes =>
        _movimentacoes.AsReadOnly();

    public MovimentacaoEstoque Movimentar(
        int codigoProduto,
        TipoMovimentacao tipo,
        int quantidade,
        string descricao)
    {
        if (!_produtos.TryGetValue(codigoProduto, out var produto))
            throw new KeyNotFoundException(
                $"Produto de código {codigoProduto} não encontrado.");

        if (quantidade <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantidade),
                "A quantidade deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException(
                "A descrição da movimentação é obrigatória.",
                nameof(descricao));

        if (tipo == TipoMovimentacao.Saida && quantidade > produto.Estoque)
            throw new InvalidOperationException(
                $"Estoque insuficiente. Estoque atual: {produto.Estoque}.");

        produto.Estoque += tipo == TipoMovimentacao.Entrada
            ? quantidade
            : -quantidade;

        var movimentacao = new MovimentacaoEstoque(
            Id: Guid.NewGuid(),
            DataHora: DateTime.Now,
            CodigoProduto: produto.CodigoProduto,
            Produto: produto.DescricaoProduto,
            Tipo: tipo,
            Quantidade: quantidade,
            Descricao: descricao.Trim(),
            EstoqueFinal: produto.Estoque);

        _movimentacoes.Add(movimentacao);

        return movimentacao;
    }
}
