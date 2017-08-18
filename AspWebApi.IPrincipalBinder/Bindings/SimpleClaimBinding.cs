using System;
using System.ComponentModel;
using System.Security.Principal;

namespace AspWebApi.IPrincipalBinder.Bindings
{
    public class SimpleClaimBinding : IClaimBinding
    {
        private readonly string _claimName;

        public SimpleClaimBinding()
        {
            
        }

        public SimpleClaimBinding(string claimName)
        {
            _claimName = claimName;
        }

        public object GetValueFromPrincipal(string descriptorParameterName, Type descriptorParameterType, IPrincipal principal)
        {
            var claimName = _claimName;

            if (string.IsNullOrEmpty(claimName))
            {
                claimName = descriptorParameterName;
            }

            var claim = principal.GetClaim(claimName);

            if (claim != null)
            {
                return TypeDescriptor.GetConverter(descriptorParameterType).ConvertFromInvariantString(claim.Value);
            }

            return null;
        }
    }
}