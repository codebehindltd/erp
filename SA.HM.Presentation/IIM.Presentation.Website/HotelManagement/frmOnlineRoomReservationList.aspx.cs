using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmOnlineRoomReservationList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static GridViewDataNPaging<RoomReservationBO, GridPaging> SearchResevationAndLoadGridInformation(string strFromDate, string strToDate, string guestName, string reserveNo, string companyName, string contactPerson, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            GridViewDataNPaging<RoomReservationBO, GridPaging> myGridData = new GridViewDataNPaging<RoomReservationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            RoomReservationDA rrDA = new RoomReservationDA();
            List<RoomReservationBO> reservationInfoList = new List<RoomReservationBO>();

            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (!string.IsNullOrWhiteSpace(strFromDate))
            {
                fromDate = hmUtility.GetDateTimeFromString(strFromDate, userInformationBO.ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(strToDate))
            {
                toDate = hmUtility.GetDateTimeFromString(strToDate, userInformationBO.ServerDateFormat);
            }

            reservationInfoList = rrDA.GetOnlineRoomReservationInformationBySearchCriteriaForPaging(fromDate, toDate, guestName, reserveNo, companyName, contactPerson, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<RoomReservationBO> distinctItems = new List<RoomReservationBO>();
            distinctItems = reservationInfoList.GroupBy(test => test.ReservationNumber).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords, "GridPagingForSearchReservation");

            return myGridData;
        }

    }
}