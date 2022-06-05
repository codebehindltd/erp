<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmServiceChargeConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmServiceChargeConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Service Charge Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#checkAllEmployee").click(function () {
                if ($(this).is(":checked") == true) {
                    $("#gvEmployee tbody tr").find("td:eq(3)").find("input").prop('checked', true);
                }
                else {
                    $("#gvEmployee tbody tr").find("td:eq(3)").find("input").prop('checked', false);
                }
            });
        });

        function EmpServiceChargeSave() {

            var ServiceChargeDetails = new Array(), ServiceCharge = {};
            EmpDeletedLst = new Array();
            var serviceConfigId = "0", totalEmploye = 0;
            serviceConfigId = $("#ContentPlaceHolder1_hfServiceId").val();

            if (serviceConfigId == "") {
                serviceConfigId = "0";
            }

            ServiceCharge = {
                ServiceChargeConfigurationId: serviceConfigId,
                ServiceAmount: $("#<%=txtServiceAmount.ClientID %>").val(),
                DepartmentId: $("#<%=ddlDepartmentId.ClientID %>").val(),
                EmpTypeId: $("#<%=ddlEmpCategoryId.ClientID %>").val()
            };

            $("#gvEmployee tbody tr").each(function () {

                var serviceId = $(this).find("td:eq(1)").text();
                var serviceDetailsid = $(this).find("td:eq(2)").text();

                if ($(this).find("td:eq(3)").find("input").is(":checked") == true) {
                    totalEmploye = totalEmploye + 1;
                    if (serviceId == "0") {
                        ServiceChargeDetails.push({
                            ServiceChargeConfigurationId: serviceConfigId,
                            EmpId: $(this).find("td:eq(0)").text()
                        });
                    }
                    if (serviceId != "0") {
                        if (serviceDetailsid == "0") {
                            ServiceChargeDetails.push({
                                ServiceChargeConfigurationId: serviceConfigId,
                                EmpId: $(this).find("td:eq(0)").text()
                            });
                        }
                    }
                }
                else {
                    if (serviceId != "0") {
                        if (serviceDetailsid != "0") {
                            EmpDeletedLst.push({
                                EmpId: $(this).find("td:eq(0)").text(),
                                ServiceChargeConfigurationDetailsId: $(this).find("td:eq(2)").text()
                            });
                        }
                    }
                }
            });

            PageMethods.SaveServiceChargeConfiguration(ServiceCharge, ServiceChargeDetails, EmpDeletedLst, totalEmploye, OnSaveServiceChargeSucceeded, OnSaveServiceChargeFailed);
            return false;
        }

        function OnSaveServiceChargeSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            $("#ContentPlaceHolder1_hfServiceId").val('0');
            $("#<%=txtServiceAmount.ClientID %>").val('');
        }

        function OnSaveServiceChargeFailed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
        }
    </script>
    <asp:HiddenField ID="hfServiceId" runat="server" Value="" />
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Service Charge Configuration</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <%--<select id="tagList">
                <option>B</option>
                <option>C</option>
            </select>--%>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblDepartmentId" runat="server" class="control-label" Text="Department"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblEmpCategoryId" runat="server" class="control-label" Text="Employee Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlEmpCategoryId" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" TabIndex="4" Text="Search" CssClass="TransactionalButton btn btn-primary"
                            OnClick="btnSearch_Click" />
                        <%--<asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                    OnClick="btnClear_Click" />--%>
                    </div>
                </div>
                <div style="padding-top: 10px;" class="childDivSection">
                    <div id="Employee" runat="server">
                    </div>
                </div>
                <div id="SaveButton" runat="server" style="padding-top: 10px;">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblServiceAmount" runat="server" class="control-label" Text="Service Charge"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtServiceAmount" runat="server" TabIndex="2" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            %
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="2" CssClass="TransactionalButton btn btn-primary"
                                OnClientClick="javascript:return EmpServiceChargeSave()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
