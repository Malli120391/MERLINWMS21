var myApp = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
myApp.controller('pickitemslist', function ($scope, $http) {
    //----------For Popup Open-----------------//
    $scope.MaterialList1 = [];
    $scope.pickeddata = [];
    var containerautocomid = "";
    $('#spanClose').click(function (event) {
        $('#divContainer').hide();
    });

    $('#btnClose').click(function (event) {
        $('#divContainer').hide();
    });

    $scope.closeData = function () {
        $('#divContainer').hide();
    }
    $scope.openDialog = function (title, data, id) {
        debugger;
        $scope.containerautocomid = 0;
        $scope.pickeddata = $.grep(data, function (a) { return a.AssignID == id });
        $scope.GetPickedList($scope.pickeddata);
        $scope.PickRequestedQty = "";
        $('#divContainer').show();
    }

    $scope.Delete = function (Id) {
        debugger;
        var httpreq = {
            method: 'POST',
            url: '../mOutbound/GroupDeliveryPickNote.aspx/DeletePickedItems',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'PickedId': Id },
            async: false
        }
        $http(httpreq).success(function (response) {
            var result = response.d;
            if (result == "1") {
                showStickyToast(true, 'Deleted Successfully', false);
                $scope.GetDeliveryNoteInfo();
                $scope.GetPickedList($scope.pickeddata);
                $('#divContainer').hide();
            }
            else {
               // showStickyToast(false, 'Error while delete the line item', false);
                showStickyToast(false, 'Unable to delete the line item , VLPD verification already done', false);
                return;
            }
        });
    }


    $scope.GetPickedList = function (list) {
        debugger;
        var obj = list;

        var MMID, LoctionID, CartonID, Vlpdid1, BatchNo, serialNo, ProjectRefNo, ExpDate, MfgDate, VLPDAssingID,MRPv;

        if (location.href.indexOf('?VLPDID=') > 0) {
            Vlpdid1 = location.href.split('?VLPDID=')[1];
        }

        MMID = obj[0].MaterialMasterID;
        CartonID = obj[0].CartonID;
        BatchNo = obj[0].BatchNo;
        serialNo = obj[0].SerialNo;
        ProjectRefNo = obj[0].ProjectRefNo;
        MRPv = obj[0].MRP;
        ExpDate = obj[0].ExpDate;
        MfgDate = obj[0].MfgDate;
        LoctionID = obj[0].LocationID
        VLPDAssingID = obj[0].VLPDAssignId    // added by lalitha on 10/04/2019
        //var s = new URL(window.href).searchParams.get(VLPDID);
        var httpreq = {
            method: 'POST',
            url: '../mOutbound/GroupDeliveryPickNote.aspx/GetPickedItems',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'MMID': MMID, 'LoctionID': LoctionID, 'CartonID': CartonID, 'Vlpdid': Vlpdid1, 'BatchNo': BatchNo, 'serialNo': serialNo, 'ProjectRefNo': ProjectRefNo, 'ExpDate': ExpDate, 'MfgDate': MfgDate, 'VLPDAssingID': VLPDAssingID,'MRP':MRPv},
            async: false
        }
        $http(httpreq).success(function (response) {
            $scope.PickedList = response.d;

        });
    }

        //----------For Popup Close-----------------//
    $scope.GetDeliveryNoteInfo = function () {
        
        var vlpdid=0;
        var TransferRequestID=0;
        if (location.href.indexOf('?VLPDID=') > 0) {
            vlpdid = location.href.split('?VLPDID=')[1];
        }

        if (location.href.indexOf('?TRANSREQID=') > 0) {
            TransferRequestID = location.href.split('?TRANSREQID=')[1];
        }

            //var s = new URL(window.href).searchParams.get(VLPDID);
        var httpreq = {
            method: 'POST',
            url: '../mOutbound/GroupDeliveryPickNote.aspx/GET_DeliveryData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'VLPDID': vlpdid, 'TransferRequestID':TransferRequestID },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            $scope.DeliveryList = response.d;
            console.log($scope.DeliveryList);
            $scope.VLPDNumber = $scope.DeliveryList[0].VLPDNumber;
            $scope.WHCode = $scope.DeliveryList[0].WHCode;
            if ($scope.VLPDNumber.indexOf('TRNS') > -1)
                $scope.VLPDTitle = "Transfer Request Number";
            else
                $scope.VLPDTitle = "VLPD Number";
            
        });
        
    }
    


    $scope.containerList = function () {
       

        debugger;

        var Prefix = $scope.container;
        if (Prefix == undefined) {
            Prefix = "";
        }

       

        var vlpdids = location.href.split('?VLPDID=')[1];

        var textfieldname = $("#txtcontainer");
        DropdownFunction(textfieldname);
        $("#txtcontainer").autocomplete({
            source: function (request, response) {
                if ($("#txtcontainer").val() == '' || $("#txtcontainer").val() == undefined) {
                    containerautocomid = 0;
                }
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetContainersForGoodsin',
                    data: "{'Prefix': '" + request.term + "','VLPDID': '" + vlpdids + "' }",
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
                containerautocomid = i.item.val;

                debugger;
            },
            minLength: 0
        });


      


    }

    $scope.containerList();






    $scope.GetDeliveryNoteInfo();
  
    $scope.pickItem = function () {
        debugger;  
        var data = $scope.pickeddata[0];
        if ($('#txtqty').val() == "") {
            showStickyToast(false, 'Please enter Qty.', false);
            return;
        }
        if ($('#txtqty').val() == "0") {
            showStickyToast(false, 'Please enter valid Qty.', false);
            return;
        }
        //if ($scope.PickRequestedQty == "") {
        //    showStickyToast(false, 'Please enter Qty.', false);
        //    return;
        //}
        //if ($scope.PickRequestedQty == 0 || $scope.PickRequestedQty == undefined) {
        //    showStickyToast(false, 'Please enter valid Qty.', false);
        //    return;
        //}
        var validateqty = ($scope.PickRequestedQty + data.PickedQty)
        var s = data.AssiginQty;

        var vlpdid = 0;
        var TransferRequestID = 0;
        if (location.href.indexOf('?VLPDID=') > 0) {
            vlpdid = location.href.split('?VLPDID=')[1];
        }

        if (location.href.indexOf('?TRANSREQID=') > 0) {
            TransferRequestID = location.href.split('?TRANSREQID=')[1];        }


       
        if (validateqty > s)
        {
            showStickyToast(false, 'Picked Qty. Exceeds Assigned Qty.', false);
            return;
        }
        
        var httpreqsft = {
            method: 'POST',
            url: '../mOutbound/GroupDeliveryPickNote.aspx/SavePickQty',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'Pickdata': data, 'Qty': $scope.PickRequestedQty, 'VLPDID': vlpdid, 'TransferRequestID': TransferRequestID, 'containerid': containerautocomid},
           
        }
        $http(httpreqsft).success(function (response) {
$scope.containerautocomid = 0;
                $("#txtcontainer").val('');
            if (response.d == "-444") {
                showStickyToast(false, 'Please receive all Items to Carton, Earlier received to carton for this Group OBD');

                return false;
            }
            if (response.d == "-333") {
                showStickyToast(false, 'Please receive all Items without  Carton, Earlier received without carton for this Group OBD');

                return false;
            }
            if (response.d != undefined && response.d != '') {
                showStickyToast(true, 'Picked Successfully', false);
                $scope.PickRequestedQty = 0;
                $scope.containerautocomid = 0;
                $("#txtcontainer").val('');
                $scope.GetDeliveryNoteInfo();
                $('#divContainer').hide();
              
                $scope.GetPickedList(data);
               
               
            }
            else {
            }
        });
       

    };

    //function UpsertPickData(vlpdid, MaterialMasterID, VLPDAssignID, PickedQty, CartonID, LocationID, MfgDate, ExpDate, BatchNo, SerialNo, ProjectRefNo) {
    //    
    //    this.VLPDID = vlpdid;
    //    this.MaterialMasterID = MaterialMasterID;
    //    this.VLPDAssignID = VLPDAssignID;
    //    this.PickedQty = PickedQty;
    //    this.CartonID = CartonID;
    //    this.LocationID = LocationID;
    //    this.MfgDate = MfgDate;
    //    this.ExpDate = ExpDate;
    //    this.BatchNo = BatchNo;
    //    this.SerialNo = SerialNo;
    //    this.ProjectRefNo = ProjectRefNo;
    //       }
});
