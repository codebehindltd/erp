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

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmInvStockVarianceApproval : BasePage
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

                if (Session["FinishProductId"] != null)
                {
                    hfIsEditedFromApprovedForm.Value = "1";
                    hfStockAdjustmentId.Value = Session["FinishProductId"].ToString();

                    Session.Remove("FinishProductId");
                }
            }
        }

        protected void gvFinishedProductInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (ItemStockAdjustmentBO)e.Row.DataItem;

                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgDetailsApproved");

                if (item.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString() && item.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    imgUpdate.Visible = isUpdatePermission;
                    imgDelete.Visible = isDeletePermission;
                    imgApproved.Visible = isSavePermission;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                    imgApproved.Visible = false;
                }
            }
        }

        protected void gvFinishedProductInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "CmdEdit")
                {
                    int receivedId = Convert.ToInt32(e.CommandArgument.ToString());

                    Session["StockAdjustmentId"] = receivedId.ToString();
                    Response.Redirect("frmInvProductAdjustment.aspx");
                }
                if (e.CommandName == "CmdDelete")
                {
                    int stockAdjustmentId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    InvItemDA itemDa = new InvItemDA();
                    bool status = itemDa.DeleteItemStockAdjustment(stockAdjustmentId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        LoadGrid();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }
                else if (e.CommandName == "CmdItemApproved")
                {
                    int finishProductId = Convert.ToInt32(e.CommandArgument.ToString());

                    ItemStockAdjustmentBO stockAdjustment = new ItemStockAdjustmentBO();
                    InvItemDA itemDa = new InvItemDA();

                    stockAdjustment.StockAdjustmentId = finishProductId;
                    stockAdjustment.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    bool status = itemDa.ApprovedAdjustmentStatusNUpdateItemStock(1, "", userInformationBO.UserInfoId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
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
            InvItemDA itemDa = new InvItemDA();
            List<ItemStockAdjustmentBO> itemAdjustment = new List<ItemStockAdjustmentBO>();

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

            itemAdjustment = itemDa.GetItemAdjustmentInfoSearch(costCenterId, 0, fromDate, toDate);

            gvFinishedProductInfo.DataSource = itemAdjustment;
            gvFinishedProductInfo.DataBind();
        }

        //************************ User Defined Web Method ********************//

        [WebMethod]
        public static List<ItemStockAdjustmentDetailsBO> GetAdjustmentProductDetails(int stockAdjustmentId)
        {
            InvItemDA itemDa = new InvItemDA();
            return itemDa.GetItemAdjustmentDetailsById(stockAdjustmentId);
        }

    }
}