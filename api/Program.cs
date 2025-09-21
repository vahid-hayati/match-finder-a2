using api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddRepositoryService();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();