using EquipmentInventory.Classes.Helper;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using EquipmentInventory.Properties;

namespace EquipmentInventory.Classes.Helpers;

public static class ApiClientHelper
{
    // Основные методы для запросов с телом
    public static Task<TResponse> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest requestData,
        Action<string> onError = null)
        => SendRequestAsync<TRequest, TResponse>(
            HttpMethod.Post,
            endpoint,
            requestData,
            onError
        );

    public static Task<TResponse> PatchAsync<TRequest, TResponse>(
        string endpoint,
        TRequest requestData,
        Action<string> onError = null)
        => SendRequestAsync<TRequest, TResponse>(
            new HttpMethod("PATCH"),
            endpoint,
            requestData,
            onError
        );

    // Основные методы для запросов без тела
    public static Task<TResponse> GetAsync<TResponse>(
        string endpoint,
        Action<string> onError = null)
        => SendRequestAsync<TResponse>(
            HttpMethod.Get,
            endpoint,
            onError
        );

    public static Task<TResponse> DeleteAsync<TResponse>(
        string endpoint,
        Action<string> onError = null)
        => SendRequestAsync<TResponse>(
            HttpMethod.Delete,
            endpoint,
            onError
        );

    // Общая логика для запросов c телом
    private static async Task<TResponse> SendRequestAsync<TRequest, TResponse>(
        HttpMethod method,
        string endpoint,
        TRequest requestData,
        Action<string> onError)
    {
        try
        {
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(method, endpoint)
            {
                Content = content
            };

            var response = await App.ApiClient.SendAsync(request);
            return await ProcessResponseAsync<TResponse>(response, onError);
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

    // Общая логика для запросов без тела
    private static async Task<TResponse> SendRequestAsync<TResponse>(
        HttpMethod method,
        string endpoint,
        Action<string> onError)
    {
        try
        {
            var response = await App.ApiClient.SendAsync(new HttpRequestMessage(method, endpoint));
            return await ProcessResponseAsync<TResponse>(response, onError);
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

    // Общая обработка ответа
    private static async Task<TResponse> ProcessResponseAsync<TResponse>(
        HttpResponseMessage response,
        Action<string> onError)
    {
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }

        var errorJson = await response.Content.ReadAsStringAsync();
        var errorMessage = ParseError(errorJson);

        if (onError != null) onError.Invoke(errorMessage);
        else DefaultErrorHandler(errorMessage);

        return default;
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
        => CustomMessageBoxHelper.Show(Strings.Error, message);

    private static void HandleNetworkError(HttpRequestException ex)
        => CustomMessageBoxHelper.Show(Strings.Network, $"{Strings.NetworkError}: {ex.Message}");

    private static void HandleUnexpectedError(Exception ex)
        => CustomMessageBoxHelper.Show(Strings.Error, $"{Strings.AnUnforeseenMistake}: {ex.Message}");
}
