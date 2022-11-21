using LolData.Services.Champions;
using LolData.Services.Items;
using LolData.Services.Runes;

namespace LolData.Services
{
    /// <summary>
    /// Abstract factory for <see cref="ILolDataChampionsService"/>, <see cref="ILolDataItemsService"/> 
    /// and <see cref="ILolDataRunesService"/>.
    /// </summary>
    /// <remarks>Also contains a method for updating services data.</remarks>
    public abstract class LolDataServicesFactory
    {
        /// <summary>
        /// Gets the champions service.
        /// </summary>
        /// <returns><see cref="ILolDataChampionsService"/> champions service.</returns>
        public abstract ILolDataChampionsService GetChampionsService();

        /// <summary>
        /// Gets the items service.
        /// </summary>
        /// <returns><see cref="ILolDataItemsService"/> items service.</returns>
        public abstract ILolDataItemsService GetItemsService();

        /// <summary>
        /// Gets the runes service.
        /// </summary>
        /// <returns><see cref="ILolDataRunesService"/> runes service.</returns>
        public abstract ILolDataRunesService GetRunesService();

        /// <summary>
        /// Updates services data.
        /// </summary>
        /// <returns></returns>
        public abstract Task Update();
    }
}
