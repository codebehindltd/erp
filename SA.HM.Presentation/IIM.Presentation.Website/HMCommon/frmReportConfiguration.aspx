<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmReportConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var ReportConfigMasterSearchList = new Array();
        var ReportConfigDetailsList = new Array();
        var editedItem = "";
        var DeletedConfigDetails = new Array();

        $(document).ready(function () {

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Staffing Budget</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#myTabs").tabs();
            CommonHelper.ApplyIntigerValidation();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlReportTypeForDetails").change(function () {
                ClearDetailsBeforeLoad();
                LoadCaptionGroup();
            });

            $("#ContentPlaceHolder1_ddlReportsType").change(function () {
                LoadParentGroup();
            });

            $("#ContentPlaceHolder1_ddlCaptionGroup").change(function () {
                ClearDetailsBeforeLoad();
                LoadChildGroup();
            });

            $("#ContentPlaceHolder1_txtAccountHead").autocomplete({

                source: function (request, response) {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: 'frmReportConfiguration.aspx/GetChartOfAccounts',
                        data: "{'searchText':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.NodeHead,
                                    value: m.NodeId,
                                    Lvl: m.Lvl,
                                    IsTransactionalHead: m.IsTransactionalHead
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
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);

                    $("#ContentPlaceHolder1_hfAccountsHeadId").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfIsTransactionalHead").val((ui.item.IsTransactionalHead == null ? "0" : "1"));

                    GetNodeMatrixInfoByAncestorNodeId();
                }
            });
        });

        function ClearDetailsBeforeLoad() {
            $("#TblChartOfAccounts tbody tr").remove();
            $("#TblReportConfigDetails tbody tr").remove();
            $("#ContentPlaceHolder1_txtAccountHead").val("");
            $("#ContentPlaceHolder1_hfAccountsHeadId").val("0");
            $("#ContentPlaceHolder1_hfIsTransactionalHead").val("0");
            $("#ContentPlaceHolder1_hfReportConfigId").val("");
            $("#ContentPlaceHolder1_hfReportTypeId").val("");
            $("#btnSaveDetails").val("Save");
        }

        function SaveUpdateReportGroup() {
            if ($("#ContentPlaceHolder1_ddlReportsType").val() == "0") {
                toastr.info("Please Select Report Type.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtCaption").val() == "") {
                toastr.info("Please Give Caption.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtSortingOrder").val() == "" || $("#ContentPlaceHolder1_txtSortingOrder").val() == "0") {
                toastr.info("Please Give Sorting Order.");
                return false;
            }

            var reportConfigId = $("#ContentPlaceHolder1_hfId").val();
            var reportsType = $("#ContentPlaceHolder1_ddlReportsType").val();
            var caption = $("#ContentPlaceHolder1_txtCaption").val();
            var captionType = $("#ContentPlaceHolder1_ddlCaptionType").val();
            var sortingOrder = $("#ContentPlaceHolder1_txtSortingOrder").val();
            var ancestorId = $("#ContentPlaceHolder1_ddlParentCaption").val();
            var isParent = $("#ContentPlaceHolder1_chkIsParentCaption").is(":checked");

            if (reportConfigId == "")
                reportConfigId = "0";

            if (ancestorId == "0")
                ancestorId = null;

            if (captionType == "0")
                captionType = null;

            var ReportConfigMaster = {
                Id: reportConfigId,
                ReportTypeId: reportsType,
                AncestorId: ancestorId,
                Caption: caption,
                SortingOrder: sortingOrder,
                NodeType: captionType,
                IsParent: isParent
            };

            PageMethods.SaveReportConfigMaster(ReportConfigMaster, OnSaveReportConfigMasterSucceed, OnSaveReportConfigMasterFailed);
            return false;
        }
        function OnSaveReportConfigMasterSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearActionReportMaster();
                LoadParentGroup();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSaveReportConfigMasterFailed(error) {
            toastr.info("Please Contact With Admin.");
        }

        function PerformClearActionReportMaster() {
            $("#ContentPlaceHolder1_txtCaption").val("");
            $("#ContentPlaceHolder1_txtSortingOrder").val("");
            $("#ContentPlaceHolder1_ddlCaptionType").val("0");
            $("#ContentPlaceHolder1_chkIsParentCaption").prop("checked", false);
            $("#ContentPlaceHolder1_btnSave").val("Save Report Group");
        }

        function LoadCaptionGroup() {
            var reportTypeId = $("#ContentPlaceHolder1_ddlReportTypeForDetails").val();
            PageMethods.LoadReportGroup(reportTypeId, OnLoadCaptionGroupSucceeded, OnLoadCaptionGroupFailed);
            return false;
        }

        function OnLoadCaptionGroupSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlCaptionGroup');
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + '---Please Select---' + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Caption + '" value="' + list[i].Id + '">' + list[i].Caption + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + '---Please Select---' + '</option>');
                }

                if ($("#ContentPlaceHolder1_hfReportConfigId").val() != "") {
                    $(control).val($("#ContentPlaceHolder1_hfReportConfigId").val());
                }
            }
        }
        function OnLoadCaptionGroupFailed(error) {
        }

        function LoadParentGroup() {
            var reportTypeId = $("#ContentPlaceHolder1_ddlReportsType").val();
            PageMethods.LoadParentGroup(reportTypeId, OnLoadParentGroupSucceeded, OnLoadParentGroupFailed);
            return false;
        }
        function OnLoadParentGroupSucceeded(result) {
            var list = result;
            var controlId = 'ContentPlaceHolder1_ddlParentCaption';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + '---Please Select---' + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Caption + '" value="' + list[i].Id + '">' + list[i].Caption + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + '---Please Select---' + '</option>');
                }
            }

            if ($("#ContentPlaceHolder1_hfParentCaptionId").val() != "") {
                $("#ContentPlaceHolder1_ddlParentCaption").val($("#ContentPlaceHolder1_hfParentCaptionId").val());
            }
        }
        function OnLoadParentGroupFailed(error) {
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            if ($("#ContentPlaceHolder1_ddlSearchReportType").val() == "") {
                toastr.info("Please Select Report Type.");
                return false;
            }

            var gridRecordsCount = $("#TblConfigReport tbody tr").length;
            var reportTypeId = $("#ContentPlaceHolder1_ddlSearchReportType").val();
            var searchType = $("#ContentPlaceHolder1_ddlSearchType").val();
            $("#TblConfigReport tbody tr").remove();
            $("#GridPagingContainer ul").html("");
            PageMethods.GetReportConfigBySearchCriteria(searchType, reportTypeId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadSearchSucceeded, OnLoadSearchFailed);

            return false;
        }

        function OnLoadSearchSucceeded(result) {
            var tr = "", totalRow = 0, editLink = "", deleteLink = "";
            var searchType = $("#ContentPlaceHolder1_ddlSearchType").val();

            ReportConfigMasterSearchList = result.GridData;

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style=\"width:35%;\">" + gridObject.Caption + "</td>";
                tr += "<td style=\"width:20%;\">" + gridObject.SortingOrder + "</td>";

                if (gridObject.NodeType != null && gridObject.NodeType != "0") {
                    tr += "<td style=\"width:30%;\">" + gridObject.NodeType + "</td>";
                }
                else {
                    tr += "<td style=\"width:30%;\"></td>";
                }

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";

                if (searchType == "Caption") {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png'  onClick= \"javascript:return EditReportConfigMaster(" + gridObject.Id + "," + gridObject.ReportTypeId + "," + gridObject.AncestorId + ")\" alt='Edit' title='Edit' border='0' />";
                }
                else if (searchType == "Details") {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png'  onClick= \"javascript:return EditReportConfigDetails(" + gridObject.Id + "," + gridObject.ReportTypeId + ")\" alt='Edit' title='Edit' border='0' />";
                }

                tr += "</td>";

                tr += "<td align='right' style=\"display:none;\">" + gridObject.Id + "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.ReportTypeId + "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.AncestorId + "</td>";

                tr += "</tr>"

                $("#TblConfigReport tbody").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
        }

        function OnLoadSearchFailed(error) {
        }

        function EditReportConfigMaster(id, reportTypeId, ancestorId) {
            var rpt = _.findWhere(ReportConfigMasterSearchList, { Id: parseInt(id, 10) });

            $("#ContentPlaceHolder1_hfId").val(rpt.Id);
            $("#ContentPlaceHolder1_hfParentCaptionId").val(rpt.AncestorId);
            PageMethods.LoadParentGroup(rpt.ReportTypeId, OnLoadParentGroupSucceeded, OnLoadParentGroupFailed);

            $("#ContentPlaceHolder1_ddlReportsType").val(rpt.ReportTypeId + "");
            $("#ContentPlaceHolder1_ddlParentCaption").val(rpt.AncestorId + "");
            $("#ContentPlaceHolder1_txtCaption").val(rpt.Caption);

            if (rpt.NodeType != null && rpt.NodeType != 0)
                $("#ContentPlaceHolder1_ddlCaptionType").val(rpt.NodeType);
            else
                $("#ContentPlaceHolder1_ddlCaptionType").val("0");

            $("#ContentPlaceHolder1_txtSortingOrder").val(rpt.SortingOrder);

            if (rpt.IsParent)
                $("#ContentPlaceHolder1_chkIsParentCaption").prop("checked", true);
            else
                $("#ContentPlaceHolder1_chkIsParentCaption").prop("checked", false);

            $("#ContentPlaceHolder1_btnSave").val("Update Report Group");
            $("#myTabs").tabs({ active: 0 });
        }

        function LoadChildGroup() {
            $("#TblReportConfigDetails tbody tr").remove();

            var reportTypeId = $("#ContentPlaceHolder1_ddlReportTypeForDetails").val();
            var reportConfigId = $("#ContentPlaceHolder1_ddlCaptionGroup").val();

            PageMethods.GetReportConfigDetailsByReportTypeAndConfigId(reportConfigId, reportTypeId, OnLoadDetailsNodeSucceeded, OnLoadDetailsNodeFailed);
            return false;
        }

        function OnLoadDetailsNodeSucceeded(result) {
            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            ReportConfigDetailsList = result;

            if (result.length > 0)
                $("#btnSaveDetails").val("Update");

            $.each(result, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style=\"width:60%;\">" + gridObject.NodeName + "</td>";

                if (gridObject.NodeType != null && gridObject.NodeType != "0") {
                    tr += "<td style=\"width:20%;\">" + gridObject.NodeType + "</td>";
                }
                else {
                    tr += "<td style=\"width:20%;\"></td>";
                }
                tr += "<td style=\"width:10%;\">";
                tr += "<input type=\"text\" id=\"s" + gridObject.Id + "\"class=\"form-control quantity\" onblur=\"ChangeSortingOrder(this, " + gridObject.ReportConfigId + "," + gridObject.ReportTypeId + "," + gridObject.Id + "," + gridObject.NodeId + ")\" value=\"" + gridObject.SortingOrder + "\"/>";
                tr += "</td>";

                tr += "<td style=\"text-align: center; width:10%; cursor:pointer;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/delete.png'  onClick= \"javascript:return DeleteReportConfigDetails(this," + gridObject.ReportConfigId + "," + gridObject.ReportTypeId + "," + gridObject.Id + "," + gridObject.NodeId + ")\" alt='Edit' title='Edit' border='0' />";
                tr += "</td>";

                tr += "<td align='right' style=\"display:none;\">" + gridObject.ReportConfigId + "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.ReportTypeId + "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.Id + "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.NodeId + "</td>";

                tr += "</tr>"

                $("#TblReportConfigDetails tbody").append(tr);
                tr = "";
            });

            CommonHelper.ApplyIntigerValidation();
        }

        function OnLoadDetailsNodeFailed(error) {
        }

        function EditReportConfigDetails(reportConfigId, reportTypeId) {
            $("#TblChartOfAccounts tbody tr").remove();
            $("#ContentPlaceHolder1_txtAccountHead").val("");
            $("#ContentPlaceHolder1_hfAccountsHeadId").val("0");
            $("#ContentPlaceHolder1_hfIsTransactionalHead").val("0");

            $("#ContentPlaceHolder1_hfReportConfigId").val(reportConfigId);
            $("#ContentPlaceHolder1_ddlReportTypeForDetails").val(reportTypeId);

            PageMethods.LoadReportGroup(reportTypeId, OnLoadCaptionGroupSucceeded, OnLoadCaptionGroupFailed);

            $("#TblReportConfigDetails tbody tr").remove();
            PageMethods.GetReportConfigDetailsByReportTypeAndConfigId(reportConfigId, reportTypeId, OnLoadDetailsNodeSucceeded, OnLoadDetailsNodeFailed);

            $("#btnSaveDetails").val("Update");
            $("#myTabs").tabs({ active: 1 });

            return false;
        }
        function OnSucceedFillForm(result) {

        }
        function OnErrorFillForm(error) {
        }

        function GetNodeMatrixInfoByAncestorNodeId() {
            $("#TblChartOfAccounts tbody tr").remove();
            accountsHeadId = $("#ContentPlaceHolder1_hfAccountsHeadId").val();
            isTransactionalHead = $("#ContentPlaceHolder1_hfIsTransactionalHead").val() == "1" ? true : false;

            PageMethods.GetNodeMatrixInfoByAncestorNodeId(accountsHeadId, isTransactionalHead, OnLoadAccountByAncestorIdSucceeded, OnLoadAccountByAncestorIdFailed);
        }
        function OnLoadAccountByAncestorIdSucceeded(result) {
            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style=\"width:8%;\">";
                tr += "<input type=\"checkbox\" class=\"form-control\" />";
                tr += "</td>";

                tr += "<td style=\"width:70%;\">" + gridObject.NodeHead + "</td>";
                tr += "<td style=\"width:22%;\">" + gridObject.NodeType + "</td>";

                tr += "<td align='right' style=\"display:none;\">" + gridObject.NodeId + "</td>";

                tr += "</tr>"

                $("#TblChartOfAccounts tbody").append(tr);
                tr = "";
            });
        }
        function OnLoadAccountByAncestorIdFailed(error) {

        }

        function CheckAllNode() {
            var accountCheckAll = $("#accountCheckAll").is(":checked");
            $("#TblChartOfAccounts tbody tr").find("td:eq(0)").find("input").prop("checked", accountCheckAll);
        }

        function AddReportDetails() {

            var tr = "", duplicateNode = "", nodeId = "0", nodeName = "", nodeType = "";

            var reportTypeId = $("#ContentPlaceHolder1_ddlReportTypeForDetails").val();
            var reportConfigId = $("#ContentPlaceHolder1_ddlCaptionGroup").val();

            if (reportTypeId == "0") {
                toastr.info("Please Select Report Type.");
                return false;
            }
            else if (reportConfigId == "") {
                toastr.info("Please Select Caption/Group To Added Details.");
                return false;
            }
            else if ($("#TblChartOfAccounts tbody tr").length == 0) {
                toastr.info("Please Search Account Head To added.");
                return false;
            }
            else if ($("#TblChartOfAccounts tbody tr").find("td:eq(0)").find("input").is(":checked") == false) {
                toastr.info("Please Checked Account Head To added.");
                return false;
            }

            $("#TblChartOfAccounts tbody tr").each(function () {

                if ($(this).find("td:eq(0)").find("input").is(":checked")) {

                    nodeId = $(this).find("td:eq(3)").text();
                    nodeName = $(this).find("td:eq(1)").text();
                    nodeType = $(this).find("td:eq(2)").text();

                    var obgDetails = _.findWhere(ReportConfigDetailsList, { NodeId: parseInt(nodeId, 10) });

                    if (obgDetails == null) {

                        tr += "<tr>";

                        tr += "<td style=\"width:60%;\">" + nodeName + "</td>";
                        tr += "<td style=\"width:20%;\">" + nodeType + "</td>";

                        tr += "<td style=\"width:10%;\">";
                        tr += "<input type=\"text\" id=\"s" + nodeId + "\"class=\"form-control quantity\" onblur=\"ChangeSortingOrder(this, " + reportConfigId + "," + reportTypeId + "," + 0 + "," + nodeId + ")\" value=\"\" />";
                        tr += "</td>";

                        tr += "<td style=\"text-align: center; width:10%; cursor:pointer;\">";
                        tr += "&nbsp;&nbsp;<img src='../Images/delete.png'  onClick= \"javascript:return DeleteReportConfigDetails(this," + reportConfigId + "," + reportTypeId + "," + 0 + "," + nodeId + ")\" alt='Edit' title='Edit' border='0' />";
                        tr += "</td>";

                        tr += "<td align='right' style=\"display:none;\">" + reportConfigId + "</td>";
                        tr += "<td align='right' style=\"display:none;\">" + reportTypeId + "</td>";
                        tr += "<td align='right' style=\"display:none;\">0</td>";
                        tr += "<td align='right' style=\"display:none;\">" + nodeId + "</td>";

                        tr += "</tr>"

                        $("#TblReportConfigDetails tbody").append(tr);

                        ReportConfigDetailsList.push({
                            ReportConfigId: parseInt(reportConfigId, 10),
                            ReportTypeId: parseInt(reportTypeId, 10),
                            Id: 0,
                            NodeId: parseInt(nodeId, 10),
                            NodeName: nodeName,
                            SortingOrder: 0,
                            NodeType: nodeType
                        });
                    }
                    else {
                        if (duplicateNode != "") {
                            duplicateNode += "," + nodeName;
                        }
                        else {
                            duplicateNode = nodeName;
                        }
                    }

                    tr = "";
                }

                CommonHelper.ApplyIntigerValidation();
            });

            $("#TblChartOfAccounts tbody tr").remove();
            $("#ContentPlaceHolder1_txtAccountHead").val("");
            $("#ContentPlaceHolder1_hfAccountsHeadId").val("0");
            $("#ContentPlaceHolder1_hfIsTransactionalHead").val("0");

            if (duplicateNode != "") {
                toastr.info("Duplicate Account Head Is Not Added. Duplicate Account Head Are : " + duplicateNode);
            }
        }

        function ChangeSortingOrder(control, reportConfigId, reportTypeId, detailsId, nodeId) {

            var sortingOrder = $.trim($(control).val());
            var obgDetails = _.findWhere(ReportConfigDetailsList, { NodeId: parseInt(nodeId, 10) });

            sortingOrder = sortingOrder == "" ? "0" : sortingOrder;
            var indexObj = _.indexOf(ReportConfigDetailsList, obgDetails);

            if (obgDetails != null) {
                ReportConfigDetailsList[indexObj].SortingOrder = parseInt(sortingOrder, 10);
            }
        }

        function DeleteReportConfigDetails(control, reportConfigId, reportTypeId, detailsId, nodeId) {

            if (parseInt(detailsId, 10) > 0) {
                DeletedConfigDetails.push({
                    ReportConfigId: parseInt(reportConfigId, 10),
                    ReportTypeId: parseInt(reportTypeId, 10),
                    Id: parseInt(detailsId, 10),
                    NodeId: parseInt(nodeId, 10)
                });
            }

            var obgDetails = _.findWhere(ReportConfigDetailsList, { NodeId: parseInt(nodeId, 10) });
            var indexObj = _.indexOf(ReportConfigDetailsList, obgDetails);
            ReportConfigDetailsList.splice(indexObj, 1);

            var tr = $(control).parent().parent();
            $(tr).remove();
        }

        function SaveReportDetails() {
            if ($("#ContentPlaceHolder1_ddlReportTypeForDetails").val() == "0") {
                toastr.info("Please Select Report Type.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlCaptionGroup").val() == "") {
                toastr.info("Please Select Caption/Group.");
                return false;
            }
            else if ($("#TblReportConfigDetails tbody tr").length == 0) {
                toastr.info("Please Add Account For Details To Save.");
                return false;
            }

            PageMethods.SaveReportConfigDetails(ReportConfigDetailsList, DeletedConfigDetails, OnSaveReportConfigDetailsSucceed, OnSaveReportConfigDetailsFailed);

            return false;
        }
        function OnSaveReportConfigDetailsSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearActionReportDetails();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSaveReportConfigDetailsFailed(error) {
            toastr.info("Please Contact With Admin.");
        }

        function PerformClearActionReportDetails() {
            $("#TblChartOfAccounts tbody tr").remove();
            $("#TblReportConfigDetails tbody tr").remove();
            $("#ContentPlaceHolder1_txtAccountHead").val("");
            $("#ContentPlaceHolder1_hfAccountsHeadId").val("0");
            $("#ContentPlaceHolder1_hfIsTransactionalHead").val("0");
            $("#ContentPlaceHolder1_ddlReportTypeForDetails").val("0");
            $("#ContentPlaceHolder1_ddlCaptionGroup").val("0");

            $("#ContentPlaceHolder1_hfReportConfigId").val("");
            $("#ContentPlaceHolder1_hfReportTypeId").val("");

            $("#btnSaveDetails").val("Save");
            $("#myTabs").tabs({ active: 1 });
        }

    </script>

    <asp:HiddenField ID="hfId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsCurrentOrPreviousPage" runat="server" Value="" />
    <asp:HiddenField ID="hfAccountsHeadId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsTransactionalHead" runat="server" Value="0" />
    <asp:HiddenField ID="hfParentCaptionId" runat="server" Value="" />
    <asp:HiddenField ID="hfReportConfigId" runat="server" Value="" />
    <asp:HiddenField ID="hfReportTypeId" runat="server" Value="" />

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="circularEntry" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-1">Report Group</a></li>
            <li id="Li1" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-2">Report Group Wise Child</a></li>
            <li id="circularSearch" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-3">Search Report Group/ Child</a></li>
        </ul>
        <div id="tab-1">
            <div id="" class="panel panel-default">
                <div class="panel-heading">
                    Report Group Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Report Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReportsType" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Parent Caption"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlParentCaption" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label7" runat="server" class="control-label required-field"
                                    Text="Caption"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCaption" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="Caption Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCaptionType" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0" Text="---Please Slect---"></asp:ListItem>
                                    <asp:ListItem Value="ProfitLoss" Text="Profit Loss"></asp:ListItem>
                                    <asp:ListItem Value="Revenue" Text="Revenue"></asp:ListItem>
                                    <asp:ListItem Value="GroupRevenue" Text="Group Revenue"></asp:ListItem>
                                    <asp:ListItem Value="GroupExpenditure" Text="Group Expenditure"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblNoOfVancancie" runat="server" class="control-label required-field"
                                    Text="Sorting Order"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSortingOrder" CssClass="form-control quantity" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label10" runat="server" class="control-label"
                                    Text="Is Parent Caption"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:CheckBox ID="chkIsParentCaption" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save Report Group" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="return SaveUpdateReportGroup();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Group Wise Child
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Report Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReportTypeForDetails" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field"
                                    Text="Caption/Group"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCaptionGroup" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Account Head"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAccountHead" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading">
                    Account Head
                </div>
                <div class="panel-body">
                    <table id="TblChartOfAccounts" class="table table-bordered table-condensed table-hover">
                        <thead>
                            <tr>
                                <th style="width: 8%;">
                                    <input id="accountCheckAll" type="checkbox" onclick="CheckAllNode()" class="form-control" />
                                </th>
                                <th style="width: 70%;">Account Head</th>
                                <th style="width: 22%;">Account Type</th>

                                <th style="display: none;">Node Id</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>

                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <input type="button" value="Add" id="btnAddAccounts" class="TransactionalButton btn btn-primary btn-sm" onclick="AddReportDetails()" />
                            </div>
                        </div>
                    </div>

                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading">
                    Already Added Details
                </div>
                <div class="panel-body">
                    <table id="TblReportConfigDetails" class="table table-bordered table-condensed table-hover">
                        <thead>
                            <tr>
                                <th style="width: 60%;">Account Head</th>
                                <th style="width: 20%;">Account Type</th>
                                <th style="width: 10%;">Sorting Order</th>
                                <th style="width: 10%;">Action</th>

                                <th style="display: none;">Report Config Id</th>
                                <th style="display: none;">Report Type Id</th>
                                <th style="display: none;">Details Id</th>
                                <th style="display: none;">Node Id</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>

                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <input type="submit" value="Save" id="btnSaveDetails" class="TransactionalButton btn btn-primary btn-sm" onclick="SaveReportDetails()" />
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div id="tab-3">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Report Configuration Search
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lbl101901" runat="server" class="control-label" Text="Seacrh Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Caption/Group Search" Value="Caption"></asp:ListItem>
                                    <asp:ListItem Text="Caption Details Search" Value="Details"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblFiscalYear" runat="server" class="control-label required-field" Text="Report Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchReportType" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" value="Search" onclick="GridPaging(1, 1)" class="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <table id="TblConfigReport" class="table table-bordered table-condensed table-hover">
                        <thead>
                            <tr>
                                <th style="width: 35%;">Caption</th>
                                <th style="width: 20%;">Sorting Order</th>
                                <th style="width: 30%;">Caption Type</th>
                                <th style="width: 15%;">Action</th>
                                <th style="width: 15%; display: none;">Id</th>
                                <th style="width: 15%; display: none;">Report Type Id</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                    <div class="text-center" id="GridPagingContainer">
                        <ul class="pagination">
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
