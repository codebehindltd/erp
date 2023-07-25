<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmCurrencyTransaction.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmCurrencyTransaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Currency Transaction </li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            CommonHelper.ApplyDecimalValidation();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#<%=txtMoneyAmount.ClientID %>").change(function () {
                ConvertCorrency();
            });

            var ddlGuestType = '<%=ddlGuestType.ClientID%>'

            if ($('#' + ddlGuestType).val() == "InHouseGuest") {
                InhousePanelVisibleTrue();
            }
            else if ($('#' + ddlGuestType).val() == "OutSideGuest") {
                InhousePanelVisibleFalse();
            }

            $('#' + ddlGuestType).change(function () {
                <%--$('#' + btnSave).val('Save');
                var txtSrcRoomNumber = '<%=txtSrcRoomNumber.ClientID%>'
                $("#" + txtSrcRoomNumber).attr("disabled", false);

                var btnSrcRoomNumber = '<%=btnSrcRoomNumber.ClientID%>'
                $("#" + btnSrcRoomNumber).attr("disabled", false);--%>

                if ($('#' + ddlGuestType).val() == "InHouseGuest") {
                    InhousePanelVisibleTrue();
                }
                else if ($('#' + ddlGuestType).val() == "OutSideGuest") {
                    InhousePanelVisibleFalse();
                }
            });

            $('#ContentPlaceHolder1_gvConversionInfo_ChkAllSelect').click(function () {
                if ($('#ContentPlaceHolder1_gvConversionInfo_ChkAllSelect').is(':checked')) {
                    CheckAllCheckBoxCreate()
                }
                else {
                    UnCheckAllCheckBoxCreate();
                }
            });

            $('#myTabs').tabs({
                select: function (event, ui) {
                    var theSelectedTab = ui.index;
                    if (theSelectedTab == 0) {
                        //alert("0");
                        $('#SearchPanel').hide();
                    }
                    else if (theSelectedTab == 1) {
                        //alert("1");
                    }
                }
            });


            $("#<%=ddlFromConversion.ClientID %>").change(function () {
                var from = $("#<%=ddlFromConversion.ClientID %>").val();
                var to = $("#<%=ddlToConversion.ClientID %>").val();
                GetConversionRateByHeadId(from, to);
            });

            $("#<%=ddlToConversion.ClientID %>").change(function () {
                var from = $("#<%=ddlFromConversion.ClientID %>").val();
                var to = $("#<%=ddlToConversion.ClientID %>").val();
                GetConversionRateByHeadId(from, to);
            });

            function InhousePanelVisibleTrue() {
                $('#ContentPlaceHolder1_txtGuestName').attr('readonly', true);
                $('#ContentPlaceHolder1_txtCountryName').attr('readonly', true);
                $('#ContentPlaceHolder1_txtPassportNumber').attr('readonly', true);
                $('#InHouseGuestSearchInfo').show();
                return false;
            }
            function InhousePanelVisibleFalse() {
                $('#ContentPlaceHolder1_txtGuestName').attr('readonly', false);
                $('#ContentPlaceHolder1_txtCountryName').attr('readonly', false);
                $('#ContentPlaceHolder1_txtPassportNumber').attr('readonly', false);
                $('#InHouseGuestSearchInfo').hide();
                return false;
            }

            function GetConversionRateByHeadId(from, to) {
                PageMethods.GetConversionRateByHeadId(from, to, GetConversionRateByHeadIdSucceeded, GetConversionRateByHeadIdFailed);
                return false;
            }

            function GetConversionRateByHeadIdSucceeded(result) {
                $("#<%=hfConversionRate.ClientID %>").val(result.ConversionRate);
                $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                $("#<%=txtConversionRateId.ClientID %>").val(result.ConversionRateId);
                ConvertCorrency();
            }
            function GetConversionRateByHeadIdFailed(error) {
            }

            function ConvertCorrency() {
                var rate = $("#<%=txtConversionRate.ClientID %>").val();
                var amount = $("#<%=txtMoneyAmount.ClientID %>").val();

                var fromConversionHeadId = $("#<%=ddlFromConversion.ClientID %>").val();
                var toConversionHeadId = $("#<%=ddlToConversion.ClientID %>").val();

                if (parseInt(toConversionHeadId) == parseInt(fromConversionHeadId)) {
                    $("#<%=hfConversionRate.ClientID %>").val('1');
                    $("#<%=txtConversionRate.ClientID %>").val('1');
                    $("#<%=txtTotalAmount.ClientID %>").val(amount);
                }
                else {

                    if (parseFloat(rate) > 0 && parseFloat(amount) > 0) {
                        var totalAmount = parseFloat(rate) * parseFloat(amount);
                        $("#<%=txtTotalAmount.ClientID %>").val(totalAmount);
                    }
                }
            }

            $("#<%=txtConversionRate.ClientID %>").change(function () {
                ConvertCorrency();
            });

            var txtFromDate = '<%=txtFromDate.ClientID%>'
            var txtToDate = '<%=txtToDate.ClientID%>'

            $('#' + txtFromDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtToDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtToDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtFromDate).datepicker("option", "maxDate", selectedDate);
                }
            });

        });

        function CheckAllCheckBoxCreate() {
            $('.Chk_Select input[type=checkbox]').each(function () {
                $(this).prop("checked", true);
            });
        }

        function UnCheckAllCheckBoxCreate() {
            $('.Chk_Select input[type=checkbox]').each(function () {
                $(this).prop("checked", false);
            });
        }

        function EntryPanelVisibleTrue() {
            $('#btnNewRoomType').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewRoomType').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        function PerformClearAction() {
            $("#<%=ddlFromConversion.ClientID %>").val("0");
            $("#<%=ddlToConversion.ClientID %>").val("0");
            $("#<%=txtConversionRate.ClientID %>").val("");
            $("#<%=txtConversionRateId.ClientID %>").val("");

            $("#<%=txtMoneyAmount.ClientID %>").val("");
            $("#<%=txtTotalAmount.ClientID %>").val("");
        }
        $(function () {
            $("#myTabs").tabs();
        });

        function LoadPopUp() {
            //popup(1, 'DivItemSelect', '', 600, 500);
            $("#DivItemSelect").dialog({
                width: 600,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", //TODO add title
                show: 'slide'
            });
            return false;
        }

        function PaymentPreview() {
            var paymentIdList = "";
            $("#ContentPlaceHolder1_gvConversionInfo tbody tr").each(function () {
                if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                    var id = $(this).find("td:eq(1)").text();
                    if (paymentIdList == "") {
                        paymentIdList = id;
                    }
                    else {
                        paymentIdList += ',' + id;
                    }
                }
            });
            if (paymentIdList != "") {
                var url = "/HMCommon/Reports/frmReportCommonCurrencyConversion.aspx?ConversionIdList=" + paymentIdList;
                var popup_window = "Preview";
                window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
            }
            else {
                toastr.warning('Select payments to preview');
            }
        }
    </script>
    <asp:HiddenField ID="hfConversionRate" runat="server" />
    <asp:HiddenField ID="hfRegistrationId" runat="server" />
    <div id="DivItemSelect" style="display: none;">
        <div id="Div1" class="panel-body">
            <asp:GridView ID="gvConversionInfo" Width="100%" runat="server" AllowPaging="True"
                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                ForeColor="#333333" PageSize="100" CssClass="table table-bordered table-condensed table-responsive">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="Select" ItemStyle-Width="05%">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ChkAllSelect" CssClass="ChkAllSelect" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkSelect" CssClass="Chk_Select" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IDNO">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("CurrencyConversionId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TransactionNumber" HeaderText="Transaction Number" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ConversionAmount" HeaderText="Conversion Amount" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ConversionRate" HeaderText="Conversion Rate" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ConvertedAmount" HeaderText="Converted Amount" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
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
            <div class="divClear">
            </div>
            <asp:Button ID="btnPreview" runat="server" Text="Preview" TabIndex="3" CssClass="TransactionalButton btn btn-primary"
                OnClientClick="javascript: return PaymentPreview();" />
        </div>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Currency Conversion</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Currency Conversion</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">Currency Conversion</div>
                <div class="form-horizontal">
                    <div class="panel-body">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtCommonConversionId" runat="server" />
                                <asp:HiddenField ID="txtConversionRateId" runat="server" />
                                <asp:Label ID="lblFromConversion" runat="server" class="control-label" Text="From Currency"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlFromConversion" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToConversion" runat="server" class="control-label" Text="To Currency"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlToConversion" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMoneyAmount" runat="server" class="control-label" Text="Amount"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtMoneyAmount" runat="server" CssClass="form-control quantitydecimal" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtConversionRate" runat="server" ReadOnly="true" CssClass="form-control quantitydecimal" TabIndex="4"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblGuestType" runat="server" class="control-label" Text="Guest Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGuestType" runat="server" CssClass="form-control" TabIndex="5">
                                    <asp:ListItem Value="OutSideGuest">Walk-In Guest</asp:ListItem>
                                    <asp:ListItem Value="InHouseGuest">In-House Guest</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTotalAmount" runat="server" class="control-label" Text="Total Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTotalAmount" runat="server" ReadOnly="true" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                            <div id="InHouseGuestSearchInfo">
                                <div class="col-md-2">
                                    <asp:Label ID="lblRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </div>
                                <div class="col-md-2" style="text-align: left; padding-left: 0;">
                                    <asp:Button ID="btnSrcRoomNumber" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                                        OnClick="btnSrcRoomNumber_Click" />
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblGuestName" runat="server" class="control-label" Text="Guest Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtGuestName" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCountryName" runat="server" class="control-label" Text="Country"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCountryName" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPassportNumber" runat="server" class="control-label" Text="Passport No."></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassportNumber" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTransactionDetails" runat="server" class="control-label" Text="Transaction Details"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtTransactionDetails" runat="server" TextMode="MultiLine" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="12" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" OnClientClick="javascript: return ValidateForm();" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="6" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Currency Conversion Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSFromConversion" runat="server" class="control-label" Text="From Currency"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSFromConversion" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSToConversion" runat="server" class="control-label" Text="To Currency"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSToConversion" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" />
                                <asp:Button ID="btnGroupPaymentPreview" runat="server" Text="Preview" TabIndex="15"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript: return LoadPopUp();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="SearchPanel" class="panel panel-default">
            <div class="panel-heading">
                Currency Conversion Details
            </div>
            <div class="panel-body">
                <asp:GridView ID="gvCurrencyConversion" Width="100%" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                    ForeColor="#333333" OnPageIndexChanging="gvCurrencyConversion_PageIndexChanging"
                    OnRowDataBound="gvCurrencyConversion_RowDataBound" OnRowCommand="gvCurrencyConversion_RowCommand"
                    PageSize="100" CssClass="table table-bordered table-condensed table-responsive">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("CurrencyConversionId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransactionNumber" HeaderText="Transaction Number" ItemStyle-Width="15%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ConversionAmount" HeaderText="Conversion Amount" ItemStyle-Width="20%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ConversionRate" HeaderText="Conversion Rate" ItemStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ConvertedAmount" HeaderText="Converted Amount" ItemStyle-Width="15%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                    CommandArgument='<%# bind("CurrencyConversionId") %>' ImageUrl="~/Images/edit.png"
                                    Text="" AlternateText="Edit" ToolTip="Edit" />
                                &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                    CommandArgument='<%# bind("CurrencyConversionId") %>' ImageUrl="~/Images/delete.png" Text="" AlternateText="Delete"
                                    ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
                                &nbsp;<asp:ImageButton ID="ImgPaymentPreview" runat="server" CausesValidation="False"
                                    CommandArgument='<%# bind("CurrencyConversionId") %>' CommandName="CmdPaymentPreview"
                                    ImageUrl="~/Images/ReportDocument.png" Text="" AlternateText="Payment Preview"
                                    ToolTip="Payment Preview" />
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
    <script type="text/javascript">

        var x = '<%=isSearchPanelEnable%>';
        if (x > -1) {
            $('#SearchPanel').show();

        }
        else {
            $('#SearchPanel').hide();
        }


        <%--var xIsInHouseGuestTransaction = '<%=IsInHouseGuestTransaction%>';
        if (xIsInHouseGuestTransaction == 0) {
            InhousePanelVisibleTrue();
        }
        else {
            InhousePanelVisibleFalse();
        }--%>
    </script>
</asp:Content>
