using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmHMFloorManagement : BasePage
    {
        HiddenField innboardMessage;
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadFloor();
                LoadFloorBlock();
                GenerateRoomAllocation();
                if (ddlFloorId.SelectedIndex != -1 && ddlFloorBlock.SelectedIndex != -1)
                {
                    LoadGridView(Convert.ToInt32(ddlFloorId.SelectedValue), Convert.ToInt32(ddlFloorBlock.SelectedValue));
                }
                CheckPermission();
            }
        }
        protected void gvHMFloorManagement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHMFloorManagement.PageIndex = e.NewPageIndex;
            if (ddlFloorId.SelectedIndex != -1 && ddlFloorBlock.SelectedIndex != -1)
            {
                LoadGridView(Convert.ToInt32(ddlFloorId.SelectedValue), Convert.ToInt32(ddlFloorBlock.SelectedValue));
            }
        }
        protected void gvHMFloorManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblIsSaveValue = (Label)e.Row.FindControl("lblchkIsActiveStatus");
                if (lblIsSaveValue.Text == "Inactive")
                {
                    ((CheckBox)e.Row.FindControl("chkIsActiveStatus")).Checked = false;
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("chkIsActiveStatus")).Checked = true;
                }
            }
        }
        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            if (ddlFloorId.SelectedIndex != -1)
            {
                int counter = 0;
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                foreach (GridViewRow row in gvHMFloorManagement.Rows)
                {
                    counter = counter + 1;
                    bool isSave = ((CheckBox)row.FindControl("chkIsActiveStatus")).Checked;

                    HMFloorManagementBO floorManagement = new HMFloorManagementBO();
                    HMFloorManagementDA floorManagementDa = new HMFloorManagementDA();

                    floorManagement.ActiveStat = isSave;
                    Label lblFloorManagementIdValue = (Label)row.FindControl("lblFloorManagementId");

                    floorManagement.FloorManagementId = Convert.ToInt32(lblFloorManagementIdValue.Text);
                    floorManagement.FloorId = Convert.ToInt32(ddlFloorId.SelectedValue);
                    Label lblgvRoomIdValue = (Label)row.FindControl("lblgvRoomId");
                    floorManagement.RoomId = Convert.ToInt32(lblgvRoomIdValue.Text);
                    floorManagement.BlockId = Convert.ToInt32(ddlFloorBlock.SelectedValue);

                    floorManagement.XCoordinate = 0;
                    floorManagement.YCoordinate = 0;
                    floorManagement.RoomWidth = 73;
                    floorManagement.RoomHeight = 55;

                    int tmpObjectPermissionId;

                    if (lblFloorManagementIdValue.Text == "0")
                    {
                        if (isSave)
                        {
                            floorManagement.CreatedBy = userInformationBO.UserInfoId;
                            Boolean statusSave = floorManagementDa.SaveHMFloorManagementInfo(floorManagement, out tmpObjectPermissionId);
                        }
                    }
                    else
                    {
                        if (!isSave)
                        {
                            DeleteData(Convert.ToInt32(lblFloorManagementIdValue.Text));
                        }
                    }
                }

                if (gvHMFloorManagement.Rows.Count == counter)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.FloorManagement.ToString(), 1,
                    ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Floor Management Added");
                }
            }

            if (ddlFloorId.SelectedIndex != -1 && ddlFloorBlock.SelectedIndex != -1)
            {
                LoadGridView(Convert.ToInt32(ddlFloorId.SelectedValue), Convert.ToInt32(ddlFloorBlock.SelectedValue));
            }

            GenerateRoomAllocation();
        }
        protected void ddlFloorId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFloorId.SelectedIndex != -1)
            {
                isNewAddButtonEnable = -1;
                if (ddlFloorId.SelectedIndex != -1 && ddlFloorBlock.SelectedIndex != -1)
                {
                    LoadGridView(Convert.ToInt32(ddlFloorId.SelectedValue), Convert.ToInt32(ddlFloorBlock.SelectedValue));
                }
            }
        }
        protected void ddlFloorBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFloorId.SelectedIndex != -1)
            {
                isNewAddButtonEnable = -1;
                if (ddlFloorId.SelectedIndex != -1 && ddlFloorBlock.SelectedIndex != -1)
                {
                    LoadGridView(Convert.ToInt32(ddlFloorId.SelectedValue), Convert.ToInt32(ddlFloorBlock.SelectedValue));
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string roomAllocationValue = txtFloorWiseRoomAllocationInfo.Value;
            string[] words;
            words = roomAllocationValue.Split('|');
            if (words.Length > 0)
            {
                for (int i = 0; i < words.Length - 1; i++)
                {
                    int mFloorManagementId = Convert.ToInt32(words[i].Split(',')[0]);
                    decimal mXCoordinate = Convert.ToDecimal(words[i].Split(',')[1]);
                    decimal mYCoordinate = Convert.ToDecimal(words[i].Split(',')[2]);
                    decimal mWidth = Convert.ToDecimal(words[i].Split(',')[3]);
                    decimal mHeight = Convert.ToDecimal(words[i].Split(',')[4]);

                    HMFloorManagementBO floorManagementBO = new HMFloorManagementBO();
                    HMFloorManagementDA floorManagementDA = new HMFloorManagementDA();
                    floorManagementBO.FloorManagementId = mFloorManagementId;

                    floorManagementBO.XCoordinate = mXCoordinate;
                    floorManagementBO.YCoordinate = mYCoordinate;
                    floorManagementBO.RoomWidth = mWidth;
                    floorManagementBO.RoomHeight = mHeight;

                    floorManagementBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean statusSave = floorManagementDA.UpdateHMFloorManagementInfo(floorManagementBO);
                    if (statusSave)
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.FloorManagement.ToString(), floorManagementBO.FloorId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FloorManagement));
                }
            }
            GenerateRoomAllocation();
        }
        protected void ddlSrcFloorId_SelectedIndexChanged(object sender, EventArgs e)
        {
            isNewAddButtonEnable = 1;
            GenerateRoomAllocation();
        }
        protected void ddlSrcFloorBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            isNewAddButtonEnable = 1;
            GenerateRoomAllocation();
        }

        //************************ User Defined Function ********************//
        private void LoadFloor()
        {
            HMFloorDA floorDA = new HMFloorDA();
            List<HMFloorBO> HMFloorBOList = new List<HMFloorBO>();
            HMFloorBOList = floorDA.GetActiveHMFloorInfo();
            ddlFloorId.DataSource = HMFloorBOList;
            ddlFloorId.DataTextField = "FloorName";
            ddlFloorId.DataValueField = "FloorId";
            ddlFloorId.DataBind();

            ddlSrcFloorId.DataSource = HMFloorBOList;
            ddlSrcFloorId.DataTextField = "FloorName";
            ddlSrcFloorId.DataValueField = "FloorId";
            ddlSrcFloorId.DataBind();
        }
        private void LoadFloorBlock()
        {
            FloorBlockDA floorDA = new FloorBlockDA();
            List<FloorBlockBO> floorList = new List<FloorBlockBO>();
            floorList = floorDA.GetActiveFloorBlockInfo();

            ddlFloorBlock.DataSource = floorList;
            ddlFloorBlock.DataTextField = "BlockName";
            ddlFloorBlock.DataValueField = "BlockId";
            ddlFloorBlock.DataBind();

            ddlSrcFloorBlock.DataSource = floorList;
            ddlSrcFloorBlock.DataTextField = "BlockName";
            ddlSrcFloorBlock.DataValueField = "BlockId";
            ddlSrcFloorBlock.DataBind();
        }
        private void LoadGridView(int floorId, int blockId)
        {
           
            if (ddlFloorId.SelectedIndex != -1)
            {
                HMFloorManagementDA floorDA = new HMFloorManagementDA();
                List<HMFloorManagementBO> files = floorDA.GetAllRoomInfoByFloorId(floorId, blockId);

                gvHMFloorManagement.DataSource = files;
                gvHMFloorManagement.DataBind();
            }
            else
            {
                gvHMFloorManagement.DataSource = null;
                gvHMFloorManagement.DataBind();
            }
        }
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
            btnSaveAll.Visible = isSavePermission;
        }
        private void DeleteData(int sEmpId)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            Boolean status = hmCommonDA.DeleteInfoById("HotelFloorManagement", "FloorManagementId", sEmpId);
            if (status)
                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),EntityTypeEnum.EntityType.FloorManagement.ToString(),sEmpId,ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FloorManagement));
        }
        private void GenerateRoomAllocation()
        {
            HMFloorManagementDA roomNumberDA = new HMFloorManagementDA();
            List<HMFloorManagementBO> roomNumberListBO = new List<HMFloorManagementBO>();

            string fullContent = string.Empty;
            int ReservedRoomCount = 0;
            int BookedRoomCount = 0;
            int AvailableRoomCount = 0;
            string roomSummary = string.Empty;

            if (ddlSrcFloorId.SelectedIndex != -1)
            {
                if (!string.IsNullOrWhiteSpace(ddlSrcFloorBlock.SelectedValue))
                {
                    roomNumberListBO = roomNumberDA.GetHMFloorManagementInfoByFloorNBlockId(Convert.ToInt32(ddlSrcFloorId.SelectedValue), Convert.ToInt32(ddlSrcFloorBlock.SelectedValue));

                    string topPart = @"<div class='block FloorRoomAllocationBGImage'>                                                            
                                <a href='#' class='block-heading' data-toggle='collapse'>";

                    string topTemplatePart = @"</a>
                                <div id='FloorRoomAllocation' class='block-body collapse in'>           
                                ";

                    string endTemplatePart = @"</div>
                            </div>";

                    string subContent = string.Empty;

                    for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
                    {
                        if (roomNumberListBO[iRoomNumber].StatusId == 4)
                        {
                            string Content0 = @"<div class='draggable FloorRoomManagementDiv' style='width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px; top:" + roomNumberListBO[iRoomNumber].YCoordinate + "px; left:" + roomNumberListBO[iRoomNumber].XCoordinate + "px;' id='" + roomNumberListBO[iRoomNumber].FloorManagementId + "'>";
                            string Content1 = @"<div class='FloorRoomManagementDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</br>(" + roomNumberListBO[iRoomNumber].RoomType + ")</div></div>";
                            subContent += Content0 + Content1;
                            ReservedRoomCount = ReservedRoomCount + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].StatusId == 3)
                        {
                            string Content0 = @"<div class='draggable FloorRoomManagementDiv' style='width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px; top:" + roomNumberListBO[iRoomNumber].YCoordinate + "px; left:" + roomNumberListBO[iRoomNumber].XCoordinate + "px;' id='" + roomNumberListBO[iRoomNumber].FloorManagementId + "'>";
                            string Content1 = @"<div class='FloorRoomManagementDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</br>(" + roomNumberListBO[iRoomNumber].RoomType + ")</div></div>";
                            subContent += Content0 + Content1;
                            ReservedRoomCount = ReservedRoomCount + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].StatusId == 2)
                        {
                            string Content0 = @"<div class='draggable FloorRoomManagementDiv' style='width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px; top:" + roomNumberListBO[iRoomNumber].YCoordinate + "px; left:" + roomNumberListBO[iRoomNumber].XCoordinate + "px;' id='" + roomNumberListBO[iRoomNumber].FloorManagementId + "'>";
                            string Content1 = @"<div class='FloorRoomManagementDiv'>
                                        </div>
                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</br>(" + roomNumberListBO[iRoomNumber].RoomType + ")</div></div>";
                            subContent += Content0 + Content1;
                            BookedRoomCount = BookedRoomCount + 1;
                        }
                        else if (roomNumberListBO[iRoomNumber].StatusId == 1)
                        {
                            string Content0 = @"<div class='draggable FloorRoomManagementDiv' style='width:" + roomNumberListBO[iRoomNumber].RoomWidth + "px; height:" + roomNumberListBO[iRoomNumber].RoomHeight + "px; top:" + roomNumberListBO[iRoomNumber].YCoordinate + "px; left:" + roomNumberListBO[iRoomNumber].XCoordinate + "px;' id='" + roomNumberListBO[iRoomNumber].FloorManagementId + "'>";
                            string Content1 = @"<a href='/HotelManagement/frmRoomRegistration.aspx?SelectedRoomNumber=" + roomNumberListBO[iRoomNumber].RoomId;
                            string Content2 = @"'><div class='FloorRoomManagementDiv'>
                                        </div></a>
                                        <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</br>(" + roomNumberListBO[iRoomNumber].RoomType + ")</div></div>";

                            subContent += Content0 + Content1 + Content2;
                            AvailableRoomCount = AvailableRoomCount + 1;
                        }
                    }

                    roomSummary = " (Reserved: " + ReservedRoomCount + ", Booked: " + BookedRoomCount + ", Available: " + AvailableRoomCount + ")";
                    fullContent += topPart + topTemplatePart + subContent + endTemplatePart;
                    ReservedRoomCount = 0;
                    BookedRoomCount = 0;
                    AvailableRoomCount = 0;

                    ltlRoomTemplate.Text = fullContent;
                }
            }
        }
    }
}