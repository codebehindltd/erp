<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmDivisionList.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmDivisionList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Administrative & Security</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Division, District, Thana, Branch List</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlSetupType").change(function () {
                var type = $("#ContentPlaceHolder1_ddlSetupType").val();
                divHideShow(type);
            });
            $("#ContentPlaceHolder1_ddlSrcSetupType").change(function () {
                var type = $("#ContentPlaceHolder1_ddlSrcSetupType").val();
                divHideShowSrc(type);
            });
            $("#ContentPlaceHolder1_ddlCountry").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });


        });
        function divHideShowSrc(type) {
            if (type == "Division") {
                $("#divDivisionSrc").show("slow");
                $("#divDistrictSrc").hide("slow");
                $("#divThanaSrc").hide("slow");

            }
            else if (type == "District") {
                $("#divDivisionSrc").hide("slow");
                $("#divDistrictSrc").show("slow");
                $("#divThanaSrc").hide("slow");
                //PageMethods.LoadDivision(OnDivisionLoadSucceed, OnFailed);
            }
            else if (type == "Thana") {
                $("#divDivisionSrc").hide("slow");
                $("#divDistrictSrc").hide("slow");
                $("#divThanaSrc").show("slow");
                //PageMethods.LoadDistrict(OnDistrictLoadSucceed, OnFailed);
            }
            else {
                $("#divDivisionSrc").hide("slow");
                $("#divDistrictSrc").hide("slow");
                $("#divThanaSrc").hide("slow");
            }
        }
        function divHideShow(type) {
            if (type == "Division") {
                $("#divDivision").show("slow");
                $("#divDistrict").hide("slow");
                $("#divThana").hide("slow");

            }
            else if (type == "District") {
                $("#divDivision").hide("slow");
                $("#divDistrict").show("slow");
                $("#divThana").hide("slow");
                //PageMethods.LoadDivision(OnDivisionLoadSucceed, OnFailed);
                $.ajax({

                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../Payroll/frmDivisionList.aspx/LoadDivision',
                    async: false,
                    //data: "{'name':'" + name.trim() + "', 'code':'" + code.trim() + "', 'email':'" + email.trim() + "','phone':'" + phone.trim() + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                    dataType: "json",
                    success: function (data) {

                        OnDivisionLoadSucceed(data);
                    },
                    error: function (result) {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    }
                });
            }
            else if (type == "Thana") {
                $("#divDivision").hide("slow");
                $("#divDistrict").hide("slow");
                $("#divThana").show("slow");
                //PageMethods.LoadDistrict(OnDistrictLoadSucceed, OnFailed);
                $.ajax({

                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../Payroll/frmDivisionList.aspx/LoadDistrict',
                    async: false,
                    //data: "{'name':'" + name.trim() + "', 'code':'" + code.trim() + "', 'email':'" + email.trim() + "','phone':'" + phone.trim() + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                    dataType: "json",
                    success: function (data) {

                        OnDistrictLoadSucceed(data);
                    },
                    error: function (result) {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    }
                });
            }
            else {
                $("#divDivision").hide("slow");
                $("#divDistrict").hide("slow");
                $("#divThana").hide("slow");
            }
        }
        function OnDistrictLoadSucceed(result) {
            var list = result.d;
            var controlId = '<%=ddlDistrict.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].DistrictName + '" value="' + list[i].DistrictId + '">' + list[i].DistrictName + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            $("#ContentPlaceHolder1_ddlDistrict").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            return false;
        }
        function OnDivisionLoadSucceed(result) {
            var list = result.d;
            var controlId = '<%=ddlDivision.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].DivisionName + '" value="' + list[i].DivisionId + '">' + list[i].DivisionName + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            return false;
        }
        function SaveDivision() {
            var countryId = $("#<%=ddlCountry.ClientID %>").val();
            var division = $("#<%=txtDivision.ClientID %>").val();
            if (countryId == "0") {
                toastr.warning("Please Select a Country");
                $("#<%=ddlCountry.ClientID %>").focus();
                return false;
            }
            else if (division == "") {
                toastr.warning("Please Insert a Division");
                $("#<%=txtDivision.ClientID %>").focus();
                return false;
            }
            var divisionId = $("#<%=hfDivisionId.ClientID %>").val();
            CommonHelper.SpinnerOpen();
            PageMethods.SaveDivision(division, countryId, divisionId, OnSaveDivisionSucceed, OnFailed);
            return false;
        }
        function OnSaveDivisionSucceed(result) {
            if (result.IsSuccess == true) {
                CommonHelper.SpinnerClose();
                CommonHelper.AlertMessage(result.AlertMessage);
                Clear();
            }
            return false;
        }
        function SaveDisctrict() {
            var divisionId = $("#<%=ddlDivision.ClientID %>").val();
            var district = $("#<%=txtDistrict.ClientID %>").val();
            if (divisionId == "0") {
                toastr.warning("Please Select a Division");
                $("#<%=ddlDivision.ClientID %>").focus();
                return false;
            }
            else if (district == "") {
                toastr.warning("Please Insert a District");
                $("#<%=txtDistrict.ClientID %>").focus();
                return false;
            }
            var districtId = $("#<%=hfDistrictId.ClientID %>").val();
            CommonHelper.SpinnerOpen();
            PageMethods.SaveDisctrict(district, divisionId, districtId, OnSaveDistrictSucceed, OnFailed);
            return false;

        }
        function OnSaveDistrictSucceed(result) {
            if (result.IsSuccess == true) {
                CommonHelper.SpinnerClose();
                CommonHelper.AlertMessage(result.AlertMessage);
                Clear();
            }
            return false;
        }
        function SaveThana() {
            var districtId = $("#<%=ddlDistrict.ClientID %>").val();
            var thana = $("#<%=txtThana.ClientID %>").val();
            if (districtId == "0") {
                toastr.warning("Please Select a Division");
                $("#<%=ddlDistrict.ClientID %>").focus();
                return false;
            }
            else if (thana == "") {
                toastr.warning("Please Insert a Thana");
                $("#<%=txtThana.ClientID %>").focus();
                return false;
            }
            var thanaId = $("#<%=hfThanaId.ClientID %>").val();
            CommonHelper.SpinnerOpen();
            PageMethods.SaveThana(thana, districtId, thanaId, OnSaveThanaSucceed, OnFailed);
            return false;
        }
        function OnSaveThanaSucceed(result) {
            if (result.IsSuccess == true) {
                CommonHelper.SpinnerClose();
                CommonHelper.AlertMessage(result.AlertMessage);
                
                Clear();
            }
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#SetupTable tbody tr").length;
            var type = $("#<%=ddlSrcSetupType.ClientID %>").val();
            var name = "";
            if (type == "0") {
                toastr.warning("Please Select a type");
                $("#<%=ddlSrcSetupType.ClientID %>").focus();
                return false;
            }
            if (type == "Division") {
                name = $("#<%=txtDivisionSrc.ClientID %>").val();

            }
            else if (type == "District") {
                name = $("#<%=txtDistrictSrc.ClientID %>").val();
            }
            else if (type == "Thana") {
                name = $("#<%=txtThanaSrc.ClientID %>").val();
            }
            $("#<%=hfTypeSrc.ClientID %>").val(type);
            PageMethods.Search(type, name, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchSucceed, OnFailed);
            return false;
        }
        function OnSearchSucceed(searchData) {

            $("#SetupTableContainer").show("slow");
            $("#SetupTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            if (searchData.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"2\" >No Data Found</td> </tr>";
                $("#SetupTable tbody ").append(emptyTr);
                return false;
            }

            $.each(searchData.GridData, function (count, gridObject) {
                var tr = "";
                var name = "";
                var id = "";
                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }
                if (gridObject.DivisionName != null && gridObject.DivisionId != null) {
                    name = gridObject.DivisionName;
                    id = gridObject.DivisionId;
                }
                else if (gridObject.DistrictName != null && gridObject.DistrictId != null) {
                    name = gridObject.DistrictName;
                    id = gridObject.DistrictId;
                }
                else if (gridObject.ThanaName != null && gridObject.ThanaId != null) {
                    name = gridObject.ThanaName;
                    id = gridObject.ThanaId;
                }
                tr += "<td style='width:60%;'>" + name + "</td>";

                tr += "<td style='width:20%;'> <a onclick=\"javascript:return FillFormEdit(" + id + ",\'" + name + '\');"' + "title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteData(" + id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";

                tr += "<td style='display:none'>" + id + "</td>";
                tr += "</tr>";

                $("#SetupTable tbody").append(tr);

                tr = "";
                i++;
            });
            //PerformCancleAction();
            $("#GridPagingContainer ul").append(searchData.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(searchData.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(searchData.GridPageLinks.NextButton);
            return false;
        }
        function FillFormEdit(id, name) {
            var type = $("#<%=hfTypeSrc.ClientID %>").val();
            PageMethods.FillForm(id, type, OnFillFormSucceed, OnFailed);
            return false;
        }
        function OnFillFormSucceed(result) {
            var division = result[0].Division;
            var district = result[0].District;
            var thana = result[0].Thana;
            var type = result[0].Type;
            divHideShow(type);
            if (division.DivisionId > 0) {
                $("#<%=ddlCountry.ClientID %>").val(division.CountryId).trigger("change");
                $("#<%=txtDivision.ClientID %>").val(division.DivisionName);
                $("#<%=hfDivisionId.ClientID %>").val(division.DivisionId);
                $("#<%=btnAddDivision.ClientID %>").val("Update");

            }
            else if (district.DistrictId > 0) {
                $("#<%=ddlDivision.ClientID %>").val(district.DivisionId);
                $("#<%=txtDistrict.ClientID %>").val(district.DistrictName);
                $("#<%=hfDistrictId.ClientID %>").val(district.DistrictId);
                $("#<%=btnAddDistrict.ClientID %>").val("Update");

            }
            else if (thana.ThanaId > 0) {
                $("#<%=ddlDistrict.ClientID %>").val(thana.DistrictId).trigger("change");
                $("#<%=txtThana.ClientID %>").val(thana.ThanaName);
                $("#<%=hfThanaId.ClientID %>").val(thana.ThanaId);
                $("#<%=btnAddThana.ClientID %>").val("Update");

            }
            $("#<%=ddlSetupType.ClientID %>").val(type);
            $("#myTabs").tabs({ active: 0 });
            return false;
        }
        function DeleteData(id) {
            if (!confirm("Do you want to delete?")) {
                return;
            }
            var type = $("#<%=hfTypeSrc.ClientID %>").val();
            PageMethods.DeleteData(id, type, OnDeleteSucceed, OnDeleteFailed);
            return false;
        }
        function OnDeleteSucceed(result) {
            if (result.IsSuccess == true) {
                CommonHelper.SpinnerClose();
                CommonHelper.AlertMessage(result.AlertMessage);      
                GridPaging(1, 1);
            }
            return false;
        }
        function OnDeleteFailed(error) {

        }
        function Clear() {
            $("#<%=txtDivision.ClientID %>").val("");
            $("#<%=txtDistrict.ClientID %>").val("");
            $("#<%=txtThana.ClientID %>").val("");
            $("#<%=hfDistrictId.ClientID %>").val("0");
            $("#<%=hfDivisionId.ClientID %>").val("0");
            $("#<%=hfThanaId.ClientID %>").val("0");
            $("#<%=btnAddDistrict.ClientID %>").val("Save");
            $("#<%=btnAddDivision.ClientID %>").val("Save");
            $("#<%=btnAddThana.ClientID %>").val("Save");
        }
        function OnFailed(error) {
            toastr.error(error);
            return false;
        }
    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDivisionId" runat="server" Value="0" />
    <asp:HiddenField ID="hfDistrictId" runat="server" Value="0" />
    <asp:HiddenField ID="hfThanaId" runat="server" Value="0" />
    <asp:HiddenField ID="hfTypeSrc" runat="server" Value="" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Setup</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="SetupPanel" class="panel panel-default">
                <div class="panel-heading">
                    Setup Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label runat="server" class="control-label" Text="Setup Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSetupType" runat="server" CssClass="form-control" TabIndex="1">
                                    <asp:ListItem Text="-- Please Select --" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Division" Value="Division"></asp:ListItem>
                                    <asp:ListItem Text="District" Value="District"></asp:ListItem>
                                    <asp:ListItem Text="Thana" Value="Thana"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        &nbsp;
                        <div id="divDivision" style="display: none">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label runat="server" class="control-label required-field"
                                        Text="Country"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label runat="server" class="control-label required-field" Text="Division"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtDivision" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnAddDivision" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="javascript:return SaveDivision()" />
                                </div>
                            </div>
                        </div>
                        <div id="divDistrict" style="display: none">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label runat="server" class="control-label required-field"
                                        Text="Division"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label runat="server" class="control-label required-field" Text="District"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtDistrict" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnAddDistrict" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="javascript:return SaveDisctrict()" />
                                </div>
                            </div>
                        </div>
                        <div id="divThana" style="display: none">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label runat="server" class="control-label required-field"
                                        Text="District"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label runat="server" class="control-label required-field" Text="Thana"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtThana" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnAddThana" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="javascript:return SaveThana()" />
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </div>

        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label runat="server" class="control-label required-field" Text="Setup Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSrcSetupType" runat="server" CssClass="form-control" TabIndex="1">
                                    <asp:ListItem Text="-- Please Select --" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Division" Value="Division"></asp:ListItem>
                                    <asp:ListItem Text="District" Value="District"></asp:ListItem>
                                    <asp:ListItem Text="Thana" Value="Thana"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        &nbsp;
                        <%--search--%>
                        <div id="divDivisionSrc" style="display: none">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label runat="server" class="control-label" Text="Division"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtDivisionSrc" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                            </div>

                        </div>
                        <div id="divDistrictSrc" style="display: none">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label runat="server" class="control-label" Text="District"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtDistrictSrc" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                            </div>

                        </div>
                        <div id="divThanaSrc" style="display: none">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label runat="server" class="control-label" Text="Thana"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtThanaSrc" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="Button1" runat="server" Text="Search" CssClass="btn btn-primary" OnClientClick="javascript:return GridPaging(1,1)" />
                            </div>
                        </div>
                        &nbsp;
                        <div class="form-group" id="SetupTableContainer" style="overflow: scroll; display: none">
                            <table class="table table-bordered table-condensed table-responsive" id="SetupTable"
                                style="width: 100%;">
                                <thead>
                                    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                        <th style="width: 60%;">Name
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
        </div>
    </div>
    <%--</div>--%>
    <%--<div style="height: 45px">
    </div>--%>
    <%--<div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Division List</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">District List</a></li>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Thana List</a></li>
            <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-4">Branch List</a></li>
        </ul>
        <div id="tab-1">
            <div id="SearchDivision" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Division List
                </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvDivision" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                        OnPageIndexChanging="gvDivision_PageIndexChanging">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("DivisionId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DivisionName" HeaderText="Division Name" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
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
                    <div class="divClear">
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchDistrict" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">District List
                </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvDistrict" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                        OnPageIndexChanging="gvDistrict_PageIndexChanging">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("DistrictId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DistrictName" HeaderText="District Name" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
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
                    <div class="divClear">
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3">
            <div id="SearchThana" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Thana List </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvThana" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                        OnPageIndexChanging="gvThana_PageIndexChanging">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("ThanaId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ThanaName" HeaderText="Thana Name" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
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
                    <div class="divClear">
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-4">
            <div id="SearchBranch" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Branch List </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvBranch" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                        OnPageIndexChanging="gvBranch_PageIndexChanging">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("WorkStationId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="WorkStationName" HeaderText="Branch Name" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
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
                    <div class="divClear">
                    </div>
                </div>
            </div>
        </div>
    </div>--%>
</asp:Content>
