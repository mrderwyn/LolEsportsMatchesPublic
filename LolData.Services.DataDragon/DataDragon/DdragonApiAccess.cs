using RestSharp;
using System.Text.Json;

namespace LolData.Services.DataDragon.DataDragon
{
    /// <summary>
    /// Provides access to RIOT's official API (https://ddragon.leagueoflegends.com/).
    /// </summary>
    internal class DdragonApiAccess
    {
        private readonly string path = @"https://ddragon.leagueoflegends.com/";
        private readonly string versionPath = @"api/versions.json";
        private readonly string champsPath = @"cdn/{0}/data/en_US/champion.json";
        private readonly string itemsPath = @"cdn/{0}/data/en_US/item.json";
        private readonly string runesPath = @"cdn/{0}/data/en_GB/runesReforged.json";

        /// <summary>
        /// Gets the last game version.
        /// </summary>
        /// <returns>Last game version if the versions API request is successful and parsed,
        /// otherwise - "unknown".</returns>
        internal async Task<string> GetLastVersion()
        {
            string[]? versions = await this.GetParsedResponse<string[]>(versionPath);
            return versions?.FirstOrDefault() ?? "unknown";
        }

        /// <summary>
        /// Gets all items for specified game version.
        /// </summary>
        /// <param name="version">The game version.</param>
        /// <returns><see cref="IEnumerable{Models.ItemJsonModel}"/> information about all items.</returns>
        internal async Task<IEnumerable<Models.ItemJsonModel>> GetItems(string version)
        {
            string? reqPath = string.Format(this.itemsPath, version);
            JsonElement response = await this.GetParsedResponse<JsonElement>(reqPath);

            JsonElement data = response
                .GetProperty("data");

            IEnumerable<Models.ItemJsonModel>? items = data
                .EnumerateObject()
                .Select(prop => new Models.ItemJsonModel
                {
                    Id = int.Parse(prop.Name),
                    Name = prop.Value.GetProperty("name").GetString() ?? "unknown",
                });

            return items;
        }

        /// <summary>
        /// Gets all champions for specified game version.
        /// </summary>
        /// <param name="version">The game version.</param>
        /// <returns><see cref="IEnumerable{Models.ChampionJsonModel?}"/> information about all champions.</returns>
        internal async Task<IEnumerable<Models.ChampionJsonModel?>> GetChampions(string version)
        {
            string? reqPath = string.Format(this.champsPath, version);
            JsonElement response = await this.GetParsedResponse<JsonElement>(reqPath);

            JsonElement data = response
                .GetProperty("data");

            IEnumerable<Models.ChampionJsonModel?>? champs = data
                .EnumerateObject()
                .Select(prop => prop.Value.Deserialize<Models.ChampionJsonModel>());

            return champs;
        }

        /// <summary>
        /// Gets all runes for specified game version.
        /// </summary>
        /// <param name="version">The game version.</param>
        /// <returns><see cref="IEnumerable{Models.RuneJsonModel}"/> information about all runes.</returns>
        internal async Task<IEnumerable<Models.RuneJsonModel?>> GetRunes(string version)
        {
            string? reqPath = string.Format(this.runesPath, version);
            JsonElement response = await this.GetParsedResponse<JsonElement>(reqPath);

            JsonElement.ArrayEnumerator outerRunes = response
                .EnumerateArray();

            IEnumerable<JsonElement>? innerRunes = outerRunes
                .SelectMany(r => r
                    .GetProperty("slots")
                    .EnumerateArray())
                .SelectMany(s => s
                    .GetProperty("runes")
                    .EnumerateArray());

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true,
            };

            return outerRunes
                .Concat(innerRunes)
                .Select(j => j.Deserialize<Models.RuneJsonModel>(options));
        }

        private async Task<T?> GetParsedResponse<T>(string requestPath)
        {
            RestResponse response = await this.GetResponse(requestPath);

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true,
            };

            if (response is null || !response.IsSuccessful)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(response.Content!, options);
        }

        private async Task<RestResponse> GetResponse(string requestPath)
        {
            RestClient client = new(path);
            RestRequest request = new(requestPath, Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            return response;
        }
    }
}
