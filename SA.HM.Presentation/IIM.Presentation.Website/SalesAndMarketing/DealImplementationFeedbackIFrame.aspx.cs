using HotelManagement.Data.Inventory;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class DealImplementationFeedbackIFrame : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEngineer();
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomDocId.Value = seatingId.ToString();
                FileUpload();
                LoadCategory();
            }
        }
        protected void LoadEngineer()
        {
            List<EmployeeBO> employees = new List<EmployeeBO>();

            DepartmentDA DA = new DepartmentDA();
            employees = DA.GetEmployeeOfTechnicalDepartment();
            ddlEngineers.DataSource = employees;
            ddlEngineers.DataTextField = "DisplayName";
            ddlEngineers.DataValueField = "EmpId";
            ddlEngineers.DataBind();
            ListItem listItem = new ListItem();
            listItem.Value = "0";
            listItem.Text = hmUtility.GetDropDownFirstAllValue();
            ddlEngineers.Items.Insert(0, listItem);
        }
        private void LoadUserInfo()
        {
            //GetUserInformationById
            UserInformationDA entityDA = new UserInformationDA();
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();
            userInformationList = entityDA.GetUserInformation();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string userId = userInformationBO.UserInfoId.ToString();

            ddlEngineers.DataSource = userInformationList;
            ddlEngineers.DataTextField = "UserName";
            ddlEngineers.DataValueField = "UserInfoId";
            ddlEngineers.DataBind();

            //ddlEngineers.SelectedValue = userId;


            //if (userInformationBO.IsAdminUser)
            //{
            //    ddlEngineers.Enabled = true;
            //}
            //else
            //{
            //    ddlEngineers.Enabled = false;
            //}
            //if (userInformationBO.UserInfoId == 1)//superadmin
            //{
            //    ddlEngineers.Enabled = true;
            //}
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEngineers.Items.Insert(0, item);
        }

        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllActiveInvItemCatagoryInfoByServiceType("Product");
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();

            List<InvCategoryBO> serviceCategory = new List<InvCategoryBO>();
            serviceCategory = da.GetAllActiveInvItemCatagoryInfoByServiceType("Service");

        }
        [WebMethod]
        public static ReturnInfo SaveUpdateDealFeedback(List<DealImpFeedbackBO> impFeedbackBOs, string hfRandom, string hfDealId, string impFeedback, DateTime impDate, string deletedDocument, string implementationStatus)
        {
            long OwnerIdForDocuments = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            long tempId = 0;
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            DealDA dealDA = new DealDA();

            try
            {
                //get ImpFeedback by deal Id
                List<DealImpFeedbackBO> alreadySavedFeedbackBOs = new List<DealImpFeedbackBO>();
                alreadySavedFeedbackBOs = dealDA.GetImpFeedbackInfoByDealId(Convert.ToInt32(hfDealId));

                foreach (var item in impFeedbackBOs)
                {
                    item.DealId = Convert.ToInt32(hfDealId);
                }

                List<DealImpFeedbackBO> newDealImpFeedbackBOs = new List<DealImpFeedbackBO>();
                List<DealImpFeedbackBO> deletedDealImpFeedbackBOs = new List<DealImpFeedbackBO>();

                if (alreadySavedFeedbackBOs.Count > 0)// update
                {
                    var newAdded = (from p in impFeedbackBOs
                                    where !(
                                    from c in alreadySavedFeedbackBOs
                                    select c.ImpEngineerId).Contains(p.ImpEngineerId)
                                    select p).ToList();

                    var deleted = (from data in alreadySavedFeedbackBOs
                                   where !(
                                   from items in impFeedbackBOs
                                   select items.ImpEngineerId).Contains(data.ImpEngineerId)
                                   select data).ToList();

                    var updated = (from data in alreadySavedFeedbackBOs
                                   where (
                                          from items in impFeedbackBOs
                                          select items.ImpEngineerId).Contains(data.ImpEngineerId)
                                   select data).ToList();
                    if (newAdded.Count > 0)
                    {
                        rtninfo.IsSuccess = dealDA.SaveImpFeedback(newAdded, userInformationBO.UserInfoId, out tempId);
                        if (rtninfo.IsSuccess)
                        {
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                EntityTypeEnum.EntityType.DealImpFeedback.ToString(), Convert.ToInt32(hfDealId),
                                ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DealImpFeedback));

                            rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        }
                        else
                        {
                            rtninfo.IsSuccess = false;
                            rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                        }
                    }
                    if (updated.Count > 0)
                    {
                        rtninfo.IsSuccess = dealDA.UpdateImpFeedback(updated, userInformationBO.UserInfoId);
                        if (rtninfo.IsSuccess)
                        {
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                EntityTypeEnum.EntityType.DealImpFeedback.ToString(), Convert.ToInt32(hfDealId),
                                ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DealImpFeedback));

                            rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        }
                        else
                        {
                            rtninfo.IsSuccess = false;
                            rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                        }
                    }
                    if (deleted.Count > 0)
                    {
                        rtninfo.IsSuccess = dealDA.DeleteImpFeedback(deleted);
                        if (rtninfo.IsSuccess)
                        {
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                EntityTypeEnum.EntityType.DealImpFeedback.ToString(), Convert.ToInt32(hfDealId),
                                ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DealImpFeedback));

                            rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        }
                        else
                        {
                            rtninfo.IsSuccess = false;
                            rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                        }
                    }

                }
                else // save
                {
                    rtninfo.IsSuccess = dealDA.SaveImpFeedback(impFeedbackBOs, userInformationBO.UserInfoId, out tempId);
                    if (rtninfo.IsSuccess)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.DealImpFeedback.ToString(), Convert.ToInt32(hfDealId),
                            ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DealImpFeedback));

                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                    else
                    {
                        rtninfo.IsSuccess = false;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }
                if (rtninfo.IsSuccess)
                {
                    OwnerIdForDocuments = Convert.ToInt64(hfDealId);
                    if (!string.IsNullOrEmpty(deletedDocument))
                    {
                        ContactInformationDA contactInformationDA = new ContactInformationDA();
                        bool delete = contactInformationDA.DeleteContactDocument(deletedDocument);
                    }
                    bool update = dealDA.UpdateImpFeedbackInfo(impFeedback, impDate, Convert.ToInt32(hfDealId), userInformationBO.UserInfoId, implementationStatus);

                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(hfRandom));
                }
                Random rd = new Random();
                int randomId = rd.Next(100000, 999999);
                rtninfo.Data = randomId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return rtninfo;
        }
        [WebMethod]
        public static ArrayList GetImpFeedbackInfoByDealId(int dealId)
        {
            ArrayList arr = new ArrayList();
            DealDA dealDA = new DealDA();

            List<DealImpFeedbackBO> alreadySavedFeedbackBOs = new List<DealImpFeedbackBO>();
            alreadySavedFeedbackBOs = dealDA.GetImpFeedbackInfoByDealId(dealId);

            SMDeal sMDeal = new SMDeal();
            sMDeal = dealDA.GetImpFeedbackFromDealTable(dealId);

            arr.Add(new { Infos = sMDeal, Engineers = alreadySavedFeedbackBOs });

            return arr;
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
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesDealFeedbackDocuments", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesDealFeedbackDocuments", (int)id));

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
        public static GridViewDataNPaging<SMDeal, GridPaging> LoadGridPaging(string dealNumber, string name, int companyId, string dateType, string fromDate, string toDate, int gridRecordsCount, int pageNumber, int IsCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            if (fromDate != "" && toDate == "")
            {
                toDate = DateTime.Now.ToShortDateString();
            }

            GridViewDataNPaging<SMDeal, GridPaging> myGridData = new GridViewDataNPaging<SMDeal, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, IsCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<SMDeal> deals = new List<SMDeal>();
            DealDA dealDA = new DealDA();
            deals = dealDA.GetDealInfoForSearchForImplemantationFeedback(dealNumber, name, companyId, dateType, fromDate, toDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(deals, totalRecords);

            return myGridData;
        }

        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "LoadDetailGridInformation", jscript, true);

            //flashUpload.QueryParameters = "DealFeedbackId=" + Server.UrlEncode(RandomProductId.Value);

            //flashUpload.QueryParameters = "InventoryProductId=" + Server.UrlEncode(RandomProductId.Value) + "~" + txtCode.Text;
        }
    }
}