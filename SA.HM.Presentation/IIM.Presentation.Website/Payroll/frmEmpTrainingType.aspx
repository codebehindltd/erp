<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpTrainingType.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpTrainingType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Training & Education</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Training Info</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            if (IsCanSave) {
                $('#ContentPlaceHolder1_btnEmpTrainingType').show();
            } else {
                $('#ContentPlaceHolder1_btnEmpTrainingType').hide();
            }

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("#gvEmployeeTrainingType").delegate("td > img.TrainingType", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var trainingTypeId = $.trim($(this).parent().parent().find("td:eq(4)").text());
                    var params = JSON.stringify({ sEmpId: trainingTypeId });

                    var $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/HMCommon/frmEmpTrainingType.aspx/DeleteData",
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

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvEmployeeTrainingType tbody tr").length;

            var trainingTypeName = $("#ContentPlaceHolder1_txtSTrainingName").val();
            PageMethods.SearchTrainingTypeAndLoadGridInformation(trainingTypeName, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvEmployeeTrainingType tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvEmployeeTrainingType tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvEmployeeTrainingType tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='right' style=\"display:none;\">" + gridObject.TrainingTypeId + "</td>";
                tr += "<td align='left' style=\"width:70%; cursor:pointer;\">" + gridObject.TrainingName + "</td>";
                tr += "<td style=\"text-align: right; width:15%; cursor:pointer;\">";
                if (IsCanEdit) {
                    tr += "&nbsp;&nbsp; <img src='../Images/edit.png' onClick= \"javascript:return PerformEditActionWithConfirmation('" + gridObject.TrainingTypeId + "')\" alt='Edit Information' border='0' />";
                }
                if (IsCanDelete) {
                    tr += "&nbsp;&nbsp; <img src='../Images/delete.png' onClick= \"javascript:return PerformDeleteAction('" + gridObject.TrainingTypeId + "')\"  alt='Delete Information' border='0' />";
                }
                tr += "</td>";
                tr += "</tr>"

                $("#gvEmployeeTrainingType tbody ").append(tr);
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
        function PerformEditAction(trainingTypeId) {
            PageMethods.LoadDetailInformation(trainingTypeId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            return false;
        }
        function PerformEditActionWithConfirmation(trainingTypeId) {
            if (!confirm("Do You Want to Edit?"))
                return false;

            PerformEditAction(trainingTypeId);
        }
        function OnLoadDetailObjectSucceeded(result) {

            if (IsCanEdit) {
                $('#ContentPlaceHolder1_btnEmpTrainingType').show();
            } else {
                $('#ContentPlaceHolder1_btnEmpTrainingType').hide();
            }

            $("#<%=txtTrainingName.ClientID %>").val(result.TrainingName);
            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
            $("#<%=btnEmpTrainingType.ClientID %>").val("Update");
            $("#<%=hfTrainingTypeId.ClientID %>").val(result.TrainingTypeId);

            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });

            return false;
        }
        function OnLoadDetailObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformDeleteAction(trainingTypeId) {
            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(trainingTypeId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            //$row.remove();
            //$("#myTabs").tabs('load', 1);
            GridPaging(1, 1);
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }     
        function ValidationBeforeSave() {
            var trainingName = $("#<%=txtTrainingName.ClientID%>").val();
            if (trainingName == '') {
                toastr.warning("Enter Training Name");
                $("#<%=txtTrainingName.ClientID%>").focus();
                return false;
            }
        }
    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Employee Training</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Employee Training</a></li>
        </ul>
        <div id="tab-1">
            <asp:HiddenField ID="hfTrainingTypeId" runat="server" Value="" />
            <div id="TrainingTypeEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Training</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTrainingName" runat="server" class="control-label required-field" Text="Training Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtTrainingName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpTrainingType" runat="server" Text="Save" TabIndex="3" CssClass="TransactionalButton btn btn-primary"
                                 OnClientClick="javascript: return ValidationBeforeSave();"   OnClick="btnEmpTrainingTypeSave_Click" />
                                <asp:Button ID="btnEmpTrainingTypeClear" runat="server" Text="Clear" TabIndex="4"
                                    CssClass="TransactionalButton btn btn-primary" OnClick="btnEmpTrainingTypeClear_Click" OnClientClick="return confirm('Do you want to Clear?');" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Training Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSTrainingName" runat="server" class="control-label" Text="Training Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSTrainingName" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                        </div>
                        <%--<div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblSFromDate" runat="server" Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="datepicker" TabIndex="6"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft" style="text-align: right">
                            <asp:Label ID="lblSToDate" runat="server" Text="To Date"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="datepicker" TabIndex="7"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>--%>
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
                    <table id='gvEmployeeTrainingType' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="display: none;" />
                            <col style="width: 80%;" />
                            <col style="width: 20%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="display: none;">
                                </th>
                                <th style="text-align: left;">
                                    Training Name
                                </th>
                                <th style="text-align: right;">
                                    Action
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
</asp:Content>
