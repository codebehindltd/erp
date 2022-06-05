<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="Depreciation.aspx.cs" Inherits="HotelManagement.Presentation.Website.FixedAsset.Depreciation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var TransactionNode = null;
        var Depreciation = null;
        $(document).ready(function () {
            $("#txtSearch").autocomplete({

                source: function (request, response) {
                    let url = 'Depreciation.aspx/AutoCompleteTransactionNode';
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
                        data: JSON.stringify({ searchText: request.term }),
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
            $("#ContentPlaceHolder1_ddlFiscalYear").change(function () {
                $("#SaveContent").hide();
                $("#depressionTable").html("");
            });
        });
        function PopulateProjects(control) {
            $("#SaveContent").hide();
            $("#depressionTable").html("");
            let companyId = $(control).val();

            $("#ContentPlaceHolder1_ddlProject").empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "./Depreciation.aspx/GetGLProjectByGLCompanyId",
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
                $("#depressionTable").html("");
                $('#<%=ddlFiscalYear.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');

                $.ajax({
                    type: "POST",
                    url: "./Depreciation.aspx/PopulateFiscalYear",
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
                });
                }
                function Search() {

                    var companyId = $("#ContentPlaceHolder1_ddlCompany").val();
                    var projectId = $("#ContentPlaceHolder1_ddlProject").val();
                    var fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYear").val();
                    var transactionNodeId = 0;
                    var hierarchy = 0;

                    if (projectId == "0") {
                        toastr.warning("Please Select Fiscal Year.");
                        $("#ContentPlaceHolder1_ddlProject").focus();
                        return false;
                    }
                    if (fiscalYearId == "0") {
                        toastr.warning("Please Select Fiscal Year.");
                        $("#ContentPlaceHolder1_ddlFiscalYear").focus();
                        return false;
                    }

                    if (TransactionNode == null) {
                        var transactionHeadName = $("#lblTransaction").text();
                        toastr.warning(`Please Search ${transactionHeadName}`);
                        $("#txtSearch").focus();
                        return false;
                    }

                    transactionNodeId = TransactionNode.NodeId;
                    hierarchy = TransactionNode.Hierarchy;

                    PageMethods.FillForm(companyId, projectId, fiscalYearId, transactionNodeId, hierarchy, OnSucceedResult, OnError);
                    return false;
                }

                function OnSucceedResult(result) {
                    DepreciationDetails = result.DepreciationDetailsBOList;
                    Depreciation = result.DepreciationBO;

                    if (DepreciationDetails.length > 0) {
                        //$("#btnApprove").show();
                        $("#btnSave").val("Update");
                    }
                    else {
                        // $("#btnApprove").hide();
                        $("#btnSave").val("Save");
                    }
                    $("#tblSearchItem").html(result.DepreciationTableString);
                    $("#SaveContent").show();

                    CommonHelper.ApplyDecimalValidationWithNegetiveValue();

                    return false;
                }

                function OnError(error) {

                    CommonHelper.AlertMessage(error.AlertMessage);
                }
                function SaveDepreciation() {

                    var companyId = $("#ContentPlaceHolder1_ddlCompany").val();
                    var projectId = $("#ContentPlaceHolder1_ddlProject").val();
                    var fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYear").val();

                    if (companyId == "0") {
                        toastr.warning("Please Select Company.");
                        $("#ContentPlaceHolder1_ddlCompany").focus();
                        return false;
                    }
                    if (projectId == "0") {
                        toastr.warning("Please Select Fiscal Year.");
                        $("#ContentPlaceHolder1_ddlProject").focus();
                        return false;
                    }
                    if (fiscalYearId == "0") {
                        toastr.warning("Please Select Fiscal Year.");
                        $("#ContentPlaceHolder1_ddlFiscalYear").focus();
                        return false;
                    }

                    var FADepreciationDetailsBO = new Array();

                    var transactionNodeId = 0, id = 0, detailsId = 0, DepreciationPercentage = 0;

                    if (Depreciation != null) {
                        id = Depreciation.Id;
                    }

                    var FADepreciationBO = {
                        Id: id,
                        CompanyId: companyId,
                        ProjectId: projectId,
                        FiscalYearId: fiscalYearId,
                        AccountHeadId: TransactionNode.NodeId
                    };

                    $("#depressionTable tbody tr").each(function () {

                        transactionNodeId = $(this).find("td:eq(0)").attr("tnid");
                        detailsId = $(this).find("td:eq(0)").attr("did");

                        DepreciationPercentage = $(this).find("td:eq(2) input").val();

                        if (DepreciationPercentage != "") {

                            FADepreciationDetailsBO.push({
                                Id: parseInt(detailsId),
                                TransactionNodeId: parseInt(transactionNodeId),
                                DepreciationPercentage: DepreciationPercentage != "" ? parseFloat(DepreciationPercentage).toFixed(2) : 0.00,
                            });
                        }
                    });
                    if (FADepreciationDetailsBO.length == 0) {
                        return false;
                    }
                    PageMethods.SaveDepreciation(FADepreciationBO, FADepreciationDetailsBO, OnSucceedSaveDepreciation, OnFailedSaveDepreciation);

                }
                function OnSucceedSaveDepreciation(result) {
                    CommonHelper.AlertMessage(result.AlertMessage);
                    if (result.IsSuccess) {
                        //ClearAll();
                    }
                }

                function OnFailedSaveDepreciation(error) {

                    CommonHelper.AlertMessage(error.AlertMessage);
                }
                function ClearAll() {
                    TransactionNode = null;
                    Depreciation = null;
                    $("#ContentPlaceHolder1_ddlCompany").val("0");
                    $("#ContentPlaceHolder1_ddlProject").val("0");
                    $("#ContentPlaceHolder1_ddlFiscalYear").val("0");
                    $("#depressionTable").html("");
                    $("#txtSearch").val("");
                    $("#SaveContent").hide();
                }
                function CheckInputValue(control) {
                    var tableRow = $(control).parent().parent();

                    var DepreciationPercentage = tableRow.find("td:eq(2) input").val() != "" ? parseFloat(tableRow.find("td:eq(2) input").val()) : 0.00;
                    if (DepreciationPercentage >= 100) {
                        $(control).val("");
                        $(control).focus();
                        toastr.warning("Depreciation Percentage greater than or equal to 100");
                        return false;
                    }
                    return true;
                }

    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Company</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" TabIndex="2" onchange="PopulateProjects(this)">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Project</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" TabIndex="3" onchange="PopulateFiscalYear(this)">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Fiscal Year</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control" TabIndex="4">
                        </asp:DropDownList>
                    </div>

                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label id="lblTransaction" class="control-label required-field">Account Head</label>
                    </div>
                    <div class="col-md-10">
                        <input type="text" id="txtSearch" class="form-control" tabindex="8" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12" id="divGenerate">
                        <input type="button" value="Search" class="TransactionalButton btn btn-primary btn-sm" onclick="Search()" tabindex="9" />
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
                    <input id="btnSave" type="button" value="Save" class="TransactionalButton btn btn-primary btn-sm" onclick="SaveDepreciation()" tabindex="10" />
                    <%--<input style="display: none;" id="btnApprove" type="button" value="Approve" class="TransactionalButton btn btn-primary btn-sm" onclick="ApproveBalance()" tabindex="11" />--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
