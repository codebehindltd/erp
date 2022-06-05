using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Data.Inventory;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmInvFinishedProductApproval : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                this.LoadCostCenter();
            }
        }
        protected void gvFinishedProductInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (FinishedProductBO)e.Row.DataItem;

                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgDetailsApproved");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgCancel = (ImageButton)e.Row.FindControl("ImgBtnCancel");

                if (item.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString())
                {
                    imgApproved.Visible = isSavePermission;
                    imgCancel.Visible = isDeletePermission;
                    //imgUpdate.Visible = true; //isSavePermission;
                    imgUpdate.Visible = isUpdatePermission;
                }
                else
                {
                    imgApproved.Visible = false;
                    imgCancel.Visible = false;
                    //imgUpdate.Visible = false;
                    imgUpdate.Visible = false;
                }
                
            }
        }
        protected void gvFinishedProductInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (e.CommandName == "CmdEdit")
                {
                    int receivedId = Convert.ToInt32(e.CommandArgument.ToString());

                    Session["FinishProductId"] = receivedId.ToString();
                    Response.Redirect("frmInvFinishedProduct.aspx");
                }
                else if (e.CommandName == "CmdItemApproved")
                {
                    int finishProductId = Convert.ToInt32(e.CommandArgument.ToString());

                    FinishedProductBO finishedProduct = new FinishedProductBO();
                    PMFinishProductDA finishGoodsDa = new PMFinishProductDA();

                    finishedProduct.FinishProductId = finishProductId;
                    finishedProduct.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();

                    bool status = finishGoodsDa.UpdateFinishGoodsOrderStatus(finishedProduct);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                            EntityTypeEnum.EntityType.FinishedProduct.ToString(), finishProductId,
                            ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                            "Finished Goods Order Status Updated");
                        LoadGrid();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }
                else if (e.CommandName == "CmdItemCancel")
                {
                    int finishProductId = Convert.ToInt32(e.CommandArgument.ToString());

                    FinishedProductBO finishedProduct = new FinishedProductBO();
                    PMFinishProductDA finishGoodsDa = new PMFinishProductDA();

                    finishedProduct.FinishProductId = finishProductId;
                    finishedProduct.ApprovedStatus = HMConstants.ApprovalStatus.Cancel.ToString();

                    bool status = finishGoodsDa.UpdateFinishGoodsOrderStatus(finishedProduct);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Cancel, AlertType.Success);
                        LoadGrid();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }
        //************************ User Defined Function ********************//
        
        private void LoadCostCenter()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();
            var List = entityDA.GetCostCentreTabInfo();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            this.ddlSearchCostCenter.DataSource = List;
            this.ddlSearchCostCenter.DataTextField = "CostCenter";
            this.ddlSearchCostCenter.DataValueField = "CostCenterId";
            this.ddlSearchCostCenter.DataBind();
            this.ddlSearchCostCenter.Items.Insert(0, item);
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void LoadGrid()
        {
            PMFinishProductDA productDa = new PMFinishProductDA();
            List<FinishedProductBO> finishGoods = new List<FinishedProductBO>();

            DateTime? fromDate = null, toDate = null;
            int costCenterId = 0;

            if (ddlSearchCostCenter.SelectedValue != "0")
            {
                costCenterId = Convert.ToInt32(ddlSearchCostCenter.SelectedValue);
            }

            if (!string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                //fromDate = Convert.ToDateTime(txtFromDate.Text);
                fromDate = CommonHelper.DateTimeToMMDDYYYY(txtFromDate.Text);
            }

            if (!string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                //toDate = Convert.ToDateTime(txtToDate.Text);
                toDate = CommonHelper.DateTimeToMMDDYYYY(txtToDate.Text);
            }

            finishGoods = productDa.GetFinishedProductSearch(costCenterId, fromDate, toDate);

            gvFinishedProductInfo.DataSource = finishGoods;
            gvFinishedProductInfo.DataBind();
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<FinishedProductDetailsBO> GetFinishProductDetails(int finishProductId)
        {
            PMFinishProductDA productDa = new PMFinishProductDA();
            return productDa.GetFinishedProductDetailsById(finishProductId);
        }
    }
}