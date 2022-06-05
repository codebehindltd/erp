using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using System.Text.RegularExpressions;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpTrainingOrganizer : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckObjectPermission();
                string editId = Request.QueryString["editId"];
                if (!string.IsNullOrWhiteSpace(editId))
                {
                    
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        btnOrganizer.Visible = isUpdatePermission;
                        FillForm(Id);
                    }
                }  
            }    
        }

        protected void btnOrganizerSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsOrganizerFrmValid())
                {
                    return;
                }                

                EmpTrainingOrganizerBO organizerBO = new EmpTrainingOrganizerBO();
                EmpTrainingOrganizerDA organizerDA = new EmpTrainingOrganizerDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                organizerBO.OrganizerName = this.txtOrganizerName.Text;
                organizerBO.Address = this.txtOrgAddress.Text;
                organizerBO.Email = this.txtEmail.Text;
                organizerBO.ContactNo = this.txtContact.Text;
                organizerBO.TrainingType = ddlTrainingType.SelectedValue;
                int tmpId;
                if (string.IsNullOrWhiteSpace(txtOrganizerId.Value))
                {
                    Boolean status = organizerDA.SaveEmpTrainingOrganizerInfo(organizerBO, out tmpId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpTrainingOrganizer.ToString(), tmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTrainingOrganizer));

                        this.Clear();
                    }
                }
                else
                {
                    organizerBO.OrganizerId = Convert.ToInt32(txtOrganizerId.Value);

                    Boolean status = organizerDA.UpdateEmpTrainingOrganizerInfo(organizerBO);
                    if (status)
                    {
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.EmpTrainingOrganizer.ToString(), organizerBO.OrganizerId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTrainingOrganizer));

                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        this.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }

        protected void btnOrganizerClear_Click(object sender, EventArgs e)
        {
            this.txtOrganizerName.Text = string.Empty;
            this.txtOrgAddress.Text = string.Empty;
            this.txtEmail.Text = string.Empty;
            this.txtContact.Text = string.Empty;
            ddlTrainingType.SelectedIndex = 0;
        }

        private bool IsOrganizerFrmValid()
        {
            bool flag = true;

            if (txtOrganizerName.Text == "")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Organizer Name.", AlertType.Warning);
                flag = false;
                txtOrganizerName.Focus();
            }
            else if (!string.IsNullOrWhiteSpace(this.txtContact.Text))
            {
                var match = Regex.Match(txtContact.Text, @"^\+?(\d[\d- ]+)?(\([\d- ]+\))?[\d- ]+\d$");
                if (match.Success)
                {

                }
                else
                {                    
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "valid contact number.", AlertType.Warning);
                    return flag = false;
                }
            }     
            return flag;
        }

        private void CheckObjectPermission()
        {
            btnOrganizer.Visible = isSavePermission;
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        public void FillForm(int editId)
        {
            EmpTrainingOrganizerBO organizerBO = new EmpTrainingOrganizerBO();
            EmpTrainingOrganizerDA organizerDA = new EmpTrainingOrganizerDA();
            organizerBO = organizerDA.GetOrganizerInfoById(editId);

            this.txtOrganizerName.Text = organizerBO.OrganizerName;
            this.txtOrgAddress.Text = organizerBO.Address;
            this.txtEmail.Text = organizerBO.Email;
            this.txtContact.Text = organizerBO.ContactNo;
            ddlTrainingType.SelectedValue = organizerBO.TrainingType;
            txtOrganizerId.Value = organizerBO.OrganizerId.ToString();
            btnOrganizer.Text = "Update";
        }

        private void Clear()
        {
            btnOrganizer.Visible = isSavePermission;
            this.txtOrganizerName.Text = string.Empty;
            this.txtOrgAddress.Text = string.Empty;
            this.txtEmail.Text = string.Empty;
            this.txtContact.Text = string.Empty;
            ddlTrainingType.SelectedIndex = 0;
            btnOrganizer.Text = "Save";

        }

        [WebMethod]
        public static GridViewDataNPaging<EmpTrainingOrganizerBO, GridPaging> SearchOrganizerAndLoadGridInformation(string organizerName, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<EmpTrainingOrganizerBO, GridPaging> myGridData = new GridViewDataNPaging<EmpTrainingOrganizerBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            EmpTrainingOrganizerDA organizerDA = new EmpTrainingOrganizerDA();
            List<EmpTrainingOrganizerBO> organizerList = new List<EmpTrainingOrganizerBO>();
            organizerList = organizerDA.GetOrganizerInfoBySearchCriteriaForPagination(organizerName, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<EmpTrainingOrganizerBO> distinctItems = new List<EmpTrainingOrganizerBO>();
            distinctItems = organizerList.GroupBy(test => test.OrganizerId).Select(group => group.First()).ToList();


            //myGridData.GridPagingProcessing(guestInfoList, totalRecords);
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo DeleteOrganizerById(int sEmpId)
        {
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            try
            {
                EmpTrainingOrganizerDA organizerDA = new EmpTrainingOrganizerDA();
                Boolean status = organizerDA.DeleteOrganizerInfoById(sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                           EntityTypeEnum.EntityType.EmpTrainingOrganizer.ToString(), sEmpId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTrainingOrganizer));
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