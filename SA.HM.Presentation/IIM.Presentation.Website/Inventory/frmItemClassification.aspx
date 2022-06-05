<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmItemClassification.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmItemClassification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#myTabs").tabs();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#searchPanel").hide();
            $("#btnSearch").click(function () {
                $("#searchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("[id=ContentPlaceHolder1_gvCostCenterWithAccountHeadMap_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvCostCenterWithAccountHeadMap tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvCostCenterWithAccountHeadMap tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

            $("#gvClassificationInfo").delegate("td > img.invClassItemDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?");
                if (answer) {
                    var classificationId = $.trim($(this).parent().parent().find("td:eq(4)").text());
                    var params = JSON.stringify({ classificationId: classificationId });
                    $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/Inventory/frmItemClassification.aspx/DeleteData",
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

        function PerformEditAction(classificationId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            $("#<%=hfEditedItemId.ClientID%>").val(classificationId);
            $("#btnEditInServer").trigger("click");
            PageMethods.LoadClassificationDetailInformation(classificationId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            return false;
        }

        function OnLoadDetailObjectSucceeded(result) {
            $("#myTabs").tabs({ active: 0 });
            return false;
        }
        function OnLoadDetailObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#gvClassificationInfo tbody tr").length;

            var classificationName = $("#<%=txtSClassificationName.ClientID %>").val();
            var activeStat = $("#<%=ddlSActiveStat.ClientID %>").val();
            if (activeStat == 0)
                activeStat = true;
            else
                activeStat = false;

            PageMethods.SearchClassificationAndLoadGridInformation(classificationName, activeStat, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }

        function OnLoadObjectSucceeded(result) {
            $("#gvClassificationInfo tbody tr").remove();
            $("#GridPagingContainer ul").html("");
            if (result.GridData.length == 0) {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvClassificationInfo tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvClassificationInfo tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:64%; cursor:pointer;\">" + gridObject.ClassificationName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.ActiveStatus + "</td>";


                if ($("#<%=IsClassificationUpdatePermission.ClientID %>").val() == "1") {
                    tr += "<td align='right' style=\"width:8%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.ClassificationId + "')\" alt='Edit Information' border='0' /></td>";
                }
                else {
                    tr += "<td align='right' style=\"width:8%; cursor:pointer;\">&nbsp;</td>";
                }

                if ($("#<%=IsClassificationDeletePermission.ClientID %>").val() == "1") {
                    tr += "<td align='right' style=\"width:8%; cursor:pointer;\"><img src='../Images/delete.png' class= 'invClassItemDelete'  alt='Delete Information' border='0' /></td>";
                }
                else {
                    tr += "<td align='right' style=\"width:8%; cursor:pointer;\">&nbsp;</td>";
                }

                tr += "<td align='right' style=\"width:8%; display:none;\">" + gridObject.ClassificationId + "</td>";

                tr += "</tr>"

                $("#gvClassificationInfo tbody ").append(tr);
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

        function PerformClearActionOnButtonClick() {
            if (!confirm("Do you want to clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function PerformClearAction() {
            $("#<%=txtClassificationName.ClientID %>").val('');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=hfClassificationId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            $("#<%=gvCostCenterWithAccountHeadMap.ClientID%> tbody tr").each(function () {
                $("#ContentPlaceHolder1_gvCostCenterWithAccountHeadMap_ChkCreate").prop("checked", false);
                $(this).find('td').find('select').val(0);
                $(this).find('td').find('input').prop("checked", false);
            });
            return false;
        }
        function ValidationNPreprocess() {
            if($("#<%=txtClassificationName.ClientID%>").val() == "" )
            {
                toastr.warning("Please Provide Classfication Name")
                return false;
            }
          var status = true;
          $("#<%=gvCostCenterWithAccountHeadMap.ClientID%> tbody tr").each(function () {
              if ($(this).find("td:eq(0) > span").find("input").is(":checked") == true)
              {
                  if ($(this).find('td').find('select').val() == 0)
                      status = false;
              }    
            });
            if (!status)
                toastr.warning("Please Select Account Head");
            return status;
        }

       
    </script>
    <asp:HiddenField ID="hfEditedItemId" runat="server" />
    <div style="display: none;">
        <asp:Button ID="btnEditInServer" runat="server" Text="Button" ClientIDMode="Static" OnClick="btnEditInServer_Click"/>
    </div>
    <div id="myTabs">
        <ul id="tabPage">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a href="#tab-1">Classification</a> </li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a href="#tab-2">Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="entryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Classification
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField runat="server" ID="hfClassificationId" />
                                <asp:HiddenField ID="IsClassificationSavePermission" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="IsClassificationUpdatePermission" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="IsClassificationDeletePermission" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblClassificationName" runat="server" Text="Classification" class="control-label required-field"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtClassificationName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" Text="Status" class="control-label"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" Class="form-control" TabIndex="2">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="costCenterWithAccountHeadMapDiv" class="panel panel-default">
                            <div class="panel-heading">
                                Cost Center 
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvCostCenterWithAccountHeadMap" runat="server" Width="50%" AutoGenerateColumns="False" AllowPaging="True" GridLines="None" OnRowDataBound="gvcostCenterWithAccountHeadMap_RowDataBound"
                                    CellPadding="4" AllowSorting="True" ForeColor="#333333" CssClass="table table-bordered table-condensed table-responsive" PageSize="200">
                                    <RowStyle BackColor="#E3EAEB"  />
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="hfMappingId" runat="server"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="hfCostCenterId" Text='<%#Bind("CostCenterId")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate >
                                                <asp:Label ID="hfClassificationId" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="05%">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cost Center" ItemStyle-Width="50%">
                                            <ItemTemplate>
                                                <%--<asp:HiddenField ID="hfClassificationId" Value='<%#Eval("ClassificationId")%>' runat="server"/>--%>
                                                <asp:Label ID="lblCostCenterName" Text='<%# Bind("CostCenter") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"/>
                                            <ItemStyle  HorizontalAlign="Left"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acount Head" ItemStyle-Width="50%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlAccountHead" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" TabIndex="3" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm" OnClick="btnSave_Click" OnClientClick="javascript: return ValidationNPreprocess();" runat="server" />
                                <asp:Button ID="btnClear" TabIndex="4" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm" runat="server" OnClientClick="javascript: return PerformClearActionOnButtonClick(); "  />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="searchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Search
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSClassificationName" Text="Classification" class="control-label" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSClassificationName" Class="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
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
            <div id="searchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <table id="gvClassificationInfo" class="table table-bordered table-condensed table-responsive">
                        <colgroup>
                            <col style="width: 50%;" />
                            <col style="width: 20%;" />
                            <col style="width: 15%;" />
                            <col style="width: 15%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Classification Name
                                </td>
                                <td>Status
                                </td>
                                <td style="text-align: right;">Edit
                                </td>
                                <td style="text-align: right;">Delete
                                </td>
                            </tr>
                        </thead>
                        <tbody></tbody>
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
