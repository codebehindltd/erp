<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="NewGLProjectIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.NewGLProjectIframe" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var isClose = false;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            IsCanSave = $("#<%=hfSavePermission.ClientID %>").val() == '1' ? true : false;
            IsCanEdit = $("#<%=hfEditPermission.ClientID %>").val() == '1' ? true : false;
            IsCanDelete = $("#<%=hfDeletePermission.ClientID %>").val() == '1' ? true : false;
            IsCanView = $("#<%=hfViewPermission.ClientID %>").val() == '1' ? true : false;

            if (!IsCanSave) {
                $("#btnSave").hide();
                $("#btnSaveNContinue").hide();
            }
            else {
                $("#btnSave").show();
                $("#btnSaveNContinue").show();
            } 
            var projectId = $.trim(CommonHelper.GetParameterByName("pid"));
            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'
            $('#' + txtStartDate).datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });
            $("#ContentPlaceHolder1_ddlCompanyName").select2({
                tags: false,
                allowClear: true,
                placeholder: "--- Please Select ---",
                width: "99.75%",
            });
            $("[id=chkAll]").on("change", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#TableCostCenterInformation tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#TableCostCenterInformation tbody tr").find("td:eq(0) ").find("input").prop("checked", false);
                }
            });
            $("#ContentPlaceHolder1_cbCompanyProject").on('change', function () {
                ShowOrHideSMCompany(this);
            });

            ShowOrHideSMCompany($("#ContentPlaceHolder1_cbCompanyProject"));
            if (projectId != "") {
                CommonHelper.SpinnerOpen();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: './NewGLProjectIframe.aspx/FillForm',

                    data: "{'Id':'" + projectId + "'}",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                       
                        // $("#AddNewStatusContaiiner").dialog({ title: "Edit Source - " + data.d.SourceName + " " });
                        FillForm(data.d)
                        $("#btnSave").val("Update And Close");
                        $("#btnClear").hide();
                        $("#btnSaveNContinue").hide();
                        CommonHelper.SpinnerClose();
                    },
                    error: function (result) {

                    }
                });
                return false;
            }
                       
        });
        function FillForm(data) {
            $("#ContentPlaceHolder1_hfProjectId").val(data.ProjectId);
            $("#<%=txtCode.ClientID%>").val(data.Code);
            $("#<%=txtName.ClientID%>").val(data.Name);
            $("#<%=txtShortName.ClientID%>").val(data.ShortName);
            $("#<%=txtDescription.ClientID%>").val(data.Description);
            $("#<%=ddlCompanyId.ClientID%>").val(data.CompanyId);
            $("#<%=txtStartDate.ClientID%>").val(CommonHelper.DateFromStringToDisplay(data.StartDate, innBoarDateFormat));
            $("#<%=txtEndDate.ClientID%>").val(CommonHelper.DateFromStringToDisplay(data.EndDate, innBoarDateFormat));
            $("#<%=ddlProjectStage.ClientID%>").val(data.StageId);
            $("#<%=txtProjectAmount.ClientID%>").val(data.ProjectAmount);
            if (data.ProjectCompanyId != 0) {
                $("#ContentPlaceHolder1_cbCompanyProject").prop("checked", true).trigger('change');
            }
            $("#<%=ddlCompanyName.ClientID%>").val(data.ProjectCompanyId).trigger('change');
            for (var i = 0; i < data.CostCenters.length; i++) {
                $("#TableCostCenterInformation tbody tr").each(function () {
                    var costCenterId = parseInt($(this).find("td:eq(2)").text(), 10)
                    if (costCenterId == data.CostCenters[i].CostCenterId && data.CostCenters[i].Id > 0) {
                        $(this).find("td:eq(0)").find("input").prop("checked", true);
                    }
                });
            }
            UploadComplete();
            //$("#ContentPlaceHolder1_RandomDocId").val(data.ProjectId);
            if (!IsCanEdit) {
                $("#btnSave").show();
                $("#btnSaveNContinue").hide();
            }
            else {
                $("#btnSave").show();
                $("#btnSaveNContinue").show();
            }
        }
        function ShowOrHideSMCompany(control) {
            if ($(control).is(':checked'))
                $("#dvSMCompany").show();
            else
                $("#dvSMCompany").hide();
        }
        function LoadDocUploader() {
            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/GeneralLedger/File/GLProject/";
            var category = "GLProjectDocument";
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
        function SaveNClose() {
            isClose = true;
            //SaveOrUpdateTask();
            $.when(SaveOrUpdateProject()).done(function () {
                if (isClose) {

                    if (typeof parent.CloseDialog === "function") {
                        parent.CloseDialog();
                    }
                    if ($("#btnSave").val() == "Update and Close") {
                        $("#btnSave").val("Save And Close");
                        $("#btnSaveNContinue").show();
                        $("#btnClear").show();
                    }
                }
            });
            return false;
        }
        function SaveOrUpdateProject() {
            var ProjectId = $("#ContentPlaceHolder1_hfProjectId").val();
            var Code = $("#<%=txtCode.ClientID%>").val();
            if (Code == "") {
                isClose = false;
                toastr.warning("Enter project Code");
                $("#ContentPlaceHolder1_txtCode").focus();
                return false;
            }
            var Name = $("#<%=txtName.ClientID%>").val();
            if (Name == "") {
                isClose = false;
                toastr.warning("Enter Project Name");
                $("#ContentPlaceHolder1_txtName").focus();
                return false;
            }
            var ShortName = $("#<%=txtShortName.ClientID%>").val();
            var Description = $("#<%=txtDescription.ClientID%>").val();
            var CompanyId = $("#<%=ddlCompanyId.ClientID%>").val();
            if (CompanyId == "0") {
                isClose = false;
                toastr.warning("Please Select Company");
                $("#ContentPlaceHolder1_ddlCompanyId").focus();
                return false;
            }

            var StartDate = $("#<%=txtStartDate.ClientID%>").val();
            if (StartDate == "") {
                isClose = false;
                toastr.warning("Please Enter Start Date");
                $("#ContentPlaceHolder1_txtStartDate").focus();
                return false;
            }
            StartDate = CommonHelper.DateFormatToMMDDYYYY(StartDate, '/');
            var EndDate = $("#<%=txtEndDate.ClientID%>").val();
            if (EndDate == "") {
                isClose = false;
                toastr.warning("Please Enter End Date");
                $("#ContentPlaceHolder1_txtEndDate").focus();
                return false;
            }
            EndDate = CommonHelper.DateFormatToMMDDYYYY(EndDate, '/');

            var StageId = $("#<%=ddlProjectStage.ClientID%>").val();
            if (StageId == "0") {
                isClose = false;
                toastr.warning("Please Select Project Stage");
                $("#ContentPlaceHolder1_ddlProjectStage").focus();
                return false;
            }
            var ProjectAmount = $("#<%=txtProjectAmount.ClientID%>").val();
            if (ProjectAmount == "") {
                ProjectAmount = 0;
            }
            var ckBox = $("#ContentPlaceHolder1_cbCompanyProject").is(":checked");
            var ProjectCompanyId = $("#<%=ddlCompanyName.ClientID%>").val();
            if (ckBox == true) {
                if (ProjectCompanyId == "0") {
                    isClose = false;
                    toastr.warning("Please Select Project Company");
                    $("#ContentPlaceHolder1_ddlCompanyName").focus();
                    return false;
                }
            }
            var SelectedCostCenter = new Array();
            $("#TableCostCenterInformation tbody tr").each(function () {
                var a = $(this).find("td:eq(0)").find("input").is(":checked");
                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                    SelectedCostCenter.push(parseInt($(this).find("td:eq(2)").text(), 10));
                }
            });
            var CostCenterList = $('#ContentPlaceHolder1_hfSelectedCostCenter').val(SelectedCostCenter).val();

            var GLProjectBO = {
                ProjectId: ProjectId,
                CompanyId: CompanyId,
                Code: Code,
                Name: Name,
                ShortName: ShortName,
                Description: Description,
                StartDate: StartDate,
                EndDate: EndDate,
                StageId: StageId,
                ProjectAmount: ProjectAmount,
                ProjectCompanyId: ProjectCompanyId
            }
            var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
            PageMethods.SaveOrUpdateProject(GLProjectBO, CostCenterList, randomDocId, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
            return false;
        }
        function OnSuccessSaveOrUpdate(result) {
            $("#ContentPlaceHolder1_RandomDocId").val(result.Data);
            if (result.IsSuccess) {
                if (result.IsSuccess) {
                    parent.ShowAlert(result.AlertMessage);
                }
                else {
                    if (typeof parent.GridPaging === "function") {
                        GridPaging(1, 1);
                    }
                }
            }
            else {
                parent.ShowAlert(result.AlertMessage);
            }
            Clear();
        }

        function OnFailSaveOrUpdate(error) {
            isClose = false;
            toastr.error(error.get_message());
            return false;
        }
        function Clear() {
            $("#ContentPlaceHolder1_hfProjectId").val("0");
            $("#<%=txtCode.ClientID%>").val("");
            $("#<%=txtName.ClientID%>").val("");
            $("#<%=txtShortName.ClientID%>").val("");
            $("#<%=txtDescription.ClientID%>").val("");
            $("#<%=ddlCompanyId.ClientID%>").val("");
            $("#<%=txtStartDate.ClientID%>").val("");
            $("#<%=txtEndDate.ClientID%>").val("");
            $("#<%=ddlProjectStage.ClientID%>").val("0");
            $("#<%=txtProjectAmount.ClientID%>").val("");
            $("#ContentPlaceHolder1_cbCompanyProject").prop("checked", false).trigger('change');
            $("#<%=ddlCompanyName.ClientID%>").val("0");

            $("#TableCostCenterInformation tbody tr").each(function () {
                $(this).find("td:eq(0)").find("input").prop("checked", false);
            });
            return false;
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
            var id = $("#ContentPlaceHolder1_hfProjectId").val();
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
            DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }
                DocTable += "<td align='left' style='width: 60%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

                if (result[row].Path != "") {
                    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 40%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + imagePath + "</td>";
                DocTable += "</tr>";
            }
            DocTable += "</table>";

            docc = DocTable;

            $("#DocumentInfo").html(DocTable);

            return false;
        }
        function OnGetUploadedDocByWebMethodFailed(error) {
            alert(error.get_message());
        }
    </script>
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSelectedCostCenter" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsProjectCodeAutoGenerate" runat="server" />
    <div>
        <div style="padding: 10px 30px 10px 30px">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="txtProjectId" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Project Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div id="AccountCompanyInfo" class="form-group" runat="server">
                    <div class="col-md-2">
                        <asp:Label ID="lblCompanyId" runat="server" class="control-label required-field" Text="Accounts Company"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlCompanyId" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2" id="CodeModelLabel" runat="server">
                        <asp:Label ID="lblCode" runat="server" class="control-label required-field" Text="Project Code"></asp:Label>
                    </div>
                    <div class="col-md-4" id="CodeModelControl" runat="server">
                        <asp:TextBox ID="txtCode" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblShortName" runat="server" class="control-label" Text="Short Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtShortName" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Start Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartDate" CssClass="datepicker form-control" runat="server" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="End Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndDate" CssClass="datepicker form-control" runat="server" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2"></div>
                    <div class="col-md-5">
                        <div class="col-md-1">
                            <asp:CheckBox ID="cbCompanyProject" runat="server" CssClass="mycheckbox" />
                        </div>
                        <div class="col-md-11">
                            <label class="control-label">Is Company Project?</label>
                        </div>
                    </div>
                </div>
                <div id="dvSMCompany" class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Accounts Company"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlCompanyName" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Project Stage"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlProjectStage" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Project Amount"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtProjectAmount" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Attachment</label>
                    </div>
                    <div class="col-md-4">
                        <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                            class="TransactionalButton btn btn-primary btn-sm" value="Attach" />
                    </div>
                </div>
                <div class="form-group">
                    <div runat="server" id="ProjectDocumentInfo" class="col-md-12">
                    </div>
                </div>
                <div id="DocumentInfo">
                </div>
                <div class="col-md-12 form-group" runat="server">
                    <div class="panel-body form-group" style="height: auto;">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Literal ID="literalCostCenter" runat="server"> </asp:Literal>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="padding-bottom: 0; padding-top: 10px;">
            <div class="col-md-12">
                <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="SaveNClose()" value="Save & Close" />
                <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return Clear();" />
                <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return SaveOrUpdateProject();" />
            </div>
        </div>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
</asp:Content>
