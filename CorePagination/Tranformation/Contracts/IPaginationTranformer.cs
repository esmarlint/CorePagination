using CorePagination.Contracts;

namespace CorePagination.Tranformation.Contracts
{
    public interface IPaginationTranformer<T, TResult>
        where T : class
        where TResult : class
    {
        TResult Transform(IPaginationResult<T> paginationResult);
    }

}
