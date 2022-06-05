using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.GeneralLedger;
using Newtonsoft.Json;
using HotelManagement.Entity.LCManagement;
using HotelManagement.Data.LCManagement;

namespace HotelManagement.Presentation.Website.LCManagement
{
    public partial class frmLCOverHeadName : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            //innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {                
                LoadIncomeAccountHead();               
            }
        }
        //************************ User Defined Function ********************//        
        private void LoadIncomeAccountHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();

            this.ddlIncomeAccountHead.DataSource = entityDA.GetNodeMatrixInfoByAncestorNodeId(14).Where(x => x.IsTransactionalHead == true).ToList();
            this.ddlIncomeAccountHead.DataTextField = "HeadWithCode";
            this.ddlIncomeAccountHead.DataValueField = "NodeId";
            this.ddlIncomeAccountHead.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlIncomeAccountHead.Items.Insert(0, item);
        }        
        private bool IsFrmValid()
        {
            bool flag = true;

            if (txtServiceName.Text == "")
            {                                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Service Name.", AlertType.Warning);
                flag = false;
                txtServiceName.Focus();
            }
            else if (ddlIncomeAccountHead.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Service Name.", AlertType.Warning);
                flag = false;
                txtServiceName.Focus();
            }
            return flag;
        }
       
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static ReturnInfo SaveUpdateLCOverHeadInformation(OverHeadNameBO overHeadNameBO)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (overHeadNameBO.OverHeadId == 0)
            {
                overHeadNameBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            else
            {
                overHeadNameBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            int OutId;
            OverHeadNameDA DA = new OverHeadNameDA(); 

            status = DA.SaveLCOverHeadNameInfo(overHeadNameBO, out OutId);
            if (status)
            {
                if (overHeadNameBO.OverHeadId == 0)
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LCOverHead.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCOverHead));

                }
                else
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.LCOverHead.ToString(), OutId,
                           ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCOverHead));
                }


            }
            else
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }
            return rtninfo;
        }
        [WebMethod]
        public static GridViewDataNPaging<OverHeadNameBO, GridPaging> SearchPaidServiceAndLoadGridInformation(string serviceName, string serviceType, string activeStat, string IsCNFHead, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<OverHeadNameBO, GridPaging> myGridData = new GridViewDataNPaging<OverHeadNameBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            OverHeadNameDA paidServiceDA = new OverHeadNameDA();
            List<OverHeadNameBO> paidServiceList = new List<OverHeadNameBO>();
            paidServiceList = paidServiceDA.GetOverHeadInfoBySearchCriteriaForPagination(serviceName, serviceType, activeStat, IsCNFHead, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<OverHeadNameBO> distinctItems = new List<OverHeadNameBO>();
            distinctItems = paidServiceList.GroupBy(test => test.OverHeadId).Select(group => group.First()).ToList();


            //myGridData.GridPagingProcessing(guestInfoList, totalRecords);
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo DeletePaidServiceById(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();   
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                //OverHeadNameDA paidServiceDA = new OverHeadNameDA();
                //Boolean status = paidServiceDA.DeletePaidServiceById(sEmpId);
                //if (status)
                //{
                //    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                //             EntityTypeEnum.EntityType.HotelService.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.HotelManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelService));
                //    rtninf.IsSuccess = true;
                //    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                //}
            }
            catch(Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate, string id)
        {
            string tableName = "LCOverHeadName";
            string pkFieldName = "OverHeadId";
            string pkFieldValue = id;
            int IsDuplicate = 0;
            if (!string.IsNullOrWhiteSpace(pkFieldValue))
            {
                isUpdate = 1;
            }
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        [WebMethod]
        public static OverHeadNameBO FillForm(int Id)
        {

            OverHeadNameBO LCOverHeadBO = new OverHeadNameBO();
            OverHeadNameDA DA = new OverHeadNameDA();
            LCOverHeadBO = DA.GetLCOverHeadNameInfoById(Id);

            return LCOverHeadBO;
        }
        [WebMethod]
        public static ReturnInfo DeleteLCOverHead(long Id)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            status = DA.DeleteInfoById("LCOverHeadName", "OverHeadId",Id);
            if (status)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.LCOverHead.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCOverHead));
            }
            return rtninfo;
        }
    }
}