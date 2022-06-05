using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;

using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmReportConfiguration : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckObjectPermission();
                LoadReportType();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridview();
        }
        //************************ Handlers ********************//
        private void LoadGridview()
        {
            SetTab("circularSearch");
        }
        private void FillForm(Int64 jobCircularId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            PayrollJobCircularBO jobCircular = new PayrollJobCircularBO();
            JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
            jobCircular = jobCircularDa.GetJobCircularById(jobCircularId);
            SetTab("circularEntry");
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmEmpLoan.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
        }
        private void SetTab(string TabName)
        {
            if (TabName == "circularEntry")
            {
                circularEntry.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                circularSearch.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "circularSearch")
            {
                circularSearch.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                circularEntry.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void ClearForm()
        {
            btnSave.Text = "Save";
        }
        private void LoadReportType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("ReportType", hmUtility.GetDropDownFirstValue());

            fields.RemoveAt(0);

            ddlReportsType.DataSource = fields;
            ddlReportsType.DataTextField = "FieldValue";
            ddlReportsType.DataValueField = "FieldId";
            ddlReportsType.DataBind();

            ddlSearchReportType.DataSource = fields;
            ddlSearchReportType.DataTextField = "FieldValue";
            ddlSearchReportType.DataValueField = "FieldId";
            ddlSearchReportType.DataBind();

            ddlReportTypeForDetails.DataSource = fields;
            ddlReportTypeForDetails.DataTextField = "FieldValue";
            ddlReportTypeForDetails.DataValueField = "FieldId";
            ddlReportTypeForDetails.DataBind();

            ListItem itemEmployee = new ListItem();
            itemEmployee.Value = string.Empty;
            itemEmployee.Text = hmUtility.GetDropDownFirstValue();

            ddlReportsType.Items.Insert(0, itemEmployee);
            ddlSearchReportType.Items.Insert(0, itemEmployee);
            ddlReportTypeForDetails.Items.Insert(0, itemEmployee);
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ReturnInfo SaveReportConfigMaster(ReportConfigMasterBO reportConfigMaster)
        {
            ReturnInfo rtninf = new ReturnInfo();
            ReportConfigDA configDa = new ReportConfigDA();
            bool status = false;
            Int64 reportConfigId = 0;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (reportConfigMaster.Id == 0)
                {

                    reportConfigMaster.CreatedBy = userInformationBO.UserInfoId;
                    status = configDa.SaveReportConfigMaster(reportConfigMaster, out reportConfigId);

                    if (status)
                    {
                        rtninf.IsSuccess = true;

                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.CommonReportConfigMaster.ToString(), reportConfigId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CommonReportConfigMaster));
                    }
                }
                else
                {
                    reportConfigMaster.LastModifiedBy = userInformationBO.UserInfoId;
                    status = configDa.UpdateReportConfigMaster(reportConfigMaster);


                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }

                if (!status)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo SaveReportConfigDetails(List<ReportConfigDetailsEditBO> reportConfigDetails, List<ReportConfigDetailsEditBO> deletedConfigDetails)
        {
            ReturnInfo rtninf = new ReturnInfo();
            ReportConfigDA configDa = new ReportConfigDA();
            bool status = false;
            Int64 reportConfigId = 0;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                status = configDa.SaveUpdateReportConfigDetails(reportConfigDetails, deletedConfigDetails);

                if (status)
                {
                    rtninf.IsSuccess = true;

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.CommonReportConfigDetails.ToString(), reportConfigId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CommonReportConfigDetails));
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static PayrollJobCircularBO LoadCaptionGroup(int jobCircularId)
        {
            PayrollJobCircularBO jobCircular = new PayrollJobCircularBO();
            JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
            jobCircular = jobCircularDa.GetJobCircularById(jobCircularId);

            return jobCircular;
        }
        [WebMethod]
        public static List<ReportConfigMasterBO> LoadParentGroup(Int64 reportTypeId)
        {
            List<ReportConfigMasterBO> parentCaption = new List<ReportConfigMasterBO>();
            ReportConfigDA reportDa = new ReportConfigDA();
            parentCaption = reportDa.GetReportCaptionByReportTypeAndIsParentFlag(reportTypeId, true);

            return parentCaption;
        }
        [WebMethod]
        public static List<ReportConfigMasterBO> LoadReportGroup(Int64 reportTypeId)
        {
            List<ReportConfigMasterBO> parentCaption = new List<ReportConfigMasterBO>();
            ReportConfigDA reportDa = new ReportConfigDA();
            parentCaption = reportDa.GetReportCaptionByReportType(reportTypeId);

            return parentCaption;
        }
        [WebMethod]
        public static GridViewDataNPaging<ReportConfigMasterBO, GridPaging> GetReportConfigBySearchCriteria(string searchType, Int64 reportTypeId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int totalRecords = 0;

            GridViewDataNPaging<ReportConfigMasterBO, GridPaging> myGridData = new GridViewDataNPaging<ReportConfigMasterBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            ReportConfigDA reportDa = new ReportConfigDA();
            List<ReportConfigMasterBO> voucher = new List<ReportConfigMasterBO>();
            voucher = reportDa.GetReportConfigBySearch(searchType, reportTypeId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(voucher, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static List<ReportConfigDetailsEditBO> GetReportConfigDetailsByReportTypeAndConfigId(Int64 reportConfigId, Int64 reportTypeId)
        {
            List<ReportConfigDetailsEditBO> parentCaption = new List<ReportConfigDetailsEditBO>();
            ReportConfigDA reportDa = new ReportConfigDA();
            parentCaption = reportDa.GetReportConfigDetailsByReportTypeAndConfigId(reportConfigId, reportTypeId);

            return parentCaption;
        }
        [WebMethod]
        public static List<NodeMatrixBO> GetChartOfAccounts(string searchText)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByAccountNameLikeSearch(searchText);

            return nodeMatrixBOList;
        }
        [WebMethod]
        public static List<NodeMatrixBO> GetNodeMatrixInfoByAncestorNodeId(Int64 nodeId, bool isTransactionalHead)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByAncestorNodeId(nodeId);

            return nodeMatrixBOList;
        }
    }
}