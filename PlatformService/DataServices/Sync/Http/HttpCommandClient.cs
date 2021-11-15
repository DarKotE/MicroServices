using System.Net.Mime;
using System.Text;
using System.Text.Json;
using PlatformService.Dto;

namespace PlatformService.DataServices.Sync.Http;

public sealed class HttpCommandClient : ICommandClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public HttpCommandClient(HttpClient httpClient, IConfiguration config)
        => (_httpClient, _config) = (httpClient, config);

    public async Task SendPlatform(PlatformReadDto platform)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platform),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
        var response = await _httpClient.PostAsync(_config["CommandService"], httpContent);
        Console.WriteLine(response.IsSuccessStatusCode
            ? "response.IsSuccessStatusCode"
            : "!response.IsSuccessStatusCode");
    }
}