<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="DeviceMemberMapping.aspx.cs" Inherits="HotelManagement.Presentation.Website.Membership.DeviceMemberMapping" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        "use strict";
        var mappingMemberList = null;
        var MemberList = null;
        $(document).ready(function () {
            $("#<%=ddlMemberType.ClientID%>").change(function () {
                let MemberDepartmentId = $("#<%=ddlMemberType.ClientID%>").val();
                LoadMemberMappingTable(MemberDepartmentId);
            });

            $("#<%=ddlMappingMemberDepartment.ClientID %>").change(function () {
                let MemberDepartmentId = parseInt($("#<%=ddlMappingMemberDepartment.ClientID%>").val());
                LoadMappingMemberDropdown(MemberDepartmentId);
            });

        });


        function LoadMemberMappingTable(MemberDepartmentId) {
            PageMethods.GetMemberListByDepartmentId(MemberDepartmentId, OnSuccesMemberLoad, OnFailedMemberLoad);
        }
        function OnSuccesMemberLoad(result) {
            $("#tbMemberMapping tbody tr").remove();
            $("#<%=ddlMappingMemberDepartment.ClientID %>").val("0");

            let tr = "";
            let options = "", totalRow = 0;

            if (result.Members.length == 0) {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Member Found</td> </tr>";
                $("#tbMemberMapping tbody ").append(emptyTr);
                return false;
            }
            options = "<select class='form-control' id=\"\">";
            options += "<option value=\"" + "0" + "\">" + "---Please Select---" + "</option>";
            options += " </select>";
            
            mappingMemberList = result.MappingMembers;
            MemberList = result.Members;

            $.each(result.Members, function (count, gridObject) {

                totalRow = $("#tbMemberMapping tbody tr").length;
                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td style=\"width:30%;\">" + gridObject.FullName + "</td>";
                tr += "<td style=\"width:30%;\">" + options + "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.MemberId + "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.AttendanceDeviceMemberId + "</td>";

                tr += "</tr>"

                $("#tbMemberMapping tbody").append(tr);
                tr = "";
            });
        }
        function OnFailedMemberLoad(error) {
            let options = "";
            options += "<option value=\"" + "0" + "\">" + "---Please Select---" + "</option>";
        }

        function LoadMappingMemberDropdown(MemberDepartmentId) {
            var list = _.where(mappingMemberList, { DepartmentId: MemberDepartmentId });
            let options = "";
            options += "<option value=\"" + "0" + "\">" + "---Please Select---" + "</option>";
            $.each(list, function (count, list) {
                options += "<option value=\"" + list.MemberId + "\">" + list.FullName + "</option>";
            });
            $("#tbMemberMapping tbody tr").each(function () {
                $(this).find('td:eq(1)').find('select').html(options);
                var flag = $(this).find('td:eq(3)').text();
                //check Member had mappingId
                if (flag != "0") {
                    $(this).find('td:eq(1)').find('select').val(parseInt(flag));
                    //if mapping department changed then select initial value
                    if (flag != $(this).find('td:eq(1)').find('select').val())
                        $(this).find('td:eq(1)').find('select').val("0");
                }
                else {
                    $(this).find('td:eq(1)').find('select').val("0");
                }
            });
        }

        function UpdateMapping() {
            var MemberList = new Array();
            var emp, mapEmp, tag, checkDuplicate, checkMapped;
            var currentMappingId, selectedMappingId;
            //check if department is not selected
            if ($("#<%=ddlMemberType.ClientID%>").val() == 0) {
                var row = $("#tbMemberMapping tbody tr").length;
                if (row < 0) {
                    toastr.warning("Select Department");
                    return false;
                }

            }
            //check if mapping department is not selected
            if ($("#<%=ddlMappingMemberDepartment.ClientID%>").val() == 0) {
                toastr.warning("Select Mapping Department");
                return false;
            }
            $("#tbMemberMapping tbody tr").each(function () {

                currentMappingId = $(this).find('td:eq(3)').text();
                selectedMappingId = $(this).find('td:eq(1)').find('select').val();
                
                
                    //check if mapping id is changed or not
                if (currentMappingId != selectedMappingId)
                {    
                    if (selectedMappingId != "0") {
                        //check duplicate in same click event
                        checkDuplicate = _.findWhere(MemberList, { MappingMemberId: parseInt(selectedMappingId) });
                        //check if mapping Member is already mapped or not
                        checkMapped = _.findWhere(MemberList, { AttendanceDeviceMemberId: parseInt(selectedMappingId) });
                    }
                        
                    if (checkDuplicate != null) {
                        tag = $(this).find('td:eq(1)').find('select option:selected').text();
                        toastr.warning(tag + " is selected for multiple Member.");
                        $(this).find('td:eq(1)').find('select').val("0").focus();
                    }
                    if (checkMapped != null) {
                        tag = $(this).find('td:eq(1)').find('select option:selected').text();
                        toastr.warning(tag + " is already Mapped with " + checkMapped.DisplayName + ".");
                        $(this).find('td:eq(1)').find('select').val("0").focus();
                    }
                    else {
                        emp = parseInt($(this).find('td:eq(2)').text());
                        mapEmp = parseInt($(this).find('td:eq(1)').find('select').val());
                        MemberList.push({
                            MemberId: emp,
                            MappingMemberId: mapEmp
                        });
                    }
                }
            });
            if (MemberList.length > 0) {
                PageMethods.UpdateMapping(MemberList, OnSuccessUpdating, OnErrorUpdating);
            }
            else {
                toastr.warning("Nothing to Update.");
            }
            return false;
        }

        function Clear() {
            $("#<%=ddlMemberType.ClientID%>").val("0");
            $("#<%=ddlMappingMemberDepartment.ClientID%>").val("0");
            $("#tbMemberMapping tbody tr").remove();
        }
        function OnSuccessUpdating(returninfo) {

            $("#tbMemberMapping tbody tr").each(function () {

                //update left Member colmn in client side
                let empId = parseInt($(this).find('td:eq(2)').text());
                for (var i = 0; i < returninfo.ObjectList.length; i++) {
                    if (empId == returninfo.ObjectList[i].MemberId)
                        $(this).find('td:eq(3)').text(returninfo.ObjectList[i].MappingMemberId);
                }
            });

            //update left Member list colmn in client side
            for (var i = 0; i < returninfo.ObjectList.length; i++) {
                var match = _.find(MemberList, function (item) { return item.EmpId === returninfo.ObjectList[i].MemberId });

                if (match) {
                    match.AttendanceDeviceMemberId = returninfo.ObjectList[i].MappingMemberId;
                }
            }
            if (returninfo.IsSuccess)
                toastr.success(returninfo.AlertMessage.Message);
            else
                toastr.error(returninfo.AlertMessage.Message);
        }
        function OnErrorUpdating(error) {
            toastr.error(error.get_message());
        }

    </script>
    <div id="EntryPanel" class="panel panel-default">
        <div class="form-horizontal">
            <div id="deviceMemberMappingDiv" class="panel panel-default">
                <div class="panel-heading">
                    Device Member Mapping
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label Text="Member Type" runat="server" class="control-label required-field"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlMemberType" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label Text="Mapping Department" runat="server" class="control-label required-field"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlMappingMemberDepartment" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <table id="tbMemberMapping" class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="background-color: #44545E">
                                <th style="width: 30%;">Display Name</th>
                                <th style="width: 30%;">Mapping Name</th>
                                <th style="display: none;">Id</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnUpdate" Text="Update" CssClass="TransactionalButton btn btn-primary btn-sm" runat="server" OnClientClick="return UpdateMapping(); " />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
