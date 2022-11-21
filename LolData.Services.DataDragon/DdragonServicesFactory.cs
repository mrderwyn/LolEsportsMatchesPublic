using LolData.Services.Champions;
using LolData.Services.Items;
using LolData.Services.Runes;
using Microsoft.Extensions.Configuration;

namespace LolData.Services.DataDragon
{
    /// <summary>
    /// Factory for <see cref="ILolDataChampionsService"/>, <see cref="ILolDataItemsService"/> and <see cref="ILolDataRunesService"/> services.
    /// Using <see cref="LocalStorage.LocalStorageData"/> from json repository.
    /// </summary>
    /// <remarks>Data is updated using RIOT's official DDragon API.</remarks>
    /// <seealso cref="LolData.Services.LolDataServicesFactory" />
    public class DdragonServicesFactory : LolDataServicesFactory
    {
        private readonly string localPath;
        private readonly LocalStorage.LocalStorageAccess dao;
        private ILolDataChampionsService champService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DdragonServicesFactory"/> class.
        /// </summary>
        /// <param name="config">
        /// The configuration (data storage path is taken from configuration section 
        /// <c>DataDragon -> LocalPath</c>. if not configured, the default path <c>lol_local_data.json</c> is used).
        /// </param>
        public DdragonServicesFactory(IConfiguration config)
        {
            this.localPath = config["DataDragon:LocalPath"] ?? "lol_local_data.json";

            this.dao = new LocalStorage.LocalStorageAccess(this.localPath, new DataDragon.DdragonApiAccess());

            this.champService = new DdragonChampionsService(this.dao.ReadData());
        }

        /// <summary>
        /// Gets the champions service.
        /// </summary>
        /// <returns>
        ///   <see cref="T:LolData.Services.Champions.ILolDataChampionsService" /> champions service.
        /// </returns>
        public override ILolDataChampionsService GetChampionsService() =>
            this.champService;

        /// <summary>
        /// Gets the items service.
        /// </summary>
        /// <returns>
        ///   <see cref="T:LolData.Services.Items.ILolDataItemsService" /> items service.
        /// </returns>
        public override ILolDataItemsService GetItemsService() =>
            new DdragonItemsService(this.dao.ReadData());

        /// <summary>
        /// Gets the runes service.
        /// </summary>
        /// <returns>
        ///   <see cref="T:LolData.Services.Runes.ILolDataRunesService" /> runes service.
        /// </returns>
        public override ILolDataRunesService GetRunesService() =>
            new DdragonRunesService(this.dao.ReadData());

        /// <summary>
        /// Updates services data.
        /// </summary>
        public override async Task Update()
        {
            await this.dao.Update();
            this.champService = new DdragonChampionsService(this.dao.ReadData());
        }
    }
}
