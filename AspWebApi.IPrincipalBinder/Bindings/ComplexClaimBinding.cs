using System;
using System.Security.Principal;

namespace AspWebApi.IPrincipalBinder.Bindings
{
    public class ComplexClaimBinding : IClaimBinding
    {
        private readonly Type _converterType;

        public ComplexClaimBinding()
        {
        }

        public ComplexClaimBinding(Type converterType)
        {
            if (!typeof(IFromClaimTypeConverter).IsAssignableFrom(converterType))
            {
                throw new ArgumentException($"Provided type does not implement IFromClaimTypeConverter.");
            }

            _converterType = converterType;
        }

        public object GetValueFromPrincipal(string descriptorParameterName, Type descriptorParameterType, IPrincipal principal)
        {
            IFromClaimTypeConverter converter = null;

            if (_converterType != null)
            {
                converter = Activator.CreateInstance(_converterType) as IFromClaimTypeConverter;
            }
            else
            {
                converter = FromClaimTypeConverters.Get(descriptorParameterType);
            }
            
            return converter.FromPrincipal(principal);
        }
    }
}