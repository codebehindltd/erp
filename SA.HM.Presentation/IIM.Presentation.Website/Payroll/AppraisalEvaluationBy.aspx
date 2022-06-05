<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="AppraisalEvaluationBy.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.AppraisalEvaluationBy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>

        var AppraisalEvaluationTable;

        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;

        $(document).ready(function () {

            AppraisalEvaluationTable = $("#tblAppraisalEvaluation").DataTable({
                data: [],
                columns: [
                    { title: "", "data": "AppraisalEvalutionById", visible: false },
                    { title: "#", "data": "SerialNumber", sWidth: '5%' },
                    { title: "Employee", "data": "EmployeeName", sWidth: '75%' },
                    { title: "Action", "data": null, sWidth: '20%' }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                    var row = '';

                    row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEdit('" + aData.AppraisalEvalutionById + "','" + aData.EmployeeName + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";

                    row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return Approval('" + aData.AppraisalEvalutionById + "','" + aData.EmployeeName + "');\"><img alt=\"Approve\" src=\"../Images/approved.png\" title='Approve' /></a>";

                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);

                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },

            });
            SearchInformation(1, 1);

        });
        function CloseDialog() {
            $("#AppraisalEvaluationDialouge").dialog('close');
            return false;
        }

        function PerformEdit(Id, Name) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }

            var iframeid = 'frmPrint';
            var url = "./frmAppraisalEvaluationBy.aspx?ae=" + Id + "&per=" + "edit";
            document.getElementById(iframeid).src = url;
            $("#AppraisalEvaluationDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 500,
                closeOnEscape: false,
                resizable: false,
                title: "Edit Appraisal Evaluation : " + Name,
                show: 'slide'
            });
            return false;
        }

        function Approval(Id, Name) {
            if (!confirm("Want to approve?")) {
                return false;
            }

            var iframeid = 'frmPrint';
            var url = "./frmAppraisalEvaluationBy.aspx?ae=" + Id + "&per=" + "approve";
            document.getElementById(iframeid).src = url;
            $("#AppraisalEvaluationDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 500,
                closeOnEscape: false,
                resizable: false,
                title: "Approve Appraisal Evaluation : " + Name,
                show: 'slide'
            });
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }

        function SearchInformation(pageNumber, IsCurrentOrPreviousPage) {


            var gridRecordsCount = AppraisalEvaluationTable.data().length;

            PageMethods.GetAppraisalEvalutionBy(gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSuccessLoading, OnFailLoading)
            return false;
        }

        function OnSuccessLoading(result) {
            FillForm(result);
            return false;
        }
        function FillForm(result) {
            //debugger;
            AppraisalEvaluationTable.clear();
            AppraisalEvaluationTable.rows.add(result.GridData);
            AppraisalEvaluationTable.draw();

            $("#GridPagingContainer ul").html("");
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }
        function OnFailLoading(error) {
            // debugger;
            toastr.error(error.get_message());
            return false;
        }
    </script>

    <div class="panel panel-default">
        <div class="panel-heading">
            Appraisal Evaluation
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div id="AppraisalEvaluationDialouge" style="display: none;">
                    <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                        clientidmode="static" scrolling="yes"></iframe>
                </div>
                <div>
                    <table id="tblAppraisalEvaluation" class="table table-bordered table-condensed table-responsive">
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
