using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AspWebApi.IPrincipalBinder
{
    public interface IClaimBinding
    {
        object GetValueFromPrincipal(string descriptorParameterName, Type descriptorParameterType, IPrincipal principal);
    }
}
