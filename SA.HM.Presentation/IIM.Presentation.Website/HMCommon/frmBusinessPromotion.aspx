<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBusinessPromotion.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmBusinessPromotion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Business Promotion</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_txtPrbFromHour').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtPrbToHour').timepicker({
                showPeriod: is12HourFormat
            });

            var txtPeriodFrom = '<%=txtPeriodFrom.ClientID%>'
            var txtPeriodTo = '<%=txtPeriodTo.ClientID%>'

            $('#' + txtPeriodFrom).datepicker({
                //defaultDate: "+1w",
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtPeriodTo).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtPeriodTo).datepicker({
                //defaultDate: "+1w",
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtPeriodFrom).datepicker("option", "maxDate", selectedDate);
                }
            });

            var ddlTransactionType = '<%=ddlTransactionType.ClientID%>'
            $('#' + ddlTransactionType).change(function () {
                if ($('#' + ddlTransactionType).val() == "Others") {
                    $('#BankInformationDiv').hide();
                }
                else if ($('#' + ddlTransactionType).val() == "Bank") {
                    $('#BankInformationDiv').show();
                }
            });
        });
        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#<%=txtBPHead.ClientID %>").val(result.BPHead);
            $("#<%=txtPeriodFrom.ClientID %>").val(GetStringFromDateTime(result.PeriodFrom));
            $("#<%=txtPeriodTo.ClientID %>").val(GetStringFromDateTime(result.PeriodTo));
            $("#<%=txtPercentAmount.ClientID %>").val(result.PercentAmount);
            $("#<%=chkIsBPPublic.ClientID%>").prop('checked', result.IsBPPublic);
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=txtBusinessPromotionId.ClientID %>").val(result.BusinessPromotionId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewBank').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
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
            window.location = "frmBusinessPromotion.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtBPHead.ClientID %>").val('');
            var date = new Date();
            $("#<%=txtPeriodFrom.ClientID %>").val(GetStringFromDateTime(date));
            $("#<%=txtPeriodTo.ClientID %>").val(GetStringFromDateTime(date));
            $("#<%=txtPercentAmount.ClientID %>").val('0');
            $("#<%=chkIsBPPublic.ClientID%>").prop('checked', false);
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=txtBusinessPromotionId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");

            var ddlTransactionType = '<%=ddlTransactionType.ClientID%>'
            $('#' + ddlTransactionType).val("Others");
            if ($('#' + ddlTransactionType).val() == "Others") {
                $('#BankInformationDiv').hide();
            }
            else if ($('#' + ddlTransactionType).val() == "Bank") {
                $('#BankInformationDiv').show();
            }
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewPromotion').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewPromotion').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewPromotion').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewPromotion').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Bussiness Promotion</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Promotion </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Business Promotion Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtBusinessPromotionId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblBPHead" runat="server" class="control-label required-field" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtBPHead" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPeriodFrom" runat="server" class="control-label" Text="Date From"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPeriodFrom" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Probable Time"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPrbFromHour" placeholder="12" CssClass="form-control" runat="server"
                                    TabIndex="2"></asp:TextBox><%--&nbsp;:
                                <asp:TextBox ID="txtPrbFromMinute" placeholder="00" CssClass="CustomMinuteSize" TabIndex="3"
                                    runat="server"></asp:TextBox>
                                <asp:DropDownList ID="ddlPrbFromAMPM" CssClass="CustomAMPMSize" runat="server" TabIndex="4">
                                    <asp:ListItem>AM</asp:ListItem>
                                    <asp:ListItem>PM</asp:ListItem>
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPeriodTo" runat="server" class="control-label" Text="Date To"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPeriodTo" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Probable Time"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPrbToHour" placeholder="12" CssClass="form-control" runat="server"
                                    TabIndex="2"></asp:TextBox><%--&nbsp;:
                                <asp:TextBox ID="txtPrbToMinute" placeholder="00" CssClass="CustomMinuteSize" TabIndex="3"
                                    runat="server"></asp:TextBox>
                                <asp:DropDownList ID="ddlPrbToAMPM" CssClass="CustomAMPMSize" runat="server" TabIndex="4">
                                    <asp:ListItem>AM</asp:ListItem>
                                    <asp:ListItem>PM</asp:ListItem>
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPercentAmount" runat="server" class="control-label" Text="Amount(%)"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtPercentAmount" runat="server" CssClass="form-control" TabIndex="4">0</asp:TextBox>
                            </div>
                            <div class="col-md-2" style="padding-right:-20px;">
                                <asp:CheckBox ID="chkIsBPPublic" runat="server" Text="For All" TabIndex="5" />
                            </div>
                            <%--<div class="col-md-1" style="padding-left:0;">
                                <asp:Label ID="lblIsForAll" runat="server" class="control-label" Text="For All"></asp:Label>
                            </div>--%>
                            <div class="col-md-2">
                                <asp:Label ID="lblTransactionType" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control"
                                    TabIndex="6">
                                    <asp:ListItem Value="Others">Others</asp:ListItem>
                                    <asp:ListItem Value="Bank">Bank</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="BankInformationDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblBankId" runat="server" class="control-label required-field" Text="Bank Name"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlBankId" runat="server" CssClass="form-control" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Card Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlCardType" runat="server" TabIndex="8" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control" TabIndex="6">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="7" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="8" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="Div1" class="panel panel-default">
                <div class="panel-heading">
                    Business Promotion Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSName" runat="server" class="control-label" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control" TabIndex="6">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" TabIndex="7" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div lass="panel-body">
                    <asp:GridView ID="gvGuestHouseService" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" OnPageIndexChanging="gvGuestHouseService_PageIndexChanging"
                        OnRowCommand="gvGuestHouseService_RowCommand" OnRowDataBound="gvGuestHouseService_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("BusinessPromotionId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BPHead" HeaderText="Name" ItemStyle-Width="40%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Date From" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvPeriodFrom" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("PeriodFrom")))%>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date To" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvPeriodTo" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("PeriodTo")))%>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ActiveStatus" HeaderText="Status" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("BusinessPromotionId") %>' ImageUrl="~/Images/edit.png"
                                        Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("BusinessPromotionId") %>' ImageUrl="~/Images/delete.png"
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
        var isBankInformationDivEnable = '<%=isBankInformationDivEnable%>';

        if (isBankInformationDivEnable > -1) {
            $('#BankInformationDiv').show();
        }
        else {
            $('#BankInformationDiv').hide();
        }      
    </script>
</asp:Content>
