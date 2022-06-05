<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeSearchWithDesignation.ascx.cs"
    Inherits="HotelManagement.Presentation.Website.HMCommon.UserControl.EmployeeSearchWithDesignation" %>
<asp:HiddenField runat="server" ID="hfEmployeeId" Value="0" />
<asp:HiddenField runat="server" ID="hfEmployeeName" Value="" />
<asp:HiddenField runat="server" ID="hfEmpDesignation" Value="" />
<asp:HiddenField runat="server" ID="hfEmpDesignationId" Value="" />
<asp:HiddenField runat="server" ID="hfEmpDepartment" Value="" />
<asp:HiddenField runat="server" ID="hfEmpDepartmentId" Value="" />
<asp:HiddenField runat="server" ID="hfCompanyId" Value="0" />
<asp:HiddenField runat="server" ID="hfProjectId" Value="0" />
<asp:HiddenField runat="server" ID="hfReportingTo1Id" Value="0" />
<asp:HiddenField runat="server" ID="hfReportingTo2Id" Value="0" />
<div>
    <div class="form-group">
        <div class="col-md-2">
            <asp:Label ID="lblEmployeeCode" runat="server" class="control-label" Text="Employee ID"></asp:Label>
            <span class="MandatoryField">*</span>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtSearchEmployee" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
        </div>
        <div class="col-md-4">
            <input id="btnSrcEmployees" type="button" runat="server" value="Search" class="TransactionalButton btn btn-primary"
                tabindex="2" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-2">
            <asp:Label ID="lblEmployeeNameCaption" runat="server" class="control-label" Text="Employee Name"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:Label ID="lblCompany" runat="server" class="control-label" Text="Company"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
    </div>
    <%--<div class="form-group">
        <div class="col-md-2">
            <asp:Label ID="lblEmpDesignation" runat="server" Text="Employee Designation"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtEmpDesignation" runat="server" Enabled="false"></asp:TextBox>
        </div>
    </div>
    <div class="divClear">
    </div>--%>
    <div class="form-group">
        <div class="col-md-2">
            <asp:Label ID="lblEmpDepart" runat="server" class="control-label" Text="Department"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtEmpDepart" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:Label ID="lblEmpDesignation" runat="server" class="control-label" Text="Designation"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtEmpDesig" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-2">
            <asp:Label ID="lblReportingTo1Id" runat="server" class="control-label" Text="Reporting To 1"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtReportingTo1" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:Label ID="lblReportingTo2" runat="server" class="control-label" Text="Reporting To 2"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtReportingTo2" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
    </div>
</div>
<script type="text/javascript">

    $(document).ready(function () {

        $("#<%= btnSrcEmployees.ClientID %>").click(function () {

            if ($("#<%= txtSearchEmployee.ClientID %>").val() == "") {
                toastr.warning("Please Give Employee Code");
                return false;
            }

            var employeeSearchCode = $("#<%= txtSearchEmployee.ClientID %>").val();
            var params = JSON.stringify({ employeeCode: employeeSearchCode });

            $.ajax({
                type: "POST",
                url: "../../../Common/WebMethodPage.aspx/SearchEmployee",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    if (result.d.EmpId == 0) {
                        toastr.warning("Employee Not Found.");
                    }

                    $("#<%= hfEmployeeId.ClientID %>").val(result.d.EmpId);
                    $("#<%= hfEmployeeName.ClientID %>").val(result.d.EmployeeName);
                    $("#<%= txtEmployeeName.ClientID %>").val(result.d.EmployeeName);

                    $("#<%= hfEmpDepartmentId.ClientID %>").val(result.d.DepartmentId);
                    $("#<%= hfEmpDepartment.ClientID %>").val(result.d.Department);
                    $("#<%= txtEmpDepart.ClientID %>").val(result.d.Department);

                    $("#<%= hfEmpDesignationId.ClientID %>").val(result.d.DesignationId);
                    $("#<%= hfEmpDesignation.ClientID %>").val(result.d.Designation);
                    $("#<%= txtEmpDesig.ClientID %>").val(result.d.Designation);

                    $("#<%= hfReportingTo1Id.ClientID %>").val(result.d.RepotingTo);
                    $("#<%= txtReportingTo1.ClientID %>").val(result.d.RepotingToOne);

                    $("#<%= hfReportingTo2Id.ClientID %>").val(result.d.RepotingTo2);
                    $("#<%= txtReportingTo2.ClientID %>").val(result.d.RepotingToTwo);

                    $("#<%= hfCompanyId.ClientID %>").val(result.d.GlCompanyId);
                    $("#<%= hfProjectId.ClientID %>").val(result.d.GlProjectId);
                    $("#<%= txtCompany.ClientID %>").val(result.d.GLCompanyName);



                    WorkAfterSearchEmployee();

                },
                error: function (error) {
                    toastr.error("error");
                    $("#<%= hfEmployeeId.ClientID %>").val("0");
                    $("#<%= txtEmployeeName.ClientID %>").val("");
                    $("#<%= hfEmployeeName.ClientID %>").val("");

                    $("#<%= hfEmpDepartmentId.ClientID %>").val("0");
                    $("#<%= hfEmpDepartment.ClientID %>").val("");
                    $("#<%= txtEmpDepart.ClientID %>").val("");

                    $("#<%= hfEmpDesignationId.ClientID %>").val("0");
                    $("#<%= hfEmpDesignation.ClientID %>").val("");
                    $("#<%= txtEmpDesig.ClientID %>").val("");

                }
            });

            return false;
        });

    });    

</script>
