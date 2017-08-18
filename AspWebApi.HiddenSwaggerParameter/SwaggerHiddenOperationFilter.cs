using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace AspWebApi.HiddenSwaggerParameter
{
    public class SwaggerHiddenOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            foreach (var description in apiDescription.ParameterDescriptions)
            {
                var bindingAttribute = description.ParameterDescriptor.ParameterBinderAttribute;

                if (bindingAttribute is ISwaggerHiddenAttribute)
                {
                    var toRemove = operation.parameters.FirstOrDefault(x => x.name == description.Name);

                    if (toRemove != null)
                    {
                        operation.parameters.Remove(toRemove);
                    }
                }
            }
        }
    }
}
