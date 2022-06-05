<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBank.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmBank" %>

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
            $("#ContentPlaceHolder1_ddlBankAccounts").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlBankAccounts").change(function () {
                if ($("#ContentPlaceHolder1_ddlBankAccounts").val() != "0") {
                    $("#ContentPlaceHolder1_txtBankName").val($("#ContentPlaceHolder1_ddlBankAccounts option:selected").text());
                }
            });

            if ($("#ContentPlaceHolder1_hfIsBankIntegratedWithAccounts").val() == '1') {

                $("#AccountForBank").show();
            }
            else {
                $("#AccountForBank").hide();
                $("#<%=ddlBankAccounts.ClientID %>").val(0);
            }

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
            $("#<%=txtBankName.ClientID %>").val(result.BankName);
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=txtBankId.ClientID %>").val(result.BankId);
            $("#<%=btnSave.ClientID %>").val("Update");
            //$('#btnNewBank').hide("slow");
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
            $("#<%=txtBranchName.ClientID %>").val('');
            $("#<%=txtAccountName.ClientID %>").val('');
            $("#<%=txtAccountNumber.ClientID %>").val('');
            $("#<%=txtAccountType.ClientID %>").val('');
            $("#<%=txtRemarksForBankInfo.ClientID %>").val('');
            $("#<%=ddlBankAccounts.ClientID %>").val(0);
            $("#<%=txtBankName.ClientID %>").val('');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=txtBankId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }
        $(function () {
            $("#myTabs").tabs();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#gvGustIngormation tbody tr").length;

            var bankName = $("#<%=txtSBankName.ClientID %>").val();
            var activeStat = $("#<%=ddlSActiveStat.ClientID %>").val();

            if (activeStat == 0)
                activeStat = true;
            else
                activeStat = false;

            PageMethods.SearchBankAndLoadGridInformation(bankName, activeStat, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
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

            //table row color 
            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#gvGustIngormation tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:64%; cursor:pointer;\">" + gridObject.BankName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.ActiveStatus + "</td>";

                //SavePermission
                if ($("#<%=IsBankSavePermission.ClientID %>").val() == "1") {
                    tr += "<td align='right' style=\"width:8%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.BankId + "')\" alt='Edit Information' border='0' /></td>";
                }
                else {
                    tr += "<td align='right' style=\"width:8%; cursor:pointer;\">&nbsp;</td>";
                }
                //DeletePermission
                if ($("#<%=IsBankDeletePermission.ClientID %>").val() == "1") {
                    tr += "<td align='right' style=\"width:8%; cursor:pointer;\"><img src='../Images/delete.png' class= 'BankDelete'  alt='Delete Information' border='0' /></td>";
                }
                else {
                    tr += "<td align='right' style=\"width:8%; cursor:pointer;\">&nbsp;</td>";
                }

                tr += "<td align='right' style=\"width:8%; display:none;\">" + gridObject.BankId + "</td>";

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
            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function PerformEditAction(bankId) {
            PageMethods.LoadDetailInformation(bankId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            return false;
        }
        function OnLoadDetailObjectSucceeded(result) {
            debugger;
            $("#<%=txtBranchName.ClientID %>").val(result.BranchName);
            $("#<%=txtAccountName.ClientID %>").val(result.AccountName);
            $("#<%=txtAccountNumber.ClientID %>").val(result.AccountNumber);
            $("#<%=txtAccountType.ClientID %>").val(result.AccountType);
            $("#<%=txtRemarksForBankInfo.ClientID %>").val(result.Description);
            $("#<%=ddlBankAccounts.ClientID %>").val(result.BankHeadId).trigger('change');

            $("#<%=txtBankName.ClientID %>").val(result.BankName);

            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=txtBankId.ClientID %>").val(result.BankId);
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
                href="#tab-1">Bank Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Bank</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Bank Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" id="AccountForBank">
                            <div class="col-md-2 ">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Cash at Bank"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlBankAccounts" runat="server" CssClass="form-control"
                                    TabIndex="6">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtFocusTabControl" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtBankId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hfIsBankIntegratedWithAccounts" runat="server" Value="0"></asp:HiddenField>
                                <asp:HiddenField ID="IsBankSavePermission" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="IsBankDeletePermission" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblBankName" runat="server" class="control-label required-field" Text="Bank Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblBranchName" runat="server" class="control-label" Text="Branch Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBranchName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAccountName" runat="server" class="control-label" Text="Account Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAccountName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblAccountNumber" runat="server" class="control-label" Text="Account Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAccountNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAccountType" runat="server" class="control-label" Text="Account Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAccountType" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label7" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarksForBankInfo" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
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
                    Bank Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSBankName" runat="server" class="control-label" Text="Bank Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSBankName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
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
                    <table id='gvGustIngormation' class="table table-bordered table-condensed table-responsive" width="100%">
                        <colgroup>
                            <col style="width: 50%;" />
                            <col style="width: 20%;" />
                            <col style="width: 15%;" />
                            <col style="width: 15%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Bank Name
                                </td>
                                <td>Status
                                </td>
                                <td style="text-align: right;">Edit
                                </td>
                                <td style="text-align: right;">Delete
                                </td>
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
        <%--var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
            if (parseInt(xNewAdd) == 2) {
                //$('#btnNewBank').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }--%>        
    </script>
</asp:Content>
