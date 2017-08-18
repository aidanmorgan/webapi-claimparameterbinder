using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using AspWebApi.HiddenSwaggerParameter;
using AspWebApi.IPrincipalBinder.Bindings;

namespace AspWebApi.IPrincipalBinder
{
    public class FromClaimAttribute : ParameterBindingAttribute, ISwaggerHiddenAttribute
    {
        private readonly IClaimBinding _binding;

        public FromClaimAttribute(Type converterType)
        {
            _binding = new ComplexClaimBinding(converterType);
        }

        public FromClaimAttribute(BindingType type = BindingType.Simple)
        {
            if (type == BindingType.Complex)
            {
                _binding = new ComplexClaimBinding();
            }
            else
            {
                _binding = new SimpleClaimBinding();
            }
        }

        public FromClaimAttribute(string claimName)
        {
            _binding = new SimpleClaimBinding(claimName);
        }

        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            return new FromClaimParameterBinding(parameter, _binding);
        }
    }
}
