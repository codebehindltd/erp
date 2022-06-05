<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmVoucherSearch.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmVoucherSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Search Voucher</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        $(document).ready(function () {
            $('#ContentPlaceHolder1_gvVoucherInfo_ChkApproved').click(function () {
                if ($('#ContentPlaceHolder1_gvVoucherInfo_ChkApproved').is(':checked')) {
                    CheckAllCheckBox()
                }
                else {
                    UnCheckAllCheckBox();
                }
            });

            var txtFromDate = '<%=txtFromDate.ClientID%>'
            var txtToDate = '<%=txtToDate.ClientID%>'
            $('#' + txtFromDate).datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtToDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtToDate).datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtFromDate).datepicker("option", "maxDate", selectedDate);
                }
            });
        });


        function AddNewItem(value) {
            SetSelectedItem(value);
            $("#<%=txtReportId.ClientID %>").val(value);
            //popup(1, 'TouchKeypad', '', 412, 210);

            $("#TouchKeypad").dialog({
                autoOpen: true,
                modal: true,
                width: 412,
                closeOnEscape: true,
                resizable: false,
                title: "Change Voucher Status",
                show: 'slide'
            });

            return false;
        }
        function SetSelectedItem(dealId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/GeneralLedger/frmVoucherSearch.aspx/SetSelected",
                data: "{'DealId':'" + dealId + "'}",
                dataType: "json",
                success: OnSetSelected,
                error: function (result) {
                    //alert("Error");
                }
            });
        }
        function OnSetSelected(response) {
            $("#<%=ddlChangeStatus.ClientID %>").val(response.d);
        }

        function PerformDeleteAction(actionId) {

            var answer = confirm("Do you want to delete this record?")
            if (answer) {

                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            window.location = "frmVoucherSearch.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }
        
        function PerformClearAction() {
            $("#<%=txtFromDate.ClientID %>").val('');
            $("#<%=txtToDate.ClientID %>").val('');
            $("#<%=txtVoucherNumber.ClientID %>").val('');
            $("#<%=txtDealId.ClientID %>").val('');
            
            return false;
        }        

        function CheckAllCheckBox() {
            $('.chkBox_Approve input[type=checkbox]').each(function () {
                $(this).prop("checked", true);
            });
        }

        function UnCheckAllCheckBox() {
            $('.chkBox_Approve input[type=checkbox]').each(function () {
                $(this).prop("checked", false);
            });
        }
    </script>    
    <div class="HMBodyContainer" id="TouchKeypad" style="display: none;">
        <div class="divSection">
            <div class="divBox divSectionLeftLeft">
                <asp:HiddenField ID="txtReportId" runat="server"></asp:HiddenField>
                <asp:Label ID="lblChangeStatus" runat="server" Text="Change Status"></asp:Label>
            </div>
            <div class="divBox divSectionLeftRight">
                <asp:DropDownList ID="ddlChangeStatus" runat="server" TabIndex="1">
                    <asp:ListItem>Pending</asp:ListItem>
                    <asp:ListItem>Checked</asp:ListItem>
                    <asp:ListItem>Approved</asp:ListItem>
                    <asp:ListItem>Cancel</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="divClear">
        </div>
        <div class="HMContainerRowButton">
            <asp:Button ID="btnChangeStatus" runat="server" Text="Change" CssClass="TransactionalButton btn btn-primary"
                TabIndex="2" OnClick="btnChangeStatus_Click" />
        </div>
    </div>
    <div id="EntryPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Company Information
        </a>
        <div class="HMBodyContainer">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
                    <asp:TextBox ID="txtFromDate" CssClass="datepicker" runat="server" TabIndex="3"></asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:TextBox ID="txtToDate" CssClass="datepicker" runat="server" TabIndex="4"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblVoucherNumber" runat="server" Text="Voucher Number"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtVoucherNumber" runat="server" TabIndex="5"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
        <div class="HMContainerRowButton">
            <%--Right Left--%>
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                CssClass="TransactionalButton btn btn-primary" TabIndex="6" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary"
                OnClientClick="javascript: return PerformClearAction();" TabIndex="7" />
        </div>
    </div>
    <div id="SearchPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
        </a>
        <div class="block-body collapse in">
            <asp:GridView ID="gvVoucherInfo" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="100"
                OnPageIndexChanging="gvVoucherInfo_PageIndexChanging" OnRowDataBound="gvVoucherInfo_RowDataBound"
                OnRowCommand="gvVoucherInfo_RowCommand" TabIndex="9">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("DealId") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CheckedBy" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblCheckedBy" runat="server" Text='<%#Eval("CheckedBy") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ApprovedBy" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblApprovedBy" runat="server" Text='<%#Eval("ApprovedBy") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CreatedBy" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="ChkApproved" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkBox" runat="server" CssClass="chkBox_Approve" Text="" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                CommandArgument='<%# bind("DealId") %>' ImageUrl="~/Images/edit.png" Text=""
                                AlternateText="Edit" ToolTip="Edit" />
                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                ImageUrl="~/Images/delete.png" Text="" AlternateText="Delete" ToolTip="Delete" />
                        </ItemTemplate>
                        <ControlStyle Font-Size="Small" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Voucher Date" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("VoucherDate"))) %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="VoucherNo" HeaderText="Voucher No" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="VoucherType" HeaderText="Type" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <%--  <asp:BoundField DataField="Narration" HeaderText="Narration" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>--%>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblgvGLStatus" runat="server" Text='<%# Eval("GLStatus").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgSelect" runat="server" CausesValidation="False" ImageUrl="~/Images/select.png"
                                Text="" AlternateText="select" ToolTip="select" />
                            &nbsp;<asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False"
                                CommandArgument='<%# bind("DealId") %>' CommandName="CmdPreview" ImageUrl="~/Images/ReportDocument.png"
                                Text="" AlternateText="Details" ToolTip="Details" />
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
        <div class="HMContainerRowButton">
            <%--Right Left--%>
            <asp:Button ID="btnApproved" runat="server" Text="Approved" OnClick="btnApproved_Click"
                CssClass="TransactionalButton btn btn-primary" TabIndex="8" />
        </div>
    </div>    
</asp:Content>
