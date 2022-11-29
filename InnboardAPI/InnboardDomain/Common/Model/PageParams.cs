using InnboardDomain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Common.Model
{
    public class PageParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = int.Parse(AppConstants.DefaultPerPageItemCount);
        public string SearchKey { get; set; } = "";
    }
}
