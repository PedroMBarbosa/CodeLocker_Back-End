using Api.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); //Adicionar essa linha

builder.Services.AddDbContext<DBCodeLockerContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
}); //Adicionar essa linha

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder => {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
}); //Adicionar essa linha

var app = builder.Build();

// Configure the HTTP request pipeline. //Adicionar IsProduction
if (app.Environment.IsDevelopment() || app.Environment.IsProduction()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(); //Adicionar essa linha

app.UseHttpsRedirection();
app.UseAuthorization(); //Adicionar essa linha
app.MapControllers(); //Adicionar essa linha

app.Run();