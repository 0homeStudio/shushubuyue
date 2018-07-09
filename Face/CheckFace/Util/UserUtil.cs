using CheckFace.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace CheckFace.Util
{
    public static partial class UserUtil
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static string clientId = "oAzMPLEaYU9yfGVfwPnYHOCO";
        private static string clientSerect = "N1GWA0v2G88zlmbsD0tkTYUDE6MwE1wd";
        public static string GetAccessToken()
        {
            string authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<string, string>> pairList = new List<KeyValuePair<string, string>>();
            pairList.Add(new KeyValuePair<string, string>("grant_type","client_credentials"));
            pairList.Add(new KeyValuePair<string, string>("client_id",clientId));
            pairList.Add(new KeyValuePair<string, string>("client_serect", clientSerect));
            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(pairList)).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }
        public static string Detect(string images,string image_type)
        {
            string host = "https://api.baidu.com/rest/2.0/face/v3/detect?access_token="+GetAccessToken();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(host);
            webRequest.Method="POST";
            webRequest.KeepAlive = true;
            Images imagess = new Images
            {
                Image = images,
                Image_type = image_type,
            };
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            byte[] buffer = System.Text.Encoding.Default.GetBytes(serializer.Serialize(imagess));
            webRequest.ContentLength = buffer.Length;
            webRequest.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
            string str = reader.ReadToEnd();
            return str;
        }
        /// <summary>
        /// 获取真ip
        /// </summary>
        /// <returns></returns>
        public static string GetRealIP()
        {
            string result = string.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (result != null && result != string.Empty)
            {
                //可能有代理
                if (result.IndexOf(".") == -1)
                {
                    result = null;
                }
                else
                {
                    if (result.IndexOf(".") != -1)
                    {
                        result = result.Replace(" ", "").Replace("'", "");
                        string[] ips = result.Split(",;".ToCharArray());
                        for (int i = 0; i < ips.Length; i++)
                        {
                            if (IsIPAddress(ips[i]) && ips[i].Substring(0, 3) != "10." && ips[i].Substring(0, 7) != "192.168" && ips[i].Substring(0, 3) != "172.16.")
                            {
                                return ips[i];
                            }
                        }
                    }
                    else if (IsIPAddress(result))
                        return result;
                    else
                        result = null;
                }
            }
            string ipAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != string.Empty ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : null);
            if (null == result || result == string.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (result == null || result == string.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }
        public static string GetLocalByIpAddress(string ip)
        {
            Uri uri = new Uri(string.Format("http://api.map.baidu.com/location/ip?ip={0}&ak={1}",ip,System.Configuration.ConfigurationManager.AppSettings["AK"]));
            WebRequest webRequest;
            WebResponse webResponse;
            StreamReader sr;
            string strReturn = string.Empty;
            try
            {
                webRequest = WebRequest.Create(uri);
                webResponse = webRequest.GetResponse();
                sr = new StreamReader(webResponse.GetResponseStream());
                strReturn = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            return U2U(strReturn);
        }
        public static string U2U(string str)
        {
            string res = str;
            MatchCollection reg = Regex.Matches(res, @"\\u\w{4}");
            for (int i = 0; i < reg.Count; i++)
            {
                res = res.Replace(reg[i].Groups[0].Value, "" + Regex.Unescape(reg[i].Value.ToString()).ToString());
            }
            return res;
        }
        public static bool IsIPAddress(string str)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length < 7 || str.Length > 15)
                return false;
            string regformat = @"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str);
        }
    }
}