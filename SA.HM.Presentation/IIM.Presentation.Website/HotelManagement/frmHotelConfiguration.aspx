<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmHotelConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmHotelConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .mycheckbox input[type="checkbox"] {
            margin-right: 10px;
            padding-top: 5px;
        }

        /*.dropdownColor:hover{
            background-color : none;
        }*/

        .dropdown:hover .dropdownColor {
            background-color: none;
        }

    </style>



    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }


            $("#ContentPlaceHolder1_CompanyCountryId").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });



            //$("#ContentPlaceHolder1_ddlRoomVacantDiv").select2({
            //    tags: "true",
            //    allowClear: true,
            //    width: "99.75%"
            //});

            //$("#ContentPlaceHolder1_ddlRoomVacantDiv").select2-menu-bg ({
            //    "data-bgcolor": "black",


            //});

            //$("#ContentPlaceHolder1_ddlRoomVacantDiv").select2-results.each( function() { 
            //    background-color :  this.value;
            //}

            //$("#ContentPlaceHolder1_ddlRoomVacantDiv > option").each(function () {
            //    background-color = this.value;
            //});

            $("#ContentPlaceHolder1_AdvancePaymentAdjustmentAccountsHeadId").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_HoldUpAccountsPostingAccountReceivableHeadId").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_FrontOfficeInvoiceRoomServiceName").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtProbableCheckInTime').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtProbableCheckOutTime').timepicker({
                showPeriod: is12HourFormat
            });

            CommonHelper.ApplyDecimalValidation();
            <%--$("#<%=txtProbableHour.ClientID %>").blur(function () {
                var hourString = $("#<%=txtProbableHour.ClientID %>").val();
                var hour = parseInt(hourString);
                if (hour > 12) {
                    hour = hour % 12;

                    $("#<%=txtProbableHour.ClientID %>").val(hour);
                    $("#<%=ddlProbableAMPM.ClientID %>").val('PM');
                }
                else {
                    $("#<%=ddlProbableAMPM.ClientID %>").val('AM');
                }
            });--%>

            $("#ContentPlaceHolder1_ddlUserGroup").change(function () {
                if ($(this).val() == "0") {
                    return false;
                }
                GetPossiblePaths();
            });
            $("#ContentPlaceHolder1_ddlRoomStatus").change(function () {
                GetPossiblePaths();
            });
            $("#chkAll").change(function () {
                if ($(this).is(":checked")) {
                    $("#PossibleActions tbody tr").find("td:eq(2)").find("input").prop("checked", true);
                }
                else {
                    $("#PossibleActions tbody tr").find("td:eq(2)").find("input").prop("checked", false);
                }
            });
        });
        $(function () {
            $("#myTabs").tabs();
        });

        //Room Status Configuration//
        function GetPossiblePaths() {
            var userGroupId = $("#ContentPlaceHolder1_ddlUserGroup").val();
            var roomStatus = $("#ContentPlaceHolder1_ddlRoomStatus").val();

            if (userGroupId == "0") {
                toastr.warning("Please Select User Group");
                return false;
            }
            PageMethods.GetPossiblePaths(userGroupId, roomStatus, OnLoadGetPossiblePathsSucceed, OnLoadGetPossiblePathsFailed);
        }



        function OnLoadGetPossiblePathsSucceed(result) {
            $("#PossibleActions tbody").html("");
            var pathHeads = result[0].PossiblePathHeads;
            var permPaths = result[1].PermittedPossiblePaths;

            var hasPermission = "", displaySequence = "", displayText = "", pathId = "0";
            var i = 0;
            var tr = "";

            for (i = 0; i < pathHeads.length; i++) {

                if (i % 2 == 0) {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }
                else {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }

                var alreadyPermittedLink = _.findWhere(permPaths, { PathId: pathHeads[i].PathId });

                if (alreadyPermittedLink != null) {
                    tr += "<td style=\"display:none;\">" + alreadyPermittedLink.MappingId + "</td>";
                    hasPermission = "checked='cheked'";
                    displayText = alreadyPermittedLink.DisplayText;
                    displaySequence = alreadyPermittedLink.DisplayOrder;
                    pathId = alreadyPermittedLink.PathId;
                }
                else {
                    tr += "<td style=\"display:none;\">0</td>"
                    hasPermission = "";
                    displayText = pathHeads[i].DisplayText;
                    displaySequence = "";
                    pathId = pathHeads[i].PathId;
                }

                tr += "<td style=\"display:none;\">" + pathId + "</td>";

                tr += "<td style=\"width: 10%; text-align:center;\">" +
                               "<input type='checkbox' " + hasPermission + "/>" +
                            "</td>" +
                            "<td style=\"width: 30%;\">" +
                                "<label>" + displayText + "</label>" +
                            "<td style=\"width: 30%;\">" +
                             "<input type='text' class='form-control' style='height:25px;' value='" + displayText + "'/>" +
                            "</td>" +
                             "<td style=\"width: 10%; text-align:center;\">" +
                                 "<input type='text' class='quantity form-control quantitydecimal' style='width:30px; height:25px;' value='" + displaySequence + "'/>" +
                            "</td>"
                "</tr>";

                $("#PossibleActions tbody").append(tr);
                tr = "";
            }
        }
        function OnLoadGetPossiblePathsFailed() {
        }

        function SaveRoomStatusPossiblePath() {
            var RoomStatusPossiblePathAdded = new Array();
            var RoomStatusPossiblePathEdited = new Array();
            var RoomStatusPossiblePathDeleted = new Array();
            var userGroupId = "0", pathType = "", mappingId = "0";

            userGroupId = $("#ContentPlaceHolder1_ddlUserGroup").val();
            pathType = $("#ContentPlaceHolder1_ddlRoomStatus").val();

            if (userGroupId == "0") {
                toastr.info("Please Select User Group");
                return false;
            }

            $("#PossibleActions tbody tr").each(function () {
                mappingId = $.trim($(this).find("td:eq(0)").text());
                if ($(this).find("td:eq(2)").find("input").is(":checked")) {

                    if ($.trim($(this).find("td:eq(5)").find("input").val()) == "" || $.trim($(this).find("td:eq(5)").find("input").val()) == "0")
                    {
                        toastr.info("Please Provide Valid Display Sequence from Grid.");
                        return false;
                    }

                    if (mappingId != "0") {
                        RoomStatusPossiblePathEdited.push({
                            MappingId: mappingId,
                            UserGroupId: userGroupId,
                            PossiblePathType: pathType,
                            PathId: $.trim($(this).find("td:eq(1)").text()),
                            DisplayText: $.trim($(this).find("td:eq(4)").find("input").val()),
                            DisplayOrder: $.trim($(this).find("td:eq(5)").find("input").val())
                        });
                    }
                    else {
                        RoomStatusPossiblePathAdded.push({
                            MappingId: mappingId,
                            UserGroupId: userGroupId,
                            PossiblePathType: pathType,
                            PathId: $.trim($(this).find("td:eq(1)").text()),
                            DisplayText: $.trim($(this).find("td:eq(4)").find("input").val()),
                            DisplayOrder: $.trim($(this).find("td:eq(5)").find("input").val())
                        });
                    }
                }
                else {
                    if (mappingId != "0") {
                        RoomStatusPossiblePathDeleted.push({
                            MappingId: mappingId,
                            UserGroupId: userGroupId,
                            PossiblePathType: pathType,
                            PathId: $.trim($(this).find("td:eq(1)").text())
                        });
                    }
                }
            });

            if (RoomStatusPossiblePathEdited.length == 0 && RoomStatusPossiblePathAdded.length == 0 && RoomStatusPossiblePathDeleted.length == 0) {
                toastr.info("Please Select Actions from Grid.");
                return false;
            }

            PageMethods.SaveRoomStatusPossiblePathPermission(RoomStatusPossiblePathAdded, RoomStatusPossiblePathEdited, RoomStatusPossiblePathDeleted, OnSaveRoomStatusPossiblePathSucceed, OnSaveRoomStatusPossiblePathFailed);
            return false;
        }

        function OnSaveRoomStatusPossiblePathSucceed(result) {
            if (result.AlertMessage.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSaveRoomStatusPossiblePathFailed() {
        }
        function ToggleGuestHouseRateIsPlusPlus() {
            var cbchkGuestHouseRateIsPlusPlus = '<%=chkGuestHouseRateIsPlusPlus.ClientID%>'
            var cbGuestHouseCityCharge = '<%=cbGuestHouseCityCharge.ClientID%>'
            if ($('#' + cbchkGuestHouseRateIsPlusPlus).is(':checked')) {
                $('#' + cbGuestHouseCityCharge).attr("disabled", false);
                $('#' + cbGuestHouseCityCharge).attr("checked", true);
            }
            else {
                $('#' + cbGuestHouseCityCharge).attr("checked", false);
                $('#' + cbGuestHouseCityCharge).attr("disabled", true);
            }
        }
        function ToggleGuestServiceRateIsPlusPlus() {
            var chkGuestServiceRateIsPlusPlus = '<%=chkGuestServiceRateIsPlusPlus.ClientID%>'
            var cbGuestServiceSDCharge = '<%=cbGuestServiceSDCharge.ClientID%>'
            if ($('#' + chkGuestServiceRateIsPlusPlus).is(':checked')) {
                $('#' + cbGuestServiceSDCharge).attr("disabled", false);
                $('#' + cbGuestServiceSDCharge).attr("checked", true);
            }
            else {
                $('#' + cbGuestServiceSDCharge).attr("checked", false);
                $('#' + cbGuestServiceSDCharge).attr("disabled", true);
            }
        }
    </script>


    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">General Configuration</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Room Possible Path Configuration</a></li>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Color Configuration</a></li>
        </ul>
        <div id="tab-1">
            <div class="row" style="display: none;">
                <div class="col-md-6">
                    <div id="MonthlySalaryDateSchedulePanel" class="panel panel-default">
                        <div class="panel-heading">
                            Hotel Bill Configuration
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-4 label-align">
                                        <asp:HiddenField ID="txtGuestHouseServiceChargeId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblGuestHouseServiceCharge" runat="server" class="control-label" Text="Service Charge"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtGuestHouseServiceCharge" TabIndex="2" CssClass="form-control quantitydecimal"
                                            runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4 label-align">
                                        <asp:HiddenField ID="txtGuestHouseCityChargeId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblGuestHouseCityCharge" runat="server" class="control-label" Text="City Charge"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="row">
                                            <div class="col-md-11">
                                                <asp:TextBox ID="txtGuestHouseCityCharge" runat="server" TabIndex="22" CssClass="form-control"></asp:TextBox>
                                                <asp:HiddenField ID="hfGuestHouseCityCharge" runat="server"></asp:HiddenField>
                                            </div>
                                            <div class="col-md-1 col-padding-left-none">
                                                <asp:HiddenField ID="cbGuestHouseCityChargeId" runat="server"></asp:HiddenField>
                                                <asp:CheckBox ID="cbGuestHouseCityCharge" runat="server" Text="" CssClass="customChkBox"
                                                    TabIndex="8" Checked="True" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4 label-align">
                                        <asp:HiddenField ID="txtGuestHouseVatId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblGuestHouseVat" runat="server" class="control-label" Text="Guest House Vat"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtGuestHouseVat" TabIndex="1" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4">
                                    </div>
                                    <div class="col-md-8">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="chkInclusiveHotelManagementBillId" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="chkInclusiveHotelManagementBill" TabIndex="3" runat="Server" Text=""
                                                Font-Bold="true" TextAlign="right" />
                                            &nbsp;&nbsp;Is Inclusive?
                                            &nbsp;&nbsp;
                                            <asp:HiddenField ID="chkGuestHouseRateIsPlusPlusId" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="chkGuestHouseRateIsPlusPlus" TabIndex="3" runat="Server" Text=""
                                                onclick="javascript: return ToggleGuestHouseRateIsPlusPlus();"
                                                Font-Bold="true" TextAlign="right" />
                                            &nbsp;&nbsp;Is ++ Rate?
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4 label-align">
                                        <asp:HiddenField ID="hfFOAdditionalChargeTypeId" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hfFOAdditionalChargeAmountId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblFOAdditionalCharge" runat="server" class="control-label" Text="Additional Charge"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlFOAdditionalChargeType" CssClass="form-control" runat="server" TabIndex="11">
                                            <asp:ListItem>Fixed</asp:ListItem>
                                            <asp:ListItem>Percentage</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtFOAdditionalChargeAmount" TabIndex="1" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="btnHotelBillCon" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"
                                            TabIndex="4" OnClick="btnHotelBillCon_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div id="EmployeeBasicPanel" class="panel panel-default">
                        <div class="panel-heading">
                            Service Bill Configuration
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-4 label-align">
                                        <asp:HiddenField ID="txtGuestServiceServiceChargeId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblGuestServiceServiceCharge" runat="server" class="control-label"
                                            Text="Service Charge"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtGuestServiceServiceCharge" TabIndex="6" CssClass="form-control quantitydecimal"
                                            runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4 label-align">
                                        <asp:HiddenField ID="txtGuestServiceSDChargeId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblGuestServiceSDCharge" runat="server" class="control-label" Text="SD Charge"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="row">
                                            <div class="col-md-11">
                                                <asp:TextBox ID="txtGuestServiceSDCharge" runat="server" TabIndex="22" CssClass="form-control"></asp:TextBox>
                                                <asp:HiddenField ID="hfGuestServiceSDCharge" runat="server"></asp:HiddenField>
                                            </div>
                                            <div class="col-md-1 col-padding-left-none">
                                                <asp:HiddenField ID="cbGuestServiceSDChargeId" runat="server"></asp:HiddenField>
                                                <asp:CheckBox ID="cbGuestServiceSDCharge" runat="server" Text="" CssClass="customChkBox"
                                                    TabIndex="8" Checked="True" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4 label-align">
                                        <asp:HiddenField ID="txtGuestServiceVatId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblGuestServiceVat" runat="server" class="control-label" Text="Service Vat"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtGuestServiceVat" TabIndex="5" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4">
                                    </div>
                                    <div class="col-md-8">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="chkInclusiveGuestServiceBillId" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="chkInclusiveGuestServiceBill" TabIndex="7" runat="Server" Text=""
                                                Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Inclusive?
                                            &nbsp;&nbsp;
                                            <asp:HiddenField ID="chkGuestServiceRateIsPlusPlusId" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="chkGuestServiceRateIsPlusPlus" TabIndex="3" runat="Server" Text=""
                                                onclick="javascript: return ToggleGuestServiceRateIsPlusPlus();"
                                                Font-Bold="true" TextAlign="right" />
                                            &nbsp;&nbsp;Is ++ Rate?
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4 label-align">
                                        <asp:HiddenField ID="hfSBAdditionalChargeTypeId" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hfSBAdditionalChargeAmountId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblSBAdditionalCharge" runat="server" class="control-label" Text="Additional Charge"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlSBAdditionalChargeType" CssClass="form-control" runat="server" TabIndex="11">
                                            <asp:ListItem>Fixed</asp:ListItem>
                                            <asp:ListItem>Percentage</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtSBAdditionalChargeAmount" TabIndex="1" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="btnServiceBillCon" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"
                                            TabIndex="8" OnClick="btnServiceBillCon_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div id="Div2" class="panel panel-default">
                        <div class="panel-heading">
                            Check In Time Configuration
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-4 label-align">
                                        <asp:HiddenField ID="txtGuestHouseCheckInTimeId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Check In Time"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtProbableCheckInTime" placeholder="12" runat="server" CssClass="form-control"
                                                    TabIndex="23"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 col-padding-left-none">
                                                <asp:HiddenField ID="txtGuestHouseCheckInExtraHourId" runat="server"></asp:HiddenField>
                                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Extra Hour"></asp:Label>
                                            </div>
                                            <div class="col-md-3 col-padding-left-none">
                                                <asp:TextBox ID="txtGuestHouseCheckInExtraHour" CssClass="form-control quantitydecimal" runat="server"
                                                    TabIndex="12"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <%--<div class="col-md-8" style="display: none;">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtProbableInHour" placeholder="12" TabIndex="9" CssClass="form-control quantitydecimal"
                                                    runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 col-padding-left-none">
                                                <asp:TextBox ID="txtProbableInMinute" placeholder="00" CssClass="form-control quantitydecimal" TabIndex="10"
                                                    runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 col-padding-left-none">
                                                <asp:DropDownList ID="ddlProbableInAMPM" CssClass="form-control" runat="server" TabIndex="11">
                                                    <asp:ListItem>AM</asp:ListItem>
                                                    <asp:ListItem>PM</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            (12:00AM)
                                        </div>
                                    </div>--%>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4 label-align">
                                    </div>
                                    <div class="col-md-8">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="btnCheckInTime" runat="server" Text="Save" TabIndex="13" CssClass="btn btn-primary btn-sm"
                                            OnClick="btnCheckInTime_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Div1" class="panel panel-default">
                        <div class="panel-heading">
                            Check Out Time Configuration
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-4 label-align">
                                        <asp:HiddenField ID="txtGuestHouseCheckOutTimeId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblGuestHouseCheckOutTime" runat="server" class="control-label" Text="Check Out Time"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtProbableCheckOutTime" placeholder="12" runat="server" CssClass="form-control"
                                                    TabIndex="23"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 col-padding-left-none">
                                                <asp:HiddenField ID="txtGuestHouseCheckOutExtraHourId" runat="server"></asp:HiddenField>
                                                <asp:Label ID="lblGuestHouseCheckOutExtraHour" runat="server" class="control-label"
                                                    Text="Extra Hour"></asp:Label>
                                            </div>
                                            <div class="col-md-3 col-padding-left-none">
                                                <asp:TextBox ID="txtGuestHouseCheckOutExtraHour" CssClass="form-control quantitydecimal" runat="server"
                                                    TabIndex="12"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <%--<div class="col-md-8">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtProbableHour" placeholder="12" TabIndex="9" CssClass="form-control quantitydecimal"
                                                    runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 col-padding-left-none">
                                                <asp:TextBox ID="txtProbableMinute" placeholder="00" CssClass="form-control quantitydecimal" TabIndex="10"
                                                    runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3 col-padding-left-none">
                                                <asp:DropDownList ID="ddlProbableAMPM" CssClass="form-control" runat="server" TabIndex="11">
                                                    <asp:ListItem>AM</asp:ListItem>
                                                    <asp:ListItem>PM</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            (12:00AM)
                                        </div>
                                    </div>--%>
                                </div>
                                <%--<div class="form-group">
                                    <div class="col-md-4 label-align">
                                        
                                    </div>
                                    <div class="col-md-8">
                                        
                                    </div>
                                </div>--%>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="btnCheckOutTime" runat="server" Text="Save" TabIndex="13" CssClass="btn btn-primary btn-sm"
                                            OnClick="btnCheckOutTime_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="ReCheckInHourConfigurationDiv" class="panel panel-default">
                        <div class="panel-heading">
                            Re Check In Hour Configuration after Check Out
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-4 label-align">
                                        <asp:HiddenField ID="hfReCheckInHourConfigurationId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Configuration Hour"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="ddlReCheckInHourConfiguration" CssClass="form-control" runat="server" TabIndex="11">
                                            <asp:ListItem Value="1">01</asp:ListItem>
                                            <asp:ListItem Value="2">02</asp:ListItem>
                                            <asp:ListItem Value="3">03</asp:ListItem>
                                            <asp:ListItem Value="4">04</asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem Value="6">06</asp:ListItem>
                                            <asp:ListItem Value="7">07</asp:ListItem>
                                            <asp:ListItem Value="8">08</asp:ListItem>
                                            <asp:ListItem Value="9">09</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="11">11</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="btnReCheckInHourConfiguration" runat="server" Text="Save" TabIndex="13" CssClass="btn btn-primary btn-sm"
                                            OnClick="btnReCheckInHourConfiguration_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="RoomReservationTermsAndConditionsDiv" class="panel panel-default">
                        <div class="panel-heading">
                            Room Reservation Terms and Conditions (~ sign use for New Line)
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:HiddenField ID="hfRoomReservationTermsAndConditions" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtRoomReservationTermsAndConditions" TabIndex="2" TextMode="MultiLine" CssClass="form-control quantitydecimal"
                                            runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="Button1" runat="server" Text="Update" TabIndex="13" CssClass="btn btn-primary btn-sm"
                                            OnClick="btnRoomReservationTermsAndConditions_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="RoomRegistrationTermsAndConditionsDiv" class="panel panel-default">
                        <div class="panel-heading">
                            Room Registration Terms and Conditions (~ sign use for New Line)
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:HiddenField ID="hfRoomRegistrationTermsAndConditions" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtRoomRegistrationTermsAndConditions" TabIndex="2" TextMode="MultiLine" CssClass="form-control quantitydecimal"
                                            runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="Button2" runat="server" Text="Update" TabIndex="13" CssClass="btn btn-primary btn-sm"
                                            OnClick="btnRoomRegistrationTermsAndConditions_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="paramCLAdditionalServiceMessegeDiv" class="panel panel-default">
                        <div class="panel-heading">
                            Additional Service Messege (~ sign use for New Line)
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:HiddenField ID="hfparamCLAdditionalServiceMessege" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtparamCLAdditionalServiceMessege" TabIndex="2" TextMode="MultiLine" CssClass="form-control quantitydecimal"
                                            runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="Button3" runat="server" Text="Update" TabIndex="13" CssClass="btn btn-primary btn-sm"
                                            OnClick="btnAdditionalServiceMessege_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="paramCLCancellationPolicyMessegeDiv" class="panel panel-default">
                        <div class="panel-heading">
                            Reservation Cancellation Policy Messege
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:HiddenField ID="hfparamCLCancellationPolicyMessege" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtparamCLCancellationPolicyMessege" TabIndex="2" TextMode="MultiLine" CssClass="form-control quantitydecimal"
                                            runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="Button4" runat="server" Text="Update" TabIndex="13" CssClass="btn btn-primary btn-sm"
                                            OnClick="btnReservationCancellationPolicyMessege_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="paramGIAggrimentMessegeDiv" class="panel panel-default">
                        <div class="panel-heading">
                            Aggriment Messege
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:HiddenField ID="hfparamGIAggrimentMessege" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtparamGIAggrimentMessege" TabIndex="2" TextMode="MultiLine" CssClass="form-control quantitydecimal"
                                            runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="Button5" runat="server" Text="Update" TabIndex="13" CssClass="btn btn-primary btn-sm"
                                            OnClick="btnAggrimentMessege_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div id="dvFOConfig" class="panel panel-default" runat="server">
                        <div class="panel-heading">
                            Front Office Configuration
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfInnboardTimeFormat" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label13" runat="server" class="control-label" Text="Innboard Time Format"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="InnboardTimeFormat" CssClass="form-control" runat="server">
                                                <asp:ListItem Value="HH:mm"> HH:mm</asp:ListItem>
                                                <asp:ListItem Value="hh:mm tt">hh:mm tt</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfHouseKeepingMorningDirtyHour" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label3" runat="server" class="control-label" Text="House Keeping Morning Dirty Hour"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="HouseKeepingMorningDirtyHour" TabIndex="2" CssClass="form-control quantitydecimal"
                                                runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfHouseKeepingDepartmentId" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label5" runat="server" class="control-label" Text="House Keeping Department Id"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="HouseKeepingDepartmentId" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfBillingConversionRate" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label6" runat="server" class="control-label" Text="Billing Conversion Rate"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="BillingConversionRate" TabIndex="2" CssClass="form-control quantitydecimal"
                                                runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfDefaultUsdToLocalCurrencyConversionRate" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label7" runat="server" class="control-label" Text="Default Usd To Local Currency Conv. Rate"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="DefaultUsdToLocalCurrencyConversionRate" TabIndex="2" CssClass="form-control quantitydecimal"
                                                runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfFrontOfficePaidOutServiceCharge" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label8" runat="server" class="control-label" Text="Front Office PaidOut Service Charge"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="FrontOfficePaidOutServiceCharge" TabIndex="2" CssClass="form-control quantitydecimal"
                                                runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfCompanyCountryId" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label9" runat="server" class="control-label" Text="Company Country"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="CompanyCountryId" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfInnboardCalenderFormat" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label10" runat="server" class="control-label" Text="Innboard Calender Format"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="InnboardCalenderFormat" CssClass="form-control" runat="server">
                                                <asp:ListItem Value="dd/MM/yyyy~dd/mm/yy">dd/MM/yyyy</asp:ListItem>
                                                <asp:ListItem Value="MM/dd/yyyy~mm/dd/yy">MM/dd/yyyy</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfGridViewPageSize" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label11" runat="server" class="control-label" Text="Grid View Page Size"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="GridViewPageSize" TabIndex="2" CssClass="form-control quantitydecimal"
                                                runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfGridViewPageLink" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label12" runat="server" class="control-label" Text="Grid View Page Link"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="GridViewPageLink" TabIndex="2" CssClass="form-control quantitydecimal"
                                                runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfFrontOfficeCostCenterId" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label16" runat="server" class="control-label" Text="Front Office CostCenter"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="FrontOfficeCostCenterId" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsBillSummaryPartWillHide" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsBillSummaryPartWillHide" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Bill Summary Part Will Hide?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsWaterMarkImageDisplayEnable" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsWaterMarkImageDisplayEnable" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Water Mark Image Display Enable?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsOnlyPdfEnableWhenReportExport" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsOnlyPdfEnableWhenReportExport" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Only Pdf Enable When Report Export?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsRoomOverbookingEnable" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsRoomOverbookingEnable" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Room Over booking Enable?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsConversionRateEditable" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsConversionRateEditable" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Conversion Rate Editable?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsDynamicBestRegardsForConfirmationLetter" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsDynamicBestRegardsForConfirmationLetter" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Dynamic Best Regards For Confirmation Letter?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsInvoiceGuestBillWithoutHeaderAndFooter" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsInvoiceGuestBillWithoutHeaderAndFooter" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Invoice Guest Bill Without Header and Footer?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsServiceBillWithoutInHouseGuest" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsServiceBillWithoutInHouseGuest" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Service Bill Without In-House Guest?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsRoomTypeCodeDisplayInRoomCalendar" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsRoomTypeCodeDisplayInRoomCalendar" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Room Type Code Display In Room Calendar?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsReservationConfirmationLetterOutletImageDisplay" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsReservationConfirmationLetterOutletImageDisplay" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Reservation Confirmation Letter Outlet Image Display?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsRoomReservationEmailAutoPostingEnable" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsRoomReservationEmailAutoPostingEnable" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is RoomReservation Email Auto Posting Enable?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsRoomRegistrationEmailAutoPostingEnable" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsRoomRegistrationEmailAutoPostingEnable" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Room Registration Email Auto Posting Enable?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsCheckOutEmailAutoPostingEnable" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsCheckOutEmailAutoPostingEnable" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Check Out Email Auto Posting Enable?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsFrontOfficeIntegrateWithAccounts" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsFrontOfficeIntegrateWithAccounts" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Front Office Integrate With Accounts?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfAdvancePaymentAdjustmentAccountsHeadId" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label14" runat="server" class="control-label" Text="Advance Payment Adjustment Accounts Head"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="AdvancePaymentAdjustmentAccountsHeadId" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfHoldUpAccountsPostingAccountReceivableHeadId" runat="server"></asp:HiddenField>
                                            <asp:Label ID="Label15" runat="server" class="control-label" Text="HoldUp Account Receivable Head"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="HoldUpAccountsPostingAccountReceivableHeadId" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:HiddenField ID="hfFrontOfficeInvoiceRoomServiceName" runat="server"></asp:HiddenField>
                                            <asp:Label ID="lblFrontOfficeInvoiceRoomServiceName" runat="server" class="control-label" Text="Invoice Room Service Name"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="FrontOfficeInvoiceRoomServiceName" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsBillLockAndPreviewEnableForCheckOut" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsBillLockAndPreviewEnableForCheckOut" TabIndex="4" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Bill Lock and Preview Enable For CheckOut?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsMinimumRoomRateCheckingForRoomTypeEnable" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsMinimumRoomRateCheckingForRoomTypeEnable" TabIndex="5" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Minimum Room Rate Checking For Room Type Enable?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsPayrollIntegrateWithFrontOffice" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsPayrollIntegrateWithFrontOffice" TabIndex="5" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Payroll Integrated With Front Office?
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div style="float: left;">
                                            <asp:HiddenField ID="hfIsAutoNightAuditAndApprovalProcessEnable" runat="server"></asp:HiddenField>
                                            <asp:CheckBox ID="IsAutoNightAuditAndApprovalProcessEnable" TabIndex="5" runat="Server" Text="" Font-Bold="true"
                                                CssClass="mycheckbox" TextAlign="right" />
                                            &nbsp;&nbsp;Is Auto Night Audit and Approval Process Enable?
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="btnSaveFOConfig" runat="server" Text="Update" TabIndex="13"
                                            CssClass="btn btn-primary btn-sm" OnClick="btnSaveFOConfig_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="RoomStatusConfEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Room Possible Path Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align label-align">
                                <asp:Label ID="lblUserGroup" runat="server" class="control-label" Text="User Group"></asp:Label>
                            </div>
                            <div class="col-md-4 label-align">
                                <asp:DropDownList ID="ddlUserGroup" runat="server" CssClass="form-control" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align label-align">
                                <asp:Label ID="lblRoomStatus" runat="server" class="control-label" Text="Room Status"></asp:Label>
                            </div>
                            <div class="col-md-4 label-align">
                                <asp:DropDownList ID="ddlRoomStatus" runat="server" CssClass="form-control" TabIndex="3">
                                    <asp:ListItem Text="Vacant" Value="VacantPossiblePath"></asp:ListItem>
                                    <asp:ListItem Text="Reserved" Value="ReservedPossiblePath"></asp:ListItem>
                                    <asp:ListItem Text="Occupaied" Value="OccupiedPossiblePath"></asp:ListItem>
                                    <asp:ListItem Text="Todays Checked In" Value="TodaysCheckedInPossiblePath"></asp:ListItem>
                                    <asp:ListItem Text="Expected Departure" Value="ExpectedDeparturePath"></asp:ListItem>
                                    <asp:ListItem Text="Vacant Dirty" Value="VacantDirtyPossiblePath"></asp:ListItem>
                                    <asp:ListItem Text="Out of Order" Value="OutOfOrderPossiblePath"></asp:ListItem>
                                    <asp:ListItem Text="Out of Service" Value="OutOfServicePossiblePath"></asp:ListItem>
                                    <asp:ListItem Text="Long Staying" Value="LongStayingPossiblePath"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <table id="PossibleActions" class="table table-bordered table-condensed table-responsive">
                                <thead>
                                    <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                                        <th style="width: 10%; text-align: center;">Select<br />
                                            <input type="checkbox" id="chkAll" />
                                        </th>
                                        <th style="width: 30%;">Possible Path
                                        </th>
                                        <th style="width: 30%;">Display Text
                                        </th>
                                        <th style="width: 10%;">Display Sequence
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnApprovePermission" runat="server" Text="Approve Permission" TabIndex="2"
                                    CssClass="btn btn-primary btn-sm" OnClientClick="javascript:return SaveRoomStatusPossiblePath()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3">
            <div id="ColorEntryPanel" class="panel panel-default" runat="server">
                <div class="panel-heading">
                    Color Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">

                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:HiddenField ID="hfRoomVacantDiv" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblRoomVacantDiv" runat="server" class="control-label" Text="Color">Room Vacant</asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="RoomVacantDiv" CssClass="form-control dropdownColor" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:HiddenField ID="hfRoomTodaysCheckInDiv" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblRoomTodaysCheckInDiv" runat="server" class="control-label" Text="Color">Room Todays Check In</asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="RoomTodaysCheckInDiv" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:HiddenField ID="hfRoomOccupaiedDiv" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblRoomOccupaiedDiv" runat="server" class="control-label" Text="Color">Room Occupaied</asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="RoomOccupaiedDiv" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:HiddenField ID="hfRoomPossibleVacantDiv" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblRoomPossibleVacantDiv" runat="server" class="control-label" Text="Color">Room Possible Vacant</asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="RoomPossibleVacantDiv" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:HiddenField ID="hfRoomVacantDirtyDiv" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblRoomVacantDirtyDiv" runat="server" class="control-label" Text="Color">Room Vacant Dirty</asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="RoomVacantDirtyDiv" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:HiddenField ID="hfRoomLongStayingDiv" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblRoomLongStayingDiv" runat="server" class="control-label" Text="Color">Room Long Staying Div</asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="RoomLongStayingDiv" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:HiddenField ID="hfRoomOutOfOrderDiv" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblRoomOutOfOrderDiv" runat="server" class="control-label" Text="Color">Room Out Of Order</asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="RoomOutOfOrderDiv" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:HiddenField ID="hfRoomOutOfServiceDiv" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblRoomOutOfServiceDiv" runat="server" class="control-label" Text="Color">Room Out Of Service</asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="RoomOutOfServiceDiv" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:HiddenField ID="hfRoomReservedDiv" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblRoomReservedDiv" runat="server" class="control-label" Text="Color">Room Reserved</asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="RoomReservedDiv" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnColorConfigurationUpdate" runat="server" Text="Update" TabIndex="13" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnColorConfigurationUpdate_Click" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
