using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmFoodDispatch : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            UserInformationDA userInformationDA = new UserInformationDA();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            string tempcostCenterId = string.Empty;
            string costCenterId = string.Empty;

            if (!string.IsNullOrWhiteSpace(Request.QueryString["CostCenterId"]))
            {
                costCenterId = Request.QueryString["CostCenterId"];

                Boolean status = userInformationDA.UpdateUserWorkingCostCenterInfo("SecurityUser", currentUserInformationBO.UserInfoId, Convert.ToInt32(costCenterId));
                if (status)
                {
                    currentUserInformationBO.WorkingCostCenterId = Convert.ToInt32(costCenterId);
                    Session.Add("UserInformationBOSession", currentUserInformationBO);
                }
                Response.Redirect("/POS/frmFoodDispatch.aspx");
            }
            //costCenterId = Convert.ToInt32(tempcostCenterId);
            hfCostCenterId.Value = currentUserInformationBO.WorkingCostCenterId.ToString();
            if (!IsPostBack)
            {
                //CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
                costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(currentUserInformationBO.WorkingCostCenterId);
                if (costCentreTabBO != null)
                {
                    string defaultView = string.Empty;
                    if (costCentreTabBO.DefaultView == "Token")
                    {
                        this.LoadDispatchInformation(Convert.ToInt32(hfCostCenterId.Value), "RestaurantToken");
                    }
                    else if (costCentreTabBO.DefaultView == "Table")
                    {
                        this.LoadDispatchInformation(Convert.ToInt32(hfCostCenterId.Value), "RestaurantTable");
                    }
                }
            }
        }
        private void LoadDispatchInformation(int costCenterId, string sourceName)
        {
            string fullContent = string.Empty;

            string headSummary = "Food Dispatch Information";

            string topPart = @"<div id='SearchPanel' class='block'>
                                <a href='#' class='block-heading' data-toggle='collapse'>";

            string topTemplatePart = @"</a>
                                <div class='block-body collapse in'>";
            string groupNamePart = string.Empty;

            string endTemplatePart = @"</div>
                            </div>";

            if (sourceName == "RestaurantToken")
            {
                KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();
                List<KotBillMasterBO> kotBillMasterList = new List<KotBillMasterBO>();

                headSummary = "Token Wise Food Dispatch Information";

                //roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);
                kotBillMasterList = kotBillMasterDA.GetRestaurantTokenInfoForFoodDispatch(costCenterId, sourceName);

                string subContent = string.Empty;

                for (int i = 0; i < kotBillMasterList.Count; i++)
                {
                    subContent += "<div class='DivRoomContainerHeight61'><div class='RoomOccupaiedDiv'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + kotBillMasterList[i].TokenNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + kotBillMasterList[i].KotId + "</div></div>";
                    //                    subContent += @"<div class='DivRoomContainerHeight61'><div class='RoomOccupaiedDiv'>
                    //                                            <div class='RoomOccupaiedDiv'>" + kotBillMasterList[i].TokenNumber + "</div></div><div style='display:none;' class='RoomNumberDiv'>" + kotBillMasterList[i].TokenNumber + "</div></div>";
                }

                fullContent += subContent;
            }
            else if (sourceName == "RestaurantTable")
            {
                TableManagementDA entityDA = new TableManagementDA();
                List<TableManagementBO> entityListBO = new List<TableManagementBO>();

                headSummary = "Table Wise Food Dispatch Information";

                entityListBO = entityDA.GetTableManagementInfo(costCenterId);

                string subContent = string.Empty;

                for (int i = 0; i < entityListBO.Count; i++)
                {
                    if (entityListBO[i].StatusId == 2)
                    {
                        subContent += @"<div class='DivRoomContainer'><div class='RestaurantTableBookedDiv'></div>
                                            <div class='RoomNumberDiv'>" + entityListBO[i].TableNumber + "</div></div></a>";
                    }
                }

                fullContent += subContent;
            }
            this.ltlFoodDispatchTemplate.Text = topPart + headSummary + topTemplatePart + fullContent + endTemplatePart;
        }

        [WebMethod]
        public static string LoadFoodDispatchPath(string kotId, string PageTitle)
        {
            int kId = Convert.ToInt32(kotId);
            HMCommonDA hmCommonDA = new HMCommonDA();
            List<CustomFieldBO> list = new List<CustomFieldBO>();
            list = hmCommonDA.GetCustomFieldList("FoodDispatch ");
            List<string[]> stringList = new List<string[]>();
            for (int i = 0; i < list.Count; i++)
            {
                string[] tokens = list[i].FieldValue.Split('~');
                stringList.Add(tokens);
            }

            string strTable = string.Empty;
            strTable += "<div style='padding:10px'>";
            for (int i = 0; i < stringList.Count; i++)
            {
                strTable += "<div style='float:left;padding-left:30px; padding-bottom:30px;width:150px'>";

                strTable += "<input type='button' style='width:150px' value='" + stringList[i][0] + "' class='TransactionalButton btn btn-primary'";

                if (stringList[i][0] == "Details")
                {
                    strTable += " onclick=\"return GetKotBillDetailListByKotId('" + kotId + "' );\"  />";
                }
                else
                {
                    strTable += " onclick=\" return DispatchFoodByKotId('" + kotId + "' );\"  />";
                }


                strTable += "</div>";
            }

            strTable += "</div>";


            return strTable;

        }

        [WebMethod]
        public static ReturnInfo DispatchFoodByKotId(string kotId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                int kId = Convert.ToInt32(kotId);
                KotBillDetailDA kotDetailDA = new KotBillDetailDA();
                bool status = kotDetailDA.UpdateKotDetailForFoodDispatchByKotId(kId);

                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo("Food Dispatched Successfully.", AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo("Food Dispatched Failed.", AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                
            }
            return rtninf;
        }
        [WebMethod]
        public static string GetKotBillDetailListByKotId(string kotId)
        {
            int kId = Convert.ToInt32(kotId);
            KotBillDetailDA kotDetailDA = new KotBillDetailDA();
            List<KotBillDetailBO> kotDetailList = new List<KotBillDetailBO>();
            kotDetailList = kotDetailDA.GetKotBillDetailListByKotId(kId);
            return GetUserDetailHtml(kotDetailList);
        }
        private static string GetUserDetailHtml(List<KotBillDetailBO> kotDetailList)
        {
            string strTable = "";
            strTable += "<table style='width:100%' cellspacing='0' cellpadding='4' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";

            strTable += "<th align='left' scope='col'>Item Name</th><th align='left' scope='col'>Quantity</th> <th align='left' scope='col'>Action</th></tr>";
            int counter = 0;

            foreach (KotBillDetailBO dr in kotDetailList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td align='left' style='width: 50%'>" + dr.ItemName + "</td>";
                strTable += "<td align='left' style='width: 30%'>" + dr.ItemUnit + "</td>";
                if (!dr.IsDispatch)
                {
                    strTable += "<td align='left' style='width: 20%'>";
                    strTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DispatchFoodByKotDetailId('" + dr.KotDetailId + "')\" alt='Edit Information' border='0' />";
                    strTable += "</td>";
                }
                if (dr.IsDispatch)
                {
                    strTable += "<td align='left' style='width: 20%'>";
                    strTable += "&nbsp;<img src='../Images/select.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return UnDispatchFoodByKotDetailId('" + dr.KotDetailId + "')\" alt='Edit Information' border='0' />";
                    strTable += "</td>";
                }


                strTable += "</tr>";
            }
            strTable += "<tr style='background-color:White;'>";
            strTable += "<td align='left' style='width: 50%'></td>";
            strTable += "<td align='left' style='width: 40%'></td>";
            strTable += "<td align='left' style='width: 10%'><input type='button' value='Back' class='btn btn-primary' id='btnBackToDecisionMaker' onClick=\"javascript:return GoBack()\"/></td>";
            strTable += "</tr>";
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }
        [WebMethod]
        public static bool DispatchFoodByKotDetailId(string type, string kotDetailId)
        {
            int kId = Convert.ToInt32(kotDetailId);
            KotBillDetailDA kotDetailDA = new KotBillDetailDA();
            bool status = kotDetailDA.UpdateKotDetailForFoodDispatchByKotDetailId(type, kId);
            return status;
        }
    }
}