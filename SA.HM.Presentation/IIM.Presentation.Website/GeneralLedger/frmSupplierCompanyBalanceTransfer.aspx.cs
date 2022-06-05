using HotelManagement.Data.GeneralLedger;
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
        }
        [WebMethod]
        public static Object[] GetSupplierCompanyList(int transactionTypeId)
        {
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            GLCompanyDA list = new GLCompanyDA();
            companyList = list.GetSupplierCompanyInfo();

            PMSupplierDA supplier = new PMSupplierDA();
            List<PMSupplierBO> supplierList = supplier.GetPMSupplierInfo();

            Object[] companySupplierList = new Object[4];
            
            if(transactionTypeId == 1)
            {
                companySupplierList[0] = supplierList;
                companySupplierList[1] = companyList;
                companySupplierList[2] = "Supplier";
                companySupplierList[3] = "Company";
            }
            else if(transactionTypeId == 2)
            {
                companySupplierList[0] = companyList;
                companySupplierList[1] = supplierList;
                companySupplierList[2] = "Company";
                companySupplierList[3] = "Supplier";
            }
            else if (transactionTypeId == 3)
            {
                companySupplierList[0] = supplierList;
                companySupplierList[1] = supplierList;
                companySupplierList[2] = "Supplier";
                companySupplierList[3] = "Supplier";
            }
            else if (transactionTypeId == 4)
            {
                companySupplierList[0] = companyList;
                companySupplierList[1] = companyList;
                companySupplierList[2] = "Company";
                companySupplierList[3] = "Company";
            }
            else
            {
                companySupplierList[2] = "";
                companySupplierList[3] = "";
            }
            return companySupplierList;
        }

        [WebMethod]
        public static Boolean SaveSupplierCompanyBalanceTransfer(string editId, string transactionType, int fromTransactionId, int toTransactionId, decimal amount)
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
            SCBalanceTransferInfo.Amount = amount;
            SCBalanceTransferInfo.CreatedBy = userInformationBO.UserInfoId;

            Boolean status = false;
            status = SCda.SaveSupplierCompanyBalanceTransfer(SCBalanceTransferInfo);
            return status;
        }

        [WebMethod]
        public static List<SupplierCompanyBalanceTransferBO> GetTransactionsBySearch(string transactionTypeSearch, int fromTransaction, int toTransaction, DateTime? dateFrom, DateTime? dateTo)
        {
            List<SupplierCompanyBalanceTransferBO> transactionInfo = new List<SupplierCompanyBalanceTransferBO>();
            SupplierCompanyBalanceTransferDA SCda = new SupplierCompanyBalanceTransferDA();
            transactionInfo = SCda.GetTransactionsBySearch(transactionTypeSearch, fromTransaction, toTransaction, dateFrom, dateTo);
            return transactionInfo;
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