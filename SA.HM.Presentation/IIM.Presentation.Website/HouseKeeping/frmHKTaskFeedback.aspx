<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmHKTaskFeedback.aspx.cs" Inherits="HotelManagement.Presentation.Website.HouseKeeping.frmHKTaskFeedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>House Keeping</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Task Feedback</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $(".dp").timepicker({
                showPeriod: is12HourFormat
            });

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("[id=ContentPlaceHolder1_gvTaskFeedback_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvTaskFeedback tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvTaskFeedback tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

        });
        function btnSearch_Client_Click() {
            if ($("#ContentPlaceHolder1_ddlShift").val() == "") {
                toastr.warning("Please select the Shift");
                    return false;
            }
        }

    </script>
    <asp:HiddenField ID="hfTemplateNo" runat="server" />
    <asp:HiddenField ID="hfEmpId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDepartmentId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">

                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="LblShift" runat="server" class="control-label" Text="Shift"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlShift" runat="server" CssClass="form-control" TabIndex="2"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlShift_Change">
                            <asp:ListItem Value="">---Please Select---</asp:ListItem>
                            <asp:ListItem Value="AM">AM</asp:ListItem>
                            <asp:ListItem Value="PM">PM</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Task Assignment Sequence"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTaskSequence" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>

            </div>
            <div class="row">
                <div class="form-group">
                    <div id="AllClean" style="display: none;">
                        <div class="col-md-2">
                            <asp:Label ID="lblAllClean" runat="server" class="control-label" Text="Make all Room Clean"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlAllClean" runat="server" CssClass="form-control" TabIndex="1"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlAllClean_Change">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchDiv" class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="4" CssClass="btn btn-primary btn-sm"
                        OnClick="btnSearch_Click" OnClientClick="btnSearch_Client_Click()" />
                </div>
            </div>
            <div id="TaskFeedback" runat="server" style="padding-top: 20px;">
                <asp:GridView ID="gvTaskFeedback" Width="100%" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                    ForeColor="#333333" PageSize="30" OnPageIndexChanging="gvTaskFeedback_PageIndexChanging"
                    OnRowDataBound="gvTaskFeedback_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblTaskId" runat="server" Text='<%#Eval("TaskId") %>'></asp:Label>
                                <asp:Label ID="lblRoomId" runat="server" Text='<%#Eval("RoomId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="2%">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="RoomNumber" HeaderText="Room" ItemStyle-Width="5%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RoomType" HeaderText="Type" ItemStyle-Width="8%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="FO Status" ShowHeader="False" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Label ID="lblFORoomStatus" runat="server" Text='<%# Eval("FORoomStatus") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Assigned Employee" ShowHeader="False" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("EmpId") %>'
                                    Visible="false" />
                                <asp:DropDownList ID="ddlAssignedEmployee" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="HK Status" ShowHeader="False" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Label ID="lblHKRoomStatus" runat="server" Text='<%# Eval("HKStatusName") %>'
                                    Visible="false" />
                                <asp:DropDownList ID="ddlHKRoomStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="In Time" ShowHeader="False" ItemStyle-Width="7%">
                            <ItemTemplate>
                                <asp:TextBox ID="txtInTime" runat="server" CssClass="form-control dp" Text='<%# GetStringFromDateTime(Convert.ToString(Eval("InTime"))) %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Out Time" ShowHeader="False" ItemStyle-Width="7%">
                            <ItemTemplate>
                                <asp:TextBox ID="txtOutTime" runat="server" CssClass="form-control dp" Text='<%# GetStringFromDateTime(Convert.ToString(Eval("OutTime"))) %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Feedback" ShowHeader="False" ItemStyle-Width="26%">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFeedback" runat="server" CssClass="form-control" Text='<%#Eval("Feedbacks") %>'></asp:TextBox>
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
            <div id="SaveDiv" class="row" style="display: none;">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" CssClass="btn btn-primary btn-sm"
                        OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
        });

        var x = '<%=IsSuccess%>';
        if (x > -1) {
            $('#SaveDiv').show();
        }

    </script>
</asp:Content>
