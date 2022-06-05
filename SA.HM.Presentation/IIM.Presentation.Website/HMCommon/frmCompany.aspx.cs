using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmCompany : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            string jscript = "function UploadComplete(){ GetUploadedImage();};";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            flashUpload.QueryParameters = "guestId=" + Server.UrlEncode("-1");

            if (!IsPostBack)
            {
                this.CheckObjectPermission();
                this.LoadCompanyForm();
                this.txtCompanyName.Focus();
                this.LoadCompanyBankForm();
                this.txtBankName.Focus();
            }

        }
        protected void btnSaveCompany_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {

                return;
            }
            
            CompanyBO companyBO = new CompanyBO();
            CompanyDA companyDA = new CompanyDA();

            companyBO.CompanyName = this.txtCompanyName.Text;
            companyBO.CompanyAddress = this.txtCompanyAddress.Text;
            companyBO.EmailAddress = this.txtEmailAddress.Text;
            companyBO.WebAddress = this.txtWebAddress.Text;
            companyBO.ContactNumber = this.txtContactNumber.Text;
            companyBO.ContactPerson = this.txtContactPerson.Text;
            companyBO.VatRegistrationNo = this.txtVatRegistrationNo.Text;
            companyBO.TinNumber = this.txtTinNumber.Text;
            companyBO.Remarks = this.txtRemarks.Text;
            companyBO.CompanyCode = this.txtCompanyCode.Text;

            if (string.IsNullOrWhiteSpace(txtCompanyId.Value))
            {
                int tmpCompanyId = 0;
                Boolean status = companyDA.SaveCompanyInfo(companyBO, out tmpCompanyId);
                if (status)
                {
                    this.SaveOrUpdateCommonCustomFieldDataInfo();
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.Company.ToString(), tmpCompanyId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Company));
                    this.LoadCompanyForm();
                }
            }
            else
            {
                companyBO.CompanyId = Convert.ToInt32(txtCompanyId.Value);
                Boolean status = companyDA.UpdateCompanyInfo(companyBO);
                if (status)
                {
                    this.SaveOrUpdateCommonCustomFieldDataInfo();
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.Company.ToString(), companyBO.CompanyId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Company));
                    this.LoadCompanyForm();
                }
            }

            this.SetTab("CompanyTab");
        }
        protected void btnSaveCompanyBank_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {

                return;
            }
            
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            CompanyBankBO companyBO = new CompanyBankBO();
            CompanyBankDA companyDA = new CompanyBankDA();
            companyBO.BankName = this.txtBankName.Text;
            companyBO.AccountName = this.txtAccountName.Text;
            companyBO.AccountNo1 = this.txtAccountNo1.Text;
            companyBO.AccountNo2 = this.txtAccountNo2.Text;
            companyBO.BranchName = this.txtBranchName.Text;
            companyBO.SwiftCode = this.txtSwiftCode.Text;


            if (string.IsNullOrWhiteSpace(txtBankId.Value))
            {
                int tmpBankId = 0;
                companyBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = companyDA.SaveCompanyBankInfo(companyBO, out tmpBankId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.CompanyBank.ToString(), tmpBankId,
                    ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CompanyBank));
                    this.LoadCompanyBankForm();
                }
            }
            else
            {
                companyBO.BankId = Convert.ToInt32(txtBankId.Value);
                companyBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = companyDA.UpdateCompanyBankInfo(companyBO);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.CompanyBank.ToString(), companyBO.BankId,
                    ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CompanyBank));    
                    this.LoadCompanyBankForm();
                }
            }
            this.SetTab("BankTab");
        }
        //************************ User Defined Function ********************//
        private void LoadCompanyBankForm()
        {
            CompanyBankDA companyDA = new CompanyBankDA();
            List<CompanyBankBO> List = new List<CompanyBankBO>();
            List = companyDA.GetCompanyBankInfo();
            if (List.Count > 0)
            {
                if (List[0].BankId > 0)
                {
                    this.txtBankName.Text = List[0].BankName;
                    this.txtBranchName.Text = List[0].BranchName;
                    this.txtAccountName.Text = List[0].AccountName;
                    this.txtSwiftCode.Text = List[0].SwiftCode;
                    this.txtAccountNo1.Text = List[0].AccountNo1;
                    this.txtAccountNo2.Text = List[0].AccountNo2;
                    this.txtBankId.Value = List[0].BankId.ToString();
                    btnSaveCompanyBank.Text = "Update";

                }
            }
        }
        public bool isValidFormBank()
        {
            bool status = true;
            if (string.IsNullOrWhiteSpace(this.txtBankName.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Bank Name.", AlertType.Warning);
                this.txtBankName.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtBranchName.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Branch Name.", AlertType.Warning);
                txtBranchName.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtAccountName.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Account Name.", AlertType.Warning);
                txtAccountName.Focus();
            }
            else if (string.IsNullOrEmpty(this.txtSwiftCode.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Swift Code.", AlertType.Warning);
                txtSwiftCode.Focus();
            }
            else if (string.IsNullOrEmpty(this.txtAccountNo1.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Account Number 1.", AlertType.Warning);
                txtAccountNo1.Focus();
            }
            else if (string.IsNullOrEmpty(this.txtAccountNo2.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Account Number 2.", AlertType.Warning);
                txtAccountNo2.Focus();
            }

            return status;

        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmCompany.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSaveCompany.Visible = isSavePermission;
            btnSaveCompanyBank.Visible = isSavePermission;

        }
        private void LoadCompanyForm()
        {
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files.Count > 0)
            {
                if (files[0].CompanyId > 0)
                {
                    this.txtCompanyName.Text = files[0].CompanyName;
                    this.txtCompanyAddress.Text = files[0].CompanyAddress;
                    this.txtContactNumber.Text = files[0].ContactNumber;
                    this.txtContactPerson.Text = files[0].ContactPerson;
                    this.txtEmailAddress.Text = files[0].EmailAddress;
                    this.txtVatRegistrationNo.Text = files[0].VatRegistrationNo;
                    this.txtTinNumber.Text = files[0].TinNumber;
                    this.txtRemarks.Text = files[0].Remarks;
                    this.txtWebAddress.Text = files[0].WebAddress;
                    this.txtCompanyCode.Text = files[0].CompanyCode;
                    this.txtCompanyId.Value = files[0].CompanyId.ToString();
                    btnSaveCompany.Text = "Update";
                }
            }
        }
        public bool isValidForm()
        {
            bool status = true;
            if (string.IsNullOrWhiteSpace(this.txtCompanyName.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Company Name.", AlertType.Warning);
                this.txtCompanyName.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtCompanyAddress.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Company Address.", AlertType.Warning);
                txtCompanyAddress.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtContactNumber.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Contact Number.", AlertType.Warning);
                txtContactNumber.Focus();
            }
            else if (!string.IsNullOrWhiteSpace(this.txtContactNumber.Text.Trim()))
            {
                var match = Regex.Match(txtContactNumber.Text.Trim(), @"^(?:(?:\(?(?:00|\+0)([1-4]\d\d|[1-9]\d?)\)?)?[\-\.\ \\\/]?)?((?:\(?\d{1,}\)?[\-\.\ \\\/]?){0,})(?:[\-\.\ \\\/]?(?:#|ext\.?|extension|x)[\-\.\ \\\/]?(\d+))?$");
                if (!match.Success)
                {
                    txtContactNumber.Focus();
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid Contact Number.", AlertType.Warning);
                    return status = false;
                }
            }
            else if (!string.IsNullOrWhiteSpace(this.txtEmailAddress.Text.Trim()))
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (!re.IsMatch(txtEmailAddress.Text.Trim()))
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid Email Address.", AlertType.Warning);
                    txtEmailAddress.Focus();
                    status = false;
                }
            }

            return status;

        }
        private int ValidatePhone(string PhoneNumber)
        {
            int status = 1;
            if (PhoneNumber[0] == '+')
            {
                PhoneNumber = PhoneNumber.Substring(1, PhoneNumber.Length - 1);
            }
            for (int i = 0; i < PhoneNumber.Length; i++)
            {
                if (char.IsDigit(PhoneNumber, i) == false)
                {
                    status = 0;
                }

            }
            return status;
        }
        private void SetTab(string TabName)
        {

            if (TabName == "CompanyTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");

            }
            else if (TabName == "BankTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }

        }
        private void SaveOrUpdateCommonCustomFieldDataInfo()
        {
            List<HMCommonSetupBO> commonSetupBOList = new List<HMCommonSetupBO>();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO fieldBO = new CustomFieldBO();
            fieldBO = commonDA.GetCustomFieldByFieldName("paramCLAppreciationMessege");
            if (!string.IsNullOrEmpty(fieldBO.FieldValue))
            {
                commonSetupBO.FieldId = fieldBO.FieldId;
                commonSetupBO.FieldType = "paramCLAppreciationMessege";
                commonSetupBO.FieldValue = this.txtCompanyName.Text;
                commonSetupBO.ActiveStat = true;
                commonSetupBOList.Add(commonSetupBO);

                Boolean statusCustomInfo = commonSetupDA.SaveOrUpdateCommonCustomFieldDataInfo(commonSetupBOList);
            }
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static string GetCompanyProfileImage()
        {
            CompanyDA companyDA = new CompanyDA();
            var Company=companyDA.GetCompanyInfo();
            string strTable = string.Empty;
            if (Company.Count > 0)
            {
                string ImageSource = Company[0].ImagePath + Company[0].ImageName;

                strTable += "<img src='" + ImageSource + "'  alt='No Image Selected' border='0' />";
            }
            return strTable;
        }
    }
}