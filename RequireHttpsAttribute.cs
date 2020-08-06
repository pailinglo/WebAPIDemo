using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebAPIDemo
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                HttpResponseMessage message = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Found);
                message.Content = new StringContent("<p>Use https instead of http</p>", Encoding.UTF8, "text/html");
                UriBuilder uri = new UriBuilder(actionContext.Request.RequestUri);
                uri.Scheme = Uri.UriSchemeHttps;
                uri.Port = 44353;
                message.Headers.Location = uri.Uri;
                actionContext.Response = message;
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }
    }
}