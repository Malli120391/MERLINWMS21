<%@ Page Language="C#" Title="User Audit Report" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="LogAuditReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.LogAuditReportNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
<%--    <script src="LogAuditReportNew.js"></script>--%>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB-amPYw4EvJGyYfY16HzhF2lqpw--FcHM&libraries=places"></script>


    <script>
        //================================== Created By M.D.Prasad ======================================//

        var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
        myApp.controller('LogAuditReportNew', function ($scope, $http) {
           

            $scope.GetUserDetails = function() {
            //function GetUserDetails() {
                debugger;
                var AccountID = "";
                var UserID = "";
                var FromDate = "";
                var ToDate = "";
                //$scope.UserAuditDetails = [];
                //$scope.UserAuditData = [];
                //$scope.UserAuditData2 = [];

                if ($("#txtAccount").val() == "")
                {
                    showStickyToast(false, "Please Select Account", false);
                    return false;
                }

                //if ($("#txtUser").val() == "") {
                //    showStickyToast(false, "Please Select User", false);
                //    return false;
                //}

                if ($("#AccountID").val() == "0") {
                    AccountID = "0";
                }
                else {
                    AccountID = $("#AccountID").val();
                }

                if ($("#UserID").val() == "0") {
                    UserID = "0";
                }
                else {
                    UserID = $("#UserID").val();
                }
                if ($("#txtFromDate").val() == "" || $("#txtFromDate").val() == undefined) {
                    FromDate = "0";
                }
                else {
                    FromDate = $("#txtFromDate").val();
                }

                if ($("#txtToDate").val() == "" || $("#txtToDate").val() == undefined) {
                    ToDate = "0";
                }
                else {
                    ToDate = $("#txtToDate").val();
                }
                debugger;
                //var dataURL = "http://192.168.1.20/SSOServices/Service/Audit.svc/UserLogs/2/0/0/0/0/0";
                var dataURL = '<%=ConfigurationManager.AppSettings["UserLogAuditURl"].ToString() %>';

                var Params = "/" + AccountID + "/" + UserID + "/" + FromDate + "/" + ToDate + "/0"
                var URL = dataURL + Params;

                var httpreq = $.ajax({
                    type: 'get', 
                    url: URL,
                    dataType: 'json',
                    success: function (data) {
                        debugger;  
                        $scope.UserAuditDetails = JSON.parse(data);
                        $scope.UserAuditData = $scope.UserAuditDetails.Table;
                        $scope.UserAuditData2 = $scope.UserAuditDetails.Table1;
                    }
                });
            }
            
    $scope.Getgedetails = function () {
        //


      if ($("#UserID").val() == "0") {
                    UserID = "0";
                }
                else {
                    UserID = $("#UserID").val();
                }
                if ($("#txtFromDate").val() == "" || $("#txtFromDate").val() == undefined) {
                    FromDate = "";
                }
                else {
                    FromDate = $("#txtFromDate").val();
                }

                if ($("#txtToDate").val() == "" || $("#txtToDate").val() == undefined) {
                    ToDate = "";
                }
                else {
                    ToDate = $("#txtToDate").val();
                }
        var httpreq = {
            method: 'POST',
            url: 'LogAuditReportNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'UserID': UserID, 'FromDate': FromDate, 'ToDate': ToDate },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            debugger;
         //   $scope.BIllingReport = response.d;
             $scope.UserAuditDetails = response.d;
                        $scope.UserAuditData = $scope.UserAuditDetails.Table;
                        $scope.UserAuditData2 = $scope.UserAuditDetails.Table1;
            if ($scope.UserAuditDetails == undefined || $scope.UserAuditDetails == null || $scope.UserAuditDetails.length == 0)
                showStickyToast(false, "No Data Found");
        })
    }

            $scope.ClearDetails = function ()
            {
                $("#AccountID").val("0");
                $("#txtAccount").val("");
                $("#UserID").val("0");
                $("#txtUser").val("");
                $("#txtFromDate").val("");
                $("#txtToDate").val("");
            }

            $("#txtFromDate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txtToDate").datepicker("option", "minDate", selected, { dateFormate: "dd-M-yy" })
                }
            });
            $("#txtToDate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                }
            });

            $('#txtFromDate, #txtToDate').keypress(function () {
                return false;
            });

            Accountid = <%=this.cp.AccountID %>
            if(Accountid == 0) {
                Accountid = 0;
            }
            else {
                Accountid = Accountid;
            }

            var TextFieldName = $("#txtAccount");
            DropdownFunction(TextFieldName);
            $("#txtAccount").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadAccountForCyccleCount") %>',
                        data: "{ 'prefix': '" + request.term + "','AccountID':'" + Accountid + "'}",//<=cp.TenantID%>
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

                    $("#AccountID").val(i.item.val);
                    $("#txtUser").val("");
                    $("#UserID").val("0")
                },
                minLength: 0
            });


            var TextFieldName = $("#txtUser");
            DropdownFunction(TextFieldName);
            $("#txtUser").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersForAudit") %>',
                        data: "{ 'prefix': '" + request.term + "','AccountID':'" + $("#AccountID").val() + "'}",//<=cp.TenantID%>
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

                    $("#UserID").val(i.item.val);

                },
                minLength: 0
            });


        });
        //================================== Created By M.D.Prasad ======================================//
    </script>


    <style>
        .mytableReportHeaderTR {
            color: #000 !important;
            background-color: #fff !important;
            text-align: justify;
        }

        .module_Green {
            border-right: none !important;
            border-left: none !important;
        }

        .mytableReportBodyTR tr:nth-child(even) {
            background-color: #fff !important;
        }
    </style>
    <link href="Scripts/Custom.css" rel="stylesheet" />
 <div class="dashed"></div>
<div class="pagewidth">   
    <div ng-app="myApp" ng-controller="LogAuditReportNew">
        <input type="hidden" id="AccountID" value="0" />
        <input type="hidden" id="UserID" value="0" />
        <div class="divlineheight"></div>
        <div class="row">
            <div class="col m4">
                <div class="buttonrowstyle" style="display: none;">
                    <button type="button" id="btnPdf" class="button button3" ng-click="exportPdf()">PDF &nbsp;<i class="fa fa-file-pdf-o" aria-hidden="true"></i></button>
                    <button type="button" id="btnExcel" class="button button1" ng-click="exportExcel()">Excel &nbsp;<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>

                </div>
            </div>
            <div class="col m8">
                <div class="flex__ right">
                    <div class="">
                        <div class="flex">
                            <input type="text" id="txtAccount" required="" />
                            <label>Account</label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                    &nbsp;&nbsp;
                    <div class="">
                        <div class="flex">
                            <input type="text" id="txtUser" required="" />
                            <label>User</label>                           
                        </div>
                    </div>
                    &nbsp;&nbsp;

                    <div>
                        <div class="flex">
                            <input type="text" required="" style="width: 120px;" id="txtFromDate" />
                            <label>From Date</label>
                        </div>
                    </div>
                    &nbsp;&nbsp;
                    <div>
                        <div class="flex">
                            <input type="text" required="" style="width: 120px;" id="txtToDate" />
                            <label>To Date</label>
                        </div>
                    </div>
                    &nbsp;&nbsp;
                    <div>
                      <%--  <button type="button" ng-click="GetUserDetails()" class="btn btn-primary">View Report <i class="fa fa-eye"></i></button>
                     --%>   <button type="button" ng-click="Getgedetails()" class="btn btn-primary">View Report <i class="fa fa-eye"></i></button>
                       
                        <button type="button" ng-click="ClearDetails()" class="btn btn-primary">Clear <i class="fa fa-ban"></i></button>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class="table" id="tbldatas">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th>S.No.</th>
                                <th>AccountName      </th>
                                <th>Client IP        </th>
                                <th>Client Name      </th>
                                <th>Client Type      </th>
                                <th>User Name        </th>
                                <th>Session Begin    </th>
                                <th>Session Duration </th>
                                <th>Session End      </th>
                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="UAD in UserAuditDetails|itemsPerPage:10" pagination-id="main">
                                <td align="center">{{$index +1}}</td>
                             <%--   <td>{{UAD.AccountName}}</td>
                                <td>{{UAD.ClientIP}}</td>
                                <td>{{UAD.ClientName}}</td>
                                <td>{{UAD.ClientType}}</td>
                                <td>{{UAD.UserName}}</td>
                                <td>{{UAD.SessionBegin}}</td>
                                <td>{{UAD.SessionDuration}}</td>
                                <td>{{UAD.SessionEnd}}</td>--%>
                                 <td>{{UAD.AccountName}}</td>
                                <td>{{UAD.IPAddress}}</td>
                                <td>{{UAD.ClientName}}</td>
                                <td>{{UAD.ClientType}}</td>
                                <td>{{UAD.EmployeeName}}</td>
                                <td>{{UAD.LoginTime}}</td>
                                <td>{{UAD.SessionTime}}</td>
                                <td>{{UAD.LogoutTime}}</td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="mytableReportFooterTR">
                                <td colspan="15">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true" pagination-id="main"> </dir-pagination-controls>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>

                    </table>
                </div>

                <table id="tbldata"></table>
            </div>
        </div>

        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class="mytableOutbound table-striped" id="tbldatas1">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th>S.No.</th>
                                <th>AccountName      </th>
                                <th>Client IP        </th>
                                <th>Client Name      </th>
                                <th>Client Type      </th>
                                <th>User Name        </th>
                                <th>Client MAC    </th>
                                <th>No. Of Unique Requests </th>
                                <th>Request Timestamp     </th>
                                <th>Status Code     </th>
                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="UAD1 in UserAuditData2|itemsPerPage:10" pagination-id="child">
                                <td align="center">{{$index +1}}</td>
                                <td>{{UAD1.AccountName}}</td>
                                <td>{{UAD1.ClientIP}}</td>
                                <td>{{UAD1.ClientName}}</td>
                                <td>{{UAD1.ClientType}}</td>
                                <td>{{UAD1.UserName}}</td>
                                <td>{{UAD1.ClientMAC}}</td>
                                <td>{{UAD1.NoOfUniqueRequests}}</td>
                                <td>{{UAD1.RequestTimestamp}}</td>
                                <td>{{UAD1.StatusCode}}</td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="mytableReportFooterTR">
                                <td colspan="15">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true" pagination-id="child"> </dir-pagination-controls>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>

                    </table>
                </div>

                <table id="tbldata"></table>
            </div>
        </div>
    </div>    
    </div>
</asp:Content>
