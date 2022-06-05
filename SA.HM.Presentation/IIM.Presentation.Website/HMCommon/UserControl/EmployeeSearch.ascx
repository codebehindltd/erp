<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeSearch.ascx.cs"
    Inherits="HotelManagement.Presentation.Website.HMCommon.UserControl.EmployeeSearch" %>
<asp:HiddenField runat="server" ID="hfEmployeeId" Value="0" />
<asp:HiddenField runat="server" ID="hfEmployeeName" Value="" />
<div class="form-group">
    <div class="col-md-2">
        <asp:Label ID="Label1" runat="server" class="control-label" Text="Search Type"></asp:Label>
    </div>
    <div class="col-md-10">
        <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control">
            <asp:ListItem Value="0" Text="--- All Employee ----"></asp:ListItem>
            <asp:ListItem Value="1" Text="Individual Employee"></asp:ListItem>
        </asp:DropDownList>
    </div>
</div>
<div id="employeeSearchSection">
    <div class="form-group">
        <div class="col-md-2">
            <asp:Label ID="lblEmployeeCode" runat="server" class="control-label required-field"
                Text="Employee ID"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtSearchEmployee" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
        </div>
        <div class="col-md-4">
            <input id="btnSrcEmployees" type="button" value="Search" class="TransactionalButton btn btn-primary btn-sm"
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
    </div>
</div>
<script type="text/javascript">

    $(document).ready(function () {

        if ($("#<%=ddlEmployee.ClientID %>").val() == "0")
            $("#employeeSearchSection").hide();

        $("#<%= ddlEmployee.ClientID %>").change(function () {
            if ($(this).val() == "0") {
                $("#employeeSearchSection").hide();
                $("#<%=hfEmployeeId.ClientID %>").val("0");
                $("#<%=txtSearchEmployee.ClientID %>").val("");
                $("#<%=txtEmployeeName.ClientID %>").val("");
            }
            else if ($(this).val() == "1") {
                $("#employeeSearchSection").show();
            }
        });


        $("#btnSrcEmployees").click(function () {

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
                    $("#<%= txtEmployeeName.ClientID %>").val(result.d.EmployeeName);

                    $("#<%= hfEmployeeName.ClientID %>").val(result.d.EmployeeName);
                },
                error: function (error) {
                    toastr.error("error");
                }
            });

            return false;
        });

    });

</script>
