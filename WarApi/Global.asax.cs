﻿//using Autofac;
//using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;

namespace WarApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
