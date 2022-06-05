<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmFiscalYearClosing.aspx.cs"
    Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmFiscalYearClosing" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_hfCompanyAll").val() != "") {
                ComanyList = JSON.parse($("#ContentPlaceHolder1_hfCompanyAll").val());               

                var company = _.findWhere(ComanyList, { CompanyId: parseInt($("#ContentPlaceHolder1_ddlGLCompany").val()) });

                if (company != null) {
                    if (company.IsProfitableOrganization) {
                        $("#DonorContainer").hide();
                    }
                    else { $("#DonorContainer").show(); }
                }
            }

            $("#ContentPlaceHolder1_ddlGLCompany").change(function () {

                var company = _.findWhere(ComanyList, { CompanyId: parseInt($(this).val()) });

                if (company != null) {

                    if (company.IsProfitableOrganization) {
                        $("#DonorContainer").hide();
                    }
                    else { $("#DonorContainer").show(); }
                }
            });

        });

        function CheckValidation() {
            if ($("#ContentPlaceHolder1_ddlFiscalYear").val() == "0") {
                toastr.warning("Please Select Fiscal Year");
                return false;
            }
        }

    </script>
    <asp:HiddenField ID="SingleprojectId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyAll" runat="server" />

    <div class="panel panel-default">
        <div class="panel-heading">Year Closing Information</div>
        <div class="panel panel-body">

            <div class="form-horizontal">
                <div class="form-group">

                    <label class="control-label required-field col-md-2">Company</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGLCompany" CssClass="form-control" runat="server" onchange="PopulateProjects();">
                        </asp:DropDownList>
                    </div>
                    <label class="control-label required-field col-md-2">Project</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGLProject" CssClass="form-control" runat="server" onchange="PopulateFiscalYear();">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group" id="DonorContainer">
                    <label class="control-label required-field col-md-2">Donor</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDonor" runat="server" CssClass="form-control" TabIndex="3">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label required-field col-md-2">Fiscal Year</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btinFiscalYearCLosing" runat="server" Text="Fiscal Year Closing" CssClass="btn btn-primary btn-large btn-sm" OnClick="btinFiscalYearCLosing_Click" OnClientClick="javascript:return CheckValidation()" />
                </div>
            </div>

        </div>
    </div>

</asp:Content>
