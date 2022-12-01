using HotelManagement.Data.AirTicketing;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.AirTicketing;
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
                LoadAirline();
                LoadCurrency();
                CheckPermission();
            }
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
        public static List<ContactInformationBO> GetGuestReferenceInfoForCompany(string searchTerm)
        {
            List<ContactInformationBO> contactList = new List<ContactInformationBO>();
            ContactInformationDA contactDa = new ContactInformationDA();
            contactList = contactDa.GetContactByAutoSearch(searchTerm);
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
        public static ReturnInfo SaveAirlineTicketInfo(AirlineTicketMasterBO AirTicketMasterInfo, List<AirlineTicketInfoBO> AddedSingleTicketInfo, List<GuestBillPaymentBO> AddedPaymentInfo)
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
                

                status = atDa.SaveAirlineTicketInfo(AirTicketMasterInfo, AddedSingleTicketInfo, AddedPaymentInfo);
                if (status)
                {
                    rtninfo.IsSuccess = true;
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
        public static GridViewDataNPaging<AirlineTicketMasterBO, GridPaging> SearchTicketInformation(DateTime? fromDate, DateTime? toDate, string invoiceNumber, string companyName, 
                                                                                          string referenceName, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage
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

            ticketList = aTDA.GetTicketInformation(fromDate, toDate, invoiceNumber, companyName, referenceName, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

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
    }
}