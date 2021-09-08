var app = angular.module('MyApp', ['angularUtils.directives.dirPagination']);


app.controller('BOMDetails', function ($scope, $http, $timeout) {
    $scope.IsMappedtoJOB = 0;
    $scope.RefTenant = 0;
    $scope.ParentMMID = 0;
    $scope.ParentUOM = '';
    $scope.ParentUOMID = 0;
    $scope.BOMHeaderData = new BOMData(0, '', 0, '', 0, '', 0, '', 0, '', '');
    $scope.BomDetails = new BOMDetailsData(0, 0, '', 0, '', '', 0);
    $scope.IsheaderCreated = false;

    var accounts = {
        method: 'POST',
        url: 'BOM.aspx/GetCurrentAccount',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(accounts).success(function (response) {
        debugger;
       
        $scope.BOMHeaderData.AccountId = response.d;

    });


    if (location.href.indexOf('?BOMID=') > 0) {
        $scope.IsheaderCreated = true;
        var accounts = {
            method: 'POST',
            url: 'BOM.aspx/CheckBOMWithJOBHeader',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'bomid': location.href.split('?BOMID=')[1] }
        }
        $http(accounts).success(function (response) {
            if (response.d > 0) {
                $scope.IsMappedtoJOB = 1
            }
            else {
                $scope.IsMappedtoJOB = 0;
            }
        });
    }
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            if ($scope.BOMHeaderData.Tenant == '' || $scope.BOMHeaderData.Tenant == undefined) {
                $scope.BOMHeaderData.TenantId = 0;
            }
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
            $scope.BOMHeaderData.TenantId = 0;
            $scope.BOMHeaderData.TenantId = i.item.val;
            // alert(Refnumber);
            //$scope.ngtenant = i.item.val;
        },
        minLength: 0
    });





    var textfieldname = $("#txtParentPartNo");
    DropdownFunction(textfieldname);
    $("#txtParentPartNo").autocomplete({
        source: function (request, response) {
            if ($scope.BOMHeaderData.TenantId == '' || $scope.BOMHeaderData.TenantId == undefined || $scope.BOMHeaderData.TenantId == 0 || $scope.BOMHeaderData.TenantId == "0") {
                return false;
            }
            if ($scope.BOMHeaderData.MCode == '' || $scope.BOMHeaderData.MCode == undefined) {
                $scope.BOMHeaderData.MMID = 0;
                $scope.BOMHeaderData.UOM = '';
                $scope.BOMHeaderData.UOMID = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/getParentMcodesForBOM',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + $scope.BOMHeaderData.TenantId + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1],
                            UOM: item.split(',')[2],
                            UOMID: item.split(',')[3]
                        }
                    }))
                }
            });
        },
        select: function (e, i) {
            $scope.BOMHeaderData.MMID = i.item.val;
            $scope.BOMHeaderData.UOM = i.item.UOM;
            $('#txtUOM').val(i.item.UOM);
            $scope.BOMHeaderData.UOMID = i.item.UOMID;
            console.log($scope.BOMHeaderData.MMID);
            console.log($scope.BOMHeaderData.UOM);
            console.log($scope.BOMHeaderData.UOMID);
        },
        minLength: 0
    });





    var textfieldname = $("#txtlinepartno");
    DropdownFunction(textfieldname);
    $("#txtlinepartno").autocomplete({

        source: function (request, response) {
            if ($scope.BomDetails.MCode == '' || $scope.BomDetails.MCode == undefined) {
                $scope.BomDetails.MMID = '0';
            }
            //$('#txtRTUoM').val('');
            //$('#txtQuantity').val('');
            //$scope.BomDetails.UOMID = '0';
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/getChildLineMcodesForBOM',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + $scope.BOMHeaderData.TenantId + "','BOMid':'" + $scope.BOMHeaderData.BOMID + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1],
                           
                        }
                    }))
                }
            });
        },
        select: function (e, i) {
            $scope.BomDetails.MMID = '0';
            $scope.BomDetails.MMID = i.item.val;
           
        },
        minLength: 0
    });



    var textfieldname = $("#txtRTUoM");
    DropdownFunction(textfieldname);
    $("#txtRTUoM").autocomplete({
        source: function (request, response) {
            debugger;
            if ($scope.BomDetails.UOM == '' || $scope.BomDetails.UOM == undefined) {
                $scope.BomDetails.UOMID = '0';
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/getChildLineMcodesUOMForBOM',
                data: "{ 'MMID': '" + $scope.BomDetails.MMID + "'}",
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
            $scope.BomDetails.UOMID = '0';
            $scope.BomDetails.UOMID = i.item.val;

        },
        minLength: 0
    });



    var accounts = {
        method: 'POST',
        url: 'BOM.aspx/GetAccounts',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(accounts).success(function (response) {
        $scope.AccountData = response.d;
    });
    $scope.getBOMHeaderData = function () {
        if (location.href.indexOf('?BOMID=') > 0) {
            var accounts = {
                method: 'POST',
                url: 'BOM.aspx/GetBOMHeaderData',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'bomid': location.href.split('?BOMID=')[1]}
            }
            $http(accounts).success(function (response) {
                debugger;
                console.log(response.d);
                var data = response.d;
                $scope.BOMHeaderData = new BOMData(parseInt(data.BOMID), data.BOMRefNo, data.TenantId, data.Tenant, parseInt(data.AccountId), data.Account, data.MMID, data.MCode, data.UOMID, data.UOM, data.Remarks);
            });
        }
    }
    $scope.getBOMHeaderData();
    $scope.getBOMDetailsData = function () {
        if (location.href.indexOf('?BOMID=') > 0) {
            var accounts = {
                method: 'POST',
                url: 'BOM.aspx/GetBOMDetails',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'objId': location.href.split('?BOMID=')[1] }
            }
            $http(accounts).success(function (response) {
                debugger;
                console.log(response.d);
                $scope.BomDetailsdata = response.d;
                
            });
        }
    }
    $scope.getBOMDetailsData();

    $scope.saveBOMHeader = function () {
        debugger;
        var data = $scope.BOMHeaderData;
        if ($scope.BOMHeaderData.AccountId == undefined || $scope.BOMHeaderData.AccountId == 0) {
            showStickyToast(false, "Please select Account ");
            return false;
        }
        else if ($scope.BOMHeaderData.TenantId == undefined || $scope.BOMHeaderData.TenantId == "" || $scope.BOMHeaderData.TenantId == "0") {
            showStickyToast(false, "Please select Tenant ");
            return false;
        }
        else if ($scope.BOMHeaderData.MMID == undefined || $scope.BOMHeaderData.MMID == "" || $scope.BOMHeaderData.MMID == "0") {
            showStickyToast(false, "Please select Part No. ");
            return false;
        }
        if ($scope.BomDetailsdata != undefined) {
            if ($scope.BomDetailsdata.length != 0) {
                showStickyToast(false, "Unable to update as the line item is added ");
                return false;
            }
        }
        
        var accounts = {
            method: 'POST',
            url: 'BOM.aspx/UpsertBOM',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.BOMHeaderData}
        }
        $http(accounts).success(function (response) {
            if (response.d > 0) {
                if ($scope.BOMHeaderData.BOMID != '' || $scope.BOMHeaderData.BOMID != 0 || $scope.BOMHeaderData.BOMID != '0') {
                    $scope.getBOMHeaderData();
                    showStickyToast(true, "Successfully updated ");
                    return false;
                }
                else {
                    showStickyToast(true, "Successfully Created ");
                    $timeout(function () { window.location.href = '../mMaterialManagement/BOM.aspx?BOMID=' + response.d; }, 1500);
                   
                }
               
            }
            else {
                showStickyToast(false, "Error While Creating ");
            }
            
        });
        
    }
    $scope.saveBOMDetails = function () {
        debugger;
        var data = $scope.BomDetails;
        if (data.MMID == 0 || data.MMID == "0" || data.MMID == '') {
            showStickyToast(false, "Please select Part No. ");
            return false;
        }
        else if (data.UOMID == 0 || data.UOMID == "0" || data.UOMID == '') {
            showStickyToast(false, "Please select UOM ");
            return false;
        }
        else if (data.Quantity == '') {
            showStickyToast(false, "Please enter Quantity ");
            return false;
        }
        else if (data.Quantity == 0 || data.UOMID == "0") {
            showStickyToast(false, "Please enter valid Quantity ");
            return false;
        }
        else if (parseInt(data.Quantity) != data.Quantity) {
            showStickyToast(false, "Please enter only integer values for Quantity  ");
            return false;
        }
        $scope.BomDetails.BOMID = location.href.split('?BOMID=')[1];
        var accounts = {
            method: 'POST',
            url: 'BOM.aspx/UpsertBOMDetails',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.BomDetails }
        }
        $http(accounts).success(function (response) {
            if (response.d == -222) {
                showStickyToast(false, "Unable to Add/update, BOM Mapped to JOB Order ");
                return false;

            }
            if (response.d > 0) {
                
                    showStickyToast(true, "Successfully Created ");
                   
               
                $scope.getBOMDetailsData();
                $("#SupModal").modal('hide');

            }
            else {
                showStickyToast(false, "Error While Creating ");
            }
            
        });

    }
    $scope.updateBOMDetails = function (obj) {
        debugger;

        //$("#SupModal").modal({
        //    show: 'true'
        //});
      
       // $scope.BomDetails = new BOMDetailsData(obj.BOMID, obj.MMID, obj.MCode, obj.UOMID, obj.UOM, obj.Quantity, obj.BOMDetailsID);
        //BOMDetailsData(BOMID, mmid, mcode, UOMId, uom, quantity, bomdetailsid);
        //if ($scope.IsMappedtoJOB > 0) {
        //    showStickyToast(false, "Unable to update, BOM Mapped to JOB Order ");
        //    return false;
        //}
        if (obj.UPdatedQuantity == "") {
            showStickyToast(false, "Please enter Quantity ");
            return false;
        }
        else if (obj.UPdatedQuantity == "0")
        {
            showStickyToast(false, "Please enter valid Quantity ");
            return false;
        }
        else if (parseInt(obj.UPdatedQuantity) != obj.UPdatedQuantity) {
            showStickyToast(false, "Please enter only integer values for Quantity  ");
            return false;
        }
        obj.Quantity = obj.UPdatedQuantity;
        var accounts = {
            method: 'POST',
            url: 'BOM.aspx/UpsertBOMDetails',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': obj }
        }
        $http(accounts).success(function (response) {
            if (response.d == -222) {
                showStickyToast(false, "Unable to Add/update, BOM Mapped to JOB Order ");
                return false;

            }
            if (response.d > 0) {
               
                 showStickyToast(true, "Successfully Updated ");

              
                $scope.getBOMDetailsData();
              

            }
            else {
                showStickyToast(false, "Error While Creating ");
            }

        });


    }
    $scope.DeleteBOMDetails = function (obj) {
        debugger;
        //if ($scope.IsMappedtoJOB > 0) {
        //    showStickyToast(false, "Unable to delete, BOM Mapped to JOB Order ");
        //    return false;
        //}
        if (confirm("Are you sure want to Delete?")) {
            var accounts = {
                method: 'POST',
                url: 'BOM.aspx/DeleteBOMDetails',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'bomdetailsid': obj.BOMDetailsID, 'bomid': location.href.split('?BOMID=')[1] }
            }
            $http(accounts).success(function (response) {
                if (response.d == -222) {
                    showStickyToast(false, "Unable to Delete, BOM Mapped to JOB Order ");
                    return false;

                }
                if (response.d > 0) {
                    showStickyToast(true, "Successfully Deleted ");


                    $scope.getBOMDetailsData();
                }
                else {
                    showStickyToast(true, "Error while deleting ");
                }
            });
        }
       

    }
    $scope.cleardata = function () {
        debugger;
       
        $('#txtlinepartno').val('');
        $('#txtRTUoM').val('');
        $scope.BomDetails = new BOMDetailsData(0, 0, '', 0, '', '', 0);
    }
    $scope.cleardataWhenOpen = function () {
        debugger;
        var accounts = {
            method: 'POST',
            url: 'BOM.aspx/CheckBOMWithJOBHeader',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'bomid': location.href.split('?BOMID=')[1] }
        }
        $http(accounts).success(function (response) {
            if (response.d > 0) {

                $scope.IsMappedtoJOB = 1
                if ($scope.IsMappedtoJOB > 0) {
                    showStickyToast(false, "Unable to add, BOM Mapped to JOB Order ");
                    return false;
                }
            }
            else {
                $("#SupModal").modal({
                    show: 'true'
                });
                $scope.IsMappedtoJOB = 0;
            }
        });
        
       
        $scope.BomDetails = new BOMDetailsData(0, 0, '', 0, '', '', 0);
        $('#txtlinepartno').val('');
        $('#txtRTUoM').val('');
    }
});

function BOMData(bomid, bomrefno, tenantid, tenant, accountid, account, mmid, mcode, UOMId, uom, remarks) {
    this.BOMID = bomid;
    this.BOMRefNo = bomrefno;
    this.TenantId = tenantid;
    this.Tenant = tenant;
    this.AccountId = accountid;
    this.Account = account;
    this.MMID = mmid;
    this.MCode = mcode;
    this.UOMID = UOMId;
    this.UOM = uom;
    this.Remarks = remarks;

}
function BOMDetailsData(bomid, mmid, mcode, UOMId, uom, quantity, bomdetailsid) {
    this.BOMDetailsID = bomdetailsid;
    this.BOMID = bomid;
    this.MMID = mmid;
    this.MCode = mcode;
    this.UOMID = UOMId;
    this.UOM = uom;
    this.Quantity = quantity;

}
function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;
    return true;
}











//------------------------------------ BOM List -------------------------------------//
var app = angular.module('ListMyApp', ['angularUtils.directives.dirPagination']);


app.controller('BOMList', function ($scope, $http, $timeout) {
var tenantid=0;
$scope.search=new BOMListSearch(0,'','','',0);
  var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
debugger;
            if ( $scope.search.Tenant == '' ||  $scope.search.Tenant == undefined) {
                 $scope.search.TenantId = 0;
            }
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
           $scope.search.TenantId = 0;
            $scope.search.TenantId = i.item.val;
            // alert(Refnumber);
            //$scope.ngtenant = i.item.val;
        },
        minLength: 0
    });


var textfieldname = $("#txtParentPartNo");
    DropdownFunction(textfieldname);
    $("#txtParentPartNo").autocomplete({
        source: function (request, response) {
            if ( $scope.search.TenantId == '' ||  $scope.search.TenantId == undefined) {
            showStickyToast(false, "Please select Tenant ");
            return false;
            }
         $scope.search.MMID =0;
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/getParentMcodesForBOMList',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + $scope.search.TenantId + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1],
                            UOM: item.split(',')[2],
                            UOMID: item.split(',')[3]
                        }
                    }))
                }
            });
        },
        select: function (e, i) {
            $scope.search.MMID =0;
            $scope.search.MMID = i.item.val;
          
          
           
        },
        minLength: 0
    });


    $scope.getBOMHeaderData = function () {
debugger;
       var data=$scope.search;
            var accounts = {
                method: 'POST',
                url: 'BOMList.aspx/GetBOMHeaderData',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: {'obj':data}
            }
            $http(accounts).success(function (response) {
                debugger;
                
                $scope.BOMList = response.d;
                console.log($scope.BOMList);
                
            });
       
    }
    $scope.getBOMHeaderData();

    
});
function BOMListSearch(tenantid,tenant,bomrefno,partno,mmid)
{
    this.TenantId=tenantid;
    this.Tenant=tenant;
    this.BomRefNo=bomrefno;
    this.PartNo=partno;
    this.MMID=mmid;
}