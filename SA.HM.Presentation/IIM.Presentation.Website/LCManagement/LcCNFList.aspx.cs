using HotelManagement.Data.HMCommon;
using HotelManagement.Data.LCManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.LCManagement;
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
    public partial class LcCNFList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static ReturnInfo SaveUpdateCNF(LCCnfInfoBO infoBO)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            int OutId = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            LcCnfInfoDA cnfInfoDA = new LcCnfInfoDA();
            try
            {
                infoBO.CreatedBy = userInformationBO.UserInfoId;
                rtninfo.IsSuccess = cnfInfoDA.SaveCNF(infoBO, out OutId);

                if (rtninfo.IsSuccess)
                {
                    if (infoBO.SupplierId == 0)
                    {
                        
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                EntityTypeEnum.EntityType.LCCnfInfo.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.LCManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCCnfInfo));
                    }
                    else
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                EntityTypeEnum.EntityType.LCCnfInfo.ToString(), infoBO.SupplierId,
                            ProjectModuleEnum.ProjectModule.LCManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCCnfInfo));
                    }
                }
            }
            catch (Exception ex)
            {
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninfo;
        }
        [WebMethod]
        public static GridViewDataNPaging<LCCnfInfoBO, GridPaging> LoadCNFSearch(string name, string code, string email, string phone,int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            LcCnfInfoDA cnfInfoDA = new LcCnfInfoDA();
            List<LCCnfInfoBO> infoBOs = new List<LCCnfInfoBO>();
            int totalRecords = 0;

            GridViewDataNPaging<LCCnfInfoBO, GridPaging> myGridData = new GridViewDataNPaging<LCCnfInfoBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            infoBOs = cnfInfoDA.GetCNFInformation(name, code, email, phone, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            myGridData.GridPagingProcessing(infoBOs, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static LCCnfInfoBO FillForm(int Id)
        {
            LcCnfInfoDA cnfInfoDA = new LcCnfInfoDA();
            LCCnfInfoBO infoBO = new LCCnfInfoBO();
            infoBO = cnfInfoDA.GetCNFInformationById(Id);

            return infoBO;
        }
        [WebMethod]
        public static ReturnInfo DeleteCNF(long Id)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            status = DA.DeleteInfoById("LCCnfInfo","SupplierId",Id);
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.LCCnfInfo.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.LCManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCCnfInfo));
            }
            return rtninf;
        }
    }
}