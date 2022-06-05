using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmAppraisalRatingFactorSetting : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            CheckObjectPermission();
            if (!IsPostBack)
            {
                LoadMarksIndicator();

                string editId = Request.QueryString["editId"];
                if (!string.IsNullOrWhiteSpace(editId))
                {
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                    }
                }                
            }

        }

        protected void CheckObjectPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
            btnRatingFact.Visible = isSavePermission;
        }

        protected void btnRatingFactSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsApprRtngFactFrmValid())
                {
                    return;
                }

                AppraisalRatingFactorBO apprRtngBO = new AppraisalRatingFactorBO();
                AppraisalRatnFactDA apprRtngDA = new AppraisalRatnFactDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                apprRtngBO.AppraisalIndicatorId = Convert.ToInt32(this.ddlAppraisalIndicator.SelectedItem.Value);
                apprRtngBO.RatingFactorName = this.txtRatingFactorName.Text;
                apprRtngBO.RatingWeight = Convert.ToDecimal(this.txtRatingWeight.Text.Trim());
                apprRtngBO.Remarks = this.txtRemarks.Text;

                if (string.IsNullOrWhiteSpace(txtApprRatnFactId.Value))
                {
                    apprRtngBO.CreatedBy = userInformationBO.UserInfoId;
                    apprRtngBO.CreatedDate = DateTime.Now;

                    Boolean status = apprRtngDA.SaveAppraisalRtngFactInfo(apprRtngBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                     EntityTypeEnum.EntityType.AppraisalEvaluation.ToString(),0,
                     ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                     hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AppraisalEvaluation));
                        ClearForm();
                    }
                }
                else
                {
                    apprRtngBO.RatingFactorId = Convert.ToInt32(txtApprRatnFactId.Value);
                    apprRtngBO.LastModifiedBy = userInformationBO.UserInfoId;
                    apprRtngBO.LastModifiedDate = DateTime.Now;

                    Boolean status = apprRtngDA.UpdateAppraisalRtngFactInfo(apprRtngBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                    EntityTypeEnum.EntityType.AppraisalEvaluation.ToString(), apprRtngBO.RatingFactorId,
                    ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AppraisalEvaluation));
                        ClearForm();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }

        protected void btnRatingFactClear_Click(object sender, EventArgs e)
        {
            this.ddlAppraisalIndicator.SelectedIndex = 0;
            this.txtRatingFactorName.Text = string.Empty;
            this.txtRatingWeight.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;
        }

        private void LoadMarksIndicator()
        {
            AppraisalMarksIndDA apprMarksIndDA = new AppraisalMarksIndDA();
            this.ddlAppraisalIndicator.DataSource = apprMarksIndDA.GetAllAppraisalMarksIndInfo();
            this.ddlAppraisalIndicator.DataTextField = "AppraisalIndicatorName";
            this.ddlAppraisalIndicator.DataValueField = "MarksIndicatorId";
            this.ddlAppraisalIndicator.DataBind();

            ListItem item = new ListItem();
            ListItem item2 = new ListItem();
            item.Value = string.Empty;
            item2.Value = string.Empty;
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlAppraisalIndicator.Items.Insert(0, item);

            this.ddlSAppraisalIndicator.DataSource = apprMarksIndDA.GetAllAppraisalMarksIndInfo();
            this.ddlSAppraisalIndicator.DataTextField = "AppraisalIndicatorName";
            this.ddlSAppraisalIndicator.DataValueField = "MarksIndicatorId";
            this.ddlSAppraisalIndicator.DataBind();
            item2.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSAppraisalIndicator.Items.Insert(0, item2);
        }

        private bool IsApprRtngFactFrmValid()
        {
            bool flag = true;
            decimal checkNumber;

            if (txtRatingFactorName.Text == "")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Rating Factor Name.", AlertType.Warning);
                flag = false;
                txtRatingFactorName.Focus();
            }
            else if (txtRatingWeight.Text == "")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Weight.", AlertType.Warning);
                flag = false;
                txtRatingWeight.Focus();
            }
            else if (decimal.TryParse(txtRatingWeight.Text, out checkNumber) == false)
            {                
                CommonHelper.AlertInfo(innboardMessage, "You have not entered valid weight.", AlertType.Warning);
                flag = false;
                txtRatingWeight.Focus();
            }
            else if (DuplicateCheckDynamicaly("RatingFactorName", txtApprRatnFactId.Value, 0) == 1)
            {
                CommonHelper.AlertInfo(innboardMessage, "Name already exists.", AlertType.Warning);
                flag = false;
                txtRatingFactorName.Focus();
            }
            return flag;
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "PayrollAppraisalRatingFactor";
            string pkFieldName = "RatingFactorId";
            string pkFieldValue = fieldValue;
            int IsDuplicate = 0;

            if (!string.IsNullOrWhiteSpace(txtApprRatnFactId.Value))
            {
                isUpdate = 1;
            }

            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        public void FillForm(int editId)
        {
            AppraisalRatingFactorBO apprRtngBO = new AppraisalRatingFactorBO();
            AppraisalRatnFactDA apprRtngDA = new AppraisalRatnFactDA();
            apprRtngBO = apprRtngDA.GetAppraisalRtngFactInfoById(editId);

            ddlAppraisalIndicator.SelectedValue = apprRtngBO.AppraisalIndicatorId.ToString();
            txtRatingFactorName.Text = apprRtngBO.RatingFactorName.ToString();
            txtRatingWeight.Text = apprRtngBO.RatingWeight.ToString();
            txtRemarks.Text = (apprRtngBO.Remarks != null ? apprRtngBO.Remarks.ToString() : string.Empty);
            txtApprRatnFactId.Value = apprRtngBO.RatingFactorId.ToString();
            btnRatingFact.Visible = isUpdatePermission;
            btnRatingFact.Text = "Update";
        }

        private void ClearForm()
        {
            ddlAppraisalIndicator.SelectedValue = string.Empty;
            txtRatingFactorName.Text = string.Empty;
            txtRatingWeight.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            btnRatingFact.Text = "Save";
        }

        [WebMethod]
        public static GridViewDataNPaging<AppraisalRatingFactorBO, GridPaging> SearchRtngFactrAndLoadGridInformation(string marksIndId, string rtngFactName, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<AppraisalRatingFactorBO, GridPaging> myGridData = new GridViewDataNPaging<AppraisalRatingFactorBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            AppraisalRatnFactDA apprRtngFactDA = new AppraisalRatnFactDA();
            List<AppraisalRatingFactorBO> apprRtngFactList = new List<AppraisalRatingFactorBO>();
            apprRtngFactList = apprRtngFactDA.GetAppraisalRtngFactInfoBySearchCriteriaForPagination(marksIndId.Trim(), rtngFactName.Trim(), userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<AppraisalRatingFactorBO> distinctItems = new List<AppraisalRatingFactorBO>();
            distinctItems = apprRtngFactList.GroupBy(test => test.RatingFactorId).Select(group => group.First()).ToList();


            //myGridData.GridPagingProcessing(guestInfoList, totalRecords);
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo DeleteRtngFactrById(int sEmpId)
        {
            string result = string.Empty;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                AppraisalRatnFactDA rtngFactDA = new AppraisalRatnFactDA();
                Boolean status = rtngFactDA.DeleteRtngFactrById(sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                    EntityTypeEnum.EntityType.AppraisalEvaluation.ToString(), sEmpId,
                    ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AppraisalEvaluation));
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch(Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }
    }
}