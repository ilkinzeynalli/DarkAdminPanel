using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebApi.Routing
{
    public class ApiRoutePrefixAttribute : RouteAttribute
    {
        private const string RouteBase = "api";
        private const string PrefixRouteBase = RouteBase + "/";

        public ApiRoutePrefixAttribute(string routePrefix)
            : base(string.IsNullOrWhiteSpace(routePrefix) ? RouteBase : PrefixRouteBase + routePrefix)
        {
        }
    }
}
