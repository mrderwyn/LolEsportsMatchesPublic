using LolData.Services.DataDragon.LocalStorage;
using LolData.Services.Runes;

namespace LolData.Services.DataDragon
{
    /// <summary>
    /// Provides access to the League of Legends runes information stored in the 
    /// <see cref="LocalStorageAccess"/> storage.
    /// </summary>
    /// <remarks>Runes data is updated using RIOT's official DDragon API.</remarks>
    /// <seealso cref="LolData.Services.Runes.ILolDataRunesService" />
    public class DdragonRunesService : ILolDataRunesService
    {
        private readonly string imgUrlFormat = @"https://ddragon.leagueoflegends.com/cdn/img/{0}";
        private readonly Dictionary<int, Rune> Runes;

        /// <summary>
        /// Initializes a new instance of the <see cref="DdragonRunesService"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public DdragonRunesService(LocalStorageData data)
        {
            this.Runes = data.Runes.ToDictionary(r => r.Id);
        }

        /// <summary>
        /// Gets the rune.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <see cref="T:LolData.Services.Runes.Rune" /> if the specified rune exists, otherwise - "unknown" rune.
        /// </returns>
        public Rune GetRune(int id)
        {
            return this.Runes.TryGetValue(id, out Rune? result)
                ? result
                : new Rune
                {
                    Id = 0,
                    Name = "unknown",
                    Icon = "",
                };
        }

        /// <summary>
        /// Gets the rune image.
        /// </summary>
        /// <param name="rune">The rune.</param>
        /// <returns>
        /// Link to rune image.
        /// </returns>
        public string GetRuneImage(Rune rune)
        {
            return (rune is not null && !string.IsNullOrWhiteSpace(rune.Icon))
                ? string.Format(this.imgUrlFormat, rune.Icon)
                : @"https://pngimg.com/uploads/question_mark/question_mark_PNG60.png";
        }
    }
}
