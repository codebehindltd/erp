<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmDisciplinaryAction.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmDisciplinaryAction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Disciplinary Action</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlEmployeeId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSEmployeeId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("#gvDisciplinaryAction").delegate("td > img.DisActionDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var actionId = $.trim($(this).parent().parent().find("td:eq(3)").text());
                    var params = JSON.stringify({ sActionId: actionId });

                    var $row = $(this).parent().parent();
                    //$(this).parent().parent().remove();
                    $.ajax({
                        type: "POST",
                        url: "/Payroll/frmDisciplinaryAction.aspx/DeleteData",
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

            $("#ContentPlaceHolder1_txtApplicableDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtStartDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtEndDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        });

        $(function () {
            $("#myTabs").tabs();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvDisciplinaryAction tbody tr").length;

            var actionType = $("#<%=ddlSActionTypeId.ClientID %>").val();
            var actionReason = $("#<%=ddlSActionreasonId.ClientID %>").val();
            var emp = $("#<%=ddlSEmployeeId.ClientID %>").val();
            var proposedAction = $("#<%=ddlSProposedActionId.ClientID %>").val();
            var fromDate = $("#ContentPlaceHolder1_txtStartDate").val();
            var toDate = $("#ContentPlaceHolder1_txtEndDate").val();
            debugger;
            if (fromDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtStartDate").val(), '/');
            }
            if (toDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtEndDate").val(), '/');
            }

            PageMethods.SearchDisciplinaryAction(actionType, actionReason, emp, proposedAction, fromDate, toDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }

        function OnLoadObjectSucceeded(result) {

            $("#gvDisciplinaryAction tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvDisciplinaryAction tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvDisciplinaryAction tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:50%; cursor:pointer;\">" + gridObject.EmpName + "</td>";
                tr += "<td align='left' style=\"width:30%; cursor:pointer;\">" + gridObject.ProposedAction + "</td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\">";

                if (IsCanEdit) {
                    tr += "<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.DisciplinaryActionId + "')\" alt='Edit Information' border='0' /> " + " ";
                }
                if (IsCanDelete) {
                    tr += "<img src='../Images/delete.png' class= 'DisActionDelete'  alt='Delete Information' border='0'/>";
                }
                tr += "</td >";
                //tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/delete.png' class= 'DisActionDelete'  alt='Delete Information' border='0' /></td>";
                tr += "<td align='right' style=\"width:10%; display:none;\">" + gridObject.DisciplinaryActionId + "</td>";

                tr += "</tr>"

                $("#gvDisciplinaryAction tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);


            return false;
        }
        function OnLoadObjectFailed(error) {
            alert(error.get_message());
        }

        function PerformEditAction(actionId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            var possiblePath = "frmDisciplinaryAction.aspx?editId=" + actionId;
            window.location = possiblePath;
        }

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Disciplinary Action</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Disciplinary Action</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Disciplinary Action Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:HiddenField ID="hfDisciplinaryActionId" runat="server"></asp:HiddenField>
                            <div class="col-md-2">
                                <asp:Label ID="lblActionReason" runat="server" class="control-label required-field" Text="Action Reason"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActionReasonId" runat="server" CssClass="form-control"
                                    TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblActionType" runat="server" class="control-label required-field" Text="Action Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActionTypeId" runat="server" CssClass="form-control"
                                    TabIndex="2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmployee" runat="server" class="control-label required-field" Text="Employee"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlEmployeeId" runat="server" CssClass="form-control"
                                    TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblProposedAction" runat="server" class="control-label" Text="Proposed Action"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlProposedAction" runat="server" CssClass="form-control"
                                    TabIndex="2">
                                    <%--<asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem Value="Incremenr HoldUp">Increment HoldUp</asp:ListItem>
                                <asp:ListItem Value="Promotion HoldUp">Promotion HoldUp</asp:ListItem>
                                <asp:ListItem Value="Suspention">Suspention</asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActionBody" runat="server" class="control-label" Text="Action Body"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtActionBody" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblApplicableDate" runat="server" class="control-label required-field" Text="Applicable Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtApplicableDate" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="4" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" OnClientClick="return confirm('Do you want to clear?');" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Disciplinary Action Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSActionType" runat="server" class="control-label" Text="Action Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActionTypeId" runat="server" CssClass="form-control"
                                    TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSActionreason" runat="server" class="control-label" Text="Action Reason"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActionreasonId" runat="server" CssClass="form-control"
                                    TabIndex="2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSEmployee" runat="server" class="control-label" Text="Employee"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSEmployeeId" runat="server" CssClass="form-control"
                                    TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSProposedAction" runat="server" class="control-label" Text="Proposed Action"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSProposedActionId" runat="server" CssClass="form-control"
                                    TabIndex="2">
                                    <%--<asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem Value="Incremenr HoldUp">Increment HoldUp</asp:ListItem>
                                <asp:ListItem Value="Promotion HoldUp">Promotion HoldUp</asp:ListItem>
                                <asp:ListItem Value="Suspention">Suspention</asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary">
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
                    <table id='gvDisciplinaryAction' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="width: 50%;" />
                            <col style="width: 30%;" />
                            <col style="width: 20%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Employee Name
                                </td>
                                <td>Proposed Action
                                </td>
                                <td style="text-align: right;">Actions
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
</asp:Content>
