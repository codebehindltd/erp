using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmPMOutProductReceive : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadPONumber();
            }
        }

        protected void btnReceive_Click(object sender, EventArgs e)
        {
            PMProductOutDA outDA = new PMProductOutDA();
            int outId = 0, lastmodifiedBy = 0;
            if (!string.IsNullOrEmpty(hfOutId.Value))
            {
                outId = Convert.ToInt32(hfOutId.Value);
            }
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            lastmodifiedBy = userInformationBO.UserInfoId;

            bool status = outDA.UpdateProductOutFromReceive(outId, lastmodifiedBy);
            if (status == true)
            {
                CommonHelper.AlertInfo("Product Receive " + AlertMessage.Success, AlertType.Success);
                LoadPONumber();
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                       EntityTypeEnum.EntityType.UserInformation.ToString(), outId,
                       ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                       "Purchase Management Out Product Received");
            }
        }

        private void LoadPONumber()
        {
            PMProductOutDA entityDA = new PMProductOutDA();
            ddlPONumber.DataSource = entityDA.GetProductOutForReceive();
            ddlPONumber.DataTextField = "PONumber";
            ddlPONumber.DataValueField = "OutId";
            ddlPONumber.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlPONumber.Items.Insert(0, item);
        }

        [WebMethod]
        public static List<PMProductOutDetailsBO> GetProductOutDetails(int outId)
        {
            PMProductOutDA outDA = new PMProductOutDA();
            return outDA.GetProductOutDetailsById(outId);
        }
    }
}