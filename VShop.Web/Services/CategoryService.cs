using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;
using VShop.Web.Utils;

namespace VShop.Web.Services;

public class CategoryService : ICategoryService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private const string _apiEndpoint = "/api/categories/";

    public CategoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
    }

    public async Task<IEnumerable<CategoryViewModel>> GetAllCategories()
    {
        var client = _httpClientFactory.CreateClient(NameServices.ProductApi);

        IEnumerable<CategoryViewModel> categories;

        var response = await client.GetAsync(_apiEndpoint);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            categories = await JsonSerializer
                .DeserializeAsync<IEnumerable<CategoryViewModel>>(apiResponse, _jsonSerializerOptions);
        }
        else
        {
            return null;
        }

        return categories;
    }
}