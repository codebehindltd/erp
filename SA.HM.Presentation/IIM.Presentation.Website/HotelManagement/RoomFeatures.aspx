<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="RoomFeatures.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.RoomFeatures" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            $("#SearchPanel").hide();

            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            
            $("#ContentPlaceHolder1_btnSave").click(function () {
                
                var features = $("#ContentPlaceHolder1_txtFeatures").val();               
                if(features=="")
                {                    
                    toastr.warning("Please Enter Any Valid Features");                    
                    return false;
                }
            });

            $("#gvTblFeaturesInfo").delegate("td > img.FeatureDelete", "click", function () {
                var answer = confirm("Do you want to delete this it?")
               
                if (answer) {
                    var featuresId = $.trim($(this).parent().parent().find("td:eq(4)").text());
                    var params = JSON.stringify({ deleteId: featuresId });
                    var $row = $(this).parent().parent();
                    
                    $.ajax({
                        type: "POST",
                        url: "/HotelManagement/RoomFeatures.aspx/DeleteData",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            CommonHelper.AlertMessage(data.d.AlertMessage);
                            $row.remove();
                            $("#myTabs").tabs('load', 1);
                        },
                        error: function (error) {

                        }
                    });
                }
            });


        });

        $(function () {
            $("#myTabs").tabs();
        });

        // for Search result with Paging---------------------------
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#gvTblFeaturesInfo tbody tr").length;

            var feature = $("#<%=txtSFeatures.ClientID %>").val();
            var activeStat = $("#<%=ddlSActiveStatus.ClientID %>").val();

            if (activeStat == 1)
                activeStat = true;
            else
                activeStat = false;

            PageMethods.SearchFeaturesAndLoadGridInformation(feature, activeStat, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadSearchSucceeded, OnLoadSearchFailed);
            return false;
        }

        function OnLoadSearchSucceeded(result) {
            $("#gvTblFeaturesInfo tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if(result.GridData == "")
            {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvTblFeaturesInfo tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            //table row color 
            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvTblFeaturesInfo tbody tr").length;
                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:60%; cursor:pointer;\">" + gridObject.Features + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.ActiveStatusSt + "</td>";

                //Edit permission enable/disable------------------
                if ($("#<%=hfSavePermission.ClientID %>").val() == "1") {
                    tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.Id + "')\" alt='Edit Information' border='0' /></td>";
                }
                else {
                    tr += "<td align='right' style=\"width:10%; cursor:pointer;\">&nbsp;</td>";
                }

                //DeletePermission
                if ($("#<%=hfDeletePermission.ClientID %>").val() == "1")
                {
                    tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/delete.png' class= 'FeatureDelete'  alt='Delete Information' border='0' /></td>";
                }
                else
                {
                    tr += "<td align='right' style=\"width:10%; cursor:pointer;\">&nbsp;</td>";
                }
                tr += "<td align='right' style=\"width:8%; display:none;\">" + gridObject.Id + "</td>";

                tr += "</tr>"

                $("#gvTblFeaturesInfo tbody ").append(tr);
                tr = "";
            });
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }

        function OnLoadSearchFailed(error) {
            toastr.error(error.get_message());
        }

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtFeatures.ClientID %>").val('');
            $("#<%=txtDescription.ClientID %>").val('');
            $("#<%=ddlActiveStatus.ClientID %>").val(0);
            $("#<%=hfFeaturesId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        //For FillForm------------------------- 
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {
            $("#<%=txtFeatures.ClientID %>").val(result.Features);
            $("#<%=txtDescription.ClientID %>").val(result.Description);
            $("#<%=ddlActiveStatus.ClientID %>").val(result.ActiveStat == true ? 1 : 0);
            $("#<%=hfFeaturesId.ClientID %>").val(result.Id);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#EntryPanel').show("slow");
            return false;
        }
        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
        }

        //For Edit------------------------- 
        function PerformEditAction(featuresId) {
            PageMethods.LoadEdiInfo(featuresId, OnLoadEditSucceed, OnLoadEditFailed);
            return false;
        }
        function OnLoadEditSucceed(result) {
             $("#<%=txtFeatures.ClientID %>").val(result.Features);
            $("#<%=txtDescription.ClientID %>").val(result.Description);
            $("#<%=ddlActiveStatus.ClientID %>").val(result.ActiveStatus == true ? 1 : 0);
            $("#<%=hfFeaturesId.ClientID %>").val(result.Id);
            $("#<%=btnSave.ClientID %>").val("Update");
            $("#myTabs").tabs({ active: 0 });
            return false;
        }
        function OnLoadEditFailed(error) {
            toastr.error(error.get_message());
        }
        

        //For Delete-------------------------        
        //function PerformDeleteAction(actionId, rowIndex) {
        //    var answer = confirm("Do you want to delete this record?")
        //    if (answer) {
        //        PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
        //    }
        //    return false;
        //}

        //function OnDeleteObjectSucceeded(result) {
        //    CommonHelper.AlertMessage(result.AlertMessage);
        //}
        //function OnDeleteObjectFailed(error) {
        //    //alert(error.get_message());
        //    toastr.error(error);
        //}
        
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Room Features Setup</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Room Features</a></li>
        </ul>
        <div id="tab-1">
             <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Features Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="hfFocusTabControl" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID ="hfFeaturesId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hfSavePermission" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hfDeletePermission" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblFeatures" runat="server" class="control-label required-field" Text="Features"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtFeatures" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TabIndex="2" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStatus" runat="server" CssClass="form-control" TabIndex="3">
                                    <asp:ListItem Value="1">Active</asp:ListItem>
                                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="4" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                     OnClick="btnSave_Click"/>
                                <asp:Button ID="btnClear" runat="server" TabIndex="5" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
             <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Features Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSFeatures" runat="server" class="control-label" Text="Features"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSFeatures" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStatus" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="1">Active</asp:ListItem>
                                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                                </asp:DropDownList>
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
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <table id='gvTblFeaturesInfo' class="table table-bordered table-condensed table-responsive" width="100%">
                        <colgroup>
                            <col style="width: 50%;" />
                            <col style="width: 20%;" />
                            <col style="width: 15%;" />
                            <col style="width: 15%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                                <td>
                                    Features
                                </td>
                                <td>
                                    Status
                                </td>
                                <td style="text-align: right;">
                                    Edit
                                </td>
                                <td style="text-align: right;">
                                    Delete
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
</asp:Content>
