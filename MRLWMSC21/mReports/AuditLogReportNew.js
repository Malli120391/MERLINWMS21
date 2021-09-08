var myApp = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
myApp.controller('AuditLogReportNew', function ($scope, $http) {

   
        var AuditID = 2;
        var RefNumberID = '1';
        var textfieldname = $("#txtRefNo");
        DropdownFunction(textfieldname);
        $("#txtRefNo").autocomplete({
            source: function (request, response) {
                AuditID = $('#ddlCategory').val();
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetReferenceNumbers',
                    data: "{ 'prefix': '" + request.term + "', 'CategoryID':'" + AuditID + "'}",
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
                RefNumberID = i.item.val;
            },
            minLength: 0
        });

       
        $scope.GetALdetails = function () {
            debugger;
            $.ajax({
                url: 'AuditLogReportNew.aspx/GetData',
                data: "{ 'CategoryID':'" + AuditID + "', 'RefID': '" + RefNumberID + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    console.log(data.d);
                    $scope.AuditList = data.d;
                    if ($scope.AuditList == undefined || $scope.AuditList == null || $scope.AuditList.length == 0)
                        showStickyToast(false, "No Data found");
                    LoadClickHandlers();
                },
                error: function (data) {

                }

            });
        }


        $scope.exportExcel = function () {
            //
            $scope.export();
            //$("#tbldata").css('display', 'block');
            $('#tbldata').tableExport({ type: 'excel' });
            //$("#tbldata").css('display', 'none');
        }


        $scope.export = function () {

            // $scope.GetALdetails();
            //var table = $scope.AuditList;
            //$("#tbldata").empty();
            //$("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> LineNo</th><th>PartNumber </th><th>Quantity</th><th>Status</th><th>QCParameters</th></thead><tbody>");
            //for (var i = 0; i < table.length; i++) {
            //    $("#tbldata").append("<tr></td><td class='aligndate'>" + table[i].LineNo + "</td><td class='aligndate'>" + table[i].PartNumber + "</td><td class='aligndate'>" + table[i].Quantity + "</td><td class='aligndate'>" + table[i].Status + "</td><td class='aligndate'>" + table[i].QCParameters + "</td></tr>");
            //}
            //$("#tbldata").append("</tbody></table>");
        }
        
});