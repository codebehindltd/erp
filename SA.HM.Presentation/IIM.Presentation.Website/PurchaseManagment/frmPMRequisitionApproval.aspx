<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmPMRequisitionApproval.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmRequisitionApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var pendingItem = [], approvedItem = [];

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Requisition Approval</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            DivShowHideFunction();
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

            $("#<%=ddlDateType.ClientID %>").change(function () {
                DivShowHideFunction();
            });
        });

        $(document).ready(function () {

            $("#btnApprove").click(function () {

                CommonHelper.SpinnerOpen();

                var requsitionId = "0", requsitionDetailsId = "0";
                var quantity = "", approvedQuantity = "";

                pendingItem = []; approvedItem = [];

                requsitionId = $("#ContentPlaceHolder1_hfRequisitionId").val();

                $("#DetailsRequisitionGrid tbody tr").each(function (index, item) {

                    if ($(item).find("td:eq(0)").find("input").is(':checkbox')) {

                        quantity = $.trim($(item).find("td:eq(3)").text());
                        approvedQuantity = $.trim($(item).find("td:eq(4)").find("input").val());
                        requsitionDetailsId = $.trim($(item).find("td:eq(6)").text());

                        if ($(item).find("td:eq(0)").find("input").is(":checked") && !$(item).find("td:eq(0)").find("input").is(":disabled")) {

                            if (approvedQuantity == "" || approvedQuantity == "0") {
                                approvedQuantity = quantity;
                            }

                            approvedItem.push({
                                RequisitionDetailsId: parseInt(requsitionDetailsId, 10),
                                RequisitionId: parseInt(requsitionId, 10),
                                Quantity: parseFloat(quantity),
                                ApprovedQuantity: parseFloat(approvedQuantity),
                                ApprovedStatus: "Approved"
                            });
                        }
                        else if (!$(item).find("td:eq(0)").find("input").is(":disabled")) {
                            pendingItem.push({
                                RequisitionDetailsId: parseInt(requsitionDetailsId, 10),
                                RequisitionId: parseInt(requsitionId, 10),
                                ApprovedStatus: "Pending"
                            });
                        }
                    }
                });


                var hfIsRequisitionCheckedByEnable = $("#ContentPlaceHolder1_hfIsRequisitionCheckedByEnable").val();

                PageMethods.ApprovedRequsition(hfIsRequisitionCheckedByEnable, requsitionId, approvedItem, pendingItem, OnApprovedRequsitionSucceed, OnApprovedRequsitionFailed);

                return false;
            });

        });

        function DivShowHideFunction() {
            if ($("#<%=ddlDateType.ClientID %>").val() == "Pending") {
                $('#DateDiv').hide("slow");
            }
            else {
                $('#DateDiv').show("slow");
            }
        }

        function OnApprovedRequsitionSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#DetailsRequisitionDialog").dialog("close");

                $("#ContentPlaceHolder1_hfRequisitionId").val("0");
                pendingItem = [];
                approvedItem = [];

                //PerformClearAction();
                $("#ContentPlaceHolder1_btnSearch").trigger("click");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function OnApprovedRequsitionFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function RequisitionDetails(requisitionId) {
            CommonHelper.SpinnerOpen();
            $("#ContentPlaceHolder1_hfRequisitionId").val(requisitionId);
            PageMethods.GetRequisitionDetails(requisitionId, OnFillRequisitionDetailsSucceed, OnFillRequisitionDetailsFailed);
            return false;
        }
        function OnFillRequisitionDetailsSucceed(result) {

            $("#DetailsRequisitionGridContaiiner").html(result);

            $("#DetailsRequisitionDialog").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 700,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Requisition Details",
                show: 'slide'
            });
            CommonHelper.SpinnerClose();
        }
        function OnFillRequisitionDetailsFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        function CheckInputValue(approvedTextBox) {

            if ($.trim($(approvedTextBox).val()) != "") {

                if (CommonHelper.IsDecimal($.trim($(approvedTextBox).val())) == false) {
                    toastr.warning("Please give valid quantity.");
                    $(approvedTextBox).val("");
                    return false;
                }
            }

            return false;
        }

        function EditApprovedRequisition(editItem) {
            var tr = $(editItem).parent().parent();
            $(tr).find("td:eq(0)").find("input").attr("disabled", false);
            $(tr).find("td:eq(4)").find("input").attr("disabled", false);
        }

        function PerformClearAction() {

            $("#<%=txtFromDate.ClientID %>").val('');
            $("#<%=txtToDate.ClientID %>").val('');
            $("#ContentPlaceHolder1_hfRequisitionId").val("");
            pendingItem = [];
            approvedItem = [];

            return false;
        }

    </script>
    <div id="DetailsRequisitionDialog" style="display: none;">
        <div id="DetailsRequisitionGridContaiiner">
        </div>
        <div class="HMContainerRowButton" style="padding-bottom: 0; padding-top: 10px;">
            <input id="btnApprove" type="button" class="TransactionalButton btn btn-primary btn-sm"
                value="Approve Requisition" />
        </div>
    </div>
    <asp:HiddenField ID="hfRequisitionId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsRequisitionCheckedByEnable" runat="server" Value="0" />
    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Requisition Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblDateType" runat="server" class="control-label" Text="Filter On"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDateType" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem Value="Pending">Requisition Pending</asp:ListItem>
                            <asp:ListItem Value="CreatedDate">Requisition Date</asp:ListItem>
                            <asp:ListItem Value="ReceivedDate">Received By Date</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Panel ID="pnlStatus" runat="server">
                        <div class="col-md-2">
                            <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Status"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="2">
                                <asp:ListItem>All</asp:ListItem>
                                <asp:ListItem>Submitted</asp:ListItem>
                                <asp:ListItem>Checked</asp:ListItem>
                                <asp:ListItem>Approved</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </asp:Panel>
                </div>
                <div class="form-group" id="DateDiv">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
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
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" TabIndex="6" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information</div>
        <div class="panel-body">
            <asp:GridView ID="gvRequisitionInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowDataBound="gvRequisitionInfo_RowDataBound"
                OnRowCommand="gvRequisitionInfo_RowCommand" TabIndex="7" CssClass="table table-bordered table-condensed table-responsive">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("RequisitionId") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PRNumber" HeaderText="PR Number" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FromCostCenter" HeaderText="Cost Center" ItemStyle-Width="30%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="RequisitionBy" HeaderText="Requisition By" ItemStyle-Width="25%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="20%">
                        <ItemTemplate>
                            <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgRequisitionDetails" runat="server" CausesValidation="False"
                                CommandName="CmdEdit" CommandArgument='<%# bind("RequisitionId") %>' ImageUrl="~/Images/detailsInfo.png"
                                OnClientClick='<%#String.Format("return RequisitionDetails({0})", Eval("RequisitionId")) %>'
                                Text="" AlternateText="Details" ToolTip="Requisition Details" />
                            &nbsp;&nbsp;<asp:ImageButton ID="ImgReportPO" runat="server" CausesValidation="False"
                                CommandName="CmdReportRI" CommandArgument='<%# bind("RequisitionId") %>' ImageUrl="~/Images/ReportDocument.png"
                                Text="" AlternateText="Invoice" ToolTip="Requisition Order Invoice" />
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
