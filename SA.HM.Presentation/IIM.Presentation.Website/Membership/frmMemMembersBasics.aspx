<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmMemMembersBasics.aspx.cs" Inherits="HotelManagement.Presentation.Website.Membership.frmMemMembersBasics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var msg = "";
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Membership</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Membership Info</li>";
            var breadCrumbs = moduleName + formName;
            var myVar;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                msg = JSON.parse($("#InnboardMessageHiddenField").val());

                $("#InnboardMessageHiddenField").val("");
            }


            $("#SearchOutput").hide();
            $("#btnSearch").click(function () {
                $("#SearchOutput").show('slow');
                GridPaging(1, 1);
            });

            $("#ContentPlaceHolder1_ddlTitle").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var middleName = $("#<%=txtMiddleName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                if (title != "none") {
                    $("#<%=txtDisplayName.ClientID %>").val(title + " " + firstName + " " + middleName + " " + lastName);
                }
                else $("#<%=txtDisplayName.ClientID %>").val(firstName + " " + middleName + " " + lastName);
            });

            $("#ContentPlaceHolder1_txtFirstName").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var middleName = $("#<%=txtMiddleName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                if (title != "none") {
                    $("#<%=txtDisplayName.ClientID %>").val(title + " " + firstName + " " + middleName + " " + lastName);
                }
                else $("#<%=txtDisplayName.ClientID %>").val(firstName + " " + middleName + " " + lastName);
            });

            $("#ContentPlaceHolder1_txtMiddleName").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var middleName = $("#<%=txtMiddleName.ClientID %>").val();
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                if (title != "none") {
                    $("#<%=txtDisplayName.ClientID %>").val(title + " " + firstName + " " + middleName + " " + lastName);
                }
                else $("#<%=txtDisplayName.ClientID %>").val(firstName + " " + middleName + " " + lastName);
            });

            $("#ContentPlaceHolder1_txtLastName").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var middleName = $("#<%=txtMiddleName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                if (title != "none") {
                    $("#<%=txtDisplayName.ClientID %>").val(title + " " + firstName + " " + middleName + " " + lastName);
                }
                else $("#<%=txtDisplayName.ClientID %>").val(firstName + " " + middleName + " " + lastName);
            });

            var btn = $("#<%=btnSave.ClientID %>").val();
            if (btn == "Update") {
                //$("#MemberId").show();
            }

            $('#ContentPlaceHolder1_A').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_B').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_C').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_D').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_E').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_F').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_G').click(function () {
                $('#SubmitButtonDiv').hide();
            });

            var txtRegDate = '<%=txtRegDate.ClientID%>'
            var txtExpiryDate = '<%=txtExpiryDate.ClientID%>'
            var txtMemDOB = '<%=txtMemDOB.ClientID%>'
            var txtDOB = '<%=txtDOB.ClientID%>'
            var txtNomineeDOB = '<%=txtNomineeDOB.ClientID%>'

            $('#' + txtRegDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtExpiryDate).datepicker("option", "minDate", selectedDate);
                }
            });
            $('#' + txtExpiryDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtRegDate).datepicker("option", "maxDate", selectedDate);
                }
            });
            $('#' + txtMemDOB).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $('#' + txtDOB).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $('#' + txtNomineeDOB).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        });

        $(function () {
            $("#myTabs").tabs();
        });

        //For Password Validation-------------------------
        function confirmPass() {
            if ($("#<%=txtMemberPassword.ClientID %>").val() != $("#<%=txtConfirmMemberPassword.ClientID %>").val()) {
                toastr.warning('Wrong confirm password !');
                $("#<%=txtConfirmMemberPassword.ClientID %>").val("");
                return false;
            }
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvMember tbody tr").length;

            var memberName = $("#<%=txtSMemberName.ClientID %>").val();
            var code = $("#<%=txtMemberCode.ClientID %>").val();
            PageMethods.SearchNLoadMemberInformation(memberName, code, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvMember tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvMember tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvMember tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:50%; cursor:pointer;\">" + gridObject.FullName + "</td>";
                tr += "<td align='left' style=\"width:40%; cursor:pointer;\">" + gridObject.MembershipNumber + "</td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.MemberId + "')\" alt='Edit Information' border='0' /></td>";

                tr += "</tr>"

                $("#gvMember tbody ").append(tr);
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

        function PerformEditAction(memberId) {
            var possiblePath = "frmMemMembersBasics.aspx?editId=" + memberId;
            window.location = possiblePath;
        }

    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Basics</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Occupation</a></li>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Educational</a></li>
            <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-4">Family Info</a></li>
            <li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-5">Nominee</a></li>
            <li id="F" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-6">Reference</a></li>
            <li id="G" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-7">Search Member</a></li>
        </ul>
        <div id="tab-1">
            <div id="BasicInformation" class="panel panel-default">
                <div class="panel-heading">
                    Member Basic Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <asp:HiddenField ID="hfMemberId" runat="server" />
                        <asp:HiddenField ID="hfCompanyId" runat="server" />
                        <asp:HiddenField ID="hfIsMemberPasswordPanalEnable" runat="server" />                        
                        <asp:HiddenField ID="txtNodeId" runat="server"></asp:HiddenField>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMemberType" runat="server" class="control-label required-field"
                                    Text="Member Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlMemberType" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div id="MemberId">
                                <div class="col-md-2">
                                    <asp:Label ID="lblMemCode" runat="server" class="control-label " Text="Membership No."></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtMemCode" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTitle" runat="server" class="control-label" Text="Title"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <%--<asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>--%>
                                <asp:DropDownList ID="ddlTitle" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="none">None</asp:ListItem>
                                    <asp:ListItem Value="Mr.">Mr.</asp:ListItem>
                                    <asp:ListItem Value="Mrs.">Mrs.</asp:ListItem>
                                    <asp:ListItem Value="Miss">Miss</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblFirstName" runat="server" class="control-label required-field"
                                    Text="First Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFirstName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMiddleName" runat="server" class="control-label" Text="Last Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMiddleName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLastName" runat="server" class="control-label required-field" Text="Sur Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLastName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDisplayName" runat="server" class="control-label required-field"
                                    Text="Full Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFathersName" runat="server" class="control-label" Text="Father's Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtFathersName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMothersName" runat="server" class="control-label" Text="Mother's Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtMothersName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMemDOB" runat="server" class="control-label required-field" Text="Date Of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMemDOB" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblGender" runat="server" class="control-label required-field" Text="Gender"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblBloodGroup" runat="server" class="control-label required-field" Text="Blood Group"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblMaritalStatus" runat="server" class="control-label required-field"
                                    Text="Marital Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlMaritalStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label26" runat="server" class="control-label" Text="Height (cms)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtHeight" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label27" runat="server" class="control-label" Text="Weight (kg)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtWeight" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblNatinality" runat="server" class="control-label required-field"
                                    Text="Nationality"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCountryId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPassportNumber" runat="server" class="control-label" Text="Passport Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassportNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <%--<div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblOccupation" runat="server" class="control-label" Text="Occupation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtOccupation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblOrganization" runat="server" class="control-label" Text="Organization"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtOrganization" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>--%>
                        <%--<div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDesignation" runat="server" class="control-label" Text="Designation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDesignation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblMonthlyIncome" runat="server" class="control-label" Text="Monthly Income"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMonthlyIncome" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>--%>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAnnualTurnover" runat="server" class="control-label" Text="Annual Turnover"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAnnualTurnover" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSecurityDep" runat="server" class="control-label required-field"
                                    Text="Security Deposit"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSecurityDep" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRegDate" runat="server" class="control-label" Text="Registration Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRegDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblExpiryDate" runat="server" class="control-label required-field"
                                    Text="Expiry Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtExpiryDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentAddress" runat="server" class="control-label" Text="Present Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPresentAddress" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMailingAddress" runat="server" class="control-label" Text="Mailing Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtMailingAddress" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <%--<div class="col-md-2">
                                <asp:Label ID="Label23" runat="server" class="control-label required-field" Text="Mobile No."></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" TabIndex="41"></asp:TextBox>
                            </div>--%>
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field"
                                    Text="Religion"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblHomePhone" runat="server" class="control-label" Text="Home Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtHomePhone" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblOfficePhone" runat="server" class="control-label" Text="Office Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtOfficePhone" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblHomeFax" runat="server" class="control-label" Text="Home Fax"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtHomeFax" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblOfficeFax" runat="server" class="control-label" Text="Office Fax"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtOfficeFax" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPersonalEmail" runat="server" class="control-label" Text="Personal Email"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPersonalEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblOfficialEmail" runat="server" class="control-label" Text="Official Email"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtOfficialEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div id="MemberPasswordDiv" class="form-group" runat="server">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Password"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMemberPassword" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Confirm Password"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtConfirmMemberPassword" TextMode="Password" CssClass="form-control" onblur="javascript: return confirmPass();" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="Occupation" class="panel panel-default">
                <div class="panel-heading">
                    Occupation Details
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="Occupation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtOccupation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label9" runat="server" class="control-label" Text="Designation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDesignation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label10" runat="server" class="control-label" Text="Organization"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtOrganization" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Monthly Income"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMonthlyIncome" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label11" runat="server" class="control-label" Text="Office Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtOfficeAddress" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3">
            <div id="Education" class="panel panel-default">
                <asp:Label ID="hfEducationId" runat="server" Text='' Visible="False"></asp:Label>
                <div class="panel-heading">
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10">
                                <label class="control-label">(Please add last one first and accordingly.)</label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label16" runat="server" class="control-label" Text="Name Of Degree"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDegree" CssClass="form-control" runat="server" TextMode="SingleLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label17" runat="server" class="control-label" Text="Institution"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtInstitution" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label18" runat="server" class="control-label" Text="Passing Year"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassingYear" CssClass="form-control quantity" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-offset-2">
                            <asp:Button ID="btnAddInst" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary btn-sm" OnClick="btnAddInst_Click" />
                            <%--<input id="btnAddInst" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" />--%>
                        </div>
                        &nbsp;
                        <div>
                            <asp:GridView ID="gvEducationList" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" PageSize="5" OnRowCommand="gvEducationList_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Degree" HeaderText="Name of Degree" ItemStyle-Width="30%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Institution" HeaderText="Institution" ItemStyle-Width="30%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PassingYear" HeaderText="Passing Year" ItemStyle-Width="20%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("Id") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("Id") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to Delete?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="Small" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
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

                        <%--<div class="form-group" id="tblEducationalDiv" style="display: none">
                            <table runat="server" id="tblEducational" class="table table-bordered table-condensed table-responsive" style="width: 100%">
                                <colgroup>
                                    <col style="width: 30%;" />
                                    <col style="width: 30%;" />
                                    <col style="width: 20%;" />
                                    <col style="width: 20%;" />
                                </colgroup>
                                <thead>
                                    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                        <td style="text-align: left;">Name Of Degree</td>
                                        <td style="text-align: left;">Institution</td>
                                        <td style="text-align: left;">Passing Year</td>
                                        <td style="text-align: center;">Action</td>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>--%>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-4">
            <div id="FamilyInfo" class="panel panel-default">
                <div class="panel-heading">
                    Family Member Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMemberName" runat="server" class="control-label required-field"
                                    Text="Member Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtMemberName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDOB" runat="server" class="control-label required-field" Text="Date Of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDOB" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblFMOccupation" runat="server" class="control-label" Text="Occupation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFMOccupation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRelationship" runat="server" class="control-label" Text="Relationship"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRelationship" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblUsageMode" runat="server" class="control-label" Text="Usage Mode"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlUsageMode" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="Active">Active</asp:ListItem>
                                    <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-offset-2 col-md-12">
                                <asp:Button ID="btnFamilyMember" runat="server" Text="Add" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnFamilyMember_Click" />
                                <asp:Label ID="hfFamilyMemberId" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gvFamilyMember" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" PageSize="5" OnRowCommand="gvFamilyMember_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MemberName" HeaderText="Name" ItemStyle-Width="40%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Relationship" HeaderText="Relationship" ItemStyle-Width="28%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UsageMode" HeaderText="Usage Mode" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("Id") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("Id") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to save?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="Small" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
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
        </div>
        <div id="tab-5">
            <div id="Nominee" class="panel panel-default">
                <div class="panel-heading">
                    Nominee Details
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label30" runat="server" class="control-label" Text="Name of Nominee"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtNomineeName" CssClass="form-control" runat="server" TextMode="SingleLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label31" runat="server" class="control-label" Text="Father's Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtNomineeFather" CssClass="form-control" runat="server" TextMode="SingleLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label32" runat="server" class="control-label" Text="Mother's Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtNomineeMother" CssClass="form-control" runat="server" TextMode="SingleLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label33" runat="server" class="control-label" Text="Date of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNomineeDOB" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label34" runat="server" class="control-label" Text="Relation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlNomineeRelation" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-6">
            <div id="ReferenceInfo" class="panel panel-default">
                <div class="panel-heading">
                    Reference Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblArbitrator" runat="server" class="control-label required-field"
                                    Text="Arbitrator"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtArbitrator" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblArbitratorMode" runat="server" class="control-label" Text="Arbitrator Mode"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlArbitratorMode" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="Proposed">Proposed</asp:ListItem>
                                    <asp:ListItem Value="Seconder">Seconder</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblArbRelationship" runat="server" class="control-label" Text="Relationship"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlArbRelationship" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class=" col-md-offset-2 col-md-12">
                                <asp:Button ID="btnReference" runat="server" Text="Add" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnReference_Click" />
                                <asp:Label ID="hfReferenceId" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>
                        &nbsp;
                        <div>
                            <asp:GridView ID="gvReference" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                                OnRowCommand="gvReference_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("ReferenceId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Arbitrator" HeaderText="Arbitrator" ItemStyle-Width="40%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ArbitratorMode" HeaderText="Arbitrator Mode" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Relationship" HeaderText="Relationship" ItemStyle-Width="28%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("ReferenceId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("ReferenceId") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to save?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="Small" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
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
        </div>
        <div id="tab-7">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMemberCode" runat="server" class="control-label" Text="Member Id"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMemberCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSMemberName" runat="server" class="control-label" Text="Member Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSMemberName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm">
                                    Search</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="SearchOutput">
                    <div class="panel-body">
                        <table id='gvMember' class="table table-bordered table-condensed table-responsive"
                            width="100%">
                            <colgroup>
                                <col style="width: 50%;" />
                                <col style="width: 40%;" />
                                <col style="width: 10%;" />
                            </colgroup>
                            <thead>
                                <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                    <td>Name
                                    </td>
                                    <td>Code
                                    </td>
                                    <td style="text-align: right;">Action
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
    </div>
    <div class="row" id="SubmitButtonDiv" style="padding: 10px 0 0 15px;">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
            OnClick="btnSave_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
            OnClick="btnCancel_Click" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if (msg.AlertType == "success") {
                setTimeout(PerformRedirect(), 12000);
                msg = "";
            }
        });

        function PerformRedirect() {
            window.location = "frmMemMembersBasics.aspx";
        }
    </script>
</asp:Content>

