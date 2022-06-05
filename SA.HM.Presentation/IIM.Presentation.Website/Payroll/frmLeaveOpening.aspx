<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmLeaveOpening.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmLeaveOpening" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Leave Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Leave Balance</li>";
            var breadCrumbs = moduleName + formName;

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            if (IsCanSave) {
                //$('#btnSaveDiv').show();
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }
           
            //$("#myTabs").tabs();

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

        });

        function LoadLeaveTypeInfoByEmp() {
            var empId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            PageMethods.LoadLeaveTypeInfo(empId, OnLoadSucceeded, OnLoadFailed);
            return false;
        }

        function OnLoadSucceeded(result) {
            $("#LeaveTypeContainer").html(result);
            $('#btnSaveDiv').show();
        }
        function OnLoadFailed() {
            toastr.error("Please Contact With Admin");
        }

        function CheckLeaveValidation() {

            if ($("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val() == "0") {
                toastr.info("Please Select Employee First");
                return false;
            }

            var saveObj = new Array();
            var editObj = new Array();
            var deleteObj = new Array();

            var empId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            var laevaTypeId = 0, openingleave = 0, carryForwardedleave = 0, encashableleave = 0, leaveClosingId = 0, dbopeningleave = "0", dbcarryForwardedleave = "0", dbencashableleave = "0";

            $("#LeaveOpening tbody tr").each(function () {

                leaveClosingId = $.trim($(this).find("td:eq(0)").text());
                laevaTypeId = $.trim($(this).find("td:eq(1)").text());
                dbopeningleave = $.trim($(this).find("td:eq(2)").text());
                dbcarryForwardedleave = $.trim($(this).find("td:eq(3)").text());
                dbencashableleave = $.trim($(this).find("td:eq(4)").text());

                openingleave = $.trim($(this).find("td:eq(6)").find("input").val());
                carryForwardedleave = $.trim($(this).find("td:eq(7)").find("input").val());
                encashableleave = $.trim($(this).find("td:eq(8)").find("input").val());

                if (openingleave == "") {
                    openingleave = "0";
                }
                if (carryForwardedleave == "") {
                    carryForwardedleave = "0";
                }
                if (encashableleave == "") {
                    encashableleave = "0";
                }

                if (leaveClosingId == "0" && (openingleave != "0" || carryForwardedleave != "0" || encashableleave != "0")) {
                    saveObj.push({
                        LeaveClosingId: "0",
                        EmpId: empId,
                        LeaveTypeId: laevaTypeId,
                        OpeningLeave: openingleave,
                        CarryForwardedLeave: carryForwardedleave,
                        EncashableLeave: encashableleave
                    });
                }
                else if (leaveClosingId != "0" && (openingleave != "0" || carryForwardedleave != "0" || encashableleave != "0") && (dbopeningleave != openingleave || dbcarryForwardedleave != carryForwardedleave || dbencashableleave != encashableleave)) {
                    editObj.push({
                        LeaveClosingId: leaveClosingId,
                        EmpId: empId,
                        LeaveTypeId: laevaTypeId,
                        OpeningLeave: openingleave,
                        CarryForwardedLeave: carryForwardedleave,
                        EncashableLeave: encashableleave
                    });
                }
                else if (leaveClosingId != "0" && openingleave == "0" && carryForwardedleave == "0" && encashableleave == "0") {
                    deleteObj.push({
                        LeaveClosingId: leaveClosingId,
                        EmpId: empId,
                        LeaveTypeId: laevaTypeId,
                        OpeningLeave: openingleave,
                        CarryForwardedLeave: carryForwardedleave,
                        EncashableLeave: encashableleave
                    });
                }
            });
            $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveObj));
            $("#<%=hfEditObj.ClientID %>").val(JSON.stringify(editObj));
            $("#<%=hfDeleteObj.ClientID %>").val(JSON.stringify(deleteObj));
            $("#<%=hfLeaveClosingId.ClientID %>").val(leaveClosingId);
            return true;
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvOpeningLeave tbody tr").length;

            var empId = $("#ContentPlaceHolder1_SearchemployeeSearch_hfEmployeeId").val();
            
            PageMethods.SearchOpeningLeave(empId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }

        function OnLoadObjectSucceeded(result) {
           
            $("#gvOpeningLeave tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvOpeningLeave tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvOpeningLeave tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:30%; cursor:pointer;\">" + gridObject.EmployeeName + "</td>";
                tr += "<td align='left' style=\"width:30%; cursor:pointer;\">" + gridObject.Deparment + "</td>";
                tr += "<td align='left' style=\"width:30%; cursor:pointer;\">" + gridObject.Designation + "</td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.EmpId + "', '" + gridObject.EmpCode + "', '" + gridObject.EmployeeName + "')\" alt='Edit Information' border='0' /></td>";

                tr += "</tr>"

                $("#gvOpeningLeave tbody ").append(tr);
                tr = "";
                
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            

            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.error("Please Contact With Admin");
        }

        function PerformEditAction(empId, empCode, empName) {
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val(empId);
            $("#ContentPlaceHolder1_employeeSearch_txtSearchEmployee").val(empCode);
            $("#ContentPlaceHolder1_employeeSearch_txtEmployeeName").val(empName);
            LoadLeaveTypeInfo(empId);
        }

        function LoadLeaveTypeInfo(empId) {
            PageMethods.LoadLeaveTypeInfo(empId, OnSuccessLoadLeave, OnFailLoadLeave);
        }

        function OnSuccessLoadLeave(result) {

            if (IsCanEdit) {
                $('#btnSaveDiv').show();
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }
            $("#LeaveTypeContainer").html(result);
            $("#<%=btnSave.ClientID %>").val("Update");
            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });
        }
        function OnFailLoadLeave(result) {
            toastr.error("Please Contact With Admin");
        }

        function WorkAfterSearchEmployee() { }

    </script>
    <asp:HiddenField ID="hfSaveObj" runat="server" />
    <asp:HiddenField ID="hfEditObj" runat="server" />
    <asp:HiddenField ID="hfDeleteObj" runat="server" />
    <asp:HiddenField ID="hfLeaveClosingId" runat="server" />

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="myTabs">
        <%--<ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Leave Balance Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Leave Balance</a></li>
        </ul>--%>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Leave Balance Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeSearch ID="employeeSearch" runat="server" />
                        <div class="row" style="padding-top:5px; padding-bottom:5px;">
                            <div class="col-md-12">
                                <asp:Button ID="btnLeaveOpeningSearch" runat="server" Text="Search" TabIndex="2"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript:return LoadLeaveTypeInfoByEmp();" />
                            </div>
                        </div>
                        <div id="LeaveTypeContainer">
                        </div>
                        <div class="row" style="padding-top:10px;">
                            <div id="btnSaveDiv" class="col-md-12" style="display:none;">
                                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" OnClientClick="javascript:return CheckLeaveValidation()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2" style="display:none;">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Search Leave Balance</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeSearch ID="SearchemployeeSearch" runat="server" />
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary">
                                    Search</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <table id='gvOpeningLeave' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="width: 30%;" />
                            <col style="width: 30%;" />
                            <col style="width: 30%;" />
                            <col style="width: 10%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>
                                    Employee Name
                                </td>
                                <td>
                                    Department
                                </td>
                                <td>
                                    Designation
                                </td>
                                <td style="text-align: right;">
                                    Actions
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
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
</asp:Content>
