<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmMenuLinks.aspx.cs" Inherits="HotelManagement.Presentation.Website.UserInformation.frmMenuLinks" %>

<%--<asp:Content ID="header" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
</asp:Content>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var gc = [];
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            IsCanSave = $("#<%=hfSavePermission.ClientID %>").val() == '1' ? true : false;
            IsCanEdit = $("#<%=hfEditPermission.ClientID %>").val() == '1' ? true : false;
            IsCanDelete = $("#<%=hfDeletePermission.ClientID %>").val() == '1' ? true : false;
            IsCanView = $("#<%=hfViewPermission.ClientID %>").val() == '1' ? true : false;
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Authorization Panel</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Menu Links</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_btnSearch").click(function () {
                GridPaging(1, 1);
                return false;
            });

            $("#ContentPlaceHolder1_ddlMenuModule").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSMenuModule").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
        });

        function SaveMenuLink() {

            var menuLinksId = "0", pageId = "", pageName = "", moduleId = "0", pageDisplayCaption = "";
            var pageExtension = "", pageType = "", linkIconClass = null, activeStat = true;

            menuLinksId = $("#<%=hfMenuLinkId.ClientID %>").val() == "" ? "0" : $("#<%=hfMenuLinkId.ClientID %>").val();
            moduleId = $("#ContentPlaceHolder1_ddlMenuModule").val();
            pageId = $("#ContentPlaceHolder1_txtPageId").val();
            pageName = $("#ContentPlaceHolder1_txtPageName").val();
            pageDisplayCaption = $("#ContentPlaceHolder1_txtPageDisplayCaption").val();
            pageExtension = $("#ContentPlaceHolder1_ddlPageExtension").val();
            pageType = $("#ContentPlaceHolder1_ddlPageType").val();
            linkIconClass = ($("#ContentPlaceHolder1_ddlLinkIconClass").val() == "0" ? null : $("#ContentPlaceHolder1_ddlLinkIconClass").val());
            activeStat = ($("#ContentPlaceHolder1_ddlActiveStatus").val() == "1" ? true : false);

            if (moduleId == "0") {
                toastr.info("Please Select Menu Module");
                return false;
            }

            var menuLinks = {
                MenuLinksId: menuLinksId,
                ModuleId: moduleId,
                PageId: pageId,
                PageName: pageName,
                PageDisplayCaption: pageDisplayCaption,
                PageExtension: pageExtension,
                PageType: pageType,
                LinkIconClass: linkIconClass,
                ActiveStat: activeStat
            };
            PageMethods.SaveMenuLink(menuLinks, OnSaveMenuLinksSucceed, OnSaveMenuLinksFailed);

            return false;
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var menuModuleId = "", pageName = "", pageType = "";

            menuModuleId = $("#ContentPlaceHolder1_ddlSMenuModule").val();
            pageName = $("#ContentPlaceHolder1_txtSPageName").val();
            pageType = $("#ContentPlaceHolder1_ddlSPageType").val();

            if (pageType == "0")
                pageType = "";

            var gridRecordsCount = $("#tblLoanSearch tbody tr").length;

            PageMethods.LoadMenuLinksInfo(menuModuleId, pageName, pageType, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoanLoadSucceed, OnLoanLoadFailed);
            return false;
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function OnLoanLoadSucceed(result) {
            vc = result;
            $("#tblMenuLinkSearch tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#tblMenuLinkSearch tbody").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#tblMenuLinkSearch tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:20%;\">" + gridObject.PageName + "</td>";
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.PageDisplayCaption + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + gridObject.PageExtension + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + gridObject.PagePath + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + gridObject.PageType + "</td>";
                tr += "<td align='center' style=\"width:10%;\"> <i class=\"" + (gridObject.LinkIconClass == null ? '' : gridObject.LinkIconClass) + "\"></i> </td>";
                tr += "<td align='left' style=\"width:10%;\">" + (gridObject.ActiveStat == true ? 'Active' : 'In-Active') + "</td>";

                editLink = "&nbsp;<a href=\"javascript:void();\"  title=\"Edit\" onclick=\"javascript:return EditMenuLink(" + gridObject.MenuLinksId + ");\"><img alt=\"Edit Menu Link\" src=\"../Images/edit.png\" /></a>";
                deleteLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" title=\"Delete\" onclick=\"javascript:return DeleteMenuLink(" + gridObject.MenuLinksId + ");\"><img alt=\"Delete Menu Link\" src=\"../Images/delete.png\" /></a>";

                tr += "<td align='left' style=\"width:10%;\">" + editLink + deleteLink + "</td>";
                tr += "</tr>";

                $("#tblMenuLinkSearch tbody").append(tr);
                tr = "";
                editLink = ""; deleteLink = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
        }
        function OnLoanLoadFailed(error) {

        }

        function OnSaveMenuLinksSucceed(result) {
            $("#<%=btnSavePageLink.ClientID %>").val("Save");
            CommonHelper.AlertMessage(result.AlertMessage);
            ClearForm();

            //            if (result.AlertMessage.IsSuccess) {                               
            //                CommonHelper.AlertMessage(result.AlertMessage);                
            //                ClearForm();
            //            }
            //            else {
            //                CommonHelper.AlertMessage(result.AlertMessage);
            //            }
        }
        function OnSaveMenuLinksFailed() {
        }

        function ClearForm() {
            $("#<%=hfMenuLinkId.ClientID %>").val("");
            $("#<%=ddlMenuModule.ClientID %>").val(0);
            $("#<%=txtPageId.ClientID %>").val("");
            $("#<%=txtPageName.ClientID %>").val("");
            $("#<%=txtPageDisplayCaption.ClientID %>").val("");
            $("#<%=ddlPageExtension.ClientID %>").val("aspx");
            $("#<%=ddlPageType.ClientID %>").val("Page");
            $("#<%=ddlLinkIconClass.ClientID %>").val(0);
            $("#<%=ddlActiveStatus.ClientID %>").val(1);
            //$("#frmMenuLinks")[0].reset();
        }

        function EditMenuLink(menuLinkId) {
            PageMethods.GetMenuLinksInfo(menuLinkId, OnEditMenuLinkSucceed, OnEditMenuLinkFailed);
        }
        function OnEditMenuLinkSucceed(result) {
            $("#<%=hfMenuLinkId.ClientID %>").val(result.MenuLinksId);
            $("#<%=ddlMenuModule.ClientID %>").val(result.ModuleId);
            $("#<%=txtPageId.ClientID %>").val(result.PageId);
            $("#<%=txtPageName.ClientID %>").val(result.PageName);
            $("#<%=txtPageDisplayCaption.ClientID %>").val(result.PageDisplayCaption);
            $("#<%=ddlPageExtension.ClientID %>").val(result.PageExtension);
            $("#<%=ddlPageType.ClientID %>").val(result.PageType);
            if (result.ActiveStat == 1) {
                $("#<%=ddlActiveStatus.ClientID %>").val(1);
            }
            else $("#<%=ddlActiveStatus.ClientID %>").val(0);
            $("#<%=btnSavePageLink.ClientID %>").val("Update");
            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });
        }
        function OnEditMenuLinkFailed(error) {
        }

        function DeleteMenuLink(menuLinkId) {
            PageMethods.DeleteMenuLinksInfo(menuLinkId, OnDeleteMenuLinkSucceed, OnDeleteMenuLinkFailed);
        }
        function OnDeleteMenuLinkSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(1);
        }
        function OnDeleteMenuLinkFailed(error) {
        }

    </script>
    <asp:HiddenField ID="hfMenuLinkId" runat="server" Value="" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Menu Links Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Menu Links Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="ApprEvaluationEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Menu Links
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label111" runat="server" class="control-label" Text="Menu Module"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlMenuModule" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcReservationGuest" runat="server" class="control-label" Text="Page Id"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPageId" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Page Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPageName" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Page Display Caption"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPageDisplayCaption" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Page Extension"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPageExtension" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Text="aspx" Value="aspx"></asp:ListItem>
                                    <asp:ListItem Text="chtml" Value="aspx"></asp:ListItem>
                                    <asp:ListItem Text="html" Value="aspx"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Page Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPageType" runat="server" CssClass="form-control"
                                    TabIndex="6">
                                    <asp:ListItem Text="Page" Value="Page"></asp:ListItem>
                                    <asp:ListItem Text="Report" Value="Report"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Link Icon"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLinkIconClass" runat="server" CssClass="form-control"
                                    TabIndex="7">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Active Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStatus" runat="server" CssClass="form-control"
                                    TabIndex="8">
                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSavePageLink" runat="server" Text="Save" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript:return SaveMenuLink()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Menu Group Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label112" runat="server" class="control-label" Text="Menu Module"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSMenuModule" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label7" runat="server" class="control-label" Text="Page Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSPageName" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="Page Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSPageType" runat="server" CssClass="form-control"
                                    TabIndex="6">
                                    <asp:ListItem Text="---Please Select---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Page" Value="Page"></asp:ListItem>
                                    <asp:ListItem Text="Report" Value="Report"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    TabIndex="6" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <table id='tblMenuLinkSearch' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                                <th>Page Name
                                </th>
                                <th>Page Display Caption
                                </th>
                                <th>Page Extension
                                </th>
                                <th>Page Path
                                </th>
                                <th>Page Type
                                </th>
                                <th>Link Icon
                                </th>
                                <th>Status
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
</asp:Content>
