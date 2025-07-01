using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Server.Bll;
using Server.Bll.Interfaces;
using Server.Bll.Services;
using Server.Dal;
using Server.Dal.Interfaces;
using Server.Server.Profiles;
using System;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

// DAL & BLL registrations
builder.Services.AddScoped<IGiftDal, GiftDal>();
builder.Services.AddScoped<IGiftService, GiftService>();
builder.Services.AddScoped<ICategoryDal, CategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDonorDal, DonorDal>();
builder.Services.AddScoped<IDonorService, DonorService>();
builder.Services.AddScoped<IUserDal, UserDal>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketDal, TicketDal>();
builder.Services.AddScoped<ITicketService, TicketSevice>();

// Controllers + JSON options
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token.\nExample: 'Bearer eyJhbGciOi...'"
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

// DB context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("1858e87833b36da4ea93df26fb950af27b7a2d1cdddda825eeb443ceeae1fde11cad965006c3c7ef3e927c611e4686981ef08be19a5d38d63b8985542e8893b6")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Development tools
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // מאפשר גישה לתמונות ולכל קובץ סטטי מתוך wwwroot
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
