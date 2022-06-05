<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpGrade.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpGrade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Administrative & Security</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Grade</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            <%--var txtName = '<%=txtName.ClientID%>'

            $('#' + txtName).blur(function () {
                var gradeName = $('#' + txtName).val();
                if (jQuery.trim(gradeName) == '') {
                    toastr.info("Grade Name must not empty");
                }
            });--%>
        });

        //        $(document).ready(function () {            
        //            var txtName = '<%=txtName.ClientID%>'

        //            $('#' + txtName).blur(function () {
        //                var txtName = $('#' + txtName).val();
        //                alert(txtName);
        //                if (jQuery.trim(txtName) == '') {                    
        //                    toastr.info("Grade Name must not empty");
        //                }                
        //            });
        //        });

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }




        function OnFillFormObjectSucceeded(result) {
            if (result.StatusId != 2) {
                $("#<%=txtGradeId.ClientID %>").val(result.GradeId);
                $("#<%=txtName.ClientID %>").val(result.Name);
                $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
                $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
                $("#<%=btnSave.ClientID %>").val("Update");
                $('#btnNewGrade').hide("slow");
                $('#EntryPanel').show("slow");
            }
            else {
                toastr.info("This is not edditable.");
            }
        }


        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
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
            window.location = "frmEmpGrade.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformClearActionForButton() {

            if (!confirm("Do you want to clear?")) {
                return false;
            }
            
            PerformClearAction();
        }

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtName.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=ddlEmpProvisionPeriod.ClientID %>").val(0);
            $("#<%=txtGradeId.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#ContentPlaceHolder1_chkIsManagement").prop("checked", false);
            
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewGrade').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewGrade').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }
        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewGrade').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewGrade').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Grade Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Grade </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Grade Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtGradeId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Grade Name"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblEmpProvisionPeriod" runat="server" class="control-label" Text="Provision Period"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlEmpProvisionPeriod" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control"
                                    TabIndex="3">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <asp:CheckBox ID="chkIsManagement" runat="server" class="control-label" Text="" CssClass="customChkBox"
                                    TabIndex="8" />
                                <asp:Label ID="lblIsManagement" runat="server" class="control-label" Text="Is Management"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" TabIndex="4" />
                                <%--<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearActionForButton();" TabIndex="5" />--%>
                                <input type="button" value="Clear"  id="btnClear" onclick="PerformClearActionForButton()" class="TransactionalButton btn btn-primary btn-sm"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Grade Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSGradeName" runat="server" class="control-label required-field" Text="Grade Name"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSGradeName" runat="server" CssClass="form-control"
                                    TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control"
                                    TabIndex="3">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" TabIndex="4" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvGrade" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="30"
                        OnPageIndexChanging="gvGrade_PageIndexChanging" OnRowDataBound="gvGrade_RowDataBound"
                        OnRowCommand="gvGrade_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("GradeId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Grade Name" ItemStyle-Width="40%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IsManagementText" HeaderText="Is Management" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("GradeId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" OnClientClick="return confirm('Do you want to edit?');" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("GradeId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to delete?');" />
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
        }
        else {
            NewAddButtonPanelHide();
        }
    </script>
</asp:Content>
