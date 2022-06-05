<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBanquetCalendar.aspx.cs" Inherits="HotelManagement.Presentation.Website.Banquet.frmBanquetCalendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Banquet Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Hall Calendar</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            //var txtCurrentDate = '<%=txtCurrentDate.ClientID%>'

            $("#ContentPlaceHolder1_txtCurrentDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        });

        function PopUpReservationInformation(title) {
            //popup(1, 'ReservationPopUp', '', 900, 500);

            $("#ReservationPopUp").dialog({
                autoOpen: true,
                modal: true,
                width: 825,
                closeOnEscape: true,
                resizable: false,
                title: "" + title,
                show: 'slide'
            });

            return false;
        }

        $(function () {
            $("#PopReservation").tabs();
        });
        function OnSucceed(result) {
            if (result.EventTitle == "") {
                PopUpReservationInformation("Hall Reservation Details");
            }
            else
                PopUpReservationInformation(result.EventTitle);
        }
        function OnFailed() {
            toastr.error("");
        }
        function RedirectToDetails(ReservationId) {
            GetReservationInformation(ReservationId);
            PageMethods.GetReservationInfoById(ReservationId, OnSucceed, OnFailed);
        }
        function GetReservationInformation(ReservationId) {

            PageMethods.GetReservationInformationByReservationId(ReservationId, GetReservationInformationSucceeded, OnOperationFailed);
            return false;
        }
        function GetReservationInformationSucceeded(result) {
            $("#ReservationInfo").html(result)
        }
        function OnOperationFailed() {
            toastr.error(error.get_message());
        }
    </script>
    <table>
        <thead>
            <tr>
                <th></th>
            </tr>
        </thead>
    </table>
    <asp:HiddenField ID="hfEventTitle" runat="server" />
    <asp:HiddenField ID="hfReservationId" runat="server" Value="0" />
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Hall Calendar
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCurrentDate" runat="server" class="control-label" Text="Start Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCurrentDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblDuration" runat="server" class="control-label" Text="Calendar Duration"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDuration" runat="server" TabIndex="2" CssClass="form-control">
                                <asp:ListItem Text="1 Day" Value="7" />
                                <%--<asp:ListItem Text="2 Days" Value="14" />--%>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4" style="padding-left: 0">
                            <asp:Button ID="btnViewCalender" runat="server" class="TransactionalButton btn btn-primary btn-sm"
                                TabIndex="3" Text="View Calendar" OnClick="btnViewCalender_Click" />
                        </div>
                    </div>
                </div>
                <div class="childDivSection">
                    <div id="SearchEntry" class="panel panel-default">
                        <div class="panel-heading">
                            Hall Calendar
                        </div>
                        <div class="panel-body">
                            <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="500px">
                                <div id="ltlCalenderControl" runat="server">
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="ReservationPopUp" style="display: none;">
        <div id="PopTabPanel" style="width: 800px">
            <%--<div id="PopReservation">--%>
            <%--<ul id="PoptabPage" class="ui-style">
                    <li id="PopA" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-1">Banquet Reservation Information</a></li>
                </ul>--%>
            <%--<div id="Poptab-1">--%>
            <%--<div id="ReservationDetails" class="block" style="font-weight: bold">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">Banquet Reservation Information
                        </a>--%>
            <div id="ReservationInfo">
            </div>
            <%-- </div>--%>
            <%--</div>--%>
            <%--<button type="button" id="btnReservationBack" onclick="javascript:return popup(-1)"
                    class="TransactionalButton btn btn-primary">
                    Back</button>--%>
            <%--</div>--%>
        </div>
        <div class='divClear'>
        </div>
    </div>
</asp:Content>
