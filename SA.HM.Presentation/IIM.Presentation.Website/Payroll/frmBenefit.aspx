<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBenefit.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmBenefit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Benefit</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <asp:HiddenField ID="hfBenefitId" runat="server"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Benefit Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Benefit</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Benefit Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblBenefitName" runat="server" class="control-label" Text="Benefit Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtBenefitName" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="4" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvBenefit" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="30"
                        OnPageIndexChanging="gvBenefit_PageIndexChanging" OnRowDataBound="gvBenefit_RowDataBound"
                        OnRowCommand="gvBenefit_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("BenefitHeadId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BenefitName" HeaderText="Benefit Name" ItemStyle-Width="75%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" OnClientClick="return confirm('Do you want to edit?');" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("BenefitHeadId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("BenefitHeadId") %>' ImageUrl="~/Images/delete.png"
                                        Text="" AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to delete?');" />
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
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
        </div>
    </div>
</asp:Content>
