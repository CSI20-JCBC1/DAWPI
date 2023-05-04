using DAL.Models;
using SendGrid;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ISendGridClient>(x =>
    new SendGridClient(builder.Configuration.GetSection("SendGrid:ApiKey").Value));


// Registrar DatabasePiContext como servicio Scoped
builder.Services.AddScoped<DatabasePiContext>();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();

// Agrega un servicio de manejo de sesiones, con control de tiempo, necesario para el usesession.
builder.Services.AddSession(options =>
{
    //Linea control de tiempo de sesion, se puede omitir porque lo pongo también abajo
    //options.IdleTimeout = TimeSpan.FromMinutes(3);
});


// Agrega un servicio de autenticación por cookies.
builder.Services.AddAuthentication("AuthScheme").AddCookie("AuthScheme", options =>
{
    //Nombre del esquema
    options.Cookie.Name = "AuthScheme";
    //Donde se encuentra el login
    options.LoginPath = "/Login/Login";
    //La cookie caduca en 30 minutos
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    //Cuando el usuario tenga rol denegado
    options.AccessDeniedPath = "/Login/AccesoDenegado";
    //Al ejecutar el logout
    options.LogoutPath = "/Login/Logout";
});

var app = builder.Build();

// Si la aplicación se está ejecutando en un entorno diferente al de desarrollo, utiliza un middleware para manejar errores.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseRouting();

// Agrega middleware para habilitar el manejo de sesiones, nos servirá para el HttpContext.Session.
app.UseSession();

// Agrega middleware para autenticar solicitudes HTTP, es necesario para el login y el control de sesión.
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();