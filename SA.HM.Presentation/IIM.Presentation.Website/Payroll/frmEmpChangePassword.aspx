<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmEmpChangePassword.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>HR Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Change Password</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        //For Password Validation-------------------------
        function confirmPass() {
            if ($("#<%=txtEmpPassword.ClientID %>").val() != $("#<%=txtEmpConfirmPassword.ClientID %>").val()) {
                alert('Passwords do not match.');
                $("#<%=txtEmpConfirmPassword.ClientID %>").focus();
            }
        }        
        
    </script>
    <div id="EntryPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">My Account </a>
        <div class="HMBodyContainer">
            <div class="block-body collapse in">
                <div class="HMContainerRow" style="display: none">
                    <div class="l-left">
                        <%--Right Left--%>
                        <asp:Label ID="lblUserName" runat="server" Text="Name"></asp:Label>
                    </div>
                    <div class="r-left">
                        <%--Left Right--%>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="ThreeColumnTextBox" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        Employee Code</div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtEmpCode" runat="server" Visible="true" CssClass="customMediumTextBoxSize"
                            TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblOldUserPassword" runat="server" Text="Old Password"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtOldEmpPassword" runat="server" CssClass="customMediumTextBoxSize"
                            TabIndex="2" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblUserPassword" runat="server" Text="New Password"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtEmpPassword" runat="server" CssClass="customMediumTextBoxSize"
                            TabIndex="4" TextMode="Password"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblUserConfirmPassword" runat="server" Text="Confirm Password"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtEmpConfirmPassword" runat="server" onblur="javascript: return confirmPass();"
                            CssClass="customMediumTextBoxSize" TabIndex="3" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRow" style="display: none">
                    <div class="left-float">
                        <div class="l-left">
                            <%--Left Left--%>
                            <asp:Label ID="lblUserEmail" runat="server" Text="User Email"></asp:Label>
                        </div>
                        <div class="r-right">
                            <%--Right Left--%>
                            <asp:TextBox ID="txtUserPhone" runat="server" CssClass="customMediumTextBoxSize"
                                TabIndex="7"></asp:TextBox>
                        </div>
                    </div>
                    <div class="right-float">
                        <div class="r-left">
                            <%--Right Left--%>
                            <asp:TextBox ID="txtUserEmail" runat="server" CssClass="customMediumTextBoxSize"
                                TabIndex="6"></asp:TextBox>
                        </div>
                        <div class="l-right">
                            <%--Left Right--%>
                            <asp:Label ID="lblUserPhone" runat="server" Text="User Phone"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRow">
                    <div class="l-left">
                        <%--Left Left--%>
                    </div>
                    <div class="r-left">
                        <%--Right Left--%>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnSave" runat="server" Text="Update" OnClick="btnSave_Click" CssClass="btn btn-primary"
                        TabIndex="9" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary"
                        OnClientClick="javascript: return PerformClearAction();" TabIndex="10" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
