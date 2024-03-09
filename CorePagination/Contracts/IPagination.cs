namespace CorePagination.Contracts
{
    public interface IPagination<T, TParameters, TResult>
    {
        /// <summary>
        /// Paginates the provided IQueryable based on the given pagination parameters.
        /// </summary>
        /// <param name="query">The IQueryable to be paginated.</param>
        /// <param name="parameters">The parameters for pagination.</param>
        /// <returns>The pagination result.</returns>
        TResult Paginate(IQueryable<T> query, TParameters parameters);
    }
}
