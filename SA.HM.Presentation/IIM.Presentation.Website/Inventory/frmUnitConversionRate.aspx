<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmUnitConversionRate.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmUnitConversionRate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Unit Conversion</li>";
            var breadCrumbs = moduleName + formName;

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

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
                $("#<%=txtConversionRate.ClientID %>").attr("disabled", false);
                 var from = $("#<%=ddlFromConversion.ClientID %>").val();
                 var to = $("#<%=ddlToConversion.ClientID %>").val();

                $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                $("#<%=hfConversionRateId.ClientID %>").val(result.ConversionId);

                if (parseFloat(result.ConversionRate) > 0) {
                    if (from == to) {
                        $("#<%=txtConversionRate.ClientID %>").attr("disabled", true);
                    }
                    $("#<%=btnSave.ClientID %>").val('Update');
                }
                else {                   
                    if (from == to) {
                        $("#<%=txtConversionRate.ClientID %>").val(1);
                         $("#<%=txtConversionRate.ClientID %>").attr("disabled", true);
                    }
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
            $("#<%=hfConversionRateId.ClientID %>").val("");
            $("#<%=btnSave.ClientID %>").val('Save');
        }
        function PerformClearActionWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function ValidateForm() {
            var fromConversionHeadId = $("#<%=ddlFromConversion.ClientID %>").val();
            var toConversionHeadId = $("#<%=ddlToConversion.ClientID %>").val();
            var conversionRate = $("#<%=txtConversionRate.ClientID %>").val();
            var isValid = true;
            if (parseInt(fromConversionHeadId) == 0) {
                toastr.info('Please select From Unit Head.');
                isValid = false;
            }
            else if (parseInt(toConversionHeadId) == 0) {
                toastr.info('Please select To Unit Head.');
                isValid = false;
            }
            //else if (parseInt(toConversionHeadId) == parseInt(fromConversionHeadId)) {
            //    toastr.info('From Unit Head And To Unit Head must not be same.');
            //    isValid = false;
            //}
            else if (parseFloat(conversionRate) <= 0) {
                toastr.info('Please provide Conversion Rate.');
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
            Conversion Setup</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group"">
                    <div class="col-md-2">
                        <asp:HiddenField ID="hfConversionRateId" runat="server" />
                        <asp:Label ID="lblFromConversion" runat="server" class="control-label" Text="From Unit Head"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFromConversion" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToConversion" runat="server" class="control-label" Text="To Unit Head"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlToConversion" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group"">
                    <div class="col-md-2">
                        <asp:Label ID="lblConversionRate" runat="server" class="control-label" Text="Conversion Rate"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" TabIndex="4" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return ValidateForm();" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear"  runat="server" TabIndex="5" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript: return PerformClearActionWithConfirmation();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
