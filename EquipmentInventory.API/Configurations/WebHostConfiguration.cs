namespace EquipmentInventory.API.Configurations;

public static class WebHostConfiguration
{
    public static IServiceCollection ConfigureWebHostUrls(
        this IServiceCollection services, 
        IWebHostBuilder webHostBuilder, 
        IConfiguration configuration)
    {
        try
        {
            var hostUrls = configuration["AppSettings:HostUrl"]?.Split(';', StringSplitOptions.RemoveEmptyEntries);

            if (hostUrls == null || hostUrls.Length == 0)
            {
                throw new ArgumentException("Host URLs are not configured in AppSettings:HostUrl");
            }

            var validUrls = new List<string>();

            foreach (var url in hostUrls)
            {
                if (Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                {
                    validUrls.Add(url);
                }
                else
                {
                    throw new UriFormatException($"Invalid URL format in configuration: '{url}'");
                }
            }

            webHostBuilder.UseUrls(validUrls.ToArray());
            return services;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to configure host URLs", ex);
        }
    }
}
