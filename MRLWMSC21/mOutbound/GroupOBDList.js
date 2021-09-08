var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('GroupOutbound', function ($scope, $http) {
    var RefTenant = '';
    var Refvlpd = '';
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetTenantList',
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
            RefTenant = i.item.val;
            // alert(Refnumber);
            //$scope.ngtenant = i.item.val;
        },
        minLength: 0
    });
    var textfieldname1 = $("#txtWH");
    DropdownFunction(textfieldname1);
    $("#txtWH").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadWarehouseData1',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant+"'}",
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
            RefWHID = i.item.val;
            // alert(Refnumber);
            //$scope.ngtenant = i.item.val;
        },
        minLength: 0
    });
    function ClearVLPD() {
        $("#txtVLPD").val() = '';
    }
    $scope.GetgroupOBDList = function () {
        var httpreqtenant = {
            method: 'POST',
            url: '../mOutbound/GroupOBDList.aspx/getGroupOUTBOUNDDataForGroupOBD',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'VLPDID': 0,'TenantID':0,'WHID':0}
        }
        $http(httpreqtenant).success(function (response) {
            debugger;
            $scope.GroupOBDList = response.d;
        });

    }

    $scope.Getdetails = function () {
        debugger;
        if ($("#txtTenant").val() == "") {
            showStickyToast(false, "Please select Tenant", false);
            return;
        }
        if ($("#txtWH").val() == "") {
            showStickyToast(false, "Please select Warehouse", false);
            return;
        }
        var vlpd = $("#txtVLPD").val();
        
      
        var vlpdid = Refvlpd;
        if (vlpd == "") {
            //vlpdid.val('');
            vlpdid = 0;
        }
        var httpreqtenant = {
            method: 'POST',
            url: '../mOutbound/GroupOBDList.aspx/getGroupOUTBOUNDDataForGroupOBD',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'VLPDID': vlpdid, 'TenantID': parseInt(RefTenant), 'WHID': parseInt(RefWHID) }
        }
        $http(httpreqtenant).success(function (response) {
            
            $scope.GroupOBDList = response.d;
            if ($scope.GroupOBDList.length == 0) {
                showStickyToast(false, "No data found", false);
                return;
            }
        });

    }
    $scope.GetgroupOBDList();

    $scope.getVLPD = function () {
       
       
        var tenantid = RefTenant;
        $("#txtVLPD").val('');
        var textfieldname = $("#txtVLPD");
        DropdownFunction(textfieldname);
        $("#txtVLPD").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadVLPDS1',
                    data: "{ 'prefix': '" + request.term + "','TenantID':'" + tenantid + "','WarehouseID':'" + RefWHID+"'}",
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
                Refvlpd = i.item.val;
                // alert(Refnumber);
                //$scope.ngtenant = i.item.val;
            },
            minLength: 0
        });

    }
    //For Initiate Pick Up
    $scope.changeVLPDStatus = function (VLPDID) {   
            
       
            var httpreqsft = {
                method: 'POST',
                url: '../mOutbound/GroupOBDList.aspx/InitiatePickUp',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'VLPDID': VLPDID },

            }
            $http(httpreqsft).success(function (response) {
                if (response.d != undefined && response.d != '') {
                    showStickyToast(true, 'Initiated Successfully', false);
                    $scope.GetgroupOBDList();
                }
                else {
                }
            });


    }
    $('#spanClose').click(function (event) {
        $('#divContainer').hide();
    });

    $('#btnClose').click(function (event) {
        $('#divContainer').hide();
    });

    $scope.closeData = function () {
        $('#divContainer').hide();
    }

    $scope.openDialog = function (title,VLPDID,VLPDNO) {
        
        debugger; $scope.VLPDNO = VLPDNO;
        //var vlpdid = Refvlpd;
        var httpreqtenant = {
            method: 'POST',
            url: '../mOutbound/GroupOBDList.aspx/getOBDList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'VLPDID': VLPDID }
        }
        $http(httpreqtenant).success(function (response) {
            
            $scope.OBDList = response.d;
        });
        $('#divContainer').show();
    }
   // Verify the details
    $scope.VerifyVLPD = function (VLPDID) {
        
     
        var httpreqsft1 = {
            method: 'POST',
            url: '../mOutbound/GroupOBDList.aspx/UpsertVerification',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'VLPDID': VLPDID }
        }
        $http(httpreqsft1).success(function (response) {
            debugger;
            if (response.d != undefined && response.d != '') {
                if (response.d == "Quantity is Not Avaiable") {
                    showStickyToast(false, 'Quantity is Not Avaiable', false);
                    return;
                }
                else if (response.d == "Picking is not at done") {
                    showStickyToast(false, 'Picking has not yet been completed', false);
                    return;
                }
                else if (response.d == "Verification is done") {
                    showStickyToast(true, 'Verification is done', false);
                    $scope.GetgroupOBDList();
                    return;
                }
                else if (response.d == "Verification is not at done") {
                    showStickyToast(false, 'Picking has not yet been completed', false);
                    return;
                }
                else {
                    showStickyToast(false, 'Error While verification', false);
                    return;
                }
               
            }
            else {
                showStickyToast(false, 'Error While verification', false);
                return;
            }
        });

    }
 
});