
var myApp = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
myApp.controller('PackSlip', function ($scope, $http) {
   
    
    if (location.href.indexOf('?obdid=') > 0) {
        var obdid = location.href.split('?obdid=')[1];
        var httpreq = $.ajax({
            type: 'POST',
            url: 'DeliveryPackSlip.aspx/GetPackList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: JSON.stringify({ 'obdid': obdid }),
            // data: JSON.stringify({ Tenantid: Tenant, Mcode: PartNo, Mtypeid: Mtype, BatchNo: Batchno, Location: Loc, kitId: kit }),
            async: false,
            success: function (data) {
               
               
                var hh = data.d;
                //console.log(hh);
                $scope.PackList = hh;
                if ($scope.PackList == null || $scope.PackList == undefined || $scope.PackList.length==0)
                {
                    showStickyToast(false, "No Data Found");
                }
            }
        });
    }

    $scope.PrintBoxLabels = function (OBDId) {
        
        
       
        if (OBDId == undefined || OBDId==""){
         (location.href.indexOf('?obdid=') > 0) 
            var OBDId = location.href.split('?obdid=')[1];
        }
        $.ajax({
            url: 'DeliveryPackSlip.aspx/btnPrintPackingData',
            data: "{ 'OBDId': '" + OBDId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                
                var res = data.d;
                if (res.length == 1) {
                    if (res == "1") {
                        window.open("../mOutbound/PackingSlip.pdf");
                    }
                    else {
                        showStickyToast(false, "No box found", false);
                    }
                }
                else {
                    showStickyToast(false, "Box filling is not completed yet", false);
                    $("#divBoxPendingDetails").html(res);
                    $('#divBlocker').show();
                }
            },
            error: function (response) {
                
                showStickyToast(false, "Please check your network connection", false);
            },
            failure: function (response) {
            }
        });

        return false;

    }

    $scope.exportPdf = function () {
        
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldata").css('display', 'none');
        $scope.export();
    }
    $scope.export = function () {
        
        var table = $scope.PackList;
        $("#tbldata").empty();
        $("#tbldata").append("<table><tr><th> MCode</th><th>OEMPartNo </th><th>MDescription</th></tr><tr><th>DockName</th><th>PickedQty</th></tr><tr><th>VehicleNo</th><th>VehicleType</th></tr><tr><th>DriverName</th><th>DriverMobileNo</th></tr><tbody>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<td class='aligndate'>" + table[i].MCode + "</td><td class='aligndate'>" + table[i].OEMPartNo + "</td><td class='aligndate'>" + table[i].MDescription + "</td></tr><tr><td class='aligndate'>" + table[i].DockName + "</td><td class='aligndate'>" + table[i].PickedQty + "</td></tr><tr><td class='aligndate'>" + table[i].VehicleNo + "</td><td class='aligndate'>" + table[i].VehicleType + "</td></tr><tr><td class='aligndate'>" + table[i].DriverName + "</td><td class='aligndate'>" + table[i].DriverMobileNo + "</td>");
        }
        $("#tbldata").append("</tbody></table>");
    }
});