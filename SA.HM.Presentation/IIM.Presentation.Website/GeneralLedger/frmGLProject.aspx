<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmGLProject.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmGLProject" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Project Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("[id=ContentPlaceHolder1_gvUserGroupCostCenterInfo_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvUserGroupCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvUserGroupCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

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
            $("#ContentPlaceHolder1_cbCompanyProject").on('change', function () {
                ShowOrHideSMCompany(this);
            });

            ShowOrHideSMCompany($("#ContentPlaceHolder1_cbCompanyProject"));
        });
        function ShowOrHideSMCompany(control) {
            if ($(control).is(':checked'))
                $("#dvSMCompany").show();
            else
                $("#dvSMCompany").hide();
        }
        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            $("#<%=txtName.ClientID %>").val(result.Name);
            $("#<%=txtShortName.ClientID %>").val(result.ShortName);
            $("#<%=txtCode.ClientID %>").val(result.Code);
            $("#<%=txtDescription.ClientID %>").val(result.Description);
            $("#<%=txtProjectId.ClientID %>").val(result.ProjectId);
            $("#<%=ddlCompanyId.ClientID %>").val(result.CompanyId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewProject').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }


        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For Delete-------------------------        
        function PerformDeleteAction(actionId) {

            $.confirm({
                title: 'Confirm!',
                content: 'Do you want to delete this record?',
                buttons: {
                    confirm: function () {
                        PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
                    },
                    cancel: function () {
                    }
                }
            });
        }

        function OnDeleteObjectSucceeded(result) {
            window.location = "frmGLProject.aspx?DeleteConfirmation=Deleted"
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtName.ClientID %>").val('');
            $("#<%=txtShortName.ClientID %>").val('');
            $("#<%=txtCode.ClientID %>").val('');
            $("#<%=txtDescription.ClientID %>").val('');
            $("#<%=txtProjectId.ClientID %>").val('');
            $("#<%=ddlCompanyId.ClientID %>").val(0);
            $("#<%=btnSave.ClientID %>").val("Save");
            $("#ContentPlaceHolder1_gvUserGroupCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewProject').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewProject').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewProject').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewProject').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });
        function AttachFile() {
            $("#projectDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Project Documents",
                show: 'slide'
            });
        }

        function UploadComplete() {
            var randomId = +$("#ContentPlaceHolder1_RandomProjectId").val();
            var id = +$("#ContentPlaceHolder1_txtProjectId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            PageMethods.LoadProjectDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }

        function OnLoadDocumentSucceeded(result) {
            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            if (totalDoc > 0) {
                guestDocumentTable += "<table id='dealDocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

                for (row = 0; row < totalDoc; row++) {
                    if (row % 2 == 0) {
                        guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                    }
                    else {
                        guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                    }

                    guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";

                    if (guestDoc[row].Path != "") {
                        if (guestDoc[row].Extention == ".jpg")
                            imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                        else
                            imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    }
                    else
                        imagePath = "";

                    guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

                    guestDocumentTable += "<td align='left' style='width: 20%'>";
                    guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                    guestDocumentTable += "</td>";
                    guestDocumentTable += "</tr>";
                }
                guestDocumentTable += "</table>";

                $("#ContentPlaceHolder1_ProjectDocumentInfo").html(guestDocumentTable);
            }
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function DeleteGuestDoc(docId, rowIndex) {
            if (confirm("Want to delete?")) {
                var deletedDoc = $("#<%=hfDeletedDoc.ClientID %>").val();

                if (deletedDoc != "")
                    deletedDoc += "," + docId;
                else
                    deletedDoc = docId;

                $("#<%=hfDeletedDoc.ClientID %>").val(deletedDoc);

                $("#trdoc" + rowIndex).remove();
            }
        }

        function LoadProjectManagement() {
            //var taskName = $("#ContentPlaceHolder1_txtTaskNameForSearch").val();
            //var fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            //var toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();
            //if (fromDate == "")
            //    fromDate = new Date();
            //if (toDate == "")
            //    toDate = new Date();
            //fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');
            //toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            //var assignToId = $("#ContentPlaceHolder1_ddlSearchAssignTo").val();
            //var assignToId = $("#ContentPlaceHolder1_hfSelectedEmpIdForSearch").val(assignToId).val();

            //window.location.href = "./TaskManagement.aspx?&taskname=" + taskName + "&frmDate=" + fromDate + "&toDate=" + toDate + "&asigned=" + assignToId ;
            window.location.href = "./ProjectStageManagement.aspx";
        }

        function LoadGanttChart() {

            window.location.href = "../TaskManagement/GanttChartInformation.aspx";
        }
    </script>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsProjectCodeAutoGenerate" runat="server" />
    <div id="projectDocuments" style="display: none;">
        <label for="Attachment" class="control-label col-md-2">
            Attachment</label>
        <div class="col-md-4">
            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
            </asp:Panel>
        </div>
    </div>
    <asp:HiddenField ID="hfValidationFlag" runat="server" Value="0" />
    <asp:Panel ID="pnlAdminUser" runat="server">
        <div id="myTabs">
            <ul id="tabPage" class="ui-style">
                <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-1">Project Information</a></li>
                <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-2">Search Project </a></li>
            </ul>
            <div id="tab-1">
                <div id="EntryPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Project Information
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:HiddenField ID="txtProjectId" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Project Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
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
                                <div class="col-md-2 label-align" id="CodeModelLabel" runat="server">
                                    <asp:Label ID="lblCode" runat="server" class="control-label required-field" Text="Project Code"></asp:Label>
                                </div>
                                <div class="col-md-4" id="CodeModelControl" runat="server">
                                    <asp:TextBox ID="txtCode" CssClass="form-control" runat="server"></asp:TextBox>
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
                                    <asp:Label ID="Label1" runat="server" class="control-label" Text="Start Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtStartDate" CssClass="datepicker form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label2" runat="server" class="control-label" Text="End Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtEndDate" CssClass="datepicker form-control" runat="server"></asp:TextBox>
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
                                    <asp:TextBox ID="txtProjectAmount" CssClass="form-control" runat="server"></asp:TextBox>
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
                                <asp:HiddenField ID="RandomProjectId" runat="server"></asp:HiddenField>
                                <div class="col-md-2">
                                    <label class="control-label">Attachment</label>
                                </div>
                                <div class="col-md-10">
                                    <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div runat="server" id="ProjectDocumentInfo" class="col-md-12">
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-12">
                                    <div id="CategoryCostCenterInformationDiv" class="panel panel-default">
                                        <div class="panel-body">
                                            <asp:GridView ID="gvUserGroupCostCenterInfo" Width="100%" runat="server" AllowPaging="True"
                                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                                ForeColor="#333333" PageSize="200" OnRowDataBound="gvUserGroupCostCenterInfo_RowDataBound"
                                                CssClass="table table-bordered table-condensed table-responsive">
                                                <RowStyle BackColor="#E3EAEB" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCostCentreId" runat="server" Text='<%#Eval("CostCenterId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProjectId" runat="server" Text='<%#Eval("ProjectId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="05%">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cost Center Information" ItemStyle-Width="55%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvCostCentre" runat="server" Text='<%# Bind("CostCenter") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
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
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                        OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                        OnClientClick="javascript: return PerformClearAction();" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="tab-2">
                <div id="SearchEntry" class="panel panel-default">
                    <div class="panel-heading">
                        Project Information
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblSProjectName" runat="server" class="control-label required-field" Text="Project Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtSProjectName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div id="srcAccountCompanyInfo" class="form-group" runat="server">
                                <div class="col-md-2">
                                    <asp:Label ID="lblSCompanyName" runat="server" class="control-label required-field" Text="Accounts Company"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlSCompanyName" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblSCompanyCode" runat="server" class="control-label required-field" Text="Project Code"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSCompanyCode" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblSShortName" runat="server" class="control-label" Text="Short Name"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSShortName" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                                        OnClick="btnSearch_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="SearchPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Search Information

                        
                        <a style="float: right; padding: 2px;" href='javascript:void();' onclick='javascript:return LoadProjectManagement()' title='Project Management'>
                            <img style='width: 22px; height: 20px;' alt='Search Quotation' src='../Images/management.png' /></a>
                        <a style="float: right; padding: 2px;" href='javascript:void();' onclick='javascript:return LoadGanttChart()' title='Gantt Chart'>
                            <img style='width: 22px; height: 20px;' alt='Gantt Chart' src='../Images/ganttchart.png' /></a>
                    </div>
                    <div class="panel-body">
                        <asp:GridView ID="gvGLProject" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                            OnPageIndexChanging="gvGLProject_PageIndexChanging" OnRowDataBound="gvGLProject_RowDataBound"
                            OnRowCommand="gvGLProject_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("ProjectId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Project Name" ItemStyle-Width="50%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Code" HeaderText="Project Code" ItemStyle-Width="30%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageDetails" runat="server" CausesValidation="False" CommandName="CmdDetails"
                                            CommandArgument='<%# Eval("ProjectId")+","+ Eval("CompanyId") %>' ImageUrl="~/Images/detailsInfo.png" Text=""
                                            AlternateText="Edit" ToolTip="Details" />
                                        &nbsp;
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("ProjectId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                        &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                            CommandArgument='<%# bind("ProjectId") %>' ImageUrl="~/Images/delete.png" Text=""
                                            AlternateText="Delete" ToolTip="Delete" OnClientClick="return CheckConfirmation(this)" />
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
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlNormalUser" runat="server">
        <div id="SearchEntryNU" class="panel panel-default">
            <div class="panel-heading">
                Project Information
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblSProjectNameNU" runat="server" class="control-label required-field" Text="Project Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtSProjectNameNU" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div id="Div1" class="form-group" runat="server">
                        <div class="col-md-2">
                            <asp:Label ID="lblSCompanyNameNU" runat="server" class="control-label required-field" Text="Company Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlSCompanyNameNU" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblSCompanyCodeNU" runat="server" class="control-label required-field" Text="Project Code"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSCompanyCodeNU" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblSShortNameNU" runat="server" class="control-label" Text="Short Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSShortNameNU" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSearchNU" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                                OnClick="btnSearch_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="SearchPanelNU" class="panel panel-default">
            <div class="panel-heading">
                Search Information
                 <a style="float: right; padding: 0px;" href='javascript:void();' onclick='javascript:return LoadProjectManagement()' title='Project Management'>
                     <img style='width: 22px; height: 20px;' alt='Search Quotation' src='../Images/management.png' /></a>

            </div>
            <div class="panel-body">
                <asp:GridView ID="gvGLProjectNU" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                    OnPageIndexChanging="gvGLProject_PageIndexChanging" OnRowDataBound="gvGLProject_RowDataBound"
                    OnRowCommand="gvGLProject_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("ProjectId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Project Name" ItemStyle-Width="50%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Code" HeaderText="Project Code" ItemStyle-Width="30%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageDetails" runat="server" CausesValidation="False" CommandName="CmdDetails"
                                    CommandArgument='<%# bind("ProjectId") %>' ImageUrl="~/Images/detailsInfo.png" Text=""
                                    AlternateText="Edit" ToolTip="Details" />
                            </ItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageDetails" runat="server" CausesValidation="False" CommandName="CmdDetails"
                                    CommandArgument='<%# Eval("ProjectId")+","+ Eval("CompanyId") %>' ImageUrl="~/Images/detailsInfo.png" Text=""
                                    AlternateText="Edit" ToolTip="Details" />
                                &nbsp;
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("ProjectId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                    CommandArgument='<%# bind("ProjectId") %>' ImageUrl="~/Images/delete.png" Text=""
                                    AlternateText="Delete" ToolTip="Delete" OnClientClick="return CheckConfirmation(this)" />
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
            </div>
        </div>
    </asp:Panel>
    <script type="text/javascript">
        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
        }
        else {
            NewAddButtonPanelHide();
        }

        function CheckConfirmation(id) {

            if ($("#ContentPlaceHolder1_hfValidationFlag").val() == "1") { $("#ContentPlaceHolder1_hfValidationFlag").val("0"); return true; }

            $.confirm({
                buttons: {
                    confirm: function () {
                        $("#ContentPlaceHolder1_hfValidationFlag").val("1");
                        $(id).trigger("click");

                        return true;
                    },
                    cancel: function () {
                        $("#hfValidationFlag").val("0");
                    }
                }
            });

            return false;
        }

    </script>
</asp:Content>
