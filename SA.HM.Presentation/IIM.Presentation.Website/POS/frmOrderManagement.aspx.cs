using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using System.Web.Services;
using HotelManagement.Entity.HotelManagement;
using System.Security;
using Microsoft.Reporting.WebForms;
using HotelManagement.Entity.UserInformation;
using System.Security.Permissions;
using HotelManagement.Presentation.Website.POS.Reports;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using HotelManagement.Data;
using HotelManagement.Data.HotelManagement;
using System.Collections;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.Membership;
using HotelManagement.Data.UserInformation;
using System.Xml;
using InnBoardSDC.SDCTool;
using HotelManagement.Data.SDC;
using HotelManagement.Entity.SDC;
using HotelManagement.Presentation.Website.Common.SDCTool;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmOrderManagement : System.Web.UI.Page
    {
        public int IsDiscountEnable = 0;
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        string irQueryString = string.Empty;
        public string innBoardDateFormat = "";
        InvCategoryDA moLocation = new InvCategoryDA();
        private Boolean IsRestaurantOrderSubmitDisable = false;
        private Boolean IsRestaurantTokenInfoDisable = false;
        /*private Boolean IsInvoiceReceived = false;
        private Boolean IsDeviceConnected = false;
        private string SdcConnectionFailedMessage = string.Empty;*/

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfoForRestaurant();

            hfIsItemCanEditDelete.Value = (userInformationBO.IsItemCanEditDelete == true ? "1" : "0");
            hfIsUserHasEditDeleteAccess.Value = (userInformationBO.IsItemCanEditDelete == true ? "1" : "0");
            hfIsBearar.Value = (userInformationBO.IsBearer == true ? "1" : "0");
            hfIsItemSearchEnable.Value = userInformationBO.IsItemSearchEnable == true ? "1" : "0";

            if (!IsPostBack)
            {

                LoadLocalCurrencyId();

                if (Request.QueryString["ot"] != null)
                {
                    string sourceType = Request.QueryString["st"];
                    string sourceId = Request.QueryString["sid"];
                    string costCenter = Request.QueryString["cc"];
                    string kotId = Request.QueryString["kid"];
                    userInformationBO.WorkingCostCenterId = Convert.ToInt32(costCenter);
                    IsDiscountApplicable(Convert.ToInt32(costCenter));
                }
                //HttpContext.Current.Session["KotHoldupBill"]
                if (Request.QueryString["ot"] == "ro") //resume order
                {
                    PaymentResume();
                    hfIsResumeBill.Value = "1";
                }
                else
                {
                    LoadRestaurantOrder();
                }

                GetTopLevelLocations(null);

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillInvoiceTemplate", "RestaurantBillInvoiceTemplate");

                if (invoiceTemplateBO != null)
                {
                    if (invoiceTemplateBO.SetupId > 0)
                    {
                        if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 1)
                        {
                            hfBillTemplate.Value = "1";
                        }
                        else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 2)
                        {
                            hfBillTemplate.Value = "2";
                        }
                        else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 3)
                        {
                            hfBillTemplate.Value = "3";
                        }
                        else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == 4)
                        {
                            hfBillTemplate.Value = "4";
                        }
                    }
                }

                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithFrontOffice", "IsRestaurantIntegrateWithFrontOffice");
                if (invoiceTemplateBO.SetupId > 0) { hfIsRestaurantIntegrateWithFrontOffice.Value = invoiceTemplateBO.SetupValue; }

                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithPayroll", "IsRestaurantIntegrateWithPayroll");
                if (invoiceTemplateBO.SetupId > 0) { hfIsRestaurantIntegrateWithPayroll.Value = invoiceTemplateBO.SetupValue; }

                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithGuestCompany", "IsRestaurantIntegrateWithGuestCompany");
                if (invoiceTemplateBO.SetupId > 0) { hfIsRestaurantIntegrateWithCompany.Value = invoiceTemplateBO.SetupValue; }

                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithMembership", "IsRestaurantIntegrateWithMembership");
                if (invoiceTemplateBO.SetupId > 0) { hfIsRestaurantIntegrateWithMember.Value = invoiceTemplateBO.SetupValue; }
            }

            if (!string.IsNullOrWhiteSpace(hfBillIdForInvoicePreview.Value))
            {
                btnPaymentInfo.Visible = true;
            }
        }

        private void IsDiscountApplicable(int costCenterId)
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            bool status = costCentreTabDA.GetIsDiscountEnableForCostCenter(costCenterId);
            if (status == true)
            {
                IsDiscountEnable = 1;
            }
            else
            {
                IsDiscountEnable = 0;
            }


        }
        private void LoadNSetBasicInfo(int costCenterId)
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(costCenterId);

            if (costCentreTabBO.IsVatEnable == true)
            {
                hfIsVatEnable.Value = "1";
                cbTPVatAmount.Checked = true;
                hfRestaurantVatAmount.Value = costCentreTabBO.VatAmount.ToString();
            }
            else
            {
                hfIsVatEnable.Value = "0";
                hfRestaurantVatAmount.Value = "0";
                cbTPVatAmount.Checked = false;
            }

            if (costCentreTabBO.IsServiceChargeEnable == true)
            {
                hfIsServiceChargeEnable.Value = "1";
                hfRestaurantServiceCharge.Value = costCentreTabBO.ServiceCharge.ToString();
                cbTPServiceCharge.Checked = true;
            }
            else
            {
                hfIsServiceChargeEnable.Value = "0";
                hfRestaurantServiceCharge.Value = "0";
                cbTPServiceCharge.Checked = false;
            }

            if (costCentreTabBO.IsCitySDChargeEnable == true)
            {
                hfIsSDChargeEnable.Value = "1";
                cbTPSDCharge.Checked = true;
                hfSDCharge.Value = costCentreTabBO.CitySDCharge.ToString();
            }
            else
            {
                hfIsSDChargeEnable.Value = "0";
                hfSDCharge.Value = "0";
                cbTPSDCharge.Checked = false;
            }

            if (costCentreTabBO.IsAdditionalChargeEnable == true)
            {
                hfIsAdditionalChargeEnable.Value = "1";
                cbTPAdditionalCharge.Checked = true;
                hfAdditionalCharge.Value = costCentreTabBO.AdditionalCharge.ToString();
                hfAdditionalChargeType.Value = costCentreTabBO.AdditionalChargeType;
            }
            else
            {
                hfIsAdditionalChargeEnable.Value = "0";
                hfAdditionalCharge.Value = "0";
                hfAdditionalChargeType.Value = "Percentage";
                cbTPAdditionalCharge.Checked = false;
            }

            if (costCentreTabBO.IsVatSChargeInclusive == 1)
                hfIsRestaurantBillInclusive.Value = "1";
            else
                hfIsRestaurantBillInclusive.Value = "0";

            if (costCentreTabBO.IsServiceChargeEnable == false)
            {
                hfltlTableWiseItemInformationDivHeight.Value = "350";
            }
            else if (costCentreTabBO.IsVatEnable == true)
            {
                hfltlTableWiseItemInformationDivHeight.Value = "350";
            }
            else if (costCentreTabBO.IsServiceChargeEnable == false && costCentreTabBO.IsVatEnable == true)
            {
                hfltlTableWiseItemInformationDivHeight.Value = "360";
            }
            else if (costCentreTabBO.IsServiceChargeEnable == true && costCentreTabBO.IsVatEnable == true)
            {
                hfltlTableWiseItemInformationDivHeight.Value = "305";
            }
            else
            {
                hfltlTableWiseItemInformationDivHeight.Value = "315";
            }

            hfIsVatOnSD.Value = costCentreTabBO.IsVatOnSDCharge ? "1" : "0";
            hfIsRatePlusPlus.Value = costCentreTabBO.IsRatePlusPlus.ToString();
            hfIsDiscountApplicableOnRackRate.Value = costCentreTabBO.IsDiscountApplicableOnRackRate ? "1" : "0";

            HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
            isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillAmountWillRound", "IsRestaurantBillAmountWillRound");
            hfIsRestaurantBillAmountWillRound.Value = isRestaurantBillAmountWillRoundBO.SetupValue.ToString();

            HMCommonSetupBO isPredefinedRemarksEnable = new HMCommonSetupBO();
            isPredefinedRemarksEnable = commonSetupDA.GetCommonConfigurationInfo("IsPredefinedRemarksEnable", "IsPredefinedRemarksEnable");
            hfIsPredefinedRemarksEnable.Value = isPredefinedRemarksEnable.SetupValue.ToString();

        }
        private void LoadRestaurantOrder()
        {
            try
            {
                string sourceType = Request.QueryString["st"].ToString().Trim();
                string sourceId = Request.QueryString["sid"].ToString().Trim();
                string costCenter = Request.QueryString["cc"].ToString().Trim();
                string kotId = Request.QueryString["kid"].ToString().Trim();
                string beararId = Request.QueryString["bid"].ToString().Trim();
                string pax = "1";

                if ((Request.QueryString["pax"]) != null)
                {
                    pax = Request.QueryString["pax"].ToString().Trim();
                    hfPaxQuantity.Value = pax;
                }
                else
                {
                    hfPaxQuantity.Value = pax;
                }

                LoadNSetBasicInfo(Convert.ToInt32(costCenter));

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                CostCentreTabDA costcneterDa = new CostCentreTabDA();
                RestaurentBillDA billDa = new RestaurentBillDA();
                KotBillMasterBO billmaster = new KotBillMasterBO();
                KotBillMasterViewBO billmasterforCheck = new KotBillMasterViewBO();
                KotBillMasterDA billmasterda = new KotBillMasterDA();

                hfKotId.Value = kotId;
                hfCostCenterId.Value = costCenter;
                hfSourceId.Value = sourceId;
                hfBearerId.Value = (beararId == "0" ? userInformationBO.BearerId.ToString() : beararId);
                hfBearerCanSettleBill.Value = (userInformationBO.IsRestaurantBillCanSettle == true ? "1" : "0");

                if (sourceType == "tkn")
                {
                    sourceType = ConstantHelper.RestaurantBillSource.RestaurantToken.ToString();

                    //string tokenNumber = billmasterda.GenarateRestaurantTokenNumber(Convert.ToInt32(costCenter));

                    lblOrderSourceName.Text = "Token Number";
                    lblOrderSourceNumber.Text = sourceId.PadLeft(4, '0');

                    billmaster.SourceId = Convert.ToInt32(sourceId);
                    billmaster.TokenNumber = sourceId.PadLeft(4, '0');

                    //hfSourceId.Value = sourceId;
                }
                else if (sourceType == "tbl")
                {
                    sourceType = ConstantHelper.RestaurantBillSource.RestaurantTable.ToString();

                    RestaurantTableDA tableDa = new RestaurantTableDA();
                    RestaurantTableBO tableBO = new RestaurantTableBO();
                    tableBO = tableDa.GetRestaurantTableInfoByTableId(Convert.ToInt32(sourceId));

                    lblOrderSourceName.Text = "Table Number";
                    lblOrderSourceNumber.Text = tableBO.TableNumber;

                    billmaster.SourceId = Convert.ToInt32(sourceId);
                }
                else if (sourceType == "rom")
                {
                    sourceType = ConstantHelper.RestaurantBillSource.GuestRoom.ToString();

                    RoomRegistrationDA roomDa = new RoomRegistrationDA();
                    RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
                    roomRegistration = roomDa.GetRoomRegistrationNRoomDetailsByRoomId(Convert.ToInt32(sourceId));

                    lblOrderSourceName.Text = "Room Number";
                    lblOrderSourceNumber.Text = roomRegistration.RoomNumber.ToString();

                    billmaster.SourceId = roomRegistration.RegistrationId;
                    hfRegistrationId.Value = roomRegistration.RegistrationId.ToString();
                    hfRoomId.Value = sourceId;
                    hfIsStopChargePosting.Value = (costcneterDa.GetRoomStopChargePostingByRegistrationAndCostCenterId(roomRegistration.RegistrationId, Convert.ToInt32(costCenter))) == true ? "1" : "0";
                }

                int kotIdNew = 0;

                billmasterforCheck = billmasterda.GetKotBillMasterBySourceIdNType(Convert.ToInt32(costCenter), sourceType, Convert.ToInt32(sourceId));

                if (Convert.ToInt32(kotId) > 0)
                {
                    kotIdNew = Convert.ToInt32(kotId);
                }
                else
                {
                    billmaster.PaxQuantity = Convert.ToInt32(pax);
                    billmaster.SourceName = sourceType;
                    billmaster.KotStatus = ConstantHelper.KotStatus.pending.ToString();
                    billmaster.BearerId = (beararId == "0" ? userInformationBO.BearerId : Convert.ToInt32(beararId));
                    billmaster.CostCenterId = Convert.ToInt32(costCenter);
                    billmaster.CreatedBy = userInformationBO.UserInfoId;
                    billmaster.IsBillHoldup = false;

                    Boolean status = billmasterda.SaveKotBillMasterInfoForNewTouchScreen(billmaster, out kotIdNew);

                    if (!status)
                    {
                        Response.Redirect("/POS/frmCostCenterSelectionForAll.aspx");
                    }

                    billmaster = billmasterda.GetKotBillMasterInfoByKotIdNSourceName(kotIdNew, sourceType);
                    //HttpContext.Current.Session["KotHoldupBill"] = billmaster;

                    string url = string.Format("/POS/frmOrderManagement.aspx?ot={0}&st={1}&sid={2}&cc={3}&kid={4}&bid={5}", "no", Request.QueryString["st"].ToString().Trim(), sourceId, costCenter, kotIdNew, beararId);

                    if ((Request.QueryString["pax"]) != null)
                    {
                        pax = Request.QueryString["pax"].ToString().Trim();
                        url = string.Format("/POS/frmOrderManagement.aspx?ot={0}&st={1}&sid={2}&cc={3}&kid={4}&pax={5}&bid={6}", "no", Request.QueryString["st"].ToString().Trim(), sourceId, costCenter, kotIdNew, pax, beararId);
                    }

                    Response.Redirect(url);
                }

                List<KotBillMasterBO> entityBOList = new List<KotBillMasterBO>();
                entityBOList = billmasterda.GetBillDetailInfoByKotId(sourceType, Convert.ToInt32(kotIdNew));

                KotBillDetailDA kotDetailsDA = new KotBillDetailDA();
                List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();
                kotDetails = kotDetailsDA.GetRestaurantOrderItemByMultipleKotId(costCenter, kotIdNew.ToString(), sourceType);

                var printedItem = kotDetails.Where(s => s.PrintFlag == true).FirstOrDefault();
                if (printedItem != null) { hfIsBillAlreadyPrint.Value = "1"; }

                if (entityBOList.Count > 0)
                {
                    hfBillIdForInvoicePreview.Value = entityBOList[0].BillId.ToString();
                }

                //if (sourceType == ConstantHelper.RestaurantBillSource.RestaurantToken.ToString())
                //{
                //    hfSourceId.Value = kotIdNew.ToString();
                //}

                RestaurantBillBO kotBill = new RestaurantBillBO();
                kotBill = billDa.GetRestaurantBillByKotId(kotIdNew, sourceType);

                if (kotBill != null)
                {
                    hfIsResumeBill.Value = "1";
                }

                hfKotId.Value = kotIdNew.ToString();
                hfsourceType.Value = sourceType;
                LoadCategoryRItemForOrder();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    Response.Redirect("/POS/frmCostCenterSelectionForAll.aspx");
            }
        }
        private void PaymentResume()
        {
            CostCentreTabDA costcneterDa = new CostCentreTabDA();
            KotBillMasterDA kotDa = new KotBillMasterDA();
            RestaurentBillDA billDa = new RestaurentBillDA();
            KotBillDetailDA kotDetailsDA = new KotBillDetailDA();
            InvCategoryDA catDa = new InvCategoryDA();

            KotBillMasterBO kotBillMaster = new KotBillMasterBO();
            List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();

            RestaurantBillBO kotBill = new RestaurantBillBO();
            List<GuestBillPaymentBO> kotBillPayment = new List<GuestBillPaymentBO>();
            List<RestaurantBillDetailBO> billDetailList = new List<RestaurantBillDetailBO>();
            List<ItemClassificationBO> classificationLst = new List<ItemClassificationBO>();

            string kotIdList = string.Empty, tableIdList = string.Empty, sourceName = string.Empty;

            string sourceType = Request.QueryString["st"].ToString().Trim();
            string sourceId = Request.QueryString["sid"].ToString().Trim();
            string costCenter = Request.QueryString["cc"].ToString().Trim();
            string kotId = Request.QueryString["kid"].ToString().Trim();

            if (sourceType == "tkn")
            {
                sourceName = ConstantHelper.RestaurantBillSource.RestaurantToken.ToString();
            }
            else if (sourceType == "tbl")
            {
                sourceName = ConstantHelper.RestaurantBillSource.RestaurantTable.ToString();
            }
            else if (sourceType == "rom")
            {
                sourceName = ConstantHelper.RestaurantBillSource.GuestRoom.ToString();
            }

            //kotBillMaster = (KotBillMasterBO)Session["KotHoldupBill"];

            kotBillMaster = kotDa.GetKotBillMasterInfoByKotIdNSourceName(Convert.ToInt32(kotId), sourceName);
            kotBill = billDa.GetRestaurantBillByKotId(kotBillMaster.KotId, kotBillMaster.SourceName);

            LoadNSetBasicInfo(kotBillMaster.CostCenterId);

            if (kotBill != null)
            {
                kotBillPayment = billDa.GetBillPaymentByBillId(kotBill.BillId, "Restaurant");
                billDetailList = billDa.GetRestaurantBillDetailsByBillId(kotBill.BillId);
                classificationLst = catDa.GetRestaurantBillClassificationDetailsByBillId(kotBill.BillId);

                if (kotBill.DiscountType == "Fixed")
                {
                    rbTPFixedDiscount.Checked = true;
                    rbTPPercentageDiscount.Checked = false;
                }
                else if (kotBill.DiscountType == "Percentage")
                {
                    rbTPPercentageDiscount.Checked = true;
                }
                else if (kotBill.DiscountType == "Percentage" && kotBill.IsComplementary == true)
                {
                    rbTPComplementaryDiscount.Checked = true;

                    rbTPFixedDiscount.Checked = false;
                    rbTPPercentageDiscount.Checked = false;
                }
                else
                {
                    rbTPPercentageDiscount.Checked = true;
                }

                hfIsVatEnable.Value = kotBill.IsInvoiceVatAmountEnable ? "1" : "0";
                hfIsServiceChargeEnable.Value = kotBill.IsInvoiceServiceChargeEnable ? "1" : "0";
                hfIsSDChargeEnable.Value = kotBill.IsInvoiceCitySDChargeEnable ? "1" : "0";
                hfIsAdditionalChargeEnable.Value = kotBill.IsInvoiceAdditionalChargeEnable ? "1" : "0";
                hfIsResumeBill.Value = "1";
                txtRemarks.Text = kotBill.Remarks;
            }

            billDetailList = billDetailList.Where(b => b.KotId != kotBillMaster.KotId).ToList();

            if (billDetailList.Count > 0)
            {
                hfAddedTableIdForBill.Value = Newtonsoft.Json.JsonConvert.SerializeObject((from b in billDetailList
                                                                                           select new
                                                                                           {
                                                                                               TableId = b.TableId,
                                                                                               KotId = b.KotId,
                                                                                               DetailId = b.DetailId,
                                                                                               BillId = b.BillId
                                                                                           }));

                foreach (RestaurantBillDetailBO bd in billDetailList)
                {
                    if (!string.IsNullOrEmpty(kotIdList))
                    {
                        kotIdList += "," + bd.KotId.ToString();
                        tableIdList += "," + bd.TableId.ToString();
                    }
                    else
                    {
                        kotIdList = bd.KotId.ToString();
                        tableIdList = bd.TableId.ToString();
                    }
                }
            }

            if (classificationLst.Count > 0)
            {
                hfAddedClassificationId.Value = Newtonsoft.Json.JsonConvert.SerializeObject((from b in classificationLst
                                                                                             select new
                                                                                             {
                                                                                                 ClassificationId = b.ClassificationId,
                                                                                                 DiscountId = b.DiscountId,
                                                                                                 DiscountAmount = b.DiscountAmount
                                                                                             }));
            }

            if (!string.IsNullOrEmpty(kotIdList))
            {
                kotIdList += "," + kotBillMaster.KotId.ToString();
            }
            else
            {
                kotIdList = kotBillMaster.KotId.ToString();
            }

            kotDetails = kotDetailsDA.GetRestaurantOrderItemByMultipleKotId(kotBillMaster.CostCenterId.ToString(), kotIdList, kotBillMaster.SourceName);

            RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
            paymentResume.KotBillMaster = kotBillMaster;
            paymentResume.KotBillDetails = kotDetails;
            paymentResume.RestaurantKotBill = kotBill;
            paymentResume.RestaurantKotBillPayment = kotBillPayment;

            var printedItem = kotDetails.Where(s => s.PrintFlag == true).FirstOrDefault();

            if (printedItem != null) { hfIsBillAlreadyPrint.Value = "1"; }

            //Session["RestaurantKotBillResume"] = paymentResume;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (kotBillMaster.KotId > 0)
                kotId = kotBillMaster.KotId.ToString();

            hfKotId.Value = kotId;
            hfCostCenterId.Value = costCenter;
            hfSourceId.Value = sourceId;
            hfKotId.Value = kotId;
            hfPaxQuantity.Value = kotBillMaster.PaxQuantity.ToString();

            hfBearerId.Value = kotBillMaster.BearerId.ToString();

            //hfBearerId.Value = userInformationBO.BearerId.ToString();

            hfBearerCanSettleBill.Value = (userInformationBO.IsRestaurantBillCanSettle == true ? "1" : "0");

            if (kotBill != null)
            {
                hfBillIdForInvoicePreview.Value = kotBill.BillId.ToString();
                hfBillId.Value = kotBill.BillId.ToString();
                hfBillIdDetailsId.Value = ((from k in billDetailList where k.BillId == kotBill.BillId select k.DetailId).FirstOrDefault()).ToString();
                //hfGuestList.Value = kotBill.CustomerName;
                hfIsBillPreviewButtonEnable.Value = kotBill.IsBillPreviewButtonEnable == true ? "1" : "0";
            }

            if (sourceType == "tkn")
            {
                sourceType = ConstantHelper.RestaurantBillSource.RestaurantToken.ToString();

                lblOrderSourceName.Text = "Token Number";
                lblOrderSourceNumber.Text = kotBillMaster.TokenNumber;
            }
            else if (sourceType == "tbl")
            {
                string addedTables = string.Empty;

                sourceType = ConstantHelper.RestaurantBillSource.RestaurantTable.ToString();

                RestaurantTableDA tableDa = new RestaurantTableDA();
                RestaurantTableBO tableBO = new RestaurantTableBO();
                List<RestaurantTableBO> tableLst = new List<RestaurantTableBO>();

                tableBO = tableDa.GetRestaurantTableInfoByTableId(kotBillMaster.SourceId);
                tableLst = tableDa.GetRestaurantTableInfoByMultipleTableId(tableIdList);

                if (tableLst.Count > 0)
                {
                    foreach (RestaurantTableBO tbl in tableLst)
                    {
                        if (!string.IsNullOrEmpty(addedTables))
                        {
                            addedTables += "," + tbl.TableNumber;
                        }
                        else
                        {
                            addedTables = tbl.TableNumber;
                        }
                    }
                }

                lblOrderSourceName.Text = "Table Number";
                lblOrderSourceNumber.Text = tableBO.TableNumber;

                if (!string.IsNullOrEmpty(addedTables))
                    lblAddedOrderSourceNumber.Text = " (" + addedTables + ")";

                if (kotBill != null)
                {
                    if (kotBill.RegistrationId > 0)
                    {
                        RoomRegistrationDA roomDa = new RoomRegistrationDA();
                        RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
                        roomRegistration = roomDa.GetRoomRegistrationInfoById(kotBill.RegistrationId); //GetRoomRegistrationInfoById, GetRoomRegistrationNRoomDetailsByRoomId

                        hfRoomNumber.Value = roomRegistration.RoomNumber.ToString();
                        hfRegistrationId.Value = roomRegistration.RegistrationId.ToString();
                        hfRoomId.Value = roomRegistration.RoomId.ToString();
                        hfIsStopChargePosting.Value = (costcneterDa.GetRoomStopChargePostingByRegistrationAndCostCenterId(roomRegistration.RegistrationId, Convert.ToInt32(costCenter))) == true ? "1" : "0";
                    }
                }

            }
            else if (sourceType == "rom")
            {
                sourceType = ConstantHelper.RestaurantBillSource.GuestRoom.ToString();

                RoomRegistrationDA roomDa = new RoomRegistrationDA();
                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
                roomRegistration = roomDa.GetRoomRegistrationNRoomDetailsByRoomId(Convert.ToInt32(sourceId)); //GetRoomRegistrationInfoById, GetRoomRegistrationNRoomDetailsByRoomId

                lblOrderSourceName.Text = "Room Number";
                lblOrderSourceNumber.Text = roomRegistration.RoomNumber.ToString();
                hfRegistrationId.Value = roomRegistration.RegistrationId.ToString();
                hfRoomId.Value = sourceId;
                hfIsStopChargePosting.Value = (costcneterDa.GetRoomStopChargePostingByRegistrationAndCostCenterId(roomRegistration.RegistrationId, Convert.ToInt32(costCenter))) == true ? "1" : "0";

            }

            hfsourceType.Value = sourceType;

            LoadCategoryWhenResumeBill(0);
            ResumeItemInformation(paymentResume);

            if (paymentResume.RestaurantKotBill != null)
                ResumePayemtInformation(paymentResume);
        }
        private void LoadCategoryRItemForOrder()
        {
            LoadCategoryNodeWise("RestaurantItemCategory:0");
        }
        protected void tvLocations_SelectedNodeChanged(object sender, EventArgs e)
        {
            string query = string.Empty;
            string valuePath = string.Empty, nodePathDepth = string.Empty;

            nodePathDepth = tvLocations.SelectedNode.ValuePath;

            if (tvLocations.SelectedNode.ChildNodes.Count > 0)
            {
                query = "RestaurantItemCategory:" + tvLocations.SelectedNode.Value.ToString();
            }
            else if (tvLocations.SelectedNode.ChildNodes.Count == 0)
            {
                query = "RestaurantItem:" + tvLocations.SelectedNode.Value.ToString();
            }

            tvLocations.CollapseAll();
            LoadCategoryNodeWise(query);

            string[] nodeDepth = nodePathDepth.Split('/');

            foreach (string node in nodeDepth)
            {
                if (!string.IsNullOrEmpty(valuePath))
                { valuePath += "/" + node; }
                else
                { valuePath = node; }

                tvLocations.FindNode(valuePath).Select();
                tvLocations.SelectedNode.ExpandAll();
            }
        }
        protected void btnLoadItemCategory_Click(object sender, EventArgs e)
        {
            string itemLoadType = string.Empty, valuePath = string.Empty, nodePathDepth = string.Empty;
            tvLocations.CollapseAll();

            itemLoadType = hfValuePath.Value;

            string[] criteria = itemLoadType.Split(',');

            LoadCategoryNodeWise(criteria[0]);
            nodePathDepth = criteria[1];

            nodePathDepth = nodePathDepth.Remove(0, 1);
            nodePathDepth = nodePathDepth.Remove(nodePathDepth.LastIndexOf('.'), 1);
            nodePathDepth = nodePathDepth.Replace('.', '/');

            string[] nodeDepth = nodePathDepth.Split('/');

            foreach (string node in nodeDepth)
            {
                if (!string.IsNullOrEmpty(valuePath))
                { valuePath += "/" + node; }
                else
                { valuePath = node; }

                tvLocations.FindNode(valuePath).Select();
                tvLocations.SelectedNode.ExpandAll();
            }

            //valuePath = "29/31/34";
            //tvLocations.FindNode("29").Select();
            //tvLocations.SelectedNode.Expand();

            //tvLocations.FindNode(valuePath).Select();
            //tvLocations.SelectedNode.Expand();
        }
        protected void tvLocations_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            e.Node.Expand();

            //foreach (TreeNode treenode in tvLocations.Nodes)
            //{
            //    if (treenode != e.Node)
            //    {
            //        treenode.Collapse();
            //    }
            //    //else if (treenode == e.Node)
            //    //{
            //    //    if(e.Node.pa)
            //    //    e.Node.Parent.Expand();
            //    //}               
            //}
        }
        protected void btnPrintPreview_Click(object sender, EventArgs e)
        {
            string queryStringId = hfBillId.Value;
            if (!string.IsNullOrEmpty(queryStringId))
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                int billID = Int32.Parse(queryStringId);
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSDCIntegrationEnable", "IsSDCIntegrationEnable");
                if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
                {
                    RestaurentBillDA rda = new RestaurentBillDA();
                    List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
                    restaurantBill = rda.GetRestaurantBillReport(billID);

                    if (setUpBO.SetupValue == "1")
                    {
                        SdcInvoiceHandler sdcInvHandler = new SdcInvoiceHandler("OrderManagement");
                        List<SdcBillReportBO> billReportsBo = new List<SdcBillReportBO>();

                        foreach (RestaurantBillReportBO bo in restaurantBill)
                        {
                            SdcBillReportBO reportBo = new SdcBillReportBO();
                            reportBo.ItemId = (int)bo.ItemId;
                            reportBo.ItemCode = bo.ItemCode;
                            reportBo.HsCode = "";
                            reportBo.ItemName = bo.ItemName;
                            reportBo.UnitRate = ((decimal)bo.Amount + (decimal)bo.ServiceCharge);
                            //reportBo.PaxQuantity = bo.PaxQuantity;
                            reportBo.PaxQuantity = 1;
                            if (bo.CitySDCharge > 0)
                            {
                                reportBo.SdCategory = "13751";
                            }
                            else
                            {
                                reportBo.SdCategory = "16051";
                            }

                            RestaurantBillBO mappingInfoBO = rda.GetItemCostCenterMappingInfo(bo.CostCenterId, bo.ItemId);
                            if (mappingInfoBO != null)
                            {
                                if (mappingInfoBO.VatAmount == 15)
                                {
                                    reportBo.VatCategory = "16042";
                                }
                                else
                                {
                                    reportBo.VatCategory = "14053";
                                }
                            }
                            else
                            {
                                reportBo.VatCategory = "14053";
                            }                         

                            billReportsBo.Add(reportBo);
                        }

                        sdcInvHandler.SdcInvoiceProcess(userInformationBO, billID, billReportsBo);
                                                
                        while (!sdcInvHandler.IsInvoiceReceived)
                        {
                            //Wait Until the invoice received from the NBR Server through the SDC Integrated device. This is just a blank while loop because i have to wait 
                            //for the response of SDC Device Event what i have fired inside the SdcInvoiceProcess above.
                            //After receiving the response I will call the ProcessReport method bellow.
                        }

                        if (sdcInvHandler.IsDeviceConnected)
                        {
                            this.ProcessReport(userInformationBO);
                        }
                        else
                        {
                            this.ProcessReportWithoutSDCIntegration(userInformationBO);
                        }
                    }
                    else
                    {
                        this.ProcessReportWithoutSDCIntegration(userInformationBO);
                    }
                }
            }
        }

        private void ProcessReport(UserInformationBO userInformationBO)
        {
            string reportName = "rptRestaurentBillForPos";

            try
            {

                string queryStringId = hfBillId.Value;
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    int billID = Int32.Parse(queryStringId);

                    //Session.Add("UserInformationBOSession", userInformationBO);

                    //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    rvTransactionShow.ProcessingMode = ProcessingMode.Local;
                    LocalReport report = rvTransactionShow.LocalReport;
                    rvTransactionShow.LocalReport.DataSources.Clear();

                    var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
                    if (!File.Exists(reportPath))
                        return;

                    rvTransactionShow.LocalReport.ReportPath = reportPath;

                    RestaurantBillBO billBO = new RestaurantBillBO();
                    RestaurentBillDA billDA = new RestaurentBillDA();
                    billBO = billDA.GetBillInfoByBillId(billID);

                    CompanyDA companyDA = new CompanyDA();
                    List<CompanyBO> files = companyDA.GetCompanyInfo();
                    List<ReportParameter> reportParam = new List<ReportParameter>();

                    if (files[0].CompanyId > 0)
                    {
                        reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                        reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
                        reportParam.Add(new ReportParameter("VatRegistrationNo", files[0].VatRegistrationNo));
                        reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

                        if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                        {
                            reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                        }

                        reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
                    }

                    rvTransactionShow.LocalReport.EnableExternalImages = true;
                    HMCommonDA hmCommonDA = new HMCommonDA();
                    //reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));

                    string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(billBO.CostCenterId);
                    if (!string.IsNullOrWhiteSpace(imageName))
                    {
                        reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                        //reportParam.Add(new ReportParameter("Path", "Hide"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("Path", "Hide"));
                    }

                    reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                    reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                    reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));
                    reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));

                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                    HMCommonSetupBO isCompanyNameShowOnRestaurantInvoice = new HMCommonSetupBO();
                    isCompanyNameShowOnRestaurantInvoice = commonSetupDA.GetCommonConfigurationInfo("IsCompanyNameShowOnRestaurantInvoice", "IsCompanyNameShowOnRestaurantInvoice");
                    if (Convert.ToInt32(isCompanyNameShowOnRestaurantInvoice.SetupValue) == 1)
                    {
                        reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "1"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "0"));
                    }

                    IsRestaurantOrderSubmitDisableInfo();

                    if (IsRestaurantOrderSubmitDisable)
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "No"));
                    }

                    IsRestaurantTokenInfoDisableInfo();

                    if (IsRestaurantTokenInfoDisable)
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
                    }

                    reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));


                    DateTime currentDate = DateTime.Now;
                    HMCommonDA printDateDA = new HMCommonDA();
                    //string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                    string printDate = currentDate.ToString(userInformationBO.ServerDateFormat + " " + userInformationBO.TimeFormat);

                    reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                    RestaurentBillDA rda = new RestaurentBillDA();
                    List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
                    restaurantBill = rda.GetRestaurantBillReport(billID);

                    String SDCInvoiceNumber = string.Empty;
                    String SDCQRCode = string.Empty;
                    HMCommonSetupBO setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSDCIntegrationEnable", "IsSDCIntegrationEnable");
                    if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
                    {

                        if (setUpBO.SetupValue == "1")
                        {
                            RestaurantBillBO RestaurantBillBOSDCInfo = new RestaurantBillBO();
                            RestaurantBillBOSDCInfo = rda.GetSDCInfoviceInformation(billID);
                            if (RestaurantBillBOSDCInfo.BillId > 0)
                            {
                                SDCInvoiceNumber = RestaurantBillBOSDCInfo.SDCInvoiceNumber;
                                SDCQRCode = RestaurantBillBOSDCInfo.QRCode;
                            }
                            //SDCQRCode = @"http://efdnbr.gov.bd/verify?param=Z01200200046.002020XWBQGEY034.TAr5odmwlLh9C%2F%2BLGDYXbFtcSeqGSfUugBeIYKTPTDuScyaNZojLOLFUsnRiPJdU";
                        }
                    }

                    // //----------------- Show Hide Related Information -------------------------------------------------------
                    string IsServiceChargeEnableConfig = "1";
                    string IsCitySDChargeEnableConfig = "1";
                    string IsVatAmountEnableConfig = "1";
                    string IsAdditionalChargeEnableConfig = "1";
                    string IsCostCenterNameShowOnInvoice = "1";

                    CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
                    CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                    if (restaurantBill != null)
                    {
                        if (restaurantBill.Count > 0)
                        {
                            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(restaurantBill[0].CostCenterId);

                            IsServiceChargeEnableConfig = costCentreTabBO.IsServiceChargeEnable ? "1" : "0";
                            IsCitySDChargeEnableConfig = costCentreTabBO.IsCitySDChargeEnable ? "1" : "0";
                            IsVatAmountEnableConfig = costCentreTabBO.IsVatEnable ? "1" : "0";
                            IsAdditionalChargeEnableConfig = costCentreTabBO.IsAdditionalChargeEnable ? "1" : "0";
                            IsCostCenterNameShowOnInvoice = costCentreTabBO.IsCostCenterNameShowOnInvoice ? "1" : "0";
                        }
                    }
                    reportParam.Add(new ReportParameter("ServiceChargeEnableConfig", IsServiceChargeEnableConfig));
                    reportParam.Add(new ReportParameter("CitySDChargeEnableConfig", IsCitySDChargeEnableConfig));
                    reportParam.Add(new ReportParameter("VatAmountEnableConfig", IsVatAmountEnableConfig));
                    reportParam.Add(new ReportParameter("AdditionalChargeEnableConfig", IsAdditionalChargeEnableConfig));
                    reportParam.Add(new ReportParameter("IsCostCenterNameShowOnInvoice", IsCostCenterNameShowOnInvoice));

                    byte[] QrImage;
                    HMCommonDA DA = new HMCommonDA();
                    QrImage = DA.GenerateQrCode(SDCQRCode);
                    reportParam.Add(new ReportParameter("SDCInvoiceNumber", SDCInvoiceNumber));
                    reportParam.Add(new ReportParameter("QrImage", Convert.ToBase64String(QrImage)));

                    rvTransactionShow.LocalReport.SetParameters(reportParam);

                    var dataSet = rvTransactionShow.LocalReport.GetDataSourceNames();
                    rvTransactionShow.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));

                    rvTransactionShow.LocalReport.DisplayName = "Invoice";
                    rvTransactionShow.LocalReport.Refresh();

                    string url = "$('#LoadReport').dialog({ " +
                           " autoOpen: true, " +
                           " modal: true, " +
                           " minWidth: 500, " +
                           " minHeight: 555, " +
                           " width: 'auto', " +
                           " closeOnEscape: false, " +
                           " resizable: false, " +
                           " height: 'auto', " +
                           " fluid: true, " +
                           " title: 'Invoice Preview', " +
                           " show: 'slide', " +
                           " close: ClosePrintDialog " +
                           "});" + "$('.ui-dialog-titlebar-close').css({ " +
                            " 'top': '27%', " +
                            " 'width': '40', " +
                            " 'height': '40', " +
                            " 'background-repeat': 'no-repeat', " +
                            " 'background-position': 'center center' " +
                            " }); " +
                            " $('.ui-dialog-titlebar').css('padding','0.8em 1em'); " +
                            " setTimeout(function () { ScrollToDown(); }, 1000); ";

                    ClientScript.RegisterStartupScript(GetType(), "script", url, true);

                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string extension;

                    byte[] bytes = rvTransactionShow.LocalReport.Render("PDF", null, out mimeType,
                                   out encoding, out extension, out streamids, out warnings);

                    string fileName = string.Empty, fileNamePrint = string.Empty;
                    DateTime dateTime = DateTime.Now;

                    fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + userInformationBO.UserInfoId.ToString() + ".pdf";
                    fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + userInformationBO.UserInfoId.ToString() + ".pdf";

                    FileStream fs = new FileStream(Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create, FileAccess.ReadWrite);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();

                    ////xml write
                    //var XmlPath = Server.MapPath(@"~/XMLDocument/Output/");

                    //if (restaurantBill.Count > 0)
                    //{
                    //    using (XmlWriter xmlWriter = XmlWriter.Create(XmlPath + restaurantBill[0].BillNumber + ".xml"))
                    //    {
                    //        xmlWriter.WriteStartElement("Invoice");

                    //        xmlWriter.WriteElementString("Plant_Code", restaurantBill[0].BillId.ToString());
                    //        xmlWriter.WriteElementString("Plant_Name", restaurantBill[0].CostCenter);
                    //        xmlWriter.WriteElementString("Invoice_Type", "Invoice");
                    //        xmlWriter.WriteElementString("Reference_Number", restaurantBill[0].BillNumber);
                    //        xmlWriter.WriteElementString("Invoice_Date", restaurantBill[0].BillDate);

                    //        xmlWriter.WriteElementString("Customer_Code", restaurantBill[0].CustomerId.ToString());////
                    //        xmlWriter.WriteElementString("Customer_Name", restaurantBill[0].CustomerName);
                    //        xmlWriter.WriteElementString("Customer_TIN", "");

                    //        if (restaurantBill[0].DiscountAmount > 0)
                    //        {
                    //            xmlWriter.WriteElementString("Payment_Type", "Cash");
                    //            xmlWriter.WriteElementString("Invoice_DiscOrAdd_Amount", (-restaurantBill[0].DiscountAmount).ToString());
                    //            xmlWriter.WriteElementString("Payment_Type", "Cash");
                    //            xmlWriter.WriteElementString("Invoice_DiscOrAdd_Amount", restaurantBill[0].DiscountAmount.ToString());
                    //        }

                    //        for (int index = 0; index < restaurantBill.Count; index++)
                    //        {
                    //            if (restaurantBill[index].TransactionType == "RestaurantPayment")
                    //            {
                    //                xmlWriter.WriteElementString("Payment_Type", restaurantBill[index].PayMode);
                    //                xmlWriter.WriteElementString("Invoice_DiscOrAdd_Amount", restaurantBill[index].GrandTotal.ToString());
                    //            }


                    //        }
                    //        for (int index = 0; index < restaurantBill.Count; index++)
                    //        {
                    //            if (restaurantBill[index].TransactionType == "RestaurantBill")
                    //            {
                    //                xmlWriter.WriteStartElement("Line_Items");
                    //                xmlWriter.WriteElementString("Item_Id", restaurantBill[index].ItemId.ToString());
                    //                xmlWriter.WriteElementString("Item_Description", restaurantBill[index].ItemName);
                    //                xmlWriter.WriteElementString("Item_Quantity", restaurantBill[index].ItemUnit.ToString());
                    //                xmlWriter.WriteElementString("Item_UOM", restaurantBill[index].UnitHead);////
                    //                xmlWriter.WriteElementString("Item_Unit_Price", restaurantBill[index].UnitRate.ToString());
                    //                xmlWriter.WriteElementString("Item_Tax_Percent", "0.00");////
                    //                xmlWriter.WriteElementString("Item_DiscOrAdd_Amount", "0.00");////
                    //                xmlWriter.WriteEndElement();
                    //            }

                    //        }
                    //        xmlWriter.WriteEndElement();

                    //        xmlWriter.Flush();
                    //    }
                    //}



                    //Open exsisting pdf

                    Document document = new Document(PageSize.LETTER);

                    PdfReader reader = new PdfReader(Server.MapPath("~/PrintDocument/" + fileName));
                    string dir_path = Server.MapPath("~/PrintDocument/" + fileNamePrint);
                    //Getting a instance of new pdf wrtiter

                    //FileStream fileStream = new FileStream(Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.CreateNew, FileAccess.ReadWrite);
                    FileStream fileStream = new FileStream(Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create, FileAccess.Write, FileShare.None);
                    //PdfWriter writer = PdfWriter.GetInstance(document, fileStream);

                    PdfWriter writer = this.PdfWriter_GetInstance(document, fileStream);
                    document.Open();
                    PdfContentByte cb = writer.DirectContent;

                    int i = 0;
                    int p = 0;
                    int n = reader.NumberOfPages;
                    Rectangle psize = reader.GetPageSize(1);

                    float width = psize.Width;
                    float height = psize.Height;

                    //Add Page to new document
                    while (i < n)
                    {
                        document.NewPage();
                        p++;
                        i++;

                        PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                        cb.AddTemplate(page1, 0, 0);
                    }

                    //Attach javascript to the document
                    PdfAction jAction = PdfAction.JavaScript("print(true);\r", writer);
                    writer.AddJavaScript(jAction);

                    document.Close();

                    frmPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;
                    //iFrameControl.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ProcessReportWithoutSDCIntegration(UserInformationBO userInformationBO)
        {
            string reportName = "rptRestaurentBillForPosWithoutSDC";

            try
            {
                string queryStringId = hfBillId.Value;
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    int billID = Int32.Parse(queryStringId);

                    CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
                    CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                    costCentreTabBO = costCentreTabDA.GetCostCenterDetailInformation("Restaurant", billID);

                    if (costCentreTabBO != null)
                    {
                        if (costCentreTabBO.InvoiceTemplate > 0)
                        {
                            if (costCentreTabBO.InvoiceTemplate == 1)
                            {
                                reportName = "rptRestaurentBillForPosWithoutSDC";
                            }
                            else if (costCentreTabBO.InvoiceTemplate == 2)
                            {
                                reportName = "rptRestaurentBillForDotMatrix";
                            }
                            else if (costCentreTabBO.InvoiceTemplate == 3)
                            {
                                reportName = "rptRestaurentBillTwoColumn";
                            }
                            else if (costCentreTabBO.InvoiceTemplate == 4)
                            {
                                reportName = "rptRestaurentBillForPosToken";
                            }
                            else if (costCentreTabBO.InvoiceTemplate == 5)
                            {
                                reportName = "rptRestaurentBillForA4Page";
                            }
                        }
                    }

                    rvTransactionShow.ProcessingMode = ProcessingMode.Local;
                    LocalReport report = rvTransactionShow.LocalReport;
                    rvTransactionShow.LocalReport.DataSources.Clear();

                    var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
                    if (!File.Exists(reportPath))
                        return;

                    rvTransactionShow.LocalReport.ReportPath = reportPath;

                    RestaurantBillBO billBO = new RestaurantBillBO();
                    RestaurentBillDA billDA = new RestaurentBillDA();
                    billBO = billDA.GetBillInfoByBillId(billID);

                    CompanyDA companyDA = new CompanyDA();
                    List<CompanyBO> files = companyDA.GetCompanyInfo();
                    List<ReportParameter> reportParam = new List<ReportParameter>();

                    if (files[0].CompanyId > 0)
                    {
                        reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                        reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
                        reportParam.Add(new ReportParameter("VatRegistrationNo", files[0].VatRegistrationNo));
                        reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

                        if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                        {
                            reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                        }
                        reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
                    }

                    rvTransactionShow.LocalReport.EnableExternalImages = true;
                    HMCommonDA hmCommonDA = new HMCommonDA();
                    string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(billBO.CostCenterId);
                    if (!string.IsNullOrWhiteSpace(imageName))
                    {
                        reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("Path", "Hide"));
                    }

                    reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                    reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                    reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));
                    reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));

                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                    HMCommonSetupBO isCompanyNameShowOnRestaurantInvoice = new HMCommonSetupBO();
                    isCompanyNameShowOnRestaurantInvoice = commonSetupDA.GetCommonConfigurationInfo("IsCompanyNameShowOnRestaurantInvoice", "IsCompanyNameShowOnRestaurantInvoice");
                    if (Convert.ToInt32(isCompanyNameShowOnRestaurantInvoice.SetupValue) == 1)
                    {
                        reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "1"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "0"));
                    }

                    IsRestaurantOrderSubmitDisableInfo();

                    if (IsRestaurantOrderSubmitDisable)
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "No"));
                    }

                    IsRestaurantTokenInfoDisableInfo();

                    if (IsRestaurantTokenInfoDisable)
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
                    }

                    reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));

                    DateTime currentDate = DateTime.Now;
                    HMCommonDA printDateDA = new HMCommonDA();
                    string printDate = currentDate.ToString(userInformationBO.ServerDateFormat + " " + userInformationBO.TimeFormat);
                    reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                    RestaurentBillDA rda = new RestaurentBillDA();
                    List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
                    restaurantBill = rda.GetRestaurantBillReport(billID);

                    // //----------------- Show Hide Related Information -------------------------------------------------------
                    string IsServiceChargeEnableConfig = "1";
                    string IsCitySDChargeEnableConfig = "1";
                    string IsVatAmountEnableConfig = "1";
                    string IsAdditionalChargeEnableConfig = "1";
                    string IsCostCenterNameShowOnInvoice = "1";

                    if (restaurantBill != null)
                    {
                        if (restaurantBill.Count > 0)
                        {
                            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(restaurantBill[0].CostCenterId);
                            IsServiceChargeEnableConfig = costCentreTabBO.IsServiceChargeEnable ? "1" : "0";
                            IsCitySDChargeEnableConfig = costCentreTabBO.IsCitySDChargeEnable ? "1" : "0";
                            IsVatAmountEnableConfig = costCentreTabBO.IsVatEnable ? "1" : "0";
                            IsAdditionalChargeEnableConfig = costCentreTabBO.IsAdditionalChargeEnable ? "1" : "0";
                            IsCostCenterNameShowOnInvoice = costCentreTabBO.IsCostCenterNameShowOnInvoice ? "1" : "0";
                        }
                    }
                    reportParam.Add(new ReportParameter("ServiceChargeEnableConfig", IsServiceChargeEnableConfig));
                    reportParam.Add(new ReportParameter("CitySDChargeEnableConfig", IsCitySDChargeEnableConfig));
                    reportParam.Add(new ReportParameter("VatAmountEnableConfig", IsVatAmountEnableConfig));
                    reportParam.Add(new ReportParameter("AdditionalChargeEnableConfig", IsAdditionalChargeEnableConfig));
                    reportParam.Add(new ReportParameter("IsCostCenterNameShowOnInvoice", IsCostCenterNameShowOnInvoice));

                    rvTransactionShow.LocalReport.SetParameters(reportParam);

                    var dataSet = rvTransactionShow.LocalReport.GetDataSourceNames();
                    rvTransactionShow.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));

                    rvTransactionShow.LocalReport.DisplayName = "Invoice";
                    rvTransactionShow.LocalReport.Refresh();

                    string url = "$('#LoadReport').dialog({ " +
                           " autoOpen: true, " +
                           " modal: true, " +
                           " minWidth: 500, " +
                           " minHeight: 555, " +
                           " width: 'auto', " +
                           " closeOnEscape: false, " +
                           " resizable: false, " +
                           " height: 'auto', " +
                           " fluid: true, " +
                           " title: 'Invoice Preview', " +
                           " show: 'slide', " +
                           " close: ClosePrintDialog " +
                           "});" + "$('.ui-dialog-titlebar-close').css({ " +
                            " 'top': '27%', " +
                            " 'width': '40', " +
                            " 'height': '40', " +
                            " 'background-repeat': 'no-repeat', " +
                            " 'background-position': 'center center' " +
                            " }); " +
                            " $('.ui-dialog-titlebar').css('padding','0.8em 1em'); " +
                            " setTimeout(function () { ScrollToDown(); }, 1000); ";

                    ClientScript.RegisterStartupScript(GetType(), "script", url, true);

                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string extension;

                    byte[] bytes = rvTransactionShow.LocalReport.Render("PDF", null, out mimeType,
                                   out encoding, out extension, out streamids, out warnings);

                    string fileName = string.Empty, fileNamePrint = string.Empty;
                    DateTime dateTime = DateTime.Now;

                    fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + userInformationBO.UserInfoId.ToString() + ".pdf";
                    fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + userInformationBO.UserInfoId.ToString() + ".pdf";

                    FileStream fs = new FileStream(Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create, FileAccess.ReadWrite);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();

                    ////xml write
                    //var XmlPath = Server.MapPath(@"~/XMLDocument/Output/");

                    //if (restaurantBill.Count > 0)
                    //{
                    //    using (XmlWriter xmlWriter = XmlWriter.Create(XmlPath + restaurantBill[0].BillNumber + ".xml"))
                    //    {
                    //        xmlWriter.WriteStartElement("Invoice");

                    //        xmlWriter.WriteElementString("Plant_Code", restaurantBill[0].BillId.ToString());
                    //        xmlWriter.WriteElementString("Plant_Name", restaurantBill[0].CostCenter);
                    //        xmlWriter.WriteElementString("Invoice_Type", "Invoice");
                    //        xmlWriter.WriteElementString("Reference_Number", restaurantBill[0].BillNumber);
                    //        xmlWriter.WriteElementString("Invoice_Date", restaurantBill[0].BillDate);

                    //        xmlWriter.WriteElementString("Customer_Code", restaurantBill[0].CustomerId.ToString());////
                    //        xmlWriter.WriteElementString("Customer_Name", restaurantBill[0].CustomerName);
                    //        xmlWriter.WriteElementString("Customer_TIN", "");

                    //        if (restaurantBill[0].DiscountAmount > 0)
                    //        {
                    //            xmlWriter.WriteElementString("Payment_Type", "Cash");
                    //            xmlWriter.WriteElementString("Invoice_DiscOrAdd_Amount", (-restaurantBill[0].DiscountAmount).ToString());
                    //            xmlWriter.WriteElementString("Payment_Type", "Cash");
                    //            xmlWriter.WriteElementString("Invoice_DiscOrAdd_Amount", restaurantBill[0].DiscountAmount.ToString());
                    //        }

                    //        for (int index = 0; index < restaurantBill.Count; index++)
                    //        {
                    //            if (restaurantBill[index].TransactionType == "RestaurantPayment")
                    //            {
                    //                xmlWriter.WriteElementString("Payment_Type", restaurantBill[index].PayMode);
                    //                xmlWriter.WriteElementString("Invoice_DiscOrAdd_Amount", restaurantBill[index].GrandTotal.ToString());
                    //            }


                    //        }
                    //        for (int index = 0; index < restaurantBill.Count; index++)
                    //        {
                    //            if (restaurantBill[index].TransactionType == "RestaurantBill")
                    //            {
                    //                xmlWriter.WriteStartElement("Line_Items");
                    //                xmlWriter.WriteElementString("Item_Id", restaurantBill[index].ItemId.ToString());
                    //                xmlWriter.WriteElementString("Item_Description", restaurantBill[index].ItemName);
                    //                xmlWriter.WriteElementString("Item_Quantity", restaurantBill[index].ItemUnit.ToString());
                    //                xmlWriter.WriteElementString("Item_UOM", restaurantBill[index].UnitHead);////
                    //                xmlWriter.WriteElementString("Item_Unit_Price", restaurantBill[index].UnitRate.ToString());
                    //                xmlWriter.WriteElementString("Item_Tax_Percent", "0.00");////
                    //                xmlWriter.WriteElementString("Item_DiscOrAdd_Amount", "0.00");////
                    //                xmlWriter.WriteEndElement();
                    //            }

                    //        }
                    //        xmlWriter.WriteEndElement();

                    //        xmlWriter.Flush();
                    //    }
                    //}



                    //Open exsisting pdf

                    Document document = new Document(PageSize.LETTER);

                    PdfReader reader = new PdfReader(Server.MapPath("~/PrintDocument/" + fileName));
                    string dir_path = Server.MapPath("~/PrintDocument/" + fileNamePrint);
                    //Getting a instance of new pdf wrtiter

                    //FileStream fileStream = new FileStream(Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.CreateNew, FileAccess.ReadWrite);
                    FileStream fileStream = new FileStream(Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create, FileAccess.Write, FileShare.None);
                    //PdfWriter writer = PdfWriter.GetInstance(document, fileStream);

                    PdfWriter writer = this.PdfWriter_GetInstance(document, fileStream);
                    document.Open();
                    PdfContentByte cb = writer.DirectContent;

                    int i = 0;
                    int p = 0;
                    int n = reader.NumberOfPages;
                    Rectangle psize = reader.GetPageSize(1);

                    float width = psize.Width;
                    float height = psize.Height;

                    //Add Page to new document
                    while (i < n)
                    {
                        document.NewPage();
                        p++;
                        i++;

                        PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                        cb.AddTemplate(page1, 0, 0);
                    }

                    //Attach javascript to the document
                    PdfAction jAction = PdfAction.JavaScript("print(true);\r", writer);
                    writer.AddJavaScript(jAction);

                    document.Close();

                    frmPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;
                    //iFrameControl.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private iTextSharp.text.pdf.PdfWriter PdfWriter_GetInstance(iTextSharp.text.Document document, System.IO.FileStream FS)
        {
            iTextSharp.text.pdf.PdfWriter writer = null;
            bool isInstanceCreated = false;
            while (!isInstanceCreated)
            {
                try
                {
                    writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, FS); // sometime rise exception on first call
                    isInstanceCreated = true;
                    break; //created, then exit loop
                }
                catch (NullReferenceException exp)
                {
                    System.Threading.Thread.Sleep(250); // wait for a while...
                }
            }
            if (writer == null) // check if instantiated
            {
                throw new Exception("iTextSharp PdfWriter is null");
            }

            return writer;
        }

        protected void btnBillNPrintPreview_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string discountType = string.Empty, costCenterId = string.Empty, discountAnount = string.Empty, categoryIdList = string.Empty;

            costCenterId = hfCostCenterId.Value;

            if (rbTPFixedDiscount.Checked)
            {
                discountType = "Fixed";
            }
            else if (rbTPPercentageDiscount.Checked)
            {
                discountType = " Percentage";
            }

            discountAnount = txtTPDiscountAmount.Text == "" ? "0" : txtTPDiscountAmount.Text.Trim();

            string queryStringId = hfBillId.Value;
            int billID = Int32.Parse(queryStringId);

            RestaurentBillDA billDA = new RestaurentBillDA();
            bool rtnValue = billDA.DistributionRestaurantBill(billID, categoryIdList, discountType, Convert.ToDecimal(discountAnount), Convert.ToInt32(costCenterId));

            if (!rtnValue)
            {
                return;
            }

            string reportName = "rptRestaurentBillForPos";

            if (!string.IsNullOrEmpty(queryStringId))
            {
                rvTransactionShow.ProcessingMode = ProcessingMode.Local;
                rvTransactionShow.LocalReport.DataSources.Clear();

                var reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/" + reportName + ".rdlc");
                if (!File.Exists(reportPath))
                    return;

                rvTransactionShow.LocalReport.ReportPath = reportPath;

                RestaurantBillBO billBO = new RestaurantBillBO();

                //billBO = billDA.GetBillInfoByBillId(billID);

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();
                List<ReportParameter> reportParam = new List<ReportParameter>();

                if (files[0].CompanyId > 0)
                {
                    reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                    reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
                    reportParam.Add(new ReportParameter("VatRegistrationNo", files[0].VatRegistrationNo));
                    reportParam.Add(new ReportParameter("CompanyType", files[0].CompanyType));

                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", files[0].ContactNumber));
                }

                rvTransactionShow.LocalReport.EnableExternalImages = true;
                HMCommonDA hmCommonDA = new HMCommonDA();
                //reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));

                string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(billBO.CostCenterId);
                if (!string.IsNullOrWhiteSpace(imageName))
                {
                    reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                }
                else
                {
                    reportParam.Add(new ReportParameter("Path", "Hide"));
                }

                reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));
                reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isCompanyNameShowOnRestaurantInvoice = new HMCommonSetupBO();
                isCompanyNameShowOnRestaurantInvoice = commonSetupDA.GetCommonConfigurationInfo("IsCompanyNameShowOnRestaurantInvoice", "IsCompanyNameShowOnRestaurantInvoice");
                if (Convert.ToInt32(isCompanyNameShowOnRestaurantInvoice.SetupValue) == 1)
                {
                    reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "1"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "0"));
                }

                IsRestaurantOrderSubmitDisableInfo();

                if (IsRestaurantOrderSubmitDisable)
                {
                    reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "No"));
                }

                IsRestaurantTokenInfoDisableInfo();

                if (IsRestaurantTokenInfoDisable)
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
                }

                HMCommonSetupBO isRestaurantIntegrateWithFrontOfficeBO = new HMCommonSetupBO();
                isRestaurantIntegrateWithFrontOfficeBO = commonSetupDA.GetCommonConfigurationInfo("IsGuestNameAndRoomNoTextShowInInvoice", "IsGuestNameAndRoomNoTextShowInInvoice");
                if (isRestaurantIntegrateWithFrontOfficeBO != null)
                {
                    if (isRestaurantIntegrateWithFrontOfficeBO.SetupValue == "1")
                    {
                        reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "1"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));
                    }
                }

                DateTime currentDate = DateTime.Now;
                HMCommonDA printDateDA = new HMCommonDA();
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                RestaurentBillDA rda = new RestaurentBillDA();
                List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
                restaurantBill = rda.GetRestaurantBillReport(billID);

                // //----------------- Show Hide Related Information -------------------------------------------------------
                string IsServiceChargeEnableConfig = "1";
                string IsCitySDChargeEnableConfig = "1";
                string IsVatAmountEnableConfig = "1";
                string IsAdditionalChargeEnableConfig = "1";
                string IsCostCenterNameShowOnInvoice = "1";

                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
                CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                if (restaurantBill != null)
                {
                    if (restaurantBill.Count > 0)
                    {
                        costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(restaurantBill[0].CostCenterId);

                        IsServiceChargeEnableConfig = costCentreTabBO.IsServiceChargeEnable ? "1" : "0";
                        IsCitySDChargeEnableConfig = costCentreTabBO.IsCitySDChargeEnable ? "1" : "0";
                        IsVatAmountEnableConfig = costCentreTabBO.IsVatEnable ? "1" : "0";
                        IsAdditionalChargeEnableConfig = costCentreTabBO.IsAdditionalChargeEnable ? "1" : "0";
                        IsCostCenterNameShowOnInvoice = costCentreTabBO.IsCostCenterNameShowOnInvoice ? "1" : "0";
                    }
                }
                reportParam.Add(new ReportParameter("ServiceChargeEnableConfig", IsServiceChargeEnableConfig));
                reportParam.Add(new ReportParameter("CitySDChargeEnableConfig", IsCitySDChargeEnableConfig));
                reportParam.Add(new ReportParameter("VatAmountEnableConfig", IsVatAmountEnableConfig));
                reportParam.Add(new ReportParameter("AdditionalChargeEnableConfig", IsAdditionalChargeEnableConfig));
                reportParam.Add(new ReportParameter("IsCostCenterNameShowOnInvoice", IsCostCenterNameShowOnInvoice));

                rvTransactionShow.LocalReport.SetParameters(reportParam);

                var dataSet = rvTransactionShow.LocalReport.GetDataSourceNames();
                rvTransactionShow.LocalReport.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));

                rvTransactionShow.LocalReport.DisplayName = "Invoice";
                rvTransactionShow.LocalReport.Refresh();

                string url = "$('#LoadReport').dialog({ " +
                      " autoOpen: true, " +
                      " modal: true, " +
                      " minWidth: 500, " +
                      " minHeight: 555, " +
                      " width: 'auto', " +
                      " closeOnEscape: false, " +
                      " resizable: false, " +
                      " height: 'auto', " +
                      " fluid: true, " +
                      " title: 'Invoice Preview', " +
                      " show: 'slide', " +
                      " });" + "$('.ui-dialog-titlebar-close').css({ " +
                       " 'top': '27%', " +
                       " 'width': '40', " +
                       " 'height': '40', " +
                       " 'background-repeat': 'no-repeat', " +
                       " 'background-position': 'center center' " +
                       " }); " +
                       " $('.ui-dialog-titlebar').css('padding','0.8em 1em'); " +
                       " setTimeout(function () { ScrollToDown(); }, 1000); ";

                ClientScript.RegisterStartupScript(GetType(), "script", url, true);

                return;

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rvTransactionShow.LocalReport.Render("PDF", null, out mimeType,
                               out encoding, out extension, out streamids, out warnings);

                string fileName = string.Empty, fileNamePrint = string.Empty;
                DateTime dateTime = DateTime.Now;
                fileName = "OutPut" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + userInformationBO.UserInfoId.ToString() + ".pdf";
                fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + userInformationBO.UserInfoId.ToString() + ".pdf";

                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                //Open exsisting pdf
                Document document = new Document(PageSize.LETTER);
                PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));
                //Getting a instance of new pdf wrtiter
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
                   HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
                document.Open();
                PdfContentByte cb = writer.DirectContent;

                int i = 0;
                int p = 0;
                int n = reader.NumberOfPages;
                Rectangle psize = reader.GetPageSize(1);

                float width = psize.Width;
                float height = psize.Height;

                //Add Page to new document
                while (i < n)
                {
                    document.NewPage();
                    p++;
                    i++;

                    PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                    cb.AddTemplate(page1, 0, 0);
                }

                //Attach javascript to the document
                PdfAction jAction = PdfAction.JavaScript("print(true);\r", writer);
                //PdfAction jAction = PdfAction.JavaScript("var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = getPrintParams().printerName;print(pp);\r", writer);
                writer.AddJavaScript(jAction);

                document.Close();

                frmPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;

                //rrp.PrintForPos();

                //string url = "/Restaurant/Reports/frmReportTPRestaurantBillInfo.aspx?billID=" + hfBillId.Value;
                //string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes'); window.onunload = CloseWindow();";
                ////Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "window.onunload = CloseWindow();");
                //ClientScript.RegisterStartupScript(GetType(), "script", s, true);

            }
        }
        //************************ User Defined Function ********************//        
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();

            //LocalCurrencyId = commonCurrencyBO.CurrencyId;
            hfLocalCurrencyId.Value = commonCurrencyBO.CurrencyId.ToString();
        }
        private void LoadCategoryWhenResumeBill(int categoryId)
        {
            LoadRestaurantItemCategory(categoryId);
        }
        private void ResumeItemInformation(RestaurantBillPaymentResume paymentResume)
        {
            string strTable = string.Empty;
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(paymentResume.KotBillMaster.CostCenterId);

            if (costCentreTabBO != null)
            {
                if (costCentreTabBO.CostCenterId > 0)
                {
                    string costCenterDefaultView = string.Empty;

                    if (costCentreTabBO.DefaultView == "Token")
                    {
                        costCenterDefaultView = "RestaurantToken";
                    }
                    else if (costCentreTabBO.DefaultView == "Table")
                    {
                        costCenterDefaultView = "RestaurantTable";
                    }
                    else if (costCentreTabBO.DefaultView == "Room")
                    {
                        costCenterDefaultView = "GuestRoom";
                    }

                    Boolean isChangedExist = false;
                    foreach (KotBillDetailBO drIsChanged in paymentResume.KotBillDetails)
                    {
                        if (drIsChanged.IsChanged)
                        {
                            isChangedExist = true;
                            break;
                        }
                    }

                    strTable = "<div id='no-more-tables'> ";
                    strTable += "<table cellspacing='0' cellpadding='4' id='TableWiseItemInformation' table-bordered table-striped table-condensed cf> <thead class='cf'> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                    strTable += "<th align='left' scope='col'>Item Name</th><th align='left' scope='col'>Unit</th><th align='left' scope='col'>U. Rate</th><th align='left' scope='col'>Total</th><th align='center' scope='col'>Action</th></tr></thead>";
                    strTable += "<tbody>";
                    int counter = 0;

                    foreach (KotBillDetailBO dr in paymentResume.KotBillDetails)
                    {
                        counter++;
                        if (counter % 2 == 0)
                        {
                            // It's even
                            strTable += "<tr style='background-color:#E3EAEB;'> <td data-title='Item Name' align='left' style='width: 40%;'>" + dr.ItemName + "</td>";
                        }
                        else
                        {
                            // It's odd
                            strTable += "<tr style='background-color:White;'><td data-title='Item Name' align='left' style='width: 40%;'>" + dr.ItemName + "</td>";
                        }

                        strTable += "<td data-title='Unit' align='left' style='width: 15%;'>" + dr.ItemUnit + "</td>";
                        strTable += "<td data-title='Unit Rate' align='left' style='width: 15%;'>" + dr.UnitRate + "</td>";
                        strTable += "<td data-title='Total' align='left' style='width: 15%;'>" + (dr.ItemUnit * dr.UnitRate) + "</td>";

                        //strTable += "<td align='center' style='width: 15%;'>";
                        if (dr.KotDetailId > 0)
                        {
                            if (!dr.PrintFlag)
                            {
                                strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-primary' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 0 + "," + (dr.IsItemEditable == true ? 1 : 0) + ")' alt='Action Decider'>Option</button></td>";
                            }
                            else
                            {
                                if (!isChangedExist)
                                {
                                    strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-success' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 1 + "," + (dr.IsItemEditable == true ? 1 : 0) + ")' alt='Action Decider'>Option</button></td>";
                                }
                                else
                                {
                                    strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-primary' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 1 + "," + (dr.IsItemEditable == true ? 1 : 0) + ")' alt='Action Decider'>Option</button></td>";
                                }
                            }

                            //strTable += "<td align='center' style='width: 15%;'><img src='../Images/actiondecider.png' onClick='javascript:return AddNewItem(" + dr.KotDetailId + ")' alt='Action Decider' border='0' /></td>";
                            //strTable += "<td align='center' style='width: 15%;'><img src='../Images/edit.png' onClick='javascript:return AddNewItem(" + dr.KotDetailId + ")' alt='Edit Information' border='0' /></td>";
                            //strTable += "<td align='center' style='width: 15%;'><img src='../Images/delete.png' onClick='javascript:return PerformDeleteAction(" + dr.KotDetailId + ")' alt='Delete Information' border='0' /></td>";
                        }

                        strTable += "<td align='left' style='display:none;'>" + dr.ItemId + "</td>";

                        strTable += "<td align='left' style='display:none;'>" + dr.ClassificationId + "</td>";

                        strTable += "<td align='left' style='display:none;'>" +
                            (dr.ItemWiseDiscountType == "Fixed" ? "Percentage" : dr.ItemWiseDiscountType)
                                 + "</td>";
                        strTable += "<td align='left' style='display:none;'>" +
                                    dr.ItemWiseIndividualDiscount.ToString()
                            //(dr.ItemWiseDiscountType == "Fixed" ? ((dr.ItemWiseIndividualDiscount / dr.UnitRate) * 100).ToString("0.00") : dr.ItemWiseIndividualDiscount.ToString())
                            + "</td>";

                        strTable += "</tr>";

                        if (dr.ItemType == "BuffetItem")
                        {
                            string strBuffetDetail = string.Empty;
                            List<RestaurantBuffetDetailBO> buffetDetailListBO = new List<RestaurantBuffetDetailBO>();
                            RestaurantBuffetDetailDA buffetDetailDA = new RestaurantBuffetDetailDA();

                            //buffetDetailListBO = buffetDetailDA.GetRestaurantBuffetDetailByBuffetId(dr.ItemId);
                            //foreach (RestaurantBuffetDetailBO drDetail in buffetDetailListBO)
                            //{
                            //    //int tmpItemUnit = Convert.ToInt32(drDetail.ItemUnit);
                            //    strBuffetDetail += ", " + drDetail.ItemName;
                            //}
                            //strBuffetDetail = strBuffetDetail.Substring(2, strBuffetDetail.Length - 2);
                            strTable += "<tr><td align='left' style='width: 40%; padding-left:20px;' colspan='5'>" + strBuffetDetail + "</td>";
                        }

                        if (dr.ItemType == "ComboItem")
                        {
                            string strComboDetail = string.Empty;
                            List<InvItemDetailsBO> ownerDetailListBO = new List<InvItemDetailsBO>();
                            InvItemDetailsDA ownerDetailDA = new InvItemDetailsDA();

                            ownerDetailListBO = ownerDetailDA.GetInvItemDetailsByItemId(dr.ItemId);
                            foreach (InvItemDetailsBO drDetail in ownerDetailListBO)
                            {
                                int tmpItemUnit = Convert.ToInt32(drDetail.ItemUnit);
                                strComboDetail += ", " + drDetail.ItemName + "(" + tmpItemUnit + ")";
                            }
                            strComboDetail = strComboDetail.Substring(2, strComboDetail.Length - 2);
                            strTable += "<tr><td align='left' style='width: 40%; padding-left:20px;' colspan='5'>" + strComboDetail + "</td>";
                        }
                    }
                    strTable += "</tbody> </table> </div>";
                    if (strTable == "")
                    {
                        strTable = "<tr><td data-title='Item Name' colspan='4' align='center'>No Record Available!</td></tr>";
                    }
                }
            }
        }
        private void ResumePayemtInformation(RestaurantBillPaymentResume paymentResume)
        {
            decimal totalPayment = 0;

            txtSalesAmount.Text = paymentResume.RestaurantKotBill.SalesAmount.ToString();
            txtDiscountedAmount.Text = (paymentResume.RestaurantKotBill.SalesAmount - paymentResume.RestaurantKotBill.DiscountAmount).ToString();

            //txtServiceCharge.Text = paymentResume.RestaurantKotBill.ServiceCharge.ToString();
            //txtVatAmount.Text = paymentResume.RestaurantKotBill.VatAmount.ToString();

            if (paymentResume.RestaurantKotBill.DiscountType == HMConstants.DiscountType.Fixed.ToString())
            {
                rbTPFixedDiscount.Checked = true;
                rbTPPercentageDiscount.Checked = false;
            }
            else if (paymentResume.RestaurantKotBill.DiscountType == HMConstants.DiscountType.Percentage.ToString())
            {
                rbTPPercentageDiscount.Checked = true;
            }

            txtTPDiscountAmount.Text = paymentResume.RestaurantKotBill.DiscountAmount.ToString();
            txtTPDiscountedAmount.Text = (paymentResume.RestaurantKotBill.SalesAmount - paymentResume.RestaurantKotBill.DiscountAmount).ToString();

            txtTPServiceCharge.Text = paymentResume.RestaurantKotBill.ServiceCharge.ToString();
            txtTPVatAmount.Text = paymentResume.RestaurantKotBill.VatAmount.ToString();

            txtTPSDCharge.Text = paymentResume.RestaurantKotBill.CitySDCharge.ToString();
            txtTPAdditionalCharge.Text = paymentResume.RestaurantKotBill.AdditionalCharge.ToString();

            txtCitySDCharge.Text = paymentResume.RestaurantKotBill.CitySDCharge.ToString();
            txtAdditionalCharge.Text = paymentResume.RestaurantKotBill.AdditionalCharge.ToString();
            txtServiceCharge.Text = paymentResume.RestaurantKotBill.ServiceCharge.ToString();
            txtVatAmount.Text = paymentResume.RestaurantKotBill.VatAmount.ToString();

            foreach (GuestBillPaymentBO rb in paymentResume.RestaurantKotBillPayment)
            {
                if (rb.PaymentMode == HMConstants.PaymentMode.Cash.ToString())
                {
                    txtCash.Text = rb.PaymentAmount.ToString();
                    totalPayment += rb.PaymentAmount;
                }
                else if (rb.PaymentMode == HMConstants.PaymentMode.Card.ToString())
                {
                    totalPayment += rb.PaymentAmount;

                    if (rb.CardType == Convert.ToChar(HMConstants.CardType.AmexCard).ToString())
                    {
                        txtAmexCard.Text = rb.PaymentAmount.ToString();
                    }
                    else if (rb.CardType == Convert.ToChar(HMConstants.CardType.MasterCard).ToString())
                    {
                        txtMasterCard.Text = rb.PaymentAmount.ToString();
                    }
                    else if (rb.CardType == Convert.ToChar(HMConstants.CardType.VisaCard).ToString())
                    {
                        txtVisaCard.Text = rb.PaymentAmount.ToString();
                    }
                    else if (rb.CardType == Convert.ToChar(HMConstants.CardType.DiscoverCard).ToString())
                    {
                        txtDiscoverCard.Text = rb.PaymentAmount.ToString();
                    }
                }
                else if (rb.PaymentMode == HMConstants.PaymentMode.Rounded.ToString())
                {
                    txtTPRoundedAmount.Text = rb.PaymentAmount.ToString();
                    txtRoundedAmount.Text = rb.PaymentAmount.ToString();
                    // txtTPRoundedAmountHiddenField.Value = rb.PaymentAmount.ToString();
                }
                else if (rb.PaymentMode == HMConstants.PaymentMode.Refund.ToString())
                {
                    txtChangeAmount.Text = rb.PaymentAmount.ToString();
                    lblTPChangeAmount.Text = "Change Amount";
                }
                else if (rb.PaymentMode == HMConstants.PaymentMode.Company.ToString())
                {
                    txtCompanyPayment.Text = rb.PaymentAmount.ToString();

                    GuestCompanyBO guestCompany = new GuestCompanyBO();
                    GuestCompanyDA company = new GuestCompanyDA();
                    guestCompany = company.GetGuestCompanyInfoById(Convert.ToInt32(paymentResume.RestaurantKotBill.TransactionId));

                    lblCompanyName.Text = guestCompany.CompanyName;
                    hfCompanyId.Value = guestCompany.CompanyId.ToString();

                }
                else if (rb.PaymentMode == HMConstants.PaymentMode.Employee.ToString())
                {
                    EmployeeDA empDa = new EmployeeDA();
                    EmployeeBO bo = new EmployeeBO();

                    bo = empDa.GetEmployeeInfoById(Convert.ToInt32(paymentResume.RestaurantKotBill.TransactionId));

                    txtEmployeePayment.Text = rb.PaymentAmount.ToString();
                    hfEmployeeId.Value = bo.EmpId.ToString();
                    lblEmployeeName.Text = bo.DisplayName + " (" + bo.EmpCode + ")";
                }
                else if (rb.PaymentMode == HMConstants.PaymentMode.Member.ToString())
                {
                    MemMemberBasicDA empDa = new MemMemberBasicDA();
                    MemMemberBasicsBO bo = new MemMemberBasicsBO();

                    bo = empDa.GetMemberInfoById(Convert.ToInt32(paymentResume.RestaurantKotBill.TransactionId));
                    lblMemberName.Text = bo.FullName + " (" + bo.MembershipNumber + ")" + "(" + bo.TypeName + ")";
                    hfMemberId.Value = bo.MemberId.ToString();
                    txtMemberPayment.Text = rb.PaymentAmount.ToString();
                }
            }

            txtTotalPaymentAmount.Text = totalPayment.ToString();

            if (paymentResume.RoomWiseBillPayment.RegistrationId != null)
            {
                RoomRegistrationDA roomDa = new RoomRegistrationDA();
                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
                roomRegistration = roomDa.GetRoomRegistrationInfoById(paymentResume.RestaurantKotBill.RegistrationId); //GetRoomRegistrationInfoById, GetRoomRegistrationNRoomDetailsByRoomId                    
                paymentResume.RestaurantKotBill.RegistrationId = roomRegistration.RegistrationId;
                hfRoomId.Value = roomRegistration.RoomId.ToString();
                hfRoomNumber.Value = roomRegistration.RoomNumber.ToString();
                hfIsRoomIsSelectForPayment.Value = "1";
                txtRoomPayment.Text = paymentResume.RoomWiseBillPayment.CalculatedTotalAmount.ToString();
            }
            else if (paymentResume.RestaurantKotBill.TransactionType == HMConstants.PaymentMode.Company.ToString())
            {
                GuestCompanyBO guestCompany = new GuestCompanyBO();
                GuestCompanyDA company = new GuestCompanyDA();
                guestCompany = company.GetGuestCompanyInfoById(Convert.ToInt32(paymentResume.RestaurantKotBill.TransactionId));

                lblCompanyName.Text = guestCompany.CompanyName;
                hfCompanyId.Value = guestCompany.CompanyId.ToString();
            }
            else if (paymentResume.RestaurantKotBill.TransactionType == HMConstants.PaymentMode.Employee.ToString())
            {
                EmployeeDA empDa = new EmployeeDA();
                EmployeeBO bo = new EmployeeBO();

                bo = empDa.GetEmployeeInfoById(Convert.ToInt32(paymentResume.RestaurantKotBill.TransactionId));

                hfEmployeeId.Value = bo.EmpId.ToString();
                lblEmployeeName.Text = bo.DisplayName + " (" + bo.EmpCode + ")";
            }
            else if (paymentResume.RestaurantKotBill.TransactionType == HMConstants.PaymentMode.Member.ToString())
            {
                MemMemberBasicDA empDa = new MemMemberBasicDA();
                MemMemberBasicsBO bo = new MemMemberBasicsBO();

                bo = empDa.GetMemberInfoById(Convert.ToInt32(paymentResume.RestaurantKotBill.TransactionId));
                lblMemberName.Text = bo.FullName + " (" + bo.MembershipNumber + ")" + "(" + bo.TypeName + ")";
                hfMemberId.Value = bo.MemberId.ToString();
            }

            txtTotalPaymentAmount.Text = totalPayment.ToString();

            if ((paymentResume.RestaurantKotBill.GrandTotal - totalPayment) > 0)
            {
                txtTPChangeAmount.Text = (paymentResume.RestaurantKotBill.GrandTotal - totalPayment).ToString();
            }
        }
        private void GetTopLevelLocations(bool? expand)
        {
            try
            {
                TreeNode oNode = null;
                List<InvCategoryBO> dtObjects;
                string selectedVal = (tvLocations.SelectedNode != null) ? tvLocations.SelectedNode.Value : string.Empty;
                TreeNode selectedNode = null;
                tvLocations.Nodes.Clear();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                dtObjects = moLocation.GetTPInvCategoryInfoByCustomString("WHERE  ic.lvl = 0 AND ic.ActiveStat = 1 AND ISNULL(ccc.IsRestaurant, 0) <> 0 ORDER BY ic.NAME ASC").Where(x => x.CostCenterId == userInformationBO.WorkingCostCenterId).ToList();

                foreach (InvCategoryBO item in dtObjects)
                {
                    oNode = new TreeNode(HttpUtility.HtmlEncode(item.Name), item.CategoryId.ToString());
                    oNode.Expanded = false;
                    //oNode.NavigateUrl = "frmRestaurantManagement.aspx?IR=RestaurantItemCategory:" + item.CategoryId.ToString();

                    if (selectedVal == oNode.Value)
                        selectedNode = oNode;

                    GetChildLocations(ref oNode, expand, ref selectedNode, selectedVal);
                    //oNode.NavigateUrl = "frmRestaurantManagement.aspx?IR=RestaurantItemCategory:" + item.CategoryId.ToString();
                    tvLocations.Nodes.Add(oNode);
                }

                if (selectedNode != null && expand != false)
                {
                    while (selectedNode.Parent != null)
                    {
                        selectedNode.Parent.Expanded = true;
                        selectedNode = selectedNode.Parent;
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogEvent(ErrorLog.EventType.eEVENT_CRITICAL, ToString(), "GetTopLevelLocations()", ex.Message, "", String.Format("\n\n{0}", ex.StackTrace), false);
                //Debug.Assert(false, ToString() + "GetTopLevelLocations() - Exception " + ex.Message);
                throw ex;
            }
            finally
            {
            }
        }
        private void GetChildLocations(ref TreeNode oParent, bool? expand, ref TreeNode selectedNode, string selectedVal)
        {
            try
            {
                List<InvCategoryBO> dtObjects;
                TreeNode oNode;
                int iLevel;
                iLevel = oParent.Depth + 1;
                dtObjects = moLocation.GetInvCategoryInfoByCustomString(String.Format("WHERE  lvl = {0} AND AncestorId = {1} ORDER BY NAME ASC", iLevel, oParent.Value));


                List<InvCategoryBO> invCategoryListBO = dtObjects.Where(x => x.ActiveStat == true).ToList();

                foreach (InvCategoryBO item in invCategoryListBO)
                {
                    oNode = new TreeNode(HttpUtility.HtmlEncode(item.Name), item.CategoryId.ToString());
                    oNode.Expanded = false;
                    //oNode.NavigateUrl = "frmRestaurantManagement.aspx?IR=RestaurantItem:" + item.CategoryId.ToString();

                    if (selectedVal == oNode.Value)
                        selectedNode = oNode;

                    oParent.ChildNodes.Add(oNode);

                    GetChildLocations(ref oNode, expand, ref selectedNode, selectedVal);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogEvent(ErrorLog.EventType.eEVENT_CRITICAL, ToString(), "Load_Locations", ex.Message, "", String.Format("\n\n{0}", ex.StackTrace), false);
                //Debug.Assert(false, String.Format("{0}Load_Locations - Exception {1}", ToString(), ex.Message));
                throw ex;
            }
            finally
            {
                ////tvLocations.EndUnboundLoad();
                ////Cursor.Current = Cursors.Default;
            }
        }
        private void LoadCategoryNodeWise(string query)
        {
            string[] querySplit = query.Split(':');

            if (querySplit[0] == "RestaurantItemCategory")
            {
                int categoryId = !string.IsNullOrWhiteSpace(querySplit[1].ToString()) ? Convert.ToInt32(querySplit[1].ToString()) : 0;
                LoadRestaurantItemCategory(categoryId);
            }
            else if (querySplit[0] == "RestaurantItem")
            {
                int itemId = !string.IsNullOrWhiteSpace(querySplit[1].ToString()) ? Convert.ToInt32(querySplit[1].ToString()) : 0;
                LoadRestaurantItem(itemId.ToString());
            }

        }
        private void IsRestaurantBillAmountWillRound()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
            isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillAmountWillRound", "IsRestaurantBillAmountWillRound");

            hfIsRestaurantBillAmountWillRound.Value = "1";
            if (isRestaurantBillAmountWillRoundBO != null)
            {
                hfIsRestaurantBillAmountWillRound.Value = isRestaurantBillAmountWillRoundBO.SetupValue;
            }
        }
        private void IsRestaurantOrderSubmitDisableInfo()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantOrderSubmitDisable", "IsRestaurantOrderSubmitDisable");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        //btnOrderSubmit.Visible = false;
                        IsRestaurantOrderSubmitDisable = false;
                    }
                    else
                    {
                        //btnOrderSubmit.Visible = true;
                        IsRestaurantOrderSubmitDisable = true;
                    }
                }
            }

            //imgBtnRoomWiseKotBill.Visible = false;
        }
        private void IsRestaurantTokenInfoDisableInfo()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantTokenInfoDisable", "IsRestaurantTokenInfoDisable");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        //btnOrderSubmit.Visible = false;
                        IsRestaurantTokenInfoDisable = false;
                    }
                    else
                    {
                        //btnOrderSubmit.Visible = true;
                        IsRestaurantTokenInfoDisable = true;
                    }
                }
            }

            //imgBtnRoomWiseKotBill.Visible = false;
        }
        #region Print Option
        public void PrintRestaurantBill(int billID)
        {
            PrintInfos pinf = new PrintInfos();

            HMUtility hmUtility = new HMUtility();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
            invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillInvoiceTemplate", "RestaurantBillInvoiceTemplate");


            PrinterInfoDA da = new PrinterInfoDA();
            RestaurentBillDA rda = new RestaurentBillDA();
            KotBillDetailDA billDA = new KotBillDetailDA();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();

            List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();

            restaurantBill = rda.GetRestaurantBillReport(billID);

            if (restaurantBill != null)
            {
                if (restaurantBill.Count > 0)
                {
                    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantOrderSubmitDisable", "IsRestaurantOrderSubmitDisable");

                    if (commonSetupBO != null)
                    {
                        if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                        {
                            if (commonSetupBO.SetupValue == "0")
                            {
                                pinf.IsRestaurantOrderSubmitDisable = false;
                            }
                            else
                            {
                                pinf.IsRestaurantOrderSubmitDisable = true;
                            }
                        }
                    }

                    pinf.TableNumberInformation = restaurantBill[0].TableNumber;
                    pinf.KotIdInformation = Convert.ToInt32(restaurantBill[0].KotId);

                    //parms[3] = new ReportParameter("TableNo", tableNumberInformation);
                    //parms[4] = new ReportParameter("KotNo", kotIdInformation.ToString());

                    List<PrinterInfoBO> files = da.GetRestaurentItemTypeInfoByKotId(Convert.ToInt32(restaurantBill[0].KotId));
                    List<PrinterInfoBO> invoicePrinter = da.GetPrinterInfoByCostCenterPrintType(restaurantBill[0].CostCenterId, "InvoiceItem");

                    //if (files.Count() == 0)
                    //{
                    //    files.Add(new PrinterInfoBO() { PrinterName = "Microsoft XPS Document Writer", StockType = "StockItem" });
                    //    files.Add(new PrinterInfoBO() { PrinterName = "Microsoft XPS Document Writer", StockType = "KitchenItem" });
                    //}

                    if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == (int)(HMConstants.PrintTemplate.Template1))
                    {
                        PrintReportRestaurantBill(invoicePrinter[0], restaurantBill, pinf);

                        foreach (PrinterInfoBO pinfo in files)
                        {
                            pinf.CostCenterId = pinfo.CostCenterId;
                            pinf.PrinterInfoId = pinfo.PrinterInfoId;
                            pinf.CostCenterId = pinfo.CostCenterId;
                            pinf.CostCenterName = pinfo.CostCenter;
                            pinf.CompanyName = pinfo.KitchenOrStockName;

                            if (pinfo.DefaultView == "Table")
                            {
                                pinf.CostCenterDefaultView = "Table # ";
                            }
                            else if (pinfo.DefaultView == "Token")
                            {
                                pinf.CostCenterDefaultView = "Token # ";
                            }
                            else if (pinfo.DefaultView == "Room")
                            {
                                pinf.CostCenterDefaultView = "Room # ";
                            }

                            UserInformationBO userInformationBO = new UserInformationBO();
                            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                            pinf.WaiterName = userInformationBO.UserName; //userInformationBO.EmployeeName.ToString();

                            if (pinfo.StockType == "StockItem")
                            {
                                entityBOList = billDA.GetKotOrderSubmitInfo(Convert.ToInt32(restaurantBill[0].KotId), restaurantBill[0].CostCenterId, "StockItem", true);
                            }
                            else
                            {
                                entityBOList = billDA.GetKotOrderSubmitInfo(Convert.ToInt32(restaurantBill[0].KotId), restaurantBill[0].CostCenterId, "KitchenItem", true).Where(x => x.KitchenId == pinfo.KitchenId && x.PrinterName == pinfo.PrinterName).ToList();
                            }

                            if (entityBOList.Count > 0)
                            {
                                PrintReportKot(pinfo, entityBOList, pinf, false);
                            }
                        }
                    }
                    else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == (int)(HMConstants.PrintTemplate.Template4))
                    {
                        entityBOList = billDA.GetKotOrderSubmitInfo(Convert.ToInt32(restaurantBill[0].KotId), restaurantBill[0].CostCenterId, "AllItem", false);

                        List<KotBillDetailBO> kotOrderSubmitEntityBOList = entityBOList.Where(x => x.ItemUnit > 0 && x.IsChanged == false).ToList();
                        List<KotBillDetailBO> changedOrEditedEntityBOList = entityBOList.Where(x => x.IsChanged == true).ToList();
                        List<KotBillDetailBO> voidOrDeletedItemEntityBOList = entityBOList.Where(x => x.ItemUnit < 0).ToList();

                        //PrintReportKot(invoicePrinter[0], restaurantBill, kotOrderSubmitEntityBOList, changedOrEditedEntityBOList, voidOrDeletedItemEntityBOList, entityBOList[0].Remarks, false);
                    }
                }
            }
        }
        public void PrintReportRestaurantBill(PrinterInfoBO files, List<RestaurantBillReportBO> restaurantBill, PrintInfos prntInf)
        {
            try
            {
                Double HeightInInch = 11;
                Double WidthInInch = 3.5;

                LocalReport report = new LocalReport();
                report.ReportPath = HttpContext.Current.Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurentBillForPos.rdlc");
                report.EnableExternalImages = true;
                report.EnableHyperlinks = true;

                DateTime dateTime = DateTime.Now;
                string kotDate = String.Format("{0:f}", dateTime);

                int kotId = Convert.ToInt32(restaurantBill[0].KotId);

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> filesCom = companyDA.GetCompanyInfo();
                List<ReportParameter> reportParam = new List<ReportParameter>();

                if (filesCom[0].CompanyId > 0)
                {
                    reportParam.Add(new ReportParameter("CompanyProfile", filesCom[0].CompanyName));
                    reportParam.Add(new ReportParameter("CompanyAddress", filesCom[0].CompanyAddress));
                    reportParam.Add(new ReportParameter("VatRegistrationNo", filesCom[0].VatRegistrationNo));

                    if (!string.IsNullOrWhiteSpace(filesCom[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", filesCom[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", filesCom[0].ContactNumber));
                }

                HMCommonDA hmCommonDA = new HMCommonDA();
                //reportParam.Add(new ReportParameter("Path", HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));
                string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(restaurantBill[0].CostCenterId);
                if (!string.IsNullOrWhiteSpace(imageName))
                {
                    reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                }
                else
                {
                    reportParam.Add(new ReportParameter("Path", "Hide"));
                }

                reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));
                reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isCompanyNameShowOnRestaurantInvoice = new HMCommonSetupBO();
                isCompanyNameShowOnRestaurantInvoice = commonSetupDA.GetCommonConfigurationInfo("IsCompanyNameShowOnRestaurantInvoice", "IsCompanyNameShowOnRestaurantInvoice");
                if (Convert.ToInt32(isCompanyNameShowOnRestaurantInvoice.SetupValue) == 1)
                {
                    reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "1"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "0"));
                }

                if (prntInf.IsRestaurantOrderSubmitDisable)
                {
                    reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "No"));
                }

                if (prntInf.IsRestaurantTokenInfoDisable)
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
                }

                string IsCostCenterNameShowOnInvoice = "1";
                reportParam.Add(new ReportParameter("IsCostCenterNameShowOnInvoice", IsCostCenterNameShowOnInvoice));

                HMCommonSetupBO isRestaurantIntegrateWithFrontOfficeBO = new HMCommonSetupBO();
                isRestaurantIntegrateWithFrontOfficeBO = commonSetupDA.GetCommonConfigurationInfo("IsGuestNameAndRoomNoTextShowInInvoice", "IsGuestNameAndRoomNoTextShowInInvoice");
                if (isRestaurantIntegrateWithFrontOfficeBO != null)
                {
                    if (isRestaurantIntegrateWithFrontOfficeBO.SetupValue == "1")
                    {
                        reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "1"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));
                    }
                }

                DateTime currentDate = DateTime.Now;
                HMCommonDA printDateDA = new HMCommonDA();
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
                report.SetBasePermissionsForSandboxAppDomain(permissions);

                report.SetParameters(reportParam);

                var dataSet = report.GetDataSourceNames();

                report.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));

                ReportDirectPrinter print = new ReportDirectPrinter();
                print.PrintDefaultPage(report, files.PrinterName, WidthInInch, HeightInInch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void PrintReportKot(PrinterInfoBO files, List<KotBillDetailBO> entityBOList, PrintInfos prntInf, Boolean IsReprint)
        {
            try
            {
                Double HeightInInch = 11;
                Double WidthInInch = 3.5;

                LocalReport report = new LocalReport();
                report.ReportPath = HttpContext.Current.Server.MapPath(@"~/POS/Reports/Rdlc/rptKotBill.rdlc");
                report.EnableExternalImages = true;
                report.EnableHyperlinks = true;

                DateTime dateTime = DateTime.Now;
                string kotDate = String.Format("{0:f}", dateTime);

                int kotId = 0;
                int paxQuantity = 1;

                foreach (KotBillDetailBO row in entityBOList)
                {
                    kotId = row.KotId;
                    if (row.PaxQuantity != 0)
                    {
                        paxQuantity = row.PaxQuantity;
                    }
                }

                string reportTitle = string.Empty;
                reportTitle = "KOT";
                if (kotId > 0)
                {
                    int updatedDataCount = 0;
                    KotBillDetailDA entityDA = new KotBillDetailDA();
                    updatedDataCount = entityDA.GetKotUpdatedDataCountInfo(kotId);
                    if (updatedDataCount > 0)
                    {
                        //reportTitle = "KOT (Updated)";
                        reportTitle = "KOT";
                    }
                    else
                    {
                        if (IsReprint)
                        {
                            reportTitle = "KOT (Reprint)";
                        }
                        else
                        {
                            reportTitle = "KOT";
                        }
                    }
                }

                ReportParameter[] parms = new ReportParameter[9];
                parms[0] = new ReportParameter("ReportTitle", reportTitle);
                parms[1] = new ReportParameter("CostCenter", prntInf.CostCenterName);
                parms[2] = new ReportParameter("SourceName", prntInf.CostCenterDefaultView);
                parms[3] = new ReportParameter("TableNo", prntInf.TableNumberInformation);
                //parms[4] = new ReportParameter("KotNo", prntInf.KotIdInformation.ToString());
                parms[4] = new ReportParameter("KotNo", prntInf.KotIdInformation.ToString() + "   Pax : " + paxQuantity.ToString());
                parms[5] = new ReportParameter("KotDate", kotDate);
                parms[6] = new ReportParameter("WaiterName", prntInf.WaiterName);
                parms[7] = new ReportParameter("SpecialRemarks", entityBOList[0].Remarks);
                parms[8] = new ReportParameter("RestaurantName", prntInf.CompanyName);

                report.SetParameters(parms);

                PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
                report.SetBasePermissionsForSandboxAppDomain(permissions);

                List<KotBillDetailBO> kotOrderSubmitEntityBOList = entityBOList.Where(x => x.ItemUnit > 0 && x.IsChanged == false).ToList();
                List<KotBillDetailBO> changedOrEditedEntityBOList = entityBOList.Where(x => x.IsChanged == true).ToList();
                List<KotBillDetailBO> voidOrDeletedItemEntityBOList = entityBOList.Where(x => x.ItemUnit < 0).ToList();

                report.DataSources.Add(new ReportDataSource("KotOrderSubmit", kotOrderSubmitEntityBOList));
                report.DataSources.Add(new ReportDataSource("ChangedOrEditedItem", changedOrEditedEntityBOList));
                report.DataSources.Add(new ReportDataSource("VoidOrDeletedItem", voidOrDeletedItemEntityBOList));

                ReportDirectPrinter print = new ReportDirectPrinter();
                print.PrintDefaultPage(report, files.PrinterName, WidthInInch, HeightInInch);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void PrintReportKot(PrinterInfoBO files, PrintInfos prntInf, List<RestaurantBillReportBO> restaurantBill, List<KotBillDetailBO> kotOrderSubmitEntityBOList, List<KotBillDetailBO> changedOrEditedEntityBOList, List<KotBillDetailBO> voidOrDeletedItemEntityBOList, string specialRemarks, Boolean IsReprint)
        {
            try
            {
                Double HeightInInch = 11;
                Double WidthInInch = 3.5;

                LocalReport report = new LocalReport();
                report.ReportPath = HttpContext.Current.Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurentBillForPosToken.rdlc");
                report.EnableExternalImages = true;
                report.EnableHyperlinks = true;

                DateTime dateTime = DateTime.Now;
                string kotDate = String.Format("{0:f}", dateTime);

                int kotId = Convert.ToInt32(restaurantBill[0].KotId);
                int paxQuantity = 1;
                //foreach (KotBillDetailBO row in entityBOList)
                //                {
                //                    paxQuantity = row.PaxQuantity;
                //                }

                string reportTitle = string.Empty;
                reportTitle = "KOT";
                if (kotId > 0)
                {
                    int updatedDataCount = 0;
                    KotBillDetailDA entityDA = new KotBillDetailDA();
                    updatedDataCount = entityDA.GetKotUpdatedDataCountInfo(kotId);
                    if (updatedDataCount > 0)
                    {
                        //reportTitle = "KOT (Updated)";
                        reportTitle = "KOT";
                    }
                    else
                    {
                        if (IsReprint)
                        {
                            reportTitle = "KOT (Reprint)";
                        }
                        else
                        {
                            reportTitle = "KOT";
                        }
                    }
                }

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> filesCom = companyDA.GetCompanyInfo();
                List<ReportParameter> reportParam = new List<ReportParameter>();

                if (filesCom[0].CompanyId > 0)
                {
                    reportParam.Add(new ReportParameter("CompanyProfile", filesCom[0].CompanyName));
                    reportParam.Add(new ReportParameter("CompanyAddress", filesCom[0].CompanyAddress));
                    reportParam.Add(new ReportParameter("VatRegistrationNo", filesCom[0].VatRegistrationNo));

                    if (!string.IsNullOrWhiteSpace(filesCom[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", filesCom[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", filesCom[0].ContactNumber));
                }

                HMCommonDA hmCommonDA = new HMCommonDA();
                //reportParam.Add(new ReportParameter("Path", HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));
                string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(restaurantBill[0].CostCenterId);
                if (!string.IsNullOrWhiteSpace(imageName))
                {
                    reportParam.Add(new ReportParameter("Path", HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                }
                else
                {
                    reportParam.Add(new ReportParameter("Path", "Hide"));
                }

                reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));

                UserInformationBO userInformationBO = new UserInformationBO();
                HMUtility hmUtility = new HMUtility();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                prntInf.WaiterName = userInformationBO.UserName.ToString();

                prntInf.CostCenterName = filesCom[0].CompanyName;
                prntInf.CostCenterDefaultView = "Token # ";
                prntInf.CompanyName = filesCom[0].CompanyName;

                reportParam.Add(new ReportParameter("ReportTitle", "KOT"));
                reportParam.Add(new ReportParameter("CostCenter", prntInf.CostCenterName));
                reportParam.Add(new ReportParameter("SourceName", prntInf.CostCenterDefaultView));
                reportParam.Add(new ReportParameter("TableNo", prntInf.TableNumberInformation));
                reportParam.Add(new ReportParameter("KotDate", kotDate));
                reportParam.Add(new ReportParameter("WaiterName", prntInf.WaiterName));
                reportParam.Add(new ReportParameter("RestaurantName", prntInf.CompanyName));
                reportParam.Add(new ReportParameter("KotNo", restaurantBill[0].KotId.ToString() + "   Pax : " + paxQuantity.ToString()));
                reportParam.Add(new ReportParameter("SpecialRemarks", specialRemarks));

                //IsRestaurantOrderSubmitDisableInfo();

                if (prntInf.IsRestaurantOrderSubmitDisable)
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
                }

                string IsCostCenterNameShowOnInvoice = "1";
                reportParam.Add(new ReportParameter("IsCostCenterNameShowOnInvoice", IsCostCenterNameShowOnInvoice));

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isRestaurantIntegrateWithFrontOfficeBO = new HMCommonSetupBO();
                isRestaurantIntegrateWithFrontOfficeBO = commonSetupDA.GetCommonConfigurationInfo("IsGuestNameAndRoomNoTextShowInInvoice", "IsGuestNameAndRoomNoTextShowInInvoice");
                if (isRestaurantIntegrateWithFrontOfficeBO != null)
                {
                    if (isRestaurantIntegrateWithFrontOfficeBO.SetupValue == "1")
                    {
                        reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "1"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));
                    }
                }

                DateTime currentDate = DateTime.Now;
                HMCommonDA printDateDA = new HMCommonDA();
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
                report.SetBasePermissionsForSandboxAppDomain(permissions);

                report.SetParameters(reportParam);

                var dataSet = report.GetDataSourceNames();

                report.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));
                report.DataSources.Add(new ReportDataSource(dataSet[1], kotOrderSubmitEntityBOList));
                report.DataSources.Add(new ReportDataSource(dataSet[2], changedOrEditedEntityBOList));
                report.DataSources.Add(new ReportDataSource(dataSet[3], voidOrDeletedItemEntityBOList));

                //HeightInInch = (entityBOList.Count + 10) * 0.314255 + 1.5 + 1; //0.64851

                ReportDirectPrinter print = new ReportDirectPrinter();
                print.PrintDefaultPage(report, files.PrinterName, WidthInInch, HeightInInch);

                //print.PrintDefaultPage(report, "");//Samsung ML-1860 Series
                //print.PrintWithCustomPage(report, 10, HeightInInch, files.PrinterName);
                //Export(report);
                //Print(files[0].PrinterName);

                //CommonHelper.AlertInfo(innboardMessage, "Print Operation Successfull", AlertType.Success);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Category Item Information
        private void LoadRestaurantItemCategory(int categoryId)
        {
            //string sourceType = Request.QueryString["st"].ToString().Trim();
            //string sourceId = Request.QueryString["sid"].ToString().Trim();
            //string costCenterId = Request.QueryString["cc"].ToString().Trim();
            //string kotId = Request.QueryString["kid"].ToString().Trim();

            //hfKotId.Value = kotId;
            //hfCostCenterId.Value = costCenterId;

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
            isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsItemNameDisplayInRestaurantOrderScreen", "IsItemNameDisplayInRestaurantOrderScreen");

            string costCenterId = hfCostCenterId.Value;

            InvCategoryDA roomNumberDA = new InvCategoryDA();
            List<InvCategoryBO> roomNumberListAllBO = new List<InvCategoryBO>();
            List<InvCategoryBO> roomNumberListWithZeroBO = new List<InvCategoryBO>();
            List<InvCategoryBO> roomNumberListWithoutZeroBO = new List<InvCategoryBO>();

            string fullContent = string.Empty;
            roomNumberListAllBO = roomNumberDA.GetInvCategoryInfoByLabel(Convert.ToInt32(costCenterId), categoryId);
            roomNumberListWithZeroBO = roomNumberListAllBO.Where(x => x.ChildCount == 0).ToList();
            roomNumberListWithoutZeroBO = roomNumberListAllBO.Where(x => x.ChildCount != 0).ToList();

            //------------------------------------------------Item Generate-----------------------------------------------------------------------------
            string fullItemContent = string.Empty;
            for (int iItemCategory = 0; iItemCategory < roomNumberListWithZeroBO.Count; iItemCategory++)
            {
                int itemCategory = roomNumberListWithZeroBO[iItemCategory].CategoryId;
                InvItemDA invItemDA = new InvItemDA();
                List<InvItemBO> roomNumberListBO = new List<InvItemBO>();

                string subItemContent = string.Empty;
                Session["IRtxtCategoryInformation"] = itemCategory;
                roomNumberListBO = invItemDA.GetInvItemInfoByCategoryId(Convert.ToInt32(costCenterId), itemCategory);

                for (int iItem = 0; iItem < roomNumberListBO.Count; iItem++)
                {
                    subItemContent += @"<div class='DivRestaurantItemContainer'>";


                    if (isRestaurantBillAmountWillRoundBO.SetupValue.ToString() == "0")
                    {
                        subItemContent += @"<div class='RestaurantItemDiv'><img class='ItemImageSize' ID='ContentPlaceHolder1_img" + roomNumberListBO[iItem].ItemId;
                        subItemContent += @"'  onclick='return PerformAction(" + roomNumberListBO[iItem].ItemId;
                        subItemContent += @");'  src='" + roomNumberListBO[iItem].ImageName;
                        subItemContent += @"' /></div>
                                        <div class='ItemNameDiv'>" + roomNumberListBO[iItem].Name + "</div>";
                    }
                    else if (isRestaurantBillAmountWillRoundBO.SetupValue.ToString() == "1")
                    {
                        subItemContent += @"<div class='RestaurantItemDiv' style='padding: 2px;'";
                        subItemContent += " onclick='return PerformAction(" + roomNumberListBO[iItem].ItemId + ")'>"
                                            + roomNumberListBO[iItem].DisplayName + "</div>";
                    }

                    subItemContent += "</div>";
                }

                fullItemContent += subItemContent;

            }

            //------------------------------------------------Category Generate-----------------------------------------------------------------------------
            //string topPart = @"<a href='javascript:void(0)' class='block-heading'><span style='color: #ffffff;'>Category Information";
            string topTemplatePart = @"</span></a> <div> <div>";
            string endTemplatePart = @"</div> </div>";

            string subContent = string.Empty;

            for (int iItemCategory = 0; iItemCategory < roomNumberListWithoutZeroBO.Count; iItemCategory++)
            {
                if (roomNumberListWithoutZeroBO[iItemCategory].ChildCount.ToString() == "1")
                {
                    subContent += string.Format(@"<div class='DivRestaurantItemContainer'> <a href='javascript:void(0)'" +
                                              " onclick =\"LoadCategoryNItem('RestaurantItem:{0}', '{1}')\" ", roomNumberListWithoutZeroBO[iItemCategory].CategoryId, roomNumberListWithoutZeroBO[iItemCategory].Hierarchy);
                }
                else
                {
                    subContent += string.Format(@"<div class='DivRestaurantItemContainer'> <a href='javascript:void(0)'" +
                                             " onclick =\"LoadCategoryNItem('RestaurantItemCategory:{0}', '{1}')\" ", roomNumberListWithoutZeroBO[iItemCategory].CategoryId, roomNumberListWithoutZeroBO[iItemCategory].Hierarchy);
                }

                if (isRestaurantBillAmountWillRoundBO.SetupValue.ToString() == "0")
                {
                    subContent += @"'><div class='RestaurantItemDiv'> " +
                                      "<img ID='ContentPlaceHolder1_img" + roomNumberListWithoutZeroBO[iItemCategory].CategoryId;
                    subContent += @"' class='RestaurantItemImage' src='" + roomNumberListWithoutZeroBO[iItemCategory].ImageName
                                     + @"' /> ";
                    subContent += "</div></a> <div class='ItemNameDiv'>" + roomNumberListWithoutZeroBO[iItemCategory].Name + "</div>";
                }
                else if (isRestaurantBillAmountWillRoundBO.SetupValue.ToString() == "1")
                {
                    subContent += @"'><div class='RestaurantItemDiv' style='padding: 2px;'>" + roomNumberListWithoutZeroBO[iItemCategory].Name + "</div></a>";
                }

                subContent += "</div>";
            }

            fullContent += topTemplatePart + fullItemContent + subContent + endTemplatePart;
            literalRestaurantTemplate.Text = fullContent;

        }
        private void LoadRestaurantItem(string queryString)
        {
            //string sourceType = Request.QueryString["st"].ToString().Trim();
            //string sourceId = Request.QueryString["sid"].ToString().Trim();
            //string costCenterId = Request.QueryString["cc"].ToString().Trim();
            //string kotId = Request.QueryString["kid"].ToString().Trim();

            //hfKotId.Value = kotId;
            //hfCostCenterId.Value = costCenterId;

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
            isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsItemNameDisplayInRestaurantOrderScreen", "IsItemNameDisplayInRestaurantOrderScreen");

            string costCenterId = hfCostCenterId.Value;

            int itemCategory = Convert.ToInt32(queryString);
            InvItemDA roomNumberDA = new InvItemDA();
            List<InvItemBO> roomNumberListBO = new List<InvItemBO>();

            string fullContent = string.Empty;

            roomNumberListBO = roomNumberDA.GetInvItemInfoByCategoryId(Convert.ToInt32(costCenterId), itemCategory);

            //string topPart = @"<a href='javascript:void(0)' class='block-heading'><span style='color: #ffffff;'>Item Information";
            string topTemplatePart = @"</span></a> <div> <div>";
            string endTemplatePart = @"</div> </div>";

            string subContent = string.Empty;

            for (int iItem = 0; iItem < roomNumberListBO.Count; iItem++)
            {
                subContent += @"<div class='DivRestaurantItemContainer'>";

                if (isRestaurantBillAmountWillRoundBO.SetupValue.ToString() == "0")
                {
                    subContent += @"<div class='RestaurantItemDiv'> ";
                    subContent += "<img class='ItemImageSize' ID='ContentPlaceHolder1_img" + roomNumberListBO[iItem].ItemId;
                    subContent += @"'  onclick='return PerformAction(" + roomNumberListBO[iItem].ItemId;
                    subContent += @");'  src='" + roomNumberListBO[iItem].ImageName;
                    subContent += @"' />";
                    subContent += "</div>  <div class='ItemNameDiv'>" + roomNumberListBO[iItem].Name + "</div>";
                }
                else if (isRestaurantBillAmountWillRoundBO.SetupValue.ToString() == "1")
                {
                    subContent += @"<div class='RestaurantItemDiv' style='padding: 2px;'";
                    subContent += " onclick='return PerformAction(" + roomNumberListBO[iItem].ItemId + ")'>"
                                        + roomNumberListBO[iItem].DisplayName + "</div>";

                    //subContent += @"<div class='RestaurantItemDiv'" + roomNumberListBO[iItem].Name;
                    //subContent += " onclick='return PerformAction(" + roomNumberListBO[iItem].ItemId; " +  + "</div>";
                }

                subContent += "</div>";
            }

            fullContent += topTemplatePart + subContent + endTemplatePart;
            literalRestaurantTemplate.Text = fullContent;

        }
        #endregion
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ReturnInfo SaveIndividualItemDetailInformation(int isItemCanEditDelete, int costCenterId, int kotId, int itemId, decimal itemQty, string sourceName)
        {
            ReturnInfo rtninf = new ReturnInfo();
            RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
            RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();

            paymentResume.RestaurantKotBill = restaurentBillDA.GetRestaurantBillByKotId(kotId, sourceName);

            rtninf.IsBillResettled = false;

            if (paymentResume.RestaurantKotBill != null)
            {
                if (paymentResume.RestaurantKotBill.IsBillSettlement)
                {
                    rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    rtninf.IsBillResettled = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo("Bill Already Settled. Redirect Within a Short Time.", AlertType.Error);
                    return rtninf;
                }
            }

            InvItemBO itemEntityBO = new InvItemBO();
            InvItemDA itemEntityDA = new InvItemDA();
            KotBillDetailBO entityBO = new KotBillDetailBO();
            KotBillDetailDA entityDA = new KotBillDetailDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            itemEntityBO = itemEntityDA.GetInvItemInfoById(costCenterId, itemId);
            if (itemEntityBO.ItemId > 0)
            {
                entityBO = entityDA.GetKotBillDetailInfoByKotNItemId(costCenterId, kotId, itemId, "IndividualItem");

                if (entityBO.KotDetailId > 0)
                {
                    entityBO.ItemType = "IndividualItem";
                    entityBO.ItemId = itemId;
                    entityBO.ItemUnit = itemQty;
                    entityBO.KotId = kotId;
                    entityBO.UnitRate = itemEntityBO.UnitPriceLocal;
                    entityBO.Amount = itemEntityBO.UnitPriceLocal * entityBO.ItemUnit;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean status = entityDA.UpdateKotBillDetailInfo(isItemCanEditDelete, entityBO, "QuantityAdd");
                }
                else
                {
                    entityBO.ItemType = "IndividualItem";
                    entityBO.ItemId = itemId;
                    entityBO.ItemUnit = itemQty;
                    entityBO.KotId = kotId;
                    entityBO.UnitRate = itemEntityBO.UnitPriceLocal;
                    entityBO.Amount = itemEntityBO.UnitPriceLocal * entityBO.ItemUnit;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean status = entityDA.SaveKotBillDetailInfo(entityBO);
                }
            }

            return rtninf;
        }
        [WebMethod]
        public static GuestBillPaymentBO SavePaymentInformationInSession(string DiscountType, string DiscountAmount, string txtCash, string txtAmexCard, string txtMasterCard, string txtVisaCard, string txtDiscoverCard)
        {
            GuestBillPaymentBO entityBO = new GuestBillPaymentBO();

            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["IRGuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

            HttpContext.Current.Session["IRRestaurantBillDiscountType"] = DiscountType;
            HttpContext.Current.Session["IRRestaurantBillDiscountAmount"] = DiscountAmount;

            // // // ------ Cash Payment Information -------------------------
            if (!string.IsNullOrWhiteSpace(txtCash))
            {
                decimal cashPaymentAmount = !string.IsNullOrWhiteSpace(txtCash) ? Convert.ToDecimal(txtCash) : 0;

                if (cashPaymentAmount > 0)
                {
                    GuestBillPaymentBO guestCashPaymentInfo = new GuestBillPaymentBO();
                    guestCashPaymentInfo.NodeId = 0; //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                    guestCashPaymentInfo.PaymentType = "Advance";
                    guestCashPaymentInfo.AccountsPostingHeadId = 0; //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                    guestCashPaymentInfo.BillPaidBy = 0;
                    guestCashPaymentInfo.BankId = 0;
                    guestCashPaymentInfo.RegistrationId = 0;
                    guestCashPaymentInfo.FieldId = 1;
                    guestCashPaymentInfo.ConvertionRate = 1;
                    guestCashPaymentInfo.CurrencyAmount = cashPaymentAmount;
                    guestCashPaymentInfo.PaymentAmount = cashPaymentAmount;
                    guestCashPaymentInfo.ChecqueDate = DateTime.Now;
                    guestCashPaymentInfo.PaymentMode = "Cash";
                    guestCashPaymentInfo.PaymentId = 1;
                    guestCashPaymentInfo.CardNumber = "";
                    guestCashPaymentInfo.CardType = "";
                    guestCashPaymentInfo.ExpireDate = null;
                    guestCashPaymentInfo.ChecqueNumber = "";
                    guestCashPaymentInfo.CardHolderName = "";
                    guestCashPaymentInfo.PaymentDescription = "";

                    guestPaymentDetailListForGrid.Add(guestCashPaymentInfo);
                }
            }

            // // // ------ Amex Card Payment Information -------------------------
            if (!string.IsNullOrWhiteSpace(txtAmexCard))
            {
                decimal amexCardPaymentAmount = !string.IsNullOrWhiteSpace(txtAmexCard) ? Convert.ToDecimal(txtAmexCard) : 0;

                if (amexCardPaymentAmount > 0)
                {
                    GuestBillPaymentBO guestAmexCardPaymentInfo = new GuestBillPaymentBO();
                    guestAmexCardPaymentInfo.NodeId = 0; //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                    guestAmexCardPaymentInfo.PaymentType = "Advance";
                    guestAmexCardPaymentInfo.AccountsPostingHeadId = 0; //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                    guestAmexCardPaymentInfo.BillPaidBy = 0;
                    guestAmexCardPaymentInfo.BankId = 0;
                    guestAmexCardPaymentInfo.RegistrationId = 0;
                    guestAmexCardPaymentInfo.FieldId = 1;
                    guestAmexCardPaymentInfo.ConvertionRate = 1;
                    guestAmexCardPaymentInfo.CurrencyAmount = amexCardPaymentAmount;
                    guestAmexCardPaymentInfo.PaymentAmount = amexCardPaymentAmount;
                    guestAmexCardPaymentInfo.ChecqueDate = DateTime.Now;
                    guestAmexCardPaymentInfo.PaymentMode = "Card";
                    guestAmexCardPaymentInfo.PaymentId = 1;
                    guestAmexCardPaymentInfo.CardNumber = "";
                    guestAmexCardPaymentInfo.CardType = "a";
                    guestAmexCardPaymentInfo.ExpireDate = null;
                    guestAmexCardPaymentInfo.ChecqueNumber = "";
                    guestAmexCardPaymentInfo.CardHolderName = "";
                    guestAmexCardPaymentInfo.PaymentDescription = "American Express";

                    guestPaymentDetailListForGrid.Add(guestAmexCardPaymentInfo);
                }
            }


            // // // ------ Master Card Payment Information -------------------------
            if (!string.IsNullOrWhiteSpace(txtMasterCard))
            {
                decimal masterCardPaymentAmount = !string.IsNullOrWhiteSpace(txtMasterCard) ? Convert.ToDecimal(txtMasterCard) : 0;

                if (masterCardPaymentAmount > 0)
                {
                    GuestBillPaymentBO guestMasterCardPaymentInfo = new GuestBillPaymentBO();
                    guestMasterCardPaymentInfo.NodeId = 0; //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                    guestMasterCardPaymentInfo.PaymentType = "Advance";
                    guestMasterCardPaymentInfo.AccountsPostingHeadId = 0; //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                    guestMasterCardPaymentInfo.BillPaidBy = 0;
                    guestMasterCardPaymentInfo.BankId = 0;
                    guestMasterCardPaymentInfo.RegistrationId = 0;
                    guestMasterCardPaymentInfo.FieldId = 1;
                    guestMasterCardPaymentInfo.ConvertionRate = 1;
                    guestMasterCardPaymentInfo.CurrencyAmount = masterCardPaymentAmount;
                    guestMasterCardPaymentInfo.PaymentAmount = masterCardPaymentAmount;
                    guestMasterCardPaymentInfo.ChecqueDate = DateTime.Now;
                    guestMasterCardPaymentInfo.PaymentMode = "Card";
                    guestMasterCardPaymentInfo.PaymentId = 1;
                    guestMasterCardPaymentInfo.CardNumber = "";
                    guestMasterCardPaymentInfo.CardType = "m";
                    guestMasterCardPaymentInfo.ExpireDate = null;
                    guestMasterCardPaymentInfo.ChecqueNumber = "";
                    guestMasterCardPaymentInfo.CardHolderName = "";
                    guestMasterCardPaymentInfo.PaymentDescription = "Master Card";

                    guestPaymentDetailListForGrid.Add(guestMasterCardPaymentInfo);
                }
            }


            // // // ------ Visa Card Payment Information -------------------------
            if (!string.IsNullOrWhiteSpace(txtVisaCard))
            {
                decimal visaCardPaymentAmount = !string.IsNullOrWhiteSpace(txtVisaCard) ? Convert.ToDecimal(txtVisaCard) : 0;

                if (visaCardPaymentAmount > 0)
                {
                    GuestBillPaymentBO guestVisaCardPaymentInfo = new GuestBillPaymentBO();
                    guestVisaCardPaymentInfo.NodeId = 0; //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                    guestVisaCardPaymentInfo.PaymentType = "Advance";
                    guestVisaCardPaymentInfo.AccountsPostingHeadId = 0; //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                    guestVisaCardPaymentInfo.BillPaidBy = 0;
                    guestVisaCardPaymentInfo.BankId = 0;
                    guestVisaCardPaymentInfo.RegistrationId = 0;
                    guestVisaCardPaymentInfo.FieldId = 1;
                    guestVisaCardPaymentInfo.ConvertionRate = 1;
                    guestVisaCardPaymentInfo.CurrencyAmount = visaCardPaymentAmount;
                    guestVisaCardPaymentInfo.PaymentAmount = visaCardPaymentAmount;
                    guestVisaCardPaymentInfo.ChecqueDate = DateTime.Now;
                    guestVisaCardPaymentInfo.PaymentMode = "Card";
                    guestVisaCardPaymentInfo.PaymentId = 1;
                    guestVisaCardPaymentInfo.CardNumber = "";
                    guestVisaCardPaymentInfo.CardType = "v";
                    guestVisaCardPaymentInfo.ExpireDate = null;
                    guestVisaCardPaymentInfo.ChecqueNumber = "";
                    guestVisaCardPaymentInfo.CardHolderName = "";
                    guestVisaCardPaymentInfo.PaymentDescription = "Visa Card";

                    guestPaymentDetailListForGrid.Add(guestVisaCardPaymentInfo);
                }
            }


            // // // ------ Discover Card Payment Information -------------------------
            if (!string.IsNullOrWhiteSpace(txtDiscoverCard))
            {
                decimal discoverCardPaymentAmount = !string.IsNullOrWhiteSpace(txtDiscoverCard) ? Convert.ToDecimal(txtDiscoverCard) : 0;

                if (discoverCardPaymentAmount > 0)
                {
                    GuestBillPaymentBO guestDiscoverCardPaymentInfo = new GuestBillPaymentBO();
                    guestDiscoverCardPaymentInfo.NodeId = 0; //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                    guestDiscoverCardPaymentInfo.PaymentType = "Advance";
                    guestDiscoverCardPaymentInfo.AccountsPostingHeadId = 0; //Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                    guestDiscoverCardPaymentInfo.BillPaidBy = 0;
                    guestDiscoverCardPaymentInfo.BankId = 0;
                    guestDiscoverCardPaymentInfo.RegistrationId = 0;
                    guestDiscoverCardPaymentInfo.FieldId = 1;
                    guestDiscoverCardPaymentInfo.ConvertionRate = 1;
                    guestDiscoverCardPaymentInfo.CurrencyAmount = discoverCardPaymentAmount;
                    guestDiscoverCardPaymentInfo.PaymentAmount = discoverCardPaymentAmount;
                    guestDiscoverCardPaymentInfo.ChecqueDate = DateTime.Now;
                    guestDiscoverCardPaymentInfo.PaymentMode = "Card";
                    guestDiscoverCardPaymentInfo.PaymentId = 1;
                    guestDiscoverCardPaymentInfo.CardNumber = "";
                    guestDiscoverCardPaymentInfo.CardType = "d";
                    guestDiscoverCardPaymentInfo.ExpireDate = null;
                    guestDiscoverCardPaymentInfo.ChecqueNumber = "";
                    guestDiscoverCardPaymentInfo.CardHolderName = "";
                    guestDiscoverCardPaymentInfo.PaymentDescription = "Discover Card";

                    guestPaymentDetailListForGrid.Add(guestDiscoverCardPaymentInfo);
                }
            }

            HttpContext.Current.Session["IRGuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;

            return entityBO;
        }
        [WebMethod]
        public static ArrayList GenerateTableWiseItemGridInformation(string costCenterId, string kotIdList)
        {
            string strTable = string.Empty;
            bool isOrderSubmit = false, isBillPreviewButtonEnable = false;

            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(Convert.ToInt32(costCenterId));

            string[] kotList = kotIdList.Split(',');
            int ownKot = Convert.ToInt32(kotList[kotList.Count() - 1]);
            string actionDisabled = "disabled='disabled'";

            if (costCentreTabBO != null)
            {
                if (costCentreTabBO.CostCenterId > 0)
                {
                    string costCenterDefaultView = string.Empty;

                    if (costCentreTabBO.DefaultView == "Token")
                    {
                        costCenterDefaultView = "RestaurantToken";
                    }
                    else if (costCentreTabBO.DefaultView == "Table")
                    {
                        costCenterDefaultView = "RestaurantTable";
                    }
                    else if (costCentreTabBO.DefaultView == "Room")
                    {
                        costCenterDefaultView = "GuestRoom";
                    }

                    KotBillDetailDA entityDA = new KotBillDetailDA();
                    List<KotBillDetailBO> files = entityDA.GetRestaurantOrderItemByMultipleKotId(costCenterId, kotIdList, costCenterDefaultView).Where(x => x.ItemUnit > 0).ToList();

                    Boolean isChangedExist = false;

                    if (files.Count > 0)
                    {
                        isBillPreviewButtonEnable = files[0].IsBillPreviewButtonEnable;
                    }

                    foreach (KotBillDetailBO drIsChanged in files)
                    {
                        if (drIsChanged.IsChanged)
                        {
                            isChangedExist = true;
                            break;
                        }
                    }

                    strTable += "<table id='TableWiseItemInformation' class='table table-bordered table-condensed table-responsive table-bordered cf'> <thead class='cf'> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                    strTable += "<th align='left' scope='col'>Item Name</th><th align='left' scope='col'>Unit</th><th align='left' scope='col'>U. Rate</th><th align='left' scope='col'>Total</th><th align='center' scope='col'>Action</th></tr></thead>";
                    strTable += "<tbody>";
                    int counter = 0;
                    foreach (KotBillDetailBO dr in files)
                    {
                        if (counter % 2 == 0)
                        {
                            // It's even
                            strTable += "<tr style='background-color:#E3EAEB;'> <td data-title='Item Name' align='left' style='width: 40%;'>" + dr.ItemName + "</td>";
                        }
                        else
                        {
                            // It's odd
                            strTable += "<tr style='background-color:White;'><td data-title='Item Name' align='left' style='width: 40%;'>" + dr.ItemName + "</td>";
                        }

                        strTable += "<td data-title='Unit' align='left' style='width: 15%;'>" + dr.ItemUnit + "</td>";
                        strTable += "<td data-title='Unit Rate' align='left' style='width: 15%;'>" + dr.UnitRate + "</td>";
                        strTable += "<td data-title='Total' align='left' style='width: 15%;'>" + Math.Round((dr.ItemUnit * dr.UnitRate), 2) + "</td>";

                        //strTable += "<td align='center' style='width: 15%;'>";
                        if (dr.KotDetailId > 0)
                        {
                            if (!dr.PrintFlag)
                            {
                                strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-primary btn-small' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 0 + "," + (dr.IsItemEditable == true ? 1 : 0) + "," + counter + ")' alt='Action Decider' " + (ownKot != dr.KotId ? actionDisabled : string.Empty) + ">Option</button></td>";
                            }
                            else
                            {
                                if (!isChangedExist)
                                {
                                    strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-success  btn-small' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 1 + "," + (dr.IsItemEditable == true ? 1 : 0) + "," + counter + ")' alt='Action Decider' " + (ownKot != dr.KotId ? actionDisabled : string.Empty) + ">Option</button></td>";
                                }
                                else
                                {
                                    strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-primary  btn-small' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 1 + "," + (dr.IsItemEditable == true ? 1 : 0) + "," + counter + ")' alt='Action Decider' " + (ownKot != dr.KotId ? actionDisabled : string.Empty) + ">Option</button></td>";
                                }

                                isOrderSubmit = true;
                            }

                            //strTable += "<td align='center' style='width: 15%;'><img src='../Images/actiondecider.png' onClick='javascript:return AddNewItem(" + dr.KotDetailId + ")' alt='Action Decider' border='0' /></td>";
                            //strTable += "<td align='center' style='width: 15%;'><img src='../Images/edit.png' onClick='javascript:return AddNewItem(" + dr.KotDetailId + ")' alt='Edit Information' border='0' /></td>";
                            //strTable += "<td align='center' style='width: 15%;'><img src='../Images/delete.png' onClick='javascript:return PerformDeleteAction(" + dr.KotDetailId + ")' alt='Delete Information' border='0' /></td>";
                        }

                        strTable += "<td align='left' style='display:none;'>" + dr.ItemId + "</td>";

                        strTable += "<td align='left' style='display:none;'>" + dr.ClassificationId + "</td>";

                        strTable += "<td align='left' style='display:none;'>" +
                           (dr.ItemWiseDiscountType == "Fixed" ? "Percentage" : dr.ItemWiseDiscountType)
                                + "</td>";
                        strTable += "<td align='left' style='display:none;'>" + dr.ItemWiseIndividualDiscount.ToString() + "</td>";

                        strTable += "<td align='left' style='display:none;'>" + dr.ServiceCharge.ToString() + "</td>";
                        strTable += "<td align='left' style='display:none;'>" + dr.CitySDCharge.ToString() + "</td>";
                        strTable += "<td align='left' style='display:none;'>" + dr.VatAmount.ToString() + "</td>";
                        strTable += "<td align='left' style='display:none;'>" + dr.AdditionalCharge.ToString() + "</td>";

                        strTable += "</tr>";

                        if (dr.ItemType == "BuffetItem")
                        {
                            string strBuffetDetail = string.Empty;
                            List<RestaurantBuffetDetailBO> buffetDetailListBO = new List<RestaurantBuffetDetailBO>();
                            RestaurantBuffetDetailDA buffetDetailDA = new RestaurantBuffetDetailDA();

                            //buffetDetailListBO = buffetDetailDA.GetRestaurantBuffetDetailByBuffetId(dr.ItemId);
                            //foreach (RestaurantBuffetDetailBO drDetail in buffetDetailListBO)
                            //{
                            //    //int tmpItemUnit = Convert.ToInt32(drDetail.ItemUnit);
                            //    strBuffetDetail += ", " + drDetail.ItemName;
                            //}
                            //strBuffetDetail = strBuffetDetail.Substring(2, strBuffetDetail.Length - 2);
                            strTable += "<tr><td align='left' style='width: 40%;'>" + strBuffetDetail + "</td>";
                        }

                        if (dr.ItemType == "ComboItem")
                        {
                            string strComboDetail = string.Empty;
                            List<InvItemDetailsBO> ownerDetailListBO = new List<InvItemDetailsBO>();
                            InvItemDetailsDA ownerDetailDA = new InvItemDetailsDA();

                            ownerDetailListBO = ownerDetailDA.GetInvItemDetailsByItemId(dr.ItemId);
                            foreach (InvItemDetailsBO drDetail in ownerDetailListBO)
                            {
                                int tmpItemUnit = Convert.ToInt32(drDetail.ItemUnit);
                                strComboDetail += ", " + drDetail.ItemName + "(" + tmpItemUnit + ")";
                            }
                            strComboDetail = strComboDetail.Substring(2, strComboDetail.Length - 2);
                            strTable += "<tr><td align='left' style='width: 40%;>" + strComboDetail + "</td>";
                        }
                        counter++;
                    }
                    strTable += "</tbody> </table>";
                    if (strTable == "")
                    {
                        strTable = "<tr><td data-title='Item Name' colspan='4' align='center'>No Record Available!</td></tr>";
                    }
                }
            }

            ArrayList arr = new ArrayList();
            arr.Add(strTable);
            arr.Add(isOrderSubmit);
            arr.Add(isBillPreviewButtonEnable);

            return arr;
        }
        [WebMethod]
        public static int GetRecipeType(int itemId)
        {
            InvItemDA it = new InvItemDA();
            int isRecipe = it.GetRecipeType(itemId);
            return isRecipe;
        }

        [WebMethod]
        public static bool SaveWithPriceUpdate(int itemId, int kotId)
        {
            InvItemDA it = new InvItemDA();
            bool status = it.UpdateDynamicKotRecipeCost(itemId, kotId);

            return status;
        }


        [WebMethod]
        public static SpecialRemarksDetailsViewBO GetSpecialRemarksDetails(int kotId, int itemId)
        {
            InvItemSpecialRemarksDA specialRemarkDa = new InvItemSpecialRemarksDA();
            List<InvItemSpecialRemarksBO> specialRemarks = new List<InvItemSpecialRemarksBO>();

            RestaurantKotSpecialRemarksDetailDA kotRemarksDa = new RestaurantKotSpecialRemarksDetailDA();
            List<RestaurantKotSpecialRemarksDetailBO> kotRemarks = new List<RestaurantKotSpecialRemarksDetailBO>();

            SpecialRemarksDetailsViewBO remarksDetailsView = new SpecialRemarksDetailsViewBO();

            specialRemarks = specialRemarkDa.GetActiveInvItemSpecialRemarksInfo();
            kotRemarks = kotRemarksDa.GetInvItemSpecialRemarksInfoById(kotId, itemId);

            remarksDetailsView.KotRemarks = kotRemarks;
            remarksDetailsView.ItemSpecialRemarks = specialRemarks;

            return remarksDetailsView;
        }

        [WebMethod]
        public static RestaurantRecipeDetailsViewForDynamicChangeBO GetItemRecipeList(int itemId, int kotId)
        {
            RestaurantRecipeDetailsViewForDynamicChangeBO info = new RestaurantRecipeDetailsViewForDynamicChangeBO();

            List<RestaurantRecipeDetailBO> itemInfo = new List<RestaurantRecipeDetailBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetExistingRecipeForRecipe(itemId);
            info.PreviousRecipe = itemInfo;

            List<RestaurantRecipeDetailBO> NewItemsInfo = new List<RestaurantRecipeDetailBO>();
            NewItemsInfo = itemDa.GetNewItemsForRecipe(itemId);
            info.NewItems = NewItemsInfo;

            List<RestaurantRecipeDetailBO> NewItemsForRecipe = new List<RestaurantRecipeDetailBO>();
            NewItemsForRecipe = itemDa.GetNewModifiedRecipeItems(itemId, kotId);
            info.NewKotRecipe = NewItemsForRecipe;

            for (int i = 0; i < info.NewKotRecipe.Count; i++)
            {
                for (int j = 0; j < info.PreviousRecipe.Count; j++)
                {
                    if (info.NewKotRecipe[i].RecipeItemId == info.PreviousRecipe[j].ItemId)
                    {
                        if (info.NewKotRecipe[i].Status == "Deleted")
                        {
                            info.PreviousRecipe.RemoveAt(j);
                            j--;
                        }
                        else
                        {
                            info.PreviousRecipe[j].RecipeId = info.NewKotRecipe[i].RecipeId;
                            info.PreviousRecipe[j].PreviousTypeId = info.NewKotRecipe[i].TypeId;

                            for (int k = 0; k < info.PreviousRecipe[j].RecipeModifierTypes.Count; k++)
                            {
                                if (info.PreviousRecipe[j].RecipeModifierTypes[k].TypeId == info.PreviousRecipe[j].PreviousTypeId)
                                {
                                    info.PreviousRecipe[j].PreviousTotalCost = info.PreviousRecipe[j].RecipeModifierTypes[k].TotalCost;
                                    info.PreviousRecipe[j].ItemCost = info.PreviousRecipe[j].PreviousTotalCost;
                                    break;
                                }

                            }
                        }

                        info.NewKotRecipe.RemoveAt(i);
                        i--;
                        break;

                    }
                }
            }

            return info;
        }

        [WebMethod]
        public static List<RestaurantRecipeDetailBO> GetPreviousRecipeModifierTypes(int IngredientId, int itemid)
        {
            //int itemid = 26;
            List<RestaurantRecipeDetailBO> itemInfo = new List<RestaurantRecipeDetailBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetPreviousRecipeModifierTypes(IngredientId, itemid);

            return itemInfo;
        }
        [WebMethod]
        public static ReturnInfo SaveNewRecipe(List<RestaurantRecipeDetailBO> newRecipeeItems, List<int> deletedItemIdList, int kotId)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();

            try
            {
                InvItemDA itemDa = new InvItemDA();
                status = itemDa.SaveNewRecipe(newRecipeeItems, deletedItemIdList, kotId);

                if (status)
                {
                    returnInfo.IsSuccess = true;
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }

                else
                {
                    returnInfo.IsSuccess = false;
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }

            }
            catch (Exception ex)
            {
                returnInfo.IsSuccess = false;
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                throw ex;
            }

            return returnInfo;
        }

        [WebMethod]
        public static ReturnInfo SaveKotSpecialRemarks(List<RestaurantKotSpecialRemarksDetailBO> kotSRemarksDetail, List<RestaurantKotSpecialRemarksDetailBO> kotSRemarksDetailTobeDelete, int kotDetailId)
        {
            int tempItemId = 0;
            bool status = false;
            ReturnInfo rtninf = new ReturnInfo();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int waiterId = userInformationBO.UserInfoId;

            RestaurantKotSpecialRemarksDetailDA kotRemarksDa = new RestaurantKotSpecialRemarksDetailDA();

            if (kotSRemarksDetailTobeDelete.Count == 0)
            {
                status = kotRemarksDa.SaveRestaurantKotSpecialRemarksDetail(kotSRemarksDetail, waiterId, kotDetailId, out tempItemId);
            }
            else
            {
                status = kotRemarksDa.UpdateRestaurantKotSpecialRemarksDetail(kotSRemarksDetail, kotSRemarksDetailTobeDelete, waiterId, kotDetailId);
            }

            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

        [WebMethod]
        public static ReturnInfo SaveKotItemWiseRemarks(RestaurantKotSpecialRemarksDetailBO itemWiseRemark, int kotDetailId)
        {
            int tempItemId = 0;
            bool status = false;
            ReturnInfo rtninf = new ReturnInfo();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int waiterId = userInformationBO.UserInfoId;

            RestaurantKotSpecialRemarksDetailDA kotRemarksDa = new RestaurantKotSpecialRemarksDetailDA();
            status = kotRemarksDa.SaveRestaurantKotItemWiseRemarksDetail(itemWiseRemark, waiterId, kotDetailId, out tempItemId);

            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.Pk = tempItemId;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }


        [WebMethod]
        public static Boolean DeleteRecipeDetailsAndUpdateDefaultPrice(int kotId, int itemid)
        {
            RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
            bool status = restaurentBillDA.DeleteRecipeDetailsAndUpdateDefaultPrice(kotId, itemid);
            return status;
        }

        [WebMethod]
        public static ReturnInfo UpdateIndividualItemDetailInformation(string userId, string userIdAuthorisedPassword, int kotId, string sourceName, int isItemCanEditDelete, string updateType, int costCenterId, int editId, decimal quantity, string updatedContent)
        {
            ReturnInfo rtninf = new ReturnInfo();
            RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
            RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();

            paymentResume.RestaurantKotBill = restaurentBillDA.GetRestaurantBillByKotId(kotId, sourceName);

            rtninf.IsBillResettled = false;

            if (paymentResume.RestaurantKotBill != null)
            {
                if (paymentResume.RestaurantKotBill.IsBillSettlement)
                {
                    rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    rtninf.IsBillResettled = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo("Bill Already Settled. Redirect Within a Short Time.", AlertType.Error);
                    return rtninf;
                }
            }

            KotBillDetailBO entityBO = new KotBillDetailBO();
            KotBillDetailDA entityDA = new KotBillDetailDA();
            KotBillDetailBO srcEntityBO = new KotBillDetailBO();

            RestaurantBuffetItemBO bItemEntityBO = new RestaurantBuffetItemBO();
            RestaurantBuffetItemDA bItemEntityDA = new RestaurantBuffetItemDA();
            RestaurantComboItemBO cItemEntityBO = new RestaurantComboItemBO();
            RestaurantComboItemDA cItemEntityDA = new RestaurantComboItemDA();
            InvItemBO itemEntityBO = new InvItemBO();
            InvItemDA itemEntityDA = new InvItemDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int userIdAuthorised = userInformationBO.UserInfoId;
            if (!string.IsNullOrEmpty(userIdAuthorisedPassword) && !string.IsNullOrEmpty(userId))
            {
                UserInformationDA userInformationDA = new UserInformationDA();
                UserInformationBO userInformation = userInformationDA.GetUserInformationByUserNameNId(userId, userIdAuthorisedPassword);
                if (userInformation.UserInfoId != 0)
                {
                    userIdAuthorised = userInformation.UserInfoId;
                }
            }

            srcEntityBO = entityDA.GetSrcRestaurantBillDetailInfoByKotDetailId(editId);
            if (srcEntityBO.KotDetailId > 0)
            {
                if (srcEntityBO.ItemType == "BuffetItem")
                {
                    bItemEntityBO = bItemEntityDA.GetRestaurantBuffetInfoById(srcEntityBO.ItemId);
                    entityBO.UnitRate = bItemEntityBO.BuffetPrice;
                    entityBO.Amount = bItemEntityBO.BuffetPrice * Convert.ToDecimal(updatedContent);
                    entityBO.ItemUnit = Convert.ToInt32(updatedContent);
                    entityBO.KotId = srcEntityBO.KotId;
                    entityBO.ItemId = srcEntityBO.ItemId;
                    entityBO.CreatedBy = userIdAuthorised;
                }
                else if (srcEntityBO.ItemType == "ComboItem")
                {
                    cItemEntityBO = cItemEntityDA.GetRestaurantComboInfoById(srcEntityBO.ItemId);
                    entityBO.UnitRate = cItemEntityBO.ComboPrice;
                    entityBO.Amount = cItemEntityBO.ComboPrice * Convert.ToDecimal(updatedContent);
                    entityBO.ItemUnit = Convert.ToInt32(updatedContent);
                    entityBO.KotId = srcEntityBO.KotId;
                    entityBO.ItemId = srcEntityBO.ItemId;
                    entityBO.CreatedBy = userIdAuthorised;
                }
                else if (srcEntityBO.ItemType == "IndividualItem")
                {
                    if (updateType == "QuantityChange")
                    {
                        itemEntityBO = itemEntityDA.GetInvItemInfoById(costCenterId, srcEntityBO.ItemId);
                        entityBO.UnitRate = itemEntityBO.UnitPriceLocal;
                        entityBO.Amount = itemEntityBO.UnitPriceLocal * Convert.ToDecimal(updatedContent);
                        entityBO.ItemUnit = Convert.ToDecimal(updatedContent);
                        entityBO.KotId = srcEntityBO.KotId;
                        entityBO.ItemId = srcEntityBO.ItemId;
                        entityBO.CreatedBy = userIdAuthorised;
                    }
                    if (updateType == "DeleteQuantity")
                    {
                        itemEntityBO = itemEntityDA.GetInvItemInfoById(costCenterId, srcEntityBO.ItemId);
                        entityBO.UnitRate = itemEntityBO.UnitPriceLocal;
                        entityBO.Amount = itemEntityBO.UnitPriceLocal * Convert.ToDecimal(updatedContent);
                        entityBO.ItemUnit = Convert.ToDecimal(updatedContent);
                        entityBO.KotId = srcEntityBO.KotId;
                        entityBO.ItemId = srcEntityBO.ItemId;
                        entityBO.CreatedBy = userIdAuthorised;
                    }
                    else if (updateType == "ItemNameChange")
                    {
                        entityBO.ItemName = updatedContent;
                        entityBO.KotId = srcEntityBO.KotId;
                        entityBO.ItemId = srcEntityBO.ItemId;
                        entityBO.CreatedBy = userIdAuthorised;
                    }
                    else if (updateType == "UnitPriceChange")
                    {
                        entityBO.UnitRate = Convert.ToDecimal(updatedContent);
                        entityBO.Amount = quantity * Convert.ToDecimal(updatedContent);
                        entityBO.KotId = srcEntityBO.KotId;
                        entityBO.ItemId = srcEntityBO.ItemId;
                        entityBO.CreatedBy = userIdAuthorised;
                    }
                }

                entityBO.KotDetailId = editId;

                Boolean status = entityDA.UpdateKotBillDetailInfo(isItemCanEditDelete, entityBO, updateType);
            }
            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int kotDetailId, int kotId, int itemId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                KotBillDetailDA kotDa = new KotBillDetailDA();
                Boolean status = kotDa.DeleteKotBillDetail(kotDetailId, kotId, itemId, userInformationBO.UserInfoId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo KotSubmit(int costCenterId, int kotId, string kotIdLst, string sourceNumber)
        {
            ReturnInfo rtnInf = new ReturnInfo();
            PrintInfos pinf = new PrintInfos();
            List<PrinterInfoBO> files = new List<PrinterInfoBO>();

            try
            {
                KotBillDetailDA entityDA = new KotBillDetailDA();
                PrinterInfoDA da = new PrinterInfoDA();

                bool rePrintStatus = false;

                List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();
                files = da.GetRestaurentItemTypeInfoByKotId(kotId);

                if (files.Count > 0)
                {
                    foreach (PrinterInfoBO pinfo in files)
                    {
                        pinf.CostCenterId = costCenterId;
                        pinf.PrinterInfoId = pinfo.PrinterInfoId;
                        pinf.KotIdInformation = kotId;
                        pinf.CostCenterName = pinfo.CostCenter;
                        pinf.CompanyName = pinfo.KitchenOrStockName;

                        pinf.TableNumberInformation = sourceNumber;

                        if (pinfo.DefaultView == "Table")
                        {
                            pinf.CostCenterDefaultView = "Table # ";
                        }
                        else if (pinfo.DefaultView == "Token")
                        {
                            pinf.CostCenterDefaultView = "Token # ";
                        }
                        else if (pinfo.DefaultView == "Room")
                        {
                            pinf.CostCenterDefaultView = "Room # ";
                        }

                        HMUtility hmUtility = new HMUtility();

                        UserInformationBO userInformationBO = new UserInformationBO();
                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                        pinf.WaiterName = userInformationBO.UserName; //userInformationBO.EmployeeName.ToString();

                        KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();
                        KotBillMasterBO waiterInformationBO = new KotBillMasterBO();
                        waiterInformationBO = kotBillMasterDA.GetWaiterInformationByKotId(kotId);
                        if (waiterInformationBO != null)
                        {
                            pinf.WaiterName = waiterInformationBO.WaiterName;
                        }

                        if (pinfo.StockType == "StockItem")
                        {
                            entityBOList = entityDA.GetKotOrderSubmitInfoForMultipleTable(kotIdLst, pinf.CostCenterId, "StockItem", false);
                        }
                        else
                        {
                            entityBOList = entityDA.GetKotOrderSubmitInfoForMultipleTable(kotIdLst, pinf.CostCenterId, "KitchenItem", false).Where(x => x.KitchenId == pinfo.KitchenId && x.PrinterName == pinfo.PrinterName).ToList();
                        }

                        if (entityBOList.Count > 0)
                        {
                            if (entityBOList.Count > 0)
                            {
                                rePrintStatus = true;

                                frmRestaurantManagement manage = new frmRestaurantManagement();
                                manage.PrintReportKot(pinfo, entityBOList, pinf, false);
                            }
                            else
                            {
                                rePrintStatus = false;
                            }
                        }
                    }

                    bool status = false;

                    if (rePrintStatus)
                    {
                        status = entityDA.UpdateKotBillDetailPrintFlagByOrderSubmitMultipleKotId(kotIdLst);
                    }

                    if (status && rePrintStatus)
                    {
                        rtnInf.IsSuccess = true;
                        rtnInf.AlertMessage = CommonHelper.AlertInfo("KOT Successfully Submitted.", AlertType.Success);
                    }
                    else if (!status && !rePrintStatus)
                    {
                        rtnInf.IsSuccess = false;
                        rtnInf.AlertMessage = CommonHelper.AlertInfo("KOT already submitted.", AlertType.Information);
                    }
                }
                else
                {
                    rtnInf.AlertMessage = CommonHelper.AlertInfo("Printer Configuration Not Found.", AlertType.Information);
                }
            }
            catch (Exception ex)
            {
                rtnInf.IsSuccess = false;
                rtnInf.AlertMessage = CommonHelper.AlertInfo("KOT is not Submitted Successfully.", AlertType.Information);

            }

            return rtnInf;
        }
        [WebMethod]
        public static string GetClassificationWiseDiscount(int kotId, string kotIdLst)
        {
            InvCategoryDA invCategoryDA = new InvCategoryDA();
            List<ItemClassificationBO> invCategoryLst = new List<ItemClassificationBO>();

            string strTable = string.Empty;
            int counter = 0;

            try
            {
                invCategoryLst = invCategoryDA.GetInvClassificationDetailsForRestaurantBill(kotId, kotIdLst);

                strTable += "<table id='TableClassificationWiseDiscount' class='table table-bordered table-condensed table-responsive'>";
                strTable += "<thead> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th style='width:8%; text-align:center;'>Select</th><th style='width:72%; text-align:left;'>Category</th>";
                strTable += "<th style='width:20%; text-align:center;'>Discount (%)</th>";
                strTable += "</tr> </thead> <tbody>";

                foreach (ItemClassificationBO cls in invCategoryLst)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        strTable += "<tr style='background-color:#E3EAEB;'>";
                    }
                    else
                    {
                        strTable += "<tr style='background-color:White;'>";
                    }

                    strTable += "<td style='width:8%; text-align:center;'>";
                    strTable += "<input type='checkbox' onclick='CategoryWiseDiscountCheckUnCheck(this)' class='form-control' style='margin:0px; padding:0px;' id='c" + cls.ClassificationId + "' name='" + cls.ClassificationId + "' value='" + cls.ClassificationId + "' />";
                    strTable += "</td>";

                    strTable += "<td style='width:72%; text-align:left;'>" + cls.ClassificationName + "</td>";

                    strTable += "<td style='width:20%;'>";
                    strTable += "<input type='text' class='quantity form-control' onblur='CheckInputCategoryDiscount(this)' style='margin:0px; padding:3px;' id='d" + cls.ClassificationId + "' value='" +
                        (cls.DiscountAmount > 0 ? cls.DiscountAmount.ToString("0.00") : "")
                        + "' />";
                    strTable += "</td>";

                    strTable += "<td align='left' style='display:none;'>" + cls.ClassificationId.ToString() + "</td>";
                    strTable += "<td align='left' style='display:none;'>" + cls.DiscountId.ToString() + "</td>";

                    strTable += "</tr>";

                }
                strTable += "</tbody> </table>";

                if (invCategoryLst.Count() == 0)
                {
                    strTable = "<tr><td colspan='3' align='center'>No Record Available !</td></tr>";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strTable;
        }
        [WebMethod(EnableSession = true)]
        public static string GetCategoryWiseDiscountAmountByDefaultSetting(int kotId, int costCenterId, string discountAmount, string discountType)
        {
            decimal discountPercentAmount = !string.IsNullOrWhiteSpace(discountAmount) ? Convert.ToDecimal(discountAmount) : 0;

            KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();
            KotBillMasterBO kotBillMasterBO = new KotBillMasterBO();

            kotBillMasterBO = kotBillMasterDA.GetCategoryWiseDiscountAmountByDefaultSetting(kotId, costCenterId, discountPercentAmount, discountType);

            List<string> categoryList = new List<string>();

            if (!string.IsNullOrEmpty(kotBillMasterBO.CategoryList))
            {
                string[] catList = kotBillMasterBO.CategoryList.Split(',');

                foreach (string s in catList)
                {
                    categoryList.Add(s);
                }
            }

            HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] = categoryList;

            return kotBillMasterBO.TotalAmount.ToString();
        }
        [WebMethod]
        public static ReturnInfo ClearOrderedItem(int costcenterId, int kotId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                KotBillDetailDA kotDa = new KotBillDetailDA();
                Boolean status = kotDa.ClearOrderedItem(costcenterId, kotId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo UpdateRestaurantBillSummary(int billId, string discountType, string costCenterId, string discountAmount, string categoryIdList)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                RestaurentBillDA billDA = new RestaurentBillDA();
                rtninf.IsSuccess = billDA.DistributionRestaurantBill(billId, categoryIdList, discountType, Convert.ToDecimal(discountAmount), Convert.ToInt32(costCenterId));

                if (rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = true;
                    rtninf.Pk = billId;

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Success, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo LoadAlreadyOccupiedTable(int costCenterId, int tableId, int kotId, string sourceName)
        {
            ReturnInfo rtninf = new ReturnInfo();
            KotBillMasterDA kotDa = new KotBillMasterDA();
            KotBillMasterBO kotBillMaster = new KotBillMasterBO();
            kotBillMaster = kotDa.GetKotBillMasterInfoByKotIdNSourceName(kotId, sourceName);

            if (kotBillMaster.KotStatus != "pending")
            {
                rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                rtninf.IsBillResettled = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo("Kot Already " + kotBillMaster.KotStatus + ". Redirect Within a Short Time.", AlertType.Error);
                return rtninf;
            }

            if (kotBillMaster.SourceId != tableId)
            {
                rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                rtninf.IsBillResettled = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo("Source Table Is Changed. Redirect Within a Short Time.", AlertType.Error);
                return rtninf;
            }

            string addedTableNumber = string.Empty;
            List<TableManagementBO> tableList = new List<TableManagementBO>();
            tableList = new TableManagementDA().GetTableInfoByCostCenterNStatusNOrder(costCenterId, 2);

            addedTableNumber = tableList.Where(t => t.TableId == tableId).Select(s => s.TableNumber).FirstOrDefault();
            if (tableList.Count() > 0)
            {
                tableList = tableList.Where(t => t.TableId != tableId).ToList();
            }

            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='OccupiedTableInformation'> <thead> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='width:8%; text-align:center;'>Select</th><th style='width:22%; text-align:left;'>Table Number</th>";
            strTable += "<th style='width:70%; text-align:left;'>Already Added Link</th>";
            strTable += "</tr> </thead> <tbody>";
            int counter = 0;

            foreach (TableManagementBO tbl in tableList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td align='center' style='width:8%; text-align:center;'>";

                if (string.IsNullOrEmpty(tbl.Remarks))
                {
                    strTable += "&nbsp;<input type='checkbox'  id='" + tbl.TableId + "' name='" + tbl.TableNumber + "' value='" + tbl.TableId + "' >";
                }
                else if (tbl.Remarks.Contains(addedTableNumber))
                {
                    strTable += "&nbsp;<input type='checkbox' checked='checked' id='" + tbl.TableId + "' name='" + tbl.TableNumber + "' value='" + tbl.TableId + "' >";
                }

                strTable += "</td>";

                strTable += "<td style='width:22%; text-align:left;'>" + tbl.TableNumber + "</td>";
                strTable += "<td style='width:70%; text-align:left;'>" + tbl.Remarks + "</td>";

                strTable += "<td style='display:none;'>" + tbl.TableId.ToString() + "</td>";
                strTable += "<td style='display:none;'>" + tbl.KotId.ToString() + "</td>";
                strTable += "<td style='display:none;'>" + tbl.DetailId.ToString() + "</td>";
                strTable += "<td style='display:none;'>" + tbl.BillId.ToString() + "</td>";

                strTable += "</tr>";

            }
            strTable += "</tbody> </table>";

            if (tableList.Count() == 0)
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            //strTable += "<div style='margin-top:8px;'>";
            //strTable += "     <button type='button' onClick='javascript:return GetAndApplySelectedTable()' style='padding: 8px 5px; width: 90px;' id='btnAddCheckedTable' class='TransactionalButton btn btn-primary'> OK</button>";
            //strTable += "     <button type='button' onclick='AddMoreTableDialogClose()'  style='padding: 8px 5px; width: 90px;' id='btnAddRoomId' class='TransactionalButton btn btn-primary'> Cancel</button>";
            //strTable += "</div>";

            rtninf.IsSuccess = true;
            rtninf.DataStr = strTable;

            return rtninf;
        }
        [WebMethod]
        public static string GetPromotionalDiscount(int costCenterId, int kotId)
        {
            BusinessPromotionDA bpDA = new BusinessPromotionDA();
            List<BusinessPromotionBO> bpList = new List<BusinessPromotionBO>();

            string strTable = string.Empty;
            int counter = 0;

            try
            {
                bpList = bpDA.GetCurrentActiveBusinessPromotionInfo();

                strTable += "<table cellspacing='0' cellpadding='4' id='TablePromotionalDiscount'> <thead> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='center' scope='col'>Select</th><th align='left' scope='col'>Discount</th><th align='left' scope='col'>Amount (%)</th></tr> </thead> <tbody>";

                foreach (BusinessPromotionBO bp in bpList)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        strTable += "<tr style='background-color:#E3EAEB;'>";
                    }
                    else
                    {
                        strTable += "<tr style='background-color:White;'>";
                    }

                    strTable += "<td align='center' style='width:50px'>";
                    strTable += "&nbsp;<input type='checkbox' id='" + bp.BusinessPromotionId + "' name='" + bp.BPHead.Replace(" ", bp.BusinessPromotionId.ToString()) + "' value='" + bp.BusinessPromotionId + "'  onclick='PromotionalCheckBox(this)'>";
                    strTable += "</td>";

                    strTable += "<td align='left' style='width:250px'>" + bp.BPHead + "</td>";
                    strTable += "<td align='center' style='width:75px'>" + bp.PercentAmount + "</td>";

                    strTable += "<td align='left' style='display:none;'>" + bp.BusinessPromotionId.ToString() + "</td>";
                    strTable += "<td align='left' style='display:none;'>" + (0).ToString() + "</td>";

                    strTable += "</tr>";
                }

                if (bpList.Count() == 0)
                {
                    strTable = "<tr><td colspan='5' align='center'>No Record Available !</td></tr>";
                }

                strTable += "</tbody> </table>";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strTable;
        }
        [WebMethod]
        public static List<GuestCompanyBO> GetGuestCompanyInfo(string companyName)
        {
            GuestCompanyDA bpDA = new GuestCompanyDA();
            return bpDA.GetGuestCompanyInfo(companyName);
        }
        [WebMethod]
        public static List<BankBO> GetBankInfoForAutoComplete(string bankName)
        {
            BankDA bpDA = new BankDA();
            return bpDA.GetBankInfoForAutoComplete(bankName);
        }
        [WebMethod]
        public static ReturnInfo UpdateKotPaxInformation(int costCenterId, int kotId, int paxQuantity)
        {
            ReturnInfo rtninf = new ReturnInfo();

            try
            {
                KotBillMasterBO entityBO = new KotBillMasterBO();
                if (costCenterId > 0)
                {
                    KotBillMasterDA entityDA = new KotBillMasterDA();
                    RestaurantTableBO tableBO = new RestaurantTableBO();
                    RestaurantTableDA tableDA = new RestaurantTableDA();

                    Boolean status = entityDA.UpdateKotBillMasterPaxInfo(kotId, costCenterId, paxQuantity);

                    if (status)
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Pax Update Succeed.", AlertType.Success);
                        rtninf.IsSuccess = true;
                    }
                    else
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                        rtninf.IsSuccess = false;
                    }
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
        public static string LoadRoomWiseGuestInfo(string roomNumer, string guestList)
        {
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumer);
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);

            string strTable = string.Empty, guestChecked = string.Empty;
            string[] guest = guestList.Split(',');
            int counter = 0, row = 0, rowCount = guest.Count();

            strTable += "<table cellspacing='0' cellpadding='4' id='RoomWiseGuestList'> <thead> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='center' scope='col' style='text-align:center; padding-bottom:5px;'><input type='checkbox' id='AllGuestSelect' onclick='CheckGuestAll()' > </th>";
            strTable += "<th align='left' scope='col'>Guest</th></tr> </thead> <tbody>";

            foreach (GuestInformationBO gst in list)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    strTable += "<tr style='background-color:White;'>";
                }

                if (!string.IsNullOrEmpty(guest[0]))
                {
                    for (row = 0; row < rowCount; row++)
                    {
                        if (guest[row].Trim() == gst.GuestName.Trim())
                        {
                            guestChecked = "checked='checked'";
                        }
                    }
                }

                strTable += "<td align='center' style='width: 60px'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + gst.GuestId + "' name='" + gst.GuestId + "' value='" + gst.GuestId + "' " + guestChecked + " >";
                strTable += "</td>";

                strTable += "<td align='left' style='width: 160px'>" + gst.GuestName + "</td>";

                strTable += "<td align='left' style='display:none;'>" + gst.GuestId.ToString() + "</td>";

                strTable += "</tr>";
                guestChecked = string.Empty;

            }
            strTable += "</tbody> </table>";

            if (list.Count() == 0)
            {
                strTable = "<tr><td colspan='2' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }

        [WebMethod]
        public static ReturnInfo UpdateKotWaiterInformation(int costCenterId, int kotId, int waiterId)
        {
            ReturnInfo rtninf = new ReturnInfo();

            try
            {
                KotBillMasterBO entityBO = new KotBillMasterBO();
                if (costCenterId > 0)
                {
                    KotBillMasterDA entityDA = new KotBillMasterDA();

                    Boolean status = entityDA.UpdateKotWaiterInformation(kotId, costCenterId, waiterId);

                    if (status)
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Waiter Change Succeed.", AlertType.Success);
                        rtninf.IsSuccess = true;
                    }
                    else
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                        rtninf.IsSuccess = false;
                    }
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
        public static GuestInformationBO GetRegistrationInformationForSingleGuestByRoomNumber(int costcenterId, string roomNumber)
        {
            CostCentreTabDA costcneterDa = new CostCentreTabDA();
            KotBillMasterDA entityDA = new KotBillMasterDA();
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            GuestInformationBO guestInformationBO = new GuestInformationBO();

            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
            list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);

            KotBillMasterBO roomDetaisForRestaurant = new KotBillMasterBO();
            roomDetaisForRestaurant = entityDA.GetBillDetailInformationForRoomByRoomNumber(roomNumber);

            guestInformationBO.RoomId = 0;
            if (list != null)
            {
                if (list.Count > 0)
                {
                    list[0].RoomRate = allocationBO.RoomRate;
                    list[0].CurrencyTypeHead = allocationBO.CurrencyTypeHead;
                    list[0].ExpectedCheckOutDate = allocationBO.ExpectedCheckOutDate;
                    list[0].RoomId = allocationBO.RoomId;
                    list[0].RoomNumber = allocationBO.RoomNumber;
                    list[0].RoomType = allocationBO.RoomType;
                    list[0].CostCenterId = roomDetaisForRestaurant.CostCenterId == 0 ? costcenterId : roomDetaisForRestaurant.CostCenterId;
                    list[0].KotId = roomDetaisForRestaurant.KotId;
                    guestInformationBO = list[0];

                    guestInformationBO.IsStopChargePosting = costcneterDa.GetRoomStopChargePostingByRegistrationAndCostCenterId(allocationBO.RegistrationId, costcenterId);
                }
            }

            return guestInformationBO;
        }
    }
}
