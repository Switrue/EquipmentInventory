using EquipmentInventory.API.Configurations;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddControllers();
services.AddDependencies();
services.AddEndpointsApiExplorer();
services.AddSwaggerServices();

services.AddDbContextConfiguration(configuration);

services.AddAuthenticationConfiguration(configuration);
services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomStatusCodeHandling();
app.UseExceptionHandlerService();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
