using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using AspWebApi.HiddenSwaggerParameter;

namespace AspWebApi.IPrincipalBinder
{
    public class FromPrincipalAttribute : ParameterBindingAttribute, ISwaggerHiddenAttribute
    {
        private readonly Type _converter;

        public FromPrincipalAttribute(Type converter)
        {
            if (!typeof(IFromPrincipalConverter).IsAssignableFrom(converter))
            {
                throw new ArgumentException("Converter Type must implement IFromPrincipalConverter.");
            }

            _converter = converter;
        }

        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            return new FromPrincipalParameterBinding(parameter, _converter);
        }
    }
}
