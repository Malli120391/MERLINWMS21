<%@ Page Title="Miscellaneous Issue" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="MiscellaneousIssue.aspx.cs" Inherits="MRLWMSC21.mInventory.MiscellaneousIssue" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">
   
    <script src="Scripts/angular.min.js"></script>
   <%-- <script src="Scripts/dirPagination.js"></script>--%>
      <script src="../mReports/Scripts/dirPagination.js"></script>
      <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <div class="container" ng-app="myApp" ng-controller="SupplierReturnsNew">
        <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">
                <div style="align-self: center;">
                    <img width="60" src="../Images/preloader.svg" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="offset-m2 offset-s2 col m3 s3">
                <div class="flex">
                    <input type="text" id="txtWH" skinid="txt_Auto" required="" />
                    <label>Warehouse </label>
                    <span class="errorMsg"></span>
                </div>
            </div>
            <div class="col m3 s3">
                <div class="flex">
                    <input type="text" id="txtTenant" required=""/>
                    <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                    <span class="errorMsg"></span>
                </div>
            </div>
            
            <div class="col m3 s3">
                <div class="flex">
                    <input type="text" id="txtMcode" skinid="txt_Auto" required="" />
                    <label><%= GetGlobalResourceObject("Resource", "PartNumber")%> </label>
                    <span class="errorMsg"></span>
                </div>
            </div>
            <div class="col m1 s1">
                <gap5></gap5>
                <button type="button" id="lnkGetMaterialDetails" ng-click="getvalidate();" class="btn btn-primary">Search <i class="material-icons">search</i></button>
            </div>
        </div>
        <gap></gap>
        <div ng-show="showList">
            <div class="row">
                <div class="col m2 ">
                    <span class="FormLabelsBlue">Description :</span> {{Details.MDescription}}
                </div>
                <div class="offset-m4 col m2">
                    <span class="FormLabelsBlue">BUoM/Qty. :</span>{{Details.UoM}}
                </div>
            </div>
            <div class="row">
                <div class="col m12">
                    <table class="table-striped">
                        <thead>
                            <tr>
                                <th>Location</th>
                                <th>Pallet Code</th>
                                <th>Quantity</th>
                                <th>Storage Location</th>
                                <th>BatchNo</th>
                                <th>Mfg. Date</th>
                                <th>Exp. Date</th>
                                <th>Serial. No</th>
                                <th>Project Ref. No.</th>
                                <th>MRP</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="list in List |itemsPerPage:25">
                                <td>{{list.Location}}</td>
                                <td>{{list.CartonCode}}</td>
                                <td>{{list.AVAILABLE}}</td>
                                <td>{{list.Code}}</td>
                                <td>{{list.BatchNo}}</td>
                                <td>{{list.MfgDate}}</td>
                                <td>{{list.ExpDate}}</td>
                                <td>{{list.SerialNo}}</td>
                                <td>{{list.ProjectRefNo}}</td>
                                <td>{{list.MRP}}</td>
                                <td>
                                    <div class="md-radio">
                                        <input type="radio" id="{{$index+1}}" name="mats" ng-model="material" ng-value="{{$index}}" ng-click="loadData(list)" /><label for="{{$index+1}}"></label></div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <gap></gap>
            <div class="row">
                <div class="col m2">
                    <div class="flex">
                        <input type="text" id="txtQuantity" onkeypress="return isNumber(event)" ng-model="serachdata.pickQuantity" />
                        <label><%= GetGlobalResourceObject("Resource", "PickQuantity")%> </label>
                        <span class="errorMsg"></span>
                    </div>

                </div>
                <div class="col m3">
                    <div class="flex">
                        <input type="text" ng-model="serachdata.Location" id="txtLocation" disabled />
                        <label><%= GetGlobalResourceObject("Resource", "Pick Location")%> </label>
                        <span class="errorMsg"></span>
                    </div>
                </div>


                <div class="col m3">
                    <div class="flex">
                        <input type="text" ng-model="serachdata.SLOC" disabled />

                        <label><%= GetGlobalResourceObject("Resource", "StorageLocation")%> </label>
                        <span class="errorMsg"></span>
                    </div>
                </div>
                <div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtRemarks" ng-model="serachdata.Remarks" />
                        <label><%= GetGlobalResourceObject("Resource", "Remarks")%></label>
                        <span class="errorMsg"></span>
                    </div>
                </div>
                <div class="col m1">
                    <button type="button" ng-if="List!=undefined && List!=null && List.length!=0 " id="lnkPickItem" class="btn btn-primary" ng-click="pickItems()">Pick Item</button>
                </div>

            </div>

        </div>
    </div>
    <script>
     var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
        myApp.controller('SupplierReturnsNew', function ($scope, $http) {
           
            $scope.serachdata = new search(0, 0, 0, '', 0, '', 0, '', 0,0,0);

            var textfieldname = $("#txtTenant");
            DropdownFunction(textfieldname);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    debugger;
            if ($('#txtTenant').val() == '' || $('#txtTenant').val() == undefined || $scope.serachdata.TenatID == 0) {
                $scope.serachdata.TenatID = 0;
                $scope.serachdata.MMID = 0;
                $('#txtMcode').val('');
               // $('#txtWH').val('');
                $scope.clearData();

            }

            $.ajax({
                //url: '../mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL',
               // data: "{ 'prefix': '" + request.term + "'}",
                url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                data: "{ 'prefix': '" + request.term + "','whid':'" + $scope.serachdata.WarehouseID + "' }",
                      
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
            $scope.serachdata.TenatID = i.item.val;
            $scope.serachdata.MMID = 0;
            $('#txtMcode').val('');
            //$scope.serachdata.WarehouseID = 0;
           // $('#txtWH').val('');
            $scope.clearData();


        },
        minLength: 0
    });

            var textfieldname = $("#txtMcode");
            DropdownFunction(textfieldname);
            $("#txtMcode").autocomplete({
                source: function (request, response) {
                    debugger;
                    if ($('#txtTenant').val() == '' || $('#txtTenant').val() == undefined || $scope.serachdata.TenatID == 0) {
                        //please shoe sticky tosat
                        return false;
                    }
                    
                    if ($('#txtMcode').val() == '' || $('#txtMcode').val() == undefined) {
                        $scope.serachdata.MMID = 0;
                        $scope.clearData();
                    }

                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/GetRawMaterial',
                        data: "{ 'prefix': '" + request.term + "','TenantID': '" + $scope.serachdata.TenatID + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('~')[0].split('`')[0],
                                    description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                    val: item.split('~')[1]
                                }
                            }))
                        }
                    });
                },
                select: function (e, i) {

                    $scope.serachdata.MMID = i.item.val;
                     //$scope.getvalidate();

                },
                minLength: 0
            });

              var TextFieldName = $("#txtWH");
                DropdownFunction(TextFieldName);
                $("#txtWH").autocomplete({
                    source: function (request, response) {
                    //     debugger;
                    //if ($('#txtTenant').val() == '' || $('#txtTenant').val() == undefined ||  $scope.serachdata.TenatID==0) {
                    //    //please shoe sticky tosat
                    //    $scope.clearData();
                    //    return false;
                    //}
                    if ($('#txtWH').val() == '' || $('#txtWH').val() == undefined) {
                        $scope.serachdata.WarehouseID = 0;
                         $scope.serachdata.MMID = 0;
                         $('#txtMcode').val('');
                    }
                        $.ajax({
                           // url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouseData") %>',
                           // data: "{ 'prefix': '" + request.term + "', 'TenantID':'"+  $scope.serachdata.TenatID+"'}",//<=cp.TenantID%>
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
                        debugger;

                        $scope.serachdata.WarehouseID = i.item.val;
                        $('#txtMcode').val('');
                        $('#txtTenant').val('');
                    },
                    minLength: 0
                });





           
            $scope.getvalidate = function () {
                debugger;

                if ($scope.serachdata.MMID == 0 || $scope.serachdata.TenatID == 0 || $scope.serachdata.WarehouseID == 0) {
                    $scope.clearData();
                    showStickyToast(false, 'Please check for Mandatory Fields', false);
                    return false;
                 }

                 var material = {
                     method: 'POST',
                     url: 'MiscellaneousIssue.aspx/getMiscellaneousList',
                     headers: {
                      'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                     },
                     data: { 'materialid': $scope.serachdata.MMID, 'WareHouseID': $scope.serachdata.WarehouseID },
                     async: false
                }   
                $http(material).success(function (response) {
                    debugger;
                    var data = JSON.parse(response.d);
                    $scope.Details = data.Table[0];
                    $scope.List = data.Table1;
                    if ($scope.List.length == 0) { showStickyToast(false, "No data found"); }
                    else { $scope.showList = true; }
                    $scope.SelectedObj = []; 
                    $scope.serachdata.Location = '';
                    $scope.serachdata.LocationID = 0;
                    $scope.serachdata.SLOC = '';
                    $scope.serachdata.SLOCID = 0;
                    $scope.serachdata.GMDID = 0;
                    $scope.serachdata.pickQuantity = 0;
                    $scope.serachdata.Remarks = '';


                });
            }
            $scope.getStorageLoc = function () {
                debugger;
                var location = {

                     method: 'POST',
                     url: 'MiscellaneousIssue.aspx/getStorageLocation',
                     headers: {
                      'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {},
                     async: false
                }
                $http(location).success(function (response) {
                    debugger;
                    var data = JSON.parse(response.d);
                    $scope.Location = data.Table;
                });
            }
          //  $scope.getStorageLoc();
            $scope.SelectedObj = [];
            $scope.loadData = function (data) {
                debugger;
                $scope.SelectedObj = data;
                $scope.serachdata.Location = data.Location;
                $scope.serachdata.SLOC = data.Code;
                $scope.serachdata.SLOCID = data.StorageLocationID;
                $scope.serachdata.GMDID = data.GoodsMovementDetailsID;
                $scope.serachdata.Topick = data.AVAILABLE;
            }
           
            $scope.pickItems = function () {
                debugger;

                if ($scope.serachdata.MMID == 0 || $scope.serachdata.TenatID == 0 || $scope.serachdata.WarehouseID == 0) {
                    $scope.clearData();
                    showStickyToast(false, 'Please check for Mandatory Fields', false);
                    return false;
                }

                if ($scope.SelectedObj.length == 0) {
                    showStickyToast(false, 'Please check atleast one reocrd', false);
                    return false;
                }
                if ($scope.serachdata.pickQuantity == '' || $scope.serachdata.pickQuantity == '0' || $scope.serachdata.pickQuantity == 0 || $scope.serachdata.pickQuantity == undefined) {
                     showStickyToast(false, 'Please Enter Quantity', false);
                    return false;
                }

                //if (parseInt($scope.serachdata.pickQuantity) > $scope.serachdata.Topick) {
                 if ($scope.serachdata.pickQuantity > $scope.serachdata.Topick) {
                     showStickyToast(false, 'Please Enter Valid Quantity', false);
                    return false;
                }
              
                if ($scope.serachdata.Remarks == '' || $scope.serachdata.Remarks == undefined) {
                     showStickyToast(false, 'Please Enter Remarks', false);
                    return false;
                }
                $scope.blockUI = true;
                var pick = {
                     method: 'POST',
                     url: 'MiscellaneousIssue.aspx/PickingItem',
                     headers: {
                      'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {'obj':$scope.serachdata},
                     async: false
                }
                $http(pick).success(function (response) {
                    showStickyToast(true,response.d);
                    $scope.getvalidate();
                     $scope.SelectedObj = [];
                    $scope.serachdata.Location = '';
                    $scope.serachdata.SLOC = '';
                    $scope.serachdata.SLOCID = 0;
                    $scope.serachdata.GMDID = 0;
                    $scope.serachdata.pickQuantity = 0;
                    $scope.serachdata.Remarks = '';
                    $scope.blockUI = false;
                });
            }


            $scope.clearData = function () {
                debugger;
            $scope.SelectedObj = [];
            $scope.Details = [];
            $scope.List = [];
            $scope.serachdata.Location = '';
            $scope.serachdata.LocationID
            $scope.serachdata.SLOC = '';
            $scope.serachdata.SLOCID = 0;
            $scope.serachdata.GMDID = 0;
            $scope.serachdata.pickQuantity = 0;
            $scope.serachdata.Remarks = '';
        }

        });
          function search(TenatID, MMID,pickQuantity,Location,LocationID,SLOC,SLOCID,Remarks,GMDID,Topick,WarehouseID) {
            this.TenatID = TenatID;
              this.MMID = MMID;
              this.pickQuantity = pickQuantity;
              this.Location = Location;
              this.LocationID = LocationID;
              this.SLOC = SLOC;
              this.SLOCID = SLOCID;
              this.Remarks = Remarks;
              this.GMDID = GMDID;
              this.Topick = Topick;
              this.WarehouseID = WarehouseID;
        }
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }


        

        
    </script>

</asp:Content>