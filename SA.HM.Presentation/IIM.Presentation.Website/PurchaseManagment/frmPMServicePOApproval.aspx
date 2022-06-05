<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmPMServicePOApproval.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmPMServicePOApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Service PO Approval</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });

        $(document).ready(function () {
            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'

            $('#' + txtStartDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });
        });
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="InfoPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Order Information
        </a>
        <div class="HMBodyContainer">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblSPONumber" runat="server" Text="PO Number"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtSPONumber" runat="server" TabIndex="1"></asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="2">
                        <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem>Submit</asp:ListItem>
                        <asp:ListItem>Approved</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtFromDate" CssClass="datepicker" runat="server" TabIndex="3"></asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:TextBox ID="txtToDate" CssClass="datepicker" runat="server" TabIndex="4"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="divClear">
        </div>
        <div class="HMContainerRowButton">
            <%--Right Left--%>
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                TabIndex="5" OnClick="btnSearch_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary"
                OnClientClick="javascript: return PerformClearAction();" TabIndex="6" />
        </div>
    </div>
    <div id="Div1" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
        </a>
        <div class="block-body collapse in">
            <asp:GridView ID="gvOrderInfo" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                OnRowCommand="gvOrderInfo_RowCommand" OnPageIndexChanging="gvOrderInfo_PageIndexChanging"
                TabIndex="9" OnRowDataBound="gvOrderInfo_RowDataBound">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("POrderId") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PONumber" HeaderText="PO Number" ItemStyle-Width="25%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Order Date" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblgvOrderDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("CreatedDate"))) %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Received By Date" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblgvReceivedByDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ReceivedByDate"))) %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                CommandArgument='<%# bind("POrderId") %>' ImageUrl="~/Images/edit.png" Text=""
                                AlternateText="Edit" ToolTip="Edit" />
                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                CommandArgument='<%# bind("POrderId") %>' ImageUrl="~/Images/delete.png" Text=""
                                AlternateText="Delete" ToolTip="Delete" />
                            &nbsp;<asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False"
                                CommandArgument='<%# bind("POrderId") %>' CommandName="CmdOrderPreview" ImageUrl="~/Images/ReportDocument.png"
                                Text="" AlternateText="Bill Preview" ToolTip="Bill Preview" />
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
</asp:Content>
