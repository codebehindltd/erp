using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Collections;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using Newtonsoft.Json;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class ItemReceiveInfo : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadSupplierInfo();
                LoadAllCostCentreInfo();
                CheckPermission();
            }
        }

        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();

            supplierBOList = entityDA.GetPMSupplierInfo();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            ddlSearchSupplier.DataSource = supplierBOList;
            ddlSearchSupplier.DataTextField = "Name";
            ddlSearchSupplier.DataValueField = "SupplierId";
            ddlSearchSupplier.DataBind();
            ddlSearchSupplier.Items.Insert(0, item);
        }
        private void LoadAllCostCentreInfo()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfo();
            costCentreTabBOList = costCentreTabBOList.Where(o => o.OutletType == 2).ToList();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            ddlCostCenterSearch.DataSource = costCentreTabBOList;
            ddlCostCenterSearch.DataTextField = "CostCenter";
            ddlCostCenterSearch.DataValueField = "CostCenterId";
            ddlCostCenterSearch.DataBind();
            ddlCostCenterSearch.Items.Insert(0, item);

        }
        private void CheckPermission()
        {
         
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        [WebMethod]
        public static GridViewDataNPaging<PMProductReceivedBO, GridPaging> SearchReceiveOrder(int companyId, int projectId, string receiveType, DateTime? fromDate, DateTime? toDate,
                                                                                        string receiveNumber, string status, int? costCenterId,
                                                                                        int? supplierId, int gridRecordsCount, int pageNumber,
                                                                                        int isCurrentOrPreviousPage
                                                                                       )
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMProductReceivedBO, GridPaging> myGridData = new GridViewDataNPaging<PMProductReceivedBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (fromDate == null) fromDate = DateTime.Now;
            if (toDate == null) toDate = DateTime.Now;

            PMProductReceivedDA receiveDA = new PMProductReceivedDA();
            List<PMProductReceivedBO> orderList = new List<PMProductReceivedBO>();

            orderList = receiveDA.GetProductreceiveInfo(companyId, projectId, receiveType, fromDate, toDate, receiveNumber, status, costCenterId, supplierId, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(orderList, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo ReceiveOrderApproval(string receiveType, int receivedId, string approvedStatus, int porderId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductReceivedDA orderDetailDA = new PMProductReceivedDA();
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDetailDA.ReceiveOrderApproval(receiveType, receivedId, approvedStatus, porderId, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;

                    rtninf.PrimaryKeyValue = receivedId.ToString();
                    rtninf.TransactionNo = TransactionNo;
                    rtninf.TransactionType = TransactionType;
                    rtninf.TransactionStatus = ApproveStatus;

                    if (approvedStatus == HMConstants.ApprovalStatus.Checked.ToString())
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                    }
                    else
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ProductReceive.ToString(), receivedId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));

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
        public static string LoadDealDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("ReceiveOrderDocuments", id);
            //docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesDealFeedbackDocuments", id));
            //docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesQuotationDocuments", id));
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


        [WebMethod]
        public static ReturnInfo ReceiveOrderDelete(string receiveType, int receivedId, string approvedStatus, int createdBy)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductReceivedDA orderDetailDA = new PMProductReceivedDA();
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDetailDA.ReceiveOrderDelete(receiveType, receivedId, approvedStatus, createdBy, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.PrimaryKeyValue = receivedId.ToString();
                    rtninf.TransactionNo = TransactionNo;
                    rtninf.TransactionType = TransactionType;
                    rtninf.TransactionStatus = ApproveStatus;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);


                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.ProductReceive.ToString(), receivedId,
                              ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));

                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

    }
}