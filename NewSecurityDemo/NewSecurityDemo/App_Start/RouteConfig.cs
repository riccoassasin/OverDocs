using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NewSecurityDemo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            #region User Docs Routes
            routes.MapRoute(
           name: "User Documents",
           url: "{controller}/{action}",
           defaults: new { controller = "UserDocs", action = "ShowUserDocs" });


            routes.MapRoute(
           name: "PublicDocUpload",
           url: "{controller}/{action}",
           defaults: new { controller = "UserDocs", action = "UploadUserFile" });
            #endregion




            #region User Private Docs
            routes.MapRoute(
                      name: "Private Documents",
                      url: "{controller}/{action}",
                      defaults: new { controller = "PrivateDocs", action = "PrivateDocDisplay" });
            #endregion


            #region Public Docs Routes

            //PublicDocDisplay
            routes.MapRoute(
            name: "PublicDocs",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "PublicDocs", action = "PublicDocDisplay", id = UrlParameter.Optional });

           
            //////This Routes to the default Public Docs Page
            ////routes.MapRoute(
            ////name: "PublicDocs",
            ////url: "{controller}/{action}/{id}",
            ////defaults: new { controller = "PublicDocs", action = "Index", id = UrlParameter.Optional });
            #endregion

        }
    }
}
