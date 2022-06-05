<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true" CodeBehind="frmGLHome.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmGLHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        
        function PerformCompanyAction(actionId) {
            PageMethods.GenerateProjectInformation(actionId, OnLoadObjectSucceeded, OnLoadObjectFailed);
            //alert(actionId);

            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#ltlGLProjectInformation").html(result);
            $("#<%=txtIsProjectModalEnable.ClientID %>").val("1");

            var isProjectModalEnable = $("#<%=txtIsProjectModalEnable.ClientID %>").val();
            $("#SelectCompanyModal").modal('hide');
            $("#SelectProjectModal").modal();


            return false;
        }

        function OnLoadObjectFailed(error) {
            alert(error.get_message());
        }
        
    </script>
    <div id="SelectCompanyModal" class="modal hide fade" tabindex="-1" role="dialog"
        aria-labelledby="SelectbuyerteamModalLabel" aria-hidden="true">
        <div class="modal-body" id="SelectCompanyInformation">
            <div id="Div1" class="block">
                <asp:HiddenField ID="txtIsProjectModalEnable" runat="server"></asp:HiddenField>
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Company Information
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvGLCompany" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" PageSize="1" AllowSorting="True" ForeColor="#333333"
                        OnPageIndexChanging="gvGLCompany_PageIndexChanging" OnRowDataBound="gvGLCompany_RowDataBound">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("CompanyId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Company Name" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Code" HeaderText="Company Code" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgSelect" runat="server" CausesValidation="False" CommandName="CmdSelect"
                                        ImageUrl="~/Images/select.png" Text="" AlternateText="Select" ToolTip="Select" />
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
        <br clear="all" />
    </div>
    <div id="SelectProjectModal" class="modal hide fade" tabindex="-1" role="dialog"
        aria-labelledby="SelectbuyerteamModalLabel" aria-hidden="true">
        <div class="modal-body" id="Div4">
            <div id="Div5" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Company Information
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                </a>
                <div class="block-body collapse in">
                    <div id="ltlGLProjectInformation">
                        </div>
                </div>
            </div>
        </div>
        <br clear="all" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var xCompanyModal = '<%=isCompanyModalEnable%>';
            if (xCompanyModal > -1) {
                $("#SelectCompanyModal").modal();
            }

        });
    </script>
</asp:Content>
