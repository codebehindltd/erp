<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="GuestBirthdayNotification.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.GuestBirthdayNotification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            $("#ContentPlaceHolder1_tbDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate",new Date());

            $("#chkAll").click(function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#tbGuestList tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#tbGuestList tbody tr").find("td:eq(0)").find("input").prop("checked", false);
                }
            });
        });

        function SearchGuest(pageNumber, IsCurrentOrPreviousPage) {

            var date = $("#ContentPlaceHolder1_tbDate").val();
            var type = $("#ContentPlaceHolder1_ddlGuestType").val();
            var gridRecordsCount = $("#tbGuestList tbody tr").length;

            var params = JSON.stringify({ date: date, guestType: type, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, isCurrentOrPreviousPage: IsCurrentOrPreviousPage });
            $.ajax({
                type: "POST",
                url: "/HotelManagement/GuestBirthdayNotification.aspx/GetGuestListForBirthdayWish",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    LoadGuestListTable(data.d);
                },
                error: function (error) {

                }
            });
            return false;
        }

        function LoadGuestListTable(guestList) {
            $("#tbGuestList tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            var tr = "", totalRow = 0;

            if (guestList.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Guest Found</td> </tr>";
                $("#tbGuestList tbody ").append(emptyTr);
                return false;
            }

            $.each(guestList.GridData, function (count, gridObject) {
                totalRow = $("#tbGuestList tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style=\"width:5%; cursor:pointer;\">" + "<input type='checkbox'/> " + "</td>";

                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.GuestName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.GuestEmail + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.CountryName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.GuestPhone + "</td>";
                if (gridObject.IsEmailSent)
                    tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" +  "<img alt=\"Sent\" src=\"/Images/approved.png\" /> " + "</td>";
                else
                    tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + "<img alt=\"Not Sent\" src=\"/Images/delete.png\" /> " + "</td>";
                if (gridObject.IsSmsSent)
                    tr += "<td align='left' style=\"width:10%; cursor:pointer;\">"  + "<img alt=\"Sent\" src=\"/Images/approved.png\" /> " + "</td>";
                else
                    tr += "<td align='left' style=\"width:10%; cursor:pointer;\">"  + "<img alt=\"Not Sent\" src=\"/Images/delete.png\" /> " + "</td>";

                tr += "<td align='left' style=\"display: none;\" >" + gridObject.Id + "</td>";
                tr += "<td align='left' style=\"display: none;\" >" + gridObject.GuestId + "</td>";
                tr += "<td align='left' style=\"display: none;\" >" + gridObject.IsEmailSent + "</td>";
                tr += "<td align='left' style=\"display: none;\" >" + gridObject.IsSmsSent + "</td>";

                $("#tbGuestList tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(guestList.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(guestList.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(guestList.GridPageLinks.NextButton);
        }

        function SendEmail() {
            var saveList = new Array();
            var updateList = new Array();
            var isChecked, isEmailSent, guestId = 0, id = 0;
            var guestName = "", email = "", phoneNo = "";
            var userInfoId = UserInfoFromDB.UserInfoId;

            $("#tbGuestList tbody tr").each(function () {

                isChecked = $(this).find("td:eq(0)").find("input").is(":checked");
                isEmailSent = $(this).find("td:eq(9)").text() == 'true';
                guestName = $(this).find("td:eq(1)").text();
                email = $(this).find("td:eq(2)").text();
                phoneNo = $(this).find("td:eq(4)").text();
                guestId = $(this).find("td:eq(8)").text();
                id = $(this).find("td:eq(7)").text();
                
                if (isChecked && !isEmailSent && email!="") {
                    if (id == "0")
                    {
                        saveList.push({
                            GuestName:guestName,
                            GuestEmail: email,
                            GuestId: parseInt(guestId),
                            IsEmailSent: true,
                            CreatedBy: userInfoId
                        });
                    }
                    else
                    {
                        updateList.push({
                            Id: parseInt(id),
                            GuestName: guestName,
                            GuestEmail: email,
                            GuestId: parseInt(guestId),
                            IsEmailSent: true,
                            LastModifiedBy: userInfoId
                        });
                    }

                }
            });
            
            if (saveList.length > 0 || updateList.length > 0)
            {
                var params = JSON.stringify({ saveGuestList: saveList, updateGuestList: updateList });

                $.ajax({
                    type: "POST",
                    url: "/HotelManagement/GuestBirthdayNotification.aspx/SendEmailToBirthdayWish",
                    data: params,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        CommonHelper.AlertMessage(data.d.AlertMessage);
                        $("#ContentPlaceHolder1_btnSearch").trigger('click');
                        Clear();
                    },
                    error: function (error) {

                    }
                });
            }
            else {
                toastr.warning("Already Sent Birthday Wish OR Email Address not found");
            }
            return false;
        }

        function SendSMS() {
            return false;
        }
        function Validate() {
            if ($("#tbGuestList tbody input:checkbox:checked").length == 0) {
                toastr.warning("Check Any Guest to wish");
                return false;
            }
            else
                SendEmail();
            return false;
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchGuest(pageNumber, IsCurrentOrPreviousPage);
        }
        function Clear() {
            $("#chkAll").prop("checked", false).triggerHandler('click');
        }
    </script>

    <div id="EntryPanel" class="panel panel-default">
        <div class="form-horizontal">
            <div id="dvGuestList" class="panel panel-default">
                <div class="panel-heading">
                    Guest List
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label Text="Date" runat="server" class="control-label required-field"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox runat="server" ID="tbDate" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label Text="Guest Type" runat="server" class="control-label required-field"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:DropDownList ID="ddlGuestType" runat="server" CssClass="form-control">
                                <asp:ListItem Text="--- ALL ---" Value="ALL"> </asp:ListItem>
                                <asp:ListItem Text="In House" Value="In House"> </asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                            OnClientClick="return SearchGuest(1,1)" />
                    </div>
                    <table id="tbGuestList" class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="background-color: #44545E">
                                <th rowspan="2" style="width: 5%;">
                                    <input id="chkAll" type="checkbox" />
                                </th>
                                <th rowspan="2" style="width: 20%;">Guest Name</th>
                                <th rowspan="2" style="width: 20%;">Email</th>
                                <th rowspan="2" style="width: 15%;">Country</th>
                                <th rowspan="2" style="width: 20%;">Phone</th>
                                <th colspan="2" style="width: 20%; text-align:center">Status
                                </th>
                                <th rowspan="2" style="display: none;">Id</th>
                            </tr>
                            <tr style="background-color: #44545E">
                                <th style="width: 30px;">Email
                                </th>
                                <th style="width: 30px;">SMS
                                </th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                    <div class="childDivSection">
                        <div class="text-center" id="GridPagingContainer">
                            <ul class="pagination">
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div style="float: right" class="col-md-3">
                    <asp:Button ID="btnEmail" Text="Send Email" CssClass="TransactionalButton btn btn-primary btn-sm" runat="server" OnClientClick="return Validate(); " />
                    <asp:Button Enabled="false" ID="btnSms" Text="Send SMS" CssClass="TransactionalButton btn btn-primary btn-sm" runat="server" OnClientClick="return SendSMS(); " />
                </div>
            </div>

        </div>
    </div>

</asp:Content>
