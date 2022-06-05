<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmInterviewType.aspx.cs" Inherits="HotelManagement.Presentation.Website.Recruitment.frmInterviewType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Administrative & Security</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Interview Type</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            CommonHelper.ApplyDecimalValidation();
        });

        $(function () {
            $("#myTabs").tabs();
        });        
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Interview Type</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Interview Type</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Interview Type Info</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:HiddenField ID="hfInterviewTypeId" runat="server"></asp:HiddenField>
                            <div class="col-md-2">
                                <asp:Label ID="lblInterviewType" runat="server" class="control-label required-field" Text="Interview Type"></asp:Label>                               
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtInterviewType" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblMarks" runat="server" class="control-label required-field" Text="Marks"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMarks" runat="server" CssClass="form-control quantitydecimal" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="5" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="6" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">                
                <div class="panel-heading">Search Information</div> 
                <div class="panel-body">
                    <asp:GridView ID="gvInterviewType" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" OnPageIndexChanging="gvInterviewType_PageIndexChanging" OnRowCommand="gvInterviewType_RowCommand"
                        OnRowDataBound="gvInterviewType_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("InterviewTypeId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="InterviewName" HeaderText="Interview Name" ItemStyle-Width="70%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Marks" HeaderText="Marks" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("InterviewTypeId") %>' ImageUrl="~/Images/edit.png"
                                        Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("InterviewTypeId") %>' ImageUrl="~/Images/delete.png"
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
</asp:Content>
