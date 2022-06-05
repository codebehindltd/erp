<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="MembershipPublicReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.Membership.Reports.MembershipPublicReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnSearch").click(function () {
                $("#SearchOutput").show('slow');
                GridPaging(1, 1);
            });
        });
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#MembersGrid tbody tr").length;
            var typeId = $("#<%=ddlMemberType.ClientID %>").val();
            var MembershipNo = $("#<%=txtMembershipNo.ClientID %>").val();
            debugger;
            PageMethods.SearchNLoadMemberInformation(typeId, MembershipNo, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearechSucceed, OnSearchFailed);
            return false;
        }
        function OnSearechSucceed(result) {
            var memberList = new Array();
            memberList = result;

            $("#MembersGrid tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"8\" >No Data Found</td> </tr>";
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
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.TypeName + "</td>";
                tr += "<td align='left' style=\"width:20%; \">" + gridObject.FullName + "</td>";
                tr += "<td align='left' style=\"width:10%; \">" + gridObject.MembershipNumber + "</td>";
                tr += "<td align='left' style=\"width:10%; \">" + gridObject.Country + "</td>";
                tr += "<td align='left' style=\"width:10%; \">" + gridObject.MobileNumber + "</td>";


                tr += "<td align='left' style=\"width:10%; \">" + gridObject.Occupation + "</td>";
                tr += "<td align='left' style=\"width:10%; \">" + gridObject.PersonalEmail + "</td>";

                tr += "<td align='left' style=\"display:none; \">" + gridObject.MemberId + "</td>";
                tr += "<td align='center' style=\"width:10%; cursor:pointer;\"><img src='../../Images/ReportDocument.png' onClick= \"javascript:return ShowReport('" + gridObject.MemberId + "')\" alt='Report' border='0' /></td>";

                tr += "</tr>";

                $("#MembersGrid tbody").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            $("#<%=ddlMemberType.ClientID %>").val("0");
            $("#<%=txtMembershipNo.ClientID %>").val("");


            return false;
        }
        function OnSearchFailed(error) {

        }
        function ShowReport(memberId) {
            var iframeid = 'printDoc';
            var url = "ReportOnlineMembership.aspx?Id=" + memberId;
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
    <div>
        <div id="displayBill" style="display: none;">
            <iframe id="printDoc" name="printDoc" width="850" height="820" style="overflow: hidden;"></iframe>
            <div id="bottomPrint">
            </div>
        </div>
        <div class="panel panel-default" id="SearchInput">
            <div class="panel-heading">
                Membership Report Search
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
                            <asp:TextBox ID="txtMembershipNo" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
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
                            <th style="width: 20%; text-align: center;">Member Type
                            </th>
                            <th style="width: 20%; text-align: center;">Name
                            </th>
                            <th style="width: 10%; text-align: center;">Membership Number
                            </th>
                            <th style="width: 10%; text-align: center;">Country
                            </th>
                            <th style="width: 10%; text-align: center;">Mobile Number
                            </th>
                            <th style="width: 10%; text-align: center;">Occupation
                            </th>
                            <th style="width: 10%; text-align: center;">Email
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
                    <div class="text-center" id="GridPagingContainer">
                        <ul class="pagination">
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
