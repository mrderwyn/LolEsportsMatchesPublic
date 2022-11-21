namespace LolData.Services.Champions
{
    /// <summary>
    /// Champions service. Provides indexing and champion information.
    /// </summary>
    public interface ILolDataChampionsService
    {
        /// <summary>
        /// Gets all champions.
        /// </summary>
        /// <returns><see cref="IList{Champion}"/> with all champions.</returns>
        IList<Champion> GetChampions();

        /// <summary>
        /// Gets the champion by index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns><see cref="Champion"/> if the specified champion exists, otherwise - "unknown" champion.</returns>
        Champion GetChampion(short index);

        /// <summary>
        /// Gets the champion by champion id.
        /// </summary>
        /// <param name="champId">The champ identifier.</param>
        /// <returns><see cref="Champion"/> if the specified champion exists, otherwise - "unknown" champion.</returns>
        Champion GetChampion(string champId);

        /// <summary>
        /// Gets the champion image.
        /// </summary>
        /// <param name="champId">The champ identifier.</param>
        /// <returns>Link to champion image.</returns>
        string GetChampionImage(string champId);
    }
}
