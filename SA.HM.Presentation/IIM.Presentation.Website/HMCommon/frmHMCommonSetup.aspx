<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmHMCommonSetup.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmHMCommonSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $(".UsingDropDownAsSelectTo").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%"
            });

        });

        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Configuration</a></li>
        </ul>
        <div id="tab-1">
            <div id="PaymentPanelDiv" class="panel panel-default">
                <div class="panel-heading">
                    Payment Mode Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:GridView ID="gvPaymentModeInfo" Width="100%" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                    ForeColor="#333333" PageSize="200"  OnRowDataBound="gvPaymentModeInfo_RowDataBound"
                                    CssClass="table table-bordered table-condensed table-responsive">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="IDNO" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentModeId" runat="server" Text='<%#Eval("PaymentModeId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Mode" ItemStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvTransactionType" runat="server" Text='<%# Bind("DisplayName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="PaymentAccountsPostingId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentAccountsPostingId" runat="server" Text='<%#Eval("PaymentAccountsPostingId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Head" ShowHeader="False" ItemStyle-Width="35%">
                                            <ItemTemplate >
                                                <asp:DropDownList ID="ddlPayment" runat="server" CssClass="form-control UsingDropDownAsSelectTo">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ControlStyle Font-Size="Small" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ReceiveAccountsPostingId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReceiveAccountsPostingId" runat="server" Text='<%#Eval("ReceiveAccountsPostingId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Receive Head" ShowHeader="False" ItemStyle-Width="35%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlReceive" runat="server" CssClass="form-control UsingDropDownAsSelectTo">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ControlStyle Font-Size="Small" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                                    </EmptyDataTemplate>
                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#7C6F57" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnPaymentMode" runat="server" TabIndex="4" Text="Update" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnPaymentMode_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SendingMailPanel" class="panel panel-default">
                <div class="panel-heading">
                    Sending Email Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtSendingMailId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblSendingEmail" runat="server" class="control-label required-field"
                                    Text="Email Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSendingEmail" runat="server" TabIndex="1" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPassword" runat="server" class="control-label required-field" Text="Password"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPassword" TextMode="password" runat="server" TabIndex="2" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSmtpHost" runat="server" class="control-label required-field" Text="Smtp Host"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSmtpPort" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSmtpPort" runat="server" class="control-label required-field" Text="Smtp Port"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSmtpHost" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSend" runat="server" TabIndex="4" Text="Save" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnSend_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="ReceivingMailPanel" class="panel panel-default">
                <div class="panel-heading">
                    Receiving Email Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtReceivingMailId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblReceivingEmail" runat="server" class="control-label" Text="Email Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRecievingEmail" TabIndex="5" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnReceive" runat="server" Text="Save" TabIndex="6" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnReceive_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    Currency Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtSetupId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblCurrencyType" runat="server" class="control-label" Text="Currency Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCurrencyType" runat="server" CssClass="form-control"
                                    TabIndex="7">
                                    <asp:ListItem Text="Single" Value="Single" />
                                    <asp:ListItem Text="Both" Value="Double" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnCurrencySetup" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"
                                    TabIndex="8" OnClick="btnCurrencySetup_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
