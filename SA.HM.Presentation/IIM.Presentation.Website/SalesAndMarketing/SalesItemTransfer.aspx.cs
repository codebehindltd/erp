using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.PurchaseManagment;
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
    public partial class SalesItemTransfer : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCostCenter();
                LoadQuotation();
            }
        }
        public void LoadCostCenter()
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId)
                    .Where(a => a.CostCenterType == "Inventory").ToList();

            ddlCostCenterFrom.DataSource = List;
            ddlCostCenterFrom.DataTextField = "CostCenter";
            ddlCostCenterFrom.DataValueField = "CostCenterId";
            ddlCostCenterFrom.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (List.Count > 1)
                ddlCostCenterFrom.Items.Insert(0, item);

        }

        public void LoadQuotation()
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            SalesTransferDA DA = new SalesTransferDA();
            var List = DA.GetQuotationForItemOut();
            var ListForSearch = DA.GetAllQuotation();

            ddlQuotation.DataSource = List;
            ddlQuotation.DataTextField = "QuotationNo";
            ddlQuotation.DataValueField = "QuotationId";
            ddlQuotation.DataBind();
            ddlQuotationForSearch.DataSource = ListForSearch;
            ddlQuotationForSearch.DataTextField = "QuotationNo";
            ddlQuotationForSearch.DataValueField = "QuotationId";
            ddlQuotationForSearch.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (List.Count > 1)
            {
                ddlQuotation.Items.Insert(0, item);

            }
            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstAllValue();
            ddlQuotationForSearch.Items.Insert(0, itemSearch);

        }

        [WebMethod]
        public static List<InvLocationBO> InvLocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);
            return location;
        }

        [WebMethod]
        public static long CheckIfQuotationIsTransfered(long quotationId)
        {
            long SalesTransferId;
            SalesTransferDA DA = new SalesTransferDA();
            SalesTransferId = DA.CheckIfQuotationIsTransfered(quotationId);
            return SalesTransferId;
        }
        [WebMethod]
        public static List<QuotationViewDetailsBO> GetQuotationDetalsByQuotationId(long quotationId, int costCenterId, int locationId)
        {
            List<QuotationViewDetailsBO> quotationDetailsList = new List<QuotationViewDetailsBO>();
            SalesTransferDA DA = new SalesTransferDA();
            quotationDetailsList = DA.GetQuotationDetailsByQuotationId(quotationId, costCenterId, locationId);
            return quotationDetailsList;
        }
        [WebMethod]
        public static ReturnInfo SerialAvailabilityCheck(string FromLocationId,
                                                    List<PMProductOutSerialInfoBO> ItemSerialDetails)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isProductOutApproval = new HMCommonSetupBO();
            string serialId = string.Empty, message = string.Empty;
            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();
            PMProductOutDA outDA = new PMProductOutDA();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                isProductOutApproval = commonSetupDA.GetCommonConfigurationInfo("IsProductOutApprovalEnable", "IsProductOutApprovalEnable");

                foreach (PMProductOutSerialInfoBO srl in ItemSerialDetails.Where(s => s.OutSerialId == 0))
                {
                    if (serialId != string.Empty)
                    {
                        serialId += "," + srl.SerialNumber;
                    }
                    else
                    {
                        serialId = srl.SerialNumber;
                    }
                }

                serial = outDA.SerialAvailabilityCheck(serialId, Convert.ToInt64(FromLocationId));

                foreach (SerialDuplicateBO p in serial)
                {
                    if (message != "")
                        message = ", " + p.ItemName + "(" + p.SerialNumber + ")";
                    else
                        message = p.ItemName + "(" + p.SerialNumber + ")";
                }

                if (!string.IsNullOrEmpty(message))
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("This Item Serial Does Not Exists. " + message, AlertType.Error);
                    return rtninfo;
                }
                else
                {
                    rtninfo.IsSuccess = true;
                    return rtninfo;
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
        public static ReturnInfo PerformSave(SMSalesTransferBO SalesTransfer,List<SMSalesTransferDetailsBO> TransferItemNewlyAdded, 
            List<SMSalesTransferDetailsBO> TransferItemForEdit, List<SMSalesTransferDetailsBO> TransferItemDeleted,
            List<SMSalesItemSerialTransferBO> AddedSerialzableProduct
                ,List<SMSalesItemSerialTransferBO> DeletedSerialzableProduct)
        {
            bool status = false;
            long transferOutId;
            int transferOutDetailsId;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            SMQuotationViewBO Quotation = new SMQuotationViewBO();
            SalesQuotationNBillingDA quotationDA = new SalesQuotationNBillingDA();
            Quotation = quotationDA.GetQuotationById(SalesTransfer.QuotationId);
            SMSalesTransferBO salesTransferBO = new SMSalesTransferBO
            {
                CostCenterId = SalesTransfer.CostCenterId,
                LocationID = SalesTransfer.LocationID,
                SalesTransferId = SalesTransfer.SalesTransferId,
                DealId = Quotation.DealId,
                QuotationId = SalesTransfer.QuotationId,
                CompanyId = Quotation.CompanyId,
                CreatedBy = userInformationBO.UserInfoId
            };
            SalesTransferDA DA = new SalesTransferDA();

            AddedSerialzableProduct = AddedSerialzableProduct.Where(i => i.SalesItemSerialTransferId == 0).ToList();

            status = DA.SaveSalesTransfer(salesTransferBO, TransferItemNewlyAdded, TransferItemForEdit, TransferItemDeleted, AddedSerialzableProduct, DeletedSerialzableProduct, out transferOutId, out transferOutDetailsId);

            if (status == true && SalesTransfer.SalesTransferId == 0)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
            }
            else if (status == true && SalesTransfer.SalesTransferId >= 0)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
            }
            else
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninfo;
        }


        [WebMethod]
        public static SMSalesTransferViewBo GetQuotationDetailsBySalesTransferId(long SalesTransferId)
        {
            SMSalesTransferViewBo SMSalesTransferViewBo = new SMSalesTransferViewBo();
            SMSalesTransferBO salesTransferBO = new SMSalesTransferBO();
            SalesTransferDA DA = new SalesTransferDA();
            SMSalesTransferViewBo.SMSalesTransferBO = DA.GetSalesItemTransferInfoById(SalesTransferId);
            SMSalesTransferViewBo.SMQuotationDetailsBOList = DA.GetQuotationDetailsBySalesTransferId(SalesTransferId);
            SMSalesTransferViewBo.SMSalesTransferItemSerialList = DA.GetSalesItemSerialTransferBySalesTransferId(SalesTransferId);
            return SMSalesTransferViewBo;
        }
        [WebMethod]
        public static GridViewDataNPaging<SMSalesTransferBO, GridPaging> SearchSalesTransfer(int QuotationId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMSalesTransferBO, GridPaging> myGridData = new GridViewDataNPaging<SMSalesTransferBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<SMSalesTransferBO> SalesTransferList = new List<SMSalesTransferBO>();
            SalesTransferDA DA = new SalesTransferDA();
            SalesTransferList = DA.GetSalesTransfer(QuotationId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(SalesTransferList, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo ApproveSalesTransfer(int salesTransferId, string approvedStatus, string remarks)
        {
            bool status = false;
            ReturnInfo rtninf = new ReturnInfo();
            SalesTransferDA DA = new SalesTransferDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                status = DA.SalesItemApproval(salesTransferId, approvedStatus, remarks, userInformationBO.UserInfoId);

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isTransferProductReceiveDisable = new HMCommonSetupBO();
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
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
        public static ReturnInfo DeleteSalesItemTransfer(long Id)
        {
            bool status = false;
            ReturnInfo rtninf = new ReturnInfo();
            SalesTransferDA DA = new SalesTransferDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                status = DA.DeleteSalesItemTransfer(Id);

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isTransferProductReceiveDisable = new HMCommonSetupBO();
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
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
        public static SalesNQuotationViewBO GetDetailsForApproval(long SalesTransferId)
        {
            SalesNQuotationViewBO SalesNQuotationView = new SalesNQuotationViewBO();
            SalesTransferDA DA = new SalesTransferDA();
            SalesNQuotationView = DA.GetSalesNQuotationViewBOForApprove(SalesTransferId);
            return SalesNQuotationView;
        }
    }
}