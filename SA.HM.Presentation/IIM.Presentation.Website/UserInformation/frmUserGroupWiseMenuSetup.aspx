<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmUserGroupWiseMenuSetup.aspx.cs" Inherits="HotelManagement.Presentation.Website.UserInformation.frmUserGroupWiseMenuSetup" %>

<%--<asp:Content ID="header" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">   
</asp:Content>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var gc = [];
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {

            $("#ContentPlaceHolder1_ddlPermissionType").change(function () {
                $("#UserWiseMenuAssign tbody").html("");
                $("#ContentPlaceHolder1_ddlUserGroup").val("0").trigger('change');
                $("#ContentPlaceHolder1_ddlUserList").val("0").trigger('change');
                var permissionTypeId = $("#ContentPlaceHolder1_ddlPermissionType").val();
                if (permissionTypeId == "0") {
                    $("#permissionWiseDiv").hide();
                } else {
                    if (permissionTypeId == "1") {
                        $("#groupUserDiv").show();
                        $("#singleUserDiv").hide();
                    } else {
                        $("#groupUserDiv").hide();
                        $("#singleUserDiv").show();
                    }
                    $("#permissionWiseDiv").show();
                    
                }
                
            });

            if ($("#ContentPlaceHolder1_ddlPermissionType").val() == "0") {
                $("#permissionWiseDiv").hide();
            } else {
                if ($("#ContentPlaceHolder1_ddlPermissionType").val() == "1") {
                    $("#groupUserDiv").show();
                    $("#singleUserDiv").hide();
                } else {
                    $("#groupUserDiv").hide();
                    $("#singleUserDiv").show();
                }
                $("#permissionWiseDiv").show();
                
            }

            IsCanSave = $("#<%=hfSavePermission.ClientID %>").val() == '1' ? true : false;
            IsCanEdit = $("#<%=hfEditPermission.ClientID %>").val() == '1' ? true : false;
            IsCanDelete = $("#<%=hfDeletePermission.ClientID %>").val() == '1' ? true : false;
            IsCanView = $("#<%=hfViewPermission.ClientID %>").val() == '1' ? true : false;

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Authorization Panel</a>";
            var formName = "<span class='divider'>/</span><li class='active'>User Permission</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_ddlUserGroup").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlUserList").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlMenuGroup").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlMenuModule").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlUserGroup").change(function () {
                var menuGroupId = $("#ContentPlaceHolder1_ddlMenuGroup").val();
                var moduleId = $("#ContentPlaceHolder1_ddlMenuModule").val();

                if ($(this).val() == "0") {
                    return false;
                }
                else if (menuGroupId == "0") {
                    return false;
                }
                else if (moduleId == "0") {
                    return false;
                }

                GetLinkByModuleId();
            });
            
            $("#ContentPlaceHolder1_ddlUserList").change(function () {
                var menuGroupId = $("#ContentPlaceHolder1_ddlMenuGroup").val();
                var moduleId = $("#ContentPlaceHolder1_ddlMenuModule").val();
                $("#ContentPlaceHolder1_ddlUserGroup").val("0");

                if ($(this).val() == "0") {
                    return false;
                }
                else if (menuGroupId == "0") {
                    return false;
                }
                else if (moduleId == "0") {
                    return false;
                }
                GetLinkByModuleId();
            });

            $("#ContentPlaceHolder1_ddlMenuGroup").change(function () {
                var userGroupId = $("#ContentPlaceHolder1_ddlUserGroup").val();
                var userId = $("#ContentPlaceHolder1_ddlUserList").val();
                var moduleId = $("#ContentPlaceHolder1_ddlMenuModule").val();

                if ($(this).val() == "0") {
                    return false;
                }
                else if (userGroupId == "0") {
                    return false;
                }
                else if (userId == "0") {
                    return false;
                }
                else if (moduleId == "0") {
                    return false;
                }

                GetLinkByModuleId();
            });

            $("#ContentPlaceHolder1_ddlMenuModule").change(function () {
                if ($(this).val() == "0") {
                    return false;
                }

                GetLinkByModuleId();
            });

            $("#chkAll").change(function () {
                if ($(this).is(":checked")) {
                    $("#UserWiseMenuAssign tbody tr").find("td:eq(2)").find("input").prop("checked", true);
                }
                else {
                    $("#UserWiseMenuAssign tbody tr").find("td:eq(2)").find("input").prop("checked", false);
                }
            });

            $("#chkCreate").change(function () {
                if ($(this).is(":checked")) {
                    $("#UserWiseMenuAssign tbody tr").find("td:eq(6)").find("input").prop("checked", true);
                }
                else {
                    $("#UserWiseMenuAssign tbody tr").find("td:eq(6)").find("input").prop("checked", false);
                }
            });

            $("#chkUpdate").change(function () {
                if ($(this).is(":checked")) {
                    $("#UserWiseMenuAssign tbody tr").find("td:eq(7)").find("input").prop("checked", true);
                }
                else {
                    $("#UserWiseMenuAssign tbody tr").find("td:eq(7)").find("input").prop("checked", false);
                }
            });

            $("#chkDelete").change(function () {
                if ($(this).is(":checked")) {
                    $("#UserWiseMenuAssign tbody tr").find("td:eq(8)").find("input").prop("checked", true);
                }
                else {
                    $("#UserWiseMenuAssign tbody tr").find("td:eq(8)").find("input").prop("checked", false);
                }
            });

            $("#chkView").change(function () {
                if ($(this).is(":checked")) {
                    $("#UserWiseMenuAssign tbody tr").find("td:eq(9)").find("input").prop("checked", true);
                }
                else {
                    $("#UserWiseMenuAssign tbody tr").find("td:eq(9)").find("input").prop("checked", false);
                }
            });

        });

        function GetLinkByModuleId() {
            $("#chkAll").prop("checked", false);
            $("#chkCreate").prop("checked", false);
            $("#chkUpdate").prop("checked", false);
            $("#chkDelete").prop("checked", false);
            $("#chkView").prop("checked", false);

            var userGroupId = $("#ContentPlaceHolder1_ddlUserGroup").val();
            var menuGroupId = $("#ContentPlaceHolder1_ddlMenuGroup").val();
            var moduleId = $("#ContentPlaceHolder1_ddlMenuModule").val();
            var userId = $("#ContentPlaceHolder1_ddlUserList").val();
            var permissionType = $("#ContentPlaceHolder1_ddlPermissionType").val();

            if (permissionType == "1") {
                userId = "0";
                if (userGroupId == "0") {
                    toastr.info("Please Select User Group");
                    return false;
                }
            }
            else if (permissionType == "2" || permissionType == "3") {
                userGroupId = "0";
                if (userId == "0") {
                    toastr.info("Please Select a User");
                    return false;
                }
            }

            if (menuGroupId == "0") {
                toastr.info("Please Select Menu Group");
                return false;
            }
            else if (moduleId == "0") {
                toastr.info("Please Select Menu Module");
                return false;
            }
            if (permissionType == "1") {
                PageMethods.GetLinkByModuleId(userGroupId, menuGroupId, moduleId, OnLoadMenuLinksSucceed, OnLoadMenuLinksFailed);
            }
            else if (permissionType == "2") {
                PageMethods.GetLinkForSingleUserByModuleId(userId, menuGroupId, moduleId, OnLoadMenuLinksSucceed, OnLoadMenuLinksFailed);
            }
            
        }

        function OnLoadMenuLinksSucceed(result) {
            gc = result;
            $("#UserWiseMenuAssign tbody").html("");

            var menuLinks = result[0].MenuLinks;
            var menuWiseLinks = result[0].MenuWisePermitedLinks;

            var isCreate = "", isUpdate = "", isDelete = "", isView = "", hasPermission = "", linksDisplaySequence = "";
            var i = 0, menuLength = menuLinks.length;
            var tr = "";

            for (i = 0; i < menuLength; i++) {

                if (i % 2 == 0) {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }
                else {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }

                var alreadySavedLink = _.findWhere(menuWiseLinks, { MenuLinksId: menuLinks[i].MenuLinksId });

                if (alreadySavedLink != null) {
                    tr += "<td id='menu" + menuLinks[i].MenuLinksId + "' style=\"display:none;\">" + alreadySavedLink.MenuWiseLinksId + "</td>";
                    isCreate = alreadySavedLink.IsSavePermission == true ? "checked='cheked'" : "";
                    isUpdate = alreadySavedLink.IsUpdatePermission == true ? "checked='cheked'" : "";
                    isDelete = alreadySavedLink.IsDeletePermission == true ? "checked='cheked'" : "";
                    isView = alreadySavedLink.IsViewPermission == true ? "checked='cheked'" : "";
                    hasPermission = "checked='cheked'";

                    linksDisplaySequence = alreadySavedLink.LinksDisplaySequence;
                }
                else {
                    tr += "<td id='menu" + menuLinks[i].MenuLinksId + "' style=\"display:none;\">0</td>"
                    isCreate = ""; isDelete = ""; isView = ""; hasPermission = "";
                    linksDisplaySequence = "1";
                }

                tr += "<td style=\"display:none;\">" + menuLinks[i].MenuLinksId + "</td>";

                tr += "<td style=\"width: 8%; text-align:center;\">" +
                    "<input type='checkbox' id='chk" + menuLinks[i].MenuLinksId + "'" + hasPermission + "/>" +
                    "</td>" +
                    "<td style=\"width: 43%;\">" +
                    menuLinks[i].PageName +
                    "</td>" +
                    "<td style=\"width: 15%;\">" +
                    menuLinks[i].PageType +
                    "</td>" +
                    "<td style=\"width: 8%; text-align:center;\">" +
                    "<input type='text' class='quantity form-control' id='ds" + menuLinks[i].MenuLinksId + "'" + "value='" + linksDisplaySequence + "'/>" +
                    "</td>" +
                    "<td style=\"width: 8%; text-align:center;\">" +
                    "<input type='checkbox' id='create" + menuLinks[i].MenuLinksId + "'" + isCreate + "/>" +
                    "</td>" +
                    "<td style=\"width: 8%; text-align:center;\">" +
                    "<input type='checkbox' id='create" + menuLinks[i].MenuLinksId + "'" + isUpdate + "/>" +
                    "</td>" +
                    "<td style=\"width: 8%; text-align:center;\">" +
                    "<input type='checkbox' id='delete" + menuLinks[i].MenuLinksId + "'" + isDelete + "/>" +
                    "</td>" +
                    "<td style=\"width:8%; text-align:center;\">" +
                    "<input type='checkbox' id='view" + menuLinks[i].MenuLinksId + "'" + isView + "/>" +
                    "</td>"
                "</tr>";

                $("#UserWiseMenuAssign tbody").append(tr);
                tr = "";
                isCreate = ""; isUpdate = ""; isDelete = ""; isView = ""; hasPermission = "";
            }

            CommonHelper.ApplyIntigerValidation();
        }
        function OnLoadMenuLinksFailed() {
        }

        function SaveMenuPermission() {

            var SecurityMenuWiseLinksNelyAdded = new Array();
            var SecurityMenuWiseLinksEdited = new Array();
            var SecurityMenuWiseLinksDeleted = new Array();
            var userGroupId = "0", menuGroupId = "0", moduleId = "0", menuWiseLinksId = "0",
                userId = "0", permissionType = "0";

            moduleId = $("#ContentPlaceHolder1_ddlMenuModule").val();
            userGroupId = $("#ContentPlaceHolder1_ddlUserGroup").val();
            menuGroupId = $("#ContentPlaceHolder1_ddlMenuGroup").val();
            userId = $("#ContentPlaceHolder1_ddlUserList").val();
            permissionType = $("#ContentPlaceHolder1_ddlPermissionType").val();

            if (permissionType == "1") {
                userId = "0";
                if (userGroupId == "0") {
                    toastr.info("Please Select User Group");
                    return false;
                }
            }
            else if (permissionType == "2") {
                userGroupId = "0";
                if (userId == "0") {
                    toastr.info("Please Select a User");
                    return false;
                }
            }

            if (menuGroupId == "0") {
                toastr.info("Please Select Menu Group");
                return false;
            }
            else if (moduleId == "0") {
                toastr.info("Please Select Menu Module");
                return false;
            }

            $("#UserWiseMenuAssign tbody tr").each(function () {

                menuWiseLinksId = $.trim($(this).find("td:eq(0)").text());
                
                if (userId == "0") {
                    if ($(this).find("td:eq(2)").find("input").is(":checked")) {

                        if (menuWiseLinksId != "0") {
                            SecurityMenuWiseLinksEdited.push({
                                MenuWiseLinksId: menuWiseLinksId,
                                UserGroupId: userGroupId,
                                MenuGroupId: menuGroupId,
                                MenuLinksId: $.trim($(this).find("td:eq(1)").text()),
                                DisplaySequence: $.trim($(this).find("td:eq(5)").find("input").val()),
                                IsSavePermission: $(this).find("td:eq(6)").find("input").is(":checked"),
                                IsUpdatePermission: $(this).find("td:eq(7)").find("input").is(":checked"),
                                IsDeletePermission: $(this).find("td:eq(8)").find("input").is(":checked"),
                                IsViewPermission: $(this).find("td:eq(9)").find("input").is(":checked"),
                                ActiveStat: true
                            });
                        }
                        else {
                            SecurityMenuWiseLinksNelyAdded.push({
                                MenuWiseLinksId: menuWiseLinksId,
                                UserGroupId: userGroupId,
                                MenuGroupId: menuGroupId,
                                MenuLinksId: $.trim($(this).find("td:eq(1)").text()),
                                DisplaySequence: $.trim($(this).find("td:eq(5)").find("input").val()),
                                IsSavePermission: $(this).find("td:eq(6)").find("input").is(":checked"),
                                IsUpdatePermission: $(this).find("td:eq(7)").find("input").is(":checked"),
                                IsDeletePermission: $(this).find("td:eq(8)").find("input").is(":checked"),
                                IsViewPermission: $(this).find("td:eq(9)").find("input").is(":checked"),
                                ActiveStat: true
                            });
                        }
                    }
                    else {
                        if (menuWiseLinksId != "0") {
                            SecurityMenuWiseLinksDeleted.push({
                                MenuWiseLinksId: menuWiseLinksId,
                                UserGroupId: userGroupId,
                                MenuGroupId: menuGroupId,
                                MenuLinksId: $.trim($(this).find("td:eq(1)").text()),
                                DisplaySequence: $.trim($(this).find("td:eq(5)").find("input").val()),
                                IsSavePermission: $(this).find("td:eq(6)").find("input").is(":checked"),
                                IsUpdatePermission: $(this).find("td:eq(7)").find("input").is(":checked"),
                                IsDeletePermission: $(this).find("td:eq(8)").find("input").is(":checked"),
                                IsViewPermission: $(this).find("td:eq(9)").find("input").is(":checked"),
                                ActiveStat: true
                            });
                        }
                    }
                }
                else {
                    if ($(this).find("td:eq(2)").find("input").is(":checked")) {

                        if (menuWiseLinksId != "0") {
                            SecurityMenuWiseLinksEdited.push({
                                MenuWiseLinksId: menuWiseLinksId,
                                UserId: userId,
                                MenuGroupId: menuGroupId,
                                MenuLinksId: $.trim($(this).find("td:eq(1)").text()),
                                DisplaySequence: $.trim($(this).find("td:eq(5)").find("input").val()),
                                IsSavePermission: $(this).find("td:eq(6)").find("input").is(":checked"),
                                IsUpdatePermission: $(this).find("td:eq(7)").find("input").is(":checked"),
                                IsDeletePermission: $(this).find("td:eq(8)").find("input").is(":checked"),
                                IsViewPermission: $(this).find("td:eq(9)").find("input").is(":checked"),
                                ActiveStat: true
                            });
                        }
                        else {
                            SecurityMenuWiseLinksNelyAdded.push({
                                MenuWiseLinksId: menuWiseLinksId,
                                UserId: userId,
                                MenuGroupId: menuGroupId,
                                MenuLinksId: $.trim($(this).find("td:eq(1)").text()),
                                DisplaySequence: $.trim($(this).find("td:eq(5)").find("input").val()),
                                IsSavePermission: $(this).find("td:eq(6)").find("input").is(":checked"),
                                IsUpdatePermission: $(this).find("td:eq(7)").find("input").is(":checked"),
                                IsDeletePermission: $(this).find("td:eq(8)").find("input").is(":checked"),
                                IsViewPermission: $(this).find("td:eq(9)").find("input").is(":checked"),
                                ActiveStat: true
                            });
                        }
                    }
                    else {
                        if (menuWiseLinksId != "0") {
                            SecurityMenuWiseLinksDeleted.push({
                                MenuWiseLinksId: menuWiseLinksId,
                                UserId: userId,
                                MenuGroupId: menuGroupId,
                                MenuLinksId: $.trim($(this).find("td:eq(1)").text()),
                                DisplaySequence: $.trim($(this).find("td:eq(5)").find("input").val()),
                                IsSavePermission: $(this).find("td:eq(6)").find("input").is(":checked"),
                                IsUpdatePermission: $(this).find("td:eq(7)").find("input").is(":checked"),
                                IsDeletePermission: $(this).find("td:eq(8)").find("input").is(":checked"),
                                IsViewPermission: $(this).find("td:eq(9)").find("input").is(":checked"),
                                ActiveStat: true
                            });
                        }
                    }
                }


            });
            debugger;
            if (SecurityMenuWiseLinksNelyAdded.length == 0 && SecurityMenuWiseLinksEdited.length == 0 && SecurityMenuWiseLinksDeleted.length == 0) {
                toastr.info("Please Select Menu Links From Grid.");
                return false;
            }
            if (userId == "0") {
                PageMethods.SaveUserGroupWiseMenuNPermission(SecurityMenuWiseLinksNelyAdded, SecurityMenuWiseLinksEdited, SecurityMenuWiseLinksDeleted, userGroupId, menuGroupId, moduleId, OnSaveMenuLinksSucceed, OnSaveMenuLinksFailed);
            }
            else {
                PageMethods.SaveUserIdWiseMenuNPermission(SecurityMenuWiseLinksNelyAdded, SecurityMenuWiseLinksEdited, SecurityMenuWiseLinksDeleted, userId, menuGroupId, moduleId, OnSaveMenuLinksSucceed, OnSaveMenuLinksFailed);
            }

            return false;
        }

        function OnSaveMenuLinksSucceed(result) {
            if (result.AlertMessage.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSaveMenuLinksFailed() {

        }

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfApprEvaId" runat="server" Value="" />
    <div id="ApprEvaluationEntryPanel" class="panel panel-default">
        <div class="panel-heading">
            User Group Wise Menu & Access Permission
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblPermissionType" runat="server" class="control-label" Text="Permission Type"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlPermissionType" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="1">Group Wise Permission</asp:ListItem>
                            <asp:ListItem Value="2">Single User Permission</asp:ListItem>
                            <%--<asp:ListItem Value="3">Remove User Permission</asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="permissionWiseDiv">
                    <div class="form-group">
                        <div id="groupUserDiv">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="User Group"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlUserGroup" runat="server" CssClass="form-control"
                                    TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="singleUserDiv">
                            <div class="col-md-2">
                                <asp:Label ID="lblUserList" runat="server" class="control-label" Text="User"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlUserList" runat="server" CssClass="form-control"
                                    TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="Label2" runat="server" class="control-label" Text="Menu Group"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlMenuGroup" runat="server" CssClass="form-control"
                                TabIndex="3">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" class="control-label" Text="Menu Module"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlMenuModule" runat="server" CssClass="form-control"
                                TabIndex="3">
                            </asp:DropDownList>
                        </div>
                        
                    </div>
                </div>

                <div class="form-group">
                    <table id="UserWiseMenuAssign" style="width: 100%;" class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                                <th style="width: 6%; text-align: center;">Select<br />
                                    <input type="checkbox" id="chkAll" />
                                </th>
                                <th style="width: 43%;">Menu Name
                                </th>
                                <th style="width: 15%;">Type
                                </th>
                                <th style="width: 8%;">Display Sequence
                                </th>
                                <th style="width: 8%; text-align: center;">Create<br />
                                    <input type="checkbox" id="chkCreate" />
                                </th>
                                <th style="width: 8%; text-align: center;">Update<br />
                                    <input type="checkbox" id="chkUpdate" />
                                </th>
                                <th style="width: 8%; text-align: center;">Delete<br />
                                    <input type="checkbox" id="chkDelete" />
                                </th>
                                <th style="width: 8%; text-align: center;">View<br />
                                    <input type="checkbox" id="chkView" />
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnApprovePermission" runat="server" Text="Approve Permission" TabIndex="2"
                            CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript:return SaveMenuPermission()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
