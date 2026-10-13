using BlazorDbzPersonagem.Components;
using BlazorDbzPersonagem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var baseUrl = builder.Configuration["DragonBallApi:BaseUrl"]
    ?? throw new InvalidOperationException("Configuração 'DragonBallApi:BaseUrl' não encontrada.");

// HttpClient tipado via IHttpClientFactory: evita esgotamento de sockets de instanciar HttpClient manualmente.
// A chamada parte do servidor (Interactive Server), então não há problema de CORS com a API externa.
builder.Services.AddHttpClient<DragonBallApiService>(client =>
{
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
