using CheckFace.Models;
using CheckFace.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CheckFace.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public string UploadImage(string imgdatabase)
        {
            Console.WriteLine(imgdatabase);
            return null;
        }
        public ActionResult Start()
        {
            return View();
        }
        public string OnlineAndMatch()
        {
            string ip = UserUtil.GetRealIP();
            if (ip.IndexOf(":") != -1)
            {
                ip = ip.Substring(0, ip.IndexOf(":"));
            }
            ip = "180.175.255.255";
            string str=UserUtil.GetLocalByIpAddress(ip);
            Dictionary<string,User> users=(Dictionary<string,User>)HttpContext.Application.Get("OnlineUsers");
            JObject obj= JObject.Parse(str);
            string local = obj["content"]["address_detail"]["city"].ToString();
            users.Add("" + ip, new User { Age = 0, Host = ip, Local = local });
            HttpContext.Application["OnlineUsers"] = users;
            return str;
        }
    }
}