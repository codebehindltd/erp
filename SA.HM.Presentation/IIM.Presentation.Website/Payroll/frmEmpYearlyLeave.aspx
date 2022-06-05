<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpYearlyLeave.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpYearlyLeave" %>

<%@ Register TagName="EmployeeSearch" TagPrefix="UserControl" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Yearly Leave</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_gvEmpLeave tbody tr:eq(1)").remove();
        });

        function PerformSaveAction() {
            var yearlyLeaveId = $("#<%=hiddenLeaveID.ClientID %>").val();
            var employeeId = $("#ContentPlaceHolder1_employeeeSearch_hfEmployeeId").val();
            var leaveType = $("#ContentPlaceHolder1_ddlLeaveType").val();
            var leaveQuantity = $("#ContentPlaceHolder1_txtLeave").val();

            if (employeeId == 0) {
                toastr.warning("Please Provide an Employee.");
                return;
            }
            else if (leaveType == 0) {
                toastr.warning("Please Provide Leave Type.");
                return;
            }
            else if (leaveQuantity == "") {
                toastr.warning("Please Provide Leave Amount.");
                return;
            }
            else {

                PageMethods.SaveYearlyLeave(yearlyLeaveId, employeeId, leaveType, leaveQuantity, OnSaveYearlyLeaveSuccedd, OnSaveYearlyLeaveFailed);
                return false;
            }
        }

        function OnSaveYearlyLeaveSuccedd(result) {
            //DisplayMessage(result.MessageType, result.Message);
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(1);
            PerformClearAction();
        }
        function OnSaveYearlyLeaveFailed(error) {
            toastr.error(error);
        }

        function OnYearlyLeaveLoadSucceed(result) {

            $("#ContentPlaceHolder1_gvEmpLeave tr:not(:first-child)").remove();
            $("#GridPagingContainer ul").html("");

            if (result.EmployeeLeave == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"3\" >No Data Found</td> </tr>";
                $("#ContentPlaceHolder1_gvEmpLeave tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#ContentPlaceHolder1_gvEmpLeave tbody tr").length + 1;
                totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:17%;\">" + gridObject.LeaveType + "</td>";
                tr += "<td align='left' style=\"width:51%;\">" + gridObject.LeaveQuantity + "</td>";

                editLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformFillFormAction('" + gridObject.YearlyLeaveId + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" /> </a>";
                deleteLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformDeleteAction('" + gridObject.YearlyLeaveId + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

                tr += "<td align='center' style=\"width:15%;\">" + editLink + deleteLink + "</td>";

                tr += "</tr>"

                $("#ContentPlaceHolder1_gvEmpLeave tbody ").append(tr);
                tr = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

        }
        function OnYearlyLeaveLoadFailed(error) {

        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#ContentPlaceHolder1_gvEmpLeave tbody tr").length - 1;
            PageMethods.YearlyLeaveLoad($("#ContentPlaceHolder1_employeeeSearch_hfEmployeeId").val(), gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnYearlyLeaveLoadSucceed, OnYearlyLeaveLoadFailed);
            return false;
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#<%=hiddenLeaveID.ClientID %>").val(result.YearlyLeaveId);
            $("#<%=txtLeave.ClientID %>").val(result.LeaveQuantity);
            $("#<%=btnSave.ClientID %>").val("Update");
            $("#<%= ddlLeaveType.ClientID %>").val(result.LeaveTypeId);

            return false;
        }
        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }
        function PerformClearAction() {

            $("#<%=hiddenLeaveID.ClientID %>").val("");
            $("#<%= ddlLeaveType.ClientID %>").val("0");
            $("#<%=txtLeave.ClientID %>").val("");
            $("#<%=btnSave.ClientID %>").val("Save");

            //$("#ContentPlaceHolder1_gvEmpLeave tr:not(:first-child)").remove();
            //$("#GridPagingContainer ul").html("");

            return false;
        }

        //For Delete-------------------------        
        function PerformDeleteAction(actionId, pageNumber) {

            if (!confirm('Do you want to Delete?'))
                return false;

            PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            return false;
        }
        function OnDeleteObjectSucceeded(result) {
            //DisplayMessage(result.MessageType, result.Message);
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(0);
            PerformClearAction();
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //----Numeric Validation-----------------------
        function fixedlength(textboxID, keyEvent, maxlength) {
            //validation for digits upto 'maxlength' defined by caller function
            if (textboxID.value.length > maxlength) {
                textboxID.value = textboxID.value.substr(0, maxlength);
            }
            else if (textboxID.value.length < maxlength || textboxID.value.length == maxlength) {
                textboxID.value = textboxID.value.replace(/[^\d]+/g, '');
                return true;
            }
            else
                return false;
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function WorkAfterSearchEmployee() {
            var employeeId = $("#ContentPlaceHolder1_employeeeSearch_hfEmployeeId").val();
            PageMethods.YearlyLeaveLoad(employeeId, 1, 1, 1, OnYearlyLeaveLoadSucceed, OnYearlyLeaveLoadFailed);
        }

    </script>
    <asp:HiddenField ID="hiddenLeaveID" runat="server"></asp:HiddenField>
    <div id="EducationInformation" class="block">
        <div class="panel panel-default">
            <div class="panel-heading">
                Leave Information
            </div>
            <%-- <a href="#page-stats" class="block-heading" data-toggle="collapse">Leave Information
            </a>--%>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="HMBodyContainer">
                        <UserControl:EmployeeSearch runat="server" ID="employeeeSearch" />
                        <div class="divClear">
                        </div>
                        <div class="divSection form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLeaveType" class="control-label required-field" runat="server" Text="Leave Type"></asp:Label>
                                <%--<span class="MandatoryField">*</span>--%>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLeaveType" runat="server" CssClass=" form-control"
                                    TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" class="control-label required-field" runat="server" Text="Leave Amount"></asp:Label>
                                <%--<span class="MandatoryField">*</span>--%>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox CssClass=" form-control" ID="txtLeave" class="numeric" TabIndex="3" runat="server" onblur="return fixedlength(this, event, 2);"
                                    onkeypress="return fixedlength(this, event, 2);" onkeyup="return fixedlength(this, event, 2);"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="HMContainerRowButton">
                            <%--Right Left--%>
                            <asp:Button ID="btnSave" runat="server" TabIndex="4" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="return PerformSaveAction();" />
                            <asp:Button ID="Button2" runat="server" TabIndex="5" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return PerformClearAction();" />
                        </div>
                        &nbsp;
                        &nbsp;
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Search Information
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvEmpLeave" Width="100%" runat="server" AutoGenerateColumns="False"
                                    CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333"
                                    CssClass="table table-bordered table-condensed table-responsive">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:BoundField DataField="LeaveType" HeaderText="Type Leave" ItemStyle-Width="17%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LeaveQuantity" HeaderText="Leave Quantity" ItemStyle-Width="51%">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                            <ItemTemplate>
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
                                <div class="childDivSection">
                                    <div class="text-center" id="GridPagingContainer">
                                        <ul class="pagination">
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--<div class="divClear">
                        </div>
                        <div class="childDivSection">
                            <div class="pagination pagination-centered" id="GridPagingContainer">
                                <ul>
                                </ul>
                            </div>
                        </div>--%>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
