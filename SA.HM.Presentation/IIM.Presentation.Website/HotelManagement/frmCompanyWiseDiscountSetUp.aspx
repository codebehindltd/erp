<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmCompanyWiseDiscountSetUp.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmCompanyWiseDiscountSetUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Company Wise Discount SetUp</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            $("#SearchPanel").hide();
            $("#ContentPlaceHolder1_btnSearch").click(function () {
                GridPaging(1, 1);
                return false;
            });

            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "0")
                $("#ContentPlaceHolder1_btnSave").hide();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var companyId = $("#<%=ddlSCompany.ClientID %>").val();
            if (companyId == 0) {
                toastr.warning("Please Select Company.");
                return false;
            }
            var activeStatus = $("#<%=ddlSActiveStatus.ClientID %>").val();
            if (activeStatus == 1)
                activeStatus = true;
            else
                activeStatus = false;
            var gridRecordsCount = $("#tblDiscountPolicySearch tbody tr").length;

            PageMethods.SearchDiscountPolicyInfo(companyId, activeStatus, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchDiscountPolicySucceed, OnSearchDiscountPolicyFailed);
            return false;
        }

        function OnSearchDiscountPolicySucceed(result) {
            $("#SearchPanel").show();
            $("#tblDiscountPolicySearch tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#tblDiscountPolicySearch tbody").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "";
            var isUpdatepermission = false;

            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "1")
                isUpdatepermission = true;

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#tblDiscountPolicySearch tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:23%;\">" + gridObject.RoomType + "</td>";
                tr += "<td align='left' style=\"width:23%;\">" + gridObject.DiscountType + "</td>";
                tr += "<td align='left' style=\"width:23%;\">" + gridObject.DiscountAmount + "</td>";
                tr += "<td align='left' style=\"width:23%;\">" + (gridObject.ActiveStat == true ? 'Active' : 'In-Active') + "</td>";
                if (isUpdatepermission)
                    editLink = "&nbsp;<a href=\"javascript:void();\"  title=\"Edit\" onclick=\"javascript:return EditDiscountPolicy(" + gridObject.CompanyWiseDiscountId + ");\"><img alt=\"Edit Discount Policy\" src=\"../Images/edit.png\" /></a>";

                tr += "<td align='left' style=\"width:8%;\">" + editLink + "</td>";
                tr += "</tr>";

                $("#tblDiscountPolicySearch tbody").append(tr);
                tr = "";
                editLink = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
        }
        function OnSearchDiscountPolicyFailed(error) {
        }

        function EditDiscountPolicy(companyWiseDiscountId) {
            PageMethods.LoadDiscountPolicyInfo(companyWiseDiscountId, OnLoadDiscountPolicySucceed, OnLoadDiscountPolicyFailed);
        }
        function OnLoadDiscountPolicySucceed(result) {
            $("#<%=hfDiscountPolicyId.ClientID %>").val(result.CompanyWiseDiscountId);
            $("#<%=ddlCompany.ClientID %>").val(result.CompanyId);
            $("#<%=ddlRoomType.ClientID %>").val(result.RoomTypeId);
            $("#<%=ddlDiscountType.ClientID %>").val(result.DiscountType);
            $("#<%=txtDiscountAmount.ClientID %>").val(result.DiscountAmount);
            if (result.ActiveStat == 1) {
                $("#<%=ddlActiveStatus.ClientID %>").val(1);
            }
            else $("#<%=ddlActiveStatus.ClientID %>").val(0);
            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "1")
                $("#<%=btnSave.ClientID %>").val("Update").show();
            else
                $("#<%=btnSave.ClientID %>").hide();
            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });
        }
        function OnLoadDiscountPolicyFailed(error) {
            toastr.error(error);
        }

    </script>
    <asp:HiddenField ID="hfDiscountPolicyId" runat="server" Value="" />
    <asp:HiddenField ID="hfIsSavePermission" runat="server" />
    <asp:HiddenField ID="hfIsUpdatePermission" runat="server" />
    <asp:HiddenField ID="hfIsDeletePermission" runat="server" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Discount Policy Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Policy</a></li>
        </ul>
        <div id="tab-1">
            <div id="DiscountPolicyEntryPanel" class="panel panel-default">
                <div class="panel-heading">Discount Policy Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCompany" runat="server" class="control-label required-field" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblRoomType" runat="server" class="control-label required-field" Text="Room Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomType" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblDiscountType" runat="server" class="control-label required-field" Text="Discount Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control" TabIndex="3">
                                    <asp:ListItem>Fixed</asp:ListItem>
                                    <asp:ListItem>Percentage</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblDiscountAmount" runat="server" class="control-label required-field" Text="Discoutn Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblActiveStatus" runat="server" class="control-label" Text="Active Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStatus" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="6" Text="Save" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="7" Text="Clear" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">Search Discount Policy</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSCompany" runat="server" class="control-label required-field" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSCompany" runat="server" CssClass="form-control" TabIndex="7">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSActiveStatus" runat="server" class="control-label" Text="Active Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStatus" runat="server" CssClass="form-control"
                                    TabIndex="8">
                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                                    TabIndex="8" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="SearchPanel" class="panel panel-default">
                    <div class="panel-body">
                        <table id='tblDiscountPolicySearch' class="table table-bordered table-condensed table-responsive" width="100%">
                            <colgroup>
                                <col style="width: 23%;" />
                                <col style="width: 23%;" />
                                <col style="width: 23%;" />
                                <col style="width: 23%;" />
                                <col style="width: 8%;" />
                            </colgroup>
                            <thead>
                                <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                                    <th>Room Type
                                    </th>
                                    <th>Discount Type
                                    </th>
                                    <th>Discount Amount
                                    </th>
                                    <th>Active Status
                                    </th>
                                    <th>Action
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
    </div>
</asp:Content>
