using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmAppraisalMarksIndicatorSetting : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckObjectPermission();
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
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
            btnApprInd.Visible = isSavePermission;
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
        protected void btnApprIndSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsApprMarksIndFrmValid())
                {
                    return;
                }

                AppraisalMarksIndicatorBO apprIndBO = new AppraisalMarksIndicatorBO();
                AppraisalMarksIndDA apprIndDA = new AppraisalMarksIndDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                apprIndBO.AppraisalIndicatorName = this.txtApprIndName.Text;
                apprIndBO.AppraisalWeight = Convert.ToDecimal(this.txtAppraisalWeight.Text.Trim());
                apprIndBO.Remarks = this.txtRemarks.Text;

                if (string.IsNullOrWhiteSpace(txtApprIndId.Value))
                {
                    apprIndBO.CreatedBy = userInformationBO.UserInfoId;
                    apprIndBO.CreatedDate = DateTime.Now;

                    Boolean status = apprIndDA.SaveAppraisalMarksIndInfo(apprIndBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                      EntityTypeEnum.EntityType.AppraisalEvaluation.ToString(), apprIndBO.MarksIndicatorId,
                      ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                      hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AppraisalEvaluation));
                        ClearForm();
                    }
                }
                else
                {
                    apprIndBO.MarksIndicatorId = Convert.ToInt32(txtApprIndId.Value);
                    apprIndBO.LastModifiedBy = userInformationBO.UserInfoId;
                    apprIndBO.LastModifiedDate = DateTime.Now;

                    Boolean status = apprIndDA.UpdateAppraisalMarksIndInfo(apprIndBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                      EntityTypeEnum.EntityType.AppraisalEvaluation.ToString(), apprIndBO.MarksIndicatorId,
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
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "PayrollAppraisalMarksIndicator";
            string pkFieldName = "MarksIndicatorId";
            string pkFieldValue = fieldValue;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        protected void btnApprIndClear_Click(object sender, EventArgs e)
        {
            this.txtApprIndName.Text = string.Empty;
            this.txtAppraisalWeight.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;
        }
        private bool IsApprMarksIndFrmValid()
        {
            bool flag = true;
            decimal checkNumber;

            if (txtApprIndName.Text == "")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Indicator Name.", AlertType.Warning);
                flag = false;
                txtApprIndName.Focus();
            }
            else if (txtAppraisalWeight.Text == "")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Weight.", AlertType.Warning);
                flag = false;
                txtAppraisalWeight.Focus();
            }
            else if (decimal.TryParse(txtAppraisalWeight.Text, out checkNumber) == false)
            {                
                CommonHelper.AlertInfo(innboardMessage, "You have not entered valid weight.", AlertType.Warning);
                flag = false;
                txtAppraisalWeight.Focus();
            }
            else if (DuplicateCheckDynamicaly("AppraisalIndicatorName", txtApprIndName.Text, 0)== 1 && string.IsNullOrWhiteSpace(txtApprIndId.Value))
            {
                CommonHelper.AlertInfo(innboardMessage, "Name already exists.", AlertType.Warning);
                flag = false;
                txtApprIndName.Focus();
            }
            return flag;
        }
        public void FillForm(int editId)
        {
            AppraisalMarksIndicatorBO apprIndBO = new AppraisalMarksIndicatorBO();
            AppraisalMarksIndDA apprIndDA = new AppraisalMarksIndDA();
            apprIndBO = apprIndDA.GetAppraisalMarksIndInfoById(editId);

            this.txtApprIndName.Text = apprIndBO.AppraisalIndicatorName.ToString();
            this.txtAppraisalWeight.Text = apprIndBO.AppraisalWeight.ToString();
            this.txtRemarks.Text = (apprIndBO.Remarks != null ? apprIndBO.Remarks.ToString() : string.Empty);
            txtApprIndId.Value = apprIndBO.MarksIndicatorId.ToString();
            btnApprInd.Visible = isUpdatePermission;
            btnApprInd.Text = "Update";
        }
        private void ClearForm()
        {
            txtApprIndName.Text = string.Empty;
            txtAppraisalWeight.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtApprIndName.Focus();
        }

        [WebMethod]
        public static GridViewDataNPaging<AppraisalMarksIndicatorBO, GridPaging> SearchMarksIndAndLoadGridInformation(string indctrName, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<AppraisalMarksIndicatorBO, GridPaging> myGridData = new GridViewDataNPaging<AppraisalMarksIndicatorBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            AppraisalMarksIndDA apprMarksIndDA = new AppraisalMarksIndDA();
            List<AppraisalMarksIndicatorBO> apprMarksIndList = new List<AppraisalMarksIndicatorBO>();
            apprMarksIndList = apprMarksIndDA.GetAppraisalMarksIndInfoBySearchCriteriaForPagination(indctrName.Trim(), userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<AppraisalMarksIndicatorBO> distinctItems = new List<AppraisalMarksIndicatorBO>();
            distinctItems = apprMarksIndList.GroupBy(test => test.MarksIndicatorId).Select(group => group.First()).ToList();


            //myGridData.GridPagingProcessing(guestInfoList, totalRecords);
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo DeleteMarksIndById(int sEmpId)
        {
            string result = string.Empty;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                AppraisalMarksIndDA marksIndDA = new AppraisalMarksIndDA();
                Boolean status = marksIndDA.DeleteMarksIndById(sEmpId);
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
