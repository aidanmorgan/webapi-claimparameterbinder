using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace AspWebApi.IPrincipalBinder
{
    public class FromPrincipalParameterBinding : HttpParameterBinding
    {
        private readonly HttpParameterDescriptor _descriptor;
        private readonly Type _converter;


        public FromPrincipalParameterBinding(HttpParameterDescriptor descriptor, Type converter) : base(descriptor)
        {
            _descriptor = descriptor;
            _converter = converter;
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var principal = actionContext.RequestContext.Principal;

            var converter = Activator.CreateInstance(_converter) as IFromPrincipalConverter;

            var value = converter.FromPrincipal(principal);
            SetValue(actionContext, value);

            return Task.CompletedTask;
        }
    }
}
