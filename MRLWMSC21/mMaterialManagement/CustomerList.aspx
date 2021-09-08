<%@ Page Title=" Customer List " Language="C#" AutoEventWireup="true" Debug="true" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" CodeBehind="CustomerList.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.CustomerList" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MMContent" runat="server">
    <asp:ScriptManager ID="mySManager" EnablePageMethods="true" EnablePartialRendering="true" runat="server" SupportsPartialRendering="true">
    </asp:ScriptManager>
    <script src="../mInventory/Scripts/angular.min.js"></script>
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <script src="../mInventory/Scripts/dirPagination.js"></script>
    <link href="../Scripts/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="../mReports/Scripts/tableExport.min.js"></script>

    <script src="../Scripts/xlsx.full.min.js"></script>
    <script src="../Scripts/xlsx-model.js"></script>
    <style>
        .pagination ul {
            display: inline-block;
            padding: 0;
            margin: 0;
        }

        .pagination li {
            display: inline;
        }

        #AddressList {
            margin-top: 15px;
            padding: unset;
        }

        .pagination li a {
            color: black;
            float: left;
            text-decoration: none;
            background-color: #CCCCCC;
            border-radius: 5px;
            border: 2px solid #FFFFFF;
            box-shadow: 1px 3px 5px #CCCCCC;
            border-spacing: 1px;
            margin: 0px 2px;
            box-shadow: var(--z1);
            display: block !important;
            width: 22px !important;
            height: 26px !important;
            text-align: center !important;
            background: #fff;
            border-radius: 2px;
            padding: 1px;
            box-shadow: var(--z1-1) !important;
            line-height: 20px;
        }

        .pagination li.active a {
            color: white;
            display: block !important;
            border: 2px solid var(--sideNav-bg) !important;
            background-color: var(--sideNav-bg) !important;
            line-height: 20px;
            position: relative;
            z-index: 0;
            text-align: center !important;
            height: 26px !important;
        }

        .ss {
            color: #0e0e0e !important;
            font-size: 19px;
            padding: 3px;
            vertical-align: middle;
        }

        .pagination li:hover.active a {
            background-color: #ffffff;
        }

        .pagination li:hover:not(.active) a {
            background-color: #ffffff !important;
            color: black;
        }

        .pagination {
            margin: 8px 5px;
        }

        .btn-info {
            box-shadow: var(--z1);
        }

        #btnEdit, #btnDelete {
            cursor: pointer;
        }

        .instyle select {
            width: 150px !important;
        }
    </style>


    <script>


</script>

    <%-- <asp:UpdateProgress ID="uprgCustomerList" runat="server" AssociatedUpdatePanelID="upnlCustomerList">
            <ProgressTemplate>
                <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">
               
                <div style="align-self:center;" >
                        <div class="spinner">
                    <div class="bounce1"></div>
                    <div class="bounce2"></div>
                    <div class="bounce3"></div>
                </div>

                </div>
                                  
                </div>
                                
                                
            </ProgressTemplate>
            </asp:UpdateProgress>--%>
    <%--<asp:UpdatePanel ID="upnlCustomerList" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
            <ContentTemplate>--%>
    <div class="dashed"></div>
    <div class="pagewidth">
        <div ng-app="CustomerList" ng-controller="CustomerList">


            <div class="gap5"></div>
            <div class="">

                <div class="row">
                    <div class="col s3 m4"></div>
                    <div class="col s3 m3">
                        <div class="flex">
                            <div>
                                <input type="text" id="txtTenant" required="" />
                                 <%--<span class="errorMsg"></span>--%>
                                <label>Tenant</label>
                            </div>
                        </div>
                    </div>
                    <div class="col s3 m3">
                        <div class="flex">
                            <input type="text" ng-model="searchKeyword" id="txtsearch" required="" class="TextboxReport" placeholder="" class="" />
                            <label>Search</label>
                        </div>

                    </div>

                    <div class="col s3 m2">
                        <div class="flex" style="float:right">
                            <button type="button" ng-click="GetCustomerList()" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;
                             <button type="button" class="btn btn-primary" id="btnCustomer" ng-click="AddCustomer()" ng-show="UserRoleDataID != '4'">Add <i class="material-icons vl">add</i></button>
                        </div>
                    </div>
                    </div>

                
                
                <div class="row">
                    <div class="col m3 s4"></div>
                    <div class="col m2 s1"></div>
                    <div class="col m3 s4">
                        <gap5></gap5>
                        <div class="flex" style="align-self: center; margin-right: 5px;">
                            <input ng-show="UserRoleDataID != '4'" id="FUCUSImportExcel" type="file" title="Import To Excel" name="upload" xlsx-model="excel" multiple>
                        </div>
                    </div>
                    <div class="col m4 s7" style="text-align:right !important">
                        <gap></gap>
                        <button type="button" id="btnImpExcel" class="btn btn-primary" ng-click="ImportExcel(excel)" ng-show="UserRoleDataID != '4'">Import<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                        <a ng-show="UserRoleDataID != '4'" class="btn btn-primary" style="align-self: center; margin-left:8px" href="SampleTemplateForMaterial/CustomerExcel.xlsx">Sample Template<i class="material-icons">file_download</i></a>

                        <button type="button" id="btnExcel" class="btn btn-primary" style="margin-left:8px" ng-click="exportExcel()" ng-show="UserRoleDataID != '4'">Export<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                        <%--<button type="button" class="btn btn-primary" id="btnCustomer" ng-click="AddCustomer()" ng-show="UserRoleDataID != '4'">Add <i class="material-icons vl">add</i></button>--%>
                    </div>
                </div>

                
                <div class="bordered">
                    <table class="table-striped" id="tblCustomerList" style="width: 100% !important;">
                        <thead>
                            <tr class="mytableBlueHeaderTR">
                                <th><%= GetGlobalResourceObject("Resource", "Tenant")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "CustomerName")%></th>
                                <%--this line added by lalitha for demo purpose 23/02/2019--%>
                                <th><%= GetGlobalResourceObject("Resource", "CustomerCode")%></th>

                                <th><%= GetGlobalResourceObject("Resource", "Email")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "Mobile")%></th>
                                <th>
                                    <span ng-show="UserRoleDataID != '4'"><%= GetGlobalResourceObject("Resource", "Edit")%></span>
                                </th>
                                <th>
                                    <span ng-show="UserRoleDataID != '4'"><%= GetGlobalResourceObject("Resource", "Delete")%></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                           <%-- <tr ng-show="CustomerList.length > 0" dir-paginate="objstr in CustomerList |filter: searchKeyword| itemsPerPage:25">--%>

                             <tr ng-show="CustomerList.length > 0" dir-paginate="objstr in CustomerList | itemsPerPage:25">
                                <td ng-if="objstr==[]" colspan="12">
                                    <p style="text-align: center">No Data Found</p>
                                </td>
                                <td><span title="{{objstr.TenantName}}">{{objstr.TenantCode}}</span></td>
                                <td><span title="{{objstr.CustomerName}}">{{objstr.CustomerName}}</span></td>
                                <%--This line is added by lalitha on 23/02/2019--%>
                                <td><span title="{{objstr.CustomerCode}}">{{objstr.CustomerCode}}</span> </td>
                                <td>{{objstr.EmailAddress}}</td>
                                <td>{{objstr.Mobile}}</td>
                                <td>
                                    <a ng-show="UserRoleDataID != '4'" ng-click="EditData(objstr)" id="btnEdit"><i class="material-icons ss">mode_edit</i><em class="sugg-tooltis">Edit</em></a>

                                </td>
                                <td><a ng-show="UserRoleDataID != '4'" ng-click="DeleteData(objstr)" id="btnDelete"><i class="material-icons ss">delete</i><em class="sugg-tooltis" style="left: 32px;">Delete</em></a></td>
                            </tr>
                            <tr ng-show="CustomerList.length == 0">
                                <td colspan="12">
                                    <p style="text-align: center">No Data Found</p>
                                </td>
                            </tr>

                        </tbody>
                    </table>


                    <div style="text-align: right;">
                        <dir-pagination-controls boundary-links="true"> </dir-pagination-controls>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div class="dashedborder"></div>
    <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    <div>
        <table id="tbldata"></table>
    </div>
    <script>
        var app = angular.module('CustomerList', ['xlsx-model', 'angularUtils.directives.dirPagination']);
        app.controller('CustomerList', function ($scope, $http) {
            //$scope.searchKeyword = 'DTB';

            $scope.tenantid = 0;




            //=================== Tenant dropdown ================================//
            debugger;
            $('#txtTenant').val("");
            var textfieldname = $("#txtTenant");
            DropdownFunction(textfieldname);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    if ($("#txtTenant").val() == '') {
                        $scope.tenantid = 0;
                    }

                    $.ajax({
                        // url: '../mWebServices/FalconWebService.asmx/GetTenantList',                
                        // data: "{ 'prefix': '" + request.term + "'}",
                        url: '../mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL',
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
                    debugger
                    $scope.tenantid = i.item.val;

                },
                minLength: 0
            });

            //======================= Added By M.D.Prasad For View Only Condition ======================//
            $scope.UserRoleDataID = "";
            var role = '';
            debugger;
            role = '<%=UserRoledat%>';
            role = role.substring(0, role.length - 1);
            role = role.split(',');
            for (var i = 0; i < role.length; i++) {
                if ('<%=UserTypeID%>' == '3' && role[i] == '4') {
                    $scope.UserRoleDataID = role[i];
                }
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            var SearchText = "";
            $scope.getData = function () {
                debugger;
                if ($scope.searchKeyword != undefined || $scope.searchKeyword != null) { SearchText = $scope.searchKeyword; }
                $scope.GetCustomerList();

            }

            $scope.CustomerList = [];
            $scope.GetCustomerList = function () {
                debugger;

                if ( $scope.tenantid == "" || $scope.tenantid == null || $scope.tenantid == undefined) {
                    $scope.tenantid = 0;
                }
                if ($scope.searchKeyword == null || $scope.searchKeyword == undefined) {
                    $scope.searchKeyword = "";
                }

                var AjaxCall =
                {
                    method: 'Post',
                    datatype: 'json',
                    url: 'CustomerList.aspx/GetCustomerList',
                    data: { 'Search': $scope.searchKeyword, 'TenantID': $scope.tenantid },
                    headers: 'application-json; charset=utf-8'
                }


                $http(AjaxCall).then(function (response) {

                    var data = JSON.parse(response.data.d);
                    debugger;
                    $scope.CustomerList = data.Table;
                    SearchText = "";

                });

            }


            $scope.EditData = function (obj) {
                window.location.href = "Customer.aspx?CusId=" + obj.CustomerID;
            }

            $scope.DeleteData = function (obj) {
                debugger;
                if (confirm("Are you sure do you want to delete?")) {
                    var AjaxCall =
                    {
                        method: 'Post',
                        datatype: 'json',
                        url: 'CustomerList.aspx/DeleteCustomer',
                        data: { 'StrId': obj.CustomerID },
                        headers: 'application-json; charset=utf-8'
                    }
                    $http(AjaxCall).then(function (response) {

                        if (response.data.d == "success") {
                            showStickyToast(true, 'Deleted Successfully');
                            $scope.GetCustomerList();
                        }
                        if (response.data.d == "Failed") {

                            showStickyToast(false, 'Could not delete this Customer , because customer mapped to SO');
                            return false;
                        }
                    })
                }
            }

            $scope.AddCustomer = function () {
                window.location.href = "Customer.aspx?CusId=0";
            }




            $scope.GetCustomerList();
            $scope.exportExcel = function () {
                debugger;
                $scope.export();
                $("#tbldata").css('display', 'block');
                $('#tbldata').tableExport({ type: 'excel' });
                $("#tbldata").css('display', 'none');
            }

            $scope.export = function () {
                //
                debugger;
                var table = $scope.CustomerList;
                $("#tbldata").empty();
                $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th>Tenant Name</th><th>Customer Name</th><th>Customer Code</th><th>Email Address</th><th>Mobile</th></thead><tbody>");
                if (table != null && table.length > 0) {
                    for (var i = 0; i < table.length; i++) {
                        $("#tbldata").append("<tr></td><td class='aligndate'>" + table[i].TenantName + "</td><td class='aligndate'>" + table[i].CustomerName + "</td><td class='aligndate'>" + table[i].CustomerCode + "</td><td class='aligndate'>" + table[i].EmailAddress + "</td><td class='aligndate'>" + table[i].Mobile + "</td></tr>");
                    }
                    $("#tbldata").append("</tbody></table>");
                }
                else {

                    $("#tbldata").append("<tr><td colspan='9' class='aligndate' style='background-color: white'>No Data Found</td></tr>");
                    $("#tbldata").append("</tbody>");
                }
            }

            function validateEmail($email) {
                var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
                return emailReg.test($email);
            }

            $scope.ImportExcel = function (data) {
                debugger;
                if (data == undefined) {
                    showStickyToast(false, "Please Upload Excel Sheet");
                }
                //var filename0 = $('input[type=file]').val().split('\\').pop();
                //var filename1 = filename0.split(".")[0];
                //var filename2 = filename1.split(" ")[0];

                //if (filename2 != "CustomerExcel") {
                //    showStickyToast(false, "Please upload valid File");
                //    return false;
                //}
                //var ss = file.split('\');
                var filename = JSON.stringify(Object.keys(data));
                var excelname = JSON.parse(filename.replace(/(\{|,)\s*(.+?)\s*:/g, '$1 "$2":'));
                var BOM = data[excelname].Sheet1;
                if (BOM == null || BOM == undefined) {
                    showStickyToast(false, "Please Upload valid File");
                    return false;
                }
                if (BOM.length == 0) {
                    showStickyToast(false, "Please Fill data in Excel");
                    return false;
                }

                for (i = 0; i < BOM.length; i++) {

                    if (BOM[i].Email != undefined) {
                        if (!validateEmail(BOM[i].Email)) {
                            showStickyToast(false, "Please enter valid Email ");
                            return false;
                        }
                    }
                    if (BOM[i].Mobile != undefined) {
                        if (BOM[i].Mobile.length != 10) {

                            showStickyToast(false, "Enter 10 digits Mobile number");
                            return false;
                        }
                    }
                }


                var http = {
                    method: 'POST',
                    url: '../mMaterialManagement/CustomerList.aspx/ImportExcelCustomerData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'inblst': BOM }
                }
                $http(http).success(function (response) {

                    debugger;

                    var result = response;
                    $scope.excel = null;
                    //$scope.inbdata = response.d;
                    if (result.d == "000" || result.d == "-8") {
                        showStickyToast(false, "Please fill the data in excel sheet ");
                        $("#FUCUSImportExcel").val("");
                        return false;
                    }

                    if (result.d == "-222") {
                        showStickyToast(true, "Few mandatory Fields are not entered");
                        return false;
                    }
                    else if (result.d == "1") {
                        showStickyToast(true, "Customer Created successfully.");
                        setTimeout(function () {
                            location.reload();
                        }, 4000);
                        //$("inbdata").css("display", "none");
                    }
                    else if (result.d == "-6") {
                        showStickyToast(false, "Error while Import");
                        return false;
                    }
                    else if (result.d == "-1" || result.d == "-2") {
                        debugger;
                        angular.element("input[type='file']").val(null);
                        showStickyToast(false, "Errors in imported Excel Sheet, Please View the downloaded File");
                        window.open('../ExcelData/FailedItems.xlsx');
                        setTimeout(function () {
                            //location.reload();
                        }, 4000);
                    }
                    //else if (result[0] == "-1") {
                    //    showStickyToast(false, "Tenant Not Exists at row" + result[1]);
                    //    setTimeout(function () {
                    //        location.reload();
                    //    }, 3000);
                    //}
                    //else if (result[0] == "-2") {
                    //    showStickyToast(false, "Customer already Exists at row" + result[1]);
                    //    setTimeout(function () {
                    //        location.reload();
                    //    }, 3000);
                    //}

                });
            }

        });

    </script>



</asp:Content>
