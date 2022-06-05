<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmPFConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmPFConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var deleteObj = [];
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Provident Fund</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var chkIsTaxPaidbyCmp = '<%=chkIsTaxPaidbyCmp.ClientID%>'
            if ($(('#' + chkIsTaxPaidbyCmp)).attr('checked')) {
                $('#PaymentInformation').show();
            }
            else {
                $('#PaymentInformation').hide();
            }

            var chkIsTaxDdctFrmSlry = '<%=chkIsTaxDdctFrmSlry.ClientID%>'
            if ($(('#' + chkIsTaxDdctFrmSlry)).attr('checked')) {
                $('#TaxDdctInfo').show();
            }
            else {
                $('#TaxDdctInfo').hide();
            }

            $("#FestivalBonusContainer").hide();
            $("#ContentPlaceHolder1_ddlBonusType").change(function () {

                if ($(this).val() == "PeriodicBonus") {
                    $("#PerioDicalBonusContainer").show();
                    $("#FestivalBonusContainer").hide();
                    $("#AddMultipleBonus").hide();
                }
                else if ($(this).val() == "FestivalBonus") {
                    $("#PerioDicalBonusContainer").hide();
                    $("#FestivalBonusContainer").show();
                    $("#AddMultipleBonus").show();
                }
            });

            $("#btnAddMultipleBonus").click(function () {
                //                var add = false;
                //                var reservationId = $("#ContentPlaceHolder1_txtReservationId").val();
                //                if (reservationId == "") {
                //                    reservationId = 0;
                //                }

                AddBonusInfo(0);

            });

        });

        $(function () {
            $("#myTabs").tabs();
        });

        function ToggleFieldVisibleForCompanyPay(ctrl) {
            if ($(ctrl).attr('checked')) {
                $('#PaymentInformation').show();
            }
            else {
                $("#<%=ddlCmpContType.ClientID %>").val('')
                $("#<%=txtCmpContAmount.ClientID %>").val('')
                $('#PaymentInformation').hide();
            }
        }

        function PerformAttendanceDeviceClearAction() {
            $("#<%=ddlAttendanceDevice.ClientID %>").val('0');
            $("#<%=ddlDeviceType.ClientID %>").val('0');
        }

        function ToggleFieldVisibleForTaxDeduct(ctrl) {
            if ($(ctrl).attr('checked')) {
                $('#TaxDdctInfo').show();
            }
            else {
                $("#<%=ddlEmpContType.ClientID %>").val('')
                $('#TaxDdctInfo').hide();
            }
        }

        function AddBonusInfo(bonusId) {
            var bonusType = $("#<%=ddlBonusType.ClientID %>").val();
            var bonusAmount = $("#<%=txtBonusAmount.ClientID %>").val();
            var amountType = $("#<%=ddlAmountType.ClientID %>").val();
            var dependsOn = $("#<%=ddlDependsOn.ClientID %>").val();
            var bonusDate = $("#<%=txtBonusDate.ClientID %>").val();

            if (amountType == "Percent(%)") {
                if (bonusAmount.length > 3) {
                    toastr.warning('Percentage cannt be more than 100%.');
                    return false;
                }
            }
            if (bonusAmount == "") {
                toastr.warning('Please Provide Bonus Amount.');
                return false;
            }
            else if (bonusDate == "") {
                toastr.warning('Please Provide Bonus Date.');
                return false;
            }
            else if (amountType == "--- Please Select ---") {
                toastr.warning('Please Select Amount Type.');
                return false;
            }

            if ($("#ltlTableWiseBonusAdd > table").length > 0) {
                AddNewRow(bonusId, bonusType, bonusAmount, amountType, dependsOn, bonusDate);
                return false;
            }

            var table = "", deleteLink = "";

            deleteLink = "<a href=\"#\" onclick= 'DeleteBonus(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            table += "<table cellspacing='0' cellpadding='4' id='BonusInformation' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            table += "<th style='display:none'>Bonus Id</th><th style='display:none'>Bonus Type</th><th style='display:none'>Dependa On</th><th align='left' scope='col' style='width: 30%;'>Bonus Amount</th><th align='left' scope='col' style='width: 30%;'>Amount Type</th><th align='left' scope='col' style='width: 30%;'>Bonus Date</th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";

            table += "<tbody>";
            table += "<tr style=\"background-color:#E3EAEB;\">";

            table += "<td align='left' style=\"display:none;\">" + bonusId + "</td>";
            table += "<td align='left' style=\"display:none;\">" + bonusType + "</td>";
            table += "<td align='left' style=\"display:none;\">" + dependsOn + "</td>";
            table += "<td align='left' style=\"width:30%; text-align:Left;\">" + bonusAmount + "</td>";
            table += "<td align='left' style=\"width:30%; text-align:Left;\">" + amountType + "</td>";
            table += "<td align='left' style=\"width:30%; text-align:Left;\">" + bonusDate + "</td>";
            table += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            table += "</tr>";
            table += "</tbody>";
            table += "</table>";

            $("#ltlTableWiseBonusAdd").html(table);
        }

        function AddNewRow(bonusId, bonusType, bonusAmount, amountType, dependsOn, bonusDate) {
            var tr = "", totalRow = 0, deleteLink = "";
            totalRow = $("#BonusInformation tbody tr").length;

            deleteLink = "<a href=\"#\" onclick= 'DeleteBonus(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }
            tr += "<td align='left' style=\"display:none;\">" + bonusId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + bonusType + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + dependsOn + "</td>";
            tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + bonusAmount + "</td>";
            tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + amountType + "</td>";
            tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + bonusDate + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "</tr>";

            $("#BonusInformation tbody").append(tr);
        }

        function ValidationNPreprocess() {

            var saveObj = [];
            var bonusId = 0, bonusType = "", bonusAmount = 0, amountType = "", dependsOn = 0, bonusDate = "";

            var rowLength = $("#ltlTableWiseBonusAdd > table tbody tr").length;

            $("#ltlTableWiseBonusAdd > table tbody tr").each(function () {
                bonusId = parseInt($.trim($(this).find("td:eq(0)").text(), 10));
                bonusType = $(this).find("td:eq(1)").text();
                dependsOn = parseInt($.trim($(this).find("td:eq(2)").text(), 10));
                bonusAmount = parseFloat($.trim($(this).find("td:eq(3)").text(), 10));
                amountType = $(this).find("td:eq(4)").text();
                bonusDate = $(this).find("td:eq(5)").text();

                if (bonusId == 0) {
                    saveObj.push({
                        BonusSettingId: bonusId,
                        BonusType: bonusType,
                        DependsOnHead: dependsOn,
                        BonusAmount: bonusAmount,
                        AmountType: amountType,
                        BonusDate: bonusDate
                    });
                }
            });

            $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveObj));
            $("#<%=hfDeleteObj.ClientID %>").val(JSON.stringify(deleteObj));
        }

        function DeleteBonus(anchor) {
            ff = anchor;
            var tr = $(anchor).parent().parent();

            var bonusId = $.trim($(tr).find("td:eq(0)").text());
            //var reservationId = $.trim($(tr).find("td:eq(1)").text());

            if (parseInt(bonusId, 10) != 0) {
                deleteObj.push({
                    BonusSettingId: bonusId
                });
            }

            $(tr).remove();
            return false;
        }
        function PerformClearActionWithConfirmation() {
            if (!confirm('Do You Want to Clear?'))
                return false;
            PerformClearAction();
        }
    </script>
    <div>
        <div id="PFEntryPanel" class="panel panel-default">
            <div class="panel-heading">
                Employee Provident Fund Information</div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-4">
                            <asp:Label ID="lblEmployeeContributionHeadId" runat="server" class="control-label"
                                Text="Employee Contribution Head"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlEmployeeContributionHeadId" runat="server" CssClass="form-control"
                                TabIndex="3">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4">
                            <asp:HiddenField ID="txtPFSettingId" runat="server"></asp:HiddenField>
                            <asp:Label ID="lblEmpCont" runat="server" class="control-label required-field" Text="Employee Contribution (%)"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="txtEmpCont" CssClass="form-control" TabIndex="3">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4">
                            <asp:Label ID="lblCompanyContributionHeadId" runat="server" class="control-label"
                                Text="Company Contribution Head"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCompanyContributionHeadId" runat="server" CssClass="form-control"
                                TabIndex="3">
                            </asp:DropDownList>
                        </div>
                    </div>                    
                    <div class="form-group">
                        <div class="col-md-4">
                            <asp:Label ID="lblCmpCont" runat="server" class="control-label required-field" Text="Company Contribution (%)"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="txtCmpCont" CssClass="form-control" TabIndex="3">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4">
                            <asp:Label ID="Label4" runat="server" class="control-label required-field"
                                Text="Company Contribution On"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCompanyContributionOn" runat="server" CssClass="form-control"
                                TabIndex="3">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4">
                            <asp:Label ID="lblEmpMaxCont" runat="server" class="control-label required-field"
                                Text="Employee Can Contribute Max Of Basic Salary (%)"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="txtEmpMaxCont" TabIndex="3" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4">
                            <asp:Label ID="lblIntDisRt" runat="server" class="control-label required-field" Text="Interest Distribution Rate (%)"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="txtIntDisRt" CssClass="form-control" TabIndex="3">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4">
                            <asp:Label ID="lblCmpContElegYear" runat="server" class="control-label required-field"
                                Text="Company Contribution Elegibility (Year)"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="txtCmpContElegYear" CssClass="form-control" TabIndex="3">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnEmpPFSave" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                                OnClick="btnEmpPFSave_Click" />
                            <asp:Button ID="btnEmpPFClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                OnClientClick="javascript: return PerformClearActionWithConfirmation();" OnClick="btnEmpPFClear_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <div style="height: 45px">
        </div>
        <div id="myTabs">
            <ul id="tabPage" class="ui-style">
                <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-1">Settings</a></li>
                <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-2">Tax</a></li>
                <%--  <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Tax Deduction</a></li>--%>
                <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-3">Provident Fund</a></li>
                <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-4">Loan</a></li>
                <li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-5">Gratuity</a></li>
                <li id="F" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-6">Bonus</a></li>
                <li id="G" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-7">Attendance Device</a></li>
            </ul>
            <div id="tab-1">
                <div class="divFullSectionWithTwoDvie">
                    <div class="divBox divSectionLeftRightSameWidth">
                        <div id="MonthlySalaryDateSchedulePanel" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Salary Month Start
                                Date Information </a>
                            <div class="HMBodyContainer">
                                <div class="form-group">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:HiddenField ID="txtStartDateId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblScheduleDate" runat="server" Text="Month Start Date"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:DropDownList ID="ddlStartDate" runat="server" TabIndex="1" CssClass="tdLeftAlignWithSize">
                                            <asp:ListItem Value="1">1</asp:ListItem>
                                            <asp:ListItem Value="2">2</asp:ListItem>
                                            <asp:ListItem Value="3">3</asp:ListItem>
                                            <asp:ListItem Value="4">4</asp:ListItem>
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                            <asp:ListItem Value="6">6</asp:ListItem>
                                            <asp:ListItem Value="7">7</asp:ListItem>
                                            <asp:ListItem Value="8">8</asp:ListItem>
                                            <asp:ListItem Value="9">9</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="11">11</asp:ListItem>
                                            <asp:ListItem Value="12">12</asp:ListItem>
                                            <asp:ListItem Value="13">13</asp:ListItem>
                                            <asp:ListItem Value="14">14</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="16">16</asp:ListItem>
                                            <asp:ListItem Value="17">17</asp:ListItem>
                                            <asp:ListItem Value="18">18</asp:ListItem>
                                            <asp:ListItem Value="19">19</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="21">21</asp:ListItem>
                                            <asp:ListItem Value="22">22</asp:ListItem>
                                            <asp:ListItem Value="23">23</asp:ListItem>
                                            <asp:ListItem Value="24">24</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="26">26</asp:ListItem>
                                            <asp:ListItem Value="27">27</asp:ListItem>
                                            <asp:ListItem Value="28">28</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <asp:Button ID="btnSalaryMonthStartDateInfoSave" runat="server" Text="Save" CssClass="btn btn-primary"
                                    TabIndex="2" OnClick="btnSalaryMonthStartDateInfoSave_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="divBox divSectionLeftRightSameWidth">
                        <div id="EmployeeBasicPanel" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Employee Basic Salary
                                Setup Information </a>
                            <div class="HMBodyContainer">
                                <div class="form-group">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:HiddenField ID="txtBasicSetupId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblBasicSalaryHeadId" runat="server" Text="Basic Salary Head"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:DropDownList ID="ddlBasicSalaryHeadId" runat="server" CssClass="customMediumDropDownSize"
                                            TabIndex="3">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <asp:Button ID="btnBasicHeadSetup" runat="server" Text="Save" OnClick="btnBasicHeadSetup_Click"
                                    CssClass="btn btn-primary" TabIndex="4" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <%--<div class="divSectionLeftRightSameWidth">
                <div id="OverTimePanel" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Employee Overtime
                        Setup Information </a>
                    <div class="HMBodyContainer">
                        <div class="block-body collapse in">
                            <div class="HMContainerRow">
                                <div class="left-float">
                                    <div class="l-left">
                                        
                                        <asp:HiddenField ID="txtOverTimeSetupId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblOverTimeAmount" runat="server" Text="Overtime Head"></asp:Label>
                                    </div>
                                    <div class="r-left">
                                        
                                        <asp:DropDownList ID="ddlSalaryHeadId" runat="server" CssClass="customMediumDropDownSize"
                                            TabIndex="5">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtMonthlyTotalHour" runat="server" TabIndex="6"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="right-float">
                                    <div class="r-left">
                                       
                                        <asp:DropDownList ID="ddlSalaryHeadId" runat="server" CssClass="customMediumDropDownSize"
                                            TabIndex="5">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="l-right">
                                        
                                        <asp:Label ID="lblMonthlyTotalHour" runat="server" Text="Monthly Hour"></asp:Label>
                                        <span class="MandatoryField">*</span>
                                    </div>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                
                                <asp:Button ID="btnOverTimeInfoSave" runat="server" Text="Save" OnClick="btnOverTimeInfoSave_Click"
                                    CssClass="btn btn-primary" TabIndex="7" />
                            </div>
                            <div class="divClear">
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
                <div class="divFullSectionWithTwoDvie">
                    <div class="divBox divSectionLeftRightSameWidth">
                        <div id="OverTimePanel" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Employee Overtime
                                Setup Information </a>
                            <div class="HMBodyContainer">
                                <div class="form-group">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:HiddenField ID="txtOverTimeSetupId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblOverTimeAmount" runat="server" Text="Overtime Head"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:DropDownList ID="ddlSalaryHeadId" runat="server" CssClass="customMediumDropDownSize"
                                            TabIndex="5">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <%--Right Left--%>
                                <asp:Button ID="btnOverTimeInfoSave" runat="server" Text="Save" OnClick="btnOverTimeInfoSave_Click"
                                    CssClass="btn btn-primary" TabIndex="7" />
                            </div>
                            <div class="divClear">
                            </div>
                        </div>
                    </div>
                    <div class="divBox divSectionLeftRightSameWidth">
                        <div id="Div5" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Minimum Overtime
                                Hour Configuration </a>
                            <div class="HMBodyContainer">
                                <div class="form-group">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:HiddenField ID="txtMinimumOvertimeHourId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="Label7" runat="server" Text="Minimum Overtime Hour"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtMinimumOvertimeHour" runat="server">
                                        </asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <asp:Button ID="btnMinimumOvertimeHour" runat="server" Text="Save" TabIndex="13"
                                    CssClass="TransactionalButton btn btn-primary" OnClick="btnMinimumOvertimeHour_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divFullSectionWithTwoDvie">
                    <div class="divBox divSectionLeftRightSameWidth">
                        <div id="Div1" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Salary Process System
                            </a>
                            <div class="HMBodyContainer">
                                <div class="form-group">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:HiddenField ID="txtSalaryProcessId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblProcessSystem" runat="server" Text="Process System"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:DropDownList ID="ddlSalaryProcessSystem" runat="server" CssClass="tdLeftAlignWithSize"
                                            TabIndex="8">
                                            <asp:ListItem Value="Group">Group</asp:ListItem>
                                            <asp:ListItem Value="Individual">Individual</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <asp:Button ID="btnSalaryProcess" runat="server" Text="Save" CssClass="btn btn-primary"
                                    TabIndex="9" OnClick="btnSalaryProcess_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="divBox divSectionLeftRightSameWidth">
                        <div id="Div8" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Monthly WorkingDay
                                For Absentee Configuration </a>
                            <div class="HMBodyContainer">
                                <div class="form-group">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:HiddenField ID="txtMonthlyWorkingDayForAbsentreeId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblMonthlyWorkingDayForAbsentree" runat="server" Text="Monthly Working Day For Absentee"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtMonthlyWorkingDayForAbsentree" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <asp:Button ID="btnMonthlyWorkingDayForAbsentree" runat="server" Text="Save" TabIndex="13"
                                    CssClass="TransactionalButton btn btn-primary" OnClick="btnMonthlyWorkingDayForAbsentree_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divFullSectionWithTwoDvie">
                    <div class="divBox divSectionLeftRightSameWidth">
                        <div id="Div3" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Monthly Working Day
                                Configuration </a>
                            <div class="HMBodyContainer">
                                <div class="form-group">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:HiddenField ID="txtMonthlyWorkingDayId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="Label3" runat="server" Text="Monthly Working Day"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox runat="server" ID="txtMonthlyWorkingDay"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <asp:Button ID="btnMonthlyWorkingDay" runat="server" Text="Save" TabIndex="13" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnMonthlyWorkingDay_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="divBox divSectionLeftRightSameWidth">
                        <div id="Div4" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Daily Working Hour
                                Configuration </a>
                            <div class="HMBodyContainer">
                                <div class="form-group">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:HiddenField ID="txtWorkingHourId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblWorkingHour" runat="server" Text="Working Hour"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtWorkingHour" runat="server"> </asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <asp:Button ID="btnWorkingHour" runat="server" Text="Save" TabIndex="13" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnWorkingHour_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divFullSectionWithTwoDvie">
                    <div class="divBox divSectionLeftRightSameWidth">
                        <div id="Div6" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Instead Leave Configuration
                            </a>
                            <div class="HMBodyContainer">
                                <div class="form-group">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:HiddenField ID="txtInsteadLeaveHeadId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblInsteadLeave" runat="server" Text="Instead Leave"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:DropDownList ID="ddlInsteadLeaveHeadId" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <asp:Button ID="btnInsteadLeaveHead" runat="server" Text="Save" TabIndex="13" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnInsteadLeaveHead_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="divBox divSectionLeftRightSameWidth">
                        <div id="Div7" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Instead Leave For
                                One Holiday Configuration </a>
                            <div class="HMBodyContainer">
                                <div class="form-group">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:HiddenField ID="txtInsteadLeaveForOneHolidayId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblInsteadLeaveForOneHoliday" runat="server" Text="Leave For One Holiday"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtInsteadLeaveForOneHoliday" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <asp:Button ID="btnInsteadLeaveForOneHoliday" runat="server" Text="Save" TabIndex="13"
                                    CssClass="TransactionalButton btn btn-primary" OnClick="btnInsteadLeaveForOneHoliday_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <%--<div class="divFullSectionWithTwoDvie">
                <div class="divBox divSectionLeftRightSameWidth">
                </div>
                <div class="divBox divSectionLeftRightSameWidth">
                </div>
            </div>
            <div class="divClear">
            </div>--%>
            </div>
            <div id="tab-2">
                <div id="TaxEntryPanel" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Employee Tax Information
                    </a>
                    <div class="HMBodyContainer">
                        <div class="form-group">
                            <div class="divBox divSectionLeftLeft">
                                <asp:HiddenField ID="txtTaxSettingId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblTaxBandM" runat="server" Text="Tax Band (Male)"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox runat="server" ID="txtTaxBandM" TabIndex="3">
                                </asp:TextBox>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblTaxBandF" runat="server" Text="Tax Band (Female)"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:TextBox runat="server" ID="txtTaxBandF" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div id="IsTaxPaidbyCmp" class="form-group">
                            <div class="divBox divSectionLeftLeft">
                                <asp:CheckBox ID="chkIsTaxPaidbyCmp" runat="server" Text="" CssClass="customChkBox"
                                    onclick="javascript: return ToggleFieldVisibleForCompanyPay(this);" TabIndex="8" />
                                <asp:Label ID="lblIsTaxPaidbyCmp" runat="server" Text="Company Pay"></asp:Label>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div id="PaymentInformation" style="display: none;">
                            <div class="form-group">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblCmpContType" runat="server" Text="Contribution Type"></asp:Label>
                                    <span class="MandatoryField">*</span>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlCmpContType" runat="server" CssClass="tdLeftAlignWithSize"
                                        TabIndex="3">
                                        <asp:ListItem Value="Basic">Basic Salary</asp:ListItem>
                                        <asp:ListItem Value="Gross">Gross Salary</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblCmpContAmount" runat="server" Text="Contribution Amount"></asp:Label>
                                    <%--<span class="MandatoryField">*</span>--%>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtCmpContAmount" runat="server" TabIndex="12"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div id="IsTaxDdctFrmSlry" class="form-group">
                            <div class="divBox divSectionLeft ThreeColumnTextBox">
                                <asp:CheckBox ID="chkIsTaxDdctFrmSlry" runat="server" Text="" CssClass="customChkBox"
                                    onclick="javascript: return ToggleFieldVisibleForTaxDeduct(this);" TabIndex="8" />
                                <asp:Label ID="lblIsTaxDdctFrmSlry" runat="server" Text="Deduction From Salary"></asp:Label>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div id="TaxDdctInfo" style="display: none;">
                            <div class="form-group">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblEmpContType" runat="server" Text="Contribution Type"></asp:Label>
                                    <span class="MandatoryField">*</span>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlEmpContType" runat="server" CssClass="tdLeftAlignWithSize"
                                        TabIndex="3">
                                        <asp:ListItem Value="Basic">Basic Salary</asp:ListItem>
                                        <asp:ListItem Value="Gross">Gross Salary</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="form-group">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblRemarks" runat="server" Text="Remarks"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="ThreeColumnTextBox" TextMode="MultiLine"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="HMContainerRowButton">
                            <asp:Button ID="btnEmpTaxSave" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                                OnClick="btnEmpTaxSave_Click" />
                            <asp:Button ID="btnEmpTaxClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                OnClientClick="javascript: return PerformClearAction();" OnClick="btnEmpTaxClear_Click" />
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
            <div id="tab-3">
                <div class="divClear">
                </div>
            </div>
            <div id="tab-4">
                <div id="LoanEntryPanel" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Employee Loan Information
                    </a>
                    <div class="HMBodyContainer">
                        <div class="form-group">
                            <div class="col-md-2" style="width: 350px;">
                                <asp:HiddenField ID="txtLoanSettingId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblCmpLnIntRate" runat="server" Text="Company Loan Interest Rate (%)"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtCmpLnIntRate" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="form-group">
                            <div class="col-md-2" style="width: 350px;">
                                <asp:Label ID="lblPFlnIntRate" runat="server" Text="Provident Fund Loan Interest Rate (%)"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtPFlnIntRate" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="form-group">
                            <div class="col-md-2" style="width: 350px;">
                                <asp:Label ID="lblMaxAmtWdrwfmPF" runat="server" Text="Max Amount Can Withdraw From Provident Fund (%)"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtMaxAmtWdrwfmPF" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="form-group">
                            <div class="col-md-2" style="width: 350px;">
                                <asp:Label ID="lblMinPFavlfrLn" runat="server" Text="Min Provident Fund Must Available To Allow Loan"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtMinPFavlfrLn" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="form-group">
                            <div class="col-md-2" style="width: 350px;">
                                <asp:Label ID="lblMinJobLnthfrCmpLn" runat="server" Text="Min Job Length To Allow Company Loan (Month)"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtMinJobLnthfrCmpLn" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="form-group">
                            <div class="col-md-2" style="width: 350px;">
                                <asp:Label ID="lblDrtnfrNxtLn" runat="server" Text="Duration For Next Loan After Completion Taken Loan (Month)"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtDrtnfrNxtLn" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="HMContainerRowButton">
                            <asp:Button ID="btnEmpLoanSave" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                                OnClick="btnEmpLoanSave_Click" />
                            <asp:Button ID="btnEmpLoanClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                OnClientClick="javascript: return PerformClearAction();" OnClick="btnEmpLoanClear_Click" />
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
            <div id="tab-5">
                <div id="GratutityEntryPanel" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Employee Gratuity
                        Information </a>
                    <div class="HMBodyContainer">
                        <div class="form-group">
                            <div class="divBox divSectionLeftLeft">
                                <asp:HiddenField ID="txtGratuityId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblNoofJobYearfrGrty" runat="server" Text="No Of Job Year"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox runat="server" ID="txtNoofJobYearfrGrty" TabIndex="3">
                                </asp:TextBox>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:CheckBox ID="chkIsGrtybsdonBasic" runat="server" Text="" CssClass="customChkBox"
                                    TabIndex="8" />
                                <asp:Label ID="lblIsGrtybsdonBasic" runat="server" Text="Gratuity On Basic"></asp:Label>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="form-group">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblGrtyPercntge" runat="server" Text="Gratuity (%)"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox runat="server" ID="txtGrtyPercntge" TabIndex="3">
                                </asp:TextBox>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblGrtyNoAdded" runat="server" Text="No. Of Gratuity Added"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:TextBox runat="server" ID="txtGrtyNoAdded" TabIndex="3">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="HMContainerRowButton">
                            <asp:Button ID="btnEmpGratutitySave" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                                OnClick="btnEmpGratutitySave_Click" />
                            <asp:Button ID="btnEmpGratutityClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                OnClientClick="javascript: return PerformClearAction();" OnClick="btnEmpGratutityClear_Click" />
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
            <div id="tab-6">
                <asp:HiddenField ID="hfSaveObj" runat="server" />
                <asp:HiddenField ID="hfDeleteObj" runat="server" />
                <div id="BonusHeadPanel" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Employee Bonus Setup
                        Information </a>
                    <div class="HMBodyContainer">
                        <div class="form-group">
                            <div class="divBox divSectionLeftLeft">
                                <asp:HiddenField ID="hfBonusHeadId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblBonusHeadId" runat="server" Text="Bonus"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:DropDownList ID="ddlBonusHeadId" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <asp:Button ID="btnBonusHead" runat="server" Text="Save" TabIndex="13" CssClass="TransactionalButton btn btn-primary"
                            OnClick="btnBonusHead_Click" />
                    </div>
                </div>
                <div id="BonusEntryPanel" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Bonus Formula</a>
                    <div class="HMBodyContainer">
                        <div class="form-group">
                            <div class="divBox divSectionLeftLeft">
                                <asp:HiddenField ID="hfBonusId" runat="server"></asp:HiddenField>
                                <asp:Label ID="Label1" runat="server" Text="Bonus Type"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:DropDownList ID="ddlBonusType" runat="server" CssClass="ThreeColumnDropDownList">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                                    <asp:ListItem Text="Festival" Value="FestivalBonus"></asp:ListItem>
                                    <asp:ListItem Text="Periodical" Value="PeriodicBonus"></asp:ListItem>
                                    <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div id="" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Festival Bonus Setup</a>
                            <div class="HMBodyContainer">
                                <div class="form-group">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="Label2" runat="server" Text="Bonus Amount"></asp:Label>
                                        <span class="MandatoryField">*</span>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtBonusAmount" runat="server" CssClass="customMediumTextBoxSize"></asp:TextBox>
                                    </div>
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblAmountType" runat="server" Text="Amount Type"></asp:Label>
                                        <span class="MandatoryField">*</span>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:DropDownList ID="ddlAmountType" runat="server" CssClass="customSmallDropDownSize"
                                            TabIndex="9">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="form-group">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblDependsOn" runat="server" Text="Depends On"></asp:Label>
                                    <span class="MandatoryField">*</span>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlDependsOn" runat="server" CssClass="customMediumDropDownSize"
                                        TabIndex="7">
                                    </asp:DropDownList>
                                </div>
                                <div id="FestivalBonusContainer">
                                    <div class="divBox divSectionRightLeft">
                                        <asp:Label ID="lblSalaryHeadId" runat="server" Text="Bonus Date"></asp:Label>
                                        <span class="MandatoryField">*</span>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:TextBox ID="txtBonusDate" runat="server" CssClass="datepicker"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="PerioDicalBonusContainer">
                                    <div class="divBox divSectionRightLeft">
                                        <asp:HiddenField ID="hfBonusMonthlyPeriodId" runat="server"></asp:HiddenField>
                                        <asp:Label ID="lblBonusMonthlyPeriod" runat="server" Text="Effected Period"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionRightRight">
                                        <asp:DropDownList ID="ddlBonusMonthlyPeriod" runat="server" TabIndex="1" CssClass="span8">
                                            <asp:ListItem Value="1">1</asp:ListItem>
                                            <asp:ListItem Value="2">2</asp:ListItem>
                                            <asp:ListItem Value="3">3</asp:ListItem>
                                            <asp:ListItem Value="4">4</asp:ListItem>
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                            <asp:ListItem Value="6">6</asp:ListItem>
                                            <asp:ListItem Value="7">7</asp:ListItem>
                                            <asp:ListItem Value="8">8</asp:ListItem>
                                            <asp:ListItem Value="9">9</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="11">11</asp:ListItem>
                                            <asp:ListItem Value="12">12</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;Month
                                    </div>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div id="AddMultipleBonus" style="display: none">
                                <div class="HMContainerRowButton">
                                    <input type="button" id="btnAddMultipleBonus" value="Add" class="TransactionalButton btn btn-primary" />
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="block-body collapse in">
                                    <div id="ltlTableWiseBonusAdd" runat="server" clientidmode="Static">
                                    </div>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <asp:Button ID="btnFestivalBonus" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnEmpBonusSave_Click" OnClientClick="javascript:return ValidationNPreprocess();" />
                                <asp:Button ID="btnClearFestivalBonus" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return PerformClearAction();" OnClick="btnEmpBonusClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
            <div id="tab-7">
                <div id="Div10" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Attendance Device</a>
                    <div class="HMBodyContainer">
                        <div class="form-group">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="Label5" runat="server" Text="Device Name"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:DropDownList ID="ddlAttendanceDevice" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAttendanceDevice_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblDeviceType" runat="server" Text="Device Type"></asp:Label>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:DropDownList ID="ddlDeviceType" runat="server" TabIndex="1" CssClass="span8">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="officein">Office In</asp:ListItem>
                                    <asp:ListItem Value="officeout">Office Out</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <asp:Button ID="btnAttendanceDeviceUpdate" runat="server" Text="Update" TabIndex="4"
                                    CssClass="TransactionalButton btn btn-primary" OnClick="btnAttendanceDeviceUpdate_Click"
                                    OnClientClick="javascript:return ValidationNPreprocess();" />
                                <button type="button" class="TransactionalButton btn btn-primary" onclick="javascript: return PerformAttendanceDeviceClearAction();">
                                    Clear</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
