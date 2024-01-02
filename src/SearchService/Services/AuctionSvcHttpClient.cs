
using MongoDB.Entities;

namespace SearchService;

public class AuctionSvcHttpClient
{

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger _logger;
    public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config, ILogger logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item, string>()
        .Sort(x => x.Descending(x => x.UpdatedAt))
        .Project(x => x.UpdatedAt.ToString())
        .ExecuteFirstAsync();

        string requestUrl = _config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated;
        
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<Item>>(requestUrl);
            _logger.LogInformation(result.ToString());
            return result;
        }
        catch (System.Exception ex)
        {
            _logger.LogError("--> Hata var" + ex.Message.ToString());
        }

        return null;
    }

}