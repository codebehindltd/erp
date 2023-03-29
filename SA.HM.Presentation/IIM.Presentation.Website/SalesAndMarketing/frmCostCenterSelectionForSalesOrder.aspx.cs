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
using System.Web.Services;
using System.IO;
using Microsoft.Reporting.WebForms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data;
using HotelManagement.Entity;
using System.Collections;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using Newtonsoft.Json;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Presentation.Website.POS;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class frmCostCenterSelectionForSalesOrder : BasePage
    {
        private Boolean IsRestaurantOrderSubmitDisable = false;
        private Boolean IsRestaurantTokenInfoDisable = false;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfoForRestaurant();

            hfIsBearar.Value = (userInformationBO.IsBearer == true ? "1" : "0");
            hfUserInfoObj.Value = JsonConvert.SerializeObject(userInformationBO);
            //HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
            //HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
            //HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
            //HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
            //HttpContext.Current.Session.Remove("IRCostCenterIdSession");
            //HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
            //HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
            //HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
            //HttpContext.Current.Session.Remove("IRToeknNumber");
            //HttpContext.Current.Session.Remove("KotHoldupBill");
            //HttpContext.Current.Session.Remove("RestaurantKotBillResume");
            //HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
            //HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
            //HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
            //HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
            //HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");            
            //HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
            //HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
            //HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
            //HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
            //HttpContext.Current.Session.Remove("IRCostCenterIdSession");
            //HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
            //HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
            //HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
            //HttpContext.Current.Session.Remove("IRToeknNumber");
            //HttpContext.Current.Session.Remove("IRtxtTableIdInformation");
            //HttpContext.Current.Session.Remove("IRtxtTableNumberInformation");
            //HttpContext.Current.Session.Remove("tbsMessage");
            //HttpContext.Current.Session.Remove("IRTableAllocatedBy");
            //HttpContext.Current.Session.Remove("IRtxtBearerIdInformation");

            //if (HttpContext.Current.Session["OrderType"] != null)
            //{
            //    hfOrderType.Value = HttpContext.Current.Session["OrderType"].ToString();
            //    hfOrderCostcenterId.Value = HttpContext.Current.Session["OrderCostcenterId"].ToString();
            //}

            //HttpContext.Current.Session.Remove("OrderType");
            //HttpContext.Current.Session.Remove("OrderCostcenterId");

            //------------------------
            //HttpContext.Current.Session.Remove("RestaurantKotBillResume");
            //HttpContext.Current.Session.Remove("KotHoldupBill");

            LoadCostCenterInformation();
            RestaurantPaxConfirmationEnable();
            CheckRestaurantWaiterConfirmationEnable();
        }
        //************************ User Defined Function ********************//
        private void RestaurantPaxConfirmationEnable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO isRestaurantPaxConfirmationEnableBO = new HMCommonSetupBO();
            isRestaurantPaxConfirmationEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantPaxConfirmationEnable", "IsRestaurantPaxConfirmationEnable");

            IsRestaurantPaxConfirmationEnable.Value = "0";
            if (isRestaurantPaxConfirmationEnableBO != null)
            {
                IsRestaurantPaxConfirmationEnable.Value = isRestaurantPaxConfirmationEnableBO.SetupValue;
            }
        }

        private void CheckRestaurantWaiterConfirmationEnable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO isRestaurantPaxConfirmationEnableBO = new HMCommonSetupBO();
            isRestaurantPaxConfirmationEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantWaiterConfirmationEnable", "IsRestaurantWaiterConfirmationEnable");

            if (isRestaurantPaxConfirmationEnableBO != null)
            {
                IsRestaurantWaiterConfirmationEnable.Value = isRestaurantPaxConfirmationEnableBO.SetupValue;
            }
        }
        private void CleanSessionInformation()
        {
            Session["IRCostCenterIdSession"] = null;
            Session["IRtxtKotIdInformation"] = null;
            Session["IRCostCenterServiceChargeSession"] = null;
            Session["IRCostCenterVatAmountSession"] = null;
            Session["GuestPaymentDetailListForGrid"] = null;
        }
        private void LoadCostCenterInformation()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO != null)
            {
                List<RestaurantBearerBO> boList = new List<RestaurantBearerBO>();
                RestaurantBearerDA da = new RestaurantBearerDA();

                boList = da.GetRestaurantBearerInfoByUserId(userInformationBO.UserInfoId, 0, 1);

                if (boList.Count > 0)
                {
                    //costCenterList = entityDA.GetRestaurantTypeCostCentreTabInfo(userInformationBO.UserId, userInformationBO.UserInfoId, 1);
                    //costCenterList = entityDA.GetRestaurantAndRetailPosTypeCostCentreTabInfo(userInformationBO.UserId, userInformationBO.UserInfoId, 1);
                    RestaurantKitchenDA entityDA = new RestaurantKitchenDA();
                    List<RestaurantKitchenBO> entityBOList = new List<RestaurantKitchenBO>();
                    entityBOList = entityDA.GetRestaurantKitchenInfoByUserInfoId(userInformationBO.UserInfoId);


                    string roomSummary = string.Empty, subContent = string.Empty, endContent = string.Empty;


                    foreach (RestaurantKitchenBO cs in entityBOList)
                    {


                        subContent += @"<a href='javascript:void(0)'" +
                                       "onclick=\"javascript:return LoadKitchenInfo(" + cs.KitchenId.ToString() + ",'" + cs.KitchenName + "')\">";

                        subContent += @"<div class='DivRoomContainer' style='padding-bottom:5px;' ><div class='RoomVacantDiv'></div>
                                            <div class='RoomNumberDiv'>" + cs.KitchenName + "</div></div></a>";

                        //subContent += @"<a href='javascript:void(0)'" +
                        //               "onclick=\"javascript:return LoadTableOrderInfo(" + cs.CostCenterId.ToString() + ",'" + cs.CostCenter + "')\">";

                        //subContent += @"<div class='DivRoomContainer' style='padding-bottom:5px;' ><div class='RoomVacantDiv'></div>
                        //                <div class='RoomNumberDiv'>" + cs.CostCenter + "</div></div></a>";

                    }
                    literalKitchenInformation.Text = subContent;
                    CostCenterListDiv.Visible = false;
                    TokenListDiv.Visible = false;
                    KitchenListDiv.Visible = true;
                    hfIsChef.Value = "1";

                }
                else
                {
                    hfIsChef.Value = "0";
                    CostCenterListDiv.Visible = true;
                    TokenListDiv.Visible = true;
                    KitchenListDiv.Visible = false;
                    Session["IRtxtBearerIdInformation"] = userInformationBO.UserInfoId.ToString();

                    CostCentreTabDA entityDA = new CostCentreTabDA();
                    List<CostCentreTabBO> costCenterList = new List<CostCentreTabBO>();
                    //costCenterList = entityDA.GetRestaurantTypeCostCentreTabInfo(userInformationBO.UserId, userInformationBO.UserInfoId, 1);
                    costCenterList = entityDA.GetRestaurantAndRetailPosTypeCostCentreTabInfo(userInformationBO.UserId, userInformationBO.UserInfoId, 1);

                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                    HMCommonSetupBO isRestaurantPaxConfirmationEnableBO = new HMCommonSetupBO();
                    isRestaurantPaxConfirmationEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsCostCenterWiseBillNumberGenerate", "IsCostCenterWiseBillNumberGenerate");

                    string roomSummary = string.Empty, subContent = string.Empty, endContent = string.Empty;

                    if (costCenterList.Count > 0)
                    {
                        var vc = costCenterList.Where(c => c.CostCenterType == "RetailPos").ToList();

                        if (costCenterList.Count == 1 && vc.Count == 1)
                        {
                            Response.Redirect("/SalesAndMarketing/frmRetailPos.aspx?cid=" + vc[0].CostCenterId);
                        }
                    }
                    if (costCenterList.Count > 0)
                    {
                        var vc = costCenterList.Where(c => c.CostCenterType == "Billing").ToList();

                        if (costCenterList.Count == 1 && vc.Count == 1)
                        {
                            Response.Redirect("/SalesAndMarketing/frmSalesOrder.aspx?cid=" + vc[0].CostCenterId);
                        }
                    }
                    if (costCenterList.Count > 0)
                    {
                        var vc = costCenterList.Where(c => c.CostCenterType == "SOBilling").ToList();

                        if (costCenterList.Count == 1 && vc.Count == 1)
                        {
                            Response.Redirect("/SalesAndMarketing/frmSOBilling.aspx?cid=" + vc[0].CostCenterId);
                        }
                    }

                    foreach (CostCentreTabBO cs in costCenterList)
                    {
                        if (cs.CostCenterType == "Restaurant")
                        {
                            if (cs.DefaultView == "Token")
                            {
                                subContent += @"<a href='javascript:void(0)'" +
                                               "onclick=\"javascript:return LoadTokenInfo('tkn'," + cs.CostCenterId.ToString() + ",'" + cs.CostCenter + "')\">";
                            }
                            else if (cs.DefaultView == "Table")
                            {
                                subContent += @"<a href='javascript:void(0)'" +
                                               "onclick=\"javascript:return LoadTableInfo(" + cs.CostCenterId.ToString() + ",'" + cs.CostCenter + "')\">";
                            }
                            else if (cs.DefaultView == "Room")
                            {
                                subContent += @"<a href='javascript:void(0)'" +
                                               "onclick=\"javascript:return LoadRoomInfo(" + cs.CostCenterId.ToString() + ",'" + cs.CostCenter + "')\">";
                            }
                            subContent += @"<div class='DivRoomContainer' style='padding-bottom:5px;' ><div class='RoomVacantDiv'></div>
                                            <div class='RoomNumberDiv'>" + cs.CostCenter + "</div></div></a>";
                        }
                        else if (cs.CostCenterType == "RetailPos")
                        {
                            subContent += @"<a href='javascript:void(0)'" +
                                           "onclick=\"javascript:return RetailPosInfo(" + cs.CostCenterId.ToString() + ",'" + cs.CostCenter + "')\">";

                            subContent += @"<div class='DivRoomContainer' style='padding-bottom:5px;' ><div class='RoomVacantDiv'></div>
                                            <div class='RoomNumberDiv'>" + cs.CostCenter + "</div></div></a>";
                        }
                        else if (cs.CostCenterType == "Billing")
                        {
                            subContent += @"<a href='javascript:void(0)'" +
                                           "onclick=\"javascript:return PosBillingInfo(" + cs.CostCenterId.ToString() + ",'" + cs.CostCenter + "')\">";

                            subContent += @"<div class='DivRoomContainer' style='padding-bottom:5px;' ><div class='RoomVacantDiv'></div>
                                            <div class='RoomNumberDiv'>" + cs.CostCenter + "</div></div></a>";
                        }
                        else if (cs.CostCenterType == "SOBilling")
                        {
                            subContent += @"<a href='javascript:void(0)'" +
                                           "onclick=\"javascript:return SOBillingInfo(" + cs.CostCenterId.ToString() + ",'" + cs.CostCenter + "')\">";

                            subContent += @"<div class='DivRoomContainer' style='padding-bottom:5px;' ><div class='RoomVacantDiv'></div>
                                            <div class='RoomNumberDiv'>" + cs.CostCenter + "</div></div></a>";
                        }
                    }

                    literalCostCenterInformation.Text = subContent;

                    hfIsCostCenterWiseBillNumberGenerate.Value = isRestaurantPaxConfirmationEnableBO.SetupValue;

                    if (isRestaurantPaxConfirmationEnableBO.SetupValue == "1")
                    {
                        subContent = string.Empty;
                        subContent = "<table id='costcentertableforreprint' class='table table-bordered table-condensed table-hover table-responsive'>";
                        subContent += " <tbody> ";
                        endContent = "</tr> </tbody> </table>";

                        foreach (CostCentreTabBO cs in costCenterList)
                        {
                            subContent += @"<tr> <td style='color:#fff; font-weight:bold;' class='progress-bar-success'>";
                            subContent += "<a href='javascript:void(0)' style='display:block; text-decoration:none;' onclick=\"CostcenterSelectionForReprint(this," + cs.CostCenterId + ",'" + cs.BillNumberPrefix + "')\">" + cs.CostCenter + "</a>";
                            subContent += @"</td></tr>";
                        }

                        literalCostCenterForReprint.Text = (subContent + endContent);
                    }
                    else
                    {
                        hfBillPrefixCostcentrwise.Value = costCenterList[0].BillNumberPrefix;
                    }
                }

            }
            else
            {
                Response.Redirect("/Login.aspx");
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
                        //this.btnOrderSubmit.Visible = false;
                        IsRestaurantOrderSubmitDisable = false;
                    }
                    else
                    {
                        //this.btnOrderSubmit.Visible = true;
                        IsRestaurantOrderSubmitDisable = true;
                    }
                }
            }

            //this.imgBtnRoomWiseKotBill.Visible = false;
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
                        //this.btnOrderSubmit.Visible = false;
                        IsRestaurantTokenInfoDisable = false;
                    }
                    else
                    {
                        //this.btnOrderSubmit.Visible = true;
                        IsRestaurantTokenInfoDisable = true;
                    }
                }
            }

            //this.imgBtnRoomWiseKotBill.Visible = false;
        }
        //************************ User Defined Web Method ********************//

        [WebMethod]
        public static ReturnInfo CheckOccupiedTableInfo(int costcenterId, int tableId, int kotId)
        {
            ReturnInfo rtn = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();

            TableManagementDA entityDA = new TableManagementDA();
            List<TableManagementBO> tableLstBO = new List<TableManagementBO>();

            tableLstBO = entityDA.GetTableManagementNOrderInfo(costcenterId, ConstantHelper.RestaurantBillSource.RestaurantTable.ToString());
            var tab = tableLstBO.Where(t => t.TableId == tableId).FirstOrDefault();

            if (tab != null)
            {
                if (kotId == 0 && tab.StatusId == 2)
                {
                    rtn.IsSuccess = false;
                }
                else if (kotId > 0 && tab.StatusId == 1)
                {
                    rtn.IsSuccess = false;
                }
                else
                {
                    rtn.IsSuccess = true;
                }
            }
            else
            {
                rtn.IsSuccess = true;
            }

            return rtn;
        }

        [WebMethod]
        public static ReturnInfo LoadHoldupBill(string sourceType, int sourceId, int costCenterId, int kotId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            string sourceName = string.Empty;

            try
            {
                KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();
                KotBillMasterBO kotBill = new KotBillMasterBO();

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

                kotBill = kotBillMasterDA.GetKotBillMasterInfoByKotIdNSourceName(kotId, sourceName);

                if (kotBill.KotStatus == ConstantHelper.KotStatus.pending.ToString() && kotBill.IsBillProcessed == false)
                {
                    //HttpContext.Current.Session["KotHoldupBill"] = kotBill;

                    rtninf.IsSuccess = true;
                    //frmRestaurantOrderManagement.aspx?st= " + sourceType + "&sid=" + sourceId + "&cc=" + costCenterId + "&kid=" + kotId;
                    //ot = order type, no = new order, ro = resume order
                    rtninf.RedirectUrl = string.Format("frmOrderManagement.aspx?ot={0}&st={1}&sid={2}&cc={3}&kid={4}", "ro", sourceType, sourceId, costCenterId, kotId);

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo("Kot is already Settled", AlertType.Information);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
        [WebMethod]
        public static TableOccupiedInfoBO LoadTableInfo(int costcenterId)
        {
            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            TableManagementDA entityDA = new TableManagementDA();
            List<TableManagementBO> tableLstBO = new List<TableManagementBO>();
            List<TableManagementBO> occupiedTableByKotCreator = new List<TableManagementBO>();

            tableLstBO = entityDA.GetTableManagementNOrderInfo(costcenterId, ConstantHelper.RestaurantBillSource.RestaurantTable.ToString());
            occupiedTableByKotCreator = tableLstBO.Where(k => k.KotCreatedBy == userInformationBO.UserInfoId && k.StatusId == 2).ToList();

            //HttpContext.Current.Session["OrderType"] = "Table";
            //HttpContext.Current.Session["OrderCostcenterId"] = costcenterId;

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isRestaurantPaxConfirmationEnableBO = new HMCommonSetupBO();
            isRestaurantPaxConfirmationEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantPaxConfirmationEnable", "IsRestaurantPaxConfirmationEnable");

            string topPart = @"<div id='legendContainerForRestaurant'>
                                
                               </div><div class='divClear'>
                               </div>
                               <div>";

            string endTemplatePart = @"</div>";
            string subContent = string.Empty;
            string occupiedTableList = string.Empty, endContent = string.Empty, waiterWiseTable = string.Empty;

            occupiedTableList = "<table class='table table-bordered table-condensed table-hover table-responsive'>";
            occupiedTableList += " <tbody> ";
            endContent = "</tbody> </table>";

            foreach (TableManagementBO tbm in occupiedTableByKotCreator)
            {
                occupiedTableList += @"<tr> <td style='background:#D63CC4; color:#fff; font-weight:bold;'>";
                occupiedTableList += "<a href='javascript:void(0)' style='display:block; text-decoration:none;' ";
                occupiedTableList += "onclick=\"javascript:return RestaurantBillOptions('tbl'," + tbm.TableId.ToString() + "," + costcenterId.ToString() + "," + tbm.KotId.ToString() + ")\">";
                occupiedTableList += tbm.TableNumber;
                occupiedTableList += @"</a></td></tr>";
            }

            waiterWiseTable = "<table class='table table-bordered table-condensed table-hover table-responsive'>";
            waiterWiseTable += " <tbody> ";

            foreach (TableManagementBO tbm in tableLstBO)
            {
                if (tbm.StatusId == 1)
                {
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    if (tbm.TableManagementId > 0)
                        docList = (new DocumentsDA().GetDocumentsByUserTypeAndUserId("VacantTableDocument", (int)tbm.TableManagementId));

                    docList = new HMCommonDA().GetDocumentListWithIcon(docList);

                    int len = docList.Count;
                    var Path = "";
                    if (len > 0)
                        Path = docList[len - 1].Path + docList[len - 1].Name;

                    subContent += @"<a href='javascript:void(0)'";
                    subContent += "onclick=\"javascript:return PaxConfirmation('tbl', " + tbm.TableId.ToString() + "," + costcenterId.ToString() + "," + tbm.KotId.ToString() + ")\">";
                    if (Path == "")
                        subContent += @"<div class='NotDraggable RestaurantTableAvailableDiv' style='width:" + tbm.TableWidth + "px; height:" + tbm.TableHeight + "px; top:" + tbm.YCoordinate + "px; left:" + tbm.XCoordinate + "px; transform:" + tbm.DivTransition + "; ' id='" + tbm.TableManagementId + "'>";
                    else
                        subContent += @"<div class='NotDraggable ' style='border:none; width:" + tbm.TableWidth + "px; height:" + tbm.TableHeight + "px; top:" + tbm.YCoordinate + "px; left:" + tbm.XCoordinate + "px; transform:" + tbm.DivTransition + ";  ' id='" + tbm.TableManagementId + "'> <img src=" + Path + " alt='Table Image' height=" + tbm.TableHeight + " width=" + tbm.TableWidth + "> ";
                    subContent += @"<div class='RestaurantTableAvailableDiv'>
                                        </div>";

                    if (Path == "")
                        subContent += @"<div class='TableNumberDiv'>" + tbm.TableNumber + "</div></div></a>";
                    else
                        subContent += @"<div class='TableNumberDiv'></div></div></a>";
                }
                else if (tbm.StatusId == 2)
                {
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    if (tbm.TableManagementId > 0)
                        docList = (new DocumentsDA().GetDocumentsByUserTypeAndUserId("OccupiedTableDocument", (int)tbm.TableManagementId));

                    docList = new HMCommonDA().GetDocumentListWithIcon(docList);

                    int len = docList.Count;
                    var Path = "";
                    if (len > 0)
                        Path = docList[len - 1].Path + docList[len - 1].Name;

                    subContent += @"<a href='javascript:void(0)'";
                    subContent += "onclick=\"javascript:return RestaurantBillOptions('tbl'," + tbm.TableId.ToString() + "," + costcenterId.ToString() + "," + tbm.KotId.ToString() + ")\">";
                    if (Path == "")
                        subContent += @"<div class='NotDraggable RestaurantTableBookedDiv' style='width:" + tbm.TableWidth + "px; height:" + tbm.TableHeight + "px; top:" + tbm.YCoordinate + "px; left:" + tbm.XCoordinate + "px; transform:" + tbm.DivTransition + ";' id='" + tbm.TableManagementId + "'>";//"../Images/TablePic/occupied.png"
                    else
                        subContent += @"<div class='NotDraggable' style='border:none; width:" + tbm.TableWidth + "px; height:" + tbm.TableHeight + "px; top:" + tbm.YCoordinate + "px; left:" + tbm.XCoordinate + "px; transform:" + tbm.DivTransition + "; ' id='" + tbm.TableManagementId + "'> <img src=" + Path + " alt='Table Image' height=" + tbm.TableHeight + " width=" + tbm.TableWidth + ">  ";//"../Images/TablePic/occupied.png"

                    subContent += @"<div class='KOTNTableNumberDiv' style='display:none;'>" + costcenterId + "," + tbm.TableId + "," + tbm.StatusId + "</div>";

                    if (Path == "")
                        subContent += @"<div class='TableNumberDiv'>" + tbm.TableNumber + "</div></div></a>";
                    else
                        subContent += @"<div class='TableNumberDiv'></div></div></a>";

                    waiterWiseTable += @"<tr> <td style='font-weight:bold; width:30%;'>";
                    waiterWiseTable += "<a href='javascript:void(0)' style='display:block; text-decoration:none;' ";
                    waiterWiseTable += "onclick=\"javascript:return RestaurantBillOptions('tbl'," + tbm.TableId.ToString() + "," + costcenterId.ToString() + "," + tbm.KotId.ToString() + ")\">";
                    waiterWiseTable += tbm.TableNumber;
                    waiterWiseTable += @"</a></td>";

                    waiterWiseTable += @"<td style='font-weight:bold;  width:70%;'>";
                    waiterWiseTable += "<a href='javascript:void(0)' style='display:block; text-decoration:none;' ";
                    waiterWiseTable += "onclick=\"javascript:return RestaurantBillOptions('tbl'," + tbm.TableId.ToString() + "," + costcenterId.ToString() + "," + tbm.KotId.ToString() + ")\">";
                    waiterWiseTable += tbm.KotCreatedByName;
                    waiterWiseTable += @"</a></td>";

                    waiterWiseTable += @"</tr>";
                }
                else if (tbm.StatusId == 3)
                {
                    subContent += @"<a href='javascript:void(0)'>";
                    subContent += @"<div class='NotDraggable RestaurantTableReservedDiv' style='width:" + tbm.TableWidth + "px; height:" + tbm.TableHeight + "px; top:" + tbm.YCoordinate + "px; left:" + tbm.XCoordinate + "px; transform:" + tbm.DivTransition + ";' id='" + tbm.TableManagementId + "'>";
                    subContent += @"<div class='RestaurantTableReservedDiv'>
                                        </div>
                                        <div class='TableNumberDiv'>" + tbm.TableNumber + "</div></div></a>";

                }
            }

            TableOccupiedInfoBO tb = new TableOccupiedInfoBO();

            tb.TableInfo = (topPart + subContent + endTemplatePart);
            tb.OccupiedTableInfo = occupiedTableList + endContent;
            tb.OccupiedTableInfoByWaiter = waiterWiseTable + endContent;
            tb.IsWaiter = userInformationBO.IsBearer;
            tb.OrderType = "Table";
            tb.OrderCostcenterId = costcenterId;

            return tb;
        }


        [WebMethod]
        public static string LoadTokenInformation()
        {
            string fullContent = string.Empty;
            string groupNamePart = string.Empty;

            string endTemplatePart = @"</div> </div>";

            RestaurantTokenDA tokenDa = new RestaurantTokenDA();
            List<RestaurantTokenBO> tokenList = new List<RestaurantTokenBO>();

            tokenList = tokenDa.GetRestaurantTokenInfo();

            string subContent = string.Empty;

            foreach (RestaurantTokenBO tkn in tokenList)
            {
                if (tkn.IsBillHoldup)
                {
                    subContent += @"<a href='javascript:void(0)'";
                    subContent += "onclick=\"javascript:return ResumeBill('tkn'," + tkn.SourceId.ToString() + "," + tkn.CostCenterId.ToString() + "," + tkn.KotId.ToString() + ")\">";
                    subContent += "<div class='DivRoomContainerHeight61'> " +
                                   "<div class='RoomOccupaiedDiv'>" +
                                   "<div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + tkn.TokenNumber + " </div></div></div>";
                    subContent += @"</a>";
                }
                else
                {
                    subContent += "<div class='DivRoomContainerHeight61'><div class='RoomVacantDiv'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + tkn.TokenNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + tkn.KotId + "</div></div>";
                }
            }

            fullContent += subContent;

            return (fullContent + endTemplatePart);
        }
        [WebMethod]
        public static string LoadRoomInfo(int costcenterId)
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            KotBillMasterDA entityDA = new KotBillMasterDA();

            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();
            List<KotBillMasterBO> roomDetaisForRestaurant = new List<KotBillMasterBO>();

            string fullContent = string.Empty;
            string subContent = string.Empty;

            string topPart = @"<div id='legendContainerForRestaurant'>                                
                               </div><div class='divClear'>
                               </div>
                               <div>";
            string endTemplatePart = @"</div>";

            roomNumberList = roomNumberDA.GetRoomNumberInfoByRoomType(0);
            roomDetaisForRestaurant = entityDA.GetBillDetailInformationForRoom();

            //?st= " + sourceType + "&sid=" + sourceId + "&cc=" + CostCenterId + "&kid=" + KotId;

            foreach (RoomNumberBO rn in roomNumberList)
            {
                if (rn.StatusId == 2)
                {
                    var vr = (from r in roomDetaisForRestaurant where r.RoomId == rn.RoomId select r).FirstOrDefault();

                    if (vr != null)
                    {
                        subContent += @"<a href='javascript:void(0)'";
                        subContent += "onclick=\"javascript:return ResumeBill('rom'," + vr.RoomId.ToString() + "," + costcenterId.ToString() + "," + vr.KotId.ToString() + ")\">";
                        subContent += @"<div class='RestaurantTableBookedDiv'><div class='RoomOccupaiedDiv'>
                                        <div class='TableNumberDiv'>" + rn.RoomNumber + "</div>" +
                                        "</div> </div></a>";
                    }
                    else
                    {
                        subContent += @"<a href='javascript:void(0)'";
                        subContent += "onclick=\"javascript:return ResumeBill('rom'," + rn.RoomId.ToString() + "," + costcenterId.ToString() + "," + (0).ToString() + ")\">";
                        subContent += @"<div class='RestaurantTableBookedDiv'><div class='RoomVacantDiv'>
                                        <div class='TableNumberDiv'>" + rn.RoomNumber + "</div>" +
                                       "</div> </div></a>";
                    }
                }
            }

            fullContent = topPart + subContent + endTemplatePart;

            return fullContent;
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
                    list[0].BillId = roomDetaisForRestaurant.BillId;
                    guestInformationBO = list[0];

                    guestInformationBO.IsStopChargePosting = costcneterDa.GetRoomStopChargePostingByRegistrationAndCostCenterId(allocationBO.RegistrationId, costcenterId);

                }
            }

            return guestInformationBO;
        }
        [WebMethod]
        public static ReturnInfo ClearRoomKot(int costCenterId, int kotId, int roomId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int lastModifiedBy = userInformationBO.UserInfoId;
            KotBillMasterDA entityDA = new KotBillMasterDA();

            rtninf.IsSuccess = entityDA.CleanRestaurantOrderForRommService(costCenterId, kotId, roomId, lastModifiedBy);

            if (rtninf.IsSuccess)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo("Room Clean is Succeed.", AlertType.Success);
            }

            rtninf.Pk = costCenterId;
            return rtninf;
        }
        [WebMethod]
        public static ArrayList GetRegistrationInformationForRoomChangeByRoomNumber(int costcenterId, string roomNumber, string newRoomNumber)
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
                list[0].BillId = roomDetaisForRestaurant.BillId;
                guestInformationBO = list[0];

                guestInformationBO.IsStopChargePosting = costcneterDa.GetRoomStopChargePostingByRegistrationAndCostCenterId(allocationBO.RegistrationId, costcenterId);
            }

            ArrayList arr = new ArrayList();
            arr.Add(list[0]);

            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(newRoomNumber);
            list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);

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

            if (list.Count > 0)
                arr.Add(list[0]);

            return arr;
        }
        [WebMethod]
        public static ReturnInfo LoadOccupiedPossiblePath(string sourceType, int sourceId, int costCenterId, int kotId)
        {
            ReturnInfo rtn = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();

            TableManagementDA entityDA = new TableManagementDA();
            List<TableManagementBO> tableLstBO = new List<TableManagementBO>();

            tableLstBO = entityDA.GetTableManagementNOrderInfo(costCenterId, ConstantHelper.RestaurantBillSource.RestaurantTable.ToString());
            var tab = tableLstBO.Where(t => t.TableId == sourceId).FirstOrDefault();

            if (kotId == 0 && tab.StatusId == 2)
            {
                rtn.IsSuccess = false;
                rtn.AlertMessage = CommonHelper.AlertInfo("Table Is Occupied In Different Way.Window Is Refreshing.Plesae Wait.",
                          AlertType.Warning);

                return rtn;
            }
            else if (kotId > 0 && tab.StatusId == 1)
            {
                rtn.IsSuccess = false;
                rtn.AlertMessage = CommonHelper.AlertInfo("Table Is Occupied In Different Way.Window Is Refreshing.Plesae Wait.",
                          AlertType.Warning);

                return rtn;
            }
            else
            {
                rtn.IsSuccess = true;
            }

            Boolean isWMDeletePermission = false;
            string sourceName = string.Empty;

            if (sourceType == "tkn")
            {
                sourceName = ConstantHelper.RestaurantBillSource.RestaurantToken.ToString();
            }
            else if (sourceType == "tbl")
            {
                sourceName = ConstantHelper.RestaurantBillSource.RestaurantTable.ToString();
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantKotContinueWithDiferentWaiter", "IsRestaurantKotContinueWithDiferentWaiter");

            if (userInformationBO.UserInfoId != 1)
            {
                List<RestaurantBearerBO> restaurantBearerBOList = new List<RestaurantBearerBO>();
                restaurantBearerBOList = restaurantBearerDA.GetRestaurantBearerInfoByUserId(userInformationBO.UserInfoId, 0, 0);
                if (restaurantBearerBOList != null)
                {
                    if (restaurantBearerBOList.Count > 0)
                    {
                        isWMDeletePermission = restaurantBearerBOList[0].IsItemCanEditDelete;
                    }
                }
            }
            else
            {
                isWMDeletePermission = true;
            }

            string strTable = string.Empty;
            List<TableManagementBO> tableList = new List<TableManagementBO>();
            tableList = new TableManagementDA().GetTableInfoByCostCenterNStatus(costCenterId, 1);

            KotBillMasterDA masterDa = new KotBillMasterDA();

            KotBillDetailDA kotDetailsDA = new KotBillDetailDA();
            List<KotBillDetailBO> files = kotDetailsDA.GetKotBillDetailInfoByKotNBearerId(costCenterId, "RestaurantTable", sourceId, kotId);
            var v = files.Where(s => s.PrintFlag == true).ToList();

            List<KotBillMasterBO> entityBOList = new List<KotBillMasterBO>();
            entityBOList = masterDa.GetBillDetailInfoByKotId(sourceName, kotId);

            if (commonSetupBO.SetupValue == "0" && entityBOList.Count > 0)
            {
                if (entityBOList[0].CreatedBy != userInformationBO.UserInfoId)
                {
                    RestaurantTableBO entityTableBO = new RestaurantTableBO();
                    RestaurantTableDA entityTableDA = new RestaurantTableDA();
                    entityTableBO = entityTableDA.GetRestaurantTableInfoById(costCenterId, "RestaurantTable", sourceId);

                    RestaurantBearerBO tableAllocatedBy = new RestaurantBearerBO();
                    tableAllocatedBy = restaurantBearerDA.GetRestaurantBearerInfoByEmpId(entityBOList[0].CreatedBy);

                    if (userInformationBO.IsBearer)
                    {
                        rtn.IsSuccess = false;
                        rtn.AlertMessage = CommonHelper.AlertInfo("This Table (" + entityTableBO.TableNumber + ") Is already Allocated By " + tableAllocatedBy.UserName + ".",
                            AlertType.Warning);
                    }
                }
            }

            int billId = 0;
            if (entityBOList.Count > 0)
            {
                billId = entityBOList[0].BillId;
            }

            strTable += "<div class='row'>";

            strTable += "<div class='col-md-3'>";
            strTable += "<input type='button' style='width:115px' value='" + "KOT Continue" + "' class='TransactionalButton btn btn-primary'";
            strTable += "onclick=\"javascript:return ResumeBill('tbl'," + sourceId.ToString() + "," + costCenterId.ToString() + "," + kotId.ToString() + ")\">";
            strTable += "</div>";

            strTable += "<div class='col-md-3'>";
            strTable += "<input type='button' style='width:115px' value='" + "Table Change" + "' class='TransactionalButton btn btn-primary'";
            strTable += " onclick=\"return OpenTableShiptPanel('" + costCenterId + "', '" + sourceId + "' );\"  />";
            strTable += "</div>";

            if (isWMDeletePermission && billId == 0 && v.Count == 0)
            {
                strTable += "<div class='col-md-3'>";
                strTable += "<input type='button' style='width:115px' value='" + "Table Clean" + "' class='TransactionalButton btn btn-primary'";
                strTable += " onclick=\"return OpenTableCleanPanel('" + costCenterId + "', '" + sourceId + "' );\"  />";
                strTable += "</div>";
            }
            else
            {
                //if (isWMDeletePermission && v.Count == 0 && billId == 0)
                //{
                //    strTable += "<div class='col-md-3'>";
                //    strTable += "<input type='button' style='width:115px' value='" + "Table Clean" + "' class='TransactionalButton btn btn-primary'";
                //    strTable += " onclick=\"return OpenTableCleanPanel('" + costCenterId + "', '" + sourceId + "' );\"  />";
                //    strTable += "</div>";
                //}
                if (v.Count == 0 && billId == 0 && v.Count == 0)
                {
                    strTable += "<div class='col-md-3'>";
                    strTable += "<input type='button' style='width:115px' value='" + "Table Clean" + "' class='TransactionalButton btn btn-primary'";
                    strTable += " onclick=\"return OpenTableCleanPanel('" + costCenterId + "', '" + sourceId + "' );\"  />";
                    strTable += "</div>";
                }
            }

            if (!userInformationBO.IsBearer)
            {
                strTable += "<div class='col-md-3'>";
                strTable += "<input type='button' style='width:115px' value='" + "KOT Preview" + "' class='TransactionalButton btn btn-primary'";
                strTable += " onclick=\"javascript:return KotPrintPreview(" + kotId + "," + sourceId + ",'" + sourceType + "'" + ")\"  />";
                strTable += "</div> </div>";
            }

            strTable += "<div class='row no-gutters'> <div class='col-md-12'> <div style='font-weight: bold; display:none; height:60px;' class='block' id='TableShiftInfo'>";

            strTable += "<div style='width:500px; margin-left:5px; margin-top:20px;' > <fieldset> <legend>Table Info</legend>";
            strTable += "<div class='form-group'> ";
            strTable += "<label class='col-md-4 control-label'>Available Table:</label>";
            strTable += "<div class='col-md-8'>";

            string availAbleTable = string.Empty;
            if (tableList.Count() > 0)
            {
                availAbleTable = "<select id='ddlAvailableTable' class='form-control'>";
                foreach (TableManagementBO tb in tableList)
                {
                    availAbleTable += "<option value='" + tb.TableId + "'>" + tb.TableNumber + "</option>";
                }
                availAbleTable += "</select>";

                strTable += availAbleTable;
            }
            strTable += "</div> </div>";

            strTable += "<input type='button' style='width:130px' value='" + "Change" + "' class='TransactionalButton btn btn-primary'";
            strTable += " onclick=\"return UpdateTableShift('" + costCenterId + "', '" + sourceId + "' );\"  />";
            strTable += "</fieldset> </div> </div> </div> </div>";

            strTable += "<div class='row' > <div class='col-md-12'> ";
            strTable += "<div style='font-weight: bold;  height:60px;' id='TableCleanInfo'>";

            //strTable += "<div style='float:left;padding-left:28px; padding-bottom:5px; margin-top:10px; margin-bottom:10px;'> <fieldset> <legend>Table Clean Info</legend>";

            //strTable += "<input type='button' style='width:150px' value='" + "Clean Table" + "' class='TransactionalButton btn btn-primary'";
            //strTable += " onclick=\"return UpdateTableStatus('" + costCenterId + "', '" + sourceId + "' );\"  />";
            //strTable += "</div>";
            //strTable += "</fieldset> </div> ";
            strTable += " </div> </div>";

            rtn.DataStr = strTable;

            return rtn;
        }
        [WebMethod]
        public static ReturnInfo UpdateRestaurantTableShift(int costCenterId, int oldTableId, int newTableId)
        {
            ReturnInfo rtninf = new ReturnInfo();

            HMUtility hmUtility = new HMUtility();
            KotBillMasterDA entityDA = new KotBillMasterDA();

            rtninf.IsSuccess = entityDA.UpdateRestaurantTableShift(costCenterId, oldTableId, newTableId);

            if (rtninf.IsSuccess)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo("Table Shift is Succeed.", AlertType.Success);
            }

            rtninf.Pk = costCenterId;

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo UpdateRestaurantTableStatus(int costCenterId, int tableId, int statusId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int lastModifiedBy = userInformationBO.UserInfoId;
            KotBillMasterDA entityDA = new KotBillMasterDA();

            rtninf.IsSuccess = entityDA.UpdateRestaurantTableStatus(costCenterId, tableId, statusId, lastModifiedBy);

            if (rtninf.IsSuccess)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo("Table Clean is Succeed.", AlertType.Success);
            }

            rtninf.Pk = costCenterId;
            return rtninf;
        }
        [WebMethod]
        public static ArrayList LoadTokenInfo(string sourceType, int costcenterId)
        {
            ArrayList arr = new ArrayList();
            RestaurantTokenDA billmasterda = new RestaurantTokenDA();
            HMUtility hmUtility = new HMUtility();

            //HttpContext.Current.Session["OrderType"] = "Token";

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string tokenNumber = billmasterda.GenarateRestaurantTokenNumber(costcenterId, userInformationBO.BearerId);
            //HttpContext.Current.Session["OrderCostcenterId"] = tokenNumber;

            arr.Add(new
            {
                TokenNumber = tokenNumber,
                SourceType = sourceType,
                OrderType = "Token",
                OrderCostcenterId = tokenNumber,
                CostcenterId = costcenterId
            });

            return arr;
        }
        [WebMethod]
        public static ReturnInfo RoomNumberShift(int costcenterId, int oldRoomId, string oldRoomNumber, int kotId, string newRoomNumber)
        {
            ReturnInfo rtninf = new ReturnInfo();

            KotBillMasterDA entityDA = new KotBillMasterDA();
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();

            KotBillMasterBO roomDetaisForRestaurant = new KotBillMasterBO();
            roomDetaisForRestaurant = entityDA.GetBillDetailInformationForRoomByRoomNumber(newRoomNumber);

            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(newRoomNumber);

            if (allocationBO.RegistrationId != 0 && roomDetaisForRestaurant.KotId == 0)
            {
                rtninf.IsSuccess = entityDA.UpdateRoomShiftForRestaurant(costcenterId, oldRoomId, oldRoomNumber, kotId, newRoomNumber, allocationBO.RegistrationId);
            }
            else
            {
                rtninf.IsSuccess = false;
            }

            if (rtninf.IsSuccess)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo("Room Shift Succeed.", AlertType.Success);
            }
            else
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo("Occupied Room Cannot be Changed. Try Un-Occupied Room.", AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static string LoadRoomAllocation()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            string fullContent = string.Empty, subContent = string.Empty, endContent = string.Empty;
            roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);

            List<KotBillMasterBO> entityBOList = new List<KotBillMasterBO>();
            KotBillMasterDA entityDA = new KotBillMasterDA();
            entityBOList = entityDA.GetBillDetailInformationForRoom();

            foreach (KotBillMasterBO kbMasterBO in entityBOList)
            {
                foreach (RoomNumberBO roomNumberBO in roomNumberListBO)
                {
                    if (roomNumberBO.RoomId == kbMasterBO.RoomId)
                    {
                        roomNumberBO.IsRestaurantItemExist = true;
                    }
                }
            }

            subContent = "<table class='table table-bordered table-condensed table-hover table-responsive'>";
            subContent += " <tbody> ";
            endContent = "</tr> </tbody> </table>";

            for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
            {
                if (roomNumberListBO[iRoomNumber].StatusId == 2)
                {
                    if (roomNumberListBO[iRoomNumber].IsRestaurantItemExist)
                    {
                        subContent += @"<tr> <td style='background:#D63CC4; color:#fff; font-weight:bold;'>";
                        subContent += "<a href='javascript:void(0)' style='display:block; text-decoration:none;' onclick=\"SearchRoomInfoFromList('" + roomNumberListBO[iRoomNumber].RoomNumber + "')\">" + roomNumberListBO[iRoomNumber].RoomNumber + "</a>";
                        subContent += @"</td></tr>";
                    }
                    else
                    {
                        subContent += @"<tr> <td style='background:#07BA28; color:#fff; font-weight:bold;'>";
                        subContent += "<a href='javascript:void(0)' style='display:block; text-decoration:none;' onclick=\"SearchRoomInfoFromList('" + roomNumberListBO[iRoomNumber].RoomNumber + "')\">" + roomNumberListBO[iRoomNumber].RoomNumber + "</a>";
                        subContent += @"</td></tr>";
                    }
                }
            }

            return (subContent + endContent);
        }
    }
}