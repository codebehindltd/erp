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
    public partial class AppraisalEvaluationBy : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static GridViewDataNPaging<EmployeeForEvalutionBO, GridPaging> GetAppraisalEvalutionBy( int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<EmployeeForEvalutionBO> AppraisalEvalutionList = new List<EmployeeForEvalutionBO>();
            GridViewDataNPaging<EmployeeForEvalutionBO, GridPaging> myGridData = new GridViewDataNPaging<EmployeeForEvalutionBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (userInformationBO.EmpId != 0)
            {
                AppraisalEvaluationDA evaluationDa = new AppraisalEvaluationDA();
                int empId = userInformationBO.IsAdminUser == false ? userInformationBO.EmpId : 0;

                AppraisalEvalutionList = evaluationDa.GetEmployeeForEvalutionForGridPaging(userInformationBO.EmpId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            }

            myGridData.GridPagingProcessing(AppraisalEvalutionList, totalRecords);
            return myGridData;
        }
    }
}