<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="OnlineMembership.aspx.cs" Inherits="HotelManagement.Presentation.Website.Membership.OnlineMembership" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var PersonalImgPath = "";
        var NIdImgPath = "";
        $(document).ready(function () {
            CommonHelper.ApplyIntigerValidation();

            $('#ContentPlaceHolder1_txtMemDOB').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $('#ContentPlaceHolder1_txtDOBfamMem').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $('#ContentPlaceHolder1_txtNomineeDOB').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $('#ContentPlaceHolder1_txtMarriageDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_ddlCountry").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_txtMemberId_1").change(function () {
                var memberNo = $("#ContentPlaceHolder1_txtMemberId_1").val();
                var temp = $("#ContentPlaceHolder1_txtMemberId_2").val();
                if (memberNo == "") {
                    $("#ContentPlaceHolder1_txtIntroducerName_1").val("");
                    return false;
                }
                else if (temp == memberNo) {
                    $("#ContentPlaceHolder1_txtIntroducerName_1").val("");
                    toastr.warning("Duplicate introducer");
                    return false;
                }
                else {
                    PageMethods.GetOnlineMemberInfoByMemNumber(memberNo, OnMemberInfo1Succeed, OnMemberInfoFailed);
                }

            });
            $("#ContentPlaceHolder1_txtMemberId_2").change(function () {
                var memberNo = $("#ContentPlaceHolder1_txtMemberId_2").val();
                var temp = $("#ContentPlaceHolder1_txtMemberId_1").val();
                if (memberNo == "") {
                    $("#ContentPlaceHolder1_txtIntroducerName_2").val("");
                    return false;
                }
                else if (temp == memberNo) {
                    $("#ContentPlaceHolder1_txtIntroducerName_2").val("");
                    toastr.warning("Duplicate introducer");
                    return false;
                }
                else {
                    PageMethods.GetOnlineMemberInfoByMemNumber(memberNo, OnMemberInfo2Succeed, OnMemberInfoFailed);
                }

            });
            $("#ContentPlaceHolder1_ddlCountry").change(function () {
                //find(":selected").text();
                var countryId = $("#ContentPlaceHolder1_ddlCountry").val();

                PageMethods.GetNationality(countryId, OnCountrySucceeded, OnCountryFailed);
            });
            $("#ContentPlaceHolder1_ddlRelationship").change(function () {
                var relationship = $("#ContentPlaceHolder1_ddlRelationship").val();
                var relationshipText = $("#ContentPlaceHolder1_ddlRelationship option:selected").text();
                if (relationshipText == "Spouse") {
                    $("#MarriageDateDiv").show();
                }
                else {
                    $("#MarriageDateDiv").hide();
                }

            });

            //$("#ContentPlaceHolder1_btnNext_1").click(function () {
            //    btnNext_1Validation();
            //    //after completing validation check show/hide function
            //    $("#Occupation").show();
            //    $("#BasicInformation").hide();

            //});
            $("#btnNext_2").click(function () {
                $("#FamilyInfo").show();
                $("#Occupation").hide();
            });
            $("#btnNext_3").click(function () {
                $("#Educational").show();
                $("#FamilyInfo").hide();
            });
            $("#btnNext_4").click(function () {
                $("#Nominee").show();
                $("#Educational").hide();
            });
            $("#btnNext_5").click(function () {
                var nomineeName = $("#<%=txtNomineeName.ClientID %>").val();
                var nomFather = $("#<%=txtNomineeFather.ClientID %>").val();
                var nomMother = $("#<%=txtNomineeMother.ClientID %>").val();
                var nomDOB = $("#<%=txtNomineeDOB.ClientID %>").val();
                var nomRelation = $("#<%=ddlNomineeRelation.ClientID %>").val();

                if (nomineeName != "") {
                    if (nomFather == "") {
                        toastr.warning("Please enter father's name.");
                        $("#ContentPlaceHolder1_txtNomineeFather").focus();
                        return false;
                    }
                    else if (nomMother == "") {
                        toastr.warning("Please enter mother's name.");
                        $("#ContentPlaceHolder1_txtNomineeMother").focus();
                        return false;
                    }
                    else if (nomDOB == "") {
                        toastr.warning("Please enter Date of birth.");
                        $("#ContentPlaceHolder1_txtNomineeDOB").focus();
                        return false;
                    }
                    else if (nomRelation == "0") {
                        toastr.warning("Please enter mother's name.");
                        $("#ContentPlaceHolder1_ddlNomineeRelation").focus();
                        return false;
                    }
                }
                $("#Introducer").show();
                $("#Nominee").hide();
            });
            $("#btnAddFamMem").click(function () {
                var relationship = $("#ContentPlaceHolder1_ddlRelationship").val();
                var famMemName = $("#ContentPlaceHolder1_txtNameFamMem").val();
                //var famMemDOB = $("#ContentPlaceHolder1_txtDOBfamMem").val();
                var famMemBlood = $("#ContentPlaceHolder1_ddlBloodFamMem").val();


                if (relationship == "0") {
                    toastr.warning("Please select relationship type.");
                    $("#ContentPlaceHolder1_ddlRelationship").focus();
                    return false;
                }
                else if (famMemName == "") {
                    toastr.warning("Please enter family member's name.");
                    $("#ContentPlaceHolder1_txtNameFamMem").focus();
                    return false;
                }
                //else if (famMemDOB == "") {
                //    toastr.warning("Please enter Date of birth.");
                //    $("#ContentPlaceHolder1_txtDOBfamMem").focus();
                //    return false;
                //}
                else if (famMemBlood == "0") {
                    toastr.warning("Please select blood group.");
                    $("#ContentPlaceHolder1_ddlBloodFamMem").focus();
                    return false;
                }
                var famMemMarriageDate = "";

                famMemMarriageDate = $("#ContentPlaceHolder1_txtMarriageDate").val();
                var relationshipText = $("#ContentPlaceHolder1_ddlRelationship option:selected").text();

                LoadFamilyMember(relationshipText, famMemName, famMemBlood, famMemMarriageDate);

            });


            $("#btnAddInst").click(function () {
                var degree = $("#ContentPlaceHolder1_txtDegree").val();
                var institution = $("#ContentPlaceHolder1_txtInstitution").val();

                if (degree == "") {
                    toastr.warning("Please add name of degree.");
                    $("#ContentPlaceHolder1_txtDegree").focus();
                    return false;
                }
                else if (institution == "") {
                    toastr.warning("Please add institution name.");
                    $("#ContentPlaceHolder1_txtInstitution").focus();
                    return false;
                }
                LoadEducation(degree, institution);

            });
            $("#tblEducational").delegate("td > img.DeleteEducation", "click", function () {
                var answer = confirm("Do you want to delete this record?");
                if (answer) {
                    CommonHelper.ExactMatch();
                    var tr = $(this).parent().parent();
                    $(tr).remove();
                }
            });
            $("#familyMembers").delegate("td > img.DeleteFamMem", "click", function () {
                var answer = confirm("Do you want to delete this record?");
                if (answer) {
                    CommonHelper.ExactMatch();
                    var tr = $(this).parent().parent();
                    $(tr).remove();
                }
            });
            $("#btnPrev1").click(function () {
                $("#Occupation").hide();
                $("#BasicInformation").show();

            });
            $("#btnPrev2").click(function () {
                $("#FamilyInfo").hide();
                $("#Occupation").show();

            });
            $("#btnPrev3").click(function () {
                $("#Educational").hide();
                $("#FamilyInfo").show();

            });
            $("#btnPrev4").click(function () {

                $("#Nominee").hide();
                $("#Educational").show();

            });
            $("#btnPrev5").click(function () {
                $("#Introducer").hide();
                $("#Nominee").show();

            });

            $("#btnImageUpload").click(function () {
                UploadPersonalImg();
            });

            var _URL = window.URL || window.webkitURL;
            $("#PersonalImage").on('change', function () {

                var file, img;
                var maxwidth = 300;
                var maxheight = 350;
                var imgwidth = 0;
                var imgheight = 0;


                if ((file = this.files[0])) {
                    img = new Image();
                    img.onload = function () {
                        imgwidth = this.width;
                        imgheight = this.height;
                        if (imgwidth > maxwidth && imgheight > maxheight) {
                            toastr.warning("Image size must be " + maxwidth + "x" + maxheight);

                            $("#PersonalImage").val('');
                            return false;
                        }
                        else {
                            UploadPersonalImg(file);
                        }

                    };
                    img.onerror = function () {
                        alert("Not a valid file:" + file.type);
                        $("#PersonalImage").val('');
                        return false;
                    };
                    img.src = _URL.createObjectURL(file);
                }
            });

            $("#NIDimage").on('change', function () {

                var file, img;
                var maxwidth = 600;
                var maxheight = 350;
                var imgwidth = 0;
                var imgheight = 0;


                if ((file = this.files[0])) {
                    img = new Image();
                    img.onload = function () {
                        imgwidth = this.width;
                        imgheight = this.height;
                        if (imgwidth > maxwidth && imgheight > maxheight) {
                            toastr.warning("Image size must be " + maxwidth + "x" + maxheight);

                            $("#NIDimage").val('');
                            return false;
                        }
                        else {
                            UploadNIdImg(file);
                        }

                    };
                    img.onerror = function () {
                        alert("Not a valid file:" + file.type);
                        $("#NIDimage").val('');
                        return false;
                    };
                    img.src = _URL.createObjectURL(file);
                }
            });
        });
        //image upload 

        function UploadPersonalImg(file) {
            var formData = new FormData();
            formData.append('file', $('#PersonalImage')[0].files[0]);

            var progressbarLabel = $('#lblPersonalImgProgressbar');
            var progressbarDiv = $('#divProgressbar');
            progressbarLabel.text('Please Wait...');
            progressbarDiv.progressbar({
                value: false
            }).fadeIn(2000);
            $.ajax({
                type: 'post',
                url: '../FileUploader.ashx',
                data: formData,
                success: function (status) {
                    if (status != 'error') {
                        PersonalImgPath = "MediaUploader/" + status;
                        //$("#myUploadedImg").attr("src", my_path);
                        progressbarLabel.text('Uploaded Successfully');
                        progressbarDiv.fadeOut(4000);
                    }

                },
                processData: false,
                contentType: false,
                error: function (ex) {
                    toastr.warning("Personal image upload went wrong! " + ex);
                    return false;
                }
            });

        }
        function UploadNIdImg(file) {
            var formData = new FormData();
            formData.append('file', $('#NIDimage')[0].files[0]);

            var progressbarLabel = $('#lblPersonalImgProgressbarNID');
            var progressbarDiv = $('#divProgressbarNID');

            progressbarDiv.progressbar({
                value: false
            }).fadeIn(2000);

            $.ajax({
                type: 'post',
                url: '../FileUploader.ashx',
                data: formData,
                success: function (status) {
                    if (status != 'error') {
                        NIdImgPath = "MediaUploader/" + status;
                        //$("#myUploadedImg").attr("src", my_path);
                        progressbarLabel.text('Uploaded Successfully');
                        progressbarDiv.fadeOut(4000);
                    }
                },
                processData: false,
                contentType: false,
                error: function (ex) {
                    toastr.warning("NID upload went wrong! " + ex);
                }
            });
        }
        function progressHandlingFunction(e) {
            if (e.lengthComputable) {
                var percentage = Math.floor((e.loaded / e.total) * 100);
                //update progressbar percent complete
                statustxt.html(percentage + '%');
                console.log("Value = " + e.loaded + " :: Max =" + e.total);
            }
        }
        function OnCountrySucceeded(result) {
            $("#ContentPlaceHolder1_txtNationality").val(result);
            //var countryId = $("#ContentPlaceHolder1_ddlCountry").val();
        }
        function OnCountryFailed() {

        }
        function OnMemberInfo1Succeed(result) {

            if (result == null) {
                toastr.warning("No member found");
                $("#ContentPlaceHolder1_txtMemberId_1").val("");
                return false;
            }
            else {
                $("#ContentPlaceHolder1_txtIntroducerName_1").val("");
                $("#ContentPlaceHolder1_txtIntroducerName_1").val(result.FullName);
                $("#ContentPlaceHolder1_hfIntroId1").val(result.MemberId);
            }
            return false;

        }
        function OnMemberInfo2Succeed(result) {

            if (result == null) {
                toastr.warning("No member found");
                $("#ContentPlaceHolder1_txtMemberId_2").val("");
                return false;
            }
            else {
                $("#ContentPlaceHolder1_txtIntroducerName_2").val("");
                $("#ContentPlaceHolder1_txtIntroducerName_2").val(result.FullName);
                $("#ContentPlaceHolder1_hfIntroId2").val(result.MemberId);
            }
            return false;

        }
        function OnMemberInfoFailed() {

        }
        function LoadFamilyMember(relationship, famMemName, famMemBlood, famMemMarriageDate) {
            var famMemDOB = $("#ContentPlaceHolder1_txtDOBfamMem").val();
            if (famMemDOB != "") {
                famMemDOB = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(famMemDOB, innBoarDateFormat);
            }
            if (famMemMarriageDate != "") {
                famMemMarriageDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(famMemMarriageDate, innBoarDateFormat);
            }
            var famMemOccu = $("#ContentPlaceHolder1_txtOccupationFamMem").val();
            //var famMemOccuText = $("#ContentPlaceHolder1_txtOccupationFamMem").text();
            var famMemBloodText = $("#ContentPlaceHolder1_ddlBloodFamMem option:selected").text();
            var tr = "";
            var rowLength = $("#familyMembers tbody tr").length;
            if (rowLength % 2 == 0) {
                tr = "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr = "<tr style='background-color:#E3EAEB;'>";
            }
            tr += "<td style='width:20%;'>" + relationship + "</td>";//0
            tr += "<td style='width:30%;'>" + famMemName + "</td>";//1
            tr += "<td style='width:20%;'>" + famMemDOB + "</td>";//2
            tr += "<td style='width:15%;'>" + famMemBloodText + "</td>";//3
            tr += "<td style='display:none'>" + famMemOccu + "</td>";//4
            tr += "<td style='display:none'>" + famMemMarriageDate + "</td>";//5
            tr += "<td style='display:none'>" + famMemBlood + "</td>";//6, id
            //tr += "<td style='width:15%;'>";
            tr += "<td align='center' style=\"width:15%; cursor:pointer;\"><img src='../Images/delete.png' class= 'DeleteFamMem'  alt='Delete' border='0' /></td>";
            tr += "</tr>";

            $("#familyMembers tbody").append(tr);

            tr = "";
            $("#FamilyMemDiv").show();
            ClearFamMem();


        }
        function LoadEducation(degree, institution) {
            var passYear = $("#ContentPlaceHolder1_txtPassingYear").val();
            var rowLength = $("#tblEducational tbody tr").length;
            var tr = "";
            var newAdded = new Array();

            if (rowLength % 2 == 0) {
                tr = "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr = "<tr style='background-color:#E3EAEB;'>";
            }
            //tr += "<tr>";
            tr += "<td style='width:30%;'>" + degree + "</td>";
            tr += "<td style='width:30%;'>" + institution + "</td>";
            tr += "<td style='width:20%;'>" + passYear + "</td>";
            //tr += "<td align='center' style='width:20%;'> <a href='#' onclick= 'DeleteEducation(this)' ><img alt='Delete' src='../Images/delete.png' /></a> &nbsp";
            tr += "<td align='center' style=\"width:15%; cursor:pointer;\"><img src='../Images/delete.png' class= 'DeleteEducation'  alt='Delete' border='0' /></td>";
            //tr += "<a href='#' onclick= 'EditEducation(this)' ><img alt='Edit' src='../Images/edit.png' /></a></td>";

            tr += "</tr>";

            $("#tblEducational tbody").append(tr);

            tr = "";

            $("#tblEducationalDiv").show();
            ClearEducation();

        }
        function ClearEducation() {
            $("#ContentPlaceHolder1_txtPassingYear").val("");
            $("#ContentPlaceHolder1_txtDegree").val("");
            $("#ContentPlaceHolder1_txtInstitution").val("");
        }
        function ClearFamMem() {
            $("#ContentPlaceHolder1_txtDOBfamMem").val("");
            $("#ContentPlaceHolder1_txtOccupationFamMem").val("");
            $("#ContentPlaceHolder1_txtMarriageDate").val("");
            $("#ContentPlaceHolder1_ddlRelationship").val("");
            $("#ContentPlaceHolder1_txtNameFamMem").val("");

            $("#ContentPlaceHolder1_ddlBloodFamMem").val("0");
        }
        //function DeleteEducation(deleteItem) {
        //    var tr = $(deleteItem).parent().parent();
        //    var apdId = $.trim($(tr).find("td:eq(0)").text());
        //    //if (apdId != "") {
        //    //    DeletedAirportPickupDrop.push({
        //    //        APDId: apdId
        //    //    });
        //    //}
        //    //tr += "<td align='center' style=\"width:15%; cursor:pointer;\"><img src='../Images/delete.png' class= 'RoomDelete'  alt='Delete Room' border='0' /></td>";
        //    $(deleteItem).parent().parent().remove();
        //}
        function EditEducation(editItem) {

        }
        function btnNext_1Validation() {
            var memberTypeId = $("#ContentPlaceHolder1_ddlMemberType").val();
            if (memberTypeId == "0") {
                toastr.warning("Please select a member type.");
                $("#ContentPlaceHolder1_ddlMemberType").focus();
                return false;
            }
            var name = $("#ContentPlaceHolder1_txtFullName").val();
            if (name == "") {
                toastr.warning("Please enter your name.");
                $("#ContentPlaceHolder1_txtFullName").focus();
                return false;
            }
            //var firstName = $("#ContentPlaceHolder1_txtFirstName").val();
            //var lastName = $("#ContentPlaceHolder1_txtLastName").val();
            //if ((firstName == "") || (lastName == "")) {
            //    if (firstName == "") {
            //        toastr.warning("Please insert First Name.");
            //        $("#ContentPlaceHolder1_txtFirstName").focus();
            //    }
            //    else if (lastName == "") {
            //        toastr.warning("Please insert Last Name.");
            //        $("#ContentPlaceHolder1_txtLastName").focus();
            //    }

            //    return false;
            //}
            var bangFullName = $("#ContentPlaceHolder1_txtFullNameBang").val();
            if (bangFullName == "") {
                toastr.warning("Please enter Your name in Bangla");
                $("#ContentPlaceHolder1_txtFullNameBang").focus();
                return false;
            }

            var father = $("#ContentPlaceHolder1_txtFathersName").val();
            if (father == "") {
                toastr.warning("Please enter Your Father's Name");
                $("#ContentPlaceHolder1_txtFathersName").focus();
                return false;
            }

            var mother = $("#ContentPlaceHolder1_txtMothersName").val();
            if (mother == "") {
                toastr.warning("Please enter Your Mother's Name");
                $("#ContentPlaceHolder1_txtMothersName").focus();
                return false;
            }

            var memDOB = $("#ContentPlaceHolder1_txtMemDOB").val();
            if (memDOB == "") {
                toastr.warning("Please Select Date of Birth");
                $("#ContentPlaceHolder1_txtMemDOB").focus();
                return false;
            }
            var genderId = $("#ContentPlaceHolder1_ddlGender").val();
            if (genderId == "0") {
                toastr.warning("Please select Gender");
                $("#ContentPlaceHolder1_ddlGender").focus();
                return false;
            }
            var blood = $("#ContentPlaceHolder1_ddlBloodGroup").val();
            if (blood == "0") {
                toastr.warning("Please select your blood group");
                $("#ContentPlaceHolder1_ddlBloodGroup").focus();
                return false;
            }

            var maritialStatusId = $("#ContentPlaceHolder1_ddlMaritalStatus").val();
            if (maritialStatusId == "0") {
                toastr.warning("Please select your Maritial status");
                $("#ContentPlaceHolder1_ddlMaritalStatus").focus();
                return false;
            }
            var countryId = $("#ContentPlaceHolder1_ddlCountry").val();
            if (countryId == "0") {
                toastr.warning("Please select country");
                $("#ContentPlaceHolder1_ddlCountry").focus();
                return false;
            }

            var nId = $("#ContentPlaceHolder1_txtNID").val();
            if (nId == "") {
                toastr.warning("Please enter your NID number");
                $("#ContentPlaceHolder1_txtNID").focus();
                return false;
            }

            var bPlace = $("#ContentPlaceHolder1_txtBirthPlace").val();
            if (bPlace == "") {
                toastr.warning("Please enter your Birth place");
                $("#ContentPlaceHolder1_txtBirthPlace").focus();
                return false;
            }

            var religionId = $("#ContentPlaceHolder1_ddlReligion").val();
            if (religionId == "0") {
                toastr.warning("Please select religion.");
                $("#ContentPlaceHolder1_ddlReligion").focus();
                return false;
            }
            var mobileNo = $("#ContentPlaceHolder1_txtMobile").val();
            if (mobileNo == "") {
                toastr.warning("Please enter mobile number.");
                $("#ContentPlaceHolder1_txtMobile").focus();
                return false;
            }
            var email = $("#ContentPlaceHolder1_txtEmail").val();
            if (email == "") {
                toastr.warning("Please enter an Email.");
                $("#ContentPlaceHolder1_txtEmail").focus();
                return false;
            }
            var presentAddrs = $("#ContentPlaceHolder1_txtPresentAddress").val();
            if (presentAddrs == "") {
                toastr.warning("Please enter your present Address.");
                $("#ContentPlaceHolder1_txtPresentAddress").focus();
                return false;
            }
            var permanentAddrs = $("#ContentPlaceHolder1_txtPermanentAddress").val();
            if (permanentAddrs == "") {
                toastr.warning("Please enter your permanent Address.");
                $("#ContentPlaceHolder1_txtPermanentAddress").focus();
                return false;
            }
            $("#Occupation").show();
            $("#BasicInformation").hide();
            ScrollToBottom();
            return false;
            //var firstName = $("#").val(); 
            /*
                toastr.warning("");
                $("#").focus();
                return false;
              
             * */

        }
        function ScrollToBottom() {
            document.getElementById('Occupation').scrollIntoView();
        }
        //function btnNext_2Validation() {
        //    $("#FamilyInfo").show();
        //    $("#Occupation").hide();
        //}
        //function btnNext_3Validation() {
        //    $("#Educational").show();
        //    $("#FamilyInfo").hide();
        //}
        //function btnNext_4Validation() {
        //    $("#Introducer").show();
        //    $("#Educational").hide();
        //}

        function SaveOnlineMember() {


            if (($("#ContentPlaceHolder1_txtIntroducerName_1").val() == "") || ($("#ContentPlaceHolder1_txtIntroducerName_2").val() == "")) {
                toastr.warning("Introducer can't remain blank.");
                return false;
            }
            var introId1 = "0";
            introId1 = $("#ContentPlaceHolder1_hfIntroId1").val() != "" ? $("#ContentPlaceHolder1_hfIntroId1").val() : "0";
            var introId2 = "0";
            introId2 = $("#ContentPlaceHolder1_hfIntroId2").val() != "" ? $("#ContentPlaceHolder1_hfIntroId2").val() : "0";
            if ((introId1 == "0") && (introId2 == "0")) {
                toastr.warning("Please add at least one Introducer");
                return false;
            }
            var nIdImg = $("#NIDimage").val();
            var personalImg = $("#PersonalImage").val();
            if (nIdImg == "" || personalImg == "") {
                toastr.warning("Please insert your passport size picture and NID copy");

                return false;
            }
            var checked = $("#chkTerms").is(":checked");
            if (checked == false) {
                toastr.warning("Please fill the check mark if you agree.");
                $("#chkTerms").focus();
                return false;
            }

            var memberTypeId = $("#ContentPlaceHolder1_ddlMemberType").val();
            //var firstName = $("#ContentPlaceHolder1_txtFirstName").val();
            //var lastName = $("#ContentPlaceHolder1_txtLastName").val();
            //var nickName = $("#ContentPlaceHolder1_txtNickName").val();
            var fatherName = $("#ContentPlaceHolder1_txtFathersName").val();
            var motherName = $("#ContentPlaceHolder1_txtMothersName").val();
            var fullName = $("#ContentPlaceHolder1_txtFullName").val();
            //var fullName = firstName + " " + lastName;
            //if (nickName != "") {
            //    fullName = firstName + " " + lastName + " " + nickName;
            //}
            var bangFullName = $("#ContentPlaceHolder1_txtFullNameBang").val();
            var memDOB = $("#ContentPlaceHolder1_txtMemDOB").val();
            if (memDOB != "") {
                memDOB = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(memDOB, innBoarDateFormat);
            }
            var genderId = $("#ContentPlaceHolder1_ddlGender").val();
            var bloodId = 0;
            var bloodId = $("#ContentPlaceHolder1_ddlBloodGroup").val();
            var maritialStatusId = 0;
            var maritialStatusId = $("#ContentPlaceHolder1_ddlMaritalStatus").val();
            var countryId = 0;
            var countryId = $("#ContentPlaceHolder1_ddlCountry").val();
            var nationality = $("#ContentPlaceHolder1_txtNationality").val();
            var religionId = $("#ContentPlaceHolder1_ddlReligion").val();
            var mobileNo = $("#ContentPlaceHolder1_txtMobile").val();
            var email = $("#ContentPlaceHolder1_txtEmail").val();
            var passPort = $("#ContentPlaceHolder1_txtPassportNumber").val();
            var professionId = null;
            var professionId = $("#ContentPlaceHolder1_ddlProfessionId").val() != "" ? $("#ContentPlaceHolder1_ddlProfessionId").val() : null;
            var homePhone = $("#ContentPlaceHolder1_txtHomePhone").val();
            var presentAddrs = $("#ContentPlaceHolder1_txtPresentAddress").val();
            var permanentAddrs = $("#ContentPlaceHolder1_txtPermanentAddress").val();
            var hobbies = $("#ContentPlaceHolder1_txtHobbies").val();
            var award = $("#ContentPlaceHolder1_txtAward").val();

            var birthPlace = $("#ContentPlaceHolder1_txtBirthPlace").val();
            var height = "0.00", weight = "0.00";
            height = $("#ContentPlaceHolder1_txtHeight").val();
            weight = $("#ContentPlaceHolder1_txtWeight").val();
            var nId = $("#ContentPlaceHolder1_txtNID").val();
            //nominee
            var nomineeName = $("#<%=txtNomineeName.ClientID %>").val();
            var nomFather = $("#<%=txtNomineeFather.ClientID %>").val();
            var nomMother = $("#<%=txtNomineeMother.ClientID %>").val();
            var nomDOB = $("#<%=txtNomineeDOB.ClientID %>").val();
            if (nomDOB != "") {
                nomDOB = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(nomDOB, innBoarDateFormat);
            }
            var nomRelation = $("#ContentPlaceHolder1_ddlNomineeRelation").val();
            //occupation
            var occupation = $("#ContentPlaceHolder1_txtOccupation").val();
            var designation = $("#ContentPlaceHolder1_txtDesignation").val();
            var organization = $("#ContentPlaceHolder1_txtOrganigation").val();
            var ofcAddress = $("#ContentPlaceHolder1_txtOfficeAddress").val();
            //family members
            var rowLength1 = $("#familyMembers tbody tr").length;
            var MemberFamily = new Array();
            var relationship = "", famMemName = "", famMemDOB = "", famMemOccu = "";
            var marriageDate = null;
            var famMemBlood = "0";
            $("#familyMembers tbody tr").each(function () {
                if (rowLength1 > 0) {
                    relationship = $(this).find("td:eq(0)").text();
                    famMemName = $(this).find("td:eq(1)").text();
                    famMemDOB = $(this).find("td:eq(2)").text();
                    famMemBlood = $(this).find("td:eq(6)").text();
                    famMemOccu = $(this).find("td:eq(4)").text();
                    marriageDate = $(this).find("td:eq(5)").text();
                }
                MemberFamily.push({
                    MemberName: famMemName,
                    MemberDOB: famMemDOB,
                    Occupation: famMemOccu,
                    Relationship: relationship,
                    FamMemMarriageDate: marriageDate,
                    FamMemBloodGroupId: famMemBlood

                });
            });
            //education
            var rowLength2 = $("#tblEducational tbody tr").length;
            var Education = new Array();
            var degree = "", institution = "";
            var passYear = null;
            $("#tblEducational tbody tr").each(function () {
                if (rowLength2 > 0) {
                    degree = $(this).find("td:eq(0)").text();
                    institution = $(this).find("td:eq(1)").text();
                    passYear = $(this).find("td:eq(2)").text();
                }
                Education.push({
                    Degree: degree,
                    Institution: institution,
                    PassingYear: passYear
                });
            });

            //Nationality: nationality,

            var OnlineMember = {
                TypeId: memberTypeId,
                FullName: fullName,
                NameBangla: bangFullName,
                FatherName: fatherName,
                MotherName: motherName,
                BirthDate: memDOB,
                BloodGroup: bloodId,
                MemberGender: genderId,
                MaritalStatus: maritialStatusId,
                CountryId: countryId,
                NationalitySt: nationality,
                PassportNumber: passPort,
                ReligionId: religionId,
                MobileNumber: mobileNo,
                PersonalEmail: email,
                ProfessionId: professionId,
                ResidencePhone: homePhone,
                MemberAddress: permanentAddrs,
                MailAddress: presentAddrs,
                Hobbies: hobbies,
                Awards: award,
                Occupation: occupation,
                Organization: organization,
                Designation: designation,
                OfficeAddress: ofcAddress,
                Introducer_1_id: introId1,
                Introducer_2_id: introId2,
                BirthPlace: birthPlace,
                Height: height,
                Weight: weight,
                NationalID: nId,
                NomineeName: nomineeName,
                NomineeFather: nomFather,
                NomineeMother: nomMother,
                NomineeDOB: nomDOB,
                NomineeRelationId: nomRelation,
                PathPersonalImg: PersonalImgPath,
                PathNIdImage: NIdImgPath
            };

            CommonHelper.SpinnerOpen();
            $.ajax({
                type: "POST",
                url: "/Membership/OnlineMembership.aspx/SaveOnlineMember",
                data: JSON.stringify({
                    OnlineMemberObj: OnlineMember,
                    MemberFamilyList: MemberFamily,
                    EducationList: Education
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d.IsSuccess == true) {
                        CommonHelper.SpinnerClose();
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                        setInterval(function () { }, 3000);
                        /*CommonHelper.AlertMessage(result.d.AlertMessage)*/;

                        ClearAll();
                    }
                    else {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    }
                },
                error: function (error) {
                    toastr.error("there is an error");
                }
            });
        }
        function checkTerms(form) {
            if (!form.terms.checked) {
                alert("Please indicate that you accept the Terms and Conditions");
                form.terms.focus();
                return false;
            }
            return true;
        }
        function ClearAll() {
            $("#ContentPlaceHolder1_hfIntroId1").val("0");
            $("#ContentPlaceHolder1_hfIntroId2").val("0");
            location.reload(true);
            //$('#form').trigger("reset"); ContentPlaceHolder1_hfMemberId

        }


        //style="display: none;"
    </script>
    <div id="form" class="panel panel-default">
        <asp:HiddenField ID="hfIntroId1" Value="0" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfIntroId2" Value="0" runat="server"></asp:HiddenField>
        <div id="BasicInformation" class="panel">
            <%--<div class="panel-heading">
                Membership Application Basic Information
            </div>--%>
            <fieldset>
                <legend style="font-weight: bold">Membership Application Basic Information</legend>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMemberType" runat="server" class="control-label required-field"
                                    Text="Member Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlMemberType" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <%--<div class="col-md-2">
                        <asp:Label ID="lblTitle" runat="server" class="control-label" Text="Title"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTitle" runat="server" CssClass="form-control">
                            <asp:ListItem Value="none">None</asp:ListItem>
                            <asp:ListItem Value="Mr.">Mr.</asp:ListItem>
                            <asp:ListItem Value="Mrs.">Mrs.</asp:ListItem>
                            <asp:ListItem Value="Miss">Miss</asp:ListItem>
                        </asp:DropDownList>
                    </div>--%>
                            <div class="col-md-2">
                                <asp:Label ID="lblFirstName" runat="server" class="control-label required-field"
                                    Text="Full Name (English)"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtFullName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <%--<div class="col-md-4">
                                <asp:TextBox ID="txtFirstName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLastName" runat="server" class="control-label required-field" Text="Last Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLastName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>--%>
                        </div>
                        <%--<div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMiddleName" runat="server" class="control-label" Text="Nick/Sur Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNickName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>

                        </div>--%>
                        <%--<div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblDisplayName" runat="server" class="control-label required-field"
                            Text="Full Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" disabled="disabled"></asp:TextBox>
                    </div>
                </div>--%>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label"
                                    Text="Full Name (Bangla)"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtFullNameBang" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field"
                                    Text="Father's Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtFathersName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Mother's Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtMothersName" CssClass="form-control" runat="server"></asp:TextBox>
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
                                <asp:Label ID="lblBloodGroup" runat="server" class="control-label required-field" Text="Blood Group"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="form-control">
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
                                <asp:Label ID="lblGender" runat="server" class="control-label required-field" Text="Gender"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
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
                                <asp:Label ID="lblCountry" runat="server" class="control-label required-field"
                                    Text="Country"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblNationality" runat="server" class="control-label" Text="Nationality"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNationality" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label28" runat="server" class="control-label required-field" Text="NID"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNID" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label29" runat="server" class="control-label required-field" Text="Birth Place"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBirthPlace" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPassportNumber" runat="server" class="control-label" Text="Passport Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassportNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
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
                                <asp:Label ID="Label23" runat="server" class="control-label required-field" Text="Mobile No."></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" TabIndex="41"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label24" runat="server" class="control-label required-field" Text="Email"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label25" runat="server" class="control-label" Text="Profession"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlProfessionId" runat="server" TabIndex="40" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblHomePhone" runat="server" class="control-label" Text="Home Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtHomePhone" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <%-- <div class="form-group">
                            
                        </div>--%>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentAddress" runat="server" class="control-label required-field" Text="Present Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPresentAddress" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Permanent Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPermanentAddress" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Hobbies & Special Inst."></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtHobbies" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label7" runat="server" class="control-label" Text="Honor & Award (if any)"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAward" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                </div>
            </fieldset>
            <div class="form-group col-md-offset-10">
                <%--<input id="btnNext_1" type="button" value="Next" class="TransactionalButton btn btn-primary btn-sm" />--%>
                <asp:Button ID="btnNext_1" runat="server" Text="Next" CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript: return btnNext_1Validation()" />
            </div>


        </div>
        <div id="Occupation" class="panel" style="display: none;">
            <fieldset>
                <legend>Occupational Details</legend>
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
                            <div class="col-md-10">
                                <asp:TextBox ID="txtOrganigation" CssClass="form-control" runat="server"></asp:TextBox>
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
            </fieldset>

            <div class="form-group col-md-offset-10">
                <input id="btnPrev1" type="button" value="Prev." class="TransactionalButton btn btn-primary btn-sm" />
                &nbsp;
                <input id="btnNext_2" type="button" value="Next" class="TransactionalButton btn btn-primary btn-sm" />
                <%--<asp:Button ID="" runat="server" Text="Next" CssClass="TransactionalButton btn btn-primary btn-sm" />--%>
            </div>
        </div>
        <div id="FamilyInfo" class="panel" style="display: none;">
            <fieldset>
                <legend>Spouse & Children</legend>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRelationship" runat="server" class="control-label" Text="Relationship"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRelationship" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div id="MarriageDateDiv" style="display: none">
                                <div class="col-md-2">
                                    <asp:Label ID="Label12" runat="server" class="control-label required-field"
                                        Text="Marriage Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtMarriageDate" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblName" runat="server" class="control-label required-field"
                                    Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNameFamMem" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label13" runat="server" class="control-label" Text="Date Of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDOBfamMem" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label15" runat="server" class="control-label" Text="Occupation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtOccupationFamMem" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label14" runat="server" class="control-label" Text="Blood Group"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBloodFamMem" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <div class="row col-md-offset-2">
                            <%--<asp:Button ID="btnAddFamMem" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary btn-sm" />--%>
                            <input id="btnAddFamMem" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" />
                        </div>

                        <div class="form-group" id="FamilyMemDiv" style="display: none">
                            <table id="familyMembers" class="table table-bordered table-condensed table-responsive" style="width: 100%">
                                <colgroup>
                                    <col style="width: 20%;" />
                                    <col style="width: 30%;" />
                                    <col style="width: 20%;" />
                                    <col style="width: 15%;" />
                                    <col style="width: 15%;" />
                                </colgroup>
                                <thead>
                                    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                        <td style="text-align: left;">Relationship</td>
                                        <td style="text-align: left;">Name</td>
                                        <td style="text-align: left;">DoB</td>
                                        <td style="text-align: left;">Blood Group</td>
                                        <td style="text-align: center;">Action</td>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </fieldset>

            <div class="form-group col-md-offset-10">
                <input id="btnPrev2" type="button" value="Prev." class="TransactionalButton btn btn-primary btn-sm" />
                &nbsp;
                <input id="btnNext_3" type="button" value="Next" class="TransactionalButton btn btn-primary btn-sm" />
                <%--<asp:Button ID="btnNext_3" runat="server" Text="Next" CssClass="TransactionalButton btn btn-primary btn-sm"/>--%>
            </div>

        </div>
        <div id="Educational" class="panel" style="display: none;">
            <fieldset>
                <legend>Educational Details</legend>
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
                        <br />
                        <div class="col-md-offset-2">
                            <%--<asp:Button ID="btnAddInst" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary btn-sm" />--%>
                            <input id="btnAddInst" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" />
                        </div>
                        &nbsp;
                        <div class="form-group" id="tblEducationalDiv" style="display: none">
                            <table id="tblEducational" class="table table-bordered table-condensed table-responsive" style="width: 100%">
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
                        </div>
                    </div>
                </div>
            </fieldset>

            <div class="form-group col-md-offset-10">
                <input id="btnPrev3" type="button" value="Prev." class="TransactionalButton btn btn-primary btn-sm" />
                &nbsp;
                <input id="btnNext_4" type="button" value="Next" class="TransactionalButton btn btn-primary btn-sm" />
                <%--<asp:Button ID="btnNext_4" runat="server" Text="Next" CssClass="TransactionalButton btn btn-primary btn-sm" />--%>
            </div>

        </div>
        <div id="Nominee" class="panel" style="display: none;">
            <fieldset>
                <legend>Nominee Details</legend>
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
            </fieldset>
            <div class="form-group col-md-offset-10">
                <input id="btnPrev4" type="button" value="Prev." class="TransactionalButton btn btn-primary btn-sm" />
                &nbsp;
                <input id="btnNext_5" type="button" value="Next" class="TransactionalButton btn btn-primary btn-sm" />
                <%--<asp:Button ID="btnNext_4" runat="server" Text="Next" CssClass="TransactionalButton btn btn-primary btn-sm" />--%>
            </div>
        </div>
        <div id="Introducer" class="panel" style="display: none">
            <fieldset class="form-group">
                <legend>Introducer</legend>
                <div class="form-horizontal">
                    <fieldset class="col-md-6">
                        <legend>Introducer-1</legend>
                        <div class="panel-body">
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="Label19" runat="server" class="control-label" Text="Member Id"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtMemberId_1" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="Label20" runat="server" class="control-label" Text="Name"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtIntroducerName_1" CssClass="form-control" disabled="disabled" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <fieldset class="col-md-6">
                        <legend>Introducer-2</legend>
                        <div class="panel-body">
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="Label21" runat="server" class="control-label" Text="Member Id"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtMemberId_2" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="Label22" runat="server" class="control-label" Text="Name"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtIntroducerName_2" CssClass="form-control" runat="server" disabled="disabled"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </fieldset>
            <div class="row">
                <div class="col-md-offset-1 col-md-3">
                    <asp:Label ID="Label35" runat="server" class="control-label required-field" Text="Upload your image (Maximum 300*350 px)"></asp:Label>
                </div>
                <div class="col-md-4">
                    <%--<asp:FileUpload ID="PersonalImage" runat="server" />--%>
                    <input type="file" class="upload" id="PersonalImage" />

                </div>
                <%-- <div class="col-md-2">
                    <input id="btnImageUpload" type="button" value="Upload" class="TransactionalButton btn btn-primary btn-sm" onclick="UploadImage()" />
                </div>--%>
                <div class="col-md-3">
                    <div id="divProgressbar" style="display: none">
                        <span id="lblPersonalImgProgressbar" style="position: absolute; left: 5%; top: 10%;">Please Wait...</span>
                    </div>
                    <%--<div id="#statustxt">0%</div>--%>
                </div>
            </div>
            &nbsp;
            <div class="row">
                <div class="col-md-offset-1 col-md-3">
                    <asp:Label ID="Label37" runat="server" class="control-label required-field" Text="Upload your NID (Maximum 600*350 px)"></asp:Label>
                </div>
                <div class="col-md-4">
                    <%--<asp:FileUpload ID="NIDimage" runat="server" />--%>
                    <input type="file" class="upload" id="NIDimage" />
                </div>
                <%--<div class="col-md-2">
                    <input id="btnNIDUpload" type="button" value="Upload" class="TransactionalButton btn btn-primary btn-sm" onclick="UploadNID()" />
                </div>--%>
                <div class="col-md-3">
                    <div id="divProgressbarNID" style="display: none">
                        <span id="lblPersonalImgProgressbarNID" style="position: absolute; left: 5%; top: 10%;">Please Wait...</span>
                    </div>
                    <%--<div id="#statustxt">0%</div>--%>
                </div>
            </div>
            &nbsp;
            <div class="row">
                <div class=" col-md-offset-1 col-md-1" style="width: 0;">
                    <input type="checkbox" value="1" id="chkTerms" name="terms" />
                </div>
                <div class="col-md-10">
                    I wish to become Member of the Signature Club Ltd. (the “SCL”) and, in the event of my membership being accepted, I agree to
                abide by the Memorandum and Articles of Association, Rules, Regulations and Policy of the SCL as well as the terms and
                conditions as regards to the membership I apply for. I understand that any violation of these may lead to my membership being
                withdrawn. I also confirm that the above-mentioned details and answers provided for this application and in the attached
                Questionnaire are all correct, true, accurate and not misleading. I acknowledge that making a false statement may lead to my
                application being rejected or membership being withdrawn. I also understand that if my membership is accepted, the amount of
                money against my membership and other subscription already paid/being paid/to be paid are absolutely non-refundable even
                my membership is withdrawn/cancelled for whatever reasons. Further, I understand that this application does not guarantee my
                membership of SCL and I may be required to appear for a personal interview for the membership.
                </div>
            </div>
            <div class="form-group">
            </div>
            <div class="form-group">
                <div class="col-md-offset-10">
                    <input id="btnPrev5" type="button" value="Prev." class="TransactionalButton btn btn-primary btn-sm" />
                    &nbsp;
                <input id="btnSave" type="button" value="Save & Submit" class="TransactionalButton btn btn-primary btn-sm" onclick="SaveOnlineMember()" />
                    <%--<asp:Button ID="btnSave" runat="server" Text="Save & Submit" CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript: return SaveValidation()" />--%>
                </div>
            </div>

        </div>
    </div>

</asp:Content>

