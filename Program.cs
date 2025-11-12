using Firma_tootajate_registreerimissusteem.Data;
using Firma_tootajate_registreerimissusteem.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors(options => options
    .WithOrigins("*")
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // ?????????, ???? ?? admin
    if (!db.Registers.Any(u => u.Email == "admin@gmail.com"))
    {
        var admin = new Register
        {
            Nimi = "Admin",
            Email = "admin@gmail.com",
            Parool = BCrypt.Net.BCrypt.HashPassword("admin123"),
            IsAdmin = true
        };

        db.Registers.Add(admin);
        db.SaveChanges();
    }
}

app.Run();