using LolData.Services.Champions;
using LolData.Services.DataDragon.LocalStorage;

namespace LolData.Services.DataDragon
{
    /// <summary>
    /// Provides access to the League of Legends champions information stored in the 
    /// <see cref="LocalStorageAccess"/> storage.
    /// </summary>
    /// <remarks>Champions data is updated using RIOT's official DDragon API.</remarks>
    /// <seealso cref="LolData.Services.Champions.ILolDataChampionsService" />
    public class DdragonChampionsService : ILolDataChampionsService
    {
        private readonly SortedDictionary<short, Champion> Champions;
        private readonly string version;
        private readonly string imgUrlFormat = @"http://ddragon.leagueoflegends.com/cdn/{0}/img/champion/{1}.png";

        /// <summary>
        /// Initializes a new instance of the <see cref="DdragonChampionsService"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public DdragonChampionsService(LocalStorageData data)
        {
            this.version = data.Version;
            this.Champions = new(data.Champions.ToDictionary(c => c.Index));
        }

        /// <summary>
        /// Gets the champion by index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <see cref="T:LolData.Services.Champions.Champion" /> if the specified champion exists, otherwise - "unknown" champion.
        /// </returns>
        public Champion GetChampion(short index)
        {
            return this.Champions.TryGetValue(index, out var result)
                ? result
                : new Champion
                {
                    Id = "unknown",
                    Name = "unknown",
                    Index = index,
                };
        }

        /// <summary>
        /// Gets the champion by champion id.
        /// </summary>
        /// <param name="champId">The champ identifier.</param>
        /// <returns>
        ///   <see cref="T:LolData.Services.Champions.Champion" /> if the specified champion exists, otherwise - "unknown" champion.
        /// </returns>
        public Champion GetChampion(string champId)
        {
            return this.Champions.FirstOrDefault(c => c.Value.Id.Equals(champId, StringComparison.OrdinalIgnoreCase)).Value
                ?? new Champion
                {
                    Id = "unknown",
                    Name = "unknown",
                    Index = 0,
                };
        }

        /// <summary>
        /// Gets the champion image.
        /// </summary>
        /// <param name="champId">The champ identifier.</param>
        /// <returns>
        /// Link to champion image.
        /// </returns>
        public string GetChampionImage(string champId)
        {
            return champId != "unknwon"
                ? string.Format(this.imgUrlFormat, this.version, champId)
                : @"https://pngimg.com/uploads/question_mark/question_mark_PNG60.png";
        }

        /// <summary>
        /// Gets all champions.
        /// </summary>
        /// <returns>
        ///   <see cref="IList{Champion}" /> with all champions.
        /// </returns>
        public IList<Champion> GetChampions()
        {
            return this.Champions.Select(p => p.Value).ToList();
        }
    }
}
