using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.LCManagement;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.LCManagement;
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

namespace HotelManagement.Presentation.Website.LCManagement
{
    
    public partial class LCInformation : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckPermission();
                LoadSupplierInfo();
            }

        }
        private void CheckPermission()
        {

            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }

        public void LoadSupplierInfo()
        {
            string supplierTypeId = "Foreign";
            Data.PurchaseManagment.PMSupplierDA entityDA = new Data.PurchaseManagment.PMSupplierDA();
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();

            supplierBOList = entityDA.GetPMSupplierInfoByOrderType(supplierTypeId);

            if (supplierBOList.Count == 1)
            {
                ddlSupplier.DataSource = supplierBOList;
                ddlSupplier.DataTextField = "Name";
                ddlSupplier.DataValueField = "SupplierId";
                ddlSupplier.DataBind(); ListItem itemSupplier = new ListItem();
                itemSupplier.Value = "0";
                itemSupplier.Text = hmUtility.GetDropDownFirstAllValue();
                ddlSupplier.Items.Insert(0, itemSupplier);

            }
            else if (supplierBOList.Count > 1)
            {
                ddlSupplier.DataSource = supplierBOList;
                ddlSupplier.DataTextField = "Name";
                ddlSupplier.DataValueField = "SupplierId";
                ddlSupplier.DataBind();
                ListItem itemSupplier = new ListItem();
                itemSupplier.Value = "0";
                itemSupplier.Text = hmUtility.GetDropDownFirstAllValue();
                ddlSupplier.Items.Insert(0, itemSupplier);
            }

        }
        [WebMethod]
        public static ReturnInfo DeleteLCInformation(long id)
        {
            ReturnInfo info = new ReturnInfo();
            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();


            try
            {
                LCInformationDA lcInformationDA = new LCInformationDA();
                var status = lcInformationDA.DeleteLCInformationById(id);
                if (status)
                {
                    info.IsSuccess = true;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
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
        [WebMethod]
        public static ReturnInfo ApprovalStatusUpdate(long id, string ApprovalStatusUpdate)
        {
            ReturnInfo info = new ReturnInfo();
            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            try
            {
                LCInformationDA lcInformationDA = new LCInformationDA();
                var status = lcInformationDA.ApprovalStatusUpdate(id, ApprovalStatusUpdate, userInformationBO.UserInfoId);
                if (status && ApprovalStatusUpdate == "Checked")
                {
                    info.IsSuccess = true;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                }
                else if (status && ApprovalStatusUpdate == "Approved")
                {
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

        [WebMethod]
        public static string LoadDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("LCDoc", (int)id);
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);

            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg" || dr.Extention == ".png")
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
            if (docList.Count == 0)
            {
                strTable = "<tr><td align='center'>No Record Available!</td></tr>";
            }
            strTable += "</div>";

            return strTable;

        }

        [WebMethod]
        public static GridViewDataNPaging<LCInformationBO, GridPaging> GetLCInformation(int companyId, int projectId, int supplierId, string LCNumber, string PINumber, DateTime fromDate, DateTime toDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<LCInformationBO> LCInformationBOList = new List<LCInformationBO>();
            GridViewDataNPaging<LCInformationBO, GridPaging> myGridData = new GridViewDataNPaging<LCInformationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (userInformationBO.EmpId != 0)
            {
                LCInformationDA lcInformationDA = new LCInformationDA();
                LCInformationBOList = lcInformationDA.GetLCInformationForGridPaging(userInformationBO.UserInfoId, companyId, projectId, supplierId, LCNumber, PINumber, fromDate, toDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            }

            myGridData.GridPagingProcessing(LCInformationBOList, totalRecords);
            return myGridData;
        }
    }
}