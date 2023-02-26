using HotelManagement.Data.AirTicketing;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.AirTicketing;
using HotelManagement.Entity.GeneralLedger;
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

namespace HotelManagement.Presentation.Website.AirTicketing
{
    public partial class frmAirlineTicketInfo : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomDocId.Value = seatingId.ToString();
                IsAdminUser();
                LoadAirline();
                LoadCurrency();
                CheckPermission();
                LoadProject();
                LoadBankName();
                FileUpload();
            }
        }
        private void IsAdminUser()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            btnAdminApproval.Visible = false;
            if (userInformationBO.UserInfoId == 1)
            {
                btnAdminApproval.Visible = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 40).Count() > 0)
                    {
                        btnAdminApproval.Visible = true;
                    }
                }
            }
            #endregion
        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfo();
            if (costCentreTabBOList != null)
            {
                if (costCentreTabBOList.Where(x => x.CostCenterType == "FrontOffice").ToList().Count == 0)
                {
                    this.ddlTransactionType.Items.Remove(ddlTransactionType.Items.FindByValue("RoomGuest"));
                }
            }
        }
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "TaskAssignDocId=" + Server.UrlEncode(RandomDocId.Value);
        }
        private void LoadAirline()
        {
            AirlineDA airlineDa = new AirlineDA();
            List<AirlineBO> airlineBO = new List<AirlineBO>();
            airlineBO = airlineDa.GetAirlineInfo();

            ddlAirlineName.DataSource = airlineBO;
            ddlAirlineName.DataTextField = "AirlineName";
            ddlAirlineName.DataValueField = "AirlineId";
            ddlAirlineName.DataBind();

            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstValue();
            ddlAirlineName.Items.Insert(0, itemSearch);
        }

        private void LoadProject()
        {
            int glCompanyId = 0;
            CostCentreTabDA entityDA = new CostCentreTabDA();
            List<CostCentreTabBO> listAirlineTicketListBO = entityDA.GetCostCentreTabInfoByType("AirlineTicket");
            if (listAirlineTicketListBO != null)
            {
                if (listAirlineTicketListBO.Count > 0)
                {
                    glCompanyId = listAirlineTicketListBO[0].GLCompanyId;
                }
            }

            GLProjectDA projectDa = new GLProjectDA();
            List<GLProjectBO> projectBO = new List<GLProjectBO>();
            projectBO = projectDa.GetProjectInfoForAirlineTikect(glCompanyId);

            ddlProject.DataSource = projectBO;
            ddlProject.DataTextField = "Name";
            ddlProject.DataValueField = "ProjectId";
            ddlProject.DataBind();

            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstValue();
            ddlProject.Items.Insert(0, itemSearch);
        }
        private void LoadBankName()
        {
            BankDA bankDa = new BankDA();
            List<BankBO> bankBo = new List<BankBO>();
            bankBo = bankDa.GetBankInfo();

            ddlPaymentInstructionBank.DataSource = bankBo;
            ddlPaymentInstructionBank.DataTextField = "BankAccountNameAndNumber";
            ddlPaymentInstructionBank.DataValueField = "BankId";
            ddlPaymentInstructionBank.DataBind();

            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstValue();
            ddlPaymentInstructionBank.Items.Insert(0, itemSearch);
        }

        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

            this.ddlCurrency.DataSource = currencyListBO;
            this.ddlCurrency.DataTextField = "CurrencyName";
            this.ddlCurrency.DataValueField = "CurrencyId";
            this.ddlCurrency.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCurrency.Items.Insert(0, item);
        }

        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyInfoForAirTicket(string searchTerm)
        {
            List<GuestCompanyBO> companyInfo = new List<GuestCompanyBO>();
            GuestCompanyDA companyDa = new GuestCompanyDA();
            companyInfo = companyDa.GetCompanyInfoForAirTicket(searchTerm);
            return companyInfo;
        }

        [WebMethod]
        public static List<GuestReferenceBO> GetGuestReferenceInfo(string searchTerm)
        {
            List<GuestReferenceBO> guestReferenceList = new List<GuestReferenceBO>();
            GuestReferenceDA guestRefDa = new GuestReferenceDA();
            guestReferenceList = guestRefDa.GetGuestReferenceBySearchCriteria(searchTerm);
            return guestReferenceList;
        }

        [WebMethod]
        public static List<ContactInformationBO> GetGuestReferenceInfoForCompany(int companyId, string searchTerm)
        {
            List<ContactInformationBO> contactList = new List<ContactInformationBO>();
            ContactInformationDA contactDa = new ContactInformationDA();
            contactList = contactDa.GetContactInformationByCompanyIdNSearchTextForAutoComplete(companyId, searchTerm);
            return contactList;
        }

        [WebMethod]
        public static List<BankBO> GetBankInfoForAutoComplete(string searchTerm)
        {
            List<BankBO> bankList = new List<BankBO>();
            BankDA bankDa = new BankDA();
            bankList = bankDa.GetBankInfoForAutoComplete(searchTerm);
            return bankList;
        }


        [WebMethod]
        public static ReturnInfo SaveAirlineTicketInfo(AirlineTicketMasterBO AirTicketMasterInfo, List<AirlineTicketInfoBO> AddedSingleTicketInfo, List<int> deletedAirlineInfoList, List<GuestBillPaymentBO> AddedPaymentInfo, 
                                                        List<int> deletedPaymentInfoList, int randomDocId, string deletedDoc)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                AirlineTicketInfoDA atDa = new AirlineTicketInfoDA();

                AirTicketMasterInfo.CreatedBy = userInformationBO.UserInfoId;
                AirTicketMasterInfo.Status = HMConstants.ApprovalStatus.Pending.ToString();

                Boolean IsUpdate = false;
                if(AirTicketMasterInfo.TicketId > 0)
                {
                    IsUpdate = true;
                }

                int costCenterId = 0;
                CostCentreTabDA entityDA = new CostCentreTabDA();
                List<CostCentreTabBO> listAirlineTicketListBO = entityDA.GetCostCentreTabInfoByType("AirlineTicket");
                if (listAirlineTicketListBO != null)
                {
                    if (listAirlineTicketListBO.Count > 0)
                    {
                        costCenterId = listAirlineTicketListBO[0].CostCenterId;
                    }
                }
                AirTicketMasterInfo.CostCenterId = costCenterId;
                long id = 0;
                int OwnerIdForDocuments = 0;
                HMCommonDA hmCommonDA = new HMCommonDA();

                status = atDa.SaveAirlineTicketInfo(AirTicketMasterInfo, AddedSingleTicketInfo, deletedAirlineInfoList, AddedPaymentInfo, deletedPaymentInfoList, out id);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    OwnerIdForDocuments = Convert.ToInt32(id);
                    DocumentsDA documentsDA = new DocumentsDA();
                    string s = deletedDoc;
                    string[] DeletedDocList = s.Split(',');
                    for (int i = 0; i < DeletedDocList.Length; i++)
                    {
                        DeletedDocList[i] = DeletedDocList[i].Trim();
                        Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                    }
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(randomDocId));
                    if (IsUpdate)
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                    else
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }                    
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
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
        public static ReturnInfo TicketInformationDelete(long ticketId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            try
            {
                
                AirlineTicketInfoDA atDa = new AirlineTicketInfoDA();
                status = atDa.TicketInformationDelete(ticketId);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;

        }
        
        [WebMethod]
        public static ReturnInfo TicketInformationCheck(long ticketId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                AirlineTicketInfoDA atDa = new AirlineTicketInfoDA();
                status = atDa.TicketInformationCheck(ticketId, userInformationBO.UserInfoId);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;

        }

        [WebMethod]
        public static ReturnInfo TicketInformationApproval(long ticketId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                AirlineTicketInfoDA atDa = new AirlineTicketInfoDA();
                status = atDa.TicketInformationApproval(ticketId, userInformationBO.UserInfoId);
                if (status)
                {
                    CommonDA commonDA = new CommonDA();
                    bool autoProcessStatus = commonDA.AutoCompanyBillGenerationProcess("Airline Ticketing", ticketId, userInformationBO.UserInfoId);
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                }
                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninfo;
        }

        [WebMethod]
        public static GridViewDataNPaging<AirlineTicketMasterBO, GridPaging> SearchTicketInformation(DateTime? fromDate, DateTime? toDate, string invoiceNumber, string companyName, 
                                                                                          string referenceName, string ticketNumber, string pnrNumber, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage
                                                                                         )
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<AirlineTicketMasterBO, GridPaging> myGridData = new GridViewDataNPaging<AirlineTicketMasterBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            
            AirlineTicketInfoDA aTDA = new AirlineTicketInfoDA();
            List<AirlineTicketMasterBO> ticketList = new List<AirlineTicketMasterBO>();

            ticketList = aTDA.GetTicketInformation(fromDate, toDate, invoiceNumber, companyName, referenceName, ticketNumber, pnrNumber, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(ticketList, totalRecords);
            return myGridData;
        }
                
        [WebMethod]
        public static ATInformationViewBO TicketInfoEdit(long ticketId)
        {
            ATInformationViewBO viewBo = new ATInformationViewBO();
            AirlineTicketInfoDA aTDA = new AirlineTicketInfoDA();

            viewBo.ATMasterInfo = aTDA.GetATMasterInfo(ticketId);
            viewBo.ATInformationDetails = aTDA.GetATInformationDetails(ticketId);
            viewBo.ATPaymentInfo = aTDA.GetATPaymentInfo(ticketId);
            return viewBo;
        }
        [WebMethod]
        public static ReturnInfo AdminApprovalStatus(string ticketNo, string ticketStatus)
        {
            Boolean status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            AirlineTicketInfoDA supportDA = new AirlineTicketInfoDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                status = supportDA.UpdateTicketStatusByATTicketNo(ticketNo, ticketStatus, userInformationBO.UserInfoId);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = "Ticket Unapprove Successfull.";
                }
                else
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
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
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("AirlineTicketInfo", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("AirlineTicketInfo", (int)id));

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
    }
}