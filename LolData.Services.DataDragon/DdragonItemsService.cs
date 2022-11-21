using LolData.Services.DataDragon.LocalStorage;
using LolData.Services.Items;

namespace LolData.Services.DataDragon
{
    /// <summary>
    /// Provides access to the League of Legends items information stored in the 
    /// <see cref="LocalStorageAccess"/> storage.
    /// </summary>
    /// <remarks>Items data is updated using RIOT's official DDragon API.</remarks>
    /// <seealso cref="LolData.Services.Items.ILolDataItemsService" />
    public class DdragonItemsService : ILolDataItemsService
    {
        private readonly SortedDictionary<int, Item> Items;
        private readonly string version;
        private readonly string imgUrlFormat = @"http://ddragon.leagueoflegends.com/cdn/{0}/img/item/{1}.png";

        /// <summary>
        /// Initializes a new instance of the <see cref="DdragonItemsService"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public DdragonItemsService(LocalStorageData data)
        {
            this.version = data.Version;
            this.Items = new(data.Items.ToDictionary(i => i.Id));
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <see cref="T:LolData.Services.Items.Item" /> if the specified item exists, otherwise - "unknown" item.
        /// </returns>
        public Item GetItem(int id)
        {
            return this.Items.TryGetValue(id, out Item? item)
                ? item
                : new Item
                {
                    Id = id,
                    Name = "unknown",
                };
        }

        /// <summary>
        /// Gets the item image.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// Link to item image.
        /// </returns>
        public string GetItemImage(Item item)
        {
            return item.Name != "unknown"
                ? string.Format(this.imgUrlFormat, this.version, item.Id)
                : "empty";
        }
    }
}
