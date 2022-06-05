<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmHKRoomDiscrepancies.aspx.cs" Inherits="HotelManagement.Presentation.Website.HouseKeeping.frmHKRoomDiscrepancies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>House Keeping</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Discrepancies</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("[id=ContentPlaceHolder1_gvRoomDiscrepancies_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvRoomDiscrepancies tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvRoomDiscrepancies tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });
        });

        function SetDiscrepancy(rowIndex, foStatus, foPersons) {
            var hkStatus = $("#ContentPlaceHolder1_gvRoomDiscrepancies_ddlHKRoomStatus_" + rowIndex).val();
            var hkPersons = $("#ContentPlaceHolder1_gvRoomDiscrepancies_ddlHKPersons_" + rowIndex).val();

            if (foStatus == "Occupied" && hkStatus == 1) {
                $("#ContentPlaceHolder1_gvRoomDiscrepancies_ddlDiscrepancy_" + rowIndex).val("Skip");
            }
            else if (foStatus == "Vacant" && hkStatus == 2) {
                $("#ContentPlaceHolder1_gvRoomDiscrepancies_ddlDiscrepancy_" + rowIndex).val("Sleep");
            }
            else if (foStatus == "Occupied" && hkStatus == 2) {
                if (foPersons != hkPersons) {
                    $("#ContentPlaceHolder1_gvRoomDiscrepancies_ddlDiscrepancy_" + rowIndex).val("Person");
                }
                else {
                    $("#ContentPlaceHolder1_gvRoomDiscrepancies_ddlDiscrepancy_" + rowIndex).val("None");
                    //$("#ContentPlaceHolder1_gvRoomDiscrepancies_ddlDiscrepancy_" + rowIndex).val("Skip");
                }
            }
            else if (foStatus == "Vacant" && hkStatus == 1) {
                $("#ContentPlaceHolder1_gvRoomDiscrepancies_ddlDiscrepancy_" + rowIndex).val("None");
                //$("#ContentPlaceHolder1_gvRoomDiscrepancies_ddlDiscrepancy_" + rowIndex).val("Skip");
            }
        }
    </script>
    <div class="panel panel-default">
        <div class="panel-body">
            <div id="RoomDiscrepancies" runat="server">
                <asp:GridView ID="gvRoomDiscrepancies" Width="100%" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                    ForeColor="#333333" PageSize="1000" OnPageIndexChanging="gvRoomDiscrepancies_PageIndexChanging"
                    OnRowDataBound="gvRoomDiscrepancies_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblRoomDiscrepancyId" runat="server" Text='<%#Eval("RoomDiscrepancyId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblRoomId" runat="server" Text='<%#Eval("RoomId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="02%">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="RoomNumber" HeaderText="Room" ItemStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RoomType" HeaderText="Type" ItemStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="FO Status" ShowHeader="False" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblFORoomStatus" runat="server" Text='<%# Eval("FORoomStatus") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HK Status" ShowHeader="False" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Label ID="lblHKRoomStatus" runat="server" Text='<%# Eval("HKRoomStatus") %>'
                                    Visible="false" />
                                <asp:DropDownList ID="ddlHKRoomStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FO Persons" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblFOPersons" runat="server" Text='<%#Eval("FOPersons") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HK Persons" ShowHeader="False" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblHKPersons" runat="server" Text='<%# Eval("HKPersons") %>' Visible="false" />
                                <asp:DropDownList ID="ddlHKPersons" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0">0</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discrepancy" ShowHeader="False" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblDiscrepancy" runat="server" Text='<%# Eval("DiscrepanciesDetails") %>'
                                    Visible="false" />
                                <asp:DropDownList ID="ddlDiscrepancy" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="None">None</asp:ListItem>
                                    <asp:ListItem Value="Skip">Skip</asp:ListItem>
                                    <asp:ListItem Value="Sleep">Sleep</asp:ListItem>
                                    <asp:ListItem Value="Person">Person</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reason" ShowHeader="False" ItemStyle-Width="20%">
                            <ItemTemplate>
                                <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" Text='<%#Eval("Reason") %>'></asp:TextBox>
                            </ItemTemplate>
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
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" CssClass="btn btn-primary btn-sm"
                        OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
