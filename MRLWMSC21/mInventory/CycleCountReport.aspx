<%@ Page Title="Cycle Count Report" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="CycleCountReport.aspx.cs" Inherits="MRLWMSC21.mInventory.CycleCountReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">

    <%--     <link href="../Scripts/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="CycleCountScripts/bootstrap.min.js"></script>
    <script src="CycleCountScripts/jquery.dataTables.min.js"></script>
    <script src="CycleCountScripts/dataTables.bootstrap.min.js"></script> 
   
    <script src="../mInbound/Scripts/InventraxAjax.js"></script>--%>

    <script src="Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <script src="../Scripts/CommonWMS.js"></script>

    <input type="hidden" value='' id="hdnViewName" />
    <input type="hidden" value='' id="hdnSp_Get" />
    <input type="hidden" value='' id="hdnSp_Set" />
    <input type="hidden" value='' id="hdnJSONMaster" />
    <input type="hidden" value='0' id="hdnCId" />

    <input type="hidden" value='1' id="hdnCreatedBy" />
    <input type="hidden" value='2018-01-04' id="hdnUpdatedOn" />
    <input type="hidden" value='1' id="hdnUpdatedBy" />

    <style>
        /* Absolute Center Spinner */
        .loading {
            position: fixed;
            z-index: 9999;
            height: 2em;
            width: 2em;
            overflow: show;
            margin: auto;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
        }

            /* Transparent Overlay */
            .loading:before {
                content: '';
                display: block;
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background-color: rgba(0,0,0,0.3);
            }

            /* :not(:required) hides these rules from IE9 and below */
            .loading:not(:required) {
                /* hide "loading..." text */
                font: 0/0 a;
                color: transparent;
                text-shadow: none;
                background-color: transparent;
                border: 0;
            }

                .loading:not(:required):after {
                    content: '';
                    display: block;
                    font-size: 10px;
                    width: 1em;
                    height: 1em;
                    margin-top: -0.5em;
                    -webkit-animation: spinner 1500ms infinite linear;
                    -moz-animation: spinner 1500ms infinite linear;
                    -ms-animation: spinner 1500ms infinite linear;
                    -o-animation: spinner 1500ms infinite linear;
                    animation: spinner 1500ms infinite linear;
                    border-radius: 0.5em;
                    -webkit-box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.5) -1.5em 0 0 0, rgba(0, 0, 0, 0.5) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                    box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) -1.5em 0 0 0, rgba(0, 0, 0, 0.75) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                }

        /* Animation */

        @-webkit-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-moz-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-o-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        .labelfontbold {
            font-weight: normal !important;
            width: 100% !important;
        }

        .lblFormItem {
            font-weight: bold !important;
            width: 100% !important;
        }

            .lblFormItem:after, .labelfontbold:after {
                content: "" !important;
            }
    </style>

    <script>
        var myApp = angular.module("myApp", ['angularUtils.directives.dirPagination']);
        myApp.controller("CCReport", function ($scope, $http, $window) {
            $scope.noofrecords = 25;
            $scope.Totalrecords = 0;
            ItemList = [];
            $scope.CTRNID = 0;
            var CycleCountID = new URL(window.location.href).searchParams.get("parm");
            $scope.CTRNID = new URL(window.location.href).searchParams.get("parm");
            $scope.getCCReportData = function (PaginationId, CCID, typeID) {
                debugger
                if (CCID == 0) {
                    CycleCountID = CycleCountID;
                }
                else {
                    CycleCountID = CCID;
                }
                $scope.CTRNID == null ? typeID = 2 : typeID = 1;
                if (typeID == 2) {
                    if ($("#txtWarehouse").val() == "" || $("#hdnWarehouse").val() == "0") {
                        $("#hdnWarehouse").val("0");
                        showStickyToast(false, "Please select Warehouse", false);
                        return false;
                    }
                    if ($("#CCM_CNF_AccountCycleCount_ID").val() == "0" || $("#txtCycleCountName").val() == "") {
                        showStickyToast(false, "Please select Cycle Count Name", false);
                        return false;
                    }
                    if ($('#txtCycleCountCode').val() == "") {
                        showStickyToast(false, "Please select Cycle Count Code", false);
                        return false;
                    }
                    //CycleCountID = $("#CycleCountCode").val();
                }
                else {
                    $("#CycleCountCode").val(CycleCountID);
                }
                var LocationID = 0; MMID = 0;
                if ($("#txtLocation").val() == "") { LocationID = 0; } else { LocationID = $("#hdnLocationID").val(); }
                if ($("#txtPartnumber").val() == "") { MMID = 0; } else { MMID = $("#hdnMID").val(); }
               
                $("#divLoading").show();
                var httpreq = {
                    method: 'POST',
                    url: 'CycleCountReport.aspx/GetList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'CTID': $("#CycleCountCode").val(), 'PageIndex': PaginationId, 'PageSize': $scope.noofrecords, 'LocationID': LocationID, 'MaterialMasterID': MMID },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    debugger
                    var dt = JSON.parse(response.d).Table;
                    if (dt != null && dt.length > 0) {
                        $scope.CCReport = dt;
                        $scope.Totalrecords = dt[0].TotalRecords;
                        ItemList = dt;
                        $("#hdnCount").val(dt.length);
                        $("#divLoading").hide();
                        $scope.SetData(JSON.parse(response.d));
                    }
                    else {
                        $scope.CCReport = null;
                        $("#divLoading").hide();
                        showStickyToast(false, "No Data Found", false);
                        return false;                        
                    }
                });
            };
            if (CycleCountID == 0 || CycleCountID == null) {
                CycleCountID = 0;
               // $scope.getCCReportData(1, CycleCountID, 2);
                $(".divDropdowns").css("display", "block");
                $("#divLabels").css("display", "none");
                $(".md-select-underline").css("display", "block");
            }
            else {
                $scope.getCCReportData(1, CycleCountID, 1);
                $(".divDropdowns").css("display", "none");
                $(".md-select-underline").css("display", "none");
                $("#divLabels").css("display", "block");
            }

            $scope.SetData = function (dataList) {
                debugger;
                //$scope.LoadDropDowns(dataList);
                if (CycleCountID == 0 || CycleCountID == null) {
                    //$scope.BindData(null);
                    $scope.BindData(dataList.Table);
                }
                else {
                    $scope.BindData(dataList.Table);
                }
            };


            $scope.BindData = function (item) {
                //debugger;
                if (item != null && item.length > 0) {
                    $('.fieldToGet').each(function () {
                        var fieldID = $(this).attr('id');
                        var paramtype = $(this).attr('type');
                        $('#' + fieldID).text(item[0][fieldID]);
                        if (fieldID = "lblCycleCountCode)") {  $('#lblCycleCountCode').text(item[0]["CycleCountCode"]);}
                    });
                }
                else {
                    $('.fieldToGet').each(function () {
                        var fieldID = $(this).attr('id');
                        var paramtype = $(this).attr('type');
                        $('#' + fieldID).text("");
                        if (fieldID = "lblCycleCountCode)") {  $('#lblCycleCountCode').text("");}
                    });
                }
            };

            /*$scope.LoadDropDowns = function (dataList) {
                $("#CCM_MST_CycleCount_ID").empty();
                $("#CCM_MST_CycleCount_ID").append($("<option></option>").val(0).html("Please Select"));
                for (var x = 0; x < dataList.Table1.length; x++) {
                    $("#CCM_MST_CycleCount_ID").append($("<option></option>").val(dataList.Table1[x].CCM_MST_CycleCount_ID).html(dataList.Table1[x].CycleCountName));
                }
            };*/

            //$("#txtWarehouse").val("");
            var textfieldname = $("#txtWarehouse");
            DropdownFunction(textfieldname);
            $("#txtWarehouse").autocomplete({
                source: function (request, response) {
                    var Accountid = <%=this.cp2.AccountID %>;
                    //alert(Accountid);
                    if (Accountid == 0) {
                        Accountid = 0;
                    }
                    else {
                        Accountid = Accountid;
                    }
                    $.ajax({
                        //url: '../mWebServices/FalconWebService.asmx/LoadWarehouseByAccount',
                        //data: "{ 'prefix': '" + request.term + "','AccountID':'" + Accountid + "'}",//<=cp.TenantID%>
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
                    $("#hdnWarehouse").val(i.item.val);
                    $('#txtCycleCountName').val("");
                    $("#CCM_CNF_AccountCycleCount_ID").val("0");
                    $('#txtCycleCountCode').val("");
                    $("#CCM_CNF_AccountCycleCount_ID").val("0");
                    $("#txtLocation").val("");
                    $("#hdnLocationID").val("0");
                    $('#txtPartnumber').val("");
                },
                minLength: 0
            });

            //$('#txtCycleCountName').val("");
            var textfieldname = $("#txtCycleCountName");
            DropdownFunction(textfieldname);
            $("#txtCycleCountName").autocomplete({
                source: function (request, response) {
                    debugger
                    if ($("#txtWarehouse").val() == "" || $("#hdnWarehouse").val() == "0") {
                        $("#hdnWarehouse").val("0");
                        showStickyToast(false, "Please select Warehouse", false);
                        return false;
                    }
                    if ($("#CCM_MST_CycleCount_ID").val() == "0") {
                        showStickyToast(false, "Please select Cycle Count Type", false);
                        return false;
                    }
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/GetCCNames',
                        data: "{ 'prefix': '" + request.term + "','CCMID':'" + $("#CCM_MST_CycleCount_ID").val() + "','AccountID':'" +<%=cp2.AccountID%>+"','WHID':'"+$("#hdnWarehouse").val()+"'}",
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
                    //ACCID = i.item.val;
                    $("#CCM_CNF_AccountCycleCount_ID").val(i.item.val);
                    $('#txtCycleCountCode').val("");
                },
                minLength: 0
            });


            //$('#txtCycleCountCode').val("");
            var textfieldname = $("#txtCycleCountCode");
            DropdownFunction(textfieldname);
            $("#txtCycleCountCode").autocomplete({
                source: function (request, response) {
                    //if ($("#CCM_MST_CycleCount_ID").val() == "0") {
                    //    showStickyToast(false, "Please select Cycle Count Type", false);
                    //    return false;
                    //}
                    if ($("#txtWarehouse").val() == "" || $("#hdnWarehouse").val() == "0") {
                        showStickyToast(false, "Please select Warehouse", false);
                        return false;
                    }
                    if ($("#CCM_CNF_AccountCycleCount_ID").val() == "0" || $("#txtCycleCountName").val() == "") {
                        showStickyToast(false, "Please select Cycle Count Name", false);
                        return false;
                    }
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/GetCycleCountCodes',
                        data: "{ 'prefix': '" + request.term + "','ACCID':'" + $("#CCM_CNF_AccountCycleCount_ID").val() + "','WHID':'"+$("#hdnWarehouse").val()+"'}",
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
                    debugger;
                    CycleCountTRNID = i.item.val;
                    $("#CycleCountCode").val(i.item.val);
                    //$scope.getCCReportData(1, CycleCountTRNID);
                },
                minLength: 0
            });

            //$("#txtLocation").val("");
            var textfieldname = $("#txtLocation");
            DropdownFunction(textfieldname);
            $("#txtLocation").autocomplete({
                source: function (request, response) {
                    //debugger;
                    if ($("#txtWarehouse").val() == "" || $("#hdnWarehouse").val() == "0") {
                        showStickyToast(false, "Please select Warehouse", false);
                        return false;
                    }
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadLocationsForCurrentStock',
                        data: "{ 'prefix': '" + request.term + "','WarehouseID':'" + $("#hdnWarehouse").val() + "'}",
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
                    debugger;
                    $("#hdnLocationID").val(i.item.val);
                },
                minLength: 0
            });

            //$('#txtTenant').val("");
            var textfieldname = $("#txtTenant");
            DropdownFunction(textfieldname);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    if ($("#txtTenant").val() == '') {
                        RefTenant = 0;
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
                    $("#hdnTenantID").val(i.item.val);
                    $("#txtPartnumber").val("");
                },
                minLength: 0
            });

            //$('#txtPartnumber').val("");
            var textfieldname = $("#txtPartnumber");
            DropdownFunction(textfieldname);
            $("#txtPartnumber").autocomplete({
                source: function (request, response) {
                    debugger;
                    //if ($("#hdnTenantID").val() == "0" || $("#txtTenant").val() == "") {
                    //    showStickyToast(false, 'Please select Tenant');
                    //    return false;
                    //}
                    $.ajax({
                        //url: '../mWebServices/FalconWebService.asmx/LoadMaterials',
                        url: '../mWebServices/FalconWebService.asmx/LoadMaterialsForCycleCountReport',   // added by Ganesh  @Oct 1 2020 -- PartNumber should be displyed by UnderWareHouse
                        data: "{ 'prefix': '" + request.term + "','WarehouseId':'" +  $("#hdnWarehouse").val() + "'}",
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
                    $("#hdnMID").val(i.item.val);
                },
                minLength: 0
            });

            $scope.getReportData = function () {
                debugger;
                if (CycleCountID == 0 || CycleCountID == null) {
                    CycleCountID = 0;
                    $scope.getCCReportData(1, CycleCountID, 2);
                }
                else {
                    $scope.getCCReportData(1, CycleCountID, 1);
                }
            };
        });
    </script>
    <div class="module_yellow">
        <div class="ModuleHeader" height="35px">
            <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Orders</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Cycle Count Report </span></div>
        </div>
    </div>
    <div class="container" ng-app="myApp" ng-controller="CCReport">
        <div class="loading" id="divLoading" style="display: none;"></div>
        <br />
        <div class="row">
            <div class="divDropdowns" style="display: none;">
                <div class="col m4" style="display: none;">
                    <div class="flex">
                        <select id="CCM_MST_CycleCount_ID" class="" required="">
                            <option value="0">Please Select</option>
                            <option value="3" selected>Standard Cycle Count</option>
                        </select>
                        <label class="lblFormItem">Cycle Count Type</label>
                        <span class="errorMsg"></span>
                    </div>
                </div>
                <div class="col m4" style="display:none;">
                    <div class="flex">
                        <asp:TextBox runat="server" ID="txtTenant" ClientIDMode="Static" required=""></asp:TextBox>
                        <label>Tenant</label>
                        <asp:HiddenField runat="server" ID="hdnTenantID" ClientIDMode="Static" Value="0" />
                    </div>
                </div>
                <div class="col m4">
                    <div class="flex">
                        <asp:TextBox runat="server" ID="txtWarehouse" ClientIDMode="Static" required=""></asp:TextBox>
                        <label>Warehouse</label>
                        <span class="errorMsg"></span>
                        <asp:HiddenField runat="server" ID="hdnWarehouse" ClientIDMode="Static" Value="0" />
                    </div>
                </div>

                <div class="col m4">
                    <div class="flex">
                        <%--<input type="text" id="txtCycleCountName" required="" />--%>
                        <asp:TextBox runat="server" ID="txtCycleCountName" ClientIDMode="Static" required=""></asp:TextBox>
                        <label>Cycle Count Name</label>
                        <span class="errorMsg"></span>
                        <asp:HiddenField runat="server" ID="CCM_CNF_AccountCycleCount_ID" ClientIDMode="Static" Value="0" />
                        <%--<input type="hidden" value="0" id="CCM_CNF_AccountCycleCount_ID" />--%>
                    </div>
                </div>

                <div class="col m4">
                    <div class="flex">
                        <%--<input type="text" id="txtCycleCountCode" required="" />--%>
                        <asp:TextBox runat="server" ID="txtCycleCountCode" ClientIDMode="Static" required=""></asp:TextBox>
                        <label>Cycle Count Seq. Code</label>
                        <span class="errorMsg"></span>
                        <asp:HiddenField runat="server" ID="CycleCountCode" ClientIDMode="Static" Value="0" />
                        <%--<input type="hidden" value="0" id="CycleCountCode" />--%>
                    </div>
                </div>
                <div class="col m4">
                    <div class="flex">
                        <asp:TextBox runat="server" ID="txtLocation" ClientIDMode="Static" required=""></asp:TextBox>
                        <label>Location</label>
                        <asp:HiddenField runat="server" ID="hdnLocationID" ClientIDMode="Static" Value="0" />
                    </div>
                </div>
                <div class="col m4">
                    <div class="flex">
                        <asp:TextBox runat="server" ID="txtPartnumber" ClientIDMode="Static" required=""></asp:TextBox>
                        <label>Part Number</label>
                        <asp:HiddenField runat="server" ID="hdnMID" ClientIDMode="Static" Value="0" />
                        <asp:HiddenField runat="server" ID="hdnCount" ClientIDMode="Static" Value="0" />
                    </div>
                </div>
                <div class="col m4"><gap5></gap5>
                    <button type="button" ng-click="getReportData();" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                    <asp:LinkButton runat="server" ID="lnkExportData" CssClass="btn btn-primary" OnClick="lnkExportData_Click">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></asp:LinkButton>
                </div>
            </div>

            <div id="divLabels">
                <div class="col m4">
                    <label class="lblFormItem">Cycle Count Type : </label>
                    <label id="CycleCountName" class="labelfontbold fieldToGet"></label>
                </div>

                <div class="col m4">
                    <label class="lblFormItem">Cycle Count Name : </label>
                    <label id="AccountCycleCountName" class="labelfontbold fieldToGet"></label>
                </div>

                <div class="col m4">
                    <label class="lblFormItem">Cycle Count Seq. Code : </label>
                    <label id="lblCycleCountCode" class="labelfontbold fieldToGet"></label>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col m4" hidden>
                <label class="lblFormItem">Cycle Time In Days : </label>
                <label class="labelfontbold fieldToGet" id="CycleTimeInDays"></label>
            </div>

             <div class="col m4">
                <label class="lblFormItem">Seq. No. : </label>
                <label class="labelfontbold fieldToGet" id="SeqNo"></label>
            </div>           

            <div class="col m4">
                <label class="lblFormItem fieldToGet">Frequency :</label>
                <label class="labelfontbold fieldToGet" id="FrequencyName"></label>
            </div>

             <div class="col m4">
                <label class="lblFormItem">Status :</label>
                <label class="labelfontbold fieldToGet" id="StatusName"></label>
            </div>
            
        </div> 
       

        <div class="row">
            <div class="col m12">
                <table class="table-striped">
                    <thead>
                        <tr>
                            <th>S.No </th>
                            <%--<th>Cycle Count Name</th>--%>                           
                            <th>Location</th>
                            <th>Part Number</th>
                            <%--<th>Container</th>--%>
                            <th>SLoc.</th>
                            <th>Mfg Date</th>
                            <th>Exp Date</th>
                            <th>Batch No.</th>
                            <th>Serial No.</th>
                            <%--<th>Project Ref. No.</th>--%>
                           <%-- <th>MRP</th>--%>
                            <th number>Logical Qty.</th>
                            <th number>Physical Qty.</th>
                            <th center>Activity Timestamp</th>
                           <%-- <th number>Seq. No.</th>--%>
                            <th>User Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr dir-paginate="cc in CCReport|filter:searchData|itemsPerPage:25" total-items="Totalrecords">
                            <%-- <td>{{$index+1}}</td>--%>
                            <td>{{cc.RID}}</td>
                           <%-- <td>{{cc.AccountCycleCountName}}</td>--%>                            
                            <td>{{cc.Location}}</td>
                            <td>{{cc.MCode}}</td>
                            <%--<td>{{cc['Carton Code']}}</td>--%>
                            <td>{{cc['SLoc.']}}</td>
                            <td>{{cc['Mfg Date']}}</td>
                            <td>{{cc['Exp Date']}}</td>
                            <td>{{cc['Batch No.']}}</td>
                            <td>{{cc['Serial No.']}}</td>
                            <%--<td>{{cc['Project Ref. No.']}}</td>--%>
                            <%--<td>{{cc.MRP}}</td>--%>
                            <td number>{{cc.LogicalQuantity}}</td>
                            <td number>{{cc.PhysicalQuantity}}</td>
                            <td style="text-align:center !important;">{{cc.ActivityTimestamp}}</td>
                            <%--<td number>{{cc.SeqNo}}</td>--%>
                            <td>{{cc.UserName}}</td>
                        </tr>
                    </tbody>
                </table>
                <p></p>
                <div class="row">
                    <div class="col m12" flex end>
                        <dir-pagination-controls direction-links="true" boundary-links="true" on-page-change="getCCReportData(newPageNumber,0,0)"></dir-pagination-controls>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
