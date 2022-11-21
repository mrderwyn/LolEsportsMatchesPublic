namespace LolEsportsMatchesApp.Models
{
    public class PagedModel<T>
    {
        public string CurrentId { get; set; } = null!;

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public bool HasPrevPage { get; set; }

        public bool HasNextPage { get; set; }

        public IEnumerable<T> Elements { get; set; } = null!;

        public static PagedModel<T> CreatePagedModel(IEnumerable<T> elements, string currentId, PagedEnumerables.PageSettings settings, decimal totalElements)
        {
            int lastPageNumber = (int)Math.Ceiling(totalElements / settings.Count);
            return new PagedModel<T>
            {
                Elements = elements,
                CurrentId = currentId,
                CurrentPage = settings.PageNumber,
                HasNextPage = settings.PageNumber < lastPageNumber,
                HasPrevPage = settings.PageNumber > 1,
                LastPage = lastPageNumber,
            };
        }
    }
}
