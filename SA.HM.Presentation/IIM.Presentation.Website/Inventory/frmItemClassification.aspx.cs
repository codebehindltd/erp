using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.GeneralLedger;
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

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmItemClassification : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        HMCommonDA hmCommonDA = new HMCommonDA();
        NodeMatrixDA entityDA = new NodeMatrixDA();
        private List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.CheckObjectPermission();
                LoadCostCenterInfoGridView();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            InvItemClassificationBO invItemClassificationBO = new InvItemClassificationBO();
            InvItemClassificationCostCenterMappingDA invItemClassificationCostCenterMappingDA = new InvItemClassificationCostCenterMappingDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            invItemClassificationBO.ClassificationId = !string.IsNullOrWhiteSpace(hfClassificationId.Value) ? Convert.ToInt64(hfClassificationId.Value) : 0;
            invItemClassificationBO.ClassificationName = txtClassificationName.Text;
            invItemClassificationBO.IsActive = ddlActiveStat.SelectedIndex == 0 ? true : false;
            invItemClassificationBO.CreatedBy =Convert.ToInt64(userInformationBO.UserInfoId);

            int gridRow = gvCostCenterWithAccountHeadMap.Rows.Count;
            List<InvItemClassificationCostCenterMappingBO> mappingBo = new List<InvItemClassificationCostCenterMappingBO>();
            DropDownList tempAccountHead;
            Label TempCostCenter;
            Label tempMappingId;
            CheckBox tempCheckBox;
            Boolean isDeleteOnEdit = false;
            List<Int64> deleteMappingIdList = new List<Int64>();
            List<InvItemClassificationCostCenterMappingBO> addMappingIdList=new List<InvItemClassificationCostCenterMappingBO>();
            for (int i = 0; i < gridRow; i++)
            {
                tempCheckBox = (CheckBox)gvCostCenterWithAccountHeadMap.Rows[i].FindControl("chkIsSavePermission");
                tempAccountHead = (DropDownList)gvCostCenterWithAccountHeadMap.Rows[i].FindControl("ddlAccountHead");
                TempCostCenter = (Label)gvCostCenterWithAccountHeadMap.Rows[i].FindControl("hfCostCenterId");
                tempMappingId = (Label)gvCostCenterWithAccountHeadMap.Rows[i].FindControl("hfMappingId");
                if (tempCheckBox.Checked==true)
                {
                    InvItemClassificationCostCenterMappingBO item = new InvItemClassificationCostCenterMappingBO()
                    {
                        MappingId = !string.IsNullOrWhiteSpace(tempMappingId.Text) ? Convert.ToInt64(tempMappingId.Text) : 0,
                        CostCenterId = Convert.ToInt64(TempCostCenter.Text),
                        ClassificationId = Convert.ToInt64(invItemClassificationBO.ClassificationId),
                        AccountHeadId = Convert.ToInt64(tempAccountHead.SelectedValue),
                    };
                    if (tempMappingId.Text == "0")
                    {
                        item.CreatedBy = Convert.ToInt64(userInformationBO.UserInfoId);
                    }
                        
                    else
                        item.LastModifiedBy = Convert.ToInt64(userInformationBO.UserInfoId);
                    mappingBo.Add(item);
                }
                else
                {
                    if(!string.IsNullOrWhiteSpace(tempMappingId.Text)&& tempMappingId.Text!="0")
                    {
                        deleteMappingIdList.Add(Convert.ToInt64(tempMappingId.Text));
                    }
                    //if(tempMappingId.Text!="0")
                        
                }
                
            }

            if (string.IsNullOrWhiteSpace(hfClassificationId.Value))
            {
                Int64 classificationId = 0;
                Int64 mappingId = 0;
                Boolean isClassificationNameExist = hmCommonDA.DuplicateDataCountDynamicaly("InvItemClassification", "Name", invItemClassificationBO.ClassificationName) > 0;
                if (isClassificationNameExist)
                {
                    CommonHelper.AlertInfo(innboardMessage, string.Format("Yor Entered {0} {1} is already exist", lblClassificationName.Text, invItemClassificationBO.ClassificationName), AlertType.Warning);
                    this.txtClassificationName.Focus();
                    return;
                }

                Boolean status = invItemClassificationCostCenterMappingDA.SaveInvClassificationName(invItemClassificationBO, mappingBo, out classificationId, out mappingId);
                mappingBo = invItemClassificationCostCenterMappingDA.GetInvItemClassificationCostCenterMappingInfoByClassificationId(classificationId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.InvItemClassification.ToString(), classificationId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItemClassification));
                    foreach (var item in mappingBo)
                    {
                        logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.InvItemClassificationMappingCostCenterWithAccountHead.ToString(), item.MappingId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItemClassificationMappingCostCenterWithAccountHead));
                    }
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    this.Cancel();
                }
                else
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

            }
            else
            {
                List<Int64> mappingId = new List<Int64>();
                invItemClassificationBO.LastModifiedBy =Convert.ToInt64(userInformationBO.UserInfoId);

                Boolean isClassificationNameExist = hmCommonDA.DuplicateCheckDynamicaly("InvItemClassification", "Name", invItemClassificationBO.ClassificationName, 1, "Id", invItemClassificationBO.ClassificationId.ToString()) > 0;
                if (isClassificationNameExist)
                {
                    CommonHelper.AlertInfo(innboardMessage, string.Format("Yor Entered {0} {1} is already exist", lblClassificationName.Text, invItemClassificationBO.ClassificationName), AlertType.Warning);
                    txtClassificationName.Focus();
                    //this.Cancel();
                    return;
                }
                if (deleteMappingIdList.Count > 0)
                    isDeleteOnEdit = true;
                List<Int64> newCostCenterList = new List<Int64>();
                newCostCenterList = mappingBo.Where(m => m.MappingId == 0).Select(m => m.CostCenterId).ToList();
                addMappingIdList = mappingBo.Where(m => m.MappingId == 0).ToList();
                Boolean status = invItemClassificationCostCenterMappingDA.UpdateInvClassificationName(invItemClassificationBO, mappingBo, isDeleteOnEdit);
                addMappingIdList = invItemClassificationCostCenterMappingDA.GetInvItemClassificationCostCenterMappingInfoByClassificationId(invItemClassificationBO.ClassificationId).Where(m => newCostCenterList.Contains(m.CostCenterId)).ToList();
                mappingBo = mappingBo.Where(m => m.MappingId != 0).ToList();
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.InvItemClassification.ToString(), invItemClassificationBO.ClassificationId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItemClassification));
                    foreach (var item in mappingBo)
                    {
                        logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.InvItemClassificationMappingCostCenterWithAccountHead.ToString(), item.MappingId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItemClassificationMappingCostCenterWithAccountHead));
                    }
                    foreach (var item in addMappingIdList)
                    {
                        logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.InvItemClassificationMappingCostCenterWithAccountHead.ToString(), item.MappingId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItemClassificationMappingCostCenterWithAccountHead));
                    }
                    if (deleteMappingIdList.Count>0)
                    {
                        foreach (var item in deleteMappingIdList)
                        {
                            logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.InvItemClassificationMappingCostCenterWithAccountHead.ToString(), item, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItemClassificationMappingCostCenterWithAccountHead));
                        }
                    }
                    

                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    Cancel();
                }
                else
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
            SetTab("EntryTab");
        }
        protected void gvcostCenterWithAccountHeadMap_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {

                DropDownList ddlAccountHead = e.Row.FindControl("ddlAccountHead") as DropDownList;

                ddlAccountHead.DataSource = entityBOList;
                ddlAccountHead.DataTextField = "HeadWithCode";
                ddlAccountHead.DataValueField = "NodeId";
                ddlAccountHead.DataBind();
                ddlAccountHead.Items.Insert(0, new ListItem() { Value = "0", Text = hmUtility.GetDropDownFirstValue() });
            }
        }
        protected void btnEditInServer_Click(object sender, EventArgs e)
        {
            Int64 editClassificationId =Convert.ToInt64(hfEditedItemId.Value);
            InvItemClassificationCostCenterMappingDA invItemClassificationCostCenterMap = new InvItemClassificationCostCenterMappingDA();
            InvItemClassificationBO classificationBO = invItemClassificationCostCenterMap.GetItemClassificationInfoById(editClassificationId);
            List<InvItemClassificationCostCenterMappingBO> mappingBO = invItemClassificationCostCenterMap.GetInvItemClassificationCostCenterMappingInfoByClassificationId(editClassificationId);
            FillEditedForm(classificationBO, mappingBO);
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            txtClassificationName.Text = string.Empty;
            ddlActiveStat.SelectedIndex = 0;
            btnSave.Text = "Save";
            hfClassificationId.Value = string.Empty;
            int rows = gvCostCenterWithAccountHeadMap.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                DropDownList ddl= gvCostCenterWithAccountHeadMap.Rows[i].FindControl("ddlAccountHead") as DropDownList;
                CheckBox chkbox = gvCostCenterWithAccountHeadMap.Rows[i].FindControl("chkIsSavePermission") as CheckBox;
                ddl.SelectedValue = "0";
                chkbox.Checked = false;
            }
            txtClassificationName.Focus();
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
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;

            if (isUpdatePermission)
            {
                IsClassificationUpdatePermission.Value = "1";
            }
            else
            {
                IsClassificationUpdatePermission.Value = "0";
            }

            if (isDeletePermission)
            {
                IsClassificationDeletePermission.Value = "1";
            }
            else
            {
                IsClassificationDeletePermission.Value = "0";
            }
        }
        private void LoadCostCenterInfoGridView()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<CostCentreTabBO> costCenterList = costCentreTabDA.GetAllCostCentreTabInfo();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("3").Where(m => m.IsTransactionalHead == true).ToList();
            gvCostCenterWithAccountHeadMap.DataSource= costCenterList;
            gvCostCenterWithAccountHeadMap.DataBind();    
        }
        private void FillEditedForm( InvItemClassificationBO classificationBO,List<InvItemClassificationCostCenterMappingBO> mappingBO )
        {
            btnSave.Visible = isUpdatePermission;
            btnSave.Text = "Update";
            ddlActiveStat.SelectedValue = classificationBO.IsActive == true ? "0" : "1";
            txtClassificationName.Text = classificationBO.ClassificationName;
            hfClassificationId.Value = Convert.ToString(classificationBO.ClassificationId);
            LoadCostCenterInfoGridView();
            int gridRow = gvCostCenterWithAccountHeadMap.Rows.Count;
            for (int i = 0; i < gridRow; i++)
            {
                DropDownList tempAccountHeadDD =(DropDownList)gvCostCenterWithAccountHeadMap.Rows[i].FindControl("ddlAccountHead");
                Label tempHfClassificationId=(Label)gvCostCenterWithAccountHeadMap.Rows[i].FindControl("hfClassificationId");
                Label tempMappingId = (Label)gvCostCenterWithAccountHeadMap.Rows[i].FindControl("hfMappingId");
                CheckBox tempCheckBox = (CheckBox)gvCostCenterWithAccountHeadMap.Rows[i].FindControl("chkIsSavePermission");
                Label tempCostCenterId = (Label)gvCostCenterWithAccountHeadMap.Rows[i].FindControl("hfCostCenterId");
                var mappingEditedItem = mappingBO.Where(m => m.CostCenterId == Convert.ToInt64(tempCostCenterId.Text)).FirstOrDefault();
                if (mappingEditedItem!=null)
                {
                    tempCheckBox.Checked = true;
                    tempMappingId.Text = Convert.ToString(mappingEditedItem.MappingId);
                    tempHfClassificationId.Text = Convert.ToString(mappingEditedItem.ClassificationId);
                    tempAccountHeadDD.SelectedValue = Convert.ToString(mappingEditedItem.AccountHeadId);
                }
                else
                {
                    tempCheckBox.Checked = false;
                    tempHfClassificationId.Text = "0";
                    tempMappingId.Text = "0";
                    tempAccountHeadDD.SelectedValue = "0";
                }
            }

        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ReturnInfo DeleteData(int classificationId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            
            try
            {
                InvItemClassificationCostCenterMappingDA mappingDA = new InvItemClassificationCostCenterMappingDA();
                List<InvItemClassificationCostCenterMappingBO> entityList = mappingDA.GetInvItemClassificationCostCenterMappingInfoByClassificationId(classificationId);
                Boolean status = mappingDA.DeleteInvClassification((Int64)classificationId);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.InvItemClassification.ToString(), classificationId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItemClassification));
                    foreach (var item in entityList)
                    {
                        logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.InvItemClassificationMappingCostCenterWithAccountHead.ToString(),item.MappingId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItemClassificationMappingCostCenterWithAccountHead));
                    }
                    
                    
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            
            return rtninf;
        }
        [WebMethod]
        public static InvItemClassificationBO LoadClassificationDetailInformation(Int64 classificationId)
        {
            InvItemClassificationCostCenterMappingDA invItemClassificationCostCenterMappingDA = new InvItemClassificationCostCenterMappingDA();
            return invItemClassificationCostCenterMappingDA.GetItemClassificationInfoById(classificationId);
        }
        [WebMethod]
        public static GridViewDataNPaging<InvItemClassificationBO, GridPaging> SearchClassificationAndLoadGridInformation(string classificationName, Boolean IsActive, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<InvItemClassificationBO, GridPaging> myGridData = new GridViewDataNPaging<InvItemClassificationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            InvItemClassificationCostCenterMappingDA invItemClassificationCostCenterMappingDA = new InvItemClassificationCostCenterMappingDA();
            List<InvItemClassificationBO> itemClassificationInfoList = new List<InvItemClassificationBO>();
            itemClassificationInfoList = invItemClassificationCostCenterMappingDA.GetItemClassificationInformationBySearchCriteriaForPaging(classificationName, IsActive, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            myGridData.GridPagingProcessing(itemClassificationInfoList, totalRecords);

            return myGridData;
        }  
    }
}