# Console de Gestão Comercial

Aplicação console em **C# / .NET 10** para gestão de operações comerciais, reunindo três módulos principais: **comissões de vendas**, **movimentação de estoque** e **cálculo de juros por atraso**.

O projeto foi desenvolvido inicialmente a partir de um desafio técnico e depois estruturado como projeto de portfólio, com separação de responsabilidades, leitura de arquivos JSON e testes unitários.

## Funcionalidades

### Comissões de vendas

A aplicação lê os registros de `vendas.json`, calcula a comissão de cada venda e apresenta:

- vendedor;
- valor da venda;
- percentual aplicado;
- valor da comissão;
- resumo consolidado por vendedor.

Regras aplicadas:

- abaixo de R$ 100,00: 0%;
- de R$ 100,00 até R$ 499,99: 1%;
- a partir de R$ 500,00: 5%.

### Movimentação de estoque

Permite registrar entradas e saídas de produtos.

Cada movimentação contém:

- identificador único (`Guid`);
- data e hora;
- produto;
- tipo da movimentação;
- quantidade;
- descrição;
- estoque final.

O sistema também impede saídas superiores ao estoque disponível e mantém as alterações enquanto a aplicação estiver em execução.

### Cálculo de juros

Recebe um valor e uma data de vencimento e calcula o valor devido na data atual.

Foi adotado **juro simples de 2,5% ao dia**:

```text
juros = valor × 0,025 × dias de atraso
```

Se a data de vencimento ainda não passou, o valor dos juros é zero.

## Tecnologias

- C#
- .NET 10
- LINQ
- System.Text.Json
- xUnit

## Estrutura

```text
ConsoleGestaoComercial/
├── global.json
├── ConsoleGestaoComercial.sln
├── README.md
├── src/
│   └── ConsoleGestaoComercial/
│       ├── Data/
│       ├── Models/
│       ├── Services/
│       ├── UI/
│       ├── Program.cs
│       └── ConsoleGestaoComercial.csproj
└── tests/
    └── ConsoleGestaoComercial.Tests/
        └── ConsoleGestaoComercial.Tests.csproj
```

## Como executar

Confira primeiro a versão do SDK:

```bash
dotnet --version
```

O projeto está configurado para:

```text
10.0.401
```

Depois execute:

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/ConsoleGestaoComercial
```

O menu principal será exibido assim:

```text
========================================
          CONSOLE DE GESTÃO COMERCIAL
========================================
1 - Comissão de vendedores
2 - Movimentação de estoque
3 - Cálculo de juros
0 - Sair
```

## Testes

Os testes cobrem:

- limites das faixas de comissão;
- cálculo do percentual por venda;
- agrupamento por vendedor;
- entrada e saída de estoque;
- bloqueio de estoque insuficiente;
- cálculo de juros em atraso;
- ausência de juros antes do vencimento.

Execute:

```bash
dotnet test
```

## Decisões técnicas

### `decimal` para valores monetários

Foi utilizado `decimal` para reduzir problemas de precisão em cálculos financeiros.

### Regras de negócio separadas da interface

As regras ficam em `Services`, enquanto a interação com o terminal fica em `UI`. Isso facilita manutenção e testes.

### JSON separado do código

Os dados de vendas e estoque ficam em arquivos na pasta `Data`, evitando que os registros precisem ser escritos diretamente no código.

### SDK fixado

O `global.json` fixa o projeto no SDK .NET 10 estável, evitando uso acidental de versões preview instaladas na máquina.

## Autora

Projeto desenvolvido para estudo, portfólio e demonstração de conhecimentos em C# e .NET.
