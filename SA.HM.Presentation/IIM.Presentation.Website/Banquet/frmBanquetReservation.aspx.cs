using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using HotelManagement.Entity.Banquet;
using HotelManagement.Data.Banquet;
using System.Web.Services;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Text.RegularExpressions;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using Newtonsoft.Json;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Threading;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Data.SalesAndMarketing;

namespace HotelManagement.Presentation.Website.Banquet
{
    public partial class frmBanquetReservation : BasePage
    {
        HiddenField innboardMessage;
        protected int _RoomReservationId;
        protected int _proId = -1;
        ArrayList arrayDelete;
        protected int _reservationId;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        private Boolean isBanquetRateEditableEnable = false;
        protected bool isSingle = true;
        //**************************** Handlers ****************************//  
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            hfMinCheckInDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.Date);
            _RoomReservationId = 0;
            AddEditODeleteDetail();

            if (!IsPostBack)
            {
                Session["ReservationDetailList"] = null;
                LoadBanquetInfo();
                LoadCountry();
                LoadSeatingPlan();
                LoadOccassion();
                LoadAffiliatedCompany();
                LoadCommonDropDownHiddenField();
                LoadIsBanquetRateEditableEnable();
                LoadSearchBanquetInfo();
                LoadRefferenceName();
                SetDefaulTime();
                IsInventoryIntegrateWithAccounts();
                IsBanquetReservationRestictionForAllUser();
                LoadEmployee();
                LoadEmployeeInfo();
                LoadEventType();

                hfDuplicateReservarionValidation.Value = "0";
                string editId = Request.QueryString["editId"];
                hfEditedId.Value = editId;
                if (!string.IsNullOrWhiteSpace(editId))
                {
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                    }
                }

                LoadGrandTotalLabelChange();
                CheckPermission();
                hfToday.Value = DateTime.Now.Date.ToShortDateString();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFormValid())
                {
                    return;
                }

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                BanquetReservationBO reservationBO = new BanquetReservationBO();
                BanquetReservationDA reservationDA = new BanquetReservationDA();

                //Store Data
                if (chkIsReturnedGuest.Checked == true)
                {
                    reservationBO.IsReturnedClient = true;
                }
                else
                {
                    reservationBO.IsReturnedClient = false;
                }
                if (ddlRefferenceId.SelectedValue != "0" && hfEventType.Value == "Rental")
                    reservationBO.RefferenceId = Int32.Parse(ddlRefferenceId.SelectedValue);
                if (ddlEmployeeId.SelectedValue != "0" && hfEventType.Value == "Internal")
                    reservationBO.RefferenceId = Int32.Parse(ddlEmployeeId.SelectedValue);
                reservationBO.GLCompanyId = Int32.Parse(ddlGLCompany.SelectedValue);
                reservationBO.GLProjectId = Int32.Parse(hfGLProjectId.Value);
                reservationBO.EventType = (ddlEventTypeId.SelectedValue).Trim();
                reservationBO.EventTitle = txtEventTitle.Text;

                reservationBO.BanquetId = Int32.Parse(ddlBanquetId.SelectedValue);
                reservationBO.CostCenterId = Int32.Parse(hfCostCenterId.Value);
                reservationBO.ReservationMode = ddlReservationMode.SelectedValue.ToString();
                reservationBO.Address = txtAddress.Text;
                reservationBO.ZipCode = null;
                reservationBO.CountryId = Int32.Parse(ddlCountryId.SelectedValue);
                reservationBO.PhoneNumber = txtPhoneNumber.Text;
                reservationBO.BookingFor = "";

                if (ddlReservationMode.SelectedValue == "Company")
                {
                    if (chkIsLitedCompany.Checked)
                    {
                        reservationBO.IsListedCompany = true;
                        reservationBO.Name = ddlCompanyId.SelectedItem.Text;
                        reservationBO.ContactPerson = txtContactPerson.Text;
                        reservationBO.CompanyId = Convert.ToInt32(ddlCompanyId.SelectedValue);
                        if (reservationBO.CompanyId == 0)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Please provide an enlisted company.", AlertType.Warning);
                            return;
                        }
                    }
                    else
                    {
                        reservationBO.IsListedCompany = false;
                        reservationBO.Name = txtName.Text;
                        reservationBO.ContactPerson = txtContactPerson.Text;
                        reservationBO.CompanyId = 0;
                    }
                }
                else
                {
                    reservationBO.IsListedCompany = false;
                    reservationBO.Name = txtName.Text;
                    reservationBO.ContactPerson = txtName.Text;
                    reservationBO.CompanyId = 0;
                }

                reservationBO.ContactPhone = txtContactPhone.Text;
                reservationBO.ContactEmail = txtContactEmail.Text;
                reservationBO.ArriveDate = Convert.ToDateTime(hmUtility.GetDateTimeFromString(txtArriveDate.Text, userInformationBO.ServerDateFormat).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(txtProbableArrivalHour.Text).ToString("HH:mm:ss"));
                reservationBO.DepartureDate = Convert.ToDateTime(hmUtility.GetDateTimeFromString(txtArriveDate.Text, userInformationBO.ServerDateFormat).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(txtProbableDepartureHour.Text).ToString("HH:mm:ss"));
                reservationBO.NumberOfPersonAdult = !string.IsNullOrWhiteSpace(txtNumberOfPersonAdult.Text) ? Convert.ToInt32(txtNumberOfPersonAdult.Text) : 0;
                reservationBO.NumberOfPersonChild = !string.IsNullOrWhiteSpace(txtNumberOfPersonChild.Text) ? Convert.ToInt32(txtNumberOfPersonChild.Text) : 0;
                reservationBO.OccessionTypeId = Int32.Parse(ddlOccessionTypeId.SelectedValue);
                reservationBO.SeatingId = Int32.Parse(ddlSeatingId.SelectedValue);
                LoadIsBanquetRateEditableEnable();
                if (isBanquetRateEditableEnable)
                {
                    reservationBO.BanquetRate = !string.IsNullOrWhiteSpace(txtBanquetRate.Text) ? Convert.ToDecimal(txtBanquetRate.Text) : 0;
                }
                else
                {
                    reservationBO.BanquetRate = !string.IsNullOrWhiteSpace(hfBanquetRate.Value) ? Convert.ToDecimal(hfBanquetRate.Value) : 0;
                }

                reservationBO.TotalAmount = !string.IsNullOrWhiteSpace(hfTotalAmount.Value) ? Convert.ToDecimal(hfTotalAmount.Value) : 0;
                reservationBO.DiscountType = ddlDiscountType.SelectedValue.ToString();
                reservationBO.DiscountAmount = !string.IsNullOrWhiteSpace(txtDiscountAmount.Text) ? Convert.ToDecimal(txtDiscountAmount.Text) : 0;
                reservationBO.DiscountedAmount = !string.IsNullOrWhiteSpace(hfDiscountedAmount.Value) ? Convert.ToDecimal(hfDiscountedAmount.Value) : 0;
                reservationBO.Remarks = txtRemarks.Text;

                reservationBO.IsInvoiceServiceChargeEnable = cbServiceCharge.Checked;
                reservationBO.IsInvoiceCitySDChargeEnable = cbSDCharge.Checked;
                reservationBO.IsInvoiceVatAmountEnable = cbVatAmount.Checked;
                reservationBO.IsInvoiceAdditionalChargeEnable = cbAdditionalCharge.Checked;
                reservationBO.AdditionalChargeType = hfAdditionalChargeType.Value;

                reservationBO.InvoiceServiceRate = !string.IsNullOrEmpty(hfServiceRate.Value) ? Convert.ToDecimal(hfServiceRate.Value) : 0.00M;
                reservationBO.InvoiceServiceCharge = !string.IsNullOrEmpty(hfServiceCharge.Value) ? Convert.ToDecimal(hfServiceCharge.Value) : 0.00M;
                reservationBO.InvoiceCitySDCharge = !string.IsNullOrEmpty(hfSDChargeAmount.Value) ? Convert.ToDecimal(hfSDChargeAmount.Value) : 0.00M;
                reservationBO.InvoiceVatAmount = !string.IsNullOrEmpty(hfVatAmount.Value) ? Convert.ToDecimal(hfVatAmount.Value) : 0.00M;
                reservationBO.InvoiceAdditionalCharge = !string.IsNullOrEmpty(hfAdditionalChargeAmount.Value) ? Convert.ToDecimal(hfAdditionalChargeAmount.Value) : 0.00M;
                reservationBO.GrandTotal = !string.IsNullOrEmpty(hfGrandTotal.Value) ? Convert.ToDecimal(hfGrandTotal.Value) : 0.00M;
                reservationBO.MeetingAgenda = txtMeetingAgenda.Value;
                string participantFromOfficeValue = hfparticipantFromOfficeValue.Value;
                reservationBO.PerticipantFromOfficeCommaSeperatedIds = participantFromOfficeValue;

                var client = reservationBO.PerticipantFromOfficeCommaSeperatedIds.Split(new string[] { "," }, StringSplitOptions.None);
                // -------------------------------------------------------------------------------------------------------------------------------------------
                List<BanquetReservationDetailBO> addList = new List<BanquetReservationDetailBO>();
                List<BanquetReservationDetailBO> editList = new List<BanquetReservationDetailBO>();
                List<BanquetReservationDetailBO> deleteList = new List<BanquetReservationDetailBO>();
                List<BanquetReservationClassificationDiscountBO> discountLst = new List<BanquetReservationClassificationDiscountBO>();
                List<BanquetReservationClassificationDiscountBO> discountDeletedLst = new List<BanquetReservationClassificationDiscountBO>();

                addList = JsonConvert.DeserializeObject<List<BanquetReservationDetailBO>>(hfSaveObj.Value);
                editList = JsonConvert.DeserializeObject<List<BanquetReservationDetailBO>>(hfEditObj.Value);
                deleteList = JsonConvert.DeserializeObject<List<BanquetReservationDetailBO>>(hfDeleteObj.Value);
                discountLst = JsonConvert.DeserializeObject<List<BanquetReservationClassificationDiscountBO>>(hfClassificationDiscountSave.Value);
                // -------------------------------------------------------------------------------------------------------------------------------------------

                int costCenterId = !string.IsNullOrWhiteSpace(hfCostCenterId.Value) ? Convert.ToInt32(hfCostCenterId.Value) : 0;
                if (costCenterId > 0)
                {
                    reservationBO.CostCenterId = costCenterId;
                    if (btnSave.Text.Equals("Save"))
                    {
                        long tmpSalesId = 0;
                        reservationBO.CreatedBy = userInformationBO.UserInfoId;

                        bool status = reservationDA.SaveBanquetReservationInfo(reservationBO, addList, discountLst, out tmpSalesId);

                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), tmpSalesId,
                            ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetReservation));

                            if (hfEventType.Value == "Rental")
                            {
                                GenerateConfirmationLetter(tmpSalesId);
                            }
                        }

                    }
                    else
                    {
                        discountDeletedLst = JsonConvert.DeserializeObject<List<BanquetReservationClassificationDiscountBO>>(hfClassificationDiscountDelete.Value);
                        reservationBO.Id = Convert.ToInt64(Session["_reservationId"]);
                        reservationBO.LastModifiedBy = userInformationBO.UserInfoId;

                        Boolean status = reservationDA.UpdateBanquetReservationInfo(reservationBO, addList, editList, deleteList, Session["arrayDelete"] as ArrayList, discountLst, discountDeletedLst);

                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success, 0, "frmBanquetReservation.aspx");
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), reservationBO.Id,
                                ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetReservation));
                        }
                    }

                    Cancel();
                    SetTab("Entry");

                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "Banquet and Cost Center Mapping Related Configuration Missing.", AlertType.Warning);
                    return;
                }

                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetInternalMeetingEmailSendEnable", "IsBanquetInternalMeetingEmailSendEnable");
                if (commonSetupBO != null)
                {
                    if (commonSetupBO.SetupId > 0)
                    {
                        if (ddlEventTypeId.SelectedValue == "Internal")
                        {
                            if (reservationBO.RefferenceId != null)
                            {
                                BanquetInformationDA banInfoDa = new BanquetInformationDA();
                                BanquetInformationBO banquetInformationBO = banInfoDa.GetBanquetInformationById(reservationBO.BanquetId);

                                CompanyDA companyDA = new CompanyDA();
                                CompanyBO companyBO = new CompanyBO();

                                EmployeeBO employeeBO = new EmployeeBO();
                                EmployeeDA empDa = new EmployeeDA();
                                int employeeId = Int32.Parse((reservationBO.RefferenceId).ToString());
                                var cordinator = empDa.GetEmployeeInfoById(employeeId);
                                companyBO = companyDA.GetCompanyInfoById(cordinator.EmpCompanyId);
                                bool send = false;

                                Email email;
                                HMCommonSetupBO SendEmailAddressBO = new HMCommonSetupBO();
                                SendEmailAddressBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
                                string mainString = SendEmailAddressBO.SetupValue;

                                foreach (var invitedEmployee in client)
                                {
                                    var employee = empDa.GetEmployeeInfoById(int.Parse(invitedEmployee));
                                    var tokens = new Dictionary<string, string>
                                    {
                                        {"Name",  employee.DisplayName},
                                        {"Cordinator", cordinator.DisplayName },
                                        {"CordinatorEmail", cordinator.OfficialEmail },
                                        {"CordinatorPhone", cordinator.PresentPhone },
                                        {"MeetingAgenda", reservationBO.MeetingAgenda},
                                        {"Date", reservationBO.ArriveDate.ToString() },
                                        {"CompanyName", companyBO.CompanyName},
                                        {"CompanyAddress", companyBO.CompanyAddress},
                                        {"Email", companyBO.EmailAddress },
                                        {"Venue",  banquetInformationBO.Name}
                                    };

                                    if (!string.IsNullOrEmpty(mainString))
                                    {
                                        string[] dataArray = mainString.Split('~');
                                        email = new Email()
                                        {
                                            From = dataArray[0],
                                            Password = dataArray[1],
                                            To = employee.OfficialEmail,
                                            Subject = reservationBO.EventTitle,
                                            Host = dataArray[2],
                                            Port = dataArray[3],
                                            TempleteName = "ParticipatedEmployeeEmail.html"
                                        };

                                        try
                                        {
                                            //Send Mail   
                                            send = EmailHelper.SendEmail(email, tokens);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                return;
            }
        }
        //************************ User Defined Function ********************//
        private void LoadEventType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BanquetEventType");

            ddlEventTypeId.DataSource = fields;
            ddlEventTypeId.DataTextField = "FieldValue";
            ddlEventTypeId.DataValueField = "FieldValue";
            ddlEventTypeId.DataBind();
            if (fields.Count == 1)
            {
                hfEventType.Value = fields[0].FieldValue;
            }
            else if (fields.Count > 1)
            {
                ListItem item = new ListItem();
                item.Value = "0";
                item.Text = hmUtility.GetDropDownFirstValue();
                ddlEventTypeId.Items.Insert(0, item);
            }
        }
        private void LoadEmployee()
        {
            EmployeeDA employeeDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = employeeDA.GetEmployeeInfo();

            ddlEmployeeId.DataSource = empList;
            ddlEmployeeId.DataTextField = "DisplayName";
            ddlEmployeeId.DataValueField = "EmpId";
            ddlEmployeeId.DataBind();

            ListItem itemEmployee = new ListItem();
            itemEmployee.Value = "0";
            itemEmployee.Text = hmUtility.GetDropDownFirstValue();
            ddlEmployeeId.Items.Insert(0, itemEmployee);
        }
        private void LoadEmployeeInfo()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();

            empList = empDa.GetEmployeeInfo();
            ddltxtParticipantFromOffice.DataSource = empList;
            ddltxtParticipantFromOffice.DataTextField = "DisplayName";
            ddltxtParticipantFromOffice.DataValueField = "EmpId";
            ddltxtParticipantFromOffice.DataBind();


            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddltxtParticipantFromOffice.Items.Insert(0, FirstItem);

        }
        private void IsInventoryIntegrateWithAccounts()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isInventoryIntegrateWithAccountsBO = new HMCommonSetupBO();
            isInventoryIntegrateWithAccountsBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetIntegrateWithAccounts", "IsBanquetIntegrateWithAccounts");

            if (isInventoryIntegrateWithAccountsBO != null)
            {
                if (isInventoryIntegrateWithAccountsBO.SetupValue == "1")
                {
                    LoadGLCompany(false);
                }
                else
                    LoadDefaultGLCompanyNProject();
            }
        }
        private void IsBanquetReservationRestictionForAllUser()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isBanquetReservationRestictionForAllUserBO = new HMCommonSetupBO();
            isBanquetReservationRestictionForAllUserBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetReservationRestictionForAllUser", "IsBanquetReservationRestictionForAllUser");

            if (isBanquetReservationRestictionForAllUserBO != null)
            {
                if (isBanquetReservationRestictionForAllUserBO.SetupValue == "1")
                {
                    hfIsBanquetReservationRestictionForAllUser.Value = "1";
                }
                else
                    hfIsBanquetReservationRestictionForAllUser.Value = "0";
            }
        }
        private void LoadGLCompany(bool isSingle)
        {
            hfIsSingle.Value = "0";
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();

            hfCompanyAll.Value = JsonConvert.SerializeObject(List);

            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (List.Count == 1)
            {
                companyList.Add(List[0]);
                ddlGLCompany.DataSource = companyList;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                hfIsSingle.Value = "1";
                LoadGLProjectByCompany(companyList[0].CompanyId);
            }
            else
            {
                ddlGLCompany.DataSource = List;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                hfIsSingle.Value = "0";
                ListItem itemCompany = new ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                ddlGLCompany.Items.Insert(0, itemCompany);

                if (hfGLCompanyId.Value != "0")
                {
                    LoadGLProjectByCompany(Convert.ToInt32(hfGLCompanyId.Value));
                }
            }
        }
        private void LoadGLProjectByCompany(int companyId)
        {
            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            var List = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(Convert.ToInt32(companyId), Convert.ToInt32(userInformationBO.UserGroupId)).Where(x => x.IsFinalStage == false).ToList();

            ddlGLProject.DataSource = List;
            ddlGLProject.DataTextField = "Name";
            ddlGLProject.DataValueField = "ProjectId";
            ddlGLProject.DataBind();

            if (List.Count > 1)
            {
                isSingle = false;
                hfIsSingle.Value = "0";
                ListItem itemProject = new ListItem();
                itemProject.Value = "0";
                itemProject.Text = hmUtility.GetDropDownFirstValue();
                ddlGLProject.Items.Insert(0, itemProject);

                ListItem itemSrcProject = new ListItem();
                itemSrcProject.Value = "0";
                itemSrcProject.Text = hmUtility.GetDropDownFirstAllValue();
            }
        }
        private void LoadDefaultGLCompanyNProject()
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            companyList.Add(entityDA.GetAllGLCompanyInfo().FirstOrDefault());

            ddlGLCompany.DataSource = companyList;
            ddlGLCompany.DataTextField = "Name";
            ddlGLCompany.DataValueField = "CompanyId";
            ddlGLCompany.DataBind();

            hfIsSingle.Value = "1";
            if (companyList.Count > 0)
            {
                GLProjectDA projectDA = new GLProjectDA();
                List<GLProjectBO> projectList = new List<GLProjectBO>();
                projectList.Add(projectDA.GetGLProjectInfoByGLCompany(companyList[0].CompanyId).FirstOrDefault());

                ddlGLProject.DataSource = projectList;
                ddlGLProject.DataTextField = "Name";
                ddlGLProject.DataValueField = "ProjectId";
                ddlGLProject.DataBind();
            }
        }
        private void LoadGrandTotalLabelChange()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetBillInclusive", "IsBanquetBillInclusive");
            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        lblGrandTotal.Text = "Grand Total";
                    }
                    else
                    {
                        lblGrandTotal.Text = "Net Amount";
                    }
                }
                else
                {
                    lblGrandTotal.Text = "Grand Total";
                }
            }
        }
        private void LoadIsBanquetRateEditableEnable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetRateEditableEnable", "IsBanquetRateEditableEnable");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        txtBanquetRate.Enabled = true;
                        isBanquetRateEditableEnable = true;
                    }
                    else
                    {
                        txtBanquetRate.Enabled = false;
                        isBanquetRateEditableEnable = false;
                    }
                }
            }
        }
        private void AddEditODeleteDetail()
        {
            //Delete------------
            if (Session["arrayDelete"] == null)
            {
                arrayDelete = new ArrayList();
                Session.Add("arrayDelete", arrayDelete);
            }
            else
                arrayDelete = Session["arrayDelete"] as ArrayList;
        }
        public static void OpenNewBrowserWindow(string Url, Control control)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Open", "window.open('" + Url + "');", true);
        }
        private void DeleteData(int _reservationId)
        {
            BanquetReservationDA reservationDA = new BanquetReservationDA();
            try
            {
                Boolean statusApproved = reservationDA.DeleteBanquetReservationInfoById(_reservationId);
                if (statusApproved)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), _reservationId,
                        ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetReservation));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    Cancel();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        private void FillForm(int ReservationId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //Master Information------------------------
            BanquetReservationBO reservationBO = new BanquetReservationBO();
            BanquetReservationDA reservationDA = new BanquetReservationDA();

            reservationBO = reservationDA.GetBanquetReservationInfoById(ReservationId);
            if (reservationBO.Id > 0)
            {
                Session["_reservationId"] = reservationBO.Id;

                ddlReservationMode.SelectedValue = reservationBO.ReservationMode.ToString();
                if (reservationBO.ReservationMode == "Company")
                {
                    txtName.Text = reservationBO.Name;
                    if (reservationBO.IsListedCompany == true)
                    {
                        chkIsLitedCompany.Checked = true;
                    }
                    else
                    {
                        chkIsLitedCompany.Checked = false;
                    }
                }
                else
                {
                    txtName.Text = reservationBO.ContactPerson;
                }

                hfGLCompanyId.Value = reservationBO.GLCompanyId.ToString();
                hfGLProjectId.Value = reservationBO.GLProjectId.ToString();
                hfReservationId.Value = reservationBO.Id.ToString();
                hfBanquetId.Value = reservationBO.BanquetId.ToString();
                hfddlEmployeeId.Value = reservationBO.RefferenceId.ToString();
                txtArriveDate.Text = hmUtility.GetStringFromDateTime(reservationBO.ArriveDate);
                IsInventoryIntegrateWithAccounts();
                if (userInformationBO.TimeFormat == "hh:mm tt")
                {
                    txtProbableArrivalHour.Text = reservationBO.ArriveDate.ToString(userInformationBO.TimeFormat.Replace(":mm ", " "));
                    txtProbableDepartureHour.Text = reservationBO.DepartureDate.ToString(userInformationBO.TimeFormat.Replace(":mm ", " "));
                }
                else
                {
                    txtProbableArrivalHour.Text = reservationBO.ArriveDate.ToString("HH");
                    txtProbableDepartureHour.Text = reservationBO.DepartureDate.ToString("HH");
                }

                hfCostCenterId.Value = reservationBO.CostCenterId.ToString();
                txtRemarks.Text = reservationBO.Remarks;
                txtContactPerson.Text = reservationBO.ContactPerson;
                txtAddress.Text = reservationBO.Address;
                txtContactEmail.Text = reservationBO.ContactEmail;
                txtPhoneNumber.Text = reservationBO.PhoneNumber;
                txtContactPhone.Text = reservationBO.ContactPhone;
                ddlCountryId.SelectedValue = reservationBO.CountryId.ToString();
                ddlBanquetId.SelectedValue = reservationBO.BanquetId.ToString();
                ddlCompanyId.SelectedValue = reservationBO.CompanyId.ToString();
                ddlOccessionTypeId.SelectedValue = reservationBO.OccessionTypeId.ToString();
                ddlSeatingId.SelectedValue = reservationBO.SeatingId.ToString();
                ddlBanquetId.SelectedValue = reservationBO.BanquetId.ToString();
                txtNumberOfPersonAdult.Text = reservationBO.NumberOfPersonAdult.ToString();
                txtNumberOfPersonChild.Text = reservationBO.NumberOfPersonChild.ToString();
                txtPhoneNumber.Text = reservationBO.PhoneNumber;
                txtReservationId.Value = reservationBO.Id.ToString();
                txtMeetingAgenda.Value = reservationBO.MeetingAgenda;
                ddlEmployeeId.SelectedValue = reservationBO.RefferenceId.ToString();
                List<int> empIdList = new List<int>();
                foreach (var employee in reservationBO.PerticipantFromOffice)
                {
                    empIdList.Add(employee.EmpId);
                }
                hfparticipantFromOfficeValue.Value = string.Join(",", empIdList.ToArray());
                //ddltxtParticipantFromOffice.SelectedValue = empIdList.ToArray()
                if (reservationBO.EventType == "Rental")
                {
                    ddlRefferenceId.SelectedValue = reservationBO.RefferenceId.ToString();
                }
                else
                {
                    ddlEmployeeId.SelectedValue = reservationBO.RefferenceId.ToString();
                }

                ddlEventTypeId.SelectedValue = reservationBO.EventType;
                txtEventTitle.Text = reservationBO.EventTitle;
                hfEventType.Value = reservationBO.EventType;
                ddlGLCompany.SelectedValue = reservationBO.GLCompanyId.ToString();
                ddlGLProject.SelectedValue = reservationBO.GLProjectId.ToString();

                if (reservationBO.IsReturnedClient == true)
                {
                    chkIsReturnedGuest.Checked = true;
                }
                else
                {
                    chkIsReturnedGuest.Checked = false;
                }

                hfTotalAmount.Value = reservationBO.TotalAmount.ToString();
                ddlDiscountType.SelectedValue = reservationBO.ReservationDiscountType;
                txtDiscountAmount.Text = reservationBO.ReservationDiscountAmount.ToString();
                hfDiscountedAmount.Value = reservationBO.DiscountedAmount.ToString();
                reservationBO.Remarks = txtRemarks.Text;

                cbServiceCharge.Checked = reservationBO.IsInvoiceServiceChargeEnable;
                cbSDCharge.Checked = reservationBO.IsInvoiceCitySDChargeEnable;
                cbVatAmount.Checked = reservationBO.IsInvoiceVatAmountEnable;
                cbAdditionalCharge.Checked = reservationBO.IsInvoiceAdditionalChargeEnable;
                hfAdditionalChargeType.Value = reservationBO.AdditionalChargeType;

                hfServiceRate.Value = reservationBO.InvoiceServiceRate.ToString();
                hfServiceCharge.Value = reservationBO.InvoiceServiceCharge.ToString();
                hfSDChargeAmount.Value = reservationBO.InvoiceCitySDCharge.ToString();
                hfVatAmount.Value = reservationBO.InvoiceVatAmount.ToString();
                hfAdditionalChargeAmount.Value = reservationBO.InvoiceAdditionalCharge.ToString();

                hfServiceRate.Value = reservationBO.InvoiceServiceRate.ToString();
                hfGrandTotal.Value = reservationBO.GrandTotal.ToString();
                hfDiscountedAmount.Value = reservationBO.DiscountedAmount.ToString();

                txtDiscountedAmount.Text = reservationBO.DiscountedAmount.ToString();
                txtServiceRate.Text = reservationBO.InvoiceServiceRate.ToString();
                txtTotalAmount.Text = reservationBO.TotalAmount.ToString();

                txtServiceCharge.Text = reservationBO.InvoiceServiceCharge.ToString();
                txtVatAmount.Text = reservationBO.InvoiceVatAmount.ToString();
                txtSDCharge.Text = reservationBO.InvoiceCitySDCharge.ToString();
                txtVatAmount.Text = reservationBO.InvoiceVatAmount.ToString();
                txtAdditionalCharge.Text = reservationBO.InvoiceAdditionalCharge.ToString();
                txtGrandTotal.Text = reservationBO.GrandTotal.ToString();

                txtBanquetRate.Text = reservationBO.BanquetRate.ToString();
                hfBanquetRate.Value = reservationBO.BanquetRate.ToString();
                btnSave.Text = "Update";

                //Detail Information------------------------
                ltlTableWiseItemInformation.InnerHtml = GenerateItemDetailTable(reservationBO.Id, reservationBO.CostCenterId);

                // Discount Policy
                List<BanquetReservationClassificationDiscountBO> discountLst = new List<BanquetReservationClassificationDiscountBO>();
                discountLst = reservationDA.GetBanquetReservationClassificationDiscount(ReservationId);
                hfClassificationDiscountAlreadySave.Value = JsonConvert.SerializeObject(discountLst);
            }
        }
        private void ClearDetailPart()
        {
            ddlItemType.SelectedIndex = -1;
            chkIscomplementary.Checked = false;
            ddlItemId.SelectedIndex = -1;
            txtItemUnit.Text = string.Empty;
            txtUnitPrice.Text = string.Empty;
            lblHiddenId.Text = string.Empty;
            txtHiddenProductId.Value = "";
            _RoomReservationId = 0;
        }
        private void Cancel()
        {
            btnSave.Text = "Save";
            Session["ReservationDetailList"] = null;
            Session["arrayDelete"] = null;
            txtName.Text = "";
            ddlBanquetId.SelectedValue = "0";
            txtAddress.Text = "";
            txtRemarks.Text = "";
            txtBanquetRate.Text = "";
            //txtZipCode.Text = "";
            ddlCountryId.SelectedValue = "19";
            //txtEmailAddress.Text = "";
            txtPhoneNumber.Text = "";
            //txtBookingFor.Text = "";
            txtContactPerson.Text = "";
            txtContactPhone.Text = "";
            txtContactEmail.Text = "";
            txtArriveDate.Text = "";
            //txtDepartureDate.Text = "";
            SetDefaulTime();
            txtNumberOfPersonAdult.Text = "";
            txtNumberOfPersonChild.Text = "";
            ddlOccessionTypeId.SelectedValue = "0";
            ddlSeatingId.SelectedValue = "0";
            //gvRegistrationDetail.DataSource = Session["ReservationDetailList"] as List<BanquetReservationDetailBO>;
            //gvRegistrationDetail.DataBind();
            ClearDetailPart();
            txtItemUnit.Text = "0";
            txtVatAmount.Text = "0";
            //txtGrandTotal.Text = "0";
            txtDiscountAmount.Text = "0";
            chkIsReturnedGuest.Checked = false;
            ddlRefferenceId.SelectedIndex = 0;

            //ddlEventTypeId.SelectedValue = "0";
            txtEventTitle.Text = "";
            hfEventType.Value = "0";
            hfGLCompanyId.Value = "0";
            hfGLProjectId.Value = "0";
            //ddlGLCompany.SelectedValue = "0";
            //ddlGLProject.SelectedValue = "0";
            ddlEmployeeId.SelectedIndex = -1;

            txtServiceCharge.Text = "0";
            //txtVatAmount.Text = "0";
            txtGrandTotal.Text = "0";
            txtTotalAmount.Text = "0";
            ltlTableWiseItemInformation.InnerHtml = string.Empty;

            hfDiscountedAmount.Value = "0";
            txtDiscountedAmount.Text = "0";
            txtServiceCharge.Text = "0";
            hfServiceCharge.Value = "0";
            txtVatAmount.Text = "0";
            hfVatAmount.Value = "0";
            txtGrandTotal.Text = "0";

            cbAdditionalCharge.Checked = true;
            cbSDCharge.Checked = true;
            cbVatAmount.Checked = true;
            cbServiceCharge.Checked = true;
            ddlReservationMode.SelectedValue = "0";
            chkIsLitedCompany.Checked = false;
            txtMeetingAgenda.Value = "";
            hfparticipantFromOfficeValue.Value = string.Empty;
        }
        private bool IsValidMail(string Email)
        {
            bool status = true;
            string expression = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" + @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" + @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

            Match match = Regex.Match(Email, expression, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }
        private bool IsFormValid()
        {
            bool status = true;

            if (String.IsNullOrWhiteSpace(txtArriveDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Event Date.", AlertType.Warning);
                txtArriveDate.Focus();
                status = false;
            }

            return status;
        }
        private void SetTab(string TabName)
        {
            if (TabName == "Search")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "Entry")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadBanquetInfo()
        {
            BanquetInformationBO banquetBO = new BanquetInformationBO();
            BanquetInformationDA banquetDA = new BanquetInformationDA();
            var List = banquetDA.GetAllBanquetInformation();
            ddlBanquetId.DataSource = List;
            ddlBanquetId.DataTextField = "Name";
            ddlBanquetId.DataValueField = "Id";
            ddlBanquetId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlBanquetId.Items.Insert(0, item);
        }
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
            hfIsUpdatePermission.Value = isUpdatePermission.ToString();
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfIsUpdatePermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
        private bool IsDetailFormValid()
        {
            bool status = true;
            decimal result;
            if (txtHiddenProductId.Value == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Valid Product.", AlertType.Warning);
                ddlItemId.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(txtItemUnit.Text) || !Decimal.TryParse(txtItemUnit.Text, out result))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Quantity.", AlertType.Warning);
                txtItemUnit.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(txtUnitPrice.Text) || !Decimal.TryParse(txtUnitPrice.Text, out result))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Quantity.", AlertType.Warning);
                txtUnitPrice.Focus();
                status = false;
            }
            return status;
        }
        private void LoadRefferenceName()
        {
            BanquetRefferenceDA refferenceDA = new BanquetRefferenceDA();
            var List = refferenceDA.GetAllBanquetRefference();
            ddlRefferenceId.DataSource = List;
            ddlRefferenceId.DataTextField = "Name";
            ddlRefferenceId.DataValueField = "Id";
            ddlRefferenceId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlRefferenceId.Items.Insert(0, item);
        }
        private void SetDefaulTime()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.TimeFormat == "hh:mm tt")
            {
                txtProbableArrivalHour.Text = DateTime.Now.ToString(userInformationBO.TimeFormat.Replace(":mm ", " "));
                txtProbableDepartureHour.Text = DateTime.Now.AddHours(1).ToString(userInformationBO.TimeFormat.Replace(":mm ", " "));
            }
            else
            {
                txtProbableArrivalHour.Text = DateTime.Now.Hour.ToString();
                txtProbableDepartureHour.Text = DateTime.Now.AddHours(1).Hour.ToString();
            }
        }
        private void LoadSearchBanquetInfo()
        {
            BanquetInformationBO banquetBO = new BanquetInformationBO();
            BanquetInformationDA banquetDA = new BanquetInformationDA();
            var List = banquetDA.GetAllBanquetInformation();
            ddlSearchBanquetName.DataSource = List;
            ddlSearchBanquetName.DataTextField = "Name";
            ddlSearchBanquetName.DataValueField = "Id";
            ddlSearchBanquetName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSearchBanquetName.Items.Insert(0, item);
        }
        private void LoadOccassion()
        {
            BanquetOccessionTypeBO occessionBO = new BanquetOccessionTypeBO();
            BanquetOccessionTypeDA occessionDA = new BanquetOccessionTypeDA();
            var List = occessionDA.GetAllBanquetTheme();
            ddlOccessionTypeId.DataSource = List;
            ddlOccessionTypeId.DataTextField = "Name";
            ddlOccessionTypeId.DataValueField = "Id";
            ddlOccessionTypeId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlOccessionTypeId.Items.Insert(0, item);
        }
        private void LoadSeatingPlan()
        {
            BanquetSeatingPlanDA planDA = new BanquetSeatingPlanDA();
            var List = planDA.GetBanquetPlanInformation();
            ddlSeatingId.DataSource = List;
            ddlSeatingId.DataTextField = "Name";
            ddlSeatingId.DataValueField = "Id";
            ddlSeatingId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSeatingId.Items.Insert(0, item);
        }
        private void LoadCountry()
        {
            HMCommonDA commonDA = new HMCommonDA();
            var List = commonDA.GetAllCountries();
            ddlCountryId.DataSource = List;
            ddlCountryId.DataTextField = "CountryName";
            ddlCountryId.DataValueField = "CountryId";
            ddlCountryId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCountryId.Items.Insert(0, item);
        }
        public void LoadAffiliatedCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = guestCompanyDA.GetAffiliatedGuestCompanyInfo();
            ddlCompanyId.DataSource = files;
            ddlCompanyId.DataTextField = "CompanyName";
            ddlCompanyId.DataValueField = "CompanyId";
            ddlCompanyId.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstValue();
            ddlCompanyId.Items.Insert(0, itemReference);
        }
        private decimal CalculateGrandTotal(List<BanquetReservationDetailBO> salesDetailList)
        {
            int Count = salesDetailList.Count;
            decimal salesAmount = 0;
            for (int i = 0; i < Count; i++)
            {
                salesAmount = salesAmount + Convert.ToDecimal(salesDetailList[i].TotalPrice);
            }
            return salesAmount;
        }
        public string GenerateItemDetailTable(long reservationId, int costCenterId)
        {
            string strTable = "";
            var deleteLink = "";

            BanquetReservationDetailDA reservationDA = new BanquetReservationDetailDA();
            List<BanquetReservationDetailBO> detailList = new List<BanquetReservationDetailBO>();

            detailList = reservationDA.GetBanquetReservationDetailInfoByReservationId(reservationId, costCenterId);
            strTable += "<table id='RecipeItemInformation' class='table table-bordered table-condensed table-hover' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th align='left' scope='col' style='width: 20%;'>Item Name</th><th align='left' scope='col' style='width: 15%;'>Unit Price</th><th align='left' scope='col' style='width: 15%;'>Unit</th><th align='left' scope='col' style='width: 15%;'>Amount</th><th align='left' scope='col' style='width: 10%;'>Arrival Time</th><th align='left' scope='col' style='width: 15%;'>Description</th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";
            strTable += "<tbody>";
            int counter = 0;
            if (detailList != null)
            {
                foreach (BanquetReservationDetailBO dr in detailList)
                {
                    deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Edit\" src=\"../Images/delete.png\" /></a>";
                    counter++;

                    strTable += "<tr>";

                    if (dr.IsItemEditable)
                    {
                        strTable += "<td style=\"width:20%;\">";
                        strTable += "<input type=\"text\" id=\"itmname" + dr.ItemId + "\"class=\"form-control\" value=\"" + dr.ItemName + "\"/>";
                        strTable += "</td>";
                    }
                    else
                    {
                        strTable += "<td align='left' style=\"width:20%; text-align:Left;\">" + dr.ItemName + "</td>";
                    }

                    if (dr.IsItemEditable)
                    {
                        strTable += "<td style=\"width:15%;\">";
                        strTable += "<input type=\"text\" id=\"itmprice" + dr.ItemId + "\"class=\"form-control quantitydecimal\" value=\"" + dr.UnitPrice + "\" onblur=\"PriceQuantityChange(this, 'price')\" />";
                        strTable += "</td>";
                    }
                    else
                    {
                        strTable += "<td align='left' style=\"width:15%; text-align:Left;\">" + dr.UnitPrice + "</td>";
                    }

                    strTable += "<td style=\"width:15%;\">";
                    strTable += "<input type=\"text\" id=\"itmqtn" + dr.ItemId + "\"class=\"form-control quantitydecimal\" value=\"" + dr.ItemUnit + "\" onblur=\"PriceQuantityChange(this, 'quantity')\"/>";
                    strTable += "</td>";

                    if (dr.IsComplementary == true)
                    {
                        strTable += "<td align='left' style='width: 15%;'>" + (dr.ItemUnit * dr.TotalPrice).ToString("0.00") + "</td>"; // dr.TotalPrice
                    }
                    else
                    {
                        strTable += "<td align='left' style='width: 15%;'>" + (dr.ItemUnit * dr.UnitPrice).ToString("0.00") + "</td>"; // dr.TotalPrice
                    }

                    strTable += "<td align='left' style=\"width:10%; cursor:pointer;\">" + dr.ItemArrivalTime.ToString("h:mm tt") + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.Id + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.ReservationId + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.ItemTypeId + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.ItemType + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.ItemId + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.ItemWiseDiscountType + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.ItemWiseIndividualDiscount + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.ServiceCharge + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.CitySDCharge + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.VatAmount + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.AdditionalChargeType + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.AdditionalCharge + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.IsComplementary + "</td>";
                    strTable += "<td align='left' style=\"width:15%; cursor:pointer;\">" + dr.ItemDescription + "</td>";
                    strTable += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";
                    strTable += "</tr>";
                }
            }
            strTable += "</tbody>";
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='6' align='center'>No Record Available!</td></tr>";
            }

            return strTable;
        }
        public void GenerateConfirmationLetter(long reservationId)
        {
            string url = "/Banquet/Reports/frmReportReservationConLatter.aspx?ReservationId=" + reservationId;
            string sPopUp = "window.open('" + url + "', 'popup_window', 'width=790,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
            ClientScript.RegisterStartupScript(GetType(), "script", sPopUp, true);
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<GLProjectBO> GetGLProjectByGLCompanyId(int companyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(companyId, userInformationBO.UserGroupId);
            if (projectList.Count == 0)
                projectList.Add(entityDA.GetGLProjectInfoByGLCompany(companyId).FirstOrDefault());
            return projectList;
        }
        [WebMethod]
        public static GuestCompanyBO GetAffiliatedCompany(int companyId)
        {
            GuestCompanyBO companyBO = new GuestCompanyBO();
            GuestCompanyDA companyDA = new GuestCompanyDA();
            companyBO = companyDA.GetGuestCompanyInfoById(companyId);

            return companyBO;
        }
        [WebMethod]
        public static InvItemViewBO GetProductDataByCriteria(int categoryId, int costCenterId, string ddlItemId)
        {
            InvItemViewBO viewBO = new InvItemViewBO();
            InvItemDA itemDA = new InvItemDA();
            var obj = itemDA.GetInvItemPriceForBanquet(categoryId, costCenterId, Int32.Parse(ddlItemId));
            if (obj != null)
            {
                viewBO.UnitPriceLocal = obj.UnitPrice;
                viewBO.ItemId = obj.ItemId;
            }

            return viewBO;
        }
        [WebMethod]
        public static List<InvItemBO> GetInvItemByCategoryNCostCenter(int costCenterId, int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetDynamicallyItemInformationByCategoryId(costCenterId, CategoryId);

            return productList;
        }
        [WebMethod]
        public static string GetUploadedImageByWebMethod(int OwnerId, string docType)
        {
            string strTable = "";
            DocumentsDA docDA = new DocumentsDA();
            var docList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId(docType, OwnerId);
            if (docList.Count > 0)
            {
                var Image = docList[0];

                strTable += "<img src='" + Image.Path + Image.Name + "'  alt='No Image Selected' border='0' />";
            }
            return strTable;
        }
        [WebMethod]
        public static List<InvCategoryBO> LoadCategory(int costCenterId)
        {
            List<InvCategoryBO> categoryList = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            categoryList = da.GetCostCenterWiseInvItemCatagoryInfo("Product", costCenterId);

            List<InvCategoryBO> requisitesList = new List<InvCategoryBO>();
            InvCategoryBO requisitesBO = new InvCategoryBO();
            requisitesBO.CategoryId = 100000;
            requisitesBO.Name = "Requisites";
            requisitesBO.MatrixInfo = "Requisites";
            requisitesList.Add(requisitesBO);

            List<InvCategoryBO> List = new List<InvCategoryBO>();
            List.AddRange(requisitesList);
            List.AddRange(categoryList);

            return List;
        }
        [WebMethod]
        public static BanquetReservationDetailBO GetReservationDetailInfo(string detailId, string reservationId)
        {
            int dId = Convert.ToInt32(detailId);
            int rId = Convert.ToInt32(reservationId);
            BanquetReservationDetailBO detailBO = new BanquetReservationDetailBO();
            BanquetReservationDetailDA detailDA = new BanquetReservationDetailDA();
            detailBO = detailDA.GetBanquetReservationDetailInfoById(dId, rId);
            return detailBO;
        }
        [WebMethod]
        public static string GetBanquetReservationInfoForDuplicateChecking(int editId, int banquetId, string fromDate, string arriveTime, string departTime)
        {
            DateTime reservationDate;
            DateTime dateTime = DateTime.Now;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                reservationDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = hmUtility.GetStringFromDateTime(dateTime);
                reservationDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            DateTime arriveFromDate = dateTime;
            DateTime departToDate = dateTime.AddHours(1);
            if (userInformationBO.TimeFormat == "hh:mm tt")
            {
                arriveFromDate = Convert.ToDateTime(reservationDate.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(arriveTime).ToString("HH:mm"));
                departToDate = Convert.ToDateTime(reservationDate.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(departTime).ToString("HH:mm"));
            }
            else
            {
                arriveFromDate = Convert.ToDateTime(reservationDate.ToString("yyyy-MM-dd")).AddHours(Convert.ToInt32(arriveTime));
                departToDate = Convert.ToDateTime(reservationDate.ToString("yyyy-MM-dd")).AddHours(Convert.ToInt32(departTime));
            }
            List<BanquetReservationBO> detailBOList = new List<BanquetReservationBO>();
            BanquetReservationDA detailDA = new BanquetReservationDA();
            detailBOList = detailDA.GetBanquetReservationInfoForDuplicateChecking(editId, banquetId, arriveFromDate, departToDate);
            return detailBOList.Count > 0 ? "1" : "0";
        }
        [WebMethod]
        public static GridViewDataNPaging<BanquetReservationBO, GridPaging> SearchReservationAndLoad(string name, string reservationNo, string banquetId, string email, string phone, string arriveDate, string departDate, string isBanquetReservationRestictionForAllUser, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            DateTime? arrDate = null, depDate = null;
            int? banId = null;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            if (!string.IsNullOrWhiteSpace(arriveDate))
            {
                arrDate = hmUtility.GetDateTimeFromString(arriveDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(departDate))
            {
                depDate = hmUtility.GetDateTimeFromString(departDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (banquetId == "0")
            {
                banId = null;
            }
            else
            {
                banId = Convert.ToInt32(banquetId);
            }

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isBanquetReservationRestictionForAllUserBO = new HMCommonSetupBO();
            isBanquetReservationRestictionForAllUserBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetReservationRestictionForAllUser", "IsBanquetReservationRestictionForAllUser");

            GridViewDataNPaging<BanquetReservationBO, GridPaging> myGridData = new GridViewDataNPaging<BanquetReservationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            BanquetReservationDA reservationDA = new BanquetReservationDA();
            List<BanquetReservationBO> reservationList = new List<BanquetReservationBO>();
            reservationList = reservationDA.GetAllReservationList(name, reservationNo, email, phone, banId, arrDate, depDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            if (isBanquetReservationRestictionForAllUserBO != null)
            {
                if (isBanquetReservationRestictionForAllUserBO.SetupValue == "1")
                {
                    reservationList = reservationList.Where(x => x.CreatedBy == userInformationBO.UserInfoId).ToList();
                }

            }

            List<BanquetReservationBO> distinctItems = new List<BanquetReservationBO>();
            distinctItems = reservationList.GroupBy(test => test.Id).Select(group => group.First()).ToList();
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ArrayList GetBanquetInfoByCriteria(string ddlItemId)
        {
            BanquetInformationBO banquetBO = new BanquetInformationBO();
            BanquetInformationDA banquetDA = new BanquetInformationDA();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();

            banquetBO = banquetDA.GetBanquetInformationById(Int32.Parse(ddlItemId));
            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(banquetBO.CostCenterId);

            ArrayList arr = new ArrayList();
            arr.Add(banquetBO);
            arr.Add(costCentreTabBO);

            return arr;
        }
        [WebMethod]
        public CostCentreTabBO LoadCommonSetupForRackRateServiceChargeVatInformation()
        {
            CostCentreTabBO costCentre = new CostCentreTabBO();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupBO;

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetBillInclusive", "IsBanquetBillInclusive");
            hfIsBanquetBillInclusive.Value = commonSetupBO.SetupValue;

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("BanquetVatAmount", "BanquetVatAmount");
            hfBanquetVatAmount.Value = commonSetupBO.SetupValue;

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("BanquetServiceCharge", "BanquetServiceCharge");
            hfBanquetServiceCharge.Value = commonSetupBO.SetupValue;

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("CompanyCountryId", "CompanyCountryId");
            ddlCountryId.SelectedValue = commonSetupBO.SetupValue;

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetBillAmountWillRound", "IsBanquetBillAmountWillRound");
            hfIsBanquetBillAmountWillRound.Value = commonSetupBO.SetupValue;

            return costCentre;
        }
        [WebMethod]
        public static ReturnInfo ActiveReservation(string id)
        {
            long banquetId = Convert.ToInt64(id);
            ReturnInfo returnInfo = new ReturnInfo();

            BanquetReservationDA reservationDA = new BanquetReservationDA();
            BanquetReservationBO reservationBO = reservationDA.GetBanquetReservationInfoById(banquetId);

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            returnInfo.IsSuccess = false;
            returnInfo.IsReservationCheckInDateValidation = false;

            if (reservationBO.ArriveDate < DateTime.Now)
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo("Reservation Time has Passed, Cannot Active Reservation for this Date & Time.", AlertType.Warning);
                return returnInfo;
            }

            List<BanquetReservationBO> detailBOList = reservationDA.GetBanquetReservationInfoForDuplicateChecking((int)reservationBO.BanquetId, reservationBO.ArriveDate, reservationBO.DepartureDate);

            if (detailBOList.Count == 0)
            {
                try
                {
                    Boolean status = reservationDA.ActivateBanquetReservation(reservationBO.Id, userInformationBO.UserInfoId);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), reservationBO.Id,
                        ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetReservation));
                        returnInfo.IsSuccess = true;
                        returnInfo.IsReservationCheckInDateValidation = true;
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.ReservationActive, AlertType.Success);
                    }
                    else
                    {
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo("Desired Hall is Occupaid, Cannot Active Reservation.", AlertType.Warning);

            }
            return returnInfo;
        }

        [WebMethod]
        public static ReturnInfo CancelReservation(long Id, string reason)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            BanquetReservationDA reservationDA = new BanquetReservationDA();
            BanquetReservationBO reservationBO = reservationDA.GetBanquetReservationInfoById(Id);
            reservationBO.Reason = reason;
            reservationBO.Status = "Cancel";
            reservationBO.LastModifiedBy = userInformationBO.UserInfoId;
            status = reservationDA.UpdateBanquetReservationStatusInfo(reservationBO);

            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Cancel, AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), reservationBO.Id,
                        ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetReservation) + "Cancel");
            }
            return rtninf;
        }
    }
}
