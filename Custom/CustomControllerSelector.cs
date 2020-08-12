using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace WebAPIDemo.Custom
{
    public class CustomControllerSelector : DefaultHttpControllerSelector
    {
        private HttpConfiguration _config;
        public CustomControllerSelector(HttpConfiguration config) : base(config)
        {
            _config = config;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            // Get all the available Web API controllers
            var controllers = GetControllerMapping();
            // Get the controller name and parameter values from the request URI
            var routeData = request.GetRouteData();

            // Get the controller name from route data.
            // The name of the controller in our case is "Students"
            var controllerName = routeData.Values["controller"].ToString();

            // Default version number to 1
            string versionNumber = "1";
            var versionQueryString = HttpUtility.ParseQueryString(request.RequestUri.Query);
            if (versionQueryString["v"] != null)
            {
                versionNumber = versionQueryString["v"];
            }

            if (versionNumber == "1")
            {
                // if version number is 1, then append V1 to the controller name.
                // So at this point the, controller name will become StudentsV1
                controllerName = controllerName + "V1";
            }
            else
            {
                // if version number is 2, then append V2 to the controller name.
                // So at this point the, controller name will become StudentsV2
                controllerName = controllerName + "V2";
            }

            HttpControllerDescriptor controllerDescriptor;
            if (controllers.TryGetValue(controllerName, out controllerDescriptor))
            {
                return controllerDescriptor;
            }

            return null;
        }
    }
}