using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Entity.HouseKeeping;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.HouseKeeping
{
    public partial class frmHKRoomDiscrepancies : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadRoomDiscrepancies();
            }
        }
        protected void gvRoomDiscrepancies_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvRoomDiscrepancies.PageIndex = e.NewPageIndex;
            this.LoadRoomDiscrepancies();
        }
        protected void gvRoomDiscrepancies_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                DropDownList ddlHKRoomStatus = (DropDownList)e.Row.FindControl("ddlHKRoomStatus");
                DropDownList ddlHKPersons = (DropDownList)e.Row.FindControl("ddlHKPersons");
                DropDownList ddlDiscrepancy = (DropDownList)e.Row.FindControl("ddlDiscrepancy");

                HKRoomStatusDA HKRoomStatusDA = new HKRoomStatusDA();
                List<HKRoomStatusBO> files = HKRoomStatusDA.GetHKRoomStatusType().Where(x => x.StatusName == "Occupied" || x.StatusName == "Vacant").ToList();

                ddlHKRoomStatus.DataSource = files;
                ddlHKRoomStatus.DataTextField = "StatusName";
                ddlHKRoomStatus.DataValueField = "HKRoomStatusId";
                ddlHKRoomStatus.DataBind();

                string foRoomStatus = (e.Row.FindControl("lblFORoomStatus") as Label).Text;
                if (!string.IsNullOrEmpty(foRoomStatus))
                    ddlHKRoomStatus.Items.FindByText(foRoomStatus).Selected = true;

                string hkPersons = (e.Row.FindControl("lblHKPersons") as Label).Text;
                if (!string.IsNullOrEmpty(hkPersons))
                    ddlHKPersons.Items.FindByText(hkPersons).Selected = true;

                string discrepancy = (e.Row.FindControl("lblDiscrepancy") as Label).Text;
                if (!string.IsNullOrEmpty(discrepancy))
                    ddlDiscrepancy.Items.FindByText(discrepancy).Selected = true;

                CheckBox cb = (CheckBox)e.Row.FindControl("chkIsSavePermission");
                Label lblRoomDiscreId = (Label)e.Row.FindControl("lblRoomDiscrepancyId");

                if (lblRoomDiscreId != null)
                {
                    if (Convert.ToInt64(lblRoomDiscreId.Text) > 0)
                    {
                        cb.Checked = true;
                    }
                }
                
                string foPersons = (e.Row.FindControl("lblFOPersons") as Label).Text;
                ddlDiscrepancy.Enabled = false;

                int rowIndex = e.Row.RowIndex;
                ddlHKRoomStatus.Attributes["onclick"] = "javascript:return SetDiscrepancy('" + rowIndex + "', '" + foRoomStatus + "', '" + foPersons + "');";
                ddlHKPersons.Attributes["onclick"] = "javascript:return SetDiscrepancy('" + rowIndex + "', '" + foRoomStatus + "', '" + foPersons + "');";
            }
        }
        protected void ddlHKRoomStatus_Change(object sender, EventArgs e)
        {
            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusViewBO> roomStatusList = new List<HKRoomStatusViewBO>();
            List<HotelRoomDiscrepancyBO> roomDiscreList = new List<HotelRoomDiscrepancyBO>();

            int floorId = 0;
            roomStatusList = hkRoomStatusDA.GetRoomStatus(floorId, 0, 0);
            roomDiscreList = hkRoomStatusDA.GetRoomDiscrepancy();

            roomStatusList = roomStatusList.Where(a => a.FORoomStatus == "Occupied" || a.FORoomStatus == "Vacant").ToList();
            foreach (HKRoomStatusViewBO bo in roomStatusList)
            {
                bo.HKRoomStatus = bo.FORoomStatus;
            }

            foreach (HotelRoomDiscrepancyBO bo in roomDiscreList)
            {
                HKRoomStatusViewBO viewBO = roomStatusList.Where(m => m.RoomId == bo.RoomId).FirstOrDefault();
                viewBO.RoomDiscrepancyId = bo.RoomDiscrepancyId;
                viewBO.HKRoomStatus = bo.HKStatusName;
                viewBO.FOPersons = bo.FOPersons;
                viewBO.HKPersons = bo.HKPersons;
                viewBO.DiscrepanciesDetails = bo.DiscrepanciesDetails;
            }

            int rows = gvRoomDiscrepancies.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                Label lblRoomId = (Label)gvRoomDiscrepancies.Rows[i].FindControl("lblRoomId");
                DropDownList ddlHKRoomStatus = (DropDownList)gvRoomDiscrepancies.Rows[i].FindControl("ddlHKRoomStatus");
                DropDownList ddlHKPersons = (DropDownList)gvRoomDiscrepancies.Rows[i].FindControl("ddlHKPersons");

                int roomId = 0;
                if (!string.IsNullOrEmpty(lblRoomId.Text))
                {
                    roomId = Convert.ToInt32(lblRoomId.Text);
                }
                HKRoomStatusViewBO viewBO = roomStatusList.Where(m => m.RoomId == roomId).FirstOrDefault();
                viewBO.HKRoomStatus = ddlHKRoomStatus.SelectedItem.Text;
                viewBO.HKPersons = Convert.ToInt32(ddlHKPersons.SelectedValue);
            }

            foreach (HKRoomStatusViewBO bo in roomStatusList)
            {
                if (bo.FORoomStatus == "Occupied" && bo.HKRoomStatus == "Vacant")
                {
                    bo.DiscrepanciesDetails = "Skip";
                }
                else if (bo.FORoomStatus == "Vacant" && bo.HKRoomStatus == "Occupied")
                {
                    bo.DiscrepanciesDetails = "Sleep";
                }
                else if (bo.FORoomStatus == "Occupied" && bo.HKRoomStatus == "Occupied")
                {
                    if (bo.FOPersons == bo.HKPersons)
                    {
                        bo.DiscrepanciesDetails = "None";
                    }
                    else bo.DiscrepanciesDetails = "Person";
                }
                else if (bo.FORoomStatus == "Vacant" && bo.HKRoomStatus == "Vacant")
                {
                    bo.DiscrepanciesDetails = "None";
                }
            }


            this.gvRoomDiscrepancies.DataSource = roomStatusList;
            this.gvRoomDiscrepancies.DataBind();
        }
        protected void ddlHKPersons_Change(object sender, EventArgs e)
        {
            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusViewBO> roomStatusList = new List<HKRoomStatusViewBO>();
            List<HotelRoomDiscrepancyBO> roomDiscreList = new List<HotelRoomDiscrepancyBO>();
            int floorId = 0;
            roomStatusList = hkRoomStatusDA.GetRoomStatus(floorId, 0, 0);
            roomDiscreList = hkRoomStatusDA.GetRoomDiscrepancy();

            roomStatusList = roomStatusList.Where(a => a.FORoomStatus == "Occupied" || a.FORoomStatus == "Vacant").ToList();
            foreach (HKRoomStatusViewBO bo in roomStatusList)
            {
                bo.HKRoomStatus = bo.FORoomStatus;
            }

            foreach (HotelRoomDiscrepancyBO bo in roomDiscreList)
            {
                HKRoomStatusViewBO viewBO = roomStatusList.Where(m => m.RoomId == bo.RoomId).FirstOrDefault();
                viewBO.RoomDiscrepancyId = bo.RoomDiscrepancyId;
                viewBO.HKRoomStatus = bo.HKStatusName;
                viewBO.FOPersons = bo.FOPersons;
                viewBO.HKPersons = bo.HKPersons;
                viewBO.DiscrepanciesDetails = bo.DiscrepanciesDetails;
            }

            int rows = gvRoomDiscrepancies.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                Label lblRoomId = (Label)gvRoomDiscrepancies.Rows[i].FindControl("lblRoomId");
                DropDownList ddlHKPersons = (DropDownList)gvRoomDiscrepancies.Rows[i].FindControl("ddlHKPersons");
                DropDownList ddlHKRoomStatus = (DropDownList)gvRoomDiscrepancies.Rows[i].FindControl("ddlHKRoomStatus");

                int roomId = 0;
                if (!string.IsNullOrEmpty(lblRoomId.Text))
                {
                    roomId = Convert.ToInt32(lblRoomId.Text);
                }
                HKRoomStatusViewBO viewBO = roomStatusList.Where(m => m.RoomId == roomId).FirstOrDefault();
                viewBO.HKPersons = Convert.ToInt32(ddlHKPersons.SelectedValue);
                viewBO.HKRoomStatus = ddlHKRoomStatus.SelectedItem.Text;
            }

            foreach (HKRoomStatusViewBO bo in roomStatusList)
            {
                if (bo.FORoomStatus == "Occupied" && bo.HKRoomStatus == "Vacant")
                {
                    bo.DiscrepanciesDetails = "Skip";
                }
                else if (bo.FORoomStatus == "Vacant" && bo.HKRoomStatus == "Occupied")
                {
                    bo.DiscrepanciesDetails = "Sleep";
                }
                else if (bo.FORoomStatus == "Occupied" && bo.HKRoomStatus == "Occupied")
                {
                    if (bo.FOPersons == bo.HKPersons)
                    {
                        bo.DiscrepanciesDetails = "None";
                    }
                    else bo.DiscrepanciesDetails = "Person";
                }
                else if (bo.FORoomStatus == "Vacant" && bo.HKRoomStatus == "Vacant")
                {
                    bo.DiscrepanciesDetails = "None";
                }
            }


            this.gvRoomDiscrepancies.DataSource = roomStatusList;
            this.gvRoomDiscrepancies.DataBind();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<HotelRoomDiscrepancyBO> saveDiscreList = new List<HotelRoomDiscrepancyBO>();
            List<HotelRoomDiscrepancyBO> editDiscreList = new List<HotelRoomDiscrepancyBO>();
            List<HotelRoomDiscrepancyBO> deleteDiscreList = new List<HotelRoomDiscrepancyBO>();
            List<HotelRoomDiscrepancyBO> alrSavedDiscreList = new List<HotelRoomDiscrepancyBO>();
            HKRoomStatusDA roomStatusDA = new HKRoomStatusDA();
            

            int rows = gvRoomDiscrepancies.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                HotelRoomDiscrepancyBO bo = new HotelRoomDiscrepancyBO();

                CheckBox cb = (CheckBox)gvRoomDiscrepancies.Rows[i].FindControl("chkIsSavePermission");
                if (cb.Checked == true)
                {
                    

                    Label lblRoomDiscrepancyId = (Label)gvRoomDiscrepancies.Rows[i].FindControl("lblRoomDiscrepancyId");
                    Label lblRoomId = (Label)gvRoomDiscrepancies.Rows[i].FindControl("lblRoomId");
                    DropDownList ddlHKRoomStatus = (DropDownList)gvRoomDiscrepancies.Rows[i].FindControl("ddlHKRoomStatus");
                    Label lblFOPersons = (Label)gvRoomDiscrepancies.Rows[i].FindControl("lblFOPersons");
                    DropDownList ddlHKPersons = (DropDownList)gvRoomDiscrepancies.Rows[i].FindControl("ddlHKPersons");
                    DropDownList ddlDiscrepancy = (DropDownList)gvRoomDiscrepancies.Rows[i].FindControl("ddlDiscrepancy");
                    TextBox txtReason = (TextBox)gvRoomDiscrepancies.Rows[i].FindControl("txtReason");
                    Label lblFORoomStatus = (Label)gvRoomDiscrepancies.Rows[i].FindControl("lblFORoomStatus");

                    bo.RoomDiscrepancyId = Convert.ToInt64(lblRoomDiscrepancyId.Text);
                    bo.RoomId = Convert.ToInt32(lblRoomId.Text);
                    bo.HKRoomStatusId = Convert.ToInt64(ddlHKRoomStatus.SelectedValue);
                    if (lblFOPersons != null)
                    {
                        if (!string.IsNullOrEmpty(lblFOPersons.Text))
                            bo.FOPersons = Convert.ToInt32(lblFOPersons.Text);
                        else bo.FOPersons = 0;
                    }
                    else bo.FOPersons = 0;
                    if (ddlHKPersons != null)
                    {
                        if (ddlHKPersons.Text != "0")
                            bo.HKPersons = Convert.ToInt32(ddlHKPersons.SelectedValue);
                        else bo.HKPersons = 0;
                    }
                    else bo.HKPersons = 0;
                    bo.DiscrepanciesDetails = ddlDiscrepancy.Text;
                    bo.Reason = txtReason.Text;
                    bo.TaskId = 0;

                    if (lblFORoomStatus.Text == "Occupied" && ddlHKRoomStatus.SelectedItem.Text == "Vacant")
                    {
                        bo.DiscrepanciesDetails = "Skip";
                    }
                    else if (lblFORoomStatus.Text == "Vacant" && ddlHKRoomStatus.SelectedItem.Text == "Occupied")
                    {
                        bo.DiscrepanciesDetails = "Sleep";
                    }
                    else if (lblFORoomStatus.Text == "Occupied" && ddlHKRoomStatus.SelectedItem.Text == "Occupied")
                    {
                        if (Convert.ToInt32(lblFOPersons.Text) != Convert.ToInt32(ddlHKPersons.Text))
                        {
                            bo.DiscrepanciesDetails = "Person";
                        }
                        else
                            bo.DiscrepanciesDetails = "None";
                    }
                    else if (lblFORoomStatus.Text == "Vacant" && ddlHKRoomStatus.SelectedItem.Text == "Vacant")
                    {
                        bo.DiscrepanciesDetails = "None";
                    }

                    saveDiscreList.Add(bo);

                }
                else
                {
                    Label lblRoomDiscrepancyId = (Label)gvRoomDiscrepancies.Rows[i].FindControl("lblRoomDiscrepancyId");
                    if (lblRoomDiscrepancyId != null)
                    {
                        if (lblRoomDiscrepancyId.Text != "0")
                        {
                            //HotelRoomDiscrepancyBO bo = new HotelRoomDiscrepancyBO();
                            bo.RoomDiscrepancyId = Convert.ToInt64(lblRoomDiscrepancyId.Text);
                            deleteDiscreList.Add(bo);
                            
                        }
                    }
                }
            }

            alrSavedDiscreList = roomStatusDA.GetRoomDiscrepancy();
            foreach (HotelRoomDiscrepancyBO savedBO in alrSavedDiscreList)
            {
                var item = saveDiscreList.Where(m => m.RoomId == savedBO.RoomId).FirstOrDefault();
                if (item != null)
                {
                    editDiscreList.Add(item);
                    saveDiscreList.Remove(item);
                }
            }            

            bool status = false;
            if (saveDiscreList.Count > 0)
            {
                status = roomStatusDA.SaveRoomDiscrepancy(saveDiscreList);

                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.HotelRoomDiscrepancy.ToString(),
                            0, ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),"Hotel room discrepancy saved");
                    
                        
                    
                }
                
            }

            if (editDiscreList.Count > 0)
            {
                status = roomStatusDA.UpdateRoomDiscrepancy(editDiscreList);

                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.HotelRoomDiscrepancy.ToString(),
                           1,/*bo.RoomDiscrepancyId,*/ ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                           "Room Discrepancy Updated");
                    
                       
                    
                }
                
            }

            if (deleteDiscreList.Count > 0)
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                foreach (var item in deleteDiscreList)
                {
                    status = hmCommonDA.DeleteInfoById("HotelRoomDiscrepancy", "RoomDiscrepancyId", item.RoomDiscrepancyId);
                }

                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                   
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.HotelRoomDiscrepancy.ToString(),
                          1,/*bo.RoomDiscrepancyId,*/ ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                          "Room Discrepancy Deleted");
                    
                       
                    
                }
            }


        }
        //************************ User Defined Function ********************//
        private void LoadRoomDiscrepancies()
        {
            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusViewBO> roomStatusList = new List<HKRoomStatusViewBO>();
            List<HotelRoomDiscrepancyBO> roomDiscreList = new List<HotelRoomDiscrepancyBO>();
            int floorId = 0;
            roomStatusList = hkRoomStatusDA.GetRoomStatus(floorId, 0, 0);
            roomDiscreList = hkRoomStatusDA.GetRoomDiscrepancy();

            roomStatusList = roomStatusList.Where(a => a.FORoomStatus == "Occupied" || a.FORoomStatus == "Vacant").ToList();
            foreach (HKRoomStatusViewBO bo in roomStatusList)
            {
                bo.HKRoomStatus = bo.FORoomStatus;
                bo.HKPersons = bo.FOPersons;
            }

            foreach (HotelRoomDiscrepancyBO bo in roomDiscreList)
            {
                HKRoomStatusViewBO viewBO = roomStatusList.Where(m => m.RoomId == bo.RoomId).FirstOrDefault();
                if (viewBO != null)
                {
                    viewBO.RoomDiscrepancyId = bo.RoomDiscrepancyId;
                    viewBO.HKRoomStatus = bo.HKStatusName;
                    viewBO.FOPersons = bo.FOPersons;
                    viewBO.HKPersons = bo.HKPersons;
                    viewBO.DiscrepanciesDetails = bo.DiscrepanciesDetails;
                    viewBO.Reason = bo.Reason;
                }
            }

            foreach (HKRoomStatusViewBO bo in roomStatusList)
            {
                if (bo.FORoomStatus == "Occupied" && bo.FOPersons == 0)
                {
                    bo.FOPersons = 1;
                    bo.HKPersons = 1;
                }
                if (bo.FORoomStatus == "Vacant" && bo.HKPersons > 0)
                {
                    bo.FOPersons = 0;
                    bo.HKPersons = 0;
                }
            }


            foreach (HKRoomStatusViewBO bo in roomStatusList)
            {
                if (bo.FORoomStatus == "Occupied" && bo.HKRoomStatus == "Vacant")
                {
                    bo.DiscrepanciesDetails = "Skip";
                }
                else if (bo.FORoomStatus == "Vacant" && bo.HKRoomStatus == "Occupied")
                {
                    bo.DiscrepanciesDetails = "Sleep";
                }
                else if (bo.FORoomStatus == "Occupied" && bo.HKRoomStatus == "Occupied")
                {
                    if (bo.FOPersons != bo.HKPersons)
                    {
                        bo.DiscrepanciesDetails = "Person";
                    }
                    else
                    {
                        bo.DiscrepanciesDetails = "None";
                        //bo.DiscrepanciesDetails = "Skip";                        
                    }
                }
                else if (bo.FORoomStatus == "Vacant" && bo.HKRoomStatus == "Vacant")
                {
                    bo.DiscrepanciesDetails = "None";
                    //bo.DiscrepanciesDetails = "Skip";
                }
            }

            this.gvRoomDiscrepancies.DataSource = roomStatusList;
            this.gvRoomDiscrepancies.DataBind();
        }
    }
}