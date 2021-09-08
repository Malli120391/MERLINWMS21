<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InfoGraphics.aspx.cs" Inherits="MRLWMSC21.mReports.InfoGraphics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="Charts/animate.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/angular.min.js"></script>  
    <script src="../mInbound/Scripts/jquery-1.8.2.min.js"></script>
    <script src="../mMisc/Scripts/bootstrap.min.js"></script>  
    <script src="Charts/Chart.min.js"></script>
    <script src="Charts.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js" type="text/javascript"></script>

    <style> 
        body {
            background-color:none;
        }

         .spanNum {
            font-size: 20pt;
            margin-top: 50px;
            text-align:center; 
            font-family:Arial; 
        }
        .spanText {
            font-size: 13pt; 
            font-family:Arial;         
        }

        .BlueTheme {
            background-color: #005891 !important;
            text-align:center;
            border-radius:5px;
            border:3px solid #FFFFFF;
            
    box-shadow: var(--z1);
        }

        .RedTheme {
            text-align:center;
            background-color:#ff3333 !important;
            border-radius:5px;
            border:3px solid #FFFFFF;
            
    box-shadow: var(--z1);
        }

        .GreenTheme {
            text-align:center;
            background-color:seagreen !important;
            border-radius:5px;
            border:3px solid #FFFFFF;
           
    box-shadow: var(--z1);
        }

        .YellowTheme {
            background-color: #FFA000;
        }
         .divTab {
            width: 32.8%;
            height: 260px;
            display: inline-table;
            cursor: pointer;
            color: white;
            font-weight: 700;
            background:red;
        }
         .divTable {
            position: absolute;
            border: 1px solid orange;
            background-color: white;
            color: grey;
            font-size: 11pt;
            text-align: left;
            display: none;
            z-index:555;
            text-align:center;
        }

            .divTable td {
                border: 1px solid lightgrey;
            }
        .popupData1
        {
            background-color: #FFFFFF; position: absolute; display: none; top: 0; color: black;
            border-radius:5px;
            border:2px solid #808080;
            
    box-shadow: var(--z1);
            padding: 20px;
        }

        .divblockstyle 
        {
         
    border-radius: 3px;
    box-shadow: var(--z1);
    padding: 10px;
        }


         /* -------------- blockUI in angularJS----------------------*/

 #blocker
{
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    opacity: .5;
    background-color: #000;
    z-index: 1000;
    overflow: auto;
}
    #blocker div
    {
        position: absolute;
        top: 50%;
        left: 50%;
        width: 5em;
        height: 2em;
        margin: -1em 0 0 -2.5em;
        color: #fff;
        font-weight: bold;
    }
    
    #blocker img
    {
        position: relative;
        top: -80px;
        left: 15%;
    }

/*----------------------end blockUI-------------------*/

    </style>
<div class="dashed"></div>
<div class="pagewidth">
    <div ng-app="MyApp" ng-controller="Charts" class="divapp">
  <%--       <div id="blocker" ng-show="blockUI">
            <div>loading... 
                <img src="../Images/ajax-loader.gif" style="width: 50px;" /></div>
        </div>--%>
       
    <div class="pagewidth divContainerr" id="divContainer">

         <div id="divTable1" class="divTable popupData1">
                <div id="divchildtable1" style="display: none;"></div>
            </div>

        <div id="divTable2" class="divTable popupData1">
                <div id="divchildtable2" style="display: none;"></div>
            </div>

        <div id="divTable3" class="divTable popupData1">
                <div id="divchildtable3" style="display: none;"></div>
            </div>

            <div class="row">
                <div class="col-lg-12">
                    <p></p>


                    <div class="divblockstyle">
                        <div class="row">
                            <div class="col-lg-4">

                                <canvas id="TotalBinpiechart" height="140"></canvas>
                                <div style="text-align: center;">
                                    <b>
                                        <label id="lblTotal"></label>
                                        <label id="lblTotalLoc" style="color: red;"></label>
                                    </b>
                                </div>
                            </div>
                            <div class="col-lg-4">
                                <canvas id="ToalVolumepiechart" height="140"></canvas>
                                <div style="text-align: center;">
                                    <b>
                                        <label id="lblTotalVolText"></label>
                                        <label id="lblTotalVolData" style="color: red;"></label>
                                    </b>
                                </div>
                            </div>
                            <div class="col-lg-4">
                                <canvas id="TotalWHWeightpiechart" height="140"></canvas>
                                <div style="text-align: center;">
                                    <b>
                                        <label id="lblTotalWeightText"></label>
                                        <label id="lblTotalWeightData" style="color: red;"></label>
                                    </b>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>






            </div>                                             
                     <p></p>      
            <div class="row">
                <div class="col-lg-12">
                    <div class="divblockstyle">
                        <div class="row">
                            <div class="col-lg-4">
                                <canvas id="doughnutReceipts" height="140"></canvas>
                                <div style="text-align: center;">
                                    <b>
                                        <label id="lblTotalInwardText"></label>
                                        <label id="lblTotalInwardData" style="color: red;"></label>
                                    </b>
                                </div>
                            </div>
                            <div class="col-lg-4">
                                <canvas id="doughnutPutaways" height="140"></canvas>
                                <div style="text-align: center;">
                                    <b>
                                        <label id="lblTotalWorkText"></label>
                                        <label id="lblTotalWorkData" style="color: red;"></label>
                                    </b>
                                </div>
                            </div>
                            <div class="col-lg-4">
                                <canvas id="doughnutPicking" height="140"></canvas>
                                <div style="text-align: center;">
                                    <b>
                                        <label id="lblTotalPickText"></label>
                                        <label id="lblTotalPickData" style="color: red;"></label>
                                    </b>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
                     <p></p>
            <div class="row">
                <div class="col-lg-6">
                     <div class="divblockstyle" style="padding:2px !important;">
                            
                            <div>
                                <canvas id="inboundLineChart" height="100"></canvas>
                                <b><div class="text-center" style="font-family:Arial;font-size:medium;text-align:center;">Inbound Ageing</div></b>
                            </div>
                        </div></div>

                <div class="col-lg-6">
                     <div class="divblockstyle" style="padding:2px !important;">
                            
                            <div>
                                <canvas id="outboundLineChart" height="100"></canvas>
                                <b><div class="text-center" style="font-family:Arial;font-size:medium;text-align:center;">Outbound Ageing</div></b>
                            </div>
                        </div></div>

        </div>
                     <p></p>   
            <div class="row">

                
                <div class="col-lg-6">
                  
                       <div class="divblockstyle" style="padding:2px !important;">
                           
                            <div>
                                <canvas id="barChart" height="130"></canvas>
                                <b><div class="text-center" style="font-family:Arial;font-size:medium;text-align:center;">Inventory Ageing</div></b>
                            </div>
                        </div>
                    </div>   

                <div class="col-lg-6">
                                    <div class="divblockstyle" style="padding:2px !important;">
                                       <div id="divTab1" class="divTab RedTheme">
                                        <br />
                                        <span style="font-family:Arial;font-size:medium;font-weight:bold;">Normalized Time</span>
                                        <div id="spanTab1" class="spanNum"></div>
                                        <br />
                                        <span class="spanText">Inbound</span>
                                    </div>
                                        <div id="divTab2" class="divTab GreenTheme">
                                        <br/>
                                        <span style="font-family:Arial;font-size:medium;font-weight:bold;">Normalized Time</span>
                                        <div id="spanTab2" class="spanNum"></div>
                                        <br />
                                        <span class="spanText">Outbound</span>
                                    </div>
                                        <div id="divTab3" class="divTab BlueTheme">
                                        <br />
                                            <span>&nbsp;</span>
                                        <div id="spanTab3" class="spanNum"></div>
                                        <br />
                                        <span class="spanText">Active [ Total Bays ]</span>
                                    </div>


                                    </div></div>



            </div>            
                </div>
    </div>
 </div>
 
</asp:Content>
