<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmBillAdjustment.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmBillAdjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Bill Adjustment</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            CommonHelper.ApplyDecimalValidation();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var ddlCostCenterId = '<%=ddlCostCenterId.ClientID%>'
            var ddlServiceId = '<%=ddlServiceId.ClientID%>'
            var ddlBillInformation = '<%=ddlBillInformation.ClientID%>'
            $('#' + ddlCostCenterId).change(function () {
                if ($('#' + ddlCostCenterId).val() == "1000") {
                    $('#FrontOfficeServiceInfo').show();
                }
                else {
                    $('#FrontOfficeServiceInfo').hide();
                }
            });

            $('#' + ddlCostCenterId).change(function () {
                var registrationId = $("#ContentPlaceHolder1_hfddlRegistrationId").val();
                var costcenterId = $("#ContentPlaceHolder1_ddlCostCenterId").val();
                var serviceId = $("#ContentPlaceHolder1_ddlServiceId").val();
                //alert(costcenterId, serviceId);
                PageMethods.LoadBillInfo(registrationId, costcenterId, serviceId, OnLoadBillInfoSucceeded, OnLoadBillInfoFailed);
                return false;
            });

            $('#' + ddlServiceId).change(function () {
                var registrationId = $("#ContentPlaceHolder1_hfddlRegistrationId").val();
                var costcenterId = $("#ContentPlaceHolder1_ddlCostCenterId").val();
                var serviceId = $("#ContentPlaceHolder1_ddlServiceId").val();
                //alert(costcenterId, serviceId);
                PageMethods.LoadBillInfo(registrationId, costcenterId, serviceId, OnLoadBillInfoSucceeded, OnLoadBillInfoFailed);
            });


            $('#' + ddlBillInformation).change(function () {
                var billId = $("#ContentPlaceHolder1_ddlBillInformation").val();
                var costcenterId = $("#ContentPlaceHolder1_ddlCostCenterId").val();
                PageMethods.LoadBillDetailInfo(costcenterId, billId, OnLoadBillDetailInfoSucceeded, OnLoadBillDetailInfoFailed);
                return false;
            });
        });

        function OnLoadBillInfoSucceeded(result) {
            var list = result;
            var controlId = '<%=ddlBillInformation.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">--- Please Select ---</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].TransactionName + '" value="' + list[i].TransactionId + '">' + list[i].TransactionName + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">--- Please Select ---</option>');
                }
            }
        }

        function OnLoadBillInfoFailed() {
        }

        function OnLoadBillDetailInfoSucceeded(result) {
            $("#ContentPlaceHolder1_txtServiceDate").val(result.ServiceDate);
            $("#ContentPlaceHolder1_txtBillNo").val(result.BillNumber);
            $("#ContentPlaceHolder1_txtAmount").val(result.TotalSales);
        }

        function OnLoadBillDetailInfoFailed() {
        }

        function PerformSaveAction() {

            if (!ValidateForm())
                return false;

            var ddlCostCenterId = $("#<%=ddlCostCenterId.ClientID %>").val();
            var ddlBillInformation = $("#<%=ddlBillInformation.ClientID %>").val();
            var adjustmentAmount = $("#<%=txtAdjustmentAmount.ClientID %>").val();
            var txtRemarks = $("#<%=txtRemarks.ClientID %>").val();

            PageMethods.PerformSaveAction(ddlCostCenterId, adjustmentAmount, ddlBillInformation, txtRemarks, OnPerformSaveActionSucceed, OnPerformSaveActionFailed);
            return false;
        }
        function OnPerformSaveActionSucceed(result) {
            toastr.success('Saved Operation Successfull.');
            PerformClearAction();
        }
        function OnPerformSaveActionFailed(error) {
            toastr.error(error.get_message());
        }
        function PerformClearAction() {
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#<%=txtAdjustmentAmount.ClientID %>").val('0');
            $("#<%=ddlCostCenterId.ClientID %>").val('0');
            $("#<%=ddlServiceId.ClientID %>").val('0');
            $("#<%=txtServiceDate.ClientID %>").val('');
            $("#<%=txtBillNo.ClientID %>").val('');
            $("#<%=txtAmount.ClientID %>").val('0');
            $("#<%=txtAdjustmentAmount.ClientID %>").val('0');
            $("#<%=ddlBillInformation.ClientID %>").val('0');

            $("#<%=txtSrcRoomNumber.ClientID %>").val('');
            $("#<%=txtGuestNameInfo.ClientID %>").val('');
            $("#<%=txtRoomTypeInfo.ClientID %>").val('');

            $("#<%=ddlCostCenterId.ClientID %>").val('0');
            $("#<%=ddlBillInformation.ClientID %>").val('0');
            $("#<%=ddlRegistrationId.ClientID %>").val('0');

            $("#<%=txtServiceDate.ClientID %>").val('');
            $("#<%=txtBillNo.ClientID %>").val('');
            $("#<%=txtAmount.ClientID %>").val('');
            $("#<%=txtAdjustmentAmount.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
        }
        function ValidateForm() {
            if ($.trim($("#<%=txtSrcRoomNumber.ClientID %>").val()) == "") {
                toastr.warning("Please Provide Room Number.");
                return false;
            }

            if ($.trim($("#<%=txtAdjustmentAmount.ClientID %>").val()) == "") {
                toastr.warning("Please Provide Adjustment Amount.");
                return false;
            }

            if ($.trim($("#<%=ddlBillInformation.ClientID %>").val()) == "") {
                toastr.warning("Please Provide Bill Information.");
                return false;
            }

            if ($.trim($("#<%=ddlBillInformation.ClientID %>").val()) == "0") {
                toastr.warning("Please Provide Bill Information.");
                return false;
            }


            var billAmount = "", adjustmentAmount = "";

            billAmount = $("#ContentPlaceHolder1_txtAmount").val();
            adjustmentAmount = $("#ContentPlaceHolder1_txtAdjustmentAmount").val();

            if (parseFloat(adjustmentAmount) > parseFloat(billAmount))
            {
                toastr.warning("Please Provide Valid Adjustment Amount.");
                $("#ContentPlaceHolder1_txtAdjustmentAmount").focus();
                return false;
            }

            if ($.trim($("#<%=txtRemarks.ClientID %>").val()) == "") {
                toastr.warning("Please Provide Remarks.");
                $("#<%=txtRemarks.ClientID %>").focus();
                return false;
            }
            return true;
        }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Bill Adjustment Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="RoomNumber" class="control-label col-md-2 required-field">
                        Room Number</label>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2" style="text-align:left; padding-left:0;">
                        <asp:Button ID="btnSrcRoomNumber" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSrcRoomNumber_Click" />
                    </div>
                    <div id="lblRegistrationNumberDiv" runat="server">
                        <label for="RegistrationNumber" class="control-label col-md-2">
                            Registration Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:HiddenField ID="RoomIdForCurrentBillHiddenField" runat="server" />
                        <asp:HiddenField ID="hfddlRegistrationId" runat="server" />
                        <asp:DropDownList ID="ddlRegistrationId" runat="server" TabIndex="3" CssClass="form-control"
                            Enabled="False">
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
                <asp:Panel ID="pnlServiceAdjustmentInfo" runat="server">
                    <div class="form-group">
                        <label for="CostCenter" class="control-label col-md-2">
                            Cost Center</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCostCenterId" runat="server" TabIndex="4" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div id="FrontOfficeServiceInfo">
                            <label for="ServiceName" class="control-label col-md-2">
                                Service Name</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlServiceId" runat="server" TabIndex="4" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="BillInformation" class="control-label col-md-2 required-field">
                            Bill Information</label>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlBillInformation" runat="server" TabIndex="4" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="ServiceDetailInformationDiv">
                        <div class="form-group">
                            <label for="ServiceDate" class="control-label col-md-2">
                                Service Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtServiceDate" TabIndex="6" CssClass="form-control" runat="server"
                                    Enabled="False"></asp:TextBox>
                            </div>
                            <label for="BillNumber" class="control-label col-md-2">
                                Bill Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBillNo" TabIndex="6" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="BillAmount" class="control-label col-md-2">
                                Bill Amount</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAmount" TabIndex="6" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
                            </div>
                            <label for="AdjustmentAmount" class="control-label col-md-2 required-field">
                                Adjustment Amount</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAdjustmentAmount" TabIndex="6" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="Remarks" class="control-label col-md-2 required-field">
                            Remarks</label>
                        <div class="col-md-10">
                            <div>
                                <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                                    TabIndex="8"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnAdjustment" runat="server" Text="Save" TabIndex="21" CssClass="btn btn-primary btn-sm"
                                    OnClientClick="javascript:return PerformSaveAction()" />
                            </div>
                        </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
