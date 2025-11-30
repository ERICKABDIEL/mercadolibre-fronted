using frontendnet.Middlewares;
using frontendnet.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// === SESIÓN ===
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var UrlWebAPI = builder.Configuration["UrlWebAPI"];

// Delegating handlers
builder.Services.AddTransient<EnviaBearerDelegatingHandler>();
builder.Services.AddTransient<RefrescaTokenDelegatingHandler>();

// === HTTP CLIENTS ===
builder.Services.AddHttpClient<AuthClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebAPI!);
});

builder.Services.AddHttpClient<CategoriasClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebAPI!);
})
.AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
.AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();

builder.Services.AddHttpClient<UsuariosClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebAPI!);
})
.AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
.AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();

builder.Services.AddHttpClient<RolesClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebAPI!);
})
.AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
.AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();

builder.Services.AddHttpClient<ProductosClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebAPI!);
})
.AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
.AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();

builder.Services.AddHttpClient<PerfilClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebAPI!);
})
.AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
.AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();

builder.Services.AddHttpClient<ArchivosClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebAPI!);
})
.AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
.AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();

builder.Services.AddHttpClient<BitacoraClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebAPI!);
})
.AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
.AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();

// === AUTENTICACIÓN ===
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".frontendnet";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.LoginPath = "/Auth";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();

app.UseRouting();

// === activar sesión antes de auth ===
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// === rutas ===
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
