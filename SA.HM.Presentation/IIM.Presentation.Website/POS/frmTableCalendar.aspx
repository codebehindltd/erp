<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmTableCalendar.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmTableCalendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Table Calendar</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        function PopUpReservationInformation() {
            popup(1, 'ReservationPopUp', '', 900, 500);
            return false;
        }

        $(function () {
            $("#PopReservation").tabs();
        });

        function RedirectToDetails(ReservationId) {
            PopUpReservationInformation();
            GetReservationInformation(ReservationId);
        }

        function GetReservationInformation(ReservationId) {
            PageMethods.GetReservationInformationByReservationId(ReservationId, GetReservationInformationSucceeded, OnOperationFailed);
            return false;
        }

        function GetReservationInformationSucceeded(result) {
            $("#ReservationInfo").html(result)
        }

        function OnOperationFailed() {
            alert(error.get_message());
        }

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="EntryPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Table Calendar
        </a>
        <div class="HMBodyContainer">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblCostCentre" runat="server" Text="Cost Centre"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlCostCenter" runat="server" TabIndex="2">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblCurrentDate" runat="server" Text="Start Date"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtCurrentDate" runat="server" CssClass="datepicker" TabIndex="1"></asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblDuration" runat="server" Text="Calendar Duration"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:DropDownList ID="ddlDuration" runat="server" TabIndex="2" CssClass="CustomTextBox">
                        <asp:ListItem Text="1 Day" Value="7" />
                        <asp:ListItem Text="2 Days" Value="14" />
                    </asp:DropDownList>
                    <asp:Button ID="btnViewCalender" runat="server" class="TransactionalButton btn btn-primary"
                        TabIndex="3" Text="View Calendar" OnClick="btnViewCalender_Click" />
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
        <div class="childDivSection">
            <div id="SearchEntry" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Table Calendar Information
                </a>
                <div class="block-body collapse in">
                    <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="500px">
                        <div id="ltlCalenderControl" runat="server">
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <div id="ReservationPopUp" style="display: none;">
        <div id="PopTabPanel" style="width: 900px">
            <div id="PopReservation">
                <ul id="PoptabPage" class="ui-style">
                    <li id="PopA" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-1">Table Reservation Information</a></li>
                </ul>
                <div id="Poptab-1">
                    <div id="ReservationDetails" class="block" style="font-weight: bold">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">Table Reservation Information
                        </a>
                        <div id="ReservationInfo">
                        </div>
                    </div>
                </div>
                <button type="button" id="btnReservationBack" onclick="javascript:return popup(-1)"
                    class="TransactionalButton btn btn-primary">
                    Back</button>
            </div>
        </div>
        <div class='divClear'>
        </div>
    </div>
</asp:Content>
