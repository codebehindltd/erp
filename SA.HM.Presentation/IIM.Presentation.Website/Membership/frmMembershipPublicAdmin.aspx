<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnBoard.Master" AutoEventWireup="true" CodeBehind="frmMembershipPublicAdmin.aspx.cs" Inherits="HotelManagement.Presentation.Website.Membership.frmMembershipPublicAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var isReject = false;
        var isDefer = false;
        var isAdmin = false;
        var isIntorducer = false;
        var hasIntro = false;
        var hasAdmin = false;
        var isValidAdminInput = false;
        $(document).ready(function () {

            ReadyCall();

            $('#ContentPlaceHolder1_txtMeetingDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $('#ContentPlaceHolder1_txtMeetingDateEC').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#btnSearch").click(function () {
                $("#SearchOutput").show('slow');
                GridPaging(1, 1);
            });

            $("#btnAdminOk").click(function () {
                CheckAdminPart();

            });
            $("#editImg").click(function () {
                EnableAdminInput();
                $("#<%=txtMeetingDate.ClientID %>").val($("#<%=lblMeetingDate.ClientID %>").text());
                $("#<%=txtMeetingDecision.ClientID %>").val($("#<%=lblMeetingDecision.ClientID %>").text());
                $("#<%=txtMeetingDateEC.ClientID %>").val($("#<%=lblMeetingDateEC.ClientID %>").text());
                $("#<%=txtMeetingDecisionEC.ClientID %>").val($("#<%=lblMeetingDecisionEC.ClientID %>").text());


                $("#<%=lblMeetingDate.ClientID %>").text("");
                $("#<%=lblMeetingDecision.ClientID %>").text("");
                $("#<%=lblMeetingDateEC.ClientID %>").text("");
                $("#<%=lblMeetingDecisionEC.ClientID %>").text("");



            });

            $("#btnAccept").click(function () {

                <%--

                if (isAdmin) {
                    CheckIntroducer();
                    if (hasIntro == false) {
                        return false;
                    }
                    if (MeetingDate == "") {
                        toastr.warning("Please provide information of admin part.");
                        $("#ContentPlaceHolder1_txtRemarks").focus();
                        return false;
                    }
                }--%>
                var introId = $("#ContentPlaceHolder1_hfIntroducerId").val();
                var validAdmin = ValidateAdmin();
                if (isAdmin) {
                    if (isValidAdminInput == false) {
                        toastr.warning("Please provide admin input.");
                        return false;
                    }
                    if (!validAdmin) {
                        toastr.warning("Please input admin part informations.");
                        return false;
                    }
                }
                else if (!isIntorducer && !isAdmin) {
                    toastr.warning("Not a Valid Intorducer");
                    return false;
                }
                
                var answer = confirm("Are you sure to Accept?");

                var memberId = $("#ContentPlaceHolder1_hfMemberId").val();
                

                var MeetingDate = $("#<%=lblMeetingDate.ClientID %>").text();
                var MeetingDecision = $("#<%=lblMeetingDecision.ClientID %>").text();
                var MeetingDateEC = $("#<%=lblMeetingDateEC.ClientID %>").text();
                var MeetingDecisionEC = $("#<%=lblMeetingDecisionEC.ClientID %>").text();

                if (answer) {

                    CommonHelper.SpinnerOpen();
                    if (introId != "0") {
                        PageMethods.UpdateAndAcceptMemByIntroducer(memberId, introId, OnLoadIntroUpdateAndAcceptSucceed, OnSearchFailed)
                    }
                    else {
                        PageMethods.UpdateAndAcceptMember(memberId, MeetingDate, MeetingDecision, MeetingDateEC, MeetingDecisionEC, OnLoadUpdateAndAcceptSucceed, OnSearchFailed);
                    }

                }
                return false;
            });
            $("#btnReject").click(function () {
                //CheckAdminPart();
               <%-- var MeetingDate = $("#<%=lblMeetingDate.ClientID %>").text();
                var MeetingDecision = $("#<%=lblMeetingDecision.ClientID %>").text();
                var MeetingDateEC = $("#<%=lblMeetingDateEC.ClientID %>").text();
                var MeetingDecisionEC = $("#<%=lblMeetingDecisionEC.ClientID %>").text();

                if (isAdmin) {
                    CheckIntroducer();
                    if (hasIntro == false) {
                        return false;
                    }
                    if (MeetingDate == "") {
                        toastr.warning("Please provide information of admin part.");
                        $("#ContentPlaceHolder1_txtRemarks").focus();
                        return false;
                    }
                }--%>
                ValidateAdmin();
                if (isAdmin) {
                    if (isValidAdminInput == false) {
                        toastr.warning("Please provide admin input.");
                        return false;
                    }
                }
                var answer = confirm("Are you sure to Reject?");
                var memberId = $("#ContentPlaceHolder1_hfMemberId").val();
                var introId = $("#ContentPlaceHolder1_hfIntroducerId").val();
                if (answer) {
                    LoadPopUp();
                    isReject = true;
                }
                return false;
            });
            $("#btnDeferred").click(function () {
                <%--//CheckAdminPart();

                var MeetingDate = $("#<%=lblMeetingDate.ClientID %>").text();
                var MeetingDecision = $("#<%=lblMeetingDecision.ClientID %>").text();
                var MeetingDateEC = $("#<%=lblMeetingDateEC.ClientID %>").text();
                var MeetingDecisionEC = $("#<%=lblMeetingDecisionEC.ClientID %>").text();

                if (isAdmin) {
                    CheckIntroducer();
                    if (hasIntro == false) {
                        return false;
                    }
                    if (MeetingDate == "") {
                        toastr.warning("Please provide information of admin part.");
                        $("#ContentPlaceHolder1_txtRemarks").focus();
                        return false;
                    }
                }--%>
                ValidateAdmin();
                if (isAdmin) {
                    if (isValidAdminInput == false) {
                        toastr.warning("Please provide admin input.");
                        return false;
                    }
                }
                var answer = confirm("Are you sure to Defer?");
                var memberId = $("#ContentPlaceHolder1_hfMemberId").val();
                var introId = $("#ContentPlaceHolder1_hfIntroducerId").val();
                if (answer) {
                    LoadPopUp();
                    isDefer = true;
                }
                return false;
            });
            $("#btnOk").click(function () {

                var remarks = $("#ContentPlaceHolder1_txtRemarks").val();
                var memberId = $("#ContentPlaceHolder1_hfMemberId").val();
                var introId = $("#ContentPlaceHolder1_hfIntroducerId").val();

                var MeetingDate = $("#<%=lblMeetingDate.ClientID %>").text();
                var MeetingDecision = $("#<%=lblMeetingDecision.ClientID %>").text();
                var MeetingDateEC = $("#<%=lblMeetingDateEC.ClientID %>").text();
                var MeetingDecisionEC = $("#<%=lblMeetingDecisionEC.ClientID %>").text();

                if (remarks == "") {
                    toastr.warning("Please provide a valid reason");
                    $("#ContentPlaceHolder1_txtRemarks").focus();
                    return false;
                }
                else {
                    $("#RejectionDiv").dialog("close");
                    CommonHelper.SpinnerOpen();
                    if (introId != "0") {
                        PageMethods.UpdateAndRejectMemByIntroducer(memberId, introId, remarks, isReject, isDefer, OnLoadUpdateAndRejectSucceed, OnSearchFailed);
                    }
                    else {
                        PageMethods.UpdateAndRejectMember(memberId, remarks, isReject, isDefer, MeetingDate, MeetingDecision, MeetingDateEC, MeetingDecisionEC, OnLoadUpdateAndRejectSucceed, OnSearchFailed);
                    }

                }
            });
            $("#btnClose").click(function () {
                $("#RejectionDiv").dialog("close");
                isReject = false;
                isDefer = false;
                $("#ContentPlaceHolder1_txtRemarks").val("");
                return false;
            });

        });
        function ReadyCall() {
            var usergGroup = $("#<%=hfGroupName.ClientID %>").val();
            var introducerHf = $("#<%=hfIntroMemNo.ClientID %>").val();

            var introId1 = $.trim(CommonHelper.GetParameterByName("Id1"));
            var introId2 = $.trim(CommonHelper.GetParameterByName("Id2"));
            debugger;
            if (usergGroup == "Membership") {
                $("#ContentPlaceHolder1_PopE").hide();
                $("#ContentPlaceHolder1_PopG").hide();
                $("#Poptab5").hide();
                $("#Poptab7").hide();

            }
            else if (usergGroup == "SuperAdmin") {
                isAdmin = true;
            }

            if (introId1 != "") {
                GridPagingIntroducer(1, 1, introId1);
                $("#SearchOutputInroducer").show("slow");
                $("#SearchInput").hide();
                isIntorducer = true;
                //$("#SearchOutput").hide();
            }
            else if (introId2 != "") {
                GridPagingIntroducer(1, 1, introId2);
                $("#SearchOutputInroducer").show("slow");
                $("#SearchInput").hide();
                isIntorducer = true;
                //$("#SearchOutput").hide();
            }
            else if (introducerHf != "") {
                GridPagingIntroducer(1, 1, introducerHf);
                $("#SearchOutputInroducer").show("slow");
                $("#SearchInput").hide();
                isIntorducer = true;
            }
            else {
                $("#SearchInput").show();
                //$("#SearchOutput").hide();
            }
        }
        function ValidateAdmin() {
            var MeetingDate = $("#<%=lblMeetingDate.ClientID %>").text();
            var MeetingDecision = $("#<%=lblMeetingDecision.ClientID %>").text();
            var MeetingDateEC = $("#<%=lblMeetingDateEC.ClientID %>").text();
            var MeetingDecisionEC = $("#<%=lblMeetingDecisionEC.ClientID %>").text();

            if (isAdmin == true) {
                CheckIntroducer();
                if (hasIntro == false) {
                    toastr.warning("This Member hasn't reviewed by introducer.");
                    return false;
                }
                else if (MeetingDate == "") {
                    $("#ContentPlaceHolder1_txtRemarks").focus();
                    return false;
                }
                else {
                    isValidAdminInput = true;
                    return true;
                }
            }
            else {
                return false;
            }
        }
        function DisableAdminInput() {
            $("#<%=txtMeetingDate.ClientID %>").prop("disabled", true);
            $("#<%=txtMeetingDecision.ClientID %>").prop("disabled", true);
            $("#<%=txtMeetingDateEC.ClientID %>").prop("disabled", true);
            $("#<%=txtMeetingDecisionEC.ClientID %>").prop("disabled", true);

            $("#<%=txtMeetingDate.ClientID %>").val("");
            $("#<%=txtMeetingDecision.ClientID %>").val("");
            $("#<%=txtMeetingDateEC.ClientID %>").val("");
            $("#<%=txtMeetingDecisionEC.ClientID %>").val("");
        }
        function EnableAdminInput() {
            $("#<%=txtMeetingDate.ClientID %>").prop("disabled", false);
            $("#<%=txtMeetingDecision.ClientID %>").prop("disabled", false);
            $("#<%=txtMeetingDateEC.ClientID %>").prop("disabled", false);
            $("#<%=txtMeetingDecisionEC.ClientID %>").prop("disabled", false);
        }
        function CheckAdminInput() {
            var MeetingDate = $("#<%=lblMeetingDate.ClientID %>").text();
            var MeetingDecision = $("#<%=lblMeetingDecision.ClientID %>").text();
            var MeetingDateEC = $("#<%=lblMeetingDateEC.ClientID %>").text();
            var MeetingDecisionEC = $("#<%=lblMeetingDecisionEC.ClientID %>").text();

            if (MeetingDate == "") {

            }
            else if (MeetingDecision == "") {

            }
            else if (MeetingDateEC == "") {

            }
            else if (MeetingDecisionEC == "") {

            }
        }
        function CheckAdminPart() {
            var meetingDate = $("#<%=txtMeetingDate.ClientID %>").val();
            var meetingDecision = $("#<%=txtMeetingDecision.ClientID %>").val();
            var meetingDateEC = $("#<%=txtMeetingDateEC.ClientID %>").val();
            var meetingDecisionEC = $("#<%=txtMeetingDecisionEC.ClientID %>").val();

            if (meetingDate == "") {
                toastr.warning("Please enter Meeting date");
                $("#ContentPlaceHolder1_txtMeetingDate").focus();
                return false;
            }
            else if (meetingDecision == "") {
                toastr.warning("Please enter Decision of the meeting/interview");
                $("#ContentPlaceHolder1_txtMeetingDecision").focus();
                return false;
            }
            else if (meetingDateEC == "") {
                toastr.warning("Please enter Meeting/interview of the EC date");
                $("#ContentPlaceHolder1_txtMeetingDateEC").focus();
                return false;
            }
            else if (meetingDecisionEC == "") {
                toastr.warning("Please enter Meeting/interview of the EC");
                $("#ContentPlaceHolder1_txtMeetingDecisionEC").focus();
                return false;
            }

            LoadAdminPart(meetingDate, meetingDecision, meetingDateEC, meetingDecisionEC);
        }
        function LoadAdminPart(meetingDate, meetingDecision, meetingDateEC, meetingDecisionEC) {
            //meetingDate = $("#<%=txtMeetingDate.ClientID %>").val();
            debugger;
            if (meetingDate != "") {
                meetingDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(meetingDate, innBoarDateFormat);
            }
            meetingDecision = $("#<%=txtMeetingDecision.ClientID %>").val();
            //meetingDateEC = $("#<%=txtMeetingDateEC.ClientID %>").val();
            if (meetingDateEC != "") {
                meetingDateEC = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(meetingDateEC, innBoarDateFormat);
            }
            meetingDecisionEC = $("#<%=txtMeetingDecisionEC.ClientID %>").val();
            $("#adminOutput").show();
            $("#<%=lblMeetingDate.ClientID %>").text(meetingDate);
            $("#<%=lblMeetingDecision.ClientID %>").text(meetingDecision);
            $("#<%=lblMeetingDateEC.ClientID %>").text(meetingDateEC);
            $("#<%=lblMeetingDecisionEC.ClientID %>").text(meetingDecisionEC);
            DisableAdminInput();
            return false;
        }
        function CheckIntroducer() {
            var remarks1 = $("#<%=lblIntroRemarks1.ClientID %>").text();
            var remarks2 = $("#<%=lblIntroRemarks2.ClientID %>").text();
            var feedBack1 = $("#<%=lblFedback1.ClientID %>").text();
            var feedBack2 = $("#<%=lblFedback2.ClientID %>").text();

            if ((remarks1 == "") || (feedBack1 == "")) {
                hasIntro = false;
                toastr.warning("1st Introducer hasn't review this applicant. Please contact with him.");
                return false;
            }
            else if ((remarks2 == "") || (feedBack2 == "")) {
                hasIntro = false;
                toastr.warning("2nd Introducer hasn't review this applicant. Please contact with him.");
                return false;
            }
            else {
                hasIntro = true;
            }

        }
        function LoadPopUp() {
            $("#RejectionDiv").dialog({
                width: 500,
                height: 200,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                title: "Reson For rejection",
                show: 'slide',
                open: function (event, ui) {
                    $('#RejectionDiv').css('overflow', 'hidden');
                    $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                }
            });
        }
        function OnLoadIntroUpdateAndAcceptSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            CommonHelper.SpinnerClose();
            $("#TouchKeypad").dialog("close");
            //setInterval(function () { }, 4000);
            //location.reload(true);
            $("#form1")[0].reset();
            ReadyCall();
            return false;
        }
        function OnLoadUpdateAndAcceptSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            CommonHelper.SpinnerClose();
            $("#TouchKeypad").dialog("close");

            return false;
        }
        function OnLoadUpdateAndRejectSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            CommonHelper.SpinnerClose();
            $("#TouchKeypad").dialog("close");
            isReject = false;
            isDefer = false;
            $("#form1")[0].reset();
            ReadyCall();
            //setInterval(function () { }, 5000);
            //location.reload(true);
            return false;
        }
        function GridPagingIntroducer(pageNumber, IsCurrentOrPreviousPage, IntroducerId) {
            //MembersGridIntroducer
            var gridRecordsCount = $("#MembersGridIntroducer tbody tr").length;
            PageMethods.GetMemberInfoByIntroducer(IntroducerId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchByIntroducerSucceed, OnSearchByIntroducerFailed);
            return false;
        }
        function OnSearchByIntroducerSucceed(result) {
            $("#MembersGridIntroducer tbody tr").remove();
            $("#GridPagingContainerIntroducer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Request Pending</td> </tr>";
                $("#MembersGridIntroducer tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, detailLink = "";

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#MembersGridIntroducer tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.TypeName + "</td>";
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.FullName + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + gridObject.Country + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + gridObject.MobileNumber + "</td>";
                tr += "<td align='left' style=\"width:15%;\">" + gridObject.Occupation + "</td>";
                tr += "<td align='left' style=\"width:15%;\">" + gridObject.PersonalEmail + "</td>";
                tr += "<td align='center' style=\"width:10%; cursor:pointer;\"><img src='../Images/detailsInfo.png' onClick= \"javascript:return PerformActionIntro('" + gridObject.MemberId + "','" + gridObject.Introducer_1_id + "','" + gridObject.Introducer_2_id + "' )\" alt='Details' border='0' /> </td>";

                tr += "</tr>";

                $("#MembersGridIntroducer tbody").append(tr);
                tr = "";
            });
            $("#GridPagingContainerIntroducer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainerIntroducer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainerIntroducer ul").append(result.GridPageLinks.NextButton);

            return false;
        }
        function OnSearchByIntroducerFailed(error) {

        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#MembersGrid tbody tr").length;
            var typeId = $("#<%=ddlMemberType.ClientID %>").val();
            var name = $("#<%=txtMemberName.ClientID %>").val();
            var mobile = $("#<%=txtMobileNumber.ClientID %>").val();
            var email = $("#<%=txtEmail.ClientID %>").val();

            PageMethods.SearchNLoadMemberInformation(typeId, name, mobile, email, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearechSucceed, OnSearchFailed);
            return false;
        }
        function OnSearechSucceed(result) {
            var memberList = new Array();
            memberList = result;

            $("#MembersGrid tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"9\" >No Data Found</td> </tr>";
                $("#MembersGrid tbody ").append(emptyTr);
                return false;
            }
            var tr = "", totalRow = 0, detailLink = "";

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#MembersGrid tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style=\"width:15%;\">" + gridObject.TypeName + "</td>";
                tr += "<td align='left' style=\"width:15%; \">" + gridObject.FullName + "</td>";
                tr += "<td align='left' style=\"width:10%; \">" + gridObject.Country + "</td>";
                tr += "<td align='left' style=\"width:10%; \">" + gridObject.MobileNumber + "</td>";

                tr += "<td align='left' style=\"width:10%; \">" + gridObject.Occupation + "</td>";
                tr += "<td align='left' style=\"width:10%; \">" + gridObject.PersonalEmail + "</td>";
                tr += "<td align='left' style=\"width:10%; \">" + gridObject.Introducer_1_Name + "</td>";
                tr += "<td align='left' style=\"width:10%; \">" + gridObject.Introducer_2_Name + "</td>";
                tr += "<td align='left' style=\"display:none; \">" + gridObject.MemberId + "</td>";
                tr += "<td align='center' style=\"width:10%; cursor:pointer;\"><img src='../Images/detailsInfo.png' onClick= \"javascript:return PerformAction('" + gridObject.MemberId + "')\" alt='Details' border='0' />&nbsp <img src='../../Images/ReportDocument.png' onClick= \"javascript:return ShowReport('" + gridObject.MemberId + "')\" alt='Report' border='0' /></td>";

                tr += "</tr>";

                $("#MembersGrid tbody").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            $("#<%=ddlMemberType.ClientID %>").val("0");
            $("#<%=txtMemberName.ClientID %>").val("");
            $("#<%=txtMobileNumber.ClientID %>").val("");
            $("#<%=txtEmail.ClientID %>").val("");

            return false;
        }
        function OnSearchFailed(error) {

        }
        function PerformActionIntro(memberId, introId1, introId2) {
            ClearAll();
            //var valid1 = 0;
            //var valid2 = 0;
            //debugger;
            if (introId1 != "0") {
                $("#ContentPlaceHolder1_hfIntroducerId").val(introId1);
            }
            else if (introId2 != "0") {
                $("#ContentPlaceHolder1_hfIntroducerId").val(introId2);
            }
            $("#ContentPlaceHolder1_hfMemberId").val(memberId);
            PageMethods.GetOnlineMemInfoByMemberIdNIntroId(memberId, introId1, introId2, OnLoadMemBasicSucceed, OnLoadFailed);
        }
        function PerformAction(memberId) {
            ClearAll();
            $("#ContentPlaceHolder1_hfMemberId").val(memberId);
            PageMethods.GetOnlineMemInfoById(memberId, OnLoadMemBasicSucceed, OnLoadFailed);
            return false;
        }
        $(function () {
            $("#PopMyTabs").tabs();
        });
        function OnLoadMemBasicSucceed(result) {
            if ($.trim(result.BirthDate) != "") {
                $("#<%=lblDOB.ClientID %>").text(GetStringFromDateTime(result.BirthDate));
            }
            $("#<%=lblFullName.ClientID %>").text(result.FullName);
            $("#<%=lblNameBangla.ClientID %>").text(result.NameBangla);
            $("#<%=lblMembershipNumber.ClientID %>").text(result.MembershipNumber);
            $("#<%=lblMemType.ClientID %>").text(result.TypeName);
            $("#<%=lblFatherName.ClientID %>").text(result.FatherName);
            $("#<%=lblMotherName.ClientID %>").text(result.MotherName);
            $("#<%=lblGender.ClientID %>").text(result.MemberGenderSt);
            $("#<%=lblMailingAdd.ClientID %>").text(result.MailAddress);
            $("#<%=lblMemberAdd.ClientID %>").text(result.MemberAddress);
            $("#<%=lblMemEmail.ClientID %>").text(result.PersonalEmail);
            $("#<%=lblMobile.ClientID %>").text(result.MobileNumber);
            $("#<%=lblMarrital.ClientID %>").text(result.MaritalSt);
            $("#<%=lblBlood.ClientID %>").text(result.BloodGroupName);
            $("#<%=lblCountry.ClientID %>").text(result.Country);
            $("#<%=lblOrganization.ClientID %>").text(result.Organization);
            $("#<%=lblOccupation.ClientID %>").text(result.Occupation);
            $("#<%=lblDesignation.ClientID %>").text(result.Designation);
            $("#<%=lblReligion.ClientID %>").text(result.Religion);
            $("#<%=lblProfession.ClientID %>").text(result.Profession);
            $("#<%=lblHobbies.ClientID %>").text(result.Hobbies);
            $("#<%=lblAwards.ClientID %>").text(result.Awards);
            //new add
            $("#<%=lblNID.ClientID %>").text(result.NationalID);
            $("#<%=lblBirthPlace.ClientID %>").text(result.BirthPlace);
            $("#<%=lblHeight.ClientID %>").text(result.Height);
            $("#<%=lblWeight.ClientID %>").text(result.Weight);
            //nominee
            $("#<%=lblNomineeName.ClientID %>").text(result.NomineeName);
            $("#<%=lblNomFather.ClientID %>").text(result.NomineeFather);
            $("#<%=lblNomMother.ClientID %>").text(result.NomineeMother);
            if (result.NomineeDOB != null) {
                $("#<%=lblNomDOB.ClientID %>").text(CommonHelper.DateFromDateTimeToDisplay(result.NomineeDOB, innBoarDateFormat));
            }

            $("#<%=lblNomRelation.ClientID %>").text(result.NomineeRelation);
           <%-- $("#<%=lblIntro1.ClientID %>").text(result.Introducer_1_name);
            $("#<%=lblIntro2.ClientID %>").text(result.Introducer_2_name);--%>

            $("#personalImg").attr("src", "../" + result.PathPersonalImg);
            $("#NIdImg").attr("src", "../" + result.PathNIdImage);
            //introducers
            $("#<%=lblIntroName1.ClientID %>").text(result.Introducer_1_Name);
            $("#<%=lblIntroType1.ClientID %>").text(result.IntroducerMemType1);
            $("#<%=lblIntroMemNo1.ClientID %>").text(result.IntroducerMemNo1);
            $("#<%=lblIntroCell1.ClientID %>").text(result.IntroducerMobileNo1);
            $("#<%=lblIntroMail1.ClientID %>").text(result.IntroducerEmail1);

            $("#<%=lblIntroName2.ClientID %>").text(result.Introducer_2_Name);
            $("#<%=lblIntroType2.ClientID %>").text(result.IntroducerMemType2);
            $("#<%=lblIntroMemNo2.ClientID %>").text(result.IntroducerMemNo2);
            $("#<%=lblIntroCell2.ClientID %>").text(result.IntroducerMobileNo2);
            $("#<%=lblIntroMail2.ClientID %>").text(result.IntroducerEmail2);

            //adminPart
            $("#adminOutput").show();
            $("#<%=lblMeetingDate.ClientID %>").text(result.MeetingDate);
            $("#<%=lblMeetingDecision.ClientID %>").text(result.MeetingDecision);
            $("#<%=lblMeetingDateEC.ClientID %>").text(result.MeetingDateEC);
            $("#<%=lblMeetingDecisionEC.ClientID %>").text(result.MeetingDecisionEC);


            if (result.IsAccepted1 == true) {
                $("#<%=lblFedback1.ClientID %>").text("Approved");
                $("#<%=lblIntroRemarks1.ClientID %>").text(result.Remarks1);
            }
            else if (result.IsRejected1 == true) {
                $("#<%=lblFedback1.ClientID %>").text("Not Approved");
                $("#<%=lblIntroRemarks1.ClientID %>").text(result.Remarks1);
            }
            else if (result.IsDeferred1 == true) {
                $("#<%=lblFedback1.ClientID %>").text("Deferred");
                $("#<%=lblIntroRemarks1.ClientID %>").text(result.Remarks1);
            }

            if (result.IsAccepted2 == true) {
                $("#<%=lblFedback2.ClientID %>").text("Approved");
                $("#<%=lblIntroRemarks2.ClientID %>").text(result.Remarks2);
            }
            else if (result.IsRejected2 == true) {
                $("#<%=lblFedback2.ClientID %>").text("Not Approved");
                $("#<%=lblIntroRemarks2.ClientID %>").text(result.Remarks2);
            }
            else if (result.IsDeferred2 == true) {
                $("#<%=lblFedback2.ClientID %>").text("Deferred");
                $("#<%=lblIntroRemarks2.ClientID %>").text(result.Remarks2);
            }
            
            if (result.MeetingDate != null) {
                $("#<%=lblMeetingDate.ClientID %>").text(CommonHelper.DateFromDateTimeToDisplay(result.MeetingDate, innBoarDateFormat));
            }
            if (result.MeetingDateEC != null) {
                $("#<%=lblMeetingDateEC.ClientID %>").text(CommonHelper.DateFromDateTimeToDisplay(result.MeetingDateEC, innBoarDateFormat));
            }
            
            $("#<%=lblMeetingDecision.ClientID %>").text(result.MeetingDecision);
            $("#<%=lblMeetingDecisionEC.ClientID %>").text(result.MeetingDecisionEC);

            if (result.IsAccepted == true) {
                $(".approvalDiv").hide();
                $("#adminBtnDiv").hide();
                $("#editImg").hide();
                
                DisableAdminInput();
            }
            //else if (result.IsAccepted1 == true) {
            //    $(".approvalDiv").hide();
            //}
            //else if (result.IsAccepted2 == true) {
            //    $(".approvalDiv").hide();
            //}
            else {
                $(".approvalDiv").show();
                $("#adminBtnDiv").show();
                EnableAdminInput();
            }
            $("#PopMyTabs").tabs({ active: 0 });

            $("#TouchKeypad").dialog({
                autoOpen: true,
                modal: true,
                width: 1000,
                height: 650,
                closeOnEscape: true,
                resizable: false,
                title: "Online Membership Approval",
                show: 'slide'
            });
            var memberId = $("#ContentPlaceHolder1_hfMemberId").val();
            PageMethods.GetOnlineFamilyInfoById(memberId, OnLoadFamilySucceed, OnLoadFailed);
        }
        function OnLoadFamilySucceed(result) {
            $("#MemFamilyGrid tbody tr").remove();
            var tr = "", totalRow = 0, detailLink = "";
            if (result.length <= 0) {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#MemFamilyGrid tbody ").append(emptyTr);
                //return false;
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    totalRow = $("#MemFamilyGrid tbody tr").length;

                    if ((totalRow % 2) == 0) {
                        tr += "<tr style=\"background-color:#E3EAEB;\">";
                    }
                    else {
                        tr += "<tr style=\"background-color:#FFFFFF;\">";
                    }
                    tr += "<td align='left' style=\"width:15%;\">" + result[i].MemberName + "</td>";
                    tr += "<td align='left' style=\"width:15%;\">" + result[i].Relationship + "</td>";
                    tr += "<td align='left' style=\"width:15%;\">" + CommonHelper.DateFromDateTimeToDisplay(result[i].MemberDOB, innBoarDateFormat) + "</td>";
                    tr += "<td align='left' style=\"width:15%;\">" + result[i].Occupation + "</td>";
                    tr += "<td align='left' style=\"width:15%;\">" + result[i].BloodGroup + "</td>";
                    tr += "<td align='left' style=\"width:15%;\">" + CommonHelper.DateFromDateTimeToDisplay(result[i].FamMemMarriageDate, innBoarDateFormat) + "</td>";

                    tr += "</tr>";

                    $("#MemFamilyGrid tbody").append(tr);
                    tr = "";
                }
            }
            var memberId = $("#ContentPlaceHolder1_hfMemberId").val();
            PageMethods.GetOnlineEducationInfoById(memberId, OnLoadEducationSucceed, OnLoadFailed);

        }
        function OnLoadEducationSucceed(result) {
            $("#MemEducationGrid tbody tr").remove();
            var tr = "", totalRow = 0, detailLink = "";
            if (result.length <= 0) {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"3\" >No Data Found</td> </tr>";
                $("#MemEducationGrid tbody ").append(emptyTr);
                //return false;
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    totalRow = $("#MemEducationGrid tbody tr").length;

                    if ((totalRow % 2) == 0) {
                        tr += "<tr style=\"background-color:#E3EAEB;\">";
                    }
                    else {
                        tr += "<tr style=\"background-color:#FFFFFF;\">";
                    }
                    tr += "<td align='left' style=\"width:25%;\">" + result[i].Degree + "</td>";
                    tr += "<td align='left' style=\"width:25%;\">" + result[i].Institution + "</td>";
                    tr += "<td align='left' style=\"width:25%;\">" + result[i].PassingYear + "</td>";

                    tr += "</tr>";

                    $("#MemEducationGrid tbody").append(tr);
                    tr = "";
                }
            }
            return false;
        }
        function OnLoadFailed() {

        }
        function ClearAll() {
            $("#ContentPlaceHolder1_hfMemberId").val("0");
            $("#ContentPlaceHolder1_hfIntroducerId").val("0");
            $("#<%=lblDOB.ClientID %>").text("");
            $("#<%=lblFullName.ClientID %>").text("");
            $("#<%=lblMemType.ClientID %>").text("");
            $("#<%=lblNameBangla.ClientID %>").text("");
            $("#<%=lblMembershipNumber.ClientID %>").text("");
            $("#<%=lblFatherName.ClientID %>").text("");
            $("#<%=lblMotherName.ClientID %>").text("");
            $("#<%=lblGender.ClientID %>").text("");
            $("#<%=lblMailingAdd.ClientID %>").text("");
            $("#<%=lblMemberAdd.ClientID %>").text("");
            $("#<%=lblMemEmail.ClientID %>").text("");
            $("#<%=lblMobile.ClientID %>").text("");
            $("#<%=lblMarrital.ClientID %>").text("");
            $("#<%=lblBlood.ClientID %>").text("");
            $("#<%=lblCountry.ClientID %>").text("");
            $("#<%=lblOrganization.ClientID %>").text("");
            $("#<%=lblOccupation.ClientID %>").text("");
            $("#<%=lblDesignation.ClientID %>").text("");
            $("#<%=lblReligion.ClientID %>").text("");
            $("#<%=lblProfession.ClientID %>").text("");
            $("#<%=lblHobbies.ClientID %>").text("");
            $("#<%=lblAwards.ClientID %>").text("");
           <%-- $("#<%=lblIntro1.ClientID %>").text("");
            $("#<%=lblIntro2.ClientID %>").text("");--%>
            $("#ContentPlaceHolder1_txtRemarks").val("");

            //new add
            $("#<%=lblNID.ClientID %>").text("");
            $("#<%=lblBirthPlace.ClientID %>").text("");
            $("#<%=lblHeight.ClientID %>").text("");
            $("#<%=lblWeight.ClientID %>").text("");
            //nominee
            $("#<%=lblNomineeName.ClientID %>").text("");
            $("#<%=lblNomFather.ClientID %>").text("");
            $("#<%=lblNomMother.ClientID %>").text("");
            $("#<%=lblNomDOB.ClientID %>").text("");
            $("#<%=lblNomRelation.ClientID %>").text("");


            EnableAdminInput();
            $("#<%=lblMeetingDate.ClientID %>").text("");
            $("#<%=lblMeetingDecision.ClientID %>").text("");
            $("#<%=lblMeetingDateEC.ClientID %>").text("");
            $("#<%=lblMeetingDecisionEC.ClientID %>").text("");

            //introducers
            $("#<%=lblIntroName1.ClientID %>").text("");
            $("#<%=lblIntroType1.ClientID %>").text("");
            $("#<%=lblIntroMemNo1.ClientID %>").text("");
            $("#<%=lblIntroCell1.ClientID %>").text("");
            $("#<%=lblIntroMail1.ClientID %>").text("");

            $("#<%=lblIntroName2.ClientID %>").text("");
            $("#<%=lblIntroType2.ClientID %>").text("");
            $("#<%=lblIntroMemNo2.ClientID %>").text("");
            $("#<%=lblIntroCell2.ClientID %>").text("");
            $("#<%=lblIntroMail2.ClientID %>").text("");
        }

        function ShowReport(memberId) {
            var iframeid = 'printDoc';
            var url = "Reports/ReportOnlineMembership.aspx?Id=" + memberId;
            //window.location = url;
            document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 850,
                height: 820,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Membership Form",
                show: 'slide'
            });
        }
    </script>
    <div class="panel panel-default" id="SearchInput">
        <asp:HiddenField ID="hfMemberId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfIntroducerId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfIntroMemNo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfGroupName" runat="server"></asp:HiddenField>

        <div id="displayBill" style="display: none;">
            <iframe id="printDoc" name="printDoc" width="850" height="820" style="overflow: hidden;"></iframe>
            <div id="bottomPrint">
            </div>
        </div>
        <div class="panel-heading">
            Membership Public Admin
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">

                    <div class="col-md-2">
                        <asp:Label ID="lblMemT" runat="server" class="control-label" Text="Member Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlMemberType" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="lb1" runat="server" class="control-label" Text="Member Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtMemberName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>

                </div>

                <div class="form-group">

                    <div class="col-md-2">
                        <asp:Label ID="lblMob" runat="server" class="control-label" Text="Mobile Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="lblE" runat="server" class="control-label" Text="Email"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" value="Search" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchOutput" class="panel panel-default" style="display: none">
        <div class="panel-heading">
            Search Information
        </div>
        <div id="" class="panel-body">
            <table id="MembersGrid" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <th style="width: 15%; text-align: center;">Member Type
                        </th>
                        <th style="width: 15%; text-align: center;">Name
                        </th>
                        <th style="width: 10%; text-align: center;">Country
                        </th>
                        <th style="width: 10%; text-align: center;">Mobile Number
                        </th>
                        <th style="width: 10%; text-align: center;">Occupation
                        </th>
                        <th style="width: 10%; text-align: center;">Email
                        </th>
                        <th style="width: 10%; text-align: center;">Introducer1
                        </th>
                        <th style="width: 10%; text-align: center;">Introducer2
                        </th>
                        <th style="width: 10%; text-align: center;">Action
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
    <div id="SearchOutputInroducer" class="panel panel-default" style="display: none">
        <div class="panel-heading">
            Member Information
        </div>
        <div id="" class="panel-body">
            <table id="MembersGridIntroducer" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <th style="width: 20%; text-align: center;">Member Type
                        </th>
                        <th style="width: 20%; text-align: center;">Name
                        </th>
                        <th style="width: 10%; text-align: center;">Country
                        </th>
                        <th style="width: 10%; text-align: center;">Mobile Number
                        </th>
                        <th style="width: 15%; text-align: center;">Occupation
                        </th>
                        <th style="width: 15%; text-align: center;">Email
                        </th>
                        <%--<th style="width: 10%; text-align: center;">Introducer1
                        </th>
                        <th style="width: 10%; text-align: center;">Introducer2
                        </th>--%>
                        <th style="width: 10%; text-align: center;">Action
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainerIntroducer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div id="TouchKeypad" style="display: none; overflow-y: scroll">
        <div id="PopMyTabs">
            <ul id="PoptabPage" class="ui-style">
                <li id="PopA" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab1">Basic Information</a></li>
                <li id="PopB" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab2">Family Information</a></li>
                <li id="PopC" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab3">Educational Information</a></li>
                <li id="PopD" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab4">Nominee Information</a></li>
                <li id="PopE" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab5">Introducer's Information</a></li>
                <li id="PopF" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab6">Photo</a></li>
                <li id="PopG" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab7">Admin Part</a></li>
            </ul>
            <div id="Poptab1">
                <div id="BasicInfo" class="block" style="font-weight: bold">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Member Basic Information
                    </a>
                    <div class="MemBodyContainer">
                        <table class="table table-striped table-bordered table-condensed table-hover">
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="lbl" runat="server" Text="Name"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblFullName" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label21" runat="server" Text="Name(in Bangla)"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblNameBangla" runat="server" Text=""></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="Label1" runat="server" Text="Member Type"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblMemType" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label23" runat="server" Text="Membership No."></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblMembershipNumber" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="Label2" runat="server" Text="Father's Name"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblFatherName" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label4" runat="server" Text="Mother's name"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblMotherName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="Label3" runat="server" Text="Date Of Birth"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblDOB" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label6" runat="server" Text="Gender"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblGender" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="Label12" runat="server" Text="Marital Status"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblMarrital" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label14" runat="server" Text="Religion"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblReligion" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="Label5" runat="server" Text="Present Address"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblMailingAdd" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label8" runat="server" Text="Permanent Address"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblMemberAdd" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="Label7" runat="server" Text="Country"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblCountry" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label10" runat="server" Text="Blood Group"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblBlood" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="Label9" runat="server" Text="Mobile Number"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblMobile" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label13" runat="server" Text="Email"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblMemEmail" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="Label17" runat="server" Text="NID"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblNID" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label20" runat="server" Text="Birth Place"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblBirthPlace" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="Label43" runat="server" Text="Height"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblHeight" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label45" runat="server" Text="Weight"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblWeight" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="Label11" runat="server" Text="Profession"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblProfession" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label16" runat="server" Text="Occupation"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblOccupation" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="Label15" runat="server" Text="Designation"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblDesignation" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label18" runat="server" Text="Organization"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblOrganization" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td class="span2">
                                    <asp:Label ID="Label19" runat="server" Text="Hobbies & Special Inst."></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblHobbies" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="Label22" runat="server" Text="Honor & Award"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblAwards" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div id="Poptab2">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Family Information
                </a>
                <div id="MemFamilyInfo">
                    <table id="MemFamilyGrid" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 20%; text-align: center;">Name
                                </th>
                                <th style="width: 20%; text-align: center;">Relation
                                </th>
                                <th style="width: 15%; text-align: center;">Date of Birth
                                </th>
                                <th style="width: 15%; text-align: center;">Occupation
                                </th>
                                <th style="width: 15%; text-align: center;">Blood Group
                                </th>
                                <th style="width: 15%; text-align: center;">Marriage Date
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
            <div id="Poptab3">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Education Information
                </a>
                <div id="MemEducationInfo">
                    <table id="MemEducationGrid" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 25%; text-align: center;">Degree
                                </th>
                                <th style="width: 25%; text-align: center;">Institution
                                </th>
                                <th style="width: 25%; text-align: center;">Passing Year
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
            <div id="Poptab4">
                <div id="NomineeInfo" class="block" style="font-weight: bold">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Member's Nominee Information
                    </a>
                    <div class="MemBodyContainer">
                        <table class="table table-striped table-bordered table-condensed table-hover">
                            <tr>
                                <td style="width: 40%">
                                    <asp:Label ID="Label24" runat="server" Text="Nominee Name"></asp:Label>
                                </td>
                                <td style="width: 60%">
                                    <asp:Label ID="lblNomineeName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%">
                                    <asp:Label ID="Label25" runat="server" Text="Father's Name"></asp:Label>
                                </td>
                                <td style="width: 60%">
                                    <asp:Label ID="lblNomFather" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%">
                                    <asp:Label ID="Label26" runat="server" Text="Mother's Name"></asp:Label>
                                </td>
                                <td style="width: 60%">
                                    <asp:Label ID="lblNomMother" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%">
                                    <asp:Label ID="Label28" runat="server" Text="Date of Birth"></asp:Label>
                                </td>
                                <td style="width: 60%">
                                    <asp:Label ID="lblNomDOB" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%">
                                    <asp:Label ID="Label30" runat="server" Text="Relation"></asp:Label>
                                </td>
                                <td style="width: 60%">
                                    <asp:Label ID="lblNomRelation" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div id="Poptab5">
                <div id="IntroducerTab" class="block" style="font-weight: bold">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">1st Introducer's Feedback
                    </a>
                    <div class="MemBodyContainer">
                        <div class="row">
                            <div id="Intro1Div">
                                <table class="table table-striped table-bordered table-condensed table-hover">
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label27" runat="server" Text="Introducer Name"></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblIntroName1" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label29" runat="server" Text="Member type"></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblIntroType1" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label33" runat="server" Text="Membership No."></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblIntroMemNo1" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label35" runat="server" Text="Cell"></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblIntroCell1" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label37" runat="server" Text="Email"></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblIntroMail1" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label31" runat="server" Text="Feedback"></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblFedback1" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label39" runat="server" Text="Remarks"></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblIntroRemarks1" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="MemBodyContainer">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">2nd Introducer's Feedback
                        </a>
                        <div class="row">
                            <div id="Intro2Div">

                                <table class="table table-striped table-bordered table-condensed table-hover">
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label32" runat="server" Text="Introducer Name"></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblIntroName2" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label34" runat="server" Text="Member type"></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblIntroType2" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label36" runat="server" Text="Membership No."></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblIntroMemNo2" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label38" runat="server" Text="Cell"></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblIntroCell2" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label40" runat="server" Text="Email"></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblIntroMail2" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label41" runat="server" Text="Feedback"></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblFedback2" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span4">
                                            <asp:Label ID="Label42" runat="server" Text="Remarks"></asp:Label>
                                        </td>
                                        <td class="span8">
                                            <asp:Label ID="lblIntroRemarks2" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Poptab6">
                <div id="PhotoTab" class="block" style="font-weight: bold">
                    <div class="MemBodyContainer">
                        <div class="row">
                            <img id="personalImg" alt="Photo" style="width: 300px; height: 350px;" />
                        </div>
                        <div class="row">
                            <img id="NIdImg" alt="Photo" style="width: 600px; height: 350px;" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="Poptab7">
                <div id="AdminInputTab" class="block" style="font-weight: bold">

                    <fieldset>
                        <legend>Admin Part </legend>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label50" runat="server" class="control-label required-field"
                                            Text="Meeting/interview of the Membership development Committee (Date)"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtMeetingDate" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label44" runat="server" class="control-label required-field"
                                            Text="Decision of the meeting/interview"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtMeetingDecision" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                                &nbsp;
                                &nbsp;
                                <div class="form-group">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label46" runat="server" class="control-label required-field"
                                            Text="Meeting/interview of the EC (Date)"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtMeetingDateEC" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label47" runat="server" class="control-label required-field"
                                            Text="Decision of the meeting/interview:"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtMeetingDecisionEC" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group" id="adminBtnDiv">
                                    <div class="col-md-offset-6 col-md-2">
                                        <input type="button" id="btnAdminOk" class="TransactionalButton btn btn-primary btn-sm" value="Okay" />
                                    </div>

                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <div class="MemBodyContainer" id="adminOutput" style="display: none">
                        <table class="table table-striped table-bordered table-condensed table-hover" id="tblAdminMeeting">
                            <caption>Meeting Summary</caption>
                            <thead>
                                <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                    <th style="width: 15%; text-align: center;">Meeting Date
                                    </th>
                                    <th style="width: 30%; text-align: center;">Meeting Decision
                                    </th>
                                    <th style="width: 15%; text-align: center;">Meeting Date EC
                                    </th>
                                    <th style="width: 30%; text-align: center;">Meeting Decision EC
                                    </th>
                                    <th style="width: 10%; text-align: center;">Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label ID="lblMeetingDate" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 30%">
                                        <asp:Label ID="lblMeetingDecision" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:Label ID="lblMeetingDateEC" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 30%">
                                        <asp:Label ID="lblMeetingDecisionEC" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 10%">
                                        <img src="../Images/edit.png" id="editImg" style="cursor: pointer; position: center" />
                                    </td>
                                </tr>
                            </tbody>


                        </table>
                    </div>

                </div>
            </div>
        </div>
        &nbsp;
        <div class="approvalDiv">
            <div class="form-group">
                <input type="button" id="btnAccept" class="TransactionalButton btn btn-primary btn-sm" value="Accepted" />
                &nbsp;
            <input type="button" id="btnReject" class="TransactionalButton btn btn-primary btn-sm" value="Rejected" />
                &nbsp;
            <input type="button" id="btnDeferred" class="TransactionalButton btn btn-primary btn-sm" value="Deferred" />
            </div>

        </div>
        <div id="RejectionDiv" class="panel panel-default" style="display: none;">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label Text="Reason" runat="server" class="control-label required-field"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtRemarks" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                &nbsp;
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <input type="button" id="btnOk" class="TransactionalButton btn btn-primary btn-sm" value="Okay" />
                        &nbsp;
                        <input type="button" id="btnClose" class="TransactionalButton btn btn-primary btn-sm" value="Close" />
                    </div>

                </div>
            </div>
        </div>

    </div>
</asp:Content>
