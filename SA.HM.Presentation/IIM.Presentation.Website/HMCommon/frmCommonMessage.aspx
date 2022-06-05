<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmCommonMessage.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmCommonMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var UserInformation = [];

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Message Compose</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                var msg = JSON.parse($("#InnboardMessageHiddenField").val());
                CommonHelper.AlertMessage(msg);
                $(".InnboardMessageHiddenField").val("");

                if (msg.IsSuccess == "1") {
                    if ($("#ContentPlaceHolder1_ddlMessageType").val() == "Individual") {
                        $(".GroupMessageContainer").hide();
                        $(".IndividualMessageContainer").show();
                    }
                    else if ($("#ContentPlaceHolder1_ddlMessageType").val() == "Group") {
                        $(".GroupMessageContainer").show();
                        $(".IndividualMessageContainer").hide();
                    }

                    $("#ContentPlaceHolder1_hfMessage").val("");
                    $("#ContentPlaceHolder1_hfMessageDetails").val("");
                }
                else if (msg.IsSuccess == "0") {
                    if ($("#ContentPlaceHolder1_ddlMessageType").val() == "Individual") {
                        $(".GroupMessageContainer").hide();
                        $(".IndividualMessageContainer").show();

                        var UserInformationIndividual = JSON.parse($("#ContentPlaceHolder1_hfMessageDetails").val());
                        var rowLengths = UserInformationIndividual.length, i = 0;

                        for (i = 0; i < rowLengths; i++) {
                            UserInformation[0] = UserInformationIndividual[i];
                            AddIndividualUser();
                        }
                    }
                    else if ($("#ContentPlaceHolder1_ddlMessageType").val() == "Group") {
                        $(".GroupMessageContainer").show();
                        $(".IndividualMessageContainer").hide();

                        if ($("#ContentPlaceHolder1_ddlUserGroup").val() != "0") {

                            var rowCounts = 0, objectLength = 0;
                            var UserInformationGroups = JSON.parse($("#ContentPlaceHolder1_hfMessageDetails").val());
                            objectLength = UserInformationGroups.length;

                            UserLoading($("#ContentPlaceHolder1_ddlUserGroup").val()).done(function () {

                                $("#GrouplyMailUserGrid tbody tr").each(function () {
                                    if (rowCounts < objectLength) {
                                        if (parseInt($.trim($(this).find("td:eq(4)").text()), 10) == UserInformationGroups[rowCounts].MessageTo) {
                                            $(this).find("td:eq(0)").find("input").prop("checked", true);
                                            rowCounts++;
                                        }
                                    }
                                });
                            });
                        }
                    }
                }
            }
            else {
                $(".GroupMessageContainer").hide();
            }

            $("#ContentPlaceHolder1_ddlUserGroup").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlMessageType").change(function () {

                $("#IndividualMailUser tbody").html("");
                $("#GrouplyMailUserGrid tbody").html("");

                if ($(this).val() == "Individual") {
                    $(".GroupMessageContainer").hide();
                    $(".IndividualMessageContainer").show();
                }
                else if ($(this).val() == "Group") {
                    $(".GroupMessageContainer").show();
                    $(".IndividualMessageContainer").hide();
                }
            });

            $("#ContentPlaceHolder1_ddlUserGroup").change(function () {
                $("#GrouplyMailUserGrid tbody").html("");
                UserLoadByGroup($(this).val());
            });

            $("#btnAddUser").click(function () {

                if (UserInformation.length == 0) {
                    toastr.warning("Please select an user.");
                    return false;
                }

                AddIndividualUser();

                return false;
            });

            $("#btnCancelUser").click(function () {
                $("#txtUserName").val("");
                UserInformation = [];
            });

            $("#txtUserName").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../HMCommon/frmCommonMessage.aspx/GetUserInformationAutoSearch',
                        data: "{'searchTerm':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.UserId + ' - ' + m.UserName,
                                    value: m.UserInfoId,
                                    UserInfoId: m.UserInfoId,
                                    UserName: m.UserName,
                                    UserPhone: m.UserPhone,
                                    UserId: m.UserId

                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field

                    UserInformation.push(ui.item);
                    $(this).val(ui.item.label);
                }
            });

        });

        function AddIndividualUser() {

            var rowLength = $("#IndividualMailUser tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:50%;'>" + UserInformation[0].UserName + "</td>";
            tr += "<td style='width:20%;'>" + UserInformation[0].UserId + "</td>";
            tr += "<td style='width:20%;'>" + UserInformation[0].UserPhone + "</td>";

            tr += "<td style='width:20%;'>";
            tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteUser(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";


            if (UserInformation[0].UserInfoId != null) {
                tr += "<td style='display:none'>" + UserInformation[0].UserInfoId + "</td>";
            }
            else {
                tr += "<td style='display:none'>" + UserInformation[0].MessageTo + "</td>";
            }

            tr += "</tr>";

            $("#IndividualMailUser tbody").append(tr);
            $("#txtUserName").val("");
            // UserInformation = [];
        }

        function DeleteUser(deleteUser) {
            $(deleteUser).parent().parent().remove();
        }

        function CheckBeforeSave() {

            if ($("#ContentPlaceHolder1_ddlMessageType").val() == "Individual") {

                if ($("#IndividualMailUser tbody tr").length == 0) {
                    toastr.warning("Please Add User");
                    return false;
                }

            }
            else if ($("#ContentPlaceHolder1_ddlMessageType").val() == "Group" && $("#ContentPlaceHolder1_ddlUserGroup").val() != "0") {
                if ($("#GrouplyMailUserGrid tbody tr").find("td:eq(0)").find("input:checked").length == 0) {
                    toastr.warning("Please Select User");
                    return false;
                }
            }

            if ($.trim($("#ContentPlaceHolder1_txtMessageSubject").val()) == "") {
                toastr.warning("Please Give Message Subject");
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtMessageBody").val()) == "") {
                toastr.warning("Please Give Message Details");
                return false;
            }

            var messageDetails = [];
            var userName = "", userinfoId = "0", userId = "", userPhone = "", messageSubject = "", messageBody = "";

            messageSubject = $("#ContentPlaceHolder1_txtMessageSubject").val();
            messageBody = $("#ContentPlaceHolder1_txtMessageBody").val();

            var CommonMessage = {
                Subjects: messageSubject,
                MessageBody: messageBody
            };

            if ($("#ContentPlaceHolder1_ddlMessageType").val() == "Individual") {

                $("#IndividualMailUser tbody tr").each(function () {

                    userinfoId = $.trim($(this).find("td:eq(4)").text());
                    userId = $.trim($(this).find("td:eq(1)").text());
                    userPhone = $.trim($(this).find("td:eq(2)").text());
                    userName = $.trim($(this).find("td:eq(0)").text());

                    messageDetails.push({
                        MessageTo: userinfoId,
                        UserId: userId,
                        UserName: userName,
                        UserPhone: userPhone
                    });

                });
            }
            else if ($("#ContentPlaceHolder1_ddlMessageType").val() == "Group" && $("#ContentPlaceHolder1_ddlUserGroup").val() != "0") {

                $("#GrouplyMailUserGrid tbody tr").each(function () {
                    if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                        userinfoId = $.trim($(this).find("td:eq(4)").text());
                        userId = $.trim($(this).find("td:eq(2)").text());

                        messageDetails.push({
                            MessageTo: userinfoId,
                            UserId: userId
                        });
                    }
                });
            }

            $("#<%=hfMessage.ClientID %>").val(JSON.stringify(CommonMessage));
            $("#<%=hfMessageDetails.ClientID %>").val(JSON.stringify(messageDetails));

            return true;
        }

        function UserLoadByGroup(groupId) {
            UserLoading(groupId);
            return false;
        }

        function UserLoading(groupId) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HMCommon/frmCommonMessage.aspx/LoadUserByGroup',
                data: "{'groupId':'" + groupId + "'}",
                dataType: "json",
                success: function (data) {

                    var rowLength = data.d.length;

                    var tr = "", i = 0;

                    for (i = 0; i < rowLength; i++) {

                        if (i % 2 == 0) {
                            tr += "<tr style='background-color:#FFFFFF;'>";
                        }
                        else {
                            tr += "<tr style='background-color:#E3EAEB;'>";
                        }

                        tr += "<td style='width:7%; text-align:center;'>";
                        tr += "<input type='checkbox' value='' />";
                        tr += "</td>";
                        tr += "<td style='width:53%;'>" + data.d[i].UserName + "</td>";
                        tr += "<td style='width:20%;'>" + data.d[i].UserId + "</td>";
                        tr += "<td style='width:20%;'>" + data.d[i].UserPhone + "</td>";

                        tr += "<td style='display:none'>" + data.d[i].UserInfoId + "</td>";
                        tr += "</tr>";

                        $("#GrouplyMailUserGrid tbody").append(tr);
                        tr = "";
                    }

                    return false;

                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }

        function CheckAllUser(topCheckBox) {
            if ($(topCheckBox).is(":checked") == true) {
                $("#GrouplyMailUserGrid tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#GrouplyMailUserGrid tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }

    </script>
    <asp:HiddenField ID="hfMessage" runat="server" Value="" />
    <asp:HiddenField ID="hfMessageDetails" runat="server" Value="" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Compose Message</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Sent Message</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default" style="margin: 5px;">
                <div class="panel-heading">User Info</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMessageMode" runat="server" class="control-label" Text="Message Type"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlMessageType" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                    <asp:ListItem Value="Individual" Text="Individual Message"></asp:ListItem>
                                    <asp:ListItem Value="Group" Text="Group Message"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="GroupMessageContainer">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="User Group"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlUserGroup" runat="server" CssClass="form-control"
                                        TabIndex="2">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="IndividualMessageContainer">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="User Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtUserName" runat="server" TabIndex="5" CssClass="form-control"
                                        ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="padding: 5px 0 5px 0;">
                                <input id="btnAddUser" type="button" value="Add User" tabindex="8" class="TransactionalButton btn btn-primary btn-sm" />
                                <input id="btnCancelUser" type="button" value="Cancel User" tabindex="9" class="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                        <div class="IndividualMessageContainer">
                            <div class="form-group">
                                <table id="IndividualMailUser" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                                    <thead>
                                        <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                            <th style="width: 50%;">User Name
                                            </th>
                                            <th style="width: 20%;">User Id
                                            </th>
                                            <th style="width: 20%;">User Phone
                                            </th>
                                            <th style="width: 10%;">Action
                                            </th>
                                            <th style="display: none;">User Info Id
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="GroupMessageContainer" style="margin-top: 7px;">
                            <div class="form-group">
                                <table id="GrouplyMailUserGrid" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                                    <thead>
                                        <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                            <th style="width: 7%; text-align: center;">
                                                <input type='checkbox' value='' onchange="CheckAllUser(this)" style="padding: 0; margin: 3px;" />
                                            </th>
                                            <th style="width: 53%;">User Name
                                            </th>
                                            <th style="width: 20%;">User Id
                                            </th>
                                            <th style="width: 20%;">User Phone
                                            </th>
                                            <th style="display: none;">User Info Id
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Div1" class="panel panel-default" style="margin: 5px;">
                <div class="panel-heading">Compose Message</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Subject"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtMessageSubject" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Body"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtMessageBody" Rows="9" Columns="50" TextMode="MultiLine" CssClass="form-control"
                                    runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="padding: 5px 0 0 5px;">
                <div class="col-md-12">
                    <asp:Button ID="btnSendMail" runat="server" TabIndex="8" Text="Send Message" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript:return CheckBeforeSave();" OnClick="btnSendMail_Click" />
                    <asp:Button ID="btnClear" runat="server" TabIndex="9" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return PerformClearAction();" />
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default" style="margin: 5px;">
                <div class="panel-heading">
                    Send Message Information
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvMessageSend" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                        CssClass="table table-bordered table-condensed table-responsive" OnPageIndexChanging="gvMessageSend_PageIndexChanging">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="Message Id" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("MessageId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UserName" HeaderText="To User Name" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Sending Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvMessageDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("MessageDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Subjects" HeaderText="Subjects" ItemStyle-Width="35%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MessageBody" HeaderText="Message Body" ItemStyle-Width="40%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#7C6F57" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
