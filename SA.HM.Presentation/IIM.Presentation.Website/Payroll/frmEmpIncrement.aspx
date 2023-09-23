<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpIncrement.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpIncrement" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearchAll" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeSearchBasic" Src="~/HMCommon/UserControl/EmployeeSearchWithBasicInfo.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        //Bread Crumbs Information-------------
        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Increment</li>";
            var breadCrumbs = moduleName + formName;

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlSearchDepartment").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            if (IsCanSave) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtEffectiveDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtIncrementDateFrom").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtIncrementDateTo").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
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

            $("#<%=txtEffectiveDate.ClientID %>").blur(function () {

                var date = $("#<%=txtEffectiveDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtEffectiveDate.ClientID %>").focus();
                        $("#<%=txtEffectiveDate.ClientID %>").val("");
                        return false;
                    }
                }
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

        function ClearSearch() {
            $("#ContentPlaceHolder1_ddlSearchDepartment").val("0");

            $("#ContentPlaceHolder1_employeeSearchall_txtSearchEmployee").val("");
            $("#ContentPlaceHolder1_employeeSearchall_txtEmployeeName").val("");

            $("#ContentPlaceHolder1_employeeSearchall_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_employeeSearchall_hfEmployeeName").val("");
        }

        $(document).ready(function () {

            var txtAmount = '<%=txtAmount.ClientID%>'

            //$('#' + txtAmount).blur(function () {
            //    var txtAmount = $('#' + txtAmount).val();
            //    if (jQuery.trim(txtName) == '') {
            //        toastr.warning("Amount must not be empty Increment");
            //    }
            //});

            //$("#ContentPlaceHolder1_gvEmpIncrement tbody tr:eq(1)").remove();
        });
            //$("#ContentPlaceHolder1_txtEffectiveDate").change(function () {
            //    if ($("#ContentPlaceHolder1_ddlIncrementMode").val() == "%") {
            //        $("#ContentPlaceHolder1_txtEffectiveDate").val("100");
            //    }

            //});

            function PerformSaveAction() {

                var employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
                var amount = $("#<%=txtAmount.ClientID %>").val();
            var remarks = $("#<%=txtRemarks.ClientID %>").val();
            var incrementMode = $("#<%=ddlIncrementMode.ClientID %>").val();
            var incrementId = $("#<%=hfIncrementId.ClientID %>").val();
            var effectiveDate = $("#<%=txtEffectiveDate.ClientID %>").val();
            if (employeeId == "0") {
                toastr.warning("Please Select an Employee.");
                return false;
            }
            if (amount == "") {
                toastr.warning("Please Provide amount.");
                return false;
            }
            else if (effectiveDate == "") {
                toastr.warning("Please Provide effective date.");
                return false;
            }

            if (parseFloat(amount) > 100 && incrementMode == "%") {
                toastr.warning("Increment Cann't be more than 100%");
                return false;
            }


            PageMethods.PerformIncrementSaveAction(employeeId, incrementId, incrementMode, amount, remarks, effectiveDate, OnSaveEmployeeIncrementSucceed, OnSaveEmployeeIncrementFailed);
            return false;
        }
        function OnSaveEmployeeIncrementSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(1);
            PerformClearAction();
        }
        function OnSaveEmployeeIncrementFailed(error) {
            toastr.error(error.get_message());
        }

        function OnEmployeeIncrementSucceed(result) {

            /*$("#ContentPlaceHolder1_gvEmpIncrement tr:not(:first-child)").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"3\" >No Data Found</td> </tr>";
                $("#ContentPlaceHolder1_gvEmpIncrement tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "", approvalLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#ContentPlaceHolder1_gvEmpIncrement tbody tr").length + 1;
                totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:17%;\">" + gridObject.Amount + "</td>";
                tr += "<td align='left' style=\"width:51%;\">" + gridObject.Remarks + "</td>";
                tr += "<td align='center' style=\"width:15%;\">";

                if (gridObject.ApprovedStatus != 'Approved') {
                    if (IsCanEdit) {
                        editLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformFillFormAction('" + gridObject.Id + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    }
                    if (IsCanDelete) {
                        deleteLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformDeleteAction('" + gridObject.Id + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"><img alt=\"Delete\" src=\"../Images/delete.png\" title='Delete' /></a>";
                    }
                    if (IsCanSave) {
                        approvalLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformApproval(" + gridObject.Id + ", " + gridObject.EmpId + "," + result.GridPageLinks.CurrentPageNumber + ");\"><img alt=\"Increment Approval\" src=\"../Images/approved.png\" title='Approved Increment' /></a>";
                    }
                    tr += editLink + deleteLink + approvalLink
                }
                tr += "</td>";


                tr += "</tr>"

                $("#ContentPlaceHolder1_gvEmpIncrement tbody ").append(tr);
                tr = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);*/

        }
        function OnEmployeeIncrementFailed(error) {

        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#ContentPlaceHolder1_gvEmpIncrement tbody tr").length - 1;
            PageMethods.LoadEmployeeIncrement($("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val(), gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnEmployeeIncrementSucceed, OnEmployeeIncrementFailed);
            return false;
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {

            $("#<%=hfIncrementId.ClientID %>").val(result.Id);
            $("#<%=txtAmount.ClientID %>").val(result.Amount);
            $("#<%=txtEffectiveDate.ClientID %>").val(GetStringFromDateTime(result.EffectiveDate));
            $("#<%=ddlIncrementMode.ClientID %>").val(result.IncrementMode);
            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
            if (IsCanEdit) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }
            $("#<%=btnSave.ClientID %>").val("Update");
        }
        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //For Delete-------------------------        
        function PerformDeleteAction(actionId) {
            if (confirm("Do you want to delete this record?")) {
                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }
        function OnDeleteObjectSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(0);
            PerformClearAction();
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformApproval(incrementId, empId) {
            if (!confirm("Do you want to approve?")) {
                return false;
            }
            PageMethods.ApproveIncrement(incrementId, empId, OnIncrementSucceeded, OnIncrementFailed);
        }
        function OnIncrementSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(0);
            PerformClearAction();
        }
        function OnIncrementFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformClearActionButton() {
            if (!confirm("Do you want to clear?")) {
                return false;
            }
            PerformClearAction();
        }

        //For ClearForm-------------------------
        function PerformClearAction() {

            $("#<%=hfIncrementId.ClientID %>").text('');
            $("#<%=txtAmount.ClientID %>").val('');
            $("#<%=txtEffectiveDate.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#<%=ddlIncrementMode.ClientID %>").val('%');
            $("#<%=btnSave.ClientID %>").val("Save");
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_employeeSearch_txtEmployeeName").val("");
            $("#ContentPlaceHolder1_employeeSearch_txtSearchEmployee").val("");
            return false;
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        /*function WorkAfterSearchEmployee() {
            var employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            PageMethods.LoadEmployeeIncrement(employeeId, 1, 1, 1, OnEmployeeIncrementSucceed, OnEmployeeIncrementFailed);
        }*/

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

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfIncrementId" runat="server" Value=""></asp:HiddenField>

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="EntryTab" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-1">Increment Entry</a></li>
            <li id="SearchTab" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-2">Increment Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="IncrementInformation" class="panel panel-default">
                <div class="panel-heading">
                    Increment Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeSearch ID="employeeSearch" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAmount" runat="server" class="control-label required-field" Text="Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAmount" runat="server" TabIndex="2" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblIncrementMode" runat="server" class="control-label" Text="Increment Mode"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIncrementMode" runat="server" TabIndex="3" CssClass="form-control">                                    
                                    <asp:ListItem Value="Fixed">Fixed</asp:ListItem>
                                    <asp:ListItem Value="%">%</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEffectiveDate" runat="server" class="control-label required-field"
                                    Text="Effective Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEffectiveDate" CssClass="form-control" runat="server"></asp:TextBox><input
                                    type="hidden" id="hidFromDate" />
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
                                <input type="button" class="TransactionalButton btn btn-primary" value="Clear" onclick="PerformClearActionButton()" tabindex="6" />
                                <%--<asp:Button ID="btnClear" runat="server" TabIndex="6" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                            OnClientClick="javascript: return PerformClearActionButton();" />--%>
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
                        <div class="form-group" id="IncrementDiv" style="display: block;">
                            <div class="col-md-2">
                                <asp:Label ID="lblIncrementDateFrom" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtIncrementDateFrom" CssClass="form-control" runat="server"></asp:TextBox><input
                                    type="hidden" id="hdIncrementDateFrom" />
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblIncrementDateTo" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtIncrementDateTo" CssClass="form-control" runat="server"></asp:TextBox><input
                                    type="hidden" id="hdIncrementDateTo" />
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
            <div id="childDivSection">
                <asp:GridView ID="gvEmpIncrement" Width="100%" runat="server" AllowPaging="True" OnRowCommand="gvEmpIncrement_RowCommand"
                    OnRowDataBound="gvEmpIncrement_RowDataBound" AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="300000" AllowSorting="True"
                    ForeColor="#333333" CssClass="table table-bordered table-condensed table-responsive" OnPageIndexChanging="gvEmpIncrement_PageIndexChanging">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpId" runat="server" Text='<%#Eval("EmpId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="EmployeeCodeAndName" HeaderText="Employee" ItemStyle-Width="25%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IncrementDateDisplay" HeaderText="Inc. Date" ItemStyle-Width="8%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EffectiveDateDisplay" HeaderText="Eff. Date" ItemStyle-Width="8%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="8%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IncrementMode" HeaderText="Inc. Mode" ItemStyle-Width="8%">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" ItemStyle-Width="22%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="10%">
                            <ItemTemplate>
                                &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                    CommandArgument='<%# bind("Id") %>' ImageUrl="~/Images/edit.png" Text=""
                                    AlternateText="Edit" ToolTip="Edit"
                                    OnClientClick="return confirm('Do you want to Edit?');" />
                                &nbsp;<asp:ImageButton ID="ImgDetailsApproved" runat="server" CausesValidation="False"
                                    CommandName="CmdIncrementApproved" CommandArgument='<%# bind("Id") %>'
                                    ImageUrl="~/Images/approved.png" Text="" AlternateText="Details" ToolTip="Approve Increment"
                                    OnClientClick="return confirm('Do you want to Approve?');" />
                                &nbsp;<asp:ImageButton ID="ImgBtnCancelPO" runat="server" CausesValidation="False"
                                    CommandName="CmdIncrementCancel" CommandArgument='<%# bind("Id") %>'
                                    ImageUrl="~/Images/delete.png" Text="" AlternateText="Delete" ToolTip="Delete"
                                    OnClientClick="return confirm('Do you want to Delete?');" />
                                &nbsp;<asp:ImageButton ID="ImgBtnIncrementLater" runat="server" CausesValidation="False"
                                    CommandName="CmdIncrementLater" CommandArgument='<%# bind("Id") %>'
                                    ImageUrl="~/Images/ReportDocument.png" Text="" AlternateText="Promotion Later" ToolTip="Increment Later" />
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
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
