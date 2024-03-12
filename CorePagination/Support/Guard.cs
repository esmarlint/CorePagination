using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Support
{
    internal static class Guard
    {
        public static void NotNull(object input, string parameterName = null)
        {
            if (input == null)
            {
                throw new ArgumentNullException(parameterName ?? "Value", "Value cannot be null.");
            }
        }

    }
}
