using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using TaskSeven_GamePlatform.Server.Domain;
using TaskSeven_GamePlatform.Server.Domain.Repo;
using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Server.Hubs;
using TaskSeven_GamePlatform.Server.Services;
using TaskSeven_GamePlatform.Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection, sqlServerOptionsAction:
    sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(maxRetryCount: 10,
        maxRetryDelay: TimeSpan.FromSeconds(30),
        errorNumbersToAdd: null);
    }));

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
       new[] { "application/octet-stream" });
});
builder.Services.AddTransient<IPlayerRepo, PlayerRepo>();
builder.Services.AddTransient<IGameStateRepo, GameStateRepo>();
builder.Services.AddTransient<IGameTypeRepo, GameTypeRepo>();
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<ITicTacToeService, TicTacToeService>();
builder.Services.AddTransient<IRockPaperScissorsService, RockPaperScissorsService>();


var app = builder.Build();
app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapHub<GameLifecycleHub>("/GameLifecycleHub");
app.MapFallbackToFile("index.html");

app.Run();