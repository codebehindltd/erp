using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.POS;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using System.Collections;
using HotelManagement.Data.Security;
using HotelManagement.Entity.Security;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity;
using HotelManagement.Data;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Membership;
using HotelManagement.Data.Membership;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;

namespace HotelManagement.Presentation.Website.Common
{
    public partial class WebMethodPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static string RoomStatisticsInfo()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            RoomReservationDA roomReservationDA = new RoomReservationDA();
            Boolean status = roomReservationDA.HotelRoomReservationNoShowProcess(DateTime.Now, userInformationBO.UserInfoId);

            HMCommonDA roomStatisticsInfoDA = new HMCommonDA();
            RoomStatisticsInfoBO roomStatisticsInfoBO = new RoomStatisticsInfoBO();
            roomStatisticsInfoBO = roomStatisticsInfoDA.GetRoomStatisticsInfo();

            string query = string.Empty;
            query = "<div class='btn-group btn-group-justified' role='group'> " +
                    "  <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('ExpectedArrival');\" style='cursor: pointer;'> " +
                    "     <div class='thumbnail'>" +
                    "         <label id = 'lblExpectedArrival' class='StatisticsQuantity'>" + roomStatisticsInfoBO.ExpectedArrival.ToString() + "</label>" +
                    "         <div class='caption'>" +
                                    "Expected Arrival" +
                    "         </div>" +
                    "     </div>" +
                    "  </div> " +
                    "   <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('ExpectedDeparture');\" style='cursor: pointer;'>" +
                    "       <div class='thumbnail'>" +
                    "           <label id = 'lblExpectedDeparture' class='StatisticsQuantity'>" + roomStatisticsInfoBO.ExpectedDeparture.ToString() + "</label>" +
                    "           <div class='caption'>" +
                    "               Expected Departure" +
                    "           </div> " +
                    "       </div> " +
                    "   </div> " +
                    " </div> ";

            query += "<div class='btn-group btn-group-justified' role='group'> " +
                    "  <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('CheckInRoom');\" style='cursor: pointer;'> " +
                    "     <div class='thumbnail'>" +
                    "         <label id = 'lblExpectedArrival' class='StatisticsQuantity'>" + roomStatisticsInfoBO.CheckInRoom.ToString() + "</label>" +
                    "         <div class='caption'>" +
                    "               Check in Room" +
                    "         </div>" +
                    "     </div>" +
                    "  </div> " +
                    "   <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('WalkInRoom');\" style='cursor: pointer;'>" +
                    "       <div class='thumbnail'>" +
                    "           <label id = 'lblExpectedDeparture' class='StatisticsQuantity'>" + roomStatisticsInfoBO.WalkInRoom.ToString() + "</label>" +
                    "           <div class='caption'>" +
                    "               Walk in Room" +
                    "           </div> " +
                    "       </div> " +
                    "   </div> " +
                    " </div> ";
            query += "<div class='btn-group btn-group-justified' role='group'> " +
                    "  <div class='btn-group text-center' role='group' onclick=\"PopUpRoomControlChartInfo();\" style='cursor: pointer;'> " +
                    "     <div class='thumbnail'>" +
                    "         <label id = 'lblForecastReport' class='StatisticsQuantity'>" + "*" + "</label>" +
                    "         <div class='caption'>" +
                    "               Forecast Report" +
                    "         </div>" +
                    "     </div>" +
                    "  </div> " +
                    "   <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('AirportPickupDrop');\" style='cursor: pointer;'>" +
                    "       <div class='thumbnail'>" +
                    "           <label id = 'lblExpectedArrival' class='StatisticsQuantity'>" + roomStatisticsInfoBO.AirportPickupDrop.ToString() + "</label>" +
                    "           <div class='caption'>" +
                    "               Pick Up/ Drop Off" +
                    "           </div> " +
                    "       </div> " +
                    "   </div> " +
                    " </div> ";

            //query += "<div class='btn-group btn-group-justified' role='group'> " +
            //        "  <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('DHotelPosition');\" style='cursor: pointer;'> " +
            //        "     <div class='thumbnail'>" +
            //        "         <label id = 'lblExpectedArrival' class='StatisticsQuantity'>" + roomStatisticsInfoBO.RoomToSell.ToString() + "</label>" +
            //        "         <div class='caption'>" +
            //                        "Room to Sell" +
            //        "         </div>" +
            //        "     </div>" +
            //        "  </div> " +
            //        "   <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('ForecastReport');\" style='cursor: pointer;'>" +
            //        "       <div class='thumbnail'>" +
            //        "           <label id = 'lblExpectedDeparture' class='StatisticsQuantity'>" + "*" + "</label>" +
            //        "           <div class='caption'>" +
            //        "               Forecast Report" +
            //        "           </div> " +
            //        "       </div> " +
            //        "   </div> " +
            //        " </div> ";

            query += "<div class='btn-group btn-group-justified' role='group'> " +
                    "  <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('InhouseRoomGuest');\" style='cursor: pointer;'> " +
                    "     <div class='thumbnail'>" +
                    "         <label id = 'lblExpectedArrival' class='StatisticsQuantity'>" + roomStatisticsInfoBO.InhouseRoomOrGuest.ToString() + "</label>" +
                    "         <div class='caption'>" +
                                    "Inhouse Room / Guest" +
                    "         </div>" +
                    "     </div>" +
                    "  </div> " +
                    "   <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('ExtraAdultsOrChild');\" style='cursor: pointer;'>" +
                    "       <div class='thumbnail'>" +
                    "           <label id = 'lblExpectedDeparture' class='StatisticsQuantity'>" + roomStatisticsInfoBO.ExtraAdultsOrChild.ToString() + "</label>" +
                    "           <div class='caption'>" +
                    "               Extra Adult / Child" +
                    "           </div> " +
                    "       </div> " +
                    "   </div> " +
                    " </div> ";

            query += "<div class='btn-group btn-group-justified' role='group'> " +
                    "  <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('InhouseForeigner');\" style='cursor: pointer;'> " +
                    "     <div class='thumbnail'>" +
                    "         <label id = 'lblExpectedArrival' class='StatisticsQuantity'>" + roomStatisticsInfoBO.InhouseForeigners.ToString() + "</label>" +
                    "         <div class='caption'>" +
                                    "Inhouse Foreigner" +
                    "         </div>" +
                    "     </div>" +
                    "  </div> " +
                    "   <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('GuestBlock');\" style='cursor: pointer;'>" +
                    "       <div class='thumbnail'>" +
                    "           <label id = 'lblExpectedDeparture' class='StatisticsQuantity'>" + roomStatisticsInfoBO.GuestBlock.ToString() + "</label>" +
                    "           <div class='caption'>" +
                    "               Room Block" +
                    "           </div> " +
                    "       </div> " +
                    "   </div> " +
                    " </div> ";

            //query += "<div class='btn-group btn-group-justified' role='group'> " +
            //        "  <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('AirportPickupDrop');\" style='cursor: pointer;'> " +
            //        "     <div class='thumbnail'>" +
            //        "         <label id = 'lblExpectedArrival' class='StatisticsQuantity'>" + roomStatisticsInfoBO.AirportPickupDrop.ToString() + "</label>" +
            //        "         <div class='caption'>" +
            //                        "Airport Pickup/ Drop" +
            //        "         </div>" +
            //        "     </div>" +
            //        "  </div> " +
            //        "   <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('DHotelPosition');\" style='cursor: pointer;'>" +
            //        "       <div class='thumbnail'>" +
            //        "           <label id = 'lblExpectedDeparture' class='StatisticsQuantity'>" + roomStatisticsInfoBO.Occupency.ToString() + "</label>" +
            //        "           <div class='caption'>" +
            //        "               Occupency (%) FO/PO" +
            //        "           </div> " +
            //        "       </div> " +
            //        "   </div> " +
            //        " </div> ";

            //query += "<div class='btn-group btn-group-justified' role='group'> " +
            //        "  <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('DHotelPosition');\" style='cursor: pointer;'> " +
            //        "     <div class='thumbnail'>" +
            //        "         <label id = 'lblExpectedArrival' class='StatisticsQuantity'>" + roomStatisticsInfoBO.RoomToSell.ToString() + "</label>" +
            //        "         <div class='caption'>" +
            //                        "Room to Sell" +
            //        "         </div>" +
            //        "     </div>" +
            //        "  </div> " +
            //        "   <div class='btn-group text-center' role='group' onclick=\"PopUpStatisticsReportInfo('RegisterComplaint');\" style='cursor: pointer;'>" +
            //        "       <div class='thumbnail'>" +
            //        "           <label id = 'lblExpectedDeparture' class='StatisticsQuantity'>" + roomStatisticsInfoBO.RegisterComplaint.ToString() + "</label>" +
            //        "           <div class='caption'>" +
            //        "               Register Complaint" +
            //        "           </div> " +
            //        "       </div> " +
            //        "   </div> " +
            //        " </div> ";


            return query;
        }

        [WebMethod]
        public static GuestInformationBO GetRegistrationInformationForSingleGuestByRoomNumber(string roomNumer)
        {
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumer);
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);
            foreach (GuestInformationBO row in list)
            {
                row.RoomRate = allocationBO.RoomRate;
                row.CurrencyTypeHead = allocationBO.CurrencyTypeHead;
                row.ArriveDate = row.ArriveDate;
                row.ExpectedCheckOutDate = allocationBO.ExpectedCheckOutDate;
            }
            return list[0];
        }

        [WebMethod]
        public static EmployeeBO SearchEmployee(string employeeCode)
        {
            EmployeeBO empBo = new EmployeeBO();
            EmployeeDA employeeDA = new EmployeeDA();
            empBo = employeeDA.GetEmployeeInfoByCode(employeeCode.Trim());

            return empBo;
        }

        [WebMethod]
        public static List<EmployeeBO> SearchEmployeeByName(string employeeName)
        {
            EmployeeDA employeeDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = employeeDA.GetEmpInformationForAutoSearch(employeeName.Trim());

            return empList;
        }

        [WebMethod]
        public static List<RestaurantBearerBO> GetBearerInfoByAutoSearch(string beararName, int costCenterId)
        {
            List<RestaurantBearerBO> bearerList = new List<RestaurantBearerBO>();
            RestaurantBearerDA bearerDa = new RestaurantBearerDA();
            bearerList = bearerDa.GetRestaurantUserByAutoSearch(beararName, costCenterId);

            return bearerList;
        }

        [WebMethod]
        public static ArrayList PopulateProjectsWithUser(int companyId)
        {
            //int UserGroupId = Session["UserGroupId"].ToString();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ArrayList list = new ArrayList();
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();
            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(Convert.ToInt32(companyId), Convert.ToInt32(userInformationBO.UserGroupId));
            int count = projectList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new ListItem(
                                        projectList[i].Name.ToString(),
                                        projectList[i].ProjectId.ToString()
                                         ));
            }
            return list;
        }

        [WebMethod]
        public static MemMemberBasicsBO GetMemberInfoByMembershipNo(string membershipNumber)
        {
            MemMemberBasicsBO bo = new MemMemberBasicsBO();
            MemMemberBasicDA memDa = new MemMemberBasicDA();

            bo = memDa.GetMemberInfoByMembershipNo(membershipNumber);

            return bo;
        }

        [WebMethod]
        public static List<MemMemberBasicsBO> GetMemberInfoByName(string memberName)
        {
            List<MemMemberBasicsBO> bo = new List<MemMemberBasicsBO>();
            MemMemberBasicDA memDa = new MemMemberBasicDA();

            bo = memDa.GetMemberInfoByName(memberName.Trim());

            return bo;
        }

        [WebMethod]
        public static List<MemMemberBasicsBO> GetMemberDetailInfoForCostcenter(int costCenterId, string memberName)
        {
            List<MemMemberBasicsBO> bo = new List<MemMemberBasicsBO>();
            MemMemberBasicDA memDa = new MemMemberBasicDA();

            bo = memDa.GetMemberDetailInfoForCostcenter(costCenterId, memberName.Trim());

            return bo;
        }

        [WebMethod]
        public static List<CountriesBO> SearchCountry(string searchTerm)
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CountriesBO> countryList = commonDA.GetCountriesBySearch(searchTerm);
            countryList = commonDA.GetCountriesBySearch(searchTerm.Trim());

            return countryList;
        }
        [WebMethod]
        public static string CalculateAge(DateTime dateOfBirth)
        {
            DateTime adtCurrentDate = DateTime.Now;
            int ageNoOfYears, ageNoOfMonths, ageNoOfDays;
            string age = string.Empty;

            ageNoOfDays = adtCurrentDate.Day - dateOfBirth.Day;
            ageNoOfMonths = adtCurrentDate.Month - dateOfBirth.Month;
            ageNoOfYears = adtCurrentDate.Year - dateOfBirth.Year;

            if (ageNoOfDays < 0)
            {
                ageNoOfDays += DateTime.DaysInMonth(adtCurrentDate.Year, adtCurrentDate.Month);
                ageNoOfDays--;
            }

            if (ageNoOfMonths < 0)
            {
                ageNoOfMonths += 12;
                ageNoOfMonths--;
            }

            DateTime date = DateTime.Now;
            //System.DateTime BirthDate = new System.DateTime(1991, 9, 11, 0, 0, 0);
            System.TimeSpan diffResult = date.Subtract(dateOfBirth);
            string TotalDays = diffResult.Days.ToString();
            string Months = ((diffResult.Days) % 365).ToString();
            string RemainingMonths = (Convert.ToInt32(Months) / 31).ToString();
            string RemainginYears = ((diffResult.Days) / 365).ToString();
            string RemainingDays = (Convert.ToInt32(Months) % 31).ToString();

            var leepYears = Enumerable.Range(dateOfBirth.Year, date.Year - dateOfBirth.Year + 1)
                              .Count(x => DateTime.IsLeapYear(x));
            int days = Convert.ToInt32(RemainingDays);

            

            //if (leepYears > 0)
            //{
            //    if (date.Month > 2 && date.Day > 28)
            //    {
            //        days = (days - leepYears);
            //    }
            //    else
            //    {
            //        leepYears -= 1;
            //        days = (days - leepYears);
            //    }
                
            //}
            if (days <= 0)
            {
                RemainingDays = "0";
            }
            else
            {
                RemainingDays = days.ToString();
            }
            

            //age = ageNoOfYears.ToString() + " years, " + ageNoOfMonths.ToString() + " months, " + ageNoOfDays.ToString() + " days";
            age = RemainginYears.ToString() + " years, " + RemainingMonths.ToString() + " months, " + RemainingDays.ToString() + " days";

            return age;

            // Antoher way
            //DateTime dob = Convert.ToDateTime("18 Feb 1987");
            //DateTime PresentYear = DateTime.Now;
            //TimeSpan ts = PresentYear - dob;
            //DateTime Age = DateTime.MinValue.AddDays(ts.Days);
            //MessageBox.Show(string.Format(" {0} Years {1} Month {2} Days", Age.Year - 1, Age.Month - 1, Age.Day - 1));
        }
        
        [WebMethod]
        public static ReturnInfo SaveRestauranBillGeneration(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment)
        {
            ReturnInfo rtninf = new ReturnInfo();
            string categoryIdList = string.Empty;

            try
            {
                int billID = 0;
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
                List<string> strCategoryIdList = new List<string>();

                if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                {
                    strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                    foreach (string rowCWPD in strCategoryIdList)
                    {
                        RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO();
                        categoryWisePercentageDiscountBO.ClassificationId = Convert.ToInt32(rowCWPD);
                        categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);

                        categoryIdList += categoryIdList != string.Empty ? ("," + rowCWPD) : rowCWPD;
                    }
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.BearerId = RestaurantBill.BearerId != 0 ? RestaurantBill.BearerId : userInformationBO.UserInfoId; ;
                RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = DateTime.Now;
                RestaurantBill.BillPaymentDate = DateTime.Now;

                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                restaurentBillDetailBO.KotId = RestaurantBill.KotId;
                restaurentBillDetailBOList.Add(restaurentBillDetailBO);

                int billId = restaurentBillDA.SaveRestaurantBill(RestaurantBill, restaurentBillDetailBOList, GuestBillPayment, categoryWisePercentageDiscountBOList, categoryIdList, true, true, out billID);

                if (billId > 0)
                {
                    //Boolean settlementStatus = restaurentBillDA.RestaurantBillSettlementInfoByBillId(billId);
                    rtninf.IsSuccess = true;
                }

                if (rtninf.IsSuccess)
                {
                    HMCommonSetupBO commonRestaurantBillPrintAndPreview = new HMCommonSetupBO();
                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

                    commonRestaurantBillPrintAndPreview = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillPrintAndPreview", "RestaurantBillPrintAndPreview");

                    rtninf.Pk = billId;

                    rtninf.BillPrintAndPreview = Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue);
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    if (rtninf.BillPrintAndPreview == (int)(HMConstants.RestaurantBillPrintAndPreview.PrintAndPreview))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(billId);

                        rtninf.RedirectUrl = "frmCostCenterSelection.aspx";

                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                        HttpContext.Current.Session.Remove("KotHoldupBill");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                        HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                        HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                        HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                        HttpContext.Current.Session.Remove("IRToeknNumber");
                        HttpContext.Current.Session.Remove("IRtxtTableIdInformation");
                        HttpContext.Current.Session.Remove("IRtxtTableNumberInformation");
                        HttpContext.Current.Session.Remove("tbsMessage");
                        HttpContext.Current.Session.Remove("IRTableAllocatedBy");
                        HttpContext.Current.Session.Remove("IRtxtBearerIdInformation");
                    }
                    else if (rtninf.BillPrintAndPreview == (int)(HMConstants.RestaurantBillPrintAndPreview.DirectPrint))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(billId);

                        rtninf.RedirectUrl = "frmCostCenterSelection.aspx";

                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                        HttpContext.Current.Session.Remove("KotHoldupBill");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                        HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                        HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                        HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                        HttpContext.Current.Session.Remove("IRToeknNumber");
                        HttpContext.Current.Session.Remove("IRtxtTableIdInformation");
                        HttpContext.Current.Session.Remove("IRtxtTableNumberInformation");
                        HttpContext.Current.Session.Remove("tbsMessage");
                        HttpContext.Current.Session.Remove("IRTableAllocatedBy");
                        HttpContext.Current.Session.Remove("IRtxtBearerIdInformation");
                    }
                    else
                    {
                        // HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        //  HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        // HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        //HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        // HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                        //  HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                        // HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                        // HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                        // HttpContext.Current.Session.Remove("IRToeknNumber");
                        // HttpContext.Current.Session.Remove("KotHoldupBill");
                        // HttpContext.Current.Session.Remove("RestaurantKotBillResume");

                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                        HttpContext.Current.Session.Remove("KotHoldupBill");
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                        HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                        HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                        HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                        HttpContext.Current.Session.Remove("IRToeknNumber");
                        HttpContext.Current.Session.Remove("IRtxtTableIdInformation");
                        HttpContext.Current.Session.Remove("IRtxtTableNumberInformation");
                        HttpContext.Current.Session.Remove("tbsMessage");
                        HttpContext.Current.Session.Remove("IRTableAllocatedBy");
                        HttpContext.Current.Session.Remove("IRtxtBearerIdInformation");


                    }
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
        public static ReturnInfo UpdateRestauranBillGeneration(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment)
        {
            ReturnInfo rtninf = new ReturnInfo();
            string categoryIdList = string.Empty;

            try
            {
                RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
                //paymentResume = (RestaurantBillPaymentResume)HttpContext.Current.Session["RestaurantKotBillResume"];

                List<GuestBillPaymentBO> paymentAdded = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentUpdate = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentDelete = new List<GuestBillPaymentBO>();

                RestaurantBill.BillId = paymentResume.RestaurantKotBill.BillId;

                var vcash = (from p in paymentResume.RestaurantKotBillPayment
                             where p.PaymentMode == HMConstants.PaymentMode.Cash.ToString()
                             select p).FirstOrDefault();

                if (vcash == null)
                {
                    vcash = (from p in GuestBillPayment
                             where p.PaymentMode == HMConstants.PaymentMode.Cash.ToString() &&
                                   p.PaymentAmount != 0
                             select p
                             ).FirstOrDefault();

                    if (vcash != null)
                    {
                        paymentAdded.Add(vcash);
                    }
                }

                var vrounded = (from p in paymentResume.RestaurantKotBillPayment
                                where p.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                                select p).FirstOrDefault();

                if (vrounded == null)
                {
                    vrounded = (from p in GuestBillPayment
                                where p.PaymentMode == HMConstants.PaymentMode.Rounded.ToString() &&
                                     p.PaymentAmount != 0
                                select p
                             ).FirstOrDefault();

                    if (vrounded != null)
                    {
                        paymentAdded.Add(vrounded);
                    }
                }

                var vrefund = (from p in paymentResume.RestaurantKotBillPayment
                               where p.PaymentMode == HMConstants.PaymentMode.Refund.ToString()
                               select p).FirstOrDefault();

                if (vrefund == null)
                {
                    vrefund = (from p in GuestBillPayment
                               where p.PaymentMode == HMConstants.PaymentMode.Refund.ToString() &&
                                     p.PaymentAmount != 0
                               select p
                             ).FirstOrDefault();

                    if (vrefund != null)
                    {
                        paymentAdded.Add(vrefund);
                    }
                }

                var vcard = (from p in paymentResume.RestaurantKotBillPayment
                             where p.PaymentMode == HMConstants.PaymentMode.Card.ToString()
                             select p).ToList();

                var vcardNew = (from p in GuestBillPayment
                                where p.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                                      p.PaymentAmount != 0
                                select p).ToList();

                if (vcard != null)
                {
                    foreach (GuestBillPaymentBO bp in vcardNew)
                    {
                        var v1 = (from pr in vcard
                                  where pr.CardType == bp.CardType &&
                                        pr.PaymentAmount != 0
                                  select pr).FirstOrDefault();

                        if (v1 == null)
                        {
                            paymentAdded.Add(bp);
                        }
                    }
                }

                var update1 = (from bp in GuestBillPayment
                               from pr in paymentResume.RestaurantKotBillPayment
                               where bp.PaymentMode == pr.PaymentMode &&
                                     (
                                         bp.PaymentMode == HMConstants.PaymentMode.Cash.ToString() ||
                                         bp.PaymentMode == HMConstants.PaymentMode.Refund.ToString() ||
                                         bp.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                                     ) &&
                                     bp.PaymentAmount != pr.PaymentAmount &&
                                     bp.PaymentAmount != 0
                               select new GuestBillPaymentBO
                               {
                                   PaymentId = pr.PaymentId,
                                   BillNumber = pr.BillNumber,
                                   ModuleName = pr.ModuleName,
                                   PaymentType = pr.PaymentType,
                                   RegistrationId = pr.RegistrationId,
                                   PaymentDate = pr.PaymentDate,
                                   CurrencyAmount = pr.CurrencyAmount,
                                   PaymentAmount = bp.PaymentAmount,
                                   ServiceBillId = pr.ServiceBillId,
                                   PaymentMode = pr.PaymentMode,
                                   BankId = pr.BankId,
                                   BranchName = pr.BranchName,
                                   ChecqueNumber = pr.ChecqueNumber,
                                   CardType = pr.CardType,
                                   CardNumber = pr.CardNumber,
                                   CardHolderName = pr.CardHolderName,
                                   CardReference = pr.CardReference
                               }).ToList();

                var update2 = (from pr in paymentResume.RestaurantKotBillPayment
                               from bp in GuestBillPayment
                               where pr.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                                     pr.CardType == bp.CardType &&
                                     pr.PaymentAmount != bp.PaymentAmount &&
                                     bp.PaymentAmount != 0
                               select new GuestBillPaymentBO
                               {
                                   PaymentId = pr.PaymentId,
                                   BillNumber = pr.BillNumber,
                                   ModuleName = pr.ModuleName,
                                   PaymentType = pr.PaymentType,
                                   RegistrationId = pr.RegistrationId,
                                   PaymentDate = pr.PaymentDate,
                                   CurrencyAmount = pr.CurrencyAmount,
                                   PaymentAmount = bp.PaymentAmount,
                                   ServiceBillId = pr.ServiceBillId,
                                   PaymentMode = pr.PaymentMode,
                                   BankId = pr.BankId,
                                   BranchName = pr.BranchName,
                                   ChecqueNumber = pr.ChecqueNumber,
                                   CardType = pr.CardType,
                                   CardNumber = pr.CardNumber,
                                   CardHolderName = pr.CardHolderName,
                                   CardReference = pr.CardReference
                               }).ToList();

                paymentUpdate.AddRange(update1);
                paymentUpdate.AddRange(update2);

                var delete1 = (from pr in paymentResume.RestaurantKotBillPayment
                               from bp in GuestBillPayment
                               where pr.PaymentMode == bp.PaymentMode &&
                                     (
                                         pr.PaymentMode == HMConstants.PaymentMode.Cash.ToString() ||
                                         pr.PaymentMode == HMConstants.PaymentMode.Refund.ToString() ||
                                         bp.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                                     ) &&
                                     bp.PaymentAmount == 0
                               select new GuestBillPaymentBO
                               {
                                   PaymentId = pr.PaymentId,
                                   BillNumber = pr.BillNumber,
                                   ModuleName = pr.ModuleName,
                                   PaymentType = pr.PaymentType,
                                   RegistrationId = pr.RegistrationId,
                                   PaymentDate = pr.PaymentDate,
                                   PaymentAmount = bp.PaymentAmount,
                                   ServiceBillId = pr.ServiceBillId,
                                   PaymentMode = pr.PaymentMode,
                                   BankId = pr.BankId,
                                   BranchName = pr.BranchName,
                                   ChecqueNumber = pr.ChecqueNumber,
                                   CardType = pr.CardType,
                                   CardNumber = pr.CardNumber,
                                   CardHolderName = pr.CardHolderName,
                                   CardReference = pr.CardReference
                               }).ToList();

                var delete2 = (from pr in paymentResume.RestaurantKotBillPayment
                               from bp in GuestBillPayment
                               where pr.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                                     pr.CardType == bp.CardType &&
                                     bp.PaymentAmount == 0
                               select new GuestBillPaymentBO
                               {
                                   PaymentId = pr.PaymentId,
                                   BillNumber = pr.BillNumber,
                                   ModuleName = pr.ModuleName,
                                   PaymentType = pr.PaymentType,
                                   RegistrationId = pr.RegistrationId,
                                   PaymentDate = pr.PaymentDate,
                                   PaymentAmount = bp.PaymentAmount,
                                   ServiceBillId = pr.ServiceBillId,
                                   PaymentMode = pr.PaymentMode,
                                   BankId = pr.BankId,
                                   BranchName = pr.BranchName,
                                   ChecqueNumber = pr.ChecqueNumber,
                                   CardType = pr.CardType,
                                   CardNumber = pr.CardNumber,
                                   CardHolderName = pr.CardHolderName,
                                   CardReference = pr.CardReference
                               }).ToList();

                paymentDelete.AddRange(delete1);
                paymentDelete.AddRange(delete2);

                int billID = 0;
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
                List<string> strCategoryIdList = new List<string>();

                if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                {
                    strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                    foreach (string rowCWPD in strCategoryIdList)
                    {
                        RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO();
                        categoryWisePercentageDiscountBO.ClassificationId = Convert.ToInt32(rowCWPD);
                        categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);

                        categoryIdList += categoryIdList != string.Empty ? ("," + rowCWPD) : rowCWPD;
                    }
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = paymentResume.RestaurantKotBill.BillDate;
                RestaurantBill.BillPaymentDate = paymentResume.RestaurantKotBill.BillPaymentDate;

                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                restaurentBillDetailBO.BillId = RestaurantBill.BillId;
                restaurentBillDetailBO.KotId = RestaurantBill.KotId;
                restaurentBillDetailBOList.Add(restaurentBillDetailBO);

                bool billId = restaurentBillDA.UpdateRestaurantBill(RestaurantBill, restaurentBillDetailBOList, paymentAdded, paymentUpdate, paymentDelete, categoryWisePercentageDiscountBOList, categoryIdList, true);

                if (billId)
                {
                    //Boolean settlementStatus = restaurentBillDA.RestaurantBillSettlementInfoByBillId(RestaurantBill.BillId);
                    rtninf.IsSuccess = true;
                }

                if (rtninf.IsSuccess)
                {
                    HMCommonSetupBO commonRestaurantBillPrintAndPreview = new HMCommonSetupBO();
                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                    commonRestaurantBillPrintAndPreview = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillPrintAndPreview", "RestaurantBillPrintAndPreview");

                    rtninf.Pk = RestaurantBill.BillId;
                    rtninf.BillPrintAndPreview = Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue);
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    if (rtninf.BillPrintAndPreview == (int)(HMConstants.RestaurantBillPrintAndPreview.DirectPrint))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(RestaurantBill.BillId);

                        rtninf.RedirectUrl = "frmCostCenterSelection.aspx";

                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                        HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                        HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                        HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                        HttpContext.Current.Session.Remove("IRToeknNumber");
                        HttpContext.Current.Session.Remove("KotHoldupBill");
                        HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                    }
                    else
                    {
                        HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                        HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                        HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                        HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                        HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                        HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                        HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                        HttpContext.Current.Session.Remove("IRToeknNumber");
                        HttpContext.Current.Session.Remove("KotHoldupBill");
                        HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                    }
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
        public static ReturnInfo HoldUpRestauranBillGeneration(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                int billID = 0;
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
                List<string> strCategoryIdList = new List<string>();

                //if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                //{
                //    strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                //    foreach (string rowCWPD in strCategoryIdList)
                //    {
                //        RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO();
                //        categoryWisePercentageDiscountBO.ClassificationId = Convert.ToInt32(rowCWPD);
                //        categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);
                //    }
                //}

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.BearerId = RestaurantBill.BearerId != 0 ? RestaurantBill.BearerId : userInformationBO.UserInfoId; ;
                RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = DateTime.Now;
                RestaurantBill.BillPaymentDate = DateTime.Now;

                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                restaurentBillDetailBO.KotId = RestaurantBill.KotId;
                restaurentBillDetailBOList.Add(restaurentBillDetailBO);

                int billId = restaurentBillDA.SaveRestaurantBillForHoldUp(RestaurantBill, restaurentBillDetailBOList, GuestBillPayment, categoryWisePercentageDiscountBOList, out billID);

                if (billId > 0)
                {
                    KotBillMasterDA kotDa = new KotBillMasterDA();
                    kotDa.UpdateKotBillMasterForHoldUp(RestaurantBill.KotId, true);

                    rtninf.IsSuccess = true;
                    rtninf.RedirectUrl = "frmCostCenterSelection.aspx";
                    rtninf.IsBillHoldUp = true;
                    rtninf.Pk = billId;

                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                    HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                    HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                    HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                    HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                    HttpContext.Current.Session.Remove("KotHoldupBill");
                    HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                    HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                    HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                    HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                    HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                    HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                    HttpContext.Current.Session.Remove("IRToeknNumber");
                    HttpContext.Current.Session.Remove("IRtxtTableIdInformation");
                    HttpContext.Current.Session.Remove("IRtxtTableNumberInformation");
                    HttpContext.Current.Session.Remove("tbsMessage");
                    HttpContext.Current.Session.Remove("IRTableAllocatedBy");
                    HttpContext.Current.Session.Remove("IRtxtBearerIdInformation");

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
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
        public static ReturnInfo UpdateHoldUpRestauranBillGeneration(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
                //paymentResume = (RestaurantBillPaymentResume)HttpContext.Current.Session["RestaurantKotBillResume"];

                List<GuestBillPaymentBO> paymentAdded = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentUpdate = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentDelete = new List<GuestBillPaymentBO>();

                RestaurantBill.BillId = paymentResume.RestaurantKotBill.BillId;

                //var vc = (from c in paymentResume.RestaurantKotBillPayment
                //          join b in GuestBillPayment on c.PaymentMode equals b.PaymentMode
                //          where c.PaymentMode != HMConstants.PaymentMode.Card.ToString()
                //          select new GuestBillPaymentBO
                //         {
                //             PaymentId = c.PaymentId,
                //             BillNumber = c.BillNumber,
                //             ModuleName = c.ModuleName,
                //             PaymentType = c.PaymentType,
                //             RegistrationId = c.RegistrationId,
                //             PaymentDate = c.PaymentDate,
                //             CurrencyAmount = b.CurrencyAmount,
                //             PaymentAmount = b.PaymentAmount,
                //             ServiceBillId = c.ServiceBillId,
                //             PaymentMode = b.PaymentMode,
                //             BankId = c.BankId,
                //             BranchName = c.BranchName,
                //             ChecqueNumber = c.ChecqueNumber,
                //             CardType = c.CardType,
                //             CardNumber = c.CardNumber,
                //             CardHolderName = c.CardHolderName,
                //             CardReference = c.CardReference
                //         }).ToList();

                //var vcc = (from c in paymentResume.RestaurantKotBillPayment
                //           join b in GuestBillPayment on c.PaymentMode equals b.PaymentMode
                //           where c.PaymentMode == HMConstants.PaymentMode.Card.ToString() && 
                //                 c.CardType == b.CardType
                //           select new GuestBillPaymentBO
                //           {
                //               PaymentId = c.PaymentId,
                //               BillNumber = c.BillNumber,
                //               ModuleName = c.ModuleName,
                //               PaymentType = c.PaymentType,
                //               RegistrationId = c.RegistrationId,
                //               PaymentDate = c.PaymentDate,
                //               CurrencyAmount = b.CurrencyAmount,
                //               PaymentAmount = b.PaymentAmount,
                //               ServiceBillId = c.ServiceBillId,
                //               PaymentMode = b.PaymentMode,
                //               BankId = c.BankId,
                //               BranchName = c.BranchName,
                //               ChecqueNumber = c.ChecqueNumber,
                //               CardType = c.CardType,
                //               CardNumber = c.CardNumber,
                //               CardHolderName = c.CardHolderName,
                //               CardReference = c.CardReference
                //           }).ToList();

                //paymentAdded = (from p in GuestBillPayment
                //                where !(
                //                         from c in paymentResume.RestaurantKotBillPayment
                //                         where c.PaymentMode != HMConstants.PaymentMode.Card.ToString()
                //                         select c.PaymentMode
                //                      ).Contains(p.PaymentMode)
                //                select p).ToList();

                //paymentAdded.AddRange((from p in GuestBillPayment
                //                       where !(
                //                                from c in paymentResume.RestaurantKotBillPayment
                //                                select c.CardType
                //                             ).Contains(p.CardType)
                //                       select p).ToList());

                //paymentUpdate = (from p in paymentResume.RestaurantKotBillPayment
                //                 where (
                //                          from c in GuestBillPayment
                //                          where c.PaymentAmount != p.PaymentAmount
                //                          select c.PaymentMode

                //                       ).Contains(p.PaymentMode)

                //                 select p).ToList();


                //paymentDelete = (from p in paymentResume.RestaurantKotBillPayment
                //                 where !(
                //                          from c in GuestBillPayment
                //                          select c.PaymentMode
                //                       ).Contains(p.PaymentMode)
                //                 select p).ToList();

                //var vcash = (from p in paymentResume.RestaurantKotBillPayment
                //             where p.PaymentMode == HMConstants.PaymentMode.Cash.ToString()
                //             select p).FirstOrDefault();

                //if (vcash == null)
                //{
                //    vcash = (from p in GuestBillPayment
                //             where p.PaymentMode == HMConstants.PaymentMode.Cash.ToString() &&
                //                   p.PaymentAmount != 0
                //             select p
                //             ).FirstOrDefault();

                //    if (vcash != null)
                //    {
                //        paymentAdded.Add(vcash);
                //    }
                //}

                //var vrounded = (from p in paymentResume.RestaurantKotBillPayment
                //                where p.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                //                select p).FirstOrDefault();

                //if (vrounded == null)
                //{
                //    vrounded = (from p in GuestBillPayment
                //                where p.PaymentMode == HMConstants.PaymentMode.Rounded.ToString() &&
                //                     p.PaymentAmount != 0
                //                select p
                //             ).FirstOrDefault();

                //    if (vrounded != null)
                //    {
                //        paymentAdded.Add(vrounded);
                //    }
                //}

                //var vrefund = (from p in paymentResume.RestaurantKotBillPayment
                //               where p.PaymentMode == HMConstants.PaymentMode.Refund.ToString()
                //               select p).FirstOrDefault();

                //if (vrefund == null)
                //{
                //    vrefund = (from p in GuestBillPayment
                //               where p.PaymentMode == HMConstants.PaymentMode.Refund.ToString() &&
                //                     p.PaymentAmount != 0
                //               select p
                //             ).FirstOrDefault();

                //    if (vrefund != null)
                //    {
                //        paymentAdded.Add(vrefund);
                //    }
                //}

                //var vcard = (from p in paymentResume.RestaurantKotBillPayment
                //             where p.PaymentMode == HMConstants.PaymentMode.Card.ToString()
                //             select p).ToList();

                //var vcardNew = (from p in GuestBillPayment
                //                where p.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                //                      p.PaymentAmount != 0
                //                select p).ToList();

                //if (vcard != null)
                //{
                //    foreach (GuestBillPaymentBO bp in vcardNew)
                //    {
                //        var v1 = (from pr in vcard
                //                  where pr.CardType == bp.CardType &&
                //                        pr.PaymentAmount != 0
                //                  select pr).FirstOrDefault();

                //        if (v1 == null)
                //        {
                //            paymentAdded.Add(bp);
                //        }
                //    }
                //}

                //var update1 = (from bp in GuestBillPayment
                //               from pr in paymentResume.RestaurantKotBillPayment
                //               where bp.PaymentMode == pr.PaymentMode &&
                //                     (
                //                         bp.PaymentMode == HMConstants.PaymentMode.Cash.ToString() ||
                //                         bp.PaymentMode == HMConstants.PaymentMode.Refund.ToString() ||
                //                         bp.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                //                     ) &&
                //                     bp.PaymentAmount != pr.PaymentAmount &&
                //                     bp.PaymentAmount != 0
                //               select new GuestBillPaymentBO
                //               {
                //                   PaymentId = pr.PaymentId,
                //                   BillNumber = pr.BillNumber,
                //                   ModuleName = pr.ModuleName,
                //                   PaymentType = pr.PaymentType,
                //                   RegistrationId = pr.RegistrationId,
                //                   PaymentDate = pr.PaymentDate,
                //                   CurrencyAmount = pr.CurrencyAmount,
                //                   PaymentAmount = bp.PaymentAmount,
                //                   ServiceBillId = pr.ServiceBillId,
                //                   PaymentMode = pr.PaymentMode,
                //                   BankId = pr.BankId,
                //                   BranchName = pr.BranchName,
                //                   ChecqueNumber = pr.ChecqueNumber,
                //                   CardType = pr.CardType,
                //                   CardNumber = pr.CardNumber,
                //                   CardHolderName = pr.CardHolderName,
                //                   CardReference = pr.CardReference
                //               }).ToList();

                //var update2 = (from pr in paymentResume.RestaurantKotBillPayment
                //               from bp in GuestBillPayment
                //               where pr.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                //                     pr.CardType == bp.CardType &&
                //                     pr.PaymentAmount != bp.PaymentAmount &&
                //                     bp.PaymentAmount != 0
                //               select new GuestBillPaymentBO
                //               {
                //                   PaymentId = pr.PaymentId,
                //                   BillNumber = pr.BillNumber,
                //                   ModuleName = pr.ModuleName,
                //                   PaymentType = pr.PaymentType,
                //                   RegistrationId = pr.RegistrationId,
                //                   PaymentDate = pr.PaymentDate,
                //                   CurrencyAmount = pr.CurrencyAmount,
                //                   PaymentAmount = bp.PaymentAmount,
                //                   ServiceBillId = pr.ServiceBillId,
                //                   PaymentMode = pr.PaymentMode,
                //                   BankId = pr.BankId,
                //                   BranchName = pr.BranchName,
                //                   ChecqueNumber = pr.ChecqueNumber,
                //                   CardType = pr.CardType,
                //                   CardNumber = pr.CardNumber,
                //                   CardHolderName = pr.CardHolderName,
                //                   CardReference = pr.CardReference
                //               }).ToList();

                //paymentUpdate.AddRange(update1);
                //paymentUpdate.AddRange(update2);

                //var delete1 = (from pr in paymentResume.RestaurantKotBillPayment
                //               from bp in GuestBillPayment
                //               where pr.PaymentMode == bp.PaymentMode &&
                //                     (
                //                         pr.PaymentMode == HMConstants.PaymentMode.Cash.ToString() ||
                //                         pr.PaymentMode == HMConstants.PaymentMode.Refund.ToString() ||
                //                         bp.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                //                     ) &&
                //                     bp.PaymentAmount == 0
                //               select new GuestBillPaymentBO
                //               {
                //                   PaymentId = pr.PaymentId,
                //                   BillNumber = pr.BillNumber,
                //                   ModuleName = pr.ModuleName,
                //                   PaymentType = pr.PaymentType,
                //                   RegistrationId = pr.RegistrationId,
                //                   PaymentDate = pr.PaymentDate,
                //                   PaymentAmount = bp.PaymentAmount,
                //                   ServiceBillId = pr.ServiceBillId,
                //                   PaymentMode = pr.PaymentMode,
                //                   BankId = pr.BankId,
                //                   BranchName = pr.BranchName,
                //                   ChecqueNumber = pr.ChecqueNumber,
                //                   CardType = pr.CardType,
                //                   CardNumber = pr.CardNumber,
                //                   CardHolderName = pr.CardHolderName,
                //                   CardReference = pr.CardReference
                //               }).ToList();

                //var delete2 = (from pr in paymentResume.RestaurantKotBillPayment
                //               from bp in GuestBillPayment
                //               where pr.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                //                     pr.CardType == bp.CardType &&
                //                     bp.PaymentAmount == 0
                //               select new GuestBillPaymentBO
                //               {
                //                   PaymentId = pr.PaymentId,
                //                   BillNumber = pr.BillNumber,
                //                   ModuleName = pr.ModuleName,
                //                   PaymentType = pr.PaymentType,
                //                   RegistrationId = pr.RegistrationId,
                //                   PaymentDate = pr.PaymentDate,
                //                   PaymentAmount = bp.PaymentAmount,
                //                   ServiceBillId = pr.ServiceBillId,
                //                   PaymentMode = pr.PaymentMode,
                //                   BankId = pr.BankId,
                //                   BranchName = pr.BranchName,
                //                   ChecqueNumber = pr.ChecqueNumber,
                //                   CardType = pr.CardType,
                //                   CardNumber = pr.CardNumber,
                //                   CardHolderName = pr.CardHolderName,
                //                   CardReference = pr.CardReference
                //               }).ToList();

                //paymentDelete.AddRange(delete1);
                //paymentDelete.AddRange(delete2);

                int billID = 0;
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
                List<string> strCategoryIdList = new List<string>();

                if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                {
                    strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                    foreach (string rowCWPD in strCategoryIdList)
                    {
                        RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO();
                        categoryWisePercentageDiscountBO.ClassificationId = Convert.ToInt32(rowCWPD);
                        categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);
                    }
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = paymentResume.RestaurantKotBill.BillDate;
                RestaurantBill.BillPaymentDate = paymentResume.RestaurantKotBill.BillPaymentDate;

                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                restaurentBillDetailBO.KotId = RestaurantBill.KotId;
                restaurentBillDetailBOList.Add(restaurentBillDetailBO);

                bool billId = restaurentBillDA.UpdateRestaurantBillForHoldUp(RestaurantBill, restaurentBillDetailBOList, paymentAdded, paymentUpdate, paymentDelete, categoryWisePercentageDiscountBOList);

                if (billId)
                {
                    KotBillMasterDA kotDa = new KotBillMasterDA();
                    //kotDa.UpdateKotBillMasterForHoldUp(RestaurantBill.KotId, true);

                    rtninf.IsSuccess = true;
                    rtninf.RedirectUrl = "frmCostCenterSelection.aspx";
                    rtninf.IsBillHoldUp = true;

                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                    HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                    HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                    HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                    HttpContext.Current.Session.Remove("RestaurantKotBillResume");
                    HttpContext.Current.Session.Remove("KotHoldupBill");
                    HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountType");
                    HttpContext.Current.Session.Remove("IRRestaurantBillDiscountAmount");
                    HttpContext.Current.Session.Remove("IRGuestPaymentDetailListForGrid");
                    HttpContext.Current.Session.Remove("IRCostCenterIdSession");
                    HttpContext.Current.Session.Remove("IRtxtKotIdInformation");
                    HttpContext.Current.Session.Remove("IRCostCenterServiceChargeSession");
                    HttpContext.Current.Session.Remove("IRCostCenterVatAmountSession");
                    HttpContext.Current.Session.Remove("IRToeknNumber");
                    HttpContext.Current.Session.Remove("IRtxtTableIdInformation");
                    HttpContext.Current.Session.Remove("IRtxtTableNumberInformation");
                    HttpContext.Current.Session.Remove("tbsMessage");
                    HttpContext.Current.Session.Remove("IRTableAllocatedBy");
                    HttpContext.Current.Session.Remove("IRtxtBearerIdInformation");

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
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

        // ----------------------------------- New Touch Screen Bill Generation -----------------------------------------------------------------------------------------
        [WebMethod]
        public static ReturnInfo SaveRestauranBillForPreview(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment, List<RestaurantBillDetailBO> BillDetail)
        {
            ReturnInfo rtninf = new ReturnInfo();
            string categoryIdList = string.Empty;

            try
            {
                int billID = 0;
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();

                // List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
                List<string> strCategoryIdList = new List<string>();

                if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                {
                    strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                    foreach (string rowCWPD in strCategoryIdList)
                    {
                        RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO();
                        categoryWisePercentageDiscountBO.ClassificationId = Convert.ToInt32(rowCWPD);
                        categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);

                        categoryIdList += categoryIdList != string.Empty ? ("," + rowCWPD) : rowCWPD;
                    }
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.BearerId = RestaurantBill.BearerId != 0 ? RestaurantBill.BearerId : userInformationBO.UserInfoId; ;
                RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = DateTime.Now;
                RestaurantBill.BillPaymentDate = DateTime.Now;

                var isRoomWisePayment = GuestBillPayment.Where(p => p.PaymentMode == "Other Room").FirstOrDefault();
                if (isRoomWisePayment != null || RestaurantBill.RoomId > 0)
                {
                    RoomRegistrationDA roomDa = new RoomRegistrationDA();
                    RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
                    roomRegistration = roomDa.GetRoomRegistrationNRoomDetailsByRoomId(RestaurantBill.RoomId); //GetRoomRegistrationInfoById, GetRoomRegistrationNRoomDetailsByRoomId                    
                    RestaurantBill.RegistrationId = roomRegistration.RegistrationId;

                    RestaurantBill.CustomerName = string.IsNullOrEmpty(RestaurantBill.CustomerName) ? (roomRegistration.GuestName + ",Room# " + roomRegistration.RoomNumber) : RestaurantBill.CustomerName;
                }

                int billId = restaurentBillDA.SaveRestaurantBillForAll(RestaurantBill, BillDetail, GuestBillPayment, null, categoryIdList, true, true, out billID);

                if (billId > 0)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RestaurantBill.ToString(), billID,
                                ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), "Bill Preview");
                    //Boolean settlementStatus = restaurentBillDA.RestaurantBillSettlementInfoByBillId(billId);
                    rtninf.IsSuccess = true;
                }

                if (rtninf.IsSuccess)
                {
                    HMCommonSetupBO commonRestaurantBillPrintAndPreview = new HMCommonSetupBO();
                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                    commonRestaurantBillPrintAndPreview = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillPrintAndPreview", "RestaurantBillPrintAndPreview");

                    rtninf.Pk = billId;
                    rtninf.BillPrintAndPreview = Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue);
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    if (rtninf.BillPrintAndPreview == (int)(HMConstants.RestaurantBillPrintAndPreview.PrintAndPreview))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(billId);

                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    }
                    else if (rtninf.BillPrintAndPreview == (int)(HMConstants.RestaurantBillPrintAndPreview.DirectPrint))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(billId);

                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    }
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
        public static ReturnInfo SaveRestaurantBillForAll(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment,
                                                          List<RestaurantBillDetailBO> BillDetail, List<ItemClassificationBO> AddedClassificationList)
        {
            ReturnInfo rtninf = new ReturnInfo();
            string categoryIdList = string.Empty, categoryIdAndDiscountList = string.Empty, kotIdList = string.Empty;

            try
            {
                int billID = 0;
                KotBillMasterDA kotDa = new KotBillMasterDA();
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
                paymentResume.RestaurantKotBill = restaurentBillDA.GetRestaurantBillByKotId(RestaurantBill.KotId, RestaurantBill.SourceName);

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

                if (AddedClassificationList != null)
                {
                    foreach (ItemClassificationBO itmcls in AddedClassificationList)
                    {
                        categoryIdList += categoryIdList != string.Empty ? ("," + itmcls.ClassificationId.ToString()) : itmcls.ClassificationId.ToString();
                        categoryIdAndDiscountList += categoryIdAndDiscountList != string.Empty ? ("," + itmcls.ClassificationId.ToString() + "#" + itmcls.DiscountAmount.ToString()) : (itmcls.ClassificationId.ToString() + "#" + itmcls.DiscountAmount.ToString());
                    }
                }

                if (BillDetail != null)
                {
                    foreach (RestaurantBillDetailBO kot in BillDetail)
                    {
                        kotIdList += kotIdList != string.Empty ? ("," + kot.KotId.ToString()) : kot.KotId.ToString();
                    }
                }

                bool IsRestaurantBillAmountWillRound = false;
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
                isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillAmountWillRound", "IsRestaurantBillAmountWillRound");
                IsRestaurantBillAmountWillRound = (isRestaurantBillAmountWillRoundBO.SetupValue.ToString() == "0" ? false : true);

                KotBillDetailDA entityDA = new KotBillDetailDA();
                List<KotBillDetailBO> kotDetails = entityDA.GetRestaurantOrderItemByMultipleKotId(RestaurantBill.CostCenterId.ToString(), kotIdList, RestaurantBill.SourceName).Where(x => x.ItemUnit > 0).ToList();

                KotBillMasterBO kotBillMaster = new KotBillMasterBO();

                if (RestaurantBill.SourceName == ConstantHelper.RestaurantBillSource.RestaurantTable.ToString())
                {
                    kotBillMaster = kotDa.GetKotBillMasterInfoByKotIdNSourceName(RestaurantBill.KotId, RestaurantBill.SourceName);

                    if (kotBillMaster.KotStatus != "pending")
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Kot Already " + kotBillMaster.KotStatus + ". Redirect Within a Short Time.", AlertType.Error);
                        return rtninf;
                    }

                    if (kotBillMaster.SourceId != RestaurantBill.TableId)
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Source Table Is Changed. Redirect Within a Short Time.", AlertType.Error);
                        return rtninf;
                    }
                }

                bool isBillCanSettle = false;
                decimal totalQuantity = 0, totalPrice = 0;
                totalQuantity = Math.Round(kotDetails.Sum(s => s.ItemUnit), 2);
                totalPrice = Math.Round(kotDetails.Sum(s => s.ItemUnit * s.UnitRate), 2);

                if (RestaurantBill.TotalQuantity == totalQuantity && RestaurantBill.TotalPrice == totalPrice)
                {
                    isBillCanSettle = true;
                }

                if (!isBillCanSettle)
                {
                    rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    rtninf.IsBillResettled = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo("Invoice Grand Total Is Not Same As Item Grand Total. Redirect Within a Short Time.", AlertType.Error);
                    return rtninf;
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.BearerId = RestaurantBill.BearerId != 0 ? RestaurantBill.BearerId : userInformationBO.UserInfoId; ;
                RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = DateTime.Now;
                RestaurantBill.BillPaymentDate = DateTime.Now;

                //RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                //restaurentBillDetailBO.KotId = RestaurantBill.KotId;
                //restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

                var isRoomWisePayment = GuestBillPayment.Where(p => p.PaymentMode == "Other Room").FirstOrDefault();
                if (isRoomWisePayment != null || RestaurantBill.RoomId > 0)
                {
                    RoomRegistrationDA roomDa = new RoomRegistrationDA();

                    roomRegistration = roomDa.GetRoomRegistrationNRoomDetailsByRoomId(RestaurantBill.RoomId); //GetRoomRegistrationInfoById, GetRoomRegistrationNRoomDetailsByRoomId                    
                    RestaurantBill.RegistrationId = roomRegistration.RegistrationId;

                    RestaurantBill.CustomerName = string.IsNullOrEmpty(RestaurantBill.CustomerName) ? (roomRegistration.GuestName + ",Room# " + roomRegistration.RoomNumber) : RestaurantBill.CustomerName;
                }

                string paymentInfo = string.Empty;

                foreach (GuestBillPaymentBO p in GuestBillPayment)
                {
                    if (p.PaymentAmount > 0)
                    {
                        if (p.PaymentMode != "Other Room" && p.PaymentMode != "Rounded")
                        {
                            paymentInfo = paymentInfo + (!string.IsNullOrEmpty(paymentInfo) ? "," : "")
                                                      + p.PaymentMode + (!string.IsNullOrEmpty(p.PaymentDescription) ? "(" + p.PaymentDescription + ")" : "")
                                                      + ":" + p.PaymentAmount.ToString();
                        }
                        else if (p.PaymentMode != "Rounded")
                        {
                            paymentInfo = paymentInfo + (!string.IsNullOrEmpty(paymentInfo) ? "," : "")
                                                      + "Room#" + roomRegistration.RoomNumber
                                                      + ":" + p.PaymentAmount.ToString();

                            p.PaymentDescription = "Room#" + roomRegistration.RoomNumber;
                        }
                    }
                    else if (p.PaymentAmount == 0 && RestaurantBill.IsNonChargeable)
                    {
                        if (RestaurantBill.RoomId > 0)
                        {
                            paymentInfo = "Non Chargeable(NC)" + " Room#" + roomRegistration.RoomNumber; ;
                        }
                        else
                        {
                            paymentInfo = "Non Chargeable(NC)";
                        }
                    }
                    else if (p.PaymentAmount == 0 && RestaurantBill.IsComplementary)
                    {
                        if (RestaurantBill.RoomId > 0)
                        {
                            paymentInfo = "Complementary" + " Room#" + roomRegistration.RoomNumber; ;
                        }
                        else
                        {
                            if (RestaurantBill.RoomId > 0)
                            {
                                paymentInfo = "Complementary" + " Room#" + roomRegistration.RoomNumber; ;
                            }
                            else
                            {
                                paymentInfo = "Complementary";
                            }
                        }
                    }
                }
                RestaurantBill.PaymentRemarks = paymentInfo;

                int billId = restaurentBillDA.SaveRestaurantBillForAll(RestaurantBill, BillDetail, GuestBillPayment, AddedClassificationList, categoryIdList, true, true, out billID);

                if (billId > 0)
                {
                    //Boolean settlementStatus = restaurentBillDA.RestaurantBillSettlementInfoByBillId(billId);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RestaurantBill.ToString(), billID,
                                ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantBill));

                    rtninf.IsSuccess = true;
                }
                else { rtninf.IsSuccess = false; }

                if (rtninf.IsSuccess)
                {
                    HMCommonSetupBO commonRestaurantBillPrintAndPreview = new HMCommonSetupBO();

                    commonRestaurantBillPrintAndPreview = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillPrintAndPreview", "RestaurantBillPrintAndPreview");

                    rtninf.Pk = billId;
                    rtninf.BillPrintAndPreview = Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue);
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    if (rtninf.BillPrintAndPreview == (int)(HMConstants.RestaurantBillPrintAndPreview.DirectPrint))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(billId);

                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    }
                    else if (rtninf.BillPrintAndPreview == (int)(HMConstants.RestaurantBillPrintAndPreview.PrintAndPreview))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(billId);
                    }
                    else if (rtninf.BillPrintAndPreview == (int)(HMConstants.RestaurantBillPrintAndPreview.BillPreviewOnly))
                    {
                        //frmRestaurantManagement manage = new frmRestaurantManagement();
                        //manage.PrintRestaurantBill(RestaurantBill.BillId);
                    }
                    else
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    }
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error + ex.Message);
            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo HoldUpRestauranBillGenerationNew(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment,
                                                                  List<RestaurantBillDetailBO> BillDetail, List<ItemClassificationBO> AddedClassificationList)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                int billID = 0;

                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
                paymentResume.RestaurantKotBill = restaurentBillDA.GetRestaurantBillByKotId(RestaurantBill.KotId, RestaurantBill.SourceName);

                rtninf.IsBillResettled = false;

                if (paymentResume.RestaurantKotBill != null)
                {
                    if (paymentResume.RestaurantKotBill.IsBillSettlement)
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Bill Already Settled. Redirect Within a Ahort Time.", AlertType.Error);

                        return rtninf;
                    }
                }

                KotBillMasterDA kotDa = new KotBillMasterDA();
                KotBillMasterBO kotBillMaster = new KotBillMasterBO();

                if (RestaurantBill.SourceName == ConstantHelper.RestaurantBillSource.RestaurantTable.ToString())
                {
                    kotBillMaster = kotDa.GetKotBillMasterInfoByKotIdNSourceName(RestaurantBill.KotId, RestaurantBill.SourceName);

                    if (kotBillMaster.KotStatus != "pending")
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Kot Already " + kotBillMaster.KotStatus + ". Redirect Within a Short Time.", AlertType.Error);
                        return rtninf;
                    }

                    if (kotBillMaster.SourceId != RestaurantBill.TableId)
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Source Table Is Changed. Redirect Within a Short Time.", AlertType.Error);
                        return rtninf;
                    }
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.BearerId = RestaurantBill.BearerId != 0 ? RestaurantBill.BearerId : userInformationBO.UserInfoId;
                RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = DateTime.Now;
                RestaurantBill.BillPaymentDate = DateTime.Now;

                GuestBillPayment = null;

                if (RestaurantBill.SourceName == "GuestRoom" || RestaurantBill.RoomId > 0)
                {
                    RoomRegistrationDA roomDa = new RoomRegistrationDA();
                    RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
                    roomRegistration = roomDa.GetRoomRegistrationNRoomDetailsByRoomId(RestaurantBill.RoomId); //GetRoomRegistrationInfoById, GetRoomRegistrationNRoomDetailsByRoomId                    
                    RestaurantBill.RegistrationId = roomRegistration.RegistrationId;

                    RestaurantBill.CustomerName = string.IsNullOrEmpty(RestaurantBill.CustomerName) ? ("Room# " + roomRegistration.RoomNumber + roomRegistration.GuestName) : RestaurantBill.CustomerName;
                }


                int billId = restaurentBillDA.SaveRestaurantBillForNewHoldUp(RestaurantBill, BillDetail, GuestBillPayment, AddedClassificationList, out billID);

                if (billId > 0)
                {

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RestaurantBill.ToString(), billID,
                                ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), "Bill Hold Up");

                    //kotDa.UpdateKotBillMasterForHoldUp(RestaurantBill.KotId, true);

                    rtninf.IsSuccess = true;
                    rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    rtninf.IsBillHoldUp = true;
                    rtninf.Pk = billId;

                    InvCategoryDA catDa = new InvCategoryDA();
                    RestaurentBillDA billDa = new RestaurentBillDA();
                    KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();

                    List<ItemClassificationBO> classificationLst = new List<ItemClassificationBO>();
                    List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();
                    List<RestaurantBillDetailBO> billDetailList = new List<RestaurantBillDetailBO>();

                    RestaurantBillBO kotBill = new RestaurantBillBO();
                    List<GuestBillPaymentBO> kotBillPayment = new List<GuestBillPaymentBO>();

                    kotBillMaster = kotBillMasterDA.GetKotBillMasterInfoKotId(RestaurantBill.KotId);
                    kotDetails = kotDa.GetKotBillDetailInfoByKotId(kotBillMaster.KotId);

                    kotBill = billDa.GetRestaurantBillByKotId(kotBillMaster.KotId, kotBillMaster.SourceName);
                    classificationLst = catDa.GetRestaurantBillClassificationDetailsByBillId(kotBill.BillId);

                    billDetailList = billDa.GetRestaurantBillDetailsByBillId(kotBill.BillId);

                    if (classificationLst.Count > 0)
                        rtninf.ObjectList = new ArrayList(classificationLst);

                    if (billDetailList.Count > 0)
                    {
                        foreach (RestaurantBillDetailBO bd in billDetailList)
                        {
                            bd.MainBillId = kotBill.BillId;
                        }

                        rtninf.ObjectList1 = new ArrayList(billDetailList);
                    }

                    if (kotBill != null)
                    {
                        kotBillPayment = billDa.GetBillPaymentByBillId(kotBill.BillId, "Restaurant");
                    }

                    paymentResume.KotBillMaster = kotBillMaster;
                    paymentResume.KotBillDetails = kotDetails;
                    paymentResume.RestaurantKotBill = kotBill;
                    paymentResume.RestaurantKotBillPayment = kotBillPayment;

                    //HttpContext.Current.Session["RestaurantKotBillResume"] = paymentResume;

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
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
        public static ReturnInfo UpdateRestauranBillGenerationNew(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment,
                                                                  List<RestaurantBillDetailBO> BillDetail, List<RestaurantBillDetailBO> BillDeletedDetail,
                                                                  List<ItemClassificationBO> AddedClassificationList, List<ItemClassificationBO> DeletedClassificationList
                                                                 )
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
                paymentResume.RestaurantKotBill = restaurentBillDA.GetRestaurantBillByKotId(RestaurantBill.KotId, RestaurantBill.SourceName);

                rtninf.IsBillResettled = false;

                if (paymentResume.RestaurantKotBill != null)
                {
                    if (paymentResume.RestaurantKotBill.IsBillSettlement)
                    {

                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Bill Already Settled. Redirect Within a Ahort Time.", AlertType.Error);

                        return rtninf;
                    }
                }

                KotBillMasterDA kotDa = new KotBillMasterDA();
                KotBillMasterBO kotBillMaster = new KotBillMasterBO();

                if (RestaurantBill.SourceName == ConstantHelper.RestaurantBillSource.RestaurantTable.ToString())
                {
                    kotBillMaster = kotDa.GetKotBillMasterInfoByKotIdNSourceName(RestaurantBill.KotId, RestaurantBill.SourceName);

                    if (kotBillMaster.KotStatus != "pending")
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Kot Already " + kotBillMaster.KotStatus + ". Redirect Within a Short Time.", AlertType.Error);
                        return rtninf;
                    }

                    if (kotBillMaster.SourceId != RestaurantBill.TableId)
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Source Table Is Changed. Redirect Within a Short Time.", AlertType.Error);
                        return rtninf;
                    }
                }

                List<RestaurantBillDetailBO> billDetailList = new List<RestaurantBillDetailBO>();
                List<GuestBillPaymentBO> paymentAdded = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentUpdate = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentDelete = new List<GuestBillPaymentBO>();
                RestaurantBill.BillId = paymentResume.RestaurantKotBill.BillId;


                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = paymentResume.RestaurantKotBill.BillDate;
                RestaurantBill.BillPaymentDate = paymentResume.RestaurantKotBill.BillPaymentDate;

                if (RestaurantBill.SourceName == "GuestRoom" || RestaurantBill.RoomId > 0)
                {
                    RoomRegistrationDA roomDa = new RoomRegistrationDA();
                    RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
                    roomRegistration = roomDa.GetRoomRegistrationNRoomDetailsByRoomId(RestaurantBill.RoomId); //GetRoomRegistrationInfoById, GetRoomRegistrationNRoomDetailsByRoomId                    
                    RestaurantBill.RegistrationId = roomRegistration.RegistrationId;

                    RestaurantBill.CustomerName = string.IsNullOrEmpty(RestaurantBill.CustomerName) ? ("Room# " + roomRegistration.RoomNumber + roomRegistration.GuestName) : RestaurantBill.CustomerName;
                }

                bool billId = restaurentBillDA.UpdateRestaurantBillForNewHoldUp(RestaurantBill, BillDetail, BillDeletedDetail, paymentAdded, paymentUpdate, paymentDelete, AddedClassificationList, DeletedClassificationList);

                if (billId)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RestaurantBill.ToString(), RestaurantBill.BillId,
                                ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), "Update Bill Holdup");

                    //kotDa.UpdateKotBillMasterForHoldUp(RestaurantBill.KotId, true);

                    rtninf.IsSuccess = true;
                    rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    rtninf.IsBillHoldUp = true;
                    rtninf.Pk = RestaurantBill.BillId;

                    InvCategoryDA catDa = new InvCategoryDA();
                    List<ItemClassificationBO> classificationLst = new List<ItemClassificationBO>();

                    classificationLst = catDa.GetRestaurantBillClassificationDetailsByBillId(RestaurantBill.BillId);
                    billDetailList = restaurentBillDA.GetRestaurantBillDetailsByBillId(RestaurantBill.BillId);

                    if (classificationLst.Count > 0)
                        rtninf.ObjectList = new ArrayList(classificationLst);

                    if (billDetailList.Count > 0)
                    {
                        foreach (RestaurantBillDetailBO bd in billDetailList)
                        {
                            bd.MainBillId = RestaurantBill.BillId;
                        }

                        rtninf.ObjectList1 = new ArrayList(billDetailList);
                    }

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
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
        public static ReturnInfo UpdateRestauranBillGenerationNewSettlement(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment,
                                                                            List<RestaurantBillDetailBO> BillDetail, List<RestaurantBillDetailBO> BillDeletedDetail,
                                                                            List<ItemClassificationBO> AddedClassificationList, List<ItemClassificationBO> DeletedClassificationList)
        {
            ReturnInfo rtninf = new ReturnInfo();
            string categoryIdList = string.Empty, categoryIdAndDiscountList = string.Empty, kotIdList = string.Empty;


            try
            {
                KotBillMasterDA kotDa = new KotBillMasterDA();
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
                paymentResume.RestaurantKotBill = restaurentBillDA.GetRestaurantBillByKotId(RestaurantBill.KotId, RestaurantBill.SourceName);

                rtninf.IsBillResettled = false;

                if (paymentResume.RestaurantKotBill != null)
                {
                    if (paymentResume.RestaurantKotBill.IsBillSettlement)
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Bill Already Settled. Redirect Within a Ahort Time.", AlertType.Error);

                        return rtninf;
                    }
                }

                KotBillMasterBO kotBillMaster = new KotBillMasterBO();

                if (RestaurantBill.SourceName == ConstantHelper.RestaurantBillSource.RestaurantTable.ToString())
                {
                    kotBillMaster = kotDa.GetKotBillMasterInfoByKotIdNSourceName(RestaurantBill.KotId, RestaurantBill.SourceName);

                    if (kotBillMaster.KotStatus != "pending")
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Kot Already " + kotBillMaster.KotStatus + ". Redirect Within a Short Time.", AlertType.Error);
                        return rtninf;
                    }

                    if (kotBillMaster.SourceId != RestaurantBill.TableId)
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Source Table Is Changed. Redirect Within a Short Time.", AlertType.Error);
                        return rtninf;
                    }
                }

                List<GuestBillPaymentBO> paymentAdded = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentUpdate = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentDelete = new List<GuestBillPaymentBO>();
                RestaurantBill.BillId = paymentResume.RestaurantKotBill.BillId;

                paymentAdded = GuestBillPayment;

                int billID = 0;

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
                List<string> strCategoryIdList = new List<string>();

                //if (AddedClassificationList != null)
                //{
                //    if (AddedClassificationList.Count > 0)
                //    {
                //        foreach (ItemClassificationBO itmcls in AddedClassificationList)
                //        {
                //            categoryIdList += categoryIdList != string.Empty ? ("," + itmcls.ClassificationId.ToString()) : itmcls.ClassificationId.ToString();
                //        }

                //        AddedClassificationList = AddedClassificationList.Where(c => c.DiscountId == 0).ToList();
                //    }
                //}

                if (AddedClassificationList != null)
                {
                    if (AddedClassificationList.Count > 0)
                    {
                        foreach (ItemClassificationBO itmcls in AddedClassificationList)
                        {
                            categoryIdList += categoryIdList != string.Empty ? ("," + itmcls.ClassificationId.ToString()) : itmcls.ClassificationId.ToString();
                            categoryIdAndDiscountList += categoryIdAndDiscountList != string.Empty ? ("," + itmcls.ClassificationId.ToString() + "#" + itmcls.DiscountAmount.ToString()) : (itmcls.ClassificationId.ToString() + "#" + itmcls.DiscountAmount.ToString());
                        }

                        AddedClassificationList = AddedClassificationList.Where(c => c.DiscountId == 0).ToList();
                    }
                }

                if (BillDetail != null)
                {
                    foreach (RestaurantBillDetailBO kot in BillDetail)
                    {
                        kotIdList += kotIdList != string.Empty ? ("," + kot.KotId.ToString()) : kot.KotId.ToString();
                    }
                }

                bool IsRestaurantBillAmountWillRound = false;
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
                isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillAmountWillRound", "IsRestaurantBillAmountWillRound");
                IsRestaurantBillAmountWillRound = (isRestaurantBillAmountWillRoundBO.SetupValue.ToString() == "0" ? false : true);

                KotBillDetailDA entityDA = new KotBillDetailDA();
                List<KotBillDetailBO> kotDetails = entityDA.GetRestaurantOrderItemByMultipleKotId(RestaurantBill.CostCenterId.ToString(), kotIdList, RestaurantBill.SourceName).Where(x => x.ItemUnit > 0).ToList();

                bool isBillCanSettle = false;
                decimal totalQuantity = 0, totalPrice = 0;
                totalQuantity = Math.Round(kotDetails.Sum(s => s.ItemUnit), 2);
                totalPrice = Math.Round(kotDetails.Sum(s => s.ItemUnit * s.UnitRate), 2);

                RestaurantBill.TotalQuantity = Math.Round(RestaurantBill.TotalQuantity, 2);
                RestaurantBill.TotalPrice = Math.Round(RestaurantBill.TotalPrice, 2);

                if (RestaurantBill.TotalQuantity == totalQuantity && RestaurantBill.TotalPrice == totalPrice)
                {
                    isBillCanSettle = true;
                }

                if (!isBillCanSettle)
                {
                    rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    rtninf.IsBillResettled = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo("Invoice Grand Total Is Not Same As Item Grand Total. Redirect Within a Short Time.", AlertType.Error);
                    return rtninf;
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = paymentResume.RestaurantKotBill.BillDate;
                RestaurantBill.BillPaymentDate = paymentResume.RestaurantKotBill.BillPaymentDate;

                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                restaurentBillDetailBO.BillId = RestaurantBill.BillId;
                restaurentBillDetailBO.KotId = RestaurantBill.KotId;
                restaurentBillDetailBOList.Add(restaurentBillDetailBO);

                var isRoomWisePayment = GuestBillPayment.Where(p => p.PaymentMode == "Other Room").FirstOrDefault();
                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

                if (isRoomWisePayment != null || RestaurantBill.RoomId > 0)
                {
                    RoomRegistrationDA roomDa = new RoomRegistrationDA();

                    roomRegistration = roomDa.GetRoomRegistrationNRoomDetailsByRoomId(RestaurantBill.RoomId); //GetRoomRegistrationInfoById, GetRoomRegistrationNRoomDetailsByRoomId                    
                    RestaurantBill.RegistrationId = roomRegistration.RegistrationId;

                    RestaurantBill.CustomerName = string.IsNullOrEmpty(RestaurantBill.CustomerName) ? ("Room# " + roomRegistration.RoomNumber + roomRegistration.GuestName) : RestaurantBill.CustomerName;
                }

                string paymentInfo = string.Empty;

                foreach (GuestBillPaymentBO p in GuestBillPayment)
                {
                    if (p.PaymentAmount > 0)
                    {
                        if (p.PaymentMode != "Other Room" && p.PaymentMode != "Rounded")
                        {
                            paymentInfo = paymentInfo + (!string.IsNullOrEmpty(paymentInfo) ? "," : "")
                                                      + p.PaymentMode + (!string.IsNullOrEmpty(p.PaymentDescription) ? "(" + p.PaymentDescription + ")" : "")
                                                      + ":" + p.PaymentAmount.ToString();
                        }
                        else if (p.PaymentMode != "Rounded")
                        {
                            paymentInfo = paymentInfo + (!string.IsNullOrEmpty(paymentInfo) ? "," : "")
                                                      + "Room#" + roomRegistration.RoomNumber
                                                      + ":" + p.PaymentAmount.ToString();

                            p.PaymentDescription = "Room#" + roomRegistration.RoomNumber;
                        }
                    }
                    else if (p.PaymentAmount == 0 && RestaurantBill.IsNonChargeable)
                    {
                        if (RestaurantBill.RoomId > 0)
                        {
                            paymentInfo = "Non Chargeable(NC)" + " Room#" + roomRegistration.RoomNumber; ;
                        }
                        else
                        {
                            paymentInfo = "Non Chargeable(NC)";
                        }
                    }
                    else if (p.PaymentAmount == 0 && RestaurantBill.IsComplementary)
                    {
                        if (RestaurantBill.RoomId > 0)
                        {
                            paymentInfo = "Complementary" + " Room#" + roomRegistration.RoomNumber; ;
                        }
                        else
                        {
                            paymentInfo = "Complementary";
                        }
                    }
                }
                RestaurantBill.PaymentRemarks = paymentInfo;

                bool billId = restaurentBillDA.UpdateRestauranBillGenerationNewSettlement(RestaurantBill, BillDetail, BillDeletedDetail, paymentAdded, paymentUpdate, paymentDelete, AddedClassificationList, DeletedClassificationList, categoryIdList, true);

                if (billId)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RestaurantBill.ToString(), RestaurantBill.BillId,
                                ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), "Update Bill");


                    rtninf.IsSuccess = true;
                }
                else { rtninf.IsSuccess = false; }

                if (rtninf.IsSuccess)
                {
                    HMCommonSetupBO commonRestaurantBillPrintAndPreview = new HMCommonSetupBO();

                    commonRestaurantBillPrintAndPreview = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillPrintAndPreview", "RestaurantBillPrintAndPreview");

                    rtninf.Pk = RestaurantBill.BillId;
                    rtninf.BillPrintAndPreview = Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue);
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    if (rtninf.BillPrintAndPreview == (int)(HMConstants.RestaurantBillPrintAndPreview.DirectPrint))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(RestaurantBill.BillId);

                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    }
                    else if (rtninf.BillPrintAndPreview == (int)(HMConstants.RestaurantBillPrintAndPreview.PrintAndPreview))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(RestaurantBill.BillId);
                    }
                    else if (rtninf.BillPrintAndPreview == (int)(HMConstants.RestaurantBillPrintAndPreview.BillPreviewOnly))
                    {
                        //frmRestaurantManagement manage = new frmRestaurantManagement();
                        //manage.PrintRestaurantBill(RestaurantBill.BillId);
                    }
                    else
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    }
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
        public static ReturnInfo HoldUpRestauranBillGenerationForAll(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                int billID = 0;
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                List<ItemClassificationBO> categoryWisePercentageDiscountBOList = new List<ItemClassificationBO>();
                List<string> strCategoryIdList = new List<string>();

                //if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                //{
                //    strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                //    foreach (string rowCWPD in strCategoryIdList)
                //    {
                //        RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO();
                //        categoryWisePercentageDiscountBO.ClassificationId = Convert.ToInt32(rowCWPD);
                //        categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);
                //    }
                //}

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.BearerId = RestaurantBill.BearerId != 0 ? RestaurantBill.BearerId : userInformationBO.UserInfoId; ;
                RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = DateTime.Now;
                RestaurantBill.BillPaymentDate = DateTime.Now;

                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                restaurentBillDetailBO.KotId = RestaurantBill.KotId;
                restaurentBillDetailBOList.Add(restaurentBillDetailBO);

                GuestBillPayment = null;

                int billId = restaurentBillDA.SaveRestaurantBillForNewHoldUp(RestaurantBill, restaurentBillDetailBOList, GuestBillPayment, categoryWisePercentageDiscountBOList, out billID);

                if (billId > 0)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RestaurantBill.ToString(), billID,
                               ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), "Save Bill Holdup");

                    //KotBillMasterDA kotDa = new KotBillMasterDA();
                    //kotDa.UpdateKotBillMasterForHoldUp(RestaurantBill.KotId, true);

                    rtninf.IsSuccess = true;
                    rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    rtninf.IsBillHoldUp = true;
                    rtninf.Pk = billId;

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
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
        public static ReturnInfo UpdateHoldUpRestauranBillGenerationForAll(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
                paymentResume.RestaurantKotBill = restaurentBillDA.GetRestaurantBillByKotId(RestaurantBill.KotId, RestaurantBill.SourceName);

                List<GuestBillPaymentBO> paymentAdded = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentUpdate = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentDelete = new List<GuestBillPaymentBO>();

                RestaurantBill.BillId = paymentResume.RestaurantKotBill.BillId;


                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();
                List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
                List<string> strCategoryIdList = new List<string>();

                if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                {
                    strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                    foreach (string rowCWPD in strCategoryIdList)
                    {
                        RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO();
                        categoryWisePercentageDiscountBO.ClassificationId = Convert.ToInt32(rowCWPD);
                        categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);
                    }
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = paymentResume.RestaurantKotBill.BillDate;
                RestaurantBill.BillPaymentDate = paymentResume.RestaurantKotBill.BillPaymentDate;

                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                restaurentBillDetailBO.KotId = RestaurantBill.KotId;
                restaurentBillDetailBOList.Add(restaurentBillDetailBO);

                bool billId = restaurentBillDA.UpdateRestaurantBillForHoldUpNew(RestaurantBill, restaurentBillDetailBOList, paymentAdded, paymentUpdate, paymentDelete, categoryWisePercentageDiscountBOList);

                if (billId)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RestaurantBill.ToString(), RestaurantBill.BillId,
                              ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), "Update Bill Holdup");

                    KotBillMasterDA kotDa = new KotBillMasterDA();
                    //kotDa.UpdateKotBillMasterForHoldUp(RestaurantBill.KotId, true);

                    rtninf.IsSuccess = true;
                    rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    rtninf.IsBillHoldUp = true;

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
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


        //--*** Split Bill Generation
        [WebMethod]
        public static ReturnInfo HoldUpRestauranBillGenerationSplitNew(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment,
                                                                List<RestaurantBillDetailBO> BillDetail, List<ItemClassificationBO> AddedClassificationList)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                int billID = 0;

                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
                paymentResume.RestaurantKotBill = restaurentBillDA.GetRestaurantBillByKotId(RestaurantBill.KotId, RestaurantBill.SourceName);

                rtninf.IsBillResettled = false;

                if (paymentResume.RestaurantKotBill != null)
                {
                    if (paymentResume.RestaurantKotBill.IsBillSettlement)
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Bill Already Settled. Redirect Within a Ahort Time.", AlertType.Error);

                        return rtninf;
                    }
                }

                KotBillMasterDA kotDa = new KotBillMasterDA();
                KotBillMasterBO kotBillMaster = new KotBillMasterBO();

                if (RestaurantBill.SourceName == ConstantHelper.RestaurantBillSource.RestaurantTable.ToString())
                {
                    kotBillMaster = kotDa.GetKotBillMasterInfoByKotIdNSourceName(RestaurantBill.KotId, RestaurantBill.SourceName);

                    if (kotBillMaster.KotStatus != "pending")
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Kot Already " + kotBillMaster.KotStatus + ". Redirect Within a Short Time.", AlertType.Error);
                        return rtninf;
                    }

                    if (kotBillMaster.SourceId != RestaurantBill.TableId)
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Source Table Is Changed. Redirect Within a Short Time.", AlertType.Error);
                        return rtninf;
                    }
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.BearerId = RestaurantBill.BearerId != 0 ? RestaurantBill.BearerId : userInformationBO.UserInfoId; ;
                RestaurantBill.CreatedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = DateTime.Now;
                RestaurantBill.BillPaymentDate = DateTime.Now;

                GuestBillPayment = null;

                int billId = restaurentBillDA.SaveRestaurantBillForNewHoldUp(RestaurantBill, BillDetail, GuestBillPayment, AddedClassificationList, out billID);

                if (billId > 0)
                {
                    //kotDa.UpdateKotBillMasterForHoldUp(RestaurantBill.KotId, true);

                    rtninf.IsSuccess = true;
                    rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    rtninf.IsBillHoldUp = true;
                    rtninf.Pk = billId;

                    RestaurentBillDA billDa = new RestaurentBillDA();
                    KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();

                    List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();
                    List<KotBillDetailBO> kotDetailsForSplit = new List<KotBillDetailBO>();
                    List<KotBillDetailBO> kotDetailsToDisplay = new List<KotBillDetailBO>();

                    RestaurantBillBO kotBill = new RestaurantBillBO();
                    List<GuestBillPaymentBO> kotBillPayment = new List<GuestBillPaymentBO>();

                    KotBillDetailDA entityDA = new KotBillDetailDA();
                    Boolean status = entityDA.UpdateKotBillDetailPrintFlagByOrderSubmitKotId(RestaurantBill.KotId);

                    kotBillMaster = kotBillMasterDA.GetKotBillMasterInfoKotId(RestaurantBill.KotId);
                    kotDetails = kotDa.GetKotBillDetailInfoByKotId(kotBillMaster.KotId);

                    kotBill = billDa.GetRestaurantBillByKotId(kotBillMaster.KotId, kotBillMaster.SourceName);

                    if (kotBill != null)
                    {
                        kotBillPayment = billDa.GetBillPaymentByBillId(kotBill.BillId, "Restaurant");
                    }

                    paymentResume.KotBillMaster = kotBillMaster;
                    paymentResume.KotBillDetails = kotDetails;
                    paymentResume.RestaurantKotBill = kotBill;
                    paymentResume.RestaurantKotBillPayment = kotBillPayment;

                    //HttpContext.Current.Session["RestaurantKotBillResume"] = paymentResume;
                    kotDetails = kotDetails.Where(x => x.ItemUnit > 0).ToList();

                    string kotIdList = string.Empty;
                    if (BillDetail.Count > 0)
                    {
                        foreach (RestaurantBillDetailBO bd in BillDetail)
                        {
                            if (kotIdList != string.Empty)
                            {
                                kotIdList = kotIdList + "," + bd.KotId.ToString();
                                //kotDetails.AddRange(kotDa.GetKotBillDetailInfoByKotId(bd.KotId).Where(x => x.ItemUnit > 0).ToList());
                            }
                            else
                            {
                                kotIdList = bd.KotId.ToString();
                            }
                        }
                    }

                    kotDetailsForSplit = entityDA.GetRestaurantOrderItemByMultipleKotId(RestaurantBill.CostCenterId.ToString(), kotIdList, "Table").Where(x => x.ItemUnit > 0).ToList();

                    foreach (KotBillDetailBO row in kotDetailsForSplit)
                    {
                        if (Convert.ToInt32(row.ItemUnit) > 1)
                        {
                            int i = 1;
                            while (i <= Convert.ToInt32(row.ItemUnit))
                            {
                                kotDetailsToDisplay.Add(row);
                                i++;
                            }
                        }
                        else
                        {
                            kotDetailsToDisplay.Add(row);
                        }
                    }

                    rtninf.Arr = new ArrayList((
                                from k in kotDetailsToDisplay
                                select new
                                {
                                    ItemId = k.ItemId,
                                    ItemName = k.ItemName
                                }).ToArray()
                        );

                    InvCategoryDA catDa = new InvCategoryDA();
                    List<ItemClassificationBO> classificationLst = new List<ItemClassificationBO>();

                    classificationLst = catDa.GetRestaurantBillClassificationDetailsByBillId(RestaurantBill.BillId);

                    if (classificationLst.Count > 0)
                        rtninf.ObjectList = new ArrayList(classificationLst);

                    List<RestaurantBillDetailBO> billDetailList = new List<RestaurantBillDetailBO>();
                    billDetailList = restaurentBillDA.GetRestaurantBillDetailsByBillId(kotBill.BillId);

                    if (billDetailList.Count > 0)
                    {
                        foreach (RestaurantBillDetailBO bd in billDetailList)
                        {
                            bd.MainBillId = RestaurantBill.BillId;
                        }
                        rtninf.ObjectList1 = new ArrayList(billDetailList);
                    }

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
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
        public static List<int> GetCommonCheckByApproveByListForSMS(string tableName, string primaryKeyName, string primaryKeyValue, string featuresValue, string statusColumnName)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();

            List<int> CheckByApproveByList = hmCommonDA.GetCommonCheckByApproveByListForSMS(tableName, primaryKeyName, primaryKeyValue, featuresValue, statusColumnName);

            
            return CheckByApproveByList;
        }

        [WebMethod]
        public static ReturnInfo UpdateRestauranBillGenerationSplitNew(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment,
                                                                  List<RestaurantBillDetailBO> BillDetail, List<RestaurantBillDetailBO> BillDeletedDetail,
                                                                  List<ItemClassificationBO> AddedClassificationList, List<ItemClassificationBO> DeletedClassificationList
                                                                 )
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
                paymentResume.RestaurantKotBill = restaurentBillDA.GetRestaurantBillByKotId(RestaurantBill.KotId, RestaurantBill.SourceName);

                rtninf.IsBillResettled = false;

                if (paymentResume.RestaurantKotBill != null)
                {
                    if (paymentResume.RestaurantKotBill.IsBillSettlement)
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Bill Already Settled. Redirect Within a Ahort Time.", AlertType.Error);

                        return rtninf;
                    }
                }

                KotBillMasterDA kotDa = new KotBillMasterDA();
                KotBillMasterBO kotBillMaster = new KotBillMasterBO();

                if (RestaurantBill.SourceName == ConstantHelper.RestaurantBillSource.RestaurantTable.ToString())
                {
                    kotBillMaster = kotDa.GetKotBillMasterInfoByKotIdNSourceName(RestaurantBill.KotId, RestaurantBill.SourceName);

                    if (kotBillMaster.KotStatus != "pending")
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Kot Already " + kotBillMaster.KotStatus + ". Redirect Within a Short Time.", AlertType.Error);
                        return rtninf;
                    }

                    if (kotBillMaster.SourceId != RestaurantBill.TableId)
                    {
                        rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                        rtninf.IsBillResettled = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo("Source Table Is Changed. Redirect Within a Short Time.", AlertType.Error);
                        return rtninf;
                    }
                }

                List<GuestBillPaymentBO> paymentAdded = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentUpdate = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentDelete = new List<GuestBillPaymentBO>();
                RestaurantBill.BillId = paymentResume.RestaurantKotBill.BillId;

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = paymentResume.RestaurantKotBill.BillDate;
                RestaurantBill.BillPaymentDate = paymentResume.RestaurantKotBill.BillPaymentDate;

                bool billId = restaurentBillDA.UpdateRestaurantBillForNewHoldUp(RestaurantBill, BillDetail, BillDeletedDetail, paymentAdded, paymentUpdate, paymentDelete, AddedClassificationList, DeletedClassificationList);

                if (billId)
                {

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RestaurantBill.ToString(), RestaurantBill.BillId,
                             ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), "Split Bill");

                    //kotDa.UpdateKotBillMasterForHoldUp(RestaurantBill.KotId, true);

                    KotBillDetailDA entityDA = new KotBillDetailDA();
                    Boolean status = entityDA.UpdateKotBillDetailPrintFlagByOrderSubmitKotId(RestaurantBill.KotId);

                    rtninf.IsSuccess = true;
                    rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    rtninf.IsBillHoldUp = true;
                    rtninf.Pk = RestaurantBill.BillId;

                    List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();
                    List<KotBillDetailBO> kotDetailsToDisplay = new List<KotBillDetailBO>();

                    RestaurantBillBO kotBill = new RestaurantBillBO();
                    kotDetails = kotDa.GetKotBillDetailInfoByKotId(RestaurantBill.KotId);

                    string kotIdList = string.Empty;
                    if (BillDetail.Count > 0)
                    {
                        foreach (RestaurantBillDetailBO bd in BillDetail)
                        {
                            if (kotIdList != string.Empty)
                            {
                                kotIdList = kotIdList + "," + bd.KotId.ToString();
                                //kotDetails.AddRange(kotDa.GetKotBillDetailInfoByKotId(bd.KotId).Where(x => x.ItemUnit > 0).ToList());
                            }
                            else
                            {
                                kotIdList = bd.KotId.ToString();
                            }
                        }
                    }

                    kotDetails = entityDA.GetRestaurantOrderItemByMultipleKotId(RestaurantBill.CostCenterId.ToString(), kotIdList, "Table").Where(x => x.ItemUnit > 0).ToList();

                    foreach (KotBillDetailBO row in kotDetails)
                    {
                        if (Convert.ToInt32(row.ItemUnit) > 1)
                        {
                            int i = 1;
                            while (i <= Convert.ToInt32(row.ItemUnit))
                            {
                                kotDetailsToDisplay.Add(row);
                                i++;
                            }
                        }
                        else
                        {
                            kotDetailsToDisplay.Add(row);
                        }
                    }

                    rtninf.Arr = new ArrayList((
                                from k in kotDetailsToDisplay
                                select new
                                {
                                    ItemId = k.ItemId,
                                    ItemName = k.ItemName
                                }).ToArray()
                        );

                    InvCategoryDA catDa = new InvCategoryDA();
                    List<ItemClassificationBO> classificationLst = new List<ItemClassificationBO>();

                    classificationLst = catDa.GetRestaurantBillClassificationDetailsByBillId(kotBill.BillId);

                    if (classificationLst.Count > 0)
                        rtninf.ObjectList = new ArrayList(classificationLst);

                    List<RestaurantBillDetailBO> billDetailList = new List<RestaurantBillDetailBO>();
                    billDetailList = restaurentBillDA.GetRestaurantBillDetailsByBillId(RestaurantBill.BillId);

                    if (billDetailList.Count > 0)
                    {
                        foreach (RestaurantBillDetailBO bd in billDetailList)
                        {
                            bd.MainBillId = kotBill.BillId;
                        }

                        rtninf.ObjectList1 = new ArrayList(billDetailList);
                    }

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
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



        //--*** Bill Re-Settlement

        [WebMethod]
        public static ReturnInfo UpdateRestauranBillReSettlement(RestaurantBillBO RestaurantBill, List<GuestBillPaymentBO> GuestBillPayment,
                                                                       List<RestaurantBillDetailBO> BillDetail, List<RestaurantBillDetailBO> BillDeletedDetail,
                                                                       List<ItemClassificationBO> AddedClassificationList, List<ItemClassificationBO> DeletedClassificationList,
                                                                       string DestinationPath)
        {
            ReturnInfo rtninf = new ReturnInfo();
            string categoryIdList = string.Empty, categoryIdAndDiscountList = string.Empty, kotIdList = string.Empty;
            bool isBillPayment = false, isGuestPayment = false;

            try
            {
                RestaurentPosDA posDa = new RestaurentPosDA();
                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                KotBillMasterBO kotBillMaster = new KotBillMasterBO();
                List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();

                //RestaurantBillBO kotBill = new RestaurantBillBO();
                //GuestExtraServiceBillApprovedBO roomWisePayment = new GuestExtraServiceBillApprovedBO();
                //List<GuestBillPaymentBO> kotBillPayment = new List<GuestBillPaymentBO>();
                RestaurantBillPaymentResume paymentResume = new RestaurantBillPaymentResume();
                paymentResume.RestaurantKotBill = restaurentBillDA.GetRestaurantBillByKotId(RestaurantBill.KotId, RestaurantBill.SourceName);
                paymentResume.RestaurantKotBillPayment = restaurentBillDA.GetBillPaymentByBillId(paymentResume.RestaurantKotBill.BillId, "Restaurant");
                paymentResume.RoomWiseBillPayment = posDa.GetRoomWiseRestaurantBillPaymentByBillIdServiceTypePaymentMode(paymentResume.RestaurantKotBill.BillId);

                //paymentResume.KotBillMaster = kotBillMaster;
                //paymentResume.KotBillDetails = kotDetails;
                //paymentResume.RestaurantKotBill = kotBill;
                //paymentResume.RestaurantKotBillPayment = kotBillPayment;
                //paymentResume.RoomWiseBillPayment = roomWisePayment;

                //paymentResume = (RestaurantBillPaymentResume)HttpContext.Current.Session["RestaurantKotBillResumeForResettlement"];

                List<GuestBillPaymentBO> paymentAdded = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> paymentUpdate = new List<GuestBillPaymentBO>();
                List<GuestBillPaymentBO> previousLedgerPayment = new List<GuestBillPaymentBO>();

                RestaurantBill.BillId = paymentResume.RestaurantKotBill.BillId;

                paymentAdded = GuestBillPayment;
                if (paymentResume.RestaurantKotBillPayment.Count > 0)
                {
                    isBillPayment = true;

                    previousLedgerPayment = paymentResume.RestaurantKotBillPayment.Where(p =>
                                            p.PaymentType == HMConstants.PaymentMode.Company.ToString() ||
                                            p.PaymentType == HMConstants.PaymentMode.Employee.ToString() ||
                                            p.PaymentType == HMConstants.PaymentMode.Member.ToString()
                    ).ToList();                    
                }

                if (paymentResume.RoomWiseBillPayment != null)
                {
                    if (paymentResume.RoomWiseBillPayment.ApprovedId != 0)
                        isGuestPayment = true;
                }

                #region  payment save, update delete

                //paymentUpdate = null;
                //paymentDelete = null;

                //var vcash = (from p in paymentResume.RestaurantKotBillPayment
                //             where p.PaymentMode == HMConstants.PaymentMode.Cash.ToString()
                //             select p).FirstOrDefault();

                //if (vcash == null)
                //{
                //    vcash = (from p in GuestBillPayment
                //             where p.PaymentMode == HMConstants.PaymentMode.Cash.ToString() &&
                //                   p.PaymentAmount != 0
                //             select p
                //             ).FirstOrDefault();

                //    if (vcash != null)
                //    {
                //        paymentAdded.Add(vcash);
                //    }
                //}

                //var vrounded = (from p in paymentResume.RestaurantKotBillPayment
                //                where p.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                //                select p).FirstOrDefault();

                //if (vrounded == null)
                //{
                //    vrounded = (from p in GuestBillPayment
                //                where p.PaymentMode == HMConstants.PaymentMode.Rounded.ToString() &&
                //                     p.PaymentAmount != 0
                //                select p
                //             ).FirstOrDefault();

                //    if (vrounded != null)
                //    {
                //        paymentAdded.Add(vrounded);
                //    }
                //}

                //var vrefund = (from p in paymentResume.RestaurantKotBillPayment
                //               where p.PaymentMode == HMConstants.PaymentMode.Refund.ToString()
                //               select p).FirstOrDefault();

                //if (vrefund == null)
                //{
                //    vrefund = (from p in GuestBillPayment
                //               where p.PaymentMode == HMConstants.PaymentMode.Refund.ToString() &&
                //                     p.PaymentAmount != 0
                //               select p
                //             ).FirstOrDefault();

                //    if (vrefund != null)
                //    {
                //        paymentAdded.Add(vrefund);
                //    }
                //}

                //var vcard = (from p in paymentResume.RestaurantKotBillPayment
                //             where p.PaymentMode == HMConstants.PaymentMode.Card.ToString()
                //             select p).ToList();

                //var vcardNew = (from p in GuestBillPayment
                //                where p.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                //                      p.PaymentAmount != 0
                //                select p).ToList();

                //if (vcard != null)
                //{
                //    foreach (GuestBillPaymentBO bp in vcardNew)
                //    {
                //        var v1 = (from pr in vcard
                //                  where pr.CardType == bp.CardType &&
                //                        pr.PaymentAmount != 0
                //                  select pr).FirstOrDefault();

                //        if (v1 == null)
                //        {
                //            paymentAdded.Add(bp);
                //        }
                //    }
                //}

                //var update1 = (from bp in GuestBillPayment
                //               from pr in paymentResume.RestaurantKotBillPayment
                //               where bp.PaymentMode == pr.PaymentMode &&
                //                     (
                //                         bp.PaymentMode == HMConstants.PaymentMode.Cash.ToString() ||
                //                         bp.PaymentMode == HMConstants.PaymentMode.Refund.ToString() ||
                //                         bp.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                //                     ) &&
                //                     bp.PaymentAmount != pr.PaymentAmount &&
                //                     bp.PaymentAmount != 0
                //               select new GuestBillPaymentBO
                //               {
                //                   PaymentId = pr.PaymentId,
                //                   BillNumber = pr.BillNumber,
                //                   ModuleName = pr.ModuleName,
                //                   PaymentType = pr.PaymentType,
                //                   RegistrationId = pr.RegistrationId,
                //                   PaymentDate = pr.PaymentDate,
                //                   FieldId = pr.FieldId,
                //                   CurrencyAmount = pr.CurrencyAmount,
                //                   PaymentAmount = bp.PaymentAmount,
                //                   ServiceBillId = pr.ServiceBillId,
                //                   PaymentMode = pr.PaymentMode,
                //                   BankId = pr.BankId,
                //                   BranchName = pr.BranchName,
                //                   ChecqueNumber = pr.ChecqueNumber,
                //                   CardType = pr.CardType,
                //                   CardNumber = pr.CardNumber,
                //                   CardHolderName = pr.CardHolderName,
                //                   CardReference = pr.CardReference
                //               }).ToList();

                //var update2 = (from pr in paymentResume.RestaurantKotBillPayment
                //               from bp in GuestBillPayment
                //               where pr.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                //                     pr.CardType == bp.CardType &&
                //                     pr.PaymentAmount != bp.PaymentAmount &&
                //                     bp.PaymentAmount != 0
                //               select new GuestBillPaymentBO
                //               {
                //                   PaymentId = pr.PaymentId,
                //                   BillNumber = pr.BillNumber,
                //                   ModuleName = pr.ModuleName,
                //                   PaymentType = pr.PaymentType,
                //                   RegistrationId = pr.RegistrationId,
                //                   PaymentDate = pr.PaymentDate,
                //                   FieldId = pr.FieldId,
                //                   CurrencyAmount = pr.CurrencyAmount,
                //                   PaymentAmount = bp.PaymentAmount,
                //                   ServiceBillId = pr.ServiceBillId,
                //                   PaymentMode = pr.PaymentMode,
                //                   BankId = pr.BankId,
                //                   BranchName = pr.BranchName,
                //                   ChecqueNumber = pr.ChecqueNumber,
                //                   CardType = pr.CardType,
                //                   CardNumber = pr.CardNumber,
                //                   CardHolderName = pr.CardHolderName,
                //                   CardReference = pr.CardReference
                //               }).ToList();

                //paymentUpdate.AddRange(update1);
                //paymentUpdate.AddRange(update2);

                //var delete1 = (from pr in paymentResume.RestaurantKotBillPayment
                //               from bp in GuestBillPayment
                //               where pr.PaymentMode == bp.PaymentMode &&
                //                     (
                //                         pr.PaymentMode == HMConstants.PaymentMode.Cash.ToString() ||
                //                         pr.PaymentMode == HMConstants.PaymentMode.Refund.ToString() ||
                //                         bp.PaymentMode == HMConstants.PaymentMode.Rounded.ToString()
                //                     ) &&
                //                     bp.PaymentAmount == 0
                //               select new GuestBillPaymentBO
                //               {
                //                   PaymentId = pr.PaymentId,
                //                   BillNumber = pr.BillNumber,
                //                   ModuleName = pr.ModuleName,
                //                   PaymentType = pr.PaymentType,
                //                   RegistrationId = pr.RegistrationId,
                //                   PaymentDate = pr.PaymentDate,
                //                   PaymentAmount = bp.PaymentAmount,
                //                   ServiceBillId = pr.ServiceBillId,
                //                   PaymentMode = pr.PaymentMode,
                //                   BankId = pr.BankId,
                //                   BranchName = pr.BranchName,
                //                   ChecqueNumber = pr.ChecqueNumber,
                //                   CardType = pr.CardType,
                //                   CardNumber = pr.CardNumber,
                //                   CardHolderName = pr.CardHolderName,
                //                   CardReference = pr.CardReference
                //               }).ToList();

                //var delete2 = (from pr in paymentResume.RestaurantKotBillPayment
                //               from bp in GuestBillPayment
                //               where pr.PaymentMode == HMConstants.PaymentMode.Card.ToString() &&
                //                     pr.CardType == bp.CardType &&
                //                     bp.PaymentAmount == 0
                //               select new GuestBillPaymentBO
                //               {
                //                   PaymentId = pr.PaymentId,
                //                   BillNumber = pr.BillNumber,
                //                   ModuleName = pr.ModuleName,
                //                   PaymentType = pr.PaymentType,
                //                   RegistrationId = pr.RegistrationId,
                //                   PaymentDate = pr.PaymentDate,
                //                   PaymentAmount = bp.PaymentAmount,
                //                   ServiceBillId = pr.ServiceBillId,
                //                   PaymentMode = pr.PaymentMode,
                //                   BankId = pr.BankId,
                //                   BranchName = pr.BranchName,
                //                   ChecqueNumber = pr.ChecqueNumber,
                //                   CardType = pr.CardType,
                //                   CardNumber = pr.CardNumber,
                //                   CardHolderName = pr.CardHolderName,
                //                   CardReference = pr.CardReference
                //               }).ToList();

                //paymentDelete.AddRange(delete1);
                //paymentDelete.AddRange(delete2);
                #endregion

                int billID = 0;

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
                List<string> strCategoryIdList = new List<string>();

                if (AddedClassificationList != null)
                {
                    if (AddedClassificationList.Count > 0)
                    {
                        foreach (ItemClassificationBO itmcls in AddedClassificationList)
                        {
                            categoryIdList += categoryIdList != string.Empty ? ("," + itmcls.ClassificationId.ToString()) : itmcls.ClassificationId.ToString();
                            categoryIdAndDiscountList += categoryIdAndDiscountList != string.Empty ? ("," + itmcls.ClassificationId.ToString() + "#" + itmcls.DiscountAmount.ToString()) : (itmcls.ClassificationId.ToString() + "#" + itmcls.DiscountAmount.ToString());
                        }

                        //AddedClassificationList = AddedClassificationList.Where(c => c.DiscountId == 0).ToList();
                    }
                }

                if (BillDetail != null)
                {
                    foreach (RestaurantBillDetailBO kot in BillDetail)
                    {
                        kotIdList += kotIdList != string.Empty ? ("," + kot.KotId.ToString()) : kot.KotId.ToString();
                    }
                }

                bool IsRestaurantBillAmountWillRound = false;
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
                isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillAmountWillRound", "IsRestaurantBillAmountWillRound");
                IsRestaurantBillAmountWillRound = (isRestaurantBillAmountWillRoundBO.SetupValue.ToString() == "0" ? false : true);

                KotBillDetailDA entityDA = new KotBillDetailDA();
                List<KotBillDetailBO> kotDetailsList = entityDA.GetRestaurantOrderItemByMultipleKotId(RestaurantBill.CostCenterId.ToString(), kotIdList, RestaurantBill.SourceName).Where(x => x.ItemUnit > 0).ToList();

                bool isBillCanSettle = false;
                decimal totalQuantity = 0, totalPrice = 0;
                totalQuantity = Math.Round(kotDetailsList.Sum(s => s.ItemUnit), 2);
                totalPrice = Math.Round(kotDetailsList.Sum(s => s.ItemUnit * s.UnitRate), 2);

                if (RestaurantBill.TotalQuantity == totalQuantity && RestaurantBill.TotalPrice == totalPrice)
                {
                    isBillCanSettle = true;
                }

                if (!isBillCanSettle)
                {
                    rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";
                    rtninf.IsBillResettled = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo("Invoice Grand Total Is Not Same As Item Grand Total. Redirect Within a Short Time.", AlertType.Error);
                    return rtninf;
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBill.LastModifiedBy = userInformationBO.UserInfoId;
                RestaurantBill.BillDate = paymentResume.RestaurantKotBill.BillDate;
                RestaurantBill.BillPaymentDate = paymentResume.RestaurantKotBill.BillPaymentDate;

                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                restaurentBillDetailBO.BillId = RestaurantBill.BillId;
                restaurentBillDetailBO.KotId = RestaurantBill.KotId;
                restaurentBillDetailBOList.Add(restaurentBillDetailBO);

                var isRoomWisePayment = GuestBillPayment.Where(p => p.PaymentMode == "Other Room").FirstOrDefault();
                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

                if (isRoomWisePayment != null || RestaurantBill.RoomId > 0)
                {
                    int roomNo = 0;
                    RoomRegistrationDA roomDa = new RoomRegistrationDA();

                    roomRegistration = roomDa.GetRoomRegistrationInfoById(RestaurantBill.RegistrationId); //GetRoomRegistrationNRoomDetailsByRoomId, GetRoomRegistrationInfoById, GetRoomRegistrationNRoomDetailsByRoomId                    
                    RestaurantBill.RegistrationId = roomRegistration.RegistrationId;

                    RestaurantBill.CustomerName = string.IsNullOrEmpty(RestaurantBill.CustomerName) ? (roomRegistration.GuestName + ",Room# " + roomRegistration.RoomNumber) : RestaurantBill.CustomerName;

                    if (RestaurantBill.CustomerName == "Room# " + roomRegistration.RoomNumber)
                    {
                        RestaurantBill.CustomerName = (roomRegistration.GuestName + ",Room# " + roomRegistration.RoomNumber);
                        RestaurantBill.BillPaidBySourceId = Convert.ToInt32(roomRegistration.RoomNumber);
                    }
                    else if (int.TryParse((RestaurantBill.CustomerName.Replace("Room#", string.Empty).Trim()), out roomNo))
                    {
                        if (Convert.ToInt32(RestaurantBill.CustomerName.Replace("Room#", string.Empty)) != Convert.ToInt32(roomRegistration.RoomNumber))
                        {
                            RestaurantBill.CustomerName = (roomRegistration.GuestName + ",Room# " + roomRegistration.RoomNumber);
                            RestaurantBill.BillPaidBySourceId = Convert.ToInt32(roomRegistration.RoomNumber);
                        }
                    }
                }

                string paymentInfo = string.Empty;

                foreach (GuestBillPaymentBO p in GuestBillPayment)
                {
                    if (p.PaymentAmount > 0)
                    {
                        if (p.PaymentMode != "Other Room" && p.PaymentMode != "Rounded")
                        {
                            paymentInfo = paymentInfo + (!string.IsNullOrEmpty(paymentInfo) ? "," : "")
                                                      + p.PaymentMode + (!string.IsNullOrEmpty(p.PaymentDescription) ? "(" + p.PaymentDescription + ")" : "")
                                                      + ":" + p.PaymentAmount.ToString();
                        }
                        else if (p.PaymentMode != "Rounded")
                        {
                            paymentInfo = paymentInfo + (!string.IsNullOrEmpty(paymentInfo) ? "," : "")
                                                      + "Room#" + roomRegistration.RoomNumber
                                                      + ":" + p.PaymentAmount.ToString();

                            p.PaymentDescription = "Room#" + roomRegistration.RoomNumber;
                        }
                    }
                    else if (p.PaymentAmount == 0 && RestaurantBill.IsNonChargeable)
                    {
                        if (RestaurantBill.RoomId > 0)
                        {
                            paymentInfo = "Non Chargeable(NC)" + " Room#" + roomRegistration.RoomNumber; ;
                        }
                        else
                        {
                            paymentInfo = "Non Chargeable(NC)";
                        }
                    }
                    else if (p.PaymentAmount == 0 && RestaurantBill.IsComplementary)
                    {
                        if (RestaurantBill.RoomId > 0)
                        {
                            paymentInfo = "Complementary" + " Room#" + roomRegistration.RoomNumber; ;
                        }
                        else
                        {
                            paymentInfo = "Complementary";
                        }
                    }
                }
                RestaurantBill.PaymentRemarks = paymentInfo;

                bool billId = restaurentBillDA.UpdateForRestauranBillReSettlement(RestaurantBill, BillDetail, BillDeletedDetail, paymentAdded, paymentUpdate, previousLedgerPayment,
                    AddedClassificationList, DeletedClassificationList, categoryIdList, true, isBillPayment, isGuestPayment);

                if (billId)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RestaurantBill.ToString(), RestaurantBill.BillId,
                             ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), "Re Settlement");

                    rtninf.IsSuccess = true;
                }
                else { rtninf.IsSuccess = false; }

                if (rtninf.IsSuccess)
                {
                    HMCommonSetupBO commonRestaurantBillPrintAndPreview = new HMCommonSetupBO();
                    commonRestaurantBillPrintAndPreview = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillPrintAndPreview", "RestaurantBillPrintAndPreview");

                    rtninf.Pk = RestaurantBill.BillId;
                    rtninf.BillPrintAndPreview = Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue);
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.BillReSettlement, AlertType.Success);

                    List<RestaurantBillDetailBO> billDetailList = new List<RestaurantBillDetailBO>();
                    billDetailList = restaurentBillDA.GetRestaurantBillDetailsByBillId(RestaurantBill.BillId);

                    if (billDetailList.Count > 0)
                    {
                        foreach (RestaurantBillDetailBO bd in billDetailList)
                        {
                            bd.MainBillId = RestaurantBill.BillId;
                        }

                        rtninf.ObjectList1 = new ArrayList(billDetailList);
                    }

                    InvCategoryDA catDa = new InvCategoryDA();
                    List<ItemClassificationBO> classificationLst = new List<ItemClassificationBO>();
                    classificationLst = catDa.GetRestaurantBillClassificationDetailsByBillId(RestaurantBill.BillId);

                    if (classificationLst.Count > 0)
                        rtninf.ObjectList = new ArrayList(classificationLst);

                    if (rtninf.BillPrintAndPreview == (int)(HMConstants.RestaurantBillPrintAndPreview.DirectPrint))
                    {
                        frmRestaurantManagement manage = new frmRestaurantManagement();
                        manage.PrintRestaurantBill(RestaurantBill.BillId);

                        //rtninf.RedirectUrl = "frmCostCenterSelectionForAll.aspx";

                    }

                    //rtninf.RedirectUrl = "frmBillSearch.aspx";

                    rtninf.RedirectUrl = DestinationPath;
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


        //--*** User Access Varification For Restaurant Pos & Other Pos

        [WebMethod]
        public static ReturnInfo UserAccessVarification(string userid, string password)
        {
            ReturnInfo rtninf = new ReturnInfo();

            RestaurantBearerDA beararDa = new RestaurantBearerDA();
            UserInformationDA userInformationDA = new UserInformationDA();
            RestaurantBearerBO bearerbo = new RestaurantBearerBO();

            try
            {
                UserInformationBO userInformation = userInformationDA.GetUserInformationByUserNameNId(userid, password);
                bearerbo = beararDa.GetRestaurantBearerInfoByEmpId(userInformation.UserInfoId);

                if (userid.Trim().Equals(userInformation.UserId))
                {
                    rtninf.IsSuccess = true;
                }
                else { rtninf.IsSuccess = false; }

                if (rtninf.IsSuccess && bearerbo.IsItemCanEditDelete)
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo("Access Varified.", AlertType.Success);
                    rtninf.IsReservationCheckInDateValidation = bearerbo.IsItemCanEditDelete;
                    rtninf.IsSuccess = true;
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    rtninf.IsSuccess = false;
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
        public static ReturnInfo UserAccessVarificationByUserIdNPassword(string userid, string password, int costcenterId)
        {
            ReturnInfo rtninf = new ReturnInfo();

            RestaurantBearerDA bearerDa = new RestaurantBearerDA();

            try
            {
                if (userid == "superadmin")
                {
                    rtninf.IsSuccess = true;
                }
                else
                {
                    rtninf.IsSuccess = bearerDa.GetCashierInfoByLoginIdNPassword(userid, password, costcenterId) > 0 ? true : false;
                }

                if (rtninf.IsSuccess)
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo("Access Varified.", AlertType.Success);
                    rtninf.IsSuccess = true;
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    rtninf.IsSuccess = false;
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
        public static List<MenuLinksBO> GetMenuLinksByMenuName(string pageName)
        {
            MenuDA menuDa = new MenuDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();

            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            return menuDa.GetMenuLinksByMenuName(pageName, userInformationBO.UserGroupId, userInformationBO.UserInfoId);
        }

        [WebMethod]
        public static string LoadVacantPossiblePath(string roomNumber)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO roomNumberBO = new RoomNumberBO();
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            //list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "VacantDirtyPossiblePath");
            roomNumberBO = numberDA.GetRoomInformationByRoomNumber(roomNumber);

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();

            if (roomNumberBO.StatusId == 1 && roomNumberBO.HKRoomStatusId == 5)
                list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "VacantPossiblePath");
            else if (roomNumberBO.StatusId == 2 && roomNumberBO.HKRoomStatusId == 5)
                list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "OccupiedPossiblePath");
            //else if (roomNumberBO.StatusId == 1 && roomNumberBO.HKRoomStatusId == 6)
            //    list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "VacantDirtyPossiblePath");

            string strTable = string.Empty;
            int row = 1;

            if (roomNumberBO.StatusId == 1 && roomNumberBO.HKRoomStatusId == 5)
            {
                row = 1;
                strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
                for (int i = 0; i < list.Count; i++)
                {
                    if (row == 1)
                        strTable += "<div class='form-group'>";

                    strTable += "<div class='col-md-4'>";
                    strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'";

                    if (list[i].DisplayText == "Room Status Change")
                    {
                        strTable += " onclick=\"return LoadCleanUpInfo('" + roomNumberBO.RoomId + "');\"  />";
                        strTable += "</div>";
                    }
                    else if (list[i].DisplayText.Trim() == "Registration")
                    {
                        strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?SelectedRoomNumber=" + roomNumberBO.RoomId + "&source=Registration';\"  />";
                        strTable += "</div>";
                    }

                    if (row == 3)
                    {
                        strTable += "</div>";
                        row = 0;
                    }

                    row++;
                }
                strTable += "</div></div>";
            }
            else if (roomNumberBO.StatusId == 2)
            {
                strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
                row = 1;

                for (int i = 0; i < list.Count; i++)
                {
                    if (row == 1)
                        strTable += "<div class='form-group'>";

                    strTable += "<div class='col-md-4'>";
                    strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'";

                    if (list[i].DisplayText == "Details")
                    {
                        strTable += " onclick=\"return TotalNumberOfGuestByRoomNumber('" + roomNumber + "', 0 );\"  />";
                        strTable += "</div>";
                    }
                    else if (list[i].DisplayText == "Bill Preview")
                    {
                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                        RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                        roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
                        if (roomAllocationBO.RoomId > 0)
                        {
                            HMUtility hmUtility = new HMUtility();
                            HttpContext.Current.Session["CheckOutRegistrationIdList"] = roomAllocationBO.RegistrationId.ToString();
                            string StartDate = hmUtility.GetFromDate();
                            string EndDate = hmUtility.GetToDate();
                            string ddlRegistrationId = roomAllocationBO.RegistrationId.ToString();
                            string txtSrcRegistrationIdList = roomAllocationBO.RegistrationId.ToString();
                            HttpContext.Current.Session["IsBillSplited"] = "0";
                            HttpContext.Current.Session["GuestBillFromDate"] = hmUtility.GetFromDate();
                            HttpContext.Current.Session["GuestBillToDate"] = hmUtility.GetToDate();
                            strTable += " onclick='BillPreviewGlobal()' />";
                            strTable += "</div>";
                        }
                    }
                    else
                    {
                        strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RoomId=" + roomNumberBO.RoomId + "';\"  />";
                        strTable += "</div>";
                    }

                    if (row == 3)
                    {
                        strTable += "</div>";
                        row = 0;
                    }

                    row++;
                }

                strTable += "</div></div>";
            }
            //else if (roomNumberBO.StatusId == 1 && roomNumberBO.HKRoomStatusId == 6)
            //{
            //    strTable += "<div style='padding:10px'> <div class='form-horizontal'>";
            //    for (int i = 0; i < list.Count; i++)
            //    {
            //        if (row == 1)
            //            strTable += "<div class='form-group'>";

            //        strTable += "<div class='col-md-4'>";
            //        strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary'";

            //        if (list[i].DisplayText == "Details")
            //        {
            //            strTable += " onclick=\"return CountTotalNumberOfGuestByRoomNumber('" + roomNumber + "', 0 );\"  />";
            //        }
            //        else
            //        {
            //            strTable += " onclick=\"return LoadCleanUpInfo('" + roomNumberBO.RoomId + "');\"  />";
            //        }

            //        if (row == 3)
            //        {
            //            strTable += "</div>";
            //            row = 0;
            //        }

            //        row++;
            //    }

            //    strTable += "</div></div>";
            //}

            return strTable;
        }
        [WebMethod]
        public static RoomNumberBO GetRoomCleanUpInfo(int EditId)
        {
            RoomNumberBO roomNumberList = new RoomNumberBO();
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            roomNumberList = roomNumberDA.GetRoomNumberInfoById(EditId);
            return roomNumberList;
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string itemName, int costCenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemNameWiseItemDetailsForAutoSearch(itemName, costCenterId, ConstantHelper.CustomerSupplierAutoSearch.CustomerItem.ToString());

            return itemInfo;
        }
        [WebMethod]
        public static string SearchGuestAndLoadGridInformationMasterPage(string CompanyName, string DateOfBirth, string EmailAddress, string FromDate, string ToDate, string GuestName, string MobileNumber, string NationalId, string PassportNumber, string RegistrationNumber, string RoomNumber)
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

            list = guestDA.GetGuestInformationBySearchCriteria(GuestName, EmailAddress, MobileNumber, NationalId, PassportNumber, dateOfBirth, CompanyName, RoomNumber, fromDate, toDate, RegistrationNumber);
            distinctItems = list.GroupBy(x => x.GuestId).Select(y => y.First()).Distinct().ToList();
            return commonDA.GetHTMLGuestGridView(distinctItems);
        }
        [WebMethod]
        public static GuestInformationBO LoadDetailInformation(string GuestId)
        {
            HMCommonDA commonDA = new HMCommonDA();
            return commonDA.GetGuestDetailInformation(GuestId);
        }

        //-----------------
        [WebMethod]
        public static string GetRegistrationInformationListByRoomNumber(string roomNumber)
        {
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);

            string strTable = "";
            int count = guestDA.CountNumberOfGuestByRegistrationId(allocationBO.RegistrationId);
            if (count > 1)
            {
                strTable = "0~" + GetUserDetailHtml(list);
            }
            else
            {
                strTable = "DGL~" + list[0].GuestId.ToString();
            }

            return strTable;
        }
        private static string GetUserDetailHtml(List<GuestInformationBO> registrationDetailListBO)
        {
            string strTable = "";
            strTable += "<table style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";

            strTable += "<th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Email</th> <th align='left' scope='col'>Action</th></tr>";
            int counter = 0;

            foreach (GuestInformationBO dr in registrationDetailListBO)
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

                strTable += "<td align='left' style='width: 50%'>" + dr.GuestName + "</td>";
                strTable += "<td align='left' style='width: 30%'>" + dr.GuestEmail + "</td>";
                strTable += "<td align='left' style='width: 20%'>";
                strTable += "&nbsp;<img src='../Images/select.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return SelectGuestInformation('" + dr.GuestId + "')\" alt='Edit Information' border='0' />";

                strTable += "</td>";
                strTable += "</tr>";
            }

            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }


    }
}