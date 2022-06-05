using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpAllowanceDeduction : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadSalaryProcessMonth();
                LoadEmpAllowDeduc("AdditionalAllowance");
                LoadDummyGridData();
                LoadDepartment();
                LoadAmountType();
                LoadYearList();
            }
            CheckObjectPermission();
        }

        public void LoadAmountType()
        {
            HMCommonDA commonDa = new HMCommonDA();
            SalaryFormulaDA salaryFormulaDA = new SalaryFormulaDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            fields = commonDa.GetCustomField("AmountType"); //salaryFormulaDA.GetCustomFields("AmountType", hmUtility.GetDropDownFirstValue());

            ddlAmountType.DataSource = fields;
            ddlAmountType.DataTextField = "FieldValue";
            ddlAmountType.DataValueField = "FieldValue";
            ddlAmountType.DataBind();

            ListItem item1 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstValue();
            ddlAmountType.Items.Insert(0, item1);
        }
        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            List<SalaryProcessMonthBO> monthBOList = new List<SalaryProcessMonthBO>();
            monthBOList = entityDA.GetSalaryProcessMonth();

            ddlEffectedMonth.DataSource = monthBOList;
            ddlEffectedMonth.DataTextField = "MonthHead";
            ddlEffectedMonth.DataValueField = "MonthValue";
            ddlEffectedMonth.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEffectedMonth.Items.Insert(0, item);

            ddlSEffectedMonth.DataSource = monthBOList;
            ddlSEffectedMonth.DataTextField = "MonthHead";
            ddlSEffectedMonth.DataValueField = "MonthValue";
            ddlSEffectedMonth.DataBind();
            ddlSEffectedMonth.Items.Insert(0, item);
        }
        private void CheckObjectPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        public void LoadEmpAllowDeduc(string salaryType)
        {
            SalaryHeadDA salaryHeadDA = new SalaryHeadDA();
            List<SalaryHeadBO> fields = new List<SalaryHeadBO>();
            fields = salaryHeadDA.GetSalaryHeadInfoByType(salaryType, true);
            ddlSalaryHeadId.DataSource = fields;
            ddlSalaryHeadId.DataTextField = "SalaryHead";
            ddlSalaryHeadId.DataValueField = "SalaryHeadId";
            ddlSalaryHeadId.DataBind();

            ListItem item1 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstValue();
            ddlSalaryHeadId.Items.Insert(0, item1);
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            ddlDepartment.DataSource = entityDA.GetDepartmentInfo();
            ddlDepartment.DataTextField = "Name";
            ddlDepartment.DataValueField = "DepartmentId";
            ddlDepartment.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDepartment.Items.Insert(0, item);
        }
        private void LoadYearList()
        {
            List<string> salaryYear = new List<string>();
            salaryYear = hmUtility.GetReportYearList();

            ddlYear.DataSource = salaryYear;
            ddlYear.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlYear.Items.Insert(0, item);

            ddlSYear.DataSource = salaryYear;
            ddlSYear.DataBind();
            ddlSYear.Items.Insert(0, item);
        }

        private void LoadDummyGridData()
        {
            List<EmpAllowanceDeductionBO> allowanceDeduction = new List<EmpAllowanceDeductionBO>();

            EmpAllowanceDeductionBO obj = new EmpAllowanceDeductionBO();

            obj.EmpAllowDeductId = 0;
            obj.EmpId = 0;
            obj.SalaryHeadId = 0;
            obj.SalaryHead = string.Empty;
            obj.AllowDeductAmount = 0;
            obj.EffectFrom = DateTime.Now;
            obj.EffectTo = DateTime.Now;
            obj.Remarks = string.Empty;

            allowanceDeduction.Add(obj);

            gvEmpAllowanceDeduction.DataSource = allowanceDeduction;
            gvEmpAllowanceDeduction.DataBind();
        }

        [WebMethod]
        public static ReturnInfo PerformAllowanceDeductionSaveAction(EmpAllowanceDeductionBO salaryAddDeduction, string effectedMonthRange)
        {
            ReturnInfo rtninf = new ReturnInfo();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            EmpAllowanceDeductionDA deductionDA = new EmpAllowanceDeductionDA();

            salaryAddDeduction.EffectFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(effectedMonthRange, salaryAddDeduction.EffectiveYear.ToString()));
            salaryAddDeduction.EffectTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(effectedMonthRange, salaryAddDeduction.EffectiveYear.ToString())); 

            try
            {
                if (salaryAddDeduction.EmpAllowDeductId == 0)
                {
                    int tmpUserInfoId = 0;
                    salaryAddDeduction.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = deductionDA.SaveAllowanceDeductionInfo(salaryAddDeduction, out tmpUserInfoId);

                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpAllowDeduct.ToString(), tmpUserInfoId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAllowDeduct));
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Warning);
                    }

                }
                else
                {
                    salaryAddDeduction.LastModifiedBy = userInformationBO.UserInfoId;

                    Boolean status = deductionDA.UpdateAllowanceDeductionInfo(salaryAddDeduction);
                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpAllowDeduct.ToString(), salaryAddDeduction.EmpAllowDeductId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAllowDeduct));
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error + ", " + ex.InnerException.ToString(), AlertType.Warning);
            }

            return rtninf;
        }

        [WebMethod]
        public static ReturnInfo SaveAllowanceDeduction(EmpAllowanceDeductionBO salaryAddDeduction, List<EmpAllowanceDeductionBO> EmpLst, List<EmpAllowanceDeductionBO> EmpEditLst, List<EmpAllowanceDeductionBO> EmpDeletedLst, string effectedMonthRange)
        {
            ReturnInfo rtninf = new ReturnInfo();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            EmpAllowanceDeductionDA deductionDA = new EmpAllowanceDeductionDA();

            salaryAddDeduction.EffectFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(effectedMonthRange, salaryAddDeduction.EffectiveYear.ToString()));
            salaryAddDeduction.EffectTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(effectedMonthRange, salaryAddDeduction.EffectiveYear.ToString())); 

            try
            {
                if (salaryAddDeduction.EmpAllowDeductId == 0)
                {
                    int tmpUserInfoId = 0;
                    salaryAddDeduction.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = deductionDA.SaveAllowanceDeductionInfo(salaryAddDeduction, EmpLst, EmpEditLst, EmpDeletedLst, out tmpUserInfoId);
                    

                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpAllowDeduct.ToString(), tmpUserInfoId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAllowDeduct));
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Warning);
                    }

                }
                else
                {
                    salaryAddDeduction.LastModifiedBy = userInformationBO.UserInfoId;

                    Boolean status = deductionDA.UpdateAllowanceDeductionInfo(salaryAddDeduction);
                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpAllowDeduct.ToString(), salaryAddDeduction.EmpAllowDeductId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAllowDeduct));
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error + ", " + ex.InnerException.ToString(), AlertType.Warning);
            }

            return rtninf;
        }

        [WebMethod]
        public static GridViewDataNPaging<EmpAllowanceDeductionBO, GridPaging> LoadEmployeeAllowanceDeduction(string allowDeductType, int empId, string effectedMonth, int effectedYear, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            HMUtility hmUtility = new HMUtility();

            int totalRecords = 0;
            DateTime EffectFrom = DateTime.Now, EffectTo = DateTime.Now;

            EffectFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(effectedMonth, effectedYear.ToString()));
            EffectTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(effectedMonth, effectedYear.ToString())); 
            
            GridViewDataNPaging<EmpAllowanceDeductionBO, GridPaging> myGridData = new GridViewDataNPaging<EmpAllowanceDeductionBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            EmpAllowanceDeductionDA deductionDA = new EmpAllowanceDeductionDA();
            List<EmpAllowanceDeductionBO> dwList = deductionDA.GetEmpAllowanceDeductionForGridPaging(allowDeductType, empId, EffectFrom, EffectTo, effectedYear, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(dwList, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            string message = string.Empty;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpAllowanceDeduction", "EmpAllowDeductId", sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmpAllowDeduct.ToString(), sEmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAllowDeduct));
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

        [WebMethod]
        public static EmpAllowanceDeductionBO FillForm(int EditId)
        {
            HMUtility hmUtility = new HMUtility();

            EmpAllowanceDeductionDA deductionDA = new EmpAllowanceDeductionDA();
            EmpAllowanceDeductionBO deductionBO = deductionDA.GetEmpAllowanceDeductionInfoByID(EditId);

            deductionBO.ToDate = deductionBO.EffectTo.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            deductionBO.FromDate = deductionBO.EffectFrom.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            return deductionBO;
        }

        [WebMethod]
        public static List<EmployeeBO> LoadDepartmentalWiseEmployee(int departmentId)
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> boList = new List<EmployeeBO>();

            boList = empDa.GetEmployeeByDepartment(departmentId);

            return boList;
        }

        [WebMethod]
        public static List<SalaryHeadBO> GetSalaryHeadInfoByType(string salaryType)
        {
            SalaryHeadDA salaryHeadDA = new SalaryHeadDA();
            List<SalaryHeadBO> fields = new List<SalaryHeadBO>();
            fields = salaryHeadDA.GetSalaryHeadInfoByType(salaryType, true);

            return fields;
        }

        [WebMethod]
        public static List<EmpAllowanceDeductionBO> GetEmpAllowanceDeductionInfoByDepartmentId(int departmentId, string effectedMonth, string effectiveYear)
        {
            HMUtility hmUtility = new HMUtility();
            EmpAllowanceDeductionDA salaryHeadDA = new EmpAllowanceDeductionDA();
            List<EmpAllowanceDeductionBO> deductionBO = new List<EmpAllowanceDeductionBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            string fromSalaryDate = string.Empty;
            string toSalaryDate = string.Empty;

            DateTime EffectFrom = DateTime.Now, EffectTo = DateTime.Now;

            EffectFrom = hmUtility.GetDateTimeFromString(CommonHelper.SalaryDateFrom(effectedMonth, effectiveYear), userInformationBO.ServerDateFormat);
            EffectTo = hmUtility.GetDateTimeFromString(CommonHelper.SalaryDateTo(effectedMonth, effectiveYear), userInformationBO.ServerDateFormat);

            deductionBO = salaryHeadDA.GetEmpAllowanceDeductionInfoByDepartmentId(departmentId, EffectFrom, EffectTo, Convert.ToInt32(effectiveYear));

            return deductionBO;
        }

    }
}