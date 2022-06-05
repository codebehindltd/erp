using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class PayrollEmployeeType : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadEmployeeType();
        }
        private void LoadEmployeeType()
        {
            HMUtility hmUtility = new HMUtility();
            List<EmployeeStatusBO> list = new List<EmployeeStatusBO>();
            EmployeeDA DA = new EmployeeDA();
            list = DA.GetEmployeeStatus();

            ddlEmployeeStatus.DataSource = list;
            ddlEmployeeStatus.DataTextField = "EmployeeStatus";
            ddlEmployeeStatus.DataValueField = "EmployeeStatusId";
            ddlEmployeeStatus.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEmployeeStatus.Items.Insert(0, item);
        }

        [WebMethod]
        public static ReturnInfo SaveEmployeeStatus(PayrollEmpStatusHistoryBO PayrollEmpStatusHistory)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                int _userId = Convert.ToInt32(hmUtility.GetCurrentApplicationUserInfo().UserInfoId);
                PayrollEmpStatusHistory.CreatedBy = _userId;

                PayrollEmpStatusHistoryDA DA = new PayrollEmpStatusHistoryDA();
                status = DA.SavePayrollEmpStatusHistory(PayrollEmpStatusHistory);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return rtninf;
        }
    }
}