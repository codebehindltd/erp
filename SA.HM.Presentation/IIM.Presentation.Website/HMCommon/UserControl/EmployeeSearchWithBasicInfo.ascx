<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeSearchWithBasicInfo.ascx.cs"
    Inherits="HotelManagement.Presentation.Website.HMCommon.UserControl.EmployeeSearchWithBasicInfo" %>
<asp:HiddenField runat="server" ID="hfEmployeeId" Value="0" />
<asp:HiddenField runat="server" ID="hfEmployeeName" Value="" />
<asp:HiddenField runat="server" ID="hfEmployeeDesignation" Value="" />
<asp:HiddenField runat="server" ID="hfEmployeeDepartment" Value="" />
<div>
    <div class="form-group">
        <div class="col-md-2">
            <asp:Label ID="lblEmployeeCode" runat="server" class="control-label required-field"
                Text="Employee Code"></asp:Label>
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
        <div class="col-md-10">
            <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>

    </div>
    <div class="form-group">
        <div class="col-md-2">
            <asp:Label ID="Label1" runat="server" class="control-label" Text="Department"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:Label ID="lblDesignation" runat="server" class="control-label" Text="Designation"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-2">
            <asp:Label ID="lbl1010" runat="server" class="control-label" Text="Join Date:"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtJoinDate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:Label ID="Label2" runat="server" class="control-label" Text="Employee Grade"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtGrade" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-2">
            <asp:Label ID="Label3" runat="server" class="control-label" Text="Email:"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtMail" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:Label ID="Label4" runat="server" class="control-label" Text="Work Station"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtWorkStation" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-2">
            <asp:Label ID="Label5" runat="server" class="control-label" Text="Employee Code"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:Label ID="Label7" runat="server" class="control-label" Text="Employee Status"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
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

                    if (result.d.EmpId == 0 || result.d.EmployeeStatus =='Inactive') {
                        toastr.warning("Employee Not Found.");
                        CommonHelper.SpinnerClose();
                        return false;
                    }
                    $("#<%= hfEmployeeId.ClientID %>").val(result.d.EmpId);
                    $("#<%= txtEmployeeName.ClientID %>").val(result.d.EmployeeName);
                    $("#<%= txtDepartment.ClientID %>").val(result.d.Department);
                    $("#<%= hfEmployeeDepartment.ClientID %>").val(result.d.DepartmentId);
                    $("#<%= hfEmployeeName.ClientID %>").val(result.d.EmployeeName);
                    $("#<%= txtDesignation.ClientID %>").val(result.d.Designation);
                    $("#<%= txtGrade.ClientID %>").val(result.d.GradeName);
                    $("#<%= txtJoinDate.ClientID %>").val(CommonHelper.DateFromStringToDisplay(result.d.JoinDate, innBoarDateFormat));
                    $("#<%= txtWorkStation.ClientID %>").val(result.d.WorkStationName);
                    $("#<%= txtStatus.ClientID %>").val(result.d.EmployeeStatus);
                    $("#<%= txtMail.ClientID %>").val(result.d.OfficialEmail);
                    $("#<%= txtCode.ClientID %>").val(result.d.EmpCode);

                    WorkAfterSearchEmployee();

                },
                error: function (error) {
                    toastr.error("error");
                    $("#<%= hfEmployeeId.ClientID %>").val("0");
                    $("#<%= txtEmployeeName.ClientID %>").val("");
                    $("#<%= hfEmployeeName.ClientID %>").val("");
                    $("#<%= hfEmployeeDepartment.ClientID %>").val("0");
                }
            });

            return false;
        });
    });    
</script>
