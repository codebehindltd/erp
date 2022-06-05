<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmSalesCustomer.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.frmSalesCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Sales Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Customer</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#<%=txtEmail.ClientID %>").blur(function () {
                var A = validEmail($("#<%=txtEmail.ClientID %>").val());
                if (A == true) {
                }
                else {
                    MessagePanelShow();
                    $("#<%=lblMessage.ClientID %>").text("Email is not in correct format");
                }
            });

            $("#<%=txtSearchPhone.ClientID %>").blur(function () {
                var A = validatePhone($("#<%=txtSearchPhone.ClientID %>").val());
                if (A == true) {
                }
                else {
                    MessagePanelShow();
                    $("#<%=lblMessage.ClientID %>").text("Phone number is not in correct format");
                }
            });

            $("#<%=txtPhone.ClientID %>").blur(function () {
                var A = validatePhone($("#<%=txtPhone.ClientID %>").val());
                if (A == true) {
                }
                else {
                    MessagePanelShow();
                    $("#<%=lblMessage.ClientID %>").text("Phone number is not in correct format");
                }
            });


            $("#<%=txtContactEmail.ClientID %>").blur(function () {
                var contactEmailValue = $("#<%=txtContactEmail.ClientID %>").val().length;
                if (contactEmailValue > 0) {
                    var isCEmailValid = IsEmail($("#<%=txtContactEmail.ClientID %>").val());
                    if (isCEmailValid == true) {
                        MessagePanelHide();
                    }
                    else {
                        MessagePanelShow();
                        $("#<%=lblMessage.ClientID %>").text("Contact Email is not in correct format");
                    }
                }
                else {
                    $("#<%=lblMessage.ClientID %>").text("");
                    MessagePanelHide();
                }
            });

            $("#imgCollapse").click(function () {

                var imageSrc = $('#imgCollapse').attr("src");
                if (imageSrc == '/HotelManagement/Image/expand_alt.png') {
                    $("#ContactInformationAdditional").show('slow');
                    $('#imgCollapse').attr("src", '/HotelManagement/Image/collapse_alt.png');

                }
                else {
                    $("#ContactInformationAdditional").hide('slow');
                    $('#imgCollapse').attr("src", '/HotelManagement/Image/expand_alt.png');
                }
            });

        });


        function validEmail(e) {
            var filter = /^\s*[\w\-\+_]+(\.[\w\-\+_]+)*\@[\w\-\+_]+\.[\w\-\+_]+(\.[\w\-\+_]+)*\s*$/;
            return String(e).search(filter) != -1;
        }


        function validatePhone(field) {
            if (field.match(/^\d{10}/)) {
                return true;
            }

            return false;
        }


        $(function () {
            $("#myTabs").tabs();
        });

        function PerformClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=txtCustomerId.ClientID %>").val('');
            $("#<%=ddlCustomerType.ClientID %>").val('0');
            $("#<%=txtName.ClientID %>").val('');
            $("#<%=txtAddress.ClientID %>").val('');
            $("#<%=txtPhone.ClientID %>").val('');
            $("#<%=txtContactFax.ClientID %>").val('');
            $("#<%=txtEmail.ClientID %>").val('');
            $("#<%=txtWebAddress.ClientID %>").val('');

            $("#<%=txtContactPerson.ClientID %>").val('');
            $("#<%=txtContactDesignation.ClientID %>").val('');
            $("#<%=txtDepartment.ClientID %>").val('');
            $("#<%=txtContactEmail.ClientID %>").val('');
            $("#<%=txtContactPhone.ClientID %>").val('');
            $("#<%=txtContactFax.ClientID %>").val('');


            $("#<%=txtContactPerson2.ClientID %>").val('');
            $("#<%=txtContactDesignation2.ClientID %>").val('');
            $("#<%=txtDepartment2.ClientID %>").val('');
            $("#<%=txtContactEmail2.ClientID %>").val('');
            $("#<%=txtContactPhone2.ClientID %>").val('');
            $("#<%=txtContactFax2.ClientID %>").val('');

            $("#<%=btnSave.ClientID %>").val("Save");
            MessagePanelHide();
            return false;
        }
        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }


    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Customer Info</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Customer </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Customer Information</a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:HiddenField ID="txtCustomerId" runat="server"></asp:HiddenField>
                            <asp:Label ID="lblName" runat="server" Text="Customer Name"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtName" runat="server" TabIndex="1" CssClass="ThreeColumnTextBox"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblCustomerType" runat="server" Text="Customer Type"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlCustomerType" TabIndex="2" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblPhone" runat="server" Text="Phone"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtPhone" TabIndex="3" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtEmail" TabIndex="4" CssClass="ThreeColumnTextBox" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblWebAddress" runat="server" Text="Web Address"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtWebAddress" TabIndex="5" CssClass="ThreeColumnTextBox" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtAddress" runat="server" TabIndex="6" CssClass="ThreeColumnTextBox" TextMode="MultiLine"
                                ></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblContactPerson" runat="server" Text="Contact Person"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtContactPerson" TabIndex="7" CssClass="ThreeColumnTextBox" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblContactDesignation" runat="server" Text="Designation"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtContactDesignation" TabIndex="8" CssClass="ThreeColumnTextBox" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtDepartment" TabIndex="9" runat="server" CssClass="ThreeColumnTextBox"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblContactEmail" runat="server" Text="Contact Email"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtContactEmail" TabIndex="10" CssClass="ThreeColumnTextBox" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblContactPhone" runat="server" Text="Contact Phone"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtContactPhone" TabIndex="11" runat="server"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblContactFax" runat="server" Text="Contact Fax"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtContactFax" TabIndex="12" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div>
                            <img id="imgCollapse" width="40px" TabIndex="13" src="/HotelManagement/Image/expand_alt.png" alt="ExpandSection"
                                title="Click to Expand" />
                            Additional Contact Info
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div id="ContactInformationAdditional" style="display: none;">
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblContactPerson2" runat="server" Text="Contact Person 2"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtContactPerson2" TabIndex="14" CssClass="ThreeColumnTextBox" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblContactDesignation2" runat="server" Text="Designation 2"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtContactDesignation2" TabIndex="15" CssClass="ThreeColumnTextBox" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblDepartment2" runat="server" Text="Department 2"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtDepartment2" runat="server" TabIndex="16" CssClass="ThreeColumnTextBox"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblContactEmail2" runat="server" Text="Contact Email 2"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtContactEmail2" TabIndex="17" CssClass="ThreeColumnTextBox" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblContactPhone2" runat="server" Text="Contact Phone 2"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtContactPhone2" TabIndex="18" runat="server"></asp:TextBox>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblContactFax2" runat="server" Text="Contact Fax 2"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtContactFax2" TabIndex="19" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <%--Right Left--%>
                        <asp:Button ID="btnSave"  runat="server" Text="Save" OnClientClick="javascript: return SaveValidation();"
                            OnClick="btnSave_Click" CssClass="TransactionalButton btn btn-primary" TabIndex="20" />
                        <asp:Button ID="btnClear" runat="server" TabIndex="21" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                            OnClientClick="javascript: return PerformClearAction();" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="InfoPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Customer
                </a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSearchName" runat="server" Text="Customer Name"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSearchName" runat="server" TabIndex="1" CssClass="ThreeColumnTextBox"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSearchCode" runat="server" Text="Customer ID"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSearchCode" TabIndex="2" runat="server"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblSearchPhone" runat="server" Text="Phone"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtSearchPhone" TabIndex="3" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                </div>
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                        CssClass="TransactionalButton btn btn-primary" TabIndex="4" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary"
                        OnClientClick="javascript: return PerformClearAction();" TabIndex="5" />
                </div>
            </div>
            <div id="SearchPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
                </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvSalesCustomer" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="5" OnPageIndexChanging="gvSalesCustomer_PageIndexChanging"
                        OnRowDataBound="gvSalesCustomer_RowDataBound" OnRowCommand="gvSalesCustomer_RowCommand"
                        TabIndex="9">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("CustomerId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Customer Name" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Code" HeaderText="Customer ID" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Phone" HeaderText="Phone" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("CustomerId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("CustomerId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
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
    <script type="text/javascript">
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {
            MessagePanelHide();
        }
    </script>
</asp:Content>
