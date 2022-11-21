using LolData.Services.Champions;
using LolData.Services.DataDragon.DataDragon;
using System.Text.Json;

namespace LolData.Services.DataDragon.LocalStorage
{
    /// <summary>
    /// Local JSON storage for <see cref="LocalStorageData"/>.
    /// </summary>
    internal class LocalStorageAccess
    {
        private readonly DdragonApiAccess api;
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalStorageAccess"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="api">RIOT's official API.</param>
        /// <exception cref="System.ArgumentNullException">path</exception>
        internal LocalStorageAccess(string path, DdragonApiAccess api)
        {
            this.path = string.IsNullOrWhiteSpace(path) ? throw new ArgumentNullException(nameof(path)) : path;
            this.api = api;
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <returns><see cref="LocalStorageData"/> data from storage.</returns>
        internal LocalStorageData ReadData()
        {
            using FileStream? stream = File.OpenRead(this.path);
            LocalStorageData data = JsonSerializer.Deserialize<LocalStorageData>(stream) ?? new();
            return data;
        }

        /// <summary>
        /// Updates data using RIOT's official API.
        /// </summary>
        /// <returns></returns>
        internal async Task Update()
        {
            LocalStorageData? data = new()
            {
                Version = await this.api.GetLastVersion()
            };

            List<Champion>? existedChamps = (this.ReadData()).Champions;
            short lastId = existedChamps.MaxBy(c => c.Index)?.Index ?? 0;

            IEnumerable<Champion>? champsToAdd = (await this.api.GetChampions(data.Version))
                .Distinct(null)
                .ExceptBy(existedChamps.Select(c => c.Id), model => model!.Id)
                .Select(c => new Champion { Index = ++lastId, Id = c!.Id, Name = c.Name });

            existedChamps.InsertRange(existedChamps.Count, champsToAdd);
            data.Champions = existedChamps;

            List<Items.Item>? items = (await this.api.GetItems(data.Version))
                .Select(i => new Items.Item { Id = i.Id, Name = i.Name })
                .ToList();
            data.Items = items;

            List<Runes.Rune>? runes = (await this.api.GetRunes(data.Version))
                .Distinct(null)
                .Select(r => new Runes.Rune { Id = r!.Id, Name = r.Name, Icon = r.Icon })
                .ToList();
            data.Runes = runes;

            await this.WriteLocalStorageData(data);
        }

        /// <summary>
        /// Creates <see cref="LocalStorageData"/>.
        /// </summary>
        /// <remarks>Use defaults champions list from path 12.19.1</remarks>
        /// <returns></returns>
        private async Task Create()
        {
            LocalStorageData? data = new()
            {
                Version = await this.api.GetLastVersion()
            };

            List<string> ChampsIds = new()
            {
                "Aatrox", "Ahri", "Akali", "Akshan", "Alistar", "Amumu", "Anivia", "Annie",
                "Aphelios", "Ashe", "AurelionSol", "Azir", "Bard", "Belveth", "Blitzcrank",
                "Brand", "Braum", "Caitlyn", "Camille", "Cassiopeia", "Chogath", "Corki",
                "Darius", "Diana", "DrMundo", "Draven", "Ekko", "Elise", "Evelynn", "Ezreal",
                "Fiddlesticks", "Fiora", "Fizz", "Galio", "Gangplank", "Garen", "Gnar",
                "Gragas", "Graves", "Gwen", "Hecarim", "Heimerdinger", "Illaoi", "Irelia",
                "Ivern", "Janna", "JarvanIV", "Jax", "Jayce", "Jhin", "Jinx", "Kaisa",  "Kalista",
                "Karma", "Karthus", "Kassadin", "Katarina", "Kayle", "Kayn", "Kennen", "Khazix",
                "Kindred", "Kled", "KogMaw", "Leblanc", "LeeSin", "Leona", "Lillia", "Lissandra",
                "Lucian", "Lulu", "Lux", "Malphite", "Malzahar", "Maokai", "MasterYi", "MissFortune",
                "Mordekaiser", "Morgana", "Nami", "Nasus", "Nautilus", "Neeko", "Nidalee", "Nilah",
                "Nocturne", "Nunu", "Olaf", "Orianna", "Ornn", "Pantheon", "Poppy", "Pyke", "Qiyana",
                "Quinn", "Rakan", "Rammus", "RekSai", "Rell", "Renata", "Renekton", "Rengar", "Riven",
                "Rumble", "Ryze", "Samira", "Sejuani", "Senna", "Seraphine", "Sett", "Shaco", "Shen",
                "Shyvana", "Singed", "Sion", "Sivir", "Skarner", "Sona", "Soraka", "Swain", "Sylas",
                "Syndra", "TahmKench", "Taliyah", "Talon", "Taric", "Teemo", "Thresh", "Tristana",
                "Trundle", "Tryndamere", "TwistedFate", "Twitch", "Udyr", "Urgot", "Varus", "Vayne",
                "Veigar", "Velkoz", "Vex", "Vi", "Zed", "Zeri", "Ziggs", "Zilean", "Zoe", "Zyra",
                "Viego", "Viktor", "Vladimir", "Volibear", "Warwick", "MonkeyKing", "Xayah",
                "Xerath", "XinZhao", "Yasuo", "Yone", "Yorick", "Yuumi", "Zac",
            };
            Dictionary<string, (int, string v)>? champsRealId = ChampsIds.Select((v, index) => (index + 1, v))
                .ToDictionary(p => p.v);

            IEnumerable<Champion> champsToAdd = (await this.api.GetChampions(data.Version))
                .Select(c => new Champion { Index = (short)champsRealId[c!.Id].Item1, Id = c.Id, Name = c.Name });

            data.Champions = champsToAdd.ToList();

            List<Items.Item> items = (await this.api.GetItems(data.Version))
                .Select(i => new Items.Item { Id = i.Id, Name = i.Name })
                .ToList();
            data.Items = items;

            List<Runes.Rune> runes = (await this.api.GetRunes(data.Version))
                .Distinct(null)
                .Select(r => new Runes.Rune { Id = r!.Id, Name = r.Name, Icon = r.Icon })
                .ToList();
            data.Runes = runes;

            await this.WriteLocalStorageData(data);
        }

        /// <summary>
        /// Writes the local storage data to json file.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private async Task WriteLocalStorageData(LocalStorageData data)
        {
            using FileStream? stream = File.Create(this.path);
            using StreamWriter? writer = new(stream);

            await writer.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
