using CorePagination.Contracts;
using CorePagination.Paginators.Common;

namespace CorePagination.Tranformation.Contracts
{
    /// <summary>
    /// Provides a base class for transformers that convert pagination results into URL-enhanced results.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the original pagination result.</typeparam>
    /// <typeparam name="TResult">The type of the transformed pagination result.</typeparam>
    public abstract class UrlResultTransformerBase<T, TResult> : IPaginationTranformer<T, TResult> where T : class where TResult : class, new()
    {
        protected readonly string _baseUrl;
        protected readonly Dictionary<string, string> _parametersToInclude = new Dictionary<string, string>();
        protected readonly Dictionary<string, string> _parameterRenames = new Dictionary<string, string>();

        protected UrlResultTransformerBase(string baseUrl = "")
        {
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Configures the transformer to include the 'page' parameter in the generated URLs.
        /// </summary>
        /// <returns>The current instance of the transformer for chaining.</returns>
        public UrlResultTransformerBase<T, TResult> IncludePage()
        {
            _parametersToInclude["page"] = "page";
            return this;
        }

        /// <summary>
        /// Configures the transformer to include the 'pageSize' parameter in the generated URLs.
        /// </summary>
        /// <returns>The current instance of the transformer for chaining.</returns>
        public UrlResultTransformerBase<T, TResult> IncludePageSize()
        {
            _parametersToInclude["pageSize"] = "pageSize";
            return this;
        }

        /// <summary>
        /// Renames a parameter in the generated URLs.
        /// </summary>
        /// <param name="originalName">The original name of the parameter.</param>
        /// <param name="newName">The new name for the parameter.</param>
        /// <returns>The current instance of the transformer for chaining.</returns>f
        public UrlResultTransformerBase<T, TResult> RenameParameter(string originalName, string newName)
        {
            if (_parametersToInclude.ContainsKey(originalName))
            {
                _parameterRenames[originalName] = newName;
            }

            return this;
        }

        /// <summary>
        /// Adds a custom parameter to be included in the generated URLs.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The current instance of the transformer for chaining.</returns>
        public UrlResultTransformerBase<T, TResult> AddParameter(string name, string value)
        {
            _parametersToInclude[name] = value;
            return this;
        }

        /// <summary>
        /// Transforms the specified pagination result into a URL-enhanced result.
        /// </summary>
        /// <param name="paginationResult">The pagination result to transform.</param>
        /// <returns>The transformed pagination result with URL enhancements.</returns>
        public abstract TResult Transform(IPaginationResult<T> paginationResult);
    }


}
