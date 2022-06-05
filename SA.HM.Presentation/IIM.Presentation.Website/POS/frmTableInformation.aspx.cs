using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmTableInformation : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            CheckObjectPermission();
        }
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    this.isNewAddButtonEnable = 2;
                    return;
                }
                RestaurantTableBO tableBO = new RestaurantTableBO();
                RestaurantTableDA tableDA = new RestaurantTableDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                tableBO.Remarks = this.txtRemarks.Text.Trim().ToString();
                tableBO.TableNumber = this.txtTableNumber.Text.Trim().ToString();
                tableBO.TableCapacity = this.txtTableCapacity.Text;

                if (string.IsNullOrWhiteSpace(txtTableId.Value))
                {
                    bool availability = tableDA.GetRestaurantTableInfoByName(txtTableNumber.Text);

                    if (availability == true)
                    {
                        int tmpTableId = 0;
                        tableBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = tableDA.SaveRestaurantTableInfo(tableBO, out tmpTableId);
                        if (status)
                        {
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RestaurantTable.ToString(), tmpTableId, ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantTable));
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            this.LoadGridView();
                            this.Cancel();
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                        }
                    }
                    else
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, "You Can't Save. Table Number " + txtTableNumber.Text + "  Already Inserted.", AlertType.Success);
                        this.LoadGridView();
                        this.Cancel();
                    }
                }
                else
                {
                    tableBO.TableId = Convert.ToInt32(txtTableId.Value);
                    tableBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = tableDA.UpdateRestaurantTableInfo(tableBO);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RestaurantTable.ToString(), tableBO.TableId, ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantTable));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        this.Cancel();
                        txtTableId.Value = "";
                        this.LoadGridView();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }

                this.SetTab("EntryTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void gvTableNumber_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvTableNumber.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvTableNumber_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                // imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                // imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvTableNumber_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int tableId = Convert.ToInt32(e.CommandArgument.ToString());
                if (e.CommandName == "CmdEdit")
                {
                    FillForm(tableId);
                    this.btnSave.Visible = isUpdatePermission;
                    this.btnSave.Text = "Update";
                    this.SetTab("EntryTab");
                }
                else if (e.CommandName == "CmdDelete")
                {
                    RestaurantTableDA tableDA = new RestaurantTableDA();
                    Boolean status = tableDA.DeleteResturantTabeInfo(tableId);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RestaurantTable.ToString(), tableId, ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantTable));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    LoadGridView();
                    this.SetTab("SearchTab");
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        public void LoadGridView()
        {
            String TableNumber = this.txtSTableNumber.Text;

            this.CheckObjectPermission();
            RestaurantTableDA tableDA = new RestaurantTableDA();
            List<RestaurantTableBO> files = tableDA.GetRestaurantTableInfoBySearchCriteria(TableNumber);
            this.gvTableNumber.DataSource = files;
            this.gvTableNumber.DataBind();
            this.SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.txtRemarks.Text = string.Empty;
            this.txtTableNumber.Text = string.Empty;
            txtTableCapacity.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtTableNumber.Focus();
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (string.IsNullOrWhiteSpace(this.txtTableNumber.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide Table Number.", AlertType.Warning);
                txtTableNumber.Focus();
                flag = false;
            }

            return flag;
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        public void FillForm(int EditId)
        {
            RestaurantTableBO tableBO = new RestaurantTableBO();
            RestaurantTableDA tableDA = new RestaurantTableDA();
            tableBO = tableDA.GetRestaurantTableInfoById(0, "RestaurantTable", EditId);
            txtRemarks.Text = tableBO.Remarks;
            txtTableId.Value = tableBO.TableId.ToString();
            txtTableNumber.Text = tableBO.TableNumber.ToString();
            txtTableCapacity.Text = tableBO.TableCapacity.ToString();
        }
    }
}