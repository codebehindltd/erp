<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmAvailableGuestList.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmAvailableGuestList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        //For FillForm-------------------------
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        } 
        function PerformFillFormForUpdateAction(actionId) {
            PageMethods.FillFormUpdateAction(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function PerformFillFormServiceAction(actionId) {
            PageMethods.FillServiceForm(actionId, OnFillServiceFormObjectSucceeded, OnFillServiceFormObjectFailed);
            return false;
        }

        function PerformFillFormServiceForUpdateAction(actionId) {
            PageMethods.FillServiceFormUpdateAction(actionId, OnFillServiceFormObjectSucceeded, OnFillServiceFormObjectFailed);
            return false;
        }

        $(document).ready(function () {
            var txtApprovedDate = '<%=txtApprovedDate.ClientID%>'

            $('#txtApprovedDate').click(function () {
            });

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Hotel Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Night Audit</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var txtRoomRate = '<%=txtRoomRate.ClientID%>'
            $('#' + txtRoomRate).blur(function () {
                var txtVatAmountPercent = '<%=txtVatAmountPercent.ClientID%>'
                var txtServiceChargePercent = '<%=txtServiceChargePercent.ClientID%>'
                var txtReferenceSalesCommission = '<%=txtReferenceSalesCommission.ClientID%>'
                var txtReferenceSalesCommissionPercent = '<%=txtReferenceSalesCommissionPercent.ClientID%>'
                var txtCalculatedPercentAmount = '<%=txtCalculatedPercentAmount.ClientID%>'

                $('#' + txtCalculatedPercentAmount).val(100)

                var VatData = (parseFloat($('#' + txtRoomRate).val()) * parseFloat($('#' + txtVatAmountPercent).val())) / parseFloat($('#' + txtCalculatedPercentAmount).val());
                //var ServiceChargeData = (parseFloat($('#' + txtRoomRate).val()) * parseFloat($('#' + txtServiceChargePercent).val())) / parseFloat($('#' + txtCalculatedPercentAmount).val());
                var ServiceChargeData = (parseFloat($('#' + txtRoomRate).val()) * parseFloat($('#' + txtServiceChargePercent).val())) / parseFloat($('#' + txtCalculatedPercentAmount).val());
                var ReferenceSalesCommission = (parseFloat($('#' + txtRoomRate).val()) * parseFloat($('#' + txtReferenceSalesCommissionPercent).val()));
                var txtVatAmount = '<%=txtVatAmount.ClientID%>'
                var txtServiceCharge = '<%=txtServiceCharge.ClientID%>'
                $('#' + txtVatAmount).val(VatData)
                $('#' + txtServiceCharge).val(ServiceChargeData)
                $('#' + txtReferenceSalesCommission).val(ReferenceSalesCommission)
            });


            var txtServiceRate = '<%=txtServiceRate.ClientID%>'
            $('#' + txtServiceRate).blur(function () {
                CalculationServiceInformation();
            });

            var txtServiceQuantity = '<%=txtServiceQuantity.ClientID%>'
            $('#' + txtServiceQuantity).blur(function () {
                CalculationServiceInformation();
            });
        });

        function CalculationServiceInformation() {
            var txtServiceRate = '<%=txtServiceRate.ClientID%>'
            var txtServiceQuantity = '<%=txtServiceQuantity.ClientID%>'
            var txtGuestServiceVatAmountPercent = '<%=txtGuestServiceVatAmountPercent.ClientID%>'
            var txtGuestServiceServiceChargePercent = '<%=txtGuestServiceServiceChargePercent.ClientID%>'
            var txtGuestServiceCalculatedPercentAmount = '<%=txtGuestServiceCalculatedPercentAmount.ClientID%>'

            $('#' + txtGuestServiceCalculatedPercentAmount).val(100)
            var VatData = (parseFloat($('#' + txtServiceRate).val() * $('#' + txtServiceQuantity).val()) * parseFloat($('#' + txtGuestServiceVatAmountPercent).val())) / parseFloat($('#' + txtGuestServiceCalculatedPercentAmount).val());
            var ServiceChargeData = (parseFloat($('#' + txtServiceRate).val() * $('#' + txtServiceQuantity).val()) * parseFloat($('#' + txtGuestServiceServiceChargePercent).val())) / parseFloat($('#' + txtGuestServiceCalculatedPercentAmount).val());
            var txtGuestServiceVatAmount = '<%=txtGuestServiceVatAmount.ClientID%>'
            var txtGuestServiceServiceCharge = '<%=txtGuestServiceServiceCharge.ClientID%>'
            $('#' + txtGuestServiceVatAmount).val(VatData.toFixed(2))
            $('#' + txtGuestServiceServiceCharge).val(ServiceChargeData.toFixed(2))
        };

        function OnFillFormObjectSucceeded(result) {
            var guest = eval(result);
            $("#<%=ApprovedIdHiddenField.ClientID %>").val(guest.ApprovedId);
            $("#<%=txtRegistrationId.ClientID %>").val(guest.RegistrationId);
            $("#<%=txtRoomIdHiddenField.ClientID %>").val(guest.RoomId);
            $("#<%=txtRoomNumber.ClientID %>").val(guest.RoomNumber);
            $("#<%=txtGuestName.ClientID %>").val(result.GuestName);
            $("#<%=txtRoomType.ClientID %>").val(result.RoomType);
            $("#<%=txtPreviousRoomRate.ClientID %>").val(result.RoomRate);
            $("#<%=txtRoomRate.ClientID %>").val(result.CalculatedRoomRate);
            $("#<%= txtBPPercentAmount.ClientID %>").val(result.BPPercentAmount);
            $("#<%= txtDiscountAmount.ClientID %>").val(result.BPDiscountAmount);
            $("#<%=txtRoomNumber.ClientID %>").attr('readonly', true);
            $("#<%=txtGuestName.ClientID %>").attr('readonly', true);
            $("#<%=txtRoomType.ClientID %>").attr('readonly', true);
            $("#<%=txtPreviousRoomRate.ClientID %>").attr('readonly', true);
            $("#<%=txtVatAmount.ClientID %>").val(result.VatAmount);
            $("#<%=txtVatAmount.ClientID %>").attr('readonly', true);
            $("#<%=txtVatAmountPercent.ClientID %>").val(result.VatAmountPercent);
            $("#<%=txtServiceChargePercent.ClientID %>").val(result.ServiceChargePercent);
            $("#<%=txtReferenceSalesCommission.ClientID %>").val(result.ReferenceSalesCommission);
            $("#<%=txtReferenceSalesCommissionPercent.ClientID %>").val(result.ReferenceSalesCommissionPercent);
            $("#<%=txtCalculatedPercentAmount.ClientID %>").val(result.CalculatedPercentAmount);
            $('#EntryPanel').show("slow");

            if (guest.ApprovedId > 0) {
                $('#btnSaveButtonDiv').hide("slow");
                $('#btnUpdateRoomApprovedDataButtonDiv').show("slow");
            }
            else {
                $('#btnSaveButtonDiv').show("slow");
                $('#btnUpdateRoomApprovedDataButtonDiv').hide("slow");
            }
        }

        function OnFillFormObjectFailed(error) {
            alert(error.get_message());
        }

        function OnFillServiceFormObjectSucceeded(result) {
            //var guest = eval(result);
            var date = new Date(result.ServiceDate);
            $("#<%=ServiceApprovedIdHiddenField.ClientID %>").val(result.ApprovedId);
            $("#<%=txtServiceBillId.ClientID %>").val(result.ServiceBillId);
            $("#<%=txtRegistrationNumber.ClientID %>").val(result.RegistrationNumber);
            $("#<%=txtServiceDate.ClientID %>").val(GetStringFromDateTime(result.ServiceDate));
            $("#<%=txtServiceName.ClientID %>").val(result.ServiceName);
            $("#<%=txtServiceRoomNumber.ClientID %>").val(result.RoomNumber);
            $("#<%=txtServiceRegistrationId.ClientID %>").val(result.RegistrationId);
            $("#<%=txtServiceServiceId.ClientID %>").val(result.ServiceId);
            $("#<%=txtServiceRate.ClientID %>").val(result.ServiceRate);
            $("#<%=txtServiceQuantity.ClientID %>").val(result.ServiceQuantity);
            $("#<%=txtDiscountAmount.ClientID %>").val(result.DiscountAmount);
            $("#<%=txtServiceBillId.ClientID %>").val(result.ServiceBillId);
            $("#<%=txtServiceType.ClientID %>").val(result.ServiceType);
            $("#<%=txtPaymentMode.ClientID %>").val(result.RoomNumber);
            $("#<%=txtServiceRegistrationNumber.ClientID %>").val(result.RegistrationNumber);
            $("#<%=txtServiceServiceName.ClientID %>").val(result.ServiceName);
            $("#<%=txtServiceServiceDate.ClientID %>").val(GetStringFromDateTime(result.ServiceDate));

            $("#<%=txtGuestServiceVatAmount.ClientID %>").val(result.VatAmount);
            $("#<%=txtGuestServiceServiceCharge.ClientID %>").val(result.ServiceCharge);
            $("#<%=txtGuestServiceVatAmount.ClientID %>").attr('readonly', true);
            $("#<%=txtGuestServiceVatAmountPercent.ClientID %>").val(result.VatAmountPercent);
            $("#<%=txtGuestServiceServiceChargePercent.ClientID %>").val(result.ServiceChargePercent);
            $("#<%=txtGuestServiceCalculatedPercentAmount.ClientID %>").val(result.CalculatedPercentAmount);


            $('#ServiceApprovedDiv').show("slow");
            if (result.ApprovedId > 0) {
                $('#ServiceBillDiv').hide("slow");
                $('#ServiceBillApprovedDiv').show("slow");
            }
            else {
                $('#ServiceBillDiv').show("slow");
                $('#ServiceBillApprovedDiv').hide("slow");
            }
        }

        function OnFillServiceFormObjectFailed(error) {
            alert(error.get_message());
        }

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=txtRegistrationId.ClientID %>").val('');
            $("#<%=txtRoomIdHiddenField.ClientID %>").val('');
            $("#<%=txtRoomNumber.ClientID %>").val('');
            $("#<%=txtGuestName.ClientID %>").val('');
            $("#<%=txtRoomType.ClientID %>").val('');
            $("#<%=txtPreviousRoomRate.ClientID %>").val('');
            $("#<%= txtBPPercentAmount.ClientID %>").val('0');
            $("#<%=txtVatAmount.ClientID %>").val('0');
            $("#<%=txtServiceCharge.ClientID %>").val('0');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        function PerformServiceClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            var date = new Date();
            $("#<%=txtServiceDate.ClientID %>").val(GetStringFromDateTime(date));
            $("#<%=txtServiceRate.ClientID %>").val('0');
            $("#<%=txtServiceName.ClientID %>").val('');
            $("#<%=txtServiceRoomNumber.ClientID %>").val('');
            $("#<%=txtServiceQuantity.ClientID %>").val('0');
            $("#<%=txtDiscountAmount.ClientID %>").val('0');
            $("#<%=txtServiceBillId.ClientID %>").val('');
            //$("#<%=btnSave.ClientID %>").val("Save");
            $("#<%=txtServiceRegistrationNumber.ClientID %>").val('');
            $("#<%=txtServiceServiceName.ClientID %>").val('');
            $("#<%=txtServiceServiceDate.ClientID %>").val(GetStringFromDateTime(date));
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        function EntryServicePanelVisibleTrue() {
            $('#ServiceApprovedDiv').show("slow");
            return false;
        }
        function EntryServicePanelVisibleFalse() {
            $('#ServiceApprovedDiv').hide("slow");
            PerformServiceClearAction();
            return false;
        }

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }

        function OpenUpdatePanel() {
            $('#ServiceApprovedDiv').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });

        function ConfirmApproveMessege() {


            var approvedDate = $("#<%=txtApprovedDate.ClientID %>").val();
            var today = GetDateTimeFromString(approvedDate);
            var dateString = GetStringFromDateTime(today);
            var answer = confirm("Do you want to Approve Night Audit for the date " + "'" + dateString + "' ?")
            if (answer) {
                return true;
            }
            else {
                return false;
            }
        }


        function ConfirmApproveMessegeForUpdate() {
            var approvedDate = $("#<%=txtApprovedDate.ClientID %>").val();
            var today = GetDateTimeFromString(approvedDate);
            var dateString = GetStringFromDateTime(today);
            var answer = confirm("Do you want to Update Approved Night Audit data for the date " + "'" + dateString + "' ?")
            if (answer) {
                return true;
            }
            else {
                return false;
            }
        }

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <%--  <div style="height: 5px">
    </div>--%>
    <div id="SearchPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
        </a>
        <div class="HMBodyContainer">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblApprovedDate" runat="server" Text="Approve Date"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtApprovedDate" runat="server" CssClass="datepicker" TabIndex="1"></asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblSrcRoomNumber" runat="server" Text="Room Number"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="customSmallTextBoxSize"
                        TabIndex="2"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <%--Right Left--%>
                <asp:Button ID="btnSearch" TabIndex="3" runat="server" Text="Search Audit Information"
                    CssClass="TransactionalButton btn btn-primary" OnClick="btnSearch_Click" />
            </div>
            <div class="divClear">
            </div>
        </div>
        <div class="divClear">
        </div>
        <div style="height: 35px">
        </div>
        <div id="myTabs">
            <ul id="tabPage" class="ui-style">
                <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-1">Room Audit</a></li>
                <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-2">Service Audit </a></li>
            </ul>
            <div id="tab-1">
                <div id="EntryPanel" class="block" style="display: none;">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Approve Information
                    </a>
                    <div class="HMBodyContainer">
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:HiddenField ID="ApprovedIdHiddenField" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtRegistrationId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtRoomIdHiddenField" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtBPPercentAmount" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblGuestName" runat="server" Text="Guest Name"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtGuestName" runat="server" CssClass="ThreeColumnTextBox" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblRoomNumber" runat="server" Text="Room Number"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtRoomNumber" runat="server" TabIndex="5"></asp:TextBox>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblRoomType" runat="server" Text="RoomType"></asp:Label>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:TextBox ID="txtRoomType" runat="server" TabIndex="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblPreviousRoomRate" runat="server" Text="Unit Price"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtPreviousRoomRate" runat="server" TabIndex="7"></asp:TextBox>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblApprovedRoomRate" runat="server" Text="Room Rate"></asp:Label>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:TextBox ID="txtRoomRate" runat="server" TabIndex="8"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                                <asp:HiddenField ID="txtVatAmountPercent" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtServiceChargePercent" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtReferenceSalesCommissionPercent" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtCalculatedPercentAmount" runat="server"></asp:HiddenField>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtVatAmount" runat="server" TabIndex="9"></asp:TextBox>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblServiceCharge" runat="server" Text="Service Charge"></asp:Label>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:TextBox ID="txtServiceCharge" runat="server" TabIndex="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblReferenceSalesCommission" runat="server" Text="Ref. Sales Commission"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtReferenceSalesCommission" runat="server" TabIndex="9"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="HMContainerRowButton">
                            <div id="btnSaveButtonDiv">
                                <asp:Button ID="btnSave" runat="server" Text="Approve" TabIndex="9" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSave_Click" OnClientClick="return ConfirmApproveMessege();" />
                                <asp:Button ID="btnCancel" runat="server" Text="Close" TabIndex="10" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return EntryPanelVisibleFalse();" OnClick="btnCancel_Click" />
                            </div>
                            <div id="btnUpdateRoomApprovedDataButtonDiv">
                                <asp:Button ID="btnUpdateRoomApprovedData" runat="server" Text="Update" TabIndex="9"
                                    CssClass="TransactionalButton btn btn-primary" OnClick="btnUpdateRoomApprovedData_Click"
                                    OnClientClick="return ConfirmApproveMessegeForUpdate();" />
                                <asp:Button ID="btnCancelRoomApproved" runat="server" Text="Close" TabIndex="10"
                                    CssClass="TransactionalButton btn btn-primary" OnClientClick="javascript: return EntryPanelVisibleFalse();"
                                    OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="RoomAuditPanel" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Room Audit Information
                    </a>
                    <div class="HMBodyContainer">
                        <div>                        
                            <asp:GridView ID="gvAvailableGuestList" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" PageSize="100" OnPageIndexChanging="gvAvailableGuestList_PageIndexChanging"
                                OnRowDataBound="gvAvailableGuestList_RowDataBound" OnRowCommand="gvAvailableGuestList_RowCommand">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ApprovedId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApprovedId" runat="server" Text='<%#Eval("ApprovedId") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IDApprovedStatus" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="05%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBoxAccept" Checked="true" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RoomId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoomId" runat="server" Text='<%#Eval("RoomId") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Room No" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvRoomNumber" runat="server" Text='<%# Bind("RoomNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Guest Name" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvGuestName" runat="server" Text='<%# Bind("GuestName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Room Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvRoomType" runat="server" Text='<%# Bind("RoomType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Room Rent">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvRoomRate" runat="server" Text='<%# Bind("RoomRate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Percent Amount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvBPPercentAmount" runat="server" Text='<%# Bind("BPPercentAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount Amount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvBPDiscountAmount" runat="server" Text='<%# Bind("BPDiscountAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvDisplayBPDiscountAmount" runat="server" Text=''></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Calculated Amount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvCalculatedRoomRateAmount" runat="server" Text='<%# Bind("CalculatedRoomRate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vat Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvVatAmount" runat="server" Text='<%# Bind("VatAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S. Charge">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvServiceCharge" runat="server" Text='<%# Bind("ServiceCharge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sales Commission" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvReferenceSalesCommission" runat="server" Text='<%# Bind("ReferenceSalesCommission") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VatAmountPercent" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvVatAmountPercent" runat="server" Text='<%# Bind("VatAmountPercent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ServiceChargePercent" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvServiceChargePercent" runat="server" Text='<%# Bind("ServiceChargePercent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CalculatedPercentAmount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvCalculatedPercentAmount" runat="server" Text='<%# Bind("CalculatedPercentAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvTotalCalculatedAmount" runat="server" Text='<%# Bind("TotalCalculatedAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                                ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgApprove" runat="server" CausesValidation="False" CommandName="CmdApprove"
                                                CommandArgument='<%# bind("RegistrationId") %>' ImageUrl="~/Images/save.png"
                                                Text="" AlternateText="Approve" ToolTip="Approve" OnClientClick="return confirm('Do you want to Approve for entered Approve date?');" />
                                            &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApproved"
                                                ImageUrl="~/Images/select.png" Text="" AlternateText="Approved" ToolTip="Approved"
                                                Enabled="False"    />
                                            &nbsp;<asp:ImageButton ID="ImgEdit" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                                ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                                CommandArgument='<%# bind("RegistrationId") %>' ImageUrl="~/Images/delete.png"
                                                Text="" AlternateText="Cancel" ToolTip="Cancel" OnClientClick="return confirm('Do you want to Cancel Night Audit?');" />
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
                            <asp:GridView ID="gvEarlyCheckInAvailableGuestList" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" PageSize="100" OnPageIndexChanging="gvEarlyCheckInAvailableGuestList_PageIndexChanging"
                                OnRowDataBound="gvEarlyCheckInAvailableGuestList_RowDataBound" OnRowCommand="gvEarlyCheckInAvailableGuestList_RowCommand" ShowHeader="False">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ApprovedId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApprovedId" runat="server" Text='<%#Eval("ApprovedId") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IDApprovedStatus" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="05%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBoxAccept" Checked="true" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RoomId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoomId" runat="server" Text='<%#Eval("RoomId") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Room No" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvRoomNumber" runat="server" Text='<%# Bind("RoomNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Guest Name" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvGuestName" runat="server" Text='<%# Bind("GuestName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Room Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvRoomType" runat="server" Text='<%# Bind("RoomType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Room Rent">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvRoomRate" runat="server" Text='<%# Bind("RoomRate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Percent Amount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvBPPercentAmount" runat="server" Text='<%# Bind("BPPercentAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount Amount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvBPDiscountAmount" runat="server" Text='<%# Bind("BPDiscountAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvDisplayBPDiscountAmount" runat="server" Text=''></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Calculated Amount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvCalculatedRoomRateAmount" runat="server" Text='<%# Bind("CalculatedRoomRate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vat Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvVatAmount" runat="server" Text='<%# Bind("VatAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S. Charge">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvServiceCharge" runat="server" Text='<%# Bind("ServiceCharge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sales Commission" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvReferenceSalesCommission" runat="server" Text='<%# Bind("ReferenceSalesCommission") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VatAmountPercent" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvVatAmountPercent" runat="server" Text='<%# Bind("VatAmountPercent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ServiceChargePercent" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvServiceChargePercent" runat="server" Text='<%# Bind("ServiceChargePercent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CalculatedPercentAmount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvCalculatedPercentAmount" runat="server" Text='<%# Bind("CalculatedPercentAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvTotalCalculatedAmount" runat="server" Text='<%# Bind("TotalCalculatedAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                                ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgApprove" runat="server" CausesValidation="False" CommandName="CmdApprove"
                                                CommandArgument='<%# bind("RegistrationId") %>' ImageUrl="~/Images/save.png"
                                                Text="" AlternateText="Approve" ToolTip="Approve" OnClientClick="return confirm('Do you want to Approve for entered Approve date?');" />
                                            &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApproved"
                                                ImageUrl="~/Images/select.png" Text="" AlternateText="Approved" ToolTip="Approved"
                                                Enabled="False"    />
                                            &nbsp;<asp:ImageButton ID="ImgEdit" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                                ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                                CommandArgument='<%# bind("RegistrationId") %>' ImageUrl="~/Images/delete.png"
                                                Text="" AlternateText="Cancel" ToolTip="Cancel" OnClientClick="return confirm('Do you want to Cancel Night Audit?');" />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="Small" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                               <%-- <EmptyDataTemplate>
                                    <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                                </EmptyDataTemplate>--%>
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="HMContainerRowButton">
                            <%--Right Left--%>
                            <asp:Button ID="btnSaveAll" runat="server" Text="Approve All" OnClientClick="return ConfirmApproveMessege();"
                                CssClass="TransactionalButton btn btn-primary" OnClick="btnSaveAll_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="tab-2">
                <div id="Div1" class="block">
                    <div id="ServiceApprovedDiv" class="block" style="display: none;">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">Approve Information
                        </a>
                        <div class="HMBodyContainer">
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblServiceRoomNumber" runat="server" Text="Room Number"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtServiceRoomNumber" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblRegistrationNumber" runat="server" Text="Registration Number"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtRegistrationNumber" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblServiceId" runat="server" Text="Service"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtServiceName" runat="server" Enabled="false" CssClass="ThreeColumnTextBox"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                <asp:HiddenField ID="ServiceApprovedIdHiddenField" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtServiceBillId" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtServiceRegistrationId" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtServiceServiceId" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtServiceRegistrationNumber" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtServiceType" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtServiceServiceName" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtServiceServiceDate" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblServiceDate" runat="server" Text="Service Date"></asp:Label>
                                    <asp:HiddenField ID="txtPaymentMode" runat="server"></asp:HiddenField>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtServiceDate" runat="server" CssClass="customSmallTextBoxSize"
                                        TabIndex="7" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblServiceRate" runat="server" Text="Service Rate"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtServiceRate" runat="server" CssClass="customSmallTextBoxSize"
                                        TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblGuestServiceVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                                    <asp:HiddenField ID="txtGuestServiceVatAmountPercent" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtGuestServiceServiceChargePercent" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtGuestServiceCalculatedPercentAmount" runat="server"></asp:HiddenField>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtGuestServiceVatAmount" runat="server" TabIndex="9"></asp:TextBox>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblServiceQuantity" runat="server" Text="Quantity"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtServiceQuantity" runat="server" CssClass="customSmallTextBoxSize"
                                        TabIndex="5"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblGuestServiceServiceCharge" runat="server" Text="Service Charge"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtGuestServiceServiceCharge" runat="server" TabIndex="10"></asp:TextBox>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblDiscountAmount" runat="server" Text="Discount Amount"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="customSmallTextBoxSize"
                                        TabIndex="6"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                            <div id="ServiceBillDiv">
                                <asp:Button ID="btnServiceApproved" runat="server" Text="Approve" OnClientClick="return ConfirmApproveMessege();"
                                    TabIndex="9" CssClass="TransactionalButton btn btn-primary" OnClick="btnServiceApproved_Click" />
                                <asp:Button ID="btnCloseServiceAudit" runat="server" Text="Close" TabIndex="10" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return EntryServicePanelVisibleFalse();" OnClick="btnCancel_Click" />
                                    </div>
                                    <div id="ServiceBillApprovedDiv">
                                <asp:Button ID="btnUpdateApprovedService" runat="server" Text="Update" OnClientClick="return ConfirmApproveMessege();"
                                    TabIndex="9" CssClass="TransactionalButton btn btn-primary" OnClick="btnUpdateApprovedService_Click" />
                                <asp:Button ID="btnCanceApprovedService" runat="server" Text="Close" TabIndex="10" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return EntryServicePanelVisibleFalse();" OnClick="btnCancel_Click" />
                                    </div>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Service Details Information
                    </a>
                    <div class="block-body collapse in">
                        <asp:GridView ID="gvGHServiceBill" Width="100%" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                            ForeColor="#333333" TabIndex="9" OnPageIndexChanging="gvGHServiceBill_PageIndexChanging"
                            OnRowDataBound="gvGHServiceBill_RowDataBound" OnRowCommand="gvGHServiceBill_RowCommand"
                            PageSize="100">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="ApprovedId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedId" runat="server" Text='<%#Eval("ApprovedId") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceBillId") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RegiId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegiId" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ServiceApprovedStatus" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ServiceType" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceType" runat="server" Text='<%#Eval("ServiceType") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ServiceId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="05%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBoxAccept" Checked="true" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvServiceDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ServiceDate"))) %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Room" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoomNumber" runat="server" Text='<%#Eval("RoomNumber") %>'></asp:Label></ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bill No." ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNumber" runat="server" Text='<%#Eval("BillNumber") %>'></asp:Label></ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label></ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceRate" runat="server" Text='<%#Eval("ServiceRate") %>'></asp:Label></ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceQuantity" runat="server" Text='<%#Eval("ServiceQuantity") %>'></asp:Label></ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Disc." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiscountAmount" runat="server" Text='<%#Eval("DiscountAmount") %>'></asp:Label></ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vat" ItemStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvVatAmount" runat="server" Text='<%# Bind("VatAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S. Charge" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvServiceCharge" runat="server" Text='<%# Bind("ServiceCharge") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvTotalCalculatedAmount" runat="server" Text='<%# Bind("TotalCalculatedAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="VatAmountPercent" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvVatAmountPercent" runat="server" Text='<%# Bind("VatAmountPercent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ServiceChargePercent" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvServiceChargePercent" runat="server" Text='<%# Bind("ServiceChargePercent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CalculatedPercentAmount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvCalculatedPercentAmount" runat="server" Text='<%# Bind("CalculatedPercentAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                            ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                        &nbsp;<asp:ImageButton ID="ImgApprove" runat="server" CausesValidation="False" CommandName="CmdApprove"
                                            CommandArgument='<%# bind("ServiceBillId") %>' ImageUrl="~/Images/save.png" Text=""
                                            AlternateText="Approve" ToolTip="Approve" OnClientClick="return ConfirmApproveMessege();" />
                                        &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApproved"
                                            ImageUrl="~/Images/select.png" Text="" AlternateText="Approved" ToolTip="Approved"
                                            Enabled="False" />
                                        &nbsp;<asp:ImageButton ID="ImgEdit" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                            ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                        &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                            CommandArgument='<%# bind("ServiceBillId") %>' ImageUrl="~/Images/delete.png"
                                            Text="" AlternateText="Cancel" ToolTip="Cancel" OnClientClick="return confirm('Do you want to Cancel Night Audit?');" />
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
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <%--Right Left--%>
                        <asp:Button ID="btnAllServiceBillApprove" runat="server" Text="Approve All" OnClientClick="return ConfirmApproveMessege();"
                            CssClass="TransactionalButton btn btn-primary" OnClick="btnAllServiceBillApproved_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {
            MessagePanelHide();
        }
    </script>
</asp:Content>
