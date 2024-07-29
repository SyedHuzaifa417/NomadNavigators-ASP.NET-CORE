using Microsoft.EntityFrameworkCore;
using NomadNavigator_BE_.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<NNContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Data Source=syedhuzaifa\\sqlexpress;Initial Catalog=N_N;Integrated Security=True;Trust Server Certificate=True"));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

builder.Services.AddCors(options =>
{
    var frontendURL = configuration.GetValue<string>("frontend_url");


    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();


app.UseAuthorization();

app.MapControllers();

app.Run();

