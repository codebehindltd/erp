using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Collections;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using Newtonsoft.Json;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmProductReceiveAccountsPostingApproval : BasePage
    {
        ArrayList arrayDelete;
        HiddenField innboardMessage;
        protected int _RestaurantComboId;
        protected int IsService = -1;
        protected int btnPadding = -1;
        HMUtility hmUtility = new HMUtility();
        
        //**************************** Handlers ********************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                Session["PMProductReceive"] = null;
                CheckPermission();
            }
            arrayDelete = new ArrayList();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridview();
        }
        protected void btnApprovedPosting_Click(object sender, EventArgs e)
        {
            bool status = false;
            int rows = 0;
            List<int> receiveId = new List<int>();

            try
            {
                rows = gvProductReceiveInfo.Rows.Count;

                for (int i = 0; i < rows; i++)
                {
                    CheckBox cb = (CheckBox)gvProductReceiveInfo.Rows[i].FindControl("chkIsSavePermission");

                    if (cb.Checked == true)
                    {
                        Label lblReceivedId = (Label)gvProductReceiveInfo.Rows[i].FindControl("lblReceivedId");
                        receiveId.Add(Convert.ToInt32(lblReceivedId.Text));
                    }
                }

                PMProductReceivedDA receiveda = new PMProductReceivedDA();
                status = receiveda.UpdateProductReceiveAccountsPostingApproval(receiveId);

                if (status)
                {
                    string receiveIdList = string.Empty;
                    foreach (int row in receiveId)
                    {
                        if (!string.IsNullOrWhiteSpace(receiveIdList))
                        {
                            receiveIdList = receiveIdList + "," + row.ToString();
                        }
                        else
                        {
                            receiveIdList = row.ToString();
                        }
                    }
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    HMDayCloseDA daycloseDA = new HMDayCloseDA();
                    HMDayCloseBO dayCloseBO = new HMDayCloseBO();
                    dayCloseBO.DayClossingDate = DateTime.Now; //hmUtility.GetDateTimeFromString(txtDayClossingDate.Text, userInformationBO.ServerDateFormat);
                    dayCloseBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean success = daycloseDA.ProductReceiveAccountsPostingProcess(dayCloseBO, receiveIdList);
                    if (success)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Purchase Journal Posting Successfull.", AlertType.Success);
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Purchase Journal Posting Failed.", AlertType.Error);
                    }

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), "Purchase Journal Posting", 0,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Purchase Journal Posting");
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    LoadGridview();
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        //************************ User Defined Function ***********************//

        public void LoadGridview()
        {
            PMProductReceivedDA receiveDA = new PMProductReceivedDA();
            List<PMProductReceivedBO> receivedList = new List<PMProductReceivedBO>();
            string orderId = string.Empty, receiveNumber = string.Empty;

            string startDate = string.Empty;
            string endDate = string.Empty;

            DateTime dateTime = DateTime.Now;
            DateTime FromDate = dateTime;
            DateTime ToDate = dateTime;

            if (!string.IsNullOrEmpty(txtReceiveNumber.Text))
            {
                receiveNumber = txtReceiveNumber.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                startDate = txtFromDate.Text;
                FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                endDate = txtToDate.Text;
                ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            receivedList = receiveDA.GetProductReceiveByReceiveNoNDateRange(FromDate, ToDate, receiveNumber);

            gvProductReceiveInfo.DataSource = receivedList;
            gvProductReceiveInfo.DataBind();
        }
        public bool isValidForm()
        {
            bool status = true;
            if (Session["PMProductReceive"] == null)
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Quantity.", AlertType.Warning);
            }
            return status;

        }

        private void CheckPermission()
        {
            btnApprovedPosting.Visible = isSavePermission;
            
         
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<PMProductReceivedDetailsBO> ReceivedDetails(int receivedId)
        {
            PMProductReceivedDA receivedDa = new PMProductReceivedDA();
            return receivedDa.GetProductreceiveDetailsInfo(receivedId);
        }
        [WebMethod]
        public static ReturnInfo ApprovedReceivedDetails(int receivedId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                PMProductReceivedBO receivedProduct = new PMProductReceivedBO();
                PMProductReceivedDA receiveDA = new PMProductReceivedDA();

                receivedProduct.ReceivedId = receivedId;
                receivedProduct.Status = "Approved";
                receivedProduct.ReceivedDate = DateTime.Now;
                receivedProduct.CreatedBy = userInformationBO.UserInfoId;

                status = receiveDA.UpdateProductReceiveStatusNItemStockNAverageCost(receivedProduct);

                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                       EntityTypeEnum.EntityType.PMProductReceived.ToString(), receivedProduct.ReceivedId,
                       ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                       hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductReceived));
                }
                else
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }
        [WebMethod]
        public static ReturnInfo CancelReceivedDetails(int receivedId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                PMProductReceivedBO receivedProduct = new PMProductReceivedBO();
                PMProductReceivedDA receiveDA = new PMProductReceivedDA();

                receivedProduct.ReceivedId = receivedId;
                receivedProduct.LastModifiedBy = userInformationBO.UserInfoId;
                receivedProduct.Status = "Cancel";

                status = receiveDA.UpdateProductReceiveStatus(receivedProduct);

                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Cancel, AlertType.Success);
                }
                else
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }
        [WebMethod(EnableSession = true)]
        public static ReturnInfo EditReceivedDetails(int receivedId)
        {
            ReturnInfo rtninfo = new ReturnInfo();

            try
            {
                HttpContext.Current.Session["ProductReceivedId"] = receivedId.ToString();
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }
    }
}