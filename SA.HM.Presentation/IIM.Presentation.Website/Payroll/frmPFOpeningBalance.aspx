<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmPFOpeningBalance.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmPFOpeningBalance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Provident Fund</a>";
            var formName = "<span class='divider'>/</span><li class='active'>PF Opening Balance</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlDepartment").select2({
                tags: false,
                allowClear: true,
                placeholder: "",
                width: "99.75%"
            });
        });

        function CheckValidation() {
            var answer = confirm("Do you want to save this record?")
            if (answer) {
                var saveObj = new Array();

                var empId = 0, empCont = 0, cmpCont = 0, interestAmount = 0;

                $("#PFOpeningBalance tbody tr").each(function () {

                    empId = parseInt($.trim($(this).find("td:eq(0)").text(), 10));

                    empCont = parseFloat($.trim($(this).find("td:eq(3)").find("input").val(), 10));
                    cmpCont = parseFloat($.trim($(this).find("td:eq(4)").find("input").val(), 10));
                    interestAmount = parseFloat($.trim($(this).find("td:eq(5)").find("input").val(), 10));
                    var IsEmpContNan = isNaN(empCont);
                    var IscmpContNan = isNaN(cmpCont);
                    var IsinterestAmountNan = isNaN(interestAmount);
                    if (IsEmpContNan) {
                        empCont = 0;
                    }if (IscmpContNan) {
                        cmpCont = 0;
                    } if (IsinterestAmountNan) {
                        interestAmount = 0; 
                    }
                    //if (empCont != 0 && cmpCont != 0 && interestAmount != 0) {
                        saveObj.push({
                            EmpId: empId,
                            EmployeeContribution: empCont,
                            CompanyContribution: cmpCont,
                            ProvidentFundInterest: interestAmount
                        });
                    //}

                });

                $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveObj));
                return true;
            }
            else {
                return false;
            }
        }        
    </script>
    <asp:HiddenField ID="hfSaveObj" runat="server" />
    <asp:HiddenField ID="hfPayrollProvidentFundTitleText" runat="server" />
    <div id="" class="panel panel-default">        
        <div class="panel-heading" runat="server" id="PanelHeadingTitleText">PF Opening Balance</div>
        <div class="panel-body">
        <div class="form-horizontal">   
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblProcessDate" runat="server" class="control-label required-field" Text="Month"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlProcessMonth" runat="server" CssClass="form-control"
                        TabIndex="2">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Year"></asp:Label>                    
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                </div>
            </div>            
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblDepartment" runat="server" class="control-label" Text="Department"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList AutoPostBack="true" ID="ddlDepartment" runat="server" CssClass="form-control"
                        OnSelectedIndexChanged="EmpDropDown_Change" TabIndex="2">
                    </asp:DropDownList>
                </div>
            </div>            
            <div style="padding-top: 10px;">
                <div id="ltlEmpList" runat="server">
                </div>
            </div>           
            <div class="row">
 <div class="col-md-12">
                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="2" CssClass="TransactionalButton btn btn-primary"
                    OnClick="btnSave_Click" OnClientClick="javascript:return CheckValidation()"/>
            </div>
            </div>
        </div>
        </div>
    </div>
</asp:Content>
