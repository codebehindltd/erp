<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpPromotion.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpPromotion" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearchAll" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithBasicInfo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Promotion</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            //$("#DepartmentDiv").hide();
            //$("#empSrcDiv").hide();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlDesignationId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSearchDepartment").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            var type = $("#ContentPlaceHolder1_ddlSrcType").val();
            ShowHideSrcDiv(type);

            $("#ContentPlaceHolder1_ddlSrcType").change(function () {
                var type = $("#ContentPlaceHolder1_ddlSrcType").val();
                ShowHideSrcDiv(type);
                ClearSearch();
            });

            $("#ContentPlaceHolder1_txtPromotionDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtPromotionDateFrom").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtPromotionDateTo").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        });
        function ShowHideSrcDiv(type) {
            if (type == "All") {
                $("#DepartmentDiv").show();
                $("#empSrcDiv").hide();

            }
            else if (type == "Individual") {
                $("#DepartmentDiv").hide();
                $("#empSrcDiv").show();
            }
            else {
                $("#DepartmentDiv").hide();
                $("#empSrcDiv").hide();
                ClearSearch();
            }
        }
        function LoadSearch() {
            var type = $("#ContentPlaceHolder1_ddlSrcType").val();
            if (type == "0") {
                toastr.warning("Please Select Search Type");
                return false;
            }
            else if (type == "Individual") {
                var empId = $("#ContentPlaceHolder1_employeeSearchall_hfEmployeeId").val();
                if (empId == "0") {
                    toastr.warning("Please Select an Employee");
                    return false;
                }
            }

            return true;
        }
        $(document).ready(function () {
            $("#ContentPlaceHolder1_gvEmpIncrement tbody tr:eq(1)").remove();
        });

        function PerformSaveAction() {

            var promotionId = $("#ContentPlaceHolder1_hfPromotionId").val();
            var employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            var designationId = $("#ContentPlaceHolder1_ddlDesignationId").val();
            var gradeId = $("#ContentPlaceHolder1_ddlGradeId").val();
            var promotionDate = $("#ContentPlaceHolder1_txtPromotionDate").val();
            var remarks = $("#ContentPlaceHolder1_txtRemarks").val();

            if (employeeId == "0") {
                toastr.warning("Please Select Employee");
                return false;
            }
            else if (designationId == "0") {
                toastr.warning("Please Select Designation");
                return false;
            }
                //else if (gradeId == "0") {
                //    toastr.warning("Please Select Grade");
                //    return false;
                //}
            else if (remarks == "0") {
                toastr.warning("Please Select Remarks");
                return false;
            }
            else if (promotionDate == "") {
                toastr.warning("Please Provide Promotion Date.");
                return false;
            }

            PageMethods.SaveEmployeePromotion(promotionId, employeeId, designationId, gradeId, promotionDate, remarks, OnEmployeePromotionSucceed, OnEmployeePromotionFailed);

            return false;
        }
        function OnEmployeePromotionSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnEmployeePromotionFailed(error) { }


        function PerformClearAction() {

            $("#form1")[0].reset();
            $("#ContentPlaceHolder1_hfPromotionId").val("0");
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_employeeSearch_txtSearchEmployee").val("");
            $("#ContentPlaceHolder1_employeeSearch_txtEmployeeName").val("");
            $("#ContentPlaceHolder1_employeeSearch_txtDepartment").val("");
            $("#ContentPlaceHolder1_employeeSearch_txtDesignation").val("");
            $("#ContentPlaceHolder1_employeeSearch_txtGrade").val("");

            $("#ContentPlaceHolder1_ddlDesignationId").val("0");
            $("#ContentPlaceHolder1_ddlGradeId").val("0");
            $("#ContentPlaceHolder1_txtPromotionDate").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");

            $("#ContentPlaceHolder1_btnSave").val("Save");
        }

        function PerformClearActionWithConfirmation() {
            if (!confirm('Do You Want to Clear?'))
                return false;
            PerformClearAction();
        }

        function WorkAfterSearchEmployee() {
            var employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            //PageMethods.LoadEmployeeIncrement(employeeId, 1, 1, 1, OnEmployeeIncrementSucceed, OnEmployeeIncrementFailed);
        }
        function ClearSearch() {
            $("#ContentPlaceHolder1_ddlSearchDepartment").val("0");

            $("#ContentPlaceHolder1_employeeSearchall_txtSearchEmployee").val("");
            $("#ContentPlaceHolder1_employeeSearchall_txtEmployeeName").val("");

            $("#ContentPlaceHolder1_employeeSearchall_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_employeeSearchall_hfEmployeeName").val("");
        }

    </script>
    <asp:HiddenField ID="hfPromotionId" runat="server" Value="0"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="EntryTab" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-1">Promotion Entry</a></li>
            <li id="SearchTab" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-2">Promotion Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="IncrementInformation" class="panel panel-default">
                <div class="panel-heading">
                    Promotion Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeSearch ID="employeeSearch" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblGradeId" runat="server" class="control-label required-field" Text="Promoted Designation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDesignationId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblProvisionPeriod" runat="server" class="control-label" Text="Promoted Grade"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGradeId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Promotion Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPromotionDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TabIndex="4"
                                    TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnSave" TabIndex="5" runat="server" Text="Save" CssClass="TransactionalButton  btn btn-primary"
                                    OnClientClick="return PerformSaveAction();" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="6" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return PerformClearActionWithConfirmation();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="Div1" class="panel panel-default">
                <div class="panel-heading">
                    Promotion Search
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Search Type"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSrcType" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0" Text="---Please Select---"></asp:ListItem>
                                    <asp:ListItem Value="All" Text="All Employee"></asp:ListItem>
                                    <asp:ListItem Value="Individual" Text="Individual"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>
                        <div class="form-group" id="DepartmentDiv" style="display: none">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Department"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSearchDepartment" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>

                        </div>
                        <div id="empSrcDiv" style="display: none">
                            <UserControl:EmployeeSearchAll ID="employeeSearchall" runat="server" />
                        </div>
                        <div class="form-group" id="PromotionDiv" style="display: block;">
                            <div class="col-md-2">
                                <asp:Label ID="lblPromotionDateFrom" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPromotionDateFrom" CssClass="form-control" runat="server"></asp:TextBox><input
                                    type="hidden" id="hdPromotionDateFrom" />
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPromotionDateTo" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPromotionDateTo" CssClass="form-control" runat="server"></asp:TextBox><input
                                    type="hidden" id="hdPromotionDateTo" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnSearch" TabIndex="5" runat="server" Text="Search" CssClass="TransactionalButton  btn btn-primary"
                                    OnClick="btnSearch_Click" OnClientClick="javascript:return LoadSearch()" />
                                <input id="btnSrcClear" type="button" class="TransactionalButton btn btn-primary" value="Clear" tabindex="11" onclick="ClearSearch()" />

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:GridView ID="gvEmpPromotion" Width="100%" runat="server" AutoGenerateColumns="False"
                CellPadding="4" PageSize="3" GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowCommand="gvEmpPromotion_RowCommand"
                OnRowDataBound="gvEmpPromotion_RowDataBound" CssClass="table table-bordered table-condensed table-responsive" OnPageIndexChanging="gvEmpPromotion_PageIndexChanging">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblEmpId" runat="server" Text='<%#Eval("EmpId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="EmployeeName" HeaderText="Employee" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PromotionDateShow" HeaderText="Date" ItemStyle-Width="8%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PreviousDesignation" HeaderText="Previous Designation"
                        ItemStyle-Width="10%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CurrentDesignation" HeaderText="Current Designation" ItemStyle-Width="10%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PreviousGrade" HeaderText="Previous Grade" ItemStyle-Width="8%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CurrentGrade" HeaderText="Current Grade" ItemStyle-Width="8%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="10%">
                        <ItemTemplate>
                            &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                CommandArgument='<%# bind("PromotionId") %>' ImageUrl="~/Images/edit.png" Text=""
                                AlternateText="Edit" ToolTip="Edit"
                                OnClientClick="return confirm('Do you want to Edit?');" />
                            &nbsp;<asp:ImageButton ID="ImgDetailsApproved" runat="server" CausesValidation="False"
                                CommandName="CmdPromotionApproved" CommandArgument='<%# bind("PromotionId") %>'
                                ImageUrl="~/Images/approved.png" Text="" AlternateText="Details" ToolTip="Approve Promotion"
                                OnClientClick="return confirm('Do you want to Approve?');" />
                            &nbsp;<asp:ImageButton ID="ImgBtnCancelPO" runat="server" CausesValidation="False"
                                CommandName="CmdPromotionCancel" CommandArgument='<%# bind("PromotionId") %>'
                                ImageUrl="~/Images/delete.png" Text="" AlternateText="Delete" ToolTip="Delete"
                                OnClientClick="return confirm('Do you want to Delete?');" />
                            &nbsp;<asp:ImageButton ID="ImgBtnPromotionLater" runat="server" CausesValidation="False"
                                CommandName="CmdPromotionLater" CommandArgument='<%# bind("PromotionId") %>'
                                ImageUrl="~/Images/ReportDocument.png" Text="" AlternateText="Promotion Later" ToolTip="Promotion Later" />
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
