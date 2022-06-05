<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"  EnableEventValidation="false" CodeBehind="frmInterviewEvalution.aspx.cs" Inherits="HotelManagement.Presentation.Website.Recruitment.frmInterviewEvalution" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Interview Evaluation</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#myTabs").tabs();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#InterviewType").hide();
            $("#ContentPlaceHolder1_ddlApplicant").change(function () {
                LoadInterviewType();
            });

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $("#<%=ddlJobCircular.ClientID %>").change(function () {
                var id = $("#<%=ddlJobCircular.ClientID %>").val();
                GetApplicantByJobCircular(id);
                return false;
            });
        });

        function LoadInterviewType() {

            var applicantId = $("#ContentPlaceHolder1_ddlJobCircular").val(), jobCircularId = $("#ContentPlaceHolder1_ddlApplicant").val();
            PageMethods.LoadInterviews(applicantId, jobCircularId, OnLoadSucceeded, OnLoadFailed);
        }

        function OnLoadSucceeded(result) {
            $("#InterviewType").show();
            $("#interviewMarksContainer").html(result);
        }

        function OnLoadFailed() {
            toastr.error(error.get_message());
        }

        function AddList() {

            var jobCircularId = $.trim($("#<%=ddlJobCircular.ClientID %>").val());
            var applicantId = $.trim($("#<%=ddlApplicant.ClientID %>").val());
            var remarks = $.trim($("#<%=txtRemarks.ClientID %>").val());

            var interviewTypeId = 0, obtainedMarks = 0, rsultDetailId = 0, examMarks = "0", dbMarks = "0";

            var saveObj = new Array();
            var editObj = new Array();
            var deleteObj = new Array();

            $("#InterviewType tbody tr").each(function () {

                rsultDetailId = $.trim($(this).find("td:eq(0)").text());
                interviewTypeId = $.trim($(this).find("td:eq(1)").text());
                dbMarks = $.trim($(this).find("td:eq(2)").text());

                obtainedMarks = $.trim($(this).find("td:eq(5)").find("input").val());

                if (obtainedMarks == "") {
                    obtainedMarks = "0";
                }

                if (rsultDetailId == "0" && obtainedMarks != "0") {
                    saveObj.push({
                        ApplicantResultId: "0",
                        JobCircularId: jobCircularId,
                        ApplicantId: applicantId,
                        InterviewTypeId: interviewTypeId,
                        MarksObtain: obtainedMarks,
                        Remarks: remarks
                    });
                }
                else if (rsultDetailId != "0" && obtainedMarks != "0" && obtainedMarks != dbMarks) {
                    editObj.push({
                        ApplicantResultId: rsultDetailId,
                        JobCircularId: jobCircularId,
                        ApplicantId: applicantId,
                        InterviewTypeId: interviewTypeId,
                        MarksObtain: obtainedMarks,
                        Remarks: remarks
                    });
                }
                else if (rsultDetailId != "0" && obtainedMarks == "0") {
                    deleteObj.push({
                        ApplicantResultId: rsultDetailId,
                        JobCircularId: jobCircularId,
                        ApplicantId: applicantId,
                        InterviewTypeId: interviewTypeId,
                        MarksObtain: obtainedMarks,
                        Remarks: remarks
                    });
                }

            });

            $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveObj));
            $("#<%=hfEditObj.ClientID %>").val(JSON.stringify(editObj));
            $("#<%=hfDeleteObj.ClientID %>").val(JSON.stringify(deleteObj));
            return true;
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvInterviewEvaluation tbody tr").length;

            var jobCircularId = $("#<%=ddlSearchJobCircular.ClientID %>").val();
            var jobTitle = $("#ContentPlaceHolder1_txtSearchJobTitle").val();
            var formDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();


            PageMethods.SearchApplicantInterviewResult(jobCircularId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, jobTitle, formDate, toDate, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }

        function OnLoadObjectSucceeded(result) {

            $("#gvInterviewEvaluation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvInterviewEvaluation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvInterviewEvaluation tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:50%; cursor:pointer;\">" + gridObject.JobTitle + "</td>";
                tr += "<td align='left' style=\"width:30%; cursor:pointer;\">" + gridObject.ApplicantName + "</td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.ApplicantId + "' ,'" + gridObject.JobCircularId + "')\" alt='Edit Information' border='0' /></td>";

                tr += "</tr>"

                $("#gvInterviewEvaluation tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);


            return false;
        }
        function OnLoadObjectFailed(error) {
            alert(error.get_message());
        }

        function PerformEditAction(applicantId, jobCircularId) {
            PageMethods.LoadApplicantResult(applicantId, jobCircularId, OnApplicantLoadSucceeded, OnApplicantLoadFailed);
        }
        function OnApplicantLoadSucceeded(result) {
            $("#<%=ddlJobCircular.ClientID %>").val(result.JobCircularId).trigger('change');
            $("#<%=ddlApplicant.ClientID %>").val(result.ApplicantId);
            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
            $("#<%=hfApplicantId.ClientID %>").val(result.ApplicantId);
            $("#<%=hfJobCircularId.ClientID %>").val(result.JobCircularId);
            $("#<%=btnSave.ClientID %>").val("Update");
            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });

            LoadInterviews(result.ApplicantId, result.JobCircularId);
        }
        function OnApplicantLoadFailed() {
            alert(error.get_message());
        }

        function LoadInterviews(applicantId, jobCircularId) {
            PageMethods.LoadInterviews(applicantId, jobCircularId, OnInterviewLoadSucceeded, OnInterviewLoadFailed);
        }
        function OnInterviewLoadSucceeded(result) {
            $("#InterviewType").show();
            $("#interviewMarksContainer").html(result);
        }
        function OnInterviewLoadFailed() {
            alert(error.get_message());
        }
        function GetApplicantByJobCircular(jobCircularId) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './frmInterviewEvalution.aspx/GetApplicantByJobCircular',
                data: JSON.stringify({ jobCircularId: jobCircularId }),
                dataType: "json",
                async: false,
                success: function (result) {
                    if (result.d.length > 0) {

                        var list = result.d;
                        var ddlItem = '<%=ddlApplicant.ClientID%>';
                        var control = $('#' + ddlItem);
                        control.empty();

                        if (list != null) {
                            if (list.length > 0) {
                                control.removeAttr("disabled");
                                control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                                for (i = 0; i < list.length; i++) {
                                    control.append('<option title="' + list[i].EmployeeName + '" value="' + list[i].ApplicantId + '">' + list[i].EmployeeName + '</option>');
                                }
                            }
                            else {
                                control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                            }
                        }
                        else {
                            control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                        }
                        return false;
                    }
                },
                error: function (result) {

                }
            });
        }
    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfSaveObj" runat="server" />
    <asp:HiddenField ID="hfEditObj" runat="server" />
    <asp:HiddenField ID="hfAddInEditObj" runat="server" />
    <asp:HiddenField ID="hfDeleteObj" runat="server" />
    <asp:HiddenField ID="hfApplicantId" runat="server" />
    <asp:HiddenField ID="hfJobCircularId" runat="server" />
    <asp:HiddenField ID="hfApplicantResultId" runat="server" Value="" />
    <asp:HiddenField ID="hfIsCurrentOrPreviousPage" runat="server" Value="" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="circularEntry" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-1">Interview Evalution Details</a></li>
            <li id="circularSearch" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-2">Search Interview Evalution</a></li>
        </ul>
        <div id="tab-1">
            <div id="" class="panel panel-default">
                <div class="panel-heading">
                    Job Circular Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLoanType" runat="server" class="control-label required-field" Text="Job Circular"></asp:Label>                               
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlJobCircular" runat="server" CssClass="form-control"
                                    >
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Applicant"></asp:Label>                               
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlApplicant" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="InterviewType" style="display: none">
                            <div id="interviewMarksContainer">
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    Rows="5"></asp:TextBox>
                            </div>                           
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSave_Click" OnClientClick="javascript:return AddList()" />
                                <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Loan Search</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSLoanType" runat="server" class="control-label" Text="Job Circular"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchJobCircular" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblJobTitle" runat="server" class="control-label" Text="Job Circular Title"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchJobTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary">
                                    Search</button>
                                <%--<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                            OnClick="btnSearch_Click" />--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <table id='gvInterviewEvaluation' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="width: 40%;" />
                            <col style="width: 40%;" />
                            <col style="width: 20%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>
                                    Job Circular
                                </td>
                                <td>
                                    Applicant
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
