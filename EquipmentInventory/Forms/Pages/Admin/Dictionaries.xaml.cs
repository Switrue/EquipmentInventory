using EquipmentInventory.Classes.API.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Admin;

/// <summary>
/// Логика взаимодействия для Dictionaries.xaml
/// </summary>
public partial class Dictionaries : UserControl
{
    private readonly HttpClient _httpClient = new HttpClient();
    private const string ApiUrl = "https://localhost:7217/api/Auth/login";
    private string jwt;

    public Dictionaries()
    {
        InitializeComponent();
    }

    private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        try
        {
            jwt = await GetJwtToken("Иван", "string");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt);
            MessageBox.Show($"Успешная авторизация! Токен: {jwt}");

            var user = await GetUserById(1);
            var message = FormatUserMessage(user);
            MessageBox.Show(message, "Информация о пользователе");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
    }

    private async Task<string> GetJwtToken(string username, string password)
    {
        var authData = new AuthRequest
        {
            Username = username,
            Password = password
        };

        var json = JsonConvert.SerializeObject(authData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(ApiUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Ошибка: {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        dynamic responseData = JsonConvert.DeserializeObject(responseContent);

        return responseData.token;
    }

    private async Task<User> GetUserById(long userId)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7217/api/Users/{userId}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<User>(json);
    }

    private string FormatUserMessage(User user)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Имя: {user.Username ?? "Не указано"}");
        sb.AppendLine($"ID: {user.Id}");
        sb.AppendLine($"Описание: {user.Description ?? "Нет описания"}");
        sb.AppendLine($"Роль: {user.Role ?? "Не назначена"}");
        return sb.ToString();
    }
}
