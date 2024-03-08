using CorePagination.Contracts;

namespace CorePagination.Tranformation.Contracts
{
    /// <summary>
    /// Defines the contract for transforming a pagination result into a different format.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the original pagination result.</typeparam>
    /// <typeparam name="TResult">The type of the transformed pagination result.</typeparam>
    public interface IPaginationTranformer<T, TResult>
        where T : class
        where TResult : class
    {
        /// <summary>
        /// Transforms the specified pagination result into a different format.
        /// </summary>
        /// <param name="paginationResult">The pagination result to be transformed.</param>
        /// <returns>The transformed pagination result.</returns>
        TResult Transform(IPaginationResult<T> paginationResult);
    }

}
