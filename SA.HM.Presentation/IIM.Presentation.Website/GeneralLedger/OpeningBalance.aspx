<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="OpeningBalance.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.OpeningBalance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var TransactionNode = null;
        var Balance = null;
        var AccountOpeningBalance = [];
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
            // txtVoucherDate
            $("#txtSearch").autocomplete({

                source: function (request, response) {
                    let url = 'OpeningBalance.aspx/AutoCompleteTransactionNode';
                    let transactionType = $("#ContentPlaceHolder1_ddlTransactionType").val();

                    let inventorySearchType = $("#ContentPlaceHolder1_ddlInventorySearchType").val();
                    let storeId = $("#ContentPlaceHolder1_ddlStore").val();
                    if (transactionType == "Inventory") {
                        if (storeId == "0") {
                            toastr.warning("Please Select Store Name.");
                            $("#ContentPlaceHolder1_ddlStore").focus();
                            return false;
                        }
                    }
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: url,
                        data: JSON.stringify({ transactionType: transactionType, searchText: request.term, inventorySearchType: inventorySearchType, storeId: storeId }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.NodeName,
                                    value: m.TransactionNodeId,
                                    NodeId: m.TransactionNodeId,
                                    Lvl: m.Lvl,
                                    Hierarchy: m.Hierarchy
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);

                    TransactionNode = ui.item;
                }
            });

            $("#ContentPlaceHolder1_ddlStore").change(function () {
                $("#SaveContent").hide();
                $("#balanceTable").html("");
                LoadStoreLocationByCostCenter($(this).val());
            });
            $("#ContentPlaceHolder1_ddlLocation").change(function () {
                $("#SaveContent").hide();
                $("#balanceTable").html("");
            });
            //$("#ContentPlaceHolder1_ddlFiscalYear").change(function () {
            //    $("#SaveContent").hide();
            //    $("#balanceTable").html("");
            //});
        });

        function LoadStoreLocationByCostCenter(storeId) {
            PageMethods.StoreLocationByCostCenter(storeId, OnLoadLocationSucceeded, OnLoadLocationFailed);
            return false;
        }

        function OnLoadLocationSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlLocation');

            control.empty();
            PopulateControlWithValueNTextField(result, control, $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "LocationId");

            //if (list.length == 1 && $("#ContentPlaceHolder1_hfLocationId").val() == "0")
            //$("#ContentPlaceHolder1_ddlLocation").val($("#ContentPlaceHolder1_ddlReceiveLocation option:first").val());
            //else
            //$("#ContentPlaceHolder1_ddlLocation").val($("#ContentPlaceHolder1_hfLocationId").val()).trigger("change");
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
            <%--$('#<%=ddlFiscalYear.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "./OpeningBalance.aspx/PopulateFiscalYear",
                data: JSON.stringify({ projectId: projectId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    PopulateControlWithValueNTextField(result.d, $("#<%=ddlFiscalYear.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "FiscalYearName", "FiscalYearId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });--%>
            }

        function ChangeSearchLabel(control = $("#ContentPlaceHolder1_ddlTransactionType")) {
            TransactionNode = null;
            Balance = null;
            $("#balanceTable").html("");
            $("#txtSearch").val("");
            let controlValue = $(control).val();
            $("#dvInventorySearchType").hide();
            $("#InventoryContent").hide();
            $("#dvSearchType").show();

            if (controlValue == "Accounts") {
                $("#lblTransaction").text("Account Head");
                $("#dvSearchType").hide();
                $("#txtSearch").hide();
                $("#lblTransaction").hide();
            }
            else if (controlValue == "Company") {
                $("#lblTransaction").text("Company Name");
                $("#txtSearch").show();
                $("#lblTransaction").show();
            }
            else if (controlValue == "Supplier")
            {
                $("#lblTransaction").text("Supplier Name");
                $("#txtSearch").show();
                $("#lblTransaction").show();
            }
            else if (controlValue == "Employee")
            {
                $("#lblTransaction").text("Employee Name");
                $("#txtSearch").show();
                $("#lblTransaction").show();
            }
            else if (controlValue == "Member")
            {
                $("#lblTransaction").text("Member Name");
                $("#txtSearch").show();
                $("#lblTransaction").show();
            }
                //else if (controlValue == "CNF")
                // $("#lblTransaction").text("Account Name");
            else if (controlValue == "Inventory") {
                $("#txtSearch").show();
                $("#dvSearchType").hide();
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
                //var fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYear").val();
                var voucherDate = $("#ContentPlaceHolder1_txtVoucherDate").val();
                let inventorySearchType = $("#ContentPlaceHolder1_ddlInventorySearchType").val();
                var searchType = $("#ContentPlaceHolder1_ddlSearchType").val();

                var storeId = "0";
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
                    storeId = $('#ContentPlaceHolder1_ddlStore').val();
                    locationId = $('#ContentPlaceHolder1_ddlLocation').val();
                    if (storeId == "0") {
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

                if(transactionType != "Accounts"){
                    if (TransactionNode == null && searchType == "0") {
                        var transactionHeadName = $("#lblTransaction").text();
                        toastr.warning(`Please Search ${transactionHeadName}`);
                        $("#txtSearch").focus();
                        return false;
                    }
                }

                transactionNodeId = TransactionNode != null ? TransactionNode.NodeId : 0;
                hierarchy = TransactionNode != null ? TransactionNode.Hierarchy : "";

                PageMethods.FillForm(transactionType, glCompanyId, glProjectId, voucherDate, inventorySearchType, storeId, locationId, transactionNodeId, hierarchy, OnSucceedResult, OnError);
                return false;
            }

            function OnSucceedResult(result) {

                let transactionType = $("#ContentPlaceHolder1_ddlTransactionType").val();

                if(transactionType == "Accounts"){

                    if(result.messageType != null && result.messageType == "Error"){
                        toastr.error( result.message);
                        $("#balanceTable").html("");
                        $("#txtSearch").val("");
                        $("#SaveContent").hide();
                        $("#btnApprove").hide();
                        return false;
                    }

                    AccountOpeningBalance  = result.AccountOpeningBalance;
                }

                if (result.OpeningBalance != null)
                {
                    Balance = result.OpeningBalance;
                    AccountOpeningBalance  = result.AccountOpeningBalance;
                }
                else
                    Balance = result.InvOpeningBalance;

                if (Balance != null) {
                    $("#btnApprove").show();
                    $("#btnSave").val("Update");
                }
                else {
                    $("#btnApprove").hide();
                    $("#btnSave").val("Save");
                }
                $("#tblSearchItem").html(result.TableString);
                $("#SaveContent").show();

                CommonHelper.ApplyDecimalValidationWithNegetiveValue();

                if(transactionType == "Accounts"){
                    setTimeout(function(){ CheckTotal(); }, 1000);
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
                var storeId = "0";
                var locationId = "0";
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
                    storeId = $('#ContentPlaceHolder1_ddlStore').val();
                    locationId = $('#ContentPlaceHolder1_ddlLocation').val();
                    if (storeId == "0") {
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
                var OpeningBalanceDetails = new Array();

                var transactionNodeId = 0, id = 0, detailsId = 0, debitAmount = 0, creditAmount = 0, unitCost = 0, stockQuantity = 0, totalCost = 0;

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
                    StoreId: storeId,
                    LocationId: locationId
                };

                let isInventoryType = transactionType == "Inventory";

                if(transactionType == "Inventory")
                {
                    $("#balanceTable tbody tr").each(function () {

                        transactionNodeId = $(this).find("td:eq(0)").attr("tnid");
                        detailsId = $(this).find("td:eq(0)").attr("did");

                        if (isInventoryType) {
                            unitCost = $(this).find("td:eq(2) input").val();
                            stockQuantity = $(this).find("td:eq(3) input").val();
                            totalCost = $(this).find("td:eq(4) input").val();

                            if (unitCost && stockQuantity) {

                                OpeningBalanceDetails.push({
                                    Id: parseInt(detailsId),
                                    TransactionNodeId: parseInt(transactionNodeId),
                                    UnitCost: unitCost != "" ? parseFloat(unitCost).toFixed(2) : 0.00,
                                    StockQuantity: stockQuantity != "" ? parseFloat(stockQuantity).toFixed(2) : 0.00,
                                    Total: totalCost != "" ? parseFloat(totalCost).toFixed(2) : 0.00
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
                else if(transactionType == "Accounts")
                    PageMethods.SaveGLOpeningBalance(OpeningBalance, AccountOpeningBalance , OnSucceedSaveOpeningBalance, OnErrorSucceedSaveOpeningBalance);
                return false;
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
                $('#ContentPlaceHolder1_ddlSearchType').val("0");
                $("#balanceTable").html("");
                $("#txtSearch").val("");
                $("#SaveContent").hide();
                $("#btnApprove").hide();
            }

            function UpdateTotal(control) {
                var tableRow = $(control).parent().parent();

                var unitCost = tableRow.find("td:eq(2) input").val() != "" ? parseFloat(tableRow.find("td:eq(2) input").val()).toFixed(2) : 0.00;
                var stockQuantity = tableRow.find("td:eq(3) input").val() != "" ? parseFloat(tableRow.find("td:eq(3) input").val()).toFixed(2) : 0.00;
                if (!isNaN(unitCost) && !isNaN(stockQuantity))
                    tableRow.find("td:eq(4) input").val(parseFloat(unitCost * stockQuantity).toFixed(2));

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

            function ApproveBalance() {
                if (!confirm("Do you want to Approve?"))
                    return false;
                let isInventoryType = $("#ContentPlaceHolder1_ddlTransactionType").val() == "Inventory";
                let id = Balance.Id;
                if (id > 0) {
                    PageMethods.ApproveOpeningBalance(isInventoryType, id, OnSucceedApproveOpeningBalance, OnErrorSucceedApproveOpeningBalance);
                    return false;
                }
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
                $("#txtSearch").val("");
                $("#SaveContent").hide();
                $("#btnApprove").hide();
                if ($(control).val() == "1")
                    $("#dvSearch").hide();
                else if ($(control).val() == "0")
                    $("#dvSearch").show();
            }
    </script>

    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
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
                        <label class="control-label required-field">Store Name</label>
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
                <div id="dvInventorySearchType" class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <label class="control-label required-field">Search Type</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlInventorySearchType" runat="server" CssClass="form-control" TabIndex="7" onchange="ChangeSearchLabel()">
                            <asp:ListItem Value="Category">Category</asp:ListItem>
                            <asp:ListItem Value="Item">Item</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="dvSearchType" class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <label class="control-label required-field">Search Type</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="form-control" TabIndex="8" onchange="ShowHideSearch(this)">
                            <asp:ListItem Value="0">Individual</asp:ListItem>
                            <asp:ListItem Value="1">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="dvSearch" class="form-group">
                    <div class="col-md-2">
                        <label id="lblTransaction" class="control-label required-field">Account Head</label>
                    </div>
                    <div class="col-md-10">
                        <input type="text" id="txtSearch" class="form-control" tabindex="9" />
                    </div>
                </div>
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
