<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="GuestBillSplit.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.GuestBillSplit" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var txtConversionRate = '<%=txtConversionRateHiddenField.ClientID%>'
            var txtConversionRateVal = $('#' + txtConversionRate).val();
            if (txtConversionRateVal > 0) {
                $('#btnBillSplitPrintPreviewForUsd').show();
                $('#btnUncheckedBillSplitPrintPreviewForUsd').show();
                
            }
            else {
                $('#btnBillSplitPrintPreviewForUsd').hide();
                $('#btnUncheckedBillSplitPrintPreviewForUsd').hide();
            }

            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            var HiddenFieldCompanyPaymentButtonInfo = '<%=HiddenFieldCompanyPaymentButtonInfo.ClientID%>'
            $('#' + ddlServiceType).change(function () {
                if ($('#' + ddlServiceType).val() == "IndividualService") {
                    $('#GroupBillSpliteCheckBoxListDiv').hide();
                    $('#IndividualBillSpliteCheckBoxListDiv').show();
                    if ($('#' + HiddenFieldCompanyPaymentButtonInfo).val() == "1") {
                        $('#ComPaymentDiv').hide();
                    }
                    else {
                        $('#ComPaymentDiv').hide();
                    }
                }
                else {
                    $('#GroupBillSpliteCheckBoxListDiv').show();
                    $('#IndividualBillSpliteCheckBoxListDiv').hide();
                    if ($('#' + HiddenFieldCompanyPaymentButtonInfo).val() == "1") {
                        $('#ComPaymentDiv').show();
                    }
                    else {
                        $('#ComPaymentDiv').hide();
                    }
                }
            });

            $("#btnBillSplitPrintPreview").click(function () {
                debugger;
                var selectedServiceIdArray = new Array();
                var selectedServiceArray = new Array();
                var SelectdServiceId = "";
                var SelectdRoomId = "";

                var SelectdServiceApprovedId = "";
                var SelectdRoomApprovedId = "";
                var SelectdPaymentId = "";
                var SelectdIndividualPaymentId = "";
                var SelectdIndividualTransferedPaymentId = "";

                var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                var ddlServiceTypeVal = $('#' + ddlServiceType).val();

                if (ddlServiceTypeVal == "GroupService") {
                    $('#checkboxServiceList input:checked').each(function () {
                        SelectdServiceId = SelectdServiceId + $(this).attr('value') + ',';
                    });

                    $('#checkboxRoomList input:checked').each(function () {
                        SelectdRoomId = SelectdRoomId + $(this).attr('value') + ',';
                    });

                    $('#checkboxPaymentList input:checked').each(function () {
                        SelectdPaymentId = SelectdPaymentId + $(this).attr('value') + ',';
                    });

                    SelectdServiceId = RemoveFirstCommas(SelectdServiceId);
                    SelectdServiceId = RomoveLastCommas(SelectdServiceId);
                    SelectdRoomId = RemoveFirstCommas(SelectdRoomId);
                    SelectdRoomId = RomoveLastCommas(SelectdRoomId);
                }
                else {
                    $('#checkboxIndividualServiceList input:checked').each(function () {
                        SelectdServiceApprovedId = SelectdServiceApprovedId + $(this).attr('value') + ',';
                    });

                    $('#checkboxIndividualRoomList input:checked').each(function () {
                        SelectdRoomApprovedId = SelectdRoomApprovedId + $(this).attr('value') + ',';
                    });

                    $('#checkboxIndividualPaymentList input:checked').each(function () {
                        SelectdIndividualPaymentId = SelectdIndividualPaymentId + $(this).attr('value') + ',';
                    });

                    $('#checkboxIndividualTransferedPaymentList input:checked').each(function () {
                        SelectdIndividualTransferedPaymentId = SelectdIndividualTransferedPaymentId + $(this).attr('value') + ',';
                    });
                }

                SelectdServiceApprovedId = RemoveFirstCommas(SelectdServiceApprovedId);
                SelectdServiceApprovedId = RomoveLastCommas(SelectdServiceApprovedId);
                SelectdRoomApprovedId = RemoveFirstCommas(SelectdRoomApprovedId);
                SelectdRoomApprovedId = RomoveLastCommas(SelectdRoomApprovedId);
                SelectdPaymentId = RemoveFirstCommas(SelectdPaymentId);
                SelectdPaymentId = RomoveLastCommas(SelectdPaymentId);
                SelectdIndividualPaymentId = RemoveFirstCommas(SelectdIndividualPaymentId);
                SelectdIndividualPaymentId = RomoveLastCommas(SelectdIndividualPaymentId);
                SelectdIndividualTransferedPaymentId = RemoveFirstCommas(SelectdIndividualTransferedPaymentId);
                SelectdIndividualTransferedPaymentId = RomoveLastCommas(SelectdIndividualTransferedPaymentId);

                var txtStartDate = '<%=txtStartDate.ClientID%>'
                var txtEndDate = '<%=txtEndDate.ClientID%>'
                var ddlRegistrationId = '<%=txtSrcRegistrationId.ClientID%>'
                var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

                var StartDate = $('#' + txtStartDate).val();
                var EndDate = $('#' + txtEndDate).val();
                var RegistrationId = $('#' + ddlRegistrationId).val();
                var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();
                var isIsplite = "1";
                //popup(-1); ////TODO close popup

                PageMethods.PerformBillSplitePrintPreview(0, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);
                return false;
            });

            $("#btnBillSplitPrintPreviewForUsd").click(function () {
                var selectedServiceIdArray = new Array();
                var selectedServiceArray = new Array();
                var SelectdServiceId = "";
                var SelectdRoomId = "";

                $('#checkboxServiceList input:checked').each(function () {
                    SelectdServiceId = SelectdServiceId + $(this).attr('value') + ',';
                });

                $('#checkboxRoomList input:checked').each(function () {
                    SelectdRoomId = SelectdRoomId + $(this).attr('value') + ',';
                });

                SelectdServiceId = RemoveFirstCommas(SelectdServiceId);
                SelectdServiceId = RomoveLastCommas(SelectdServiceId);
                SelectdRoomId = RemoveFirstCommas(SelectdRoomId);
                SelectdRoomId = RomoveLastCommas(SelectdRoomId);

                var SelectdServiceApprovedId = "";
                var SelectdRoomApprovedId = "";
                var SelectdPaymentId = "";
                var SelectdIndividualPaymentId = "";
                var SelectdIndividualTransferedPaymentId = "";
                $('#checkboxIndividualServiceList input:checked').each(function () {
                    SelectdServiceApprovedId = SelectdServiceApprovedId + $(this).attr('value') + ',';
                });

                $('#checkboxIndividualRoomList input:checked').each(function () {
                    SelectdRoomApprovedId = SelectdRoomApprovedId + $(this).attr('value') + ',';
                });

                $('#checkboxPaymentList input:checked').each(function () {
                    SelectdPaymentId = SelectdPaymentId + $(this).attr('value') + ',';
                });

                $('#checkboxIndividualPaymentList input:checked').each(function () {
                    SelectdIndividualPaymentId = SelectdIndividualPaymentId + $(this).attr('value') + ',';
                });

                $('#checkboxIndividualTransferedPaymentList input:checked').each(function () {
                    SelectdIndividualTransferedPaymentId = SelectdIndividualTransferedPaymentId + $(this).attr('value') + ',';
                });

                SelectdServiceApprovedId = RemoveFirstCommas(SelectdServiceApprovedId);
                SelectdServiceApprovedId = RomoveLastCommas(SelectdServiceApprovedId);
                SelectdRoomApprovedId = RemoveFirstCommas(SelectdRoomApprovedId);
                SelectdRoomApprovedId = RomoveLastCommas(SelectdRoomApprovedId);
                SelectdPaymentId = RemoveFirstCommas(SelectdPaymentId);
                SelectdPaymentId = RomoveLastCommas(SelectdPaymentId);
                SelectdIndividualPaymentId = RemoveFirstCommas(SelectdIndividualPaymentId);
                SelectdIndividualPaymentId = RomoveLastCommas(SelectdIndividualPaymentId);
                SelectdIndividualTransferedPaymentId = RemoveFirstCommas(SelectdIndividualTransferedPaymentId);
                SelectdIndividualTransferedPaymentId = RomoveLastCommas(SelectdIndividualTransferedPaymentId);

                var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                var ddlServiceTypeVal = $('#' + ddlServiceType).val();

                var txtStartDate = '<%=txtStartDate.ClientID%>'
                var txtEndDate = '<%=txtEndDate.ClientID%>'
                var ddlRegistrationId = '<%=txtSrcRegistrationId.ClientID%>'
                var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

                var StartDate = $('#' + txtStartDate).val();
                var EndDate = $('#' + txtEndDate).val();
                var RegistrationId = $('#' + ddlRegistrationId).val();
                var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();
                var isIsplite = "1";
                var txtConversionRate = '<%=txtConversionRateHiddenField.ClientID%>'
                var txtConversionRateVal = $('#' + txtConversionRate).val();

                PageMethods.PerformBillSplitePrintPreview(txtConversionRateVal, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);
                return false;
            });





            $("#btnUncheckedBillSplitPrintPreview").click(function () {
                debugger;
                var selectedServiceIdArray = new Array();
                var selectedServiceArray = new Array();
                var SelectdServiceId = "";
                var SelectdRoomId = "";

                var SelectdServiceApprovedId = "";
                var SelectdRoomApprovedId = "";
                var SelectdPaymentId = "";
                var SelectdIndividualPaymentId = "";
                var SelectdIndividualTransferedPaymentId = "";

                var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                var ddlServiceTypeVal = $('#' + ddlServiceType).val();

                if (ddlServiceTypeVal == "GroupService") {
                    $('#checkboxServiceList input:not(:checked)').each(function () {
                        SelectdServiceId = SelectdServiceId + $(this).attr('value') + ',';
                    });

                    $('#checkboxRoomList input:not(:checked)').each(function () {
                        SelectdRoomId = SelectdRoomId + $(this).attr('value') + ',';
                    });

                    $('#checkboxPaymentList input:not(:checked)').each(function () {
                        SelectdPaymentId = SelectdPaymentId + $(this).attr('value') + ',';
                    });

                    SelectdServiceId = RemoveFirstCommas(SelectdServiceId);
                    SelectdServiceId = RomoveLastCommas(SelectdServiceId);
                    SelectdRoomId = RemoveFirstCommas(SelectdRoomId);
                    SelectdRoomId = RomoveLastCommas(SelectdRoomId);
                }
                else {
                    $('#checkboxIndividualServiceList input:not(:checked)').each(function () {
                        SelectdServiceApprovedId = SelectdServiceApprovedId + $(this).attr('value') + ',';
                    });

                    $('#checkboxIndividualRoomList input:not(:checked)').each(function () {
                        SelectdRoomApprovedId = SelectdRoomApprovedId + $(this).attr('value') + ',';
                    });

                    $('#checkboxIndividualPaymentList input:not(:checked)').each(function () {
                        SelectdIndividualPaymentId = SelectdIndividualPaymentId + $(this).attr('value') + ',';
                    });

                    $('#checkboxIndividualTransferedPaymentList input:not(:checked)').each(function () {
                        SelectdIndividualTransferedPaymentId = SelectdIndividualTransferedPaymentId + $(this).attr('value') + ',';
                    });
                }

                SelectdServiceApprovedId = RemoveFirstCommas(SelectdServiceApprovedId);
                SelectdServiceApprovedId = RomoveLastCommas(SelectdServiceApprovedId);
                SelectdRoomApprovedId = RemoveFirstCommas(SelectdRoomApprovedId);
                SelectdRoomApprovedId = RomoveLastCommas(SelectdRoomApprovedId);
                SelectdPaymentId = RemoveFirstCommas(SelectdPaymentId);
                SelectdPaymentId = RomoveLastCommas(SelectdPaymentId);
                SelectdIndividualPaymentId = RemoveFirstCommas(SelectdIndividualPaymentId);
                SelectdIndividualPaymentId = RomoveLastCommas(SelectdIndividualPaymentId);
                SelectdIndividualTransferedPaymentId = RemoveFirstCommas(SelectdIndividualTransferedPaymentId);
                SelectdIndividualTransferedPaymentId = RomoveLastCommas(SelectdIndividualTransferedPaymentId);

                var txtStartDate = '<%=txtStartDate.ClientID%>'
                var txtEndDate = '<%=txtEndDate.ClientID%>'
                var ddlRegistrationId = '<%=txtSrcRegistrationId.ClientID%>'
                var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

                var StartDate = $('#' + txtStartDate).val();
                var EndDate = $('#' + txtEndDate).val();
                var RegistrationId = $('#' + ddlRegistrationId).val();
                var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();
                var isIsplite = "1";
                //popup(-1); ////TODO close popup

                PageMethods.PerformBillSplitePrintPreview(0, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);
                return false;
            });

            $("#btnUncheckedBillSplitPrintPreviewForUsd").click(function () {
                var selectedServiceIdArray = new Array();
                var selectedServiceArray = new Array();
                var SelectdServiceId = "";
                var SelectdRoomId = "";

                $('#checkboxServiceList input:not(:checked)').each(function () {
                    SelectdServiceId = SelectdServiceId + $(this).attr('value') + ',';
                });

                $('#checkboxRoomList input:not(:checked)').each(function () {
                    SelectdRoomId = SelectdRoomId + $(this).attr('value') + ',';
                });

                SelectdServiceId = RemoveFirstCommas(SelectdServiceId);
                SelectdServiceId = RomoveLastCommas(SelectdServiceId);
                SelectdRoomId = RemoveFirstCommas(SelectdRoomId);
                SelectdRoomId = RomoveLastCommas(SelectdRoomId);

                var SelectdServiceApprovedId = "";
                var SelectdRoomApprovedId = "";
                var SelectdPaymentId = "";
                var SelectdIndividualPaymentId = "";
                var SelectdIndividualTransferedPaymentId = "";
                $('#checkboxIndividualServiceList input:not(:checked)').each(function () {
                    SelectdServiceApprovedId = SelectdServiceApprovedId + $(this).attr('value') + ',';
                });

                $('#checkboxIndividualRoomList input:not(:checked)').each(function () {
                    SelectdRoomApprovedId = SelectdRoomApprovedId + $(this).attr('value') + ',';
                });

                $('#checkboxPaymentList input:not(:checked)').each(function () {
                    SelectdPaymentId = SelectdPaymentId + $(this).attr('value') + ',';
                });

                $('#checkboxIndividualPaymentList input:not(:checked)').each(function () {
                    SelectdIndividualPaymentId = SelectdIndividualPaymentId + $(this).attr('value') + ',';
                });

                $('#checkboxIndividualTransferedPaymentList input:not(:checked)').each(function () {
                    SelectdIndividualTransferedPaymentId = SelectdIndividualTransferedPaymentId + $(this).attr('value') + ',';
                });

                SelectdServiceApprovedId = RemoveFirstCommas(SelectdServiceApprovedId);
                SelectdServiceApprovedId = RomoveLastCommas(SelectdServiceApprovedId);
                SelectdRoomApprovedId = RemoveFirstCommas(SelectdRoomApprovedId);
                SelectdRoomApprovedId = RomoveLastCommas(SelectdRoomApprovedId);
                SelectdPaymentId = RemoveFirstCommas(SelectdPaymentId);
                SelectdPaymentId = RomoveLastCommas(SelectdPaymentId);
                SelectdIndividualPaymentId = RemoveFirstCommas(SelectdIndividualPaymentId);
                SelectdIndividualPaymentId = RomoveLastCommas(SelectdIndividualPaymentId);
                SelectdIndividualTransferedPaymentId = RemoveFirstCommas(SelectdIndividualTransferedPaymentId);
                SelectdIndividualTransferedPaymentId = RomoveLastCommas(SelectdIndividualTransferedPaymentId);

                var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                var ddlServiceTypeVal = $('#' + ddlServiceType).val();

                var txtStartDate = '<%=txtStartDate.ClientID%>'
                var txtEndDate = '<%=txtEndDate.ClientID%>'
                var ddlRegistrationId = '<%=txtSrcRegistrationId.ClientID%>'
                var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

                var StartDate = $('#' + txtStartDate).val();
                var EndDate = $('#' + txtEndDate).val();
                var RegistrationId = $('#' + ddlRegistrationId).val();
                var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();
                var isIsplite = "1";
                var txtConversionRate = '<%=txtConversionRateHiddenField.ClientID%>'
                var txtConversionRateVal = $('#' + txtConversionRate).val();

                PageMethods.PerformBillSplitePrintPreview(txtConversionRateVal, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);
                return false;
            });


        });

        function RemoveFirstCommas(flag) {
            var length = flag.length;
            var Index = 0;
            for (var j = 0; j < length; j++) {
                if (flag.charAt(j) == '0' || flag.charAt(j) == '1' || flag.charAt(j) == '2' || flag.charAt(j) == '3' || flag.charAt(j) == '4' || flag.charAt(j) == '5' || flag.charAt(j) == '6' || flag.charAt(j) == '7' || flag.charAt(j) == '8' || flag.charAt(j) == '9') {
                    Index = j;
                    break;
                }
            }
            flag = flag.substring(Index, length - Index);

            return flag;
        }

        function RomoveLastCommas(flag) {
            var length = flag.length;
            var Index = 0;
            var lastIndex = flag.lastIndexOf(',');
            flag = flag.substring(0, length - (length - lastIndex));
            return flag;
        }

        function OnPerformBillSplitePrintPreviewSucceeded(result) {
            $('#ContentPlaceHolder1_chkIsBillSplit').attr('checked', false)
            var url = "/HotelManagement/Reports/frmReportGuestBillInfo.aspx";
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=825,height=680,left=300,top=50,resizable=yes");
            ClearDetailsPart();
        }

        function OnPerformBillSplitePrintPreviewSucceededForFinalBill(result) {
            $('#ContentPlaceHolder1_chkIsBillSplit').attr('checked', false)
            var url = "/HotelManagement/Reports/frmReportGuestBillInfo.aspx";
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=825,height=680,left=300,top=50,resizable=yes");
            ClearDetailsPart();

            $("#BillPreviewWithoutRebateDiv").hide();
            $("#BillPreviewWithRebateDiv").hide();
        }

        function OnPerformBillSplitePrintPreviewFailed(error) {

        }
    </script>
    <asp:HiddenField ID="txtSrcRegistrationId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtSrcRegistrationIdList" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtConversionRateHiddenField" runat="server"></asp:HiddenField>
    <div class="panel panel-default">
        <div class="panel-heading">
            Bill Split Information
        </div>
        <div class="childDivSection" id="BillSplitPopUpForm">
            <div class="panel-body">
                <div class="form-horizontal" style="padding-bottom: 5px;">
                    <div class="form-group" style="display: none;">
                        <div class="col-md-2">
                            <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" TabIndex="-1"></asp:TextBox><input
                                type="hidden" id="hidFromDate" />
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server" TabIndex="-1"></asp:TextBox><input
                                type="hidden" id="hidToDate" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblServiceType" runat="server" class="control-label" Text="Split Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control" TabIndex="-1">
                                <asp:ListItem Value="GroupService">Group Service</asp:ListItem>
                                <asp:ListItem Value="IndividualService">Individual Service</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="display: none">
                        <div id="IndividualServiceDivInfo" style="display: none">
                            <asp:GridView ID="gvIndividualServiceInformationForBillSplit" Width="100%" runat="server"
                                AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" GridLines="None"
                                AllowSorting="True" ForeColor="#333333" OnRowDataBound="gvIndividualServiceInformationForBillSplit_RowDataBound"
                                CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" ItemStyle-Width="05%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsSelected" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="95%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvMenuHead" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Type" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvServiceType" runat="server" Text='<%# Bind("ServiceType") %>'></asp:Label>
                                        </ItemTemplate>
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
                        <div id="GroupServiceDivInfo" style="display: none">
                            <asp:GridView ID="gvGroupServiceInformationForBillSplit" Width="100%" runat="server"
                                AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" GridLines="None"
                                AllowSorting="True" ForeColor="#333333" OnRowDataBound="gvGroupServiceInformationForBillSplit_RowDataBound"
                                CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" ItemStyle-Width="05%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsSelected" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="95%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvMenuHead" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Type" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvServiceType" runat="server" Text='<%# Bind("ServiceType") %>'></asp:Label>
                                        </ItemTemplate>
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
                </div>
                <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="478px">
                    <div id="GroupBillSpliteCheckBoxListDiv">
                        <div class="checkbox checkboxList" id="checkboxRoomList">
                            <asp:CheckBoxList ID="chkBillSpliteRoomItem" runat="server" CssClass="customChkBox">
                            </asp:CheckBoxList>
                        </div>
                        <div class="checkbox checkboxList" id="checkboxServiceList" style="margin-top: -13px">
                            <asp:CheckBoxList ID="chkBillSpliteServiceItem" runat="server" CssClass="customChkBox">
                            </asp:CheckBoxList>
                        </div>
                        <div class="checkbox checkboxList" id="checkboxPaymentList" style="margin-top: -13px">
                            <asp:CheckBoxList ID="chkBillSplitePaymentItem" runat="server" CssClass="customChkBox">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div id="IndividualBillSpliteCheckBoxListDiv" style="display: none;">
                        <div class="checkbox checkboxList" id="checkboxIndividualRoomList">
                            <asp:CheckBoxList ID="chkBillSpliteIndividualRoomItem" runat="server" CssClass="customChkBox">
                            </asp:CheckBoxList>
                        </div>
                        <div class="checkbox checkboxList" id="checkboxIndividualServiceList" style="margin-top: -13px">
                            <asp:CheckBoxList ID="chkBillSpliteIndividualServiceItem" runat="server" CssClass="customChkBox">
                            </asp:CheckBoxList>
                        </div>
                        <div class="checkbox checkboxList" id="checkboxIndividualPaymentList" style="margin-top: -13px">
                            <asp:CheckBoxList ID="chkBillSpliteIndividualPaymentItem" runat="server" CssClass="customChkBox">
                            </asp:CheckBoxList>
                        </div>
                        <div class="checkbox checkboxList" id="checkboxIndividualTransferedPaymentList" style="margin-top: -13px">
                            <asp:CheckBoxList ID="chkBillSpliteIndividualTransferedPaymentItem" runat="server"
                                CssClass="customChkBox">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </asp:Panel>
                <div class="form-horizontal">
                    <div class="form-group" style="display: none;">
                        <div class="col-md-2">
                            Total Bill Amount
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtTotalBillAmount" CssClass="form-control" runat="server" Width="190px"></asp:TextBox>
                            <asp:HiddenField ID="hfAdvanceOrCashOutCalculatedAmount" runat="server"></asp:HiddenField>
                        </div>
                        <div class="col-md-2">
                        </div>
                    </div>

                </div>
                <div class="form-group" style="padding-top: 4px;">
                    <div class="form-group">
                        <div class="col-md-6">
                            <fieldset>
                            <legend>Checked Item Preview</legend>
                            <div class="form-group">
                                <div id="PrintPreviewCheckedDiv">
                                    <input type="button" id="btnBillSplitPrintPreview" style="width:49%" value="Print Preview" class="btn btn-primary" />
                                    <input type="button" id="btnBillSplitPrintPreviewForUsd" style="width:49%" value="Print Preview (USD)"
                                        class="btn btn-primary" />
                                </div>
                            </div>
                        </fieldset>
                        </div>
                        <div class="col-md-6">
                            <fieldset>
                            <legend>Unchecked Item Preview</legend>
                            <div class="form-group">
                                <div id="PrintPreviewUncheckedDiv">
                                    <input type="button" id="btnUncheckedBillSplitPrintPreview" style="width:49%" value="Print Preview" class="btn btn-primary" />
                                    <input type="button" id="btnUncheckedBillSplitPrintPreviewForUsd" style="width:49%" value="Print Preview (USD)"
                                        class="btn btn-primary" />
                                </div>
                            </div>
                        </fieldset>
                        </div>
                    </div>
                </div>
            </div>
            <div class="childDivSection" id="CompanyPaymentPopUpForm" style="display: none;">
                <div class="panel panel-default" style="height: 478px;">
                    <div class="panel-body">
                        <div class="panel-body" style="display: none">
                            <div id="Div3">
                                <asp:GridView ID="gvCompanyPaymentGroupServiceInformationForBillSplit" Width="100%"
                                    runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4"
                                    GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowDataBound="gvGroupServiceInformationForBillSplit_RowDataBound"
                                    CssClass="table table-bordered table-condensed table-responsive">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="90%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvMenuHead" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Type" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvServiceType" runat="server" Text='<%# Bind("ServiceType") %>'></asp:Label>
                                            </ItemTemplate>
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
                        <asp:Panel ID="pnlCompanyPaymentRoomCalender" runat="server" ScrollBars="Both" Height="478px">
                            <div id="CompanyPaymentGroupBillSpliteCheckBoxListDiv">
                                <div class="checkboxList" id="CompanyPaymentcheckboxRoomList">
                                    <asp:CheckBoxList ID="chkCompanyPaymentBillSpliteRoomItem" runat="server" CssClass="customChkBox">
                                    </asp:CheckBoxList>
                                </div>
                                <div class="checkboxList" id="CompanyPaymentcheckboxServiceList" style="margin-top: -13px">
                                    <asp:CheckBoxList ID="chkCompanyPaymentBillSpliteServiceItem" runat="server" CssClass="customChkBox">
                                    </asp:CheckBoxList>
                                </div>
                                <div class="checkboxList" id="CompanyPaymentcheckboxPaymentList" style="margin-top: -13px;">
                                    <asp:CheckBoxList ID="chkCompanyPaymentBillSplitePaymentItem" runat="server" CssClass="customChkBox">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group" style="padding-top: 4px;">
                            <asp:HiddenField ID="HiddenFieldCompanyPaymentButtonInfo" runat="server"></asp:HiddenField>
                            <input type="button" id="btnCompanyPayment" value="Company Pay" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            $(document).ready(function () {
                //// Group Service -----------
                $('#checkboxServiceList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });

                $('#checkboxRoomList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });

                $('#checkboxPaymentList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });

                //// Individual Service -----------
                $('#checkboxIndividualRoomList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });

                $('#checkboxIndividualServiceList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });

                $('#checkboxIndividualPaymentList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });

                $('#checkboxIndividualTransferedPaymentList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });
            });
        </script>
</asp:Content>
