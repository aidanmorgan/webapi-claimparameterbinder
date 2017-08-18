using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspWebApi.IPrincipalBinder
{
    public class FromClaimTypeConverters
    {
        private static readonly IDictionary<Type, IFromClaimTypeConverter> _converters = new Dictionary<Type, IFromClaimTypeConverter>();

        public static void Add(Type t, IFromClaimTypeConverter converter)
        {
            _converters[t] = converter;
        }

        public static IFromClaimTypeConverter Get(Type t)
        {
            return _converters[t];
        }
    }
}
