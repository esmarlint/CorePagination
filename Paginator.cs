
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

public interface IPagination<T, TParameters, TResult>
    where TParameters : PaginationParameters
    where TResult : class
{
    Task<TResult> PaginateAsync(IQueryable<T> query, TParameters parameters);
}

public class PaginationParameters
{
    public int PageSize { get; set; } = 10;
}

public class PageNumberPaginationParameters : PaginationParameters
{
    public int PageNumber { get; set; } = 1;
}

public enum PaginationOrder
{
    Ascending,
    Descending
}

public class CursorPaginationParameters<TKey> : PaginationParameters
{
    public TKey CurrentCursor { get; set; }
    public PaginationOrder Order { get; set; } = PaginationOrder.Ascending;
}

public interface IPaginationResult<T>
{
    IEnumerable<T> Items { get; set; }
    int PageSize { get; set; }
}

public abstract class PaginationResult<T> : IPaginationResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int PageSize { get; set; }
}

public class SimplePaginationResult<T> : PaginationResult<T>
{
    public int CurrentPage { get; set; }
}

public class TotalPaginationResult<T> : SimplePaginationResult<T>
{
    public int TotalItems { get; set; }
}

public class CursorPaginationResult<T, TKey> : PaginationResult<T>
{
    public TKey CurrentCursor { get; set; }
    public TKey NextCursor { get; set; }

    public bool HasMore { get; set; } 

}

public class SimplePaginator<T> : IPagination<T, PageNumberPaginationParameters, SimplePaginationResult<T>>
{
    public async Task<SimplePaginationResult<T>> PaginateAsync(IQueryable<T> query, PageNumberPaginationParameters parameters)
    {
        var items = await query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync();
        return new SimplePaginationResult<T>
        {
            Items = items,
            PageSize = parameters.PageSize,
            CurrentPage = parameters.PageNumber
        };
    }

    public async Task<SimplePaginationResult<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize)
    {
        var parameters = new PageNumberPaginationParameters { PageNumber = pageNumber, PageSize = pageSize };
        return await PaginateAsync(query, parameters);
    }
}

public class TotalPaginator<T> : IPagination<T, PageNumberPaginationParameters, TotalPaginationResult<T>>
{
    public async Task<TotalPaginationResult<T>> PaginateAsync(IQueryable<T> query, PageNumberPaginationParameters parameters)
    {
        var totalItems = await query.CountAsync();
        var items = await query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync();
        return new TotalPaginationResult<T>
        {
            Items = items,
            PageSize = parameters.PageSize,
            CurrentPage = parameters.PageNumber,
            TotalItems = totalItems
        };
    }

    public async Task<TotalPaginationResult<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize)
    {
        var parameters = new PageNumberPaginationParameters { PageNumber = pageNumber, PageSize = pageSize };
        return await PaginateAsync(query, parameters);
    }
}

public class CursorPaginator<T, TKey> : IPagination<T, CursorPaginationParameters<TKey>, CursorPaginationResult<T, TKey>>
    where T : class
    where TKey : IComparable
{
    private readonly Expression<Func<T, TKey>> _keySelector;

    public CursorPaginator(Expression<Func<T, TKey>> keySelector)
    {
        _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
    }

    public async Task<CursorPaginationResult<T, TKey>> PaginateAsync(IQueryable<T> query, CursorPaginationParameters<TKey> parameters)
    {
        IQueryable<T> orderedQuery = parameters.Order == PaginationOrder.Ascending
            ? query.OrderBy(_keySelector)
            : query.OrderByDescending(_keySelector);

        if (parameters.CurrentCursor != null)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Invoke(_keySelector, parameter);
            var cursorValue = Expression.Constant(parameters.CurrentCursor, typeof(TKey));
            var comparison = parameters.Order == PaginationOrder.Ascending
                ? Expression.GreaterThan(property, cursorValue)
                : Expression.LessThan(property, cursorValue);
            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);

            orderedQuery = orderedQuery.Where(lambda);
        }

        var itemsWithPossibleNext = await orderedQuery.Take(parameters.PageSize + 1).ToListAsync();
        var hasMore = itemsWithPossibleNext.Count > parameters.PageSize;
        var items = itemsWithPossibleNext.Take(parameters.PageSize).ToList();

        TKey nextCursor = default(TKey);
        if (hasMore)
        {
            var propInfo = typeof(T).GetProperty(((_keySelector.Body as MemberExpression)?.Member as PropertyInfo)?.Name);
            var lastItem = items.LastOrDefault();
            if (lastItem != null && propInfo != null)
            {
                nextCursor = (TKey)propInfo.GetValue(lastItem);
            }
        }

        return new CursorPaginationResult<T, TKey>
        {
            Items = items,
            PageSize = parameters.PageSize,
            CurrentCursor = parameters.CurrentCursor,
            NextCursor = nextCursor,
            HasMore = hasMore
        };
    }
}

public static class PaginationResultExtensions
{
    public static TResult TransformTo<T, TResult>(
        this IPaginationResult<T> paginationResult,
        Func<IPaginationResult<T>, TResult> transformation) where TResult : class
    {
        return transformation(paginationResult);
    }

    public static TResult TransformToWithItems<T, TResult>(
        this IPaginationResult<T> paginationResult,
        Func<IPaginationResult<T>, TResult> transformation) where TResult : IPaginationResult<T>, new()
    {
        var result = transformation(paginationResult);
        if (result is IPaginationResult<T> paginationResultWithItems)
        {
            paginationResultWithItems.Items = paginationResult.Items;
        }
        return result;
    }
}

public interface IPaginationResultTransformer<T, TResult>
    where T : class
    where TResult : class
{
    TResult Transform(IPaginationResult<T> paginationResult);
}

public class SimpleUrlPaginationResultTransformer<T> : IPaginationResultTransformer<T, UrlPaginationResult<T>> where T : class
{
    private readonly string _baseUrl;

    public SimpleUrlPaginationResultTransformer(string baseUrl)
    {
        _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
    }

    public UrlPaginationResult<T> Transform(IPaginationResult<T> paginationResult)
    {
        var currentPage = paginationResult.CurrentPage;
        var hasNextPage = true; 
        var hasPrevPage = currentPage > 1;

        return new UrlPaginationResult<T>
        {
            Items = paginationResult.Items,
            PageSize = paginationResult.PageSize,
            NextUrl = hasNextPage ? $"{_baseUrl}?page={currentPage + 1}" : null,
            PreviusUrl = hasPrevPage ? $"{_baseUrl}?page={currentPage - 1}" : null,
            CurrentUrl = $"{_baseUrl}?page={currentPage}"
        };
    }
}

public static class PaginatorExtensions
{
    public static async Task<SimplePaginationResult<T>> SimplePaginateAsync<T>(
        this IQueryable<T> query, int pageSize, int pageNumber = 1)
    {
        var paginator = new SimplePaginator<T>();
        var parameters = new PageNumberPaginationParameters { PageSize = pageSize, PageNumber = pageNumber };
        return await paginator.PaginateAsync(query, parameters);
    }

    public static async Task<SizeAwarePaginationResult<T>> PaginateAsync<T>(
        this IQueryable<T> query, int pageSize, int pageNumber = 1)
    {
        var paginator = new SizeAwarePaginator<T>();
        var parameters = new PageNumberPaginationParameters { PageSize = pageSize, PageNumber = pageNumber };
        return await paginator.PaginateAsync(query, parameters);
    }

    public static async Task<CursorPaginationResult<T, TKey>> CursorPaginateAsync<T, TKey>(
        this IQueryable<T> query, Expression<Func<T, TKey>> keySelector, int pageSize, TKey currentCursor = default, PaginationOrder order = PaginationOrder.Ascending)
        where T : class
        where TKey : IComparable
    {
        var paginator = new CursorPaginator<T, TKey>(keySelector);
        var parameters = new CursorPaginationParameters<TKey> { PageSize = pageSize, CurrentCursor = currentCursor, Order = order };
        return await paginator.PaginateAsync(query, parameters);
    }
}