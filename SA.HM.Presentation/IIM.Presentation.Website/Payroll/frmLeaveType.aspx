<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmLeaveType.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmLeaveType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        fieldset {
            border: 0 none;
            margin-top: 5px;
            padding: 2px;
            padding-top: 5px;
        }
    </style>
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Leave Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Leave Type</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ckbCanCarryForward").click(function () {

                if ($(this).is(':checked') == true) {
                    $("#ContentPlaceHolder1_txtMaxDayCanCarryForwardYearly").attr('disabled', false);
                    $("#ContentPlaceHolder1_txtMaxDayCanKeepAsCarryForwardLeave").attr('disabled', false);
                }
                else if ($(this).is(':checked') == false) {
                    $("#ContentPlaceHolder1_txtMaxDayCanCarryForwardYearly").attr('disabled', true);
                    $("#ContentPlaceHolder1_txtMaxDayCanKeepAsCarryForwardLeave").attr('disabled', true);
                }

                $("#ContentPlaceHolder1_txtMaxDayCanCarryForwardYearly").val("");
                $("#ContentPlaceHolder1_txtMaxDayCanKeepAsCarryForwardLeave").val("");
            });

            $("#ContentPlaceHolder1_ckbCanCash").click(function () {

                if ($(this).is(':checked') == true) {
                    $("#ContentPlaceHolder1_txtMaxDayCanEncash").attr('disabled', false);
                }
                else if ($(this).is(':checked') == false) {
                    $("#ContentPlaceHolder1_txtMaxDayCanEncash").attr('disabled', true);
                }

                $("#ContentPlaceHolder1_txtMaxDayCanEncash").val("");
            });

        });

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {

            $("#<%=txtMaxDayCanCarryForwardYearly.ClientID %>").val(result.CarryForward);
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=txtBankId.ClientID %>").val(result.LeaveTypeId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewBank').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function OnFillFormObjectFailed(error) {
            alert(error.get_message());
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
            window.location = "frmLeaveType.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtLeaveTypeName.ClientID %>").val('');
            $("#<%=txtYearlyLeave.ClientID %>").val('');
            $("#<%=ddlLeaveMode.ClientID %>").val('0');
            $("#<%=txtMaxDayCanCarryForwardYearly.ClientID %>").val('');
            $("#<%=txtMaxDayCanKeepAsCarryForwardLeave.ClientID %>").val('');
            $("#ContentPlaceHolder1_ckbCanCarryForward").prop("checked", false);
            $("#ContentPlaceHolder1_ckbCanCash").prop("checked", false);
            $("#<%=txtMaxDayCanEncash.ClientID %>").val('');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=txtBankId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        function PerformClearActionWithConfirmation() {
            if (!confirm("Do You Want to Clear!"))
                return false
            PerformClearAction();

        }
        function ValidateLeaveType() {

            if ($("#ContentPlaceHolder1_txtLeaveTypeName").val() == "") {
                toastr.warning("Please Give Leave Name.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlLeaveMode").val() == "0") {
                toastr.warning("Please select leave mode.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtYearlyLeave").val() == "") {
                toastr.warning("Please Give Yearly Leave");
                return false;
            }
            else if (CommonHelper.IsInt($("#ContentPlaceHolder1_txtYearlyLeave").val()) == false) {
                toastr.warning("Please Give Valid Yearly Leave");
                return false;
            }

            if ($("#ContentPlaceHolder1_ckbCanCarryForward").is(":checked")) {
                if (CommonHelper.IsInt($("#ContentPlaceHolder1_txtMaxDayCanCarryForwardYearly").val()) == false) {
                    toastr.warning("Please Give Valid Max Day Can Carry Forward Yearly");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtMaxDayCanKeepAsCarryForwardLeave").val() == "") {
                    toastr.warning("Please Give Max Day Can Carry Forward Yearly");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtMaxDayCanKeepAsCarryForwardLeave").val() == "") {
                    toastr.warning("Please Give Max Day Can Keep As Carry Forward Leave.");
                    return false;
                }
            }

            if ($("#ContentPlaceHolder1_ckbCanCash").is(":checked")) {
                if (CommonHelper.IsInt($("#ContentPlaceHolder1_txtMaxDayCanEncash").val()) == false) {
                    toastr.warning("Please Give Valid Max Day Can Carry Forward Yearly");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtYearlyLeave").val() == "") {
                    toastr.warning("Please Give Max Day Can Carry Forward Yearly");
                    return false;
                }
            }

            return true;
        }

    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Leave Type Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Leave Type </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Leave Type Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtBankId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblBankName" runat="server" class="control-label required-field" Text="Leave Type Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtLeaveTypeName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="padding-bottom: 5px;">
                            <div class="col-md-2">
                                <asp:Label ID="lblLeaveTypeId" runat="server" class="control-label required-field"
                                    Text="Leave Mode"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLeaveMode" runat="server" CssClass="form-control" TabIndex="5">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblYearlyLeave" runat="server" class="control-label required-field" Text="Yearly Leave"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtYearlyLeave" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <fieldset>
                            <legend>Carry Forward</legend>
                            <div style="padding-top: 5px;">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ckbCanCarryForward" runat="server" />
                                        &nbsp;
                                    <asp:Label ID="Label1" runat="server" class="control-label" Text="Can Carry Forward"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblCarryForward" runat="server" class="control-label" Text="Max Day Can Carry Forward Yearly"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox runat="server" ID="txtMaxDayCanCarryForwardYearly" Enabled="false" CssClass="form-control"
                                            TabIndex="3">
                                        </asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Max Day Can Keep As Carry Forward Leave"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox runat="server" ID="txtMaxDayCanKeepAsCarryForwardLeave" Enabled="false"
                                            CssClass="form-control" TabIndex="3">
                                        </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Leave Encash</legend>
                            <div style="padding-top: 5px;">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ckbCanCash" runat="server" />&nbsp;
                                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Can Encash"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Max Day Can Encash"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox runat="server" ID="txtMaxDayCanEncash" TabIndex="3" CssClass="form-control"
                                            Enabled="false">
                                        </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Leave Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript:return ValidateLeaveType()" OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return PerformClearActionWithConfirmation();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvGuestHouseService" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" OnPageIndexChanging="gvGuestHouseService_PageIndexChanging"
                        OnRowCommand="gvGuestHouseService_RowCommand" OnRowDataBound="gvGuestHouseService_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("LeaveTypeId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TypeName" HeaderText="Type Name" ItemStyle-Width="37%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="YearlyLeave" HeaderText="Yearly Leave" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MaxDayCanCarryForwardYearly" HeaderText="Carry Forward"
                                ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MaxDayCanEncash" HeaderText="Day Encash" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActiveStatus" HeaderText="Status" ItemStyle-Width="8%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("LeaveTypeId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" OnClientClick="return confirm('Do you want to Edit?');" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("LeaveTypeId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
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
