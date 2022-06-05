<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmNoShowApproval.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmNoShowApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var minCheckInDate = "";
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>No-Show Approval</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            minCheckInDate = $("#<%=hfMinCheckInDate.ClientID %>").val();

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("#ContentPlaceHolder1_txtSearchDate").datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: minCheckInDate,
                dateFormat: innBoarDateFormat
            });
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#gvGustInformation tbody tr").length;
            var seacrhDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtSearchDate").val(), '/');
            PageMethods.GetRoomReservationInfoForNoShow(seacrhDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            var format = innBoarDateFormat.replace('mm', 'MM');
            var format = format.replace('yy', 'yyyy');

            $("#gvGustInformation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"8\" >No Data Found</td> </tr>";
                $("#gvGustInformation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";
            var isSavepermission = false;

            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "1")
                isSavepermission = true;
            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvGustInformation tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='right' style=\"display:none;\">" + gridObject.ReservationId + "</td>";
                tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.ReservationNumber + "</td>";
                tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + gridObject.GuestName + "</td>";
                tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + gridObject.CompanyName + "</td>";
                tr += "<td align='left' style=\"width:5%; cursor:pointer;\">" + gridObject.RoomInformation + "</td>";
                tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.DateIn.format(format) + "</td>";
                tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.DateOut.format(format) + "</td>";
                if (gridObject.Status == "" || gridObject.Status == "NoShow") {
                    tr += "<td align='left' style=\"width:10%;\">" +
                                        "<select id='NoshowAction" + gridObject.ReservationDetailId + "' class='form-control' style=\"width:100px;\">"
                                            + "<option value='NoShow'>Charge</option>"
                                            + "<option value='Hold'>Hold</option>"
                                            + "<option value='Canceled'>Cancel</option>"
                                            + "</select>" + "</td>";
                }
                else if (gridObject.Status == "Hold") {
                    tr += "<td align='left' style=\"width:10%;\">" +
                                        "<select id='NoshowAction" + gridObject.ReservationDetailId + "' class='form-control' style=\"width:100px;\">"
                                            + "<option value='Hold'>Hold</option>"
                                            + "<option value='NoShow'>Charge</option>"
                                            + "<option value='Canceled'>Cancel</option>"
                                            + "</select>" + "</td>";
                }
                else if (gridObject.Status == "Canceled") {
                    tr += "<td align='left' style=\"width:10%;\">" +
                                        "<select id='NoshowAction" + gridObject.ReservationDetailId + "' class='form-control' style=\"width:100px;\">"
                                            + "<option value='Canceled'>Cancel</option>"
                                            + "<option value='Hold'>Hold</option>"
                                            + "<option value='NoShow'>Charge</option>"
                                            + "</select>" + "</td>";
                }
                tr += "<td align='left' style=\"width:5%;\">";

                if (isSavepermission)
                    tr +=  "<input type='button' class='btn btn-primary btn-sm' value='Save' onclick=\"javascript:return PerformApproveAction('" + gridObject.ReservationId + "', '" + gridObject.ReservationDetailId + "')\">" ;
                tr += "</td>";
                //                if (gridObject.ReservationMode == "Active") {
                //                    tr += "<td align='right' style=\"width:5%; cursor:pointer;\"><img src='../Images/approved.png' onClick= \"javascript:return PerformApproveAction('" + gridObject.ReservationId + "')\" alt='Approve NoShow' border='0' /></td>";
                //                }
                //                else {
                //                    tr += "<td align='right' style=\"width:5%; cursor:pointer;\"><img src='../Images/cancel.png' onClick= \"javascript:return PerformCancelAction('" + gridObject.ReservationId + "')\" alt='Cancel NoShow' border='0' /></td>";
                //                }

                tr += "</tr>"

                $("#gvGustInformation tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);


            return false;
        }
        function OnLoadObjectFailed(error) {
            //toastr.error(error.get_message());
        }
        function PerformApproveAction(reservationId, detailId) {
            var status = $("#NoshowAction" + detailId + "").val();
            PageMethods.ApproveRoomReservationNoShow(reservationId, detailId, status, OnApprovalSucceess, OnApprovalFail);
        }
        function OnApprovalSucceess(result) {
            toastr.success("Saved Successfully.");
            //GridPaging(1, 1);
            //CommonHelper.AlertMessage(result.AlertMessage);            
        }
        function OnApprovalFail(error) {
            toastr.error(error);
        }

        function PerformCancelAction(reservationId) {
            PageMethods.CancelRoomReservationNoShow(reservationId, OnCancelSucceess, OnCancelFail);
        }
        function OnCancelSucceess(result) {
            GridPaging(1, 1);
            //CommonHelper.AlertMessage(result.AlertMessage);
        }
        function OnCancelFail(error) {
            toastr.error(error);
        }

    </script>
    <asp:HiddenField ID="hfMinCheckInDate" runat="server" />
    <asp:HiddenField ID="hfIsSavePermission" runat="server" />
    <div id="SearchEntry" class="panel panel-default">
        <div class="panel-heading">
            No-Show Approval
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSearchDate" runat="server" class="control-label" Text="Search Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchDate" runat="server" CssClass="form-control"
                            TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnSearch" class="btn btn-primary btn-sm">
                            Search</button>
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
            <table id='gvGustInformation' class="table table-bordered table-condensed table-responsive">
                <colgroup>
                    <col style="width: 10%;" />
                    <col style="width: 25%;" />
                    <col style="width: 25%;" />
                    <col style="width: 5%;" />
                    <col style="width: 10%;" />
                    <col style="width: 10%;" />
                    <col style="width: 10%;" />
                    <col style="width: 5%;" />
                </colgroup>
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <td>Reserv. No
                        </td>
                        <td>Guest Name
                        </td>
                        <td>Company
                        </td>
                        <td>Room Info
                        </td>
                        <td>Check In
                        </td>
                        <td>Check Out
                        </td>
                        <td style="text-align: center">Action
                        </td>
                        <td></td>
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
