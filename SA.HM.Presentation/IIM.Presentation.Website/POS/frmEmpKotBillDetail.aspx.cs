using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmEmpKotBillDetail : System.Web.UI.Page
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
                this.LoadEmployee();
                this.btnSaveFeedback.Visible = false;
                this.pnlJobAssignmentInfo.Visible = false;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsFrmValid())
                {
                    string strKotDetailIdList = string.Empty;
                    EmpKotBillDetailBO tableBO = new EmpKotBillDetailBO();
                    RestaurantTableDA tableDA = new RestaurantTableDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    int kotId = 0;

                    tableBO.BillNumber = txtBillNumber.Text;
                    tableBO.EmpId = Convert.ToInt32(ddlEmpId.SelectedValue);
                    foreach (GridViewRow row in gvItemInformation.Rows)
                    {
                        bool isSave = ((CheckBox)row.FindControl("chkIsSavePermission")).Checked;
                        if (isSave)
                        {
                            Label lblKotIdValue = (Label)row.FindControl("lblKotId");
                            kotId = Convert.ToInt32(lblKotIdValue.Text);
                            Label lblKotDetailIdValue = (Label)row.FindControl("lblKotDetailId");
                            //int KotDetailId = Convert.ToInt32(lblKotDetailIdValue.Text);
                            if (string.IsNullOrWhiteSpace(strKotDetailIdList))
                            {
                                strKotDetailIdList = lblKotDetailIdValue.Text;
                            }
                            else
                            {
                                strKotDetailIdList = strKotDetailIdList + "," + lblKotDetailIdValue.Text;
                            }
                        }
                    }

                    tableBO.KotId = kotId;
                    tableBO.KotDetailIdList = strKotDetailIdList;
                    tableBO.JobStartDate = hmUtility.GetDateTimeFromString(this.txtStartDate.Text, userInformationBO.ServerDateFormat);
                    tableBO.JobEndDate = hmUtility.GetDateTimeFromString(this.txtDeliveryDate.Text, userInformationBO.ServerDateFormat);
                    tableBO.Remarks = this.txtRemarks.Text.Trim().ToString();
                    tableBO.CreatedBy = userInformationBO.UserInfoId;

                    if (!string.IsNullOrWhiteSpace(strKotDetailIdList))
                    {
                        Boolean status = tableDA.SaveEmpKotBillDetailInfo(tableBO);
                        if (status)
                        {
                            //Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Save.ToString(), EntityTypeEnum.EntityType.EmpKotBillDetail.ToString(), tmpTableId, ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpKotBillDetail));
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            //this.LoadGridView();
                            this.Cancel();
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                        }
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please Select Item Information", AlertType.Warning);
                    }

                    this.SetTab("EntryTab");
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void gvTableNumber_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvTableNumber.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvTableNumber_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                DropDownList dpdListEstatus = (DropDownList)e.Row.FindControl("ddlJobStatus");
                if (this.ddlDeliveryStatus.SelectedValue == "Pending")
                {
                    dpdListEstatus.SelectedValue = "Pending";
                }
                else if (this.ddlDeliveryStatus.SelectedValue == "Delivered")
                {
                    dpdListEstatus.SelectedValue = "Delivered";
                }
            }
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmTableInformation.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadEmployee()
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmployeeInfoByStatus("0");
            this.ddlEmpId.DataSource = empList;
            this.ddlEmpId.DataTextField = "DisplayName";
            this.ddlEmpId.DataValueField = "EmpId";
            this.ddlEmpId.DataBind();

            this.ddlSrcEmpId.DataSource = empList;
            this.ddlSrcEmpId.DataTextField = "DisplayName";
            this.ddlSrcEmpId.DataValueField = "EmpId";
            this.ddlSrcEmpId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEmpId.Items.Insert(0, item);
            this.ddlSrcEmpId.Items.Insert(0, item);
        }
        public void LoadGridView()
        {
            if (this.ddlSrcEmpId.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select User Name", AlertType.Information);
            }
            else if (this.ddlSearchType.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select Search Type", AlertType.Information);
            }
            else
            {
                DateTime srcFromDate = DateTime.Now;
                DateTime srcToDate = DateTime.Now;
                int empId = Convert.ToInt32(this.ddlSrcEmpId.SelectedValue);
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
                {
                    srcFromDate = hmUtility.GetDateTimeFromString(this.txtSrcStartDate.Text, userInformationBO.ServerDateFormat);
                }

                if (!string.IsNullOrWhiteSpace(this.txtSrcToDate.Text))
                {
                    srcToDate = hmUtility.GetDateTimeFromString(this.txtSrcToDate.Text, userInformationBO.ServerDateFormat);
                }

                string srcType = this.ddlSearchType.SelectedValue;
                string jobStatus = this.ddlDeliveryStatus.SelectedValue;

                this.CheckObjectPermission();
                RestaurantTableDA tableDA = new RestaurantTableDA();
                List<EmpKotBillDetailBO> files = tableDA.GetEmpKotBillDetailInformation(empId, srcFromDate, srcToDate, srcType, jobStatus);

                if (files != null)
                {
                    if (files.Count > 0)
                    {
                        this.btnSaveFeedback.Visible = true;
                    }
                }

                this.gvTableNumber.DataSource = files;
                this.gvTableNumber.DataBind();
            }
            this.SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.ddlEmpId.SelectedValue = "0";
            this.txtBillNumber.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            txtDeliveryDate.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;

            this.gvItemInformation.DataSource = null;
            this.gvItemInformation.DataBind();
            this.txtBillNumber.Focus();
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (this.ddlEmpId.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select User Name.", AlertType.Warning);
                ddlEmpId.Focus();
                flag = false;
            }

            return flag;
        }
        private bool IsSrcFrmValid()
        {
            bool flag = true;
            if (this.ddlSrcEmpId.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select User Name.", AlertType.Warning);
                ddlSrcEmpId.Focus();
                flag = false;
            }

            return flag;
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
        public void FillForm(int EditId)
        {
            //RestaurantTableBO tableBO = new RestaurantTableBO();
            //RestaurantTableDA tableDA = new RestaurantTableDA();
            //tableBO = tableDA.GetRestaurantTableInfoById(0, "RestaurantTable", EditId);
            //txtRemarks.Text = tableBO.Remarks;
            //txtTableId.Value = tableBO.TableId.ToString();
            //txtTableNumber.Text = tableBO.TableNumber.ToString();
            //txtTableCapacity.Text = tableBO.TableCapacity.ToString();
        }
        protected void btnSearchBill_Click(object sender, EventArgs e)
        {
            string BillNo = txtBillNumber.Text.Trim();
            int empId = Convert.ToInt32(ddlEmpId.SelectedValue);
            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();
            KotBillDetailDA entityDA = new KotBillDetailDA();
            entityBOList = entityDA.GetKotBillDetailInfoForBillNumber(BillNo).Where(x => (x.EmpId == 0 || x.EmpId == empId) && (x.DeliveryStatus == "Pending")).ToList();
            
            if (entityBOList != null)
            {
                if (entityBOList.Count > 0)
                {
                    this.pnlJobAssignmentInfo.Visible = true;
                }
            }
            {
                CommonHelper.AlertInfo(innboardMessage, "Pending Item Not Available", AlertType.Information);
            }

            this.gvItemInformation.DataSource = entityBOList;
            this.gvItemInformation.DataBind();
        }
        protected void gvItemInformation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblEmpId");
                if (lblValue.Text != "0")
                {
                    ((CheckBox)e.Row.FindControl("chkIsSavePermission")).Checked = true;
                }
            }
        }
        protected void btnSaveFeedback_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsSrcFrmValid())
                {
                    string strKotDetailIdList = string.Empty;

                    RestaurantTableDA tableDA = new RestaurantTableDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();


                    foreach (GridViewRow row in gvTableNumber.Rows)
                    {
                        Label lblDetailIdValue = (Label)row.FindControl("lblDetailId");
                        Label lblKotDetailIdValue = (Label)row.FindControl("lblKotDetailId");
                        DropDownList ddlDeliveryStatus = row.FindControl("ddlJobStatus") as DropDownList;

                        EmpKotBillDetailBO tableBO = new EmpKotBillDetailBO();
                        tableBO.EmpId = Convert.ToInt32(ddlSrcEmpId.SelectedValue);
                        tableBO.DetailId = Convert.ToInt32(lblDetailIdValue.Text);
                        tableBO.KotDetailId = Convert.ToInt32(lblKotDetailIdValue.Text);
                        tableBO.JobStatus = ddlDeliveryStatus.SelectedValue;
                        tableBO.CreatedBy = userInformationBO.UserInfoId;

                        Boolean status = tableDA.UpdateEmpKotBillDetailInfo(tableBO);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            this.CancelFeedback();
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                        }
                    }
                    this.SetTab("SearchEntry");
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        private void CancelFeedback()
        {
            this.ddlSrcEmpId.SelectedValue = "0";
            this.txtSrcStartDate.Text = string.Empty;
            this.txtSrcToDate.Text = string.Empty;
            this.ddlSearchType.SelectedValue = "0";

            this.gvTableNumber.DataSource = null;
            this.gvTableNumber.DataBind();
        }
    }
}