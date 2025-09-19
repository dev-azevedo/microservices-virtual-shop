using System.Text;
using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;
using VShop.Web.Utils;

namespace VShop.Web.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string _apiEndpoint = "/api/products/";
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private ProductViewModel _productVM;
    private IEnumerable<ProductViewModel> _productsVM;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    

    public async Task<IEnumerable<ProductViewModel>> GetAllProducts()
    {
        var client = _httpClientFactory.CreateClient(NameServices.ProductApi);
        using (var response = await client.GetAsync(_apiEndpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _productsVM = await JsonSerializer.DeserializeAsync<IEnumerable<ProductViewModel>>(apiResponse, _jsonSerializerOptions);
            }
            else
            {
                return null;
            }
        }
        
        return _productsVM;
    }

    public async Task<ProductViewModel> FindProductById(int id)
    {
        var client = _httpClientFactory.CreateClient(NameServices.ProductApi);
        using (var response = await client.GetAsync(_apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _productVM = await JsonSerializer
                    .DeserializeAsync<ProductViewModel>
                        (apiResponse, _jsonSerializerOptions);
            }
            else
            {
                return null;
            }
        }
        
        return _productVM;
    }

    public async Task<ProductViewModel> CreateProduct(ProductViewModel productVM)
    {
        var client = _httpClientFactory.CreateClient(NameServices.ProductApi);
        StringContent content = new StringContent(
            JsonSerializer.Serialize(productVM), 
            Encoding.UTF8, "application/json");
        
        using (var response = await client.PostAsync(_apiEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productVM = await JsonSerializer
                    .DeserializeAsync<ProductViewModel>
                        (apiResponse, _jsonSerializerOptions);
            }
            else
            {
                return null;
            }
        }
        
        return productVM;
    }

    public async Task<ProductViewModel> UpdateProduct(ProductViewModel productVM)
    {
        var client = _httpClientFactory.CreateClient(NameServices.ProductApi);
        ProductViewModel productUpdated = new ProductViewModel();
   
        using (var response = await client.PutAsJsonAsync(_apiEndpoint, productVM))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productUpdated = await JsonSerializer
                    .DeserializeAsync<ProductViewModel>
                        (apiResponse, _jsonSerializerOptions);
            }
            else
            {
                return null;
            }
        }
        
        return productUpdated;
    }

    public async Task<bool> DeleteProductById(int id)
    {
        var client = _httpClientFactory.CreateClient(NameServices.ProductApi);
        
        using (var response = await client.DeleteAsync(_apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        
        return false;
    }
}