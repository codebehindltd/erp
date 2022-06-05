using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Entity.HouseKeeping;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.HouseKeeping
{
    public partial class frmHKRoomConditions : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadRoomConditions();
            }
        }
        protected void gvRoomConditions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ListBox ddlRoomConditions = (ListBox)e.Row.FindControl("ddlRoomConditions");
                HKRoomStatusDA HKRoomStatusDA = new HKRoomStatusDA();
                List<HotelRoomConditionBO> files = HKRoomStatusDA.GetHotelRoomConditions();

                ddlRoomConditions.DataSource = files;
                ddlRoomConditions.DataTextField = "ConditionName";
                ddlRoomConditions.DataValueField = "RoomConditionId";
                ddlRoomConditions.DataBind();
                if (((e.Row.DataItem) as HKRoomStatusViewBO).RoomConditionList.Count > 0)
                {
                    var selectedItems = ((e.Row.DataItem) as HKRoomStatusViewBO).RoomConditionList.Select(i => i.RoomConditionId.ToString()).ToList();
                    var roomId = Int32.Parse(((e.Row.DataItem) as HKRoomStatusViewBO).RoomId.ToString());


                    foreach (string item in selectedItems)
                    {
                        if(item!="0")
                        {
                            ddlRoomConditions.Items.FindByValue(item).Selected = true;
                        }
                    }
                       

                    foreach (string item in selectedItems)
                    {
                        var a = Int32.Parse(item);
                        var b = roomId;
                    }
                }
                CheckBox cb = (CheckBox)e.Row.FindControl("chkIsSavePermission");
                Label lblRoomConId = (Label)e.Row.FindControl("lblRoomConId");

                if (lblRoomConId != null)
                {
                    if (Convert.ToInt64(lblRoomConId.Text) > 0)
                    {
                        cb.Checked = true;
                    }
                }
            }
        }
        protected void gvRoomConditions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvRoomConditions.PageIndex = e.NewPageIndex;
            this.LoadRoomConditions();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<HotelDailyRoomConditionBO> saveRoomConditionList = new List<HotelDailyRoomConditionBO>();
            List<HotelDailyRoomConditionBO> editRoomConditionList = new List<HotelDailyRoomConditionBO>();
            List<HotelDailyRoomConditionBO> deleteRoomConditionList = new List<HotelDailyRoomConditionBO>();
            List<HotelDailyRoomConditionBO> alrSavedRoomConditionList = new List<HotelDailyRoomConditionBO>();
            HKRoomStatusDA roomStatusDA = new HKRoomStatusDA();

            int rows = gvRoomConditions.Rows.Count;
            int x = 0;

            for (int i = 0; i < rows; i++)
            {
                
                List<CheckBox> checkBoxes = new List<CheckBox>();
                checkBoxes.Add((CheckBox)gvRoomConditions.Rows[i].FindControl("chkIsSavePermission"));
                CheckBox cb = (CheckBox)gvRoomConditions.Rows[i].FindControl("chkIsSavePermission");

                HotelDailyRoomConditionBO bo = new HotelDailyRoomConditionBO();
                Label lblRoomId = (Label)gvRoomConditions.Rows[i].FindControl("lblRoomId");
                ListBox ddlConId = (ListBox)gvRoomConditions.Rows[i].FindControl("ddlRoomConditions");
                Label lblRoomConId = (Label)gvRoomConditions.Rows[i].FindControl("lblRoomConId");

                if (cb.Checked == true)
                {
                    x++;
                    List<int> RoomConditionSelected = new List<int>();
                    List<int> RoomConditionNotSelected = new List<int>() { 1,2,3,4};
                    int a = Convert.ToInt32((gvRoomConditions.Rows[i].FindControl("lblRoomId") as Label).Text);
                    foreach (ListItem listItem in ddlConId.Items)
                    {
                        if (listItem.Selected == true)
                        {
                            var val = Convert.ToInt32(listItem.Value);
                            RoomConditionSelected.Add(val);
                            RoomConditionNotSelected.Remove(val);
                        }
                    }
                    if(RoomConditionSelected.Count<=0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "You have checked room but did not select room condition", AlertType.Warning);
                        return;
                    }
                    
                    foreach (int item in RoomConditionSelected)
                    {
                        roomStatusDA.SaveRoomCondition(a, item);
                    }
                    foreach (int item in RoomConditionNotSelected)
                    {
                        roomStatusDA.DeleteRoomCondition(a, item);
                    }

                }
                else
                {
                    if (x==0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please Check the Room", AlertType.Information);
                        return;
                    }    
                    int a = Convert.ToInt32((gvRoomConditions.Rows[i].FindControl("lblRoomId") as Label).Text);
                    roomStatusDA.DeleteUncheckedRoom(a);      
                }
            }

            CommonHelper.AlertInfo(innboardMessage, " Operation Successful", AlertType.Success);
        }
        //************************ User Defined Function ********************//
        private void LoadRoomConditions()
        {
            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusViewBO> roomStatusList = new List<HKRoomStatusViewBO>();
            List<HotelDailyRoomConditionBO> roomConditionList = new List<HotelDailyRoomConditionBO>();
            int floorId = 0;
            roomStatusList = hkRoomStatusDA.GetRoomStatus(floorId, 0, 0);
            roomConditionList = hkRoomStatusDA.GetDailyRoomCondition();

            foreach (HKRoomStatusViewBO bo in roomStatusList)
            {
                var list = roomConditionList.Where(m => m.RoomId == bo.RoomId).ToList();
                if (list.Count > 0)
                {
                    roomStatusList.Where(m => m.RoomId == bo.RoomId).FirstOrDefault().RoomConditionList = list;
                    roomStatusList.Where(m => m.RoomId == bo.RoomId).FirstOrDefault().DailyRoomConditionId = list.FirstOrDefault().DailyRoomConditionId;
                }

            }

            this.gvRoomConditions.DataSource = roomStatusList;
            this.gvRoomConditions.DataBind();
        }
    }
}