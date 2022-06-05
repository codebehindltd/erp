<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="CashRequisitionApprovalDetailsIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.CashRequisitionApprovalDetailsIframe" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>

        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        var isClose;
        var GlobalProjectId = 0;
        var CashRequisitionAdjustmentTable;
        var RequisitionForBillVoucherTable;

        var GlobalDeletedId = 0;
        var GlobalCreatedBy = 0;
        var CreatedDate;
        $(document).ready(function () {

            var cashRequsitionId = $.trim(CommonHelper.GetParameterByName("crid"));
            var createdBy = $.trim(CommonHelper.GetParameterByName("cbid"));
            $('#ContentPlaceHolder1_hfCashRequsitionId').val(cashRequsitionId);

            $('#ContentPlaceHolder1_hfcreatedById').val(createdBy);
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            $("#ContentPlaceHolder1_ddlGLCompany").change(function () {

                // debugger;
                var CompanyId = parseInt($("#ContentPlaceHolder1_ddlGLCompany").val().trim());

                PageMethods.LoadProjectByCompanyId(CompanyId, OnLoadProjectByCompanyIdSucceed, OnLoadProjectByCompanyIdFailed);


                return false;

            });
            $("#ContentPlaceHolder1_ddlAssignEmployee").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            if ($("#ContentPlaceHolder1_hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects").val() == '0') {

                RequisitionForBillVoucherTable = $("#RequisitionForBillVoucherTbl").DataTable({
                    data: [],
                    columns: [
                        { title: "", "data": "DetailId", visible: false },
                        { title: "Particulars", "data": "RequisitionForHeadName", sWidth: '20%' },
                        { title: "Amount", "data": "RequsitionAmount", sWidth: '20%' },
                        { title: "Purpose", "data": "IndividualRemarks", sWidth: '20%' }
                    ],
                    fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                        var row = '';
                        if (iDisplayIndex % 2 == 0) {
                            $('td', nRow).css('background-color', '#E3EAEB');
                        }
                        else {
                            $('td', nRow).css('background-color', '#FFFFFF');
                        }

                    },
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
            }
            else {
                RequisitionForBillVoucherTable = $("#RequisitionForBillVoucherTbl").DataTable({
                    data: [],
                    columns: [
                        { title: "", "data": "DetailId", visible: false },
                        { title: "Particulars", "data": "RequisitionForHeadName", sWidth: '20%' },
                        { title: "Amount", "data": "RequsitionAmount", sWidth: '20%' },
                        { title: "Company", "data": "IndividualCompanyName", sWidth: '20%' },
                        { title: "Project", "data": "IndividualProjectName", sWidth: '20%' },
                        { title: "Purpose", "data": "IndividualRemarks", sWidth: '20%' }
                    ],
                    fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                        var row = '';
                        if (iDisplayIndex % 2 == 0) {
                            $('td', nRow).css('background-color', '#E3EAEB');
                        }
                        else {
                            $('td', nRow).css('background-color', '#FFFFFF');
                        }

                    },
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
            }

            
            CashRequisitionAdjustmentTable = $("#tblCashRequisitionAdjustment").DataTable({
                data: [],
                columns: [
                    { title: "Date", "data": "CreatedDate", sWidth: '10%' },
                    { title: "Amount", "data": "Amount", sWidth: '10%' },
                    //{ title: "Employee Name", "data": "EmployeeName", sWidth: '20%' },                    
                    { title: "Status", "data": "ApprovedStatus", sWidth: '10%' },
                    { title: "Authorized By", "data": "AuthorizedByList", sWidth: '50%' },
                    { title: "Action", "data": null, sWidth: '30%' },
                    { title: "", "data": "Id", visible: false }
                ],
                columnDefs: [
                    {
                        "targets": 0,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            //debugger;
                            return CommonHelper.DateFromDateTimeToDisplay(data, innBoarDateFormat);
                        }
                    }],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    //debugger;
                    //$('td:eq(0)').val(CommonHelper.DateFormatDDMMMYYY(aData.CreatedDate));
                    //$('td:eq(0)').val(CommonHelper.DateTimeClientFormatWiseConversionForDisplay(aData.CreatedDate));
                    //debugger;
                    //$('td:eq(' + (0) + ')', nRow).html(CommonHelper.DateFromStringToDisplay(aData.CreatedDate, innBoarDateFormat));
                    CreatedDate = aData.CreatedDate;
                    if (IsCanEdit && aData.IsCanEdit && (aData.ApprovedStatus == "Pending" || aData.IsCanCheck || aData.IsCanApprove)) {
                        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEditOperation('" + aData.Id + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";

                    }
                    if (aData.IsCanDelete && IsCanDelete && (aData.ApprovedStatus == "Pending" || aData.IsCanCheck || aData.IsCanApprove)) {
                        row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"DeleteCashRequisition('" + aData.Id + "','" + aData.CreatedBy + "','" + aData.TransactionNo + "','" + aData.TransactionType + "');\"> <img alt='Cancel' src='../Images/delete.png' title='Cancel' /></a>";
                    }
                    if ( aData.IsCanCheck) {
                        row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return Approval('" + aData.IsCanCheck + "','" + aData.IsCanApprove + "','" + aData.Id + "','" + aData.ApprovedStatus + "','" + aData.Amount + "');\"><img alt=\"Check\" src=\"../Images/checked.png\" title='Check' /></a>";
                    }
                    else if ( aData.IsCanApprove) {
                        row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return Approval('" + aData.IsCanCheck + "','" + aData.IsCanApprove + "','" + aData.Id + "','" + aData.ApprovedStatus + "','" + aData.Amount + "');\"><img alt=\"Approve\" src=\"../Images/approved.png\" title='Approve' /></a>";

                    }


                    row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return viewCashRequisitionAdjustment('" + aData.Id + "');\"><img alt=\"Details\" src=\"../Images/detailsInfo.png\" title='Details' /></a>";
                    if (aData.ApprovedStatus != 'Cancel') {
                        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ShowInvoice('" + aData.Id + "');\"> <img alt=\"Invoice\" src=\"../Images/ReportDocument.png\" title='Invoice' /> </a>";
                    }

                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);

                    //$('td', row).eq(1).append(CommonHelper.DateTimeClientFormatWiseConversionForDisplay(aData.CreatedDate));


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

        function ShowInvoice(id) {
            var iframeid = 'frmPrint';
            var url = "../payroll/reports/CashRequsitionInvoice.aspx?Id=" + id;
            document.getElementById(iframeid).src = url;
            $("#RequsitionDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 500,
                closeOnEscape: false,
                resizable: false,
                title: "Cash Requisition Adjustment Invoice",
                show: 'slide'
            });
            return false;
        }

        function viewCashRequisitionAdjustment(id) {

            //debugger;
            $("#AdjustmentDetailsDiv").show();
            $("#ContentPlaceHolder1_hfId").val(id);
            $("#ContentPlaceHolder1_txtAdjustmentDate").val("");
            $("#ContentPlaceHolder1_txtAdjustmentBalance").val("");
            RequisitionForBillVoucherTable.clear();
            PageMethods.GetRequsitionById(id, OnSuccessGet, OnfailedGet);

            return false;
        }
        function OnSuccessGet(result) {
            //debugger;
            $("#AdjustmentDetailsDiv").show();
            $("#ContentPlaceHolder1_txtAdjustmentDate").val(CommonHelper.DateFormatDDMMYYY(result[0].CreatedDate));
            $("#ContentPlaceHolder1_txtAdjustmentBalance").val(result[0].Amount);
            RequisitionForBillVoucherTable.clear();
            RequisitionForBillVoucherTable.rows.add(result);
            RequisitionForBillVoucherTable.draw();
            GetDocuments();
            return false;
        }
        function OnfailedGet(error) {
            //debugger;
            toastr.error(error.get_message());
            return false;
        }



        function Approval(isCanCheck, isCanApprove, id, status, approvalAmount) {
            //debugger;
            

            var temp = "";
            var updatedStatus = "";
            if (isCanCheck == "true") {
                 temp = "check";
                 updatedStatus = "Checked";
            }
            else if (isCanApprove == "true") {
                 temp = "approve";
                 updatedStatus = "Approved";
            }

            var remainingAmount = $("#ContentPlaceHolder1_txtRemainingBalance").val();

            //if (parseFloat(approvalAmount) > parseFloat(remainingAmount)) {
            //    toastr.warning('You can not check or approve greater than remaining amount');
            //    return false;
            //}

            if (confirm("Want to " + temp + "?")) {

                

                PageMethods.ApprovalStatusUpdate(id, updatedStatus, OnApprovalSucceed, OnFailed);
            }

            return false;
        }

        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                    data: JSON.stringify({ tableName: 'CashRequisition', primaryKeyName: 'Id', primaryKeyValue: result.PrimaryKeyValue, featuresValue: 'CashRequisition', statusColumnName: 'ApproveStatus' }),
                    dataType: "json",
                    success: function (data) {

                        SendSMSToUserList(data.d, result.PrimaryKeyValue, result.TransactionNo, result.TransactionType, result.TransactionStatus);

                    },
                    error: function (result) {
                        toastr.error("Can not load Check or Approve By List.");
                    }
                });
                //Clear();
                SearchInformation(1, 1);
            }
            return false;
        }

        function SendSMSToUserList(UserList, PrimaryKeyValue, TransactionNo, TransactionType, TransactionStatus) {

            var str = '';
            if (TransactionStatus == 'Approved') {
                str += TransactionType + ' No.(' + TransactionNo + ')  is Approved.';
            }
            else {
                str += TransactionType + ' No.(' + TransactionNo + ') is waiting for your Approval Process.';
            }

            var CommonMessage = {
                Subjects: str,
                MessageBody: str
            };
            debugger;

            var messageDetails = [];
            if (UserList.length > 0) {

                for (var i = 0; i < UserList.length; i++) {
                    messageDetails.push({
                        MessageTo: UserList[i]
                    });
                }

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/HMCommon/frmCommonMessage.aspx/SendMailByID',
                    data: JSON.stringify({ CMB: CommonMessage, CMD: messageDetails }),
                    dataType: "json",
                    success: function (data) {

                        //CommonHelper.AlertMessage(data.d.AlertMessage);

                    },
                    error: function (result) {
                        //alert("Error");

                    }
                });

            }

            return false;
        }
        function OnFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function DeleteCashRequisition(id, createdBy, TransactionNo, TransactionType) {
            if (confirm("Want to Cancel?")) {


                GlobalDeletedId = id;
                GlobalCreatedBy = createdBy;

                $("#ReasonDialogue").dialog({
                    autoOpen: true,
                    modal: true,
                    width: "83%",
                    height: 400,
                    closeOnEscape: false,
                    resizable: false,
                    title: "Cancel : " + TransactionType + " (" + TransactionNo + ") ",
                    show: 'slide'
                });

                $("#ContentPlaceHolder1_txtTitle").val(TransactionType + " (" + TransactionNo + ") ");
                $("#ContentPlaceHolder1_txtReason").val("");


            }

            return false;
        }
        function PerformDelete() {
            var reason = $("#ContentPlaceHolder1_txtReason").val();
            if (reason == "") {
                toastr.warning("Give Some Reason.");
                $("#ContentPlaceHolder1_txtReason").focus();
                return false;
            }

            PageMethods.DeleteCashRequisition(GlobalDeletedId, OnSuccessDelete, OnFailedDelete);
        }

        function PerformDeleteCancel() {
            GlobalDeletedId = 0;
            GlobalCreatedBy = 0;
            $('#ReasonDialogue').dialog('close');
        }

        function OnSuccessDelete(result) {
            if (result.IsSuccess) {
                debugger;

                var CommonMessage = {
                    Subjects: $("#ContentPlaceHolder1_txtTitle").val() + " has been canceled.",
                    MessageBody: $("#ContentPlaceHolder1_txtReason").val()
                };

                var messageDetails = [];

                messageDetails.push({
                    MessageTo: GlobalCreatedBy
                });

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/HMCommon/frmCommonMessage.aspx/SendMailByID',
                    data: JSON.stringify({ CMB: CommonMessage, CMD: messageDetails }),
                    dataType: "json",
                    success: function (data) {

                        CommonHelper.AlertMessage(data.d.AlertMessage);

                    },
                    error: function (result) {
                        //alert("Error");

                    }
                });

                PerformDeleteCancel();

                CommonHelper.AlertMessage(result.AlertMessage);
                //Clear();
                SearchInformation(1, 1);
            }
            return false;
        }
        function PerformEditOperation(AdjustmentDetailsId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            //debugger;

            var Id = $('#ContentPlaceHolder1_hfCashRequsitionId').val();
            var iframeid = 'frmPrint';
            var url = "./CashRequisitionApprovalIframe.aspx?craid=" + Id + "&craeid=" + AdjustmentDetailsId;
            document.getElementById(iframeid).src = url;
            $("#RequsitionDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 800,
                closeOnEscape: false,
                resizable: false,
                title: "Edit Requisition Cash Adjustment",
                show: 'slide'
            });
            return false;
        }


        function PerformAdustmentWithReturn() {
            if (!confirm("Want to Adjust With Return?")) {
                return false;
            }
            debugger;
            var Id = $('#ContentPlaceHolder1_hfCashRequsitionId').val();
            PageMethods.AdjustmentWithReturn(Id, OnSuccessPerformAdustmentWithReturn, OnFailedPerformAdustmentWithReturn);
            return false;
        }
        function OnSuccessPerformAdustmentWithReturn(result) {
            debugger;
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                //Clear();
                SearchInformation(1, 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnFailedPerformAdustmentWithReturn(error) {
            debugger;
            toastr.error(error.get_message());
            return false;
        }

        function PerformAdustment() {
            if (!confirm("Want to Adjust?")) {
                return false;
            }
            var Id = $('#ContentPlaceHolder1_hfCashRequsitionId').val();
            var iframeid = 'frmPrint';
            var url = "./CashRequisitionApprovalIframe.aspx?craid=" + Id + "&craeid=" + "";
            document.getElementById(iframeid).src = url;
            $("#RequsitionDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 800,
                closeOnEscape: false,
                resizable: false,
                title: "Requisition Cash Adjustment",
                show: 'slide'
            });
            return false;
        }

        function OnFailedDelete(error) {
            toastr.error(error.get_message());
            return false;
        }
        function SearchInformation(pageNumber, IsCurrentOrPreviousPage) {
            // debugger;
            $("#AdjustmentDetailsDiv").hide();
            var id = $('#ContentPlaceHolder1_hfCashRequsitionId').val();

            var createdById = $('#ContentPlaceHolder1_hfcreatedById').val();

            var gridRecordsCount = CashRequisitionAdjustmentTable.data().length;
            PageMethods.GetCashRequsitionById(id, OnSuccessGetCashRequsitionByIdLoading, OnFailGetCashRequsitionByIdLoading);

            PageMethods.GetALLAdjustmentForCashRequisitionById(id, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSuccessLoading, OnFailLoading)
            return false;
        }


        function OnSuccessGetCashRequsitionByIdLoading(Result) {

            $("#ContentPlaceHolder1_ddlGLCompany").val(Result.CompanyId).change();
            //$("#ContentPlaceHolder1_ddlGLProject").val(Result.ProjectId);
            $("#ContentPlaceHolder1_ddlAssignEmployee").val(Result.EmployeeId).change();
            $("#ContentPlaceHolder1_txtRemarks").val(Result.Remarks);
            $("#ContentPlaceHolder1_txtAmount").val(Result.Amount);
            GlobalProjectId = Result.ProjectId;
            $("#ContentPlaceHolder1_txtRemainingBalance").val(Result.RemainingAmount);
            debugger;
            if (Result.RemainingAmount > 0) {
                var createdById = $('#ContentPlaceHolder1_hfcreatedById').val();

                PageMethods.GetFeatureWisePermission(createdById, OnSuccessGetFeatureWisePermissionLoading, OnFailGetFeatureWisePermissionLoading);

                $("#btnAdjust").show();
                
                
            }
            else {
                $("#btnAdjust").hide();
                $("#btnAdjustReturn").hide();
            }

            return false;
        }
        function OnFailGetCashRequsitionByIdLoading(error) {
            //debugger;
            toastr.error(error.get_message());
            return false;
        }

        function OnSuccessGetFeatureWisePermissionLoading(Result) {


            debugger;
            if (Result.IsCanApprove == 1) {
                $('#ContentPlaceHolder1_hfIsCanApprove').val('1');
                //$("#btnAdjustReturn").show();
                $("#btnAdjustReturn").hide();
            }
            else {
                $('#ContentPlaceHolder1_hfIsCanApprove').val('0');
                $("#btnAdjustReturn").hide();
            }

            if (Result.IsCanCheck == 1) {
                $('#ContentPlaceHolder1_hfIsCanCheck').val('1');
            }
            else {
                $('#ContentPlaceHolder1_hfIsCanCheck').val('0');
            }

            return false;
        }
        function OnFailGetFeatureWisePermissionLoading(error) {
            //debugger;
            toastr.error(error.get_message());
            return false;
        }
        function ShowAlert(Message, PrimaryKeyValue, TransactionNo, TransactionType, TransactionStatus) {
            debugger;
            CommonHelper.AlertMessage(Message);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                data: JSON.stringify({ tableName: 'CashRequisition', primaryKeyName: 'Id', primaryKeyValue: PrimaryKeyValue, featuresValue: 'CashRequisition', statusColumnName: 'ApproveStatus' }),
                dataType: "json",
                success: function (data) {

                    SendSMSToUserList(data.d, PrimaryKeyValue, TransactionNo, TransactionType, TransactionStatus);

                },
                error: function (result) {
                    toastr.error("Can not load Check or Approve By List.");
                }
            });
        }
        function OnLoadProjectByCompanyIdSucceed(result) {

            //debugger;
            typesList = [];
            $("#ContentPlaceHolder1_ddlGLProject").empty();
            var i = 0, fieldLength = result.length;

            if (fieldLength > 0) {
                typesList = result;
                $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddlGLProject');
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + result[i].ProjectId + '">' + result[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlGLProject');
                }

                if (GlobalProjectId > 0) {

                    $("#ContentPlaceHolder1_ddlGLProject").val(GlobalProjectId).change();
                    GlobalProjectId = 0;
                }
                //$("#ContentPlaceHolder1_ddlGLProject").attr("Enabled", true);
            }
            else {
                $("<option value='0'>--No Projects Found--</option>").appendTo("#ContentPlaceHolder1_ddlGLProject");
                var company = $("#ContentPlaceHolder1_ddlGLCompany").val();
                //$("#ContentPlaceHolder1_ddlGLProject").attr("Enabled", false);


            }

            return false;

        }

        function OnLoadProjectByCompanyIdFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }


        function OnSuccessLoading(result) {
            FillForm(result);
            return false;
        }
        function FillForm(result) {
            //debugger;
            CashRequisitionAdjustmentTable.clear();
            CashRequisitionAdjustmentTable.rows.add(result.GridData);
            CashRequisitionAdjustmentTable.draw();

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
        function CloseDialog() {
            $("#RequsitionDialouge").dialog('close');
            return false;
        }
        function GetDocuments() {
            var id = $("#ContentPlaceHolder1_hfId").val();
            PageMethods.GetRequsitionDocumentsById(id, LoadAttachments, LoadAttachmentsFailed);
            return false;
        }
        function LoadAttachments(result) {
            var guestDoc = result;

            //if (result.length > 0)
            //    $("#btnAttachment").val("Add Another Attachment");
            //else
            //    $("#btnAttachment").val("Add Attachment");

            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='DocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th>"; // <th align='left' scope='col'>Action</th></tr>
            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                //guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";
                guestDocumentTable += "<td align='left' style='width: 50%'>";
                guestDocumentTable += "<a style='color:#333333;' target='_blank' href='" + guestDoc[row].Path + guestDoc[row].Name + "'>"
                guestDocumentTable += guestDoc[row].Name + "</a></td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png" || guestDoc[row].Extention == ".PNG" || guestDoc[row].Extention == ".JPG")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

                //guestDocumentTable += "<td align='left' style='width: 20%'>";
                //guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer;\" onClick=\"javascript:return DeleteDealDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Document' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            
            guestDocumentTable += "</table>";

            // docc = guestDocumentTable;

            $("#DocumentInfo").html(guestDocumentTable);
        }
        function LoadAttachmentsFailed(error) {
            toastr.warning(error);
            return false;
        }

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfCashRequsitionId" Value="0" runat="server" />
    <asp:HiddenField ID="hfcreatedById" Value="0" runat="server" />
    <asp:HiddenField ID="hfIsCanCheck" Value="0" runat="server" />
    <asp:HiddenField ID="hfIsCanApprove" Value="0" runat="server" />
    <asp:HiddenField ID="hfId" Value="0" runat="server" />
    <asp:HiddenField ID="hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects" runat="server" Value="0" />

    <div id="ReasonDialogue" class="panel panel-default" style="display: none">
        <div class="panel-body">
            <div class="form-horizontal">

                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label " Text="Delete"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblReason" runat="server" class="control-label required-field" Text="Reason"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtReason" runat="server" Height="170px" TextMode="multiline" CssClass="form-control"
                            TabIndex="8">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="row" id="saveBtnDiv">
                    <div class="col-md-12">
                        <input type="button" value="Delete" style="width: 100px;" id="btnSave" onclick="PerformDelete()" class="TransactionalButton btn btn-primary btn-sm" />
                        &nbsp;
                        <input type="button" value="Close" style="width: 100px;" id="btnClear" onclick="PerformDeleteCancel()" class="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="RequsitionDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="col-md-6">
        <div id="SearchPanel" class="panel panel-default">
            <div class="panel-heading">Adjustment Information</div>
            <div class="panel-body">

                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblGLCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control" Enabled="false">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblGLProject" runat="server" class="control-label" Text="Project"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlGLProject" runat="server" CssClass="form-control" Enabled="false">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblAssignEmployee" runat="server" class="control-label" Text="Employee"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlAssignEmployee" CssClass="form-control" runat="server" Enabled="false">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Amount</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="quantitydecimal form-control" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Purpose</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox TextMode="MultiLine" ID="txtRemarks" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                    </div>

                </div>

                <table id="tblCashRequisitionAdjustment" class="table table-bordered table-condensed table-responsive">
                </table>
                <div class="childDivSection">
                    <div class="text-center" id="GridPagingContainer">
                        <ul class="pagination">
                        </ul>
                    </div>
                </div>

                <div>
                    <div class="form-group">
                        <div class="col-md-4">
                            <label class="control-label">Remaining Balance</label>
                        </div>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtRemainingBalance" runat="server" CssClass="quantitydecimal form-control" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                    <div class="col-md-12">
                        <input id="btnAdjust" type="button" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="PerformAdustment()" value="New Adjustment" />
                        <input id="btnAdjustReturn" type="button" style="display:none;" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="PerformAdustmentWithReturn()" value="Adjustment With Return" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-md-6">
        <div class="panel panel-default" style="height: 600px; overflow-y: scroll;">
            <div class="panel-heading">
                Adjustment Details
            </div>
            <div class="panel-body">
                <div class="form-horizontal" id="AdjustmentDetailsDiv" style="display: none">
                    <div>
                        <div class="form-group">
                            <div class="col-md-4">
                                <label class="control-label">Adjustment Date</label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtAdjustmentDate" runat="server" CssClass="quantitydecimal form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-4">
                                <label class="control-label ">Adjustment Balance</label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtAdjustmentBalance" runat="server" CssClass="quantitydecimal form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <table id="RequisitionForBillVoucherTbl" class="table table-bordered table-condensed table-hover">
                    </table>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Attachments
                        </div>
                        <div class="panel-body">
                            <div id="DocumentInfo" style="overflow-x: scroll;">
                            </div>
                           <%-- <div class="row" style="text-align: right">
                                <div class="col-md-12">
                                    <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Add Attachment" onclick="AttachFile()" />
                                </div>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
