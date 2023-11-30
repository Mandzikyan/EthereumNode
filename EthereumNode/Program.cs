using EthereumNode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<EthereumNodeConfiguration>(builder.Configuration.GetSection("EthereumNodeConfiguration"));

var app = builder.Build();

// Add middleware to the pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// Add EthereumController to the controllers
app.MapControllers();

app.Run();
