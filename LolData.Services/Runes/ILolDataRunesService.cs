namespace LolData.Services.Runes
{
    /// <summary>
    /// League of Legends runes service.
    /// </summary>
    public interface ILolDataRunesService
    {
        /// <summary>
        /// Gets the rune.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><see cref="Rune"/> if the specified rune exists, otherwise - "unknown" rune.</returns>
        Rune GetRune(int id);

        /// <summary>
        /// Gets the rune image.
        /// </summary>
        /// <param name="rune">The rune.</param>
        /// <returns>Link to rune image.</returns>
        string GetRuneImage(Rune rune);
    }
}
