<%@ Page Title="Vehicle Yard Availability Status" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="VehicleStatus.aspx.cs" Inherits="MRLWMSC21.mReports.VehicleStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">



     <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script>
        var myApp = angular.module("myApp", ["angularUtils.directives.dirPagination"]);
        myApp.filter('startFrom', function () {
            return function (input, start) {
                if (!input || !input.length) { return; }
                start = +start; //parse to int
                return input.slice(start);
            }
        });

        myApp.controller("VehicleStatus", function ($scope, $http, $compile,$interval)
        {
            $scope.newPageNumber = 1;
            $scope.TotalRecords = 0;
            $scope.count = 0;
            $scope.getData = function () {
                
                //$(".loaderforCurrentStock").show();
                var httpreq = {
                    method: 'POST',
                    url: 'VehicleStatus.aspx/GetVehicleYardAvailability_Status',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {},
                    async: false,
                }
                $http(httpreq).success(function (response) {
                    debugger;
                    $scope.Data = JSON.parse(response.d).Table;
                    $scope.Start = 0;
                    var dt = null;
                    dt = $scope.Data;
                    $scope.TotalRecords = 0;
                    $scope.count = 0;
                    $scope.TotalRecords = dt.length;
                    $scope.count = parseInt($scope.TotalRecords / 10);
                    $("#divAnimation").attr("data-aos","fade-up");
                    //$scope.ChangeData();
                });
            };
            $scope.getData();

            $scope.ChangeData = function () {
                //
                var val = $scope.count + 1;
                $scope.newVal = 0;

                var timer = $interval(function () {
                    $scope.newVal = $scope.newVal + 1;
                    if (val != $scope.newVal) {
                        
                        $scope.Start = $scope.Start + 10;
                        $("#divAnimation").attr("data-aos","");
                        $("#divAnimation").attr("data-aos","fade-up");
                    }
                    else {
                        $scope.getData();
                        $interval.cancel(timer);
                    }
                }, 20000);
            };
        });
    </script>

<style>

.mytableOutbound tr th {
    padding: 15px 15px 10px 16px;
    border-bottom: 3px double #5f5e5e;
    border-top: 1px solid #a0a0a0;
    text-align: left;
    /*font-weight: 500;*/
    font-size: 15px;
    color: #fff;
    background: #000!important;
    border-right: 1px solid #e0ddd83d;
}

.mytableOutbound tr td {
    border-bottom: 0px solid #e0ddd8;
    empty-cells: show;
    padding: 3px 10px 3px 10px !important;
    vertical-align: top;
    font-weight: normal;
    text-align: left;
    font-size: 11.5px;
    color: #fff;
    background-color:transparent;
    cursor:pointer !important;
    transition:0.3s all;
    
}

    .mytableOutbound tr th:first-of-type {
      border-left: 0px solid #e0ddd83d !important;
    }

    .mytableOutbound tr td {
           border-left: 0px solid #e0ddd82e !important;
    }

    #divPage {
        float: right;
        margin: 0px 10px;
    }

        #divPage button {
            background: #fff;
            border: #fff;
            box-shadow: 0 2px 2px 0 rgba(0,0,0,0.16), 0 0 0 1px rgba(0,0,0,0.08);
            border-radius: 3px;
            color: black;
            border: 1px soliD #FFF;
        }

        #divPage .Highlight {
            background: #29328b;
            border: #29328b;
            box-shadow: 0 2px 2px 0 rgba(0,0,0,0.16), 0 0 0 1px rgba(0,0,0,0.08);
            border-radius: 3px;
            color: black;
            border: 1px soliD #29328b;
            color: #FFF;
        }

    .text{
  mso-number-format:"\@";
}

  .divblockstyle {
            transition: transform 500ms !important;
            transition-timing-function: ease-in;
            box-shadow:0px 2px 21px #dadada;
            border-radius: 0px;
            padding: 10px;
            background-color:#FFFFFF;
        }
    #chartdiv {
    width: 100%;
    height: 500px;
}
    .Cube {
        position: relative;
        padding: 10px;
        width: 45px;
        height: 260px;
        border-radius: 3px;
        background-color: #FFFFFF;
        box-shadow: var(--z1);
        cursor: pointer;
        background: rgb(6,168,240);
        background: linear-gradient(90deg, rgba(6,168,240,1) 0%, rgba(4,200,246,1) 50%, rgba(4,134,224,1) 100%, rgba(0,212,255,1) 100%);
        color: white;
    }
    .divRotate {
    transform: rotate(-90deg);
    text-align: left;
    line-height: 310px;
    font-size: medium;
    font-weight: bold;
    }
    .divIcon {
        font-size:35px;
        color:#1071c5;
    }
    
     .tooltip_ {
            position: relative;
            font-size:14px;
        }

        .tooltip__ {
            position: absolute;
            background-color:#f3f8fd;
            padding:10px;
            border:3px solid #4f91f9;
            border-radius: 4px;
            color: #000;
            width: 25vw;
            margin-bottom: -70px;
            margin-left: 40px;
            opacity: 0;
            visibility: hidden;
            text-align:left;
                box-shadow: 0 15px 40px #808080;
        }

        .Cube:hover .tooltip__ {
            visibility:visible;
            opacity:1;
            z-index:9999;
        }
    .spanTool {
        font-weight:bold; font-size: 14px !important; letter-spacing: 1px;
    }
    .divTop {
        border: 1px solid #5bb85b;
        padding: 6px;
        border-top-left-radius: 0px;
        border-top-right-radius: 0px;
        width: 250px;
        font-size: 15px;
        font-weight: 500;
        background-color: #5bb85b;
         color:white;
    }
    .divMiddle {
        border: 1px solid #5bb85b;
        padding: 0px 12px;
        width: 250px;
        color:#736d6d;
    }
    .divBottom {
    border: 1px solid #5bb85b;
    padding: 2px 7px;
    border-bottom-left-radius: 0px;
    border-bottom-right-radius: 0px;
    width: 250px;
    font-size: medium;
    font-weight: 500;
    background-color: #5bb85b;
    color:white;
}
    .tableClass {
        font-size: 13px;font-weight: 500;
    }
        .tableClass td {           
            border-bottom : 1px solid #f2f2f2;
            padding : 5px 0px;
        }
    .divBorder {
        border: 0px solid #000;
        padding: 5px 10px;
        color: #fdd503;
        box-shadow: inset 0px 0px 12px 2px #6b6969;
        font-size: 14px;
        font-weight: 500;
        border-radius: 3px;
        background: black;
        box-shadow:var(--z1);
        height:55px;
    }
    .c-container-fluid {
        position: fixed;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        background-color: #44434a;
        z-index: 99999999999999999;
         background-image:url(../Images/bg76.jpg) !important;
         background-repeat:no-repeat;
         background-size:cover;
         background-blend-mode: multiply;
         background-color: #414244;       
    }

    [menu] {
        display:none;
    }
    .main-content .content {
        margin-left:0;
    }
    .Header {
        width:100%;
    }

    .progress-bar {
    background-color: #1a1a1a;
    width:100%;          
}

.progress-bar {
    display: inline-block;
    transition: width .4s ease-in-out;    
}

    .ColorClass {
        background-color:transparent;
        padding:0px 0px;
        margin-top: -7px;
    }

.stripes {
    background-size: 15px 15px;
    background-image: linear-gradient(135deg, rgba(255, 255, 255, .15) 25%, transparent 25%,
                        transparent 50%, rgba(255, 255, 255, .15) 50%, rgba(255, 255, 255, .15) 75%,
                        transparent 75%, transparent);            
    
    animation: animate-stripes 5s linear infinite;    
    text-align:left;
}

@keyframes animate-stripes {
    0% {background-position: 0 0;} 100% {background-position: 60px 0;}
}
    .footer {
        background-color:#000;
        padding-left:10px;
    }
    .footer-at-floating {
        z-index : 9999999999999999999;
    }
    .VIPLogo {
        color:white;
        font-size :large;
        position: relative;
        left: 10px;
    }
    .module_login {
        background-color:transparent;
    }

      .module_LightOliveGreen {
        background-color:transparent;
    }
    .footer-at-floating  {
        display:none;
    }

    .right-fotter {
         width: unset;
         float: unset; 
         text-align: unset; 
         height: unset; 
         padding-right: unset; 
         line-height: unset; 
    }
    .setAside {
        display:none;
    }

        .Animation {
    position: relative;
    -webkit-animation: myfirst 5s 2; /* Safari 4.0 - 8.0 */
    -webkit-animation-direction: alternate; /* Safari 4.0 - 8.0 */
    animation: myfirst 5s linear;
    animation-direction: alternate;
    }

@keyframes myfirst {
    0%   {transform:translateX(0%)}
    100% {transform:translateX(-100%)}
}

    .ModuleHeader {
        display:none;
    }

</style>
    <div class="dashed"></div>
     <div ng-app="myApp" ng-controller="VehicleStatus" ng-init="Start = 0;">
         <div class="loaderforCurrentStock" style="display: none;">
             <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                 <div style="align-self: center;">
                     <div class="spinner">
                         <div class="bounce1"></div>
                         <div class="bounce2"></div>
                         <div class="bounce3"></div>
                     </div>
                 </div>
             </div>
         </div>         

         <div class="row" style="display:none;">
             <div class="col m12 text-center">
                 <div flex left>
                     <div id="divContainer" flex>
                     </div>
                 </div>
             </div>
         </div>

         <div class="row" style="margin: 0;">
             <div class="col-sm-12 col-lg-12 ColorClass">
                 <div id="divAnimation" class="divmainwidth element" data-aos="">
                     <table class="mytableOutbound" id="tbldatas">
                         <thead>
                             <tr class="mytableReportHeaderTR">
                                 <th><i class="material-icons">repeat_one</i>&emsp;Vehicle</th>
                                 <th><i class="material-icons">local_shipping</i>&emsp;Type</th>
                                 <th style="width:180px !important;"><i class="material-icons">account_circle</i>&emsp;Frei. Company</th>
                                 <th><i class="material-icons">settings_phone</i>&emsp;Contact</th>
                                 <th><i class="material-icons">low_priority</i>&emsp;Dock</th>
                                 <th><i class="material-icons">restore</i>&emsp;Gate In</th>
                                 <th><i class="material-icons">restore</i>&emsp;Waiting (hr)</th>
                                 <th><i class="material-icons">restore</i>&emsp;Docking (hr)</th>
                                 <th><i class="material-icons">local_shipping</i>&emsp;Status</th>
                             </tr>
                         </thead>
                         <tbody>
                             <tr ng-repeat="dt in Data | startFrom: Start | limitTo:10">
                                 <td><div class="divBorder">{{dt.RegistrationNumber}}</div></td>
                                 <td>
                                     <div ng-show="dt.VehicleType=='40 Feet Container'" class="divBorder" style="color:white;">
                                        40 Feet
                                     </div>
                                     <div ng-show="dt.VehicleType=='20 Feet Container'" class="divBorder" style="color:#55dff7;">
                                         20 Feet
                                     </div>

                                 </td>
                                 <td><div class="divBorder" style="width:180px !important;">{{dt.FreightCompany}}</div></td>
                                 <td><div class="divBorder">{{dt.DriverContactNumber}}</div></td>
                                 <td><div class="divBorder">{{dt.DockNo}}</div></td>
                                 <td><div class="divBorder">{{dt.ActGateInTime}}</div></td>
                                 <td Title=""><div class="divBorder">{{dt.WaitingTime}}</div></td>
                                 <td Title=""><div class="divBorder">{{dt.DockingTime}}</div></td>
                                 <td>
                                     <div class="divBorder progress-bar stripes" ng-show="dt.VehicleStatus=='Gate In'" style="color:white;background-color:#0b8be8 !important;">
                                         {{dt.VehicleStatus}}
                                     </div>

                                     <div class="divBorder progress-bar stripes" ng-show="dt.VehicleStatus=='Vehicle Reported'" style="color:white;background-color:#7b7979 !important;">
                                         {{dt.VehicleStatus}}
                                     </div>

                                     <div class="divBorder progress-bar stripes" ng-show="dt.VehicleStatus=='Vehicle Docked'" style="color:white;background-color:#0670a0 !important;">
                                         {{dt.VehicleStatus}}
                                     </div>

                                     <div class="divBorder progress-bar stripes" ng-show="dt.VehicleStatus=='Unloading In-Progress'" style="color:white;background-color:#ce5e05;">
                                         {{dt.VehicleStatus}}
                                     </div>

                                     <div class="divBorder progress-bar stripes" ng-show="dt.VehicleStatus=='Unloading Completed'" style="color:white;background-color:#a54f3d !important;">
                                         {{dt.VehicleStatus}}
                                     </div>

                                     <div class="divBorder progress-bar stripes" ng-show="dt.VehicleStatus=='Waiting for Documents'" style="color:white;background-color:#5f8604 !important;">
                                         {{dt.VehicleStatus}}
                                     </div>

                                     <div class="divBorder" ng-show="dt.VehicleStatus=='Vehicle Released'" style="color:white;background-color:#0aa542  !important;">
                                         {{dt.VehicleStatus}}
                                     </div>

                                     <div class="divBorder progress-bar stripes" ng-show="dt.VehicleStatus=='Gate Out'" style="color:white;background:#292727 !important;">
                                         {{dt.VehicleStatus}}
                                     </div>

                                      <div class="divBorder progress-bar stripes" ng-show="dt.VehicleStatus=='Loading In-Progress'" style="color:white;background-color:#0aa542 !important;">
                                         {{dt.VehicleStatus}}
                                     </div>

                                      <div class="divBorder progress-bar stripes" ng-show="dt.VehicleStatus=='Loading Completed'" style="color:white;background-color:#56a793 !important;">
                                         {{dt.VehicleStatus}}
                                     </div>

                                 </td>
                             </tr>
                             <tr ng-show="Data.length == 0">
                                 <td colspan="12" style="text-align: center !important;">No Data Found</td>
                             </tr>
                         </tbody>
                     </table>
                 </div>
                 <br />
             </div>
             <div class="divlineheight"></div>

         </div>

       <%--   <div class="row" style="margin: 0;">
               <div class="col-sm-12 col-lg-12 ColorClass">
                   <div></div>
               </div>
          </div>--%>

     </div>

</asp:Content>
