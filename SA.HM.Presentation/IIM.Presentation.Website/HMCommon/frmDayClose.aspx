<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmDayClose.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmDayClose" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Audit Process</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Day Close</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtDayClossingDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                }
            });

            $("#ContentPlaceHolder1_txtKotDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                }
            });

            $(function () {
                $("#myTabs").tabs();
            });
        });

        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

        function ValidateForm() {
            var txtDayClossingDate = '<%=txtDayClossingDate.ClientID%>'
            var date = $('#' + txtDayClossingDate).val();
            if (date == '') {
                toastr.info('Please provide Day Clossing Date.');
                return false;
            }
            else {
                return true;
            }
        }
        function ValidateKotDate() {
            var txtDayClossingDate = '<%=txtKotDate.ClientID%>'
            var date = $('#' + txtKotDate).val();
            if (date == '') {
                toastr.info('Please provide KOT Date.');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="hfDayCloseInformation" runat="server" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Day Close</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Pending KOT</a></li>
        </ul>
        <div id="tab-1">
            <div class="row">
                <div class="col-md-6">
                    <div id="Div2" class="panel panel-default">
                        <div class="panel-heading">
                            Day Close Information
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-md-2 label-align">
                                            <label for="DayCloseDate" class="control-label col-md-2">
                                                Date</label>
                                        </div>
                                        <div class="col-md-10">
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtDayClossingDate" runat="server" CssClass="form-control"
                                                    TabIndex="1"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Button ID="btnProcess" runat="server" Text="Process" OnClientClick="javascript: return ValidateForm();"
                                                    TabIndex="4" CssClass="TransactionalButton btn btn-primary btn-sm" OnClick="btnProcess_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="RoomStatusConfEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Pending KOT List
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group col-md-6">
                            <div class="row">
                                <div class="col-md-2 label-align">
                                    <label for="KOTDate" class="control-label col-md-2">
                                        Date</label>
                                </div>
                                <div class="col-md-10">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtKotDate" runat="server" CssClass="form-control"
                                            TabIndex="1"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="btnKotPendingList" runat="server" Text="Generate" OnClientClick="javascript: return ValidateKotDate();"
                                            TabIndex="4" CssClass="TransactionalButton btn btn-primary btn-sm" OnClick="btnKotPendingList_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12 label-align label-align">
                                <asp:GridView ID="gvKotPendingListInfo" Width="100%" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                    ForeColor="#333333" PageSize="200"
                                    CssClass="table table-bordered table-condensed table-responsive">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="IDNO" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblKotId" runat="server" Text='<%#Eval("KotId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Outlet Name" ItemStyle-Width="70%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvCostCenter" runat="server" Text='<%# Bind("CostCenter") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="KOT No." ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvKotId" runat="server" Text='<%# Bind("KotId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Kot Status" ItemStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlKotStatus" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                                    <asp:ListItem Value="cleaned">Clean</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ControlStyle Font-Size="Small" />
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
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnKotUpdate" runat="server" Text="Process" OnClientClick="javascript: return ValidateKotDate();"
                                    TabIndex="4" CssClass="TransactionalButton btn btn-primary btn-sm" OnClick="btnKotUpdate_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
