<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmLCOverHeadName.aspx.cs" Inherits="HotelManagement.Presentation.Website.LCManagement.frmLCOverHeadName" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        var flag = 0;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Service Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#ContentPlaceHolder1_ddlIncomeAccountHead").select2({
                tags: true,
                width: "100%",
                dropdownParent: $("#CreateNewDialog")
            });
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("#gvPaidServiceInformation").delegate("td > img.PaidServiceDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var itemId = $.trim($(this).parent().parent().find("td:eq(4)").text());
                    var params = JSON.stringify({ sEmpId: itemId });

                    var $row = $(this).parent().parent();
                    //$(this).parent().parent().remove();
                    $.ajax({
                        type: "POST",
                        url: "/LCManagement/frmOverHeadName.aspx/DeletePaidServiceById",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $row.remove();
                            CommonHelper.AlertMessage(data.d.AlertMessage);
                            $("#myTabs").tabs('load', 1);
                        },
                        error: function (error) {
                        }
                    });
                }
            });
            GridPaging(1, 1);
        });

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvPaidServiceInformation tbody tr").length;

            var serviceName = $("#<%=txtSServiceName.ClientID %>").val();
            var serviceType = "";
            var activeStat = $("#<%=ddlSActiveStat.ClientID %>").val();
            var IsCNFHead = $("#<%=ddlSIsCNFHead.ClientID %>").val();

            PageMethods.SearchPaidServiceAndLoadGridInformation(serviceName, serviceType, activeStat, IsCNFHead, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {

            //$("#ltlTableWiseItemInformation").html(result);

            $("#gvPaidServiceInformation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvPaidServiceInformation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvPaidServiceInformation tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + gridObject.OverHeadName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.ActiveStatus + "</td>";
                //tr += "<td align='right' style=\"width:5%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.OverHeadId + "')\" alt='Edit Information' border='0' />&nbsp;&nbsp;<img src='../Images/delete.png' class= 'PaidServiceDelete'  alt='Delete Information' border='0' /></td>";
                tr += "<td align='right' style='width:25%;'> <a onclick=\"javascript:return PerformEditAction(" + gridObject.OverHeadId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteOverHead(" + gridObject.OverHeadId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";

                tr += "<td align='right' style=\"width:5%; display:none;\">" + gridObject.OverHeadId + "</td>";

                tr += "</tr>"

                $("#gvPaidServiceInformation tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);


            return false;
        }
        function OnLoadObjectFailed(error) {
            alert(error.get_message());
        }
        function PerformEditAction(Id) {
            FillForm(Id);
        }
        function SaveOrUpdateLCOverHead() {

            var id = $("#ContentPlaceHolder1_hfId").val()
            var OverHeadName = $("#ContentPlaceHolder1_txtServiceName").val();
            if (id == 0) {
                var IsUpdate = 0;
            }
            else {
                IsUpdate = 1;
            }
            if (OverHeadName == "") {
                toastr.warning("Enter LCOverHead Name");
                $("#ContentPlaceHolder1_txtServiceName").focus();
                return false;
            }
            PageMethods.DuplicateCheckDynamicaly("OverHeadName", OverHeadName, IsUpdate, id, DuplicateCheckDynamicalySucceed, DuplicateCheckDynamicalyFailed);
            return false;
        }
        function DuplicateCheckDynamicalySucceed(result) {
            id = $("#ContentPlaceHolder1_hfId").val()
            OverHeadName = $("#ContentPlaceHolder1_txtServiceName").val();
            NodeId = $("#ContentPlaceHolder1_ddlIncomeAccountHead").val();
            if (NodeId == 0) {
                toastr.warning("Please select Accounts Head");
                $("#ContentPlaceHolder1_ddlIncomeAccountHead").focus();
                return false;
            }
            description = $("#ContentPlaceHolder1_txtDescription").val();
            IsCNFHead = $("#ContentPlaceHolder1_ddlIsCNFHead").val() == "1" ? true : false;
            status = $("#ContentPlaceHolder1_ddlActiveStat").val() == "0" ? true : false;
            if (result > 0) {
                toastr.warning("Duplicate Over Head Name");
                $("#ContentPlaceHolder1_txtServiceName").focus();
                return false;
            }
            else {
                var overHeadNameBO = {
                    OverHeadId: id,
                    OverHeadName: OverHeadName,
                    Description: description,
                    NodeId: NodeId,
                    IsCNFHead: IsCNFHead,
                    ActiveStat: status,
                }
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../LCManagement/frmLCOverHeadName.aspx/SaveUpdateLCOverHeadInformation',

                    data: JSON.stringify({ overHeadNameBO: overHeadNameBO }),
                    dataType: "json",
                    success: function (data) {
                        GridPaging(1, 1);
                        PerformClearAction();
                        CommonHelper.AlertMessage(data.d.AlertMessage);
                        if (flag == 1) {
                            $('#CreateNewDialog').dialog('close');
                        }
                        flag = 0;
                    },
                    error: function (result) {

                    }
                });
            }

            $("#ContentPlaceHolder1_txtServiceName").focus();
        }
        function DuplicateCheckDynamicalyFailed() {

        }
        function FillForm(Id) {
            $("#ContentPlaceHolder1_btnClear").hide();

            CommonHelper.SpinnerOpen();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../LCManagement/frmLCOverHeadName.aspx/FillForm',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    if (!confirm("Do you want to edit - " + data.d.OverHeadName + "?")) {
                        return false;
                    }
                    $("#CreateNewDialog").dialog({
                        autoOpen: true,
                        modal: true,
                        width: '75%',
                        closeOnEscape: true,
                        resizable: false,
                        height: 'auto',
                        fluid: true,
                        title: "Update Over Head - " + data.d.OverHeadName,
                        show: 'slide'
                    });
                    $("#btnSave").val("Update And Close");
                    $("#btnClear").hide();
                    $("#btnSaveNContinue").hide();
                    // $("#AddNewStatusContaiiner").dialog({ title: "Edit Source - " + data.d.SourceName + " " });
                    $("#ContentPlaceHolder1_hfId").val(data.d.OverHeadId)
                    $("#ContentPlaceHolder1_txtServiceName").val(data.d.OverHeadName);
                    $("#ContentPlaceHolder1_ddlIncomeAccountHead").val(data.d.NodeId).trigger('change');
                    $("#ContentPlaceHolder1_txtDescription").val(data.d.Description);
                    $("#ContentPlaceHolder1_ddlIsCNFHead").val(data.d.IsCNFHead == false ? "0" : "1");
                    $("#ContentPlaceHolder1_ddlActiveStat").val(data.d.ActiveStat == false ? "1" : "0");

                    CommonHelper.SpinnerClose();
                },
                error: function (result) {

                }
            });
            return false;
        }
        function CreateNew() {
            PerformClearAction();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '75%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Over Head Information",
                show: 'slide'
            });

            return false;
        }
        function Clean() {
            $("#ContentPlaceHolder1_txtSServiceName").val("");
            return false;
        }
        function SaveAndClose() {
            flag = 1;
            SaveOrUpdateLCOverHead();
            return false;

        }
        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfId").val("0")
            $("#ContentPlaceHolder1_txtServiceName").val("");
            $("#ContentPlaceHolder1_ddlIncomeAccountHead").val("0");
            $("#ContentPlaceHolder1_txtDescription").val("");
            $("#ContentPlaceHolder1_ddlIsCNFHead").val("0");
            $("#ContentPlaceHolder1_ddlActiveStat").val("0");
            $("#btnClear").show();
            $("#btnSave").val("Save & Close");
            $("#btnSaveNContinue").val("Save & Continue").show();
        }
        function DeleteOverHead(Id) {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../LCManagement/frmLCOverHeadName.aspx/DeleteLCOverHead',
                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    GridPaging(1, 1);
                },
                error: function (result) {

                }
            });
            return false;
        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <div id="MessageBox" class="alert alert-info" style="display: none">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="CreateNewDialog" style="display: none; overflow: unset">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2 ">
                    <asp:HiddenField ID="txtPaidServiceId" runat="server"></asp:HiddenField>
                    <asp:Label ID="lblServiceName" runat="server" class="control-label required-field" Text="Over Head Name"></asp:Label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtServiceName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2 ">
                    <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2 ">
                    <asp:Label ID="lblIncomeAccountHead" runat="server" class="control-label required-field" Text="Accounts Head"></asp:Label>
                </div>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlIncomeAccountHead" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2 ">
                    <asp:Label ID="lblIsCNFHead" runat="server" class="control-label required-field" Text="Is CNF Head"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlIsCNFHead" runat="server" CssClass="form-control"
                        TabIndex="4">
                        <asp:ListItem Value="0">No</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2 ">
                    <asp:Label ID="lblActiveStat" runat="server" class="control-label required-field" Text="Status"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control"
                        TabIndex="5">
                        <asp:ListItem Value="0">Active</asp:ListItem>
                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="SaveAndClose()" value="Save & Close" />
                    <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return PerformClearAction();" />
                    <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return SaveOrUpdateLCOverHead();" />
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Over Head Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSServiceName" runat="server" class="control-label " Text="Over Head Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSServiceName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 ">
                        <asp:Label ID="lblSIsCNFHead" runat="server" class="control-label " Text="Is CNF Head"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSIsCNFHead" runat="server" CssClass="form-control">
                            <asp:ListItem Value="All">All</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 ">
                        <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control">                            
                            <asp:ListItem Value="0">Active</asp:ListItem>
                            <asp:ListItem Value="1">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnSearch" class="btn btn-primary btn-sm">
                            Search</button>
                        <asp:Button ID="btnClean" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return Clean();" />
                        <asp:Button ID="btnCreateNew" runat="server" Text="New LC Over Head" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">Search Information</div>
        <div class="panel-body">
            <table class="table table-bordered table-condensed table-responsive" id='gvPaidServiceInformation' width="100%">
                <colgroup>
                    <col style="width: 55%;" />
                    <col style="width: 20%;" />
                    <col style="width: 10%;" />
                </colgroup>
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <td>Name
                        </td>
                        <td>Status
                        </td>
                        <td style="text-align: right;">Option
                        </td>
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
</asp:Content>
