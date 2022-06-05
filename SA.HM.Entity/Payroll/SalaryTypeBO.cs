using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class SalaryTypeBO
    {
        public string SalaryType { get; set; }
        public string SalaryTypeValue { get; set; }

        public SalaryTypeBO() { }

        public SalaryTypeBO(string st, string stv)
        {
            SalaryType = st;
            SalaryTypeValue = stv;
        }

        public List<SalaryTypeBO> SalaryTypeList()
        {
            List<SalaryTypeBO> stl = new List<SalaryTypeBO>();
            SalaryTypeBO st;

            st = new SalaryTypeBO("Allowance", "Allowance");
            stl.Add(st);

            st = new SalaryTypeBO("Deduction", "Deduction");
            stl.Add(st);

            st = new SalaryTypeBO("Additional Allowance", "AdditionalAllowance");
            stl.Add(st);

            return stl;
        }
    }
}
