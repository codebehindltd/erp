using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.Membership;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Membership;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class OpeningBalance : BasePage
    {
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPermission();
                LoadTransactionType();
                LoadCompany();
                LoadCommonDropDownHiddenField();
                LoadStore();
            }
        }
        private void CheckPermission()
        {
            dvSave.Visible = isSavePermission;
        }
        private void LoadTransactionType()
        {
            Array itemValues = Enum.GetValues(typeof(ConstantHelper.GLTransactionType));
            List<ListItem> fields = new List<ListItem>();

            foreach (ConstantHelper.GLTransactionType field in itemValues)
            {
                fields.Add(new ListItem(hmUtility.GetEnumDescription(field), field.ToString()));
            }

            ddlTransactionType.DataSource = fields;
            ddlTransactionType.DataTextField = "Text";
            ddlTransactionType.DataValueField = "Value";
            ddlTransactionType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTransactionType.Items.Insert(0, item);
        }
        private void LoadCompany()
        {
            GLCompanyDA companyDA = new GLCompanyDA();
            List<GLCompanyBO> List = new List<GLCompanyBO>();
            List = companyDA.GetAllGLCompanyInfo();

            ddlCompany.DataSource = List;
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlCompany.Items.Insert(0, itemNodeId);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadStore()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfo();
            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId).Where(x => x.CostCenterType == "Inventory").ToList();
            ddlStore.DataSource = costCentreTabBOList;
            ddlStore.DataTextField = "CostCenter";
            ddlStore.DataValueField = "CostCenterId";
            ddlStore.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            if (costCentreTabBOList.Count > 1)
            {
                ddlStore.Items.Insert(0, item);
            }
        }
        [WebMethod]
        public static List<GLProjectBO> GetGLProjectByGLCompanyId(int companyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(companyId, userInformationBO.UserGroupId);

            return projectList;
        }

        [WebMethod]
        public static List<GLFiscalYearBO> PopulateFiscalYear(int projectId)
        {
            ArrayList list = new ArrayList();
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA entityDA = new GLFiscalYearDA();
            fiscalYearList = entityDA.GetFiscalYearListByProjectId(Convert.ToInt32(projectId));

            return fiscalYearList;
        }

        [WebMethod]
        public static List<GLOpeningBalanceTransactionNodeAutoComplete> AutoCompleteTransactionNode(string transactionType, string searchText, string inventorySearchType, int storeId)
        {
            List<GLOpeningBalanceTransactionNodeAutoComplete> nodeList = new List<GLOpeningBalanceTransactionNodeAutoComplete>();

            if (transactionType == ConstantHelper.GLTransactionType.Accounts.ToString())
            {
                List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
                NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

                nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByNameAndTransactionFlag(searchText, false);
                nodeList = (from n in nodeMatrixBOList
                            select new GLOpeningBalanceTransactionNodeAutoComplete()
                            {
                                TransactionNodeId = n.NodeId,
                                NodeName = n.NodeHead,
                                Lvl = n.Lvl,
                                Hierarchy = n.Hierarchy
                            }).ToList();
            }
            else if (transactionType == ConstantHelper.GLTransactionType.Company.ToString())
            {
                List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
                GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

                companyList = guestCompanyDA.GetCompanyInfoByNameForAutoComplete(searchText);

                nodeList = (from n in companyList
                            select new GLOpeningBalanceTransactionNodeAutoComplete()
                            {

                                TransactionNodeId = n.CompanyId,
                                NodeName = n.CompanyNameWithCode,
                                Lvl = (int)n.Lvl,
                                Hierarchy = n.Hierarchy
                            }).ToList();
            }
            else if (transactionType == ConstantHelper.GLTransactionType.Supplier.ToString())
            {
                List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();
                PMSupplierDA entityDA = new PMSupplierDA();

                supplierBOList = entityDA.GetPMSupplierInfoForAutoSearch(searchText);

                nodeList = (from n in supplierBOList
                            select new GLOpeningBalanceTransactionNodeAutoComplete()
                            {
                                TransactionNodeId = n.SupplierId,
                                NodeName = n.NameWithCode
                            }).ToList();
            }
            else if (transactionType == ConstantHelper.GLTransactionType.Employee.ToString())
            {
                List<EmployeeBO> empList = new List<EmployeeBO>();
                EmployeeDA entityDA = new EmployeeDA();

                empList = entityDA.GetEmpInformationForAutoSearch(searchText);

                nodeList = (from n in empList
                            select new GLOpeningBalanceTransactionNodeAutoComplete()
                            {

                                TransactionNodeId = n.EmpId,
                                NodeName = n.EmpCode + "(" + n.DisplayName + ")"
                            }).ToList();
            }
            else if (transactionType == ConstantHelper.GLTransactionType.Member.ToString())
            {
                MemMemberBasicDA memberBasicDA = new MemMemberBasicDA();
                List<MemMemberBasicsBO> memberList = new List<MemMemberBasicsBO>();

                memberList = memberBasicDA.GetMemberInfoForAutoSearch(searchText);

                nodeList = (from n in memberList
                            select new GLOpeningBalanceTransactionNodeAutoComplete()
                            {
                                TransactionNodeId = n.MemberId,
                                NodeName = n.NameWithMembershipNumber
                            }).ToList();
            }
            else if (transactionType == ConstantHelper.GLTransactionType.Inventory.ToString())
            {
                if (inventorySearchType == "Item")
                {
                    List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
                    InvItemDA itemDa = new InvItemDA();
                    itemInfo = itemDa.GetInvItemByCostCenterIdForAutoSearch(searchText, storeId);
                    nodeList = (from n in itemInfo
                                select new GLOpeningBalanceTransactionNodeAutoComplete()
                                {
                                    TransactionNodeId = n.ItemId,
                                    NodeName = n.ItemNameAndCode
                                }).ToList();
                }
                else if (inventorySearchType == "Category")
                {
                    List<InvCategoryBO> invCategories = new List<InvCategoryBO>();
                    InvCategoryDA categoryDA = new InvCategoryDA();
                    invCategories = categoryDA.GetCategoryByCostcenterForAutoSearch(searchText, storeId);
                    nodeList = (from n in invCategories
                                select new GLOpeningBalanceTransactionNodeAutoComplete()
                                {

                                    TransactionNodeId = n.CategoryId,
                                    NodeName = n.HeadWithCode,
                                    Lvl = n.Lvl,
                                    Hierarchy = n.Hierarchy
                                }).ToList();
                }
            }
            return nodeList;
        }

        [WebMethod]
        public static GLOpeningBalanceView FillForm(string transactionType, int glCompanyId, int projectcId,
                                                        string voucherDate, string inventorySearchType, int storeId,
                                                                int locationId, long transactionNodeId = 0, string hierarchy = "")
        {
            List<OpeningBalanceAccountList> chartOfAccountsList = new List<OpeningBalanceAccountList>();
            GLOpeningBalanceView openingBalanceView = new GLOpeningBalanceView();
            List<GLOpeningBalanceTransactionNodeAutoComplete> nodeList = new List<GLOpeningBalanceTransactionNodeAutoComplete>();
            HMUtility hmUtility = new HMUtility();

            GLOpeningBalanceDA openingBalanceDA = new GLOpeningBalanceDA();
            string TableHeader = string.Empty;
            DateTime VoucherDate = hmUtility.GetDateTimeFromString(voucherDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            bool isInventoryType = transactionType == ConstantHelper.GLTransactionType.Inventory.ToString();
            openingBalanceView = openingBalanceDA.GetGLOpeningBalanceDetailByTransactionTypeNGLCompanyIdNProjectIdNFiscalYearId(transactionType, glCompanyId, projectcId, VoucherDate, storeId, locationId, isInventoryType);

            if (transactionType == ConstantHelper.GLTransactionType.Accounts.ToString())
            {
                NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

                string openingDate = hmUtility.GetUnivarsalDateTimeFromString(voucherDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                bool isAlreadyHaveOpening = nodeMatrixDA.AlreadyHaveOpening(openingDate);

                if (isAlreadyHaveOpening)
                {
                    openingBalanceView.message = "Already have the opening for the current fiscal year";
                    openingBalanceView.messageType = CommonHelper.MessageTpe.Error.ToString();

                    return openingBalanceView;
                }

                //long retainedEaringsId = Convert.ToInt64(System.Web.Configuration.WebConfigurationManager.AppSettings["RetainedEaringsId"].ToString());

                long retainedEaringsId = 24;
                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("RetainedEaringsId", "RetainedEaringsId");
                if (commonSetupBO != null)
                {
                    if (commonSetupBO.SetupId > 0)
                    {
                        if (commonSetupBO.SetupValue != "0")
                        {
                            retainedEaringsId = Convert.ToInt64(commonSetupBO.SetupValue);
                        }
                    }
                }

                chartOfAccountsList = nodeMatrixDA.GetAccountsForOpeningBalance(retainedEaringsId);
                TableHeader = "Accounts Head";
            }
            else if (transactionType == ConstantHelper.GLTransactionType.Company.ToString())
            {
                List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
                GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

                companyList = guestCompanyDA.GetCompanyInfoByHierarchy((int)transactionNodeId, hierarchy);
                TableHeader = "Company Name";
                nodeList = (from n in companyList
                            select new GLOpeningBalanceTransactionNodeAutoComplete()
                            {
                                TransactionNodeId = n.CompanyId,
                                NodeName = n.CompanyName,
                                Code = n.CompanyNumber
                            }).ToList();
            }
            else if (transactionType == ConstantHelper.GLTransactionType.Supplier.ToString())
            {
                List<PMSupplierBO> supplierList = new List<PMSupplierBO>();
                PMSupplierDA entityDA = new PMSupplierDA();
                if (transactionNodeId > 0)
                    supplierList.Add(entityDA.GetPMSupplierInfoById((int)transactionNodeId));
                else
                    supplierList = entityDA.GetPMSupplierInfo();

                nodeList = (from n in supplierList
                            select new GLOpeningBalanceTransactionNodeAutoComplete()
                            {
                                TransactionNodeId = n.SupplierId,
                                NodeName = n.Name,
                                Code = n.Code
                            }).ToList();
            }
            else if (transactionType == ConstantHelper.GLTransactionType.Employee.ToString())
            {
                List<EmployeeBO> employeeList = new List<EmployeeBO>();
                EmployeeDA entityDA = new EmployeeDA();

                if (transactionNodeId > 0)
                    employeeList.Add(entityDA.GetEmployeeInfoById((int)transactionNodeId));
                else
                    employeeList = entityDA.GetEmployeeInfo();

                nodeList = (from n in employeeList
                            select new GLOpeningBalanceTransactionNodeAutoComplete()
                            {
                                TransactionNodeId = n.EmpId,
                                NodeName = n.DisplayName,
                                Code = n.EmpCode
                            }).ToList();
            }
            else if (transactionType == ConstantHelper.GLTransactionType.Member.ToString())
            {
                MemMemberBasicDA memberBasicDA = new MemMemberBasicDA();
                List<MemMemberBasicsBO> memberList = new List<MemMemberBasicsBO>();

                if (transactionNodeId > 0)
                    memberList.Add(memberBasicDA.GetMemberInfoById((int)transactionNodeId));
                else
                    memberList = memberBasicDA.GetMemMemberList();
                nodeList = (from n in memberList
                            select new GLOpeningBalanceTransactionNodeAutoComplete()
                            {
                                TransactionNodeId = n.MemberId,
                                NodeName = n.FullName,
                                Code = n.MembershipNumber
                            }).ToList();
            }
            else if (transactionType == ConstantHelper.GLTransactionType.Inventory.ToString())
            {
                TableHeader = "Item Name";
                if (inventorySearchType == "Item")
                {
                    InvItemBO itemInfo = new InvItemBO();
                    InvItemDA itemDA = new InvItemDA();
                    itemInfo = itemDA.GetInvItemInfoByItemId((int)transactionNodeId);
                    if (itemInfo.ItemId > 0)
                        nodeList.Add(new GLOpeningBalanceTransactionNodeAutoComplete()
                        {
                            TransactionNodeId = itemInfo.ItemId,
                            NodeName = itemInfo.Name,
                            Code = itemInfo.Code
                        });
                }
                else if (inventorySearchType == "Category")
                {
                    List<InvItemBO> invItems = new List<InvItemBO>();
                    InvItemDA itemDA = new InvItemDA();

                    invItems = itemDA.GetItemByCategoryNCostcenter(storeId, (int)transactionNodeId);
                    nodeList = (from n in invItems
                                select new GLOpeningBalanceTransactionNodeAutoComplete()
                                {
                                    TransactionNodeId = n.ItemId,
                                    NodeName = n.Name,
                                    Code = n.Code
                                }).ToList();
                }
            }

            if (isInventoryType)
            {
                openingBalanceView.TableString = GenerateTableForInventory(nodeList, openingBalanceView.InvOpeningBalanceDetails, TableHeader);
            }
            else
            {
                openingBalanceView.AccountOpeningBalance = chartOfAccountsList;
                openingBalanceView.TableString = GenerateTableForAccountsOpening(openingBalanceView, TableHeader);
            }

            return openingBalanceView;
        }

        [WebMethod]
        public static ReturnInfo SaveGLOpeningBalance(GLOpeningBalance OpeningBalance, List<OpeningBalanceAccountList> OpeningBalanceDetails)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            GLOpeningBalanceDA openingBalanceDA = new GLOpeningBalanceDA();
            long OpeningBalaceId = 0;

            try
            {
                GLOpeningBalanceView openingBalanceView = new GLOpeningBalanceView();
                UserInformationBO userInformation = hmUtility.GetCurrentApplicationUserInfo();
                List<GLOpeningBalanceDetail> balanceDetails = new List<GLOpeningBalanceDetail>();
                GLOpeningBalanceDetail openingBalanceAsset = new GLOpeningBalanceDetail();
                GLOpeningBalanceDetail openingBalanceLiabilities = new GLOpeningBalanceDetail();

                OpeningBalance.CreatedBy = userInformation.UserInfoId;
                OpeningBalance.LastModifiedBy = userInformation.UserInfoId;

                openingBalanceView = openingBalanceDA.GetGLOpeningBalanceDetailByTransactionTypeNGLCompanyIdNProjectIdNFiscalYearId(OpeningBalance.TransactionType, OpeningBalance.CompanyId, OpeningBalance.ProjectId, Convert.ToDateTime(OpeningBalance.VoucherDate), 0, 0, false);

                foreach (OpeningBalanceAccountList opb in OpeningBalanceDetails)
                {
                    var accountAsset = openingBalanceView.OpeningBalanceDetails.Where(a => a.AccountNodeId == opb.AssetNodeId).FirstOrDefault();
                    var accountLiabilities = openingBalanceView.OpeningBalanceDetails.Where(a => a.AccountNodeId == opb.LiabilitiesNodeId).FirstOrDefault();

                    if (accountAsset != null)
                    {
                        openingBalanceAsset = new GLOpeningBalanceDetail()
                        {
                            Id = accountAsset.Id,
                            GLOpeningBalanceId = accountAsset.GLOpeningBalanceId,
                            AccountNodeId = opb.AssetNodeId,
                            AccountType = opb.AssetNodeType,
                            AccountName = opb.AssetNodeHead,
                            OpeningBalance = opb.AssetAmount
                        };

                        balanceDetails.Add(openingBalanceAsset);
                    }
                    else if (opb.AssetAmount != 0)
                    {
                        openingBalanceAsset = new GLOpeningBalanceDetail()
                        {
                            Id = 0,
                            GLOpeningBalanceId = 0,
                            AccountNodeId = opb.AssetNodeId,
                            AccountType = opb.AssetNodeType,
                            AccountName = opb.AssetNodeHead,
                            OpeningBalance = opb.AssetAmount
                        };

                        balanceDetails.Add(openingBalanceAsset);
                    }

                    if (accountLiabilities != null)
                    {
                        openingBalanceLiabilities = new GLOpeningBalanceDetail()
                        {
                            Id = accountLiabilities.Id,
                            GLOpeningBalanceId = accountLiabilities.GLOpeningBalanceId,
                            AccountNodeId = opb.LiabilitiesNodeId,
                            AccountType = opb.LiabilitiesNodeType,
                            AccountName = opb.LiabilitiesNodeHead,
                            OpeningBalance = opb.LiabilitiesAmount
                        };

                        balanceDetails.Add(openingBalanceLiabilities);
                    }
                    else if (opb.LiabilitiesAmount != 0)
                    {
                        openingBalanceLiabilities = new GLOpeningBalanceDetail()
                        {
                            Id = 0,
                            GLOpeningBalanceId = 0,
                            AccountNodeId = opb.LiabilitiesNodeId,
                            AccountType = opb.LiabilitiesNodeType,
                            AccountName = opb.LiabilitiesNodeHead,
                            OpeningBalance = opb.LiabilitiesAmount
                        };
                        balanceDetails.Add(openingBalanceLiabilities);
                    }

                }

                returnInfo.IsSuccess = openingBalanceDA.SaveOrUpdateGLOpeningBalance(OpeningBalance, balanceDetails, out OpeningBalaceId);

                if (returnInfo.IsSuccess)
                {
                    if (OpeningBalance.Id == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.GLOpeningBalance.ToString(), OpeningBalaceId,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLOpeningBalance));
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.GLOpeningBalance.ToString(), OpeningBalance.Id,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLOpeningBalance));
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }
                else
                {
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                throw ex;
            }
            return returnInfo;
        }

        [WebMethod]
        public static ReturnInfo SaveInvOpeningBalance(InvOpeningBalance OpeningBalance, List<InvOpeningBalanceDetail> OpeningBalanceDetails)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            GLOpeningBalanceDA openingBalanceDA = new GLOpeningBalanceDA();
            long OpeningBalaceId = 0;
            try
            {
                UserInformationBO userInformation = hmUtility.GetCurrentApplicationUserInfo();
                OpeningBalance.CreatedBy = userInformation.UserInfoId;
                OpeningBalance.LastModifiedBy = userInformation.UserInfoId;
                returnInfo.IsSuccess = openingBalanceDA.SaveOrUpdateInvOpeningBalance(OpeningBalance, OpeningBalanceDetails, out OpeningBalaceId);

                if (returnInfo.IsSuccess)
                {
                    if (OpeningBalance.Id == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.GLOpeningBalance.ToString(), OpeningBalaceId,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvOpeningBalance));
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.GLOpeningBalance.ToString(), OpeningBalance.Id,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvOpeningBalance));
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }
                else
                {
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                throw ex;
            }
            return returnInfo;
        }

        [WebMethod]
        public static List<InvLocationBO> StoreLocationByCostCenter(int storeId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(storeId);

            return location;
        }

        [WebMethod]
        public static ReturnInfo ApproveOpeningBalance(bool isInventoryType, long id)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            GLOpeningBalanceDA openingBalanceDA = new GLOpeningBalanceDA();
            try
            {
                UserInformationBO userInformation = hmUtility.GetCurrentApplicationUserInfo();
                returnInfo.IsSuccess = openingBalanceDA.ApproveOpeningBalance(isInventoryType, id, userInformation.UserInfoId);

                if (returnInfo.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                    EntityTypeEnum.EntityType.GLOpeningBalance.ToString(), id,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvOpeningBalance));
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                }
                else
                {
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                throw ex;
            }
            return returnInfo;
        }

        private static string GenerateTableForAccountsOpening(GLOpeningBalanceView openingBalanceView, string TableHeader)
        {
            //List<OpeningBalanceAccountList> nodeList, List<GLOpeningBalanceDetail> OpeningBalanceDetails,

            string tHead = string.Empty, tBody = string.Empty, tFoot = string.Empty;
            int count = 0;

            tHead = string.Format(@"<table id='balanceTable' class='table table-bordered table-hover' style='width:100%;'>
                                    <thead>");

            tHead += string.Format(@"<tr> <th style=""text-align:center;"" colspan='2'>Assets</th> <th colspan='2' style=""text-align:center;"">Liabilities</th></tr>");

            tHead += string.Format(@"<tr> <th style=""width:35%;"">" + TableHeader + "</th>");
            tHead += string.Format(@"<th style=""width:15%; text-align:left;"">{0}</th>", "Amount");
            tHead += string.Format(@"<th style=""width:35%;"">" + TableHeader + "</th>");
            tHead += string.Format(@"<th style=""width:15%; text-align:left;"">{0}</th>", "Amount");

            tHead += string.Format(@"</tr></thead>");
            tBody = string.Format(@"<tbody>");

            foreach (OpeningBalanceAccountList opd in openingBalanceView.AccountOpeningBalance)
            {
                var assetsDetail = openingBalanceView.OpeningBalanceDetails.Where(i => i.AccountNodeId == opd.AssetNodeId).FirstOrDefault();
                var liabilitiesDetail = openingBalanceView.OpeningBalanceDetails.Where(i => i.AccountNodeId == opd.LiabilitiesNodeId).FirstOrDefault();

                if (assetsDetail != null)
                {
                    opd.AssetAmount = assetsDetail.OpeningBalance;
                }

                if (liabilitiesDetail != null)
                {
                    opd.LiabilitiesAmount = liabilitiesDetail.OpeningBalance;
                }

                tBody += string.Format(@"<tr> <td did =""{0}""  tnid=""{1}"" style=""width:35%;"">{2}</td>", assetsDetail == null ? 0 : assetsDetail.Id, opd.AssetNodeId, string.IsNullOrEmpty(opd.AssetNodeHead) ? "" : opd.AssetNodeHead);

                if (opd.AssetNodeId > 0)
                    tBody += "<td style=\"width:15%;\" > <input type=\"text\" onblur=\"return CheckAssetInputValue(this," + count + ")\" class=\"form-control quantitynegativedecimal\" value=" + (assetsDetail == null ? "" : assetsDetail.OpeningBalance.ToString()) + "> </td>";
                else
                    tBody += "<td style=\"width:15%;\" ></td>";

                tBody += string.Format(@"<td did =""{0}""  tnid=""{1}"" style=""width:35%;"">{2}</td>", liabilitiesDetail == null ? 0 : liabilitiesDetail.Id, opd.LiabilitiesNodeId, string.IsNullOrEmpty(opd.LiabilitiesNodeHead) ? "" : opd.LiabilitiesNodeHead);

                if (opd.LiabilitiesNodeId > 0)
                    tBody += "<td style=\"width:15%;\" > <input type=\"text\" onblur=\"return CheckLiabilitiesInputValue(this," + count + ")\" class=\"form-control quantitynegativedecimal\" value=" + (liabilitiesDetail == null ? "" : liabilitiesDetail.OpeningBalance.ToString()) + "> </td>";
                else
                    tBody += "<td style=\"width:15%;\" ></td>";

                tBody += string.Format(@"</tr>");

                count++;
            }
            tBody += string.Format(@"</tbody>");

            tFoot = "<tfoot>";
            tFoot += string.Format(@"<tr> <td style=""width:35%; text-align:right;"">Assets Total: </td>");
            tFoot += string.Format(@"<td id='assetsTotal' style=""width:15%; text-align:left;""></td>");
            tFoot += string.Format(@"<td style=""width:35%; text-align:right;"">Liabilities Total: </td>");
            tFoot += string.Format(@"<td id= 'liabilitiesTotal' style=""width:15%; text-align:left;""></td>");
            tFoot += "</tr>";

            tFoot += string.Format(@"<tr> <td colspan='3' style=""text-align:right;"">Opening Balance Equity: </td>");
            tFoot += string.Format(@"<td id= 'openingBalanceEquity' style=""width:15%; text-align:left;""></td>");
            tFoot += "</tr>";
            tFoot += "</tfoot> </table>";

            return (tHead + tBody + tFoot);
        }
        private static string GenerateTableForInventory(List<GLOpeningBalanceTransactionNodeAutoComplete> nodeList, List<InvOpeningBalanceDetail> OpeningBalanceDetails, string TableHeader)
        {
            string tHead = string.Empty, tBody = string.Empty, tFoot = string.Empty;

            tHead = string.Format(@"<table id='balanceTable' class='table table-bordered table-hover' style='width:100%;'>
                                    <thead>");
            tHead += string.Format(@"<tr> <th style=""width:30%;"">" + TableHeader + "</th>");
            tHead += string.Format(@"<th style=""width:10%; text-align:left;"">{0}</th>", "Code");
            tHead += string.Format(@"<th style=""width:20%; text-align:left;"">{0}</th>", "Unit Cost");
            tHead += string.Format(@"<th style=""width:20%; text-align:left;"">{0}</th>", "Stock Qunatity");
            tHead += string.Format(@"<th style=""width:20%; text-align:left;"">{0}</th>", "Total");

            tHead += string.Format(@"</tr></thead>");
            tBody = string.Format(@"<tbody>");

            foreach (GLOpeningBalanceTransactionNodeAutoComplete opd in nodeList)
            {
                var detail = OpeningBalanceDetails.Where(i => i.TransactionNodeId == opd.TransactionNodeId).FirstOrDefault();
                tBody += string.Format(@"<tr> <td did =""{0}""  tnid=""{1}"" style=""width:30%;"">{2}</td>", detail == null ? 0 : detail.Id, opd.TransactionNodeId, opd.NodeName);
                tBody += "<td style=\"width:10%;\" >" + opd.Code + "</td>";
                tBody += "<td style=\"width:20%;\" > <input type=\"text\" onblur=\"UpdateTotal(this)\" class=\"form-control quantitynegativedecimal\" value=" + (detail == null ? "" : detail.UnitCost.ToString()) + "> </td>";
                tBody += "<td style=\"width:20%;\" > <input type=\"text\" onblur=\"UpdateTotal(this)\" class=\"form-control quantitynegativedecimal\" value=" + (detail == null ? "" : detail.StockQuantity.ToString()) + "> </td>";
                tBody += "<td style=\"width:20%;\" > <input disabled=\"disabled\" type=\"text\"  class=\"form-control quantitynegativedecimal\" value=" + (detail == null ? "" : detail.Total.ToString()) + "> </td>";

                tBody += string.Format(@"</tr>");
            }
            tFoot = string.Format(@"</tbody></table>");

            return (tHead + tBody + tFoot);
        }
    }
}