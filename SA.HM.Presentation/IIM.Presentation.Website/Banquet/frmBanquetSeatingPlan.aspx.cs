using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Banquet;
using HotelManagement.Entity.Banquet;
using System.IO;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using System.Web.Script.Services;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Banquet
{
    public partial class frmBanquetSeatingPlan : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected int isNewAddButtonEnable = 1;

        //**************************** Handlers ****************************//


        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");            
            if (!IsPostBack)
            {
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomSeatingId.Value = seatingId.ToString();
                tempSeatingId.Value = seatingId.ToString();
                FileUpload();
                CheckPermission();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            flashUpload.QueryParameters = "BanquetSeatingId=" + Server.UrlEncode(RandomSeatingId.Value);

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFormValid())
                {
                    this.isNewAddButtonEnable = 2;
                    return;
                }
                int OwnerIdForDocuments = 0;
                BanquetSeatingPlanBO entityBO = new BanquetSeatingPlanBO();
                BanquetSeatingPlanDA entityDA = new BanquetSeatingPlanDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                entityBO.Name = this.txtName.Text;
                entityBO.Code = this.txtCode.Text;
                entityBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                entityBO.Description = txtDescription.Text;
                entityBO.RandomSeatingId = Int32.Parse(RandomSeatingId.Value);

                if (string.IsNullOrWhiteSpace(txtSeatingId.Value))
                {
                    if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Plan Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtName.Focus();
                        return;
                    }

                    if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Plan Code" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtCode.Focus();
                        return;
                    }

                    long tmpPKId = 0;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = entityDA.SaveBanquetSeatingPlanInfo(entityBO, out tmpPKId);
                    if (status)
                    {
                        OwnerIdForDocuments = Convert.ToInt32(tmpPKId) ;                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.BanquetSeatingPlan.ToString(), tmpPKId,
                        ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetSeatingPlan));
                        this.Cancel();
                    }
                }
                else
                {
                    entityBO.Id = Convert.ToInt64(txtSeatingId.Value);
                    entityBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = entityDA.UpdateBanquetSeatingPlanInfo(entityBO);
                    if (status)
                    {
                        OwnerIdForDocuments = Convert.ToInt32(entityBO.Id);                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetSeatingPlan.ToString(), entityBO.Id,
                        ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetSeatingPlan));
                        Random rd = new Random();
                        int ItemId = rd.Next(100000, 999999);
                        RandomSeatingId.Value = ItemId.ToString();
                        tempSeatingId.Value = ItemId.ToString();
                        this.Cancel();
                    }
                }
                // Update Uploaded Documents Information
                HMCommonDA hmCommonDA = new HMCommonDA();
                string docPath = Server.MapPath("") + "\\SeatingPlanImage\\";
                Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformation("BanquetSeatingPlan", docPath, OwnerIdForDocuments);
                this.SetTab("EntryTab");
            }
            catch(Exception ex){
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void gvSeatingPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            this.gvSeatingPlan.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvSeatingPlan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                //imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvSeatingPlan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _seatingPlanId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(_seatingPlanId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                try
                {
                    HMCommonDA hmCommonDA = new HMCommonDA();
                    Boolean status = hmCommonDA.DeleteInfoById("BanquetSeatingPlan", "Id", _seatingPlanId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.BanquetSeatingPlan.ToString(), _seatingPlanId,
                        ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetSeatingPlan));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    }
                    LoadGridView();
                    this.SetTab("SearchTab");
                }
                catch(Exception ex){
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    
                }
            }
        }

        //************************ User Defined Function ********************//
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
        }
        private void LoadGridView()
        {
            string Name = txtSName.Text;
            string Code = txtSCode.Text;
            Boolean ActiveStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;
            BanquetSeatingPlanDA da = new BanquetSeatingPlanDA();

            List<BanquetSeatingPlanBO> files = da.GetBanquetSeatingPlanBySearchCriteria(Name, Code, ActiveStat);
            this.gvSeatingPlan.DataSource = files;
            this.gvSeatingPlan.DataBind();
            this.SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.txtName.Text = string.Empty;
            this.txtCode.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.txtDescription.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtSeatingId.Value = string.Empty;
            this.txtName.Focus();
        }
        private bool IsFormValid()
        {
            bool status = true;
            if (string.IsNullOrWhiteSpace(this.txtName.Text))
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Plan Name.", AlertType.Warning);
                this.txtName.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtCode.Text))
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Plan Code.", AlertType.Warning);
                this.txtCode.Focus();
                status = false;
            }
            return status;
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
        public void FillForm(long EditId)
        {
            BanquetSeatingPlanBO entityBO = new BanquetSeatingPlanBO();
            BanquetSeatingPlanDA entityDA = new BanquetSeatingPlanDA();
            entityBO = entityDA.GetBanquetSeatingPlanInfoById(EditId);
            txtName.Text = entityBO.Name;
            txtCode.Text = entityBO.Code;
            ddlActiveStat.SelectedValue = (entityBO.ActiveStat == true ? 0 : 1).ToString();
            txtSeatingId.Value = entityBO.Id.ToString();
            RandomSeatingId.Value = entityBO.Id.ToString();
            FileUpload();
            txtDescription.Text = entityBO.Description;
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "BanquetSeatingPlan";
            string pkFieldName = "SeatingId";
            string pkFieldValue = this.txtSeatingId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UploadData()
        {
            //get reference to posted file and do what you want with this file
            HttpPostedFile postedfile = this.Context.Request.Files.Get(0) as HttpPostedFile;
            return "";

        }
        [WebMethod]
        public static string GetUploadedImageByWebMethod(int OwnerId, string docType)
        {
            string strTable = "";
            DocumentsDA docDA = new DocumentsDA();
            var docList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId(docType, OwnerId);
            if (docList.Count > 0)
            {
                var Image = docList[0];

                strTable += "<img src='" + Image.Path + Image.Name + "'  alt='No Image Selected' border='0' />";
            }
            return strTable;
        }
    }
}