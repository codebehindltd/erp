using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class RFQComparativeStatement : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadIndentDropdown();
                CheckPermission();
            }
        }

        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }


        [WebMethod]
        public static List<RFQuotationItemsFeedbackBO> GetRFQuotationItemFeedbackByRFQItemId(int RFQItemId)
        {

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<RFQuotationItemsFeedbackBO> RFQuotationItemsFeedbackList = new List<RFQuotationItemsFeedbackBO>();


            PMRFQuotationDA rfqDA = new PMRFQuotationDA();

            RFQuotationItemsFeedbackList = rfqDA.GetRFQuotationItemFeedbackByRFQItemId(RFQItemId);


            return RFQuotationItemsFeedbackList;
        }

        [WebMethod]
        public static GridViewDataNPaging<RFQuotationItemBO, GridPaging> GetRFQuotationItems(int quotationId, DateTime? fromDate, DateTime? toDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<RFQuotationItemBO> RFQuotationItemBOList = new List<RFQuotationItemBO>();
            GridViewDataNPaging<RFQuotationItemBO, GridPaging> myGridData = new GridViewDataNPaging<RFQuotationItemBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);



            PMRFQuotationDA rfqDA = new PMRFQuotationDA();

            RFQuotationItemBOList = rfqDA.GetRFQuotationItemsForGridPaging(quotationId, fromDate, toDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);



            myGridData.GridPagingProcessing(RFQuotationItemBOList, totalRecords);
            return myGridData;
        }

        //[WebMethod]
        //public static List<RFQuotationItemBO> LoadItemNameByIndentId(int RFQid)
        //{
        //    HMUtility hmUtility = new HMUtility();

        //    PMRFQuotationDA rfqDA = new PMRFQuotationDA();
        //    List<RFQuotationItemBO> RFQuotationItemBOList = rfqDA.GetItemsByIndentId(RFQid);

        //    return RFQuotationItemBOList;
        //}


        private void loadIndentDropdown()
        {
            PMRFQuotationDA rfqDA = new PMRFQuotationDA();
            List<RFQuotationBO> RFQuotationBOList = rfqDA.GetAllIndentInfo();

            ddlIndentName.DataSource = RFQuotationBOList;
            ddlIndentName.DataTextField = "IndentName";
            ddlIndentName.DataValueField = "RFQId";
            ddlIndentName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlIndentName.Items.Insert(0, item);
        }
    }
}