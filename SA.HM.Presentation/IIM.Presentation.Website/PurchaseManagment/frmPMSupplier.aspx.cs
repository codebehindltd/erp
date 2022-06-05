using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using System.Net.Mail;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using System.Web.Services;
using Newtonsoft.Json;
using HotelManagement.Data.UserInformation;
using System.Text;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmPMSupplier : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        private int isNewChartOfAccountsHeadCreateForSupplier = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            isNewChartOfAccountsHeadCreateForSupplier = 1;
            HMCommonSetupBO commonSetupNewCOABO = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupCompanyBO = new HMCommonSetupBO();
            commonSetupNewCOABO = commonSetupDA.GetCommonConfigurationInfo("IsNewChartOfAccountsHeadCreateForSupplier", "IsNewChartOfAccountsHeadCreateForSupplier");

            commonSetupCompanyBO = commonSetupDA.GetCommonConfigurationInfo("IsSupplierDifferentWithGLCompany", "IsSupplierDifferentWithGLCompany");

            if (commonSetupNewCOABO != null)
            {
                isNewChartOfAccountsHeadCreateForSupplier = Convert.ToInt32(commonSetupNewCOABO.SetupValue);
            }
            if (!IsPostBack)
            {
                FileUpload();
                LoadSupplierContactType();
                if (commonSetupCompanyBO != null && Convert.ToInt32(commonSetupCompanyBO.SetupValue) == 1)
                {
                    LoadGlCompanyGridView();
                    CompanyInformationDiv.Visible = true;
                    hfIsSupplierDifferentWithGLCompany.Value = "1";
                }
                else
                {
                    CompanyInformationDiv.Visible = false;
                    hfIsSupplierDifferentWithGLCompany.Value = "0";
                }
            }

            IsSupplierCodeAutoGenerate();
            IsSupplierUserPanalEnable();
            CheckObjectPermission();
        }
        protected void gvSupplierInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int SupplierId = 0;
            if (e.CommandName == "CmdEdit")
            {
                SupplierId = Convert.ToInt32(e.CommandArgument.ToString());
                PMSupplierBO supplierBO = new PMSupplierBO();
                List<PMSupplierBO> ContactPersonsList = new List<PMSupplierBO>();
                PMSupplierDA supplierDA = new PMSupplierDA();
                supplierBO = supplierDA.GetPMSupplierInfoById(SupplierId);
                this.FillForm(supplierBO);

                UserInformationDA userInformationDA = new UserInformationDA();
                UserInformationBO userInformation = userInformationDA.GetUserInformationBySupplierId(SupplierId);
                if (userInformation.UserInfoId != 0)
                {
                    txtSupplierUserId.Text = userInformation.UserId;
                    txtSupplierUserInfoId.Value = userInformation.UserInfoId.ToString();
                }

                ContactPersonsList = supplierDA.GetPMSupplierInfoDetailsById(SupplierId);
                hfContactPersons.Value = JsonConvert.SerializeObject(ContactPersonsList);
                btnSave.Visible = isUpdatePermission;
                LoadCompanyInfo(SupplierId);
                btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                SupplierId = Convert.ToInt32(e.CommandArgument.ToString());
                string result = string.Empty;

                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PMSupplier", "SupplierId", SupplierId);
                status = hmCommonDA.DeleteInfoById("PMSupplierDetails", "SupplierId", SupplierId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.Supplier.ToString(), SupplierId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Supplier));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    this.isNewAddButtonEnable = 2;
                    LoadGridView();

                }
                this.SetTab("SearchTab");
            }
        }
        protected void gvSupplierInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvSupplierInfo.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvSupplierInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                ImageButton ImgSelect = (ImageButton)e.Row.FindControl("ImgSelect");
                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";

                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }

            int OwnerIdForDocuments = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            PMSupplierBO supplierBO = new PMSupplierBO();
            PMSupplierDA supplierDA = new PMSupplierDA();
            GLCompanyBO companyBO = new GLCompanyBO();
            GLCompanyDA companyDA = new GLCompanyDA();
            List<GLCompanyBO> company = new List<GLCompanyBO>();
            supplierBO.Phone = txtPhone.Text;
            supplierBO.Name = txtName.Text;
            supplierBO.SupplierTypeId = ddlSupplierType.SelectedValue;
            supplierBO.Fax = txtFax.Text;
            supplierBO.Email = txtEmail.Text;
            supplierBO.Code = txtCode.Text;
            supplierBO.Address = txtAddress.Text;
            supplierBO.Remarks = txtRemarks.Text;
            supplierBO.ContactPerson = txtContactPerson.Text;
            supplierBO.ContactPhone = txtContactPhone.Text;
            supplierBO.ContactType = ddlContactType.SelectedValue;

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupNewCOABO = new HMCommonSetupBO();
            commonSetupNewCOABO = commonSetupDA.GetCommonConfigurationInfo("IsNewChartOfAccountsHeadCreateForSupplier", "IsNewChartOfAccountsHeadCreateForSupplier");

            List<string> comIdList = new List<string>();
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            int rowCompany = 0;

            if (hfIsSupplierDifferentWithGLCompany.Value == "1")
            {
                rowCompany = glCompany.Rows.Count;

                for (int i = 0; i < rowCompany; i++)
                {
                    CheckBox cb = (CheckBox)glCompany.Rows[i].FindControl("chkIsSavePermission");
                    if (cb.Checked == true)
                    {
                        GLCompanyBO com = new GLCompanyBO();
                        Label lbl = (Label)glCompany.Rows[i].FindControl("lblCompanyId");

                        com.CompanyId = Convert.ToInt32(string.IsNullOrEmpty(lbl.Text) ? "0" : lbl.Text);

                        if (com.CompanyId > 0)
                        {
                            comIdList.Add(com.CompanyId.ToString());
                        }
                        companyList.Add(com);
                    }
                }
            }
            else
            {
                List<GLCompanyBO> glCompanyListBO = new List<GLCompanyBO>();
                GLCompanyDA glCompanyDA = new GLCompanyDA();
                glCompanyListBO = companyDA.GetAllGLCompanyInfo();
                if (glCompanyListBO != null)
                {
                    rowCompany = glCompanyListBO.Count();
                }

                foreach (GLCompanyBO row in glCompanyListBO)
                {
                    if (row.CompanyId > 0)
                    {
                        comIdList.Add(row.CompanyId.ToString());
                    }
                    companyList.Add(row);
                }
            }

            string commaSeperatedCompanyIds = string.Join(",", comIdList);

            supplierBO.CompanyCommaSeperatedIds = commaSeperatedCompanyIds;

            if (commonSetupNewCOABO != null)
            {
                isNewChartOfAccountsHeadCreateForSupplier = Convert.ToInt32(commonSetupNewCOABO.SetupValue);
            }

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SupplierAccountsHeadId", "SupplierAccountsHeadId");
            int ancestorId = Convert.ToInt32(Convert.ToInt32(Application["CompanyAccountInfoForSalesBillPayment"].ToString()));
            if (commonSetupBO != null)
            {
                ancestorId = Convert.ToInt32(commonSetupBO.SetupValue);
            }
            List<PMSupplierBO> contactInformation = new List<PMSupplierBO>();
            List<PMSupplierBO> contactInformationDeleted = new List<PMSupplierBO>();
            contactInformation = (JsonConvert.DeserializeObject<List<PMSupplierBO>>(hfContactPersons.Value));
            contactInformationDeleted = (JsonConvert.DeserializeObject<List<PMSupplierBO>>(hfContactPersonsDeleted.Value));

            UserInformationBO userInformation = new UserInformationBO();
            UserInformationDA userInformationDA = new UserInformationDA();
            if (hfIsSupplierUserPanalEnable.Value == "1")
            {
                UserInformationBO currentUserInformationBO = new UserInformationBO();
                currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                int supplierUserGroupId = 5000;
                HMCommonSetupBO supplierUserGroupIdBO = new HMCommonSetupBO();
                supplierUserGroupIdBO = commonSetupDA.GetCommonConfigurationInfo("SupplierUserGroupId", "SupplierUserGroupId");
                if (supplierUserGroupIdBO != null)
                {
                    supplierUserGroupId = Convert.ToInt32(commonSetupBO.SetupValue);
                }

                userInformation.UserName = supplierBO.Name;
                userInformation.UserId = txtSupplierUserId.Text.Trim();
                userInformation.UserPassword = txtUserPassword.Text.Trim();
                userInformation.UserGroupId = supplierUserGroupId;
                userInformation.UserEmail = supplierBO.Email;
                userInformation.UserPhone = supplierBO.Phone;
                userInformation.UserDesignation = "Supplier";
                userInformation.ActiveStat = true;
                userInformation.IsAdminUser = false;
                userInformation.EmpId = 0;
                userInformation.CreatedBy = currentUserInformationBO.UserInfoId;
            }

            //Save Block
            if (string.IsNullOrWhiteSpace(txtSupplierId.Value))
            {
                #region Save Block Code
                if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 0) > 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Supplier Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                    this.txtName.Focus();
                    return;
                }
                if (hfIsSupplierCodeAutoGenerate.Value == "0")
                {
                    if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 0) > 0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Code" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtCode.Focus();
                        return;
                    }
                }

                if (hfIsSupplierUserPanalEnable.Value == "1")
                {
                    int IsDuplicate = 0;
                    HMCommonDA hmCommonDA = new HMCommonDA();
                    IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly("SecurityUserInformation", "UserId", userInformation.UserId, 0, "UserInfoId", "");

                    if (IsDuplicate == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "User Id Already Exist.", AlertType.Warning);
                        txtSupplierUserId.Focus();
                        return;
                    }
                }



                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();

                int tmpSupplierId = 0;
                supplierBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = supplierDA.SavePMSupplierInfo(supplierBO, contactInformation, contactInformationDeleted, out tmpSupplierId);
                //Boolean status = supplierDA.SavePMSupplierInfo(supplierBO, contactInformation, contactInformationDeleted, out tmpSupplierId);
                if (status)
                {
                    if (hfIsSupplierUserPanalEnable.Value == "1")
                    {
                        int tmpUserInfoId = 0;
                        userInformation.SupplierId = Convert.ToInt32(tmpSupplierId);
                        Boolean statusUser = userInformationDA.SaveUserInformation(userInformation, adminAuthorizationList, out tmpUserInfoId);
                    }

                    //document upload
                    if (status)
                    {
                        DocumentsDA documentsDA = new DocumentsDA();
                        OwnerIdForDocuments = Convert.ToInt32(tmpSupplierId);
                        long RandomId = HttpContext.Current.Session["SupplierDocId"] != null ? Convert.ToInt64(HttpContext.Current.Session["SupplierDocId"]) : 0;

                        string s = hfDeletedDoc.Value.ToString();
                        string[] DeletedDocList = s.Split(',');
                        for (int i = 0; i < DeletedDocList.Length; i++)
                        {
                            DeletedDocList[i] = DeletedDocList[i].Trim();
                            Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                        }

                        Boolean updateStatus = new HMCommonDA().UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(RandomId));
                        Boolean logStatusDoc = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.Supplier.ToString(), tmpSupplierId,
                                    ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Supplier));
                    }

                    int tmpNodeId = 0;

                    if (isNewChartOfAccountsHeadCreateForSupplier != 0)
                    {
                        this.CreadeNodeMatrixAccountHeadInfo(ancestorId, out tmpNodeId);
                    }
                    else
                    {
                        tmpNodeId = ancestorId;
                    }

                    Boolean postingStatus = supplierDA.UpdateSupplierNAccountsInfo(tmpSupplierId, tmpNodeId, isNewChartOfAccountsHeadCreateForSupplier);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.Supplier.ToString(), tmpSupplierId,
                            ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Supplier));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    this.isNewAddButtonEnable = 2;
                    this.Cancel();
                }
                #endregion
            }
            else  //Update Block
            {

                supplierBO.SupplierId = Convert.ToInt32(txtSupplierId.Value);
                supplierBO.LastModifiedBy = userInformationBO.UserInfoId;

                if (hfIsSupplierUserPanalEnable.Value == "1")
                {
                    userInformation.SupplierId = supplierBO.SupplierId;
                    if (string.IsNullOrWhiteSpace(txtSupplierUserInfoId.Value))
                    {
                        int IsDuplicate = 0;
                        HMCommonDA hmCommonDA = new HMCommonDA();
                        IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly("SecurityUserInformation", "UserId", userInformation.UserId, 0, "UserInfoId", "");

                        if (IsDuplicate == 1)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "User Id Already Exist.", AlertType.Warning);
                            txtSupplierUserId.Focus();
                            return;
                        }
                    }
                    else
                    {
                        int IsDuplicate = 0;
                        HMCommonDA hmCommonDA = new HMCommonDA();
                        IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly("SecurityUserInformation", "UserId", userInformation.UserId, 1, "UserInfoId", txtSupplierUserInfoId.Value);

                        if (IsDuplicate == 1)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "User Id Already Exist.", AlertType.Warning);
                            txtSupplierUserId.Focus();
                            return;
                        }
                    }
                }

                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                Boolean status = supplierDA.UpdatePMSupplierInfo(supplierBO, contactInformation, contactInformationDeleted, isNewChartOfAccountsHeadCreateForSupplier);
                if (status)
                {
                    if (hfIsSupplierUserPanalEnable.Value == "1")
                    {
                        if (string.IsNullOrWhiteSpace(txtSupplierUserInfoId.Value))
                        {
                            int tmpUserInfoId = 0;
                            Boolean statusUser = userInformationDA.SaveUserInformation(userInformation, adminAuthorizationList, out tmpUserInfoId);
                        }
                        else
                        {
                            userInformation.UserInfoId = Convert.ToInt32(txtSupplierUserInfoId.Value);
                            userInformation.LastModifiedBy = userInformationBO.UserInfoId;
                            Boolean statusUser = userInformationDA.UpdateUserInformation(userInformation, adminAuthorizationList);
                        }
                    }

                    //document upload
                    if (status)
                    {
                        OwnerIdForDocuments = Convert.ToInt32(supplierBO.SupplierId);
                        long RandomId = HttpContext.Current.Session["SupplierDocId"] != null ? Convert.ToInt64(HttpContext.Current.Session["SupplierDocId"]) : 0;

                        DocumentsDA documentsDA = new DocumentsDA();

                        string s = hfDeletedDoc.Value.ToString();
                        string[] DeletedDocList = s.Split(',');
                        for (int i = 0; i < DeletedDocList.Length; i++)
                        {
                            DeletedDocList[i] = DeletedDocList[i].Trim();
                            Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                        }

                        Boolean updateStatus = new HMCommonDA().UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(RandomId));
                        Boolean logStatusDoc = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.Supplier.ToString(), supplierBO.SupplierId,
                                    ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Supplier));
                    }
                    if (isNewChartOfAccountsHeadCreateForSupplier != 0)
                    {
                        this.UpdateNodeMatrixAccountHeadInfo(Convert.ToInt32(this.txtNodeId.Value));
                    }

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.Supplier.ToString(), supplierBO.SupplierId,
                            ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Supplier));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    this.isNewAddButtonEnable = 2;
                    this.Cancel();
                }
            }
        }
        //************************ User Defined Function ********************//
        private void LoadSupplierContactType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("SupplierContactType");

            ddlContactType.DataSource = fields;
            ddlContactType.DataTextField = "Description";
            ddlContactType.DataValueField = "FieldValue";
            ddlContactType.DataBind();

            ListItem itemSupplier = new ListItem();
            itemSupplier.Value = "0";
            itemSupplier.Text = hmUtility.GetDropDownFirstValue();
            this.ddlContactType.Items.Insert(0, itemSupplier);
        }


        private void LoadGlCompanyGridView()
        {
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            GLCompanyDA companyDA = new GLCompanyDA();
            companyList = companyDA.GetAllGLCompanyInfo();
            glCompany.DataSource = companyList;
            glCompany.DataBind();

        }

        private void LoadCompanyInfo(int EditId)
        {
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            GLCompanyDA companyDA = new GLCompanyDA();
            companyList = companyDA.GetCompanyBySupplierId(EditId);
            int rowsCompany = glCompany.Rows.Count;

            //Clear up all the check boxes.
            for (int i = 0; i < rowsCompany; i++)
            {
                CheckBox cb = (CheckBox)glCompany.Rows[i].FindControl("chkIsSavePermission");
                cb.Checked = false;
            }

            for (int i = 0; i < rowsCompany; i++)
            {
                Label lbl = (Label)glCompany.Rows[i].FindControl("lblCompanyId");
                int CompanyId = Int32.Parse(lbl.Text);
                GLCompanyBO boInList = companyList.Find(x => x.CompanyId == CompanyId);
                if (boInList != null)
                {
                    if (CompanyId == boInList.CompanyId)
                    {
                        CheckBox cb = (CheckBox)glCompany.Rows[i].FindControl("chkIsSavePermission");
                        cb.Checked = true;
                    }
                }
            }
        }

        [WebMethod]
        public static int ChangeRandomId()
        {
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            HttpContext.Current.Session["SupplierDocId"] = randomId;
            return randomId;
        }
        [WebMethod]
        public static string GetDocumentsByUserTypeAndUserId(string Id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();
            DocumentsDA docDA = new DocumentsDA();
            docList = docDA.GetDocumentsByUserTypeAndUserId("SupplierDocuments", Int32.Parse(Id));
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg")
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
                else
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:100px; height:100px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 100px; height: 100px;' src='" + dr.IconImage + "' alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td align='center'>No Record Available!</td></tr>";
            }
            return strTable;
        }
        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocByWebMethod(int randomId, int id, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }
            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("SupplierDocuments", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SupplierDocuments", (int)id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));
            foreach (DocumentsBO dc in docList)
            {

                if (dc.DocumentType == "Image")
                    dc.Path = (dc.Path + dc.Name);

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }
            docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            return docList;
        }
        private void IsSupplierCodeAutoGenerate()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsSupplierCodeAutoGenerate", "IsSupplierCodeAutoGenerate");
            hfIsSupplierCodeAutoGenerate.Value = homePageSetupBO.SetupValue;
        }
        private void IsSupplierUserPanalEnable()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsSupplierUserPanalEnable", "IsSupplierUserPanalEnable");
            hfIsSupplierUserPanalEnable.Value = homePageSetupBO.SetupValue;
        }
        private void Cancel()
        {
            this.txtCode.Enabled = true;
            this.txtName.Text = string.Empty;
            ddlSupplierType.SelectedValue = "0";
            hfIsEdit.Value = "0";
            hfSupplierId.Value = "0";
            hfClearContactForSupplierTbl.Value = "1";
            this.txtSupplierId.Value = string.Empty;
            this.txtSupplierUserInfoId.Value = string.Empty;
            this.txtSupplierUserId.Text = string.Empty;
            hfContactPersons.Value = string.Empty;
            hfContactPersonsDeleted.Value = string.Empty;
            this.txtAddress.Text = string.Empty;
            this.txtCode.Text = string.Empty;
            this.txtContactPerson.Text = string.Empty;
            this.txtPhone.Text = string.Empty;
            this.txtFax.Text = string.Empty;
            this.txtEmail.Text = string.Empty;
            txtContactEmail.Text = string.Empty;
            txtContactPhone.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtName.Focus();

            if (hfIsSupplierDifferentWithGLCompany.Value == "1")
            {
                int rowsCompany = glCompany.Rows.Count;
                CheckBox ChkBoxHeader = (CheckBox)glCompany.HeaderRow.FindControl("ChkCreate");
                ChkBoxHeader.Checked = false;
                //Clear up all the check boxes.
                for (int i = 0; i < rowsCompany; i++)
                {
                    CheckBox cb = (CheckBox)glCompany.Rows[i].FindControl("chkIsSavePermission");
                    cb.Checked = false;
                }
            }
        }
        private void FileUpload()
        {
            Random rd = new Random();
            int seatingId = rd.Next(100000, 999999);
            RandomDocId.Value = seatingId.ToString();
            tempDocId.Value = seatingId.ToString();
            HttpContext.Current.Session["SupplierDocId"] = RandomDocId.Value;
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
        }
        private void LoadGridView()
        {
            List<PMSupplierBO> supplierList = new List<PMSupplierBO>();
            PMSupplierDA supplierDA = new PMSupplierDA();
            supplierList = supplierDA.GetSupplierInfoBySearchCriteria(txtSearchCode.Text, txtSearchContact.Text, txtSearchEmail.Text, txtSearchName.Text, txtSearchPhone.Text, ddlSrcSupplierType.SelectedValue);
            if (ddlSrcSupplierType.SelectedValue != "0")
            {
                supplierList = supplierList.Where(x => x.SupplierTypeId == ddlSrcSupplierType.SelectedValue).ToList();
            }
            this.CheckObjectPermission();
            gvSupplierInfo.DataSource = supplierList;
            gvSupplierInfo.DataBind();
            SetTab("SearchTab");
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
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            //bool isContactEmail = false;
            if (string.IsNullOrEmpty(txtName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Supplier Name.", AlertType.Warning);
                txtName.Focus();
                flag = false;
            }
            //else if (string.IsNullOrEmpty(txtEmail.Text))
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Email Id.", AlertType.Warning);
            //    flag = false;
            //}
            //try
            //{
            //    if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            //    {
            //        string address = new MailAddress(txtEmail.Text).Address;
            //    }
            //    else
            //    {
            //        CommonHelper.AlertInfo(innboardMessage, "Email is not in correct format.", AlertType.Warning);
            //        txtEmail.Focus();
            //        flag = false;
            //    }

            //    if (!string.IsNullOrWhiteSpace(txtContactEmail.Text))
            //    {
            //        isContactEmail = true;
            //        string cAddress = new MailAddress(txtContactEmail.Text).Address;
            //    }
            //}
            //catch (FormatException)
            //{
            //    if (isContactEmail)
            //    {
            //        CommonHelper.AlertInfo(innboardMessage, "Contact Email is not in correct format.", AlertType.Warning);
            //        txtContactEmail.Focus();
            //        flag = false;
            //    }
            //    else
            //    {
            //        CommonHelper.AlertInfo(innboardMessage, "Email is not in correct format.", AlertType.Warning);
            //        txtEmail.Focus();
            //        flag = false;
            //    }
            //}
            return flag;
        }
        private void FillForm(PMSupplierBO supplierBO)
        {
            hfIsEdit.Value = "1";
            hfSupplierId.Value = supplierBO.SupplierId.ToString();
            txtCode.Enabled = true;
            txtNodeId.Value = supplierBO.NodeId.ToString();
            txtPhone.Text = supplierBO.Phone;
            txtName.Text = supplierBO.Name;
            ddlSupplierType.SelectedValue = supplierBO.SupplierTypeId;
            txtFax.Text = supplierBO.Fax;
            txtEmail.Text = supplierBO.Email;
            txtCode.Text = supplierBO.Code;
            txtAddress.Text = supplierBO.Address;
            txtSupplierId.Value = supplierBO.SupplierId.ToString();
            txtRemarks.Text = supplierBO.Remarks;
            txtContactPerson.Text = supplierBO.ContactPerson;
            txtContactPhone.Text = supplierBO.ContactPhone;

        }
        private void CreadeNodeMatrixAccountHeadInfo(int AncestorId, out int NodeId)
        {
            NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            // Need to Fix----------------------------------
            nodeMatrixBO.AncestorId = AncestorId;
            nodeMatrixBO.NodeHead = this.txtName.Text.Trim();
            nodeMatrixBO.NodeMode = true;
            Boolean status = nodeMatrixDA.SaveNodeMatrixInfoFromOtherPage(nodeMatrixBO, out NodeId);
        }
        private void UpdateNodeMatrixAccountHeadInfo(int nodeId)
        {
            NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            nodeMatrixBO.NodeId = nodeId;
            nodeMatrixBO.NodeHead = this.txtName.Text.Trim();
            nodeMatrixBO.NodeMode = true;
            Boolean status = nodeMatrixDA.UpdateNodeMatrixInfoFromOtherPage(nodeMatrixBO);
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "PMSupplier";
            string pkFieldName = "SupplierId";
            string pkFieldValue = txtSupplierId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
    }
}