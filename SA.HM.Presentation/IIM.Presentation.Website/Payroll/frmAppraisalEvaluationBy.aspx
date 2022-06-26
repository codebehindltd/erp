<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true"
    CodeBehind="frmAppraisalEvaluationBy.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmAppraisalEvaluationBy" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeForLoanSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var gc = [];
        var isClose;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Appraisal</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Appraisal Evaluation</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#AppraisalAccordion").accordion();

            $("#ContentPlaceHolder1_ddlEmployee").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            if ($("#ContentPlaceHolder1_hfApprEvaId").val() != "") {
                var apprEvaId = $("#ContentPlaceHolder1_hfApprEvaId").val();
                PerformEditAction(apprEvaId);
            }

            var txtEvaltnDate = '<%=txtEvaltnDate.ClientID%>'
            var txtEvalToDate = '<%=txtEvalToDate.ClientID%>'
            $('#' + txtEvaltnDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $('#' + txtEvalToDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            var approvalEvolutionId = $.trim(CommonHelper.GetParameterByName("ae"));
            var perform = $.trim(CommonHelper.GetParameterByName("per"));
            if (approvalEvolutionId != "" && ($("#ContentPlaceHolder1_hfEditLoad").val() =="0")) {                
                $("#ContentPlaceHolder1_hfEditLoad").val("1");
                $("#ContentPlaceHolder1_hfPerform").val(perform);
                $("#ContentPlaceHolder1_ddlEmployee").val(approvalEvolutionId).trigger('change');
            }            

            if (($("#ContentPlaceHolder1_hfEditLoad").val() == "1")) {
                var apprEvaId = $("#<%=hfApprEvaId.ClientID %>").val();
                if (apprEvaId == "")
                    apprEvaId = 0;
                else
                    apprEvaId = parseInt(apprEvaId, 10);

                if (apprEvaId > 0) {
                    $("#ContentPlaceHolder1_btnEmpApprEvaluation").val('Update').show();
                    $("#ContentPlaceHolder1_btnAppraisalEvalutionApproval").hide();
                    $("#ContentPlaceHolder1_btnEmpApprEvaluationClear").hide();
                }
                else {
                    $("#ContentPlaceHolder1_btnEmpApprEvaluation").val('Save').show();
                    $("#ContentPlaceHolder1_btnAppraisalEvalutionApproval").hide();
                    $("#ContentPlaceHolder1_btnEmpApprEvaluationClear").hide();
                }

                if ($("#ContentPlaceHolder1_hfPerform").val() == "approve") {
                    $("#ContentPlaceHolder1_btnEmpApprEvaluation").hide();
                    $("#ContentPlaceHolder1_btnAppraisalEvalutionApproval").show();
                    $("#ContentPlaceHolder1_btnEmpApprEvaluationClear").hide();
                }

                PerformEdit(approvalEvolutionId);
            }

        });

        function PerformEdit(approvalEvolutionId) {
            // debugger;
            PageMethods.GetAppraisalEvalutionRatingFactorDetailsByAppraisalEvalutionById(approvalEvolutionId, OnSuccessLoading, OnFailLoading)
            return false;
        }

        function OnSuccessLoading(result) {
            FillForm(result);
            return false;
        }
        function FillForm(Result) {
            debugger;

            var message = "";
            var accordionLength = $("#AppraisalAccordion > h3").length;
            var row = 0, indicatorName = "", remarksText = "", indicatorNameText = "", remarks = '';
            var ratingRow = 0, ratingFactorLength = 0, IsRemarksMandatory = 0;
            var indicatorId = 0, marks = 0, marksIndicatorId = 0, ratingValue = 0, weight = 0, totalWeight = 0;

            for (row = 0; row < accordionLength; row++) {

                indicatorName = $("#h" + (row + 1)).text();
                indicatorNameText = indicatorName.replace('Indicator:', '');

                //indicatorName = $.trim(indicatorName);
                //indicatorName = CommonHelper.WhiteSpaceRemove(indicatorName.replace('Indicator:', ''));

                marksIndicatorId = parseInt($.trim($("#mi" + (row + 1)).text()), 10);
                totalWeight = parseFloat($.trim($("#apw" + (row + 1)).text()));

                ratingFactorLength = $("#" + (row + 1) + " > tbody tr").length;

                for (ratingRow = 0; ratingRow < ratingFactorLength; ratingRow++) {

                    var options = $("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(4)").find("select");
                    //IsRemarksMandatory = options.val().split(',')[1];

                    remarks = $("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(5)").find("input");

                    //remarksText = $.trim(remarks.val());

                    var textt = $("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(2)").text();
                    var detailsId = +$("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(1)").text();

                    var EvolutionObj = _.findWhere(Result, { RatingFacotrDetailsId: detailsId });
                    if (EvolutionObj != null) {
                        var index = _.indexOf(Result, EvolutionObj);

                        options.val(Result[index].RatingDropDownValue).trigger('change');
                        remarks.val(Result[index].Remarks);
                    }
                }
            }

            return false;
        }

        function OnFailLoading(error) {
            toastr.error(error.get_message());
            return false;
        }

        function AppraisalEvalutionSave(approvedStatus) {

            if ($("#ContentPlaceHolder1_ddlEmployee").val() == "0") {
                toastr.warning('Please Provide Employee To Appraise.');
                isClose = false;
                return false;
            }

            var message = "";
            var accordionLength = $("#AppraisalAccordion > h3").length;

            var row = 0, indicatorName = "", remarksText = "", indicatorNameText = "", remarks = '';
            var ratingRow = 0, ratingFactorLength = 0, IsRemarksMandatory = 0;
            var indicatorId = 0, marks = 0, marksIndicatorId = 0, ratingValue = 0, weight = 0, totalWeight = 0;

            var apprEvaId = $("#<%=hfApprEvaId.ClientID %>").val();

            if (apprEvaId == "")
                apprEvaId = 0;
            else
                apprEvaId = parseInt(apprEvaId, 10);

            var evaDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($("#ContentPlaceHolder1_txtEvaltnDate").val(), innBoarDateFormat);
            var evaToDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($("#ContentPlaceHolder1_txtEvalToDate").val(), innBoarDateFormat);

            var appraisalEvalution = [];

            var appraisalIndicator = {
                AppraisalEvalutionById: apprEvaId,
                EmpId: $("#ContentPlaceHolder1_ddlEmployee").val(),
                AppraisalType: $("#<%=ddlAppraisalType.ClientID %>").val(),
                EvaluationFromDate: evaDate,
                EvaluationToDate: evaToDate,
                ApprovalStatus: approvedStatus
            }

            var empId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();

            if (empId == 0) {

                isClose = false;
                alert('Please Provide Employee Information');
                return false;
            }
            else if (evaDate == "") {

                isClose = false;
                alert('Please Provide Evaluation From Date');
                return false;
            }
            else if (evaToDate == "") {
                isClose = false;
                alert('Please Provide Evaluation To Date');
                return false;
            }                       

            for (row = 0; row < accordionLength; row++) {

                indicatorName = $("#h" + (row + 1)).text();
                indicatorNameText = indicatorName.replace('Indicator:', '');

                marksIndicatorId = parseInt($.trim($("#mi" + (row + 1)).text()), 10);
                totalWeight = parseFloat($.trim($("#apw" + (row + 1)).text()));

                ratingFactorLength = $("#" + (row + 1) + " > tbody tr").length;

                for (ratingRow = 0; ratingRow < ratingFactorLength; ratingRow++) {

                    var options = $("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(4)").find("select");
                    IsRemarksMandatory = options.val().split(',')[1];

                    remarks = $("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(5)").find("input");

                    remarksText = $.trim(remarks.val());

                    var textt = $("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(2)").text();
                    var detailsId = $("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(1)").text();

                    //if (IsRemarksMandatory == "1" && remarksText == "") {
                    //    message = "Please Give Remarks for " + textt + " of " + indicatorNameText;
                    //    break;
                    //}

                    if (detailsId == "") {
                        detailsId = 0;
                    }

                    debugger;
                    indicatorId = $("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(0)").text();
                    weight = $("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(3)").text();
                    ratingValue = parseFloat(options.val().split(',')[0]);

                    if (approvedStatus == "Approved") {
                        if (ratingValue == -1) {
                            message = "Please Provide Ratings for Indicator: " + indicatorNameText;
                            break;
                        }
                    }

                    marks = (parseFloat(options.val().split(',')[0]) * parseFloat(weight)) / totalWeight;

                    appraisalEvalution.push({
                        MarksIndicatorId: marksIndicatorId,
                        AppraisalRatingFactorId: indicatorId,
                        RatingFacotrDetailsId: detailsId,
                        Marks: marks,
                        Weight: weight,
                        RatingValue: ratingValue,
                        Remarks: remarksText
                    });

                }

                if (message != "") {
                    appraisalEvalution = [];
                    break;
                }
            }

            if (message != "") {
                isClose = false;
                alert(message);
                return false;
            }
            if (apprEvaId > 0) {
                PageMethods.UpdateAppraisalEvalution(appraisalIndicator, appraisalEvalution, OnAppraisalEvalutionUpdateSucceed, OnAppraisalEvalutionUpdateFailed);
            }
            else {
                PageMethods.SaveAppraisalEvalution(appraisalIndicator, appraisalEvalution, OnAppraisalEvalutionSaveSucceed, OnAppraisalEvalutionSaveFailed);
            }

            return false;
        }
        
        function SaveNClose(approvedStatus) {
            //debugger;
            isClose = true;
            $.when(AppraisalEvalutionSave(approvedStatus)).done(function () {
                if (isClose) {                    
                    if (typeof parent.CloseDialog === "function") {
                        parent.CloseDialog();
                    }
                }
            });
            return false;
        }
        var vvv = [];
        function OnAppraisalEvalutionSaveSucceed(result) {
            if (result.IsSuccess) {
                parent.ShowAlert(result.AlertMessage);
                parent.SearchInformation(1, 1);
                $("#<%=hfApprEvaId.ClientID %>").val("");
                ClearForm();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnAppraisalEvalutionSaveFailed(error) {
            alert('Please Provide Employee Information');
        }
        function OnAppraisalEvalutionUpdateSucceed(result) {

            if (result.IsSuccess) {
                parent.ShowAlert(result.AlertMessage);
                parent.SearchInformation(1, 1);
                $("#<%=hfApprEvaId.ClientID %>").val("");
                ClearForm();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnAppraisalEvalutionUpdateFailed(error) {
            toastr.error("Please Contact With Admin.");
        }

        function WorkAfterSearchEmployee() { }

        function PerformEditAction(apprEvaId) {
            CommonHelper.SpinnerOpen();

            var empId = $("#ContentPlaceHolder1_ddlEmployee").val();
            PageMethods.EditAppraisalEvaluation(apprEvaId, empId, OnEditSuccess, OnEditFailaure);
        }
        var vv = [];
        function OnEditSuccess(result) {
            vv = result;

            if (result.Master == null) {
                return;
            }

            var row = 0, indicatorName = "", id = 0;
            var ratingRow = 0, ratingFactorLength = 0;
            var indicatorIdCount = 0, detailsId = '';
            var md = {}, v = {};

            var accordionLength = $("#AppraisalAccordion > h3").length;

            for (row = 0; row < accordionLength; row++) {

                indicatorName = $("#h" + (row + 1)).text();

                indicatorName = $.trim(indicatorName);
                indicatorName = CommonHelper.WhiteSpaceRemove(indicatorName.replace('Indicator:', ''));

                ratingFactorLength = $("#" + (row + 1) + " > tbody tr").length;

                for (ratingRow = 0; ratingRow < ratingFactorLength; ratingRow++) {

                    id = parseInt($("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(0)").text(), 10);

                    md = _.findWhere(result.Details, { AppraisalRatingFactorId: id });

                    gc = md;

                    if (md != null) {
                        v = _.findWhere(result.RatingFactorScale, { RatingValue: md.RatingValue });
                        detailsId = $("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(1)").text(md.RatingFacotrDetailsId);
                    }

                    var options = $("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(4)").find("select");
                    var remarks = $("#" + (row + 1) + " > tbody tr:eq(" + ratingRow + ")").find("td:eq(5)").find("input");

                    var av = '';

                    if (v != null) {
                        av = v.RatingValue + "," + (v.IsRemarksMandatory == true ? 1 : 0);
                        options.val(av);
                    }

                    if (md != null)
                        remarks.val(md.Remarks);

                    indicatorIdCount++;
                    md = {};
                    v = {};
                    detailsId = '';
                }

                indicatorIdCount -= 1;
            }

            CommonHelper.SpinnerClose();
        }

        function OnEditFailaure(error) {
            CommonHelper.SpinnerClose();
            toastr.error("Please Contact With Admin.");
        }

        function ClearForm() {
            $("#ContentPlaceHolder1_ddlEmployee").val("0");
            $("#ContentPlaceHolder1_txtEvaltnDate").val("");
            $("#ContentPlaceHolder1_txtEvalToDate").val("");
            $("#ContentPlaceHolder1_ddlAppraisalType").val("0");
        }

    </script>

    <asp:HiddenField ID="hfEditLoad" runat="server" Value="0" />
    <asp:HiddenField ID="hfPerform" runat="server" Value="" />
    <asp:HiddenField ID="hfApprEvaId" runat="server" Value="" />
    <div id="ApprEvaluationEntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Appraisal Evaluation
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group" style="display:none">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Employee"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control" AutoPostBack="true"
                            TabIndex="3" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblEvaltnDate" runat="server" class="control-label" Text="Evaluation From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEvaltnDate" runat="server" CssClass="form-control" TabIndex="1"
                            Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblEvalToDate" runat="server" class="control-label" Text="Evaluation To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEvalToDate" runat="server" CssClass="form-control" TabIndex="2"
                            Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblAppraisalType" runat="server" class="control-label" Text="Appraisal Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlAppraisalType" runat="server" Enabled="false" CssClass="form-control"
                            TabIndex="3">
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
                        <asp:TextBox ID="txtCompletionBy" runat="server" Enabled="false" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="myTabs" class="form-group" style="width: 97%;">
        <div id="appraisalEvalutionConatainer" runat="server">
        </div>
    </div>
    <div id="SaveButtonPanel" class="row" style="padding-top: 5px; display: none;">
        <div class="col-md-12">
            <asp:Button ID="btnEmpApprEvaluation" runat="server" Text="Save" TabIndex="2" CssClass="TransactionalButton btn btn-primary"
                OnClientClick="javascript:return SaveNClose('')" OnClick="btnEmpApprEvaluation_Click" />
            <asp:Button ID="btnAppraisalEvalutionApproval" runat="server" Text="Approve" TabIndex="2"
                CssClass="TransactionalButton btn btn-primary" OnClientClick="javascript:return SaveNClose('Approved')" />
            <asp:Button ID="btnEmpApprEvaluationClear" runat="server" Text="Clear" TabIndex="9"
                CssClass="TransactionalButton btn btn-primary" OnClick="btnEmpApprEvaluationClear_Click" />
        </div>
    </div>
    <script type="text/javascript">
        var isSaveButtonEnable = '<%=isSaveButtonEnable%>';
        if (isSaveButtonEnable > -1)
            $("#SaveButtonPanel").show();
    </script>
</asp:Content>
