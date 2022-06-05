<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ContactTransfer.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.ContactTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {

            $.extend($.expr[':'], {
                'containsi': function (elem, i, match, array) {
                    return (elem.textContent || elem.innerText || '').toLowerCase()
                        .indexOf((match[3] || "").toLowerCase()) >= 0;
                }
            });

            //CommonHelper.AutoSearchClientDataSource("txtCompanyName", "ContentPlaceHolder1_ddlCompany", "ContentPlaceHolder1_hfCompanyId");

            $("#txtCompanyName").autocomplete({

                source: function (request, response) {

                    var options = [];
                    var exceptCompany = $("#ContentPlaceHolder1_hfCurrentCompanyId").val();
                    if (exceptCompany == "0") {
                        toastr.warning("Select Contact Name First.");
                        $("#txtContactName").focus();
                        return false;
                    }
                    options = $("#ContentPlaceHolder1_ddlCompany").find('option:containsi(' + request.term + ')');
                    //options = options.filter(a => a.value != exceptCompany);

                    var searchData = $.map(options, function (m) {
                        if (m.value != exceptCompany)
                            return {
                                label: m.text,
                                value: m.value
                            };
                    });

                    if (searchData.length <= 0) { $("#ContentPlaceHolder1_hfCompanyId").val(''); }

                    response(searchData);
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
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfCompanyId").val(ui.item.value);
                }
            });

            $("#txtContactName").autocomplete({

                source: function (request, response) {

                    var options = [];
                    options = $("#ContentPlaceHolder1_ddlContact").find('option:containsi(' + request.term + ')');
                    var searchData = $.map(options, function (m) {
                        return {
                            label: m.text,
                            value: m.value
                        };
                    });

                    if (searchData.length <= 0) { $("#ContentPlaceHolder1_hfContactId").val(''); }

                    response(searchData);
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
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfContactId").val(ui.item.value);
                    GetCurrentContactInformation(ui.item.value);
                }
            });
        });

        function GetCurrentContactInformation(contactId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../SalesAndMarketing/ContactTransfer.aspx/GetCurrentContactInformation",
                dataType: "json",
                data: JSON.stringify({ contactId: parseInt(contactId) }),
                async: false,
                success: (data) => {
                    LoadCurrentContactInformation(data.d);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });

        }
        function LoadCurrentContactInformation(result) {
            $("#txtCurrentCompanyName").val(result.CompanyName);
            $("#txtCurrentTitle").val(result.JobTitle);
            $("#txtCurrentDepartment").val(result.Department);
            $("#txtCurrentMobile").val(result.MobileWork);
            $("#txtCurrentPhone").val(result.PhoneWork);
            $("#ContentPlaceHolder1_hfCurrentCompanyId").val(result.CompanyId);
            $("#txtCurrentEmail").val(result.EmailWork);
        }


        function TransferContact() {

            const contactId = parseInt($("#ContentPlaceHolder1_hfContactId").val());

            if (!contactId) {
                $("#txtContactName").focus();
                toastr.warning("Select Contact Name.");
                return false;
            }

            const previousCompanyId = parseInt($("#ContentPlaceHolder1_hfCurrentCompanyId").val());
            const previousCompany = $("#txtCurrentCompanyName").val();
            const transferredCompanyId = parseInt($("#ContentPlaceHolder1_hfCompanyId").val());

            if (!transferredCompanyId) {
                $("#txtCompanyName").focus();
                toastr.warning("Select Company Name.");
                return false;
            }

            const transferredCompany = $("#txtCompanyName").val();
            const title = $("#txtTitle").val().trim();
            const department = $("#txtDepartment").val().trim();
            const mobile = $("#txtMobile").val().trim();
            const phone = $("#txtPhone").val().trim();
            const email = $("#txtEmail").val().trim();

            const Contact = {
                ContactId: contactId,
                PreviousCompanyId: previousCompanyId,
                PreviousCompany: previousCompany,
                TransferredCompanyId: transferredCompanyId,
                TransferredCompany: transferredCompany,
                Title: title,
                Department: department,
                Mobile: mobile,
                Phone: phone,
                Email: email
            }

            PageMethods.TransferContact(Contact, OnTransferSuccess, OnTransferFail);
            return false;
        }

        function OnTransferSuccess(data) {
            CommonHelper.AlertMessage(data.AlertMessage);
            if (data.IsSuccess)
                Clear();
        }
        function OnTransferFail(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
        }

        function Clear() {
            $("#txtContactName").val('');
            $("#txtCurrentCompanyName").val('');
            $("#txtCurrentTitle").val('');
            $("#txtCurrentDepartment").val('');
            $("#txtCurrentMobile").val('');
            $("#txtCurrentPhone").val('');
            $("#ContentPlaceHolder1_hfCurrentCompanyId").val('0');
            $("#ContentPlaceHolder1_hfContactId").val('0');
            $("#ContentPlaceHolder1_hfCompanyId").val('0');
            $("#txtCurrentEmail").val('');
            $("#txtCompanyName").val('');
            $("#txtTitle").val('');
            $("#txtDepartment").val('');
            $("#txtMobile").val('');
            $("#txtPhone").val('');
            $("#ContentPlaceHolder1_hfCurrentCompanyId").val('0');
            $("#txtEmail").val('');

        }
    </script>
    <asp:HiddenField ID="hfContactId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCurrentCompanyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0" />
    <div class="panel panel-default">
        <div class="panel panel-default">
            <div class="panel-heading">
                Contact Transfer Information
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Contact Name</label>
                        </div>
                        <div class="col-md-4">
                            <div style="display: none;">
                                <asp:DropDownList ID="ddlContact" runat="server"></asp:DropDownList>
                            </div>
                            <input id="txtContactName" type="text" class="form-control" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        Current Company Information
                                    </div>
                                    <div class="panel-body">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class="control-label">Company Name</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <input disabled="disabled" id="txtCurrentCompanyName" type="text" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class="control-label">Title</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <input disabled="disabled" id="txtCurrentTitle" type="text" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class="control-label">Department</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <input disabled="disabled" id="txtCurrentDepartment" type="text" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class="control-label">Mobile (Work)</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <input disabled="disabled" id="txtCurrentMobile" type="text" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class="control-label">Phone (Work)</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <input disabled="disabled" id="txtCurrentPhone" type="text" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class="control-label">Email</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <input disabled="disabled" id="txtCurrentEmail" type="text" class="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        Transfer Company Information
                                    </div>
                                    <div class="panel-body">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class="control-label">Company Name</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <div style="display: none">
                                                        <asp:DropDownList ID="ddlCompany" runat="server"></asp:DropDownList>
                                                    </div>
                                                    <input id="txtCompanyName" type="text" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class="control-label">Title</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <input id="txtTitle" type="text" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class="control-label">Department</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <input id="txtDepartment" type="text" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class="control-label">Mobile (Work)</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <input id="txtMobile" type="text" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class="control-label">Phone (Work)</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <input id="txtPhone" type="text" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4">
                                                    <label class="control-label">Email</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <input id="txtEmail" type="text" class="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" id="btnTransfer" value="Transfer" class="btn btn-primary btn-sm" onclick="TransferContact()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
