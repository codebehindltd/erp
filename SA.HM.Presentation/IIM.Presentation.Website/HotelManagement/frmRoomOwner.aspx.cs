using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using System.Collections;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmRoomOwner : System.Web.UI.Page
    {
        ArrayList arrayDelete;
        protected int _RoomOwnerId;
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        HMUtility hmUtility = new HMUtility();
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        private Boolean isViewPermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            this.AddEditODeleteDetail();
            if (!IsPostBack)
            {
                this.LoadRoomNumber();

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            RoomOwnerDA da = new RoomOwnerDA();
            RoomOwnerBO bo = new RoomOwnerBO();

            bo.FirstName = this.txtFirstName.Text;
            bo.LastName = this.txtLastName.Text;
            bo.Description = this.txtDescription.Text;
            bo.Address = this.txtAddress.Text;
            bo.CityName = this.txtCityName.Text;
            bo.ZipCode = this.txtZipCode.Text;
            bo.StateName = this.txtStateName.Text;

            bo.Country = this.txtCountry.Text;
            bo.Phone = this.txtPhone.Text;
            bo.Fax = this.txtFax.Text;
            bo.Email = this.txtEmail.Text;


            if (this.btnSave.Text.Equals("Save"))
            {
                int tmpOwnerId = 0;
                bo.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = da.SaveRoomOwnerInfo(bo, out tmpOwnerId, Session["OwnerDetailList"] as List<OwnerDetailBO>);
                if (status)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomOwner.ToString(), tmpOwnerId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomOwner));
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomOwnerDetail.ToString(), tmpOwnerId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomOwnerDetail)+".EntityId is OwnerId");
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull.";

                    this.Cancel();
                }
            }
            else
            {
                bo.OwnerId = Convert.ToInt32(Session["_RoomOwnerId"]);
                bo.LastModifiedBy = userInformationBO.UserInfoId;
                List<OwnerDetailBO> ownerDetailsList =Session["OwnerDetailList"] as List<OwnerDetailBO>;
                ArrayList deleteList= Session["arrayDelete"] as ArrayList;
                Boolean status = da.UpdateRoomOwnerInfo(bo, ownerDetailsList, deleteList);
                if (status)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomOwner.ToString(), bo.OwnerId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomOwner));
                    if(ownerDetailsList.Count > 0)
                    {
                        foreach (var item in ownerDetailsList)
                        {
                            if(item.OwnerId > 0)
                                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomOwnerDetail.ToString(), item.OwnerId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomOwnerDetail) + ".EntityId is OwnerId");
                            else
                                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomOwnerDetail.ToString(), item.DetailId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomOwnerDetail));
                        }
                    }
                    if(deleteList.Count > 0)
                    {
                        foreach (int item in deleteList)
                        {
                            hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RoomOwnerDetail.ToString(),item, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomOwnerDetail));
                        }
                    }    
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Update Operation Successfull.";

                    this.Cancel();
                }
            }
        }
        protected void btnOwnerDetails_Click(object sender, EventArgs e)
        {
            int dynamicDetailId = 0;
            List<OwnerDetailBO> detailListBO = Session["OwnerDetailList"] == null ? new List<OwnerDetailBO>() : Session["OwnerDetailList"] as List<OwnerDetailBO>;

            if (!string.IsNullOrWhiteSpace(lblHiddenOwnerDetailtId.Text))
                dynamicDetailId = Convert.ToInt32(lblHiddenOwnerDetailtId.Text);

            OwnerDetailBO detailBO = dynamicDetailId == 0 ? new OwnerDetailBO() : detailListBO.Where(x => x.DetailId == dynamicDetailId).FirstOrDefault();
            if (detailListBO.Contains(detailBO))
                detailListBO.Remove(detailBO);

            detailBO.RoomId = Convert.ToInt32(this.ddlRoomId.SelectedValue);
            detailBO.RoomNumber = this.ddlRoomId.SelectedItem.Text;
            detailBO.CommissionValue = Convert.ToDecimal(this.txtCommissionValue.Text);

            detailBO.DetailId = dynamicDetailId == 0 ? detailListBO.Count + 1 : dynamicDetailId;

            detailListBO.Add(detailBO);

            Session["OwnerDetailList"] = detailListBO;

            this.gvRoomOwnerDtail.DataSource = Session["OwnerDetailList"] as List<OwnerDetailBO>;
            this.gvRoomOwnerDtail.DataBind();

            this.ClearDetailPart();
        }
        protected void gvRoomOwnerDtail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _ownerDetailId;

            if (e.CommandName == "CmdEdit")
            {
                _ownerDetailId = Convert.ToInt32(e.CommandArgument.ToString());
                lblHiddenOwnerDetailtId.Text = _ownerDetailId.ToString();
                var ownerDetailBO = (List<OwnerDetailBO>)Session["OwnerDetailList"];
                var ownerDetail = ownerDetailBO.Where(x => x.DetailId == _ownerDetailId).FirstOrDefault();
                if (ownerDetail != null && ownerDetail.DetailId > 0)
                {
                    this.ddlRoomId.SelectedValue = ownerDetail.RoomId.ToString();
                    this.txtCommissionValue.Text = ownerDetail.CommissionValue.ToString();

                    btnOwnerDetails.Text = "Edit";
                }
                else
                {
                    btnOwnerDetails.Text = "Add";
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _ownerDetailId = Convert.ToInt32(e.CommandArgument.ToString());
                var ownerDetailBO = (List<OwnerDetailBO>)Session["OwnerDetailList"];
                var ownerDetail = ownerDetailBO.Where(x => x.DetailId == _ownerDetailId).FirstOrDefault();
                ownerDetailBO.Remove(ownerDetail);
                Session["OwnerDetailList"] = ownerDetailBO;
                arrayDelete.Add(_ownerDetailId);
                this.gvRoomOwnerDtail.DataSource = Session["OwnerDetailList"] as List<OwnerDetailBO>;
                this.gvRoomOwnerDtail.DataBind();
            }
        }
        protected void gvRoomOwner_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdEdit")
            {
                this._RoomOwnerId = Convert.ToInt32(e.CommandArgument.ToString());
                Session["_RoomOwnerId"] = this._RoomOwnerId;
                this.isNewAddButtonEnable = 2;
                this.FillForm(this._RoomOwnerId);
            }
            else if (e.CommandName == "CmdDelete")
            {
                try
                {
                    this._RoomOwnerId = Convert.ToInt32(e.CommandArgument.ToString());
                    Session["_RoomOwnerId"] = this._RoomOwnerId;
                    this.DeleteData(this._RoomOwnerId);
                    this.Cancel();
                    this.LoadGridView();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();

        }
        //************************ User Defined Function ********************//
        private void LoadRoomNumber()
        {
            RoomNumberDA entityDA = new RoomNumberDA();
            this.ddlRoomId.DataSource = entityDA.GetRoomNumberInfo();
            this.ddlRoomId.DataTextField = "RoomNumber";
            this.ddlRoomId.DataValueField = "RoomId";
            this.ddlRoomId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRoomId.Items.Insert(0, item);
        }
        private bool IsFormValid()
        {
            bool status = true;

            if (string.IsNullOrWhiteSpace(this.txtFirstName.Text))
            {
                this.isMessageBoxEnable = 1;
                this.isNewAddButtonEnable = 2;
                lblMessage.Text = "Please provide First Name.";
                this.txtFirstName.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtPhone.Text))
            {
                this.isMessageBoxEnable = 1;
                this.isNewAddButtonEnable = 2;
                lblMessage.Text = "Please provide Phone Number.";
                this.txtPhone.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtEmail.Text))
            {
                this.isMessageBoxEnable = 1;
                this.isNewAddButtonEnable = 2;
                lblMessage.Text = "Please provide Email ID.";
                this.txtEmail.Focus();
                status = false;
            }
            else if (Session["OwnerDetailList"] == null)
            {
                this.isMessageBoxEnable = 1;
                this.isNewAddButtonEnable = 2;
                lblMessage.Text = "Please add at least one Details.";
                status = false;
            }
            return status;
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmRoomNumber.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadGridView()
        {
            string FirstName = this.txtSFirstName.Text;
            string LastName = this.txtSLastName.Text;
            string Email = this.txtSEmail.Text;


            this.CheckObjectPermission();
            RoomOwnerDA da = new RoomOwnerDA();
            List<RoomOwnerBO> files = da.GetRoomOwnerInfoBySearchCriteria(FirstName, LastName, Email);

            this.gvRoomOwner.DataSource = files;
            this.gvRoomOwner.DataBind();
            this.SetTab("SearchTab");
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
        private void Cancel()
        {
            this.txtFirstName.Text = string.Empty;
            this.txtLastName.Text = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.txtAddress.Text = string.Empty;
            this.txtCityName.Text = string.Empty;
            this.txtZipCode.Text = string.Empty;
            this.txtStateName.Text = string.Empty;
            this.txtCountry.Text = string.Empty;
            this.txtPhone.Text = string.Empty;
            this.txtFax.Text = string.Empty;
            this.txtEmail.Text = string.Empty;
            this.btnSave.Text = "Save";
            Session["OwnerDetailList"] = null;
            this.gvRoomOwnerDtail.DataSource = Session["OwnerDetailList"] as List<OwnerDetailBO>;
            this.gvRoomOwnerDtail.DataBind();
            this.ClearDetailPart();
        }
        private void AddEditODeleteDetail()
        {
            //Delete------------
            if (Session["arrayDelete"] == null)
            {
                arrayDelete = new ArrayList();
                Session.Add("arrayDelete", arrayDelete);
            }
            else
                arrayDelete = Session["arrayDelete"] as ArrayList;
        }
        private void ClearDetailPart()
        {
            this.btnOwnerDetails.Text = "Add";
            this.ddlRoomId.SelectedValue = "0";
            this.txtCommissionValue.Text = string.Empty;
            this.lblHiddenOwnerDetailtId.Text = string.Empty;
            //this._RoomReservationId = 0;
        }
        private void FillForm(int EditId)
        {
            lblMessage.Text = "";

            //Master Information------------------------
            RoomOwnerBO roomOwnerBO = new RoomOwnerBO();
            RoomOwnerDA roomOwnerDA = new RoomOwnerDA();

            roomOwnerBO = roomOwnerDA.GetRoomOwnerInfoById(EditId);
            Session["_RoomOwnerId"] = roomOwnerBO.OwnerId;
            this.txtFirstName.Text = roomOwnerBO.FirstName;
            this.txtLastName.Text = roomOwnerBO.LastName;
            this.txtDescription.Text = roomOwnerBO.Description;
            this.txtAddress.Text = roomOwnerBO.Address;
            this.txtCityName.Text = roomOwnerBO.CityName;
            this.txtZipCode.Text = roomOwnerBO.ZipCode;
            this.txtStateName.Text = roomOwnerBO.StateName;
            this.txtCountry.Text = roomOwnerBO.Country;
            this.txtPhone.Text = roomOwnerBO.Phone;
            this.txtFax.Text = roomOwnerBO.Fax;
            this.txtEmail.Text = roomOwnerBO.Email;
            this.btnSave.Text = "Update";
            //this.btnNewReservation.Visible = false;

            //Detail Information------------------------
            List<OwnerDetailBO> ownerDetailListBO = new List<OwnerDetailBO>();
            OwnerDetailDA ownerDetailDA = new OwnerDetailDA();

            ownerDetailListBO = ownerDetailDA.GetOwnerDetailByOwnerId(EditId);

            Session["OwnerDetailList"] = ownerDetailListBO;

            this.gvRoomOwnerDtail.DataSource = Session["OwnerDetailList"] as List<OwnerDetailBO>;
            this.gvRoomOwnerDtail.DataBind();

            this.SetTab("EntryTab");
        }
        private void DeleteData(int pkId)
        {
            RoomOwnerDA roomOwnerDA = new RoomOwnerDA();
            Boolean statusApproved = roomOwnerDA.DeleteOwnerDetailInfoByOwnerId(pkId);
            if (statusApproved)
            {
                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),EntityTypeEnum.EntityType.RoomOwnerDetail.ToString(),pkId,ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomOwnerDetail)+".EntityId is OwnerId");
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Delete Operation Successfull.";
                this.LoadGridView();
                this.Cancel();
            }
        }
    }
}