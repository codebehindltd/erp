<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmInboardED.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmInboardED" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
           var ddlActiveStatValue = $("#<%=ddlActiveStat.ClientID %>").val();
                if (ddlActiveStatValue == "DE") {
                    $('#lblExpireDateFormat').show();
                    $('#IsExipreDateUpdateDiv').show();
                }
                else {
                    $('#lblExpireDateFormat').hide();
                    $('#IsExipreDateUpdateDiv').hide();
                }

            $("#<%=ddlActiveStat.ClientID %>").change(function () {
                var ddlActiveStatValue = $("#<%=ddlActiveStat.ClientID %>").val();
                if (ddlActiveStatValue == "DE") {
                    $('#lblExpireDateFormat').show();
                    $('#IsExipreDateUpdateDiv').show();
                }
                else {
                    $('#lblExpireDateFormat').hide();
                    $('#IsExipreDateUpdateDiv').hide();
                }
            });
        });
    </script>
    <div id="EntryPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Inboard Information
        </a>
        <div class="HMBodyContainer">
            <asp:Panel ID="pnlInnboardCheckingInformation" runat="server">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblInnboardHead" runat="server" Text="Innboard Head"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtInnboardHead" runat="server" CssClass="ThreeColumnTextBox" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnInnboardHeadCheck" runat="server" TabIndex="3" Text="Check" CssClass="TransactionalButton btn btn-primary"
                        OnClick="btnInnboardHeadCheck_Click" />
                    <asp:Button ID="btnInnboardHeadClear" runat="server" TabIndex="4" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                        OnClientClick="javascript: return PerformClearAction();" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlInnboardDetailInformation" runat="server">
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblHeadName" runat="server" Text="Input"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtHeadName" runat="server" CssClass="ThreeColumnTextBox" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblActiveStat" runat="server" Text="Status"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="CustomTextBox" TabIndex="2">
                            <asp:ListItem Value="CS">CS</asp:ListItem>
                            <asp:ListItem Value="ED">ED</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="CustomTextBox" TabIndex="2">
                            <asp:ListItem Value="ED">E=>D</asp:ListItem>
                            <asp:ListItem Value="DE">D=>E</asp:ListItem>
                            <asp:ListItem Value="Clear">Clear</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="IsExipreDateUpdateDiv">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblIsExipreDateUpdate" runat="server" Text="Datebase Update"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftLeft">
                            <asp:DropDownList ID="ddlIsExipreDateUpdate" runat="server" CssClass="CustomTextBox" TabIndex="2">
                                <asp:ListItem Value="0">No</asp:ListItem>
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                            </asp:DropDownList>
                            <span id="lblExpireDateFormat">MM/DD/YYYY</span>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate"
                            CssClass="TransactionalButton btn btn-primary" OnClick="btnGenerate_Click" />
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblHeadValue" runat="server" Text="Output"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtHeadValue" runat="server" CssClass="ThreeColumnTextBox" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
