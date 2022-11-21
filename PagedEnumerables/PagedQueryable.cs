using System.Collections;

namespace PagedEnumerables
{
    /// <summary>
    /// Provide access to specified page of query.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="PagedEnumerables.IPagedEnumerable&lt;T&gt;" />
    public class PagedQueryable<T> : IPagedEnumerable<T>
    {
        private readonly IQueryable<T> query;
        private readonly int offset;
        private readonly int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedQueryable{T}"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        public PagedQueryable(IQueryable<T> query, int offset, int count)
        {
            this.query = query;
            this.offset = Math.Max(offset, 0);
            this.count = Math.Max(count, 0);
        }

        /// <summary>
        /// Return empty instance of <see cref="PagedQueryable{T}"/> class.
        /// </summary>
        /// <returns>Empty instance of <see cref="PagedQueryable{T}"/> class</returns>
        public static PagedQueryable<T> Empty()
        {
            return new PagedQueryable<T>(Array.Empty<T>().AsQueryable(), 0, 0);
        }

        /// <summary>
        /// Return instance of <see cref="PagedQueryable{T}"/> class with single element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Iinstance of <see cref="PagedQueryable{T}"/> class with single element.</returns>
        public static PagedQueryable<T> Single(T value)
        {
            return new PagedQueryable<T>((new[] { value }).AsQueryable(), 0, 1);
        }

        /// <summary>
        /// Gets total count of elements on all pages of this collection.
        /// </summary>
        public int TotalCount
        {
            get
            {
                return query is null
                    ? 0
                    : query.Count();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through specified page of this collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through specified page of this collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return
                query
                .Skip(this.offset)
                .Take(this.count)
                .ToList()
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
