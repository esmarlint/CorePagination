using CorePagination.Contracts;
using CorePagination.Paginators.Common;

namespace CorePagination.Tranformation.Contracts
{
    public abstract class UrlResultTransformerBase<T, TResult> : IPaginationTranformer<T, TResult> where T : class where TResult : UrlPaginationResult<T>, new()
    {
        protected readonly string _baseUrl;
        protected readonly Dictionary<string, string> _parametersToInclude = new Dictionary<string, string>();
        protected readonly Dictionary<string, string> _parameterRenames = new Dictionary<string, string>();

        protected UrlResultTransformerBase(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public UrlResultTransformerBase<T, TResult> IncludePage()
        {
            _parametersToInclude["page"] = "page";
            return this;
        }

        public UrlResultTransformerBase<T, TResult> IncludePageSize()
        {
            _parametersToInclude["pageSize"] = "pageSize";
            return this;
        }

        public UrlResultTransformerBase<T, TResult> RenameParameter(string originalName, string newName)
        {
            if (_parametersToInclude.ContainsKey(originalName))
            {
                _parameterRenames[originalName] = newName;
            }

            return this;
        }

        public UrlResultTransformerBase<T, TResult> AddParameter(string name, string value)
        {
            _parametersToInclude[name] = value;
            return this;
        }

        public abstract TResult Transform(IPaginationResult<T> paginationResult);
    }


}
