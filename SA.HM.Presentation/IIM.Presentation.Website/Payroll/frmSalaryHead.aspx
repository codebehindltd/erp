<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmSalaryHead.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmSalaryHead" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Salary Head</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }            

            $("#ContentPlaceHolder1_ddlNodeId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_txtEffectedMonth").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            document.getElementById("ContentPlaceHolder1_lblSCategory").style.textAlign = "left";
            document.getElementById("ContentPlaceHolder1_lblSalaryCategory").style.textAlign = "left";
        });

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function alert2(message, title, buttonText) {
            buttonText = (buttonText == undefined) ? "Ok" : buttonText;
            title = (title == undefined) ? "The page says:" : title;
            var div = $('<div>');
            div.html(message);
            div.attr('title', title);
            div.dialog({
                autoOpen: true,
                modal: true,
                draggable: false,
                resizable: false,
                buttons: [{
                    text: buttonText,
                    click: function () {
                        $(this).dialog("close");
                        div.remove();
                    }
                }]
            });
        }
        
        function OnFillFormObjectSucceeded(result) {
            if (result.StatusId != 2) {
                $("#<%=txtSalaryHeadId.ClientID %>").val(result.SalaryHeadId);
                $("#<%=txtSalaryHead.ClientID %>").val(result.SalaryHead);
                $("#<%=ddlShowOnlyAllownaceDeductionPage.ClientID %>").val(result.SalaryCategory);
                $("#<%=ddlSalaryType.ClientID %>").val(result.SalaryType);
                $("#<%=ddlTransactionType.ClientID %>").val(result.TransactionType);
                $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
                $("#<%=btnSave.ClientID %>").val("Update");
                $('#btnNewSalaryHead').hide("slow");
                $('#EntryPanel').show("slow");

                if (result.TransactionType == "Yearly") {
                    $("#<%=txtEffectedMonth.ClientID %>").val(result.EffectedMonth);
                    $('#divEffectedMonth').show("slow");
                }
                else {
                    $("#<%=txtEffectedMonth.ClientID %>").val('');
                    $('#divEffectedMonth').hide("slow");
                }
            }
            else {
                alert2("This is not edditable.");
            }
        }

        function OnFillFormObjectFailed(error) {
            alert2(error.get_message());
        }

        //For Delete-------------------------        
        function PerformDeleteAction(actionId) {
            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            window.location = "frmSalaryHead.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert2(error.get_message());
        }

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtSalaryHead.ClientID %>").val('');
            $("#<%=ddlShowOnlyAllownaceDeductionPage.ClientID %>").val('Group');
            $("#<%=ddlSalaryType.ClientID %>").val('Allowance');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=txtSalaryHeadId.ClientID %>").val('');
            $("#<%=ddlTransactionType.ClientID %>").val(0);
            $("#<%=txtEffectedMonth.ClientID %>").val('');
            $('#divEffectedMonth').hide("slow");
            $("#ContentPlaceHolder1_btnSave").val("Save");
            return false;
        }
        function PerformClearActionWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
            return false;
        }

        function CheckValidity() {
            var name = $("#<%=txtSalaryHead.ClientID %>").val();
            var type = $("#<%=ddlSalaryType.ClientID %>").val();
            var nodeId = $("#<%=ddlNodeId.ClientID %>").val();
            var transactionType = $("#<%=ddlTransactionType.ClientID %>").val();

            if (name == "") {
                toastr.warning("Please Enter Salary Head.");
                $("#<%=txtSalaryHead.ClientID %>").focus();
                return false;
            }
            if (type == "0") {
                toastr.warning("Please Select Salary Type.");
                $("#<%=ddlSalaryType.ClientID %>").focus();
                return false;
            }
            if (nodeId == "0") {
                toastr.warning("Please Select Accounts Head.");
                $("#<%=ddlNodeId.ClientID %>").focus();
                return false;
            }
            if (transactionType == "0") {
                toastr.warning("Please Select Transaction Type.");
                $("#<%=ddlTransactionType.ClientID %>").focus();
                return false;
            }
            return true;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewSalaryHead').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }

        function EntryPanelVisibleFalse() {
            $('#btnNewSalaryHead').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewSalaryHead').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewSalaryHead').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Head Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Head </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">Salary Head Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtSalaryHeadId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Salary Head"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSalaryHead" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSalaryType" runat="server" class="control-label required-field" Text="Salary Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSalaryType" runat="server" CssClass="form-control" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTransactionType" runat="server" class="control-label required-field" Text="Transaction Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control"
                                    TabIndex="4">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="Effective">Effective</asp:ListItem>
                                    <asp:ListItem Value="NotEffective">Not Effective</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="AccountHeadId">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Accounts Head"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlNodeId" runat="server" CssClass="form-control"
                                    TabIndex="6">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSalaryCategory" runat="server" class="control-label" Text="Show Only Allownace Deduction Page"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlShowOnlyAllownaceDeductionPage" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="0">Not Show</asp:ListItem>
                                    <asp:ListItem Value="1">Show</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="ContributionTypeDiv">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Contribution Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlContributionType" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="Company">Company</asp:ListItem>
                                    <asp:ListItem Value="Employee">Employee</asp:ListItem>
                                    <asp:ListItem Value="Both">Company & Employee</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="divEffectedMonth" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblEffectedMonth" runat="server" class="control-label" Text="Effected Month"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEffectedMonth" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSave_Click" TabIndex="7" OnClientClick="javascript:return CheckValidity()" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return PerformClearActionWithConfirmation();" TabIndex="8" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">Salary Head Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSSalaryHead" runat="server" class="control-label" Text="Salary Head"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSSalaryHead" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSHeadType" runat="server" class="control-label" Text="Salary Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSHeadType" runat="server" CssClass="form-control" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSTransactionType" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSTransactionType" runat="server" CssClass="form-control"
                                    TabIndex="4">
                                    <asp:ListItem Value="0">--- ALL ---</asp:ListItem>
                                    <asp:ListItem Value="Effective">Effective</asp:ListItem>
                                    <asp:ListItem Value="NotEffective">Not Effective</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSCategory" runat="server" class="control-label" Text="Show Only Allownace Deduction Page"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchShowOnlyAllownaceDeductionPage" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="">--- ALL ---</asp:ListItem>
                                    <asp:ListItem Value="0">Show</asp:ListItem>
                                    <asp:ListItem Value="1">Not Show</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Value="-1">--- ALL ---</asp:ListItem>
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSearch_Click" TabIndex="7" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvSalaryHead" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="30"
                        OnPageIndexChanging="gvSalaryHead_PageIndexChanging" OnRowDataBound="gvSalaryHead_RowDataBound"
                        OnRowCommand="gvSalaryHead_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("SalaryHeadId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SalaryHead" HeaderText="Salary Head" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SalaryType" HeaderText="Salary Type" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TransactionType" HeaderText="Transaction Type" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActiveStatus" HeaderText="Status" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("SalaryHeadId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" OnClientClick="return confirm('Do you want to Edit?');" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("SalaryHeadId") %>' ImageUrl="~/Images/delete.png"
                                        Text="" AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
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
    <script type="text/javascript">
        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
        }
        else {
            NewAddButtonPanelHide();
        }
    </script>
</asp:Content>
