using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ProjectEvalutionSystem
{
    public class AppConstants
    {
        public static string GetCopyLeaksEndPoint()
        {
            return ConfigurationManager.AppSettings["CopyLeaksAPIEndPoint"].ToString();
        }

    }
}