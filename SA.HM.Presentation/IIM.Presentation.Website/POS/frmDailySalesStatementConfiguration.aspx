<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmDailySalesStatementConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmDailySalesStatementConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var vv = [];
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Daily Sales Statement Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            CommonHelper.ApplyDecimalValidation();
            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $('#ContentPlaceHolder1_txtStartDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtEndDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtEndDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtStartDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtSStartDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSEndDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtSEndDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSStartDate').datepicker("option", "maxDate", selectedDate);
                }
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
            //alert(error.get_message());
            toastr.error(error);
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=btnSave.ClientID %>").val("Save");
            $("#ContentPlaceHolder1_txtStartDate").val("");
            $("#ContentPlaceHolder1_txtEndDate").val("");
            $("#ContentPlaceHolder1_txtPercentageAmount").val("");
            $("#ContentPlaceHolder1_hfDailySalesStatementId").val("");

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

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#gvGustIngormation tbody tr").length;
            var dateFrom = $("#ContentPlaceHolder1_txtSStartDate").val();
            var dateTo = $("#ContentPlaceHolder1_txtSEndDate").val();
            PageMethods.SearchtDailySalesStatementConfiguration(dateFrom, dateTo, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
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

                tr += "<td align='left' style=\"width:30%; cursor:pointer;\">" + GetStringFromDateTime(gridObject.DateFrom) + "</td>";
                tr += "<td align='left' style=\"width:30%; cursor:pointer;\">" + GetStringFromDateTime(gridObject.DateTo) + "</td>";
                tr += "<td align='right' style=\"width:25%;\">" + gridObject.PercentageAmount + "</td>";
                tr += "<td style=\"width:15%; text-align:right; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.DailySalesStatementId + "')\" alt='Edit Information' border='0' /></td>";
                tr += "</tr>"

                $("#gvGustIngormation tbody").append(tr);
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
            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function PerformEditAction(dailySalesStatementId) {
            PageMethods.FillForm(dailySalesStatementId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            return false;
        }
        function OnLoadDetailObjectSucceeded(result) {

            $("#myTabs").tabs({ active: 0 });

             $("#<%=btnSave.ClientID %>").val("Update");

            $("#ContentPlaceHolder1_txtStartDate").val(GetStringFromDateTime(result.DateFrom));
            $("#ContentPlaceHolder1_txtEndDate").val(GetStringFromDateTime(result.DateTo));
            $("#ContentPlaceHolder1_txtPercentageAmount").val(result.PercentageAmount);
            $("#ContentPlaceHolder1_hfDailySalesStatementId").val(result.DailySalesStatementId);

            return false;
        }
        function OnLoadDetailObjectFailed(error) {
            toastr.error(error.get_message());
        }
    </script>

    <asp:HiddenField ID="hfDailySalesStatementId" runat="server" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Daily Sales Statement Configuration</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Daily Sales Statement Configuration Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Sales Statement Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Percentage(%)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPercentageAmount" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
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
                    Sales Statement Configuration
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSStartDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSEndDate" CssClass="form-control" runat="server"></asp:TextBox>
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
                    Search Information
                </div>
                <div class="panel-body">
                    <table id='gvGustIngormation' class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 30%;">Date From
                                </th>
                                <th style="width: 30%;">Date To
                                </th>
                                <th style="width: 25%;">Percentage Amount
                                </th>
                                <th style="text-align: right; width: 15%;">Edit
                                </th>
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
