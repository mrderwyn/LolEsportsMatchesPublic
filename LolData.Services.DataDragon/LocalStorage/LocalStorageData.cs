using LolData.Services.Champions;
using LolData.Services.Items;
using LolData.Services.Runes;

namespace LolData.Services.DataDragon.LocalStorage
{
    public class LocalStorageData
    {
        public string Version { get; set; } = null!;

        public List<Champion> Champions { get; set; } = null!;

        public List<Item> Items { get; set; } = new();

        public List<Rune> Runes { get; set; } = new();
    }
}
