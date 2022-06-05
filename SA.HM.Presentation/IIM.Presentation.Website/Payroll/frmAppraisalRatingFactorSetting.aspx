<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmAppraisalRatingFactorSetting.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmAppraisalRatingFactorSetting" %>

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
            var formName = "<span class='divider'>/</span><li class='active'>Appraisal Rating Factor</li>";
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

            $("#gvRatingFactor").delegate("td > img.RtngFactrDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var advanceId = $.trim($(this).parent().parent().find("td:eq(4)").text());
                    var params = JSON.stringify({ sEmpId: advanceId });

                    var $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/Payroll/frmAppraisalRatingFactorSetting.aspx/DeleteRtngFactrById",
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

            var gridRecordsCount = $("#gvRatingFactor tbody tr").length;

            var marksIndId = $("#<%=ddlSAppraisalIndicator.ClientID %>").val();
            var rtngFactrName = $("#<%=txtSRatingFactorName.ClientID %>").val();

            PageMethods.SearchRtngFactrAndLoadGridInformation(marksIndId, rtngFactrName, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvRatingFactor tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvRatingFactor tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvRatingFactor tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:40%; cursor:pointer;\">" + gridObject.RatingFactorName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.RatingWeight + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.AppraisalIndicatorName + "</td>";
                tr += "<td align='right' style=\"width:20%; cursor:pointer;\">";

                if (IsCanEdit) {
                    tr += "&nbsp&nbsp&nbsp<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.RatingFactorId + "')\" alt='Edit Information' border='0' />";
                }
                else {
                    tr += "";
                }
                if (IsCanDelete) {
                    tr += "&nbsp&nbsp&nbsp<img src='../Images/delete.png' class= 'RtngFactrDelete'  alt='Delete Information' border='0' />";
                }
                else {
                    tr += "";
                }
                tr += "</td>";
                tr += "<td align='right' style=\"width:8%; display:none;\">" + gridObject.RatingFactorId + "</td>";

                tr += "</tr>"

                $("#gvRatingFactor tbody ").append(tr);
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

        function PerformEditAction(RtngFactId) {
             if (!confirm("Do you want to edit?")) {
                return false;
            }
            //alert('edit');
            var possiblePath = "frmAppraisalRatingFactorSetting.aspx?editId=" + RtngFactId;
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
                href="#tab-1">Set Appraisal Rating Factor</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Appraisal Rating Factor</a></li>
        </ul>
        <div id="tab-1">
            <div id="TaxDeductionEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Appraisal Rating Factor</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtApprRatnFactId" runat="server" Value="" />
                                <asp:Label ID="lblAppraisalIndicatorId" runat="server" class="control-label required-field"
                                    Text="Appraisal Indicator"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlAppraisalIndicator" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRatingFactorName" runat="server" class="control-label required-field"
                                    Text="Rating Factor Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRatingFactorName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRatingWeight" runat="server" class="control-label required-field"
                                    Text="Weight"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtRatingWeight" CssClass="form-control quantitydecimal" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnRatingFact" runat="server" Text="Save" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnRatingFactSave_Click" />
                                <asp:Button ID="btnRatingFactClear" OnClientClick="return confirm('Do you want to clear?');" runat="server" Text="Clear" TabIndex="6" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnRatingFactClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Department Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSApprIndName" runat="server" class="control-label" Text="Marks Indicator"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSAppraisalIndicator" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSRatingFactorName" runat="server" class="control-label" Text="Rating Factor Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtSRatingFactorName" CssClass="form-control" TabIndex="8">
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
                    Search Information</div>
                <div class="panel-body">
                    <table class="table table-bordered table-condensed table-responsive" id='gvRatingFactor'
                        width="100%">
                        <colgroup>
                            <col style="width: 40%;" />
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>
                                    Rating Factor Name
                                </td>
                                <td>
                                    Weight
                                </td>
                                <td>
                                    Marks Indicator
                                </td>
                                <td style="text-align: center;">
                                    Action
                                </td>
                                <%--<td style="text-align: right;">
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
</asp:Content>
