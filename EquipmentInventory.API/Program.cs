using EquipmentInventory.API.Configurations;
using EquipmentInventory.API.Data.Enums;
using EquipmentInventory.API.Services;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;
    var services = builder.Services;

    try
    {
        services.ConfigureWebHostUrls(builder.WebHost, configuration);

        services.AddControllers();
        services.AddDependencies();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerServices();

        services.AddDbContextConfiguration(configuration);

        services.AddAuthenticationConfiguration(configuration);
        services.AddAuthorization();
    }
    catch (Exception configError)
    {
        ErrorDisplayService.ShowError(configError, ErrorType.Configuration);
        ErrorDisplayService.LogErrorToFile(configError);
        return;
    }

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
}
catch (Exception ex)
{
    ErrorDisplayService.ShowError(ex, ErrorType.Startup);
    ErrorDisplayService.LogErrorToFile(ex);
    return;
}

