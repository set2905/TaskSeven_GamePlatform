using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TaskSeven_GamePlatform.Client;
using TaskSeven_GamePlatform.Client.Services;
using TaskSeven_GamePlatform.Client.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IGameSessionHub, GameSessionHub>();
builder.Services.AddScoped<ITicTacToeClientService, TicTacToeClientService>();
builder.Services.AddScoped<IPlayerClientService, PlayerClientService>();
builder.Services.AddScoped<TicTacToeSessionService>();

builder.Services.AddMudServices();


await builder.Build().RunAsync();