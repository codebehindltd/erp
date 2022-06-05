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

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmFrontOfficeUser : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
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

                List<int> addedCostCenterList = new List<int>();
                List<int> editedCostCenterList = new List<int>();
                List<int> deletedCostCenterList = new List<int>();

                RestaurantBearerBO bearerBO = new RestaurantBearerBO();
                RestaurantBearerDA bearerDA = new RestaurantBearerDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                bearerBO.UserInfoId = Convert.ToInt32(this.ddlEmpId.SelectedValue);
                int pMin = !string.IsNullOrWhiteSpace(this.txtProbableFromMinute.Text) ? Convert.ToInt32(this.txtProbableFromMinute.Text) : 0;
                int pHour = this.ddlProbableFromAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableFromHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableFromHour.Text) % 12) + 12);
                bearerBO.FromDate = hmUtility.GetDateTimeFromString(this.txtFromDate.Text, userInformationBO.ServerDateFormat).AddHours(pHour).AddMinutes(pMin);
                pMin = !string.IsNullOrWhiteSpace(this.txtProbableToMinute.Text) ? Convert.ToInt32(this.txtProbableToMinute.Text) : 0;
                pHour = this.ddlProbableToAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableToHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableToHour.Text) % 12) + 12);
                bearerBO.ToDate = hmUtility.GetDateTimeFromString(this.txtToDate.Text, userInformationBO.ServerDateFormat).AddHours(pHour).AddMinutes(pMin);
                bearerBO.IsBearer = false;
                bearerBO.BearerPassword = this.txtUserPassword.Text.Trim();
                bearerBO.IsRestaurantBillCanSettle = true;
                bearerBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                bearerBO.IsItemCanEditDelete = false;

                if (!string.IsNullOrEmpty(ddlIsItemCanEditDelete.SelectedValue))
                {
                    bearerBO.IsItemCanEditDelete = ddlIsItemCanEditDelete.SelectedValue == "1" ? true : false;
                }

                List<int> costCenterIdList = new List<int>();
                List<int> deletedCostCenterIdList = new List<int>();
                int rows = gvCostCenterInfo.Rows.Count;
                for (int i = 0; i < rows; i++)
                {
                    CheckBox cb = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                    if (cb.Checked == true)
                    {
                        int costCenterId = 0;
                        Label lbl = (Label)gvCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
                        costCenterId = Convert.ToInt32(lbl.Text);
                        costCenterIdList.Add(costCenterId);
                    }
                    else
                    {
                        int costCenterId = 0;
                        Label lbl = (Label)gvCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
                        costCenterId = Convert.ToInt32(lbl.Text);
                        deletedCostCenterIdList.Add(costCenterId);
                    }
                }

                if (costCenterIdList.Count == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Select a Cost Center.", AlertType.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtBearerId.Value))
                {
                    if (BearerDuplicateCheck(0, Convert.ToInt32(this.ddlEmpId.SelectedValue), 0, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Employee  Already Exist.", AlertType.Warning);
                        this.ddlEmpId.Focus();
                        return;
                    }

                    int tmpUserInfoId = 0;
                    bearerBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = bearerDA.SaveRestaurantBearerInfo(bearerBO, costCenterIdList, out tmpUserInfoId);
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
                    if (BearerDuplicateCheck(1, Convert.ToInt32(this.ddlEmpId.SelectedValue), Convert.ToInt32(txtBearerId.Value), 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Employee  Already Exist.", AlertType.Warning);
                        this.ddlEmpId.Focus();
                        return;
                    }

                    int empId = 0;
                    if (!string.IsNullOrEmpty(hfEmpId.Value))
                        empId = Convert.ToInt32(hfEmpId.Value);
                    List<RestaurantBearerBO> boList = new List<RestaurantBearerBO>();
                    boList = bearerDA.GetRestaurantBearerInfoByUserId(empId, 0,0);

                    if (boList.Count > 0)
                    {
                        for (int i = 0; i < costCenterIdList.Count; i++)
                        {
                            int count = 0;
                            for (int j = 0; j < boList.Count; j++)
                            {
                                if (costCenterIdList[i] == boList[j].CostCenterId)
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                addedCostCenterList.Add(costCenterIdList[i]);
                            }
                            else
                            {
                                editedCostCenterList.Add(costCenterIdList[i]);
                            }
                        }

                        for (int i = 0; i < boList.Count; i++)
                        {
                            int count = 0;
                            for (int j = 0; j < deletedCostCenterIdList.Count; j++)
                            {
                                if (boList[i].CostCenterId == deletedCostCenterIdList[j])
                                {
                                    count++;
                                }
                            }
                            if (count > 0)
                            {
                                RestaurantBearerBO bo = new RestaurantBearerBO();
                                bo.BearerId = boList[i].BearerId;
                                deletedCostCenterList.Add(bo.BearerId);
                            }
                        }
                    }

                    bearerBO.BearerId = Convert.ToInt32(txtBearerId.Value);
                    bearerBO.LastModifiedBy = userInformationBO.UserInfoId;

                    addedCostCenterList = addedCostCenterList.Distinct().ToList();
                    editedCostCenterList = editedCostCenterList.Distinct().ToList();
                    deletedCostCenterList = deletedCostCenterList.Distinct().ToList();

                    Boolean status = bearerDA.UpdateRestaurantBearerInfo(bearerBO, addedCostCenterList, editedCostCenterList, deletedCostCenterList);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RestaurantCashier.ToString(), bearerBO.BearerId, ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantCashier));
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
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvBearerInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int empId = Convert.ToInt32(e.CommandArgument.ToString());
                if (e.CommandName == "CmdEdit")
                {
                    FillForm(empId);
                    this.btnSave.Text = "Update";
                    this.SetTab("EntryTab");
                }
                else if (e.CommandName == "CmdDelete")
                {
                    RestaurantBearerDA bearerDA = new RestaurantBearerDA();
                    Boolean status = bearerDA.DeleteRestaurantBearerInfo(empId, 0);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RestaurantCashier.ToString(), empId, ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantCashier));
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
            this.ddlEmpId.DataSource = entityDA.GetUserInformation();
            this.ddlEmpId.DataTextField = "UserName";
            this.ddlEmpId.DataValueField = "UserInfoId";
            this.ddlEmpId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEmpId.Items.Insert(0, item);
        }
        private void LoadCostCenter()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();
            List<CostCentreTabBO> list = new List<CostCentreTabBO>();
            list = entityDA.GetAllRestaurantTypeCostCentreTabInfo();

            this.gvCostCenterInfo.DataSource = list;
            this.gvCostCenterInfo.DataBind();
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmFrontOfficeUser.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (ddlEmpId.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, "Please select User Name.", AlertType.Warning);
                flag = false;
                ddlEmpId.Focus();
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
            string EmployeeName = this.txtSName.Text;
            DateTime X;
            DateTime? FromDate = DateTime.Now, ToDate = DateTime.Now;

            if (DateTime.TryParse(this.txtSFromDate.Text, out X))
            {
                FromDate = hmUtility.GetDateTimeFromString(this.txtSFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else { FromDate = null; }

            if (DateTime.TryParse(this.txtSToDate.Text, out X))
            {
                ToDate = hmUtility.GetDateTimeFromString(this.txtSToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else { ToDate = null; }

            Boolean ActiveStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;
            this.CheckObjectPermission();
            RestaurantBearerDA bearerDA = new RestaurantBearerDA();
            List<RestaurantBearerBO> files = bearerDA.GetRestaurantBearerInfoBySearchCriteria(0, EmployeeName, FromDate, ToDate, ActiveStat);
            this.gvBearerInformation.DataSource = files;
            this.gvBearerInformation.DataBind();
            this.SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.ddlEmpId.SelectedValue = "0";
            this.txtFromDate.Text = string.Empty;
            this.txtToDate.Text = string.Empty;
            this.txtUserPassword.Text = string.Empty;
            this.txtUserConfirmPassword.Text = string.Empty;
            txtBearerId.Value = "";
            this.btnSave.Text = "Save";
            this.ddlEmpId.Focus();

            int rows = gvCostCenterInfo.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                CheckBox cb = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                cb.Checked = false;
            }
            hfEmpId.Value = string.Empty;
        }
        public void FillForm(int empId)
        {
            List<RestaurantBearerBO> boList = new List<RestaurantBearerBO>();
            RestaurantBearerDA da = new RestaurantBearerDA();

            hfEmpId.Value = empId.ToString();
            boList = da.GetRestaurantBearerInfoByUserId(empId, 0,0);

            ddlActiveStat.SelectedValue = (boList[0].ActiveStat == true ? 0 : 1).ToString();
            ddlEmpId.SelectedValue = boList[0].UserInfoId.ToString();
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

            txtBearerId.Value = boList[0].BearerId.ToString();

            ddlIsItemCanEditDelete.SelectedValue = boList[0].IsItemCanEditDelete == true ? "1" : "0";

            //CostCenter Load
            int rowsStockItem = gvCostCenterInfo.Rows.Count;
            List<int> costCenterList = new List<int>();
            for (int i = 0; i < rowsStockItem; i++)
            {
                int costcenterId = 0;
                Label lbl = (Label)gvCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
                costcenterId = Int32.Parse(lbl.Text);
                costCenterList.Add(costcenterId);
            }

            for (int i = 0; i < costCenterList.Count; i++)
            {
                for (int j = 0; j < boList.Count; j++)
                {
                    if (costCenterList[i] == boList[j].CostCenterId)
                    {
                        CheckBox cb = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                        cb.Checked = true;
                    }
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
        //************************ Web Method Defined Function ********************//
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
