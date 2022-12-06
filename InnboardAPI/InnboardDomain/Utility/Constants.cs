using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Utility
{
    public static class AppConstants
    {
        public static string PassCode = "€ƒŒˆ";

        public static string CompanyCode = System.Web.Configuration.WebConfigurationManager.AppSettings["CompanyCode"].ToString();

        public static string DefaultPerPageItemCount = System.Web.Configuration.WebConfigurationManager.AppSettings["DefaultPerPageItemCount"].ToString();
    }
}
