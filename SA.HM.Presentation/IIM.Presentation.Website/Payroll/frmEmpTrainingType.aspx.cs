using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpTrainingType : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckPermission();
            }
        }
        protected void btnEmpTrainingTypeSave_Click(object sender, EventArgs e)
        {
            bool status = false;
            try
            {
                PayrollEmpTrainingTypeBO trainingTypeBO = new PayrollEmpTrainingTypeBO();
                EmpTrainingTypeDA trainingTypeDA = new EmpTrainingTypeDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                trainingTypeBO.TrainingName = txtTrainingName.Text;
                trainingTypeBO.Remarks = txtRemarks.Text;
                int tmpId;
                int hfId = 0;
                if (hfTrainingTypeId.Value != "")
                {
                    hfId = Convert.ToInt32(hfTrainingTypeId.Value);
                }

                if (hfId == 0)
                {
                    trainingTypeBO.CreatedBy = userInformationBO.UserInfoId;
                    status = trainingTypeDA.SaveEmpTrainingTypeInfo(trainingTypeBO, out tmpId);

                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpTrainingType.ToString(), tmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTrainingType));
                        ClearForm();
                    }
                    else
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, "Save Operation Failed.", AlertType.Error);
                    }
                }
                else
                {
                    trainingTypeBO.TrainingTypeId = hfId;
                    trainingTypeBO.LastModifiedBy = userInformationBO.UserInfoId;
                    status = trainingTypeDA.UpdateEmpTrainingTypeInfo(trainingTypeBO);

                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                             EntityTypeEnum.EntityType.EmpTrainingType.ToString(), trainingTypeBO.TrainingTypeId,
                             ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                             hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTrainingType));
                        ClearForm();
                    }
                    else
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, "Update Operation Failed.", AlertType.Error);
                    }
                }
            }
            catch(Exception ex){
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void btnEmpTrainingTypeClear_Click(object sender, EventArgs e)
        {
            txtTrainingName.Text = string.Empty;
            txtRemarks.Text = string.Empty;
        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }

        private void ClearForm()
        {
            hfTrainingTypeId.Value = string.Empty;

            txtTrainingName.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            btnEmpTrainingType.Text = "Save";
        }        
        [WebMethod]
        public static GridViewDataNPaging<PayrollEmpTrainingTypeBO, GridPaging> SearchTrainingTypeAndLoadGridInformation(string trainingTypeName, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;            

            GridViewDataNPaging<PayrollEmpTrainingTypeBO, GridPaging> myGridData = new GridViewDataNPaging<PayrollEmpTrainingTypeBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            EmpTrainingTypeDA trainingTypeDA = new EmpTrainingTypeDA();
            List<PayrollEmpTrainingTypeBO> trainingList = new List<PayrollEmpTrainingTypeBO>();
            trainingList = trainingTypeDA.GetEmpTrainingTypeInfoBySearchCriteriaForPagination(trainingTypeName, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<PayrollEmpTrainingTypeBO> distinctItems = new List<PayrollEmpTrainingTypeBO>();
            distinctItems = trainingList.GroupBy(test => test.TrainingTypeId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            string result = string.Empty;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                PayrollEmpTrainingTypeBO trainingTypeBO = new PayrollEmpTrainingTypeBO();
                EmpTrainingTypeDA trainingTypeDA = new EmpTrainingTypeDA();

                Boolean status = trainingTypeDA.DeleteEmpTrainingTypeInfo(sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmpTrainingType.ToString(), sEmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTrainingType));

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch(Exception ex){
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }
        [WebMethod]
        public static PayrollEmpTrainingTypeBO LoadDetailInformation(int trainingTypeId)
        {

            PayrollEmpTrainingTypeBO trainingTypeBO = new PayrollEmpTrainingTypeBO();
            EmpTrainingTypeDA trainingTypeDA = new EmpTrainingTypeDA();
            trainingTypeBO = trainingTypeDA.GetEmpTrainingTypeById(trainingTypeId);

            return trainingTypeBO;

        }
    }
}