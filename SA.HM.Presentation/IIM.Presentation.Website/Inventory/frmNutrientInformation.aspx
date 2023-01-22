<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmNutrientInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmNutrientInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var vv = [];
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlSetupType").change(function () {
                if ($("#ContentPlaceHolder1_ddlSetupType").val() == "0") {
                    $("#EntryPanel").hide();
                }
                else if ($("#ContentPlaceHolder1_ddlSetupType").val() == "1") {
                    $("#EntryPanel").show();
                    $("#nutritionTypeTextDiv").show();
                    $("#nutrientInfoTextDiv").hide();
                    $("#nutritionTypeList").hide();
                }
                else if ($("#ContentPlaceHolder1_ddlSetupType").val() == "2") {
                    $("#EntryPanel").show();
                    $("#nutrientInfoTextDiv").show();
                    $("#nutritionTypeTextDiv").hide();
                    $("#nutritionTypeList").show();
                }
            });

            $("#ContentPlaceHolder1_ddlSetupTypeSearch").change(function () {
                if ($("#ContentPlaceHolder1_ddlSetupTypeSearch").val() == "0") {
                    $("#SearchEntry").hide();
                }
                else if ($("#ContentPlaceHolder1_ddlSetupTypeSearch").val() == "1") {
                    $("#SearchEntry").show();
                    $("#nutritionTypeTextDivSearch").show();
                    $("#nutrientInfoTextDivSearch").hide();
                    $("#nutritionTypeListSearch").hide();
                }
                else if ($("#ContentPlaceHolder1_ddlSetupTypeSearch").val() == "2") {
                    $("#SearchEntry").show();
                    $("#nutrientInfoTextDivSearch").show();
                    $("#nutritionTypeTextDivSearch").hide();
                    $("#nutritionTypeListSearch").show();
                }
            });

            $("#ContentPlaceHolder1_ddlNutritionType").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlNutritionTypeSearch").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Bank Name</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            

            $("#ContentPlaceHolder1_ddlBankAccounts").change(function () {
                if ($("#ContentPlaceHolder1_ddlBankAccounts").val() != "0") {
                    $("#ContentPlaceHolder1_txtBankName").val($("#ContentPlaceHolder1_ddlBankAccounts option:selected").text());
                }
            });

            if ($("#ContentPlaceHolder1_hfIsBankIntegratedWithAccounts").val() == '1') {

                $("#AccountForBank").show();
            }
            else {
                $("#AccountForBank").hide();
                <%--$("#<%=ddlBankAccounts.ClientID %>").val(0);--%>
            }

            $("#SearchPanel").hide();

            $("#gvGustIngormation").delegate("td > img.BankDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var bankId = $.trim($(this).parent().parent().find("td:eq(4)").text());
                    var params = JSON.stringify({ sEmpId: bankId });

                    var $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/HMCommon/frmBank.aspx/DeleteData",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            CommonHelper.AlertMessage(data.d.AlertMessage);
                            $row.remove();
                            $("#myTabs").tabs('load', 1);
                        },
                        error: function (error) {
                        }
                    });
                }
            });

            $("#ContentPlaceHolder1_txtDisplaySequence").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    return false;
                }
            });
        });

        function ValidationBeforeSave() {
            var setupTypeId = $("#ContentPlaceHolder1_ddlSetupType option:selected").val();
            var setupType = $("#ContentPlaceHolder1_ddlSetupType option:selected").text();
            var nutritionTypeId = $("#ContentPlaceHolder1_ddlNutritionType option:selected").val();
            var nutritionType = $("#ContentPlaceHolder1_ddlNutritionType option:selected").text();
            var code = $("#ContentPlaceHolder1_txtCode").val();
            var name = $("#ContentPlaceHolder1_txtName").val();
            var remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            var displaySequence = $("#ContentPlaceHolder1_txtDisplaySequence").val();
            var statusId = $("#ContentPlaceHolder1_ddlActiveStat option:selected").val();
            var status = $("#ContentPlaceHolder1_ddlActiveStat option:selected").text();
            if (statusId == 1) {
                statusId = 0;
            }
            else {
                statusId = 1;
            }

            if ($("#ContentPlaceHolder1_ddlSetupType").val() == "0") {
                toastr.warning("Please Select Setup Type.");
                return false;
            }
            if (setupTypeId == "2") {
                if ($("#ContentPlaceHolder1_ddlNutritionType").val() == "0") {
                    toastr.warning("Please Select Nuitrition Type.");
                    return false;
                }
            }
            if (code == "") {
                toastr.warning("Please Provide Code.");
                return false;
            }
            else if (name == "") {
                toastr.warning("Please Provide Name.");
                return false;
            }
            var nutrientId = 0, isEdit = false;
            if ($("#ContentPlaceHolder1_hfEditId").val() == 1) {
                isEdit = true;
                if (setupTypeId == "1") {
                    nutritionTypeId = $("#ContentPlaceHolder1_hfNutritionTypeId").val();
                }
                else if (setupTypeId == "2") {
                    nutrientId = $("#ContentPlaceHolder1_hfNutrientInfoId").val();
                }
            }
            
            
            var NutritionTypeInfo = {
                NutritionTypeId: nutritionTypeId,
                Code: code,
                Name: name,
                Remarks: remarks,
                ActiveStat: Boolean(statusId),
                IsEdit: isEdit,
                DisplaySequence: displaySequence
            }

            var NutrientInfo = {
                NutrientId: nutrientId,
                NutritionTypeId: nutritionTypeId,
                Code: code,
                Name: name,
                Remarks: remarks,
                ActiveStat: Boolean(statusId),
                IsEdit: isEdit,
                DisplaySequence: displaySequence
            }
            
            if (setupTypeId == "1") {
                PageMethods.SaveNutritionTyepInfo(NutritionTypeInfo, OnSaveNutritionTyepInfoSucceeded, OnSaveNutritionTyepInfoFailed);
            }
            else if (setupTypeId == "2") {
                PageMethods.SaveNutrientInfo(NutrientInfo, OnSaveNutrientInfoSucceeded, OnSaveNutrientInfoFailed);
            }
            return false;
        }
        function OnSaveNutritionTyepInfoSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                $("#btnSave").val("Save");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnSaveNutritionTyepInfoFailed(error) {
            toastr.error(error.get_message());
        }
        function OnSaveNutrientInfoSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                $("#btnSave").val("Save");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnSaveNutrientInfoFailed(error) {
            toastr.error(error.get_message());
        }

        function SearchNutrientInformation(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#NutrientInformationGrid tbody tr").length;
            var setupTypeId = $("#ContentPlaceHolder1_ddlSetupTypeSearch").val();
            var nutritionTypeId = $("#ContentPlaceHolder1_ddlNutritionTypeSearch").val();
            var code = $("#ContentPlaceHolder1_txtSCode").val();
            var name = $("#ContentPlaceHolder1_txtSName").val();
            var statusId = $("#ContentPlaceHolder1_ddlSActiveStat").val();
            if (statusId == 0) {
                statusId = 1;
            }
            else {
                statusId = 0;
            }

            var nutrientInfo = {
                SetupTypeId: setupTypeId,
                NutritionTypeId: nutritionTypeId,
                Code: code,
                Name: name,
                ActiveStat: Boolean(statusId)
            }

            $("#GridPagingContainer ul").html("");
            $("#NutrientInformationGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchNutrientInformation(nutrientInfo, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchNutrientInformationSucceed, OnSearchNutrientInformationFailed);

            return false;
        }
        function OnSearchNutrientInformationSucceed(result) {

            var tr = "";
            $.each(result.GridData, function (count, gridObject) {
                var activeStat = "InActive";
                if (gridObject.ActiveStat) {
                    activeStat = "Active";
                }
                tr += "<tr>";

                tr += "<td style='width:20%;'>" + gridObject.Code + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.Name + "</td>";
                tr += "<td style='width:30%;'>" + gridObject.Remarks + "</td>";
                tr += "<td style='width:20%;'>" + activeStat + "</td>";

                tr += "<td style=\"text-align: center; width:10%; cursor:pointer;\">";


                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return NutrientInfoEditWithConfirmation('" + gridObject.NutrientId + "','" + gridObject.NutritionTypeId + "')\" alt='Edit'  title='Edit' border='0' />";

                tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return NutrientInfoDeleteWithConfirmation('" + gridObject.NutrientId + "','" + gridObject.NutritionTypeId + "')\" alt='Delete'  title='Delete' border='0' />";

                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.NutrientId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.NutritionTypeId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.CreatedBy + "</td>";

                tr += "</tr>";

                $("#NutrientInformationGrid tbody").append(tr);

                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSearchNutrientInformationFailed() {

        }

        function ClearSearch() {
            $("#ContentPlaceHolder1_ddlNutritionTypeSearch").val(0).trigger('change');
            $("#ContentPlaceHolder1_txtSCode").val("");
            $("#ContentPlaceHolder1_txtSName").val("");
            $("#ContentPlaceHolder1_ddlSActiveStat").val(0).trigger('change');
        }

        function NutrientInfoDeleteWithConfirmation(NutrientId, NutritionTypeId) {
            if (!confirm("Do you Want To Delete?")) {
                return false;
            }
            NutrientInfoDelete(NutrientId, NutritionTypeId);
        }
        function NutrientInfoDelete(NutrientId, NutritionTypeId) {
            PageMethods.NutrientInfoDelete(NutrientId, NutritionTypeId, OnNutrientInfoDeleteSucceed, OnNutrientInfoDeleteFailed);
            return false;
        }
        function OnNutrientInfoDeleteSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchNutrientInformation(1, 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnNutrientInfoDeleteFailed() {
            toastr.error(error.get_message());
        }

        function NutrientInfoEditWithConfirmation(NutrientId, NutritionTypeId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            NutrientInfoEdit(NutrientId, NutritionTypeId);
        }
        function NutrientInfoEdit(NutrientId, NutritionTypeId) {

            PageMethods.NutrientInfoEdit(NutrientId, NutritionTypeId, OnNutrientInfoEditSucceed, OnNutrientInfoEditFailed);
            return false;
        }
        function OnNutrientInfoEditSucceed(result) {

            $("#myTabs").tabs({ active: 0 });
            $("#btnSave").val("Update");
            $("#ContentPlaceHolder1_hfEditId").val(1);
            var status = result.ActiveStat == true ? 0 : 1;
            if (result.NutrientId == 0) {
                $("#ContentPlaceHolder1_ddlSetupType").val(1).trigger('change');
                $("#EntryPanel").show();
                $("#nutritionTypeTextDiv").show();
                $("#nutrientInfoTextDiv").hide();
                $("#nutritionTypeList").hide();
            }
            else if (result.NutrientId > 0) {
                $("#ContentPlaceHolder1_ddlSetupType").val(2).trigger('change');
                $("#EntryPanel").show();
                $("#nutrientInfoTextDiv").show();
                $("#nutritionTypeTextDiv").hide();
                $("#nutritionTypeList").show();
                $("#ContentPlaceHolder1_ddlNutritionType").val(result.NutritionTypeId).trigger('change');
            }

            $("#ContentPlaceHolder1_hfNutritionTypeId").val(result.NutritionTypeId);
            $("#ContentPlaceHolder1_hfNutrientInfoId").val(result.NutrientId);
            $("#ContentPlaceHolder1_txtCode").val(result.Code);
            $("#ContentPlaceHolder1_txtName").val(result.Name);
            $("#ContentPlaceHolder1_txtRemarks").val(result.Remarks);
            $("#ContentPlaceHolder1_txtDisplaySequence").val(result.DisplaySequence);
            $("#ContentPlaceHolder1_ddlActiveStat").val(status).trigger('change');
        }
        function OnNutrientInfoEditFailed() { }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $('#EntryPanel').show("slow");
            return false;
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For Delete-------------------------        
        function PerformDeleteAction(actionId, rowIndex) {
            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
        }
        function OnDeleteObjectFailed(error) {
            //alert(error.get_message());
            toastr.error(error);
        }
        //For ClearForm-------------------------

        function PerformClearActionWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfNutritionTypeId").val(0);
            $("#ContentPlaceHolder1_hfNutrientInfoId").val(0);
            $("#ContentPlaceHolder1_hfEditId").val(0);
            $("#ContentPlaceHolder1_ddlSetupType").val(0).trigger('change');
            $("#ContentPlaceHolder1_ddlNutritionType").val(0).trigger('change');
            $("#ContentPlaceHolder1_txtCode").val("");
            $("#ContentPlaceHolder1_txtName").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_txtDisplaySequence").val("");
            $("#ContentPlaceHolder1_ddlActiveStat").val(0).trigger('change');
            return false;
        }
        $(function () {
            $("#myTabs").tabs();
        });


        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();
            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function PerformEditAction(bankId) {
            PageMethods.LoadDetailInformation(bankId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            return false;
        }
        function OnLoadDetailObjectSucceeded(result) {

            <%--$("#<%=ddlBankAccounts.ClientID %>").val(result.BankHeadId).trigger('change');

            $("#<%=txtBankName.ClientID %>").val(result.BankName);--%>

            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            <%--$("#<%=txtBankId.ClientID %>").val(result.BankId);--%>
            $("#myTabs").tabs({ active: 0 });
            return false;
        }
        function OnLoadDetailObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchNutrientInformation(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
    </script>
    <asp:HiddenField ID="hfNutrientInfoId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfNutritionTypeId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfEditId" runat="server" Value="0"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Nutrient Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Nutrient Information</a></li>
        </ul>
        <div id="tab-1">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSetupType" runat="server" class="control-label required-field" Text="Setup Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSetupType" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="1">Nutrition Type</asp:ListItem>
                            <asp:ListItem Value="2">Nutrient Information</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div id="EntryPanel" class="panel panel-default" style="display: none;">
                <div class="panel-heading">
                    <div id="nutrientInfoTextDiv" style="display: none;">
                        Nutrient Information
                    </div>
                    <div id="nutritionTypeTextDiv" style="display: none;">
                        Nutrition Type
                    </div>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" id="nutritionTypeList" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblNutritionType" runat="server" class="control-label required-field" Text="Nutrition Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlNutritionType" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblCode" runat="server" class="control-label required-field" Text="Code"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDisplaySequence" runat="server" class="control-label" Text="Display Sequence"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDisplaySequence" CssClass="quantity form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input id="btnSave" type="button" value="Save" onclick="ValidationBeforeSave()"
                                    class="TransactionalButton btn btn-primary btn-sm" />
                                <input id="btnCancelTicket" type="button" value="Cancel" onclick="PerformClearActionWithConfirmation()"
                                    class="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSetupTypeSearch" runat="server" class="control-label required-field" Text="Setup Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSetupTypeSearch" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="1">Nutrition Type</asp:ListItem>
                            <asp:ListItem Value="2">Nutrient Information</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div id="SearchEntry" class="panel panel-default" style="display: none;">
                <div class="panel-heading">
                    <div id="nutrientInfoTextDivSearch" style="display: none;">
                        Nutrient Information
                    </div>
                    <div id="nutritionTypeTextDivSearch" style="display: none;">
                        Nutrition Type
                    </div>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" id="nutritionTypeListSearch" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblNutritionTypeSearch" runat="server" class="control-label required-field" Text="Nutrition Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlNutritionTypeSearch" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSName" runat="server" class="control-label" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSCode" runat="server" class="control-label" Text="Code"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSCode" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="SearchNutrientInformation(1, 1)" />
                                <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="ClearSearch()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Div1" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <table id="NutrientInformationGrid" class="table table-bordered table-condensed table-responsive" width="100%">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 20%;">Code
                                </th>
                                <th style="width: 20%;">Name
                                </th>
                                <th style="width: 30%;">Description
                                </th>
                                <th style="width: 20%;">ActiveStat
                                </th>
                                <th style="width: 10%;">Action
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="childDivSection">
                        <div class="text-center" id="GridPagingContainer">
                            <ul class="pagination">
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="divClear">
    </div>
</asp:Content>