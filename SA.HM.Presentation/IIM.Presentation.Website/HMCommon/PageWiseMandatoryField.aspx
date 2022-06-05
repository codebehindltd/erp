<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="PageWiseMandatoryField.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.PageWiseMandatoryField" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlFormName").change(function () {
                var formId = parseInt($("#ContentPlaceHolder1_ddlFormName").val().trim());

                if (formId == 0) {
                    toastr.info("Please Select a User Form");
                    return false;
                }
                $("#FormWiseMenuAssign thead tr").find("th:eq(1)").find("input").prop("checked", false);

                PageMethods.GetFieldsByFormId(formId, OnLoadGetFieldsSucceed, OnLoadGetFieldsFailed);
                return false;

            });

            $("#chkMandatory").change(function () {
                if ($(this).is(":checked")) {
                    $("#FormWiseMenuAssign tbody tr").find("td:eq(1)").find("input").prop("checked", true);
                }
                else {
                    $("#FormWiseMenuAssign tbody tr").find("td:eq(1)").find("input").prop("checked", false);
                }
            });

        });


        function OnLoadGetFieldsSucceed(results) {


            $("#FormWiseMenuAssign tbody").html("");

            var isMandatory = "";
            var i = 0, fieldLength = results.length;
            var tr = "";

            for (i = 0; i < fieldLength; i++) {

                if (i % 2 == 0) {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }
                else {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }

                tr += "<td style=\"width: 50%;\">" +
                    results[i].FieldName +
                    "</td>";
                tr += "<td style=\"width: 50%; text-align:center;\">" +
                    "<input type='checkbox' " + (results[i].IsMandatory == true ? "checked='cheked'" : "") + "/>" +
                    "</td>";

                tr += "<td style=\"display:none;\">" + results[i].IsMandatory + "</td>";
                tr += "<td style=\"display:none;\">" + results[i].Id + "</td>";
                tr += "<td style=\"display:none;\">" + results[i].PageId + "</td>";

                tr += "</tr>";

                $("#FormWiseMenuAssign tbody").append(tr);
                tr = "";
            }
            
        }
        function OnLoadGetFieldsFailed() {
        }
        function UpdateMandatoryFields() {

            var formId = parseInt($("#ContentPlaceHolder1_ddlFormName").val().trim());

            if (formId == 0) {
                toastr.info("Please Select a User Form");
                return false;
            }


            var PageID = $("#ContentPlaceHolder1_ddlFormName").val();

            var FormWiseFieldSetupList = new Array();
            $("#FormWiseMenuAssign tbody tr").each(function () {

                isChecked = $(this).find("td:eq(1)").find("input").is(":checked");
                isCheckedPrevious = ($(this).find("td:eq(2)").text());

                if (("" + isChecked) != isCheckedPrevious) {

                    FormWiseFieldSetupList.push({
                        Id: ($(this).find("td:eq(3)").text()),
                        IsMandatory: isChecked
                    });

                }

            });
            if (FormWiseFieldSetupList.length > 0) {
                PageMethods.UpdateMandatoryFields(FormWiseFieldSetupList, UpdateMandatoryFieldsSucceed, UpdateMandatoryFieldsFailed);
            }
            return false;
        }
        function UpdateMandatoryFieldsSucceed(result) {

            $("#FormWiseMenuAssign thead tr").find("th:eq(1)").find("input").prop("checked", false);

            if (result.AlertMessage.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function UpdateMandatoryFieldsFailed() {

        }
    </script>
    <div id="MandatoryFieldEntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Mandatory Fields Entry
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Form Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFormName" runat="server" CssClass="form-control"
                            TabIndex="3">
                        </asp:DropDownList>
                    </div>
                </div>
                <br />
                <div class="form-group">
                    <table id="FormWiseMenuAssign" style="width: 100%;" class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                                <th style="width: 50%; vertical-align: middle">Field Name 
                                </th>
                                <th style="width: 50%; text-align: center;">Is Mandatory 
                                    <br />
                                    <input type="checkbox" id="chkMandatory" />
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="2"
                            CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript:return UpdateMandatoryFields()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
