using HotelManagement.Data;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class RoomFeaturesInfo : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        HMCommonDA hmCommonDA = new HMCommonDA();

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
        }

        private void Cancel()
        {
            txtRoomNumber.Text = string.Empty;
            txtRoomtype.Text = string.Empty;
            lblAllFeatures.Visible = false;
        }

        [WebMethod]
        public static List<RoomFeaturesBO> GetActiveRoomFeatures(long roomId)
        {
            List<RoomFeaturesBO> roomFtList = new List<RoomFeaturesBO>();
            RoomFeaturesDA roomFeatureDA = new RoomFeaturesDA();

            List<RoomFeaturesBO> duplicateList = new List<RoomFeaturesBO>();

            List<RoomFeaturesInfoBO> roomFeaturesInfos = new List<RoomFeaturesInfoBO>();
            RoomFeaturesInfoDA roomFeaturesInfoDA = new RoomFeaturesInfoDA();

            roomFeaturesInfos = roomFeaturesInfoDA.GetRoomFtInfoByRoomId(roomId);

            roomFtList = roomFeatureDA.GetAllActiveRoomFeatures();

            var v = (from data in roomFtList
                     where !(
                     from items in roomFeaturesInfos
                     select items.FeaturesId).Contains(data.Id)
                     select data
                        ).ToList();

            return v;
        }

        [WebMethod]
        public static RoomNumberBO GetRoomInfoByRoomNumber(string roomNum)
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            RoomNumberBO room = new RoomNumberBO();

            room = roomNumberDA.GetRoomInfoByRoomNumber(roomNum);
            return room;
        }

        [WebMethod]
        public static List<RoomFeaturesBO> GetRoomFeaturesByRoomId(long roomId)
        {
            List<RoomFeaturesInfoBO> roomFeaturesInfoList = new List<RoomFeaturesInfoBO>();
            RoomFeaturesInfoDA roomFeaturesInfoDA = new RoomFeaturesInfoDA();

            List<RoomFeaturesBO> roomFtList = new List<RoomFeaturesBO>();
            RoomFeaturesDA roomFeatureDA = new RoomFeaturesDA();

            roomFeaturesInfoList = roomFeaturesInfoDA.GetRoomFtInfoByRoomId(roomId);

            roomFtList = roomFeatureDA.GetAllActiveRoomFeatures();

            //List<RoomFeaturesBO> filteredList = new List<RoomFeaturesBO>();
            var v = (from r in roomFtList
                     join f in roomFeaturesInfoList
                      on r.Id equals f.FeaturesId
                     select r
                        ).ToList();
            return v;
        }

        [WebMethod]
        public static ReturnInfo SaveRoomFeaturesInfo(long[] featuresIds, string roomNumber, long roomId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            long tempId = 0;

            try
            {
                HMUtility hmUtility = new HMUtility();
                HMCommonDA hmCommonDA = new HMCommonDA();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                List<RoomFeaturesInfoBO> alreadySavedFeatureRoomList = new List<RoomFeaturesInfoBO>();

                RoomFeaturesInfoDA roomFeaturesInfoDA = new RoomFeaturesInfoDA();
                RoomFeaturesInfoBO roomFeaturesInfoBO = new RoomFeaturesInfoBO();

                //get already saved features for that room
                alreadySavedFeatureRoomList = roomFeaturesInfoDA.GetRoomFtInfoByRoomId(roomId); 

                List<RoomFeaturesInfoBO> newList = new List<RoomFeaturesInfoBO>();

                for (int i = 0; i < featuresIds.Length; i++)
                {
                    RoomFeaturesInfoBO roomFeaturesNew = new RoomFeaturesInfoBO
                    {
                        RoomId = roomId,
                        FeaturesId = featuresIds[i],
                    };
                    newList.Add(roomFeaturesNew);
                }// put added features in a seperate list with room ID


                List<RoomFeaturesInfoBO> tempList = new List<RoomFeaturesInfoBO>(newList);

                var newAddedFeatures = (from p in newList
                                        where !(
                                        from c in alreadySavedFeatureRoomList
                                        select c.FeaturesId).Contains(p.FeaturesId)
                                        select p).ToList();

                var deleteFeatures = (from data in alreadySavedFeatureRoomList
                                      where !(
                                      from items in newList
                                      select items.FeaturesId).Contains(data.FeaturesId)
                                      select data).ToList();

                var updateFeatures = (from data in alreadySavedFeatureRoomList
                                      where (
                                      from items in newList
                                      select items.FeaturesId).Contains(data.FeaturesId)
                                      select data).ToList();

                
                if (newAddedFeatures.Count >0)
                {
                    rtninf.IsSuccess = roomFeaturesInfoDA.SaveRoomFeaturesInfo(newAddedFeatures, userInformationBO.UserInfoId, out tempId);

                    if (rtninf.IsSuccess)
                    {
                        rtninf.IsSuccess = true;

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), 
                            EntityTypeEnum.EntityType.RoomFeaturesInfo.ToString(), tempId, 
                            ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), 
                            "Room features info saved for room "+roomNumber);

                        rtninf.AlertMessage = CommonHelper.AlertInfo("Features Updated for the room no. "+roomNumber, AlertType.Success);
                        //return true;
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }

                if(deleteFeatures.Count>0)
                {

                    rtninf.IsSuccess = roomFeaturesInfoDA.DeleteRoomFeatureInfo(deleteFeatures);

                    if (rtninf.IsSuccess)
                    {
                       
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                           EntityTypeEnum.EntityType.RoomFeaturesInfo.ToString(), 0,
                           ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                           "Room features info deleted for room " + roomNumber);

                        rtninf.AlertMessage = CommonHelper.AlertInfo("Features Updated for the room no. " + roomNumber, AlertType.Success);
                        //return true;
                    }
                    else
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }
                if (updateFeatures.Count>0)
                {
                    rtninf.IsSuccess = roomFeaturesInfoDA.UpdateRoomFeaturesInfo(updateFeatures, userInformationBO.UserInfoId);

                    if (rtninf.IsSuccess)
                    {
                        
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.RoomFeaturesInfo.ToString(), 0,
                           ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                           "Room features info updated for room " + roomNumber);

                        rtninf.AlertMessage = CommonHelper.AlertInfo("Features Updated for the room no. " + roomNumber, AlertType.Success);
                        //return true;
                    }
                }

            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                throw ex;
            }

            return rtninf;
        }
    }
}