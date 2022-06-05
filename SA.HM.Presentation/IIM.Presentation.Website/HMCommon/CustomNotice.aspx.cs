using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class CustomNotice : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPermission();
            }
        }
        private void CheckPermission()
        {
            hfViewPermission.Value = isViewPermission ? "1" : "0";
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        [WebMethod]
        public static GridViewDataNPaging<CustomNoticeBO, GridPaging> LoadGridPaging(string name, string fromDate, string toDate, int gridRecordsCount, int pageNumber, int IsCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            if (fromDate != "" && toDate == "")
            {
                toDate = DateTime.Now.ToShortDateString();
            }

            GridViewDataNPaging<CustomNoticeBO, GridPaging> myGridData = new GridViewDataNPaging<CustomNoticeBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, IsCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<CustomNoticeBO> noticeBOs = new List<CustomNoticeBO>();
            CustomNoticeDA customNoticeDA = new CustomNoticeDA();
            noticeBOs = customNoticeDA.GetNoticeInfoForSearch(name, fromDate, toDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(noticeBOs, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo DeleteNotice(int Id)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            int id = 0;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            CustomNoticeDA customNoticeDA = new CustomNoticeDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            status = customNoticeDA.DeleteNotice(Id);
            if (status)
            {
                info.IsSuccess = true;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                info.Data = 0;
            }
            else
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }
        [WebMethod]
        public static List<DocumentsBO> LoadNotice(int Id)
        {
            List<DocumentsBO> notice = new List<DocumentsBO>();            
            notice = new DocumentsDA().GetDocumentsByUserTypeAndUserId("CustomNoticeDocument", Id);
            return notice;
        }
    }
}