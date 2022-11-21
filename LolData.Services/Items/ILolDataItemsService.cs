namespace LolData.Services.Items
{
    /// <summary>
    /// League of Legends items service.
    /// </summary>
    public interface ILolDataItemsService
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><see cref="Item"/> if the specified item exists, otherwise - "unknown" item.</returns>
        Item GetItem(int id);

        /// <summary>
        /// Gets the item image.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Link to item image.</returns>
        string GetItemImage(Item item);
    }
}
