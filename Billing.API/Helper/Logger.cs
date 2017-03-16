using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Helper
{
    public class Logger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Log(string Message, string Level = "ERROR")
        {
           //if (Token != null) Message += "..." + User.Name;
      
            string url = HttpContext.Current.Request.Url.AbsoluteUri;

            if (HttpContext.Current != null) Message += ":" + url;

            if (Level == "INFO") log.Info(Message); else log.Error(Message);

           // if (!url.ToLower().Contains("localhost"))
            //Email.Send(“admin@billing.com", “>> " + Level, Message);


        }
    }
}