using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using AspWebApi.HiddenSwaggerParameter;
using AspWebApi.IPrincipalBinder.Bindings;

namespace AspWebApi.IPrincipalBinder
{
    public class FromClaimParameterBinding : HttpParameterBinding
    {
        private readonly HttpParameterDescriptor _descriptor;
        private readonly IClaimBinding _binding;


        public FromClaimParameterBinding(HttpParameterDescriptor descriptor, IClaimBinding binding = null) : base(descriptor)
        {
            _descriptor = descriptor;
            _binding = binding ?? new SimpleClaimBinding();
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var principal = actionContext.RequestContext.Principal;

            var value = _binding.GetValueFromPrincipal(_descriptor.ParameterName, _descriptor.ParameterType, principal);

            if (value != null)
            {
                SetValue(actionContext, value);
                return Task.CompletedTask;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }
    }



}
