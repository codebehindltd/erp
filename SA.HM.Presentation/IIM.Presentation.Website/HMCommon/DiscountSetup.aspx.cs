using HotelManagement.Data;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class DiscountSetup : BasePage
    {
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCostCenter();
                LoadAppliedOnDropdownFirstValue();
                LoadCategory();
                LoadRoomType();
                CheckPermission();
            }
        }

        private void LoadAppliedOnDropdownFirstValue()
        {
            ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            ddlDiscountAppliedOn.Items.Insert(0, item);
        }

        private void LoadCategory()
        {
            InvCategoryDA categoryDA = new InvCategoryDA();
            var category = categoryDA.GetAllActiveInvItemCatagoryInfoByServiceType("All");

            ddlCategory.DataSource = category;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCategory.Items.Insert(0, item);
        }

        private void LoadRoomType()
        {
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            var List = roomTypeDA.GetRoomTypeInfo();

            this.ddlRoomType.DataSource = List;
            this.ddlRoomType.DataTextField = "RoomType";
            this.ddlRoomType.DataValueField = "RoomTypeId";
            this.ddlRoomType.DataBind();

            ListItem itemRoomType = new ListItem();
            itemRoomType.Value = "0";
            itemRoomType.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRoomType.Items.Insert(0, itemRoomType);
        }

        private void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> files = costCentreTabDA.GetCostCentreTabInfo().Where(x => x.CostCenterType == "RetailPos").ToList();

            ddlCostCenter.DataSource = files;
            ddlCostCenter.DataTextField = "CostCenter";
            ddlCostCenter.DataValueField = "CostCenterId";
            ddlCostCenter.DataBind();

            ddlSearchCostCenter.DataSource = files;
            ddlSearchCostCenter.DataTextField = "CostCenter";
            ddlSearchCostCenter.DataValueField = "CostCenterId";
            ddlSearchCostCenter.DataBind();

            if (files.Count > 0)
            {
                ListItem item = new ListItem();

                item.Value = "0";
                item.Text = hmUtility.GetDropDownFirstValue();

                ddlCostCenter.Items.Insert(0, item);
                ddlSearchCostCenter.Items.Insert(0, item);
            }

        }

        private void CheckPermission()
        {

            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
        [WebMethod]
        public static List<DiscountDetailBO> GetDiscountItemList(string appliedon, int type, int costcenterid)
        {
            HMCommonDA commonda = new HMCommonDA();
            InvItemDA invitemda = new InvItemDA();
            List<DiscountDetailBO> detailbos = new List<DiscountDetailBO>();

            if (appliedon == "Item")
            {
                List<InvItemBO> invitemlist = new List<InvItemBO>();
                invitemlist = invitemda.GetInvItemInfoByCategory(costcenterid, type);

                List<InvItemBO> distinctitems = new List<InvItemBO>();
                distinctitems = (invitemlist.GroupBy(test => test.ItemId).Select(group => group.First()).ToList()).OrderBy(p => p.Name).ToList(); ;
                detailbos = distinctitems.Select(x => new DiscountDetailBO()
                {
                    DiscountForId = x.ItemId,
                    DiscountForName = x.Name
                }).ToList();
            }
            else if (appliedon == "Category")
            {
                InvCategoryDA categoryda = new InvCategoryDA();
                var categorylist = categoryda.GetInvItemCatagoryByCostCenter("All", costcenterid);

                detailbos = categorylist.Select(x => new DiscountDetailBO()
                {
                    DiscountForId = x.CategoryId,
                    DiscountForName = x.Name
                }).ToList();
            }
            else if (appliedon == "RoomType")
            {
                RoomTypeDA roomtypeda = new RoomTypeDA();
                List<RoomTypeBO> files = roomtypeda.GetRoomTypeInfo();

                detailbos = files.Select(x => new DiscountDetailBO()
                {
                    DiscountForId = x.RoomTypeId,
                    DiscountForName = x.RoomType
                }).ToList();
            }
            else
            {
                RoomNumberDA roomnumberda = new RoomNumberDA();
                List<RoomNumberBO> files = roomnumberda.GetRoomNumberByRoomType(type);

                detailbos = files.Select(x => new DiscountDetailBO()
                {
                    DiscountForId = x.RoomId,
                    DiscountForName = x.RoomNumber
                }).ToList();
            }

            return detailbos;
        }

        [WebMethod]
        public static ReturnInfo SaveOrUpdateDiscount(DiscountMasterBO discount, List<int> deletedItemIdList)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            DiscountDA discountDA = new DiscountDA();
            long discountId;
            bool status=false;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string deletedItemIds = string.Empty;

            if (deletedItemIdList.Count > 0)
                deletedItemIds = string.Join(",", deletedItemIdList);
            try
            {
                if (discount.Id == 0)
                {
                    discount.CreatedBy = userInformationBO.UserInfoId;
                    status = discountDA.SaveDiscount(discount, out discountId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.DiscountMaster.ToString(), discountId,
                                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DiscountMaster));
                        returnInfo.IsSuccess = true;
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                    
                else
                {
                    discount.LastModifiedBy= userInformationBO.UserInfoId;
                    status = discountDA.UpdateDiscount(discount, deletedItemIds);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.DiscountMaster.ToString(), discount.Id,
                                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DiscountMaster));
                        returnInfo.IsSuccess = true;
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }
                if (!status)
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
        public static List<DiscountMasterBO> GetAllDiscount(DateTime fromDate, DateTime toDate, int costCenterId, string Name)
        {
            List<DiscountMasterBO> discountMasterBOs = new List<DiscountMasterBO>();
            DiscountDA discountDA = new DiscountDA();

            discountMasterBOs = discountDA.GetAllDiscount(fromDate, toDate, costCenterId, Name);

            return discountMasterBOs;
        }

        [WebMethod]
        public static DiscountMasterBO GetDiscount(long discountId)
        {
            DiscountMasterBO masterBO = new DiscountMasterBO();
            DiscountDA discountDA = new DiscountDA();

            masterBO = discountDA.GetDiscountById(discountId);
            return masterBO;
        }

        [WebMethod]
        public static ReturnInfo  DeleteDiscount(long discountId)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            DiscountMasterBO masterBO = new DiscountMasterBO();
            DiscountDA discountDA = new DiscountDA();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            try
            {
                status = discountDA.DeleteDiscount(discountId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.DiscountMaster.ToString(), discountId,
                            ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DiscountMaster));
                    returnInfo.IsSuccess = true;
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);

                }
                else
                {
                    returnInfo.IsSuccess = false;
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }

            catch(Exception ex)
            {
                returnInfo.IsSuccess = false;
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                throw ex;
            }
            
            return returnInfo;
        }

        [WebMethod]
        public static string GetDiscountDetails(int id)
        {
            DiscountMasterBO masterBO = new DiscountMasterBO();
            DiscountDA discountDA = new DiscountDA();

            masterBO = discountDA.GetDiscountById(id);

            int row = 0;
            string tr = string.Empty;

            tr = "<table id='tblDiscountDetails' class='table table-bordered table-condensed table-responsive' style='width: 100%;'> " +
                 "<thead> " +
                 "    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'> " +
                 "       <th style='width: 35%;'> " +
                 "            Item " +
                 "        </th> " +
                 "        <th style='width: 35%;'> " +
                 "            Discount Type " +
                 "        </th> " +
                 "        <th style='width: 30%;'> " +
                 "            Discount " +
                 "        </th> " +
                 "    </tr> " +
                 "</thead> " +
                 "<tbody> ";

            foreach (DiscountDetailBO dd in masterBO.DiscountDetails)
            {
                if (row % 2 == 0)
                {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else
                {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:30%;'>" + dd.DiscountForName + "</td>";
                tr += "<td style='width:20%;'>" + dd.DiscountType + "</td>";
                tr += "<td style='width:20%;'>" + dd.Discount + "</td>";
                
                row++;
            }

            tr += "</tbody> " + "</table> ";

            return tr;
        }
        //    [WebMethod]
        //    public static GridViewDataNPaging<DiscountDetailBO, GridPaging> GetDiscountItemList(string appliedOn, int type, int costCenterId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        //    {
        //        int totalRecords = 0;
        //        HMUtility hmUtility = new HMUtility();
        //        UserInformationBO userInformationBO = new UserInformationBO();
        //        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

        //        GridViewDataNPaging<DiscountDetailBO, GridPaging> myGridData = new GridViewDataNPaging<DiscountDetailBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
        //        pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

        //        List<DiscountDetailBO> detailBOs = new List<DiscountDetailBO>();
        //        InvItemDA invItemDA = new InvItemDA();

        //        if (appliedOn == "Item")
        //        {
        //            List<InvItemBO> invItemList = new List<InvItemBO>();
        //            invItemList = invItemDA.GetInvItemInfoByCostCenterIdNCatagoryIdForPagination(costCenterId, type, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

        //            List<InvItemBO> distinctItems = new List<InvItemBO>();
        //            distinctItems = (invItemList.GroupBy(test => test.ItemId).Select(group => group.First()).ToList()).OrderBy(p => p.Name).ToList(); ;
        //            detailBOs = distinctItems.Select(x => new DiscountDetailBO()
        //            {
        //                DiscountForId = x.ItemId,
        //                DiscountForName = x.Name
        //            }).ToList();
        //        }
        //        else if (appliedOn == "Category")
        //        {
        //            InvCategoryDA categoryDA = new InvCategoryDA();
        //            var categoryList = categoryDA.GetInvItemCatagoryByCostCenter("All", costCenterId);

        //            detailBOs = categoryList.Select(x => new DiscountDetailBO()
        //            {
        //                DiscountForId = x.CategoryId,
        //                DiscountForName = x.Name
        //            }).ToList();
        //        }
        //        else if (appliedOn == "RoomType")
        //        {
        //            RoomTypeDA roomTypeDA = new RoomTypeDA();
        //            List<RoomTypeBO> files = roomTypeDA.GetRoomTypeInfo();

        //            detailBOs = files.Select(x => new DiscountDetailBO()
        //            {
        //                DiscountForId = x.RoomTypeId,
        //                DiscountForName = x.RoomType
        //            }).ToList();
        //        }
        //        else
        //        {
        //            RoomNumberDA roomNumberDA = new RoomNumberDA();
        //            List<RoomNumberBO> files = roomNumberDA.GetRoomNumberByRoomType(type);

        //            detailBOs = files.Select(x => new DiscountDetailBO()
        //            {
        //                DiscountForId = x.RoomId,
        //                DiscountForName = x.RoomNumber
        //            }).ToList();
        //        }

        //        //discountMasterBOs = discountDA.GetAllDiscount(fromDate, toDate, costCenterId, Name, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

        //        //allrequisitionList = requisitionDA.GetPMRequisitionInfoByStatus(FromDate, ToDate, status, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

        //        myGridData.GridPagingProcessing(detailBOs, totalRecords);
        //        return myGridData;
        //    }
    }
}