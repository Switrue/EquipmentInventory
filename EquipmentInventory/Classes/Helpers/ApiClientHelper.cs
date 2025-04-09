using EquipmentInventory.Classes.Helper;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace EquipmentInventory.Classes.Helpers;

public static class ApiClientHelper
{
    public static async Task<TResponse> PostAsync<TRequest, TResponse>(
    string endpoint,
    TRequest requestData,
    Action<string> onError = null)
    {
        try
        {
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await App.ApiClient.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }

            var errorJson = await response.Content.ReadAsStringAsync();
            var errorMessage = ParseError(errorJson);

            if (onError != null)
            {
                onError.Invoke(errorMessage);
            }
            else
            {
                DefaultErrorHandler(errorMessage);
            }

            return default;
        }
        catch (HttpRequestException ex)
        {
            HandleNetworkError(ex);
            return default;
        }
        catch (Exception ex)
        {
            HandleUnexpectedError(ex);
            return default;
        }
    }

    public static async Task<TResponse> GetAsync<TResponse>(
        string endpoint,
        Action<string> onError = null)
    {
        try
        {
            var response = await App.ApiClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }

            var errorJson = await response.Content.ReadAsStringAsync();
            var errorMessage = ParseError(errorJson);

            if (onError != null)
            {
                onError.Invoke(errorMessage);
            }
            else
            {
                DefaultErrorHandler(errorMessage);
            }

            return default;
        }
        catch (HttpRequestException ex)
        {
            HandleNetworkError(ex);
            return default;
        }
        catch (Exception ex)
        {
            HandleUnexpectedError(ex);
            return default;
        }
    }

    // Парсинг ошибок (универсальный)
    private static string ParseError(string errorJson)
    {
        try
        {
            var errorData = JObject.Parse(errorJson);

            // Стандартные ошибки валидации ASP.NET Core
            if (errorData["errors"] != null)
            {
                var errors = errorData["errors"]
                    .Children()
                    .SelectMany(x => x.Values())
                    .Select(x => x.ToString());

                return string.Join("\n", errors);
            }

            // Кастомные сообщения
            return errorData["message"]?.ToString()
                ?? errorData["error"]?.ToString()
                ?? errorJson;
        }
        catch
        {
            return errorJson;
        }
    }

    private static void DefaultErrorHandler(string message)
        => CustomMessageBoxHelper.Show("Ошибка", message);

    private static void HandleNetworkError(HttpRequestException ex)
        => CustomMessageBoxHelper.Show("Сеть", $"Ошибка подключения: {ex.Message}");

    private static void HandleUnexpectedError(Exception ex)
        => CustomMessageBoxHelper.Show("Ошибка", $"Непредвиденная ошибка: {ex.Message}");
}
