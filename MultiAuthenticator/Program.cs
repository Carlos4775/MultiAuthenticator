using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(config =>
{
    config.UseSqlServer(builder.Configuration.GetConnectionString("conn"), builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
});

string[] projects = new string[] { "BusinessLogic", "CrossCutting" };

foreach (string project in projects)
{
    List<Type> listImplementations = Assembly.Load(project).GetTypes().Where(B => B.Name.EndsWith("Service") && !B.IsInterface).ToList();

    foreach (Type implement in listImplementations)
    {
        Type? implentationInterface = implement.GetInterface("I" + implement.Name);

        if (implentationInterface != null)
        {
            builder.Services.AddScoped(implentationInterface, implement);
        }
        else
        {
            builder.Services.AddScoped(implement);
        }
    }
}

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option => 
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidAudience = builder.Configuration["Token:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            ClockSkew = TimeSpan.Zero
        };
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
    .AddFacebook(facebookOptions => 
    {
        facebookOptions.ClientId = builder.Configuration["Authentication:Facebook:ClientId"];
        facebookOptions.ClientSecret = builder.Configuration["Authentication:Facebook:ClientSecret"];
        facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
    });

builder.Services.AddCors();

builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(options => options
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
            );

app.MapControllers();

app.Run();
