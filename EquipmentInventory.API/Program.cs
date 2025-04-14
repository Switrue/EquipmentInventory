using EquipmentInventory.API.Configurations;
using EquipmentInventory.API.Helpers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddSingleton<AuthorizationHelper>();

services.AddControllers();
services.AddHandlers();
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

app.UseExceptionHandlerService();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
