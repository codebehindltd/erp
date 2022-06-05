<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmPayrollHoliday.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmPayrollHoliday" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Administrative & Security</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Holiday Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtHolidayStartDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtHolidayEndDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtHolidayEndDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtHolidayStartDate").datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtStartDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtEndDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtEndDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtStartDate").datepicker("option", "maxDate", selectedDate);
                }
            });
            $("#<%=txtHolidayStartDate.ClientID %>").blur(function () {

                var date = $("#<%=txtHolidayStartDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtHolidayStartDate.ClientID %>").focus();
                        $("#<%=txtHolidayStartDate.ClientID %>").val("");
                        return false;
                    }
                }
            });$("#<%=txtHolidayEndDate.ClientID %>").blur(function () {

                var date = $("#<%=txtHolidayEndDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtHolidayEndDate.ClientID %>").focus();
                        $("#<%=txtHolidayEndDate.ClientID %>").val("");
                        return false;
                    }
                }
            });$("#<%=txtStartDate.ClientID %>").blur(function () {

                var date = $("#<%=txtStartDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtStartDate.ClientID %>").focus();
                        $("#<%=txtStartDate.ClientID %>").val("");
                        return false;
                    }
                }
            });$("#<%=txtEndDate.ClientID %>").blur(function () {

                var date = $("#<%=txtEndDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtEndDate.ClientID %>").focus();
                        $("#<%=txtEndDate.ClientID %>").val("");
                        return false;
                    }
                }
            });

        });

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            $("#<%=txtHolidayId.ClientID %>").val(result.HolidayId);
            $("#<%=txtHolidayStartDate.ClientID %>").val(result.StartDate);
            $("#<%=txtHolidayEndDate.ClientID %>").val(result.EndDate);
            $("#<%=txtDescription.ClientID %>").val(result.Description);

            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewBank').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }


        function OnFillFormObjectFailed(error) {
            alert(error.get_message());
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
            window.location = "frmPayrollHoliday.aspx?DeleteConfirmation=Deleted"
        }
        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtHolidayId.ClientID %>").val('');
            $("#<%=txtHolidayName.ClientID %>").val('');
            $("#<%=txtHolidayStartDate.ClientID %>").val('');
            $("#<%=txtHolidayEndDate.ClientID %>").val('');
            $("#<%=txtDescription.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").text("Save");
            return false;
        }

        function PerformClearActionWithConfirmation() {
             if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }
        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewBank').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewBank').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
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
        function SaveValidation() {
            
            var name = $("#<%=txtHolidayName.ClientID %>").val();
            var StartDate = $("#<%=txtHolidayStartDate.ClientID %>").val();
            var EndDate = $("#<%=txtHolidayEndDate.ClientID %>").val();
            var Description = $("#<%=txtDescription.ClientID %>").val();  
            if (name == "") {
                toastr.warning("Enter Holiday Name");
                $("#ContentPlaceHolder1_txtHolidayName").focus();
                return false;
            }
            if (StartDate == "") {
                toastr.warning("Enter Holiday Start Date");
                $("#ContentPlaceHolder1_txtHolidayStartDate").focus();
                return false;
            }
            if (EndDate == "") {
                toastr.warning("Enter Holiday End Date");
                $("#ContentPlaceHolder1_txtHolidayEndDate").focus();
                return false;
            } if (Description == "") {
                toastr.warning("Enter Description");
                $("#ContentPlaceHolder1_txtDescription").focus();
                return false;
            }
            
        }
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Holiday Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Holiday </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Holiday Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblHolidayName" runat="server" class="control-label required-field" Text="Holiday Name"></asp:Label>                                
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtHolidayName" runat="server" TabIndex="1" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtHolidayId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblHolidayStartDate" runat="server" class="control-label required-field" Text="Holiday Start Date"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtHolidayStartDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblHolidayEndDate" runat="server" class="control-label required-field" Text="Holiday End Date"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtHolidayEndDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDescription" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="form-control"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="3" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return SaveValidation();" OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="4" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearActionWithConfirmation();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Holiday Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblStartDate" runat="server" class="control-label" Text="Start Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblEndDate" runat="server" class="control-label" Text="End Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvPayrollHoliday" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" OnPageIndexChanging="gvPayrollHoliday_PageIndexChanging"
                        OnRowCommand="gvPayrollHoliday_RowCommand" OnRowDataBound="gvPayrollHoliday_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("HolidayId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="HolidayName" HeaderText="Holiday" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Start Date " ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("StartDate")))%>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="End Date " ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblToDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("EndDate")))%>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="40%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("HolidayId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" OnClientClick="return confirm('Do you want to Edit?');" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("HolidayId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
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
            if (parseInt(xNewAdd) == 2) {
                $('#btnNewBank').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }
        
    </script>
</asp:Content>
