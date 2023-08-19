using HotelManagement.Data;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Restaurant;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmGuestManagement : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
        //**************************** Handlers ********************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPermission();
                LoadCountryList();
                LoadCompanyCountryId();
                LoadProfession();
                LoadGuestTitle();
                hfToday.Value = DateTime.Now.Date.ToShortDateString();
                hfQueryStringRoomNumber.Value = string.Empty;

                string roomId = Request.QueryString["RoomId"];
                if (!string.IsNullOrEmpty(roomId))
                {
                    RoomNumberDA numberDA = new RoomNumberDA();
                    RoomNumberBO numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(roomId));
                    txtSrcRoomNumber.Text = numberBO.RoomNumber;
                    txtRoomNumberSearch.Text = numberBO.RoomNumber;
                    hfQueryStringRoomNumber.Value = numberBO.RoomNumber;
                }
            }
        }
        //************************ User Defined Function ***********************//
        private void CheckPermission()
        {
            hfIsSavePermission.Value = isSavePermission ? "1" : "0";
            hfIsUpdatePermission.Value = isUpdatePermission ? "1" : "0";
            hfIsDeletePermission.Value = isDeletePermission ? "1" : "0";
        }
        private void LoadCountryList()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CountriesBO> countryList = commonDA.GetAllCountries();
            this.ddlGuestCountry.DataSource = countryList;
            this.ddlGuestCountry.DataTextField = "CountryName";
            this.ddlGuestCountry.DataValueField = "CountryId";
            this.ddlGuestCountry.DataBind();
            string bangladesh = "19";
            ddlGuestCountry.SelectedValue = bangladesh;
        }
        private void LoadCompanyCountryId()
        {
            HMCommonSetupBO commonCountrySetupBO = new HMCommonSetupBO();
            commonCountrySetupBO = commonSetupDA.GetCommonConfigurationInfo("CompanyCountryId", "CompanyCountryId");

            if (commonCountrySetupBO != null)
            {
                hfDefaultCountryId.Value = commonCountrySetupBO.SetupValue;
            }
        }
        private void LoadProfession()
        {
            CommonProfessionDA professionDA = new CommonProfessionDA();
            List<CommonProfessionBO> entityBOList = new List<CommonProfessionBO>();
            entityBOList = professionDA.GetProfessionInfo();

            this.ddlProfessionId.DataSource = entityBOList;
            this.ddlProfessionId.DataTextField = "ProfessionName";
            this.ddlProfessionId.DataValueField = "ProfessionId";
            this.ddlProfessionId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProfessionId.Items.Insert(0, item);
        }
        public static string GridHeader()
        {
            string gridHead = string.Empty;

            gridHead += "<table id='ExpressCheckInDetailsGrid' class='table table-bordered table-condensed table-responsive'>" +
                         "       <thead>" +
                         "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                         "               <th style='width: 5%;' text-align:center;'>" +
                         "                   #" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   <input id='chkAllOutlet' type='checkbox' class='' value = 'chkStopCharge' onclick='CheckAll()' onkeydown='if (event.keyCode == 13) {return true;}' maxlength='50' style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' />" +
                         "               </th>" +
                         "               <th style='width: 85%;'>" +
                         "                   Outlet Name" +
                         "               </th>" +
                         "           </tr>" +
                         "       </thead>" +
                         "       <tbody>";

            return gridHead;
        }

        public static string GridHeaderForLinkedAmend()
        {
            string gridHead = string.Empty;

            gridHead += "<table id='LikedRoomsInfoGrid' class='table table-bordered table-condensed table-responsive'>" +
                         "       <thead>" +
                         "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                         "               <th style='width: 5%;' text-align:center;'>" +
                         "                   #" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   <input id='chkAllRooms' type='checkbox' class='' value = 'chkAllRoomsStopCharge' onclick='CheckAllRooms()' onkeydown='if (event.keyCode == 13) {return true;}' maxlength='50' style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' />" +
                         "               </th>" +
                         "               <th style='width: 15%;'>" +
                         "                   Link Rooms" +
                         "               </th>" +
                         "               <th style='width: 70%;'>" +
                         "                   Guest Name" +
                         "               </th>" +
                         "           </tr>" +
                         "       </thead>" +
                         "       <tbody>";

            return gridHead;
        }
        public static string GridHeaderForLinkedStopCharge()
        {
            string gridHead = string.Empty;

            gridHead += "<table id='LikedRoomsInfoGridStop' class='table table-bordered table-condensed table-responsive'>" +
                         "       <thead>" +
                         "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                         "               <th style='width: 5%;' text-align:center;'>" +
                         "                   #" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   <input id='chkAllRoomsStop' type='checkbox' class='' value = 'chkAllRoomsStopCharge' onclick='CheckAllRoomsStop()' onkeydown='if (event.keyCode == 13) {return true;}' maxlength='50' style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' />" +
                         "               </th>" +
                         "               <th style='width: 15%;'>" +
                         "                   Link Rooms" +
                         "               </th>" +
                         "               <th style='width: 70%;'>" +
                         "                   Guest Name" +
                         "               </th>" +
                         "           </tr>" +
                         "       </thead>" +
                         "       <tbody>";

            return gridHead;
        }
        public static string GridHeaderForMultiFromGuestSwap()
        {
            string gridHead = string.Empty;

            gridHead += "<table id='FromGuestSwapGrid' class='table table-bordered table-condensed table-responsive'>" +
                         "       <thead>" +
                         "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                         "               <th style='width: 10%;' text-align:center;'>" +
                         "                   #" +
                         "               </th>" +
                         "               <th style='width: 15%;'>" +
                         "                   <input id='chkAllFromGuestSwap' type='checkbox' class='' value = 'chkForAllRGuestSwap' onclick='CheckAllFromGuestsSwap()' onkeydown='if (event.keyCode == 13) {return true;}' maxlength='50' style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' />" +
                         "               </th>" +
                         //"               <th style='width: 15%;'>" +
                         //"                   Room Number" +
                         //"               </th>" +
                         "               <th style='width: 75%;'>" +
                         "                   Guest Name" +
                         "               </th>" +
                         "           </tr>" +
                         "       </thead>" +
                         "       <tbody>";

            return gridHead;
        }
        public static string GridHeaderForMultiToGuestSwap()
        {
            string gridHead = string.Empty;

            gridHead += "<table id='ToGuestSwapGrid' class='table table-bordered table-condensed table-responsive'>" +
                         "       <thead>" +
                         "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                         "               <th style='width: 10%;' text-align:center;'>" +
                         "                   #" +
                         "               </th>" +
                         "               <th style='width: 15%;'>" +
                         "                   <input id='chkAllToGuestSwap' type='checkbox' class='' value = 'chkForAllToGuestSwap' onclick='CheckAllToGuestsSwap()' onkeydown='if (event.keyCode == 13) {return true;}' maxlength='50' style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' />" +
                         "               </th>" +
                         //"               <th style='width: 15%;'>" +
                         //"                   Room Number" +
                         //"               </th>" +
                         "               <th style='width: 75%;'>" +
                         "                   Guest Name" +
                         "               </th>" +
                         "           </tr>" +
                         "       </thead>" +
                         "       <tbody>";

            return gridHead;
        }

        public static string GetHTMLGuestReferenceGridView(List<GuestPreferenceBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='GuestPreferenceInformation' width='100%' border: '1px solid #cccccc'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='center' scope='col'>Select</th><th align='left' scope='col'>Preference</th></tr>";
            //strTable += "<tr> <td colspan='2'>";
            //strTable += "<div style=\"height: 375px; overflow-y: scroll; text-align: left;\">";
            //strTable += "<table cellspacing='0' cellpadding='4' width='100%' id='GuestReference' >";
            int counter = 0;
            foreach (GuestPreferenceBO dr in List)
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

                strTable += "<td style='display:none;'>" + dr.PreferenceId + "</td>";
                strTable += "<td align='center' style='width: 20px'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.PreferenceId + "' name='" + dr.PreferenceName + "' value='" + dr.PreferenceId + "' >";
                strTable += "</td><td align='left' style='width: 138px'>" + dr.PreferenceName + "</td></tr>";
            }

            //strTable += "</table> </div> </td> </tr> </table>";
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Preference Available !</td></tr>";
            }

            strTable += "<div style='margin-top:12px;'>";
            strTable += "   <button type='button' onClick='javascript:return GetCheckedGuestPreference()' id='btnAddRoomId' class='btn btn-primary'> OK</button>";
            //strTable += "   <button type='button' onclick='javascript:return ClosePreferenceDialog()' id='btnAddRoomId' class='btn btn-primary'> Cancel</button>";
            strTable += "</div>";
            return strTable;
        }
        private void SetTab(string TabName)
        {
            if (TabName == "RoomLinkedInfo")
            {
                E.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            //else if (TabName == "EntryTab")
            //{
            //    A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
            //    B.Attributes.Add("class", "ui-state-default ui-corner-top");
            //}
        }

        private void LoadGuestTitle()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> titleList = new List<CustomFieldBO>();
            titleList = commonDA.GetCustomField("GuestTitle");

            ddlTitle.DataSource = titleList;
            ddlTitle.DataValueField = "FieldValue";
            ddlTitle.DataTextField = "Description";
            ddlTitle.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTitle.Items.Insert(0, item);
        }

        //************************ User Defined Web Method ********************//

        //[WebMethod]
        //public static ArrayList SearchAndLoadGuestInfo(string guestName, string companyName, string email, string phoneNumber, string nId, string doB, string passport)
        //{
        //    HMUtility hmUtility = new HMUtility();
        //    HMCommonDA commonDA = new HMCommonDA();
        //    GuestInformationDA guestDA = new GuestInformationDA();
        //    List<GuestInformationBO> guestList = new List<GuestInformationBO>();
        //    List<GuestInformationBO> distinctGuest = new List<GuestInformationBO>();

        //    guestList = guestDA.GetGuestInfoBySearchCriteria(guestName, companyName, email, phoneNumber, nId, doB, passport);

        //    distinctGuest = guestList.GroupBy(x => x.GuestId).Select(y => y.First()).Distinct().ToList();

        //    ArrayList arr = new ArrayList();

        //    arr.Add(new { GuestInfo = distinctGuest });

        //    return arr;

        //}
        [WebMethod]
        public static GridViewDataNPaging<GuestInformationBO, GridPaging> SearchAndLoadGuestInfo(string guestName, string companyName, string email, string phoneNumber, string nId, string doB, string passport, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            List<GuestInformationBO> guestList = new List<GuestInformationBO>();
            List<GuestInformationBO> distinctGuest = new List<GuestInformationBO>();
            //List<GuestInformationBO> distinctGuest2 = new List<GuestInformationBO>();  
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            GridViewDataNPaging<GuestInformationBO, GridPaging> myGridData = new GridViewDataNPaging<GuestInformationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            guestList = guestDA.GetGuestInfoBySearchCriteria(guestName, companyName, email, phoneNumber, nId, doB, passport, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            //distinctGuest2 = guestList.GroupBy(x => x.GuestName).Select(y => y.First()).ToList();

            distinctGuest = guestList.GroupBy(x => x.GuestId).Select(y => y.First()).Distinct().ToList();

            //ArrayList arr = new ArrayList(); bankInfoList.GroupBy(test => test.BankName).Select(group => group.First()).ToList()

            //arr.Add(new { GuestInfo = distinctGuest });

            //return arr;
            myGridData.GridPagingProcessing(distinctGuest, totalRecords);

            return myGridData;

        }
        [WebMethod]
        public static GuestInformationBO GetGuestFromHotelGuestInfo(int guestId)
        {
            GuestInformationDA guestDA = new GuestInformationDA();
            GuestInformationBO guest = new GuestInformationBO();

            guest = guestDA.GetGuestInformationByGuestId(guestId);
            return guest;
        }

        [WebMethod]
        public static string GetNationality(int countryId)
        {
            CountriesBO country = new CountriesBO();

            try
            {
                HMCommonDA commonDa = new HMCommonDA();
                country = commonDa.GetCountriesById(countryId);

            }
            catch (Exception ex)
            {
                country.Nationality = string.Empty;
            }

            return country.Nationality;
        }

        //Multi Guest Swap
        [WebMethod]
        public static ArrayList GetAllFromGuestByRoomNumber(string fromRoomNo)
        {
            List<GuestInformationBO> guestList = new List<GuestInformationBO>();
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            ArrayList list = new ArrayList();
            int rowCount = 0;

            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(fromRoomNo);
            if (allocationBO.RegistrationId > 0)
            {
                guestList = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);

            }
            string grid = string.Empty, tr = string.Empty;
            if (guestList.Count > 0)
            {
                foreach (var item in guestList)
                {
                    if (rowCount % 2 == 0)
                    {
                        tr += "<tr style='background-color:#FFFFFF;'>";
                    }
                    else
                    {
                        tr += "<tr style='background-color:#E3EAEB;'>";
                    }
                    tr += "<td style='width:10%; text-align:center;'>" + (rowCount + 1).ToString() + "</td>";
                    tr += "<td style='width:15%; text-align:center;'> <input type='checkbox' class='' TabIndex=" + (item.RegistrationId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                    //tr += "<td style='width:15%; text-align:left;'>" + item.RoomNumber + "</td>";
                    tr += "<td style='width:75%; text-align:left;'>" + item.GuestName + "</td>";
                    tr += "<td style='display:none'>" + item.RegistrationId + "</td>";
                    tr += "<td style='display:none'>" + item.GuestId + "</td>";
                    tr += "</tr>";

                    rowCount++;
                }
                grid += GridHeaderForMultiFromGuestSwap() + tr + "</tbody> </table>";
            }
            list.Add(grid);

            return list;
        }
        [WebMethod]
        public static ArrayList GetAllToGuestByRoomNumber(string toRoomNo)
        {
            List<GuestInformationBO> guestList = new List<GuestInformationBO>();
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            ArrayList list = new ArrayList();
            int rowCount = 0;

            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(toRoomNo);
            if (allocationBO.RegistrationId > 0)
            {
                guestList = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);

            }
            string grid = string.Empty, tr = string.Empty;
            if (guestList.Count > 0)
            {
                foreach (var item in guestList)
                {
                    if (rowCount % 2 == 0)
                    {
                        tr += "<tr style='background-color:#FFFFFF;'>";
                    }
                    else
                    {
                        tr += "<tr style='background-color:#E3EAEB;'>";
                    }
                    tr += "<td style='width:10%; text-align:center;'>" + (rowCount + 1).ToString() + "</td>";
                    tr += "<td style='width:15%; text-align:center;'> <input type='checkbox' class='' TabIndex=" + (item.RegistrationId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                    //tr += "<td style='width:15%; text-align:left;'>" + item.RoomNumber + "</td>";
                    tr += "<td style='width:75%; text-align:left;'>" + item.GuestName + "</td>";
                    tr += "<td style='display:none'>" + item.RegistrationId + "</td>";
                    tr += "<td style='display:none'>" + item.GuestId + "</td>";
                    tr += "</tr>";

                    rowCount++;
                }
                grid += GridHeaderForMultiToGuestSwap() + tr + "</tbody> </table>";
            }
            list.Add(grid);

            return list;
        }
        [WebMethod]
        public static ArrayList GetMultipleGuestFromRoom(string fromRoomNumber)
        {
            List<GuestInformationBO> guestList = new List<GuestInformationBO>();
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            ArrayList list = new ArrayList();

            //from room 
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(fromRoomNumber);
            if (allocationBO.RegistrationId > 0)
            {
                guestList = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);

                int count = guestList.Count;
                for (int i = 0; i < count; i++)
                {
                    list.Add(new ListItem(
                        guestList[i].GuestName.ToString(),
                                            guestList[i].GuestId.ToString()

                                             ));
                }
            }
            else
            {
                list.Add(allocationBO);
            }
            return list;
        }
        [WebMethod]
        public static ArrayList GetMultipleGuestToRoom(string toRoomNumber)
        {
            List<GuestInformationBO> guestList = new List<GuestInformationBO>();
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            ArrayList list = new ArrayList();

            //to room 
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(toRoomNumber);
            if (allocationBO.RegistrationId > 0)
            {
                guestList = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);

                int count = guestList.Count;
                for (int i = 0; i < count; i++)
                {
                    list.Add(new ListItem(
                        guestList[i].GuestName.ToString(),
                                            guestList[i].GuestId.ToString()

                                             ));
                }
            }
            else
            {
                list.Add(allocationBO);
            }
            return list;
        }
        [WebMethod]
        public static ArrayList GetRoomSwapInformationByRoomNumber(string roomNumber)
        {
            KotBillMasterDA entityDA = new KotBillMasterDA();
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomAlocationBO allocationBONew = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            GuestInformationBO guestInformationBO = new GuestInformationBO();
            ArrayList arr = new ArrayList();

            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
            if (allocationBO.RegistrationId > 0)
            {
                list = guestDA.GetGuestInformationByRegistrationIdForSwap(allocationBO.RegistrationId);
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
                        list[0].CostCenterId = 1; //roomDetaisForRestaurant.CostCenterId == 0 ? costcenterId : roomDetaisForRestaurant.CostCenterId;
                        list[0].KotId = 0; //roomDetaisForRestaurant.KotId;
                        guestInformationBO = list[0];


                        arr.Add(list[0]);
                    }
                }
            }
            else
            {
                arr.Add(allocationBO);
            }
            return arr;
        }
        [WebMethod]
        public static ArrayList GetRoomSwapInformationForRoomChangeByRoomNumber(int costcenterId, string roomNumber, string newRoomNumber)
        {
            KotBillMasterDA entityDA = new KotBillMasterDA();
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomAlocationBO allocationBONew = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            GuestInformationBO guestInformationBO = new GuestInformationBO();
            ArrayList arr = new ArrayList();

            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
            if (allocationBO.RegistrationId > 0)
            {
                list = guestDA.GetGuestInformationByRegistrationIdForSwap(allocationBO.RegistrationId);
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
                        list[0].CostCenterId = 1; //roomDetaisForRestaurant.CostCenterId == 0 ? costcenterId : roomDetaisForRestaurant.CostCenterId;
                        list[0].KotId = 0; //roomDetaisForRestaurant.KotId;
                        guestInformationBO = list[0];


                        arr.Add(list[0]);
                    }
                }
            }
            else
            {
                arr.Add(allocationBO);
            }


            //KotBillMasterBO roomDetaisForRestaurant = new KotBillMasterBO();
            //roomDetaisForRestaurant = entityDA.GetBillDetailInformationForRoomByRoomNumber(roomNumber);




            //ArrayList arr = new ArrayList();
            //arr.Add(list[0]);

            allocationBONew = registrationDA.GetActiveRegistrationInfoByRoomNumber(newRoomNumber);
            if (allocationBONew.RegistrationId > 0)
            {
                list = guestDA.GetGuestInformationByRegistrationIdForSwap(allocationBONew.RegistrationId);

                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        list[0].RoomRate = allocationBONew.RoomRate;
                        list[0].CurrencyTypeHead = allocationBONew.CurrencyTypeHead;
                        list[0].ExpectedCheckOutDate = allocationBONew.ExpectedCheckOutDate;
                        list[0].RoomId = allocationBONew.RoomId;
                        list[0].RoomNumber = allocationBONew.RoomNumber;
                        list[0].RoomType = allocationBONew.RoomType;
                        list[0].CostCenterId = 1; //roomDetaisForRestaurant.CostCenterId == 0 ? costcenterId : roomDetaisForRestaurant.CostCenterId;
                        list[0].KotId = 0; //roomDetaisForRestaurant.KotId;
                        guestInformationBO = list[0];

                        arr.Add(list[0]);
                    }
                }
            }
            else
            {
                arr.Add(allocationBONew);
            }


            //arr.Add(list[0]);
            return arr;
        }
        [WebMethod]
        public static ArrayList GetRoomSwapInformationForRoomChangeByRoomNumber_GuestId(string roomNumber, int guestId)
        {
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            //RoomAlocationBO allocationBONew = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            GuestInformationBO guestInformationBO = new GuestInformationBO();
            ArrayList arr = new ArrayList();
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
            if (allocationBO.RegistrationId > 0)
            {
                list = guestDA.GetGuestInformationByRegistrationIdForSwap(allocationBO.RegistrationId);
                var searchResult = list.Where(data => data.GuestId == guestId).ToList();

                if (searchResult != null)
                {
                    if (searchResult.Count > 0)
                    {
                        searchResult[0].RoomRate = allocationBO.RoomRate;
                        searchResult[0].CurrencyTypeHead = allocationBO.CurrencyTypeHead;
                        searchResult[0].ExpectedCheckOutDate = allocationBO.ExpectedCheckOutDate;
                        searchResult[0].RoomId = allocationBO.RoomId;
                        searchResult[0].RoomNumber = allocationBO.RoomNumber;
                        searchResult[0].RoomType = allocationBO.RoomType;
                        searchResult[0].CostCenterId = 1; //roomDetaisForRestaurant.CostCenterId == 0 ? costcenterId : roomDetaisForRestaurant.CostCenterId;
                        searchResult[0].KotId = 0; //roomDetaisForRestaurant.KotId;
                        guestInformationBO = searchResult[0];


                        arr.Add(searchResult[0]);
                    }
                }
                else
                {
                    arr.Add(allocationBO);
                }
            }

            return arr;
        }
        [WebMethod]
        public static string SaveUpdateRoomSwapInformation(int fromRegistrationId, int toRegistrationId, int fromGuestId, int toGuestId)
        {
            string message = "";
            HMUtility hmUtility = new HMUtility();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            Boolean status = registrationDA.UpdateRoomSwapInformation(fromRegistrationId, toRegistrationId, fromGuestId, toGuestId);
            if (status)
            {
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), "Guest Swap", fromRegistrationId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Guest Swap Information Between RegistrationId: " + fromRegistrationId.ToString() + " AND " + toRegistrationId.ToString());

                message = "Guest Swap Successfull.";
            }
            else
            {
                message = "Guest Swap Failed.";
            }

            return message;
        }
        [WebMethod]
        public static string SaveUpdateRoomSwapInformationMulti(int fromRegId, int toRegId, List<GuestInformationBO> FromGuestList, List<GuestInformationBO> ToGuestList)
        {
            string message = "";
            HMUtility hmUtility = new HMUtility();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            string fromGuestIdList = string.Empty, toGuestIdList = string.Empty;

            foreach (var item in FromGuestList)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(fromGuestIdList))
                    {
                        fromGuestIdList = fromGuestIdList + "," + item.GuestId.ToString();
                    }
                    else
                    {
                        fromGuestIdList = item.GuestId.ToString();
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            foreach (var item in ToGuestList)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(toGuestIdList))
                    {
                        toGuestIdList = toGuestIdList + "," + item.GuestId.ToString();
                    }
                    else
                    {
                        toGuestIdList = item.GuestId.ToString();
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            Boolean status = registrationDA.UpdateRoomSwapInformationMultiGuest(fromRegId, toRegId, fromGuestIdList, toGuestIdList);

            if (status)
            {
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), "Guest Swap", fromRegId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Guest Swap Information Between RegistrationId: " + fromRegId.ToString() + " AND " + toRegId.ToString());

                message = "Guest Swap Successfull.";
            }
            else
            {
                message = "Guest Swap Failed.";
            }

            return message;
        }
        [WebMethod]
        public static ArrayList GetRoomAmendStayByRoomNumber(string roomNumber)
        {
            ArrayList arr = new ArrayList();
            KotBillMasterDA entityDA = new KotBillMasterDA();
            RoomAlocationBO allocationBO = new RoomAlocationBO();

            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();

            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            GuestInformationBO guestInformationBO = new GuestInformationBO();

            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
            if (allocationBO.RegistrationId > 0)
            {
                list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);
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
                        list[0].CostCenterId = 1; //roomDetaisForRestaurant.CostCenterId == 0 ? costcenterId : roomDetaisForRestaurant.CostCenterId;
                        list[0].KotId = 0; //roomDetaisForRestaurant.KotId;
                        guestInformationBO = list[0];
                    }
                }

                arr.Add(list[0]);
            }
            else
            {
                arr.Add(allocationBO);
            }


            return arr;
        }
        [WebMethod]
        public static string RoomAmendStayInformation(int registrationId, string expectedCheckOutDate)
        {
            string message = "";
            HMUtility hmUtility = new HMUtility();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            DateTime depDate = hmUtility.GetDateTimeFromString(expectedCheckOutDate, userInformationBO.ServerDateFormat);

            // // -------------Room Availability Checking ------------------------------
            DateTime dateTime = DateTime.Now;
            DateTime StartDate = dateTime;
            DateTime EndDate = dateTime;

            int roomId = 0;
            int roomTypeId = 0;
            int isReservation = 0;
            int reservationId = 0;

            Boolean isChangedRegistrationData = true;
            if (registrationId > 0)
            {
                RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(registrationId);
                if (roomRegistrationBO != null)
                {
                    if (roomRegistrationBO.RegistrationId > 0)
                    {
                        if (roomRegistrationBO.ReservationId > 0)
                        {
                            isReservation = 1;                            
                        }

                        roomId = roomRegistrationBO.RoomId;
                        roomTypeId = roomRegistrationBO.EntitleRoomType;
                        reservationId = roomRegistrationBO.ReservationId;
                        StartDate = roomRegistrationBO.ArriveDate.Date;
                        EndDate = depDate;

                        isChangedRegistrationData = false;
                        if (roomRegistrationBO.ArriveDate.Date != StartDate.Date)
                        {
                            isChangedRegistrationData = true;
                        }

                        if (roomRegistrationBO.ExpectedCheckOutDate.Date <= EndDate.Date)
                        {
                            isChangedRegistrationData = true;
                        }

                        if (isChangedRegistrationData)
                        {
                            StartDate = roomRegistrationBO.ExpectedCheckOutDate.Date;
                        }
                    }
                }
            }

            bool flag = true;
            if (isChangedRegistrationData)
            {
                RoomNumberDA roomNumberDA = new RoomNumberDA();
                List<RoomNumberBO> list = new List<RoomNumberBO>();
                list = roomNumberDA.GetAvailableRoomNumberInformation(roomTypeId, isReservation, StartDate, EndDate, reservationId);
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        List<RoomNumberBO> roomExistList = new List<RoomNumberBO>();
                        bool hasData = list.Any(cus => cus.RoomId == roomId);
                        if (!hasData)
                        {
                            message = "Your Entered Room is not available between Check-In Date and Departure Date.";
                            flag = false;
                        }
                    }
                }
            }

            if (flag)
            {
                Boolean status = registrationDA.UpdateRoomAmendStayInformation(registrationId, depDate);
                if (status)
                {
                    message = "Room Amend Stay Successfull.";
                }
                else
                {
                    message = "Room Amend Stay Failed.";
                }
            }

            return message;
        }
        [WebMethod]
        public static string RoomAmendStayInformationLinked(List<RoomStopChargePostingDetailsBO> RoomStopChargePostingRegIds, string expectedCheckOutDate)
        {
            string message = "", regIdQuery = string.Empty;
            HMUtility hmUtility = new HMUtility();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            DateTime depDate = hmUtility.GetDateTimeFromString(expectedCheckOutDate, userInformationBO.ServerDateFormat);
            if (RoomStopChargePostingRegIds.Count > 0)
            {
                foreach (var item in RoomStopChargePostingRegIds)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(regIdQuery))
                        {
                            regIdQuery = regIdQuery + "," + item.RegistrationId.ToString();
                        }
                        else
                        {
                            regIdQuery = item.RegistrationId.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        //CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                        throw ex;
                    }
                }

            }
            Boolean status = registrationDA.UpdateRoomAmendStayInformationLinked(regIdQuery, depDate, userInformationBO.UserInfoId);
            if (status)
            {
                message = "Room Amend Stay Successfull.";
            }
            else
            {
                message = "Room Amend Stay Failed.";
            }

            return message;
        }
        [WebMethod]
        public static ArrayList GetRoomAmendStayForLinkedRooms(long masterId)
        {
            ArrayList arr = new ArrayList();
            int rowCount = 0;
            List<HotelLinkedRoomDetailsBO> detailRoomList = new List<HotelLinkedRoomDetailsBO>();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomAlocationBO roomBO = new RoomAlocationBO();
            List<RoomAlocationBO> rooms = new List<RoomAlocationBO>();

            detailRoomList = roomRegistrationDA.GetDetailRoomsByMasterId(masterId);
            foreach (var item in detailRoomList)
            {
                roomBO = roomRegistrationDA.GetRegistrationInfoByRegistrationIdNEW(item.RegistrationId);
                if (roomBO.RegistrationId > 0)
                {
                    rooms.Add(roomBO);
                }

            }
            string grid = string.Empty, tr = string.Empty;
            int regId = 0;
            if (rooms != null)
            {
                if (rooms.Count > 0)
                {
                    foreach (var item in rooms)
                    {
                        if (rowCount % 2 == 0)
                        {
                            tr += "<tr style='background-color:#FFFFFF;'>";
                        }
                        else
                        {
                            tr += "<tr style='background-color:#E3EAEB;'>";
                        }
                        tr += "<td style='width:5%; text-align:center;'>" + (rowCount + 1).ToString() + "</td>";

                        //if (item.IsStopChargePosting == true)
                        //{
                        //    tr += "<td style='width:10%; text-align:center;'> <input type='checkbox' class='form-control' TabIndex=" + (item.RegistrationId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        //}
                        //else
                        //{
                        //    tr += "<td style='width:10%; text-align:center;'> <input type='checkbox' checked class='form-control' TabIndex=" + (item.RegistrationId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        //    regId = item.RegistrationId;
                        //}
                        tr += "<td style='width:10%; text-align:center;'> <input type='checkbox' class='' TabIndex=" + (item.RegistrationId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        tr += "<td style='width:15%; text-align:left;'>" + item.RoomNumber + "</td>";
                        tr += "<td style='width:70%; text-align:left;'>" + item.GuestName + "</td>";
                        tr += "<td style='display:none'>" + item.RegistrationId + "</td>";
                        tr += "</tr>";

                        rowCount++;
                    }
                    grid += GridHeaderForLinkedAmend() + tr + "</tbody> </table>";
                }
                arr.Add(grid);
            }

            return arr;
        }
        // Stop CHarge Posting ------------------------
        [WebMethod]
        public static ArrayList GetMasterLinkRoomForddl()
        {
            ArrayList list = new ArrayList();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomAlocationBO detailRooms = new RoomAlocationBO();
            RoomAlocationBO masterRoom = new RoomAlocationBO();

            List<HotelLinkedRoomMasterBO> allLMasterRoomList = new List<HotelLinkedRoomMasterBO>();

            allLMasterRoomList = roomRegistrationDA.GetAllMasterRoom();

            if (allLMasterRoomList.Count > 0)
            {
                int count = allLMasterRoomList.Count;
                for (int i = 0; i < count; i++)
                {
                    list.Add(new ListItem(
                        allLMasterRoomList[i].LinkName.ToString(),
                                            allLMasterRoomList[i].Id.ToString()

                                             ));
                }
            }

            return list;
        }
        [WebMethod]
        public static ArrayList GetRoomStopChargePostingForLinkedRooms(long masterId)
        {
            ArrayList arr = new ArrayList();
            int rowCount = 0;
            int rowCount1 = 0;
            List<HotelLinkedRoomDetailsBO> detailRoomList = new List<HotelLinkedRoomDetailsBO>();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomAlocationBO roomBO = new RoomAlocationBO();
            List<RoomAlocationBO> rooms = new List<RoomAlocationBO>();
            var flag = 0;
            int regId = 0;

            detailRoomList = roomRegistrationDA.GetDetailRoomsByMasterId(masterId);

            foreach (var item in detailRoomList)
            {
                roomBO = roomRegistrationDA.GetRegistrationInfoByRegistrationIdNEW(item.RegistrationId);
                if (roomBO.RegistrationId > 0)
                {
                    rooms.Add(roomBO);
                }

            }
            string grid = string.Empty, tr = string.Empty;
            string grid1 = string.Empty, tr1 = string.Empty;
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabListBO = new List<CostCentreTabBO>();

            if (rooms != null)
            {
                if (rooms.Count > 0)
                {
                    foreach (var item in rooms)
                    {
                        if (rowCount % 2 == 0)
                        {
                            tr += "<tr style='background-color:#FFFFFF;'>";
                        }
                        else
                        {
                            tr += "<tr style='background-color:#E3EAEB;'>";
                        }
                        tr += "<td style='width:5%; text-align:center;'>" + (rowCount + 1).ToString() + "</td>";

                        if (item.IsStopChargePosting == true)
                        {
                            tr += "<td style='width:10%; text-align:center;'> <input type='checkbox' class='' TabIndex=" + (item.RegistrationId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        }
                        else
                        {
                            tr += "<td style='width:10%; text-align:center;'> <input type='checkbox' checked class='' TabIndex=" + (item.RegistrationId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                            regId = item.RegistrationId;
                        }
                        //tr += "<td style='width:10%; text-align:center;'> <input type='checkbox' class='form-control' TabIndex=" + (item.RegistrationId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                        tr += "<td style='width:15%; text-align:left;'>" + item.RoomNumber + "</td>";
                        tr += "<td style='width:70%; text-align:left;'>" + item.GuestName + "</td>";
                        tr += "<td style='display:none'>" + item.RegistrationId + "</td>";
                        tr += "</tr>";

                        rowCount++;
                    }
                    grid += GridHeaderForLinkedStopCharge() + tr + "</tbody> </table>";
                }


                costCentreTabListBO = costCentreTabDA.GetAllRestaurantTypeCostCentreInfoForStopChargePosting(regId);
                if (costCentreTabListBO != null)
                {
                    if (costCentreTabListBO.Count > 0)
                    {
                        foreach (CostCentreTabBO stck in costCentreTabListBO)
                        {
                            if (rowCount1 % 2 == 0)
                            {
                                tr1 += "<tr style='background-color:#FFFFFF;'>";
                            }
                            else
                            {
                                tr1 += "<tr style='background-color:#E3EAEB;'>";
                            }

                            tr1 += "<td style='width:5%; text-align:center;'>" + (rowCount1 + 1).ToString() + "</td>";
                            if (stck.StopChargePostingStatus == 0)
                            {
                                tr1 += "<td style='width:10%; text-align:center;'> <input type='checkbox' class='' TabIndex=" + (stck.CostCenterId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                            }
                            else
                            {
                                tr1 += "<td style='width:10%; text-align:center;'> <input type='checkbox' checked class='' TabIndex=" + (stck.CostCenterId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                            }
                            tr1 += "<td style='width:85%; text-align:left;'>" + stck.CostCenter + "</td>";
                            tr1 += "<td style='display:none'>" + stck.CostCenterId + "</td>";
                            tr1 += "</tr>";

                            rowCount1++;
                        }

                        grid1 += GridHeader() + tr1 + "</tbody> </table>";
                    }
                }
                arr.Add(grid);
                arr.Add(grid1);
            }
            //else
            //{

            //}


            return arr;
        }
        [WebMethod]
        public static ArrayList GetRoomStopChargePostingByRoomNumber(string roomNumber)
        {
            int rowCount = 0;
            ArrayList arr = new ArrayList();
            KotBillMasterDA entityDA = new KotBillMasterDA();
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            GuestInformationBO guestInformationBO = new GuestInformationBO();

            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
            if (allocationBO != null)
            {
                if (allocationBO.RegistrationId > 0)
                {
                    list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);
                    guestInformationBO.RoomId = 0;

                    string grid = string.Empty, tr = string.Empty;
                    CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                    List<CostCentreTabBO> costCentreTabListBO = new List<CostCentreTabBO>();
                    costCentreTabListBO = costCentreTabDA.GetAllRestaurantTypeCostCentreInfoForStopChargePosting(allocationBO.RegistrationId);

                    if (costCentreTabListBO != null)
                    {
                        if (costCentreTabListBO.Count > 0)
                        {
                            foreach (CostCentreTabBO stck in costCentreTabListBO)
                            {
                                if (rowCount % 2 == 0)
                                {
                                    tr += "<tr style='background-color:#FFFFFF;'>";
                                }
                                else
                                {
                                    tr += "<tr style='background-color:#E3EAEB;'>";
                                }

                                tr += "<td style='width:5%; text-align:center;'>" + (rowCount + 1).ToString() + "</td>";
                                if (stck.StopChargePostingStatus == 0)
                                {
                                    tr += "<td style='width:10%; text-align:center;'> <input type='checkbox' class='' TabIndex=" + (stck.CostCenterId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                                }
                                else
                                {
                                    tr += "<td style='width:10%; text-align:center;'> <input type='checkbox' checked class='' TabIndex=" + (stck.CostCenterId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                                }
                                tr += "<td style='width:85%; text-align:left;'>" + stck.CostCenter + "</td>";
                                tr += "<td style='display:none'>" + stck.CostCenterId + "</td>";
                                tr += "</tr>";

                                rowCount++;
                            }

                            grid += GridHeader() + tr + "</tbody> </table>";
                        }
                    }

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
                            list[0].CostCenterId = 1;
                            list[0].KotId = 0;
                            guestInformationBO = list[0];
                        }
                    }


                    arr.Add(list[0]);
                    arr.Add(grid);

                }
                else
                {
                    arr.Add(allocationBO);
                }
            }
            return arr;
        }
        [WebMethod]
        public static ArrayList BillPendingReleaseProcess(string roomNumber)
        {
            int rowCount = 0;
            ArrayList arr = new ArrayList();
            KotBillMasterDA entityDA = new KotBillMasterDA();
            RoomAlocationBO registrationBO = new RoomAlocationBO();
            //RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            //List<GuestInformationBO> list = new List<GuestInformationBO>();
            //GuestInformationDA guestDA = new GuestInformationDA();
            //GuestInformationBO guestInformationBO = new GuestInformationBO();

            registrationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
            if (registrationBO != null)
            {
                if (registrationBO.RegistrationId > 0)
                {
                    Boolean status = registrationDA.BillPendingReleaseProcess(registrationBO.RegistrationNumber);

                    //list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);
                    //guestInformationBO.RoomId = 0;

                    //string grid = string.Empty, tr = string.Empty;
                    //CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                    //List<CostCentreTabBO> costCentreTabListBO = new List<CostCentreTabBO>();
                    //costCentreTabListBO = costCentreTabDA.GetAllRestaurantTypeCostCentreInfoForStopChargePosting(allocationBO.RegistrationId);

                    //if (costCentreTabListBO != null)
                    //{
                    //    if (costCentreTabListBO.Count > 0)
                    //    {
                    //        foreach (CostCentreTabBO stck in costCentreTabListBO)
                    //        {
                    //            if (rowCount % 2 == 0)
                    //            {
                    //                tr += "<tr style='background-color:#FFFFFF;'>";
                    //            }
                    //            else
                    //            {
                    //                tr += "<tr style='background-color:#E3EAEB;'>";
                    //            }

                    //            tr += "<td style='width:5%; text-align:center;'>" + (rowCount + 1).ToString() + "</td>";
                    //            if (stck.StopChargePostingStatus == 0)
                    //            {
                    //                tr += "<td style='width:10%; text-align:center;'> <input type='checkbox' class='' TabIndex=" + (stck.CostCenterId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                    //            }
                    //            else
                    //            {
                    //                tr += "<td style='width:10%; text-align:center;'> <input type='checkbox' checked class='' TabIndex=" + (stck.CostCenterId).ToString() + " style='width:100%; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                    //            }
                    //            tr += "<td style='width:85%; text-align:left;'>" + stck.CostCenter + "</td>";
                    //            tr += "<td style='display:none'>" + stck.CostCenterId + "</td>";
                    //            tr += "</tr>";

                    //            rowCount++;
                    //        }

                    //        grid += GridHeader() + tr + "</tbody> </table>";
                    //    }
                    //}

                    //if (list != null)
                    //{
                    //    if (list.Count > 0)
                    //    {
                    //        list[0].RoomRate = allocationBO.RoomRate;
                    //        list[0].CurrencyTypeHead = allocationBO.CurrencyTypeHead;
                    //        list[0].ExpectedCheckOutDate = allocationBO.ExpectedCheckOutDate;
                    //        list[0].RoomId = allocationBO.RoomId;
                    //        list[0].RoomNumber = allocationBO.RoomNumber;
                    //        list[0].RoomType = allocationBO.RoomType;
                    //        list[0].CostCenterId = 1;
                    //        list[0].KotId = 0;
                    //        guestInformationBO = list[0];
                    //    }
                    //}


                    //arr.Add(list[0]);
                    //arr.Add(grid);

                }
                else
                {
                    arr.Add(registrationBO);
                }
            }
            return arr;
        }
        [WebMethod]
        public static ArrayList RoomBillPostingProcess(string processType, string roomNumber)
        {
            Boolean status = true;
            ArrayList arr = new ArrayList();
            HMUtility hmUtility = new HMUtility();
            KotBillMasterDA entityDA = new KotBillMasterDA();
            RoomAlocationBO registrationBO = new RoomAlocationBO();            
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            registrationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
            if (registrationBO != null)
            {
                if (registrationBO.RegistrationId > 0)
                {
                    if (processType == "1") // No Show Bill Posting
                    {
                        status = registrationDA.RoomNoShowBillPostingProcess(registrationBO.RegistrationId, userInformationBO.UserInfoId);
                    }
                    else if (processType == "2")  // Advance Room Bill Posting
                    { 
                        status = registrationDA.AdvanceRoomBillPostingProcess(registrationBO.RegistrationId, userInformationBO.UserInfoId);
                    }
                }
                else
                {
                    arr.Add(registrationBO);
                }
            }
            return arr;
        }
        [WebMethod]
        public static String SaveUpdateStopChargePosting(int registrationId, List<RoomStopChargePostingDetailsBO> RoomStopChargePostingDetailsBO)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninfo = new ReturnInfo();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string customQuery = string.Empty;
            string regIdQuery = string.Empty;
            foreach (RoomStopChargePostingDetailsBO rowDetails in RoomStopChargePostingDetailsBO)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(customQuery))
                    {
                        customQuery = customQuery + "," + rowDetails.CostCenterId.ToString();
                    }
                    else
                    {
                        customQuery = rowDetails.CostCenterId.ToString();
                    }
                }
                catch (Exception ex)
                {
                    //CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    throw ex;
                }
            }
            regIdQuery = registrationId.ToString();

            string message = "";
            Boolean delStatus = roomRegistrationDA.SaveUpdateRoomStopChargePosting(regIdQuery, customQuery, userInformationBO.UserInfoId);
            if (delStatus)
            {
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), "Room Stop Charge Posting", registrationId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Room Stop Charge Posting for the RegistrationId: " + registrationId.ToString());

                message = "Room Stop Charge Posting Successfull.";
            }
            else
            {
                message = "Room Stop Charge Posting Failed.";
            }

            return message;
        }
        [WebMethod]
        public static String SaveUpdateStopChargePostingLink(List<RoomStopChargePostingDetailsBO> RoomStopChargePostingRegIds, List<RoomStopChargePostingDetailsBO> RoomStopChargePostingDetailsBO, List<RoomStopChargePostingDetailsBO> NotSelectedRegIds)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninfo = new ReturnInfo();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string customQuery = string.Empty;
            string regIdQuery = string.Empty;
            string NotSelectedRegIdQuery = string.Empty;
            string message = "";
            foreach (RoomStopChargePostingDetailsBO rowDetails in RoomStopChargePostingDetailsBO)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(customQuery))
                    {
                        customQuery = customQuery + "," + rowDetails.CostCenterId.ToString();
                    }
                    else
                    {
                        customQuery = rowDetails.CostCenterId.ToString();
                    }
                }
                catch (Exception ex)
                {
                    //CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    throw ex;
                }
            }
            if (RoomStopChargePostingRegIds.Count > 0)
            {
                foreach (var item in RoomStopChargePostingRegIds)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(regIdQuery))
                        {
                            regIdQuery = regIdQuery + "," + item.RegistrationId.ToString();
                        }
                        else
                        {
                            regIdQuery = item.RegistrationId.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        //CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                        throw ex;
                    }
                }

            }
            if (NotSelectedRegIds.Count > 0)
            {
                foreach (var item in NotSelectedRegIds)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(NotSelectedRegIdQuery))
                        {
                            NotSelectedRegIdQuery = NotSelectedRegIdQuery + "," + item.RegistrationId.ToString();
                        }
                        else
                        {
                            NotSelectedRegIdQuery = item.RegistrationId.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        //CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                        throw ex;
                    }
                }

            }

            Boolean delStatus = roomRegistrationDA.SaveUpdateRoomStopChargePostingLink(regIdQuery, customQuery, userInformationBO.UserInfoId);

            if (delStatus)
            {
                Boolean status = roomRegistrationDA.UpdateStopChargePosting(NotSelectedRegIdQuery, userInformationBO.UserInfoId);

                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), "Room Stop Charge Posting", RoomStopChargePostingRegIds[0].RegistrationId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Room Stop Charge Posting for the RegistrationId: " + RoomStopChargePostingRegIds[0].RegistrationId.ToString());

                message = "Room Stop Charge Posting Successfull.";

            }
            else
            {
                message = "Room Stop Charge Posting Failed.";
            }



            return message;
        }
        [WebMethod]
        public static string SearchGuestAndLoadGridInformation(string companyName, string DateOfBirth, string EmailAddress, string FromDate, string ToDate, string GuestName, string MobileNumber, string NationalId, string PassportNumber, string RegistrationNumber, string RoomNumber)
        {
            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            List<GuestInformationBO> distinctItems = new List<GuestInformationBO>();

            DateTime? dateOfBirth = null;
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrWhiteSpace(DateOfBirth))
                dateOfBirth = hmUtility.GetDateTimeFromString(DateOfBirth, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            if (!string.IsNullOrWhiteSpace(FromDate))
                fromDate = hmUtility.GetDateTimeFromString(FromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            if (!string.IsNullOrWhiteSpace(ToDate))
                toDate = hmUtility.GetDateTimeFromString(ToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            list = guestDA.GetGuestInformationBySearchCriteria(GuestName, EmailAddress, MobileNumber, NationalId, PassportNumber, dateOfBirth, companyName, RoomNumber, fromDate, toDate, RegistrationNumber);
            distinctItems = list.GroupBy(x => x.GuestId).Select(y => y.First()).Distinct().ToList();
            return commonDA.GetHTMLGuestDetailInfoGridView(distinctItems);
        }

        // Documents //
        [WebMethod]
        public static List<DocumentsBO> LoadDocument(long id, int randomId, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("Guest", randomId);

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("Guest", id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }

        [WebMethod]
        public static GuestInformationViewBO LoadDetailInformation(int GuestId)
        {
            //HMCommonDA commonDA = new HMCommonDA();
            //return commonDA.GetGuestDetailInformation(GuestId);

            GuestInformationViewBO guestInfo = new GuestInformationViewBO();

            GuestInformationBO guestBO = new GuestInformationBO();
            GuestInformationDA guestDA = new GuestInformationDA();
            guestBO = guestDA.GetGuestInformationByGuestIdNew(GuestId);
            if (guestBO.Title == "Mr. & Mrs.")
            {
                guestBO.Title = "MrNMrs.";
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("Guest", GuestId);

            GuestPreferenceDA preferenceDA = new GuestPreferenceDA();
            List<GuestPreferenceBO> preferenceList = new List<GuestPreferenceBO>();
            preferenceList = preferenceDA.GetGuestPreferenceInfoByGuestId(GuestId);

            if (preferenceList.Count > 0)
            {
                foreach (GuestPreferenceBO preference in preferenceList)
                    if (guestInfo.GuestPreference != null)
                    {
                        guestInfo.GuestPreference += ", " + preference.PreferenceName;
                        guestInfo.GuestPreferenceId += "," + preference.PreferenceId;
                    }
                    else
                    {
                        guestInfo.GuestPreference = preference.PreferenceName;
                        guestInfo.GuestPreferenceId = preference.PreferenceId.ToString();
                    }
            }

            string extension = ".txt, .doc, .docx, .pdf, .trf";

            foreach (DocumentsBO dc in docList)
            {
                if (!extension.Contains(dc.Extention))
                    dc.Path = (dc.Path + dc.Name);
                else
                    dc.Path = string.Empty;

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }

            guestInfo.GuestInfo = guestBO;
            guestInfo.GuestDoc = docList;

            return guestInfo;
        }
        [WebMethod]
        public static string LoadGuestPreferenceInfo()
        {
            List<GuestPreferenceBO> guestReferenceList = new List<GuestPreferenceBO>();
            GuestPreferenceDA guestReferenceDA = new GuestPreferenceDA();

            string HTML = string.Empty;

            guestReferenceList = guestReferenceDA.GetActiveGuestPreferenceInfo();
            HTML = GetHTMLGuestReferenceGridView(guestReferenceList);

            return HTML;
        }
        [WebMethod(EnableSession = true)]
        public static string SaveGuestInformation(string IntOwner, string txtTitle, string txtFirstName, string txtLastName, string txtGuestName, string txtGuestEmail, string hiddenGuestId, string txtGuestDrivinlgLicense, string txtGuestDOB, string txtGuestAddress1, string txtGuestAddress2, string ddlProfessionId, string txtGuestCity, string ddlGuestCountry, string txtGuestNationality, string txtGuestPhone, string ddlGuestSex, string txtGuestZipCode, string txtNationalId, string txtPassportNumber, string txtPExpireDate, string txtPIssueDate, string txtPIssuePlace, string txtVExpireDate, string txtVisaNumber, string txtVIssueDate, string guestDeletedDoc, string deletedGuestId, string selectedPreferenceId)
        {
            //GuestTemidPrint = "Paramater From Client:" + tempRegId;

            HMUtility hmUtility = new HMUtility();
            RoomRegistrationDA regDA = new RoomRegistrationDA();
            GuestInformationBO detailBO = new GuestInformationBO();
            detailBO.GuestId = Int32.Parse(IntOwner);
            detailBO.GuestAddress1 = txtGuestAddress1;
            detailBO.GuestAddress2 = txtGuestAddress2;
            detailBO.GuestAuthentication = "";
            detailBO.ProfessionId = Int32.Parse(ddlProfessionId);
            detailBO.GuestCity = txtGuestCity;
            detailBO.GuestCountryId = Int32.Parse(ddlGuestCountry);
            if (!string.IsNullOrWhiteSpace(txtGuestDOB))
            {
                detailBO.GuestDOB = hmUtility.GetDateTimeFromString(txtGuestDOB, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.GuestDOB = null;
            }
            detailBO.GuestDrivinlgLicense = txtGuestDrivinlgLicense;
            detailBO.GuestEmail = txtGuestEmail;
            detailBO.Title = txtTitle;
            detailBO.FirstName = txtFirstName;
            detailBO.LastName = txtLastName;
            detailBO.GuestName = txtGuestName;
            detailBO.GuestNationality = txtGuestNationality;
            detailBO.GuestPhone = txtGuestPhone;
            detailBO.GuestSex = ddlGuestSex;
            detailBO.GuestZipCode = txtGuestZipCode;
            detailBO.NationalId = txtNationalId;
            detailBO.PassportNumber = txtPassportNumber;
            if (!string.IsNullOrWhiteSpace(txtPExpireDate))
            {
                detailBO.PExpireDate = hmUtility.GetDateTimeFromString(txtPExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.PExpireDate = null;
            }
            if (!string.IsNullOrWhiteSpace(txtPIssueDate))
            {
                detailBO.PIssueDate = hmUtility.GetDateTimeFromString(txtPIssueDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.PIssueDate = null;
            }
            detailBO.PIssuePlace = txtPIssuePlace;
            if (!string.IsNullOrWhiteSpace(txtVExpireDate))
            {
                detailBO.VExpireDate = hmUtility.GetDateTimeFromString(txtVExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.VExpireDate = null;
            }
            detailBO.VisaNumber = txtVisaNumber;
            if (!string.IsNullOrWhiteSpace(txtVIssueDate))
            {
                detailBO.VIssueDate = hmUtility.GetDateTimeFromString(txtVIssueDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.VIssueDate = null;
            }
            //detailBO.GuestPreferences = gstPreferences;

            DateTime checkIndate = DateTime.Now;
            //  detailBO.Document = documentBO;

            List<GuestPreferenceMappingBO> preferenList = new List<GuestPreferenceMappingBO>();

            long prfId = 0;
            if (!string.IsNullOrEmpty(selectedPreferenceId))
            {
                string[] preferenceIds = selectedPreferenceId.Split(',');
                for (int i = 0; i < preferenceIds.Count(); i++)
                {
                    GuestPreferenceMappingBO preferenceBO = new GuestPreferenceMappingBO();
                    prfId = Convert.ToInt64(preferenceIds[i]);
                    preferenceBO.PreferenceId = prfId;
                    preferenList.Add(preferenceBO);
                }
            }

            string message = string.Empty;
            GuestInformationDA guestDA = new GuestInformationDA();
            Boolean status = guestDA.UpdateGuestInformationNew(detailBO, "0", guestDeletedDoc, preferenList);
            if (status)
            {
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), "Guest Information Update", detailBO.GuestId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Guest Information Update for the GuestId: " + detailBO.GuestId.ToString());

                message = "Guest Information Update Successfully.";
            }
            else
            {
                message = "Guest Information Update Failed.";
            }

            return message;
        }

        [WebMethod]
        public static ArrayList GetRegistrationInfoByRoomNumber(string roomNumber)
        {

            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomAlocationBO roomAllocation = new RoomAlocationBO();

            List<HotelLinkedRoomDetailsBO> hotelLinkedRoomDetailsList = new List<HotelLinkedRoomDetailsBO>();
            List<HotelLinkedRoomDetailsBO> newAddList = new List<HotelLinkedRoomDetailsBO>();

            roomAllocation = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);

            HotelLinkedRoomDetailsBO hotelLinkedRoomDetails = new HotelLinkedRoomDetailsBO()//individual info
            {
                GuestName = roomAllocation.GuestName,
                RegistrationNumber = roomAllocation.RegistrationNumber,
                RoomNumber = roomAllocation.RoomNumber,
                RegistrationId = roomAllocation.RegistrationId
            };
            newAddList.Add(hotelLinkedRoomDetails);
            var count = newAddList.Count;

            hotelLinkedRoomDetailsList = roomRegistrationDA.GetHotelLinkedRoomDetailsByRegId(roomAllocation.RegistrationId);//saved linked rooms



            var alreadyExist = (from data in hotelLinkedRoomDetailsList
                                where (
                                from item in newAddList
                                select item.RegistrationId).Contains(data.RegistrationId)
                                select data).ToList();

            ArrayList arr = new ArrayList();
            arr.Add(new { NewAddedRoom = newAddList, AlreadyExistRoom = alreadyExist });

            return arr;
        }
        [WebMethod] //search reasult
        public static ArrayList GetLinkedRoomByRoomNumber(string roomNumber)
        {
            ArrayList arr = new ArrayList();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomAlocationBO roomAllocationDetail = new RoomAlocationBO();

            RoomAlocationBO roomAllocationMaster = new RoomAlocationBO();

            List<HotelLinkedRoomDetailsBO> allDetailRoomList = new List<HotelLinkedRoomDetailsBO>();

            List<HotelLinkedRoomMasterBO> allLMasterRoomList = new List<HotelLinkedRoomMasterBO>();

            List<HotelLinkedRoomDetailsBO> detailRoomList = new List<HotelLinkedRoomDetailsBO>();

            roomAllocationMaster = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
            if (roomAllocationMaster.RegistrationId > 0)
            {
                //get all master room data
                allLMasterRoomList = roomRegistrationDA.GetLinkedMasterRoomInfoByRegistrationId(roomAllocationMaster.RegistrationId);

                //saved linked rooms
                allDetailRoomList = roomRegistrationDA.GetLinkedDetailsRoomInfoByRegistrationId(roomAllocationMaster.RegistrationId);

                var searchedRoom = allDetailRoomList.Where(data => data.RegistrationId == roomAllocationMaster.RegistrationId).ToList();



                var getDetailRooms = (from data in allDetailRoomList
                                      where (
                                      from item in searchedRoom
                                      select item.MasterId).Contains(data.MasterId)
                                      select data).ToList(); //seperate the detail rooms

                var getMasterRoom = (from data in allLMasterRoomList
                                     where (
                                     from item in searchedRoom
                                     select item.MasterId).Contains(data.Id)
                                     select data).ToList();

                foreach (var item in getDetailRooms)
                {
                    roomAllocationDetail = roomRegistrationDA.GetRegistrationInfoByRegistrationId(item.RegistrationId);

                    HotelLinkedRoomDetailsBO hotelLinkedRoomDetails = new HotelLinkedRoomDetailsBO()//individual info
                    {
                        Id = item.Id,
                        GuestName = roomAllocationDetail.GuestName,
                        RegistrationNumber = roomAllocationDetail.RegistrationNumber,
                        RoomNumber = roomAllocationDetail.RoomNumber,
                        RegistrationId = roomAllocationDetail.RegistrationId,
                        MasterId = item.MasterId
                    };
                    detailRoomList.Add(hotelLinkedRoomDetails);
                }

                if (getMasterRoom != null)
                {
                    if (getMasterRoom.Count > 0)
                    {
                        var x = (from data in detailRoomList
                                 where (
                                 from item in getMasterRoom
                                 select item.RegistrationId).Contains(data.RegistrationId)
                                 select data).ToList();// master room with details info

                        detailRoomList.RemoveAll(m => m.RegistrationId == getMasterRoom[0].RegistrationId);

                        arr.Add(new { DetailRooms = detailRoomList, MasterRoomInfo = x, MasterRoom = getMasterRoom });
                    }
                }
            }
            return arr;
        }

        [WebMethod]
        public static ReturnInfo SaveMasterAndDetailRooms(List<HotelLinkedRoomDetailsBO> tableData, List<HotelLinkedRoomMasterBO> masterData, long masterRoomRegId, string groupName, string description)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            long tempId = 0;
            long masterPk = 0;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                int createdBy = userInformationBO.UserInfoId;

                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocation = new RoomAlocationBO();

                List<HotelLinkedRoomDetailsBO> detailRoomList = new List<HotelLinkedRoomDetailsBO>();
                List<HotelLinkedRoomDetailsBO> alreadySavedDetailRooms = new List<HotelLinkedRoomDetailsBO>();
                List<HotelLinkedRoomDetailsBO> newDetailRooms = new List<HotelLinkedRoomDetailsBO>();
                List<HotelLinkedRoomDetailsBO> tempNewDetailRooms = new List<HotelLinkedRoomDetailsBO>();

                List<HotelLinkedRoomMasterBO> masterRoomList = new List<HotelLinkedRoomMasterBO>();
                List<HotelLinkedRoomMasterBO> savedMasterRoomsByRegId = new List<HotelLinkedRoomMasterBO>();
                List<HotelLinkedRoomMasterBO> savedMasterRooms = new List<HotelLinkedRoomMasterBO>();
                List<HotelLinkedRoomMasterBO> newMasterRoom = new List<HotelLinkedRoomMasterBO>();

                //get the master room by regId
                savedMasterRoomsByRegId = roomRegistrationDA.GetMasterRoomInfoByRegId(masterRoomRegId);

                savedMasterRooms = roomRegistrationDA.GetAllMasterRoom();

                alreadySavedDetailRooms = roomRegistrationDA.GetAllDetailsRoom();

                var newAddedMaster = (from data in masterData
                                      where !(
                                      from item in alreadySavedDetailRooms
                                      select item.RegistrationId).Contains(data.RegistrationId)
                                      select data).ToList();

                var duplicateMaster = (from data in savedMasterRooms
                                       where (
                                       from item in tableData
                                       select item.RegistrationId).Contains(data.RegistrationId)
                                       select data).ToList();

                var newAddDetail = (from data in tableData
                                    where !(
                                    from item in alreadySavedDetailRooms
                                    select item.RegistrationId).Contains(data.RegistrationId)
                                    select data).ToList();



                if (duplicateMaster.Count <= 0 && newAddedMaster.Count > 0 && newAddDetail.Count > 0)//new add for master and detail
                {
                    rtninfo.IsSuccess = roomRegistrationDA.SaveMasterRoom(newAddedMaster, createdBy, out masterPk);

                    rtninfo.IsSuccess = roomRegistrationDA.SaveDetailsRoom(newAddDetail, masterPk, createdBy, out tempId);

                    rtninfo.IsSuccess = roomRegistrationDA.SaveMasterRoomInDetailTable(masterRoomRegId, masterPk, createdBy, out tempId);

                    if (rtninfo.IsSuccess)
                    {
                        rtninfo.IsSuccess = true;

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LinkedRoom.ToString(), tempId,
                            ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                            "New group created");

                        rtninfo.AlertMessage = CommonHelper.AlertInfo("Room Linking Saved.", AlertType.Success);
                        //return true;
                    }
                    else
                    {
                        rtninfo.IsSuccess = false;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }

                if (duplicateMaster.Count > 0 && newAddedMaster.Count > 0)//changing master room by adding a new one 
                {
                    long id = duplicateMaster[0].Id;

                    rtninfo.IsSuccess = roomRegistrationDA.UpdateMasterRoom(newAddedMaster, createdBy, id);

                    rtninfo.IsSuccess = roomRegistrationDA.SaveMasterRoomInDetailTable(masterRoomRegId, id, createdBy, out tempId);

                    if (rtninfo.IsSuccess)
                    {
                        rtninfo.IsSuccess = true;

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LinkedRoom.ToString(), tempId,
                            ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                            "New master room added");

                        rtninfo.AlertMessage = CommonHelper.AlertInfo("Room Linking Saved. ", AlertType.Success);
                        //return true;
                    }
                    else
                    {
                        rtninfo.IsSuccess = false;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }

                if (duplicateMaster.Count > 0 && newAddedMaster.Count <= 0)// just changing master room
                {
                    long id = duplicateMaster[0].Id;
                    bool isMasterUpdate = false;

                    rtninfo.IsSuccess = roomRegistrationDA.UpdateMasterRoom(masterData, createdBy, id);
                    if (rtninfo.IsSuccess)
                    {
                        List<GuestBillPaymentBO> billPayBO = new List<GuestBillPaymentBO>();
                        GuestBillPaymentDA billDA = new GuestBillPaymentDA();
                        //get previous master room's bill payment
                        billPayBO = billDA.GetGuestBillPaymentInfoByRegistrationIdForLink(Convert.ToInt32(duplicateMaster[0].RegistrationId));
                        if (billPayBO.Count > 0)
                        {
                            isMasterUpdate = billDA.UpdateGuestBillPaymentInfoForLink(billPayBO, Convert.ToInt32(masterData[0].RegistrationId));
                        }

                        rtninfo.IsSuccess = true;

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LinkedRoom.ToString(), id,
                            ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                            "Master room updated");

                        rtninfo.AlertMessage = CommonHelper.AlertInfo("Room Linking Saved. ", AlertType.Success);
                        //return true;
                    }
                    else
                    {
                        rtninfo.IsSuccess = false;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }

                if ((newAddDetail.Count > 0) && (duplicateMaster.Count <= 0) && (savedMasterRoomsByRegId.Count > 0))//new detail room add in prev group
                {
                    masterPk = savedMasterRoomsByRegId[0].Id;
                    rtninfo.IsSuccess = roomRegistrationDA.SaveDetailsRoom(newAddDetail, masterPk, createdBy, out tempId);

                    if (rtninfo.IsSuccess)
                    {
                        rtninfo.IsSuccess = true;

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LinkedRoom.ToString(), tempId,
                            ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                            "New Detail room added with previous group");

                        rtninfo.AlertMessage = CommonHelper.AlertInfo("Room Linking Saved.", AlertType.Success);
                        //return true;
                    }
                    else
                    {
                        rtninfo.IsSuccess = false;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }
                if ((newAddDetail.Count <= 0) && (duplicateMaster.Count <= 0) && (newAddedMaster.Count <= 0))
                {
                    var updateMaster = (from data in savedMasterRooms
                                        where (
                                        from item in masterData
                                        select item.RegistrationId).Contains(data.RegistrationId)
                                        select data).ToList();
                    long masterId = updateMaster[0].Id;
                    rtninfo.IsSuccess = roomRegistrationDA.UpdateMasterRoomLinkName(masterData, createdBy, masterId);
                    if (rtninfo.IsSuccess)
                    { rtninfo.AlertMessage = CommonHelper.AlertInfo("Room Linking Saved. ", AlertType.Success); }
                }



                //if (rtninfo.IsSuccess)
                //{
                //    rtninfo.IsSuccess = true;

                //    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                //        EntityTypeEnum.EntityType.LinkedRoom.ToString(), tempId,
                //        ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                //        "");

                //    rtninfo.AlertMessage = CommonHelper.AlertInfo("Saved ", AlertType.Success);
                //    //return true;
                //}
                //else
                //{
                //    rtninfo.IsSuccess = false;
                //    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                //}


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return rtninfo;
        }
        [WebMethod]
        public static ReturnInfo CheckLinkedRooms(long masterId)
        {
            ArrayList arr = new ArrayList();
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            List<HotelLinkedRoomDetailsBO> hotelLinkedRoomDetailsList = new List<HotelLinkedRoomDetailsBO>();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            hotelLinkedRoomDetailsList = roomRegistrationDA.GetDetailRoomsByMasterId(masterId);
            bool status1 = false, status2 = false;
            if (hotelLinkedRoomDetailsList.Count < 2)
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                status1 = hmCommonDA.DeleteInfoById("HotelLinkedRoomDetails", "MasterId", masterId);
                status2 = hmCommonDA.DeleteInfoById("HotelLinkedRoomMaster", "Id", masterId);
            }

            if ((status1 == true) && (status2 == true))
            {

                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                    EntityTypeEnum.EntityType.LinkedRoom.ToString(), masterId,
                    ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(),
                    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LinkedRoom));

                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
            }
            //else
            //{
            //    rtninf.IsSuccess = false;
            //    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            //}

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo DeleteRoomData(long deleteId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();

            //List<HotelLinkedRoomDetailsBO> hotelLinkedRoomDetailsBO = new List<HotelLinkedRoomDetailsBO>();
            //RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            //hotelLinkedRoomDetailsBO = roomRegistrationDA.GetDetailRoomsByMasterId(masterId);
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                bool status = hmCommonDA.DeleteInfoById("HotelLinkedRoomDetails", "RegistrationId", deleteId);

                if (status)
                {

                    //bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), 
                    //    EntityTypeEnum.EntityType.RoomFeatures.ToString(), deleteId, 
                    //    ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), 
                    //    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomFeatures));

                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return rtninf;
        }

        [WebMethod]
        public static ReturnInfo UpdateGuestBlockInfo(int guestId, bool isGuestBlock, string description)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            try
            {
                GuestInformationDA guestDA = new GuestInformationDA();
                rtninfo.IsSuccess = guestDA.UpdateGuestBlockInfo(guestId, isGuestBlock, description);

                if (rtninfo.IsSuccess)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), guestId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("Updated ", AlertType.Success);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return rtninfo;
        }
        
    }
}