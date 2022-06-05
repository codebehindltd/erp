<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBestEmployeeSelection.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmBestEmployeeSelection" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeForLoanSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Best Employee Selection</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            $("#checkAllEmployee").click(function () {
                if ($(this).is(":checked") == true) {
                    $("#gvEmployee tbody tr").find("td:eq(2)").find("input").prop('checked', true);
                }
                else {
                    $("#gvEmployee tbody tr").find("td:eq(2)").find("input").prop('checked', false);
                }
            });

            $("#<%=ddlBeastEmployeeType.ClientID %>").change(function () {
                var ddlBeastEmployeeTypeVal = $("#<%=ddlBeastEmployeeType.ClientID %>").val();
                if (ddlBeastEmployeeTypeVal == "Month") {
                    $("#MonthSelectionDiv").show();
                }
                else {
                    $("#MonthSelectionDiv").hide();
                }
            });

            $("#<%=ddlSProcessType.ClientID %>").change(function () {
                var ddlSProcessType = $("#<%=ddlSProcessType.ClientID %>").val();
                if (ddlSProcessType == "Month") {
                    $("#MonthSelectionSDiv").show();
                }
                else {
                    $("#MonthSelectionSDiv").hide();
                }
            });

        });

        function EmployeeNominationSelectionSave() {

            var BestEmployeeNominationDetails = new Array(), BestEmployeeNomination = {};
            var bestEmpNomineeId = "0", bestEmpNomineeDetailsId = "0";

            bestEmpNomineeId = $("#ContentPlaceHolder1_hfBestEmpNomineeId").val();

            var processType = $("#<%=ddlBeastEmployeeType.ClientID %>").val();

            if (bestEmpNomineeId == "")
                bestEmpNomineeId = "0";

            BestEmployeeNomination = {
                BestEmpNomineeId: bestEmpNomineeId,
                DepartmentId: $("#ContentPlaceHolder1_ddlDepartmentId").val(),
                Years: $("#ContentPlaceHolder1_ddlSelectionYear").val(),
                Month: $("#ContentPlaceHolder1_ddlSelectionMonth").val()
            };

            $("#gvEmployee tbody tr").each(function () {

                if ($(this).find("td:eq(3)").find("input").is(":checked") == true) {

                    bestEmpNomineeId = $(this).find("td:eq(1)").text();
                    bestEmpNomineeDetailsId = $(this).find("td:eq(2)").text();

                    if (bestEmpNomineeDetailsId == "")
                        bestEmpNomineeDetailsId = "0";

                    BestEmployeeNominationDetails.push({
                        BestEmpNomineeId: bestEmpNomineeId,
                        BestEmpNomineeDetailsId: bestEmpNomineeDetailsId,
                        BestEmpNomineeId: bestEmpNomineeId,
                        EmpId: $(this).find("td:eq(0)").text()
                    });
                }
            });

            PageMethods.UpdateBestEmployeeSelection(BestEmployeeNomination, BestEmployeeNominationDetails, processType, OnEmployeeNominationSucceeded, OnEmployeeNominationFailed);

            return false;
        }

        function OnEmployeeNominationSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            $("#frmHotelManagement")[0].reset();
        }

        function OnEmployeeNominationFailed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
        }

    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfBestEmpNomineeId" runat="server" Value="" />
    <asp:HiddenField ID="hfDepartmentId" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Best Employee Selection</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Best Employee Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="ApprEvaluationEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Best Employee Selection</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDepartmentId" runat="server" class="control-label required-field" Text="Department"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlDepartmentId_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblBeastEmployeeType" runat="server" class="control-label required-field" Text="Process Type"></asp:Label>                               
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList AutoPostBack="true" ID="ddlBeastEmployeeType" runat="server" CssClass="form-control"
                                    OnSelectedIndexChanged="ProcessTypeDropDown_Change">
                                    <asp:ListItem Text="Employee Of The Month" Value="Month"></asp:ListItem>
                                    <asp:ListItem Text="Employee Of The Year" Value="Year"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Year"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSelectionYear" runat="server" CssClass="form-control"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlSelectionYear_SelectedIndexChanged">
                                    <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                    <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                    <asp:ListItem Text="2021" Value="2021"></asp:ListItem>
                                    <asp:ListItem Text="2022" Value="2022"></asp:ListItem>
                                    <asp:ListItem Text="2023" Value="2023"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="MonthSelectionDiv">
                                <div class="col-md-2">
                                    <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Month"></asp:Label>                                    
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlSelectionMonth" runat="server" CssClass="form-control"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlSelectionMonth_SelectedIndexChanged">
                                        <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="December" Value="12"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div style="padding-top: 10px;" class="childDivSection">
                            <div id="DepartmentWiseEmployee" runat="server">
                            </div>
                            <div class="form-group" style="width: 97%;">
                                <div id="appraisalEvalutionConatainer" runat="server">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpApprEvaluation" runat="server" Text="Save" TabIndex="2" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript:return EmployeeNominationSelectionSave()" />
                                <asp:Button ID="btnEmpApprEvaluationClear" OnClientClick="return confirm('Do you want to clear?');" runat="server" Text="Clear" TabIndex="9"
                                    CssClass="TransactionalButton btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Best Employee Search</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSProcessType" runat="server" class="control-label required-field" Text="Process Type"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSProcessType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Employee Of The Month" Value="Month"></asp:ListItem>
                                    <asp:ListItem Text="Employee Of The Year" Value="Year"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSDepartment" runat="server" class="control-label required-field" Text="Department"></asp:Label>                               
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSDepartment" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSYear" runat="server" class="control-label required-field" Text="Year"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSYear" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                    <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                    <asp:ListItem Text="2021" Value="2021"></asp:ListItem>
                                    <asp:ListItem Text="2022" Value="2022"></asp:ListItem>
                                    <asp:ListItem Text="2023" Value="2023"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="MonthSelectionSDiv">
                                <div class="col-md-2">
                                    <asp:Label ID="lblSMonth" runat="server" class="control-label required-field" Text="Month"></asp:Label>                                    
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlSMonth" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="December" Value="12"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="Button1" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div style="padding-top: 10px;" class="childDivSection">
                    <div id="SearchInfo" runat="server">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var ddlBeastEmployeeTypeVal = $("#<%=ddlBeastEmployeeType.ClientID %>").val();
            if (ddlBeastEmployeeTypeVal == "Month") {
                $("#MonthSelectionDiv").show();
            }
            else {
                $("#MonthSelectionDiv").hide();
            }

            var ddlProcessType = $("#<%=ddlSProcessType.ClientID %>").val();
            if (ddlProcessType == "Month") {
                $("#MonthSelectionSDiv").show();
            }
            else {
                $("#MonthSelectionSDiv").hide();
            }
        });
    </script>
</asp:Content>
