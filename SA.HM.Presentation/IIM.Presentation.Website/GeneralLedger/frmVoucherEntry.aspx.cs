using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft;
using Newtonsoft.Json;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmVoucherEntry : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            //isSingle = hmUtility.GetSingleProjectAndCompany();

            if (!IsPostBack)
            {
                IsAdminUser();
                companyProjectUserControl.ddlFirstValueVar = "select";
                companyProjectSearchUserControl.ddlFirstValueVar = "all";
                LoadCurrentDate();
                LoadCurrency();
                LoadGLDonor();
                LoadChartofAccounts();
                //FileUpload();
                LoadCommonDropDownHiddenField();
                LoadConfiguration();

                Random rd = new Random();
                int randomId = rd.Next(100000, 999999);
                RandomLedgerMasterId.Value = randomId.ToString();
                HttpContext.Current.Session["LedgerMasterId"] = randomId;
            }
        }
        //************************ User Defined Function ********************//
        private void IsAdminUser()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            hfIsAdminUser.Value = "0";
            if (userInformationBO.UserInfoId == 1)
            {
                hfIsAdminUser.Value = "1";
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 2).Count() > 0)
                    {
                        hfIsAdminUser.Value = "1";
                    }
                }
            }
            #endregion
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            txtVoucherDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

            ddlCurrency.DataSource = currencyListBO;
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataValueField = "CurrencyId";
            ddlCurrency.DataBind();

            hfCurrencyAll.Value = JsonConvert.SerializeObject(currencyListBO);
        }
        private void LoadGLDonor()
        {
            GLDonorDA entityDA = new GLDonorDA();
            List<GLDonorBO> donorList = new List<GLDonorBO>();
            donorList = entityDA.GetAllGLDonorInfo();

            ddlDonor.DataSource = donorList;
            ddlDonor.DataTextField = "Name";
            ddlDonor.DataValueField = "DonorId";
            ddlDonor.DataBind();
            ListItem itemDonor = new ListItem();
            itemDonor.Value = "0";
            itemDonor.Text = hmUtility.GetDropDownFirstValue();
            ddlDonor.Items.Insert(0, itemDonor);

        }
        private void LoadChartofAccounts()
        {
            NodeMatrixDA nmda = new NodeMatrixDA();
            List<NodeMatrixBO> nm = new List<NodeMatrixBO>();

            nm = nmda.GetNodeMatrixInfoForVoucherEntry();
            hfNodeMatrix.Value = JsonConvert.SerializeObject(nm);
        }
        private void FileUpload()
        {
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            RandomLedgerMasterId.Value = randomId.ToString();
            HttpContext.Current.Session["LedgerMasterId"] = randomId;

            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsShowReferenceVoucherNumber", "IsShowReferenceVoucherNumber");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsShowReferenceVoucherNumber.Value = setUpBO.SetupValue.ToString();
            }
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ArrayList AccountHeadForCPCRBPBR(string projectId, string voucherType)
        {
            ArrayList list = new ArrayList();
            List<GLAccountConfigurationBO> entityBOList = new List<GLAccountConfigurationBO>();

            if (!string.IsNullOrEmpty(projectId))
            {
                GLAccountConfigurationDA entityDA = new GLAccountConfigurationDA();
                entityBOList = entityDA.GetCashAndCashEquivalantTransactionalHead(Convert.ToInt32(projectId), voucherType);
            }
            return new ArrayList(entityBOList);
        }
        [WebMethod]
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currecyType);
            return conversionBO;
        }
        [WebMethod(EnableSession = true)]
        public static ReturnInfo SaveVoucher(GLLedgerMasterBO LedgerMaster, List<GLLedgerDetailsBO> LedgerDetails, string deletedDocument)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            long ledgerId = 0;
            string voucherNo = string.Empty;

            VoucherEntryDA voucherDa = new VoucherEntryDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                LedgerMaster.CreatedBy = userInformationBO.UserInfoId;
                long RandomId = HttpContext.Current.Session["LedgerMasterId"] != null ? Convert.ToInt64(HttpContext.Current.Session["LedgerMasterId"]) : 0;
                status = voucherDa.SaveVoucher(LedgerMaster, LedgerDetails, deletedDocument, RandomId, out ledgerId, out voucherNo);

                if (status)
                {
                    Random rd = new Random();
                    int randomId = rd.Next(100000, 999999);
                    HttpContext.Current.Session["LedgerMasterId"] = randomId;
                    rtninfo.DataStr = randomId.ToString();
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save + " The Voucher Number Is " + voucherNo, AlertType.Success);
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
        [WebMethod(EnableSession = true)]
        public static ReturnInfo UpdateVoucher(GLLedgerMasterBO LedgerMaster, List<GLLedgerDetailsBO> LedgerDetails, List<GLLedgerDetailsBO> LedgerNodeDelete, string deletedDocument)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;

            VoucherEntryDA voucherDa = new VoucherEntryDA();
            List<GLLedgerDetailsBO> NewLedgerDetails = new List<GLLedgerDetailsBO>();
            List<GLLedgerDetailsBO> EditLedgerDetails = new List<GLLedgerDetailsBO>();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                NewLedgerDetails = (from ld in LedgerDetails
                                    where ld.LedgerDetailsId == 0
                                    select ld).ToList();

                EditLedgerDetails = (from ld in LedgerDetails
                                     where ld.LedgerDetailsId != 0 && ld.IsEdited == true
                                     select ld).ToList();

                LedgerMaster.LastModifiedBy = userInformationBO.UserInfoId;

                long RandomId = HttpContext.Current.Session["LedgerMasterId"] != null ? Convert.ToInt64(HttpContext.Current.Session["LedgerMasterId"]) : 0;
                status = voucherDa.UpdateVoucher(LedgerMaster, NewLedgerDetails, EditLedgerDetails, LedgerNodeDelete, deletedDocument, RandomId);

                if (status)
                {
                    Random rd = new Random();
                    int randomId = rd.Next(100000, 999999);
                    HttpContext.Current.Session["LedgerMasterId"] = randomId;
                    rtninfo.DataStr = randomId.ToString();

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
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
        public static ReturnInfo DeleteVoucher(long ledgerMasterId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;

            VoucherEntryDA voucherDa = new VoucherEntryDA();

            try
            {
                status = voucherDa.DeleteVoucher(ledgerMasterId);

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
        public static GridViewDataNPaging<GLLedgerMasterVwBO, GridPaging> GetVoucherBySearchCriteria(int companyId, int projectId, string voucherType, string voucherStatus, string voucherNo, string fromDate, string toDate, string referenceNo, string referenceVoucherNo, string narration, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("GLVoucherEditAfterApprovalBeforeYearClosing", "GLVoucherEditAfterApprovalBeforeYearClosing");

            int totalRecords = 0, approvedAfterUserGroupId = Convert.ToInt32(commonSetupBO.SetupValue.ToString());

            GridViewDataNPaging<GLLedgerMasterVwBO, GridPaging> myGridData = new GridViewDataNPaging<GLLedgerMasterVwBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (approvedAfterUserGroupId == userInformationBO.UserGroupId)
                myGridData.UserGroupId = userInformationBO.UserGroupId;
            else
                myGridData.UserGroupId = 0;

            DateTime? startDate = null;
            DateTime? endDate = null;

            if (!string.IsNullOrEmpty(fromDate))
            {
                startDate = hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                endDate = hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat);
            }

            HMCommonDA commonDA = new HMCommonDA();
            VoucherEntryDA voucherDa = new VoucherEntryDA();
            List<GLLedgerMasterVwBO> voucher = new List<GLLedgerMasterVwBO>();
            voucher = voucherDa.GetVoucherBySearchCriteria(companyId, projectId, userInformationBO.UserInfoId, userInformationBO.UserGroupId, voucherType, voucherStatus, voucherNo, startDate, endDate, referenceNo, referenceVoucherNo, narration, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(voucher, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static GLVoucherVwBO GetVoucherDetailsForDisplay(Int64 ledgerMasterId)
        {
            GLVoucherVwBO vouchervw = new GLVoucherVwBO();
            VoucherEntryDA voucherDa = new VoucherEntryDA();

            vouchervw.LedgerMaster = voucherDa.GetVoucherById(ledgerMasterId);
            vouchervw.LedgerMasterDetails = voucherDa.GetVoucherDetailsById(ledgerMasterId);

            vouchervw.TotalCrAmount = vouchervw.LedgerMasterDetails.Sum(c => c.CRAmount);
            vouchervw.TotalDrAmount = vouchervw.LedgerMasterDetails.Sum(c => c.DRAmount);

            return vouchervw;
        }
        [WebMethod(EnableSession = true)]
        public static GLVoucherVwBO GetVoucherDetailsForEdit(Int64 ledgerMasterId)
        {
            GLVoucherVwBO vouchervw = new GLVoucherVwBO();
            VoucherEntryDA voucherDa = new VoucherEntryDA();
            List<GLLedgerDetailsVwBO> LedgerMasterDetails = new List<GLLedgerDetailsVwBO>();

            vouchervw.LedgerMaster = voucherDa.GetVoucherById(ledgerMasterId);
            vouchervw.LedgerMasterDetails = voucherDa.GetVoucherDetailsById(ledgerMasterId);

            if ((vouchervw.LedgerMaster.VoucherType == "CP" || vouchervw.LedgerMaster.VoucherType == "BP"))
            {
                LedgerMasterDetails = (from ld in vouchervw.LedgerMasterDetails
                                       where ld.LedgerMode == 2
                                       select ld).ToList();

                LedgerMasterDetails.AddRange((from ld in vouchervw.LedgerMasterDetails
                                              where ld.LedgerMode == 1
                                              select ld).ToList());

                vouchervw.LedgerMasterDetails = LedgerMasterDetails;
            }
            if ((vouchervw.LedgerMaster.VoucherType == "CR" || vouchervw.LedgerMaster.VoucherType == "BR"))
            {
                LedgerMasterDetails = (from ld in vouchervw.LedgerMasterDetails
                                       where ld.LedgerMode == 1
                                       select ld).ToList();

                LedgerMasterDetails.AddRange((from ld in vouchervw.LedgerMasterDetails
                                              where ld.LedgerMode == 2
                                              select ld).ToList());

                vouchervw.LedgerMasterDetails = LedgerMasterDetails;
            }

            vouchervw.TotalCrAmount = vouchervw.LedgerMasterDetails.Sum(c => c.CRAmount);
            vouchervw.TotalDrAmount = vouchervw.LedgerMasterDetails.Sum(c => c.DRAmount);

            List<GLAccountConfigurationBO> entityBOList = new List<GLAccountConfigurationBO>();
            GLAccountConfigurationDA entityDA = new GLAccountConfigurationDA();
            //entityBOList = entityDA.GetAllAccountConfigurationInfoByProjectIdNAccountType(Convert.ToInt32(vouchervw.LedgerMaster.ProjectId), vouchervw.LedgerMaster.VoucherType);
            entityBOList = entityDA.GetCashAndCashEquivalantTransactionalHead(Convert.ToInt32(vouchervw.LedgerMaster.ProjectId), vouchervw.LedgerMaster.VoucherType);
            vouchervw.CpBpCrBrAccountHead = new ArrayList(entityBOList);

            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            HttpContext.Current.Session["LedgerMasterId"] = randomId;
            vouchervw.RandomLedgerMasterId = randomId;
            return vouchervw;
        }
        [WebMethod]
        public static List<DocumentsBO> LoadVoucherDocument(long id, int randomId, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();
            if (randomId > 0)
                docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("GLVoucherDocuments", randomId);

            if (id > 0)
            {
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("GLVoucherDocuments", id));
            }

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }
        [WebMethod]
        public static int ChangeRandomId()
        {
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            HttpContext.Current.Session["LedgerMasterId"] = randomId;
            return randomId;
        }
        [WebMethod]
        public static List<GLProjectBO> GetGLProjectByGLCompanyId(int companyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(companyId, userInformationBO.UserGroupId);

            return projectList;
        }
        [WebMethod]
        public static string LoadVoucherDocumentById(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("GLVoucherDocuments", id);
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
