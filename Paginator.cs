using Microsoft.EntityFrameworkCore;

namespace CorePagination
{
    public interface IPaginator<T>
    {
        Task<PaginationResult<T>> PaginateAsync(
            IQueryable<T> query,
            int page,
            int pageSize,
            bool calculateTotal = true
        );
    }

    public class Paginator<T> : IPaginator<T> where T : class
    {
        public async Task<PaginationResult<T>> PaginateAsync(IQueryable<T> query, int page, int pageSize, bool calculateTotal = true)
        {
            Guard.NotNull(query, nameof(query));
            Guard.NotNegative(page, nameof(page));
            Guard.NotNegative(pageSize, nameof(pageSize));

            var result = new PaginationResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize
            };

            result.Items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            if (calculateTotal)
            {
                result.TotalCount = await query.CountAsync();
                result.TotalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize);
            }

            return result;
        }

    }

    public class PaginatorWithUrls<T> : Paginator<T> where T : class
    {
        private readonly string _baseUrl;

        public PaginatorWithUrls(string baseUrl)
        {
            Guard.NotNullOrWhiteSpace(baseUrl, nameof(baseUrl));
            _baseUrl = baseUrl;
        }

        public new async Task<PaginationResultWithUrls<T>> PaginateAsync(IQueryable<T> query, int page, int pageSize, bool calculateTotal = true)
        {
            Guard.NotNull(query, nameof(query));
            Guard.NotNegative(page, nameof(page));
            Guard.NotNegative(pageSize, nameof(pageSize));

            var baseResult = await base.PaginateAsync(query, page, pageSize, calculateTotal);

            var resultWithUrls = new PaginationResultWithUrls<T>
            {
                Items = baseResult.Items,
                CurrentPage = baseResult.CurrentPage,
                TotalPages = baseResult.TotalPages,
                PageSize = baseResult.PageSize,
                TotalCount = baseResult.TotalCount,
                NextPageUrl = page < baseResult.TotalPages ? $"{_baseUrl}?page={page + 1}&pageSize={pageSize}" : null,
                PreviousPageUrl = page > 1 ? $"{_baseUrl}?page={page - 1}&pageSize={pageSize}" : null,
                FirstPageUrl = $"{_baseUrl}?page=1&pageSize={pageSize}",
                LastPageUrl = $"{_baseUrl}?page={baseResult.TotalPages}&pageSize={pageSize}"
            };

            return resultWithUrls;
        }

    }

    public static class PaginationExtensions
    {
        public static async Task<PaginationResult<T>> PaginateAsync<T>(
            this IQueryable<T> query,
            int page,
            int pageSize,
            bool calculateTotal = true) where T : class
        {
            var paginator = new Paginator<T>();
            return await paginator.PaginateAsync(query, page, pageSize, calculateTotal);
        }

        public static IQueryable<T> PreparePagination<T>(
            this IQueryable<T> query,
            int page,
            int pageSize) where T : class
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static async Task<PaginationResultWithUrls<T>> PaginateUrlsAsync<T>(
           this IQueryable<T> query,
           int page,
           int pageSize,
           string baseUrl, bool calculateTotal) where T : class
        {
            var paginatorWithUrls = new PaginatorWithUrls<T>(baseUrl);
            return await paginatorWithUrls.PaginateAsync(query, page, pageSize, calculateTotal);
        }
    }

    public class PaginationResult<T>
    {
        public List<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }

    public class PaginationResultWithUrls<T> : PaginationResult<T>
    {
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
        public string FirstPageUrl { get; set; }
        public string LastPageUrl { get; set; }
    }


    public static class Guard
    {
        public static void NotNull<T>(T argument, string paramName) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(paramName, "Argument cannot be null.");
            }
        }

        public static void NotNegative(int value, string paramName)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, "Argument cannot be negative.");
            }
        }

        public static void NotNullOrWhiteSpace(string argument, string paramName)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", paramName);
            }
        }
    }

}
