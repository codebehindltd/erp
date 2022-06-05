<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpTrainingOrganizer.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpTrainingOrganizer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Training & Education</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Training Organizer</li>";
            var breadCrumbs = moduleName + formName;
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("#gvTrainingOrganizer").delegate("td > img.OrganizerDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var organizerId = $.trim($(this).parent().parent().find("td:eq(3)").text());
                    var params = JSON.stringify({ sEmpId: organizerId });

                    var $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/Payroll/frmEmpTrainingOrganizer.aspx/DeleteOrganizerById",
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
            $('#ContentPlaceHolder1_txtEmail').blur(function () {
                var email = $("#ContentPlaceHolder1_txtEmail").val();
                if (CommonHelper.IsValidEmail(email) == false) {
                    toastr.warning("Invalid Email");
                    $("#<%=txtEmail.ClientID %>").focus();
                    return false;
                }
            });
        });

        $(function () {
            $("#myTabs").tabs();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvTrainingOrganizer tbody tr").length;

            var organizerName = $("#<%=txtSOrganizerName.ClientID %>").val();

            PageMethods.SearchOrganizerAndLoadGridInformation(organizerName, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvTrainingOrganizer tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvTrainingOrganizer tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvTrainingOrganizer tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:50%; cursor:pointer;\">" + gridObject.OrganizerName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.ContactNo + "</td>";
                tr += "<td align='right' style=\"width:30%; cursor:pointer;\">"
                if (IsCanEdit) {
                    tr += "<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.OrganizerId + "')\" alt='Edit Information' border='0' />&nbsp;&nbsp;";
                }
                
                if (IsCanDelete) {
                    tr += "<img src='../Images/delete.png' class='OrganizerDelete' alt='Delete Information' border='0' /> ";
                }
                tr += "</td>";
                tr += "<td align='right' style=\"width:8%; display:none;\">" + gridObject.OrganizerId + "</td>";

                tr += "</tr>"

                $("#gvTrainingOrganizer tbody ").append(tr);
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

        function PerformEditAction(OrganizerId) {
            //alert('edit');
            if (!confirm('Do You Want to Edit?'))
                return false;
            var possiblePath = "frmEmpTrainingOrganizer.aspx?editId=" + OrganizerId;
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
                href="#tab-1">Training Organizer</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Organizer</a></li>
        </ul>
        <div id="tab-1">
            <div id="TaxDeductionEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Training Organizer</div>
                 <div class="panel-body">
                    <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:HiddenField ID="txtOrganizerId" runat="server" Value="" />
                            <asp:Label ID="lblOrganizerName" runat="server" class="control-label required-field" Text="Organizer Name"></asp:Label>                            
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtOrganizerName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblOrgAddress" runat="server" class="control-label" Text="Address"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtOrgAddress" runat="server" CssClass="form-control" TextMode="MultiLine"
                                TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblEmail" runat="server" class="control-label" Text="Email"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblContact" runat="server" class="control-label" Text="Contact No"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblTraiingType" runat="server" class="control-label" Text="Training Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlTrainingType" runat="server" CssClass="form-control"
                                TabIndex="2">
                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem Value="Inhouse">Inhouse</asp:ListItem>
                                <asp:ListItem Value="Outside">Outside</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnOrganizer" runat="server" Text="Save" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                                OnClick="btnOrganizerSave_Click" />
                            <asp:Button ID="btnOrganizerClear" runat="server" Text="Clear" TabIndex="6" CssClass="TransactionalButton btn btn-primary"
                                OnClick="btnOrganizerClear_Click" OnClientClick="return confirm('Do you want to Clear?');" />
                        </div>
                    </div>
                </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Organizer Information</div>
                 <div class="panel-body">
                    <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblSOrganizerName" runat="server" class="control-label" Text="Organizer Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtSOrganizerName" CssClass="form-control"
                                TabIndex="7"></asp:TextBox>
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
                    <table id='gvTrainingOrganizer' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="width: 50%;" />
                            <col style="width: 20%;" />
                            <col style="width: 30%;" />                            
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>
                                    Organizer Name
                                </td>
                                <td>
                                    Contact No
                                </td>
                                <td style="text-align: right;">
                                    Action
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
