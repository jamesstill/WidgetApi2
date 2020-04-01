using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WidgetApi.Swagger
{
    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            var versionToken = "v";
            var controllerNamespace = controller.ControllerType.Namespace; // e.g. "FooApi.V1.Controllers"
            // by convention the letter 'V' must always be in namespace. E.g.: "V1, V2, Vn"
            var namespaceTokens = controllerNamespace.Split('.');
            var apiVersion = namespaceTokens.FirstOrDefault(n => n.Contains(versionToken, StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrEmpty(apiVersion))
            {
                apiVersion = "v1";
            }

            controller.ApiExplorer.GroupName = apiVersion.ToLower(CultureInfo.CurrentCulture);
        }
    }
}
