<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmRoomShift.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmRoomShift" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Change</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            CommonHelper.ApplyDecimalValidation();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var ddlIsCompanyGuest = '<%=ddlIsCompanyGuest.ClientID%>'
            var ddlIsHouseUseRoom = '<%=ddlIsHouseUseRoom.ClientID%>'
            var ddlDiscountType = '<%=ddlDiscountType.ClientID%>'
            var txtDiscountAmount = '<%=txtDiscountAmount.ClientID%>'
            if ($('#' + ddlIsCompanyGuest).val() == "Yes") {
                $('#' + ddlIsHouseUseRoom).attr("disabled", false);
                $('#' + ddlDiscountType).val("Fixed");
                $('#' + ddlDiscountType).attr("disabled", true);
                $('#' + txtDiscountAmount).val("0");
                $('#' + txtDiscountAmount).attr("disabled", true);
            }
            else {
                $('#' + ddlIsHouseUseRoom).val("No");
                $('#' + ddlIsHouseUseRoom).attr("disabled", true);
                $('#' + ddlDiscountType).val("Fixed");
                $('#' + ddlDiscountType).attr("disabled", false);
                $('#' + txtDiscountAmount).val("0");
                $('#' + txtDiscountAmount).attr("disabled", false);
            }

            $('#' + ddlIsCompanyGuest).change(function () {
                if ($('#' + ddlIsCompanyGuest).val() == "Yes") {
                    $('#' + ddlIsHouseUseRoom).attr("disabled", false);
                    $('#' + ddlDiscountType).val("Fixed");
                    $('#' + ddlDiscountType).attr("disabled", true);
                    $('#' + txtDiscountAmount).val("0");
                    $('#' + txtDiscountAmount).attr("disabled", true);
                }
                else {
                    $('#' + ddlIsHouseUseRoom).val("No");
                    $('#' + ddlIsHouseUseRoom).attr("disabled", true);
                    $('#' + ddlDiscountType).val("Fixed");
                    $('#' + ddlDiscountType).attr("disabled", false);
                    $('#' + txtDiscountAmount).val("0");
                    $('#' + txtDiscountAmount).attr("disabled", false);
                }
            });

            var ddlRoomType = '<%=ddlRoomType.ClientID%>'
            $('#' + ddlRoomType).change(function (event) {
                PopulateRooms();
                PerformFillFormActionByTypeId($('#<%=ddlRoomType.ClientID%>').val());
                UpdateTotalCostWithDiscount();
            });

            var ddlDiscountType = '<%=ddlDiscountType.ClientID%>'
            $('#' + ddlDiscountType).change(function (event) {
                var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_ddlDiscountType"), $("#ContentPlaceHolder1_txtDiscountAmount"), $("#ContentPlaceHolder1_txtUnitPrice"), "Discount Amount");
                if (checkValue == false) {
                    return false;
                }
                UpdateTotalCostWithDiscount();
            });

            var ddlRoomId = '<%=ddlRoomId.ClientID%>'
            $('#' + ddlRoomId).change(function (event) {
                $("#<%=ddlRoomIdHiddenField.ClientID %>").val($('#<%=ddlRoomId.ClientID%>').val());
            });

            $("#<%=txtDiscountAmount.ClientID %>").blur(function () {
                var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_ddlDiscountType"), $("#ContentPlaceHolder1_txtDiscountAmount"), $("#ContentPlaceHolder1_txtUnitPrice"), "Discount Amount");
                if (checkValue == false) {
                    return false;
                }
                UpdateTotalCostWithDiscount();
            });
            $("#<%=txtRoomRate.ClientID %>").blur(function () {
                UpdateDiscountAmount();
            });

        });

        function UpdateDiscountAmount() {
            var txtDiscountAmount = '<%=txtDiscountAmount.ClientID%>'
            var txtUnitPrice = '<%=txtUnitPrice.ClientID%>'
            var txtRoomRate = '<%=txtRoomRate.ClientID%>'
            var ddlDiscountType = '<%=ddlDiscountType.ClientID%>'

            var discountType = $('#' + ddlDiscountType).val();
            var unitPrice = 0.0, roomRate = 0.0;

            if ($('#' + txtUnitPrice).val() != "")
                unitPrice = parseFloat($('#' + txtUnitPrice).val());

            if ($('#' + txtRoomRate).val() != "")
                roomRate = parseFloat($('#' + txtRoomRate).val());

            var discount = 0;
            if (discountType == "Fixed") {
                discount = unitPrice - roomRate;
            }
            else {
                var diff = (unitPrice - roomRate) * 100;
                discount = diff / unitPrice;
            }

            var isCheckMinimumRoomRate = $("#<%=hfIsMinimumRoomRateCheckingEnable.ClientID %>").val() == "1";

            if (isCheckMinimumRoomRate) {
                var minimumRoomRate = parseFloat($("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val());

                if (toFixed(roomRate, 2) < toFixed(minimumRoomRate, 2)) {
                    
                    toastr.warning(`Minimum Room Rate For ${$("#<%=ddlRoomType.ClientID %> :selected").text()} : ${minimumRoomRate}`);
                    $('#' + txtRoomRate).val(minimumRoomRate).blur().focus();
                    return;
                }
            }

            $('#' + txtDiscountAmount).val(discount.toFixed(2));

        }

        function PopulateRooms() {
            $('#<%=ddlEntitleRoomType.ClientID%>').val($('#<%=ddlRoomType.ClientID%>').val());
            if ($('#<%=ddlRoomType.ClientID%>').val() == 0) {
                $("#<%=ddlRoomId.ClientID%>").attr("disabled", "disabled");
            }
            else {
                $("#<%=ddlRoomId.ClientID%>").attr("disabled", false);
            }

            $('#<%=ddlRoomId.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            var StartDate = $('#<%=txtCheckInDateHiddenField.ClientID%>').val();
            var EndDate = $('#<%=txtDepartureDateHiddenField.ClientID%>').val();
            $.ajax({
                type: "POST",
                url: "/HotelManagement/frmRoomShift.aspx/PopulateRooms",
                data: '{RoomTypeId: ' + $('#<%=ddlRoomType.ClientID%>').val() + ',ResevationId:0,FromDate:"' + StartDate + '",ToDate:"' + EndDate + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnRoomPopulated,
                failure: function (response) {
                    toastr.info(response.d);
                }
            });
        }
        function OnRoomPopulated(response) {
            var ddlRoomId = '<%=ddlRoomId.ClientID%>'
            PopulateControlWithOutDefault(response.d, $("#<%=ddlRoomId.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
        }
        function PerformFillFormActionByTypeId(actionId) {
            PageMethods.PerformFillFormActionByTypeId(actionId, OnFillFormObjectSucceededByTypeId, OnFillFormObjectFailedByTypeId);
            return false;
        }
        function OnFillFormObjectSucceededByTypeId(result) {
            $("#<%=ddlDiscountType.ClientID %>").val('Fixed');
            $("#<%=txtDiscountAmount.ClientID %>").val('0');
            $("#<%=txtUnitPrice.ClientID %>").val('0');
            $("#<%=txtUnitPriceHiddenField.ClientID %>").val('0');
            $("#<%=txtRoomRate.ClientID %>").val('0');

            var hfRoomType = $("#<%=HiddenFieldRoomType.ClientID %>").val();
            var hfDiscountType = $("#<%=HiddenFieldDiscountType.ClientID %>").val();
            var hfDiscountAmount = $("#<%=HiddenFieldDiscountAmount.ClientID %>").val();
            var hfRoomRate = $("#<%=HiddenFieldRoomRate.ClientID %>").val();
            var hfUnitPrice = $("#<%=HiddenFieldUnitPrice.ClientID %>").val();

            var roomType = $("#<%=ddlRoomType.ClientID %>").val();

            if (hfRoomType != roomType) {
                var answer = confirm("Do you want to recalculate Room Rent?")
                if (answer) {
                    if ($("#<%=ddlCurrencyHiddenField.ClientID %>").val() == '1') {
                        $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRate);
                        $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRate);
                        $("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val(result.MinimumRoomRate);   
                        $("#<%=txtRoomRate.ClientID %>").val(result.RoomRate);
                    }
                    else {
                        $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRateUSD);
                        $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRateUSD);
                        $("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val(result.MinimumRoomRateUSD);
                        $("#<%=txtRoomRate.ClientID %>").val(result.RoomRateUSD);
                    }
                    UpdateTotalCostWithDiscount();
                }
                else {
                    $("#<%=ddlDiscountType.ClientID %>").val(hfDiscountType);
                    $("#<%=txtDiscountAmount.ClientID %>").val(hfDiscountAmount);

                    <%--if ($("#<%=ddlCurrencyHiddenField.ClientID %>").val() == '1') {
                        $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRate);
                        $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRate);
                    }
                    else {
                        $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRateUSD);
                        $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRateUSD);
                    }--%>

                    $("#<%=txtRoomRate.ClientID %>").val(hfRoomRate);
                    $("#<%=txtUnitPrice.ClientID %>").val(hfUnitPrice);
                    $("#<%=txtUnitPriceHiddenField.ClientID %>").val(hfUnitPrice);
                }
            }
            else {
                $("#<%=ddlDiscountType.ClientID %>").val(hfDiscountType);
                $("#<%=txtDiscountAmount.ClientID %>").val(hfDiscountAmount);
                if ($("#<%=ddlCurrencyHiddenField.ClientID %>").val() == '1') {
                        $("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val(result.MinimumRoomRate); 
                    }
                    else {
                        $("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val(result.MinimumRoomRateUSD);
                    }
                $("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val(result.MinimumRoomRate);
                $("#<%=txtRoomRate.ClientID %>").val(hfRoomRate);
                $("#<%=txtUnitPrice.ClientID %>").val(hfUnitPrice);
                $("#<%=txtUnitPriceHiddenField.ClientID %>").val(hfUnitPrice);
            }

            return false;
        }
        function OnFillFormObjectFailedByTypeId(error) {
            toastr.error(error.get_message());
        }
        function GetTotalCostWithCompanyOrPersonalDiscount() {
            var promId = $("#<%=ddlBusinessPromotionIdHiddenField.ClientID %>").val();
            var companyId = $("#<%=ddlCompanyNameHiddenField.ClientID %>").val();

            if (companyId <= 0) { companyId = 0; }
            if (promId <= 0) { promId = 0; }

            if (companyId != 0 || promId != 0) {
                PageMethods.GetCalculatedDiscount(companyId, promId, GetCalculatedDiscountObjectSucceeded, GetCalculatedDiscountObjectFailed);
                return false;
            }
            else {
                return true;
            }
        }

        function GetCalculatedDiscountObjectSucceeded(result) {
            var prevDiscount = parseFloat($("#<%=txtDiscountAmount.ClientID %>").val());
            if (isNaN(prevDiscount)) {
                prevDiscount = 0;
            }
            $("#<%=ddlDiscountType.ClientID %>").val('Percentage');
            var resultFloat = parseFloat(result.DiscountPercent);
            if (resultFloat > prevDiscount) {
                $("#<%=txtDiscountAmount.ClientID %>").val(resultFloat);
            }
            else {
                $("#<%=txtDiscountAmount.ClientID %>").val(prevDiscount);
            }
            UpdateDiscountAmount();
            UpdateTotalCostWithDiscount();
            return false;
        }
        function GetCalculatedDiscountObjectFailed(error) {
            toastr.error(error.get_message());
        }
        function UpdateTotalCostWithDiscount() {
            var discountAmount = $("#<%=txtDiscountAmount.ClientID %>").val();
            if ($("#<%=txtDiscountAmount.ClientID %>").val() == "")
            {
                $("#<%=txtDiscountAmount.ClientID %>").val("0");
                discountAmount = 0;
            }

            var FinalAmount = 0.00;
            var discountType = $("#<%=ddlDiscountType.ClientID %>").val();
            var txtUnitPrice = $("#<%=txtUnitPrice.ClientID %>").val();
            var txtUnitPriceHiddenField = $("#<%=txtUnitPriceHiddenField.ClientID %>").val();
            if (discountAmount != '') {
                if (discountType == 'Fixed') {
                    FinalAmount = parseFloat(txtUnitPriceHiddenField) - parseFloat(discountAmount);
                    $("#<%=txtRoomRate.ClientID %>").val(FinalAmount);
                }
                else {
                    if (txtUnitPriceHiddenField != '') {
                        var percentage = parseFloat(txtUnitPriceHiddenField) * parseFloat(discountAmount) / 100;
                        FinalAmount = parseFloat(txtUnitPriceHiddenField) - percentage;
                        $("#<%=txtRoomRate.ClientID %>").val(FinalAmount);
                    }
                    else {
                        $("#<%=txtRoomRate.ClientID %>").val('0');
                    }
                }
                var isCheckMinimumRoomRate = $("#<%=hfIsMinimumRoomRateCheckingEnable.ClientID %>").val() == "1";

                if (isCheckMinimumRoomRate) {
                    var minimumRoomRate = parseFloat($("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val());
                    
                    if (toFixed(FinalAmount, 2) < toFixed(minimumRoomRate, 2)) {
                        var actualRoomRate = parseFloat(txtUnitPrice);
                        var maximumDiscount = actualRoomRate - minimumRoomRate;
                        toastr.warning(`Minimum Room Rate For ${$("#<%=ddlRoomType.ClientID %> :selected").text()} : ${minimumRoomRate}. Discount Amount Cannot Greater than ${maximumDiscount}.`);
                        $('#<%=txtDiscountAmount.ClientID%>').val(maximumDiscount).trigger('blur').focus();
                        return true;
                    }
                }
            }
        }
    </script>
    <asp:HiddenField ID="hfIsMinimumRoomRateCheckingEnable" runat="server"></asp:HiddenField>   
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="RoomNumber" class="control-label col-md-2">
                        Room Number</label>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2" style="text-align: left; padding-left: 0;">
                        <asp:Button ID="btnSrcRoomNumber" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSrcRoomNumber_Click" />
                    </div>
                    <label for="RoomNumber" class="control-label col-md-2">
                        Registration Number</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRegistrationId" runat="server" TabIndex="3" CssClass="form-control"
                            Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <asp:HiddenField ID="HiddenFieldGuestID" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HiddenFieldRoomType" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HiddenFieldDiscountType" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HiddenFieldDiscountAmount" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HiddenFieldRoomRate" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HiddenFieldUnitPrice" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblRoomNumber" runat="server" class="control-label required-field" Text="Room Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRooms" runat="server" TabIndex="4" CssClass="form-control"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlRooms_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label for="GuestName" class="control-label col-md-2">
                        Guest Name</label>
                    <div class="col-md-10">
                        <asp:HiddenField ID="txtCheckInDateHiddenField" runat="server" />
                        <asp:HiddenField ID="txtDepartureDateHiddenField" runat="server" />
                        <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server" />
                        <asp:HiddenField ID="ddlBusinessPromotionIdHiddenField" runat="server" />
                        <asp:HiddenField ID="ddlCompanyNameHiddenField" runat="server" />
                        <asp:TextBox ID="txtGuestNameInfo" TabIndex="5" runat="server" CssClass="form-control"
                            Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="RoomType" class="control-label col-md-2">
                        Room Type</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRoomTypeInfo" TabIndex="6" runat="server" CssClass="form-control"
                            Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" runat="server" id="ComplimentaryGuestDisplayDiv">
                    <label for="ComplimentaryGuest" class="control-label col-md-2">
                        Complimentary Guest</label>
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsCompanyGuestDisplay" runat="server" CssClass="form-control" TabIndex="70" Enabled="False">
                                    <asp:ListItem>No</asp:ListItem>
                                    <asp:ListItem>Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4 col-padding-left-none">
                                <asp:Label ID="lblIsHouseUseRoomDisplay" runat="server" class="control-label" Text="House Use"></asp:Label>
                            </div>
                            <div class="col-md-4 col-padding-left-none">
                                <asp:DropDownList ID="ddlIsHouseUseRoomDisplay" runat="server" CssClass="form-control" TabIndex="70" Enabled="False">
                                    <asp:ListItem>No</asp:ListItem>
                                    <asp:ListItem>Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="ShiftPanel" class="panel panel-default">
        <div class="panel-heading">
            Room Change Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="RoomType" class="control-label col-md-2 required-field">
                        Room Type</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRoomType" runat="server" TabIndex="7" CssClass="form-control">
                        </asp:DropDownList>
                        <div style="display: none">
                            <asp:TextBox ID="txtViewRoomType" runat="server" CssClass="form-control" TabIndex="3"
                                Visible="false"></asp:TextBox>
                        </div>
                    </div>
                    <label for="RoomNumber" class="control-label col-md-2 required-field">
                        Room Number</label>
                    <div class="col-md-4">
                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtEntiteledRoomType" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="ddlRoomIdHiddenField" runat="server"></asp:HiddenField>
                        <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="form-control" TabIndex="8"
                            Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="display: none;">
                    <label for="EntitleRoomType" class="control-label col-md-2">
                        Entitle Room Type</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlEntitleRoomType" runat="server" TabIndex="9" CssClass="form-control"
                            Visible="false">
                        </asp:DropDownList>
                    </div>
                    <label for="EntitleRoomRate" class="control-label col-md-2">
                        Entitle Room Rate</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEntitleRoomRate" runat="server" CssClass="form-control" TabIndex="10"
                            Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="ComplimentaryGuest" class="control-label col-md-2">
                        Complimentary Guest</label>
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsCompanyGuest" runat="server" CssClass="form-control" TabIndex="70">
                                    <asp:ListItem>No</asp:ListItem>
                                    <asp:ListItem>Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4 col-padding-left-none">
                                <asp:Label ID="lblIsHouseUseRoom" runat="server" class="control-label" Text="House Use"></asp:Label>
                            </div>
                            <div class="col-md-4 col-padding-left-none">
                                <asp:DropDownList ID="ddlIsHouseUseRoom" runat="server" CssClass="form-control" TabIndex="70">
                                    <asp:ListItem>No</asp:ListItem>
                                    <asp:ListItem>Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <label for="DiscountType" class="control-label col-md-2">
                        Discount Type</label>
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-5">
                                <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control" TabIndex="11">
                                    <asp:ListItem>Fixed</asp:ListItem>
                                    <asp:ListItem>Percentage</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3 col-padding-left-none">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Amount"></asp:Label>
                            </div>
                            <div class="col-md-4 col-padding-left-none">
                                <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="form-control quantitydecimal" TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label for="UnitPrice" class="control-label col-md-2">
                        Unit Price</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" TabIndex="13"
                            Enabled="false"></asp:TextBox>
                        <asp:HiddenField ID="txtMinimumUnitPriceHiddenField" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtUnitPriceHiddenField" runat="server"></asp:HiddenField>
                    </div>
                    <label for="CalculatedRoomRate" class="control-label col-md-2">
                        Calculated Room Rate</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRoomRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="14"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="Remarks" class="control-label col-md-2">
                        Remarks</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                            TabIndex="15"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnShift" runat="server" TabIndex="15" Text="Room Change" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnShift_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
