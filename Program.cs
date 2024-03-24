using APIBanco.Services;
using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using APIBanco.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Jwt
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    byte[] key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]!);

    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RequireExpirationTime = false
    };
});

// Services
builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<IdentityDbContext>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<BankAccountService>();
builder.Services.AddScoped<TransactionsService>();
builder.Services.AddScoped<AdressService>();
builder.Services.AddScoped<LoanService>();
builder.Services.AddScoped<InsuranceService>();
builder.Services.AddScoped<CreditCardService>();
builder.Services.AddScoped<CardTransactionService>();


// DbContext , EntityFramework , Sqlite
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string? defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseLazyLoadingProxies().UseSqlite(defaultConnectionString);
});

// autoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "APIBanco",
        Description = "APIBanco",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "APIBanco",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "APIBanco",
            Url = new Uri("https://example.com/license")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authentication header using the Bearer scheme: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

});

// Cors Erros
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Cors erros
app.UseCors("CorsPolicy");
app.UseRouting();

app.UseHttpsRedirection();

// Jwt
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
