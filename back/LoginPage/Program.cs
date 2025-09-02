using System.Text;
using LoginPage.Context;
using LoginPage.Services.Login;
using LoginPage.Services.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
var jwtKey = builder.Configuration["AppSettings:Token"];
var jwtIssuer = builder.Configuration["AppSettings:Issuer"];
var jwtAudience = builder.Configuration["AppSettings:Audience"];

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ITasksService, TasksService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});


var app = builder.Build();

// Configure the HTTP request pipeline.




app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseRouting();
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate();
    Console.WriteLine("Database migrated successfully.");
}
catch (Exception ex)
{
    Console.WriteLine("Migration failed: " + ex.Message);
    throw;
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();