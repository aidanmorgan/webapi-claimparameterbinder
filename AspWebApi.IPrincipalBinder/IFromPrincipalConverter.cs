using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AspWebApi.IPrincipalBinder
{
    public interface IFromPrincipalConverter
    {
        object FromPrincipal(IPrincipal principal);
    }
}
