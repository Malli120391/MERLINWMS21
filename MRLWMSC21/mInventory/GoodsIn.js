//[sP_QCCaptureChecking]

var myApp = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
myApp.controller('stockin', function ($scope, $http, $window) {
   
    debugger;
    var locationautocomid = "";
    var Containerautoid = "";
    var StorageLocationID = "";
    var isselected = 0;
    var validlocation = '';
    var SKU1 = [];
    var InbounbID =0;
    var MMID = 0;
    var Line = 0;
    var POHeaderID =0;
    var supplierinvoiceid = 0;
    $scope.hide = false
    $scope.hide1 = false;
     $scope.QChide=false;
    $scope.MandatoryList = [];
 $scope.QCValues=[];
    $scope.Receive = '';
    $scope.SupplierInvoiceID = 0;
    var storagelocationputwayid = 0;
    var InboundID = 0;
    $scope.QCserial = 0;
    if (location.href.indexOf('?ibdno=') > 0) {
       

        //ReceiveGoodIN(location, qty, qty, LineNumber, mmid, IsDamaged, LocationID, hasDiscrepancy, CreatedBy, remarks, Carton, StorageLocationID, UserID, MfgDate, ExpDate, SerialNo, BatchNo, ProjectRefNo, SPID, POheaderID) {

        $scope.goodsindata = new ReceiveGoodIN('', '', '', 0,0, 0, false, 0,'', false, 0, '', '', '','', '', '', '', '', '', '', 0, 0,0,0,'');
        var obj = location.href.split('?')[1].split('&')
        InboundID = obj[0].split('=')[1]
    }
    else {
        InboundID = 0;
    }
    
   $("#spanClose").click(function () {
        $('#divContainer').hide();
        return false;
    });
    $("#btnClose").click(function () {
        $('#divContainer').hide();
        return false;
    });
    // alert('1');
    var TenatID = 0;

    //////----------------- added by durga for getting QC parameters on 05/03/2018 -------------//

    $scope.GetQCParams = function () {
        debugger;
        var MMID = 0;
if(location.href.split('?')[1]!=undefined)
{
        var obj = location.href.split('?')[1].split('&')
        InbounbID = obj[0].split('=')[1];
        MMID = obj[1].split('=')[1];
        if (MMID != undefined) {
            var httpreqtenant = {
                method: 'POST',
                url: '../mInventory/GoodsIn.aspx/GetQCParameters',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'MMID': MMID }
            }
            $http(httpreqtenant).success(function (response) {
                $scope.QCParams = response.d;

               
            });
        }
           

}
        
          
      

       
    }
    $scope.GetQCParams();









    //-------------------------- QC Parameters end ---------------------------------------//
    $scope.GetTenat = function () {
        debugger;
        var httpreqtenant = {
            method: 'POST',
            url: '../mOutbound/CreateOutbound.aspx/getTenantData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {}
        }
        $http(httpreqtenant).success(function (response) {
            $scope.tenants = response.d;

            $scope.GetStorefeNum();
        })
    }
    var httppt = {
        method: 'POST',
        url: '../mInventory/GoodsIn.aspx/Printer',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httppt).success(function (response) {

        $scope.Printers = response.d;
    });


    var httppt = {
        method: 'POST',
        url: '../mInventory/GoodsIn.aspx/GetShipReason',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httppt).success(function (response) {

        $scope.Skiplist = response.d;
    });


 $scope.NMlocationsList = function () { 
debugger;
var Prefix=$("#txtNMLocation").val();
                if(Prefix==undefined)
            {
             Prefix="";
               }
         var nonConformity =$scope.NonConfirmity;
         var asis = $scope.AsIs;
   
        var httpNM = {
            method: 'POST',
            url: '../mInventory/GoodsIn.aspx/GetNonConformityLocationFor3PL',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
             data: { 'Prefix':Prefix ,'IsNonConformity':nonConformity,'InboundID':InbounbID},
        }
        $http(httpNM).success(function (response) {

            $scope.NMLOC = response.d;
        });
    }
 $scope.GetNMLocs = function () {
     debugger;
     if ($scope.NonConfirmity) {
         $scope.NMlocationsList();
     }
     else {
         $scope.NormalLocation = disabled;
     }
 }

/*Location Drop down*/
    $scope.locationsList = function () {
        //if ($scope.goodsindata.Location == "" || $scope.goodsindata.Location == null || $scope.goodsindata.Location == undefined) {
        //    data1 = "";
        //}
       
        debugger;  

        var Prefix =$scope.goodsindata.Location;
        if (Prefix == undefined) {
            Prefix = "";
        }

        var InboundID = getParameterByName('ibdno');
       
        //var httplc = {
        //    method: 'POST',
        //    url: '../mInventory/GoodsIn.aspx/locations',
        //    headers: {
        //        'Content-Type': 'application/json; charset=utf-8',
        //        'dataType': 'json'
        //    },
        //    data: { 'Prefix': Prefix, 'InboundID': InboundID}
        //}
        //$http(httplc).success(function (response) {

        //    $scope.Locations = response.d;
            
        //});


      
        var textfieldname = $("#txtlocation");
        DropdownFunction(textfieldname);
        $("#txtlocation").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/Loadloaction_GoodsIn',
                    data: "{ 'Prefix': '" + request.term + "','InboundID' : '" + InboundID + "'}",
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
                locationautocomid = i.item.val;
                $scope.goodsindata.Location = i.item.label;
               
                
            },
            minLength: 0
        });


        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }


    }
  //  $scope.locationsList();

/* Normal Location Drop down */
    $scope.locationsList();
    $scope.LoadVehicleLists = function (InbounbID) {
        debugger;

        //var InboundID = getParameterByName('ibdno');


        var textfieldname = $("#txtvehicled");
        DropdownFunction(textfieldname);
        $("#txtvehicled").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadVehicleList_GoodsIn',
                    data: "{ 'prefix': '" + request.term + "','InboundId' : '" + InbounbID + "'}",
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
                locationautocomid = i.item.val;
              //  $scope.goodsindata.Location = i.item.label;


            },
            minLength: 0
        });


    }
  //  $scope.LoadVehicleLists();
    var httplb = {
        method: 'POST',
        url: '../mInventory/GoodsIn.aspx/Label',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httplb).success(function (response) {
        $scope.labels = response.d;
    });

    $scope.getvehiclelist = function () {
        $scope.LoadVehicleLists(InboundID);
    }

    $scope.getalllocations = function () {

        debugger;
        $scope.locationsList();
       // $scope.GetContainers(InboundID, $scope.goodsindata.Location);
       
    }
    $scope.GetMandatory = function (MMID) {
        debugger;
        var httpreqMN = {
            method: 'POST',
            async: false,
            url: '../mInventory/GoodsIn.aspx/Getmandatory',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'MMID': MMID }
        }
        $http(httpreqMN).success(function (response) {
            //alert(response.d);
            $scope.MandatoryList = response.d;
            if ($scope.MandatoryList.length > 0) {
                var data = $.grep($scope.MandatoryList, function (a) { a.IsRequired == "true" });
            }
        })

    }
    $scope.getallcartons = function (Location) {
      
         Location = $('#txtlocation').val();
        $scope.GetContainers(InboundID,Location);
       $scope.LoadSL();
    }
    //var httpsl = {
    //    method: 'POST',
    //    url: '../mInventory/GoodsIn.aspx/SLOCs',
    //    headers: {
    //        'Content-Type': 'application/json; charset=utf-8',
    //        'dataType': 'json'
    //    },
    //    data: {}
    //}
    //$http(httpsl).success(function (response) {
    //    $scope.SLOC = response.d;
    //});

    $scope.LoadSL = function () {
        //debugger;
        var inbb = "1";
        var textfieldname1 = $("#txtStorage");
        DropdownFunction(textfieldname1);
       
        $("#txtStorage").autocomplete({
            source: function (request, response) {
                if ($("#txtStorage").val() == '') {
                    StorageLocationID = 0;
                }
                StorageLocationID = 0;
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadStorageLocation_GoodsIn',
                    data: "{ 'prefix': '" + request.term + "'}",
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
                StorageLocationID = i.item.val;
               
                debugger;
            },
            minLength: 0
        });
    }

    $scope.LoadSL();

   

    $scope.GetcontainerDetails = function () {
        debugger;
        $scope.GetContainers(InboundID, $scope.goodsindata.Location);
    }

    $scope.GetContainers = function (id, location) {

        debugger;
        $scope.Containers = '';
        var data1;
        if ($('#txtcantainer').val() == "" || $('#txtcantainer').val() == null || $('#txtcantainer').val() == undefined) {
            data1 = "";
        }
        else {
            data1 = $('#txtinvoice').val();
        }
       
        //var InboundID = 0;
        //if (location.href.indexOf('?ibdno=') > 0) {
        //    var obj = location.href.split('?')[1].split('&')
        //    InboundID = obj[0].split('=')[1]
        //}
        //else {
        //    InboundID = 0;
        //}

        var Prefix =  $('#txtcantainer').val();
        if (Prefix == undefined) {
            Prefix = "";
        }


        var textfieldname = $("#txtcantainer");
        DropdownFunction(textfieldname);
        $("#txtcantainer").autocomplete({
            source: function (request, response) {
                if ($("#txtcantainer").val()=='') {
                    Containerautoid = 0;
                }
                Containerautoid = 0;
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadContainer_GoodsIn',
                    //data: "{ 'INBID':'" + id + "','prefix': '" + request.term + "','Location' : '" + location + "'}",  Commented by lalitha on 05/03/2019
                    data: "{ 'INBID':'" + id + "','prefix': '" + request.term + "','Location' : '" + $("#txtlocation").val() + "'}",   /*added  by lalitha on 05/03/2019 for get the containers based on selected location.*/
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                         if (data.d == "" || data.d == "/,") {
                             showStickyToast(false, 'Containers are not available', false);
                          return;
                        }
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
                Containerautoid = i.item.val;

                
            },
            minLength: 0
        });

        
        //var httpct = {
        //    method: 'POST',
        //    url: '../mInventory/GoodsIn.aspx/Containers',
        //    headers: {
        //        'Content-Type': 'application/json; charset=utf-8',
        //        'dataType': 'json'
        //    },
        //    data: { 'INBID': id, 'Prefix': Prefix,'Location': location}
        //}
        //$http(httpct).success(function (response) {
            
        //    $scope.Containers = response.d;
        //});

    }


    //$scope.GetcontainerDetails = function () {
    //    debugger;
    //    var Prefix = $scope.txtcantainerID;
    //    var httpct = {
    //        method: 'POST',
    //        url: '../mInventory/GoodsIn.aspx/Containers',
    //        headers: {
    //            'Content-Type': 'application/json; charset=utf-8',
    //            'dataType': 'json'
    //        },
    //        data: { 'INBID': InboundID, 'Prefix': Prefix }
    //    }
    //    $http(httpct).success(function (response) {
    //        $scope.Containers = response.d;
    //    });
    //}
    $scope.GetTenat();
    $scope.GetStorefeNum = function () {

        var Tenant = 0;
        // $("#txtstoreid") = "";
        //$scope.txtstore = "";
        //$scope.txtskuid = "";
        if ($scope.tenants != undefined && $scope.tenants != null) {
            Tenant = $.grep($scope.tenants, function (refnum) {
                return refnum.Name == $("#txttenat").val();
            });
            if (Tenant.length == 0) {
                // showStickyToast(false, 'Please select  Tenant', false);
                return false;
            }
        }

        var TenantID = Tenant[0].Id;
        var Prefix = '';
        var httpreqtenant = {
            method: 'POST',
            url: '../mInventory/GoodsIn.aspx/GetStoreRefInfo',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': TenantID, 'Prefix': Prefix }
        }
        $http(httpreqtenant).success(function (response) {
            $scope.StoreIf = response.d;
            $scope.GetSKUList();

        })
    }
    //$scope.GetStorefeNum();
    $scope.GetSKUList = function () {
        debugger;
        //$scope.txtskuid = "";
        var Store = "";
        if ($scope.StoreIf != undefined && $scope.StoreIf != null) {
            Store = $.grep($scope.StoreIf, function (refnum) {
                return refnum.Name == $("#txtstoreid").val();
            });
            if (Store.length == 0) {
                // showStickyToast(false, 'Please select  Tenant', false);
                return false;
            }
        }
        var StoreRefId = Store[0].Id;
        var Prefix = '';
        var httpreqtenant = {
            method: 'POST',
            url: '../mInventory/GoodsIn.aspx/GetSKUS',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'InboundID': StoreRefId }
        }
        $http(httpreqtenant).success(function (response) {
            $scope.SkuInfo = response.d;
        });
    }

    $scope.GetDetails = function () {
        debugger;
        var Storere = "";
        var SKU = "";
        if ($scope.StoreIf != undefined && $scope.StoreIf != null) {
            Storere = $.grep($scope.StoreIf, function (refnum) {
                return refnum.Name == $("#txtstoreid").val();
            });
            if (Storere.length == 0) {
                showStickyToast(false, 'Please select  StoreRefNo', false);
                return false;
            }
        }

        if ($scope.SkuInfo != undefined && $scope.SkuInfo != null) {
            SKU = $.grep($scope.SkuInfo, function (refnum) {
                return refnum.Name == $("#txtsku").val();
            });
            if (SKU.length == 0) {
                showStickyToast(false, 'Please select  SKU', false);
                return false;
            }
        }

        debugger;
        var SKUID = SKU[0].Id;
        var PODetailsID = SKU[0].PODetailsID;
        var LineNumber = SKU[0].LineNumber;
        var InboundID = Storere[0].Id;
        var POHeaderID = SKU[0].POHeaderID;
        var location = $scope.txtlocationid;
        var storagelocationID = SKU[0].StorageLocationID;
        var qty = $scope.txtQtyid;
 
        window.location.href = '../mInventory/GoodsIn.aspx?ibdno=' + InboundID + '&mmid=' + SKUID + '&lno=' + LineNumber + '&PODH=' + POHeaderID + '&SIID=0';



    }
    


    $scope.GetInfo = function ()
    {
        
        if (location.href.indexOf('?ibdno=') > 0 && location.href.indexOf('&mmid=') > 0 && location.href.indexOf('&lno=') > 0 && location.href.indexOf('&PODH=') > 0) {
            var obj = location.href.split('?')[1].split('&')
             InbounbID = obj[0].split('=')[1];
             MMID = obj[1].split('=')[1];
             Line = obj[2].split('=')[1];
             POHeaderID = obj[3].split('=')[1];
            supplierinvoiceid = obj[4].split('=')[1];
            SupplierInvoiceDetailsID = obj[5].split('=')[1];
            $scope.locationsList();
            $scope.LoadVehicleLists(InbounbID);
         
            var httpreqtenant = {
                method: 'POST',
                url: '../mInventory/GoodsIn.aspx/GetSuggestedList',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'InboundID': InbounbID, 'MaterialMasterID': MMID, 'POHeaderID': POHeaderID, 'LineNumber': Line, 'SIID': supplierinvoiceid, 'SIDetID': SupplierInvoiceDetailsID },
                //async:false
            }
            $http(httpreqtenant).success(function (response) {
              
                $scope.SuggestedInfo = response.d;
                $scope.QCPendingList();
               
                if ($scope.SuggestedInfo.length != 0) {

                    $("#txttenat").val($scope.SuggestedInfo[0].TenatName);
                    $("#txtstoreid").val($scope.SuggestedInfo[0].StorefNumber);
                    $("#txtsku").val($scope.SuggestedInfo[0].Mcode);
                    
                    $scope.hide = true;
                    $scope.hide1 = true;

                    $scope.hideerrorcode = false;
                }
                else {
                    var httpreqtenant = {
                        method: 'POST',
                        url: '../mInventory/GoodsIn.aspx/GetEoorList',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: { 'InboundID': InbounbID }
                    }
                    $http(httpreqtenant).success(function (response) {
                        $scope.Suggestederrorcode = response.d;
                        if ($scope.Suggestederrorcode.length != 0) {
                            $scope.hideerrorcode = false;
                        }
                        else {
                            $scope.hideerrorcode = true;
                        }
                    });
                }
              

            })
      
           
           
        }
    }
    $scope.GetInfo();

    $scope.Getputawaylist = function (data, invoice, id)
    {
    
        debugger;
        $("#txtStorage").val("");
        $scope.goodsindata.Carton = "";
        $scope.goodsindata.Location = "";
        $scope.goodsindata.DocQty = "";
        debugger;
        var list = data;
        if (list.BatchNo != "") {
            $scope.goodsindata.BatchNo = list.BatchNo;
            $scope.noneditableBatch = true;
        }
        if (list.MfgDate != "") {
            debugger;
            $scope.goodsindata.MfgDate = list.MfgDate;

            document.getElementById('txtmfgdate').disabled = true;
           

        }
        else {
            document.getElementById('txtmfgdate').disabled = false;

        }
        if (list.ExpDate != "") {
            $scope.goodsindata.ExpDate = list.ExpDate;
            document.getElementById('txtexpdate').disabled = true;
        }
        else {
            document.getElementById('txtexpdate').disabled = false;
        }
        if (list.ProjectRef != "") {
            $scope.goodsindata.ProjectRefNo = list.ProjectRef;
            document.getElementById('txtProjectrefNo').disabled = true;
        }
        else {
            document.getElementById('txtProjectrefNo').disabled = false;
        }
        if (list.SerialNumber != "") {
            $scope.goodsindata.SerialNo = list.SerialNumber;
            document.getElementById('txtserialno').disabled = true;
        }
        else {
            document.getElementById('txtserialno').disabled = false;
        }
        if (list.MRP != "") {
            $scope.goodsindata.MRP = list.MRP;
            document.getElementById('txtMRP').disabled = true;
        }
        else
        {
            document.getElementById('txtMRP').disabled = false;
        }

        $scope.GetContainers(InboundID, list.Location);
        validlocation = list.Location;
        $scope.goodsindata.Location = list.Location;
        $scope.goodsindata.ProjectRefNo = list.ProjectRef;
        $scope.goodsindata.SerialNo = list.SerialNumber;
        $scope.SuggestedQty = list.SuggestedQty;
        $scope.SuggestedPendingQty = list.SuggestedPendingQty;
        $scope.HUNo = list.HUNo;
        $scope.HUSize = list.HUSize;

        $scope.BUoMQty = list.BUoMQty;


        storagelocationputwayid = list.SuggestedPutawayID;
        $scope.SupplierInvoiceID = list.SupplierInvoiceID;
        isselected = 1;
    }
    $scope.GetReceive = function ()
    {
        debugger;
        if ($("#txtcantainer").val() == '' || $("#txtcantainer").val() == undefined)
        {
            showStickyToast(false, 'Please select container', false);
            return;
        }


   
       
        if (isselected == 0)
        {
            showStickyToast(false, 'Please select an Item', false);
            return;
        }


        var locid
        var cartonid = Containerautoid;
        var slocid 
        if ($("#txtlocation").val() == '' || $("#txtlocation").val() == undefined) {
            showStickyToast(false, 'Please select location', false);
            return;
        }
        else
        {
            $scope.goodsindata.Location =  $("#txtlocation").val();
        }
        
    

        // commented by lalitha on 19/02/2019

        $scope.goodsindata.Carton = $("#txtcantainer").val();

        if ($("#txtStorage").val() == '' || $("#txtStorage").val() == undefined || StorageLocationID==0) {
            showStickyToast(false, 'Please select appropriate Storage Location', false);
            return;
        }
        if ($scope.goodsindata.DocQty == 0 || $scope.goodsindata.DocQty == undefined) {
            showStickyToast(false, 'Please enter Quantity', false);
            return;
        }
        if ($scope.goodsindata.DocQty < 0)
        {
            showStickyToast(false, 'Quantity Cannot be zero', false);
            return;
        }
        if ($scope.goodsindata.DocQty == '' || $scope.goodsindata.DocQty == undefined) {
            showStickyToast(false, 'Please enter Quantity', false);
            return;
        }
        

        if (validlocation != $scope.goodsindata.Location && validlocation != '') {
            if ($("#ddskipid").val() == undefined || $("#ddskipid").val() == 0 || $("#ddskipid").val() == "") {
                showStickyToast(false, 'Suggested location is changed please select skip reason', false);
                $scope.skiplist = true;
                return;
            }
        }
        else {
            $scope.skiplist = false;
           
            if ($("#ddskipid").val() == undefined || $("#ddskipid").val() == 0 || $("#ddskipid").val() == "") {
                $scope.goodsindata.SkipReasonID = "0";
            }
            else {
                var skipid = $("#ddskipid").val();
                var arr = skipid.split(':');
                skipid = arr[1];
                if (skipid != "" && skipid != undefined && skipid != null) {
                    if (validlocation == $scope.goodsindata.Location) {
                        showStickyToast(false, 'Suggested location and Receive location are same. Please unselect skip', false);
                        $scope.skiplist = false;
                        return;

                    }
                }
                
                $scope.goodsindata.SkipReasonID = skipid
            }
           
        }

      

        if ($scope.txtmfgdateid == undefined) {
            $scope.txtmfgdateid = "";
        }
        if ($scope.txtexpdateid == undefined) {
            $scope.txtexpdateid = "";
        }
        if ($scope.txtserialnoid == undefined) {
            $scope.txtserialnoid = "";
        }
        if ($scope.txtbatchid == undefined) {
            $scope.txtbatchid = "";
        }
        if ($scope.goodsindata.SerialNo == undefined || $scope.goodsindata.SerialNo == null || $scope.goodsindata.SerialNo  == "") {
            $scope.goodsindata.SerialNo = "";
        }
        else {
            if ($scope.goodsindata.DocQty != "1") {
                showStickyToast(false, 'Quantity should be 1 for serial no., cannot receive', false);
                return;
            }
        }
        if ($scope.goodsindata.ProjectRefNo == undefined || $scope.goodsindata.ProjectRefNo == null) {
            $scope.goodsindata.ProjectRefNo = "";
            
        }

        if ($("#txtvehicled").val() == '' || $("#txtvehicled").val() == undefined) {
            showStickyToast(false, 'Please select Vehicle', false);
            return;
        }

       
        var vehiclenumber = $("#txtvehicled").val();

        $scope.goodsindata.Quantity = $scope.goodsindata.DocQty;
        $scope.goodsindata.LineNumber = Line;
        $scope.goodsindata.MMID = MMID;
        $scope.goodsindata.LocationID = 1;
        $scope.goodsindata.CreatedBy = 1;
        $scope.goodsindata.StorageLocationID = 1;
        $scope.goodsindata.UserID = 1;
        $scope.goodsindata.SPID = storagelocationputwayid;
        $scope.goodsindata.POheaderID = POHeaderID;
        $scope.goodsindata.InboundID = InbounbID;
        $scope.goodsindata.MCode = $("#txtsku").val();
        $scope.goodsindata.SupplierInvoiceID = $scope.SupplierInvoiceID;
        $scope.goodsindata.StorageLocation = $("#txtStorage").val();
        $scope.goodsindata.MfgDate = $("#txtmfgdate").val();
        $scope.goodsindata.HUNo = $scope.HUNo;
        $scope.goodsindata.HUSize = $scope.HUSize;
        $scope.goodsindata.StorefNumber = $("#txtstoreid").val();

        $scope.blockUI = true;
        var httpreqtenant = {
            method: 'POST',
            url: '../mInventory/GoodsIn.aspx/SetGoodsIn',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'Rec': $scope.goodsindata, 'VehicleNumber': vehiclenumber },
            async:false,

        }
        $http(httpreqtenant).success(function (response)
        {
            console.log(response);
            if (response.d.Status)
            {
                showStickyToast(true, 'Successfully Received', false);
                document.getElementById('txtmfgdate').disabled = false;
                document.getElementById('txtexpdate').disabled = false;
                isselected = 0;
                $("#txtStorage").val("");
                $scope.goodsindata.Carton = "";
                $scope.goodsindata.Location = "";
                $scope.goodsindata.DocQty = "";
                $("#ddskipid").val("");
                
                $scope.goodsindata.BatchNo = "";
                $scope.goodsindata.SerialNo = "";
                $scope.goodsindata.ExpDate = "";
                $scope.goodsindata.MfgDate = "";
                $scope.goodsindata.ProjectRefNo = "";
                $scope.goodsindata.MRP = "";
                $scope.blockUI = false;
                setTimeout(function () {
                    $scope.GetInfo();
                }, 800);

            }else
            {
                showStickyToast(false, response.d.Message, false);
                $scope.blockUI = false;
                return;
            }
            $scope.skiplist = false;
          

        });
    }
    $scope.QCPendingList = function ()
    {
        debugger;
        var httpreqQC = {
            method: 'POST',
            async: false,
            url: '../mInventory/GoodsIn.aspx/GetQCPendinglist',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
          
            data: { 'InboundID': InbounbID, 'MaterialMasterID': MMID, 'POHeaderID': POHeaderID, 'LineNumber': Line, 'SIID': supplierinvoiceid, 'SIDetID': SupplierInvoiceDetailsID }
        }
        $http(httpreqQC).success(function (response) {
            debugger;
            $scope.GMDInfo = response.d;
            if ($scope.GMDInfo.length != 0)
            $scope.hide2 = false;
           



        });
    }


    $scope.bindlocs = function ()
    {
       
        if($scope.NonConfirmity)
        {
            $scope.QChide=true;
        }
        else
        {
         $scope.QChide=false;
        }
           $scope.NMlocationsList();
    }
    $scope.GetQCList = function (GMID)
    {
          
        var httpreqMN =
        {
            method: 'POST',
            async: false,
            url: '../mInventory/GoodsIn.aspx/GetQCList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'GMID': GMID }
        }
         $http(httpreqMN).success(function (response)
         {
               debugger;
               $scope.QCUpdatedList = response.d;
          
               $scope.TotalQty = $scope.QCUpdatedList[0].totalQty;
               $scope.CompletedQty = $scope.QCUpdatedList[0].completedQty;
         
            

        });
    }
    $scope.GetbulkQty = function (Qty, QCserial, QCList)
{
    debugger;
   
    $scope.QCserial = QCserial;
    if ($scope.QCdetailID == true) {
        QCList.Isselect = true;
        $scope.QCQuantity = $scope.QCUpdatedList[0].Quantity;
    }
    else {
        QCList.Isselect = false;
        $scope.QCQuantity = '';
    }
    return QCList;
}

//Open POPUP
    $scope.openDialog = function (title, list, id, QC) {
        debugger;
        for (var i = 0; i < $scope.QCParams.length; i++) {
            $scope.QCParams[i].value = '';
        }
        $scope.QCRefNo = '';
        $scope.QCQuantity = '';
        $scope.QCMfgDate = QC.MfgDate;
        $scope.QCExpDate = QC.ExpDate;
        $scope.QCBatchNo = QC.BatchNo;
        $scope.QCSerialNo = QC.SerialNo;
        $scope.GoodsMID = id;
        var httpreqMN = {
            method: 'POST',
            async: false,
            url: '../mInventory/GoodsIn.aspx/GetQCvalue',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'GMID': id }
        }
        $http(httpreqMN).success(function (response) {
            debugger;
            $scope.QCValues = response.d;

            if ($scope.QCValues.length > 0) {
                $scope.QCLocation = $scope.QCValues[0].Location;
                $scope.QCIsDamaged = $scope.QCValues[0].IsDamaged;
                $scope.QCHasDiscrepancy = $scope.QCValues[0].HasDiscrepancy;
                $scope.QCAsIs = $scope.QCValues[0].AsIs;
                $scope.QCIsNonConfirmity = $scope.QCValues[0].IsNonConfirmity;
            }


        });
        $scope.GetQCList(id);
        $('#divContainer').show();
        //$scope.GetParametersList(id);
    }



//Dynamic Parameters

    $scope.disablenonconfirm = function () {
        debugger;
        $scope.isDisabled = $scope.AsIs;
    }

    $scope.CheckPArentSelected = function (obj)
    {
        debugger;
        for (var i = 0; i < $scope.GMDInfo.length; i++)
        {
           
            if (obj.GoodsMovementDetailsID == $scope.GMDInfo[i].GoodsMovementDetailsID)
            {
                if (obj.IsSelected == true) {
                    $scope.GMDInfo[i].IsSelected = true;
                }
                else {
                    $scope.GMDInfo[i].IsSelected = false;   
                }
            }

        }
    }
    $scope.selectAll = function () {
        debugger;

        console.log($("#Deleteall").is(":checked"));
        // console.log($("#MainContent_IBContent_GVPOLineItems_action").prop("checked"));
        if ($("#Deleteall").prop("checked") == true) {
            $(".alldel").prop("checked", true);
            for (var i = 0; i < $scope.GMDInfo.length; i++) 
            {

                $scope.GMDInfo[i].IsSelected = true;
            }
        }
        else {
            $(".alldel").prop("checked", false);
            for (var i = 0; i < $scope.GMDInfo.length; i++) {
                $scope.GMDInfo[i].IsSelected = false;

            }
        }

    }   
    // added by lalitha on 18/02/2018
    // Commented by swamy on 16 NOV 2019
    $scope.deleteQCitem = function ()
    {
        debugger;

        var items = $.grep($scope.GMDInfo, function (strn)
        {
            return (strn.IsSelected == true);
        });
        if (items.length == 0)
        {
            showStickyToast(false, 'Please select atleast one Checkbox ', false);
            return false;
        }
        if ($window.confirm("Are you sure do you want to delete?"))
        {
            var httpreqdt = {
                method: 'POST',
                url: 'GoodsIn.aspx/DeletedItem',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'Items': items }
            }
            $http(httpreqdt).success(function (response)
            {
                if (response.d.Status)
                {
                    showStickyToast(true, 'The item deleted successfully', false);
                    $scope.QCPendingList();
                } else
                {
                    showStickyToast(false, response.d.Message, false);
                }
               
                
            
            });

          
            
            $('#Deleteall').attr('checked', false);
            $('#Deleteall').prop('checked', false);
            $scope.GetInfo();


        }

    }


   // added by swamy on 16/NOV/2019

    $scope.RevertReceiving = function ()
    {

        console.log($scope.GMDInfo[0].SupplierInvoiceDetailsID);

      
        if ($window.confirm("Are you sure do you want to delete?"))
        {
            var httpreqdt =
            {
                method: 'POST',
                url: 'GoodsIn.aspx/RevertReceivingLineItem',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'supplierInvoiceDetailsID': $scope.GMDInfo[0].SupplierInvoiceDetailsID }
            }
            $http(httpreqdt).success(function (response)
            {
                if (response.d.Status) {
                    $scope.QCPendingList();
                    showStickyToast(true, 'The item deleted successfully', false);
                } else
                {
                    showStickyToast(false, response.d.Message, false);

                }
               
            });

           
        


            $scope.GetInfo();


        }

    }

        $scope.deleteQCCaptureitem = function (QCList) {
            debugger;
            //if($window.confirm("Are you sure you want to delete?"))
            //{
            // alert("Are you sure do you want to delete?");
            if (confirm("Are you sure do you want to delete?")) {


                var httpreqdt = {
                    method: 'POST',
                    url: 'GoodsIn.aspx/DeletedQCCaptureItem',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'qcdata': QCList }
                }
                $http(httpreqdt).success(function (response) {
                    showStickyToast(true, 'The item deleted successfully', false);
                    $scope.GetQCList(QCList.GoodsMovementDetailsID);
                    $scope.QCPendingList();
                });

            }
            else {

            }


            //}

        }
        $scope.saveBulkQCItemsParameters = function () {
            debugger;
            var data;
            data = $.grep($scope.QCUpdatedList, function (po) {
                return (po.Isselect == true);
            });
            var selecteddata = data;
            if (data == undefined || data == null || data.length == 0) {
                showStickyToast(false, 'Please select QC parameters', false);
                return false;
            }
            if ($scope.QCQuantity == undefined || $scope.QCQuantity == '') {
                showStickyToast(false, 'Please Enter QC Qty.', false);
                return false;
            }
            else if ($scope.QCQuantity == '0') {
                showStickyToast(false, 'Please Enter valid QC Qty.', false);
                return false;
            }
            data = null;
            data = $.grep($scope.QCUpdatedList, function (po) {
                return (po.SerailName != $scope.QCRefNo && po.SerailName!='');
            });
            var totalqty = 0;
            if (data != undefined || data != null) {
                if (data.length != 0) {
                    var quantity = 0;
                    totalqty = data[0].totalQty;
                    for (var j = 0; j < data.length; j++) {
                        quantity = quantity + parseFloat(data[j].Quantity);
                    }
                    if (totalqty < (quantity + parseFloat($scope.QCQuantity))) {
                        showStickyToast(false, 'QC Qty. Exceeded', false);
                        return false;
                    }
                }
                
            }
           
         
            var count = 0;
            var IsNonConfirmity = 0;
            if ($scope.QCParams != undefined && $scope.QCParams != null) {
                if ($scope.QCParams.length != 0) {
                    for (var i = 0; i < $scope.QCParams.length; i++) {
                        if ($scope.QCParams[i].value == "" || $scope.QCParams[i].value==undefined) {
                            showStickyToast(false, 'Please enter ' + $scope.QCParams[i].ParameterName + ' value', false);
                            return false;
                        }
                        if ($scope.QCParams[i].MinTolerance > parseFloat($scope.QCParams[i].value)) {
                            count = count + 1;
                        }
                        else if ($scope.QCParams[i].MaxTolerance < parseFloat($scope.QCParams[i].value)) {
                            count = count + 1;
                        }
                        //if ($scope.QCParams[i].MinTolerance > parseFloat($scope.QCParams[i].value)) {
                        //    showStickyToast(false, $scope.QCParams[i].ParameterName + ' Should be greater than or equal to ' + $scope.QCParams[i].MinTolerance, false);
                        //    return false;
                        //}
                        //if ($scope.QCParams[i].MaxTolerance < parseFloat($scope.QCParams[i].value)) {
                        //    showStickyToast(false, $scope.QCParams[i].ParameterName + ' Should be less than or equal to ' + $scope.QCParams[i].MaxTolerance, false);
                        //    return false;
                        //}
                    }
                }
            }
            if (count != 0) {
                IsNonConfirmity = 1;
            }
            var httpreqtenant = {
                method: 'POST',
                url: '../mInventory/GoodsIn.aspx/AddParameters',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: {'qcdata': selecteddata[0], 'qcparams': $scope.QCParams, 'Quantity': $scope.QCQuantity, 'NonConfirmity': IsNonConfirmity}
            }
            $http(httpreqtenant).success(function (response) {
                debugger;
                if (response.d == 'success') {

                    $scope.GetQCList($scope.QCUpdatedList[0].GoodsMovementDetailsID);
                        
                    for (var i = 0; i < $scope.QCParams.length; i++) {
                        $scope.QCParams[i].value = '';
                    }
                    $scope.QCRefNo = '';
                    $scope.QCQuantity = '';
                    showStickyToast(true, 'QC parameters  Captured Successfully', false);
                     $scope.QCPendingList();
                    return false;
                }
                else {
                    showStickyToast(true, 'Error while Capturing', false);
                    return false;
                }


            });

           
            
        }
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }
    $scope.UpdatePrimary = function (contact, index) {
        debugger;

        for (var i = 0; i < $scope.QCParams.length; i++) {
            $scope.QCParams[i].value = '';
        }
        for (var i = 0; i < $scope.QCUpdatedList.length; i++) {
            if ($scope.QCUpdatedList[i].SerailName == contact.SerailName) {
                $scope.QCUpdatedList[i].Isselect = true;
            }
            else {
                $scope.QCUpdatedList[i].Isselect = false;
            }
        }
        //contact.Isselect = true;
        $scope.QCQuantity = contact.Quantity;
        $scope.QCRefNo = contact.SerailName;
        $scope.SerialNo = contact.QCSerialNo;
        if (contact.QCSerialNo != '') {
            var qcvalues = contact.paramvalues;
            for (var i = 0; i < $scope.QCParams.length; i++) {
                for (var j = 0; j < qcvalues.length; j++) {
                    if ($scope.QCParams[i].QualityParameterID == qcvalues[j].QualityParameterID) {
                        $scope.QCParams[i].value = qcvalues[j].value;
                    }
                }
            }
        }
        else {
            for (var i = 0; i < $scope.QCParams.length; i++) {
                $scope.QCParams[i].value = '';
            }

        }

        //return contact;

    }

   

    $scope.backTo = function () {
        debugger;
        console.log(InbounbID);

        window.location.href = "../mInbound/RTReport.aspx?ibdid=" + InbounbID;
    }
    function ReceiveGoodIN(location, qty, qty, LineNumber, InboundID, mmid, IsDamaged, MCode, LocationID, hasDiscrepancy, CreatedBy, remarks, Carton, StorageLocationID, StorageLocation, UserID, MfgDate, ExpDate, SerialNo, BatchNo, ProjectRefNo, SPID, POheaderID, SupplierInvoiceID, SkipReasonID, MRP) {

        this.Location = location;
        this.DocQty = qty;
        this.Quantity = qty;
        this.LineNumber = LineNumber;
        this.InboundID = InboundID;
        this.MMID = mmid;
        this.IsDamaged = IsDamaged;
        this.MCode = MCode;
        this.LocationID = LocationID;
        this.hasDiscrepancy = hasDiscrepancy;
        this.CreatedBy = CreatedBy;
        this.remarks = remarks;
        this.Carton = Carton;
        this.StorageLocationID = StorageLocationID;
        this.StorageLocation = StorageLocation;
        this.UserID = UserID;
        this.MfgDate = MfgDate;
        this.ExpDate = ExpDate;
        this.BatchNo = BatchNo;
        this.SerialNo = SerialNo;
        this.ProjectRefNo = ProjectRefNo;
        this.SPID = SPID;
        this.POheaderID = POheaderID;
        this.SupplierInvoiceID = SupplierInvoiceID;
        this.SkipReasonID = SkipReasonID;
        this.MRP = MRP;
    }
});