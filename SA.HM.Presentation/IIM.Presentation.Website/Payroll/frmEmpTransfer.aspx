<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmEmpTransfer.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpTransfer" %>

<%@ Register TagPrefix="UserControl" TagName="companyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithDesignation.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeForLeaveSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Transfer</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            $('#ContentPlaceHolder1_txtTransferDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtReportingDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtReportingDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtTransferDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $("#ContentPlaceHolder1_ddlReportingTo").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            //$("#ContentPlaceHolder1_ddlCompanyTo").select2({
            //    tags: "true",
            //    placeholder: "--- Please Select ---",
            //    allowClear: true,
            //    width: "99.75%"
            //});
            $("#ContentPlaceHolder1_ddlReportingTo2").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlDepartmentTo").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlDesignationTo").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $('#ContentPlaceHolder1_txtDateFrom').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtDateTo').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtDateTo').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtDateFrom').datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#<%=txtDateFrom.ClientID %>").blur(function () {
                var date = $("#<%=txtDateFrom.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtDateFrom.ClientID %>").focus();
                        $("#<%=txtDateFrom.ClientID %>").val("");
                        return false;
                    }
                }
            });
            $("#<%=txtDateTo.ClientID %>").blur(function () {
                var date = $("#<%=txtDateTo.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtDateTo.ClientID %>").focus();
                        $("#<%=txtDateTo.ClientID %>").val("");
                        return false;
                    }
                }
            }); $("#<%=txtTransferDate.ClientID %>").blur(function () {
                var date = $("#<%=txtTransferDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtTransferDate.ClientID %>").focus();
                        $("#<%=txtTransferDate.ClientID %>").val("");
                        return false;
                    }
                }
            }); $("#<%=txtReportingDate.ClientID %>").blur(function () {
                var date = $("#<%=txtReportingDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtReportingDate.ClientID %>").focus();
                        $("#<%=txtReportingDate.ClientID %>").val("");
                        return false;
                    }
                }
            });

            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            console.log(companyId);
            var projectId = 0;
            $("#<%=hfGLCompanyId.ClientID %>").val(companyId);

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").change(function () {
                companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                $("#<%=hfGLCompanyId.ClientID %>").val(companyId);
            });

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").change(function () {
                projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
                $("#<%=hfGLProjectId.ClientID %>").val(projectId);
                console.log(projectId);
            });
        });

        function WorkAfterSearchEmployee() {

        }

        function PerformClearAction() {
            if (!confirm("Do You Want to Clear?"))
                return false;
            $("#<%=ddlDepartmentTo.ClientID %>").val("0");
            $("#<%=ddlDesignationTo.ClientID %>").val("0");
            $("#<%=ddlReportingTo.ClientID %>").val("0").trigger("change");
            $("#<%=txtTransferDate.ClientID %>").val("");
            $("#<%=txtReportingDate.ClientID %>").val("");
            $("#<%=hfTransferId.ClientID %>").val("");
            $("#<%=btnSave.ClientID %>").text("Save");
            $("#ContentPlaceHolder1_employeeForLeaveSearch_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_employeeeSearch_txtSearchEmployee").val("");
            $("#ContentPlaceHolder1_employeeeSearch_txtEmployeeName").val("");
            $("#ContentPlaceHolder1_employeeeSearch_txtEmpDepart").val("");
            $("#ContentPlaceHolder1_employeeeSearch_txtEmpDesig").val("");
            return false;
        }
        function PerformSearchClear() {
            if (!confirm("Do You Want to Clear?"))
                return false;
            $("#ContentPlaceHolder1_employeeForLeaveSearch_ddlEmployee").val("0");
            $("#ContentPlaceHolder1_employeeForLeaveSearch_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_employeeForLeaveSearch_txtSearchEmployee").val("");
            $("#ContentPlaceHolder1_employeeForLeaveSearch_txtEmployeeName").val("");
            $("#employeeSearchSection").hide();
            $("#<%=txtDateFrom.ClientID %>").val("");
            $("#<%=txtDateTo.ClientID %>").val("");
            return false;
        }

        function PerformSaveAction() {
            var a = $('#ContentPlaceHolder1_ddlDepartmentTo option:selected').text();
            var b = $('#ContentPlaceHolder1_employeeeSearch_txtEmpDepart').val();
            if (a == b) {
                toastr.warning("Cann't be transferred to the same department");
                return false;
            }
        }
        function PerformSearch() {
            var type = $("#ContentPlaceHolder1_employeeForLeaveSearch_ddlEmployee").val();
            var empId = $("#ContentPlaceHolder1_employeeForLeaveSearch_hfEmployeeId").val();
            if (type == "1") {
                if (empId == "0") {
                    toastr.warning("Please Insert Employee.");
                    return false;
                }
            }
            return true;
        }
        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/Payroll/Images/Transfer/";
            var category = "EmpTransferDocuments";
            var iframeid = 'frmPrint';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#DocumentDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 300,
                closeOnEscape: false,
                resizable: false,
                title: "Documents Upload",
                show: 'slide'
            });

        }

        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
        }
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            ShowUploadedDocument(randomId);
        }

        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_hfTransferId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            PageMethods.GetUploadedDocByWebMethod(randomId, id, deletedDoc, OnGetUploadedDocByWebMethodSucceeded, OnGetUploadedDocByWebMethodFailed);
            return false;
        }

        function OnGetUploadedDocByWebMethodSucceeded(result) {
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            DocTable = "";

            DocTable += "<table id='DocTableList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }
                DocTable += "<td align='left' style='width: 50%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

                if (result[row].Path != "") {
                    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                DocTable += "<td align='left' style='width: 20%'>";
                DocTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + result[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                DocTable += "</td>";
                DocTable += "</tr>";
            }
            DocTable += "</table>";

            docc = DocTable;

            $("#DocumentInfo").html(DocTable);

            return false;
        }
        function DeleteDoc(docId, rowIndex) {
            var deletedDoc = $("#<%=hfDeletedDoc.ClientID %>").val();

            if (deletedDoc != "")
                deletedDoc += "," + docId;
            else
                deletedDoc = docId;

            $("#<%=hfDeletedDoc.ClientID %>").val(deletedDoc);

            $("#trdoc" + rowIndex).remove();
        }
        function OnGetUploadedDocByWebMethodFailed(error) {
            alert(error.get_message());
        }
        function ShowDocument(path, name) {
            var iframeid = 'fileIframe';
            document.getElementById(iframeid).src = path;
            $("#ShowDocumentDiv").dialog({
                autoOpen: true,
                modal: true,
                width: "82%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Document - " + name,
                show: 'slide'
            });
            return false;
        }
        function ShowDocumentById(id) {
            PageMethods.LoadEmpTransferDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            $("#imageDiv").html(result);

            $("#TransferDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: "70%",
                height: 300,
                closeOnEscape: true,
                resizable: false,
                title: "Employee Transfer Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }
    </script>
    <asp:HiddenField ID="hfTransferId" runat="server" Value="0"></asp:HiddenField>

    <asp:HiddenField ID="hfGLCompanyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfGLProjectId" runat="server" Value="0" />
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <div id="TransferDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="entry" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-1">Transfer Entry</a></li>
            <li id="search" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-2">Search Transfer</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Transfer Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeSearch ID="employeeeSearch" runat="server" />
                        <UserControl:companyProjectUserControl ID="companyProjectUserControl" runat="server" />
                        
                        <%--<div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCompanyTo" runat="server" class="control-label required-field" Text="Company To"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCompanyTo" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>--%>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDepartmentId" runat="server" class="control-label required-field" Text="Department To"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDepartmentTo" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDesignation" runat="server" class="control-label" Text="Designation To"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDesignationTo" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="Transfer Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTransferDate" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="Reporting Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportingDate" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblReportingTo" runat="server" class="control-label" Text="Reporting To (1)"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlReportingTo" CssClass="form-control" TabIndex="10"
                                    runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblReportingTo2" runat="server" class="control-label" Text="Reporting To (2)"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlReportingTo2" CssClass="form-control" TabIndex="10"
                                    runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Attachment</label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Employee Transfer Doc..." />
                            </div>
                        </div>
                        <div id="DocumentInfo">
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox TextMode="MultiLine" ID="txtDescription" CssClass="form-control" runat="server" autocomplete="off" Rows="4"> </asp:TextBox>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                        TabIndex="11" OnClientClick="return PerformSaveAction();" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" runat="server" TabIndex="12" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                                        OnClientClick="javascript: return PerformClearAction();" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Search Transfer
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeForLeaveSearch ID="employeeForLeaveSearch" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Reporting From"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Reporting To"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSearch_Click" OnClientClick="javascript: return PerformSearch();" />
                                <asp:Button ID="btnSrcClear" runat="server" TabIndex="12" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return PerformSearchClear();" />
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
                    <asp:GridView ID="gvEmployeeTransfer" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                        TabIndex="13" OnRowCommand="gvEmployeeTransfer_RowCommand" OnRowDataBound="gvEmployeeTransfer_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" ItemStyle-Width="25">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Transfer Date" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("TransferDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="PreviousDepartmentName" HeaderText="Previous Department"
                                ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CurrentDepartmentName" HeaderText="Current Department"
                                ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Reporting Date" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ReportingDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApproved"
                                        CommandArgument='<%# bind("TransferId") %>' ImageUrl="~/Images/approved.png"
                                        Text="" AlternateText="Details" ToolTip="Approve Item" OnClientClick="return confirm('Do you want to Approve?');" />
                                    &nbsp;&nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False"
                                        CommandName="CmdDelete" CommandArgument='<%# bind("TransferId") %>' ImageUrl="~/Images/cancel.png"
                                        Text="" AlternateText="Details" ToolTip="Cancel Item" OnClientClick="return confirm('Do you want to Cancle Item?');" />
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("TransferId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" OnClientClick="return confirm('Do you want to Update?');" />
                                    &nbsp;<asp:ImageButton ID="ImgDocument" runat="server" CausesValidation="False" CommandName="CmdDocument"
                                        CommandArgument='<%# bind("TransferId") %>' ImageUrl="~/Images/document.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit"  OnClientClick='<%# Eval("TransferId", "ShowDocumentById(\"{0}\"); return false;") %>' />
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
        </div>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
</asp:Content>
