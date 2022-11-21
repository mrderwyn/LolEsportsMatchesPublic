namespace PagedEnumerables
{
    /// <summary>
    /// Page settings class. Contain information about Offset and Count parameters.
    /// </summary>
    public class PageSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageSettings"/> class.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="elementsLimit">The elements limit on page.</param>
        public PageSettings(int pageNumber, int elementsLimit)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.Count = elementsLimit < 0 ? 0 : elementsLimit;
        }

        /// <summary>
        /// Gets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int PageNumber { get; }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public int Offset => (this.PageNumber - 1) * this.Count;

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; }
    }
}
