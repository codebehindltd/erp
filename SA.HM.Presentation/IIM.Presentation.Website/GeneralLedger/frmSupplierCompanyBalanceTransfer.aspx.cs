using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmSupplierCompanyBalanceTransfer : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            //innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            //if (!IsPostBack)
            //{
            //    CheckPermission();
            //}
        }

        //private void CheckPermission()
        //{

        //    hfSavePermission.Value = isSavePermission ? "1" : "0";
        //    hfDeletePermission.Value = isDeletePermission ? "1" : "0";
        //    hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        //    hfViewPermission.Value = isViewPermission ? "1" : "0";
        //}

        [WebMethod]
        public static Object[] GetSupplierCompanyList(int transactionTypeId)
        {
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            GLCompanyDA list = new GLCompanyDA();
            companyList = list.GetSupplierCompanyInfo();

            PMSupplierDA supplier = new PMSupplierDA();
            List<PMSupplierBO> supplierList = supplier.GetPMSupplierInfo();

            NodeMatrixDA accountHead = new NodeMatrixDA();
            List<NodeMatrixBO> accoutHeadList = (accountHead.GetNodeMatrixInfo().Where(xActive => xActive.NodeMode == true).ToList()).Where(xITH => xITH.IsTransactionalHead == true).ToList();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupSupplierAccountsHeadBO = new HMCommonSetupBO();            
            commonSetupSupplierAccountsHeadBO = commonSetupDA.GetCommonConfigurationInfo("SupplierAccountsHeadId", "SupplierAccountsHeadId");
            if (commonSetupSupplierAccountsHeadBO != null)
            {
                if (commonSetupSupplierAccountsHeadBO.SetupId > 0)
                {
                    accoutHeadList = accoutHeadList.Where(x => x.NodeId.ToString() != commonSetupSupplierAccountsHeadBO.SetupValue).ToList();
                }
            }

            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> accountsReceivableAccountHeadBO = new List<CustomFieldBO>();
            accountsReceivableAccountHeadBO = commonDA.GetCustomField("AccountsReceivableAccountHeadForHotelGuest", hmUtility.GetDropDownFirstValue());
            foreach (CustomFieldBO row in accountsReceivableAccountHeadBO)
            {
                accoutHeadList = accoutHeadList.Where(x => x.NodeId.ToString() != row.FieldValue).ToList();
            }

            Object[] companySupplierAccountList = new Object[4];
            
            if(transactionTypeId == 1)
            {
                companySupplierAccountList[0] = supplierList;
                companySupplierAccountList[1] = companyList;
                companySupplierAccountList[2] = "Supplier";
                companySupplierAccountList[3] = "Company";
            }
            else if(transactionTypeId == 2)
            {
                companySupplierAccountList[0] = companyList;
                companySupplierAccountList[1] = supplierList;
                companySupplierAccountList[2] = "Company";
                companySupplierAccountList[3] = "Supplier";
            }
            else if (transactionTypeId == 3)
            {
                companySupplierAccountList[0] = supplierList;
                companySupplierAccountList[1] = supplierList;
                companySupplierAccountList[2] = "Supplier";
                companySupplierAccountList[3] = "Supplier";
            }
            else if (transactionTypeId == 4)
            {
                companySupplierAccountList[0] = companyList;
                companySupplierAccountList[1] = companyList;
                companySupplierAccountList[2] = "Company";
                companySupplierAccountList[3] = "Company";
            }
            else if (transactionTypeId == 5)
            {
                companySupplierAccountList[0] = companyList;
                companySupplierAccountList[1] = accoutHeadList;
                companySupplierAccountList[2] = "Company";
                companySupplierAccountList[3] = "Accounts Head";
            }
            else if (transactionTypeId == 6)
            {
                companySupplierAccountList[0] = accoutHeadList;
                companySupplierAccountList[1] = companyList;
                companySupplierAccountList[2] = "Accounts Head";
                companySupplierAccountList[3] = "Company";
            }
            else if (transactionTypeId == 7)
            {
                companySupplierAccountList[0] = supplierList;
                companySupplierAccountList[1] = accoutHeadList;
                companySupplierAccountList[2] = "Supplier";
                companySupplierAccountList[3] = "Accounts Head";
            }
            else if (transactionTypeId == 8)
            {
                companySupplierAccountList[0] = accoutHeadList;
                companySupplierAccountList[1] = supplierList;
                companySupplierAccountList[2] = "Accounts Head";
                companySupplierAccountList[3] = "Supplier";
            }
            else
            {
                companySupplierAccountList[2] = "";
                companySupplierAccountList[3] = "";
            }
            return companySupplierAccountList;
        }

        [WebMethod]
        public static Boolean SaveSupplierCompanyBalanceTransfer(string editId, string transactionType, int fromTransactionId, int toTransactionId, DateTime transactionDate, decimal amount, string remarks)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            SupplierCompanyBalanceTransferBO SCBalanceTransferInfo = new SupplierCompanyBalanceTransferBO();
            SupplierCompanyBalanceTransferDA SCda = new SupplierCompanyBalanceTransferDA();
            int editIdInt = 0;
            int.TryParse(editId, out editIdInt);
            SCBalanceTransferInfo.Id = editIdInt;
            SCBalanceTransferInfo.TransactionType = transactionType;
            SCBalanceTransferInfo.FromTransactionId = fromTransactionId;
            SCBalanceTransferInfo.ToTransactionId = toTransactionId;
            SCBalanceTransferInfo.TransactionDate = transactionDate;
            SCBalanceTransferInfo.Amount = amount;
            SCBalanceTransferInfo.Remarks = remarks;
            SCBalanceTransferInfo.CreatedBy = userInformationBO.UserInfoId;
            SCBalanceTransferInfo.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();

            Boolean status = false;
            status = SCda.SaveSupplierCompanyBalanceTransfer(SCBalanceTransferInfo);
            return status;
        }

        [WebMethod]
        public static List<SupplierCompanyBalanceTransferBO> GetTransactionsBySearch(string transactionTypeSearch, int fromTransaction, int toTransaction, DateTime? dateFrom, DateTime? dateTo)
        {
            List<SupplierCompanyBalanceTransferBO> transactionInfo = new List<SupplierCompanyBalanceTransferBO>();
            SupplierCompanyBalanceTransferDA SCda = new SupplierCompanyBalanceTransferDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            transactionInfo = SCda.GetTransactionsBySearch(transactionTypeSearch, fromTransaction, toTransaction, dateFrom, dateTo, userInformationBO.UserInfoId);
            return transactionInfo;
        }
        [WebMethod]
        public static ReturnInfo CheckedTransfer(Int64 transferId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            SupplierCompanyBalanceTransferDA SCda = new SupplierCompanyBalanceTransferDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = SCda.CheckedTransfer(transferId, userInformationBO.UserInfoId);
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            if (rtninfo.IsSuccess)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
            }

            return rtninfo;
        }
        [WebMethod]
        public static ReturnInfo ApprovedTransfer(Int64 transferId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            SupplierCompanyBalanceTransferDA SCda = new SupplierCompanyBalanceTransferDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = SCda.ApprovedTransfer(transferId, userInformationBO.UserInfoId);
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            if (rtninfo.IsSuccess)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
            }

            return rtninfo;
        }

        [WebMethod]
        public static SupplierCompanyBalanceTransferBO FillForm(int Id)
        {
            SupplierCompanyBalanceTransferBO currentInfo = new SupplierCompanyBalanceTransferBO();
            SupplierCompanyBalanceTransferDA SCda = new SupplierCompanyBalanceTransferDA();
            currentInfo = SCda.GetCurrentSupplierCompanyInfoForEdit(Id);
            return currentInfo;
        }

        [WebMethod]
        public static Boolean DeleteTransactionInfo(int Id)
        {
            Boolean result;
            SupplierCompanyBalanceTransferDA SCda = new SupplierCompanyBalanceTransferDA();
            result = SCda.DeleteTransactionInfo(Id);
            return result;
        }
    }
}