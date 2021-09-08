var app = angular.module('MyApp', ['angularUtils.directives.dirPagination']);


app.controller('JOBOrder', function ($scope, $http, $timeout) {

    $scope.JOBHeader = new JOBOrderHeader(0, '', 0, '', 0, '', 0, '', '', '', 0, 'NEW', 0,0);
    $scope.JoborderTypes = [{ ID: 1, Value: 'Nesting' }, { ID: 2, Value: 'De Nesting' }]
    $scope.hidedata = false;

   
    var accounts = {
        method: 'POST',
        url: 'JobOrder.aspx/GetCurrentAccount',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(accounts).success(function (response) {
        debugger;

        $scope.JOBHeader.AccountId = response.d;

    });

    var httpWH = {
        method: 'POST',
        url: '../mMaterialManagement/JobOrder.aspx/getWareHouseData',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpWH).success(function (response) {
        $scope.WareHouseData = response.d;
        console.log($scope.WareHouseData);
    });
    $scope.changemenulink = function () {
        window.location.href = '../mMaterialManagement/JobOrderList.aspx';
    }
    var accounts = {
        method: 'POST',
        url: 'JobOrder.aspx/GetAccounts',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(accounts).success(function (response) {
        $scope.AccountData = response.d;
        console.log($scope.AccountData);
    });




    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            if ($scope.JOBHeader.Tenant == '' || $scope.JOBHeader.Tenant == undefined) {
                $scope.JOBHeader.TenantId = 0;
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
            $scope.JOBHeader.TenantId = 0;
            $scope.JOBHeader.BOMId = 0;
            $('#txtbomrefno').val('');
            $scope.JOBHeader.BOMRefNo = '';
            $scope.JOBHeader.TenantId = i.item.val;
            // alert(Refnumber);
            //$scope.ngtenant = i.item.val;
        },
        minLength: 0
    });


    var textfieldname = $("#txtbomrefno");
    DropdownFunction(textfieldname);
    $("#txtbomrefno").autocomplete({
        source: function (request, response) {
            if ($scope.JOBHeader.TenantId == '' || $scope.JOBHeader.TenantId == undefined) {
                showStickyToast(false, "Please select Tenant ");
                return false;
            }
            if ($scope.JOBHeader.BOMRefNo == '' || $scope.JOBHeader.BOMRefNo == undefined) {
                $scope.JOBHeader.BOMId = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/getBOMRefNo',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + $scope.JOBHeader.TenantId + "'}",
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
            $scope.JOBHeader.BOMId = 0;
            $scope.JOBHeader.BOMId = i.item.val;
            
        },
        minLength: 0
    });

    $scope.saveJOBHeader = function () {
        debugger;
        if ($scope.JOBHeader.AccountId == undefined || $scope.JOBHeader.AccountId == 0) {
            showStickyToast(false, "Please select Account ");
            return false;
        }
        else if ($scope.JOBHeader.TenantId == undefined || $scope.JOBHeader.TenantId == "" || $scope.JOBHeader.TenantId == "0") {
            showStickyToast(false, "Please select Tenant ");
            return false;
        }
        else if ($scope.JOBHeader.BOMId == undefined || $scope.JOBHeader.BOMId == "" || $scope.JOBHeader.BOMId == "0") {
            showStickyToast(false, "Please select BOM Ref No. ");
            return false;
        }
        else if ($scope.JOBHeader.Quantity == undefined || $scope.JOBHeader.Quantity == "") {
            showStickyToast(false, "Please enter Quantity ");
            return false;
        }
        else if ($scope.JOBHeader.Quantity == "0") {
            showStickyToast(false, "Please enter valid Quantity ");
            return false;
        }
        else if ($scope.JOBHeader.JobOrderTypeId == undefined || $scope.JOBHeader.JobOrderTypeId == "") {
            showStickyToast(false, "Please select JOB Order Type ");
            return false;
        }
    else if ($scope.JOBHeader.WareHouseId == undefined || $scope.JOBHeader.WareHouseId == "" ||  $scope.JOBHeader.WareHouseId == 0) {
            showStickyToast(false, "Please select Warehouse ");
            return false;
        }
        $scope.blockUI = true;
        var job = {
            method: 'POST',
            url: 'JobOrder.aspx/UpsertJOBOrderHeader',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.JOBHeader }
        }
        $http(job).success(function (response) {
            debugger;

            if (response.d > 0) {
                if ($scope.JOBHeader.JOBOrderId != '' || $scope.JOBHeader.JOBOrderId != 0 || $scope.JOBHeader.JOBOrderId != '0') {
                    $scope.getJOBHeaderData();
                    $scope.blockUI = false;
                    showStickyToast(true, "Successfully updated ");
                    return false;
                }
              
                else {
                    showStickyToast(true, "Successfully Created ");
                    window.location.href = '../mMaterialManagement/JobOrder.aspx?JOBID=' + response.d;
                    return false;
                }

            }
           else if (response.d < 0)
            {
                showStickyToast(false, "Please Enter Qty. as Multiples of  " + -(response.d) + ", because LCM of BOM '" + $("#txtbomrefno").val()+"' is "+ -(response.d));
                $scope.blockUI = false;
                return false;
            }
            else {
                showStickyToast(false, "Error While Creating ");
                $scope.blockUI = false;
                return false;
            }


        });
    }




    $scope.getJOBHeaderData = function () {
        if (location.href.indexOf('?JOBID=') > 0) {
            $scope.hidedata = true;
            var accounts = {
                method: 'POST',
                url: 'JobOrder.aspx/GetJOBHeaderData',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'jobid': location.href.split('?JOBID=')[1] }
            }
            $http(accounts).success(function (response) {
                debugger;
                console.log(response.d);
                var data = response.d;
                $scope.JOBHeader = new JOBOrderHeader(data.JOBOrderId, data.JOBRefNo, data.TenantId, data.Tenant, parseInt(data.AccountId), data.Account, data.BOMId, data.BOMRefNo, '', data.Quantity, parseInt(data.JobOrderTypeId), data.Status, data.StatusId, data.WareHouseId);
            });


            var accounts1 = {
                method: 'POST',
                url: 'JobOrder.aspx/GetReleaseData',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'jobid': location.href.split('?JOBID=')[1] }
            }
            $http(accounts1).success(function (response) {
                debugger;
                if (response.d != '' || response.d!=null)
                    $scope.releasedata = JSON.parse(response.d)[0];
                console.log( $scope.releasedata[0]);
            });
        }
    }
    $scope.getJOBHeaderData();

    $scope.InitiateOutward = function () {

        debugger;
       $scope.PendingQuantities=null;
       $scope.resultantassigndata = null;
       $scope.blockUI = true;
        var httpreq = {
            method: 'POST',
            url: 'JobOrder.aspx/InitiateOutbound',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            //data: { 'OutboundID': ID }
            data: { 'jobid': location.href.split('?JOBID=')[1] },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            var resultdata = response.d;
            $scope.blockUI = false;
            if (resultdata == null) {
                showStickyToast(false, 'Error while Initiate Outward', false);
                return false;
            }
             else if (resultdata.Status == -4) {
                showStickyToast(false, 'Job Order already Released', false);
                //$scope.resultantassigndata = JSON.parse(response.d.ResultData);
                $scope.getJOBHeaderData();
               

            }
            else if (resultdata.Status == -1) {
                showStickyToast(false, 'Problem with Initiating Outward', false);
                $scope.resultantassigndata = JSON.parse(response.d.ResultData);
                $scope.getJOBHeaderData();
               

            }
             else if (resultdata.Status == -2) {
                showStickyToast(false, 'Problem with Initiating Outward', false);
                $scope.PendingQuantities = JSON.parse(response.d.ResultData);
                $scope.getJOBHeaderData();
               

            }
            else if (response.d.Status == 1) {
                showStickyToast(true, 'Successfully Initiated', false);
                $scope.getJOBHeaderData();
                
            }


        })


    }


    $scope.InitiateInward = function () {

        debugger;

        $scope.blockUI = true;
        var httpreq = {
            method: 'POST',
            url: 'JobOrder.aspx/InitiateInward',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            //data: { 'OutboundID': ID }
            data: { 'jobid': location.href.split('?JOBID=')[1] },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            var resultdata = response.d;
            $scope.blockUI = false;
            if (resultdata == null) {
                showStickyToast(false, 'Error while Initiate Inward', false);
                return false;
            }
            else if (resultdata.Status == -4) {
                showStickyToast(false, 'Inward already Initiated', false);



            }
            else if (resultdata.Status == -1) {
                showStickyToast(false, 'Putaway suggestions failed', false);
               


            }
            else if(resultdata.Status == -2) {
                showStickyToast(false, 'Error while Initiate Inward', false);
                return false;
            }
            else if (response.d.Status == 1) {
                showStickyToast(true, 'Successfully Initiated', false);
                $scope.getJOBHeaderData();

            }


        })


    }
});
function JOBOrderHeader(joborderid, jobrefno, tenantid, tenant, accountid, account, bomid, bomrefno, remarks,quantity,jobordertype,status,statusid,warehouseid) {
    this.JOBOrderId = joborderid;
    this.JOBRefNo = jobrefno;
    this.TenantId = tenantid;
    this.Tenant = tenant;
    this.AccountId = accountid;
    this.Account = account;
    this.BOMId = bomid;
    this.BOMRefNo = bomrefno;
    this.Remarks = remarks;
    this.Quantity = quantity;
    this.JobOrderTypeId = jobordertype;
    this.Status = status;
    this.StatusId = statusid;
    this.WareHouseId = warehouseid;

}
function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;
    return true;
}

var app = angular.module('JOBListMyApp', ['angularUtils.directives.dirPagination']);


app.controller('JOBOrderList', function ($scope, $http, $timeout) {
    $scope.changemenulink = function () {
        window.location.href = '../mMaterialManagement/JobOrder.aspx';
    }
     $scope.search=new JOBListSearch(0,'','',0,0,'');


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

    var textfieldname = $("#txtBOMRefNo");
    DropdownFunction(textfieldname);
    $("#txtBOMRefNo").autocomplete({
        source: function (request, response) {
            if ($scope.search.TenantId == '' || $scope.search.TenantId == undefined) {
                showStickyToast(false, "Please select Tenant ");
                return false;
            }
            if ($scope.search.BomRefNo == '' || $scope.search.BomRefNo == undefined) {
                $scope.search.BOMID = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/getBOMRefNo',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + $scope.search.TenantId + "'}",
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
            $scope.search.BOMID = 0;
            $scope.search.BOMID = i.item.val;
            
        },
        minLength: 0
    });



var textfieldname = $("#txtJobRefNo");
    DropdownFunction(textfieldname);
    $("#txtJobRefNo").autocomplete({
        source: function (request, response) {
            if ($scope.search.TenantId == '' || $scope.search.TenantId == undefined) {
                showStickyToast(false, "Please select Tenant ");
                return false;
            }
            if ($scope.search.JOBRefNo == '' || $scope.search.JOBRefNo == undefined) {
                $scope.search.JOBID = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/getJOBRefNo',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + $scope.search.TenantId + "'}",
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
            $scope.search.JOBID = 0;
            $scope.search.JOBID = i.item.val;
            
        },
        minLength: 0
    });







    $scope.getJOBHeaderData = function () {
        debugger;
        $scope.blockUI = true;
        var data = $scope.search;
        var accounts = {
            method: 'POST',
            url: 'JobOrderList.aspx/GetJOBHeaderData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': data }
        }
        $http(accounts).success(function (response) {
            debugger;
            $scope.blockUI = false;
            $scope.JOBList = response.d;
            console.log($scope.JOBList);

        });

    }
    $scope.getJOBHeaderData();
});

function JOBListSearch(tenantid,tenant,bomrefno,bomid,jobid,jobrefno)
{
    this.TenantId=tenantid;
    this.Tenant=tenant;
    this.BomRefNo=bomrefno;
    this.BOMID=bomid;
    this.JOBID=jobid;
    this.JOBRefNo=jobrefno;
}