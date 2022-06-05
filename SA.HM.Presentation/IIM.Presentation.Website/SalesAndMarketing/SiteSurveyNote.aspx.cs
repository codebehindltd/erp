using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class SiteSurveyNote : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomDocId.Value = seatingId.ToString();
                tempDocId.Value = seatingId.ToString();
                //hfId.Value = "0";
                hfParentDoc.Value = "0";
                FileUpload();

                LoadCompany();
                LoadSurveyFor();
                LoadCompanyIndependentContacts();
            }
        }
        private void LoadCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = new List<GuestCompanyBO>();
            files = guestCompanyDA.GetALLGuestCompanyInfo();

            ddlCompany.DataSource = files;
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCompany.Items.Insert(0, item);
        }
        private void LoadSurveyFor()
        {
            SetupDA DA = new SetupDA();
            List<SMSegmentInformationBO> sMSegments = new List<SMSegmentInformationBO>();
            sMSegments = DA.GetAllSegmentInformation();
            sMSegments = sMSegments.Where(p => p.Status == true).ToList();
            ddlSurveyFor.DataSource = sMSegments;
            ddlSurveyFor.DataTextField = "SegmentName";
            ddlSurveyFor.DataValueField = "Id";
            ddlSurveyFor.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSurveyFor.Items.Insert(0, item);
        }
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            flashUpload.QueryParameters = "SiteSurveyDocId=" + Server.UrlEncode(RandomDocId.Value);
        }
        private void LoadCompanyIndependentContacts()
        {
            List<ContactInformationBO> contacts = new List<ContactInformationBO>();
            ContactInformationDA contactDA = new ContactInformationDA();
            contacts = contactDA.GetContactInformation().Where(x => x.CompanyId == 0).ToList();

            ddlContact.DataSource = contacts;
            ddlContact.DataTextField = "Name";
            ddlContact.DataValueField = "Id";
            ddlContact.DataBind();

            if (contacts.Count > 1)
            {
                ListItem item = new ListItem();
                item.Value = "0";
                item.Text = hmUtility.GetDropDownFirstValue();
                ddlContact.Items.Insert(0, item);
            }

        }
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    int OwnerIdForDocuments = 0;
        //    bool status = false;

        //    HMUtility hmUtility = new HMUtility();
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
        //    SMSiteSurveyNoteBO sMSiteSurveyNoteBO = new SMSiteSurveyNoteBO();
        //    sMSiteSurveyNoteBO.Id = Convert.ToInt32(hfId.Value);
        //    sMSiteSurveyNoteBO.CompanyId = Convert.ToInt32(ddlCompany.SelectedValue);
        //    sMSiteSurveyNoteBO.Address = "";
        //    sMSiteSurveyNoteBO.DealId = Convert.ToInt64(hfSelectedDealId.Value);

        //    sMSiteSurveyNoteBO.IsDealNeedSiteSurvey = NoSiteSurvey.Checked;
        //    if(!sMSiteSurveyNoteBO.IsDealNeedSiteSurvey)
        //    {
        //        sMSiteSurveyNoteBO.SegmentId = Convert.ToInt32(ddlSurveyFor.SelectedValue);
        //        sMSiteSurveyNoteBO.Description = txtDescription.Text;
        //    }
        //    if (sMSiteSurveyNoteBO.Id == 0)
        //    {
        //        sMSiteSurveyNoteBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
        //    }
        //    else
        //    {
        //        sMSiteSurveyNoteBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
        //    }

        //    long OutId;
        //    SiteSurveyNoteDA DA = new SiteSurveyNoteDA();

        //    status = DA.SaveUpdateSiteSurveyNote(sMSiteSurveyNoteBO, out OutId);
        //    if (status)
        //        OwnerIdForDocuments = Convert.ToInt32(OutId);

        //    DocumentsDA documentsDA = new DocumentsDA();
        //    string s = hfDeletedDoc.Value;
        //    string[] DeletedDocList = s.Split(',');
        //    for (int i = 0; i < DeletedDocList.Length; i++)
        //    {
        //        DeletedDocList[i] = DeletedDocList[i].Trim();
        //        Boolean DeleteStatus=documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
        //    }

        //    HMCommonDA hmCommonDA = new HMCommonDA();
        //    string docPath = Server.MapPath("") + "\\Images\\SiteSurvey\\";
        //    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(RandomDocId.Value));
        //    if (status)
        //    {
        //        if (sMSiteSurveyNoteBO.Id == 0)
        //        {
        //            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
        //                    EntityTypeEnum.EntityType.WorkingPlan.ToString(), OutId,
        //                    ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
        //                    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.WorkingPlan));

        //            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
        //        }
        //        else
        //        {
        //            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
        //                   EntityTypeEnum.EntityType.SalaryHead.ToString(), OutId,
        //                   ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
        //                   hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryHead));
        //            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
        //        }

        //        NoSiteSurvey.Checked = false;
        //        ddlCompany.SelectedValue = "0";
        //        ddlSurveyFor.SelectedValue = "0";
        //        txtDescription.Text = "";
        //        hfId.Value = "0";
        //        hfSelectedDealId.Value = "0";
        //    }


        //}

        [WebMethod]
        public static GuestCompanyBO LoadCompanyAddress(int Id)
        {

            GuestCompanyBO guestCompany = new GuestCompanyBO();
            GuestCompanyDA DA = new GuestCompanyDA();
            guestCompany = DA.GetGuestCompanyInfoById(Id);
            return guestCompany;
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
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("SiteSurveyDoc", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SiteSurveyDoc", id));

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

        [WebMethod]
        public static List<SMDeal> GetDealByCompanyId(int companyId, int contactId)
        {
            DealDA dealDA = new DealDA();
            List<SMDeal> deals = new List<SMDeal>();
            deals = dealDA.GetAllDealByCompanyIdNContactIdForDropdown(companyId, contactId);
            return deals;
        }

        [WebMethod]
        public static ReturnInfo SaveSiteSurveyNote(SMSiteSurveyNoteBO sMSiteSurveyNoteBO, int randomDocId, string deletedDoc)
        {
            int OwnerIdForDocuments = 0;
            bool status = false;


            ReturnInfo rtninfo = new ReturnInfo();
            SiteSurveyNoteDA siteSurveyNoteDA = new SiteSurveyNoteDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            long OutId;
            SiteSurveyNoteDA DA = new SiteSurveyNoteDA();

            status = DA.SaveUpdateSiteSurveyNote(sMSiteSurveyNoteBO, out OutId);
            if (status)
            {
                OwnerIdForDocuments = Convert.ToInt32(OutId);
                if (!sMSiteSurveyNoteBO.IsDealNeedSiteSurvey)
                    ApproveSiteSurveyNote((long)sMSiteSurveyNoteBO.DealId);
            }

            DocumentsDA documentsDA = new DocumentsDA();
            string s = deletedDoc;
            string[] DeletedDocList = s.Split(',');
            for (int i = 0; i < DeletedDocList.Length; i++)
            {
                DeletedDocList[i] = DeletedDocList[i].Trim();
                Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
            }

            HMCommonDA hmCommonDA = new HMCommonDA();
            if (status)
            {
                Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(randomDocId));

                if (Convert.ToInt32(sMSiteSurveyNoteBO.Id) == 0)
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.WorkingPlan.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.WorkingPlan));

                }
                else
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.SalaryHead.ToString(), OutId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryHead));
                }


            }
            else
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            rtninfo.Data = randomId;
            return rtninfo;
        }

        [WebMethod]
        public static SMSiteSurveyNoteBO GetSiteSurveyNoteById(int siteSurveyNoteId)
        {

            SMSiteSurveyNoteBO sMSiteSurvey = new SMSiteSurveyNoteBO();
            SiteSurveyNoteDA DA = new SiteSurveyNoteDA();
            sMSiteSurvey = DA.GetSiteSurveyNoteById(siteSurveyNoteId);
            return sMSiteSurvey;
        }
        [WebMethod]
        public static ReturnInfo ApproveSiteSurveyNote(long dealId)
        {
            ReturnInfo info = new ReturnInfo();
            SalesMarketingLogType<SMDeal> logDA = new SalesMarketingLogType<SMDeal>();
            bool status = false;
            try
            {
                SMDeal previousBO = new DealDA().GetDealInfoBOById(dealId);
                SiteSurveyNoteDA siteSurveyNoteDA = new SiteSurveyNoteDA();
                status = siteSurveyNoteDA.ApproveSiteSurveyNote(dealId);
                if (status)
                {
                    SMDeal deal = new DealDA().GetDealInfoBOById(dealId);
                    logDA.Log(ConstantHelper.SalesandMarketingLogType.DealActivity, deal, previousBO);
                    info.IsSuccess = true;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }
    }
}