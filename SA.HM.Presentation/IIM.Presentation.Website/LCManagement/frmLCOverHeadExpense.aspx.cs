using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.GeneralLedger;
using Newtonsoft.Json;
using HotelManagement.Entity.LCManagement;
using HotelManagement.Data.LCManagement;

namespace HotelManagement.Presentation.Website.LCManagement
{
    public partial class frmLCOverHeadExpense : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
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
                string editId = Request.QueryString["editId"];
                LoadCommonDropDownHiddenField();
                LoadLCNumber();
                LoadOverHeadName();
                LoadCNFName();
                LoadCashAndCashEquivalantAccountHead();
                LoadCurrency();
                LoadBank();
            }
        }

        //************************ User Defined Function ********************//
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "TaskAssignDocId=" + Server.UrlEncode(RandomDocId.Value);
        }
        private void LoadOverHeadName()
        {
            OverHeadNameDA bankDA = new OverHeadNameDA();
            List<OverHeadNameBO> entityBOList = new List<OverHeadNameBO>();
            entityBOList = bankDA.GetActiveLCOverHeadNameInfo();

            this.ddlOverHeadId.DataSource = entityBOList;
            this.ddlOverHeadId.DataTextField = "OverHeadName";
            this.ddlOverHeadId.DataValueField = "OverHeadId";
            this.ddlOverHeadId.DataBind();

            this.ddlSrcOverHeadId.DataSource = entityBOList;
            this.ddlSrcOverHeadId.DataTextField = "OverHeadName";
            this.ddlSrcOverHeadId.DataValueField = "OverHeadId";
            this.ddlSrcOverHeadId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlOverHeadId.Items.Insert(0, itemBank);

            ListItem itemBankSrc = new ListItem();
            itemBankSrc.Value = "0";
            itemBankSrc.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSrcOverHeadId.Items.Insert(0, itemBankSrc);
        }
        private void LoadCNFName()
        {
            LcCnfInfoDA cnfInfoDA = new LcCnfInfoDA();
            List<LCCnfInfoBO> entityBOList = new List<LCCnfInfoBO>();
            entityBOList = cnfInfoDA.GetAllCNFInfoList();

            this.ddlCNFName.DataSource = entityBOList;
            this.ddlCNFName.DataTextField = "Name";
            this.ddlCNFName.DataValueField = "SupplierId";
            this.ddlCNFName.DataBind();

            ListItem list = new ListItem();
            list.Value = "0";
            list.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCNFName.Items.Insert(0, list);
        }
        private void LoadLCNumber()
        {
            LCInformationDA bankDA = new LCInformationDA();
            List<LCInformationBO> entityBOList = new List<LCInformationBO>();
            entityBOList = bankDA.GetApprovedLCInformation();

            this.ddlLCId.DataSource = entityBOList;
            this.ddlLCId.DataTextField = "LCNumber";
            this.ddlLCId.DataValueField = "LCId";
            this.ddlLCId.DataBind();

            this.ddlSrcLCId.DataSource = entityBOList;
            this.ddlSrcLCId.DataTextField = "LCNumber";
            this.ddlSrcLCId.DataValueField = "LCId";
            this.ddlSrcLCId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlLCId.Items.Insert(0, itemBank);

            ListItem itemBankSrc = new ListItem();
            itemBankSrc.Value = "0";
            itemBankSrc.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSrcLCId.Items.Insert(0, itemBankSrc);
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (txtExpenseDate.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Expense Date.", AlertType.Warning);
                flag = false;
                txtExpenseDate.Focus();
            }
            else if (ddlLCId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "LC Number.", AlertType.Warning);
                flag = false;
                ddlLCId.Focus();
            }
            else if (ddlOverHeadId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Expense Head.", AlertType.Warning);
                flag = false;
                ddlOverHeadId.Focus();
            }

            return flag;

        }
        private void LoadCashAndCashEquivalantAccountHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();

            this.ddlIncomeAccountHead.DataSource = entityDA.GetNodeMatrixInfoByAncestorNodeId(16).Where(x => x.IsTransactionalHead == true).ToList();
            this.ddlIncomeAccountHead.DataTextField = "HeadWithCode";
            this.ddlIncomeAccountHead.DataValueField = "NodeId";
            this.ddlIncomeAccountHead.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlIncomeAccountHead.Items.Insert(0, item);
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

            this.ddlCurrencyType.DataSource = currencyListBO;
            this.ddlCurrencyType.DataTextField = "CurrencyName";
            this.ddlCurrencyType.DataValueField = "CurrencyId";
            this.ddlCurrencyType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCurrencyType.Items.Insert(0, item);
        }
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            List<BankBO> entityBOList = new List<BankBO>();
            entityBOList = bankDA.GetBankInfo();

            this.ddlCompanyBank.DataSource = entityBOList;
            this.ddlCompanyBank.DataTextField = "BankName";
            this.ddlCompanyBank.DataValueField = "BankId";
            this.ddlCompanyBank.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCompanyBank.Items.Insert(0, itemBank);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static GridViewDataNPaging<OverHeadExpenseBO, GridPaging> SearchPaidServiceAndLoadGridInformation(string transactionType, DateTime fromDate, DateTime toDate, int LCId, int overHeadId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            GridViewDataNPaging<OverHeadExpenseBO, GridPaging> myGridData = new GridViewDataNPaging<OverHeadExpenseBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            OverHeadExpenseDA paidServiceDA = new OverHeadExpenseDA();
            List<OverHeadExpenseBO> paidServiceList = new List<OverHeadExpenseBO>();
            paidServiceList = paidServiceDA.GetOverHeadExpenseInfoBySearchCriteriaForPagination(transactionType, fromDate, toDate, LCId, overHeadId, Convert.ToInt32(userInformationBO.UserInfoId), userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<OverHeadExpenseBO> distinctItems = new List<OverHeadExpenseBO>();
            distinctItems = paidServiceList.GroupBy(test => test.ExpenseId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo DeletePaidServiceById(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean delStatus = hmCommonDA.DeleteInfoById("LCOverHeadExpense", "ExpenseId", sEmpId);
                if (delStatus)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                             EntityTypeEnum.EntityType.HotelService.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelService));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static bool SearchCNFNameByOverHead(int headId)
        {
            List<LCCnfInfoBO> CNFInfoList = new List<LCCnfInfoBO>();
            OverHeadNameBO overHead = new OverHeadNameBO();
            OverHeadNameDA headNameDA = new OverHeadNameDA();
            LcCnfInfoDA cnfInfoDA = new LcCnfInfoDA();
            overHead = headNameDA.GetLCOverHeadNameInfoById(headId);

            return overHead.IsCNFHead;
        }
        [WebMethod]
        public static ReturnInfo SaveOrUpdateOverHeadExpense(OverHeadExpenseBO OverHeadExpenseBO, int randomDocId, string deletedDoc)
        {
            int OwnerIdForDocuments = 0;
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (OverHeadExpenseBO.ExpenseId == 0)
            {
                OverHeadExpenseBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            else
            {
                OverHeadExpenseBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            int OutId;
            OverHeadExpenseDA DA = new OverHeadExpenseDA();
            status = DA.SaveLCOverHeadExpenseInfo(OverHeadExpenseBO, out OutId);
            if (status)
                OwnerIdForDocuments = Convert.ToInt32(OutId);

            DocumentsDA documentsDA = new DocumentsDA();
            string s = deletedDoc;
            string[] DeletedDocList = s.Split(',');
            for (int i = 0; i < DeletedDocList.Length; i++)
            {
                DeletedDocList[i] = DeletedDocList[i].Trim();
                Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
            }
            Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(randomDocId));

            if (status)
            {
                if (OverHeadExpenseBO.ExpenseId == 0)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LCOverHead.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCOverHead));

                }
                else
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.LCOverHead.ToString(), OutId,
                           ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCOverHead));
                }
            }
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            rtninfo.Data = randomId;
            return rtninfo;
        }
        [WebMethod]
        public static OverHeadExpenseBO FillForm(int Id)
        {

            OverHeadExpenseBO OverHeadExpenseBO = new OverHeadExpenseBO();
            OverHeadExpenseDA DA = new OverHeadExpenseDA();
            OverHeadExpenseBO = DA.GetLCOverHeadExpenseInfoById(Id);

            return OverHeadExpenseBO;
        }
        [WebMethod]
        public static ReturnInfo DeleteLCOverHeadExpense(long Id)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            status = DA.DeleteInfoById("LCOverHeadExpense", "ExpenseId", Id);
            if (status)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.LCOverHead.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.LCManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCOverHead));
            }
            return rtninfo;
        }
        [WebMethod]
        public static ReturnInfo ExpenseApproval(int Id, string approvedStatus)
        {
            ReturnInfo rtninf = new ReturnInfo();
            OverHeadExpenseDA DA = new OverHeadExpenseDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = DA.ExpenseApproval(Id, approvedStatus, userInformationBO.UserInfoId);

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isTransferProductReceiveDisable = new HMCommonSetupBO();

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    if (approvedStatus == "Checked")
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                    }
                    else if (approvedStatus == "Approved")
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));
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
        public static CommonCurrencyBO LoadCurrencyType(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(currecyType);
            return commonCurrencyBO;
        }
        [WebMethod]
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currecyType);
            return conversionBO;
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
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("OverHeadDoc", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("OverHeadDoc", (int)id));

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
        public static string LoadOverHeadExpenseDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("OverHeadDoc", (int)id);
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
                    strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
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
                    strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
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
    }
}