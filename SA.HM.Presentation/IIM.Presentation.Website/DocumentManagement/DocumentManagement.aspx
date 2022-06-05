<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="DocumentManagement.aspx.cs" Inherits="HotelManagement.Presentation.Website.DocumentManagement.DocumentManagement" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var DocumentTable, isClose;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        var DocTable = "";
        $(document).ready(function () {
            CommonHelper.ApplyDecimalValidation();
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            $("#ContentPlaceHolder1_ddlDocumentType").change(function () {
                if ($(this).val() == "Sales") {
                    $("#DivSaleNo").show();
                    $("#DivProjectName").hide();
                }
                else if ($(this).val() == "Project") {
                    $("#DivSaleNo").hide();
                    $("#DivProjectName").show();
                }
                else if ($(this).val() == "Internal") {
                    $("#DivSaleNo").hide();
                    $("#DivProjectName").hide();
                }

            });
            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker();

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_ddlSearchAssignTo").select2();
            DocumentTable = $("#tblDocument").DataTable({
                data: [],
                columns: [
                    { title: "Document Name", "data": "DocumentName", sWidth: '30%' },
                    { title: "Description", "data": "Description", sWidth: '35%' },
                    { title: "Create Date", "data": "CreatedDate", sWidth: '20%' },
                    { title: "Action", "data": null, sWidth: '15%' },
                    { title: "", "data": "Id", visible: false }
                ],
                columnDefs: [
                    {
                        "targets": 3,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            var status;
                            if (data == true)
                                img = 'Done';
                            else
                                img = 'Pending';
                            return img;
                        }
                    },
                    {
                        "targets": 2,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            return moment(data).format("DD/MM/YYYY");
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
                    if (/*IsCanEdit &&*/ !aData.IsCompleted)
                        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEdit('" + aData.Id + "','" + aData.DocumentName + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    if (/*IsCanDelete &&*/ !aData.IsCompleted)
                        row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"DeleteDocument('" + aData.Id + "');\"> <img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                    row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"ShowDocument('" + aData.Id + "',false);\"> <img alt='Document' src='../Images/document.png' title='document' /></a>";

                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);
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

            $("#btnClear").show();
            SearchDocument(1, 1);
        });
        function CreateNew() {
            var iframeid = 'frmPrint';
            var url = "./AddNewDocumentIFrame.aspx?docid=" + "0";
            document.getElementById(iframeid).src = url;
            $("#NewDocumentDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "Document Management",
                show: 'slide'
            });
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchDocument(pageNumber, IsCurrentOrPreviousPage);
        }
        function SearchDocument(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = DocumentTable.data().length;

            var documentName = $("#ContentPlaceHolder1_txtDocumentNameForSearch").val();
            var fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();
            if (fromDate == "") {                
                fromDate = "01 / 01 / 1901";                
            }
            if (toDate == "")
                toDate = new Date();
            //if (filterBy == "Custom" || filterBy == "Overdue" || filterBy == "Completed") {
            fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');
            toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            var assignToId = $("#ContentPlaceHolder1_ddlSearchAssignTo").val();
            var assignToId = $("#ContentPlaceHolder1_hfSelectedEmpIdForSearch").val(assignToId).val();

            PageMethods.GetDocumentListBySearchCriteria(documentName, fromDate, toDate, assignToId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnDocumentLoadingSucceed, OnDocumentLoadingFailed);
            return false;
        }

        function OnDocumentLoadingSucceed(result) {

            DocumentTable.clear();
            DocumentTable.rows.add(result.GridData);
            DocumentTable.draw();

            $("#GridPagingContainer ul").html("");
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }

        function OnDocumentLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }
        function PerformEdit(Id, name) {
            if (!confirm("Want to edit - " + name + "?")) {
                return false;
            }
            var iframeid = 'frmPrint';
            var url = "./AddNewDocumentIFrame.aspx?docid=" + Id;
            document.getElementById(iframeid).src = url;
            $("#NewDocumentDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Document Management",
                show: 'slide'
            });
        }
        function ShowDocument(id) {
            PageMethods.LoadDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            $("#imageDiv").html(result);

            $("#DocumentDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: "70%",
                height: 300,
                closeOnEscape: true,
                resizable: false,
                title: "Document",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }
        function DeleteDocument(id) {
            if (confirm("Want to delete?")) {
                PageMethods.DeleteDocument(id, OnSuccessDelete, OnFailedDelete);
            }
            return false;
        }

        function OnSuccessDelete(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#ContentPlaceHolder1_btnSearch").trigger("click");
                GridPaging(1, 1);
            }
            return false;
        }

        function OnFailedDelete(error) {
            toastr.error(error.get_message());
            return false;
        }
        function CloseDialog() {
            $("#NewDocumentDialog").dialog('close');
            return false;
        }
    </script>
    <asp:HiddenField ID="hfSelectedEmpIdForSearch" runat="server" Value="0" />
    <div id="DocumentDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <div id="NewDocumentDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Document Assignment
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Document Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtDocumentNameForSearch" runat="server" CssClass="form-control">                            
                        </asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Assign To</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSearchAssignTo" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;"></asp:DropDownList>
                    </div>
                </div>
                <div id="dvSearchDateTime" class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">From Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">To Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return SearchDocument(1,1);" />
                        <asp:Button ID="btnCreateNew" runat="server" TabIndex="4" Text="Create New" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-heading">
            Document            
        </div>
        <div class="panel-body">
            <table id="tblDocument" class="table table-bordered table-condensed table-responsive">
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfDocumentId" Value="0" runat="server" />
    <div id="popUpImage" style="display: none">
        <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
            <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
        </asp:Panel>
    </div>
</asp:Content>
