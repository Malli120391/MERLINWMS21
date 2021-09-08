<%@  Page Title ="Search Outbound" Language="C#" AutoEventWireup="true" MasterPageFile="~/mOutbound/OutboundMaster.master" CodeBehind="OutboundSearchNew.aspx.cs" Inherits="MRLWMSC21.mOutbound.OutboundSearch_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
      <style>
              .ishideNow {
            display: none;
        }
        /* Absolute Center Spinner */
        .loading {
            position: fixed;
            z-index: 9999;
            height: 2em;
            width: 2em;
            overflow: show;
            margin: auto;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0; 
        }
        

            /* Transparent Overlay */

            .loading:before {
                content: '';
                display: block;
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background-color: rgba(0,0,0,0.3);
            }

            /* :not(:required) hides these rules from IE9 and below */
            .loading:not(:required) {
                /* hide "loading..." text */
                font: 0/0 a;
                color: transparent;
                text-shadow: none;
                background-color: transparent;
                border: 0;
            }

                .loading:not(:required):after {
                    content: '';
                    display: block;
                    font-size: 10px;
                    width: 1em;
                    height: 1em;
                    margin-top: -0.5em;
                    -webkit-animation: spinner 1500ms infinite linear;
                    -moz-animation: spinner 1500ms infinite linear;
                    -ms-animation: spinner 1500ms infinite linear;
                    -o-animation: spinner 1500ms infinite linear;
                    animation: spinner 1500ms infinite linear;
                    border-radius: 0.5em;
                    -webkit-box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.5) -1.5em 0 0 0, rgba(0, 0, 0, 0.5) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                    box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) -1.5em 0 0 0, rgba(0, 0, 0, 0.75) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                }

        /* Animation */

        @-webkit-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-moz-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-o-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }
    </style>
       <style>
            .pagination ul {
                display: inline-block;
                padding: 0;
                margin: 0;
            }

            .pagination li {
                display: inline;
            }

                .pagination li a {
                    margin: 0px 2px;
                    box-shadow: var(--z1);
                    display: inline-block !important;
                    width: 20px !important;
                    height: 20px !important;
                    text-align: center !important;
                    background: #fff;
                    border-radius: 2px;
                    padding: 1px;
                    line-height: 20px;
                    text-decoration: none;
                    color: black;
                }

                .pagination li.active a {
                    box-shadow: var(--z1);
                    padding: 0px;
                    display: inline-block !important;
                    border: 2px solid var(--sideNav-bg) !important;
                    background-color: var(--sideNav-bg) !important;
                    width: 20px !important;
                    height: 20px !important;
                    text-align: center;
                    line-height: 20px;
                    color: #fff;
                }

                .pagination li:hover.active a {
                    background-color: #f2f2f2;
                }

                .pagination li:hover:not(.active) a {
                    background-color: #f2f2f2 !important;
                    color: black;
                }

           




          

          

    </style>
     
    <script src="../Scripts/angular.min.js"></script>
      
    <script src="../mReports/Scripts/dirPagination.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                debugger;
                $("#txtFromDate").datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date(),
                    onSelect: function (selected) {
                        $("#txtToDate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                    }
                });
                $("#txtToDate").datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date()
                });
                  $("#txtDueDate").datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date(),
                    onSelect: function (selected) {
                        $("#txtToDate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                    }
                });
        });

    </script>
    <script>
        var app = angular.module('OBSApp', ['angularUtils.directives.dirPagination']);
        app.controller('OBSearch', function ($scope, $http, $timeout, $window, $filter) {
            debugger;
           
            $scope.search = new OBSearch(0, 0, '', '', '', '', 0, 0, '', 0);
            var PageIndex1 = 1;
           
              $scope.search.CategoryID = "0";
            $scope.search.DeliveryStatusID = "0";
            var tenantid=0; 
            var Warehouseid;
          //  var storeID;
            //Tenant drop down
            

          
            $("#txtOutboundType").val("");
           
            var textfieldname = $("#txtOutboundType");
            debugger;
            DropdownFunction(textfieldname);
            $("#txtOutboundType").autocomplete({
                source: function (request, response) {

                    if ($("#txtOutboundType").val() == "" || $("#txtOutboundType").val() == null || $("#txtOutboundType").val() == undefined) {
                        $scope.search.DocumentTypeID = 0;
                    }

                    $.ajax({
                        <%--url: '../mWebServices/FalconWebService.asmx/LoadTenantsDataFor3PL',
                        data: "{ 'prefix': '" + request.term + "','Accountid': <%= cp.AccountID%>  }",--%>
                        url: '../mWebServices/FalconWebService.asmx/LoadDocumentType',
                        data: "{ 'prefix': '" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))
                        }
                    });
                },
                select: function (e, i) {
                    $scope.search.DocumentTypeID = i.item.val;
                   
                    
                },
                minLength: 0
            });
             var textfieldname5 =$("#txtStore");
            debugger;

            DropdownFunction(textfieldname5);
            $("#txtStore").autocomplete({
                source: function (request, response) {
                    //if (Tenantid == 0) {
                    //    showStickyToast(false, "Select Tenant");
                    //}
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" +tenantid + "'  }",
                          dataType: "json",
                          type: "POST",
                          contentType: "application/json; charset=utf-8",
                          success: function (data) {
                              response($.map(data.d, function (item) {
                                  return {
                                      label: item.split(',')[0],
                                      val: item.split(',')[1]
                                  }
                              }))
                          }
                      });
                  },
                  select: function (e, i) {
                      debugger;
                      storeid=i.item.val;

                },
                minLength: 0
            });
        




      //==== warehouse dropdown====================================//
                 $("#txtWarehouse").val("");
                    var textfieldname = $("#txtWarehouse");
                    DropdownFunction(textfieldname);
                    $("#txtWarehouse").autocomplete({
                        source: function (request, response) {
                            //if (RefTenant == 0 || RefTenant == "0" || RefTenant == undefined || RefTenant == null) {
                            //    showStickyToast(false, 'Please select Tenant');
                            //    return false;
                            //}
                            //if ($("#txtWarehouse").val() == '') {
                            //    WHID = 0;
                            //}
                            $.ajax({
                                // url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList_CurrentStock',
                                //data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant + "'}",
                                // data: "{ 'prefix': '" + request.term + "','TenantID':'" + 0 + "'}",
                                url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                                data: "{ 'prefix': '" + request.term + "'  }",
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    response($.map(data.d, function (item) {
                                        return {
                                            label: item.split(',')[0],
                                            val: item.split(',')[1]
                                        }
                                    }))
                                }
                            });
                        },
                        select: function (e, i) {
                             Warehouseid = i.item.val;
                    $("#txtWarehouse").val("");
                    $scope.search.Warehouseid = Warehouseid;
          

                        },
                        minLength: 0
                    });


    //=================== Tenant dropdown ================================//
 debugger;
    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            if ($("#txtTenant").val() == '') {
                RefTenant = 0;
                $scope.search.tenantid = 0;
            }
            if ($scope.search.Warehouseid == 0 || $scope.search.Warehouseid == "0" || $scope.search.Warehouseid == undefined || $scope.search.Warehouseid == null) {
                showStickyToast(false, 'Please select WareHouse');
                return false;
            }
            $.ajax({
                // url: '../mWebServices/FalconWebService.asmx/GetTenantList',                
                // data: "{ 'prefix': '" + request.term + "'}",
                url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                data: "{ 'prefix': '" + request.term + "','whid':'" + $scope.search.Warehouseid + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1]
                        }
                    }))
                }
            });
        },
        select: function (e, i) {
            debugger
          tenantid = i.item.val;
                 //   $("#txtTenant").val("");
                    $scope.search.tenantid = tenantid;   
          
        },
        minLength: 0
    });





            $scope.Fromdate = $filter('date')(new Date(), 'dd-MMM-yyyy');
            $('#txtFromDate').val($scope.Fromdate);
            $('#txtToDate').val($scope.Fromdate);
            $scope.GetOBSearchData = function (pageindex) {
                debugger;
                PageIndex1 = pageindex;


                if (Warehouseid == "0" || Warehouseid == null || Warehouseid == undefined|| $("#txtWarehouse").val().trim() == "" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined) {
                    tenantid = 0;
                    showStickyToast(false, "Please select WareHouse");
                    return false;
                }
               


               
                if ($('#txtFromDate').val() == undefined || $('#txtFromDate').val() == '') {
                    $scope.search.FromDate = '';
                  
                }
                else {
                    $scope.search.FromDate = $('#txtFromDate').val();
                }
                if ($('#txtToDate').val() == undefined || $('#txtToDate').val() == '') {
                    $scope.search.ToDate = '';
                  
                }
                else {
                    $scope.search.ToDate = $('#txtToDate').val();
                }
                debugger
                 $scope.blockUI = true;
                var httpreq = {
                    method: 'POST',
                    url: 'OutboundSearchNew.aspx/GetOBSearchData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'obj': $scope.search,'PageSize':100,'PageIndex':pageindex },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    debugger;
                  
                    var dt = JSON.parse(response.d);
                    $scope.obdData = dt.Table1;
                    $scope.blockUI = false;
                    if ($scope.obdData == undefined || $scope.obdData.length == 0) {
                        $scope.TotalRecords = 0;
                        showStickyToast(false, "No Data found", false); return false;
                    }
                    else {
                        $scope.TotalResult = dt.Table1[0].TotalRecords;
                        $scope.TotalRecords = $scope.TotalResult;
                    }

                });
            }
         
              $scope.generateSlip = function () {
        debugger;
        $scope.generateData = $.grep($scope.obdData, function (a) { return a.Selected == true; });
        if ($scope.generateData.length == 0 || $scope.generateData.length == undefined || $scope.generateData.length == null) {
            showStickyToast(false, "Please Select Items to Generate PDF", false);
            return false;
        }
        $("#divLoading").show();
        var httpreq = {
            method: 'POST',
            url: 'OutboundSearchNew.aspx/generatePckingSlip',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.generateData },
            async: false,
        }
        $http(httpreq).success(function (response) {
            debugger;
            if (response.d == "Error") {
                showStickyToast(false, "Exception while generating PDF ", false);
                $("#divLoading").hide();
                return false;
            }
            else if (response.d == "No Data Found") {
                showStickyToast(false, "No Data Found ", false);
                $("#divLoading").hide();
                return false;
            }
            else {
                var obj = response.d;
                showStickyToast(true, "PDF Generated Successfully ", false);
                $("#divLoading").hide();
                window.open('../mOutbound/PackingSlip/' + obj);
                return false;
            }
        });
    };

    $scope.CheckUncheckHeader = function () {
        debugger;
        if ($scope.obdData != null) {
            $scope.IsAllChecked = true;
            for (var i = 0; i < $scope.obdData.length; i++) {
                if (!$scope.obdData[i].Selected) {
                    $scope.IsAllChecked = false;
                    break;
                }
            };
        }
    };
    $scope.CheckUncheckHeader();

    $scope.CheckUncheckAll = function () {
        for (var i = 0; i < $scope.obdData.length; i++) {
            $scope.obdData[i].Selected = $scope.IsAllChecked;
        }
    };
 $(function () {
            $('.isvisibleNow').on('click', function () {
                $('.ishideNow').slideToggle();
            });
        });
            $scope.downloadExcel = function () {
                debugger;
                if ($scope.search == undefined || $scope.search == null || $scope.search.length == 0) {
                    showStickyToast(false, "No Data found to Download Excel ");
                    return;
                }
                if ($('#txtFromDate').val() == undefined || $('#txtFromDate').val() == '') {
                    $scope.search.FromDate = '';
                   
                }
                else {
                    $scope.search.FromDate = $('#txtFromDate').val();
                }
                if ($('#txtToDate').val() == undefined || $('#txtToDate').val() == '') {
                    $scope.search.ToDate = '';
                   
                }
                else {
                    $scope.search.ToDate = $('#txtToDate').val();
                }

                   PaginationId = 1;

                   PageSize = 1;

                $scope.blockUI = true;

                var httpreq = {
                    method: 'POST',
                    url: 'OutboundSearchNew.aspx/DownloadExcelForLog',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'obj': $scope.search,'PageSize':100,'PageIndex':PageIndex1 },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    debugger;
                    $scope.blockUI = false;
                    if (response.d == "0") {
                        
                    }
                    window.open('../ExcelData/' + response.d + '.xlsx');

                });
            }
        });




        function OBSearch(DeliveryStatusID, DocumentTypeID,SearchText,FromDate, ToDate,DueDate,Warehouseid,tenantid,AWBNo,CategoryID) {
            this.DeliveryStatusID = DeliveryStatusID;
            this.DocumentTypeID = DocumentTypeID;
            this.FromDate = FromDate;
            this.ToDate = ToDate;
            this.DueDate = DueDate;
            this.Warehouseid = Warehouseid;
            this.tenantid = tenantid;
            this.AWBNo = AWBNo;
            this.CategoryID = CategoryID;
            this.SearchText = SearchText;
            
          

        }

    </script>
  <div class="module_yellow">
        <div class="ModuleHeader" height="35px">
            <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Outbound</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Outbound Search</span></div>
        </div>
    </div>    
    <div ng-app="OBSApp" ng-controller="OBSearch" class="container">
        <div ng-show="blockUI">
            <div class="row">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                <div style="align-self: center;">
                  <img width="60" src="../Images/preloader.svg" />

                </div>

            </div>

        </div>
            </div>
        <br/>
        <div style="">
            <div class="row">
            

                <div class="col s3 m3">
                    <div class="flex">
                        <div>
                             <input type="text" id="txtWarehouse" required="" />
                            <label>Warehouse</label>
                            <span class="errorMsg"></span>                           

                        </div>
                    </div>
                </div>
                  <div class="col s3 m3">
                    <div class="flex">
                        <div>
                             <input type="text" id="txtTenant" required="" />
                            <label>Tenant</label>
                                                      

                        </div>
                    </div>
                </div>
             
                <div class="col s3 m3">
                    <div class="flex">
                        <div>

                            <input type="text" id="txtFromDate" ng-model="search.FromDate" required="" />
                            <label>From Date</label>
                           
                        </div>
                    </div>
                </div>
                <div class="col s3 m3">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtToDate" ng-model="search.ToDate" required="" />
                            <label>To Date</label>
                           
                        </div>
                    </div>
                </div>
                 
                </div>
            <div class="row ishideNow">
                 <div class="col s3 m3">
                    <div class="flex">
                        <div>
                            <select  id="Category" required="" ng-model="search.CategoryID" style="width: 100% !important;" >
                                <option value="0" selected>Select Category</option>
                                <option value="1">Delv. Doc. Number</option>
                                <option value="2">Customer Name</option>
                                  <option value="5">Customer PONumber</option>
                                  <option value="6">Invoice Number</option>
                                  <option value="7">SO Number</option>
                                 
                            </select>
                           <label> </label>
                                         </div>
                        </div>
                    </div>
                 <div class="col s3 m3">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtsearchText" ng-model="search.SearchText" required="" />
                            <label>Search Text</label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                </div>
                 <div class="col s3 m3">
                    <div class="flex">
                        <div>
                            <select class="p1save" id="DeliveryStatusID" required="" style="width: 100% !important;" ng-model="search.DeliveryStatusID">
                                <option value="0" selected>All</option>
                                <option value="1">Sent to Store</option>
                                <option value="2">Sent for PGI</option>
                                  <option value="3">PGI Done/Sent to Delivery</option>
                                  <option value="4">Delivered</option>
                                  <option value="6">Sent to Packing</option>
                                  <option value="12">On Hold</option>
                                  <option value="13">Sent to Picking</option>
                                  <option value="14">Packing in process</option>
                                  <option value="15">Picking in Process</option>
                                  <option value="16">Sent to Loading</option>
                            </select>
                           <label><%= GetGlobalResourceObject("Resource", "DeliveryStatus")%> </label>
                                         </div>
                        </div>
                    </div>
                 
                  <div class="col s3 m3">
                    <div class="flex">
                        <div>
                             <input type="text" id="txtOutboundType" ng-model="search.DocumentType" required="" />
                            <label>Outbound Type</label>
                          <%--  <span class="errorMsg"></span>                           --%>

                        </div>
                    </div>
                </div>
                </div>
            <div class="row ishideNow">
                 <div class="col s3 m3">
                    <div class="flex">
                        <div>
                             <input type="text" id="txtAWBNo" ng-model="search.AWBNo" required="" />
                            <label>AWB No.</label>
                          <%--  <span class="errorMsg"></span>                           --%>

                        </div>
                    </div>
                </div>
                 <div class="col s3 m3">
                    <div class="flex">
                        <div>

                            <input type="text" id="txtDueDate" ng-model="search.DueDate" required="" />
                            <label>Due Date</label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                </div>
                </div>
            <div class="row">
                <div class="col m2">
                    <b>Total : {{TotalRecords}}</b>
                </div>

                <div class="col m10" flex end>
                    <gap></gap>
                    <div class="">

                        <div class="flex__ end">
                             <button type="button" class="isvisibleNow btn btn-primary">Advanced Search <i class="material-icons vl">youtube_searched_for</i></button>
               
                            <button type="button" ng-click="GetOBSearchData(1)" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;
                             <button type='button' class="btn btn-primary" ng-click="downloadExcel()">Export <i class="material-icons">cloud_download</i></button>
                                <button type="button" class="btn btn-primary" ng-click="generateSlip();">Generate Slip <%=MRLWMSC21Common.CommonLogic.btnfaList %></button>
         
                        </div>
                    </div>
                </div>
            </div>
        </div>

             <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth" style="width: 100%; overflow: auto;">           
                    <table class="table-striped" id="" style="white-space: nowrap;">
                        <thead>
                            <tr>
                                <th>Delv. Doc. No.</th>
                                <th>Type</th>
                                <th>Tenant </th>
                                <th>Customer</th>
                                <th>Warehouse</th>
                                <th>Delv. Doc. Date</th>
                                <th>PGI Date/Done By</th>
                                <th>Delivery Date</th>
                                <th>SO Number</th>
                                <th>Status</th>
                               <th>AWB No.</th>
                                <th>Courier</th>
                                <th>Due Date</th>
                                <th>Pick Note</th>
                                <th center>Delv. Note</th>
                                <th center>Modify</th>
                               <th>
                                <div class="checkbox" style="margin-left:0px !important;">
                                    <input type="checkbox" id="chkParent" ng-click="CheckUncheckAll()" ng-model="IsAllChecked"/>
                                    <label for="chkParent"></label>
                                </div>
                            </th>
                            </tr>
                           <%-- <span ng-if="dt.PGIUser!=null">/</span>--%>
                        </thead>
                        <tbody>
                            <tr dir-paginate="dt in obdData|itemsPerPage:100" pagination-id="nonAvaible" total-items="TotalRecords">
                               <%-- <td number>{{$index+1}}</td>--%>  
                                <td>{{dt.OBDNumber}}</td>
                                <td>{{dt.DocumentType}}</td>
                                <td>{{dt.TenantCode}}</td>
                                <td>{{dt.CustomerName}}</td>
                                <td>{{dt.Warehouse}}</td>
                                <td>{{dt.OBDDate|date:'dd-MMM-yyyy'}}</td>
                                <td>{{dt.PGIDoneOn|date:'dd-MMM-yyyy'}}<br />{{dt.PGIUser}}</td>
                                <td> 
                                    
                                    <div ng-show="dt.DeliveryDate===''"><span style="color:red">[Not Delivered]</span></div>
                                    <div ng-show="dt.DeliveryDate==null"></div>
                                    <div ng-show="dt.DeliveryDate!=null"><span style="color:green">[Delivered On:{{dt.DeliveryDate}}]</span></div>
                                    
<%--                                    <div ng-show="dt.DeliveryDate==='' "><span style="color:red">[Not Delivered]</span></div>
                                    <div ng-show="dt.DeliveryDate!=null"><span style="color:green">[Delivered On:{{dt.DeliveryDate}}]</span>


                                                                    </div>--%>
                                </td>
                                <td>{{dt.SONumber}}</td>
                                <td>{{dt.DeliveryStatus}}</td>
                                <td>{{dt.AWBNo}}</td>
                                <td>{{dt.Courier}}</td>
                                <td style="text-align:center;">{{dt.DueDate|date:'dd-MMM-yyyy'}}</td>
                                 <td><a ToolTip="Pick Note | Pick note with barcoded material codes to pick items for delivery." style="cursor:pointer;" href="../mOutbound/DeliveryPickNote.aspx?obdid={{dt.OutboundID}}&lineitemcount={{dt.LineCount}}&TN={{dt.TenantName}}"><span style="color:red">Pick Note [{{dt.LineCount}}]</span></a></td>
                                <td><a ToolTip="Del. Pick Note" style="cursor:pointer;" href="../mOutbound/DeliveryPickNoteData.aspx?obdid={{dt.OutboundID}}&lineitemcount={{dt.LineCount}}&TN={{dt.TenantName}}"><span style="color:red">Delv. Note [{{dt.LineCount}}]</span></a></td>
                               
                                <td>   <a href="../mOutbound/OutboundDetails.aspx?obdid={{dt.OutboundID}}"><i class='material-icons ss'>mode_edit</i></a></td>
                                 <td>
                                <div class="checkbox">
                                    <input type="checkbox" id="{{$index}}" class="checkedone checkselectall" ng-model="dt.Selected" ng-change="CheckUncheckHeader()" data-obj='{"OutboundID":"{{obd.OutboundID}}"}' />
                                    <label for="chk{{$index}}"></label>
                                </div>
                            </td>
                            </tr>
                        </tbody>
                        <tfoot>
                        <tr>
                            <td colspan="17">
                                <div flex end>
                                    <dir-pagination-controls direction-links="true" on-page-change="GetOBSearchData(newPageNumber)" pagination-id="nonAvaible" boundary-links="true"> </dir-pagination-controls>
                                </div>
                              <%--  <div class="divpaginationstyle" flex end>                                    
                                    <dir-pagination-controls direction-links="true" pagination-id="nonAvaible" boundary-links="true"> </dir-pagination-controls>
                                </div>--%>
                            </td>
                        </tr>
                    </tfoot>
                    </table>
                    <br />


                </div>




                <div class="divlineheight"></div>
            </div>
        </div>
        <br />
     
    </div>
  
</asp:Content>
