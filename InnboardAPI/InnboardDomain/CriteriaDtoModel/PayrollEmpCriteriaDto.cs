using InnboardDomain.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.CriteriaDtoModel
{
    public class PayrollEmpCriteriaDto
    {
        public PayrollEmpCriteriaDto()
        {
            pageParams = new PageParams();
        }
        public int EmpId { get; set; }
        public PageParams pageParams { get; set; }
    }
}
