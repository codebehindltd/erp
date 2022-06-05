<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmHKRoomConditions.aspx.cs" Inherits="HotelManagement.Presentation.Website.HouseKeeping.frmHKRoomConditions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/MultipleSelectDropDown/CSS/multiple-select.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>House Keeping</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Conditions</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("[id=ContentPlaceHolder1_gvRoomConditions_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvRoomConditions tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvRoomConditions tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });
        });
    </script>
    <div class="panel panel-default">
        <div class="panel-body">
            <div id="RoomConditions" runat="server">
                <asp:GridView ID="gvRoomConditions" Width="100%" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                    ForeColor="#333333" PageSize="1000" OnRowDataBound="gvRoomConditions_RowDataBound"
                    OnPageIndexChanging="gvRoomConditions_PageIndexChanging" CssClass="table table-bordered table-condensed table-responsive">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblRoomConId" runat="server" Text='<%#Eval("DailyRoomConditionId") %>'></asp:Label>
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
                        <asp:BoundField DataField="FORoomStatus" HeaderText="FO Status" ItemStyle-Width="15%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HKRoomStatus" HeaderText="HK Status" ItemStyle-Width="15%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="Features" HeaderText="Features" ItemStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>--%>
                        <asp:TemplateField HeaderText="Room Condition" ShowHeader="False" ItemStyle-Width="45%">
                            <ItemTemplate>
                                <asp:Label ID="lblCondition" runat="server" Text='<%# Eval("Conditions") %>' Visible="false" />
                                <asp:ListBox ID="ddlRoomConditions" runat="server" multiple="multiple" SelectionMode="Multiple" Width="50%">
                                </asp:ListBox>
                                <%--<select multiple="multiple" style="width:250px;">
                                    <option value="1">January</option>
                                    <option value="2">February</option>
                                    <option value="3">March</option>
                                    <option value="4">April</option>
                                    <option value="5">May</option>
                                    <option value="6">June</option>
                                </select>--%>
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
    <script src="/MultipleSelectDropDown/JS/multiple-select.js"></script>
    <script>
        $('select').multipleSelect();
    </script>
</asp:Content>
