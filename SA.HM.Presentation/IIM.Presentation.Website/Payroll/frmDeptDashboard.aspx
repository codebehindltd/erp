<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmDeptDashboard.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmDeptDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var deleteObj = [];
        var vv = [];
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Administrative & Security</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Department Head Dashboard</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            PageMethods.LoadDepartmentWiseLeaveBalance(OnLoadLeaveBalanceSucceeded, OnLoadLeaveBalanceFailed);
            PageMethods.LoadEmpTypeWiseEmpNo(OnLoadEmpTypeSucceeded, OnLoadEmpTypeFailed);
        });

        /*Leave Balance Section*/
        function OnLoadLeaveBalanceSucceeded(data) {
            vv = data;

            var xAxisCategories = _.pluck(data, "LeaveTypeName");
            xAxisCategories = _.uniq(xAxisCategories);

            var dataSeries = [];

            dataSeries.push({
                name: 'TotalLeave',
                data: _.pluck(vv, "TotalLeave")
            });

            dataSeries.push({
                name: 'Remaining',
                data: _.pluck(vv, "RemainingLeave")
            });

            dataSeries.push({
                name: 'Taken',
                data: _.pluck(vv, "TotalTakenLeave")
            });

            $('#LeaveBalanceContainer').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: ' '
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: xAxisCategories,
                    title: {
                        text: null
                    },
                    labels: {
                        rotation: -45,
                        style: {
                            fontSize: '11px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                },

                yAxis: {
                    min: 1,
                    max: 200,
                    tickInterval: 1,
                    //gridLineWidth: 1,
                    //lineWidth: 1,
                    title: {
                        text: 'Leave (Days)',
                        align: 'high'
                    },
                    labels: {
                        rotation: 0,
                        overflow: 'justify',
                        style: {
                            fontSize: '9px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                },
                tooltip: {
                    valueSuffix: ' days',
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                            '<td style="padding:0"><b>{point.y:.1f} days</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            crop: false
                        }
                    },
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,
                        crop: false
                    }
                },
                legend: {
                    layout: 'horzintal',
                    align: 'right',
                    verticalAlign: 'top',
                    x: -40,
                    y: 1,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                    shadow: true
                },
                credits: {
                    enabled: false
                },
                series: dataSeries
            });
        }
        function OnLoadLeaveBalanceFailed() {
        }

        /* Emp Type Pie Chart*/
        function OnLoadEmpTypeSucceeded(data) {
            v = data;

            var dataSeries = [];

            for (var i = 0; i < v.length; i++) {
                dataSeries.push({
                    name: v[i].TypeName,
                    y: v[i].NoOfEmp
                });
            }

            $('#EmpTypeContainer').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: ' '
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    name: "Brands",
                    colorByPoint: true,
                    data: dataSeries
                }]
            });
        }
        function OnLoadEmpTypeFailed() {
        }
    </script>
    <div class="divFullSectionWithTwoDvie">
        <div class="divBox divSectionLeftRightSameWidth">
            <div id="DashboardPanel1" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Leave Balance</a>
                <div id="LeaveBalanceContainer">
                </div>
            </div>
        </div>
        <div class="divBox divSectionLeftRightSameWidth">
            <div id="DashboardPanel2" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Type Wise Employee</a>
                <div id="EmpTypeContainer">
                </div>
            </div>
        </div>
    </div>
    <div class="divClear">
    </div>
    <div class="divFullSectionWithTwoDvie">
        <div class="divBox divSectionLeftRightSameWidth">
            <div id="DashboardPanel3" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Leave Balance</a>
                <div id="LeaveBalanceContainer2">
                    <asp:GridView ID="gvLeaveBalance" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <%--<asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("TrainingId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="LeaveTypeName" HeaderText="Leave" ItemStyle-Width="40%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalLeave" HeaderText="Total Leave Balance" ItemStyle-Width="40%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalTakenLeave" HeaderText="Taken" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RemainingLeave" HeaderText="Remaining" ItemStyle-Width="30%">
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
                </div>
            </div>
        </div>
        <div class="divBox divSectionLeftRightSameWidth">
            <div id="DashboardPanel4" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Upcoming Training
                    List</a>
                <div id="TrainingListContainer">
                    <asp:GridView ID="gvTrainingList" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("TrainingId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TrainingName" HeaderText="Training Name" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FromDate" HeaderText="Start Date" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ToDate" HeaderText="End Date" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Organizer" HeaderText="Organizer" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Trainer" HeaderText="Trainer" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Location" HeaderText="Location" ItemStyle-Width="10%">
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
                </div>
            </div>
        </div>
    </div>
</asp:Content>
