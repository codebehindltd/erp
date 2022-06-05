<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpOverTime.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpOverTime" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>HR Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Overtime</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var txtStartDate = '<%=txtOverTimeDate.ClientID%>'

            $("#ContentPlaceHolder1_gvOvertime tbody tr:eq(1)").remove();

            $("#ContentPlaceHolder1_txtOverTimeDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

        });

        function PerformSaveAction() {

            var employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            var overTimeHour = $("#<%=txtOverTimeHour.ClientID %>").val();
            var overTimeDate = $("#<%=txtOverTimeDate.ClientID %>").val();
            var overTimeId = $("#<%=hfOverTimeId.ClientID %>").val();

            PageMethods.PerformOverTimeSaveAction(employeeId, overTimeId, overTimeHour, overTimeDate, OnSaveOvertimeSucceed, OnSaveOvertimeFailed);
            return false;
        }
        function OnSaveOvertimeSucceed(result) {

            CommonHelper.AlertMessage(result.AlertMessage);

            ReloadGrid(1);
            PerformClearAction();
        }
        function OnSaveOvertimeFailed(error) {
            toastr.error(error.get_message());
        }

        function OnEmployeeOvertimeSucceed(result) {

            $("#ContentPlaceHolder1_gvOvertime tr:not(:first-child)").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"3\" >No Data Found</td> </tr>";
                $("#ContentPlaceHolder1_gvOvertime tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#ContentPlaceHolder1_gvOvertime tbody tr").length + 1;
                totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:17%;\">" + CommonHelper.DateFromDateTimeToDisplay(gridObject.OverTimeDate, innBoarDateFormat) + "</td>";
                tr += "<td align='left' style=\"width:51%;\">" + gridObject.OTHour + "</td>";

                editLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformFillFormAction('" + gridObject.OverTimeId + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" /> </a>";
                deleteLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformDeleteAction('" + gridObject.OverTimeId + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

                tr += "<td align='center' style=\"width:15%;\">" + editLink + deleteLink + "</td>";

                tr += "</tr>"

                $("#ContentPlaceHolder1_gvOvertime tbody ").append(tr);
                tr = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

        }
        function OnEmployeeOvertimeFailed(error) {

        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#ContentPlaceHolder1_gvOvertime tbody tr").length - 1;
            PageMethods.LoadEmployeeOvertime($("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val(), gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnEmployeeOvertimeSucceed, OnEmployeeOvertimeFailed);
            return false;
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {

            $("#<%=txtOverTimeDate.ClientID %>").val(GetStringFromDateTime(result.Date));
            $("#<%=txtOverTimeHour.ClientID %>").val(result.OverTimeHour);
            $("#<%=hfOverTimeId.ClientID %>").val(result.OverTimeId);
            $("#<%=btnSave.ClientID %>").val("Update");

            return false;
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
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(0);
            PerformClearAction();
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //For ClearForm-------------------------
        function PerformClearActionForButton() {

            if (!confirm("Do you want to clear?")) {
                return false;
            }

            PerformClearAction();
        }

        function PerformClearAction() {

            $("#<%=txtOverTimeDate.ClientID %>").val('');
            $("#<%=txtOverTimeHour.ClientID %>").val('');
            $("#<%=hfOverTimeId.ClientID %>").val('');

            $("#<%=btnSave.ClientID %>").val("Save");

            return false;
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function WorkAfterSearchEmployee() {
            var employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            PageMethods.LoadEmployeeOvertime(employeeId, 1, 1, 1, OnEmployeeOvertimeSucceed, OnEmployeeOvertimeFailed);
        }

    </script>
    <asp:HiddenField ID="hfOverTimeId" runat="server"></asp:HiddenField>
    <div id="EmployeeOvertime" class="panel panel-default">
        <div class="panel-heading">Overtime Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <UserControl:EmployeeSearch ID="employeeSearch" runat="server" />
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblOverTimeDate" runat="server" class="control-label required-field" Text="Working Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtOverTimeDate" runat="server" TabIndex="3" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblOverTimeHour" runat="server" class="control-label required-field" Text="Working Hour"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtOverTimeHour" runat="server" TabIndex="4" CssClass="form-control"
                            placeholder="Full day working hour..."></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <%--Right Left--%>
                        <asp:Button ID="btnSave" TabIndex="5" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                            OnClientClick="return PerformSaveAction();" />
                        <asp:Button ID="btnClear" TabIndex="6" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                            OnClientClick="javascript: return PerformClearActionForButton();" />
                    </div>
                </div>
                &nbsp;
                <div id="SearchPanel" class="panel panel-default">
                     <div class="panel-heading">Search Information</div>
                    <div class="panel-body">
                        <asp:GridView ID="gvOvertime" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                            CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="Overtime Date" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvOverTimeDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("OverTimeDate"))) %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="OverTimeHour" HeaderText="Overtime Hour" ItemStyle-Width="50%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
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

                        <div class="childDivSection">
                            <div class="text-center" id="GridPagingContainer">
                                <ul class="pagination">
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
