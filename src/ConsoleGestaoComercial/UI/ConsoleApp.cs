using System.Globalization;
using System.Text.Json;
using ConsoleGestaoComercial.Models;
using ConsoleGestaoComercial.Services;

namespace ConsoleGestaoComercial.UI;

public sealed class ConsoleApp
{
    private readonly JsonSerializerOptions _jsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    public async Task ExecutarAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("          CONSOLE DE GESTÃO COMERCIAL");
            Console.WriteLine("========================================");
            Console.WriteLine("1 - Comissão de vendedores");
            Console.WriteLine("2 - Movimentação de estoque");
            Console.WriteLine("3 - Cálculo de juros");
            Console.WriteLine("0 - Sair");
            Console.WriteLine();
            Console.Write("Escolha uma opção: ");

            var opcao = Console.ReadLine();

            try
            {
                switch (opcao)
                {
                    case "1":
                        await ExecutarComissoesAsync();
                        break;
                    case "2":
                        await ExecutarEstoqueAsync();
                        break;
                    case "3":
                        ExecutarJuros();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        Pausar();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Erro: {ex.Message}");
                Pausar();
            }
        }
    }

    private async Task ExecutarComissoesAsync()
    {
        Console.Clear();

        var caminho = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "vendas.json");

        var dados = await LerJsonAsync<VendasPayload>(caminho);

        var service = new ComissaoService();
        var detalhes = service.CalcularDetalhes(dados.Vendas);
        var resumo = service.CalcularPorVendedor(dados.Vendas);

        Console.WriteLine("DETALHAMENTO DAS VENDAS");
        Console.WriteLine(new string('-', 82));
        Console.WriteLine(
            $"{"Vendedor",-24} {"Valor da venda",18} {"Comissão %",12} {"Comissão",16}");
        Console.WriteLine(new string('-', 82));

        foreach (var item in detalhes)
        {
            Console.WriteLine(
                $"{item.Vendedor,-24} " +
                $"{item.ValorVenda,18:C} " +
                $"{item.Percentual,11:P0} " +
                $"{item.ValorComissao,16:C}");
        }

        Console.WriteLine();
        Console.WriteLine("RESUMO POR VENDEDOR");
        Console.WriteLine(new string('-', 72));
        Console.WriteLine(
            $"{"Vendedor",-24} {"Vendas",8} {"Total vendido",18} {"Comissão",16}");
        Console.WriteLine(new string('-', 72));

        foreach (var item in resumo)
        {
            Console.WriteLine(
                $"{item.Vendedor,-24} " +
                $"{item.QuantidadeVendas,8} " +
                $"{item.TotalVendido,18:C} " +
                $"{item.TotalComissao,16:C}");
        }

        Console.WriteLine(new string('-', 72));
        Pausar();
    }

    private async Task ExecutarEstoqueAsync()
    {
        var caminho = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "estoque.json");

        var dados = await LerJsonAsync<EstoquePayload>(caminho);
        var service = new EstoqueService(dados.Estoque);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("MOVIMENTAÇÃO DE ESTOQUE");
            Console.WriteLine(new string('-', 65));
            Console.WriteLine(
                $"{"Código",-8} {"Produto",-38} {"Estoque",8}");

            foreach (var produto in service.Produtos)
            {
                Console.WriteLine(
                    $"{produto.CodigoProduto,-8} " +
                    $"{produto.DescricaoProduto,-38} " +
                    $"{produto.Estoque,8}");
            }

            Console.WriteLine(new string('-', 65));
            Console.WriteLine("1 - Nova movimentação");
            Console.WriteLine("2 - Ver movimentações realizadas");
            Console.WriteLine("0 - Voltar");
            Console.Write("Opção: ");

            var opcao = Console.ReadLine();

            if (opcao == "0")
                return;

            if (opcao == "2")
            {
                MostrarMovimentacoes(service.Movimentacoes);
                continue;
            }

            if (opcao != "1")
            {
                Console.WriteLine("Opção inválida.");
                Pausar();
                continue;
            }

            try
            {
                Console.Write("Código do produto: ");
                var codigo = LerInteiro();

                Console.Write("Tipo (1 = Entrada, 2 = Saída): ");
                var tipoNumero = LerInteiro();

                if (!Enum.IsDefined(typeof(TipoMovimentacao), tipoNumero))
                    throw new ArgumentException("Tipo de movimentação inválido.");

                Console.Write("Quantidade: ");
                var quantidade = LerInteiro();

                Console.Write("Descrição da movimentação: ");
                var descricao = Console.ReadLine() ?? string.Empty;

                var movimentacao = service.Movimentar(
                    codigo,
                    (TipoMovimentacao)tipoNumero,
                    quantidade,
                    descricao);

                Console.WriteLine();
                Console.WriteLine("Movimentação registrada com sucesso.");
                Console.WriteLine($"ID: {movimentacao.Id}");
                Console.WriteLine($"Produto: {movimentacao.Produto}");
                Console.WriteLine($"Tipo: {movimentacao.Tipo}");
                Console.WriteLine($"Quantidade: {movimentacao.Quantidade}");
                Console.WriteLine($"Estoque final: {movimentacao.EstoqueFinal}");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Não foi possível movimentar o estoque: {ex.Message}");
            }

            Pausar();
        }
    }

    private static void MostrarMovimentacoes(
        IReadOnlyList<MovimentacaoEstoque> movimentacoes)
    {
        Console.Clear();
        Console.WriteLine("MOVIMENTAÇÕES REALIZADAS");
        Console.WriteLine(new string('-', 90));

        if (movimentacoes.Count == 0)
        {
            Console.WriteLine("Nenhuma movimentação realizada nesta execução.");
        }
        else
        {
            foreach (var mov in movimentacoes)
            {
                Console.WriteLine($"ID: {mov.Id}");
                Console.WriteLine($"Data: {mov.DataHora:dd/MM/yyyy HH:mm:ss}");
                Console.WriteLine($"Produto: {mov.CodigoProduto} - {mov.Produto}");
                Console.WriteLine($"Tipo: {mov.Tipo}");
                Console.WriteLine($"Quantidade: {mov.Quantidade}");
                Console.WriteLine($"Descrição: {mov.Descricao}");
                Console.WriteLine($"Estoque final: {mov.EstoqueFinal}");
                Console.WriteLine(new string('-', 90));
            }
        }

        Pausar();
    }

    private static void ExecutarJuros()
    {
        Console.Clear();
        Console.WriteLine("CÁLCULO DE JUROS");
        Console.WriteLine();

        Console.Write("Valor original (ex.: 1000,00): R$ ");
        var valor = LerDecimal();

        Console.Write("Data de vencimento (dd/MM/yyyy): ");
        var dataVencimento = LerData();

        var hoje = DateOnly.FromDateTime(DateTime.Today);

        var service = new JurosService();
        var resultado = service.Calcular(
            valor,
            dataVencimento,
            hoje);

        Console.WriteLine();
        Console.WriteLine("RESULTADO");
        Console.WriteLine(new string('-', 45));
        Console.WriteLine($"Valor original: {resultado.ValorOriginal:C}");
        Console.WriteLine($"Vencimento: {resultado.DataVencimento:dd/MM/yyyy}");
        Console.WriteLine($"Data do cálculo: {resultado.DataCalculo:dd/MM/yyyy}");
        Console.WriteLine($"Dias de atraso: {resultado.DiasAtraso}");
        Console.WriteLine($"Taxa diária: {resultado.TaxaDiaria:P1}");
        Console.WriteLine($"Juros: {resultado.ValorJuros:C}");
        Console.WriteLine($"Total atualizado: {resultado.ValorTotal:C}");

        Pausar();
    }

    private async Task<T> LerJsonAsync<T>(string caminho)
    {
        if (!File.Exists(caminho))
            throw new FileNotFoundException(
                $"Arquivo não encontrado: {caminho}");

        var json = await File.ReadAllTextAsync(caminho);

        return JsonSerializer.Deserialize<T>(json, _jsonOptions)
            ?? throw new JsonException(
                "Não foi possível interpretar os dados do arquivo JSON.");
    }

    private static int LerInteiro()
    {
        var entrada = Console.ReadLine();

        if (!int.TryParse(entrada, out var valor))
            throw new FormatException("Informe um número inteiro válido.");

        return valor;
    }

    private static decimal LerDecimal()
    {
        var entrada = Console.ReadLine();

        if (!decimal.TryParse(
                entrada,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out var valor))
        {
            throw new FormatException(
                "Informe um valor monetário válido.");
        }

        return valor;
    }

    private static DateOnly LerData()
    {
        var entrada = Console.ReadLine();

        if (!DateOnly.TryParseExact(
                entrada,
                "dd/MM/yyyy",
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out var data))
        {
            throw new FormatException(
                "Informe a data no formato dd/MM/yyyy.");
        }

        return data;
    }

    private static void Pausar()
    {
        Console.WriteLine();
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadLine();
    }
}
