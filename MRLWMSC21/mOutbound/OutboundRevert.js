var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('RevertOBD', function ($scope, $http) {
    $scope.RevertSearchData = new RevertSearch(0, '', '', '', '', '', '', '');
    //$scope.RevertTypes = [{ 'Id': 1, 'Name': 'Line Item' }, { 'Id': 2, 'Name': 'OBD' }, { 'Id': 3, 'Name': 'PGI Revert' }];
    $scope.RevertTypes = [{ 'Id': 2, 'Name': 'OBD' }, { 'Id': 3, 'Name': 'PGI Revert' }];
    $scope.ishide = false;
    $scope.Tenant = 0;

    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadTenantDataByUserWH',
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
            $scope.Tenant = i.item.val;
        },
        minLength: 0
    });

    var textfieldname = $("#txtNo");
    DropdownFunction(textfieldname);
    $("#txtNo").autocomplete({
        source: function (request, response) {
            if ($scope.RevertSearchData.RevertType == '' || $scope.RevertSearchData.RevertType == undefined || $scope.RevertSearchData.RevertType==0) {
                return
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetOBD_ItemsForRevert',
                data: "{ 'prefix': '" + request.term + "','type': '" + $scope.RevertSearchData.RevertType + "','TenantID':'" + $scope.Tenant+"'}",
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
           
            $scope.RevertSearchData.OBDNO = i.item.label;
            // alert(Refnumber);
            //$scope.ngtenant = i.item.val;
        },
        minLength: 0
    });

    























    $scope.getOBDItems = function () {
        debugger;


        $("#txtNo").val("");
        $scope.RevertSearchData.OBDNO = "";
        $scope.RevertSearchData.BatchNo = "";
        $scope.RevertSearchData.SNO = "";
        $scope.RevertSearchData.ProjectRefNo = "";
        $scope.RevertSearchData.MFGDate = "";
        $scope.RevertSearchData.EXPDate = "";

        $scope.Item_OBD_Data = null;
        $scope.OBDData = null;
        // $scope.RevertSearchData.OBDNO = '';
        if ($scope.RevertSearchData.RevertType == undefined || $scope.RevertSearchData.RevertType == null || $scope.RevertSearchData.RevertType == 0) {
            //showStickyToast(false, "Please Select Revert Type");
            return false;
        }
        var URL = '';
        if ($scope.RevertSearchData.OBDNO == undefined) {
            $scope.RevertSearchData.OBDNO = '';
        }
        if ($scope.RevertSearchData.RevertType == 2) {
            URL = '../mOutbound/OutboundRevert.aspx/getOBDNumbers'
        }
        else if ($scope.RevertSearchData.RevertType == 3) {
            URL = '../mOutbound/OutboundRevert.aspx/getOBDNumbersForPGIrevert'
        }
        else {
            URL = '../mOutbound/OutboundRevert.aspx/getItems'
        }
        var tablenames = {
            method: 'POST',
            url: URL,
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'prefix': $scope.RevertSearchData.OBDNO }
        }
        $http(tablenames).success(function (response) {
            $scope.Item_OBD_Data = response.d;

        });
    }

    $scope.advancesearch = function () {
        if ($scope.ishide) {
            $scope.ishide = false;


        }
        else {
            $scope.ishide = true;
            $scope.issearch = false;
        }


    }
    $scope.GetdetailsAfterSearch = function () {
        debugger;
        $scope.OBDData = null;
        if ($scope.RevertSearchData.RevertType == undefined || $scope.RevertSearchData.RevertType == null || $scope.RevertSearchData.RevertType == 0) {
            showStickyToast(false, "Please select Revert Type");
            return false;
        }
        if ($scope.RevertSearchData.RevertType == 2) {
            if ($scope.RevertSearchData.OBDNO == undefined || $scope.RevertSearchData.OBDNO == null || $scope.RevertSearchData.OBDNO == 0) {
                showStickyToast(false, "Please enter OBD No.");
                return false;
            }
        }
        if ($scope.RevertSearchData.OBDNO == undefined || $scope.RevertSearchData.OBDNO == null || $scope.RevertSearchData.OBDNO == 0) {
            $scope.RevertSearchData.OBDNO = '';

        }
        if ($scope.RevertSearchData.MFGDate == undefined) {
            $scope.RevertSearchData.MFGDate = '';
        }
        if ($scope.RevertSearchData.EXPDate == undefined) {
            $scope.RevertSearchData.EXPDate = '';
        }
        if ($scope.RevertSearchData.BatchNo == undefined) {
            $scope.RevertSearchData.BatchNo = '';
        }
        if ($scope.RevertSearchData.SNO == undefined) {
            $scope.RevertSearchData.SNO = '';
        }
        if ($scope.RevertSearchData.ProjectRefNo == undefined) {
            $scope.RevertSearchData.ProjectRefNo = '';
        }

        var tablenames = {
            method: 'POST',
            url: '../mOutbound/OutboundRevert.aspx/GetOutBoundDetailsDataForRevert',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.RevertSearchData }
        }
        $http(tablenames).success(function (response) {
            debugger;
            if (response.d == null) {
               // showStickyToast(false, "Error while getting data");
                return false;
            }
            else if (response.d.length == 0) {
                //showStickyToast(false, "No Data for given Search Crieteria");
                return false;
            }
            else {
                $scope.OBDData = response.d;
                console.log($scope.OBDData);
            }

        });


    }
    $scope.Getdetails = function () {
        debugger;

        if ($("#txtTenant").val() == "") {
            $scope.Tenant = 0;
            $scope.RevertSearchData.TenantID = 0;
            showStickyToast(false, "Please select Tenant ", false);
            return false;
        }
        $scope.RevertSearchData.TenantID = $scope.Tenant;
        $scope.OBDData = null;
        if ($scope.RevertSearchData.RevertType == undefined || $scope.RevertSearchData.RevertType == null || $scope.RevertSearchData.RevertType == 0) {
            showStickyToast(false, "Please select Revert Type", false);
            return false;
        }
        if ($scope.RevertSearchData.RevertType == 2) {
            if ($scope.RevertSearchData.OBDNO == undefined || $scope.RevertSearchData.OBDNO == null || $scope.RevertSearchData.OBDNO == 0) {
                showStickyToast(false, "Please enter OBD No.", false);
                return false;
            }
        }
        if ($scope.RevertSearchData.OBDNO == undefined || $scope.RevertSearchData.OBDNO == null || $scope.RevertSearchData.OBDNO == 0) {
            $scope.RevertSearchData.OBDNO = '';

        }
        if ($scope.RevertSearchData.MFGDate == undefined) {
            $scope.RevertSearchData.MFGDate = '';
        }
        if ($scope.RevertSearchData.EXPDate == undefined) {
            $scope.RevertSearchData.EXPDate = '';
        }
        if ($scope.RevertSearchData.BatchNo == undefined) {
            $scope.RevertSearchData.BatchNo = '';
        }
        if ($scope.RevertSearchData.SNO == undefined) {
            $scope.RevertSearchData.SNO = '';
        }
        if ($scope.RevertSearchData.ProjectRefNo == undefined) {
            $scope.RevertSearchData.ProjectRefNo = '';
        }

        if ($scope.RevertSearchData.TenantID == undefined || $scope.RevertSearchData.TenantID == '') {
            $scope.RevertSearchData.TenantID = 0;
        }

        var tablenames = {
            method: 'POST',
            url: '../mOutbound/OutboundRevert.aspx/GetOutBoundDetailsDataForRevert',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.RevertSearchData }
        }
        $http(tablenames).success(function (response) {
            debugger;
            if (response.d == null) {
                showStickyToast(false, "Error while getting data");
                return false;
            }
            else if (response.d.length == 0) {
                showStickyToast(false, "No data for given Search criteria");
                return false;
            }
            else {
                $scope.OBDData = response.d;
                console.log($scope.OBDData);
            }

        });


    }

    $scope.RevertOBDData = function (objdata) {
        var URL = '';
        var obj = null;
        debugger;

        if ($scope.RevertSearchData.RevertType == 1) {
            if (objdata.RevertQty > objdata.PickedQuantity) {
                showStickyToast(false, 'Revert Qty. Exceeded');
                return false;
            }
            var tablenames = {
                method: 'POST',
                url: '../mOutbound/OutboundRevert.aspx/RevertOutbounDetails',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: {
                    'outboundid': objdata.OutboundID, 'SodetailsId': objdata.SODetailsID, 'Qty': objdata.RevertQty, 'MMID': objdata.MaterialMasterID
                }
            }
            $http(tablenames).success(function (response) {
                debugger;
                if (response.d == "Successfully Reverted") {
                    showStickyToast(true, 'Successfully Reverted');
                    $scope.GetdetailsAfterSearch();
                }
                else  {
                    showStickyToast(false, 'Error While Revert');
                    return false;
                }
                
            });
        }
        else if ($scope.RevertSearchData.RevertType == 3) {

            var tablenames = {
                method: 'POST',
                url: '../mOutbound/OutboundRevert.aspx/RevertPGI',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'outboundid': objdata.OutboundID }
            }
            $http(tablenames).success(function (response) {
                debugger;
                if (response.d.indexOf("Error") > 0) {
                    showStickyToast(false, response.d);
                    return false;

                }
                else {
                    showStickyToast(true, response.d);
                    $scope.GetdetailsAfterSearch();
                    return false;
                }
            });

        }

    }
    $scope.RevertOutBound = function () {
        debugger;
        var tablenames = {
            method: 'POST',
            url: '../mOutbound/OutboundRevert.aspx/RevertOutbound',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'OBDnumber': $scope.RevertSearchData.OBDNO
            }
        }
        $http(tablenames).success(function (response) {
            //if (response.d == -1) {
            //    showStickyToast(false, 'No Line Items in given outbound to revert');
            //    return false;
            //}
            //else if (response.d == -2) {
            //    showStickyToast(false, 'Error While Revert');
            //    return false;
            //}
            //else if (response.d == -2) {
            //    showStickyToast(false, 'Invalid OBD No.');
            //    return false;
            //}
            //else {
            //    showStickyToast(true, 'Successfully Reverted');
            //    $scope.GetdetailsAfterSearch();
            //    return false;
            //}

            if (response.d == "Successfully Reverted") {
                showStickyToast(true, 'Successfully Reverted');
                $scope.GetdetailsAfterSearch();
            }
            else if (response.d == '-7')
            {
                showStickyToast(false, 'Invalid OBD No.');
                return false;
            }
            else {
                showStickyToast(false, response.d);
                return false;
            }
        });
    }

    $scope.clearDetails = function ()
    {
        $("#txtTenant").val("");
        $("#txtNo").val("");
        $("#txtMFGdate").val("");
        $("#txtEXPdate").val("");
        $scope.RevertSearchData.RevertType = 0;
        $scope.RevertSearchData.OBDNO = "";
        $scope.Tenant = 0;
        $scope.RevertSearchData.TenantID = 0;

        $scope.RevertSearchData.BatchNo = "";
        $scope.RevertSearchData.SNO = "";
        $scope.RevertSearchData.ProjectRefNo = "";
        $scope.RevertSearchData.MFGDate = "";
        $scope.RevertSearchData.EXPDate = "";
    }
});
function RevertSearch(revertType, OBDnumber, mcode, mfgDate, ExpDate, batchno, sno, projectRefno) {
    this.RevertType = revertType
    this.OBDNO = OBDnumber;
    this.Mcode = mcode;
    this.MFGDate = mfgDate;
    this.EXPDate = ExpDate;
    this.BatchNo = batchno;
    this.SNO = sno;
    this.ProjectRefNo = projectRefno;
}
function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;
    return true;
}