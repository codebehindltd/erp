using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.SalesManagment;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmSalaryFormula : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadGrade();
                LoadSalaryHead();
                LoadAmountType();
                LoadCommonDropDownHiddenField();
                LoadDepartment();
            }
        }
        protected void gvSalaryFormula_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvSalaryFormula.PageIndex = e.NewPageIndex;
            //LoadGridView(1);
        }
        protected void gvSalaryFormula_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                // imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                // imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvSalaryFormula_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int formulaId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(formulaId);
                btnSave.Visible = isUpdatePermission;
                btnSave.Text = "Update";
                SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollSalaryFormula", "FormulaId", formulaId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.SalaryFormula.ToString(), formulaId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryFormula));
                        btnSearch_Click(null, new EventArgs());
                    }
                    //LoadGridView();
                    SetTab("SearchTab");
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SalaryFormulaBO salaryFormulaBO = new SalaryFormulaBO();

            salaryFormulaBO.TransactionType = "Grade";
            salaryFormulaBO.ActiveStat = ddlSearchActiveStat.SelectedIndex == 0 ? true : false;
            salaryFormulaBO.SalaryHeadId = Int32.Parse(ddlSearchSelaryHead.SelectedValue);
            salaryFormulaBO.GradeIdOrEmployeeId = Convert.ToInt32(ddlSearchGrade.SelectedValue);

            LoadGridView(salaryFormulaBO);

        }
        protected void btnSrcEmployee_Click(object sender, EventArgs e)
        {
            LoadEmployeeInformation();
            SetTab("EntryTab");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    return;
                }

                SalaryFormulaBO salaryFormulaBO = new SalaryFormulaBO();
                SalaryFormulaDA salaryFormulaDA = new SalaryFormulaDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                salaryFormulaBO.TransactionType = ddlTransactionType.SelectedValue;
                salaryFormulaBO.GradeIdOrEmployeeId = Int32.Parse(ddlGradeId.SelectedValue);

                salaryFormulaBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                salaryFormulaBO.Amount = Convert.ToDecimal(txtAmount.Text.Trim());
                salaryFormulaBO.AmountType = ddlAmountType.SelectedValue;
                salaryFormulaBO.DependsOn = Int32.Parse(ddlDependsOn.SelectedValue);
                salaryFormulaBO.SalaryHeadId = Int32.Parse(ddlSalaryHeadId.SelectedValue);
                string salarySheetSpecialNotes = string.Empty;

                if (string.IsNullOrWhiteSpace(txtFormulaId.Value))
                {
                    int tmpUserInfoId = 0;
                    salaryFormulaBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = salaryFormulaDA.SaveSalaryFormulaInfo(salaryFormulaBO, salarySheetSpecialNotes, out tmpUserInfoId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.SalaryFormula.ToString(), tmpUserInfoId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryFormula));
                        Cancel();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, "You Can't Save.This Salary Formula Already Inserted.", AlertType.Warning);
                        Cancel();
                    }

                }
                else
                {
                    salaryFormulaBO.FormulaId = Convert.ToInt32(txtFormulaId.Value);
                    salaryFormulaBO.LastModifiedBy = userInformationBO.UserInfoId;

                    Boolean status = salaryFormulaDA.UpdateSalaryFormulaInfo(salaryFormulaBO, salarySheetSpecialNotes);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.SalaryFormula.ToString(), salaryFormulaBO.FormulaId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryFormula));
                        Cancel();

                        txtFormulaId.Value = "";
                    }
                }
                SetTab("EntryTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, CommonHelper.ExceptionErrorMessage(ex), AlertType.Error);

            }
        }
        //************************ User Defined Function ********************//        
        private void CheckObjectPermission()
        {
            Button1.Visible = isSavePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            ddlDepartmentId.DataTextField = "Name";
            ddlDepartmentId.DataValueField = "DepartmentId";
            ddlDepartmentId.DataBind();

            ddlDepartmentId.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Text = hmUtility.GetDropDownFirstValue(), Value = "0" });
        }
        private void Cancel()
        {
            ddlGradeId.SelectedIndex = 0;
            ddlSalaryHeadId.SelectedIndex = 0;
            ddlDependsOn.SelectedIndex = 0;
            ddlActiveStat.SelectedIndex = 0;
            txtAmount.Text = string.Empty;
            btnSave.Text = "Save";
            txtEmployeeId.Value = "";
            selectedDependsOn.Value = "";
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (ddlSalaryHeadId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Salary Head.", AlertType.Warning);
                flag = false;
                ddlSalaryHeadId.Focus();
            }
            else if (txtIsIndividual.Value == "Individual")
            {
                if (txtEmployeeId.Value == "")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "A Employee.", AlertType.Warning);
                    flag = false;
                }
            }
            else if (ddlGradeId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Grade.", AlertType.Warning);
                flag = false;
                ddlGradeId.Focus();
            }
            //else if (ddlDependsOn.SelectedIndex == 0)
            //{
            //    isMessageBoxEnable = 1;
            //    lblMessage.Text = "Please Provide Valid Input ";
            //    flag = false;
            //}
            else if (string.IsNullOrWhiteSpace(txtAmount.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Amount.", AlertType.Warning);
                flag = false;
                txtAmount.Focus();
            }
            return flag;
        }
        public void LoadGrade()
        {
            SalaryFormulaDA salaryFormulaDA = new SalaryFormulaDA();
            List<EmpGradeBO> fields = new List<EmpGradeBO>();
            fields = salaryFormulaDA.GetAllGrade();
            fields = fields.Where(i => i.ActiveStat == true).ToList();
            ddlGradeId.DataSource = fields;
            ddlGradeId.DataTextField = "Name";
            ddlGradeId.DataValueField = "GradeId";
            ddlGradeId.DataBind();

            ddlSearchGrade.DataSource = fields;
            ddlSearchGrade.DataTextField = "Name";
            ddlSearchGrade.DataValueField = "GradeId";
            ddlSearchGrade.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlGradeId.Items.Insert(0, itemNodeId);

            ddlSearchGrade.Items.Insert(0, itemNodeId);
        }
        public void LoadSalaryHead()
        {
            SalaryHeadDA salaryHeadDA = new SalaryHeadDA();
            List<SalaryHeadBO> fields = new List<SalaryHeadBO>();
            fields = salaryHeadDA.GetSalaryHeadInfoByCategory(null).Where(x => x.ContributionType != "Both").ToList();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            List<HMCommonSetupBO> salarySettingHeadId = new List<HMCommonSetupBO>();

            HMCommonSetupBO salryHeadSetup = new HMCommonSetupBO();
            salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("SalaryProcessDependsOn", "SalaryProcessDependsOn");

            if (salryHeadSetup.SetupValue.ToString() == "Basic")
            {
                salryHeadSetup = new HMCommonSetupBO();
                salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("Basic Salary Head", "Basic Salary Setup");
                salarySettingHeadId.Add(salryHeadSetup);
            }

            salryHeadSetup = new HMCommonSetupBO();
            salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("PayrollAttendanceAbsentHeadId", "PayrollAttendanceAbsentHeadId");
            salarySettingHeadId.Add(salryHeadSetup);

            salryHeadSetup = new HMCommonSetupBO();
            salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("PayrollPFEmployeeContributionId", "PayrollPFEmployeeContributionId");
            salarySettingHeadId.Add(salryHeadSetup);

            salryHeadSetup = new HMCommonSetupBO();
            salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("PayrollPFCompanyContributionId", "PayrollPFCompanyContributionId");
            salarySettingHeadId.Add(salryHeadSetup);

            salryHeadSetup = new HMCommonSetupBO();
            salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("Gross Salary Head", "Gross Salary Setup");
            salarySettingHeadId.Add(salryHeadSetup);

            fields = (from d in fields
                      where !(salarySettingHeadId.Select(x => x.SetupValue.ToString()).Contains(d.SalaryHeadId.ToString()))
                      select new SalaryHeadBO
                      {
                          SalaryHeadId = d.SalaryHeadId,
                          SalaryHead = d.SalaryHead + "  (" + d.SalaryType + ")",
                          IsShowOnlyAllownaceDeductionPage = d.IsShowOnlyAllownaceDeductionPage,
                          SalaryType = d.SalaryType,
                          TransactionType = d.TransactionType,
                          ActiveStat = d.ActiveStat,
                          ActiveStatus = d.ActiveStatus

                      }).ToList();

            ddlSalaryHeadId.DataSource = fields;
            ddlSalaryHeadId.DataTextField = "SalaryHead";
            ddlSalaryHeadId.DataValueField = "SalaryHeadId";
            ddlSalaryHeadId.DataBind();

            ListItem item1 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstValue();
            ddlSalaryHeadId.Items.Insert(0, item1);

            ListItem all = new ListItem();
            all.Value = "0";
            all.Text = "--- All ---";

            ddlSearchSelaryHead.DataSource = fields;
            ddlSearchSelaryHead.DataTextField = "SalaryHead";
            ddlSearchSelaryHead.DataValueField = "SalaryHeadId";
            ddlSearchSelaryHead.DataBind();
            ddlSearchSelaryHead.Items.Insert(0, all);

            ddlDependsOn.DataSource = fields;
            ddlDependsOn.DataTextField = "SalaryHead";
            ddlDependsOn.DataValueField = "SalaryHeadId";
            ddlDependsOn.DataBind();

            ListItem item2 = new ListItem();
            item2.Value = "0";
            item2.Text = hmUtility.GetDropDownFirstValue();
            ddlDependsOn.Items.Insert(0, item2);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        public void LoadAmountType()
        {
            HMCommonDA commonDa = new HMCommonDA();
            SalaryFormulaDA salaryFormulaDA = new SalaryFormulaDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            fields = commonDa.GetCustomField("AmountType").Where(s => s.FieldValue != HMConstants.AmountType.Days.ToString()).ToList(); //salaryFormulaDA.GetCustomFields("AmountType", hmUtility.GetDropDownFirstValue());

            ddlAmountType.DataSource = fields;
            ddlAmountType.DataTextField = "FieldValue";
            ddlAmountType.DataValueField = "FieldValue";
            ddlAmountType.DataBind();

            ListItem item1 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstValue();
            ddlAmountType.Items.Insert(0, item1);
        }
        private void LoadEmployeeInformation()
        {
            //string EmployeeCode = txtSrcEmployee.Text;
            EmployeeDA employeeDA = new EmployeeDA();
            var employee = employeeDA.GetEmployeeInfoByCode("");
            if (employee != null)
            {
                txtEmployeeId.Value = employee.EmpId.ToString();
                //txtEmployeeName.Text = employee.EmployeeName;
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Employee not found for this Emplyee Code.", AlertType.Warning);
                //txtSrcEmployee.Focus();
            }
        }
        public void LoadGridView(SalaryFormulaBO salaryFormula)
        {
            CheckObjectPermission();
            SalaryFormulaBO salaryFormulaBO = new SalaryFormulaBO();
            SalaryFormulaDA salaryFormulaDA = new SalaryFormulaDA();
            List<SalaryFormulaBO> files = new List<SalaryFormulaBO>();
            files = salaryFormulaDA.GetSalaryFormulaInfoBySearchCritaria(salaryFormula);
            gvSalaryFormula.DataSource = files;
            gvSalaryFormula.DataBind();
            SetTab("SearchTab");

        }
        public void FillForm(int EditId)
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            //commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Salary Process System", "Salary Process System Setup");
            //if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            //{
            //    txtIsIndividual.Value = commonSetupBO.SetupValue;
            //}

            SalaryFormulaBO salaryFormulaBO = new SalaryFormulaBO();
            SalaryFormulaDA salaryFormulaDA = new SalaryFormulaDA();
            salaryFormulaBO = salaryFormulaDA.getSalaryFormulaInfoById(EditId);
            ddlTransactionType.SelectedValue = salaryFormulaBO.TransactionType;
            txtIsIndividual.Value = salaryFormulaBO.TransactionType;
            txtAmount.Text = salaryFormulaBO.Amount.ToString();
            ddlActiveStat.SelectedIndex = salaryFormulaBO.ActiveStat == true ? 0 : 1;
            ddlAmountType.SelectedValue = salaryFormulaBO.AmountType;

            if (!string.IsNullOrEmpty(salaryFormulaBO.EmployeeCode.ToString()))
            {
                //txtSrcEmployee.Text = salaryFormulaBO.EmployeeCode.ToString();
            }
            if (!string.IsNullOrWhiteSpace(txtIsIndividual.Value))
            {
                if (txtIsIndividual.Value == "Individual")
                {
                    EmployeeDA employeeDA = new EmployeeDA();
                    var employee = employeeDA.GetEmployeeInfoById(salaryFormulaBO.GradeIdOrEmployeeId);
                    txtEmployeeId.Value = salaryFormulaBO.GradeIdOrEmployeeId.ToString();
                    //txtEmployeeName.Text = employee.EmployeeName;
                }
                else
                {
                    int maxGradeId = 0;
                    EmpGradeDA gradeDA = new EmpGradeDA();
                    var List = gradeDA.GetGradeInfo();

                    if (List != null)
                    {
                        maxGradeId = List.Max(m => m.GradeId);
                    }

                    if (maxGradeId > salaryFormulaBO.GradeIdOrEmployeeId)
                    {
                        ddlGradeId.SelectedValue = salaryFormulaBO.GradeIdOrEmployeeId.ToString();
                    }
                }
                if (ddlSalaryHeadId.Items.FindByValue(salaryFormulaBO.SalaryHeadId.ToString()) != null)
                    ddlSalaryHeadId.SelectedValue = salaryFormulaBO.SalaryHeadId.ToString();

                ddlDependsOn.DataSource = GetDependsOnHeadList(salaryFormulaBO.SalaryHeadId.ToString());
                ddlDependsOn.DataTextField = "ItemName";
                ddlDependsOn.DataValueField = "ItemId";
                ddlDependsOn.DataBind();

                ListItem item2 = new ListItem();
                item2.Value = "0";
                item2.Text = hmUtility.GetDropDownFirstValue();
                ddlDependsOn.Items.Insert(0, item2);

                ddlDependsOn.SelectedValue = salaryFormulaBO.DependsOn.ToString();
                selectedDependsOn.Value = salaryFormulaBO.DependsOn.ToString();

                txtFormulaId.Value = salaryFormulaBO.FormulaId.ToString();
                btnSave.Text = "Update";
            }
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
        private static string GridProcess()
        {
            int rowCounter = 0;
            string grid = string.Empty, alternateColor = string.Empty, dependsOn = string.Empty, amountType = string.Empty;

            dependsOn = DependsOnSalaryHeadOptionProcess();
            amountType = AmountTypeOptionProcess();

            SalaryHeadDA salaryHeadDA = new SalaryHeadDA();
            List<SalaryHeadBO> salaryhed = new List<SalaryHeadBO>();
            salaryhed = salaryHeadDA.GetSalaryHeadInfoByCategory(null).Where(x => x.ContributionType != "Both").ToList();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            List<HMCommonSetupBO> salarySettingHeadId = new List<HMCommonSetupBO>();

            HMCommonSetupBO salryHeadSetup = new HMCommonSetupBO();
            salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("SalaryProcessDependsOn", "SalaryProcessDependsOn");

            if (salryHeadSetup.SetupValue.ToString() == "Basic")
            {
                salryHeadSetup = new HMCommonSetupBO();
                salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("Basic Salary Head", "Basic Salary Setup");
                salarySettingHeadId.Add(salryHeadSetup);
            }

            salryHeadSetup = new HMCommonSetupBO();
            salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("PayrollAttendanceAbsentHeadId", "PayrollAttendanceAbsentHeadId");
            salarySettingHeadId.Add(salryHeadSetup);

            salryHeadSetup = new HMCommonSetupBO();
            salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("PayrollPFEmployeeContributionId", "PayrollPFEmployeeContributionId");
            salarySettingHeadId.Add(salryHeadSetup);

            salryHeadSetup = new HMCommonSetupBO();
            salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("PayrollPFCompanyContributionId", "PayrollPFCompanyContributionId");
            salarySettingHeadId.Add(salryHeadSetup);

            salryHeadSetup = new HMCommonSetupBO();
            salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("Gross Salary Head", "Gross Salary Setup");
            salarySettingHeadId.Add(salryHeadSetup);

            salaryhed = (from d in salaryhed
                         where !(salarySettingHeadId.Select(x => x.SetupValue.ToString()).Contains(d.SalaryHeadId.ToString()))
                         select new SalaryHeadBO
                         {
                             SalaryHeadId = d.SalaryHeadId,
                             SalaryHead = d.SalaryHead,
                             IsShowOnlyAllownaceDeductionPage = d.IsShowOnlyAllownaceDeductionPage,
                             SalaryType = d.SalaryType,
                             TransactionType = d.TransactionType,
                             ActiveStat = d.ActiveStat,
                             ActiveStatus = d.ActiveStatus

                         }).ToList();

            grid = "<table id='EmployeeWiseSalaryFormulatbl' style=\"width: 100%;\" class=\"table table-bordered table-condensed table-responsive\">" +
                        "<thead>" +
                            "<tr style=\"color: White; background-color: #44545E; font-weight: bold; text-align =left; \">" +
                                "<th style=\"width: 8%;\">" +
                                    "Select" +
                                "</th>" +
                                "<th style=\"width: 25%;\">" +
                                    "Salary Head" +
                                "</th>" +
                                "<th style=\"width: 15%;\">" +
                                    "Type" +
                                "</th>" +
                                 "<th style=\"width: 20%;\">" +
                                    "Amount Type" +
                                "</th>" +
                                "<th style=\"width: 20%;\">" +
                                    "Depends" +
                                "</th>" +
                                "<th style=\"width: 12%;\">" +
                                    "Amount" +
                                "</th>" +
                            "</tr>" +
                        "</thead>" +
                        "<tbody>";

            foreach (SalaryHeadBO sh in salaryhed)
            {
                rowCounter++;
                if (rowCounter % 2 == 0)
                {
                    alternateColor = "style=\"background-color:#E3EAEB;\"";
                }
                else
                    alternateColor = "style=\"background-color:#FFFFFF;\"";

                grid += "<tr " + alternateColor + ">" +
                             "<td id='frmula" + sh.SalaryHeadId + "' style=\"display:none;\">0</td>" +
                             "<td style=\"display:none;\">" + sh.SalaryHeadId.ToString() + "</td>" +
                            "<td style=\"width: 8%;\">" +
                               "<input type='checkbox' id='chk" + sh.SalaryHeadId.ToString() + "' />" +
                            "</td>" +
                            "<td style=\"width: 25%;\">" +
                                sh.SalaryHead +
                            "</td>" +
                            "<td style=\"width: 15%;\">" +
                                sh.SalaryType +
                            "</td>" +
                             "<td style=\"width: 20%;\">" +
                                amountType.Replace("####", "amnttyp" + sh.SalaryHeadId.ToString()) +
                            "</td>" +
                            "<td style=\"width: 20%;\">" +
                                dependsOn.Replace("####", "depnd" + sh.SalaryHeadId.ToString()) +
                            "</td>" +
                            "<td style=\"width: 12%;\">" +
                                "<input type='text' style='width:60px;' class='form-control'  onblur='CheckInputValidation(this)' id='amnt" + sh.SalaryHeadId + "' />" +
                            "</td>" +
                            "<td id='amtdb" + sh.SalaryHeadId + "' style=\"display:none;\">0</td>" +
                            "<td id='depndb" + sh.SalaryHeadId + "' style=\"display:none;\">0</td>" +
                            "<td id='amntdb" + sh.SalaryHeadId + "' style=\"display:none;\">0</td>" +
                        "</tr>";
            }
            grid += " </tbody> </table>";

            return grid;
        }
        private static string AmountTypeOptionProcess()
        {
            string options = string.Empty;

            options = "<select id='####' class='form-control' style='width:150px;'>" +
                      "<option value='0'>--- Please Select ---</option> " +
                      "<option value='Percentage'>Percentage</option> " +
                      "<option value='Fixed'>Fixed</option> " +
                      "</select> ";

            return options;
        }
        private static string DependsOnSalaryHeadOptionProcess()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Basic Salary Head", "Basic Salary Setup");

            HMCommonSetupBO commonSetupDirectSalaryProcessDependsOn = new HMCommonSetupBO();
            commonSetupDirectSalaryProcessDependsOn = commonSetupDA.GetCommonConfigurationInfo("SalaryProcessDependsOn", "SalaryProcessDependsOn");

            if ((commonSetupDirectSalaryProcessDependsOn.SetupValue).ToString() != HMConstants.SalaryProcessDependsOn.Gross.ToString())
            {
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Basic Salary Head", "Basic Salary Setup");
            }
            else if ((commonSetupDirectSalaryProcessDependsOn.SetupValue).ToString() == HMConstants.SalaryProcessDependsOn.Gross.ToString())
            {
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Gross Salary Head", "Gross Salary Setup");
            }

            SalaryHeadDA salaryHeadDA = new SalaryHeadDA();
            List<SalaryHeadBO> salaryHead = new List<SalaryHeadBO>();

            SalaryHeadBO bo = new SalaryHeadBO();
            bo = salaryHeadDA.GetSalaryHeadInfoById(Int32.Parse(commonSetupBO.SetupValue));
            salaryHead.Add(bo);

            string options = string.Empty;

            options = "<select id='####' class='form-control' style='width:150px;'>";
            options += "<option value='0'>--- Please Selecct ---</option>";

            foreach (SalaryHeadBO sh in salaryHead)
            {
                options += "<option value=\"" + sh.SalaryHeadId + "\">" + sh.SalaryHead + "</option>";
            }

            options += " </select>";

            return options;
        }
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static List<ItemViewBO> GetDependsOnHeadList(string salaryHead)
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO commonSetupDirectSalaryProcessDependsOn = new HMCommonSetupBO();
            commonSetupDirectSalaryProcessDependsOn = commonSetupDA.GetCommonConfigurationInfo("SalaryProcessDependsOn", "SalaryProcessDependsOn");

            if ((commonSetupDirectSalaryProcessDependsOn.SetupValue).ToString() != HMConstants.SalaryProcessDependsOn.Gross.ToString())
            {
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Basic Salary Head", "Basic Salary Setup");
            }
            else if ((commonSetupDirectSalaryProcessDependsOn.SetupValue).ToString() == HMConstants.SalaryProcessDependsOn.Gross.ToString())
            {
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Gross Salary Head", "Gross Salary Setup");
            }

            SalaryHeadDA salaryHeadDA = new SalaryHeadDA();
            List<SalaryHeadBO> fields = new List<SalaryHeadBO>();
            //fields = salaryHeadDA.GetSalaryHeadInfoByCategory("Group");
            List<ItemViewBO> List = new List<ItemViewBO>();

            if (Int32.Parse(salaryHead) != 0)
            {
                var entity = salaryHeadDA.GetSalaryHeadInfoById(Int32.Parse(commonSetupBO.SetupValue));
                if (salaryHead == commonSetupBO.SetupValue)
                {
                    ItemViewBO itemViewBO = new ItemViewBO();
                    itemViewBO.ItemId = entity.SalaryHeadId;
                    itemViewBO.ItemName = entity.SalaryHead;
                    List.Add(itemViewBO);
                }
                else
                {
                    ItemViewBO itemViewBO = new ItemViewBO();
                    itemViewBO.ItemId = entity.SalaryHeadId;
                    itemViewBO.ItemName = entity.SalaryHead;
                    List.Add(itemViewBO);
                }
            }
            return List;
        }
        [WebMethod]
        public static string EmployeeWiseSalaryFormulaHead()
        {
            string grid = string.Empty;

            try
            {
                grid = GridProcess();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return grid;
        }
        [WebMethod]
        public static SalaryFormulaViewBO GetEmployeeWiseSalaryFormulaHead(int employeeId)
        {
            string grid = string.Empty;
            SalaryFormulaDA formulaDa = new SalaryFormulaDA();
            SalaryFormulaViewBO salaryFormula = new SalaryFormulaViewBO();

            try
            {
                salaryFormula.EmployeeIndividualWise = formulaDa.GetSalaryFormulaInfoByEmpId(employeeId);
                salaryFormula.EmployeeGradeWise = formulaDa.GetSalaryFormulaByEmpGradeId(employeeId);

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                List<HMCommonSetupBO> salarySettingHeadId = new List<HMCommonSetupBO>();

                HMCommonSetupBO salryHeadSetup = new HMCommonSetupBO();
                salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("SalaryProcessDependsOn", "SalaryProcessDependsOn");

                if (salryHeadSetup.SetupValue.ToString() == "Basic")
                {
                    salryHeadSetup = new HMCommonSetupBO();
                    salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("Basic Salary Head", "Basic Salary Setup");
                    salarySettingHeadId.Add(salryHeadSetup);
                }

                salryHeadSetup = new HMCommonSetupBO();
                salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("PayrollPFEmployeeContributionId", "PayrollPFEmployeeContributionId");
                salarySettingHeadId.Add(salryHeadSetup);

                salryHeadSetup = new HMCommonSetupBO();
                salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("PayrollPFCompanyContributionId", "PayrollPFCompanyContributionId");
                salarySettingHeadId.Add(salryHeadSetup);

                salryHeadSetup = new HMCommonSetupBO();
                salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("Gross Salary Head", "Gross Salary Setup");
                salarySettingHeadId.Add(salryHeadSetup);

                salaryFormula.EmployeeIndividualWise = (from d in salaryFormula.EmployeeIndividualWise
                                                        where !(salarySettingHeadId.Select(x => x.SetupValue.ToString()).Contains(d.SalaryHeadId.ToString()))
                                                        select new SalaryFormulaBO
                                                        {
                                                            FormulaId = d.FormulaId,
                                                            TransactionType = d.TransactionType,
                                                            GradeIdOrEmployeeId = d.GradeIdOrEmployeeId,
                                                            EmplyeeName = d.EmplyeeName,
                                                            EmployeeCode = d.EmployeeCode,
                                                            Grade = d.Grade,
                                                            SalaryHeadId = d.SalaryHeadId,
                                                            SalaryHead = d.SalaryHead,
                                                            DependsOn = d.DependsOn,
                                                            DependsOnHead = d.DependsOnHead,
                                                            Amount = d.Amount,
                                                            AmountType = d.AmountType,
                                                            ActiveStat = d.ActiveStat,
                                                            ActiveStatus = d.ActiveStatus

                                                        }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return salaryFormula;
        }
        [WebMethod]
        public static ReturnInfo SaveEmployeeWiseSalaryFormula(List<SalaryFormulaBO> salaryFormula, List<SalaryFormulaBO> salaryFormulaEdited, List<SalaryFormulaBO> salaryFormulaDeleted, string salarySheetSpecialNotes)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                int tmpId;

                SalaryFormulaDA salaryDa = new SalaryFormulaDA();

                if (salaryFormula.Count > 0)
                {
                    status = salaryDa.SaveSalaryFormulaInfo(salaryFormula, userInformationBO.UserInfoId, salarySheetSpecialNotes, out tmpId);
                    if (status == true)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                EntityTypeEnum.EntityType.SalaryFormula.ToString(), tmpId,
                                ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryFormula));
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }

                }


                if (salaryFormulaEdited.Count > 0)
                {
                    status = salaryDa.UpdateSalaryFormulaInfo(salaryFormulaEdited, userInformationBO.UserInfoId, salarySheetSpecialNotes);
                    if (status == true)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                EntityTypeEnum.EntityType.SalaryFormula.ToString(), 0,
                                ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                                "Salary Formula Updated for Employee");
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }


                if (salaryFormulaDeleted.Count > 0)
                {
                    status = salaryDa.DeleteSalaryFormulaInfo(salaryFormulaDeleted);
                    if (status == true)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                                EntityTypeEnum.EntityType.SalaryFormula.ToString(), 0,
                                ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                                "Salary Formula Deleted for Employee");
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }



            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error + ", " + ex.InnerException.ToString(), AlertType.Error);

            }

            return rtninf;
        }
        [WebMethod]
        public static string GetGradeWiseBasicSalary()
        {
            string grid = string.Empty, alternateColor = "";
            int rowCounter = 0;

            EmpGradeDA gradeDa = new EmpGradeDA();
            List<EmpGradeBO> gradeList = new List<EmpGradeBO>();

            gradeList = gradeDa.GetActiveGradeInfo();

            grid = "<table id='GradeWiseSalaryTbl' style=\"width: 100%;\" class=\"table table-bordered table-condensed table-responsive\">" +
                        "<thead>" +
                            "<tr style=\"color: White; background-color: #44545E; font-weight: bold; text-align =left; \">" +
                                "<th style=\"width: 80%;\">" +
                                    "Grade" +
                                "</th>" +
                                "<th style=\"width: 20%;\">" +
                                    "Amount" +
                                "</th>" +
                            "</tr>" +
                        "</thead>" +
                        "<tbody>";

            foreach (EmpGradeBO sh in gradeList)
            {
                rowCounter++;
                if (rowCounter % 2 == 0)
                {
                    alternateColor = "style=\"background-color:#E3EAEB;\"";
                }
                else
                    alternateColor = "style=\"background-color:#FFFFFF;\"";

                grid += "<tr " + alternateColor + ">" +
                             "<td id='grade" + sh.GradeId + "' style=\"display:none;\">" + sh.GradeId + "</td>" +
                            "<td style=\"width: 80%;\">" +
                                sh.Name +
                            "</td>" +
                            "<td style=\"width: 20%;\">" +
                                "<input type='text' style='width:187px;' class='form-control'  onblur='CheckInputValidation(this)' id='amnt" + sh.GradeId + "' value=" + Convert.ToDecimal(sh.BasicAmount).ToString("0.00") + " />" +
                            "</td>" +
                            "<td id='amtdb" + sh.GradeId + "' style=\"display:none;\">0</td>" +
                        "</tr>";
            }
            grid += " </tbody> </table>";

            return grid;

        }
        [WebMethod]
        public static string GetEmployeeBasicSalary(int departmentId)
        {
            string grid = string.Empty, alternateColor = "";
            int rowCounter = 0;

            EmployeeDA gradeDa = new EmployeeDA();
            List<EmployeeBO> employee = new List<EmployeeBO>();

            employee = gradeDa.GetEmployeeByDepartment(departmentId);
            employee = employee.Where(i => i.EmployeeStatus != "Inactive").ToList();
            employee.Sort((emp1, emp2) => emp1.EmpCode.CompareTo(emp2.EmpCode));

            grid = "<table id='EmployeeWiseSalaryBasicTbl' style=\"width: 100%;\" class=\"table table-bordered table-condensed table-responsive\">" +
                        "<thead>" +
                            "<tr style=\"color: White; background-color: #44545E; font-weight: bold; text-align =left; \">" +
                                "<th style=\"width: 20%;\">" +
                                    "Employee Code" +
                                "</th>" +
                                "<th style=\"width: 40%;\">" +
                                    "Employee" +
                                "</th>" +
                                "<th style=\"width: 20%;\">" +
                                    "Payroll Currency" +
                                "</th>" +
                                "<th style=\"width: 20%;\">" +
                                    "Amount" +
                                "</th>" +
                            "</tr>" +
                        "</thead>" +
                        "<tbody>";

            foreach (EmployeeBO sh in employee)
            {
                rowCounter++;
                if (rowCounter % 2 == 0)
                {
                    alternateColor = "style=\"background-color:#E3EAEB;\"";
                }
                else
                    alternateColor = "style=\"background-color:#FFFFFF;\"";

                grid += "<tr " + alternateColor + ">" +
                             "<td id='emp" + sh.EmpId + "' style=\"display:none;\">" + sh.EmpId + "</td>" +
                            "<td style=\"width: 20%;\">" +
                                sh.EmpCode +
                            "</td>" +
                            "<td style=\"width: 40%;\">" +
                                sh.FullName +
                            "</td>" +
                            "<td style=\"width: 20%;\">" +
                                sh.CurrencyName +
                            "</td>" +
                            "<td style=\"width: 20%;\">" +
                                "<input type='text' style='width:187px;' class='form-control'  onblur='CheckInputValidation(this)' id='amnt" + sh.GradeId + "' value=" + sh.BasicAmount.ToString("0.00") + " />" +
                            "</td>" +
                            "<td id='amtdb" + sh.EmpId + "' style=\"display:none;\">0</td>" +
                        "</tr>";
            }
            grid += " </tbody> </table>";

            return grid;

        }
        [WebMethod]
        public static ReturnInfo UpdateEmployeeBasicSalary(List<EmpGradeBO> grade, List<EmployeeBO> employee, string transactionType)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                SalaryFormulaDA salaryDa = new SalaryFormulaDA();
                status = salaryDa.UpdateEmployeeOrGradeWiseBasicGrossSalary(grade, employee, transactionType);

                if (status == true)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                               EntityTypeEnum.EntityType.SalaryFormula.ToString(), 0,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               "Basic Salary Updated");
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
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error + ", " + ex.InnerException.ToString(), AlertType.Error);

            }

            return rtninf;
        }
        [WebMethod]
        public static string GetEmployeeSalarySheetSpecialNotes(int employeeId)
        {
            string salarySheetSpecialNotes = string.Empty;

            EmployeeDA employeeDa = new EmployeeDA();
            EmployeeBO employeeBo = employeeDa.GetEmployeeInfoById(employeeId);
            if (employeeBo.EmpId > 0)
            {
                salarySheetSpecialNotes = employeeBo.SalarySheetSpecialNotes;
            }

            return salarySheetSpecialNotes;

        }
    }
}