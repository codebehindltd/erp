<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmMachineTest.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmMachineTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var vv = [];
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Bank Name</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_txtTestDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

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

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("#gvGustIngormation").delegate("td > img.BankDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var bankId = $.trim($(this).parent().parent().find("td:eq(4)").text());
                    var params = JSON.stringify({ sEmpId: bankId });

                    var $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/HMCommon/frmBank.aspx/DeleteData",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            CommonHelper.AlertMessage(data.d.AlertMessage);
                            $row.remove();
                            $("#myTabs").tabs('load', 1);
                        },
                        error: function (error) {
                        }
                    });
                }
            });
        });

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            $("#<%=txtRemarks.ClientID %>").val(result.BankName);
            $("#<%=txtTestId.ClientID %>").val(result.BankId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewBank').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For Delete-------------------------        
        function PerformDeleteAction(actionId, rowIndex) {
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
            toastr.error(error);
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#<%=txtTestId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
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

       
        $(function () {
            $("#myTabs").tabs();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#gvGustIngormation tbody tr").length;
            var costCenterId = $("#<%=ddlSCostCenter.ClientID %>").val();
            var fromDate = $("#<%=txtFromDate.ClientID %>").val();
            var toDate = $("#<%=txtToDate.ClientID %>").val();

            PageMethods.SearchMachineTestAndLoadGridInformation(costCenterId, fromDate, toDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvGustIngormation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvGustIngormation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#gvGustIngormation tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.BeforeMachineReadNumber + "</td>";
                tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.TestQuantity + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.AfterMachineReadNumber + "</td>";
                tr += "<td align='left' style=\"width:50%; cursor:pointer;\">" + gridObject.Remarks + "</td>";

                /*if ($("#<%=IsBankSavePermission.ClientID %>").val() == "1") {
                    tr += "<td align='right' style=\"width:8%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.BankId + "')\" alt='Edit Information' border='0' /></td>";
                }
                else {
                    tr += "<td align='right' style=\"width:8%; cursor:pointer;\">&nbsp;</td>";
                }

                if ($("#<%=IsBankDeletePermission.ClientID %>").val() == "1") {
                    tr += "<td align='right' style=\"width:8%; cursor:pointer;\"><img src='../Images/delete.png' class= 'BankDelete'  alt='Delete Information' border='0' /></td>";
                }
                else {
                    tr += "<td align='right' style=\"width:8%; cursor:pointer;\">&nbsp;</td>";
                }*/

                tr += "<td align='right' style=\"width:8%; display:none;\">" + gridObject.TestId + "</td>";

                tr += "</tr>"

                $("#gvGustIngormation tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();
            if (currentPageNumber == "") {
                currentPageNumber = 1;
            }
            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function PerformEditAction(bankId) {
            PageMethods.LoadDetailInformation(bankId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            return false;
        }
        function OnLoadDetailObjectSucceeded(result) {
            $("#<%=txtRemarks.ClientID %>").val(result.BankName);
            $("#<%=txtTestId.ClientID %>").val(result.BankId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $("#myTabs").tabs({ active: 0 });

            return false;
        }
        function OnLoadDetailObjectFailed(error) {
            toastr.error(error.get_message());
        }
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Machine Test</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Test</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Machine Test Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtFocusTabControl" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtTestId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="IsBankSavePermission" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="IsBankDeletePermission" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblCostCenter" runat="server" class="control-label required-field"
                                    Text="Machine Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTestDate" runat="server" class="control-label" Text="Test Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTestDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTestQuantity" runat="server" class="control-label" Text="Test Quantity"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTestQuantity" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="form-control"
                                    TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="3" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="4" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Machine Test Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSCostCenter" runat="server" class="control-label" Text="Machine Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSCostCenter" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm">
                                    Search</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>                
                <div class="panel-body">
                    <table id='gvGustIngormation' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="width: 15%;" />
                            <col style="width: 10%;" />
                            <col style="width: 15%;" />
                            <col style="width: 50%;" />
                           <%-- <col style="width: 10%;" />--%>
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>
                                    Before Test
                                </td>
                                <td>
                                    Test Quantity
                                </td>
                                <td>
                                    After Test
                                </td>
                                <td>
                                    Description
                                </td>
                               <%-- <td style="text-align: right;">
                                    Edit
                                </td>
                                <td style="text-align: right;">
                                    Delete
                                </td>--%>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="childDivSection">
                        <div class="text-center" id="GridPagingContainer">
                            <ul class="pagination">
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="divClear">
    </div>
</asp:Content>
