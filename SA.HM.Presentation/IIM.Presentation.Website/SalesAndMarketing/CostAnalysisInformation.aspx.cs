using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class CostAnalysisInformation : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static GridViewDataNPaging<SMCostAnalysis, GridPaging> GetCostAnalysisWithPagination(string name, string fromDate, string toDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = new HMUtility().GetCurrentApplicationUserInfo();

            int totalRecords = 0;

            GridViewDataNPaging<SMCostAnalysis, GridPaging> myGridData = new GridViewDataNPaging<SMCostAnalysis, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            CostAnalysisDA costAnalysisDA = new CostAnalysisDA();
            List<SMCostAnalysis> costAnalysisList = new List<SMCostAnalysis>();

            DateTime FromDate = !string.IsNullOrEmpty(fromDate) ? CommonHelper.DateTimeToMMDDYYYY(fromDate) : DateTime.Now;
            DateTime ToDate = !string.IsNullOrEmpty(toDate) ? CommonHelper.DateTimeToMMDDYYYY(toDate) : DateTime.Now;

            costAnalysisList = costAnalysisDA.GetCostAnalysisWithPagination(name, FromDate, ToDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(costAnalysisList, totalRecords);

            return myGridData;
        }
    }
}