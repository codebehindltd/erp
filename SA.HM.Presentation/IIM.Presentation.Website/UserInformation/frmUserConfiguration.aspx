<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmUserConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.UserInformation.frmUserConfiguration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var abc = {};
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false
        $(document).ready(function () {
            ;

            IsCanSave = $("#<%=hfSavePermission.ClientID %>").val() == '1' ? true : false;
            IsCanEdit = $("#<%=hfEditPermission.ClientID %>").val() == '1' ? true : false;
            IsCanDelete = $("#<%=hfDeletePermission.ClientID %>").val() == '1' ? true : false;
            IsCanView = $("#<%=hfViewPermission.ClientID %>").val() == '1' ? true : false;

            var moduleName = "<a href='/UserInformation/frmUserConfiguration.aspx' class='inActive'>Authorization Panel</a>";
            var formName = "<span class='divider'>/</span><li class='active'>User Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_ddlUserGroupName").select2({
                tags: "true",
                //placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlFeatures").select2({
                tags: "true",
                //placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlFeatures").change(function () {
                var userGroupId = $("#ContentPlaceHolder1_ddlUserGroupName").val();
                var featuresId = $("#ContentPlaceHolder1_ddlFeatures").val();

                if ($(this).val() == "0") {
                    return false;
                }
                else if (userGroupId == "0") {
                    return false;
                }
                GetLinkByGroupId();
            });
            $("#ContentPlaceHolder1_ddlUserGroupName").change( function(){
                if ($(this).val() == "0") {
                    return false;
                }
                GetLinkByGroupId();
            });            

            $("#chkCheckedBy").change(function () {
                if ($(this).is(":checked")) {
                    $("#GroupWiseUser tbody tr").find("td:eq(3)").find("input").prop("checked", true);
                }
                else {
                    $("#GroupWiseUser tbody tr").find("td:eq(3)").find("input").prop("checked", false);
                }
            });

            $("#chkApprovedBy").change(function () {
                if ($(this).is(":checked")) {
                    $("#GroupWiseUser tbody tr").find("td:eq(4)").find("input").prop("checked", true);
                }
                else {
                    $("#GroupWiseUser tbody tr").find("td:eq(4)").find("input").prop("checked", false);
                }
            });
        });

        function GetLinkByGroupId() {
            var userGroupId = $("#ContentPlaceHolder1_ddlUserGroupName").val();
            var featuresId = $("#ContentPlaceHolder1_ddlFeatures").val();

            if (userGroupId == "0") {
                toastr.info("Please Select Group Name");
                return false;
            }
            else if (featuresId == "0")
            {
                toastr.info("Please Select Any Features");
                return false;
            }

            PageMethods.GetLinkByGroupId(userGroupId, featuresId, OnLoadUserSucceed, OnSaveMenuLinksFailed);
        }

        function OnLoadUserSucceed(result) {

            $("#GroupWiseUser tbody").html("");

            abc = result;

            var userInformationList = result[0].UserList;
            var userConfiguredList = result[0].UserConfigured;

            var isCheckedBy = "", isApprovedBy = "";
            var i = 0, userLength = userInformationList.length;
            var tr = "";

            for(i=0; i<userLength; i++)
            {
                if (i % 2 == 0) {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }
                else {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                //var alreadySavedLink = _.findWhere(menuWiseLinks, { MenuLinksId: menuLinks[i].MenuLinksId });
                var alreadySavedUsers = _.findWhere(userConfiguredList, { UserInfoId: userInformationList[i].UserInfoId });
                

                if(alreadySavedUsers!=null)
                {
                    //toastr.info(alreadySavedUsers.UserInfoId);
                    tr += "<td id =user" + userInformationList[i].UserInfoId + "' style=\"display:none;\">" + alreadySavedUsers.UserInfoId + "</td>";
                    isCheckedBy = alreadySavedUsers.IsCheckedBy == true ? "checked='checked'" : "";
                    isApprovedBy = alreadySavedUsers.IsApprovedBy == true ? "checked='checked'" : "";
                }
                else
                {
                    tr += "<td id =user" + userInformationList[i].UserInfoId + "' style=\"display:none;\">0</td>";
                    isCheckedBy = "";
                    isApprovedBy = "";
                }

                tr += "<td style=\"display:none;\">" + userInformationList[i].UserInfoId + "</td>";

                tr += "<td style=\"width: 50%;\">" +
                                 userInformationList[i].UserIdAndName +
                            "</td>" +
                            "<td style=\"width: 25%; text-align:center;\">" +
                                 "<input type='checkbox' id='create" + userInformationList[i].UserInfoId + "'" + isCheckedBy + "/>" +
                            "</td>" +
                            "<td style=\"width: 25%; text-align:center;\">" +
                                 "<input type='checkbox' id='create" + userInformationList[i].UserInfoId + "'" + isApprovedBy + "/>" +
                            "</td>"
                "</tr>";
                $("#GroupWiseUser tbody").append(tr);

                tr = "";
                isCheckedBy = "", isApprovedBy = "";
            }
            CommonHelper.ApplyIntigerValidation();

        }

        function SaveUserPermission() {

            var GroupWiseUserAdd = new Array();
            var GroupWiseUserEdit = new Array();
            var userGroupId = "0", featuresId = "0", groupWiseUserId="0", userInfoId = "";

             userGroupId = $("#ContentPlaceHolder1_ddlUserGroupName").val();
             featuresId = $("#ContentPlaceHolder1_ddlFeatures").val();

             
             if (userGroupId == "0") {
                 toastr.info("Please Select Group Name");
                 return false;
             }
             else if (featuresId == "0") {
                 toastr.info("Please Select Any Features");
                 return false;
             }
             
             $("#GroupWiseUser tbody tr").each(function () {

                 groupWiseUserId = $.trim($(this).find("td:eq(0)").text());
                 userInfoId = $.trim($(this).find("td:eq(1)").text());

                     if(groupWiseUserId != "0")
                     {                         
                        GroupWiseUserEdit.push({
                             FeaturesId: featuresId,
                             UserInfoId: userInfoId,
                             IsCheckedBy: $(this).find("td:eq(3)").find("input").is(":checked"),
                             IsApprovedBy: $(this).find("td:eq(4)").find("input").is(":checked")
                         });
                         
                     }
                     else {
                         
                         GroupWiseUserAdd.push({
                             FeaturesId: featuresId,
                             UserInfoId: userInfoId,
                             IsCheckedBy: $(this).find("td:eq(3)").find("input").is(":checked"),
                             IsApprovedBy: $(this).find("td:eq(4)").find("input").is(":checked")
                         });
                     }
                
             });

             if (GroupWiseUserAdd.length == 0 && GroupWiseUserEdit.length == 0) {
                 toastr.info("Please Select Users From List.");
                 return false;
             }

             PageMethods.SaveCheckedByApprovedByUsers(GroupWiseUserAdd, GroupWiseUserEdit, userGroupId, featuresId, OnSaveSucceed, OnSaveMenuLinksFailed);

             return false;
        }

        function OnSaveSucceed(result) {
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
   <div id="ApprEvaluationEntryPanel" class="panel panel-default">
        <div class="panel-heading">
            User Configuration</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Features"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFeatures" runat="server" CssClass="form-control"
                            TabIndex="3">
                        </asp:DropDownList>
                    </div>
                    <div class="col-offset-2">
                        <%--<asp:Label ID="Label2" runat="server" class="control-label" Text="User Group"></asp:Label>--%>
                    </div>
                    <div class="col-offset-4">
                        <%--<asp:DropDownList ID="ddlUserGroup" runat="server" CssClass="form-control"
                            TabIndex="3">
                        </asp:DropDownList>--%>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Group Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlUserGroupName" runat="server" CssClass="form-control"
                            TabIndex="3">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                    </div>
                </div>
                <div class="form-group">
                    <table id="GroupWiseUser" style="width: 100%;" class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                                <%--<th style="width: 6%; text-align: center;">
                                    Select<br />
                                    <input type="checkbox" id="chkAll" />
                                </th>--%>
                                <th style="width: 50%; ">
                                    User
                                </th>                                
                                <th style="width: 25%; text-align: center;">
                                    Checked By<br />
                                    <input type="checkbox" id="chkCheckedBy" />
                                </th>
                                <th style="width: 25%; text-align: center;">
                                    Approved By<br />
                                    <input type="checkbox" id="chkApprovedBy" />
                                </th>                                
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnApprovePermission" runat="server" Text="Save" TabIndex="2"
                            CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript:return SaveUserPermission()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
