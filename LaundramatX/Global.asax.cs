using LaundramatX.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LaundramatX
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public LaundramatX.Models.LaundromatModel LaundromatX = new Models.LaundromatModel();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        protected void Session_Start(object sender, EventArgs args)
        {

            try
            {
                var user = Request.Cookies.Get("UserDetails");

                if (user != null)
                {
                    var id = user[X.CookieuserID];
                    var isActive = Convert.ToBoolean(user[X.CookieIsActive]);

                    //The user was logged out last time
                    if (isActive)
                    {
                        int ID = Convert.ToInt32(id);
                        var userX = LaundromatX.Accounts.Where(acc => acc.AccountID == ID).ToList()[0];

                        Session.Add(X.UserX, userX);
                        Session.Add(X.isActivatedX, true);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void Session_End(object sender, EventArgs args)
        {

            try
            {
                HttpCookie VisitedCookie = new HttpCookie("visitedCookie", "true");
                Response.SetCookie(VisitedCookie);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
