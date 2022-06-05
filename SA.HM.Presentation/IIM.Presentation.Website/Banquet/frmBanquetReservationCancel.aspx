<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBanquetReservationCancel.aspx.cs" Inherits="HotelManagement.Presentation.Website.Banquet.frmBanquetReservationCancel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Banquet Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Cancel Reservation</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_txtProbableArrivalHour').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtProbableDepartureHour').timepicker({
                showPeriod: is12HourFormat
            });

            $("#ContentPlaceHolder1_txtArriveDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtDepartureDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        });

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtReason.ClientID %>").val('');
            return false;
        }
    </script>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Cancel Reservation
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSrcRoomNumber" runat="server" class="control-label required-field" Text="Reservation Number"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:HiddenField ID="txtSrcRegistrationIdList" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtSrcBillNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-1" style="padding-left: 0">
                        <asp:Button ID="btnSrcBillNumber" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSrcBillNumber_Click" />
                        <div style="display: none;">
                            <asp:DropDownList ID="ddlReservationId" runat="server" CssClass="form-control"
                                TabIndex="3" Enabled="False">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnlBanquerReservationInfo" runat="server" Visible="false">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblArriveDate" runat="server" class="control-label" Text="Party Start Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtArriveDate" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblProbableArrivalTime" runat="server" class="control-label" Text="Party Start Time"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtProbableArrivalHour" placeholder="12" CssClass="form-control"
                                runat="server" TabIndex="2"></asp:TextBox>
                            <%--<asp:TextBox ID="txtProbableArrivalMinute" placeholder="00" CssClass="CustomMinuteSize"
                                TabIndex="3" runat="server" disabled></asp:TextBox>
                            <asp:DropDownList ID="ddlProbableArrivalAMPM" CssClass="CustomAMPMSize" runat="server"
                                TabIndex="4">
                                <asp:ListItem>AM</asp:ListItem>
                                <asp:ListItem>PM</asp:ListItem>
                            </asp:DropDownList>
                            (12:00AM)--%>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblDepartureDate" runat="server" class="control-label" Text="Party End Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDepartureDate" runat="server" CssClass="form-control" TabIndex="13"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblProbableDepartureTime" runat="server" class="control-label" Text="Party End Time"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtProbableDepartureHour" placeholder="12" CssClass="form-control"
                                runat="server" TabIndex="2"></asp:TextBox>
                            <%--<asp:TextBox ID="txtProbableDepartureMinute" placeholder="00" CssClass="CustomMinuteSize"
                                TabIndex="3" runat="server"></asp:TextBox>
                            <asp:DropDownList ID="ddlProbableDepartureAMPM" CssClass="CustomAMPMSize" runat="server"
                                TabIndex="4">
                                <asp:ListItem>AM</asp:ListItem>
                                <asp:ListItem>PM</asp:ListItem>
                            </asp:DropDownList>
                            (12:00AM)--%>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblOccessionTypeId" runat="server" class="control-label" Text="Occasion Type"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlOccessionTypeId" CssClass="form-control" runat="server"
                                TabIndex="14">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblBanquetId" runat="server" class="control-label" Text="Banquet Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlBanquetId" runat="server" TabIndex="15" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblReservationMode" runat="server" class="control-label" Text="Reservation Mode"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlReservationMode" runat="server" CssClass="form-control" TabIndex="11">
                                <asp:ListItem Value="Company">Company</asp:ListItem>
                                <asp:ListItem Value="Personal">Personal</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblSeatingId" runat="server" class="control-label" Text="Seating Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSeatingId" runat="server" CssClass="form-control" TabIndex="16">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblName" runat="server" class="control-label" Text="Company Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblReason" runat="server" class="control-label required-field" Text="Reason"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" TextMode="MultiLine"
                            TabIndex="2"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnUpdate" runat="server" Text="Save For Cancellation" TabIndex="4" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnUpdate_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnClear_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
