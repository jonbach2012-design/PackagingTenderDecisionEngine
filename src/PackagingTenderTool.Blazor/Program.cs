using MudBlazor.Services;
using PackagingTenderTool.Blazor;
using PackagingTenderTool.Blazor.Components;
using PackagingTenderTool.Core.Services.LabelTenderScoring;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddScoped<PackagingProfileSession>();
builder.Services.AddSingleton<ILabelTenderScoringStrategy, RelativeToBestScoringStrategy>();
builder.Services.AddSingleton<LabelTenderScoringService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

