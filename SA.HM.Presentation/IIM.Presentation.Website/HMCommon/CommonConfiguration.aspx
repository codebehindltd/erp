<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="CommonConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.CommonConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        if ($("#InnboardMessageHiddenField").val() != "") {
            CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
            $("#InnboardMessageHiddenField").val("");
        }
    </script>

    <div class="row">
        <div class="col-md-12">
            <div id="ConfigurationSettingPanel" class="panel panel-default" runat="server">
                <div class="panel-heading">
                    Common Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">

                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:HiddenField ID="hfApprovalPolicyConfiguration" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblApprovalPolicyConfiguration" runat="server" class="control-label" Text="Approval Policy"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ApprovalPolicyConfiguration" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="0"> OR Configuration </asp:ListItem>
                                    <asp:ListItem Value="1"> AND Configuration </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-12">
                                <div style="float: left;">
                                    <asp:HiddenField ID="hfIsBankIntegratedWithAccounts" runat="server"></asp:HiddenField>
                                    <asp:CheckBox ID="IsBankIntegratedWithAccounts" TabIndex="17" runat="Server" Text="" Font-Bold="true"
                                        CssClass="mycheckbox" TextAlign="right" />
                                    &nbsp;&nbsp;Is Bank Integrated With Accounts?
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnUpdateSettings" runat="server" Text="Update" CssClass="btn btn-primary"
                                    TabIndex="2" OnClick="btnUpdateSettings_Click" />
                            </div>
                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
