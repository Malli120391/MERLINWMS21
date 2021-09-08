<%@ Page Title="Put Away Statistics" Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="PutAwayReport.aspx.cs" Inherits="MRLWMSC21.mInbound.PutAwayReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IBContent" runat="server">
    <style>
        .formControl {
            width: 150px;
            padding: 5px;
            border-radius: 3px;
            border: 1px solid black;
        }
        #Loading {
            position:fixed;
            width:100%;
            top:0;right:0;left:0;bottom:0;
            background-color:rgba(0,0,0,0.3);
            z-index:1000;
            display:none;
        }
        #tblPutAwaysStats {
            font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
            border-collapse: collapse;
            width: 100%;
        }

        #tblPutAwaysStats td, #tblPutAwaysStats th {
            border: 1px solid #ddd;
            padding: 8px;
        }

        #tblPutAwaysStats tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        #tblPutAwaysStats tr:hover {
            background-color: #ddd;
        }

        #tblPutAwaysStats th {
            padding-top: 12px;
            padding-bottom: 12px;
            text-align: left;
            background-color: #4CAF50;
            color: white;
        }
    </style>
    <script src="Scripts/JqChart.js"></script>
    <script src="Scripts/JqChartUtils.js"></script>
    <script src="Scripts/InventraxAjax.js"></script>
<script>

    $(document).ready(function () {
        $('#btnShow').trigger('click');
    });

    function btnShow_Click() {
        $('#Loading').fadeIn(400);
        LoadGraph();
    }



    function LoadGraph()
    {
        //window.onload = function () {
        $('.tblPutAwaysStats').empty();
        var Year = $('.ddlYear').val();
        var User = $('.ddlUser').val();

        var data = "{ year:  '" + Year + "' , user:  '" + User + "'}";
        InventraxAjax.AjaxResultExecute("PutAwayReport.aspx/GetPutAwayData", data, 'GetDataOnSuccess', 'GetDataOnError',null);

    }

    var randomScalingFactor = function () {
        return Math.round(Math.random() * 50 * (Math.random() > 0.5 ? 1 : 1)) + 50;
    };
   
       
    function GetDataOnSuccess(data) {
        var obj = JSON.parse(data.Result);
        var months = []
        var dataPointsC = [];
        var dataPointsW = [];
        var TableData = "";

        var config = {
            type: 'line',
            data: {
                //labels: ["January", "February", "March", "April", "May", "June", "July"],
                labels: months,
                datasets: [{
                    label: "Wrong Put Aways",
                    fill: false,
                    borderColor: window.chartColors.red,
                    backgroundColor: window.chartColors.red,
                    data: dataPointsW
                    //data: [
                    //    randomScalingFactor(),
                    //    randomScalingFactor(),
                    //    randomScalingFactor(),
                    //    randomScalingFactor(),
                    //    randomScalingFactor(),
                    //    randomScalingFactor(),
                    //    randomScalingFactor()
                    //]
                }, {
                    label: "Correct Put Aways",
                    fill: false,
                    borderColor: window.chartColors.blue,
                    backgroundColor: window.chartColors.blue,
                    data: dataPointsC
                    //data: [
                    //    randomScalingFactor(),
                    //    randomScalingFactor(),
                    //    randomScalingFactor(),
                    //    randomScalingFactor(),
                    //    randomScalingFactor(),
                    //    randomScalingFactor(),
                    //    randomScalingFactor()
                    //]
                }]
            },
            options: {
                responsive: true,
                title: {
                    display: true,
                    text: "PUT AWAY SUGGETIONS - ACTUAL vs SUGGESTED"
                },
                scales: {
                    xAxes: [{
                        display: true,
                        ticks: {
                            callback: function (dataLabel, index) {
                                // Hide the label of every 2nd dataset. return null to hide the grid line too
                                return index % 2 === 0 ? dataLabel : '';
                            }
                        }
                    }],
                    yAxes: [{
                        display: true,
                        beginAtZero: false
                    }]
                }
            }
        };


        var obj = obj.Table;
        for (var i = 0; i < obj.length; i++) {
            months.push(obj[i].Date);
            dataPointsC.push(eval(obj[i].Corrects));
            dataPointsW.push(eval(obj[i].Wrongs));

            TableData += "<tr><td>" + (i + 1) + "</td><td>" + obj[i].Date + "</td><td>" + obj[i].Corrects + "</td><td>" + obj[i].Wrongs + "</td></tr>";
        }

       
        var ctx = document.getElementById("canvas").getContext("2d");
        window.myLine = new Chart(ctx, config);


        //Table Building
        $('.tblPutAwaysStats').append("<tr><thead><th style='width:50px;'><%= GetGlobalResourceObject("Resource", "SNo")%></th><th><%= GetGlobalResourceObject("Resource", "Date")%></th><th><%= GetGlobalResourceObject("Resource", "Corrects")%></th><th><%= GetGlobalResourceObject("Resource", "Wrongs")%></th></thead></tr>");
        $('.tblPutAwaysStats').append("<tbody>");
        $('.tblPutAwaysStats').append(TableData);
        $('.tblPutAwaysStats').append("</tbody>");
        $('#Loading').fadeOut(400);
    }
    function GetDataOnError(data) {
        $('#Loading').fadeOut(400);
    }

    function LoadYears() {
        $('.ddlYear').empty();
        var d = new Date();
        var n = d.getFullYear();

        for (var i = 0; i < 10; i++) {
            $('.ddlYear').append("<option value='" + (n - i) + "'>" + (n - i) + "</option>");
        }
    }

</script>
    <style>
        .ui-btn {
             top: 0px; 
        }        
        
        .sssss {width: 22px;
    height: 17px;
    margin-right: 5px;
    top: 3px;
    position: relative;
        }
    </style>
    <!--breadcrumb-->
    <div class="module_yellow">
        <div class="ModuleHeader">
            <div>
                <a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> 
                 <span>Reports</span> <i class="material-icons">arrow_right</i> 
                 <span>mInbound</span> <i class="material-icons">arrow_right</i> 
                 <span class="breadcrumbd" contenteditable="false">PutAwayReport</span>
            </div>
        </div>
    </div>
<!--ends-breadcrumb-->  
    <div class="container">
        <div class="pull-right" style="width: 30%;">
            <table class="Headertablewidth">
                <tr>
                    <td align="right">
                        <div class="flex">
                            <!-- Globalization Tag is added for multilingual  -->

                            <select id="ddlYear" runat="server" class="ddlYear"></select>
                        </div>
                    </td>
                    <td>
                        <div class="flex">
                            <select id="ddlUser" runat="server" class="ddlUser"></select>
                        </div>
                    </td>
                    <td>
                        <%-- <button type="button" id="btnShow" class="btn btn-primary"  onclick="return btnShow_Click();">Show<i class="material-icons vl">remove_red_eye</i></button>--%>
                        <button type="button" id="btnShow" class="btn btn-primary" onclick="return btnShow_Click();"><%= GetGlobalResourceObject("Resource", "Show")%><i class="material-icons vl">remove_red_eye</i></button>
                    </td>
                </tr>
            </table>

        </div>
         <div></div>
        <br />
        <div style="width: 100%;  display:none;">
            <canvas id="canvas"></canvas>
        </div>
        <br />
        <br />
        <div>
            <table id="tblPutAwaysStats" class="tblPutAwaysStats" style="width: 100%; border-collapse: collapse;" border="1"></table>
        </div>
        <div id="Loading">
            <img src="../Images/async_obd.GIF" style="margin-left: 40%; margin-top: 20%; background-color: white; padding: 10px 100px 10px 100px; border-radius: 5px;" />
        </div>

    </div>

</asp:Content>
