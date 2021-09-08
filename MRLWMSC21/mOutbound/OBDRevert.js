var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('OBDRevert', function ($scope, $http) {

    var RefWH = '';
    var Warehouseid = 0;
    $scope.StatusID = 0
    $scope.PageNumber = 1
    $("#txtFromDate").datepicker({
        todayBtn: 1,
        singleDatePicker: true,
        showDropdowns: true,
        dateFormat: "dd-M-yy",
        changeMonth: true,
        changeYear: true,
        autoclose: true,
        forceParse: false,
        startDate: "today",

    });
    $("#txtToDate").datepicker({
        todayBtn: 1,
        singleDatePicker: true,
        showDropdowns: true,
        changeMonth: true,
        changeYear: true,
        autoclose: true,
        forceParse: false,
        dateFormat: "dd-M-yy",
        startDate: "today",

    });
    
        
    

        var textfieldname = $("#txtWarehouse");
        debugger;
        DropdownFunction(textfieldname);
        $("#txtWarehouse").autocomplete({
            source: function (request, response) {
                $.ajax({
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

            },
            minLength: 0
        });
        //ending of warehouse dropdown
    

    $scope.TenantId = "0";
    var TenantId = 0;
    $scope.MMID = "0";
    
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            if ($('#txtTenant').val() == '' || $('#txtTenant').val() == undefined) {
                $scope.TenantId = "0";
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                data: "{ 'prefix': '" + request.term + "','whid':'" + Warehouseid + "' }",
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
            $scope.TenantId = "0";
            $scope.TenantId = i.item.val;
            TenantId = i.item.val;
            //$scope.LoadWareHouse();
        },
        minLength: 0
    });
    


    $scope.getOBDlist = function (pageid) {
        debugger;
        $scope.PageNumber = pageid
        if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined || $("#txtTenant").val() == "") {

            showStickyToast(false, " Please select Tenant ");
            return false;
        }
        if ($("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined || $("#txtWarehouse").val() == "") {

            showStickyToast(false, " Please select Warehouse ");
            return false;
        }
        $("#tbldatas").addClass("tableLoader");
        $scope.StatusID = $('#txtprpo option:selected').toArray().map(item => item.value).join();
        let CategoryID = $('#txtCategory option:selected').toArray().map(item => item.value).join();
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'OBDRevert.aspx/GetOBDList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'TenantId': $scope.TenantId, 'WareHouseId': Warehouseid,
                'NoofRecords': 25, 'PageNo': pageid, 'StatusID': $scope.StatusID,
                'FromDate': $("#txtFromDate").val(), 'ToDate': $("#txtToDate").val(),
                'CategoryID': CategoryID,
                'SearchText': $("#txtSearch").val()
            },
            async: false
        }
        $scope.OBDList = null;
        $http(httpreq).success(function (response) {
            debugger;
            var dt = JSON.parse(response.d).Table;
            if (dt == undefined || dt == null || dt.length == 0) {
                showStickyToast(false, "No Data Found", false);
                $scope.OBDList = null;
                $scope.Totalrecords = 0;
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
            $scope.OBDList = dt;
            $scope.Totalrecords = $scope.OBDList[0].TotalRecords;
            document.querySelector('#tbldatas').classList.remove("tableLoader");
        })
    };




    $scope.GetOBDDetails = (OutboundID) => {
        debugger;
        $scope.OBDDetails = null;
        $("#tblModel").addClass("tableLoader");
        document.querySelector('#tblModel').classList.add("tableLoader");
        $.ajax({
            url: 'OBDRevert.aspx/GetOBDDetails',
            data: "{'OutboundID' : '" + OutboundID + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                debugger;
                const dt = JSON.parse(response.d).Table;
                if (dt == undefined || dt == null || dt.length == 0) {
                    showStickyToast(false, "No Data Found", false);
                    document.querySelector('#tblModel').classList.remove("tableLoader");
                    return false;
                }
                else {
                    document.querySelector('#tblModel').classList.remove("tableLoader");
                    $scope.OBDDetails = dt
                }

            },
            error: function (response) {

            },
            failure: function (response) {

            }
        });
    }


    $scope.RevertPickItem = function (obj) {

        //OBDLinePickRevert
        debugger

        if (obj.ReasonID <= 0) {
            showStickyToast(false, "Please Select Reason for Revert", false);
            //document.querySelector('#tbldatas').classList.remove("tableLoader");
            return false;
        }
        //if (obj.RevertQty > obj.PickedQty) {
        //    showStickyToast(false, "Revert Qty more than the Picked Qty.", false);
        //    //document.querySelector('#tbldatas').classList.remove("tableLoader");
        //    return false;
        //}
        if (obj.RevertQty > 0) {
            if (obj.RevertQty > obj.AssignedQuantity && obj.ReasonID != 1) {
                showStickyToast(false, "Revert Qty more than the Assigned Qty.", false);
                //document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
            if (obj.RevertQty > obj.PickedQty && obj.ReasonID == 1) {
                showStickyToast(false, "Revert Qty more than the Picked Qty.", false);
                //document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
        }
        else {
            showStickyToast(false, "Revert Qty Should be Grater than 0 ", false);
            return false;
        }


        $("#tblModel").addClass("tableLoader");
        document.querySelector('#tblModel').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'OBDRevert.aspx/OBDLinePickRevert',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'AssignedID': obj.AssignedID, 'RevertQty': obj.RevertQty, 'RevertTypeID': obj.ReasonID
            },
            async: false
        }

        $http(httpreq).success(function (response) {
            debugger;
            var dt = response.d
            if (dt == undefined || dt == null) {
                document.querySelector('#tblModel').classList.remove("tableLoader");
                $('#AddEntityToCreate').modal('hide');
                showStickyToast(false, "No Data Found", false);
                //document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
            else if (dt.includes('Success')) {
                document.querySelector('#tblModel').classList.remove("tableLoader");
                $('#AddEntityToCreate').modal('hide');
                $scope.OBDDetails = [];
                showStickyToast(true, "Success: Revert Request Completed", true);
                $scope.getOBDlist(1);



                //document.querySelector('#tbldatas').classList.remove("tableLoader");
                return;
            }
            //else if (dt == -5) {
            //    showStickyToast(true, "Please Contact Inventrax Support Team..!", true);
            //    document.querySelector('#tbldatas').classList.remove("tableLoader");
            //    return;
            //}

            else {
                document.querySelector('#tblModel').classList.remove("tableLoader");
                $('#AddEntityToCreate').modal('hide');
                showStickyToast(false, dt, false);
                //document.querySelector('#tbldatas').classList.remove("tableLoader");
                return;
            }

        })


    }

    $scope.RevertOBD = function (OBDNumber, DeliveryTypeID) {
        debugger;

        $("#tbldatas").addClass("tableLoader");

        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'OBDRevert.aspx/setOBDRevert',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'OBDNumber': OBDNumber, 'DeliveryTypeID': DeliveryTypeID
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            var dt = JSON.parse(response.d)
            if (dt == undefined || dt == null) {
                showStickyToast(false, "No Data Found", false);
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
            else if (dt == 1) {
                showStickyToast(true, "Success: Revert Request Completed", true);
                $scope.getOBDlist($scope.PageNumber)
                document.querySelector('#tbldatas').classList.remove("tableLoader");

                return;
            }
            //else if (dt == -5) {
            //    showStickyToast(true, "Please Contact Inventrax Support Team..!", true);
            //    document.querySelector('#tbldatas').classList.remove("tableLoader");
            //    return;
            //}
            else if (dt == -3) {
                showStickyToast(false, "Stock is not avaiable in Bin Location..!", false);
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return;
            }
            else {
                showStickyToast(false, "Please Contact Inventrax Support Team..!", false);
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return;
            }
            $scope.OBDList = dt;
            $scope.Totalrecords = $scope.OBDList[0].TotalRecords;
            document.querySelector('#tbldatas').classList.remove("tableLoader");
        })
    };

});
