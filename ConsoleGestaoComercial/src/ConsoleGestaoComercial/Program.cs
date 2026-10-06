using System.Globalization;
using ConsoleGestaoComercial.UI;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

var app = new ConsoleApp();
await app.ExecutarAsync();
