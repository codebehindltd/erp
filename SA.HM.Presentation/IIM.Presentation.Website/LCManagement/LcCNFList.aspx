<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="LcCNFList.aspx.cs" Inherits="HotelManagement.Presentation.Website.LCManagement.LcCNFList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            CommonHelper.ApplyIntigerValidation();
            GridPaging(1, 1);
        });
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadCNFSearch(pageNumber, IsCurrentOrPreviousPage);
        }
        function LoadCNFSearch(pageNumber, IsCurrentOrPreviousPage) {
             var name = $("#<%=txtNameSrc.ClientID %>").val();
            var code = $("#<%=txtCodeSrc.ClientID %>").val();
            var email = $("#<%=txtEmailSrc.ClientID %>").val();
            var phone = $("#<%=txtPhoneSrc.ClientID %>").val();
            var gridRecordsCount = $("#CNFTable tbody tr").length;
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../LCManagement/LcCNFList.aspx/LoadCNFSearch',

                data: "{'name':'" + name.trim() + "', 'code':'" + code.trim() + "', 'email':'" + email.trim() + "','phone':'" + phone.trim() + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {

                    LoadTable(data);
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }
        function LoadTable(searchData) {
            var rowLength = $("#CNFTable tbody tr").length;
            var dataLength = searchData.length;
            $("#CNFTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            var i = 0;

            if (searchData.d.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"5\" >No Data Found</td> </tr>";
                $("#CNFTable tbody ").append(emptyTr);
                return false;
            }

            $.each(searchData.d.GridData, function (count, gridObject) {
                var tr = "";
                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }
                tr += "<td style='width:20%;'>" + gridObject.Name + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.Code + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.ContactEmail + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.ContactPhone + "</td>";

                tr += "<td style='width:20%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.SupplierId + ",\'" + gridObject.Name + '\');"' + "title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteCNF(" + gridObject.SupplierId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";

                tr += "<td style='display:none'>" + gridObject.SupplierId + "</td>";
                tr += "</tr>";

                $("#CNFTable tbody").append(tr);

                tr = "";
                i++;
            });
            //PerformCancleAction();
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.NextButton);
            return false;
        }
        function DeleteCNF(id) {
            if (!confirm("Do you want to delete?")) {
                return;
            }
            PageMethods.DeleteCNF(id, OnDeleteSucceed, OnDeleteFailed);
            return false;
        }
        function OnDeleteSucceed(result) {
            LoadCNFSearch(1, 1);
            CommonHelper.AlertMessage(result.AlertMessage);
            return false;
        }
        function OnDeleteFailed(error) {
            toastr.error(error);
            return false;
        }
        function FillFormEdit(id, name) {

            if (!confirm("Do you want to edit ?")) {
                return false;
            }
             $("#AddNewContactContaiiner").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Update - " + name,
                show: 'slide'
            });
            CommonHelper.SpinnerOpen();
            PageMethods.FillForm(id, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {
          
            PerformClearAction();
            $("#<%=hfSupplierId.ClientID %>").val(result.SupplierId);
            $("#<%=txtAddress.ClientID %>").val(result.Address);
            $("#<%=txtCode.ClientID %>").val(result.Code);
            $("#<%=txtContactPerson.ClientID %>").val(result.ContactPerson);
            $("#<%=txtContactPhone.ClientID %>").val(result.ContactPhone);
            $("#<%=txtContactEmail.ClientID %>").val(result.ContactEmail);
            $("#<%=txtEmail.ClientID %>").val(result.Email);
            $("#<%=txtFax.ClientID %>").val(result.Fax);
            $("#<%=txtName.ClientID %>").val(result.Name);
            $("#<%=txtPhone.ClientID %>").val(result.Phone);
            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
            $("#<%=txtWebAddress.ClientID %>").val(result.WebAddress);

            $("#<%=btnSaveClose.ClientID %>").val("Update");
            CommonHelper.SpinnerClose();
        }
        function OnFillFormFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error);
            return false;
        }
        function CreateNewCNF() {
            PerformClearAction();
            $("#<%=btnSaveClose.ClientID %>").val("Save");

            $("#AddNewContactContaiiner").dialog({
                autoOpen: true,
                modal: true,
                width: 1000,
                height: 500,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Create New CNF",
                show: 'slide'
            });
            return false;
        }
        function SaveAndClose() {
            var address = $("#<%=txtAddress.ClientID %>").val();
            var code = $("#<%=txtCode.ClientID %>").val();
            var contactPerson = $("#<%=txtContactPerson.ClientID %>").val();
            var contactPhone  = $("#<%=txtContactPhone.ClientID %>").val();
            var contactEmail  = $("#<%=txtContactEmail.ClientID %>").val();
            var email = $("#<%=txtEmail.ClientID %>").val();
            var fax = $("#<%=txtFax.ClientID %>").val();
            var name = $("#<%=txtName.ClientID %>").val();
            var phone = $("#<%=txtPhone.ClientID %>").val();
            var remarks = $("#<%=txtRemarks.ClientID %>").val();
            var webAddress = $("#<%=txtWebAddress.ClientID %>").val();
            var hfSupplierId = $("#<%=hfSupplierId.ClientID %>").val();
            if (name == "") {
                toastr.warning("Please insert Name");
                $("#<%=txtName.ClientID %>").focus();
                return false;
            }
            else if (contactPhone == "") {
                toastr.warning("Please insert Contact Phone");
                $("#<%=txtContactPhone.ClientID %>").focus();
                return false;
            }
            else if (contactEmail == "") {
                toastr.warning("Please insert Contact Email");
                $("#<%=txtContactEmail.ClientID %>").focus();
                return false;
            }
            var LCCnfInfoBO = {
                SupplierId : hfSupplierId,
                Name: name,
                Code: code,
                Address: address,
                Phone: phone,
                Fax: fax,
                Email: email,
                WebAddress: webAddress,
                ContactPerson: contactPerson,
                ContactEmail: contactEmail,
                ContactPhone: contactPhone,
                Remarks: remarks
            }
           return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../LCManagement/LcCNFList.aspx/SaveUpdateCNF',
                data: JSON.stringify({ infoBO: LCCnfInfoBO}),
                dataType: "json",
                async: false,
                success: function (data) {
                    OnSaveCNFSucceed(data.d);
                },
                error: function (result) {
                    OnSaveCNFFailed(result.d);
                }
            });
        }
        function OnSaveCNFSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.SpinnerClose();
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                $("#AddNewContactContaiiner").dialog('close');
                GridPaging(1, 1);
            }
            return false;
            //PerformClearAction();

        }
        function OnSaveCNFFailed(error) {
            CommonHelper.SpinnerClose();
            CommonHelper.AlertMessage(error.AlertMessage);
            return false;
        }
        function PerformClearAction() {
            $("#<%=hfSupplierId.ClientID %>").val("0");
            $("#<%=txtAddress.ClientID %>").val("");
            $("#<%=txtCode.ClientID %>").val("");
            $("#<%=txtContactPerson.ClientID %>").val("");
            $("#<%=txtContactPhone.ClientID %>").val("");
            $("#<%=txtContactEmail.ClientID %>").val("");
            $("#<%=txtEmail.ClientID %>").val("");
            $("#<%=txtFax.ClientID %>").val("");
            $("#<%=txtName.ClientID %>").val("");
            $("#<%=txtPhone.ClientID %>").val("");
            $("#<%=txtRemarks.ClientID %>").val("");
            $("#<%=txtWebAddress.ClientID %>").val("");
        }
    </script>
    <asp:HiddenField ID="hfSupplierId" runat="server" Value="0"></asp:HiddenField>
    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            CNF List
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label " Text="CNF Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtNameSrc" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label  runat="server" class="control-label" Text="Code"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCodeSrc" runat="server" CssClass="form-control">
                        </asp:TextBox>
                    </div>
                    <div>
                        <asp:Label runat="server" class="control-label col-md-2" Text="Email"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmailSrc" runat="server" CssClass="form-control">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Phone"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPhoneSrc" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                &nbsp;
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="javascript: return LoadCNFSearch(1,1);"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                        <asp:Button ID="btnAdd" runat="server" Text="New CNF" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNewCNF();" TabIndex="6" />
                    </div>
                </div>
            </div>
        </div>
        <div id="SearchPanel" class="panel panel-default">
            <div class="panel-heading">
                Search Information
            </div>
            <div class="panel-body">
                <div class="form-group" id="CNFTableContainer" style="overflow: scroll;">
                    <table class="table table-bordered table-condensed table-responsive" id="CNFTable"
                        style="width: 100%;">
                        <thead>
                            <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                <th style="width: 20%;">Name
                                </th>
                                <th style="width: 20%;">Code
                                </th>
                                <th style="width: 20%;">Email
                                </th>
                                <th style="width: 20%;">Phone
                                </th>
                                <th style="width: 20%;">Action
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
    <div id="AddNewContactContaiiner" style="display:none">
        <div id="AddPanel" class="panel panel-default">
            <%--<div class="panel-heading">
                New CNF
            </div>--%>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Name</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label ">Code</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtCode" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                       <div class="col-md-2">
                            <label class="control-label">Phone</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtPhone" CssClass="form-control quantity" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                        <label class="control-label">Email</label>
                            </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                        <label class="control-label">Fax</label>
                            </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                        <label class="control-label  ">Address</label>
                            </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtAddress" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                        <label class="control-label ">Contact Person</label>
                            </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                        <label class="control-label required-field">Contact Phone</label>
                            </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtContactPhone" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                        <label class="control-label required-field">Contact Email</label>
                            </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtContactEmail" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                        <label class="control-label ">Web Address</label>
                            </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtWebAddress" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                        <label class="control-label">Remarks</label>
                            </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSaveClose" runat="server" Text="Save" OnClientClick="javascript:return SaveAndClose();"
                                CssClass="TransactionalButton btn btn-primary btn-sm" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return PerformClearAction();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
