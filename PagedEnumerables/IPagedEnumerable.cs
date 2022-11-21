namespace PagedEnumerables
{
    /// <summary>
    /// Interface represents paged enumerable collection of elements.
    /// </summary>
    /// <typeparam name="T">type of elements in collection.</typeparam>
    public interface IPagedEnumerable<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets total count of elements on all pages of this collection.
        /// </summary>
        int TotalCount { get; }
    }
}
