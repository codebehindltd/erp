<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpTraining.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpTraining" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Training & Education</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Training</li>";
            var breadCrumbs = moduleName + formName;
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#ContentPlaceHolder1_ddlTraining").select2();
            $("#ContentPlaceHolder1_ddlOrganizer").select2();
            $("#myTabs").tabs();

            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $('#ContentPlaceHolder1_ddlType').change(function () {
                var addType = $("#<%=ddlType.ClientID %>").val();
                if (addType == "employee") {
                    $("#Employee").show();
                    $("#Department").hide();
                    $("#Department").hide();
                    $("#ApplicantResultGridContainer").hide();
                }
                else {
                    $("#Department").show();
                    $("#Employee").hide();
                    $("#ApplicantResultGridContainer").show();
                }
            });

            $('#ContentPlaceHolder1_ddlDepartment').change(function () {
                var departmentId = $("#<%=ddlDepartment.ClientID %>").val();
                PageMethods.LoadEmployeeByDepartment(departmentId, OnLoadDepartmentSucceeded, OnLoadDepartmentFailed);
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

            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $("#<%=ddlEnableReminder.ClientID %>").change(function () {
                if ($("#<%=ddlEnableReminder.ClientID %>").val() == '1') {
                    $('#mailRemonder').hide();
                }
                else {
                    $('#mailRemonder').show();
                }
            });

            if ($('#ContentPlaceHolder1_ddlType').val() == "employee") {
                $("#Employee").show();
                $("#Department").hide();
                $("#Department").hide();
                $("#ApplicantResultGridContainer").hide();
            }
            else {
                $("#Department").show();
                $("#Employee").hide();
                $("#ApplicantResultGridContainer").show();
            }
        });

        function OnLoadDepartmentSucceeded(result) {
            var tr = "", totalRow = 1, editLink = "", deleteLink = "";
            $("#EmpTrainingDetailsTbl tbody").html("");

            $.each(result, function (count, gridObject) {

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td style = 'text-align: center; width:10%;' > <input type='checkbox' id='chk" + gridObject.EmpId + "' /> </td>";
                tr += "<td align='left' style=\"width:35%; cursor:pointer;\">" + gridObject.DisplayName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.Department + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.Designation + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.PresentPhone + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.EmpId + "</td>";

                tr += "</tr>"

                $("#EmpTrainingDetailsTbl tbody").append(tr);
                tr = "";
                totalRow += 1;
            });
        }

        function OnLoadDepartmentFailed(error) {

        }

        function AddList() {

            var saveObj = new Array();
            $("#EmpTrainingDetailsTbl tbody tr").each(function () {
                if ($(this).find("input").is(":checked") == true) {
                    empId = $.trim($(this).find("td:eq(5)").text());
                    empName = $.trim($(this).find("td:eq(1)").text());
                    saveObj.push({
                        EmpId: empId,
                        EmpName: empName
                    });
                }
            });
            $("#ContentPlaceHolder1_hfAddBy").val($("#ContentPlaceHolder1_ddlType").val());
           
            $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveObj));
            if ($("#ContentPlaceHolder1_ddlType").val() == 'employee') {
                if ($("#ContentPlaceHolder1_employeeSearch_txtEmployeeName").val() == "") {
                    toastr.warning("Please Add an Employee");
                    $("#ContentPlaceHolder1_employeeSearch_txtEmployeeName").focus();
                    return false;
                }
            }
            else {
                if (saveObj.length < 1) {
                    toastr.warning("Please Add an Employee");
                    return false;
                }
            }
            $("#ContentPlaceHolder1_employeeSearch_txtSearchEmployee").val("");
            $("#ContentPlaceHolder1_employeeSearch_txtEmployeeName").val("");
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvEmployeeTraining tbody tr").length;

            var trainer = $("#ContentPlaceHolder1_txtSearchTrainer").val();
            var courseName = $("#ContentPlaceHolder1_txtSearchCourseName").val();
            var location = $("#ContentPlaceHolder1_txtSearchLocation").val();
            var organizer = $("#ContentPlaceHolder1_ddlSearchOrganizer").val();
            var startDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            var endDate = $("#ContentPlaceHolder1_txtSearchToDate").val();

            PageMethods.SearchTrainingAndLoadGridInformation(trainer, courseName, organizer, location, startDate, endDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvEmployeeTraining tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvEmployeeTraining tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvEmployeeTraining tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='right' style=\"display:none;\">" + gridObject.TrainingId + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.TrainingName + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.Organizer + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.Trainer + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.Location + "</td>";
                tr += "<td align='right' style=\"width:20%; cursor:pointer;\">" + GetStringFromDateTime(gridObject.StartDate) + " - " + GetStringFromDateTime(gridObject.EndDate) + "</td>";

                tr += "<td align='right' style=\"width:15%; cursor:pointer;\">";
                if (IsCanEdit) {
                    tr += "<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.TrainingId + "')\" alt='Edit Information' border='0'/>";
                }
                if (IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return PerformDeleteAction('" + gridObject.TrainingId + "')\"  alt='Delete Information' border='0'/>"
                }
                tr += '</td>';
                tr += "</tr>";

                $("#gvEmployeeTraining tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);


            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformEditAction(trainingId) {
            if (!confirm("Do You Want to Edit?"))
                return false;
            var possiblePath = "frmEmpTraining.aspx?editId=" + trainingId;
            window.location = possiblePath;
        }

        function PerformDeleteAction(trainingId) {
            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteEmpTrainingInfoById(trainingId, OnDeleteSuccess, OnDeleteFail);
            }
            return false;
        }
        function OnDeleteSuccess(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(0);
            PerformClearAction();
        }
        function OnDeleteFail(error) {
            alert(error.get_message());
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }
        function PerformClearAction() {

            $("#<%=hfTrainingId.ClientID %>").val('');
            $("#<%=txtTrainer.ClientID %>").val('');
            $("#<%=ddlTraining.ClientID %>").val('');
            $("#<%=txtLocation.ClientID %>").val('');
            $("#<%=ddlOrganizer.ClientID %>").val('');
            $("#<%=txtFromDate.ClientID %>").val('');
            $("#<%=txtToDate.ClientID %>").val('');
            $("#<%=ddlEnableReminder.ClientID %>").val('');
            $("#<%=ddlSendMail.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val('');
            $("#<%=btnEmpTraining.ClientID %>").val("Save");
            return false;
        }

        function WorkAfterSearchEmployee() { }

        function SaveValidation() {
            var training = $("#ContentPlaceHolder1_ddlTraining").val();
            var organizer = $("#ContentPlaceHolder1_ddlOrganizer").val();
            var remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            if (training == "0") {
                toastr.warning("Please select a training.");
                $("#ContentPlaceHolder1_ddlTraining").focus();
                return false;
            }
            else if (organizer == "0") {
                toastr.warning("Please select an organizer");
                $("#ContentPlaceHolder1_ddlOrganizer").focus();
                return false;
            }
            else if (remarks == "") {
                toastr.warning("Please Enter Traning Agenda.");
                $("#ContentPlaceHolder1_txtRemarks").focus();
                return false;
            }
            var rowsCount = <%=gvTrainingDetail.Rows.Count %>;

            if (rowsCount <= 0) {
                toastr.warning("Please select an employee");
                return false;
            }
            
            return true;
        }
        function CheckAllGroupUser(topCheckBox) {
            if ($(topCheckBox).is(":checked") == true) {
                $("#EmpTrainingDetailsTbl tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#EmpTrainingDetailsTbl tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }
    </script>
    <asp:HiddenField ID="hfSaveObj" runat="server" Value="" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfAddBy" runat="server" Value="" />

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Employee Training</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Employee Training</a></li>
        </ul>
        <div id="tab-1">
            <asp:HiddenField ID="hfTrainingId" runat="server" Value="" />
            <div id="TrainingEntryPanel" class="panel panel-default">               
                <div class="panel-heading">Employee Training</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblTrainer" runat="server" class="control-label" Text="Trainer"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtTrainer" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>                   
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblCourseName" runat="server" class="control-label required-field" Text="Training Name"></asp:Label>                            
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlTraining" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>                    
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLocation" runat="server" class="control-label" Text="Location"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                        </div>
                        <div class="col-md-2" style="text-align: right">
                            <asp:Label ID="lblOrganizer" runat="server" class="control-label required-field" Text="Organizer"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlOrganizer" runat="server" CssClass="form-control"
                                TabIndex="4">
                            </asp:DropDownList>
                        </div>
                    </div>                    
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                        </div>
                        <div class="col-md-2" style="text-align: right">
                            <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                        </div>
                    </div>                   
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label7" runat="server" class="control-label" Text="Enable Reminder"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <%--<div style="margin-top: 10px;">
                                <asp:CheckBox ID="chkIsReminder" runat="server" Text="" CssClass="customChkBox" TabIndex="9" />
                            </div>--%>
                            <asp:DropDownList ID="ddlEnableReminder" runat="server" CssClass="form-control"
                                TabIndex="10">
                                <asp:ListItem Value="0">Yes</asp:ListItem>
                                <asp:ListItem Value="1">No</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div id="mailRemonder">
                            <div class="col-md-2" style="text-align: right">
                                <asp:Label ID="lblSendMail" runat="server" class="control-label" Text="Reminder Mail Before"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSendMail" runat="server" CssClass="form-control"
                                    TabIndex="10">
                                    <asp:ListItem Value="0">One Hour</asp:ListItem>
                                    <asp:ListItem Value="1">Two Hours</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>       
                    <div class="ChildSectionDiv">
                        <div id="TrainingDetailsInfoDiv" class="panel panel-default">                            
                                <div class="panel-heading">Training Details
                                Information</div>
                            <div class="panel-body">
                                <div class="form-horizontal">                            
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblType" runat="server" class="control-label" Text="Add By"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control"
                                                TabIndex="5">
                                                <asp:ListItem Value="employee">Individual Employee</asp:ListItem>
                                                <asp:ListItem Value="department">Department</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>                                                       
                                <div id="Department" style="display: none">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblDepartment" runat="server" class="control-label" Text="Department"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control"
                                                TabIndex="5">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>                               
                                <div id="Employee">                                    
                                    <UserControl:EmployeeSearch ID="employeeSearch" runat="server" />                                    
                                </div>
                                <div style="padding-top: 10px;">
                                </div>
                                <div id="ApplicantResultGridContainer" style="display: none">
                                    <table id="EmpTrainingDetailsTbl" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                                        <colgroup>
                                            <col style="width: 10%;" />
                                            <col style="width: 35%;" />
                                            <col style="width: 20%;" />
                                            <col style="width: 20%;" />
                                            <col style="width: 15%;" />
                                        </colgroup>
                                        <thead>
                                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                                <th style="text-align:center">
                                                    <input type="checkbox" id="chkbApplicant" onchange="CheckAllGroupUser(this)" title="Select Applicant"/>
                                                </th>
                                                <th style="text-align: left;">
                                                    Name
                                                </th>
                                                <th style="text-align: left;">
                                                    Department
                                                </th>
                                                <th style="text-align: left;">
                                                    Position
                                                </th>
                                                <th style="text-align: left;">
                                                    Phone No
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>                                
                                <div class="row">
                                   <div class="col-md-12">
                                    <%--Right Left--%>
                                    <asp:Button ID="btnAddTrainingDetail" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary"
                                        TabIndex="25" OnClick="btnAddTrainingDetail_Click" OnClientClick="javascript:return AddList();" />
                                    <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                                </div>  
                                </div>                              
                                <div>
                                    <asp:GridView ID="gvTrainingDetail" Width="100%" runat="server" AllowPaging="True"
                                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                        ForeColor="#333333" PageSize="5" OnRowCommand="gvTrainingDetail_RowCommand" 
                                        CssClass="table table-bordered table-condensed table-responsive">
                                        <RowStyle BackColor="#E3EAEB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("TrainingDetailId") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="50%">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <%--<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("TrainingDetailId") %>'
                                                        CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                        ToolTip="Edit" />
                                                    &nbsp;--%><asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False"
                                                        CommandArgument='<%# bind("TrainingDetailId") %>' CommandName="CmdDelete" ImageUrl="~/Images/delete.png"
                                                        OnClientClick="return confirm('Do you want to delete?');" Text="" AlternateText="Delete"
                                                        ToolTip="Delete" />
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
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblRemarks" runat="server" class="control-label required-field" Text="Traning Agenda"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                TabIndex="11"></asp:TextBox>
                        </div>
                    </div>               
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblDiscussed" runat="server" class="control-label" Text="Traning Discussed"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtDiscussed" runat="server" CssClass="form-control" TextMode="MultiLine"
                                TabIndex="11"></asp:TextBox>
                        </div>
                    </div>  
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblCallToAction" runat="server" class="control-label" Text="Call To Action"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtCallToAction" runat="server" CssClass="form-control" TextMode="MultiLine"
                                TabIndex="11"></asp:TextBox>
                        </div>
                    </div>  
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblConclusion" runat="server" class="control-label" Text="Conclusion"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtConclusion" runat="server" CssClass="form-control" TextMode="MultiLine"
                                TabIndex="11"></asp:TextBox>
                        </div>
                    </div>                    
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnEmpTraining" runat="server" Text="Save" TabIndex="12" CssClass="TransactionalButton btn btn-primary"
                                OnClick="btnEmpTrainingSave_Click" OnClientClick ="javascript:return SaveValidation()"/>
                            <asp:Button ID="btnEmpTrainingClear" runat="server" Text="Clear" TabIndex="9" CssClass="TransactionalButton btn btn-primary"
                                OnClick="btnEmpTrainingClear_Click" OnClientClick="return confirm('Do you want to Clear?');"/>
                        </div>
                    </div>
                </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Training Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Trainer"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSearchTrainer" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Course Name"></asp:Label>                                
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSearchCourseName" runat="server" CssClass="form-control"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Location"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchLocation" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                            </div>
                            <div class="col-md-2" style="text-align: right">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Organizer"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchOrganizer" runat="server" CssClass="form-control"
                                    TabIndex="4">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="From"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                            <div class="col-md-2" style="text-align: right">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="To"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
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
                    Search Information</div>
                <div class="panel-body">
                    <table id='gvEmployeeTraining' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="display: none;" />
                            <col style="width: 20%;" />
                            <col style="width: 15%;" />
                            <col style="width: 15%;" />
                            <col style="width: 15%;" />
                            <col style="width: 20%;" />
                            <col style="width: 15%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="display: none;">
                                </th>
                                <th>
                                    Course Name
                                </th>
                                <th>
                                    Organizer
                                </th>
                                <th ">
                                    Trainer
                                </th>
                                <th >
                                    Location
                                </th>
                                <th style="text-align: right;">
                                    Duration
                                </th>
                                <th style="text-align: right;">
                                    Action
                                </th>
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
