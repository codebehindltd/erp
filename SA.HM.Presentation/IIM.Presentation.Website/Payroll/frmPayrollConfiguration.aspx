<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmPayrollConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmPayrollConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var deleteObj = [];
        var EditedLeaveDeductionPolicy = new Array();
        var deletedDeductionPolicyDetailList = new Array();
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Administrative & Security</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var chkIsTaxPaidbyCmp = '<%=chkIsTaxPaidbyCmp.ClientID%>'
            if ($(('#' + chkIsTaxPaidbyCmp)).attr('checked')) {
                //$('#PaymentInformation').show();
                $('#PaymentInformation').hide();
            }
            else {
                $('#PaymentInformation').hide();
            }

            $("#ContentPlaceHolder1_ddlEmployeeContributionHeadId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCompanyContributionHeadId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#FestivalBonusContainer").hide();
            $("#ContentPlaceHolder1_ddlBonusType").change(function () {

                if ($(this).val() == "PeriodicBonus") {
                    $("#PerioDicalBonusContainer").show();
                    $("#FestivalBonusContainer").hide();
                    $("#AddMultipleBonus").hide();
                }
                else if ($(this).val() == "FestivalBonus") {
                    $("#PerioDicalBonusContainer").hide();
                    $("#FestivalBonusContainer").show();
                    $("#AddMultipleBonus").show();
                }
            });

            $("#btnAddMultipleBonus").click(function () {
                //                var add = false;
                //                var reservationId = $("#ContentPlaceHolder1_txtReservationId").val();
                //                if (reservationId == "") {
                //                    reservationId = 0;
                //                }

                AddBonusInfo(0);

            });
            $("[id=ChkAll]").on("change", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#LeaveTable tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#LeaveTable tbody tr").find("td:eq(0) ").find("input").prop("checked", false);
                }
                return false;
            });
            var txtBonusDate = '<%=txtBonusDate.ClientID%>'
            $('#' + txtBonusDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            CommonHelper.ApplyIntigerValidation();
            LoadLeaveInformationTable();
            SearchLeaveDeductionPolicy();
        });

        $(function () {
            $("#myTabs").tabs();
        });

        function ToggleFieldVisibleForCompanyPay(ctrl) {
            if ($(ctrl).attr('checked')) {
                //$('#PaymentInformation').show();
                $('#PaymentInformation').hide();
            }
            else {
                $("#<%=ddlCmpContType.ClientID %>").val('')
                $("#<%=txtCmpContAmount.ClientID %>").val('')
                $('#PaymentInformation').hide();
            }
        }

        function PerformAttendanceDeviceClearAction() {
            $("#<%=ddlAttendanceDevice.ClientID %>").val('0');
            $("#<%=ddlDeviceType.ClientID %>").val('0');
        }

        <%--function ToggleFieldVisibleForTaxDeduct(ctrl) {
            if ($(ctrl).attr('checked')) {
                $('#TaxDdctInfo').show();                
            }
            else {
                $("#<%=ddlEmpContType.ClientID %>").val('')
                //$('#TaxDdctInfo').hide();
                $('#TaxDdctInfo').show();
            }
        }--%>

        function AddBonusInfo(bonusId) {
            var bonusType = $("#<%=ddlBonusType.ClientID %>").val();
            var bonusAmount = $("#<%=txtBonusAmount.ClientID %>").val();
            var amountType = $("#<%=ddlAmountType.ClientID %>").val();
            var dependsOn = $("#<%=ddlDependsOn.ClientID %>").val();
            var bonusDate = $("#<%=txtBonusDate.ClientID %>").val();

            if (amountType == "Percent(%)") {
                if (bonusAmount.length > 3) {
                    toastr.warning('Percentage cannt be more than 100%.');
                    return false;
                }
            }
            if (bonusAmount == "") {
                toastr.warning('Please Provide Bonus Amount.');
                return false;
            }
            else if (bonusDate == "") {
                toastr.warning('Please Provide Bonus Date.');
                return false;
            }
            else if (amountType == "--- Please Select ---") {
                toastr.warning('Please Select Amount Type.');
                return false;
            }

            if ($("#ltlTableWiseBonusAdd > table").length > 0) {
                AddNewRow(bonusId, bonusType, bonusAmount, amountType, dependsOn, bonusDate);
                return false;
            }

            var table = "", deleteLink = "";

            deleteLink = "<a href=\"#\" onclick= 'DeleteBonus(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            table += "<table cellspacing='0' cellpadding='4' id='BonusInformation' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            table += "<th style='display:none'>Bonus Id</th><th style='display:none'>Bonus Type</th><th style='display:none'>Dependa On</th><th align='left' scope='col' style='width: 30%;'>Bonus Amount</th><th align='left' scope='col' style='width: 30%;'>Amount Type</th><th align='left' scope='col' style='width: 30%;'>Bonus Date</th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";

            table += "<tbody>";
            table += "<tr style=\"background-color:#E3EAEB;\">";

            table += "<td align='left' style=\"display:none;\">" + bonusId + "</td>";
            table += "<td align='left' style=\"display:none;\">" + bonusType + "</td>";
            table += "<td align='left' style=\"display:none;\">" + dependsOn + "</td>";
            table += "<td align='left' style=\"width:30%; text-align:Left;\">" + bonusAmount + "</td>";
            table += "<td align='left' style=\"width:30%; text-align:Left;\">" + amountType + "</td>";
            table += "<td align='left' style=\"width:30%; text-align:Left;\">" + bonusDate + "</td>";
            table += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            table += "</tr>";
            table += "</tbody>";
            table += "</table>";

            $("#ltlTableWiseBonusAdd").html(table);
        }

        function AddNewRow(bonusId, bonusType, bonusAmount, amountType, dependsOn, bonusDate) {
            var tr = "", totalRow = 0, deleteLink = "";
            totalRow = $("#BonusInformation tbody tr").length;

            deleteLink = "<a href=\"#\" onclick= 'DeleteBonus(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }
            tr += "<td align='left' style=\"display:none;\">" + bonusId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + bonusType + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + dependsOn + "</td>";
            tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + bonusAmount + "</td>";
            tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + amountType + "</td>";
            tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + bonusDate + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "</tr>";

            $("#BonusInformation tbody").append(tr);
        }

        function ValidationNPreprocess() {

            var saveObj = [];
            var bonusId = 0, bonusType = "", bonusAmount = 0, amountType = "", dependsOn = 0, bonusDate = "";

            var rowLength = $("#ltlTableWiseBonusAdd > table tbody tr").length;

            $("#ltlTableWiseBonusAdd > table tbody tr").each(function () {
                bonusId = parseInt($.trim($(this).find("td:eq(0)").text(), 10));
                bonusType = $(this).find("td:eq(1)").text();
                dependsOn = parseInt($.trim($(this).find("td:eq(2)").text(), 10));
                bonusAmount = parseFloat($.trim($(this).find("td:eq(3)").text(), 10));
                amountType = $(this).find("td:eq(4)").text();
                bonusDate = $(this).find("td:eq(5)").text();

                if (bonusId == 0) {
                    saveObj.push({
                        BonusSettingId: bonusId,
                        BonusType: bonusType,
                        DependsOnHead: dependsOn,
                        BonusAmount: bonusAmount,
                        AmountType: amountType,
                        BonusDate: bonusDate
                    });
                }
            });

            $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveObj));
            $("#<%=hfDeleteObj.ClientID %>").val(JSON.stringify(deleteObj));
        }

        function DeleteBonus(anchor) {
            ff = anchor;
            var tr = $(anchor).parent().parent();

            var bonusId = $.trim($(tr).find("td:eq(0)").text());
            //var reservationId = $.trim($(tr).find("td:eq(1)").text());

            if (parseInt(bonusId, 10) != 0) {
                deleteObj.push({
                    BonusSettingId: bonusId
                });
            }

            $(tr).remove();
            return false;
        }
        function LoadLeaveInformationTable() {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/frmPayrollConfiguration.aspx/LoadLeaveTable',
                dataType: "json",
                success: function (data) {

                    $("#LeaveTable tbody").empty();

                    i = 0;

                    $.each(data.d, function (count, gridObject) {

                        var tr = "";

                        if (i % 2 == 0) {
                            tr = "<tr style='background-color:#FFFFFF;'>";
                        }
                        else {
                            tr = "<tr style='background-color:#E3EAEB;'>";
                        }

                        tr += "<td style='width:5%; text-align:center'><input type='checkbox' </td>";
                        tr += "<td style='width:55%;'>" + gridObject.TypeName + "</td>";
                        tr += "<td style='width:40%;'><input class='form-control quantity'  style='width:100%;' align='left' value='" + parseInt(i + 1) + "' type='text' ></td>";


                        tr += "<td style='display:none'>" + gridObject.LeaveTypeId + "</td>";
                        tr += "<td style='display:none'> 0 </td>";

                        tr += "</tr>";

                        $("#LeaveTable tbody").append(tr);

                        tr = "";
                        i++;
                    });

                    CommonHelper.ApplyIntigerValidation();
                },
                error: function (result) {
                    //PerformClearAction();
                }
            });
            return false;
        }
        function SaveLeaveDeduction() {
            var id = $("#ContentPlaceHolder1_hfLeaveDeductionId").val();
            var noOfLate = $("#ContentPlaceHolder1_txtBoxNoOfLate").val();
            var noOfLeave = $("#ContentPlaceHolder1_txtBoxNoOfLeave").val();
            if (noOfLate == "") {
                toastr.warning("Enter No of Late Days");
                $("#ContentPlaceHolder1_txtBoxNoOfLate").focus();
                return false;
            }
            if (noOfLeave == "") {
                toastr.warning("Enter No of Leave will be diducted");
                $("#ContentPlaceHolder1_txtBoxNoOfLeave").focus();
                return false;
            }
            var PayrollLeaveDeductionPolicyMasterBO = {
                Id: id,
                NoOfLate: noOfLate,
                NoOfLeave: noOfLeave
            }
            var DeductionPolicyDetailList = new Array();
            var _return = "";
            $("#LeaveTable tbody tr").each(function () {
                if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                    var id = parseInt($.trim($(this).find("td:eq(4)").text()));
                    var sequence = parseInt($.trim($(this).find("td:eq(2)").find("input").val()));
                    if ($.trim($(this).find("td:eq(2)").find("input").val()) == "") {
                        _return = $.trim($(this).find("td:eq(1)").text());
                        return false;
                    }
                    var leaveId = parseInt($.trim($(this).find("td:eq(3)").text()));
                    DeductionPolicyDetailList.push({
                        Id: id,
                        LeaveId: leaveId,
                        Sequence: sequence
                    });
                }
            });
            if (_return != "") {
                toastr.warning("Enter sequence of " + _return);
                _return = "";
                return false;
            }
            $.each(EditedLeaveDeductionPolicy, function (count1, obj1) {
                var count = 0, count_3 = 0;
                $.each(DeductionPolicyDetailList, function (count2, obj2) {
                    count_3++;
                    if (obj1.LeaveId != obj2.LeaveId) {
                        count++;
                    }
                })
                if (count == count_3) {
                    deletedDeductionPolicyDetailList.push(obj1);
                }

            });
            if (DeductionPolicyDetailList.length < 1) {
                toastr.warning("Select atleast one leave Type");
                //$("#ContentPlaceHolder1_txtBoxNoOfLeave").focus();
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/frmPayrollConfiguration.aspx/SaveOrUpdateLeaveDeductionConfig',
                data: JSON.stringify({ PayrollLeaveDeductionPolicyMasterBO: PayrollLeaveDeductionPolicyMasterBO, DeductionPolicyDetailList: DeductionPolicyDetailList, deletedDeductionPolicyDetailList: deletedDeductionPolicyDetailList }),
                dataType: "json",
                success: function (data) {
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    ClearLeaveDeduction();
                    SearchLeaveDeductionPolicy();
                },
                error: function (result) {
                    PerformClearAction();
                    //CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }
        function SearchLeaveDeductionPolicy() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/frmPayrollConfiguration.aspx/SearchLeaveDeductionPolicy',
                dataType: "json",
                success: function (data) {
                    $("#LeaveDeductionSearchTable tbody").empty();
                    var i = 0;

                    $.each(data.d, function (count, gridObject) {

                        var tr = "";

                        if (i % 2 == 0) {
                            tr = "<tr style='background-color:#FFFFFF;'>";
                        }
                        else {
                            tr = "<tr style='background-color:#E3EAEB;'>";
                        }

                        tr += "<td style='width:75%;'>" + "Monthly <b>" + gridObject.NoOfLate + "</b> days Late <b>" + gridObject.NoOfLeave + "</b> Leave deduct." + "</td>";

                        tr += "<td style='width:25%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                        tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteLeaveDeductionPolicy(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";

                        tr += "<td style='display:none'>" + gridObject.Id + "</td>";


                        tr += "</tr>";

                        $("#LeaveDeductionSearchTable tbody").append(tr);

                        tr = "";
                        i++;
                    });
                    return false;
                },
                error: function (result) {

                }
            });
            return false;
        }
        function DeleteLeaveDeductionPolicy(id) {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/frmPayrollConfiguration.aspx/DeleteLeaveDeductionPolicy',
                data: "{'Id':'" + id + "'}",
                dataType: "json",
                success: function (data) {
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    SearchLeaveDeductionPolicy();
                },
                error: function (result) {

                }
            });
            return false;
        }
        function FillFormEdit(id) {
            if (!confirm("Do you want to edit ?")) {
                return false;
            }
            CommonHelper.SpinnerOpen();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/frmPayrollConfiguration.aspx/FillForm',

                data: "{'Id':'" + id + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    $("#ContentPlaceHolder1_hfLeaveDeductionId").val(data.d.Id);
                    $("#ContentPlaceHolder1_btnAddLeaveDeduction").val("Update");
                    $("#ContentPlaceHolder1_txtBoxNoOfLate").val(data.d.NoOfLate);
                    $("#ContentPlaceHolder1_txtBoxNoOfLeave").val(data.d.NoOfLeave);
                    LoadLeaveDeductionPolicyForEdit(data.d.DetailList)

                    CommonHelper.SpinnerClose();
                },
                error: function (result) {

                }
            });
            return false;
        }
        function LoadLeaveDeductionPolicyForEdit(result) {
            EditedLeaveDeductionPolicy = new Array();
            $("#LeaveTable tbody tr").find("td:eq(0) ").find("input").prop("checked", false);
            //$.each(result, function (count, obj) {
            for (var i = 0; i < result.length; i++) {
                EditedLeaveDeductionPolicy.push(result[i]);
                $("#LeaveTable tbody tr").each(function () {
                    if (parseFloat($(this).find("td:eq(3)").text()) == result[i].LeaveId) {
                        $.trim($(this).find("td:eq(0)").find("input").prop("checked", true));
                        $.trim($(this).find("td:eq(1)").find("input").val(result[i].LeaveName));
                        $.trim($(this).find("td:eq(2)").find("input").val(result[i].Sequence));
                        //$.trim($(this).find("td:eq(3)").find("input").val(result[i].Description));
                        $.trim($(this).find("td:eq(4)").text(result[i].Id));

                    }
                });
            }
            //});
        }
        function ClearLeaveDeduction() {
            $("#ContentPlaceHolder1_btnAddLeaveDeduction").val("Save");
            $("#ContentPlaceHolder1_hfLeaveDeductionId").val("0");
            $("#ContentPlaceHolder1_txtBoxNoOfLate").val("");
            $("#ContentPlaceHolder1_txtBoxNoOfLeave").val("");
            LoadLeaveInformationTable();
            $("#ChkAll").prop("checked", false);
            return false;
        }

    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Settings</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Tax</a></li>
            <%--  <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Tax Deduction</a></li>--%>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Provident Fund</a></li>
            <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-4">Loan</a></li>
            <li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-5">Gratuity</a></li>
            <li id="F" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-6">Bonus</a></li>
            <li id="G" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-7">Attendance Device</a></li>
            <li id="H" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-8">Leave</a></li>
        </ul>
        <div id="tab-1">
            <div class="row">
                <div class="col-md-12">
                    <div id="ConfigurationSetting" class="panel panel-default">
                        <div class="panel-heading">
                            Configuration
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtStartDateId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblScheduleDate" runat="server" class="control-label" Text="Month Start Date"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlStartDate" runat="server" TabIndex="1" CssClass="form-control">
                                            <asp:ListItem Value="1">1</asp:ListItem>
                                            <asp:ListItem Value="2">2</asp:ListItem>
                                            <asp:ListItem Value="3">3</asp:ListItem>
                                            <asp:ListItem Value="4">4</asp:ListItem>
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                            <asp:ListItem Value="6">6</asp:ListItem>
                                            <asp:ListItem Value="7">7</asp:ListItem>
                                            <asp:ListItem Value="8">8</asp:ListItem>
                                            <asp:ListItem Value="9">9</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="11">11</asp:ListItem>
                                            <asp:ListItem Value="12">12</asp:ListItem>
                                            <asp:ListItem Value="13">13</asp:ListItem>
                                            <asp:ListItem Value="14">14</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="16">16</asp:ListItem>
                                            <asp:ListItem Value="17">17</asp:ListItem>
                                            <asp:ListItem Value="18">18</asp:ListItem>
                                            <asp:ListItem Value="19">19</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="21">21</asp:ListItem>
                                            <asp:ListItem Value="22">22</asp:ListItem>
                                            <asp:ListItem Value="23">23</asp:ListItem>
                                            <asp:ListItem Value="24">24</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="26">26</asp:ListItem>
                                            <asp:ListItem Value="27">27</asp:ListItem>
                                            <asp:ListItem Value="28">28</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtBasicSetupId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblBasicSalaryHeadId" runat="server" class="control-label" Text="Basic Salary Head"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlBasicSalaryHeadId" runat="server" CssClass="form-control"
                                            TabIndex="3">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group" id="GrossSalarySetup" runat="server">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtGrossSetupId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="Label6" runat="server" class="control-label" Text="Gross Salary Head"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlGrossSalaryHeadId" runat="server" CssClass="form-control"
                                            TabIndex="3">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtOverTimeSetupId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblOverTimeAmount" runat="server" class="control-label" Text="Overtime Head"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlSalaryHeadId" runat="server" CssClass="form-control" TabIndex="5">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtMinimumOvertimeHourId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="Label7" runat="server" class="control-label" Text="Minimum Overtime Hour"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtMinimumOvertimeHour" CssClass="form-control" runat="server">
                                        </asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtSalaryProcessId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblProcessSystem" runat="server" class="control-label" Text="Process System"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlSalaryProcessSystem" runat="server" CssClass="form-control"
                                            TabIndex="8">
                                            <asp:ListItem Value="Group">Group</asp:ListItem>
                                            <asp:ListItem Value="Individual">Individual</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtMonthlyWorkingDayForAbsentreeId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblMonthlyWorkingDayForAbsentree" runat="server" class="control-label"
                                            Text="Monthly Working Day For Absentee"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtMonthlyWorkingDayForAbsentree" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtMonthlyWorkingDayId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Monthly Working Day"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtMonthlyWorkingDay"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtWorkingHourId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblWorkingHour" runat="server" class="control-label" Text="Working Hour"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtWorkingHour" CssClass="form-control" runat="server"> </asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtInsteadLeaveHeadId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblInsteadLeave" runat="server" class="control-label" Text="Substitute Leave"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlInsteadLeaveHeadId" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtInsteadLeaveForOneHolidayId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblInsteadLeaveForOneHoliday" runat="server" class="control-label"
                                            Text="Leave For One Holiday"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtInsteadLeaveForOneHoliday" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtDeductionAmountForEachEmployeeFromSalaryId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblDeductionAmountForEachEmployeeFromSalary" runat="server" class="control-label"
                                            Text="Deduction Amount For Each Employee From Salary"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtDeductionAmountForEachEmployeeFromSalary" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtTotalWorkingDayForOvertimeId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblTotalWorkingDayForOvertime" runat="server" class="control-label"
                                            Text="Total Working Day For Overtime"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtTotalWorkingDayForOvertime" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtEmpLateBufferingTimeId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblEmpLateBufferingtimee" runat="server" class="control-label"
                                            Text="Late Buffering Time (Min) "></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtEmpLateBufferingTime" CssClass="form-control" runat="server" PlaceHolder="Minutes"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtPayrollAfterServiceBenefitId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblPayrollAfterServiceBenefitId" runat="server" class="control-label" Text="After Service Benefit"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPayrollAfterServiceBenefitId" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtPayrollAdvanceHeadId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblPayrollAdvanceHeadId" runat="server" class="control-label" Text="Advance Head"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPayrollAdvanceHeadId" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtPayrollBonusMonthlyEffectedPeriod" runat="server"></asp:HiddenField>
                                        <asp:Label ID="ldlPayrollBonusMonthlyEffectedPeriod" runat="server" class="control-label" Text="Payroll Bonus Monthly Effected Period"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPayrollBonusMonthlyEffectedPeriod" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtLastPayHeadIdLeaveBalance" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblLastPayHeadIdLeaveBalance" runat="server" class="control-label" Text="Last Pay Head Leave Balance"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlLastPayHeadIdLeaveBalance" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtWithoutSalaryLeaveHeadId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblWithoutSalaryLeaveHeadId" runat="server" class="control-label" Text="Without Salary Leave Head"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlWithoutSalaryLeaveHeadId" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtServiceChargeAllowanceHeadId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblServiceChargeAllowanceHeadId" runat="server" class="control-label" Text="Service Charge Allowance Head"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlServiceChargeAllowanceHeadId" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtOvertimeAllowanceHeadId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblOvertimeAllowanceHeadId" runat="server" class="control-label" Text="Overtime Allowance Head"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlOvertimeAllowanceHeadId" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtMedicalAllowance" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblMedicalAllowance" runat="server" class="control-label" Text="Medical Allowance"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlMedicalAllowance" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtPayrollSalaryExecutionProcessType" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblPayrollSalaryExecutionProcessType" runat="server" class="control-label" Text="Payroll Salary Execution Process Type"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPayrollSalaryExecutionProcessType" CssClass="form-control" runat="server">
                                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                            <asp:ListItem Value="1">Regular</asp:ListItem>
                                            <asp:ListItem Value="2">RedCross</asp:ListItem>
                                            <asp:ListItem Value="3">IPTech</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:HiddenField ID="txtPayrollReportingTo" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblPayrollReportingTo" runat="server" class="control-label" Text="Reporting To"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPayrollReportingTo" CssClass="form-control" runat="server">
                                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                            <asp:ListItem Value="1">Designation Wise</asp:ListItem>
                                            <asp:ListItem Value="2">Employee Wise</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsPayrollIntegrateWithInventory" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsPayrollIntegrateWithInventory" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Payroll Integrate With Inventory?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsPayrollIntegrateWithAccounts" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsPayrollIntegrateWithAccounts" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Payroll Integrate With Accounts?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsPayrollWorkStationHide" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsPayrollWorkStationHide" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Payroll Work Station Hide?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsPayrollDonorNameAndActivityCodeHide" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsPayrollDonorNameAndActivityCodeHide" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Payroll Donor Name And Activity Code Hide?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsPayrollDependentHide" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsPayrollDependentHide" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Payroll Dependent Hide ?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsPayrollBeneficiaryHide" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsPayrollBeneficiaryHide" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Payroll Beneficiary Hide?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsPayrollReferenceHide" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsPayrollReferenceHide" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Payroll Reference Hide?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsPayrollBenefitsHide" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsPayrollBenefitsHide" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Payroll Benefits Hide?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsSalaryProcessBasedOnAttendance" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsSalaryProcessBasedOnAttendance" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Salary Process Based On Attendance?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsServiceChargeShowsInSalarySheetAndPaySlip" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsServiceChargeShowsInSalarySheetAndPaySlip" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Service Charge Shows In Salary Sheet And Pay Slip?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsEmployeeCodeAutoGenerate" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsEmployeeCodeAutoGenerate" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Employee Code Auto Generate?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsEmployeeBasicSetUpOnly" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsEmployeeBasicSetUpOnly" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Employee Basic Set Up Only?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsPayrollProvidentFundDeductHide" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsPayrollProvidentFundDeductHide" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Provident Fund Deduct Hide?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsPayrollCostCenterDivHide" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsPayrollCostCenterDivHide" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Cost Center Hide?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Cash Requisition Adjustment With Different Company Or Projects?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsEmployeeCanEditDetailsInfo" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsEmployeeCanEditDetailsInfo" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Can Employee Edit Details Information?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsAutoLoanCollectionProcessEnable" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsAutoLoanCollectionProcessEnable" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Auto Loan Collection Process Enable?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsEmpSearchFromDashboardEnable" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsEmpSearchFromDashboardEnable" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Employee Search From Dashboard Enable?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsEmpSearchDetailsFromDashboardEnable" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsEmpSearchDetailsFromDashboardEnable" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Employee Search Details From Dashboard Enable?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsCashRequisitionEnable" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsCashRequisitionEnable" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Cash Requisition Enable?
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="hfIsBillVoucherEnable" runat="server"></asp:HiddenField>
                                        <asp:CheckBox ID="IsBillVoucherEnable" runat="Server" Text="" Font-Bold="true"
                                            CssClass="customChkBox" TextAlign="right" />
                                        &nbsp;&nbsp;Is Bill Voucher Enable?
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="btnUpdateSettings" runat="server" Text="Update" CssClass="btn btn-primary"
                                            TabIndex="2" OnClick="btnUpdateSettings_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="TaxEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Tax Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" style="display: none;">
                            <div class="col-md-3">
                                <asp:HiddenField ID="txtTaxSettingId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblTaxBandM" runat="server" class="control-label required-field" Text="Tax Band (Male)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtTaxBandM" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="display: none;">
                            <div class="col-md-3">
                                <asp:Label ID="lblTaxBandF" runat="server" class="control-label required-field" Text="Tax Band (Female)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtTaxBandF" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div id="PaymentInformation" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-3">
                                    <asp:Label ID="lblCmpContType" runat="server" class="control-label required-field"
                                        Text="Company Contribution Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlCmpContType" runat="server" CssClass="form-control" TabIndex="3">
                                        <asp:ListItem Value="Basic">Basic Salary</asp:ListItem>
                                        <asp:ListItem Value="Gross">Gross Salary</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="col-md-3">
                                    <asp:Label ID="lblCmpContAmount" runat="server" class="control-label" Text="Contribution Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCmpContAmount" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="TaxDdctInfo">
                            <div class="form-group">
                                <div class="col-md-3">
                                    <asp:Label ID="lblEmpContType" runat="server" class="control-label required-field"
                                        Text="Employee Contribution Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlEmpContType" runat="server" CssClass="form-control" TabIndex="3">
                                        <asp:ListItem Value="Basic">Basic Salary</asp:ListItem>
                                        <asp:ListItem Value="Gross">Gross Salary</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:HiddenField ID="ddlPayrollTaxDeductionId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblPayrollTaxDeductionId" runat="server" class="control-label required-field" Text="Tax Head"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPayrollTaxDeduction" runat="server" CssClass="form-control"
                                    TabIndex="4">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="IsTaxDdctFrmSlry" class="form-group">
                            <div class="col-md-8">
                                <asp:CheckBox ID="chkIsTaxDdctFrmSlry" runat="server" Text="" CssClass="customChkBox"
                                    TabIndex="8" />
                                <asp:Label ID="lblIsTaxDdctFrmSlry" runat="server" class="control-label" Text="Deduction From Salary"></asp:Label>
                            </div>
                        </div>
                        <div id="IsTaxPaidbyCmp" class="form-group" style="display: none;">
                            <div class="col-md-8">
                                <asp:CheckBox ID="chkIsTaxPaidbyCmp" runat="server" Text="" CssClass="customChkBox"
                                    onclick="javascript: return ToggleFieldVisibleForCompanyPay(this);" TabIndex="8" />
                                <asp:Label ID="lblIsTaxPaidbyCmp" runat="server" class="control-label" Text="Company Pay"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group" style="display: none;">
                            <div class="col-md-3">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpTaxSave" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnEmpTaxSave_Click" />
                                <asp:Button ID="btnEmpTaxClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return PerformClearAction();" OnClick="btnEmpTaxClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3">
            <div id="PFEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Provident Fund Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:Label ID="lblEmployeeContributionHeadId" runat="server" class="control-label" Text="Employee Contribution Head"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlEmployeeContributionHeadId" runat="server" CssClass="form-control" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:HiddenField ID="txtPFSettingId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblEmpCont" runat="server" class="control-label required-field" Text="Employee Contribution (%)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtEmpCont" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:Label ID="lblCompanyContributionHeadId" runat="server" class="control-label" Text="Company Contribution Head"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCompanyContributionHeadId" runat="server" CssClass="form-control" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:Label ID="lblCmpCont" runat="server" class="control-label required-field" Text="Company Contribution (%)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtCmpCont" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:Label ID="Label8" runat="server" class="control-label required-field"
                                    Text="Company Contribution On"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCompanyContributionOn" runat="server" CssClass="form-control"
                                    TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:Label ID="lblEmpMaxCont" runat="server" class="control-label required-field"
                                    Text="Employee Can Contribute Max Of Basic Salary (%)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtEmpMaxCont" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:Label ID="lblIntDisRt" runat="server" class="control-label required-field" Text="Interest Distribution Rate (%)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtIntDisRt" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:Label ID="lblCmpContElegYear" runat="server" class="control-label required-field"
                                    Text="Company Contribution Elegibility (Year)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtCmpContElegYear" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpPFSave" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnEmpPFSave_Click" />
                                <asp:Button ID="btnEmpPFClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return PerformClearAction();" OnClick="btnEmpPFClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-4">
            <div id="LoanEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Loan Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:HiddenField ID="txtLoanSettingId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblCmpLnIntRate" runat="server" class="control-label required-field"
                                    Text="Company Loan Interest Rate (%)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtCmpLnIntRate" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:Label ID="lblPFlnIntRate" runat="server" class="control-label required-field"
                                    Text="Provident Fund Loan Interest Rate (%)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtPFlnIntRate" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:Label ID="lblMaxAmtWdrwfmPF" runat="server" class="control-label required-field"
                                    Text="Max Amount Can Withdraw From Provident Fund (%)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtMaxAmtWdrwfmPF" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:Label ID="lblMinPFavlfrLn" runat="server" class="control-label required-field"
                                    Text="Min Provident Fund Must Available To Allow Loan"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtMinPFavlfrLn" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:Label ID="lblMinJobLnthfrCmpLn" runat="server" class="control-label required-field"
                                    Text="Min Job Length To Allow Company Loan (Month)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtMinJobLnthfrCmpLn" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:Label ID="lblDrtnfrNxtLn" runat="server" class="control-label required-field"
                                    Text="Duration For Next Loan After Completion Taken Loan (Month)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtDrtnfrNxtLn" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-5">
                                <asp:HiddenField ID="txtPFLoanHeadId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblPFLoanHeadId" runat="server" class="control-label" Text="PF Loan Head"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPFLoanHeadId" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpLoanSave" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnEmpLoanSave_Click" />
                                <asp:Button ID="btnEmpLoanClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return PerformClearAction();" OnClick="btnEmpLoanClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-5">
            <div id="GratutityEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Gratuity Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtGratuityId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblNoofJobYearfrGrty" runat="server" class="control-label required-field"
                                    Text="No Of Job Year"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtNoofJobYearfrGrty" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:CheckBox ID="chkIsGrtybsdonBasic" runat="server" Text="" CssClass="customChkBox"
                                    TabIndex="8" />
                                <asp:Label ID="lblIsGrtybsdonBasic" runat="server" class="control-label" Text="Gratuity On Basic"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblGrtyPercntge" runat="server" class="control-label" Text="Gratuity (%)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtGrtyPercntge" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblGrtyNoAdded" runat="server" class="control-label required-field"
                                    Text="No. Of Gratuity Added"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtGrtyNoAdded" CssClass="form-control" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpGratutitySave" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnEmpGratutitySave_Click" />
                                <asp:Button ID="btnEmpGratutityClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return PerformClearAction();" OnClick="btnEmpGratutityClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-6">
            <asp:HiddenField ID="hfSaveObj" runat="server" />
            <asp:HiddenField ID="hfDeleteObj" runat="server" />
            <div id="BonusHeadPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Bonus Setup Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="hfBonusHeadId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblBonusHeadId" runat="server" class="control-label" Text="Bonus"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBonusHeadId" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnBonusHead" runat="server" Text="Save" TabIndex="13" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnBonusHead_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="BonusEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Bonus Formula
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="hfBonusId" runat="server"></asp:HiddenField>
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Bonus Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBonusType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                                    <asp:ListItem Text="Festival" Value="FestivalBonus"></asp:ListItem>
                                    <asp:ListItem Text="Periodical" Value="PeriodicBonus"></asp:ListItem>
                                    <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="" class="panel panel-default">
                            <div class="panel-heading">
                                Festival Bonus Setup
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Bonus Amount"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtBonusAmount" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="lblAmountType" runat="server" class="control-label required-field"
                                                Text="Amount Type"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlAmountType" runat="server" CssClass="form-control" TabIndex="9">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblDependsOn" runat="server" class="control-label required-field"
                                                Text="Depends On"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlDependsOn" runat="server" CssClass="form-control" TabIndex="7">
                                            </asp:DropDownList>
                                        </div>
                                        <div id="FestivalBonusContainer">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblSalaryHeadId" runat="server" class="control-label required-field"
                                                    Text="Bonus Date"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtBonusDate" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="PerioDicalBonusContainer">
                                            <div class="col-md-2">
                                                <asp:HiddenField ID="hfBonusMonthlyPeriodId" runat="server"></asp:HiddenField>
                                                <asp:Label ID="lblBonusMonthlyPeriod" runat="server" class="control-label" Text="Effected Period"></asp:Label>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ddlBonusMonthlyPeriod" runat="server" TabIndex="1" CssClass="form-control">
                                                    <asp:ListItem Value="1">1</asp:ListItem>
                                                    <asp:ListItem Value="2">2</asp:ListItem>
                                                    <asp:ListItem Value="3">3</asp:ListItem>
                                                    <asp:ListItem Value="4">4</asp:ListItem>
                                                    <asp:ListItem Value="5">5</asp:ListItem>
                                                    <asp:ListItem Value="6">6</asp:ListItem>
                                                    <asp:ListItem Value="7">7</asp:ListItem>
                                                    <asp:ListItem Value="8">8</asp:ListItem>
                                                    <asp:ListItem Value="9">9</asp:ListItem>
                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                    <asp:ListItem Value="11">11</asp:ListItem>
                                                    <asp:ListItem Value="12">12</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Month"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="AddMultipleBonus" style="display: none">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <input type="button" id="btnAddMultipleBonus" value="Add" class="TransactionalButton btn btn-primary" />
                                            </div>
                                        </div>
                                        <div class="panel-body">
                                            <div id="ltlTableWiseBonusAdd" runat="server" clientidmode="Static">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Button ID="btnFestivalBonus" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                                                OnClick="btnEmpBonusSave_Click" OnClientClick="javascript:return ValidationNPreprocess();" />
                                            <asp:Button ID="btnClearFestivalBonus" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                                OnClientClick="javascript: return PerformClearAction();" OnClick="btnEmpBonusClear_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-7">
            <div id="Div10" class="panel panel-default">
                <div class="panel-heading">
                    Attendance Device
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Device Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAttendanceDevice" runat="server" CssClass="form-control"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlAttendanceDevice_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDeviceType" runat="server" class="control-label" Text="Device Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDeviceType" runat="server" TabIndex="1" CssClass="form-control">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="officein">Office In</asp:ListItem>
                                    <asp:ListItem Value="officeout">Office Out</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnAttendanceDeviceUpdate" runat="server" Text="Update" TabIndex="4"
                                    CssClass="TransactionalButton btn btn-primary" OnClick="btnAttendanceDeviceUpdate_Click"
                                    OnClientClick="javascript:return ValidationNPreprocess();" />
                                <button type="button" class="TransactionalButton btn btn-primary" onclick="javascript: return PerformAttendanceDeviceClearAction();">
                                    Clear</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-8">
            <asp:HiddenField ID="hfLeaveDeductionId" runat="server" Value="0" />
            <div id="LeaveDeductionEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Leave Deduction Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <p>
                                    <b>Monthly
                                <asp:TextBox runat="server" ID="txtBoxNoOfLate" CssClass="quantity" Width="50px " autocomplete="off">
                                </asp:TextBox>
                                        days Late
                                <asp:TextBox runat="server" ID="txtBoxNoOfLeave" CssClass="quantity" Width="50px" autocomplete="off">
                                </asp:TextBox>
                                        Leave deduct.
                                    </b>
                                </p>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div id="SearchPanel" class="panel panel-default">
                                    <div class="panel-heading">
                                        Deduction Priority
                                    </div>
                                    <div class="panel-body">
                                        <div class="form-group" id="LeaveTableContainer">
                                            <table class="table table-bordered table-condensed table-responsive" id="LeaveTable"
                                                style="width: 100%;">
                                                <thead>
                                                    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                                        <th style="width: 5%; text-align: center">
                                                            <input type="checkbox" id="ChkAll" />
                                                        </th>
                                                        <th style="width: 55%;">Leave Name
                                                        </th>
                                                        <th style="width: 40%;">Sequence
                                                        </th>
                                                        <th style="display: none">LeavId
                                                        </th>
                                                        <th style="display: none">DetailsId
                                                        </th>

                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnAddLeaveDeduction" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return SaveLeaveDeduction();" />
                                <asp:Button ID="btnClearLeaveDeduction" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return ClearLeaveDeduction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="LeaveDeductionSearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <div class="form-group" id="LeaveDeductionTableContainer">
                        <table class="table table-bordered table-condensed table-responsive" id="LeaveDeductionSearchTable"
                            style="width: 100%;">
                            <thead>
                                <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                    <th style="width: 85%;">Policy
                                    </th>
                                    <th style="width: 15%;">Action
                                    </th>
                                    <th style="display: none">Id
                                    </th>

                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
