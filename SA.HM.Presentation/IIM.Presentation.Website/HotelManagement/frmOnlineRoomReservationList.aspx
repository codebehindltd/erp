<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmOnlineRoomReservationList.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmOnlineRoomReservationList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var minCheckInDate = "";
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Online Room Reservation List</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#btnSrchRsrvtn").click(function () {
                $("#SearchPanel").show('slow');
                GridPagingForSearchReservation(1, 1);
            });

            var txtFromDate = '<%=txtFromDate.ClientID%>'
            var txtToDate = '<%=txtToDate.ClientID%>'

            $('#' + txtFromDate).datepicker({                
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtToDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtToDate).datepicker({               
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtFromDate).datepicker("option", "maxDate", selectedDate);
                }
            });

        });

        function PerformEditAction(reservationId) {
            var possiblePath = "frmManageOnlineRoomReservation.aspx?editId=" + reservationId;
            window.location = possiblePath;
        }

        function GridPagingForSearchReservation(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#tblRoomReserve tbody tr").length;

            var fromDate = $("#<%=txtFromDate.ClientID %>").val();
            var toDate = $("#<%=txtToDate.ClientID %>").val();
            var guestName = $("#<%=txtSrcReservationGuest.ClientID %>").val();
            var reserveNo = $("#<%=txtSearchReservationNumber.ClientID %>").val();
            var companyName = $("#<%=txtSearchCompanyName.ClientID %>").val();
            var contactPerson = $("#<%=txtCntPerson.ClientID %>").val();
            PageMethods.SearchResevationAndLoadGridInformation(fromDate, toDate, guestName, reserveNo, companyName, contactPerson, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSuccess, OnFail);
            return false;
        }
        function OnSuccess(result) {
            var format = innBoarDateFormat.replace('mm', 'MM');
            var format = format.replace('yy', 'yyyy');
            vvc = result;
            $("#tblRoomReserve tbody tr").remove();
            $("#GridPagingContainerForSearchReservation ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Data Found</td> </tr>";
                $("#tblRoomReserve tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#tblRoomReserve tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:15%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.ReservationNumber + "</td>";
                tr += "<td align='left' style=\"width:30%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.GuestName + "</td>";
                tr += "<td align='left' style=\"width:5%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.ReservationDate.format(format) + "</td>";
                tr += "<td align='left' style=\"width:5%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.DateIn.format(format) + "</td>";
                tr += "<td align='left' style=\"width:5%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.DateOut.format(format) + "</td>";
                tr += "<td align='left' style=\"width:5%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.ReservationMode + "</td>";

                if ($.trim(gridObject.ReservationMode) == "Pending")
                    tr += "<td align='right' style=\"width:45%; cursor:pointer;\"><img src='../Images/edit.png' ToolTip='Edit' onClick= \"javascript:return PerformEditAction('" + gridObject.ReservationId + "')\" alt='Edit Information' border='0' /></td>";
                tr += "<td align='right' style=\"width:5%; display:none;\">" + gridObject.ReservationId + "</td>";

                tr += "</tr>"

                $("#tblRoomReserve tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainerForSearchReservation ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainerForSearchReservation ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainerForSearchReservation ul").append(result.GridPageLinks.NextButton);

            return false;
        }
        function OnFail(error) {
            toastr.error(error.get_message());
        }
    </script>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Online Room Reservation Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TabIndex="63"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" runat="server" TabIndex="64" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSrcReservationGuest" runat="server" class="control-label" Text="Guest Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSrcReservationGuest" runat="server" CssClass="form-control" TabIndex="65"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblSearchReservationNumber" runat="server" class="control-label" Text="Reservation Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchReservationNumber" runat="server" CssClass="form-control" TabIndex="66"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCmpName" runat="server" class="control-label" Text="Company Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchCompanyName" runat="server" CssClass="form-control" TabIndex="66"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblCntPerson" runat="server" class="control-label" Text="Contact Person"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCntPerson" runat="server" CssClass="form-control" TabIndex="67"></asp:TextBox>
                    </div>
                </div>
                <div class="row" style="padding-top:10px;">
                    <div class="col-md-12">
                        <button type="button" id="btnSrchRsrvtn" class="TransactionalButton btn btn-primary btn-sm">
                            Search</button>
                        <asp:Button ID="btnClearSearch" runat="server" TabIndex="68" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearSearchAction();" />
                    </div>
                </div>
                <div id="SearchResultPanel" class="panel panel-default" style="margin-top:10px;">
                    <div class="panel-heading">
                        Search Information</div>
                    <div class="panel-body">
                        <table class="table table-bordered table-condensed table-responsive" id='tblRoomReserve'
                            width="100%">
                            <colgroup>
                                <col style="width: 15%;" />
                                <col style="width: 34%;" />
                                <col style="width: 12%;" />
                                <col style="width: 12%;" />
                                <col style="width: 12%;" />
                                <col style="width: 5%;" />
                                <col style="width: 5%;" />
                                <col style="width: 5%;" />
                            </colgroup>
                            <thead>
                                <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                    <td>
                                        Reservation No.
                                    </td>
                                    <td>
                                        Guest Name
                                    </td>
                                    <td>
                                        Reservation
                                    </td>
                                    <td>
                                        Date-In
                                    </td>
                                    <td>
                                        Date-Out
                                    </td>
                                    <td>
                                        Status
                                    </td>
                                    <td style="text-align: right;">
                                        Action
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                        <div class="childDivSection">
                            <div class="text-center" id="GridPagingContainerForSearchReservation">
                                <ul class="pagination">
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
