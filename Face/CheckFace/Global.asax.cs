using CheckFace.Models;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using static Antlr.Runtime.Tree.TreeWizard;

namespace CheckFace
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //online user primary key
        public const string OnlineUsers = "OnlineUsers";
        public Dictionary<string, User> loginUsers=new Dictionary<string, User>();
        private Logger logger = LogManager.GetCurrentClassLogger();
        protected void Application_Start(object sender,EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DataTable userTable = new DataTable();
            Application.Lock();
            Application[OnlineUsers] = loginUsers;
            Application.UnLock();
        }
      
    }
}
