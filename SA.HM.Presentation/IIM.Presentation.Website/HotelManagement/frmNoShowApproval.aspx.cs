using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmNoShowApproval : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            hfMinCheckInDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.Date.AddDays(-1));
            CheckPermission();
        }
        //************************ User Defined Web Method ********************//
        private void CheckPermission()
        {
            hfIsSavePermission.Value = isSavePermission ? "1" : "0";
        }
        [WebMethod]
        public static GridViewDataNPaging<RoomReservationBO, GridPaging> GetRoomReservationInfoForNoShow(DateTime searchDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            GridViewDataNPaging<RoomReservationBO, GridPaging> myGridData = new GridViewDataNPaging<RoomReservationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            RoomReservationDA rrDA = new RoomReservationDA();
            List<RoomReservationBO> reservationInfoList = new List<RoomReservationBO>();
            reservationInfoList = rrDA.GetRoomReservationInfoForNoShow(searchDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            myGridData.GridPagingProcessing(reservationInfoList, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo ApproveRoomReservationNoShow(int reservationId, int detailId, string reservationStatus)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                RoomReservationDA roomReservationDA = new RoomReservationDA();
                Boolean status = roomReservationDA.UpdateRoomReservationStatus(reservationId, detailId, reservationStatus);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomReservationNoShow.ToString(), reservationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservationNoShow));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
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
        public static ReturnInfo CancelRoomReservationNoShow(int reservationId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                RoomReservationDA roomReservationDA = new RoomReservationDA();
                Boolean status = roomReservationDA.CancelRoomReservationNoShow(reservationId);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomReservationNoShow.ToString(), reservationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservationNoShow));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Cancel, AlertType.Success);
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
    }
}