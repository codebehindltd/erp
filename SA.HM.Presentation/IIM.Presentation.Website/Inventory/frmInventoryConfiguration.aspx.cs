using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmInventoryConfiguration : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadSupplierAccountsHeadIdDropdown();
                LoadInventoryConfiguration();

            }
            CheckObjectPermission();
        }
        //id name and hiddenfield name should be simillar for dynamic save.
        protected void btnSaveConfig_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            foreach (Control item in restuarantConfigPanel.Controls)
            {
                UpdateConfiguration(item, userInformationBO.UserInfoId);
            }
            LoadInventoryConfiguration();
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            //btnPercentageDiscountCategory.Visible = isSavePermission;
            //btnServiceBillCon.Visible = isSavePermission;
        }
        private void LoadInventoryConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            List<HMCommonSetupBO> setUpBOList = new List<HMCommonSetupBO>();
            setUpBOList = commonSetupDA.GetAllCommonConfigurationInfo();

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsPurchaseOrderApprovalEnable" && x.SetupName == "IsPurchaseOrderApprovalEnable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPurchaseOrderApprovalEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPurchaseOrderApprovalEnable.Checked = false;
                else
                    IsPurchaseOrderApprovalEnable.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsReceivedProductApprovalEnable" && x.SetupName == "IsReceivedProductApprovalEnable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsReceivedProductApprovalEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsReceivedProductApprovalEnable.Checked = false;
                else
                    IsReceivedProductApprovalEnable.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsItemVarianceApprovalEnable" && x.SetupName == "IsItemVarianceApprovalEnable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemVarianceApprovalEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsItemVarianceApprovalEnable.Checked = false;
                else
                    IsItemVarianceApprovalEnable.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsProductAdjustmentApprovalEnable" && x.SetupName == "IsProductAdjustmentApprovalEnable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsProductAdjustmentApprovalEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsProductAdjustmentApprovalEnable.Checked = false;
                else
                    IsProductAdjustmentApprovalEnable.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsProductOutApprovalEnable" && x.SetupName == "IsProductOutApprovalEnable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsProductOutApprovalEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsProductOutApprovalEnable.Checked = false;
                else
                    IsProductOutApprovalEnable.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsRequisitionCheckedByEnable" && x.SetupName == "IsRequisitionCheckedByEnable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRequisitionCheckedByEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRequisitionCheckedByEnable.Checked = false;
                else
                    IsRequisitionCheckedByEnable.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsPurchaseOrderCheckedByEnable" && x.SetupName == "IsPurchaseOrderCheckedByEnable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsPurchaseOrderCheckedByEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsPurchaseOrderCheckedByEnable.Checked = false;
                else
                    IsPurchaseOrderCheckedByEnable.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsInventoryIntegrateWithAccounts" && x.SetupName == "IsInventoryIntegrateWithAccounts").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsInventoryIntegrateWithAccounts.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsInventoryIntegrateWithAccounts.Checked = false;
                else
                    IsInventoryIntegrateWithAccounts.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsStockSummaryEnableInStockReport" && x.SetupName == "IsStockSummaryEnableInStockReport").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsStockSummaryEnableInStockReport.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsStockSummaryEnableInStockReport.Checked = false;
                else
                    IsStockSummaryEnableInStockReport.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsInventoryReportItemCostRescitionForNonAdminUsers" && x.SetupName == "IsInventoryReportItemCostRescitionForNonAdminUsers").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsInventoryReportItemCostRescitionForNonAdminUsers.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsInventoryReportItemCostRescitionForNonAdminUsers.Checked = false;
                else
                    IsInventoryReportItemCostRescitionForNonAdminUsers.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsRequisitionApprovalEnable" && x.SetupName == "IsRequisitionApprovalEnable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsRequisitionApprovalEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsRequisitionApprovalEnable.Checked = false;
                else
                    IsRequisitionApprovalEnable.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsInvCategoryCodeAutoGenerate" && x.SetupName == "IsInvCategoryCodeAutoGenerate").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsInvCategoryCodeAutoGenerate.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsInvCategoryCodeAutoGenerate.Checked = false;
                else
                    IsInvCategoryCodeAutoGenerate.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsInvItemCodeAutoGenerate" && x.SetupName == "IsInvItemCodeAutoGenerate").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsInvItemCodeAutoGenerate.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsInvItemCodeAutoGenerate.Checked = false;
                else
                    IsInvItemCodeAutoGenerate.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsSupplierCodeAutoGenerate" && x.SetupName == "IsSupplierCodeAutoGenerate").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsSupplierCodeAutoGenerate.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsSupplierCodeAutoGenerate.Checked = false;
                else
                    IsSupplierCodeAutoGenerate.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsItemOriginHide" && x.SetupName == "IsItemOriginHide").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemOriginHide.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsItemOriginHide.Checked = false;
                else
                    IsItemOriginHide.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsItemSerialFillWithAutoSearch" && x.SetupName == "IsItemSerialFillWithAutoSearch").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemSerialFillWithAutoSearch.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsItemSerialFillWithAutoSearch.Checked = false;
                else
                    IsItemSerialFillWithAutoSearch.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsInventoryIntegratationWithAccountsAutomated" && x.SetupName == "IsInventoryIntegratationWithAccountsAutomated").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsInventoryIntegratationWithAccountsAutomated.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsInventoryIntegratationWithAccountsAutomated.Checked = false;
                else
                    IsInventoryIntegratationWithAccountsAutomated.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsSupplierUserPanalEnable" && x.SetupName == "IsSupplierUserPanalEnable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsSupplierUserPanalEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsSupplierUserPanalEnable.Checked = false;
                else
                    IsSupplierUserPanalEnable.Checked = true;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsTransferProductReceiveDisable" && x.SetupName == "IsTransferProductReceiveDisable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsTransferProductReceiveDisable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsTransferProductReceiveDisable.Checked = false;
                else
                    IsTransferProductReceiveDisable.Checked = true;
            }
            setUpBO = setUpBOList.Where(x => x.TypeName == "IsItemDescriptionSuggestInPurchaseOrder" && x.SetupName == "IsItemDescriptionSuggestInPurchaseOrder").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemDescriptionSuggestInPurchaseOrder.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsItemDescriptionSuggestInPurchaseOrder.Checked = false;
                else
                    IsItemDescriptionSuggestInPurchaseOrder.Checked = true;
            }
            setUpBO = setUpBOList.Where(x => x.TypeName == "IsItemAttributeEnable" && x.SetupName == "IsItemAttributeEnable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemAttributeEnable.Value = setUpBO.SetupId.ToString();
                if (setUpBO.SetupValue == "0")
                    IsItemAttributeEnable.Checked = false;
                else
                    IsItemAttributeEnable.Checked = true;
            }
            setUpBO = setUpBOList.Where(x => x.TypeName == "InventoryTransactionSetup" && x.SetupName == "InventoryTransactionSetup").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfInventoryTransactionSetup.Value = setUpBO.SetupId.ToString();
                InventoryTransactionSetup.SelectedValue = setUpBO.SetupValue;
            }

            setUpBO = setUpBOList.Where(x => x.TypeName == "SupplierAccountsHeadId" && x.SetupName == "SupplierAccountsHeadId").FirstOrDefault();
            if (!string.IsNullOrEmpty(setUpBO.SetupValue))
            {
                hfSupplierAccountsHeadId.Value = setUpBO.SetupId.ToString();
                SupplierAccountsHeadId.SelectedValue = setUpBO.SetupValue;
            }

            btnSaveConfig.Visible = isUpdatePermission;
            btnSaveConfig.Text = "Update";
        }
        private void LoadSupplierAccountsHeadIdDropdown()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("9").Where(x => x.IsTransactionalHead == true).ToList();

            SupplierAccountsHeadId.DataSource = entityBOList;
            SupplierAccountsHeadId.DataTextField = "HeadWithCode";
            SupplierAccountsHeadId.DataValueField = "NodeId";
            SupplierAccountsHeadId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            SupplierAccountsHeadId.Items.Insert(0, itemBank);
        }
        private void UpdateConfiguration(Control control, int UserInfoId)
        {
            int tmpSetupId = 0;
            string hiddenField = "hf" + control.ID;

            Boolean status = false;
            Control hiddenControl = restuarantConfigPanel.FindControl(hiddenField);

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            if (control != null && hiddenControl != null)
            {
                commonSetupBO.SetupId = 0;
                if (control is TextBox)
                {
                    commonSetupBO.SetupValue = ((TextBox)control).Text;
                }
                else if (control is CheckBox)
                {
                    if (((CheckBox)control).Checked)
                        commonSetupBO.SetupValue = "1";
                    else
                        commonSetupBO.SetupValue = "0";
                }
                else if (control is DropDownList)
                {
                    commonSetupBO.SetupValue = ((DropDownList)control).SelectedValue;
                }
                if (!string.IsNullOrEmpty(((HiddenField)hiddenControl).Value))
                {
                    commonSetupBO.SetupId = Int32.Parse(((HiddenField)hiddenControl).Value);
                }

                commonSetupBO.CreatedBy = UserInfoId;
                commonSetupBO.LastModifiedBy = UserInfoId;
                commonSetupBO.SetupName = control.ID;
                commonSetupBO.TypeName = control.ID;

                if(commonSetupBO.TypeName == "IsSupplierCodeAutoGenerate")
                {
                    commonSetupBO.Description = "SC";
                }

                if (commonSetupBO.TypeName == "IsInvItemCodeAutoGenerate")
                {
                    commonSetupBO.Description = "IN";
                }

                try
                {
                    status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);

                    if (status)
                    {
                        if (commonSetupBO.SetupId > 0)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Inventory Configuration Updated Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.InventoryConfiguration.ToString(), commonSetupBO.SetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestuarantConfiguration));
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Inventory Configuration Saved Successfully.", AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.InventoryConfiguration.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestuarantConfiguration));
                            //this.btnServiceBillCon.Text = "Update";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}