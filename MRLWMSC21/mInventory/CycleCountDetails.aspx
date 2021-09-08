<%@ Page Title="Cycle Count Details" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="CycleCountDetails.aspx.cs" Inherits="MRLWMSC21.mInventory.CycleCountDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">

    <style>
        .row {
            margin-left: 0;
            margin-right: 0;
        }

        table {
            border-collapse: inherit !important;
        }

        a {
            box-sizing: initial !important;      
        }

        .h3Header {
            font-size: 16px !important;
            border-radius: 0px !important;   
            box-shadow: var(--z1);
            margin-bottom: 0px !important;
            border: 0px;
            font-weight: normal !important;
            background: var(--sideNav-bg);
            color: #fff;
            padding: 8px 5px;         
        }

        .dataTables_filter, .dataTables_length, .dataTables_info {
            display: inline-block;
        }

        .dataTables_info {
            margin-left: 20px;
        }

        .dataTables_filter {
            float: right;
        }

        .material-icons {
            cursor: pointer;
        }

        .dataTables_paginate {
            float: right !important;
        }

        .text-danger {
            color: red !important;
        }



        .table-striped .text-right {
            padding: 0 !important;
            border: 0;
            box-shadow: none;
        }

        .table-striped tr td {
            font-size: 13px !important;
        }

        .form-control {
            color: #000 !important;
            padding: 0px 5px !important;
        }


        .form-horizontal .checkbox {
            padding-top: 0px !important;
        }
    </style>
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
    </style>

    <div class="module_yellow">
        <div class="ModuleHeader" height="35px">
            <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Orders</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">New Cycle Count </span></div>

        </div>

    </div>
    <div class="dashed"></div>
    <div class="container">

        <button type="button" onclick="BacktoList();" style="float: right;" class="btn btn-primary"><i class="material-icons vl">arrow_back</i>Back to List</button>
        <br />
        <br />
        <div>
            <h4 class="h3Header">Cycle Count Header</h4>
            <div class="boxp">
                <input type="hidden" id="hdnCreatedBy" class="hdnCreatedBy" runat="server" value="0" />
                <input type="hidden" id="hdnCreatedOn" value="2018-01-04" />
                <input type="hidden" id="hdnUpdatedBy" class="hdnUpdatedBy" runat="server" value="0" />
                <input type="hidden" id="hdnUpdatedOn" value="2018-01-04" />
                <input type="hidden" id="AM_MST_Account_ID" value="0" class="fieldToGet" />
                <input type="hidden" id="WarehouseID" class="fieldToGet" value="0" />
                <input type="hidden" id="ZoneId" class="fieldToGet" value="0" />
                <input type="hidden" id="TenantId" class="fieldToGet" value="0" />
                <div class="row">
                    <div style="padding: 15px !important;">
                        <div id="divValidationCycleCountMessages" class="text-danger validitems"></div>
                    </div>
                    <div class="col-md-4 col-sm-6">
                        <div class="flex">
                            <%-- <select class=" fieldToGet" id="AM_MST_Account_ID" onchange="GetTenants(this)" required=""></select>--%>
                            <input type="text" id="txtAccount" required="" />
                            <label class="lblFormItem">Select Account</label>
                            <span class="requiredlabel"></span>
                        </div>

                    </div>

                    <div class="col-md-4 col-sm-6" hidden>
                        <div class="flex">
                            <%--<select class=" fieldToGet" id="TM_MST_Tenant_ID" required=""></select>
                            --%>

                            <input type="text" id="TM_MST_Tenant_ID" required="" />
                            <label class="lblFormItem">Tenant</label>
                            <span class="requiredlabel"></span>

                        </div>
                    </div>

                    <div class="col-md-4 col-sm-6">
                        <div class="flex">
                            <%-- <select class=" fieldToGet" id="WarehouseID" required=""></select>--%>
                            <input type="text" id="txtWareHouse" required="" />
                            <label class="lblFormItem">Warehouse</label>
                            <span class="requiredlabel"></span>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-6">
                        <div class="flex">
                            <input type="text" id="txtZone" required="" />
                            <label class="lblFormItem">Zone</label>
                            <span class="requiredlabel"></span>
                        </div>
                    </div>

                </div>
                <br />
                <div class="row">
                    <div class="col-md-4 col-sm-6">
                        <div class="flex">
                            <select class=" fieldToGet" id="CCM_MST_CycleCount_ID" onchange="getcyclecountentites(this);" required=""></select>
                            <label class="lblFormItem">Cycle Count Type</label>
                            <span class="requiredlabel"></span>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-6">
                        <div class="flex">
                            <input type="text" class=" fieldToGet" id="AccountCycleCountName" required="" />
                            <label class="lblFormItem">Cycle Count Name</label>
                            <span class="requiredlabel"></span>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-6">
                        <div class="flex">
                            <select class=" fieldToGet" id="CCM_MST_Freequency_ID" required=""></select>
                            <label class="lblFormItem">Frequency</label>
                            <span class="requiredlabel"></span>
                        </div>
                    </div>




                </div>

                <br />
                <div class="row">
                    <div class="col-md-4 col-sm-6">
                        <div class="flex">
                            <input type="number" class=" fieldToGet" min="0" max="365" id="CycleCountDuration" required="">
                            <label class="lblFormItem">Cycle Count Duration(in Days)</label>
                            <span class="requiredlabel"></span>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-6">
                        <div class="flex">
                            <input type="text" class="" id="ValidFrom" readonly="true" required="required">
                            <label class="lblFormItem">Valid From</label>
                            <span class="requiredlabel"></span>
                        </div>


                        <div class="sr-only">
                            <div class="flex__ checkbox">
                                <input type="checkbox" id="IsDeleted" class="i-checks fieldToGet" />
                                <label for="IsDeleted" class="lblFormItem">&nbsp;&nbsp;Deleted</label>
                            </div>
                        </div>
                        <p></p>
                    </div>
                    <div class="col-md-4 col-sm-6">
                        <%--   <div class="flex__">--%>
                        <div class="flex">
                            <input type="text" class="" id="ValidThru" required="">
                            <label class="lblFormItem">Valid To</label>
                        </div>

                        <%-- </div>
                           <div class="flex__ checkbox">
                                <input type="checkbox" id="IsBlindCycleCount" class="i-checks fieldToGet" checked />
                                <label for="IsBlindCycleCount" class="lblFormItem">&nbsp;&nbsp;Is Blind Cycle Count</label>
                            </div>--%>
                    </div>


                    <div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4 col-sm-6">
                        <div class="flex">
                            <textarea class=" fieldToGet" id="Remarks" required=""></textarea>
                            <label class="lblFormItem">Remarks</label>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-6" hidden>
                        <div class="flex__ checkbox">
                            <input type="checkbox" id="IsBlindCycleCount" class="i-checks fieldToGet" checked />
                            <label for="IsBlindCycleCount" class="lblFormItem">&nbsp;&nbsp;Is Blind Cycle Count</label>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-6">
                        <br />
                        <div class="flex__ checkbox">
                            <input type="checkbox" id="IsActive" class="i-checks fieldToGet" checked />
                            <label for="IsActive" class="lblFormItem">&nbsp;&nbsp;Active</label>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-6">      
                         <gap5></gap5><gap5></gap5><gap5></gap5>
                            <input type="hidden" value="0" id="CCM_CNF_AccountCycleCount_ID" class="fieldToGet" />
                            <button type="button" id="btnCreate" class="btn btn-primary" onclick="UpsertData();">Create<i class="material-icons vl">add</i></button>
                       
                    </div>

                </div>
            </div>
        </div>

        <div class="ibox-content" id="EntityList" style="display: none;">
            <h4 class="h3Header">Entity Configuration</h4>
            <div class="invScrool" style="padding: 10px;">

                <table class="table-striped dataTables-example" id="tblList"></table>
            </div>
        </div>
        <br />
        <br />

        <div class="ibox-content" style="display: none;" id="divcyclecountPref">
            <h4 class="h3Header">Preferences</h4>
            <div class="invScrool" style="padding: 10px;">
                <div id="divPreferences"></div>
            </div>
        </div>

    </div>
    <div class="loading" id="divLoading" style="display: none;"></div>
    <!-- ========================= Modal Popup For Cycle Count Entity Details ========================================== -->
    <div class="modal inmodal" id="AddEntityToCreate" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog" style="width: 50% !important;">
            <div class="modal-content animated fadeIn">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Entity Configuration</h4>
                </div>

                <div class="modal-body">
                    <div id="divValidationCycleCountEntityMessages" class="text-danger"></div>
                    <p></p>
                    <p></p>
                    <br />
                    <div id="divEntityDetails" class="form-horizontal">
                        <form role="form">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <div class="flex">
                                        <select id="CCM_MST_CycleCountEntity_ID" class="EntityfieldToGet" required=""></select>
                                        <label class="lblFormItem">Cycle Count Entity : </label>
                                        <span class="errorMsg"></span>
                                        <%--onchange="GetEntities(this);"--%>
                                    </div>
                                </div>
                                <div class="col-md-6">

                                    <input type="hidden" id="Entity_ID" value="0" class="EntityfieldToGet" />
                                    <div class="flex">
                                        <input type="text" id="txtEntity_ID" required="" />
                                        <label id="lblEntityName">Entity </label>
                                        <span class="errorMsg"></span>
                                    </div>

                                </div>
                            </div>

                            <div class="row">
                                <div class="col m4">
                                    <div class="flex">
                                        <select id="FromRackID" class="EntityfieldToGet">
                                        </select>
                                        <label>From Rack</label>
                                        <span class="errormsg"></span>
                                    </div>
                                </div>                                

                                <div class="col m4">
                                    <div class="flex">
                                        <select id="FromColumnID" class="EntityfieldToGet">
                                        </select>
                                        <label>From Column</label>
                                        <span class="errormsg"></span>
                                    </div>
                                </div>

                                <div class="col m4">
                                    <div class="flex">
                                        <select id="FromLevelID" class="EntityfieldToGet">
                                        </select>
                                        <label>From Level</label>
                                        <span class="errormsg"></span>
                                    </div>
                                </div>

                                <div class="col m4">
                                    <div class="flex">
                                        <select id="ToRackID" class="EntityfieldToGet">
                                        </select>
                                        <label>To Rack</label>
                                        <span class="errormsg"></span>
                                    </div>
                                </div>                               

                                <div class="col m4">
                                    <div class="flex">
                                        <select id="ToColumnID" class="EntityfieldToGet">
                                        </select>
                                        <label>To Column</label>
                                        <span class="errormsg"></span>
                                    </div>
                                </div>

                                 <div class="col m4">
                                    <div class="flex">
                                        <select id="ToLevelID" class="EntityfieldToGet">
                                        </select>
                                        <label>To Level</label>
                                        <span class="errormsg"></span>
                                    </div>
                                </div>

                            </div>

                            <div class="form-group">
                                <div class="col-md-6">
                                    <div class="checkbox">
                                        <input type="checkbox" id="IsActiveEntity" class="i-checks" checked />
                                        <label for="IsActiveEntity" class="lblFormItem">&nbsp;&nbsp;Active</label>
                                    </div>
                                    <div class=" checkbox" style="display: none !important;">
                                        <input type="checkbox" id="IsDeletedEntity" class="i-checks" />
                                        <label for="IsDeletedEntity" class="lblFormItem">&nbsp;&nbsp;Is Deleted</label>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
                <div class="modal-footer">
                    <input type="hidden" value="0" id="CCM_CNF_AccountCycleCountDetail_ID" class="EntityfieldToGet" />
                    <button type="button" class="btn btn-white" data-dismiss="modal" style="color: white !important;">Close</button>
                    <button type="button" class="btn btn-primary" id="btnEntityCreate" onclick="return UpsertEntityData();">Create</button>
                </div>
            </div>
        </div>
    </div>
    <!-- ========================= END Modal Popup For Cycle Count Entity Details ========================================== -->

    <link href="../Scripts/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="CycleCountScripts/jquery-1.11.3.min.js"></script>
    <script src="../Scripts/jquery-ui-1.8.24.min.js"></script>
    <script src="CycleCountScripts/bootstrap.min.js"></script>
    <script src="CycleCountScripts/jquery.dataTables.min.js"></script>
    <script src="CycleCountScripts/dataTables.bootstrap.min.js"></script>
    <script src="CycleCountScripts/bootstrap-datepicker.js"></script>
    <link href="CycleCountScripts/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="CycleCountScripts/bootstrap-multiselect.css" rel="stylesheet" />
    <script src="CycleCountScripts/bootstrap-multiselect.min.js"></script>
    <script src="../Scripts/CommonWMS.js"></script>
    <script src="Scripts/toast/jquery.toastmessage.js"></script>
    <script src="../mInbound/Scripts/InventraxAjax.js"></script>
    <script src="CycleCountScripts/jquery.cookie.min.js"></script>

    <script type="text/javascript">
        var ItemList = [];
        var CycleCountID = 0;
        var Accountid;
        var TotalCycleCountList = [];
        $(document).ready(function () {
            LoadPage();
            //GetCycleCountList();
            //debugger;
            Accountid = <%=this.cp.AccountID %>
            //alert(Accountid);
            if (Accountid == 0) {
                Accountid = 0;
            }
            else {
                Accountid = Accountid;
            }

            //debugger;
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

                    $("#AM_MST_Account_ID").val(i.item.val);
                    $("#txtWareHouse").val("");
                    $("#WarehouseID").val("0")
                },
                minLength: 0
            });

            var TextFieldName = $("#txtZone");
            DropdownFunction(TextFieldName);
            $("#txtZone").autocomplete({
                source: function (request, response) {
                    var eName = "Zone";
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetCycleCountEntities") %>',
                        data: "{ 'prefix': '" + request.term + "','AccountID':'" + Accountid + "','EntityName':'" + eName + "','WarehouseID':'" + $("#WarehouseID").val() + "'}",//<=cp.TenantID%>
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
                    $("#ZoneId").val(i.item.val);
                },
                minLength: 0
            });

            var TextFieldName = $("#TM_MST_Tenant_ID");
            DropdownFunction(TextFieldName);
            $("#TM_MST_Tenant_ID").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantList_CC") %>',
                        data: "{ 'prefix': '" + request.term + "','AccountID':'" + $("#AM_MST_Account_ID").val() + "'}",//<=cp.TenantID%>
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

                    $("#TenantId").val(i.item.val);
                    LoadWareHouse();
                },
                minLength: 0
            });





            var TextFieldName = $("#txtEntity_ID");
            DropdownFunction(TextFieldName);
            $("#txtEntity_ID").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetCycleCountEntities") %>',
                        //data: "{ 'prefix': '" + request.term + "','AccountID':'" + Accountid + "','EntityName' : '" + $('#CCM_MST_CycleCountEntity_ID option:selected').text() + "','WarehouseID' : '" + $("#WarehouseID").val() + "'}",//<=cp.TenantID%>
                        data: "{ 'prefix': '" + request.term + "','AccountID':'" + Accountid + "','EntityName' : 'User','WarehouseID' : '" + $("#WarehouseID").val() + "'}",//<=cp.TenantID%>
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
                    $("#Entity_ID").val(i.item.val);
                },
                minLength: 0
            });

            
            $('#CCM_MST_CycleCountEntity_ID').change(function () {
                ToggleEntityFields($(this).val());
               /* if ($(this).val() == 6) {
                    $("#txtEntity_ID").val($("#TM_MST_Tenant_ID").val());
                    $("#Entity_ID").val($("#TenantId").val());
                    $("#txtEntity_ID").attr("disabled", true);
                }
                else {
                    $("#txtEntity_ID").val('');
                    $("#Entity_ID").val('0');
                    $("#txtEntity_ID").removeAttr("disabled", false);
                }*/
            });
        });

        //function LoadWareHouse() {
        //debugger;
        var TenantId = $("#TenantId").val();
        var TextFieldName = $("#txtWareHouse");
        DropdownFunction(TextFieldName);
        $("#txtWareHouse").autocomplete({
            source: function (request, response) {
                debugger
                $.ajax({
                   <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouseByAccount") %>',
                    data: "{ 'prefix': '" + request.term + "','AccountID':'" + $("#AM_MST_Account_ID").val() + "'}",//<=cp.TenantID%>--%>
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

                $("#WarehouseID").val(i.item.val);

            },
            minLength: 0
        });
        //}
        function LoadPage() {
            $("#divLoading").show();
            CycleCountID = new URL(window.location.href).searchParams.get("parm");
            var data = "{ Id:  '" + CycleCountID + "' }";
            InventraxAjax.AjaxResultExecute("CycleCountDetails.aspx/GetList", data, 'GetListOnSuccess', 'GetListOnError', null);
        }

        function GetCycleCountList() {
            $("#divLoading").show();
            var data = "{ Id:  '0' }";
            InventraxAjax.AjaxResultExecute("CycleCountDetails.aspx/CheckCycleCountNameExits", data, 'GetListOnSuccessList', 'GetListOnError', null);
        }
        function GetListOnSuccessList(data) {
            TotalCycleCountList = JSON.parse(data.Result);
        }

        function GetListOnSuccess(data) {
            debugger;
            var dataList = JSON.parse(data.Result);
            ItemList = dataList;
            LoadDropdowns(dataList);
            setDatePickers();
            CycleCountID = new URL(window.location.href).searchParams.get("parm");


            if (CycleCountID != 0) {
                EditItem(CycleCountID);
                LoadGrid(ItemList, CycleCountID);
                $("#EntityList").css("display", "block");
                $("#tblList").css("width", "100%");

                $('.EntityListCollapse').on('click', function (e) {
                    e.cancelBubble = true;
                });

                //Getting Counts and Disable Fields
                DisableUnAuthorisedFields(CycleCountID);
                $("#divLoading").hide();
            }

            else {
                setDatePickers();
                $('.EntityListCollapse').on('click', function (e) {
                    e.stopPropagation();
                });
            }
            $("#divLoading").hide();

        }


        var dataList = null;
        function LoadGrid(Obj, CycleCountID) {
            //debugger;
            if (Obj != null) { // && Obj.length > 0
                dataList = Obj.Table3;
                var item = dataList;//$.grep(dataList, function (a) { return a.CCM_CNF_AccountCycleCount_ID == CycleCountID });

                $("#tblList").empty();
                $("#tblList").append("<thead><tr><th colspan='9' class='text-right' style='background-color: white;padding-bottom:10px !important;'><div class='text-right'><button type='button' class='btn btn-primary' data-toggle='modal' data-target='#AddEntityToCreate' onclick='GetCycleCountEntity();'>Add <i class='fa fa-plus' aria-hidden='true'></i></button></div></th></tr><tr><th class='text-center'>S.No </th><th class='text-center'>Cycle Count Entity</th><th class='text-center'>User</th><th class='text-center'>Rack</th><th class='text-center'>Column</th><th class='text-center'>Level</th><th class='text-center'>Active</th><th class='text-center'>Action</th></tr></thead><tbody>");
                for (var i = 0; i < item.length; i++) {
                    $("#tblList").append("<tr><td class='text-right'>" + (i + 1) + "</td><td class='text-left'>" + item[i].CycleCountEntityName + "</td><td class='text-left'>" + item[i].FirstName + "</td><td class='text-left'>" + item[i].Rack + "</td><td class='text-left'>" + item[i].Column + "</td><td class='text-left'>" + item[i].Level + "</td><td class='text-left'>" + item[i].IsActive + "</td><td class='text-center'> <a data-toggle='modal' data-target='#AddEntityToCreate' onclick='EditEntityItem(" + item[i].CCM_CNF_AccountCycleCountDetail_ID + ");'><i class='material-icons ss'>mode_edit</i></a>&emsp; <a onclick='DeleteEntity(" + item[i].CCM_CNF_AccountCycleCountDetail_ID + "," + item[i].CCM_CNF_AccountCycleCount_ID + ");'><i class='material-icons ss'>delete</i></a></td></tr>");

                }
                $("#tblList").append("</tbody>");
                SetTableSettings();
            }
            else {
                $("#tblList").empty();
                $("#tblList").append("<thead><tr><th colspan='9' class='text-right' style='background-color: white'><div class='text-right'><button type='button' class='btn btn-primary' data-toggle='modal' data-target='#AddEntityToCreate'  onclick='GetCycleCountEntity();'>Add <i class='fa fa-plus' aria-hidden='true'></i></button></div></th></tr><tr><th class='text-center'>S.No </th><th class='text-center'>Cycle Count Entity Name</th><th class='text-center'>Entity</th><th class='text-center'>Quantity</th><th class='text-center'>Active</th><th class='text-center'>Action</th></tr><tr><th colspan='6' class='text-center' style='background-color: white'><strong>No Data</strong></th></tr></thead><tbody>");
                $("#tblList").append("</tbody>");
            }
        }

        function LoadDropdowns(dataList) {
            $("#CCM_MST_Freequency_ID").empty();
            $("#CCM_MST_Freequency_ID").append($("<option></option>").val(0).html("Please Select"));
            for (var x = 0; x < dataList.Table.length; x++) {
                $("#CCM_MST_Freequency_ID").append($("<option></option>").val(dataList.Table[x].CCM_MST_Freequency_ID).html(dataList.Table[x].FreequencyName));
            }

            $("#CCM_MST_CycleCount_ID").empty();
            $("#CCM_MST_CycleCount_ID").append($("<option></option>").val(0).html("Please Select"));
            for (var x = 0; x < dataList.Table1.length; x++) {
                $("#CCM_MST_CycleCount_ID").append($("<option></option>").val(dataList.Table1[x].CCM_MST_CycleCount_ID).html(dataList.Table1[x].CycleCountName));
            }

            $("#Entity_ID").empty();
            $("#Entity_ID").append($("<option></option>").val(0).html("Please Select"));
            //debugger;
            Accountid = <%=this.cp.AccountID %>;
            //alert(Accountid);
            if (Accountid == 0 || Accountid == null) {
                Accountid = 0;
                $("#txtAccount").attr("disabled", false);
            }
            else {
                debugger;
                Accountid = Accountid;
                var Account = '<%=this.cp.Account%>';
                $("#txtAccount").val(Account);
                $("#AM_MST_Account_ID").val(Accountid);

                $("#txtAccount").attr("disabled", true);
            }
            // alert($("#AM_MST_Account_ID").val(Account));
        }

        function GetCycleCountEntity() {
            if ($("#CCM_MST_CycleCount_ID").val() != 0) {
                var CycleCountEntityList = $.grep(ItemList.Table4, function (a) { return a.CCM_MST_CycleCount_ID == $("#CCM_MST_CycleCount_ID").val() });
                $("#CCM_MST_CycleCountEntity_ID").empty();
                $("#CCM_MST_CycleCountEntity_ID").append($("<option></option>").val(0).html("Please Select"));
                for (var x = 0; x < CycleCountEntityList.length; x++) {
                    $("#CCM_MST_CycleCountEntity_ID").append($("<option></option>").val(CycleCountEntityList[x].CCM_MST_CycleCountEntity_ID).html(CycleCountEntityList[x].CycleCountEntityName));
                }
            }
            else {
                $("#CCM_MST_CycleCountEntity_ID").empty();
                $("#CCM_MST_CycleCountEntity_ID").append($("<option></option>").val(0).html("Please Select"));
            }
            ClearEntity();
        }

        function getcyclecountentites(id) {
        }
        function EditItem(CycleCountID) {
            //debugger;
            var data = "{ Id:  " + CycleCountID + " }";
            InventraxAjax.AjaxResultExecute("CycleCountDetails.aspx/GetEditData", data, 'GetEditDatatOnSuccess', 'GetListOnError', null);
        }

        function GetEditDatatOnSuccess(data) {
            //debugger;
            var item = JSON.parse(data.Result);
            BuildCycleCountFormtoEdit(item.Table);
            generateLocData(item);
            $("#btnCreate").html('Update');
        }

        function BuildCycleCountFormtoEdit(item) {

            var retitem = 0;
            $('.fieldToGet').each(function () {

                var fieldID = $(this).attr('id');
                var paramtype = $(this).attr('type');
                if (paramtype == "checkbox") {
                    if (item[0][fieldID] == true)
                        $('#' + fieldID).attr('checked', 'checked');
                    else
                        $('#' + fieldID).removeAttr('checked');
                }
                else {
                    //debugger;                  
                    if (fieldID == "AM_MST_Account_ID") {
                        $("#txtAccount").val(item[0].Account);
                        $("#TenantId").val(item[0].TenantId);
                        $("#TM_MST_Tenant_ID").val(item[0].TenantName);
                        //$("#TM_MST_Tenant_ID").val(item[0].TenantCode);
                        //$("#AM_MST_Account_ID").val(item[0].AccountID);
                        $("#AM_MST_Account_ID").val(item[0].AM_MST_Account_ID);
                    }
                    else if (fieldID == "WarehouseID") {
                        $("#txtWareHouse").val(item[0].WHCode);
                        $("#WarehouseID").val(item[0].WarehouseID);
                    }
                    else if (fieldID == "ZoneId") {
                        $("#txtZone").val(item[0].ZoneCode);
                        $("#ZoneId").val(item[0].ZoneId);
                    }
                    else {
                        $('#' + fieldID).val(item[0][fieldID]);
                    }
                }

            });

            var ValidFrom = item[0].ValidFrom;
            $('#ValidFrom').val(ValidFrom);
            var ValidThru = item[0].ValidThru;
            $('#ValidThru').val(ValidThru);

            $("#ValidFrom").datepicker({
                todayBtn: 1,
                singleDatePicker: true,
                showDropdowns: true,
                autoclose: true,
                forceParse: false,
                format: "dd-M-yyyy",
                startDate: "today",
                onClick: function (ValidThru) {
                    $("#ValidThru").datepicker("option", "minDate", ValidThru, { dateFormat: "dd/mm/yy" })
                }
            }).on('changeDate', function (selected) {
                var minDate = new Date(selected.date.valueOf());
                $('#ValidThru').datepicker('setStartDate', minDate);
            });



            $("#ValidThru").datepicker({
                todayBtn: 1,
                singleDatePicker: true,
                showDropdowns: true,
                autoclose: true,
                forceParse: false,
                format: "dd-M-yyyy",
                startDate: ValidFrom,
                onClick: function (ValidFrom) {
                    $("#ValidFrom").datepicker("option", "maxDate", ValidFrom, { dateFormat: "dd/mm/yy" })
                }
            });
        }

        function generateLocData(data) {

            $("#FromRackID").empty();
            $("#FromRackID").append($("<option></option>").val(0).html("Please Select"));
            for (var x = 0; x < data.Table1.length; x++) {
                $("#FromRackID").append($("<option></option>").val(data.Table1[x].LocationRackID).html(data.Table1[x].LocationRack));
            }

            $("#ToRackID").empty();
            $("#ToRackID").append($("<option></option>").val(0).html("Please Select"));
            for (var x = 0; x < data.Table1.length; x++) {
                $("#ToRackID").append($("<option></option>").val(data.Table1[x].LocationRackID).html(data.Table1[x].LocationRack));
            }

            $("#FromColumnID").empty();
            $("#FromColumnID").append($("<option></option>").val(0).html("Please Select"));
            for (var x = 0; x < data.Table2.length; x++) {
                $("#FromColumnID").append($("<option></option>").val(data.Table2[x].LocationColumnID).html(data.Table2[x].LocationColumn));
            }

            $("#ToColumnID").empty();
            $("#ToColumnID").append($("<option></option>").val(0).html("Please Select"));
            for (var x = 0; x < data.Table2.length; x++) {
                $("#ToColumnID").append($("<option></option>").val(data.Table2[x].LocationColumnID).html(data.Table2[x].LocationColumn));
            }   

            $("#FromLevelID").empty();
            $("#FromLevelID").append($("<option></option>").val(0).html("Please Select"));
            for (var x = 0; x < data.Table3.length; x++) {
                $("#FromLevelID").append($("<option></option>").val(data.Table3[x].LocationLevelID).html(data.Table3[x].LocationLevel));
            }

            $("#ToLevelID").empty();
            $("#ToLevelID").append($("<option></option>").val(0).html("Please Select"));
            for (var x = 0; x < data.Table3.length; x++) {
                $("#ToLevelID").append($("<option></option>").val(data.Table3[x].LocationLevelID).html(data.Table3[x].LocationLevel));
            }
        }

        function GetCycleCountFormData() {
            //debugger;
            var fieldDataOut = '{';
            var fieldData = '<root><data>';
            $('.fieldToGet').each(function () {
                var param = $(this).attr('id');
                var Actualval = $(this).val() == undefined || $(this).val() == null ? " " : $(this).val();
                var val = Actualval.trim();
                var paramtype = $(this).attr('type');
                if (paramtype == "checkbox") {
                    val = $(this).prop('checked');
                    if (val == true) {
                        val = 1;
                    }
                    else {
                        val = 0;
                    }
                }
                fieldData += '<' + param + '>' + val + '</' + param + '>';
            });

            var validto = null;
            validto = $("#ValidThru").val().trim();
            if (validto == null || validto == "" || validto == undefined) {
                validto = "";
            }
            else {
                validto = formatDateTime($("#ValidThru").val());
            }

            fieldData += '<CreatedBy>' + $('.hdnCreatedBy').val() + '</CreatedBy>' + '<UpdatedBy>' + $('.hdnUpdatedBy').val() + '</UpdatedBy>' + '<CreatedOn>' + $('#hdnUpdatedOn').val() + '</CreatedOn>' + '<UpdatedOn>' + $('#hdnUpdatedOn').val() + '</UpdatedOn>' + '<NewCCM_CNF_AccountCycleCount_ID>' + 0 + '</NewCCM_CNF_AccountCycleCount_ID>';
            fieldData += '<ValidFrom>' + formatDateTime($("#ValidFrom").val()) + '</ValidFrom>' + '<ValidThru>' + validto + '</ValidThru>';
            fieldData = fieldData + '</data></root>';
            fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'UpdatedBy' + '":"' + <%=this.cp.UserID%> + '",' + '"' + String.fromCharCode(64) + 'CCM_CNF_AccountCycleCount_ID' + '":"' + $('#CCM_CNF_AccountCycleCount_ID').val() + '",';
            fieldDataOut = fieldDataOut.substring(0, fieldDataOut.length - 1);
            fieldDataOut += '}';
            return fieldDataOut;
        }



        function UpsertData() {

            var TenantID = $("#TenantId").val();

            //if (TenantID == "0" || TenantID == "") {
            //    showStickyToast(false, 'Please Select Tenant', false);
            //    return;
            //}

            if (ValidateCycleCountData()) {
                $("#divLoading").show();
                var param = JSON.stringify({ "Sp_Set": "", "JSON": JSON.stringify(GetCycleCountFormData()), "CCID": CycleCountID, "TenantId": TenantID });
                $.ajax({
                    url: "CycleCountDetails.aspx/MasterDetailsSet",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    cache: false,
                    data: param,
                    success: function (response) {                       
                        debugger;
                        var result = JSON.parse(response.d).Table[0].CycleState;
                        if ($('#btnCreate').text() == "Update") {
                            if (result == 1) {
                                showStickyToast(true, 'User can update only Frequency And Valid To, As CC Is Already Initiated.', false);
                            }
                            else if (result == 2) { showStickyToast(true, 'User can update only Valid To,  As CC Is Already Completed.', false); }
                            else if (result == 3) { showStickyToast(false, 'Cannot update this Cycle Count, as there is no Sequence.', false); }
                            else { showStickyToast(true, 'Cycle Count Details Updated Successfully', false); }

                            LoadPage();
                        }
                        else {
                            showStickyToast(true, 'Cycle Count Details Created Successfully', false);
                            setTimeout(function () {
                                BacktoList();
                            }, 2000);
                        }

                    },
                    failure: function (errMsg) {

                    }
                });
            }
            else {
                $("#divValidationCycleCountMessages").show();
            }
        }


        function ValidateCycleCountData() {
            debugger;
            $("#divValidationCycleCountMessages").empty();
            var IsValid = true;

            if ($("#txtAccount").val() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Select Account.', false);
                return false;
            }

            if ($("#txtWareHouse").val() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Select Warehouse.', false);
                return false;
            }

            if ($("#txtZone").val() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Select Zone.', false);
                return false;
            }

            if ($("#CCM_MST_CycleCount_ID").val() == "0") {
                IsValid = false;
                showStickyToast(false, 'Please Select Cycle Count.', false);
                return false;
            }

            if ($("#AccountCycleCountName").val().trim() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Enter Cycle Count Name.', false);
                return false;
            }


            var Dur = $("#CycleCountDuration").val().trim();
            if (Dur == "" || Dur == "0") {
                IsValid = false;
                showStickyToast(false, 'Please Enter Cycle Count Duration.', false);
                return false;
            }
            else if (Number.isInteger(eval(Dur)) == false || eval(Dur) < 0) {
                IsValid = false;
                showStickyToast(false, 'Please Enter Valid Cycle Count Duration.', false);
                return false;
            }

            if ($("#ValidFrom").val().trim() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Enter Valid From.', false);
                return false;
            }

            var from = $("#ValidFrom").val();
            var to = $("#ValidThru").val();

            if (Date.parse(from) > Date.parse(to)) {
                IsValid = false;
            }

            if ($("#CCM_MST_Freequency_ID").val() == "0") {
                IsValid = false;
                showStickyToast(false, 'Please Select Frequency.', false);
                return false;
            }

            if ($("#AccountCycleCountName").val().trim() != "") {
                var item = ItemList.Table5, Count = 0;

                if ($('#CCM_CNF_AccountCycleCount_ID').val() != 0) {

                    item = $.grep(ItemList.Table5, function (data) {
                        return data['CCM_CNF_AccountCycleCount_ID'] != $('#CCM_CNF_AccountCycleCount_ID').val();
                    });
                }

                Count = $.grep(item, function (data) {

                    return data['AccountCycleCountName'] == $('#AccountCycleCountName').val().trim();
                });

                if (Count.length != 0) {
                    IsValid = false;
                    showStickyToast(false, 'Cycle Count Name already exists.', false);
                    return false;
                }
            }


            var DayDiff = ((new Date($('#ValidThru').val())) - (new Date($('#ValidFrom').val()))) / 86400000; //It gives in Days

            if (Dur > (DayDiff + 1)) {
                var DateTo = new Date($('#ValidFrom').val());
                DateTo.setDate(DateTo.getDate() + eval(Dur));
                var MM = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
                DateTo = DateTo.getDate() + '-' + MM[DateTo.getMonth()] + '-' + DateTo.getFullYear();
                showStickyToast(false, "The Valid Through Date must be  greater than " + DateTo + " as the duration selected is " + Dur + " days.", false);
                return false;
                IsValid = false;
            }

            var dur = $.grep(ItemList.Table, function (a) { return a.CCM_MST_Freequency_ID == $("#CCM_MST_Freequency_ID").val() });
            if (dur[0].CycleTimeInDays < $("#CycleCountDuration").val()) {
                IsValid = false;
                showStickyToast(false, 'Cycle Count Duration should be less than or equal to Frequency.', false);
                return false;
            }


            return IsValid;
        }

        function formatDate(date) {
            var d = new Date(date),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;

            return [year, month, day].join('-');
        }

        function CycleCountDataAppendMessage(Message) {
            $("#divValidationCycleCountMessages").append("<li style='margin-top: 0.3em;'>" + Message + "</li>");
        }

        function BacktoList() {
            $("#divLoading").show();
            window.location.href = "CycleCountList.aspx";
        }

        function ToggleEntityFields(selectedVal) {
            if (selectedVal == 0)
                $('#Entity_ID, #lblEntityName').hide();
            else
                $('#Entity_ID, #lblEntityName').show();

           // $('#lblEntityName').html($('#CCM_MST_CycleCountEntity_ID option:selected').text());
             $('#lblEntityName').html("User");
        }



        function ClearEntity() {
            $("#divValidationCycleCountEntityMessages").empty();
            $("#txtEntity_ID").val('');
            $("#Entity_ID").val("0");
            $('#lblEntityName').text('Entity');
            $("#CountQuantity").val("");
            $("#btnEntityCreate").html('Create');
            $('#CCM_CNF_AccountCycleCountDetail_ID').val("0");
            $("#txtEntity_ID").removeAttr("disabled", false);

            $("#FromRackID").val("0"); $("#ToRackID").val("0");
            $("#FromColumnID").val("0"); $("#ToColumnID").val("0");
            $("#FromLevelID").val("0");$("#ToLevelID").val("0");
        }

        function LoadDropdownWithSearchFilter(id) {
        }

        function EditEntityItem(id) {
            //debugger;
            var item = $.grep(ItemList.Table3, function (a) { return a.CCM_CNF_AccountCycleCountDetail_ID == id });
            BuildCycleCountEntityFormtoEdit(item);
            $("#btnEntityCreate").html('Update');
            $("#divValidationCycleCountEntityMessages").empty();
            ToggleEntityFields(id);
            DisableFields();
        }

        function BuildCycleCountEntityFormtoEdit(item) {
            //debugger;
            if ($("#CCM_MST_CycleCount_ID").val() != 0) {
                var CycleCountEntityList = $.grep(ItemList.Table4, function (a) { return a.CCM_MST_CycleCount_ID == $("#CCM_MST_CycleCount_ID").val() });
                $("#CCM_MST_CycleCountEntity_ID").empty();
                $("#CCM_MST_CycleCountEntity_ID").append($("<option></option>").val(0).html("Please Select"));
                for (var x = 0; x < CycleCountEntityList.length; x++) {
                    $("#CCM_MST_CycleCountEntity_ID").append($("<option></option>").val(CycleCountEntityList[x].CCM_MST_CycleCountEntity_ID).html(CycleCountEntityList[x].CycleCountEntityName));
                }
            }
            else {
                $("#CCM_MST_CycleCountEntity_ID").empty();
                $("#CCM_MST_CycleCountEntity_ID").append($("<option></option>").val(0).html("Please Select"));
            }
            $('.EntityfieldToGet').each(function () {
                var fieldID = $(this).attr('id');
                var paramtype = $(this).attr('type');
                $('#' + fieldID).val(item[0][fieldID]);
            });

            var id = $("#CCM_MST_CycleCountEntity_ID").val();
            if (id != 0) {

                //$("#Entity_ID").val(item[0].Entity_ID);
                //$("#txtEntity_ID").val(item[0].EntityName);
                $("#Entity_ID").val(item[0].UserID);
                $("#txtEntity_ID").val(item[0].FirstName);
                if (id == 6) {
                    $("#txtEntity_ID").attr("disabled", true);
                }
                else { $("#txtEntity_ID").attr("disabled", false); }
            }
            else {
                $("#Entity_ID").empty();
                $("#Entity_ID").append($("<option></option>").val(0).html("Please Select"));
            }
        }


        function GetCycleCountEntityFormData() {
            debugger
            var fieldDataOut = '{';
            var fieldData = '<root><data>';
            $('.EntityfieldToGet').each(function () {
                var param = $(this).attr('id');
                var val = $(this).val().trim();
                fieldData += '<' + param + '>' + val + '</' + param + '>';
            });
            var isactive = 0;
            var isdeleted = 0;
            var quantity = null;
            if ($('#IsActiveEntity').prop('checked') == true) {
                isactive = 1;
            }
            else {
                isactive = 0;
            }

            if ($('#IsDeletedEntity').prop('checked') == true) {
                isdeleted = 1;
            }
            else {
                isdeleted = 0;
            }

            fieldData += '<CreatedBy>' + $('.hdnCreatedBy').val() + '</CreatedBy>' + '<UpdatedBy>' + $('.hdnUpdatedBy').val() + '</UpdatedBy>' + '<CreatedOn>' + $('#hdnUpdatedOn').val() + '</CreatedOn>' + '<UpdatedOn>' + $('#hdnUpdatedOn').val() + '</UpdatedOn>' + '<NewCCM_CNF_AccountCycleCountDetail_ID>' + 0 + '</NewCCM_CNF_AccountCycleCountDetail_ID>';
            fieldData += '<CCM_CNF_AccountCycleCount_ID>' + $("#CCM_CNF_AccountCycleCount_ID").val() + '</CCM_CNF_AccountCycleCount_ID>' + '<CCM_MST_CycleCount_ID>' + $('#CCM_MST_CycleCount_ID').val() + '</CCM_MST_CycleCount_ID>';
            fieldData += '<IsActive>' + isactive + '</IsActive>' + '<IsDeleted>' + isdeleted + '</IsDeleted>' + '<Entity_ID>' + $("#Entity_ID").val() + '</Entity_ID>';
            fieldData = fieldData + '</data></root>';
            fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'UpdatedBy' + '":"' + <%=this.cp.UserID%> + '",' + '"' + String.fromCharCode(64) + 'CCM_CNF_AccountCycleCountDetail_ID' + '":"' + $('#CCM_CNF_AccountCycleCountDetail_ID').val() + '",';
            fieldDataOut = fieldDataOut.substring(0, fieldDataOut.length - 1);
            fieldDataOut += '}';
            return fieldDataOut;
        }


        function UpsertEntityData() {
            debugger
            if (ValidateCycleCountEntityData()) {
                $("#divLoading").show();
                var param = JSON.stringify({ "Sp_Set": "", "JSON": JSON.stringify(GetCycleCountEntityFormData()), "CCID": 0 });
                $.ajax({
                    url: "CycleCountDetails.aspx/MasterDetailsSetEntity",// "@Url.Action("MasterDetailsSet", "Master")",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    cache: false,
                    data: param,
                    success: function (response) {
                        var result = response;
                        if ($('#btnEntityCreate').text() == "Update") {
                            showStickyToast(true, 'Cycle Count Entity Details Updated Successfully', false);
                            //$('#CCM_CNF_AccountCycleCountDetail_ID').val("0");
                            var table = $('#tblList').DataTable();
                            table.destroy();
                            $('#tblList').empty();
                            LoadPage();
                            $('#AddEntityToCreate').modal('hide');

                        }
                        else {
                            showStickyToast(true, 'Cycle Count Entity Details Created Successfully', false);
                            var table = $('#tblList').DataTable();
                            table.destroy();
                            $('#tblList').empty();
                            LoadPage();
                            $('#AddEntityToCreate').modal('hide');
                        }

                    },
                    failure: function (errMsg) {

                    }
                });
            }
            else {
                $("#divValidationCycleCountEntityMessages").show();
            }
        }

        function ValidateCycleCountEntityData() {
            debugger;
            $("#divValidationCycleCountEntityMessages").empty();
            var IsValid = true;

            if ($("#CCM_MST_CycleCountEntity_ID").val() == "0") {
                IsValid = false;
                showStickyToast(false, 'Please Select Cycle Count Entity.', false);
                return false;
            }

            if ($("#txtEntity_ID").val() == "0" || $("#Entity_ID").val() == "0" || $("#txtEntity_ID").val() == "" || $("#txtEntity_ID").val() == null) {
                IsValid = false;
                showStickyToast(false, 'Please Select User.', false);
                return false;
            }

            if ($("#FromRackID").val() == "0") {
                IsValid = false;
                showStickyToast(false, 'Please Select From Rack.', false);
                return false;
            }

            if ($("#ToRackID").val() == "0") {
                IsValid = false;
                showStickyToast(false, 'Please Select To Rack.', false);
                return false;
            }

            if ($("#FromColumnID").val() == "0") {
                IsValid = false;
                showStickyToast(false, 'Please Select From Column.', false);
                return false;
            }

            if ($("#ToColumnID").val() == "0") {
                IsValid = false;
                showStickyToast(false, 'Please Select To Column.', false);
                return false;
            }

            if ($("#FromLevelID").val() == "0") {
                IsValid = false;
                showStickyToast(false, 'Please Select From Level.', false);
                return false;
            }

            if ($("#ToLevelID").val() == "0") {
                IsValid = false;
                showStickyToast(false, 'Please Select To Level.', false);
                return false;
            }


            if ($("#CCM_MST_CycleCountEntity_ID").val() != "0") {
                var item = ItemList.Table6, Count = 0;

                if ($('#CCM_CNF_AccountCycleCountDetail_ID').val() != 0) {

                    item = $.grep(ItemList.Table6, function (data) {
                        return data['CCM_CNF_AccountCycleCountDetail_ID'] != $('#CCM_CNF_AccountCycleCountDetail_ID').val();
                    });
                }

                Count = $.grep(item, function (data) {

                    //return data['CCM_MST_CycleCountEntity_ID'] == $('#CCM_MST_CycleCountEntity_ID').val() && data['Entity_ID'] == $('#Entity_ID').val() && data['CCM_CNF_AccountCycleCount_ID'] == $('#CCM_CNF_AccountCycleCount_ID').val();
                    return data['CCM_MST_CycleCountEntity_ID'] == $('#CCM_MST_CycleCountEntity_ID').val() && data['UserID'] == $('#Entity_ID').val() && data['CCM_CNF_AccountCycleCount_ID'] == $('#CCM_CNF_AccountCycleCount_ID').val();
                });

                if (Count.length != 0) {
                    IsValid = false;
                    showStickyToast(false, 'Record already exists.', false);
                    return false;
                }
            }

            //if ($("#CCM_MST_CycleCountEntity_ID").val() != "0") {
            //    var item = ItemList.Table6, Count = 0;

            //    if ($('#CCM_CNF_AccountCycleCountDetail_ID').val() != 0) {

            //        item = $.grep(ItemList.Table6, function (data) {
            //            return data['CCM_CNF_AccountCycleCountDetail_ID'] != $('#CCM_CNF_AccountCycleCountDetail_ID').val();
            //        });
            //    }

            //    Count = $.grep(item, function (data) {

            //        return data['CCM_MST_CycleCountEntity_ID'] == $('#CCM_MST_CycleCountEntity_ID').val() && data['Entity_ID'] == $('#Entity_ID').val() && data['CCM_CNF_AccountCycleCount_ID'] == $('#CCM_CNF_AccountCycleCount_ID').val();
            //    });

            //    if (Count.length != 0) {
            //        IsValid = false;
            //        showStickyToast(false, 'Record already exists.', false);
            //        return false;
            //    }
            //}

            return IsValid;
        }

        function CycleCountEntityDataAppendMessage(Message) {
            $("#divValidationCycleCountEntityMessages").append("<li style='margin-top: 0.3em;'>" + Message + "</li>");
        }




        function DisableUnAuthorisedFields(CycleCountID) {
            //debugger;
            var data = "{ CCID:  " + CycleCountID + " }";
            InventraxAjax.AjaxResultExecute("CycleCountDetails.aspx/GetCounts", data, 'GetCountsOnSuccess', 'GetCountsOnError', null);
        }


        var FreqCount = 0;
        var CompCount = 0;
        var TransCount = 0;
        var ValidTo = '';
        var TotalComStatus = 0;
        var CurrentDate = new Date();
        function GetCountsOnSuccess(data) {
            //debugger;
            Counts = data.Result;
            FreqCount = eval(Counts.split(',')[0]);
            CompCount = eval(Counts.split(',')[1]);
            TransCount = eval(Counts.split(',')[2]);
            ValidTo = Counts.split(',')[3];
            TotalComStatus = eval(Counts.split(',')[4]);
            DisableFields();

        }

        function DisableFields() {
            //debugger;
            if (FreqCount == CompCount) {
                $('.fieldToGet, #txtAccount, #txtWareHouse, .EntityfieldToGet, #Entity_ID, #txtEntity_ID, #IsActiveEntity, #ValidFrom').attr('disabled', 'disabled').css('background-color', '#eee');
            }
            else if (CompCount > 0) {
                $('.fieldToGet, #txtAccount, #txtWareHouse, .EntityfieldToGet,#Entity_ID,#txtEntity_ID, #IsActiveEntity, #ValidFrom').attr('disabled', 'disabled').css('background-color', '#eee');
                $('#ValidThru').removeAttr('disabled');
            }
            else if (TransCount > 0) {
                $('.fieldToGet, #txtAccount, #txtWareHouse, .EntityfieldToGet, #Entity_ID,#txtEntity_ID, #IsActiveEntity, #ValidFrom').attr('disabled', 'disabled').css('background-color', '#eee');
                $('#ValidThru, #CCM_MST_Freequency_ID').removeAttr('disabled', 'disabled').css('background-color', '#fff');
            }
            if (CurrentDate > new Date(ValidTo)) {
                $('#ValidThru').attr('disabled', 'disabled').css('background-color', '#eee');
            }
            if (TotalComStatus > 0) {
                $('.fieldToGet, #txtAccount, #txtWareHouse, .EntityfieldToGet,#Entity_ID,#txtEntity_ID, #IsActiveEntity, #ValidFrom, #ValidThru').attr('disabled', 'disabled').css('background-color', '#eee');
            }
        }

        function DeleteParams(id) {
            var date = new Date();
            var UTCtime = JSON.stringify(date);
            var fieldData = '{';
            fieldData += '"' + String.fromCharCode(64) + 'PK":' + id + ',';
            // fieldData += '"' + String.fromCharCode(64) + 'LoggedInUserID":' + $("#hdnUpdatedBy").val().trim() + ',';
            fieldData += '"' + String.fromCharCode(64) + 'UTCTimestamp":' + UTCtime + ',';
            fieldData = fieldData.substring(0, fieldData.length - 1);
            fieldData += '}';
            return fieldData;
        }

        var msg = "Deleted Successfully.";
        function DeleteEntity(id, cid) {
            if (window.confirm("Do you want to Delete?")) {
                procname = "[dbo].[USP_Delete_CCM_CNF_AccountCycleCountDetails]";
                var msgtitle = " Cycle Count Entity Details";
                //DeleteItem(id, msg, msgtitle);
                checkCount(id, cid);
            }

        }

        function checkCount(id, cid) {
            debugger;
            $("#divLoading").show();
            $.ajax({
                url: '<%=ResolveUrl("~/mInventory/CycleCountDetails.aspx/GetCountsNew") %>',
                data: "{ 'CCID': '" + cid + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: function (response) {
                    debugger;
                    var dt = response.d;
                    var InCount = eval(dt.split(',')[0]);
                    var CompCount = eval(dt.split(',')[1]);
                    if (InCount > 0 || CompCount > 0) {
                        showStickyToast(false, "Cannot delete this Cycle Count, as it is already Initiated", false);
                        $("#divLoading").hide();
                        return false;
                    }
                    else {
                        msg = "Deleted Successfully.";
                        DeleteItem(id, msg);
                        //setTimeout(function () {
                        //    location.reload();
                        //        }, 2000);
                    }
                },
                error: function (response) {

                },
                failure: function (response) {

                }
            });
        }


        function DeleteItem(id, msg) {
            debugger;
            $("#divLoading").show();
            var param = JSON.stringify({ "SP_Del": procname, "ID": id });
            $.ajax({
                url: "CycleCountDetails.aspx/DeleteItemsById",// "@Url.Action("DeleteItemsById", "Master")",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                data: param,
                success: function (response) {
                    //getparameters('success', msg, msgtitle);
                    showStickyToast(true, msg, false);
                    setTimeout(function () {
                        location.reload();
                    }, 2500);
                },
                failure: function (errMsg) {

                }
            });
        }


        function SetTableSettings() {
            $('.dataTables-example').DataTable({
                pageLength: 25,
                //searching: false,
                //paging: false,
                retrieve: true,
                //columnDefs: [
                //{ orderable: false, targets: -1 }
                //],
                dom: '<"html5buttons"B>lTfgitp',
                language: {
                    paginate: {
                        next: '>>', // or '→'
                        previous: '<<' // or '←' 
                    },
                    "sSearch": "<%= GetGlobalResourceObject("Resource", "Search")%> :  ",
                },
                buttons: [
                    { extend: 'copy' },
                    { extend: 'csv' },
                    { extend: 'excel', title: 'ExampleFile' },
                    { extend: 'pdf', title: 'ExampleFile' },

                    {
                        extend: 'print',
                        customize: function (win) {
                            $(win.document.body).addClass('white-bg');
                            $(win.document.body).css('font-size', '10px');

                            $(win.document.body).find('table')
                                .addClass('compact')
                                .css('font-size', 'inherit');
                        }
                    }
                ]

            });
        }

        function setDatePickers() {
            $("#ValidFrom").datepicker({
                todayBtn: 1,
                singleDatePicker: true,
                showDropdowns: true,
                autoclose: true,
                forceParse: false,
                format: "dd-M-yyyy",
                startDate: "today",
            }).on('changeDate', function (selected) {
                var minDate = new Date(selected.date.valueOf());
                $('#ValidThru').datepicker('setStartDate', minDate);
            });

            $("#ValidThru").datepicker({
                singleDatePicker: true,
                showDropdowns: true,
                autoclose: true,
                forceParse: false,
                format: "dd-M-yyyy",
            }).on('changeDate', function (selected) {
                var maxDate = new Date(selected.date.valueOf());
                $('#ValidFrom').datepicker('setEndDate', maxDate);
            });
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#hdnCreatedBy").val(<%=cp.UserID%>);
             $("#hdnUpdatedBy").val(<%=cp.UserID%>);
        });
    </script>

    <script>
        //=================================== Preferences =========================================//
        var ACCList = null;
        AccountCCId = new URL(window.location.href).searchParams.get("parm");
        //alert(AccountCCId);
        //debugger;
        if (AccountCCId == null || AccountCCId == "0") {
            //debugger;
            $("#divcyclecountPref").css("display", "none");
        }
        else {
            $("#divcyclecountPref").css("display", "none");
        }
        
        function isNumberKeyEvent(e) {
            //debugger;
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        }

        function RestrictSpace() {
            if (event.keyCode == 32) {
                return false;
            }

        }
    </script>


</asp:Content>
