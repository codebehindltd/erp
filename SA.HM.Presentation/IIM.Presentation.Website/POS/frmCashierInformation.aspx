<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmCashierInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmCashierInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        
        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Cashier Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/
            
            $("#ContentPlaceHolder1_ddlEmpId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var txtToDate = '<%=txtToDate.ClientID%>'
            var txtFromDate = '<%=txtFromDate.ClientID%>'
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

            var txtSFromDate = '<%=txtSFromDate.ClientID%>'
            var txtSToDate = '<%=txtSToDate.ClientID%>'
            $('#ContentPlaceHolder1_txtSFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSToDate').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtSToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtProbableFromHour").blur(function () {
                var hourString = $("#<%=txtProbableFromHour.ClientID %>").val();
                if ($.trim(hourString) == "") {
                    $("#ContentPlaceHolder1_txtProbableFromHour").val("12");
                    return;
                }

                var hour = parseInt(hourString);
                if (hour > 12) {
                    hour = hour % 12;
                    $("#<%=txtProbableFromHour.ClientID %>").val(hour);
                    $("#<%=ddlProbableFromAMPM.ClientID %>").val('PM');
                }
                else {
                    $("#<%=ddlProbableFromAMPM.ClientID %>").val('AM');
                }
            });

            $("#ContentPlaceHolder1_txtProbableToHour").blur(function () {
                var hourString = $("#<%=txtProbableToHour.ClientID %>").val();

                if ($.trim(hourString) == "") {
                    $("#ContentPlaceHolder1_txtProbableToHour").val("12");
                    return;
                }

                var hour = parseInt(hourString);
                if (hour > 12) {
                    hour = hour % 12;
                    $("#<%=txtProbableToHour.ClientID %>").val(hour);
                    $("#<%=ddlProbableToAMPM.ClientID %>").val('PM');
                }
                else {
                    $("#<%=ddlProbableToAMPM.ClientID %>").val('AM');
                }
            });

            $("#ContentPlaceHolder1_txtProbableFromMinute").blur(function () {

                var minuteString = $("#<%=txtProbableFromMinute.ClientID %>").val();

                if ($.trim(minuteString) == "") {
                    $("#ContentPlaceHolder1_txtProbableFromMinute").val("00");
                    return;
                }
            });

            $("#ContentPlaceHolder1_txtProbableToMinute").blur(function () {

                var minuteString = $("#<%=txtProbableToMinute.ClientID %>").val();

                if ($.trim(minuteString) == "") {
                    $("#ContentPlaceHolder1_txtProbableToMinute").val("00");
                    return;
                }
            });
            $("[id=ContentPlaceHolder1_gvCostCenterInfo_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

        });

        //For Password Validation-------------------------
        function confirmPass() {
            if ($("#<%=txtUserPassword.ClientID %>").val() != $("#<%=txtUserConfirmPassword.ClientID %>").val()) {
                toastr.warning('Wrong confirm password !');
                $("#<%=txtUserConfirmPassword.ClientID %>").focus();
            }
        }

        //----Numeric Validation-----------------------
        function fixedlength(textboxID, keyEvent, maxlength) {
            //validation for digits upto 'maxlength' defined by caller function
            if (textboxID.value.length > maxlength) {
                textboxID.value = textboxID.value.substr(0, maxlength);
            }
            else if (textboxID.value.length < maxlength || textboxID.value.length == maxlength) {
                textboxID.value = textboxID.value.replace(/[^\d]+/g, '');
                return true;
            }
            else
                return false;
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {

            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=ddlEmpId.ClientID %>").val(result.EmpId);
            var fromDate = new Date(result.FromDate);
            $("#<%=txtFromDate.ClientID %>").val(GetStringFromDateTime(result.FromDate));
            var toDate = new Date(result.ToDate);
            $("#<%=txtToDate.ClientID %>").val(GetStringFromDateTime(result.ToDate));
            
            
            $("#<%=btnSave.ClientID %>").val("Update");
            return false;
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For Delete-------------------------        
        function PerformDeleteAction(actionId) {

            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearActionForButton() {
            if (!confirm("Do you want to clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function PerformClearAction() {

            $("#<%=ddlActiveStat.ClientID %>").val('---None---');
            $("#<%=ddlEmpId.ClientID %>").val('---None---');
            $("#<%=txtFromDate.ClientID %>").val('');
            $("#<%=txtToDate.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewBank').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewBank').hide("slow");
        }

        $(function () {
            $("#myTabs").tabs();
        });

    </script>
    <asp:HiddenField ID="hfEmpId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsbearar" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsChef" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">User Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search User </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    User Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <%--<div class="col-md-2">                                
                                <asp:Label ID="lblEmpId" runat="server" class="control-label" Text="User Name"></asp:Label>
                            </div>--%>
                            <asp:HiddenField ID="txtBearerId" runat="server"></asp:HiddenField>
                            <label for="UserName" class="control-label col-md-2">
                                User Name</label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlEmpId" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <%--<div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblCostCenterId" runat="server" Text="Cost Center"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:DropDownList ID="ddlCostCenterId" runat="server" CssClass="ThreeColumnDropDownList"
                                TabIndex="1">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>--%>
                        <div class="form-group">
                            <%--<div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>--%>
                            <label for="FromDate" class="control-label col-md-2">
                                From Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                            <%--<div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Probable Time"></asp:Label>
                            </div>--%>
                            <label for="ProbableTime" class="control-label col-md-2">
                                Probable Time</label>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtProbableFromHour" placeholder="12" CssClass="form-control" runat="server"
                                            TabIndex="2"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3 col-padding-left-none">
                                        <asp:TextBox ID="txtProbableFromMinute" placeholder="00" CssClass="form-control"
                                            TabIndex="3" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3 col-padding-left-none">
                                        <asp:DropDownList ID="ddlProbableFromAMPM" CssClass="form-control" runat="server"
                                            TabIndex="4">
                                            <asp:ListItem>AM</asp:ListItem>
                                            <asp:ListItem>PM</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    (12:00AM)
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <%--<div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>--%>
                            <label for="ToDate" class="control-label col-md-2">
                                To Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                            <%--<div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Probable Time"></asp:Label>
                            </div>--%>
                            <label for="ProbableTime" class="control-label col-md-2">
                                Probable Time</label>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtProbableToHour" placeholder="12" CssClass="form-control" runat="server"
                                            TabIndex="2"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3 col-padding-left-none">
                                        <asp:TextBox ID="txtProbableToMinute" placeholder="00" CssClass="form-control" TabIndex="3"
                                            runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3 col-padding-left-none">
                                        <asp:DropDownList ID="ddlProbableToAMPM" CssClass="form-control" runat="server" TabIndex="4">
                                            <asp:ListItem>AM</asp:ListItem>
                                            <asp:ListItem>PM</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    (12:00AM)
                                </div>
                            </div>
                        </div>
                        <div style="display: none;">
                            <div class="form-group">
                                <%--<div class="col-md-2">
                                    <asp:Label ID="lblUserPassword" runat="server" class="control-label" Text="Password"></asp:Label>
                                </div>--%>
                                <label for="Password" class="control-label col-md-2">
                                    Password</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtUserPassword" TextMode="Password" class="numeric" CssClass="form-control"
                                        TabIndex="4" runat="server" onblur="return fixedlength(this, event, 50);" onkeypress="return fixedlength(this, event, 50);"
                                        onkeyup="return fixedlength(this, event, 50);"></asp:TextBox>
                                </div>
                                <%--<div class="col-md-2">
                                    <asp:Label ID="lblUserConfirmPassword" runat="server" class="control-label" Text="Confirm Pass."></asp:Label>
                                </div>--%>
                                <label for="ConfirmPass" class="control-label col-md-2">
                                    Confirm Pass.</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtUserConfirmPassword" runat="server" onblur="javascript: return confirmPass();"
                                        CssClass="form-control" TabIndex="5" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="BillSettlement" class="control-label col-md-2">
                                Bill Settlement</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsRestaurantBillCanSettle" runat="server" CssClass="form-control"
                                    TabIndex="6">
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <label class="control-label col-md-2">Item Search Enable</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsItemSearchEnable" runat="server" CssClass="form-control"
                                    TabIndex="6">
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="IsItemCanEditDelete" class="control-label col-md-2">Item Can Edit & Delete</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsItemCanEditDelete" runat="server" CssClass="form-control" TabIndex="6">
                                    <asp:ListItem Value="">---Please Select---</asp:ListItem>
                                    <asp:ListItem Value="1">Can Edit & Delete</asp:ListItem>
                                    <asp:ListItem Value="0">Can Not Edit & Delete</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <label for="IsItemCanEditDelete" class="control-label col-md-2">User Type</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsCashierOrWaiter" runat="server" CssClass="form-control" TabIndex="6">
                                    <asp:ListItem Value="0">Cashier</asp:ListItem>
                                    <asp:ListItem Value="1">Waiter</asp:ListItem>
                                    <asp:ListItem Value="2">Chef</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Status" class="control-label col-md-2">
                                Status</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control" TabIndex="6">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div id="CostCenterInformationDiv" class="panel panel-default">
                            <div class="panel-body">
                                <div>
                                    <asp:GridView ID="gvCostCenterInfo" Width="100%" runat="server" AllowPaging="True"
                                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                        ForeColor="#333333" PageSize="200" CssClass="table table-bordered table-condensed table-responsive">
                                        <RowStyle BackColor="#E3EAEB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCostCentreId" runat="server" Text='<%#Eval("CostCenterId") %>'></asp:Label>
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
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary btn-sm"
                                    TabIndex="7" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="8" Text="Clear" CssClass="btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearActionForButton();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Search User Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <%--<div class="col-md-2">                                
                                <asp:Label ID="lblSName" runat="server" class="control-label" Text="User Name"></asp:Label>
                            </div>--%>
                            <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                            <label for="UserName" class="control-label col-md-2">
                                User Name</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <%--<div class="col-md-2">
                                <asp:Label ID="lblSFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>--%>
                            <label for="FromDate" class="control-label col-md-2">
                                From Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSFromDate" runat="server" CssClass="form-control"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                            <%--<div class="col-md-2">
                                <asp:Label ID="lblSToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>--%>
                            <label for="ToDate" class="control-label col-md-2">
                                To Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSToDate" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="IsItemCanEditDelete" class="control-label col-md-2">User Type</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSIsCashierOrBearer" runat="server" CssClass="form-control" TabIndex="6">
                                    <asp:ListItem Value="-1">---All---</asp:ListItem>
                                    <asp:ListItem Value="0">Cashier</asp:ListItem>
                                    <asp:ListItem Value="1">Waiter</asp:ListItem>
                                    <asp:ListItem Value="2">Chef</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <label for="Status" class="control-label col-md-2">
                                Status</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control" TabIndex="4">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    CssClass="btn btn-primary btn-sm" TabIndex="5" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvBearerInformation" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="200" OnPageIndexChanging="gvBearerInformation_PageIndexChanging"
                        OnRowDataBound="gvBearerInformation_RowDataBound" OnRowCommand="gvBearerInformation_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("UserInfoId") %>'></asp:Label>
                                    <asp:Label ID="lblIsBearer" runat="server" Text='<%#Eval("IsBearer") %>'></asp:Label>
                                    <asp:Label ID="lblIsChef" runat="server" Text='<%#Eval("IsChef") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UserName" HeaderText="User Name" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IsBearerStatus" HeaderText="Waiter/Cashier/Chef" ItemStyle-Width="35%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" OnClientClick="return confirm('Do you want to edit?');" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("UserInfoId") %>' ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                        ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("UserInfoId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to delete?');" />
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
    <script type="text/javascript">

        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
        }
        else {
            NewAddButtonPanelHide();
            $('#EntryPanel').hide("slow");
        }

    </script>
</asp:Content>
