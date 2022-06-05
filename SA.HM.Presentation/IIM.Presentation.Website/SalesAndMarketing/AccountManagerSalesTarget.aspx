<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="AccountManagerSalesTarget.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.AccountManagerSalesTarget" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var Budget = null;
        var AccountsHead = null;
        var FiscalYear = new Array();

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>General Ledger</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#SaveContent").hide();

            if ($("#ContentPlaceHolder1_hfFiscalYear").val() != "") {
                FiscalYear = JSON.parse($("#ContentPlaceHolder1_hfFiscalYear").val());
                //$("#ContentPlaceHolder1_hfFiscalYear").val("");                
            }

            $("#txtSearch").autocomplete({

                source: function (request, response) {

                    var groupRIndividualSearch = $("#ContentPlaceHolder1_ddlCoaType").val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: 'SalesAndMarketing.aspx/GetAutoCompleteData',
                        data: "{'searchText':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.NodeHead,
                                    value: m.NodeId,
                                    NodeId: m.NodeId,
                                    Lvl: m.Lvl,
                                    IsTransactionalHead: m.IsTransactionalHead,
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

                    AccountsHead = ui.item;
                    $("#ContentPlaceHolder1_hfAccountsHeadId").val(ui.item.value);
                }
            });

            $("#ContentPlaceHolder1_ddlFiscalYear").change(function () {
                var fy = _.findWhere(FiscalYear, { FiscalYearId: parseInt($(this).val()) });
                //toastr.info(fy.FiscalYearName);
            });

        });

        function GenerateForBudget() {
            var glCompanyId = $("#ContentPlaceHolder1_ddlCompany").val();
            var glProjectId = $("#ContentPlaceHolder1_ddlProject").val();
            var fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYear").val();
            //var nodeId = AccountsHead.NodeId;
            //var hierarchy = AccountsHead.Hierarchy;

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
            if (fiscalYearId == "0") {
                toastr.warning("Please Select Fiscal Year.");
                $("#ContentPlaceHolder1_ddlFiscalYear").focus();
                return false;
            }

            PageMethods.FillForm(glCompanyId, glProjectId, fiscalYearId, OnSucceedResult, OnError);
        }

        function OnSucceedResult(result) {
            Budget = result.salesTarget;
            $("#generatedTable").html(result.SalesTargetTable);
            $("#SaveContent").show();

            CommonHelper.ApplyDecimalValidation();

            return false;
        }

        function OnError() { }

        function SaveBudget() {
            var glCompanyId = $("#ContentPlaceHolder1_ddlCompany").val();
            var glProjectId = $("#ContentPlaceHolder1_ddlProject").val();
            var fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYear").val();

            if (fiscalYearId == "0") {
                toastr.warning("Please Select Fiscal Year.");
                return false;
            }

            var BudgetDetails = new Array();

            var rowCount = 0, nodeId = 0, budgetId = 0, detailsId = 0, amount = "";

            if (Budget != null) {
                budgetId = Budget.TargetId;
            }

            var BudgetNew = {
                TargetId: budgetId,
                CompanyId: glCompanyId,
                ProjectId: glProjectId,
                FiscalYearId: fiscalYearId,
                CheckedBy: 0,
                ApprovedBy: 0,
                ApprovedStatus: ''
            };

            $("#salesTargetTable tbody tr").each(function () {

                nodeId = $(this).find("td:eq(0)").attr("mid");

                $("#salesTargetTable tbody tr:eq(" + rowCount + ") td").each(function () {

                    detailsId = $(this).attr("did");

                    if ($(this).find("input").is(":input")) {

                        amount = $(this).find("input").val();

                        if (amount != "") {
                            BudgetDetails.push({
                                TargetDetailsId: detailsId,
                                TargetId: budgetId,
                                MonthId: $(this).attr("mid"),
                                AccountManagerId: nodeId,
                                Amount: amount
                            });
                        }

                        amount = "";
                    }
                });

                rowCount++;
            });

            PageMethods.SaveAccountManagerSalesTarget(BudgetNew, BudgetDetails, OnSucceedSaveBudget, OnErrorSucceedSaveBudget);
        }

        function OnSucceedSaveBudget(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ClearAll();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }

        function OnErrorSucceedSaveBudget() { }

        function ClearAll() {
            Budget = null;
            $("#generatedTable").html("");
            $("#SaveContent").hide();
        }

        function PopulateProjects(control) {
            $("#SaveContent").hide();
            $("#balanceTable").html("");
            let companyId = $(control).val();

            $("#ContentPlaceHolder1_ddlProject").empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "./AccountManagerSalesTarget.aspx/GetGLProjectByGLCompanyId",
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
            $('#<%=ddlFiscalYear.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "./AccountManagerSalesTarget.aspx/PopulateFiscalYear",
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

    </script>
    <asp:HiddenField ID="hfAccountsHead" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfFiscalYear" runat="server" Value=""></asp:HiddenField>
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
                        <asp:Label ID="lblFiscalYear" runat="server" class="control-label required-field"
                            Text="Fiscal Year"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12" id="divGenerate">
                        <input type="button" value="Generate" class="TransactionalButton btn btn-primary btn-sm" onclick="GenerateForBudget()" />
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div style="overflow-x: scroll;">
                        <div id="generatedTable" style="width: 1200px;">
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="row" id="SaveContent">
                <div class="col-md-12">
                    <input type="button" value="Save Target" class="TransactionalButton btn btn-primary btn-sm" onclick="SaveBudget()" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
