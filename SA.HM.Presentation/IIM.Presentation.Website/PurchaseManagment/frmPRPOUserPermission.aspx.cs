using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmPRPOUserPermission : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                this.LoadCostCenter();
                this.LoadEmployee();

                this.txtProbableFromHour.Text = "12";
                this.txtProbableFromMinute.Text = "00";
                this.ddlProbableFromAMPM.SelectedIndex = 1;

                this.txtProbableToHour.Text = "12";
                this.txtProbableToMinute.Text = "00";
                this.ddlProbableToAMPM.SelectedIndex = 1;
            }
            CheckObjectPermission();
        }
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    return;
                }

                List<PrPoUserPermissionBO> addedCostCenterList = new List<PrPoUserPermissionBO>();
                List<PrPoUserPermissionBO> editedCostCenterList = new List<PrPoUserPermissionBO>();
                List<PrPoUserPermissionBO> deletedCostCenterList = new List<PrPoUserPermissionBO>();

                PrPoUserPermissionBO upBO = new PrPoUserPermissionBO();
                PrPoUserPermissionDA upDA = new PrPoUserPermissionDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                upBO.UserInfoId = Convert.ToInt32(this.ddlUserInfoId.SelectedValue);

                int pMin = !string.IsNullOrWhiteSpace(this.txtProbableFromMinute.Text) ? Convert.ToInt32(this.txtProbableFromMinute.Text) : 0;
                int pHour = this.ddlProbableFromAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableFromHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableFromHour.Text) % 12) + 12);

                upBO.FromDate = hmUtility.GetDateTimeFromString(this.txtFromDate.Text, userInformationBO.ServerDateFormat).AddHours(pHour).AddMinutes(pMin);

                pMin = !string.IsNullOrWhiteSpace(this.txtProbableToMinute.Text) ? Convert.ToInt32(this.txtProbableToMinute.Text) : 0;
                pHour = this.ddlProbableToAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableToHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableToHour.Text) % 12) + 12);

                upBO.ToDate = hmUtility.GetDateTimeFromString(this.txtToDate.Text, userInformationBO.ServerDateFormat).AddHours(pHour).AddMinutes(pMin);

                upBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                
                int rows = gvCostCenterInfo.Rows.Count;

                for (int i = 0; i < rows; i++)
                {
                    CheckBox cb = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                    CheckBox requisition = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkRequisition");
                    CheckBox purchase = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkPurchase");
                    Label lblMappingId = (Label)gvCostCenterInfo.Rows[i].FindControl("lblMappingId");
                    Label lblCostCentreId = (Label)gvCostCenterInfo.Rows[i].FindControl("lblCostCentreId");

                    if (cb.Checked == true && Convert.ToInt32(lblMappingId.Text) == 0)
                    {
                        addedCostCenterList.Add(new PrPoUserPermissionBO()
                        {
                            EmpId = userInformationBO.UserInfoId,
                            CostCenterId = Convert.ToInt32(lblCostCentreId.Text),
                            IsPRAllow = requisition.Checked == true ? true : false,
                            IsPOAllow = purchase.Checked == true ? true : false
                        });
                    }
                    else if (cb.Checked == true && Convert.ToInt32(lblMappingId.Text) != 0)
                    {
                        editedCostCenterList.Add(new PrPoUserPermissionBO()
                        {
                            MappingId = Convert.ToInt32(lblMappingId.Text),
                            EmpId = userInformationBO.UserInfoId,
                            CostCenterId = Convert.ToInt32(lblCostCentreId.Text),
                            IsPRAllow = requisition.Checked == true ? true : false,
                            IsPOAllow = purchase.Checked == true ? true : false
                        });
                    }
                    else if (cb.Checked == false && Convert.ToInt32(lblMappingId.Text) != 0)
                    {
                        Label lbl = (Label)gvCostCenterInfo.Rows[i].FindControl("lblCostCentreId");

                        deletedCostCenterList.Add(new PrPoUserPermissionBO()
                        {
                            MappingId = Convert.ToInt32(lblMappingId.Text),
                            EmpId = userInformationBO.UserInfoId,
                            CostCenterId = Convert.ToInt32(lblCostCentreId.Text),
                            IsPRAllow = requisition.Checked == true ? true : false,
                            IsPOAllow = purchase.Checked == true ? true : false
                        });
                    }
                }

                if (addedCostCenterList.Count == 0 && string.IsNullOrWhiteSpace(hfMappingId.Value))
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Select a Cost Center.", AlertType.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(hfMappingId.Value))
                {
                    if (BearerDuplicateCheck(0, Convert.ToInt32(this.ddlUserInfoId.SelectedValue), 0, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Employee  Already Exist.", AlertType.Warning);
                        this.ddlUserInfoId.Focus();
                        return;
                    }

                    int tmpUserInfoId = 0;
                    upBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = upDA.SavePRPOUserPermissionInfo(upBO, addedCostCenterList, out tmpUserInfoId);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RestaurantCashier.ToString(), tmpUserInfoId, ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantCashier));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.Cancel();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }
                else
                {
                    if (BearerDuplicateCheck(1, Convert.ToInt32(this.ddlUserInfoId.SelectedValue), Convert.ToInt32(hfMappingId.Value), 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Employee  Already Exist.", AlertType.Warning);
                        this.ddlUserInfoId.Focus();
                        return;
                    }                    

                    upBO.MappingId = Convert.ToInt32(hfMappingId.Value);
                    upBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = upDA.UpdatePRPOUserPermissionInfo(upBO, addedCostCenterList, editedCostCenterList, deletedCostCenterList);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RestaurantCashier.ToString(), upBO.MappingId, ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantCashier));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        this.LoadGridView();
                        this.Cancel();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }
                SetTab("EntryTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void gvBearerInformation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvBearerInformation.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvBearerInformation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                //   imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //   imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvBearerInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int userInfoId = Convert.ToInt32(e.CommandArgument.ToString());
                if (e.CommandName == "CmdEdit")
                {
                    FillForm(userInfoId);
                    this.btnSave.Visible = isUpdatePermission;
                    this.btnSave.Text = "Update";
                    this.SetTab("EntryTab");
                }
                else if (e.CommandName == "CmdDelete")
                {
                    //HMCommonDA hmCommonDA = new HMCommonDA();
                    RestaurantBearerDA bearerDA = new RestaurantBearerDA();

                    //Boolean status = hmCommonDA.DeleteInfoById("RestaurantBearer", "BearerId", bearerId);
                    Boolean status = bearerDA.DeleteRestaurantBearerInfo(userInfoId, 0);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RestaurantCashier.ToString(), userInfoId, ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantCashier));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    LoadGridView();
                    this.SetTab("SearchTab");
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        //************************ User Defined Function ********************//
        private void LoadEmployee()
        {
            UserInformationDA entityDA = new UserInformationDA();
            this.ddlUserInfoId.DataSource = entityDA.GetUserInformation();
            this.ddlUserInfoId.DataTextField = "UserName";
            this.ddlUserInfoId.DataValueField = "UserInfoId";
            this.ddlUserInfoId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlUserInfoId.Items.Insert(0, item);
        }
        private void LoadCostCenter()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();
            List<CostCentreTabBO> list = new List<CostCentreTabBO>();
            list = entityDA.GetAllCostCentreTabInfo();

            this.gvCostCenterInfo.DataSource = list;
            this.gvCostCenterInfo.DataBind();
        }
        private void CheckObjectPermission()
        {
            
            btnSave.Visible = isSavePermission;
           
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (ddlUserInfoId.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, "Please select User Name.", AlertType.Warning);
                flag = false;
                ddlUserInfoId.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtFromDate.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide From Date.", AlertType.Warning);
                flag = false;
                txtFromDate.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtToDate.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide To Date.", AlertType.Warning);
                flag = false;
                txtToDate.Focus();
            }

            return flag;
        }
        private void LoadGridView()
        {
            string userName = this.txtSName.Text;
            DateTime X;
            DateTime? fromDate = DateTime.Now, toDate = DateTime.Now;

            if (DateTime.TryParse(this.txtSFromDate.Text, out X))
            {
                fromDate = hmUtility.GetDateTimeFromString(this.txtSFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else { fromDate = null; }

            if (DateTime.TryParse(this.txtSToDate.Text, out X))
            {
                toDate = hmUtility.GetDateTimeFromString(this.txtSToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else { toDate = null; }

            Boolean activeStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;
            this.CheckObjectPermission();
            PrPoUserPermissionDA bearerDA = new PrPoUserPermissionDA();
            List<PrPoUserPermissionBO> files = bearerDA.GetPRPOUserPermissionInfoBySearchCriteria(userName, fromDate, toDate, activeStat);
            this.gvBearerInformation.DataSource = files;
            this.gvBearerInformation.DataBind();
            this.SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.ddlUserInfoId.SelectedValue = "0";
            this.txtFromDate.Text = string.Empty;
            this.txtToDate.Text = string.Empty;
            this.txtUserPassword.Text = string.Empty;
            this.txtUserConfirmPassword.Text = string.Empty;

            hfMappingId.Value = "";
            this.btnSave.Text = "Save";
            this.ddlUserInfoId.Focus();

            int rows = gvCostCenterInfo.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                CheckBox cb = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                CheckBox requisition = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkRequisition");
                CheckBox purchase = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkPurchase");
                Label lblMappingId = (Label)gvCostCenterInfo.Rows[i].FindControl("lblMappingId");

                cb.Checked = false;
                requisition.Checked = false;
                purchase.Checked = false;
                lblMappingId.Text = "0";
            }
            hfUserInfoId.Value = string.Empty;
        }
        public void FillForm(int userInfoId)
        {
            List<PrPoUserPermissionBO> boList = new List<PrPoUserPermissionBO>();
            PrPoUserPermissionDA da = new PrPoUserPermissionDA();
            hfUserInfoId.Value = userInfoId.ToString();
            boList = da.GetPRPOUserPermissionByUserInfoId(userInfoId);

            ddlActiveStat.SelectedValue = (boList[0].ActiveStat == true ? 0 : 1).ToString();
            ddlUserInfoId.SelectedValue = boList[0].UserInfoId.ToString();
            txtFromDate.Text = hmUtility.GetStringFromDateTime(boList[0].FromDate);
            txtToDate.Text = hmUtility.GetStringFromDateTime(boList[0].ToDate);
            
            string hour = boList[0].FromDate.ToString("hh");
            string minute = boList[0].FromDate.ToString("mm");
            string ampm = boList[0].FromDate.ToString("tt");
            txtProbableFromHour.Text = hour;
            txtProbableFromMinute.Text = minute;
            ddlProbableFromAMPM.SelectedValue = ampm;
            hour = boList[0].ToDate.ToString("hh");
            minute = boList[0].ToDate.ToString("mm");
            ampm = boList[0].ToDate.ToString("tt");

            txtProbableToHour.Text = hour;
            txtProbableToMinute.Text = minute;
            ddlProbableToAMPM.SelectedValue = ampm;

            hfMappingId.Value = boList[0].MappingId.ToString();

            //CostCenter Load
            int rowsStockItem = gvCostCenterInfo.Rows.Count;
            for (int i = 0; i < rowsStockItem; i++)
            {
                Label lbl = (Label)gvCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
                Label lblMappingId = (Label)gvCostCenterInfo.Rows[i].FindControl("lblMappingId");

                var v = (from b in boList where b.CostCenterId == Int32.Parse(lbl.Text) select b).FirstOrDefault();

                if (v != null)
                {
                    CheckBox cb = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                    CheckBox cbRequisition = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkRequisition");
                    CheckBox cbPurchaseOrder = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkPurchase");

                    lblMappingId.Text = v.MappingId.ToString();
                    cb.Checked = true;
                    cbRequisition.Checked = v.IsPRAllow;
                    cbPurchaseOrder.Checked = v.IsPOAllow;
                }
            }
        }
        public int BearerDuplicateCheck(int isUpdate, int empId, int bearerId, int isBearer)
        {
            int IsDuplicate = 0;
            RestaurantBearerDA hmCommonDA = new RestaurantBearerDA();
            IsDuplicate = hmCommonDA.BearerDuplicateCheck(isUpdate, empId, bearerId, isBearer);
            return IsDuplicate;
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            ReturnInfo rtninf = new ReturnInfo();

            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("RestaurantBearer", "BearerId", sEmpId);
                if (status)
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                
            }

            return rtninf;
        }
    }
}
