<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmApplicant.aspx.cs" Inherits="HotelManagement.Presentation.Website.Recruitment.frmApplicant" %>

<%@ Register Assembly="flashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        //Bread Crumbs Information-------------

        function OnEmpCodeCheckSucceeded(result) {
            if (!result) {

                toastr.warning("This Employee ID is Duplicate. Please Give Another Unique One.");
                $("#ContentPlaceHolder1_txtEmpCode").val("");
            }
        }
        function OnEmpCodeCheckFailed() { }

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Applicant Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_ddlPresentCountry").select2({
                tags: "true",
                placeholder: "--Please Select--",
                allowClear: true,
                width: "99.75%"
            });

            if ($("#<%=txtFirstName.ClientID %>").val() != "" && $("#<%=txtLastName.ClientID %>").val() != "") {
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                $("#<%=txtDisplayName.ClientID %>").val(firstName + " " + lastName);
            }

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
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
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_H').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_I').click(function () {
                $('#SubmitButtonDiv').hide();
            });
            
            $("#SearchOutput").hide();
            $("#btnSearch").click(function () {
                $("#SearchOutput").show('slow');
                GridPaging(1, 1);
            });

            var txtEmpDateOfBirth = '<%=txtEmpDateOfBirth.ClientID%>'
            UploadComplete();

            $("#ContentPlaceHolder1_txtEmpCode").change(function () {

                if ($("#ContentPlaceHolder1_txtEmpCode").val() != "") {

                    if ($("#ContentPlaceHolder1_hfEmpCode").val() == $("#ContentPlaceHolder1_txtEmpCode").val()) {
                        return;
                    }

                    PageMethods.CheckDuplicateEmployeeCode($("#ContentPlaceHolder1_txtEmpCode").val(), OnEmpCodeCheckSucceeded, OnEmpCodeCheckFailed);
                }
                return false;
            });

            $('#ContentPlaceHolder1_txtDateOfBirth').datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate,
                yearRange: "-100:+0",
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    AgeCalc(selectedDate, 'ContentPlaceHolder1_txtAge', 'ContentPlaceHolder1_hfAge');
                }
            });

            $("#ContentPlaceHolder1_txtJoinDate").datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                yearRange: "-100:+0",
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtLeaveDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtLeaveDate").datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtJoinDate").datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtPExpireDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtPIssueDate").datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtPIssueDate").datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtPExpireDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtEmpDateOfBirth").datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtLastName").blur(function () {
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                $("#<%=txtDisplayName.ClientID %>").val(firstName + " " + lastName);
            });

            $("#ContentPlaceHolder1_txtFirstName").blur(function () {
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                $("#<%=txtDisplayName.ClientID %>").val(firstName + " " + lastName);
            });

            var btn = $("#<%=btnSave.ClientID %>").val();
            if (btn == "Update") {
                $("#ApplicantId").show();
            }
            $('#ContentPlaceHolder1_txtPersonalEmail').blur(function () {
                var email = $("#ContentPlaceHolder1_txtPersonalEmail").val();
                if (CommonHelper.IsValidEmail(email) == false) {
                    toastr.warning("Invalid Email");
                    $("#<%=txtEmail.ClientID %>").focus();
                    return false;
                }
            });
            $('#ContentPlaceHolder1_txtAlternativeEmail').blur(function () {
                var email = $("#ContentPlaceHolder1_txtAlternativeEmail").val();
                if (CommonHelper.IsValidEmail(email) == false) {
                    toastr.warning("Invalid Email");
                    $("#<%=txtEmail.ClientID %>").focus();
                    return false;
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
            CommonHelper.ApplyDecimalValidation();
            CommonHelper.ApplyIntigerValidation();
        });
        

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvGustIngormation tbody tr").length;

            var empName = $("#<%=txtEmployeeName.ClientID %>").val();
            var code = $("#<%=txtEmployeeCode.ClientID %>").val();
            PageMethods.SearchNLoadEmpInformation(empName, code, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }

        function OnLoadObjectSucceeded(result) {
            $("#gvEmployee tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvEmployee tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvEmployee tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:50%; cursor:pointer;\">" + gridObject.DisplayName + "</td>";
                tr += "<td align='left' style=\"width:40%; cursor:pointer;\">" + gridObject.EmpCode + "</td>";
                //tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.Department + "</td>";
                //tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.Designation + "</td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.EmpId + "')\" alt='Edit Information' border='0' /></td>";
                //                tr += "<td align='right' style=\"width:8%; cursor:pointer;\"><img src='../Images/delete.png' class= 'BankDelete'  alt='Delete Information' border='0' /></td>";
                //                tr += "<td align='right' style=\"width:8%; display:none;\">" + gridObject.BankId + "</td>";

                tr += "</tr>"

                $("#gvEmployee tbody ").append(tr);
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

        function PerformEditAction(empId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            var possiblePath = "frmApplicant.aspx?editId=" + empId;
            window.location = possiblePath;
        }

        function AgeCalc(dateOfBirth, display, hf) {
            if ($.trim(dateOfBirth) == "") {
                return;
            }

            var params = JSON.stringify({ dateOfBirth: $.trim(dateOfBirth) });

            $.ajax({
                type: "POST",
                url: "../../../Common/WebMethodPage.aspx/CalculateAge",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d != "") {
                        $("#" + display).val(result.d);
                        $("#" + hf).val(result.d);
                    }
                },
                error: function (error) {
                    toastr.error("error");
                }
            });
        }

        $(function () {
            $("#myTabs").tabs();
        });

        $(function () {
            $("#tab-10").tabs();
        });

        function LoadSignatureUploader() {
            //alert('Item: ' + val);
            //popup(1, 'popUpSignature', '', 600, 300);
            var randomId = +$("#ContentPlaceHolder1_RandomEmpId").val();
            var path = "/Recruitment/Images/Signature/";
            var category = "Applicant Signature";
            var iframeid = 'Iframe1';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#popUpSignature").dialog({
                width: 600,
                height: 300,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "",
                show: 'slide'
            });
            return false;
        }

        function LoadDocumentUploader() {
            //alert('hi');
            //alert('Item: ' + val);
            //popup(1, 'popUpDocument', '', 600, 300);
            var randomId = +$("#ContentPlaceHolder1_RandomEmpId").val();
            var path = "/Recruitment/Images/Documents/";
            var category = "Applicant Document";
            var iframeid = 'Iframe2';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;
            $("#popUpDocument").dialog({
                width: 600,
                height: 300,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "",
                show: 'slide'
            });
            return false;
        }

        function LoadOthersDocumentUploader() {
            //alert('hi');
            //alert('Item: ' + val);
            //popup(1, 'popUpOthersDocument', '', 600, 300);
            $("#popUpOthersDocument").dialog({
                width: 600,
                height: 300,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "",
                show: 'slide'
            });
            return false;
        }

        function UploadComplete() {
            var id = $("#ContentPlaceHolder1_RandomEmpId").val();
            ShowUploadedDocument(id);
            ShowUploadedSignature(id);
            ShowUploadedOthersDocument(id);
        }

        function ShowUploadedDocument(id) {
            PageMethods.GetUploadedImageByWebMethod(id, "Applicant Document", OnGetUploadedImageByWebMethodSucceeded, OnGetUploadedImageByWebMethodFailed);
            return false;
        }

        function ShowUploadedSignature(id) {
            PageMethods.GetUploadedImageByWebMethod(id, "Applicant Signature", OnGetUploadedSignatureByWebMethodSucceeded, OnGetUploadedSignatureByWebMethodFailed);
            return false;
        }

        function ShowUploadedOthersDocument(id) {
            PageMethods.GetUploadedDocumentsByWebMethod(id, "Applicant Other Documents", OnGetUploadedOthersDocumentByWebMethodSucceeded, OnGetUploadedOthersDocumentByWebMethodFailed);
            return false;
        }

        function OnGetUploadedSignatureByWebMethodSucceeded(result) {
            $('#DocDiv').html(result);
            return false;
        }
        function OnGetUploadedSignatureByWebMethodFailed(error) {
            toastr.error(error.get_message());
        }


        function OnGetUploadedImageByWebMethodSucceeded(result) {
            $('#SigDiv').html(result);
            return false;
        }
        function OnGetUploadedImageByWebMethodFailed(error) {
            toastr.error(error.get_message());
        }

        function OnGetUploadedOthersDocumentByWebMethodSucceeded(result) {
            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var DocumentTable = "";

            if (totalDoc != 0) {


                DocumentTable += "<table id='DocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                DocumentTable += "<th align='left' scope='col'>Document Name</th><th align='left' scope='col'>Image</th> <th align='left' scope='col'>Action</th></tr>";

                for (row = 0; row < totalDoc; row++) {
                    if (row % 2 == 0) {
                        DocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                    }
                    else {
                        DocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                    }

                    DocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";

                    if (guestDoc[row].Path != "") {
                        if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png")
                            imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                        else
                            imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    }
                    else
                        imagePath = "";

                    DocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

                    DocumentTable += "<td align='left' style='width: 20%'>";
                    //DocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                    DocumentTable += "</td>";
                    DocumentTable += "</tr>";
                }
                DocumentTable += "</table>";

            }

            $("#ContentPlaceHolder1_DocumentInfo").html(DocumentTable);

            //$('#OthersDocDiv').html(result);
            return false;
        }
        function OnGetUploadedOthersDocumentByWebMethodFailed(error) {
            toastr.error(error.get_message());
        }

        function CheckAgeEligibility() {

            var dob = $.trim($("#ContentPlaceHolder1_txtDateOfBirth").val());

            if (dob == "") {
                toastr.warning("Please Give Date of Birth");
                return false;
            }

            var vc = $("#ContentPlaceHolder1_hfAge").val();
            var year = 0, month = 0, days = 0;

            if (vc != "") {
                var vcc = vc.split(',');
                year = parseInt($.trim(vcc[0].replace(' years', '')));
                month = parseInt($.trim(vcc[1].replace(' months', '')));
                days = parseInt($.trim(vcc[2].replace(' days', '')));

                if (year > 18) {
                    toastr.warning("Age Should be 18 years");
                    return false;
                }
                else if (year == 18 && (month > 0 || days > 0)) {
                    toastr.warning("Age Should be 18 years");
                    return false;
                }
            }

            return true;
        }

        function LoadDocUploader() {
            
            var randomId = +$("#ContentPlaceHolder1_RandomEmpId").val();
            var path = "/Recruitment/Images/Others/";
            var category = "Applicant Other Documents";
            var iframeid = 'frmPrint';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#DocumentDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 300,
                closeOnEscape: false,
                resizable: false,
                title: "Documents Upload",
                show: 'slide'
            });
            return false;
        }
        <%--function DeleteDoc(docId, rowIndex) {
            var deletedDoc = $("#<%=hfDeletedDoc.ClientID %>").val();

            if (deletedDoc != "")
                deletedDoc += "," + docId;
            else
                deletedDoc = docId;

            $("#<%=hfDeletedDoc.ClientID %>").val(deletedDoc);

            $("#trdoc" + rowIndex).remove();
        }--%>
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Personal Info</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Career Info</a></li>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Education</a></li>
            <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-4">Experience</a></li>
            <li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-5">Training</a></li>
            <li id="F" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-6">Language Proficiency</a></li>
            <li id="G" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-7">Reference</a></li>
            <li id="H" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-8">Documents</a></li>
            <li id="I" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-9">Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="PersonalInformation" class="panel panel-default">
                <div class="panel-heading">
                    Applicant Personal Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblGoToScrolling" runat="server" class="control-label" Text="Go To Scrolling"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGoToScrolling" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div id="ApplicantId" class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmpCode" runat="server" class="control-label required-field" Text="Applicant Id"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfEmpCode" runat="server" />
                                <asp:TextBox ID="txtEmpCode" CssClass="form-control" runat="server" disabled="disabled"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFirstName" runat="server" class="control-label required-field"
                                    Text="First Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFirstName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLastName" runat="server" class="control-label required-field" Text="Last Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLastName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDisplayName" runat="server" class="control-label" Text="Full Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" disabled></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFathersName" runat="server" class="control-label" Text="Father's Name"></asp:Label>
                                <%--<span class="MandatoryField">*</span>--%>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtFathersName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMothersName" runat="server" class="control-label" Text="Mother's Name"></asp:Label>
                                <%--<span class="MandatoryField">*</span>--%>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtMothersName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmpDateOfBirth" runat="server" class="control-label required-field"
                                    Text="Date Of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmpDateOfBirth" CssClass="form-control" runat="server"></asp:TextBox>
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
                                <asp:Label ID="lblBloodGroup" runat="server" class="control-label" Text="Blood Group"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblReligion" runat="server" class="control-label required-field" Text="Religion"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblHeight" runat="server" class="control-label" Text="Height"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtHeight" CssClass="form-control" runat="server"></asp:TextBox>
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
                                <asp:Label ID="lblNatinality" runat="server" class="control-label required-field"
                                    Text="Nationality"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCountryId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblNationalId" runat="server" class="control-label" Text="National Id/SSN"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNationalId" CssClass="form-control" runat="server"></asp:TextBox>
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
                                <asp:Label ID="lblPIssueDate" runat="server" class="control-label" Text="Pass. Issue Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPIssueDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPIssuePlace" runat="server" class="control-label" Text="Pass. Issue Place"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPIssuePlace" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPExpireDate" runat="server" class="control-label" Text="Pass. Expiry Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPExpireDate" CssClass="form-control" runat="server"></asp:TextBox>
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
                                <asp:Label ID="lblPresentCity" runat="server" class="control-label" Text="Present City"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPresentCity" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentZipCode" runat="server" class="control-label" Text="Present Zip/ P.O. Box"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPresentZipCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentCountry" runat="server" class="control-label" Text="Present Country"></asp:Label>
                            </div>

                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPresentCountry" runat="server" CssClass="form-control" TabIndex="47">
                                </asp:DropDownList>
                            </div>

                            <%--<div class="col-md-4">
                                <asp:TextBox ID="txtPresentCountry" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>--%>
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentPhone" runat="server" class="control-label" Text="Present Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPresentPhone" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentAddress" runat="server" class="control-label" Text="Permanent Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPermanentAddress" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentCity" runat="server" class="control-label" Text="Permanent City"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPermanentCity" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentZipCode" runat="server" class="control-label" Text="Per. Zip/ P.O. Box"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPermanentZipCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentCountry" runat="server" class="control-label" Text="Permanent Country"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPermanentCountry" runat="server" CssClass="form-control" TabIndex="47">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentPhone" runat="server" class="control-label" Text="Permanent Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPermanentPhone" CssClass="form-control" runat="server"></asp:TextBox>
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
                                <asp:Label ID="lblAlternativeEmail" runat="server" class="control-label" Text="Alternative Email"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAlternativeEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="CareerInfo" class="panel panel-default">
                <asp:Label ID="hfEmpCareerInfoId" runat="server" Text='' Visible="False"></asp:Label>
                <div class="panel-heading">
                    Career Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblObjective" runat="server" class="control-label" Text="Objective"></asp:Label>
                            </div>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtObjective" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblPresentSalary" runat="server" class="control-label" Text="Present Salary"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtPresentSalary" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblExpectedSlary" runat="server" class="control-label" Text="Expected Salary"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtExpectedSlary" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblCurrency" runat="server" class="control-label required-field" Text="Currency"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblPrfJobLocation" runat="server" class="control-label"
                                    Text="Prefered Job Location"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlPrfJobLocation" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblJobLevel" runat="server" class="control-label" Text="Looking for"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlJobLevel" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="Entry Level">Entry Level</asp:ListItem>
                                    <asp:ListItem Value="Mid Level">Mid Level</asp:ListItem>
                                    <asp:ListItem Value="Top Level">Top Level</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblAvailableType" runat="server" class="control-label" Text="Available for"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlAvailableType" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="Full Time">Full Time</asp:ListItem>
                                    <asp:ListItem Value="Part Time">Part Time</asp:ListItem>
                                    <asp:ListItem Value="Contractual">Contractual</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblPrfJobCategory" runat="server" class="control-label required-field"
                                    Text="Prefered Job Category"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlJobCategory" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblPrfOrganizationType" runat="server" class="control-label required-field"
                                    Text="Prefered Organization Type"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlOrganizationType" runat="server" CssClass="form-control"
                                    TabIndex="2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblExtraActivities" runat="server" class="control-label" Text="Extra Curriculum Activities"></asp:Label>
                            </div>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtExtraActivities" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblCareerSummary" runat="server" class="control-label" Text="Career Summary"></asp:Label>
                            </div>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtCareerSummary" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3">
            <div id="EducationInformation" class="panel panel-default">
                <div class="panel-heading">
                    Education Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblExamName" runat="server" class="control-label required-field" Text="Degree"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtExamName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblInstituteName" runat="server" class="control-label required-field"
                                    Text="Institute Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtInstituteName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSubjectName" runat="server" class="control-label" Text="Subject Name (Major)"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSubjectName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPassYear" runat="server" class="control-label required-field" Text="Passing Year"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassYear" CssClass="quantity form-control" runat="server" onKeyPress="if(this.value.length==4) return false;"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPassClass" runat="server" class="control-label" Text="Result (Class/GPA)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassClass" CssClass="quantitydecimal form-control" runat="server" ></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnEmpEducation" runat="server" Text="Add" CssClass="btn btn-primary"
                                    OnClick="btnEmpEducation_Click" />
                                <asp:Label ID="hfEducationId" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gvEmpEducation" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" PageSize="5" OnRowCommand="gvEmpEducation_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("EducationId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ExamName" HeaderText="Exam Name" ItemStyle-Width="51%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PassYear" HeaderText="Passing Year" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PassClass" HeaderText="Result (Class/GPA)" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("EducationId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" OnClientClick="return confirm('Do you want to edit?');"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("EducationId") %>'
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
        <div id="tab-4">
            <div id="ExperienceInformation" class="panel panel-default">
                <div class="panel-heading">
                    Experience Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCompanyName" runat="server" class="control-label required-field"
                                    Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCompanyName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCompanyUrl" runat="server" class="control-label" Text="Company Website"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCompanyUrl" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblJoinDate" runat="server" class="control-label required-field" Text="Join Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtJoinDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblJoinDesignation" runat="server" class="control-label" Text="Join Designation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtJoinDesignation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLeaveDate" runat="server" class="control-label" Text="Leave Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLeaveDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLeaveDesignation" runat="server" class="control-label" Text="Leave Designation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLeaveDesignation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAchievements" runat="server" class="control-label" Text="Achievements"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAchievements" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpExperience" runat="server" Text="Add" CssClass="btn btn-primary"
                                    OnClick="btnEmpExperience_Click" />
                                <asp:Label ID="hfExperienceId" runat="server" Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gvExperience" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                                OnRowCommand="gvExperience_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("ExperienceId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CompanyName" HeaderText="Company" ItemStyle-Width="51%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShowJoinDate" HeaderText="Join Date" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShowLeaveDate" HeaderText="Leave Date" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("ExperienceId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" OnClientClick="return confirm('Do you want to edit?');" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("ExperienceId") %>'
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
            <div id="TrainingInfo" class="panel panel-default">
                <div class="panel-heading">
                    Training Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTrainingTitle" runat="server" class="control-label required-field"
                                    Text="Training Title"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtTrainingTitle" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTopic" runat="server" class="control-label" Text="Topic Covered"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtTopic" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblInstitute" runat="server" class="control-label required-field"
                                    Text="Institute Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtInstitute" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCountry" runat="server" class="control-label" Text="Country"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <%--<asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>--%>
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLocation" runat="server" class="control-label" Text="Location"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLocation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTrainingYear" runat="server" class="control-label required-field"
                                    Text="Training Year"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTrainingYear" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDuration" runat="server" class="control-label required-field" Text="Duration"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlDuration" runat="server" CssClass="form-control"
                                    TabIndex="4">
                                    <asp:ListItem>Days</asp:ListItem>
                                    <asp:ListItem>Months</asp:ListItem>
                                    <asp:ListItem>Years</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnEmpCareerTraining" runat="server" Text="Add" CssClass="btn btn-primary"
                                    OnClick="btnEmpCareerTraining_Click" />
                                <asp:Label ID="hfCareerTrainingId" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gvEmpCareerTraining" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" PageSize="5" OnRowCommand="gvEmpCareerTraining_RowCommand"
                                CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("CareerTrainingId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TrainingTitle" HeaderText="Training Title" ItemStyle-Width="40%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Institute" HeaderText="Institute" ItemStyle-Width="40%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TrainingYear" HeaderText="Training Year" ItemStyle-Width="5%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("CareerTrainingId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" OnClientClick="return confirm('Do you want to edit?');" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("CareerTrainingId") %>'
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
        <div id="tab-6">
            <div id="LanguageInfo" class="panel panel-default">
                <div class="panel-heading">
                    Language Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLanguage" runat="server" class="control-label required-field" Text="Language"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLanguage" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblReading" runat="server" class="control-label required-field" Text="Reading"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReading" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblWriting" runat="server" class="control-label required-field" Text="Writing"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlWriting" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSpeaking" runat="server" class="control-label required-field" Text="Speaking"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSpeaking" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnEmpLanguage" runat="server" Text="Add" CssClass="btn btn-primary"
                                    OnClick="btnEmpLanguage_Click" />
                                <asp:Label ID="hfLanguageId" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gvEmpLanguage" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                                OnRowCommand="gvEmpLanguage_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("LanguageId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Language" HeaderText="Language" ItemStyle-Width="40%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ReadingLevel" HeaderText="Reading" ItemStyle-Width="15%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="WritingLevel" HeaderText="Writing" ItemStyle-Width="15%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SpeakingLevel" HeaderText="Speaking" ItemStyle-Width="15%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("LanguageId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" OnClientClick="return confirm('Do you want to edit?');" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("LanguageId") %>'
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
            <div id="Reference" class="panel panel-default">
                <div class="panel-heading">
                    Reference Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblOrganization" runat="server" class="control-label required-field"
                                    Text="Organization"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtOrganization" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDesignation" runat="server" class="control-label required-field"
                                    Text="Designation"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDesignation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAddress" runat="server" class="control-label" Text="Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAddress" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmail" runat="server" class="control-label" Text="Email"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMobile" runat="server" class="control-label" Text="Mobile"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMobile" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblRelation" runat="server" class="control-label" Text="Relation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRelation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnEmpReference" runat="server" Text="Add" CssClass="btn btn-primary"
                                    OnClick="btnEmpReference_Click" />
                                <asp:Label ID="hfReferenceId" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gvEmpReference" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" PageSize="5" OnRowCommand="gvEmpReference_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("ReferenceId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="40%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Organization" HeaderText="Organization" ItemStyle-Width="40%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Designation" HeaderText="Designation" ItemStyle-Width="5%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("ReferenceId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" OnClientClick="return confirm('Do you want to edit?');" AlternateText="Edit"
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
        <div id="tab-8">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Applicant Documents
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:HiddenField ID="RandomEmpId" runat="server" />
                            <asp:HiddenField ID="tempEmpId" runat="server" />
                            <div class="col-md-2">
                                <asp:Label ID="lblSignatureImage" runat="server" class="control-label" Text="Applicant Signature"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnSignatureUp" type="button" onclick="javascript: return LoadSignatureUploader();"
                                    class="TransactionalButton btn btn-primary" value="Upload Signature" />
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDocumentImage" runat="server" class="control-label" Text="Applicant Picture"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnDocumentUp" type="button" onclick="javascript: return LoadDocumentUploader();"
                                    class="TransactionalButton btn btn-primary" value="Upload Picture" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                                <div id="DocDiv" style="width: 150px; height: 150px">
                                </div>
                            </div>
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                                <div id="SigDiv" style="width: 150px; height: 150px">
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Attachment</label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Others Document..." />
                            </div>
                        </div>
                        
                        <div class="block-body collapse in">

                            <div class="form-group">
                                <div id="DocumentInfo" runat="server" class="col-md-12">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="block-body collapse in" style="display:none">
                        <asp:GridView ID="gvEmployeeDocument" Width="100%" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                            ForeColor="#333333" PageSize="20" OnRowCommand="gvEmployeeDocument_RowCommand"
                            CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("DocumentId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Document Name">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:ImageField DataImageUrlField="ImageUrl" HeaderText="Image" ControlStyle-Width="40px"
                                    ControlStyle-Height="40px">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:ImageField>
                                <%--<asp:BoundField DataField="Path" HeaderText="Display" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>   --%>
                                <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("DocumentId") %>'
                                            CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to delete?');"
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
        <div id="tab-9">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmployeeCode" runat="server" class="control-label" Text="Applicant Id"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmployeeCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblEmployeeName" runat="server" class="control-label" Text="Applicant Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmployeeName" CssClass="form-control" runat="server"></asp:TextBox>
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
                <div id="SearchOutput">
                    <div class="panel-body">
                        <table id='gvEmployee' class="table table-bordered table-condensed table-responsive"
                            width="100%">
                            <colgroup>
                                <col style="width: 50%;" />
                                <col style="width: 40%;" />
                                <%--<col style="width: 15%;" />
                                <col style="width: 15%;" />--%>
                                <col style="width: 10%;" />
                            </colgroup>
                            <thead>
                                <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                    <td>Name
                                    </td>
                                    <td>ID
                                    </td>
                                    <%--<td>
                                        Department
                                    </td>
                                    <td>
                                        Designation
                                    </td>--%>
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
    <div class="row" id="SubmitButtonDiv" style="padding-top: 10px;">
        <div class="col-md-12">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                OnClick="btnCancel_Click" />
        </div>
    </div>
    <%--<div id="popUpSignature" style="display: none">
        <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
            <cc1:ClientUploader ID="flashUploadSignature" runat="server" UploadPage="Upload.axd"
                OnUploadComplete="UploadComplete()" FileTypeDescription="Images" FileTypes=""
                UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
        </asp:Panel>
    </div>
    <div id="popUpDocument" style="display: none">
        <asp:Panel ID="Panel1" runat="server" Style="text-align: center;">
            <cc1:ClientUploader ID="flashUploadDocuments" runat="server" UploadPage="Upload.axd"
                OnUploadComplete="UploadComplete()" FileTypeDescription="Images" FileTypes=""
                UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
        </asp:Panel>
    </div>--%>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="popUpSignature" style="display: none;">
        <iframe id="Iframe1" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="popUpDocument" style="display: none;">
        <iframe id="Iframe2" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <%--<div id="popUpOthersDocument" style="display: none">
        <asp:Panel ID="Panel2" runat="server" Style="text-align: center;">
            <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
        </asp:Panel>
    </div>--%>
</asp:Content>
