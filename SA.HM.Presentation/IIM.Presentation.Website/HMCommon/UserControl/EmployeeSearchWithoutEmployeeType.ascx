<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeSearchWithoutEmployeeType.ascx.cs"
    Inherits="HotelManagement.Presentation.Website.HMCommon.UserControl.EmployeeSearchWithoutEmployeeType" %>
<asp:HiddenField runat="server" ID="hfEmployeeId" Value="0" />
<asp:HiddenField runat="server" ID="hfEmployeeName" Value="" />

<div class="form-group">
    <div class="col-md-2">
        <asp:Label ID="lblEmployeeCode" runat="server" class="control-label required-field"
            Text="Employee ID"></asp:Label>
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

                    if (result.d.EmpId == 0 || result.d.EmployeeStatus == 'Inactive') {
                        toastr.warning("Employee Not Found.");
                        return false;
                    }

                    $("#<%= hfEmployeeId.ClientID %>").val(result.d.EmpId);
                    $("#<%= txtEmployeeName.ClientID %>").val(result.d.EmployeeName);

                    $("#<%= hfEmployeeName.ClientID %>").val(result.d.EmployeeName);


                    //WorkAfterSearchEmployee();

                },
                error: function (error) {
                    toastr.error("error");
                    $("#<%= hfEmployeeId.ClientID %>").val("0");
                    $("#<%= txtEmployeeName.ClientID %>").val("");
                    $("#<%= hfEmployeeName.ClientID %>").val("");

                }
            });

            return false;
        });

    });

</script>
