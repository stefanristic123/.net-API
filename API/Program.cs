using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// builder.Services.AddDbContext<DataContext>(options => { // Lambda izraz - options je parametar koji proslijedjujem bloku
//     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
// });

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();
 
// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// using var scope = app.Services.CreateScope();
// var services  = scope.ServiceProvider;
// try{
//     var context = services.GetRequiredService<DataContext>();
//     await context.Database.MigrateAsync();
//     await Seed.SeedUsers(context);
// }
// catch (Exception ex){
//     var logger = services.GetService<ILogger<Program>>();
//     logger.LogError(ex, "An error occured during migration");
// }

app.Run(); 