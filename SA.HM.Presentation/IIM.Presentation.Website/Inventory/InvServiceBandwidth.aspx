<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="InvServiceBandwidth.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.InvServiceBandwidth" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            CommonHelper.ApplyDecimalValidation();
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            GridPaging(1, 1);
        });
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            debugger;
            var status = false;
            var name = $("#<%=txtSrcBandwidthName.ClientID %>").val();
            //status = $("#<%=ddlSrcStatus.ClientID %>").val();
            if ($("#<%=ddlSrcStatus.ClientID %>").val() == '1') {
                status = true;
            }
            var gridRecordsCount = $("#TableContainer tbody tr").length;

            PageMethods.GridPaging(name, status, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSuccessSearch, OnFailed);
            return false;
        }
        function OnSuccessSearch(result) {
            var rowLength = $("#TableContainer tbody tr").length;
            var dataLength = result.GridData.length;
            $("#TableContainer tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"8\" >No Data Found</td> </tr>";
                $("#TableContainer tbody ").append(emptyTr);
                return false;
            }

            $.each(result.GridData, function (count, gridObject) {
                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {

                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='display:none'>" + gridObject.ServiceBandWidthId + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.BandWidthName + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.UplinkFrequency + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.DownlinkFrequency + "</td>";
                tr += "<td style='width:20%;'>" + (gridObject.ActiveStat == true ? "Active" : "Inactive") + "</td>";
                //tr += "<td style='width:10%;'>" + CommonHelper.DateFromStringToDisplay(gridObject.CreatedDate, innBoarDateFormat) + "</td>";
                tr += "<td style='width:20%;cursor:pointer;'>";
                if (IsCanEdit) {
                    tr += '<a href="javascript:void();" onclick="javascript:return FillForm(' + gridObject.ServiceBandWidthId + ",\'" + gridObject.BandWidthName + '\');"' + "title='Edit' ><img src='../Images/edit.png' alt='Edit'></a>";
                }
                if (IsCanDelete) {
                    tr += '&nbsp;&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return Delete(' + gridObject.ServiceBandWidthId + ",\'" + gridObject.BandWidthName + '\');" ><img alt="Delete" src="../Images/delete.png" /></a>';
                }
                tr += "</td>";
                tr += "</tr>";

                $("#TableContainer tbody").append(tr);

                tr = "";
                i++;
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }
        function CreateNew() {
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '90%',
                height: 400,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "New Bandwidth",
                show: 'slide'
            });
        }
        function PerformSave() {
            var status = false;
            var id = $("#ContentPlaceHolder1_hfId").val();
            var name = $("#<%=txtBandwidthName.ClientID %>").val();
            var uplink = $("#<%=txtUplink.ClientID %>").val();
            var downlink = $("#<%=txtDownlink.ClientID %>").val();
            var upfreq = $("#<%=ddlUplinkFrequency.ClientID %>").val();
            var downfreq = $("#<%=ddlDownlinkFrequency.ClientID %>").val();
            //status = $("#<%=ddlStatus.ClientID %>").val();
            if ($("#<%=ddlSrcStatus.ClientID %>").val() == '1') {
                status = true;
            }
            var description = $("#<%=txtDescription.ClientID %>").val();

            var ServiceBandwidth = {
                ServiceBandWidthId : id,
                BandWidthName : name,
                Uplink : uplink,
                UplinkFrequency : upfreq,
                Downlink : downlink,
                DownlinkFrequency : downfreq,
                ActiveStat : status,
                Description : description
            }

            PageMethods.SaveOrUpdateServiceBandwidth(ServiceBandwidth, OnSuccessSaveOrUpdate, OnFailed);
            return false;
        }
        function OnSuccessSaveOrUpdate(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging(1, 1);
                Clear();
                CloseDialog();
            }
        }
        function CloseDialog() {
            $("#CreateNewDialog").dialog('close');
            return false;
        }
        function Clear() {
            $("#ContentPlaceHolder1_hfId").val("0");
            $("#<%=txtBandwidthName.ClientID %>").val("");
            $("#<%=txtUplink.ClientID %>").val("");
            $("#<%=txtDownlink.ClientID %>").val("");
            $("#<%=ddlUplinkFrequency.ClientID %>").val("0");
            $("#<%=ddlDownlinkFrequency.ClientID %>").val("0");
            $("#<%=ddlStatus.ClientID %>").val("1");
            $("#<%=txtDescription.ClientID %>").val("");
        }
        function FillForm(id, name) {
            if (!confirm("Do you want to edit " + name + " ?")) {
                return false;
            }
            PageMethods.FillForm(id, OnFillFormSucceed, OnFailed);
            return false;
        }
        function OnFillFormSucceed(result) {
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '90%',
                height: 400,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Update Bandwidth",
                show: 'slide'
            });
            Clear();
            $("#ContentPlaceHolder1_hfId").val(result.ServiceBandWidthId);
            $("#<%=txtBandwidthName.ClientID %>").val(result.BandWidthName);
            $("#<%=txtUplink.ClientID %>").val(result.Uplink);
            $("#<%=txtDownlink.ClientID %>").val(result.Downlink);
            $("#<%=ddlUplinkFrequency.ClientID %>").val(result.UplinkFrequency);
            $("#<%=ddlDownlinkFrequency.ClientID %>").val(result.DownlinkFrequency);
            if (result.ActiveStat) {
                $("#<%=ddlStatus.ClientID %>").val('1');
            }
            else {
                $("#<%=ddlStatus.ClientID %>").val('0');
            }
            
            $("#<%=txtDescription.ClientID %>").val(result.Description);

            $("#btnSave").val("Update & Close");

        }
        function Delete(id, name) {
            if (!confirm("Do you want to Delete " + name + " ?")) {
                return false;
            }
            PageMethods.Delete(id, OnDeleteSucceed, OnFailed);
            return false;
        }
        function OnDeleteSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging(1, 1);
            }
        }
        function OnFailed(result) {
            toastr.warning(result);
        }

    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Bandwidth Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSrcBandwidthName" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblContactOwner" runat="server" class="control-label" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSrcStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Atcive" Value="1" />
                            <asp:ListItem Text="Inatcive" Value="0" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" value="Search" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" onclick="javascript: return  GridPaging(1, 1);"/>
                        <%--<input type="button" value="Clear" id="btnSrcClear" class="TransactionalButton btn btn-primary btn-sm" />--%>
                        <input type="button" value="New Bandwidth" id="btnNewBandwidth" class="TransactionalButton btn btn-primary btn-sm" onclick="javascript: return CreateNew();" />
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
            <div class="form-group" id="TableContainer" style="overflow: scroll;">
                <table class="table table-bordered table-condensed table-responsive" id="ContactTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 20%;">Name
                            </th>
                            <th style="width: 20%;">Uplink Frequency
                            </th>
                            <th style="width: 20%;">Downlink Frequency
                            </th>
                            <th style="width: 20%;">Status
                            </th>
                            <th style="width: 20%;">Action
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
    <div id="CreateNewDialog" style="display: none;">
        <div class="panel panel-default">
            <%--            <div class="panel-heading">
                New Bandwidth
            </div>--%>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label2" runat="server" class="control-label" Text="Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtBandwidthName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label3" runat="server" class="control-label" Text="Uplink"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtUplink" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="Label4" runat="server" class="control-label" Text="Frequency"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlUplinkFrequency" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label5" runat="server" class="control-label" Text="Downlink"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDownlink" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="Label6" runat="server" class="control-label" Text="Frequency"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDownlinkFrequency" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label7" runat="server" class="control-label" Text="Status"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Atcive" Value="1" />
                                <asp:ListItem Text="Inatcive" Value="0" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label8" runat="server" class="control-label" Text="Description"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                        <div class="col-md-12">
                            <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                                onclick="PerformSave()" value="Save & Close" />
                            <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                                onclick="javascript: return Clear();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
