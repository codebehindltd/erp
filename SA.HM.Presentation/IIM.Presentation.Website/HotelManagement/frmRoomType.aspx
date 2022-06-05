<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmRoomType.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmRoomType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var txtRoomRate = '<%=txtRoomRate.ClientID%>'
            var lblMessage = '<%=lblMessage.ClientID%>'

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Type</li>";
            var breadCrumbs = moduleName + formName;
            var adjustmentAccountHeadId = 0;
            adjustmentAccountHeadId = $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val();
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            CommonHelper.ApplyDecimalValidation();
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#' + txtRoomRate).blur(function () {
                var roomRate = $('#' + txtRoomRate).val();
                if (roomRate >= 0) {
                    $('#' + lblMessage).text("");
                }
                else {
                    //$('#' + lblMessage).text("Vat amount is not in correct format");
                    toastr.info("Vat amount is not in correct format");
                }
            });
            $("#ContentPlaceHolder1_ddlAccountHead").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
        });

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#<%=txtRoomType.ClientID %>").val(result.RoomType);
            $("#<%=txtRoomRate.ClientID %>").val(result.RoomRate);
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=txtRoomTypeId.ClientID %>").val(result.RoomTypeId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewRoomType').hide("slow");
            $('#EntryPanel').show("slow");
            $("#ContentPlaceHolder1_ddlAccountHead").val(result.CompanyPayment.AdjustmentAccountHeadId + '').trigger('change');
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
            window.location = "frmRoomType.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=txtRoomType.ClientID %>").val('');
            $("#<%=txtRoomRate.ClientID %>").val('');
            $("#<%=txtPaxQuantity.ClientID %>").val('');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=txtRoomTypeId.ClientID %>").val('');
            $("#<%=txtTypeCode.ClientID %>").val('');
            $("#<%=txtRoomRateUSD.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            $("#ContentPlaceHolder1_ddlAccountHead").val('0').trigger('change');

            return false;
        }

        //Div Visible True/False-------------------
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
        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewRoomType').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewRoomType').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <asp:HiddenField ID="hfIsFrontOfficeIntegrateWithAccounts" runat="server"></asp:HiddenField>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Room Type Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Room Type </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Room Type Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Room Type"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:HiddenField ID="txtRoomTypeId" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtRoomType" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Type Code"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTypeCode" runat="server" CssClass="form-control" TabIndex="1" MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Suite Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSuiteType" runat="server" TabIndex="3" CssClass="form-control">
                                    <asp:ListItem Value="0">Yes</asp:ListItem>
                                    <asp:ListItem Value="1">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div style="display: none;">
                            <div id="USDCurrencyInfo" runat="server">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblSellingPriceLocal" runat="server" class="control-label" Text="Selling Price One"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlSellingPriceLocal" runat="server" CssClass="customSmallDropDownSize"
                                            TabIndex="7" Visible="False">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtSellingPriceLocal" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblSellingPriceUsd" runat="server" class="control-label" Text="Selling Price Two"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlSellingPriceUsd" runat="server" CssClass="customSmallDropDownSize"
                                            TabIndex="10" Visible="False">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtSellingPriceUsd" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLocalRoomRate" runat="server" class="control-label required-field" Text="Room Rate(BDT)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRoomRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="2"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblUsdRoomRate" runat="server" class="control-label required-field" Text="Room Rate(USD)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRoomRateUSD" runat="server" CssClass="form-control quantitydecimal" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div id="dvMinimumRoomRate" class="form-group" runat="server">
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Minimum Room Rate(BDT)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMinimumRoomRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="2"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Minimum Room Rate(USD)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMinimumRoomRateUSD" runat="server" CssClass="form-control quantitydecimal" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Pax Quantity"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPaxQuantity" runat="server" CssClass="form-control quantityint" TabIndex="4"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label required-field" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" TabIndex="3" CssClass="form-control">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <asp:Panel runat="server" ID="pnlIsFrontOfficeIntegrateWithAccounts">
                            <div class="form-group">
                                <div class="col-sm-2">
                                    <asp:Label ID="lblAccountHead" runat="server" class="control-label required-field" Text="Account Head"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlAccountHead" runat="server" CssClass="form-control" TabIndex="5">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </asp:Panel>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="4" Text="Save" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="5" Text="Clear" CssClass="btn btn-primary btn-sm"
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
                    Room Type Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="RoomType" class="control-label col-sm-2">
                                Room Type</label>
                            <div class="col-md-10">
                                <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtSRoomType" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="SuiteType" class="control-label col-sm-2">
                                Suite Type</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSSuiteType" runat="server" TabIndex="3" CssClass="form-control">
                                    <asp:ListItem Value="-1">--- All ---</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <label for="Status" class="control-label col-sm-2">
                                Status</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSStatus" runat="server" TabIndex="3" CssClass="form-control">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">In Active</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" TabIndex="4" Text="Search" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvRoomTypeInfo" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="100" OnPageIndexChanging="gvRoomTypeInfo_PageIndexChanging"
                        OnRowCommand="gvRoomTypeInfo_RowCommand" OnRowDataBound="gvRoomTypeInfo_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("RoomTypeId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RoomType" HeaderText="Room Type" ItemStyle-Width="35%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RoomRate" HeaderText="Room Rate" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PaxQuantity" HeaderText="Pax Quantity" ItemStyle-Width="15%">
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
                                        CommandArgument='<%# bind("RoomTypeId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("RoomTypeId") %>' ImageUrl="~/Images/delete.png" Text=""
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
    <script type="text/javascript">
        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
            if (parseInt(xNewAdd) == 2) {
                $('#btnNewRoomType').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }
    </script>
</asp:Content>
