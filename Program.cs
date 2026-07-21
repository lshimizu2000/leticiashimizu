using HelpDeskMvc.Data;

var builder = WebApplication.CreateBuilder(args);

// Registra os serviços necessários para Controllers e Views.
builder.Services.AddControllersWithViews();

// Registra a implementação do Repository.
//
// Scoped significa que uma instância será criada para cada
// requisição HTTP recebida pela aplicação.
builder.Services.AddScoped<IChamadoRepository, ChamadoRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

// Define a rota padrão da aplicação.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chamados}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();