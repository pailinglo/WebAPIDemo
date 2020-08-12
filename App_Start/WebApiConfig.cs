using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using WebApiContrib.Formatting.Jsonp;
using WebAPIDemo.Custom;

namespace WebAPIDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            

            //When the request is from browser, the Accept type is text/html, so by default, we will send
            //data back in xml format. to always send back data in json format when the request is from browser:
            //but the drawback of this approach is the content type in response still says text/html not json.
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            //approach 2 - add a custom formatter
            //config.Formatters.Add(new CustomJsonFormatter());

            //to always send back data in Json format:
            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            //vice versa, if we want our response always in XML format irrespective of the accept type:
            //config.Formatters.Remove(config.Formatters.JsonFormatter);


            //JsonFormatter:JsonMediaTypeFormatter is also inherited from MediaTypeFormatter -> BaseJsonMediaTypeFormatter -> JsonMediaTypeFormatter
            //make the json data properly indented
            //config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //make variable name camelcase.
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();	

            //to support the JSONP format for cross-origin ajax request.
            //var jsonpFormatter = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter);
            //config.Formatters.Insert(0, jsonpFormatter);

            //to enable CORS:
            //EnableCorsAttribute cors = new EnableCorsAttribute("https://localhost:44355", "*", "*");
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            //config.EnableCors();

            //redirect http request to https
            //register this attribute globally such that every WebApi request will need to be https request
            config.Filters.Add(new RequireHttpsAttribute());

            //apply basic authentication globally
            //config.Filters.Add(new BasicAuthenticationAttribute());

            //this is to demo using query parameters for api version switching
            config.Services.Replace(typeof(IHttpControllerSelector), new CustomControllerSelector(config));
        }
    
    
        
    }

    public class CustomJsonFormatter : JsonMediaTypeFormatter
    {
        public CustomJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}
