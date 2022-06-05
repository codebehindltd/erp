<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpKotBillDetail.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmEmpKotBillDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Shop</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Job assignment and Feedback</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var txtFromDate = '<%=txtStartDate.ClientID%>'
            var txtToDate = '<%=txtDeliveryDate.ClientID%>'

            $('#ContentPlaceHolder1_txtStartDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtDeliveryDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtDeliveryDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtStartDate').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtSrcStartDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSrcToDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtSrcToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSrcStartDate').datepicker("option", "minDate", selectedDate);
                }
            });
        });

        function CheckValidation() {
            var tableNumber = $('#<%=txtBillNumber.ClientID%>').val();
            if ($.trim(tableNumber) == "") {
                toastr.warning("Bill Numbe must not empty");
                return false;
            }
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function alert2(message, title, buttonText) {

            buttonText = (buttonText == undefined) ? "Ok" : buttonText;
            title = (title == undefined) ? "The page says:" : title;

            var div = $('<div>');
            div.html(message);
            div.attr('title', title);
            div.dialog({
                autoOpen: true,
                modal: true,
                draggable: false,
                resizable: false,
                buttons: [{
                    text: buttonText,
                    click: function () {
                        $(this).dialog("close");
                        div.remove();
                    }
                }]
            });
        }


        function OnFillFormObjectSucceeded(result) {
            if (result.StatusId != 2) {
                $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
                $("#<%=btnSave.ClientID %>").val("Update");
            }
            else {
                toastr.info("This is not edditable.");
            }
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
        function PerformClearAction() {
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <div style="height: 45px">
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Job Assign</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Job Feedback</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Job Assignment Information</div>
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
                        <div class="form-group">
                            <%--<div class="col-md-2">
                                <asp:Label ID="lblBillNumber" runat="server" class="control-label" Text="Bill Number"></asp:Label>
                            </div>--%>
                            <label for="BillNumber" class="control-label col-md-2">
                                Bill Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBillNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnSearchBill" runat="server" TabIndex="4" Text="Search" CssClass="btn btn-primary"
                                    OnClick="btnSearchBill_Click" />
                            </div>
                            <div class="col-md-2">
                            </div>
                        </div>
                        <asp:Panel ID="pnlJobAssignmentInfo" runat="server">
                            <div id="ItemInformationDiv" class="panel panel-default">
                                <div class="panel-body">
                                    <div>
                                        <asp:GridView ID="gvItemInformation" Width="100%" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                            ForeColor="#333333" PageSize="200" OnRowDataBound="gvItemInformation_RowDataBound"
                                            CssClass="table table-bordered table-condensed table-responsive">
                                            <RowStyle BackColor="#E3EAEB" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpId" runat="server" Text='<%#Eval("EmpId") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="KotId" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblKotId" runat="server" Text='<%#Eval("KotId") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="KotDetailId" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblKotDetailId" runat="server" Text='<%#Eval("KotDetailId") %>'></asp:Label></ItemTemplate>
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
                                                <asp:TemplateField HeaderText="Item Information" ItemStyle-Width="55%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvItemName" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>
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
                            <div class="form-group">
                                <%--<div class="col-md-2">                                    
                                    <asp:Label ID="lblStartDate" runat="server" class="control-label" Text="Start Date"></asp:Label>
                                </div>--%>
                                <asp:HiddenField ID="txtTableId" runat="server"></asp:HiddenField>
                                <label for="StartDate" class="control-label col-md-2">
                                    Start Date</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </div>
                                <%--<div class="col-md-2">
                                    <asp:Label ID="lblDeliveryDate" runat="server" class="control-label" Text="Delivery Date"></asp:Label>
                                </div>--%>
                                <label for="DeliveryDate" class="control-label col-md-2">
                                    Delivery Date</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDeliveryDate" TabIndex="1" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <%-- <div class="col-md-2">
                                    <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                                </div>--%>
                                <label for="Remarks" class="control-label col-md-2">
                                    Remarks</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                        TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSave" runat="server" TabIndex="4" Text="Save" CssClass="btn btn-primary"
                                        OnClientClick="javascript:return CheckValidation();" OnClick="btnSave_Click" />
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Search Job Feedback</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <%--<div class="col-md-2">
                                <asp:Label ID="lblSrcUserName" runat="server" class="control-label" Text="User Name"></asp:Label>
                            </div>--%>
                            <label for="UserName" class="control-label col-md-2">
                                User Name</label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSrcEmpId" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <%--<div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Start Date"></asp:Label>
                            </div>--%>
                            <label for="StartDate" class="control-label col-md-2">
                                Start Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcStartDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <%--<div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>--%>
                            <label for="ToDate" class="control-label col-md-2">
                                To Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcToDate" TabIndex="1" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <%--<div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Search Type"></asp:Label>
                            </div>--%>
                            <label for="SearchType" class="control-label col-md-2">
                                Search Type</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="Assignment">Assignment Date</asp:ListItem>
                                    <asp:ListItem Value="Delivery">Delivery Date</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <%--<div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Feedback Status"></asp:Label>
                            </div>--%>
                            <label for="FeedbackStatus" class="control-label col-md-2">
                                Feedback Status</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDeliveryStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                    <asp:ListItem Value="Delivered">Delivered</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" TabIndex="4" Text="Search" CssClass="btn btn-primary"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div>
                        <div class="panel-body">
                            <asp:GridView ID="gvTableNumber" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="30"
                                OnPageIndexChanging="gvTableNumber_PageIndexChanging" OnRowDataBound="gvTableNumber_RowDataBound"
                                CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="DetailId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDetailId" runat="server" Text='<%#Eval("DetailId") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="KotDetailId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblKotDetailId" runat="server" Text='<%#Eval("KotDetailId") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BillNumber" HeaderText="Bill Number" ItemStyle-Width="20%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-Width="70%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="JobStartDateString" HeaderText="Assign Date" ItemStyle-Width="35%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="JobEndDateString" HeaderText="Delivery Date" ItemStyle-Width="40%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlJobStatus" runat="server" Width="150px">
                                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                                <asp:ListItem Value="Delivered">Delivered</asp:ListItem>
                                                <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                            </asp:DropDownList>
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
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSaveFeedback" runat="server" TabIndex="4" Text="Save" CssClass="btn btn-primary"
                                    OnClick="btnSaveFeedback_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
