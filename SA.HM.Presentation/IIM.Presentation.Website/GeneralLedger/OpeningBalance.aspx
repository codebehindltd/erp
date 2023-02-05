<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="OpeningBalance.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.OpeningBalance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var TransactionNode = null;
        var Balance = null;
        var AccountOpeningBalance = [];
        var CompanyDebitCreditList = [];
        var SupplierDebitCreditList = [];
        var EmployeeDebitCreditList = [];
        var MemberDebitCreditList = [];
        var CNFDebitCreditList = [];
        var InvOpeningBalanceDetails = [];
        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlStore").select2({
                tags: "true",
                placeholder: $("#<%=CommonDropDownHiddenField.ClientID %>").val(),
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlLocation").select2({
                tags: "true",
                placeholder: $("#<%=CommonDropDownHiddenField.ClientID %>").val(),
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: $("#<%=CommonDropDownHiddenField.ClientID %>").val(),
                allowClear: true,
                width: "99.75%"
            });
            $('#ContentPlaceHolder1_txtVoucherDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_ddlProject").select2({
                tags: "true",
                placeholder: $("#<%=CommonDropDownHiddenField.ClientID %>").val(),
                allowClear: true,
                width: "99.75%"
            });

            LoadStoreLocationByCostCenter($('#ContentPlaceHolder1_ddlStore').val());

            $("#ContentPlaceHolder1_ddlStore").change(function () {
                $("#SaveContent").hide();
                $("#balanceTable").html("");
                LoadStoreLocationByCostCenter($(this).val());
            });
            $("#ContentPlaceHolder1_ddlLocation").change(function () {
                $("#SaveContent").hide();
                $("#balanceTable").html("");
            });
        });

        function LoadStoreLocationByCostCenter(costCenterId) {
            PageMethods.StoreLocationByCostCenter(costCenterId, OnLoadLocationSucceeded, OnLoadLocationFailed);
            return false;
        }

        function OnLoadLocationSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlLocation');

            control.empty();
            PopulateControlWithValueNTextField(result, control, $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "LocationId");
            return false;
        }

        function OnLoadLocationFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
        }

        function PopulateProjects(control) {
            $("#SaveContent").hide();
            $("#balanceTable").html("");
            let companyId = $(control).val();

            $("#ContentPlaceHolder1_ddlProject").empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "./OpeningBalance.aspx/GetGLProjectByGLCompanyId",
                data: JSON.stringify({ companyId: companyId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    PopulateControlWithValueNTextField(response.d, $("#ContentPlaceHolder1_ddlProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
            }

            function PopulateFiscalYear(control) {
                let projectId = $(control).val();
                $("#SaveContent").hide();
                $("#balanceTable").html("");
            }

        function ChangeSearchLabel(control) {
            TransactionNode = null;
            Balance = null;
            $("#balanceTable").html("");
            let controlValue = $(control).val();
            $("#dvInventorySearchType").hide();
            $("#InventoryContent").hide();

            if (controlValue == "Accounts") {
                $("#lblTransaction").text("Account Head");
                $("#lblTransaction").hide();
            }
            else if (controlValue == "Company") {
                $("#lblTransaction").text("Company Name");
                $("#lblTransaction").show();
            }
            else if (controlValue == "Supplier")
            {
                $("#lblTransaction").text("Supplier Name");
                $("#lblTransaction").show();
            }
            else if (controlValue == "Employee")
            {
                $("#lblTransaction").text("Employee Name");
                $("#lblTransaction").show();
            }
            else if (controlValue == "Member")
            {
                $("#lblTransaction").text("Member Name");
                $("#lblTransaction").show();
            }
            else if (controlValue == "Inventory") {
                $("#dvInventorySearchType").show();
                $("#InventoryContent").show();
                $("#lblTransaction").show();
                $("#ContentPlaceHolder1_ddlInventorySearchType").val() == "Item" ? $("#lblTransaction").text("Item Name") : $("#lblTransaction").text("Category Name");
            }
        }


            function Search() {
                let transactionType = $("#ContentPlaceHolder1_ddlTransactionType").val();
                var glCompanyId = $("#ContentPlaceHolder1_ddlCompany").val();
                var glProjectId = $("#ContentPlaceHolder1_ddlProject").val();
                // var fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYear").val();
                var voucherDate = $("#ContentPlaceHolder1_txtVoucherDate").val();
                let inventorySearchType = $("#ContentPlaceHolder1_ddlInventorySearchType").val();
                // var searchType = $("#ContentPlaceHolder1_ddlSearchType").val();
                var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();

                var costCenterId = "0";
                var locationId = "0";
                var transactionNodeId = 0;
                var hierarchy = "";

                if (transactionType == "0") {
                    toastr.warning("Please Select Transaction Type.");
                    $("#ContentPlaceHolder1_ddlTransactionType").focus();
                    return false;
                }
                if (glCompanyId == "0") {
                    toastr.warning("Please Select Company.");
                    $("#ContentPlaceHolder1_ddlCompany").focus();
                    return false;
                }
                if (glProjectId == "0") {
                    toastr.warning("Please Select Fiscal Year.");
                    $("#ContentPlaceHolder1_ddlProject").focus();
                    return false;
                }
                if (voucherDate == "") {
                    toastr.warning("Please Select Opening Date.");
                    $("#ContentPlaceHolder1_txtVoucherDate").focus();
                    return false;
                }
                //if (fiscalYearId == "0") {
                //    toastr.warning("Please Select Fiscal Year.");
                //    $("#ContentPlaceHolder1_ddlFiscalYear").focus();
                //    return false;
                //}
                if (transactionType == "Inventory") {
                    costCenterId = $('#ContentPlaceHolder1_ddlStore').val();
                    locationId = $('#ContentPlaceHolder1_ddlLocation').val();
                    if (costCenterId == "0") {
                        toastr.warning("Please Select Store Name.");
                        $("#ContentPlaceHolder1_ddlStore").focus();
                        return false;
                    }
                    if (locationId == "0") {
                        toastr.warning("Please Select Location Name.");
                        $("#ContentPlaceHolder1_ddlLocation").focus();
                        return false;
                    }
                }

                //if(transactionType != "Accounts"){
                //    if (TransactionNode == null) {
                //        var transactionHeadName = $("#lblTransaction").text();
                //        toastr.warning(`Please Search ${transactionHeadName}`);
                //        // $("#txtSearch").focus();
                //        return false;
                //    }
                //}
                transactionNodeId = TransactionNode != null ? TransactionNode.NodeId : 0;
                hierarchy = TransactionNode != null ? TransactionNode.Hierarchy : "";
                PageMethods.FillForm(transactionType, glCompanyId, glProjectId, voucherDate, categoryId, inventorySearchType, costCenterId, locationId, transactionNodeId, hierarchy, OnSucceedResult, OnError);
                return false;
            }

            function OnSucceedResult(result) {
                let transactionType = $("#ContentPlaceHolder1_ddlTransactionType").val();
                CompanyDebitCreditList = [];
                SupplierDebitCreditList = [];
                EmployeeDebitCreditList = [];
                MemberDebitCreditList = [];
                CNFDebitCreditList = [];
                InvOpeningBalanceDetails = [];

                if(transactionType == "Accounts"){

                    if(result.messageType != null && result.messageType == "Error"){
                        toastr.error( result.message);
                        $("#balanceTable").html("");
                        // $("#txtSearch").val("");
                        $("#SaveContent").hide();
                        $("#btnApprove").hide();
                        return false;
                    }

                    AccountOpeningBalance  = result.AccountOpeningBalance;
                }
                else if (transactionType == "Company") {
                    CompanyDebitCreditList = result.CompanyDebitCreditList;
                }
                else if (transactionType == "Supplier") {
                    SupplierDebitCreditList = result.SupplierDebitCreditList;
                }
                else if (transactionType == "Employee") {
                    EmployeeDebitCreditList = result.EmployeeDebitCreditList;
                }
                else if (transactionType == "Member") {
                    MemberDebitCreditList = result.MemberDebitCreditList;
                }
                else if (transactionType == "CNF") {
                    CNFDebitCreditList = result.CNFDebitCreditList;
                }
                else if (transactionType == "Inventory") {
                    InvOpeningBalanceDetails = result.InvOpeningBalanceDetails;
                }

                if (result.OpeningBalance != null)
                {
                    Balance = result.OpeningBalance;
                    AccountOpeningBalance  = result.AccountOpeningBalance;
                }
                else
                    Balance = result.InvOpeningBalance;

                //if (Balance != null) {
                //    $("#btnApprove").show();
                //    $("#btnSave").val("Update");
                //}
                //else {
                //    $("#btnApprove").hide();
                //    $("#btnSave").val("Save");
                //}

                if (Balance != null || CompanyDebitCreditList.length > 0 || SupplierDebitCreditList.length > 0 || EmployeeDebitCreditList.length > 0 || MemberDebitCreditList.length > 0 || CNFDebitCreditList.length > 0 || InvOpeningBalanceDetails.length > 0) {
                    $("#btnApprove").show();
                    $("#btnSave").val("Update");
                } else {
                    $("#btnApprove").hide();
                    $("#btnSave").val("Save");
                }

                $("#tblSearchItem").html(result.TableString);
                $("#SaveContent").show();

                CommonHelper.ApplyDecimalValidationWithNegetiveValue();

                if(transactionType == "Accounts"){
                    setTimeout(function () { CheckTotal(); }, 1000);
                }
                else if (transactionType == "Company") {
                    setTimeout(function () { setCompanyTotal(); }, 1000);
                }
                else if (transactionType == "Supplier") {
                    setTimeout(function () { setSupplierTotal(); }, 1000);
                }
                else if (transactionType == "Employee") {
                    setTimeout(function () { setEmployeeTotal(); }, 1000);
                }
                else if (transactionType == "Member") {
                    setTimeout(function () { setMemberTotal(); }, 1000);
                }
                else if (transactionType == "CNF") {
                    setTimeout(function () { setCNFTotal(); }, 1000);
                }
                return false;
            }

            function OnError(error) {

                CommonHelper.AlertMessage(error.AlertMessage);
            }


            function SaveBalance() {
                let transactionType = $("#ContentPlaceHolder1_ddlTransactionType").val();
                var glCompanyId = $("#ContentPlaceHolder1_ddlCompany").val();
                var glProjectId = $("#ContentPlaceHolder1_ddlProject").val();
                //var fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYear").val();
                var voucherDate = $("#ContentPlaceHolder1_txtVoucherDate").val();
                
                var costCenterId = "0";
                var locationId = "0";
                var categoryId = "0";
                if (transactionType == "0") {
                    toastr.warning("Please Select Transaction Type.");
                    $("#ContentPlaceHolder1_ddlTransactionType").focus();
                    return false;
                }
                if (glCompanyId == "0") {
                    toastr.warning("Please Select Company.");
                    $("#ContentPlaceHolder1_ddlCompany").focus();
                    return false;
                }
                if (glProjectId == "0") {
                    toastr.warning("Please Select Project.");
                    $("#ContentPlaceHolder1_ddlProject").focus();
                    return false;
                }
                //if (fiscalYearId == "0") {
                //    toastr.warning("Please Select Fiscal Year.");
                //    $("#ContentPlaceHolder1_ddlFiscalYear").focus();
                //    return false;
                //}
                if (voucherDate == "") {
                    toastr.warning("Please Select Voucher Date.");
                    $("#ContentPlaceHolder1_txtVoucherDate").focus();
                    return false;
                }
                if (transactionType == "Inventory") {
                    costCenterId = $('#ContentPlaceHolder1_ddlStore').val();
                    locationId = $('#ContentPlaceHolder1_ddlLocation').val();
                    categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    if (costCenterId == "0") {
                        toastr.warning("Please Select Store Name.");
                        $("#ContentPlaceHolder1_ddlStore").focus();
                        return false;
                    }
                    if (locationId == "0") {
                        toastr.warning("Please Select Location Name.");
                        $("#ContentPlaceHolder1_ddlLocation").focus();
                        return false;
                    }
                    if (categoryId == "0") {
                        toastr.warning("Please Select Category Name.");
                        $("#ContentPlaceHolder1_ddlCategory").focus();
                        return false;
                    }
                }
                var OpeningBalanceDetails = new Array();
                var CompanyDebitCreditList = new Array();
                var SupplierDebitCreditList = new Array();
                var EmployeeDebitCreditList = new Array();
                var MemberDebitCreditList = new Array();
                var CNFDebitCreditList = new Array();

                var transactionNodeId = 0, id = 0, detailsId = 0, debitAmount = 0, creditAmount = 0, unitCost = 0, stockQuantity = 0, totalCost = 0;
                var drAmount = 0, crAmount = 0, quantity = 0;
                if (Balance != null) {
                    id = Balance.Id;
                }
                if (voucherDate != "") {
                    voucherDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(voucherDate, innBoarDateFormat);
                }
                var OpeningBalance = {
                    Id: id,
                    TransactionType: transactionType,
                    CompanyId: glCompanyId,
                    ProjectId: glProjectId,
                    VoucherDate: voucherDate,
                    StoreId: costCenterId,
                    LocationId: locationId,
                    CategoryId: categoryId
                };

                let isInventoryType = transactionType == "Inventory";

                if (transactionType == "Inventory") {
                    $("#balanceTable tbody tr").each(function () {
                        console.log(EditedItemList);
                        transactionNodeId = $(this).find("td:eq(0)").attr("tnid");
                        detailsId = $(this).find("td:eq(0)").attr("did");
                        if (isInventoryType) {
                            unitCost = $(this).find("td:eq(4) input").val();
                            stockQuantity = $(this).find("td:eq(5) input").val();
                            totalCost = $(this).find("td:eq(6) input").val();
                            unitHead = $(this).find("td").eq(7).html();
                            console.log(EditedItemList.includes(transactionNodeId));
                            debugger;
                            if (unitCost || stockQuantity || EditedItemList.includes(transactionNodeId)) {
                                debugger;
                                OpeningBalanceDetails.push({
                                    Id: parseInt(detailsId),
                                    TransactionNodeId: parseInt(transactionNodeId),
                                    UnitCost: unitCost != "" ? parseFloat(unitCost).toFixed(2) : 0.00,
                                    StockQuantity: stockQuantity != "" ? parseFloat(stockQuantity).toFixed(2) : 0.00,
                                    Total: totalCost != "" ? parseFloat(totalCost).toFixed(2) : 0.00,
                                    UnitHead: unitHead
                                });
                                unitCost = "0";
                                stockQuantity = "0";
                                totalCost = "0";
                            }
                        }

                        debitAmount = 0;
                        creditAmount = 0;
                    });
                }
                else if(transactionType == "Accounts"){
                    OpeningBalance.OpeningBalanceEquity = $.trim($("#openingBalanceEquity").text()) == "" ? 0.00 : parseFloat($.trim($("#openingBalanceEquity").text()));
                    OpeningBalance.OpeningBalanceDate = voucherDate;
                }
                else if (transactionType == "Company") {
                    $("#balanceTable tbody tr").each(function () {
                        transactionNodeId = $(this).find("td:eq(0)").attr("tnid");
                        detailsId = $(this).find("td:eq(0)").attr("did");
                        drAmount = $(this).find("td:eq(1) input").val();
                        crAmount = $(this).find("td:eq(2) input").val();
                        if (drAmount || crAmount) {
                            CompanyDebitCreditList.push({
                                Id: parseInt(detailsId),
                                CompanyId: parseInt(transactionNodeId),
                                DrAmount: drAmount != "" ? parseFloat(drAmount).toFixed(5) : 0.00000,
                                CrAmount: crAmount != "" ? parseFloat(crAmount).toFixed(5) : 0.00000
                            });
                        }
                    });
                }
                else if (transactionType == "Supplier") {
                    $("#balanceTable tbody tr").each(function () {
                        transactionNodeId = $(this).find("td:eq(0)").attr("tnid");
                        detailsId = $(this).find("td:eq(0)").attr("did");
                        drAmount = $(this).find("td:eq(1) input").val();
                        crAmount = $(this).find("td:eq(2) input").val();
                        if (drAmount || crAmount) {
                            SupplierDebitCreditList.push({
                                Id: parseInt(detailsId),
                                SupplierId: parseInt(transactionNodeId),
                                DrAmount: drAmount != "" ? parseFloat(drAmount).toFixed(5) : 0.00000,
                                CrAmount: crAmount != "" ? parseFloat(crAmount).toFixed(5) : 0.00000
                            });
                        }
                    });
                }
                else if (transactionType == "Employee") {
                    $("#balanceTable tbody tr").each(function () {
                        transactionNodeId = $(this).find("td:eq(0)").attr("tnid");
                        detailsId = $(this).find("td:eq(0)").attr("did");
                        drAmount = $(this).find("td:eq(1) input").val();
                        crAmount = $(this).find("td:eq(2) input").val();
                        if (drAmount || crAmount) {
                            EmployeeDebitCreditList.push({
                                Id: parseInt(detailsId),
                                EmployeeId: parseInt(transactionNodeId),
                                DrAmount: drAmount != "" ? parseFloat(drAmount).toFixed(5) : 0.00000,
                                CrAmount: crAmount != "" ? parseFloat(crAmount).toFixed(5) : 0.00000
                            });
                        }
                    });
                }
                else if (transactionType == "Member") {
                    $("#balanceTable tbody tr").each(function () {
                        transactionNodeId = $(this).find("td:eq(0)").attr("tnid");
                        detailsId = $(this).find("td:eq(0)").attr("did");
                        drAmount = $(this).find("td:eq(1) input").val();
                        crAmount = $(this).find("td:eq(2) input").val();
                        if (drAmount || crAmount) {
                            MemberDebitCreditList.push({
                                Id: parseInt(detailsId),
                                MemberId: parseInt(transactionNodeId),
                                DrAmount: drAmount != "" ? parseFloat(drAmount).toFixed(5) : 0.00000,
                                CrAmount: crAmount != "" ? parseFloat(crAmount).toFixed(5) : 0.00000
                            });
                        }
                    });
                }
                else if (transactionType == "CNF") {
                    $("#balanceTable tbody tr").each(function () {
                        transactionNodeId = $(this).find("td:eq(0)").attr("tnid");
                        detailsId = $(this).find("td:eq(0)").attr("did");
                        drAmount = $(this).find("td:eq(1) input").val();
                        crAmount = $(this).find("td:eq(2) input").val();
                        if (drAmount || crAmount) {
                            CNFDebitCreditList.push({
                                Id: parseInt(detailsId),
                                SupplierId: parseInt(transactionNodeId),
                                DrAmount: drAmount != "" ? parseFloat(drAmount).toFixed(5) : 0.00000,
                                CrAmount: crAmount != "" ? parseFloat(crAmount).toFixed(5) : 0.00000
                            });
                        }
                    });
                }

                if (transactionType == "Inventory" && OpeningBalanceDetails.length == 0) {
                    toastr.warning("Please Give Inventory Opening.");
                    return false;
                }
                else if(transactionType == "Accounts"){
                    var assetTtoal = $.trim($("#assetsTotal").text());
                    var liabilitiesTtoal = $.trim($("#liabilitiesTotal").text());

                    if(assetTtoal == "" || liabilitiesTtoal == "" || assetTtoal == "0" || liabilitiesTtoal == "0"){
                        toastr.warning("Please Give Asset/Liabilities Accounts Opening.");
                        return false;
                    }                  
                }
                if (transactionType == "Inventory")
                    PageMethods.SaveInvOpeningBalance(OpeningBalance, OpeningBalanceDetails, OnSucceedSaveOpeningBalance, OnErrorSucceedSaveOpeningBalance);
                else if (transactionType == "Accounts")
                    PageMethods.SaveGLOpeningBalance(OpeningBalance, AccountOpeningBalance, OnSucceedSaveOpeningBalance, OnErrorSucceedSaveOpeningBalance);
                else if (transactionType == "Company")
                    PageMethods.SaveCompanyOpeningBalance(OpeningBalance, CompanyDebitCreditList, OnSaveCompanyOpeningBalanceSucceeded, OnSaveCompanyOpeningBalanceFailed);
                else if (transactionType == "Supplier")
                    PageMethods.SaveSupplierOpeningBalance(OpeningBalance, SupplierDebitCreditList, OnSaveSupplierOpeningBalanceSucceeded, OnSaveSupplierOpeningBalanceFailed);
                else if (transactionType == "Employee")
                    PageMethods.SaveEmployeeOpeningBalance(OpeningBalance, EmployeeDebitCreditList, OnSaveEmployeeOpeningBalanceSucceeded, OnSaveEmployeeOpeningBalanceFailed);
                else if (transactionType == "Member")
                    PageMethods.SaveMemberOpeningBalance(OpeningBalance, MemberDebitCreditList, OnSaveMemberOpeningBalanceSucceeded, OnSaveMemberOpeningBalanceFailed);
                else if (transactionType == "CNF")
                    PageMethods.SaveCNFOpeningBalance(OpeningBalance, CNFDebitCreditList, OnSaveCNFOpeningBalanceSucceeded, OnSaveCNFOpeningBalanceFailed);
                return false;
            }

            function OnSaveCompanyOpeningBalanceSucceeded(result) {
                CommonHelper.AlertMessage(result.AlertMessage);
                if (result.IsSuccess) {
                    ClearAll();
                }
            }

            function OnSaveCompanyOpeningBalanceFailed(result) {
                CommonHelper.AlertMessage(error.AlertMessage);
            }

            function OnSaveSupplierOpeningBalanceSucceeded(result) {
                CommonHelper.AlertMessage(result.AlertMessage);
                if (result.IsSuccess) {
                    ClearAll();
                }
            }

            function OnSaveSupplierOpeningBalanceFailed(result) {
                CommonHelper.AlertMessage(error.AlertMessage);
            }
            
            function OnSaveEmployeeOpeningBalanceSucceeded(result) {
                CommonHelper.AlertMessage(result.AlertMessage);
                if (result.IsSuccess) {
                    ClearAll();
                }
            }

            function OnSaveEmployeeOpeningBalanceFailed(result) {
                CommonHelper.AlertMessage(error.AlertMessage);
            }

            function OnSaveMemberOpeningBalanceSucceeded(result) {
                CommonHelper.AlertMessage(result.AlertMessage);
                if (result.IsSuccess) {
                    ClearAll();
                }
            }

            function OnSaveMemberOpeningBalanceFailed(result) {
                CommonHelper.AlertMessage(error.AlertMessage);
            }
            
            function OnSaveCNFOpeningBalanceSucceeded(result) {
                CommonHelper.AlertMessage(result.AlertMessage);
                if (result.IsSuccess) {
                    ClearAll();
                }
            }

            function OnSaveCNFOpeningBalanceFailed(result) {
                CommonHelper.AlertMessage(error.AlertMessage);
            }

            function OnSucceedSaveOpeningBalance(result) {
                CommonHelper.AlertMessage(result.AlertMessage);
                if (result.IsSuccess) {
                    ClearAll();
                }
            }

            function OnErrorSucceedSaveOpeningBalance(error) {

                CommonHelper.AlertMessage(error.AlertMessage);
            }
            function ClearAll() {
                TransactionNode = null;
                Balance = null;
                $("#ContentPlaceHolder1_ddlCompany").val("0");
                $("#ContentPlaceHolder1_ddlProject").val("0");
                //$("#ContentPlaceHolder1_ddlFiscalYear").val("0");
                $("#ContentPlaceHolder1_txtVoucherDate").val("");
                $('#ContentPlaceHolder1_ddlStore').val("0").trigger('change');
                $('#ContentPlaceHolder1_ddlLocation').val("0").trigger('change');
                // $('#ContentPlaceHolder1_ddlSearchType').val("0");
                $("#balanceTable").html("");
                // $("#txtSearch").val("");
                $("#SaveContent").hide();
                $("#btnApprove").hide();
            }
            
        //var DeleteInvOpeningBalanceDetailList = [];
            var EditedItemList = [];
            function IsValueExisted(control) {
                var tableRow = $(control).parent().parent();
                var unitCost = tableRow.find("td:eq(4) input").val();
                var stockQuantity = tableRow.find("td:eq(5) input").val();
                if (unitCost != "" || stockQuantity != "") {
                    var transactionNodeId = tableRow.find("td:eq(0)").attr("tnid");
                    if (!EditedItemList.includes(transactionNodeId)) {
                        EditedItemList.push(transactionNodeId);
                    }                    
                }
            }
            function UpdateTotal(control) {
                var tableRow = $(control).parent().parent();

                var unitCost = tableRow.find("td:eq(4) input").val() != "" ? parseFloat(tableRow.find("td:eq(4) input").val()).toFixed(2) : 0.00;
                var stockQuantity = tableRow.find("td:eq(5) input").val() != "" ? parseFloat(tableRow.find("td:eq(5) input").val()).toFixed(2) : 0.00;
                if (!isNaN(unitCost) && !isNaN(stockQuantity))
                    tableRow.find("td:eq(6) input").val(parseFloat(unitCost * stockQuantity).toFixed(2));

                //if (unitCost == 0.00 && stockQuantity == 0.00) {
                //    var transactionNodeId = $(this).find("td:eq(0)").attr("tnid");
                //    var detailsId = $(this).find("td:eq(0)").attr("did");
                //    DeleteInvOpeningBalanceDetailList.push({
                //        TransactionNodeId: transactionNodeId,
                //        UnitCost: unitCost,
                //        StockQuantity: stockQuantity
                //    });
                //}
            }

            function CheckAssetInputValue(control, index) {
                var tableRow = $(control).parent().parent();
                var amount = tableRow.find("td:eq(1) input").val() != "" ? parseFloat(tableRow.find("td:eq(1) input").val()) : 0.00;
                AccountOpeningBalance[index].AssetAmount = amount;
                CheckTotal();
                return true;
            }

            function CheckLiabilitiesInputValue(control, index) {
                var tableRow = $(control).parent().parent();

                var amount = tableRow.find("td:eq(3) input").val() != "" ? parseFloat(tableRow.find("td:eq(3) input").val()) : 0.00;
                AccountOpeningBalance[index].LiabilitiesAmount = amount;
                CheckTotal();
                return true;
            }

            function CheckTotal(){
                var count = AccountOpeningBalance.length;
                var row = 0, sumAsset = 0, liabilitiesAmount = 0;
                for(row = 0; row< count ; row++){
                    sumAsset = sumAsset + AccountOpeningBalance[row].AssetAmount;
                    liabilitiesAmount = liabilitiesAmount + AccountOpeningBalance[row].LiabilitiesAmount;
                }

                $("#assetsTotal").text(sumAsset);
                $("#liabilitiesTotal").text(liabilitiesAmount);

                if(liabilitiesAmount < 0)
                    $("#openingBalanceEquity").text(sumAsset + liabilitiesAmount); 
                else
                    $("#openingBalanceEquity").text(sumAsset - liabilitiesAmount); 
            }

            //function CheckCreditExists(control) {
            //    let tableRow = $(control).parent().parent();
            //    if (parseFloat(tableRow.find("td:eq(2) input").val()) > 0) {
            //        tableRow.find("td:eq(1) input").attr("disabled", true);
            //    }
            //    else if (parseFloat(tableRow.find("td:eq(1) input").val()) == 0) {
            //        tableRow.find("td:eq(2) input").attr("disabled", false);
            //    }
            //    else if (tableRow.find("td:eq(1) input").val() == "") {
            //        tableRow.find("td:eq(2) input").attr("disabled", false);
            //    }
        //}

            function CheckCreditExists(control) {
                let tableRow = $(control).parent().parent();
                if (parseFloat(tableRow.find("td:eq(1) input").val()) > 0) {
                    tableRow.find("td:eq(2) input").attr("disabled", true);
                }
                else if (parseFloat(tableRow.find("td:eq(1) input").val()) == 0) {
                    tableRow.find("td:eq(2) input").attr("disabled", false);
                }
                else if (tableRow.find("td:eq(1) input").val() == "") {
                    tableRow.find("td:eq(2) input").attr("disabled", false);
                }
            }

            function OnDebitClick(control) {
                let tableRow = $(control).parent().parent();
                if (parseFloat(tableRow.find("td:eq(2) input").val()) > 0) {
                    tableRow.find("td:eq(1) input").attr("disabled", true);
                }
            }
            
            function OnCreditClick(control) {
                let tableRow = $(control).parent().parent();
                if (parseFloat(tableRow.find("td:eq(1) input").val()) > 0) {
                    tableRow.find("td:eq(2) input").attr("disabled", true);
                }
            }
            
            function CheckDebitExists(control) {
                let tableRow = $(control).parent().parent();
                if (parseFloat(tableRow.find("td:eq(2) input").val()) > 0) {
                    tableRow.find("td:eq(1) input").attr("disabled", true);
                }
                else if (parseFloat(tableRow.find("td:eq(2) input").val()) == 0) {
                    tableRow.find("td:eq(1) input").attr("disabled", false);
                }
                else if (tableRow.find("td:eq(2) input").val() == "") {
                    tableRow.find("td:eq(1) input").attr("disabled", false);
                }
            }
            
            function CheckDebitInputValue(control, index) {
                let tableRow = $(control).parent().parent();
                let amount = tableRow.find("td:eq(1) input").val() != "" ? parseFloat(tableRow.find("td:eq(1) input").val()) : 0.00000;
                CompanyDebitCreditList[index].DrAmount = amount;
                setCompanyTotal();
                return true;
            }
            
            function CheckCreditInputValue(control, index) {
                let tableRow = $(control).parent().parent();
                let amount = tableRow.find("td:eq(2) input").val() != "" ? parseFloat(tableRow.find("td:eq(2) input").val()) : 0.00000;
                CompanyDebitCreditList[index].CrAmount = amount;
                setCompanyTotal();
                return true;
            }
            
            function setCompanyTotal() {
                let count = CompanyDebitCreditList.length;
                let row = 0, sumDebit = 0, sumCredit = 0;
                for (row = 0; row < count; row++) {
                    sumDebit = sumDebit + CompanyDebitCreditList[row].DrAmount;
                    sumCredit = sumCredit + CompanyDebitCreditList[row].CrAmount;
                }

                $("#debitTotal").text(sumDebit);
                $("#creditTotal").text(sumCredit);
            }

            function CheckSupplierDebitInputValue(control, index) {
                let tableRow = $(control).parent().parent();
                let amount = tableRow.find("td:eq(1) input").val() != "" ? parseFloat(tableRow.find("td:eq(1) input").val()) : 0.00000;
                SupplierDebitCreditList[index].DrAmount = amount;
                setSupplierTotal();
                return true;
            }

            function CheckSupplierCreditInputValue(control, index) {
                let tableRow = $(control).parent().parent();
                let amount = tableRow.find("td:eq(2) input").val() != "" ? parseFloat(tableRow.find("td:eq(2) input").val()) : 0.00000;
                SupplierDebitCreditList[index].CrAmount = amount;
                setSupplierTotal();
                return true;
            }
            
            function setSupplierTotal() {
                let count = SupplierDebitCreditList.length;
                let row = 0, sumDebit = 0, sumCredit = 0;
                for (row = 0; row < count; row++) {
                    sumDebit = sumDebit + SupplierDebitCreditList[row].DrAmount;
                    sumCredit = sumCredit + SupplierDebitCreditList[row].CrAmount;
                }
                $("#debitTotal").text(sumDebit);
                $("#creditTotal").text(sumCredit);
            }

            function CheckEmployeeDebitInputValue(control, index) {
                let tableRow = $(control).parent().parent();
                let amount = tableRow.find("td:eq(1) input").val() != "" ? parseFloat(tableRow.find("td:eq(1) input").val()) : 0.00000;
                EmployeeDebitCreditList[index].DrAmount = amount;
                setEmployeeTotal();
                return true;
            }

            function CheckEmployeeCreditInputValue(control, index) {
                let tableRow = $(control).parent().parent();
                let amount = tableRow.find("td:eq(2) input").val() != "" ? parseFloat(tableRow.find("td:eq(2) input").val()) : 0.00000;
                EmployeeDebitCreditList[index].CrAmount = amount;
                setEmployeeTotal();
                return true;
            }

            function setEmployeeTotal() {
                let count = EmployeeDebitCreditList.length;
                let row = 0, sumDebit = 0, sumCredit = 0;
                for (row = 0; row < count; row++) {
                    sumDebit += EmployeeDebitCreditList[row].DrAmount;
                    sumCredit += EmployeeDebitCreditList[row].CrAmount;
                }
                $("#debitTotal").text(sumDebit);
                $("#creditTotal").text(sumCredit);
            }

            function CheckMemberDebitInputValue(control, index) {
                let tableRow = $(control).parent().parent();
                let amount = tableRow.find("td:eq(1) input").val() != "" ? parseFloat(tableRow.find("td:eq(1) input").val()) : 0.00000;
                MemberDebitCreditList[index].DrAmount = amount;
                setMemberTotal();
                return true;
            }

            function CheckMemberCreditInputValue(control, index) {
                let tableRow = $(control).parent().parent();
                let amount = tableRow.find("td:eq(2) input").val() != "" ? parseFloat(tableRow.find("td:eq(2) input").val()) : 0.00000;
                MemberDebitCreditList[index].CrAmount = amount;
                setMemberTotal();
                return true;
            }

            function setMemberTotal() {
                let count = MemberDebitCreditList.length;
                let row = 0, sumDebit = 0, sumCredit = 0;
                for (row = 0; row < count; row++) {
                    sumDebit += MemberDebitCreditList[row].DrAmount;
                    sumCredit += MemberDebitCreditList[row].CrAmount;
                }
                $("#debitTotal").text(sumDebit);
                $("#creditTotal").text(sumCredit);
            }

            function CheckCNFDebitInputValue(control, index) {
                let tableRow = $(control).parent().parent();
                let amount = tableRow.find("td:eq(1) input").val() != "" ? parseFloat(tableRow.find("td:eq(1) input").val()) : 0.00000;
                CNFDebitCreditList[index].DrAmount = amount;
                setCNFTotal();
                return true;
            }

            function CheckCNFCreditInputValue(control, index) {
                let tableRow = $(control).parent().parent();
                let amount = tableRow.find("td:eq(2) input").val() != "" ? parseFloat(tableRow.find("td:eq(2) input").val()) : 0.00000;
                CNFDebitCreditList[index].CrAmount = amount;
                setCNFTotal();
                return true;
            }

            function setCNFTotal() {
                let count = CNFDebitCreditList.length;
                let row = 0, sumDebit = 0, sumCredit = 0;
                for (row = 0; row < count; row++) {
                    sumDebit += CNFDebitCreditList[row].DrAmount;
                    sumCredit += CNFDebitCreditList[row].CrAmount;
                }
                $("#debitTotal").text(sumDebit);
                $("#creditTotal").text(sumCredit);
            }

            function ApproveBalance() {
                if (!confirm("Do you want to Approve?"))
                    return false;
                let isInventoryType = $("#ContentPlaceHolder1_ddlTransactionType").val() == "Inventory";
                let id = 0;
                let transactionType = $("#ContentPlaceHolder1_ddlTransactionType").val();
                if (transactionType == "Accounts"){
                    id = Balance.Id;
                }
                let companyId, supplierId, employeeId, memberId, itemId;
                let transactionInfo = [];
                let transactionId = 0;
                if (transactionType == "Company") {
                    transactionInfo = CompanyDebitCreditList;
                }
                else if (transactionType == "Supplier") {
                    transactionInfo = SupplierDebitCreditList;
                }
                else if (transactionType == "Employee") {
                    transactionInfo = EmployeeDebitCreditList;
                }
                else if (transactionType == "Member") {
                    transactionInfo = MemberDebitCreditList;
                }
                else if (transactionType == "CNF") {
                    transactionInfo = CNFDebitCreditList;
                }
                else if (transactionType == "Inventory") {
                    id = InvOpeningBalanceDetails[0].InvOpeningBalanceId;
                }
                
                if (transactionInfo.length > 0) {
                    PageMethods.ApproveTransactionOpeningBalance(transactionInfo, transactionType, OnApproveTransactionOpeningBalanceSucceeded, OnApproveTransactionOpeningBalanceFailed);
                    return false;
                }

                if (id > 0) {
                    PageMethods.ApproveOpeningBalance(isInventoryType, id, OnSucceedApproveOpeningBalance, OnErrorSucceedApproveOpeningBalance);
                    return false;
                }
            }
            
            function OnApproveTransactionOpeningBalanceSucceeded(result) {
                CommonHelper.AlertMessage(result.AlertMessage);
                if (result.IsSuccess) {
                    ClearAll();
                }
            }
            
            function OnApproveTransactionOpeningBalanceFailed() {
                CommonHelper.AlertMessage(error.AlertMessage);
            }

            function OnSucceedApproveOpeningBalance(result) {
                CommonHelper.AlertMessage(result.AlertMessage);
                if (result.IsSuccess) {
                    ClearAll();
                }
            }

            function OnErrorSucceedApproveOpeningBalance(error) {

                CommonHelper.AlertMessage(error.AlertMessage);
            }

            function ShowHideSearch(control) {
                TransactionNode = null;
                $("#balanceTable").html("");
                // $("#txtSearch").val("");
                $("#SaveContent").hide();
                $("#btnApprove").hide();
                if ($(control).val() == "1")
                    $("#dvSearch").hide();
                else if ($(control).val() == "0")
                    $("#dvSearch").show();
            }
    </script>

    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsValueExisted" Value="0" runat="server"></asp:HiddenField>
    <div class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Transaction Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control" TabIndex="1" onchange="ChangeSearchLabel(this)">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Company</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" TabIndex="2" onchange="PopulateProjects(this)">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Project</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" TabIndex="3" onchange="PopulateFiscalYear(this)">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Opening Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtVoucherDate"></asp:TextBox>
                        <%--<asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control" TabIndex="4">
                        </asp:DropDownList>--%>
                    </div>
                </div>
                <div class="form-group" id="InventoryContent" style="display: none;">
                    <div class="col-md-2">
                        <label class="control-label required-field">Cost Center</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="form-control" TabIndex="5">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Location</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" TabIndex="6">
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="dvInventorySearchType" style="display: none;">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Search Type</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlInventorySearchType" runat="server" CssClass="form-control" TabIndex="7">
                                <asp:ListItem Value="Category">Category</asp:ListItem>
                                <asp:ListItem Value="Item">Item</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Category</label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" TabIndex="7">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <%--<div id="dvSearchType" class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <label class="control-label required-field">Search Type</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="form-control" TabIndex="8" onchange="ShowHideSearch(this)">
                            <asp:ListItem Value="0">Individual</asp:ListItem>
                            <asp:ListItem Value="1">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>--%>
                <%--<div id="dvSearch" class="form-group">
                    <div class="col-md-2">
                        <label id="lblTransaction" class="control-label required-field">Account Head</label>
                    </div>
                    <div class="col-md-10">
                        <input type="text" id="txtSearch" class="form-control" tabindex="9" />
                    </div>
                </div>--%>
                <div class="row">
                    <div class="col-md-12" id="divGenerate">
                        <input type="button" value="Search" class="TransactionalButton btn btn-primary btn-sm" onclick="Search()" tabindex="10" />
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div style="overflow-x: scroll;">
                        <div id="tblSearchItem" style="width: 100%;">
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="row" id="SaveContent" style="display: none;">
                <div id="dvSave" class="col-md-12" runat="server">
                    <input id="btnSave" type="button" value="Save" class="TransactionalButton btn btn-primary btn-sm" onclick="SaveBalance()" tabindex="11" />
                    <input style="display: none;" id="btnApprove" type="button" value="Approve" class="TransactionalButton btn btn-primary btn-sm" onclick="ApproveBalance()" tabindex="12" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
