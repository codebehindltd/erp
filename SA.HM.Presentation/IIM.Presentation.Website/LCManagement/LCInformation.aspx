<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="LCInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.LCManagement.LCInformation" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var LCInformationTable;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;

        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlSupplier").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                // defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;

            LCInformationTable = $("#tblLCInformation").DataTable({
                data: [],
                columns: [
                    { title: "LC Number", "data": "LCNumber", sWidth: '20%' },
                    { title: "LC Type", "data": "LCTypes", sWidth: '15%', className:'text-center' },
                    { title: "Open Date", "data": "LCOpenDate", sWidth: '15%' },
                    { title: "LC Value ($)", "data": "LCValue", sWidth: '20%' }, 
                    { title: "Status", "data": "ApprovedStatus", sWidth: '15%', className: 'text-center' },
                    { title: "Action", "data": null, sWidth: '15%' },
                    { title: "", "data": "LCId", visible: false }
                ],
                columnDefs: [
                    {
                        "targets": 2,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            //debugger;
                            return CommonHelper.DateFormatDDMMYYY(data);
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
                    if (IsCanEdit && aData.IsCanEdit) {
                        
                            row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformLCEdit('" + aData.LCId + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";

                    
                    }
                    if (aData.IsCanDelete && IsCanDelete) {
                        row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"DeleteLCInformation('" + aData.LCId + "');\"> <img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                    }
                    if ( aData.IsCanCheck) {
                        row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return Approval('" + aData.IsCanCheck + "','" + aData.IsCanApprove + "','" + aData.LCId + "','" + aData.ApprovedStatus + "');\"><img alt=\"Check\" src=\"../Images/checked.png\" title='Check' /></a>";
                    }
                    else if ( aData.IsCanApprove) {
                        row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return Approval('" + aData.IsCanCheck + "','" + aData.IsCanApprove + "','" + aData.LCId + "','" + aData.ApprovedStatus + "');\"><img alt=\"Approve\" src=\"../Images/approved.png\" title='Approve' /></a>";

                    }
                    row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ShowDocuments('" + aData.LCId + "');\"> <img alt=\"Documents\" src=\"../Images/document.png\" title='Documents' /> </a>";
                    row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ShowReport('" + aData.LCId + "');\"> <img alt=\"LC Information\" src=\"../Images/ReportDocument.png\" title='LC Information' /> </a>";

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
            var single = $("#ContentPlaceHolder1_hfIsSingle").val();
            if (single == "1") {
                $('#CompanyProjectPanel').hide();
                //$('#SearchTypePanel').show();
            }
            else {
                $('#CompanyProjectPanel').show();
                //$('#SearchTypePanel').hide();
            }
            
        });

        function ShowReport(lcId) {
            var iframeid = 'printDoc';
            var url = "Reports/frmLCInformation.aspx?LCId=" + lcId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 1050,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "LC Information",
                show: 'slide'
            });
        }

        function Approval(isCanCheck, isCanApprove, id, status) {

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

            

            if (confirm("Want to " + temp + "?")) {

                

                PageMethods.ApprovalStatusUpdate(id, updatedStatus, OnApprovalSucceed, OnFailed);
            }

            return false;
        }

        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                //Clear();

                if ($("#ContentPlaceHolder1_hfPageNumber").val() == "") {
                    GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
                }
                else {
                    var pageNumber = $("#ContentPlaceHolder1_hfPageNumber").val();
                    GridPaging(pageNumber, 1);
                }

                //$("#ContentPlaceHolder1_btnSearch").trigger("click");
            }
            return false;
        }

        function OnFailed(error) {
            toastr.error(error.get_message());
            return false;
        }
        function DeleteLCInformation(id) {
            if (confirm("Want to delete?")) {
                PageMethods.DeleteLCInformation(id, OnSuccessDelete, OnFailedDelete);
            }
            return false;
        }

        function OnSuccessDelete(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                //Clear();
                $("#ContentPlaceHolder1_btnSearch").trigger("click");
            }
            return false;
        }

        function OnFailedDelete(error) {
            toastr.error(error.get_message());
            return false;
        }

        function ShowDocuments(id) {
            PageMethods.LoadDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }

        function OnLoadDocumentSucceeded(result) {
            $("#imageDiv").html(result);

            $("#showDocumentsDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function CreateNewLCInformation() {
            var iframeid = 'frmPrint';
            var url = "./NewLCIframe.aspx?lci=" + "";
            document.getElementById(iframeid).src = url;
            $("#LCInformationDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "LC Information",
                show: 'slide'
            });
            return false;
        }

        function PerformLCEdit(Id) {
            if (!confirm("Do You Want To Edit?")) {
                return false;
            }


            var iframeid = 'frmPrint';
            var url = "./NewLCIframe.aspx?lci=" + Id;
            document.getElementById(iframeid).src = url;
            $("#LCInformationDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "Edit LC Information",
                show: 'slide'
            });
            return false;;
        }
        function CloseDialog() {
            $("#LCInformationDialouge").dialog('close');
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function SearchInformation(pageNumber, IsCurrentOrPreviousPage) {
            //debugger;
            var gridRecordsCount = LCInformationTable.data().length;
            $("#ContentPlaceHolder1_hfPageNumber").val(pageNumber);
            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            if (projectId == null)
                projectId = "0";
            
            var SupplierId = parseInt($("#ContentPlaceHolder1_ddlSupplier").val().trim());
            var LCNumber = $("#ContentPlaceHolder1_txtLCNumber").val();
            var PINumber = $("#ContentPlaceHolder1_txtPINumber").val();

            var fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();
            if (fromDate == "")
                fromDate = new Date();
            if (toDate == "")
                toDate = new Date();

            fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');
            toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            PageMethods.GetLCInformation(companyId, projectId, SupplierId, LCNumber, PINumber, fromDate, toDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLCInformationLoadingSucceed, OnLCInformationLoadingFailed);
            return false;
        }

        function OnLCInformationLoadingSucceed(result) {
            //debugger;
            LCInformationTable.clear();
            LCInformationTable.rows.add(result.GridData);
            LCInformationTable.draw();

            $("#GridPagingContainer ul").html("");
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }

        function OnLCInformationLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchInformation(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
    </script>    
    <asp:HiddenField ID="hfPageNumber" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsSingle" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="1000" height="800" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>
    <div id="showDocumentsDiv" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <div id="LCInformationDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            LC Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group col-md-12" id="CompanyProjectPanel">                    
                     <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Supplier</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label ">LC Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtLCNumber" runat="server" CssClass=" form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">PI Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPINumber" runat="server" CssClass=" form-control"></asp:TextBox>
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
            </div>

            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                         OnClientClick="javascript: return SearchInformation(1,1);" />

                    <asp:Button ID="btnCreateNewLCInformation" runat="server" TabIndex="4" Text="New LC Information" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return CreateNewLCInformation();" />

                </div>

            </div>
        </div>
        <div>
            <table id="tblLCInformation" class="table table-bordered table-condensed table-responsive">
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>

     <script type="text/javascript">
         $(document).ready(function () {
             if ($("#ContentPlaceHolder1_companyProjectUserControl_hfIsSingle").val() != "1") {

                 if ($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val() == "0") {
                     $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val()).trigger("change");
                 }
             }
         });
     </script>
</asp:Content>
