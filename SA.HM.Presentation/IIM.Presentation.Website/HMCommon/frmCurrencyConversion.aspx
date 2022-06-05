<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmCurrencyConversion.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmCurrencyConversion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Currency Conversion</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            CommonHelper.ApplyDecimalValidation();
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#<%=ddlFromConversion.ClientID %>").change(function () {
                var from = $("#<%=ddlFromConversion.ClientID %>").val();
                var to = $("#<%=ddlToConversion.ClientID %>").val();
                GetConversionRateByHeadId(from, to);
            });

            $("#<%=ddlToConversion.ClientID %>").change(function () {
                var from = $("#<%=ddlFromConversion.ClientID %>").val();
                var to = $("#<%=ddlToConversion.ClientID %>").val();
                GetConversionRateByHeadId(from, to);
            });

            function GetConversionRateByHeadId(from, to) {
                PageMethods.GetConversionRateByHeadId(from, to, GetConversionRateByHeadIdSucceeded, GetConversionRateByHeadIdFailed);
                return false;
            }

            function GetConversionRateByHeadIdSucceeded(result) {
                $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                $("#<%=txtConversionRateId.ClientID %>").val(result.ConversionId);
                if (parseFloat(result.ConversionRate) > 0) {
                    $("#<%=btnSave.ClientID %>").val('Update');
                }
                else {
                    $("#<%=btnSave.ClientID %>").val('Save');
                }
            }
            function GetConversionRateByHeadIdFailed(error) {
            }

        });

        function PerformClearAction() {
            $("#<%=ddlFromConversion.ClientID %>").val("0");
            $("#<%=ddlToConversion.ClientID %>").val("0");
            $("#<%=txtConversionRate.ClientID %>").val("");
            $("#<%=txtConversionRateId.ClientID %>").val("");
            $("#<%=btnSave.ClientID %>").val('Save');
        }

        function ValidateForm() {
            var fromConversionHeadId = $("#<%=ddlFromConversion.ClientID %>").val();
            var toConversionHeadId = $("#<%=ddlToConversion.ClientID %>").val();
            var conversionRate = $("#<%=txtConversionRate.ClientID %>").val();
            var isValid = true;
            if (parseInt(fromConversionHeadId) == 0) {
                toastr.warning('Please select From Conversion .');
                isValid = false;
            }
            else if (parseInt(toConversionHeadId) == 0) {
                toastr.warning('Please select To Conversion.');
                isValid = false;
            }
            else if (parseInt(toConversionHeadId) == parseInt(fromConversionHeadId)) {
                toastr.warning('From Conversion And To Conversion must not be same.');
                isValid = false;
            }
            else if (parseFloat(conversionRate) <= 0) {
                toastr.warning('Please provide Conversion Rate.');
                isValid = false;
            }
            else {
                isValid = true;
            }
            return isValid;
        }


    </script>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Currency Conversion</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="txtConversionRateId" runat="server" />
                        <asp:Label ID="lblFromConversion" runat="server" class="control-label" Text="From Conversion"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFromConversion" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToConversion" runat="server" class="control-label" Text="To Conversion"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlToConversion" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>                
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblConversionRate" runat="server" class="control-label" Text="Conversion Rate"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="3"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" TabIndex="4" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return ValidateForm();" OnClick="btnSave_Click" />
                    <asp:Button ID="btnClear" runat="server" TabIndex="5" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return PerformClearAction();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
