<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmInHouseGuestBreakfast.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmInHouseGuestBreakfast" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var gc = [];
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {

            IsCanSave = $("#<%=hfSavePermission.ClientID %>").val() == '1' ? true : false;
            IsCanEdit = $("#<%=hfEditPermission.ClientID %>").val() == '1' ? true : false;
            IsCanDelete = $("#<%=hfDeletePermission.ClientID %>").val() == '1' ? true : false;
            IsCanView = $("#<%=hfViewPermission.ClientID %>").val() == '1' ? true : false;

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Authorization Panel</a>";
            var formName = "<span class='divider'>/</span><li class='active'>User Permission</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);


            $("#chkAll").change(function () {
                if ($(this).is(":checked")) {
                    $("#UserWiseMenuAssign tbody tr").find("td:eq(2)").find("input").prop("checked", true);
                }
                else {
                    $("#UserWiseMenuAssign tbody tr").find("td:eq(2)").find("input").prop("checked", false);
                }
            });

            GetInHouseGuestBreakfastInfo();
        });

        function GetInHouseGuestBreakfastInfo() {
            $("#chkAll").prop("checked", false);

            PageMethods.GetInHouseGuestBreakfastInfo(OnLoadGetInHouseGuestBreakfastInfoSucceed, OnLoadMenuLinksFailed);
        }

        function OnLoadGetInHouseGuestBreakfastInfoSucceed(result) {
            gc = result;
            
            $("#UserWiseMenuAssign tbody").html("");

            //var menuLinks = result[0].MenuLinks;
            //var menuWiseLinks = result[0].MenuWisePermitedLinks;

            //var isCreate = "", isUpdate = "", isDelete = "", isView = "", hasPermission = "", linksDisplaySequence = "";
            //var i = 0, menuLength = menuLinks.length;
            var i = 0, menuLength = result.length;
            var tr = "";

            for (i = 0; i < menuLength; i++) {
                var object = result[i];
                if (i % 2 == 0) {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }
                else {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }


                tr += "<td style=\"display:none;\">" + object.GuestId + "</td>";
                tr += "<td style=\"display:none;\">" + object.RegistrationId + "</td>";

                tr += "<td style=\"width: 5%; text-align:center;\">" +
                    "<input type='checkbox' id='chk" + object.GuestId + "'" +
                    "</td>" +
                    "<td style=\"width: 8%;\">" +
                    object.RoomNumber +
                    "</td>" +
                    "<td style=\"width: 20%;\">" +
                    object.GuestName +
                    "</td>" +
                    "<td style=\"width: 10%;\">" +
                    object.ArriveDateString +
                    "</td>" +
                    "<td style=\"width: 8%;\">" +
                    object.ExpectedCheckOutDateString +
                    "</td>" +
                    "<td style=\"width: 8%;\">" +
                    object.RegistrationNumber +
                    "</td>" +
                    "<td style=\"width: 15%;\">" +
                    object.Remarks +
                    "</td>" +
                    "<td style=\"width: 15%;\">" +
                    object.POSRemarks +
                    "</td>" +
                "</tr>";

                $("#UserWiseMenuAssign tbody").append(tr);
                tr = "";
            }

            //CommonHelper.ApplyIntigerValidation();
        }
        function OnLoadMenuLinksFailed() {
        }

        function SaveMenuPermission() {

            var GuestBreakfastCompletedList = new Array();
            
            $("#UserWiseMenuAssign tbody tr").each(function () {
                if ($(this).find("td:eq(2)").find("input").is(":checked")) {
                    GuestBreakfastCompletedList.push({
                            GuestId: $.trim($(this).find("td:eq(0)").text()),
                            RegistrationId: $.trim($(this).find("td:eq(1)").text()),
                            BreakfastDate: $.trim($(this).find("td:eq(2)").text())
                        });
                }
            });

            PageMethods.SaveGuestBreakfastCompletedList(GuestBreakfastCompletedList, OnSaveMenuLinksSucceed, OnSaveMenuLinksFailed);

            return false;
        }

        function OnSaveMenuLinksSucceed(result) {
            location.reload();
            if (result.AlertMessage.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSaveMenuLinksFailed() {

        }

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfApprEvaId" runat="server" Value="" />
    <div id="ApprEvaluationEntryPanel" class="panel panel-default">
        <div class="panel-heading">
            In-house Guest Breakfast List
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <table id="UserWiseMenuAssign" style="width: 100%;" class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                                <th style="width: 5%; text-align: center;">Select<br />
                                    <input type="checkbox" id="chkAll" />
                                </th>
                                <th style="width: 8%;">Room Number
                                </th>
                                <th style="width: 20%;">Guest Name
                                </th>
                                <th style="width: 10%;">Arrival Date
                                </th>
                                <th style="width: 8%;">Exp. Departure
                                </th>
                                <th style="width: 8%;">Registration No.
                                </th>
                                <th style="width: 15%;">Hotel Remarks
                                </th>
                                <th style="width: 15%;">POS Remarks
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnApprovePermission" runat="server" Text="Breakfast Done" TabIndex="2"
                            CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript:return SaveMenuPermission()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
