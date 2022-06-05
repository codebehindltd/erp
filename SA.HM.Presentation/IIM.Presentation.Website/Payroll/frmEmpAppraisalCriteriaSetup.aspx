<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmEmpAppraisalCriteriaSetup.aspx.cs"
    Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpAppraisalCriteriaSetup" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeForLoanSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        fieldset {
            border: 0 none;
            margin-top: 5px;
            padding: 2px;
        }
    </style>
    <script type="text/javascript">
        var minApprisalDate = "";
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Appraisal</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Appraisal Criteria Setup</li>";
            var breadCrumbs = moduleName + formName;

            if (IsCanSave) {
                $('#ContentPlaceHolder1_btnEmpApprEvaluation').show();
            } else {
                $('#ContentPlaceHolder1_btnEmpApprEvaluation').hide();
            }

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#IndividualAppraisalBlock").hide();
            $("#GroupAppraisalBlock").hide();

            if ($("#ContentPlaceHolder1_ddlAppraisalSetupType").val() == "Individual") {
                $("#IndividualAppraisalBlock").show();
                $("#GroupAppraisalBlock").hide();
                $("#DepartmentLabelDiv").hide();
                $("#DepartmentDropDownDiv").hide();
            }
            else if ($("#ContentPlaceHolder1_ddlAppraisalSetupType").val() == "Group") {
                $("#IndividualAppraisalBlock").hide();
                $("#GroupAppraisalBlock").show();
                $("#DepartmentLabelDiv").show();
                $("#DepartmentDropDownDiv").show();
            }

            $("#ContentPlaceHolder1_ddlAppraisalSetupType").change(function () {
                var setupType = '';
                setupType = $(this).val();

                if (setupType == "0") {
                    toastr.warning("Please Select Setup Criteria Type");
                    return false;
                }

                if (setupType == "Individual") {
                    $("#IndividualAppraisalBlock").show();
                    $("#GroupAppraisalBlock").hide();
                    $("#DepartmentLabelDiv").hide();
                    $("#DepartmentDropDownDiv").hide();
                }
                else if (setupType == "Group") {
                    $("#IndividualAppraisalBlock").hide();
                    $("#GroupAppraisalBlock").show();
                    $("#DepartmentLabelDiv").show();
                    $("#DepartmentDropDownDiv").show();
                }
            });

            $("#ContentPlaceHolder1_ddlDepartmentId").change(function () {

                var setupType = '';
                setupType = $("#ContentPlaceHolder1_ddlAppraisalSetupType").val();

                if (setupType == "0") {
                    toastr.warning("Please Select Setup Criteria Type");
                    return false;
                }

                //LoadDepartmentalManager($(this).val());

                if (setupType == "Individual") {

                }
                else if (setupType == "Group") {
                    LoadEmployeeGroup();
                }
            });

            $("#checkAllEmployee").click(function () {
                if ($(this).is(":checked") == true) {
                    $("#gvEmployee tbody tr").find("td:eq(0)").find("input").prop('checked', true);
                }
                else {
                    $("#gvEmployee tbody tr").find("td:eq(0)").find("input").prop('checked', false);
                }
            });

            $("#gvApprEvaluation").delegate("td > img.RtngFactrDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var evaluationId = $.trim($(this).parent().parent().find("td:eq(6)").text());
                    var params = JSON.stringify({ sEmpId: evaluationId });

                    var $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/Payroll/frmAppraisalEvaluationBy.aspx/DeleteApprEvaluationById",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {

                            $row.remove();
                            $("#myTabs").tabs('load', 1);
                        },
                        error: function (error) {
                        }
                    });
                }
            });

            $("#AppraisalAccordion").delegate("input.checkAllRatingFactor", "click", function () {
                var tablId = $(this).parent().parent().parent().parent().attr("id");

                if ($(this).is(":checked") == true) {
                    $("#" + tablId + " tbody tr").find("td:eq(2)").find("input").prop('checked', true);
                }
                else {
                    $("#" + tablId + " tbody tr").find("td:eq(2)").find("input").prop('checked', false);
                }
            });

            minApprisalDate = $("#<%=hfMinApprisalDate.ClientID %>").val();

            $("#ContentPlaceHolder1_txtCompletionBy").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: minApprisalDate,
            });

            $("#ContentPlaceHolder1_txtEvaltnDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtEvalToDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtEvalToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtEvaltnDate").datepicker("option", "maxDate", selectedDate);
                }
            });
            $("#ContentPlaceHolder1_txtSEvaFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtSToDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtSToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtSEvaFromDate").datepicker("option", "maxDate", selectedDate);
                }
            });
            $("#ContentPlaceHolder1_ddlEvaluatorName").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#btnSearch").click(function () {
                GridPaging(1, 1);
            });
        });

        <%--function LoadDepartmentalManager(departmentId) {
            PageMethods.LoadDepartmentalManager(departmentId, OnLoadDepartmentalManagerSucceeded, OnDepartmentalManagerFailed);
            return false;
        }

        function OnLoadDepartmentalManagerSucceeded(result) {
            var list = result;
            var controlId = '<%=ddlEvaluatorName.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].ItemName + '" value="' + list[i].EmpId + '">' + list[i].DisplayName + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            var department = $("#ContentPlaceHolder1_hfDepartmentId").val();
            $('#' + controlId).val(department);
        }
        function OnDepartmentalManagerFailed(error) {
        }--%>

        function AppraisalEvalutionSave() {

            if ($("#ContentPlaceHolder1_ddlAppraisalSetupType").val() == "0") {
                toastr.warning('Please Provide Appraisal Config Type.');
                return false;
            }            
            else if ($("#ContentPlaceHolder1_ddlEvaluatorName").val() == "0") {
                toastr.warning('Please Select A Evaluator Name.');
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtCompletionBy").val()) == "") {
                toastr.warning('Please Give Date For Appraisal Completion.');
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtEvaltnDate").val()) == "") {
                toastr.warning('Please Give Date For Appraisal From Date.');
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtEvalToDate").val()) == "") {
                toastr.warning('Please Give Date For Appraisal To Date.');
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlAppraisalType").val() == "0") {
                toastr.warning('Please Select Appraisal Type.');
                return false;
            }

            if ($("#ContentPlaceHolder1_ddlAppraisalSetupType").val() == "Group") {
                if ($("#ContentPlaceHolder1_ddlDepartmentId").val() == "0") {
                    toastr.warning('Please Select A Department.');
                    return false;
                }
            }

            var setupType = '';
            setupType = $("#ContentPlaceHolder1_ddlAppraisalSetupType").val();

            var EmpLst = new Array();
            var empId = '';

            if (setupType == "Individual") {

                if ($("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val() != "0") {
                    empId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();

                    EmpLst.push(
                        empId
                    );
                }
            }
            else if (setupType == "Group") {
                $("#gvEmployee tbody tr").each(function () {
                    if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                        EmpLst.push(
                            $(this).find("td:eq(4)").text()
                        );
                    }
                });
            }

            if (EmpLst.length == 0) {
                toastr.warning("Please Select Employee For Evalution.");
                return false;
            }

            var message = "";
            var accordionLength = $("#AppraisalAccordion > h3").length;

            var row = 0, indicatorName = "", indicatorId = 0, remarksText = "", indicatorNameText = "";
            var ratingRow = 0, ratingFactorLength = 0, IsRemarksMandatory = 0;
            var ratingFactorId = 0, ratingWeight = 0, appraisalWeight = 0; marksIndicatorId = 0;

            var apprEvaId = $("#<%=hfApprEvaId.ClientID %>").val();

            if (apprEvaId == "")
                apprEvaId = 0;
            else
                apprEvaId = parseInt(apprEvaId, 10);


            var evaluationFromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($("#ContentPlaceHolder1_txtEvaltnDate").val(), innBoarDateFormat);
            var evaluationToDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($("#ContentPlaceHolder1_txtEvalToDate").val(), innBoarDateFormat);
            var evalutionCompletionBy = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($("#ContentPlaceHolder1_txtCompletionBy").val(), innBoarDateFormat);

            var AppraisalEvalutionBy = {
                AppraisalEvalutionById: apprEvaId,
                AppraisalConfigType: setupType,
                EvalutiorId: $("#ContentPlaceHolder1_ddlEvaluatorName").val(),
                EvalutionCompletionBy: evalutionCompletionBy,
                AppraisalType: $("#<%=ddlAppraisalType.ClientID %>").val(),
                EvaluationFromDate: evaluationFromDate,
                EvaluationToDate: evaluationToDate
            }

            var AppraisalEvalutionCriteria = new Array(), EditedAppraisalEvalutionCriteria = new Array(), DeletedAppraisalEvalutionCriteria = new Array();

            for (row = 0; row < accordionLength; row++) {
                indicatorId = row + 1;

                marksIndicatorId = $.trim($("#mi" + indicatorId).text());
                appraisalWeight = $.trim($("#apw" + indicatorId).text());

                ratingFactorLength = $("#" + indicatorId + " > tbody tr").length;

                for (ratingRow = 0; ratingRow < ratingFactorLength; ratingRow++) {

                    if ($("#" + indicatorId + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(2)").find('input').is(":checked") == true) {

                        var detailsId = $("#" + indicatorId + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(1)").text();
                        if (detailsId == "") {
                            detailsId = 0;
                        }

                        ratingFactorId = $("#" + indicatorId + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(0)").text();
                        ratingWeight = $("#" + indicatorId + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(4)").text();

                        if (detailsId == 0) {
                            AppraisalEvalutionCriteria.push({
                                RatingFacotrDetailsId: detailsId,
                                MarksIndicatorId: marksIndicatorId,
                                AppraisalRatingFactorId: ratingFactorId,
                                AppraisalWeight: appraisalWeight,
                                RatingWeight: ratingWeight,
                                RatingValue: 0.0,
                                Marks: 0.0
                            });
                        }
                        else {
                            EditedAppraisalEvalutionCriteria.push({
                                RatingFacotrDetailsId: detailsId,
                                MarksIndicatorId: marksIndicatorId,
                                AppraisalRatingFactorId: ratingFactorId,
                                AppraisalWeight: appraisalWeight,
                                RatingWeight: ratingWeight,
                                RatingValue: 0.0,
                                Marks: 0.0
                            });
                        }
                    }
                    else {
                        var detailsId = $("#" + indicatorId + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(1)").text();
                        if (detailsId == "") {
                            detailsId = 0;
                        }

                        if (detailsId > 0) {
                            DeletedAppraisalEvalutionCriteria.push({
                                RatingFacotrDetailsId: detailsId,
                                MarksIndicatorId: marksIndicatorId,
                                AppraisalRatingFactorId: ratingFactorId,
                                AppraisalWeight: appraisalWeight,
                                RatingWeight: ratingWeight,
                                RatingValue: 0.0,
                                Marks: 0.0
                            });
                        }
                    }
                }
            }

            if (AppraisalEvalutionCriteria.length == 0 && apprEvaId == 0) {
                toastr.warning("Please Select Rating Factors For Evalution Criteria.");
                return false;
            }
            else if (EditedAppraisalEvalutionCriteria.length == 0 && apprEvaId > 0) {
                toastr.warning("Please Select Rating Factors For Evalution Criteria.");
                return false;
            }
            if (apprEvaId > 0) {
                PageMethods.UpdateAppraisalEvalution(AppraisalEvalutionBy, AppraisalEvalutionCriteria, EditedAppraisalEvalutionCriteria, DeletedAppraisalEvalutionCriteria, OnAppraisalEvalutionUpdateSucceed, OnAppraisalEvalutionUpdateFailed);
            }
            else {
                PageMethods.SaveAppraisalEvalutionCriteria(AppraisalEvalutionBy, EmpLst, AppraisalEvalutionCriteria, OnAppraisalEvalutionSaveSucceed, OnAppraisalEvalutionSaveFailed);
            }

            return false;
        }

        function OnAppraisalEvalutionSaveSucceed(result) {

            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#form1")[0].reset();
                $("#<%=hfApprEvaId.ClientID %>").val("");
                $("#gvEmployee tbody").html("");
                $("#<%=btnEmpApprEvaluation.ClientID %>").val("Save");
                setTimeout(function () {
                    window.location.href = "./frmEmpAppraisalCriteriaSetup.aspx";
                }, 100);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

        }

        function OnAppraisalEvalutionSaveFailed(error) {
            toastr.error('Please Contact With Admin');
        }

        function OnAppraisalEvalutionUpdateSucceed(result) {

            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#<%=btnEmpApprEvaluation.ClientID %>").val("Save");
                $("#form1")[0].reset();
                $("#<%=hfApprEvaId.ClientID %>").val("");
                $("#gvEmployee tbody").html("");
                $("#ContentPlaceHolder1_ddlAppraisalSetupType").val("0");
                $("#ContentPlaceHolder1_ddlDepartmentId").val("0");
                $("#ContentPlaceHolder1_ddlEvaluatorName").val("0").trigger('change');

                setTimeout(function () {
                    window.location.href = "./frmEmpAppraisalCriteriaSetup.aspx";
                }, 100);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

        }

        function OnAppraisalEvalutionUpdateFailed(error) {
        }

        function WorkAfterSearchEmployee() { }

        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
            $("#AppraisalAccordion").accordion();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var evaFromDate, evaToDate;
            var gridRecordsCount = $("#gvApprEvaluation tbody tr").length;
            var employeeId = $("#ContentPlaceHolder1_employeeForLoanSearch_hfEmployeeId").val();
            var appraisalType = $("#<%=ddlSAppraisalType.ClientID %>").val();
            //var evaFromDate = $("#<%=txtSEvaFromDate.ClientID %>").val();
            //var evaToDate = $("#<%=txtSToDate.ClientID %>").val();
            var fromDate = $("#ContentPlaceHolder1_txtSEvaFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtSToDate").val();
            if (fromDate != "") {
                evaFromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($("#ContentPlaceHolder1_txtSEvaFromDate").val(), innBoarDateFormat);
            }
            else evaFromDate = null;
            if (toDate != "") {
                evaToDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($("#ContentPlaceHolder1_txtSToDate").val(), innBoarDateFormat);
            }
            else evaToDate = null;

            if (employeeId == "0")
                employeeId = "";

            PageMethods.SearchApprEvaluationAndLoadGridInformation(employeeId, appraisalType, evaFromDate, evaToDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvApprEvaluation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Data Found</td> </tr>";
                $("#gvApprEvaluation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvApprEvaluation tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + gridObject.EmployeeName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.EvalutiorName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.AppraisalDuration + "</td>";
                tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.EvalutionCompletionByString + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.MarksOutOf + "</td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\">";
                if (IsCanEdit) {
                    tr += "<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.AppraisalEvalutionById + "')\" alt='Edit Information' border='0' />";
                }
                //else {
                //    tr += "<td align='right' style=\"width:8%; cursor:pointer;\"></td>";
                //}
                if (IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' class= 'RtngFactrDelete'  alt='Delete Information' border='0' />";
                }
                //else {
                //    tr += "<td align='right' style=\"width:8%; cursor:pointer;\"></td>";
                //}
                tr += "</td>";
                tr += "<td align='right' style=\"width:8%; display:none;\">" + gridObject.AppraisalEvalutionById + "</td>";

                tr += "</tr>"

                $("#gvApprEvaluation tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);


            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.warning(error.get_message());
        }

        function PerformEditAction(ApprEvaId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }

            PageMethods.EditAppraisalEvaluation(ApprEvaId, OnEditSuccess, OnEditFailaure);
        }
        var vv = [];
        function OnEditSuccess(result) {
            vv = result;

            if (result.Master == null) {
                return;
            }
            if (IsCanEdit) {
                $('#ContentPlaceHolder1_btnEmpApprEvaluation').show();
            } else {
                $('#ContentPlaceHolder1_btnEmpApprEvaluation').hide();
            }
            $("#<%=btnEmpApprEvaluation.ClientID %>").val("Update");

            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });


            $("#<%=hfApprEvaId.ClientID%>").val(result.Master.AppraisalEvalutionById);
            $("#ContentPlaceHolder1_txtEvaltnDate").val(GetStringFromDateTime(result.Master.EvaluationFromDate));
            $("#ContentPlaceHolder1_txtEvalToDate").val(GetStringFromDateTime(result.Master.EvaluationToDate));
            $("#ContentPlaceHolder1_ddlAppraisalType").val(result.Master.AppraisalType);

            //LoadDepartmentalManager(result.Master.DepartmentId);
            //var evalutiorId = result.Master.EvalutiorId;
            //setTimeout(function () {
            //    $("#ContentPlaceHolder1_ddlEvaluatorName").val(evalutiorId).trigger('change');
            //}, 100);

            $("#ContentPlaceHolder1_ddlEvaluatorName").val(result.Master.EvalutiorId).trigger('change');
            $("#ContentPlaceHolder1_ddlAppraisalSetupType").val(result.Master.AppraisalConfigType);
            $("#ContentPlaceHolder1_ddlDepartmentId").val(result.Master.DepartmentId);
            $("#ContentPlaceHolder1_txtCompletionBy").val(GetStringFromDateTime(result.Master.EvalutionCompletionBy));

            var row = 0, indicatorName = "", id = 0, indicatorId = 0;
            var ratingRow = 0, ratingFactorLength = 0;
            var indicatorIdCount = 0;

            var accordionLength = $("#AppraisalAccordion > h3").length;

            $("#ContentPlaceHolder1_employeeSearch_txtSearchEmployee").val(result.Master.EmployeeCode);
            $("#ContentPlaceHolder1_employeeSearch_btnSrcEmployees").trigger("click");

            if (result.Master.AppraisalConfigType == "Individual") {
                $("#IndividualAppraisalBlock").show();
                $("#GroupAppraisalBlock").hide();
                $("#DepartmentLabelDiv").hide();
                $("#DepartmentDropDownDiv").hide();
            }
            else if (result.Master.AppraisalConfigType == "Group") {
                $("#IndividualAppraisalBlock").hide();
                $("#GroupAppraisalBlock").show();
                $("#DepartmentLabelDiv").show();
                $("#DepartmentDropDownDiv").show();
            }

            for (row = 0; row < accordionLength; row++) {

                indicatorId = row + 1;

                ratingFactorLength = $("#" + indicatorId + " > tbody tr").length;


                for (ratingRow = 0; ratingRow < ratingFactorLength; ratingRow++) {
                    var detailsId = 0;
                    id = parseInt($("#" + indicatorId + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(0)").text(), 10);

                    var md = _.findWhere(result.Details, { AppraisalRatingFactorId: id });

                    if (md != null) {
                        if (md.RatingFacotrDetailsId > 0) {
                            $("#" + indicatorId + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(2)").find("input").prop("checked", true);
                            $("#" + indicatorId + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(1)").text(md.RatingFacotrDetailsId);
                        }
                    }
                    //indicatorIdCount++;
                }
                //indicatorIdCount -= 1;
            }
        }

        function OnEditFailaure(error) {
            toastr.warning(error.get_message());
        }

        function LoadEmployeeGroup() {
            var departmentId = $("#ContentPlaceHolder1_ddlDepartmentId").val();
            PageMethods.LoadDepartmentalWiseEmployee(departmentId, OnLoadEmployeeSucceeded, OnLoadEmployeeFailed);
            return false;
        }

        function OnLoadEmployeeSucceeded(result) {
            $("#gvEmployee tbody").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvEmployee tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result, function (count, gridObject) {

                totalRow += 1;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td style = 'text-align: center; width:7%;' > <input type='checkbox' id='chk" + gridObject.EmpId + "' /> </td>";
                tr += "<td align='left' style=\"width:53%; cursor:pointer;\">" + gridObject.DisplayName + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.EmpCode + "</td>";
                tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + gridObject.Designation + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + gridObject.EmpId + "</td>";

                tr += "</tr>"

                $("#gvEmployee tbody ").append(tr);
                tr = "";
            });

            return false;
        }
        function OnLoadEmployeeFailed(error) {
            toastr.error(error.get_message());
        }

    </script>
    <div id="MessageBox" class="toastr.warning toastr.warning-info" style="display: none;">
        <button type="button" class="close" data-dismiss="toastr.warning">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="hfMinApprisalDate" runat="server" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfApprEvaId" runat="server" Value="" />
    <asp:HiddenField ID="hfDepartmentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Criteria Setup</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Criteria Setup</a></li>
        </ul>
        <div id="tab-1">
            <div id="ApprEvaluationEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Appraisal Criteria Setup
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Appraisal Config Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAppraisalSetupType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="---Please Select---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Department Appraisal" Value="Group"></asp:ListItem>
                                    <asp:ListItem Text="Individual Appraisal" Value="Individual"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2" id="DepartmentLabelDiv" style="display:none;">
                                <asp:Label ID="lblDepartmentId" runat="server" class="control-label required-field"
                                    Text="Department"></asp:Label>
                            </div>
                            <div class="col-md-4" id="DepartmentDropDownDiv" style="display:none;">
                                <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Evaluator Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlEvaluatorName" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <fieldset>
                            <legend></legend>
                            <div style="padding-top: 5px;">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblEvaltnDate" runat="server" class="control-label required-field"
                                            Text="Evaluation From Date"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtEvaltnDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblEvalToDate" runat="server" class="control-label required-field"
                                            Text="Evaluation To Date"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtEvalToDate" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblAppraisalType" runat="server" class="control-label required-field"
                                            Text="Appraisal Type"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlAppraisalType" runat="server" CssClass="form-control" TabIndex="3">
                                            <asp:ListItem Value="0">---Please Select---</asp:ListItem>
                                            <asp:ListItem Value="Probationary">Probationary</asp:ListItem>
                                            <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                            <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                            <asp:ListItem Value="Annual">Annual</asp:ListItem>
                                            <asp:ListItem Value="Contractual">End of Contract</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label Style="text-align: left" ID="Label3" runat="server" class="control-label required-field" Text="Appraisal Compl. By"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCompletionBy" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <div id="IndividualAppraisalBlock" class="panel panel-default" style="height: 150px;">
                            <div class="panel-heading">
                                Individual Employee Assign
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                        </div>
                                        <div class="col-md-4">
                                        </div>
                                    </div>
                                    <UserControl:EmployeeSearch ID="employeeSearch" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div id="GroupAppraisalBlock" class="panel panel-default" style="min-height: 150px; max-height: auto;">
                            <div class="panel-heading">
                                Group Employee Assign
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div id="EmployeeGridContainer">
                                        <table id='gvEmployee' class="table table-bordered table-condensed table-responsive"
                                            width="100%">
                                            <colgroup>
                                                <col style="width: 7%;" />
                                                <col style="width: 53%;" />
                                                <col style="width: 15%;" />
                                                <col style="width: 25%;" />
                                            </colgroup>
                                            <thead>
                                                <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                                    <th style="text-align: center;">
                                                        <input type="checkbox" id="checkAllEmployee" title="Select All Employee" />
                                                    </th>
                                                    <th style="text-align: left;">Name
                                                    </th>
                                                    <th style="text-align: left;">Code
                                                    </th>
                                                    <th style="text-align: left;">Designation
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
                        <div class="form-group" style="width: 97%;">
                            <div id="appraisalEvalutionConatainer" runat="server">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpApprEvaluation" runat="server" Text="Save" TabIndex="2" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript:return AppraisalEvalutionSave()" OnClick="btnEmpApprEvaluation_Click" />
                                <asp:Button ID="btnEmpApprEvaluationClear" OnClientClick="return confirm('Do you want to clear?');" runat="server" Text="Clear" TabIndex="9"
                                    CssClass="TransactionalButton btn btn-primary" OnClick="btnEmpApprEvaluationClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Search Appraisal Evaluation
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeForLoanSearch ID="employeeForLoanSearch" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSEvaFromDate" runat="server" class="control-label" Text="Evaluation From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSEvaFromDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSEvaToDate" runat="server" class="control-label" Text="Evaluation To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSToDate" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSApprType" runat="server" class="control-label" Text="Appraisal Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSAppraisalType" runat="server" CssClass="form-control" TabIndex="3">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Value="Probationary">Probationary</asp:ListItem>
                                    <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                    <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                    <asp:ListItem Value="Annual">Annual</asp:ListItem>
                                    <asp:ListItem Value="Contractual">End of Contract</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
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
                    Search Information
                </div>
                <div class="panel-body">
                    <table id='gvApprEvaluation' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="width: 25%;" />
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            <col style="width: 10%;" />
                            <col style="width: 15%;" />
                            <col style="width: 10%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Employee
                                </td>
                                <td>Evaloator
                                </td>
                                <td>Duration
                                </td>                                
                                <td>Completion By
                                </td>
                                <td>Total Marks
                                </td>
                                <td style="text-align: right;">Action
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
