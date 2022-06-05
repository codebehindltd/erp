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
    public partial class frmCashierInformation : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadCostCenter();
                LoadEmployee();

                txtProbableFromHour.Text = "12";
                txtProbableFromMinute.Text = "00";
                ddlProbableFromAMPM.SelectedIndex = 1;

                txtProbableToHour.Text = "12";
                txtProbableToMinute.Text = "00";
                ddlProbableToAMPM.SelectedIndex = 1;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
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
                UserInformationBO userInformation = new UserInformationBO();

                userInformation = new UserInformationDA().GetUserInformationById(Convert.ToInt32(ddlEmpId.SelectedValue));
                bearerBO.UserInfoId = userInformation.UserInfoId;

                int pMin = !string.IsNullOrWhiteSpace(txtProbableFromMinute.Text) ? Convert.ToInt32(txtProbableFromMinute.Text) : 0;
                int pHour = ddlProbableFromAMPM.SelectedIndex == 0 ? (Convert.ToInt32(txtProbableFromHour.Text) % 12) : ((Convert.ToInt32(txtProbableFromHour.Text) % 12) + 12);
                bearerBO.FromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, userInformationBO.ServerDateFormat).AddHours(pHour).AddMinutes(pMin);
                pMin = !string.IsNullOrWhiteSpace(txtProbableToMinute.Text) ? Convert.ToInt32(txtProbableToMinute.Text) : 0;
                pHour = ddlProbableToAMPM.SelectedIndex == 0 ? (Convert.ToInt32(txtProbableToHour.Text) % 12) : ((Convert.ToInt32(txtProbableToHour.Text) % 12) + 12);
                bearerBO.ToDate = hmUtility.GetDateTimeFromString(txtToDate.Text, userInformationBO.ServerDateFormat).AddHours(pHour).AddMinutes(pMin);
                bearerBO.IsBearer = ddlIsCashierOrWaiter.SelectedIndex == 1 ? true:false ;
                bearerBO.IsChef = ddlIsCashierOrWaiter.SelectedIndex == 2 ? true : false;
                bearerBO.BearerPassword = txtUserPassword.Text.Trim();
                bearerBO.IsRestaurantBillCanSettle = ddlIsRestaurantBillCanSettle.SelectedIndex == 0 ? true : false;
                bearerBO.IsItemSearchEnable = ddlIsItemSearchEnable.SelectedIndex == 1 ? true : false;
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
                    if (BearerDuplicateCheck(0, Convert.ToInt32(ddlEmpId.SelectedValue), 0, 0) > 0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "User  Already Exist.", AlertType.Warning);
                        ddlEmpId.Focus();
                        return;
                    }

                    int tmpUserInfoId = 0;
                    bearerBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = bearerDA.SaveRestaurantBearerInfo(bearerBO, costCenterIdList, out tmpUserInfoId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RestaurantCashier.ToString(), tmpUserInfoId, ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantCashier));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Cancel();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }
                else
                {
                    if (BearerDuplicateCheck(1, Convert.ToInt32(ddlEmpId.SelectedValue), Convert.ToInt32(txtBearerId.Value), 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "User  Already Exist.", AlertType.Warning);
                        ddlEmpId.Focus();
                        return;
                    }

                    int empId = 0;
                    if (!string.IsNullOrEmpty(hfEmpId.Value))
                        empId = Convert.ToInt32(hfEmpId.Value);

                    int Isbearar = 0;
                    if (!string.IsNullOrEmpty(hfIsbearar.Value))
                        Isbearar = Convert.ToInt32(hfIsbearar.Value);

                    int IsChef = 0;
                    if (!string.IsNullOrEmpty(hfIsChef.Value))
                        IsChef = Convert.ToInt32(hfIsChef.Value);

                    List<RestaurantBearerBO> boList = new List<RestaurantBearerBO>();
                    boList = bearerDA.GetRestaurantBearerInfoByUserId(empId, Isbearar, IsChef);

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
                        LoadGridView();
                        Cancel();
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
            gvBearerInformation.PageIndex = e.NewPageIndex;
            LoadGridView();
        }
        protected void gvBearerInformation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvBearerInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {               
                if (e.CommandName != "Page")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    Label lblIsBearer = row.FindControl("lblIsBearer") as Label;

                    bool isBearar = false;
                    isBearar = lblIsBearer.Text == "True" ? true : false;

                    Label lblIsChef = row.FindControl("lblIsChef") as Label;

                    bool isChef = false;
                    isChef = lblIsChef.Text == "True" ? true : false;

                    int empId = Convert.ToInt32(e.CommandArgument.ToString());
                    if (e.CommandName == "CmdEdit")
                    {
                        FillForm(empId, isBearar, isChef);
                        btnSave.Visible = isUpdatePermission;
                        btnSave.Text = "Update";
                        SetTab("EntryTab");
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
                        SetTab("SearchTab");
                    }
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
            List<UserInformationBO> GetEmpAssignedUserInformationList = entityDA.GetEmpAssignedUserInformation();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if(userInformationBO.UserInfoId == 1)
            {
                ddlEmpId.DataSource = GetEmpAssignedUserInformationList;
            }
            else
            {
                ddlEmpId.DataSource = GetEmpAssignedUserInformationList.Where(x => x.UserInfoId != 1).ToList();
            }
            
            ddlEmpId.DataTextField = "UserIdAndUserName";
            ddlEmpId.DataValueField = "UserInfoId";
            ddlEmpId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmpId.Items.Insert(0, item);
        }
        private void LoadCostCenter()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();
            List<CostCentreTabBO> list = new List<CostCentreTabBO>();
            //list = entityDA.GetAllRestaurantTypeCostCentreTabInfo();
            list = entityDA.GetCostCentreTabInfoByType("Restaurant,RetailPos,Billing,SOBilling");

            gvCostCenterInfo.DataSource = list;
            gvCostCenterInfo.DataBind();
        }
        private void CheckObjectPermission()
        {
            
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
            else if (string.IsNullOrWhiteSpace(txtFromDate.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide From Date.", AlertType.Warning);
                flag = false;
                txtFromDate.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtToDate.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide To Date.", AlertType.Warning);
                flag = false;
                txtToDate.Focus();
            }

            return flag;
        }
        private void LoadGridView()
        {
            string EmployeeName = txtSName.Text;
            DateTime X;
            DateTime? FromDate = DateTime.Now, ToDate = DateTime.Now;

            if (DateTime.TryParse(txtSFromDate.Text, out X))
            {
                FromDate = hmUtility.GetDateTimeFromString(txtSFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else { FromDate = null; }

            if (DateTime.TryParse(txtSToDate.Text, out X))
            {
                ToDate = hmUtility.GetDateTimeFromString(txtSToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else { ToDate = null; }

            int isBearer = Convert.ToInt32(ddlSIsCashierOrBearer.SelectedValue);

            Boolean ActiveStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;
            CheckObjectPermission();
            RestaurantBearerDA bearerDA = new RestaurantBearerDA();

            List<RestaurantBearerBO> files = bearerDA.GetRestaurantBearerInfoBySearchCriteria(isBearer, EmployeeName, FromDate, ToDate, ActiveStat);

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                gvBearerInformation.DataSource = files;
            }
            else
            {
                gvBearerInformation.DataSource = files.Where(x => x.UserInfoId != 1).ToList();
            }

            gvBearerInformation.DataBind();
            SetTab("SearchTab");
        }
        private void Cancel()
        {
            ddlEmpId.SelectedValue = "0";
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            txtUserPassword.Text = string.Empty;
            txtUserConfirmPassword.Text = string.Empty;
            txtBearerId.Value = "";
            btnSave.Text = "Save";
            ddlEmpId.Focus();

            int rows = gvCostCenterInfo.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                CheckBox cb = (CheckBox)gvCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                cb.Checked = false;
            }
            hfEmpId.Value = string.Empty;
        }
        public void FillForm(int empId, bool isBearar, bool isChef)
        {
            List<RestaurantBearerBO> boList = new List<RestaurantBearerBO>();
            RestaurantBearerDA da = new RestaurantBearerDA();

            hfEmpId.Value = empId.ToString();
            hfIsbearar.Value = isBearar == true ? "1" : "0";
            hfIsChef.Value = isChef == true ? "1" : "0";

            int isBearer = isBearar == true ? 1 : 0;
            int IsChef = isChef == true ? 1 : 0;

            boList = da.GetRestaurantBearerInfoByUserId(empId, isBearer, IsChef);

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

            string restaurentUserId ;
            if(boList[0].IsBearer == true)
            {
                restaurentUserId = "1";
            }
            else if (boList[0].IsChef == true)
            {
                restaurentUserId = "2";
            }
            else
            {
                restaurentUserId = "0";
            }


            ddlIsCashierOrWaiter.SelectedValue = restaurentUserId;
            ddlIsItemCanEditDelete.SelectedValue = boList[0].IsItemCanEditDelete == true ? "1" : "0";
            ddlIsItemSearchEnable.SelectedValue = boList[0].IsItemSearchEnable == true ? "1" : "0";
            ddlIsRestaurantBillCanSettle.SelectedValue = boList[0].IsRestaurantBillCanSettle == true ? "1" : "0";

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
