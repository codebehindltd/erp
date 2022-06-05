<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmPayrollDashboard.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmPayrollDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var deleteObj = [];
        var vv = [];
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Administrative & Security</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Payroll Dashboard</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            PageMethods.LoadLeave(OnLoadLeaveSucceeded, OnLoadLeaveFailed);
            PageMethods.LoadAttendance(OnLoadAttendanceSucceeded, OnLoadAttendanceFailed);
            PageMethods.LoadLeaveForpieChart(OnLoadSucceeded, OnLoadFailed);
            PageMethods.LoadEmpMonthlyAttendance(OnLoadMonthlyAttendanceSucceeded, OnLoadMonthlyAttendanceFailed);

            PageMethods.LoadDepartmentWiseLeaveBalance(OnLoadLeaveBalanceSucceeded, OnLoadLeaveBalanceFailed);
        });

        /*Monthly Attendance section*/
        function OnLoadMonthlyAttendanceSucceeded(data) {
            vvvv = data;

            var dataSeries = [];
            var dataCategories = [];

            dataCategories.push({
                categories: _.pluck(vvvv, "MonthName")
            });

            var bb = _.pluck(vvvv, "DisplayName")

            dataSeries.push({
                name: bb[0],
                data: _.pluck(vvvv, "NoOfDays")
            });

            $('#MonthlyAttendanceContainer').highcharts({
                title: {
                    text: ' ',
                    x: -20 //center
                },
                subtitle: {
                    text: ' ',
                    x: -20
                },
                xAxis: dataCategories,
                yAxis: {
                    title: {
                        text: 'Days'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    valueSuffix: 'Days'
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                series: dataSeries
            });
        }
        function OnLoadMonthlyAttendanceFailed() {
        }

        /*Leave pie chart section*/
        function OnLoadSucceeded(data) {
            v = data;

            var dataSeries = [];

            for (var i = 0; i < v.length; i++) {
                dataSeries.push({
                    name: v[i].LeaveTypeName,
                    y: v[i].RemainingLeave
                });
            }

            $('#RemainingLeaveContainer').highcharts({
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
        function OnLoadFailed() {
        }

        /*Leave Section*/
        function OnLoadLeaveSucceeded(data) {
            vv = data;

            var xAxisCategories = _.pluck(data, "LeaveTypeName");
            xAxisCategories = _.uniq(xAxisCategories);

            var dataSeries = [];

            dataSeries.push({
                name: 'Remaining',
                data: _.pluck(vv, "RemainingLeave")
            });

            dataSeries.push({
                name: 'Taken',
                data: _.pluck(vv, "TotalTakenLeave")
            });

            $('#LeaveContainer').highcharts({
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
        function OnLoadLeaveFailed() {
        }

        /*Attendance Section*/

        function OnLoadAttendanceSucceeded(data) {
            vvv = data;

            var xAxisCategories = _.pluck(data, "Status");
            xAxisCategories = _.uniq(xAxisCategories);

            var dataSeries = [];

            dataSeries.push({
                name: 'No of Employee',
                data: _.pluck(vvv, "NoOfEmp")
            });

            //            dataSeries.push({
            //                name: 'Taken',
            //                data: _.pluck(vv, "TotalTakenLeave")
            //            });

            $('#AttendanceContainer').highcharts({
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
                    max: 50,
                    tickInterval: 1,
                    //gridLineWidth: 1,
                    //lineWidth: 1,
                    title: {
                        text: 'Employee (Persons)',
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
                            '<td style="padding:0"><b>{point.y:.1f} persons</b></td></tr>',
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
        function OnLoadAttendanceFailed() {
        }

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

    </script>
    <div class="divFullSectionWithTwoDvie">
        <div class="divBox divSectionLeftRightSameWidth">
            <div id="DashboardPanel1" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Remaining Leave</a>
                <div id="RemainingLeaveContainer">
                </div>
            </div>
        </div>
        <div class="divBox divSectionLeftRightSameWidth">
            <div id="DashboardPanel2" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Employee Total Leave
                    Information</a>
                <div id="LeaveContainer">
                </div>
            </div>
        </div>
    </div>
    <div class="divClear">
    </div>
    <div class="divFullSectionWithTwoDvie">
        <div class="divBox divSectionLeftRightSameWidth">
            <div id="DashboardPanel3" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Employee Monthly
                    Attendance</a>
                <div id="MonthlyAttendanceContainer">
                </div>
            </div>
        </div>
        <div class="divBox divSectionLeftRightSameWidth">
            <div id="DashboardPanel4" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Daily Attendance
                    Information</a>
                <div id="AttendanceContainer">
                </div>
            </div>
        </div>
    </div>
    <div class="divClear">
    </div>
    <div class="divFullSectionWithTwoDvie">
        <div class="divBox divSectionLeftRightSameWidth">
            <div id="DashboardPanel5" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Leave Balance</a>
                <div id="LeaveBalanceContainer">
                </div>
            </div>
        </div>
        <%--<div class="divBox divSectionLeftRightSameWidth">
            <div id="Div3" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Daily Attendance
                    Information</a>
                <div id="Div4">
                </div>
            </div>
        </div>--%>
    </div>
</asp:Content>
