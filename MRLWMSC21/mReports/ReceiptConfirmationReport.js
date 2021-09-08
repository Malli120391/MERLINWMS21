var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('ReceiptConfirmationReport', function ($scope, $http) {


    var ibdid = new URL(window.location.href).searchParams.get("ibdid");
    var lineitemcount = new URL(window.location.href).searchParams.get("lineitemcount");
    $scope.TenantID = new URL(window.location.href).searchParams.get("TN");


        debugger
        var inboundID = new URL(window.location.href).searchParams.get("ibdid");
        var tenantID = new URL(window.location.href).searchParams.get("TN");
        $("#hdnInboundID").val(inboundID);
        $("#hdnTenant").val(tenantID);

  
    $scope.getReceiptConfirm = function () {
       // TN = TN.replace(/+/g, " ");
        $("#tbldatas").addClass("tableLoader");
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'ReceiptConfirmationReport.aspx/getData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'INBID': ibdid },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            var dt = JSON.parse(response.d).Table;
            $scope.HeaderData = JSON.parse(response.d).Table1;
            $scope.HeaderData = $scope.HeaderData != null ? $scope.HeaderData[0] : null;
            if (dt != null && dt.length > 0) {
                $scope.ReceiptConfirm = dt;
                document.querySelector('#tbldatas').classList.remove("tableLoader");
            }
            else {
                $scope.ReceiptConfirm = null;
                showStickyToast(false, "No Data Found");
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }                
        })
    }

    $scope.getLogoName = function () {
        var logoName = "";
        var httpreq = {
            method: 'POST',
            url: 'ReceiptConfirmationReport.aspx/getLogoName',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {},
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            logoName = response.d;
        });
        return logoName;
    }

    $scope.exportRCRPDF = function () {

        $scope.blockUI = true;

        var httpreq = {
            method: 'POST',
            url: 'ReceiptConfirmationReport.aspx/generateRCReport',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'INBID': ibdid, 'TenantID': $scope.TenantID
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            if (response.d != null) {
                var obj = response.d;
                showStickyToast(true, "PDF Generated Successfully ", false);
                window.open('../mOutbound/PackingSlip/' + obj);
                $("#divLoading").hide();
                $scope.blockUI = false;
                return false;

            }
            else {
                showStickyToast(false, "No Data Found ", false);
                $("#divLoading").hide();
                $scope.blockUI = false;
                return false;
            }
        });
    }


    $scope.getReceiptConfirm();
    $scope.exportExcel = function () {
        if ($scope.ReceiptConfirm == null || $scope.ReceiptConfirm == undefined) {
            showStickyToast(false, "No Data Found", false);
            return false;
        }
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'excel' });
        $("#tbldata").css('display', 'none');
    }


    $scope.downloadPDF = function () {
        debugger;
        var table = $scope.ReceiptConfirm;
        var tableFormat = "";
        $("#divFormat").empty();     

        var logo = $scope.getLogoName();
        debugger
        if (logo == "" || logo == null) {
            logo = "/MRLWMSC21_SL/Images/inventrax.png";
        }
        else {
            logo = "/MRLWMSC21_SL/Images/" + logo;
        }
        var pdfElement = document.getElementById('divFormat');
        pdfElement.innerHTML = "";
        pdfElement.innerHTML += "<span style='display:flex;justify-content:center;'>";
        pdfElement.innerHTML += " <img id='Image1' width='166' height='50' src='" + logo + "'/>";
        pdfElement.innerHTML += "<span style='font-size: 25px;font-weight: 500;position: relative;bottom: 10px;'>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;Receipt Confirmation Report </span></span>";
        var shipment = "";
        var vehicle = "";
        if ($scope.HeaderData.ShipmentType == null) { shipment = ""; } else { shipment = $scope.HeaderData.ShipmentType; }
        if ($scope.HeaderData.VehicleRegistrationNo == null) { vehicle = ""; } else { vehicle = $scope.HeaderData.VehicleRegistrationNo; }

        pdfElement.innerHTML += "<br/><br/><span><b>Receipt ID &emsp;&emsp;: </b> &emsp;" + $scope.HeaderData.StoreRefNo + "</span>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;<span><b>Company Name &emsp;&emsp;: </b> &emsp; " + $scope.HeaderData.TenantName + "</span><br/>";
        pdfElement.innerHTML += "<br/><span><b>Document Date : </b> &emsp;" + $scope.HeaderData.ShipmentReceivedOn + "</span>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;<span><b>Address&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;: </b> &emsp; " + $scope.HeaderData.Address1 + "</span><br/>";
        pdfElement.innerHTML += "<br/>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;<span><b>Shipment Type&emsp;&emsp;&emsp;: </b> &emsp; " + shipment + "</span><br/>";
        pdfElement.innerHTML += "<br/>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;<span><b>Truck No./TrallerID&emsp;: </b> &emsp; " + vehicle + "</span><br/>";

        tableFormat += "<br/><br/><table><thead><tr><th>Item Code</th>";

        if ($scope.TenantID == 58 || $scope.TenantID == 52) {
            tableFormat += "<th>Size</th><th>Collection</th><th>Description</th><th>UoM</th><th>Mfg Date</th><th>Exp Date</th> <th>Expected Qty.</th> <th>Received Qty.</th> <th>Good Qty.</th> <th>Damaged Qty.</th> <th>Excess Qty.</th> <th>Short Qty.</th> <th>Volume (CBM)</th></thead > <tbody>";

        }
        else {
            tableFormat += "<th>Serial No.</th><th>PO Number</th><th>Airway Bill No.</th> <th>UoM</th> <th>Mfg Date</th><th>Exp Date</th><th>Expected Qty.</th> <th>Received Qty.</th> <th>Good Qty.</th> <th>Damaged Qty.</th> <th>Excess Qty.</th> <th>Short Qty.</th> <th>Volume (CBM)</th></thead > <tbody>";

        }

        if (table != null && table.length > 0) {
            for (var i = 0; i < table.length; i++) {
                tableFormat += "<tr></td><td class='aligntext'>" + table[i].MCode + "</td><td class='aligntext'>" + table[i].MCodeAlternative1 + "</td><td class='aligntext'>" + table[i].MCodeAlternative2 + "</td><td class='aligntext'>" + table[i].MDescription + "</td><td class='aligntext'>" + table[i].BUoM + "</td><td class='aligntext'>" + table[i].MfgDate + "</td><td class='aligntext'>" + table[i].ExpDate + "</td><td class='aligntext'>" + table[i].InvoiceQuantity + "</td><td class='aligntext'>" + table[i].ReceivedQty + "</td><td class='aligntext'>" + table[i].GoodQty + "</td><td class='aligntext'>" + table[i].DamagedQty + "</td><td class='aligntext'>" + table[i].ExcessQty + "</td><td class='aligntext'>" + table[i].ExcessOrShortQty + "</td><td class='aligntext'>" + table[i].MVolume + "</td></tr>";
            }
            tableFormat += "</tbody></table>";
            pdfElement.innerHTML += tableFormat;
        }
        else {
            tableFormat += "<tr><td colspan='15' class='aligndate' style='background-color: white'>No Data found</td></tr></tbody></table>";
            pdfElement.innerHTML += tableFormat;

        }
        pdfElement.innerHTML += "<br/><br/><b>DATE</b>";
        pdfElement.innerHTML += "<br/><br/><b>SIGNATURE</b> &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;<b>SIGNATURE</b> &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;<b>SIGNATURE</b>";
        pdfElement.innerHTML += "<br/><br/><b>WAREHOUSE MANAGER</b> &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp;&nbsp;<b>OPERATION SUPERVISOR</b> &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;<b>WMS SUPERVISOR</b>";

        pdfElement.classList.remove("temp");

                var pdf = new jsPDF('p', 'pt', 'a4');
        pdf.setFontSize(18);
        pdf.fromHTML(pdfElement,
            margins.left, // x coord
            margins.top,
            {
                // y coord
                width: margins.width// max width of content on PDF
            }, function (dispose) {
                headerFooterFormatting(pdf)
            },
            margins);

        //var iframe = document.createElement('iframe');
        //iframe.setAttribute('style', 'position:absolute;right:0; top:0; bottom:0; height:100%; width:650px; padding:20px;');
        //document.body.appendChild(iframe);

        //iframe.src = pdf.output('datauristring');

        pdf.save('test.pdf');


        //html2canvas(pdfElement, {
        //    scale: window.devicePixelRatio,
        //    useCORS: true,
        //    onrendered: function (canvas) {
        //        var data = canvas.toDataURL();
        //        var docDefinition = {
        //            content: [{
        //                image: data,
        //                width: 500,
        //                pageSize: 'A4',
        //                height: 900,
        //            }]
        //        };
        //        pdfMake.createPdf(docDefinition).download("ReceiptConfirmationReport.pdf");

        //    }

        //});
        pdfElement.className += " temp";

    }

    $scope.export = function () {
    debugger
        var table = $scope.ReceiptConfirm;
        var tableFormat = "";
        $("#tbldata").empty();
        tableFormat += "<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th>Item Code</th>";
        //$("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th>Item Code</th>");
        if ($scope.TenantID == 58 || $scope.TenantID== 52) {
            tableFormat += "<th>Size</th><th>Collection</th><th>Description</th><th>UoM</th><th>Mfg Date</th><th>Exp Date</th> <th>Expected Qty.</th> <th>Received Qty.</th> <th>Good Qty.</th> <th>Damaged Qty.</th> <th>Excess Qty.</th> <th>Short Qty.</th> <th>Volume (CBM)</th></thead > <tbody>";
            // $("#tbldata").append("<th>Size</th><th>Collection</th><th>Description</th><th>UoM</th> <th>Expected Qty.</th> <th>Received Qty.</th> <th>Good Qty.</th> <th>Damaged Qty.</th> <th>Excess Qty.</th> <th>Short Qty.</th> <th>Volume (CBM)</th></thead > <tbody>");
        }
        else {
            tableFormat += "<th>Serial No.</th><th>PO Number</th><th>Airway Bill No.</th> <th>UoM</th><th>Mfg Date</th><th>Exp Date</th> <th>Expected Qty.</th> <th>Received Qty.</th> <th>Good Qty.</th> <th>Damaged Qty.</th> <th>Excess Qty.</th> <th>Short Qty.</th> <th>Volume (CBM)</th></thead > <tbody>";
            //$("#tbldata").append("<th>Serial No.</th><th>PO Number</th><th>Airway Bill No.</th> <th>UoM</th> <th>Expected Qty.</th> <th>Received Qty.</th> <th>Good Qty.</th> <th>Damaged Qty.</th> <th>Excess Qty.</th> <th>Short Qty.</th> <th>Volume (CBM)</th></thead > <tbody>");
        }
        
        if (table != null && table.length > 0) {
            for (var i = 0; i < table.length; i++) {
                tableFormat += "<tr></td><td class='aligntext'>" + table[i].MCode + "</td><td class='aligntext'>" + table[i].MCodeAlternative1 + "</td><td class='aligntext'>" + table[i].MCodeAlternative2 + "</td><td class='aligntext'>" + table[i].MDescription + "</td><td class='aligntext'>" + table[i].BUoM + "</td><td class='aligntext'>" + table[i].MfgDate + "</td><td class='aligntext'>" + table[i].ExpDate + "</td><td class='aligntext'>" + table[i].InvoiceQuantity + "</td><td class='aligntext'>" + table[i].ReceivedQty + "</td><td class='aligntext'>" + table[i].GoodQty + "</td><td class='aligntext'>" + table[i].DamagedQty + "</td><td class='aligntext'>" + table[i].ExcessQty + "</td><td class='aligntext'>" + table[i].ExcessOrShortQty + "</td><td class='aligntext'>" + table[i].MVolume + "</td></tr>";
                //$("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].MCode + "</td><td class='aligntext'>" + table[i].MCodeAlternative1 + "</td><td class='aligntext'>" + table[i].MCodeAlternative2 + "</td><td class='aligntext'>" + table[i].MDescription + "</td><td class='aligntext'>" + table[i].InvoiceQuantity + "</td><td class='aligntext'>" + table[i].ReceivedQty + "</td><td class='aligntext'>" + table[i].GoodQty + "</td><td class='aligntext'>" + table[i].DamagedQty + "</td><td class='aligntext'>" + table[i].ExcessQty + "</td><td class='aligntext'>" + table[i].ExcessOrShortQty + "</td><td class='aligntext'>" + table[i].MVolume + "</td></tr>");
            }
            tableFormat += "</tbody></table>";
            $("#tbldata").append(tableFormat);
            //$("#tbldata").append("</tbody></table>");
        }
        else {
            tableFormat += "<tr><td colspan='6' class='aligndate' style='background-color: white'>No Data found</td></tr></tbody></table>";
            $("#tbldata").append(tableFormat);
            //$("#tbldata").append("<tr><td colspan='6' class='aligndate' style='background-color: white'>No Data found</td></tr>");
            //$("#tbldata").append("</tbody>");
        }
    }

});
