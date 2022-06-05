<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmMenuGroup.aspx.cs" Inherits="HotelManagement.Presentation.Website.UserInformation.frmMenuGroup" %>

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
            var formName = "<span class='divider'>/</span><li class='active'>Menu Group</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_btnSearch").click(function () {
                GridPaging(1, 1);
                return false;
            });

            var iconList = JSON.parse($("#ContentPlaceHolder1_hfGroupIconList").val());
            LoadIconList(iconList);


        });

        function LoadIconList(iconList) {
            var ddlGroupIconClass = '<%=ddlGroupIconClass.ClientID%>';
            var control = $('#' + ddlGroupIconClass);
            control.empty();

            control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            if (iconList.length == 0)
                return false;

            for (i = 0; i < iconList.length; i++) {
                control.append("<option style='padding-bottom:4px;' title='" + iconList[i].Name + "' value='" + iconList[i].Class + "'>" + "&nbsp;&nbsp;" + iconList[i].Code + "&nbsp;&nbsp;&nbsp;" + iconList[i].Name + "</option>");
            }
        }

        function SaveMenuGroup() {

            var menuGroupId = "0", menuGroupName = "0", menuGroupDisplayCaption = "", displaySequence = "0";
            var menuGroupIcon = null, activeStat = true;

            menuGroupId = $("#<%=hfMenuGroupId.ClientID %>").val() == "" ? "0" : $("#<%=hfMenuGroupId.ClientID %>").val();
            menuGroupName = $("#ContentPlaceHolder1_txtMenuGroupName").val();
            menuGroupDisplayCaption = $("#ContentPlaceHolder1_txtGroupDisplayCaption").val();
            displaySequence = $("#ContentPlaceHolder1_txtDisplaySequence").val() == "" ? "1" : $("#ContentPlaceHolder1_txtDisplaySequence").val();
            menuGroupIcon = ($("#ContentPlaceHolder1_ddlGroupIconClass").val() == "0" ? null : $("#ContentPlaceHolder1_ddlGroupIconClass").val());
            activeStat = ($("#ContentPlaceHolder1_ddlActiveStatus").val() == "1" ? true : false);
            var menuGroup = {
                MenuGroupId: menuGroupId,
                MenuGroupName: menuGroupName,
                GroupDisplayCaption: menuGroupDisplayCaption,
                DisplaySequence: displaySequence,
                GroupIconClass: menuGroupIcon,
                ActiveStat: activeStat
            };

            PageMethods.SaveMenuGroup(menuGroup, OnMenuGroupSucceed, OnMenuGroupFailed);
            return false;
        }

        function OnMenuGroupSucceed(result) {
            $("#<%=btnMenuGroup.ClientID %>").val("Save");
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
        function OnMenuGroupFailed() {

        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var menuGroupName = "";

            menuGroupName = $("#ContentPlaceHolder1_txtSMenuGroupName").val();

            var gridRecordsCount = $("#tblLoanSearch tbody tr").length;

            PageMethods.LoadMenuGroupInfo(menuGroupName, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoanLoadSucceed, OnLoanLoadFailed);
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
            $("#tblMenuGroupSearch tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#tblMenuGroupSearch tbody").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#tblMenuGroupSearch tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:30%;\">" + gridObject.MenuGroupName + "</td>";
                tr += "<td align='left' style=\"width:30%;\">" + gridObject.GroupDisplayCaption + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + gridObject.DisplaySequence + "</td>";
                tr += "<td align='center' style=\"width:10%;\"> <i class=\"" + (gridObject.GroupIconClass == null ? '' : gridObject.GroupIconClass) + "\" aria-hidden=true></i> </td>";
                tr += "<td align='left' style=\"width:10%;\">" + (gridObject.ActiveStat == true ? 'Active' : 'In-Active') + "</td>";

                editLink = "&nbsp;<a href=\"javascript:void();\"  title=\"Edit\" onclick=\"javascript:return EditMenuGroup(" + gridObject.MenuGroupId + ");\"><img alt=\"Edit Menu Group\" src=\"../Images/edit.png\" /></a>";
                deleteLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" title=\"Delete\" onclick=\"javascript:return DeleteMenuGroup(" + gridObject.MenuGroupId + ");\"><img alt=\"Delete Menu Group\" src=\"../Images/delete.png\" /></a>";

                tr += "<td align='left' style=\"width:10%;\">" + editLink + deleteLink + "</td>";
                tr += "</tr>";

                $("#tblMenuGroupSearch tbody").append(tr);
                tr = "";
                editLink = ""; deleteLink = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
        }
        function OnLoanLoadFailed(error) {
        }

        function ClearForm() {
            $("#<%=hfMenuGroupId.ClientID %>").val("");
            $("#<%=txtMenuGroupName.ClientID %>").val("");
            $("#<%=txtGroupDisplayCaption.ClientID %>").val("");
            $("#<%=txtDisplaySequence.ClientID %>").val("");
            $("#<%=ddlGroupIconClass.ClientID %>").val(0);
            $("#<%=ddlActiveStatus.ClientID %>").val(1);
        }

        function EditMenuGroup(menuGroupId) {
            PageMethods.GetMenuGroupInfo(menuGroupId, OnEditMenuGroupSucceed, OnEditMenuGroupFailed);
        }
        function OnEditMenuGroupSucceed(result) {
            $("#<%=hfMenuGroupId.ClientID %>").val(result.MenuGroupId);
            $("#<%=txtMenuGroupName.ClientID %>").val(result.MenuGroupName);
            $("#<%=txtGroupDisplayCaption.ClientID %>").val(result.GroupDisplayCaption);
            $("#<%=txtDisplaySequence.ClientID %>").val(result.DisplaySequence);
            $("#<%=ddlGroupIconClass.ClientID %>").val(result.GroupIconClass);
            if (result.ActiveStat == 1) {
                $("#<%=ddlActiveStatus.ClientID %>").val(1);
            }
            else $("#<%=ddlActiveStatus.ClientID %>").val(0);
            $("#<%=btnMenuGroup.ClientID %>").val("Update");
            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });
        }
        function OnEditMenuGroupFailed(error) {
        }

        function DeleteMenuGroup(menuGroupId) {
            PageMethods.DeleteMenuGroupInfo(menuGroupId, OnDeleteMenuGroupSucceed, OnDeleteMenuGroupFailed);
        }
        function OnDeleteMenuGroupSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(1);
        }
        function OnDeleteMenuGroupFailed(error) {
        }

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfMenuGroupId" runat="server" Value="" />
    <asp:HiddenField ID="hfGroupIconList" runat="server" Value="" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Menu Group Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Menu Group Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="ApprEvaluationEntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Menu Group
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcReservationGuest" runat="server" class="control-label" Text="Menu Group Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMenuGroupName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Group Display Caption"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGroupDisplayCaption" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Display Sequence"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDisplaySequence" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Menu Group Icon"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGroupIconClass" runat="server" CssClass="form-control"
                                    Style="font-family: 'FontAwesome', Arial;" TabIndex="7" Font-Names="fontawesome"
                                    DataTextFormatString="fontawesome">
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
                                <asp:Button ID="btnMenuGroup" runat="server" Text="Save" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript:return SaveMenuGroup()" />
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
                                <asp:Label ID="lblSearchGrade" runat="server" class="control-label required-field" Text="Menu Group Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSMenuGroupName" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
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
                    <table id='tblMenuGroupSearch' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="width: 30%;" />
                            <col style="width: 30%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                                <th>Menu Group Name
                                </th>
                                <th>Group Display Caption
                                </th>
                                <th>Display Sequence
                                </th>
                                <th>Group Icon
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
