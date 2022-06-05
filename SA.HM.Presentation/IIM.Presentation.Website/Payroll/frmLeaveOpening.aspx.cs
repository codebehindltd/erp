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
using Newtonsoft.Json;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmLeaveOpening : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                CheckObjectPermission();
            }
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
        private void LoadGridView()
        {
            this.CheckObjectPermission();
            LeaveTypeDA da = new LeaveTypeDA();
            List<LeaveTypeBO> files = da.GetLeaveTypeInfo();
        }
        private void Cancel()
        {
            //this.txtLeaveTypeName.Text = string.Empty;
            //this.ddlActiveStat.SelectedIndex = 0;
            //this.btnSave.Text = "Save";
            //this.txtBankId.Value = string.Empty;
            //this.txtLeaveTypeName.Focus();
            //this.txtYearlyLeave.Text = string.Empty;

            //ckbCanCarryForward.Checked = false;
            //txtMaxDayCanCarryForwardYearly.Text = string.Empty;
            //txtMaxDayCanKeepAsCarryForwardLeave.Text = string.Empty;

            //txtMaxDayCanCarryForwardYearly.Enabled = false;
            //txtMaxDayCanKeepAsCarryForwardLeave.Enabled = false;

            //ckbCanCash.Checked = false;
            //txtMaxDayCanEncash.Text = string.Empty;
            //txtMaxDayCanEncash.Enabled = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool status = false;

                List<LeaveBalanceClosingBO> leaveBalanceClosingBO = new List<LeaveBalanceClosingBO>();
                LeaveInformationDA leaveDA = new LeaveInformationDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                List<LeaveBalanceClosingBO> addList = new List<LeaveBalanceClosingBO>();
                addList = JsonConvert.DeserializeObject<List<LeaveBalanceClosingBO>>(hfSaveObj.Value);

                List<LeaveBalanceClosingBO> editList = new List<LeaveBalanceClosingBO>();
                editList = JsonConvert.DeserializeObject<List<LeaveBalanceClosingBO>>(hfEditObj.Value);

                List<LeaveBalanceClosingBO> deleteList = new List<LeaveBalanceClosingBO>();
                deleteList = JsonConvert.DeserializeObject<List<LeaveBalanceClosingBO>>(hfDeleteObj.Value);

                LeaveTypeBO leaveTypeBO = new LeaveTypeBO();
                LeaveTypeDA leaveTypeDA = new LeaveTypeDA();

                foreach (LeaveBalanceClosingBO lv in addList)
                {
                    leaveTypeBO = leaveTypeDA.GetLeaveTypeInfoById(lv.LeaveTypeId);

                    lv.MaxDayCanCarryForwardYearly = leaveTypeBO.MaxDayCanCarryForwardYearly;
                    lv.MaxDayCanKeepAsCarryForwardLeave = leaveTypeBO.MaxDayCanKeepAsCarryForwardLeave;
                    lv.MaxDayCanEncash = leaveTypeBO.MaxDayCanEncash;
                    lv.TakenLeave = (lv.OpeningLeave != leaveTypeBO.YearlyLeave ? leaveTypeBO.YearlyLeave - lv.OpeningLeave : 0);
                    lv.RemainingLeave = lv.OpeningLeave;
                    lv.TotalCarryforwardLeave = 0;
                    lv.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                    lv.Status = "Open";
                }

                foreach (LeaveBalanceClosingBO lv in editList)
                {
                    leaveTypeBO = leaveTypeDA.GetLeaveTypeInfoById(lv.LeaveTypeId);
                    lv.MaxDayCanCarryForwardYearly = leaveTypeBO.MaxDayCanCarryForwardYearly;
                    lv.MaxDayCanKeepAsCarryForwardLeave = leaveTypeBO.MaxDayCanKeepAsCarryForwardLeave;
                    lv.MaxDayCanEncash = leaveTypeBO.MaxDayCanEncash;
                    lv.TakenLeave = (lv.OpeningLeave != leaveTypeBO.YearlyLeave ? leaveTypeBO.YearlyLeave - lv.OpeningLeave : 0);
                    lv.RemainingLeave = lv.OpeningLeave;
                    lv.TotalCarryforwardLeave = 0;
                    lv.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();
                    lv.Status = "Open";
                }

                status = leaveDA.UpdateOpeningLeave(addList, editList, deleteList, userInformationBO.UserInfoId);                
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    btnSave.Text = "Save";
                    
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.LeaveInformation.ToString(), 0,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Opening Leave updated for employees");

                    hfLeaveClosingId.Value = "0";
                    Clear();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        
        private void SetTab(string TabName)
        {
            //if (TabName == "SearchTab")
            //{
            //    B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
            //    A.Attributes.Add("class", "ui-state-default ui-corner-top");
            //}
            //else if (TabName == "EntryTab")
            //{
            //    A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
            //    B.Attributes.Add("class", "ui-state-default ui-corner-top");
            //}
        }
        private void Clear()
        {
            btnSave.Text = "Save";
            SetTab("EntryTab");

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl empSearch;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeSearch");
            ((HiddenField)empSearch.FindControl("hfEmployeeId")).Value = string.Empty;
            ((TextBox)empSearch.FindControl("txtSearchEmployee")).Text = string.Empty;
            ((TextBox)empSearch.FindControl("txtEmployeeName")).Text = string.Empty;
        }

        [WebMethod]
        public static GridViewDataNPaging<LeaveBalanceClosingViewBO, GridPaging> SearchOpeningLeave(string employeeId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            long empId = 0;
            if (!string.IsNullOrWhiteSpace(employeeId))
            {
                empId = Convert.ToInt64(employeeId);
            }

            GridViewDataNPaging<LeaveBalanceClosingViewBO, GridPaging> myGridData = new GridViewDataNPaging<LeaveBalanceClosingViewBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            LeaveInformationDA leaveDA = new LeaveInformationDA();
            List<LeaveBalanceClosingViewBO> actionList = new List<LeaveBalanceClosingViewBO>();
            actionList = leaveDA.GetLeaveBalanceClosingBySearchCriteriaForPaging(empId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<LeaveBalanceClosingViewBO> distinctItems = new List<LeaveBalanceClosingViewBO>();
            distinctItems = actionList.GroupBy(test => test.LeaveClosingId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static string LoadLeaveTypeInfo(Int32 empId)
        {
            string strTable = "", openingLeave = string.Empty, forwardLeave = string.Empty, encashLeave = string.Empty, leaveClosingId = "0";
            int counter = 0;
            string empGender = string.Empty;

            EmployeeDA empDA = new EmployeeDA();
            EmployeeBO empBO = new EmployeeBO();
            empBO = empDA.GetEmployeeInfoById(empId);
            if (empBO != null)
            {
                if (empBO.EmpId > 0)
                {
                    List<LeaveTypeBO> leaveTypeList = new List<LeaveTypeBO>();
                    LeaveTypeDA leaveTypeDA = new LeaveTypeDA();

                    if (empBO.Gender == "Male")
                    {
                        leaveTypeList = leaveTypeDA.GetLeaveTypeInfo().Where(x => x.LeaveTypeId != 4).ToList();
                    }
                    else if (empBO.Gender == "Female")
                    {
                        leaveTypeList = leaveTypeDA.GetLeaveTypeInfo().Where(x => x.LeaveTypeId != 5).ToList();
                    }
                    else
                    {
                        leaveTypeList = leaveTypeDA.GetLeaveTypeInfo();
                    }

                    HMUtility hmUtility = new HMUtility();
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    LeaveInformationDA leaveDA = new LeaveInformationDA();
                    List<LeaveBalanceClosingBO> leaveBalanceClosingList = new List<LeaveBalanceClosingBO>();

                    if (empId > 0)
                    {
                        leaveBalanceClosingList = leaveDA.GetLeaveBalanceClosingByEmpId(empId, userInformationBO.UserInfoId);
                    }

                    strTable += "<table  id='LeaveOpening' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                    strTable += "<th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th>";
                    strTable += "<th align='left' scope='col' style='width: 40%;'>Leave Type</th>";
                    strTable += "<th align='center' scope='col' style='width: 20%;'>Leave Balance</th>";
                    strTable += "<th align='center' scope='col' style='width: 20%;'>Leave Forward</th>";
                    strTable += "<th align='center' scope='col' style='width: 20%;'>Leave Encash</th>";
                    strTable += "</tr></thead><tbody>";

                    foreach (LeaveTypeBO lev in leaveTypeList)
                    {
                        var v = (from br in leaveBalanceClosingList where br.LeaveTypeId == lev.LeaveTypeId select br).FirstOrDefault();
                        if (v != null)
                        {
                            openingLeave = v.RemainingLeave.ToString();
                            forwardLeave = v.CarryForwardedLeave.ToString();
                            encashLeave = v.EncashableLeave.ToString();
                            leaveClosingId = v.LeaveClosingId.ToString();
                        }

                        counter++;
                        if (counter % 2 == 0)
                        {
                            strTable += "<tr style='background-color:White;'>";
                        }
                        else
                        {
                            strTable += "<tr style='background-color:#E3EAEB;'>";
                        }

                        strTable += "<td align='left' style=\"display:none;\">" + leaveClosingId + "</td>";
                        strTable += "<td align='left' style=\"display:none;\">" + lev.LeaveTypeId + "</td>";
                        strTable += "<td align='left' style=\"display:none;\">" + openingLeave + "</td>";
                        strTable += "<td align='left' style=\"display:none;\">" + forwardLeave + "</td>";
                        strTable += "<td align='left' style=\"display:none;\">" + encashLeave + "</td>";
                        strTable += "<td align='left' style=\"'width:40%;\">" + lev.TypeName + "</td>";
                        strTable += "<td align='left' style='width:20%;'><input type='text' class='form-control' value='" + openingLeave + "' /></td>";
                        strTable += "<td align='left' style='width: 20%;'><input type='text' class='form-control' value='" + forwardLeave + "' /></td>";
                        strTable += "<td align='left' style='width: 20%;'><input type='text' class='form-control' value='" + encashLeave + "' /></td>";
                        strTable += "</tr>";

                        openingLeave = string.Empty;
                        forwardLeave = string.Empty;
                        encashLeave = string.Empty;
                        leaveClosingId = "0";
                    }

                    strTable += "</tbody> </table>";
                }
            }           

            return strTable;
        }
    }
}