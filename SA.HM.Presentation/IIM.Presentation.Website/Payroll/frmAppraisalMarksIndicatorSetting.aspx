<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmAppraisalMarksIndicatorSetting.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmAppraisalMarksIndicatorSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            CommonHelper.ApplyDecimalValidation();
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Appraisal</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Appraisal Marks Indicator</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#SearchPanel').hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("#gvApprMarksInd").delegate("td > img.MarksIndDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var advanceId = $.trim($(this).parent().parent().find("td:eq(3)").text());
                    var params = JSON.stringify({ sEmpId: advanceId });

                    var $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/Payroll/frmAppraisalMarksIndicatorSetting.aspx/DeleteMarksIndById",
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

        $(function () {
            $("#myTabs").tabs();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvApprMarksInd tbody tr").length;

            var indctrName = $("#<%=txtSApprIndName.ClientID %>").val();

            PageMethods.SearchMarksIndAndLoadGridInformation(indctrName, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvApprMarksInd tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvApprMarksInd tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvApprMarksInd tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:60%; cursor:pointer;\">" + gridObject.AppraisalIndicatorName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.AppraisalWeight + "</td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\">";
                if (IsCanEdit) {
                    tr +="<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.MarksIndicatorId + "')\" alt='Edit Information' border='0' />";
                }
                //else {
                //    //tr += "<td align='right' style=\"width:10%; cursor:pointer;\"></td>";
                //}
                if (IsCanDelete) {
                    
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png'  class= 'MarksIndDelete'  alt='Delete Information' border='0' />";
                     
                }
                //else {
                //    tr += "<td align='right' style=\"width:10%; cursor:pointer;\"></td>";
                //}
                tr += "</td>";
                tr += "<td align='right' style=\"width:8%; display:none;\">" + gridObject.MarksIndicatorId + "</td>";

                tr += "</tr>"

                $("#gvApprMarksInd tbody ").append(tr);
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

        function PerformEditAction(MarksIndId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            var possiblePath = "frmAppraisalMarksIndicatorSetting.aspx?editId=" + MarksIndId;
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
                href="#tab-1">Set Appraisal Marks Indicator</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Appraisal Marks Indicator</a></li>
        </ul>
        <div id="tab-1">
            <div id="TaxDeductionEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Appraisal Marks Indicator
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtApprIndId" runat="server" Value="" />
                                <asp:Label ID="lblApprIndName" runat="server" class="control-label required-field" Text="Indicator Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtApprIndName" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAppraisalWeight" runat="server" class="control-label required-field" Text="Weight"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAppraisalWeight" runat="server" CssClass="form-control quantitydecimal" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnApprInd" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnApprIndSave_Click" />
                                <asp:Button ID="btnApprIndClear" runat="server" Text="Clear" TabIndex="5" OnClientClick="return confirm('Do you want to clear?');" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnApprIndClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Search Appraisal Marks Indicator
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSApprIndName" runat="server" class="control-label" Text="Indicator Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtSApprIndName" CssClass="form-control" TabIndex="6">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
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
                    <table class="table table-bordered table-condensed table-responsive" id='gvApprMarksInd'
                        width="100%">
                        <colgroup>
                            <col style="width: 60%;" />
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            <%--<col style="width: 10%;" />--%>
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Appraisal Indicator Name
                                </td>
                                <td>Weight
                                </td>
                                <td style="text-align: right;">Action
                                </td>
                                <%--<td style="text-align: right;">Delete
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
</asp:Content>
