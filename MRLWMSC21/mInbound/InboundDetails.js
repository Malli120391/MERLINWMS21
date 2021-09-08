var MyApp = angular.module('MyApp', ['xlsx-model']);
MyApp.controller('createinbound', function ($scope, $http) {

    $scope.GRNHeader = new GRN('', 0, '', 0, '', 0, '', 0);
    var inboundId = 0;
    if (location.href.indexOf('?ibdid=') > 0) {
        inboundId = location.href.split('?ibdid=')[1];
    }

    //alert(inboundId);
    if (inboundId != 0 && inboundId != undefined && inboundId != null && inboundId != "")
    {
        var http = {
            method: 'POST',
            url: '../mInbound/InboundDetails.aspx/getdiscrepancylist',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'InboundID': inboundId }
        }
        $http(http).success(function (response) {
            $scope.descrepancy = response.d;

            if ($scope.descrepancy.length != 0 && $scope.descrepancy != undefined) {
                for (var i = 0; $scope.descrepancy.length > i; i++) {
                    if ($scope.descrepancy[i].TotalPendingQty != 0) {
                        $('#MainContent_IBContent_cbisdesc').attr('checked', 'checked');

                    }
                }
            }


        });
    }


    $scope.OpenGRNPopUP = function ()
    {
        $("#AddGRN").modal({
            show: 'true'
        });

        $scope.GRNHeader = new GRN('', 0, '', 0, '', 0, '', 0);
        $('#txtPONO').val('');
        $('#txtInvoiceNo').val('');
        $('#txtGRNvehicle').val('');
        $scope.GRNProcessingData = [];

    }


    //==================== getting GRN PO Number ========================//
    var textfieldname = $("#txtPONO");
    DropdownFunction(textfieldname);
    $("#txtPONO").autocomplete({
        source: function (request, response) {
            debugger;

            var inboundId = 0;
            if (location.href.indexOf('?ibdid=') > 0) {
                inboundId = location.href.split('?ibdid=')[1].split('&')[0];
            }

            if ($("#txtPONO").val() == "") {
                $scope.GRNHeader.POHeaderID = 0;
                $('#txtInvoiceNo').val('');
                $('#txtGRNvehicle').val('');
                $scope.GRNHeader.SupplierInvoiceID = 0;
                $scope.GRNHeader.GateEntryID = 0;
                $scope.GRNHeader.GateEntryID = 0;
            }
    
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadPoNumberForGRN',
                data: "{'InboundID': '" + inboundId+"' }",
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
            $('#txtInvoiceNo').val('');
            $('#txtGRNvehicle').val('');
            $scope.GRNHeader.SupplierInvoiceID = 0;
            $scope.GRNHeader.GateEntryID = 0;
            $scope.GRNHeader.POHeaderID = i.item.val;


        },
        minLength: 0
    });
    //==================== getting GRN Invoicde  Number ========================//
    var textfieldname = $("#txtInvoiceNo");
    DropdownFunction(textfieldname);
    $("#txtInvoiceNo").autocomplete({
        source: function (request, response) {


            if ($("#txtInvoiceNo").val() == "") {
                $scope.GRNHeader.SupplierInvoiceID = 0;
                $('#txtGRNvehicle').val('');
                $scope.GRNHeader.GateEntryID = 0;
            }
            if ($scope.GRNHeader.POHeaderID == 0 || $scope.GRNHeader.POHeaderID == undefined || $("#txtPONO").val() == '') {
                showStickyToast(false, "Please select PO No. ", false);
                return false;
            }
            var inboundid = location.href.split('?ibdid=')[1].split('&')[0];
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadGRNPOInvoiceNumbers',
                data: "{'prefix': '" + request.term + "','POHeaderID': '" + $scope.GRNHeader.POHeaderID + "','SupplierInvoiceID':'" + 1 + "','InboundID': '" + inboundid + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split('~')[0],
                            val: item.split('~')[1],

                        }
                    }))
                }
            });
        },
        select: function (e, i) {
            $('#txtGRNvehicle').val('');
            $scope.GRNHeader.GateEntryID = 0;
            $scope.GRNHeader.SupplierInvoiceID = i.item.val;


        },
        minLength: 0
    });




    $scope.FetchGRNDataForInbound = function ()
    {

        debugger;
        if ($scope.GRNHeader.POHeaderID == 0 || $scope.GRNHeader.POHeaderID == "0" || $scope.GRNHeader.POHeaderID == null || $('#txtPONO').val() == '')
        {
            showStickyToast(false, 'Please select PO No.');
            return false;
        }
        if ($scope.GRNHeader.SupplierInvoiceID == 0 || $scope.GRNHeader.SupplierInvoiceID == "0" || $scope.GRNHeader.SupplierInvoiceID == null || $('#txtInvoiceNo').val() == '')
        {
            showStickyToast(false, 'Please select Invoice No.');
            return false;
        }
        debugger;
        if (location.href.indexOf('?ibdid=') > 0)
        {
            $scope.GRNProcessingData = null;
            $scope.IsheaderCreated = true;
            var accounts =
            {
                method: 'POST',
                url: 'InboundDetails.aspx/FetchGRNDataForInbound',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: {
                    'InboundId': location.href.split('?ibdid=')[1].split('&')[0], 'PoheaderID': $scope.GRNHeader.POHeaderID,
                    'supplierinvoiceid': $scope.GRNHeader.SupplierInvoiceID, 
                },
                
            }
            $http(accounts).success(function (response)
            {

                debugger;
                console.log(response);
                if (response.d != null && response.d != undefined)
                {
                    $scope.GRNProcessingData = response.d;
                }
                else
                {
                    showStickyToast(false, 'No Pending Data to Post GRN');
                    return false;
                }
                if (response.d.length == 0)
                {
                    showStickyToast(false, 'No Pending Data to Post GRN');
                }


            });
        }
    }


    var isLoad = 0;
    $scope.AddGRNDetails = function ()
    {

        debugger;

        // Below code Added By lalitha on 23-12-2020


        if ($scope.GRNHeader.Remarks == "" || $scope.GRNHeader.Remarks == null || $scope.GRNHeader.Remarks == undefined) {
            showStickyToast(false, 'Please enter Remarks');
            return false;
        } 
   

        $('#imgGRNLLoadingSAP').show();
        $('#btnaddGRNDetails').css('display', 'none');
        var accounts = {
            method: 'POST',
            url: 'InboundDetails.aspx/CreateGRN',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data:
            {
                'ReceivingInfo': $scope.GRNProcessingData,
                'InboundId': location.href.split('?ibdid=')[1].split('&')[0], 'PoheaderID': $scope.GRNHeader.POHeaderID,
                'supplierinvoiceid': $scope.GRNHeader.SupplierInvoiceID,'remarks': $scope.GRNHeader.Remarks, 
            }
        }
        $http(accounts).success(function (response)
        {

            debugger;
            $scope.GetGRNData();
            $('#btnaddGRNDetails').show();
            $('#imgGRNLLoadingSAP').css('display', 'none');


           
            if (response.d.indexOf("Error") == -1)
            {
                showStickyToast(true, response.d);
                $("#AddGRN").modal('hide');
                $scope.GETInboundStatus();

                if ($scope.GRNData.length == 0) {
                    setTimeout(function () {
                        debugger
                        var IBDID = new URL(window.location.href).searchParams.get("ibdid");
                        window.location.href = "InboundDetails.aspx?ibdid=" + IBDID;
                    }, 1000);
                }
                return false;
            }

            else
            {
                showStickyToast(false, response.d);
                $("#AddGRN").modal('hide');
                return false;
            }

        });



    }

    //============= GET Status ==============//

    $scope.GETInboundStatus = function () {
        debugger;
        if (location.href.indexOf('?ibdid=') > 0) {
            $scope.IsheaderCreated = true;
            var status = {
                method: 'POST',
                url: 'InboundDetails.aspx/GET_InboundStatus',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'InboundID': location.href.split('?ibdid=')[1] },
                //async: false,
            }              
            $http(status).success(function (response) {
                debugger
                if (response.d != null && response.d != undefined) {
                    $("#MainContent_IBContent_lblInboundStatus").text(response.d);
                    //$(".Discrepency").show();
                    //$(".Verification").show();
                    //$("#pnlDiscInfo").show();
                    //$("#pnlshipmentVerification").show();                  
                }
            });
        }
    };
    //$scope.GETInboundStatus();

    //================== get GRN Data Details===================//
    $scope.GetGRNData = function ()
    {
        debugger;
        if (location.href.indexOf('?ibdid=') > 0) {
            $scope.IsheaderCreated = true;
            var accounts = {
                method: 'POST',
                url: 'InboundDetails.aspx/GetGRNData',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'inboundid': location.href.split('?ibdid=')[1].split('&')[0] },
                async: false,
            }
            $http(accounts).success(function (response)
            {
                console.log('SWAMY');
                console.log(response);
                debugger
                if (response.d != null && response.d != undefined)
                {
                    $scope.GRNData = JSON.parse(response.d).Table;
                }
                  
                console.log($scope.GRNData);
            });
        }
    }

    $scope.GetGRNData();



});



function GRN(VehicleNo, GateEntryID, PONo, POHeaderID, InvoiceNo, SupplierInvoiceID, Remarks, InboundID) {
    this.VehicleNo = VehicleNo;
    this.GateEntryID = GateEntryID;
    this.PONo = PONo;
    this.POHeaderID = POHeaderID;
    this.InvoiceNo = InvoiceNo;
    this.SupplierInvoiceID = SupplierInvoiceID;
    this.Remarks = Remarks;
    this.InboundID = InboundID;
}