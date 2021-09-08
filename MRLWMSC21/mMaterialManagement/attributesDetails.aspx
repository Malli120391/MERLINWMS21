<%@ Page Title=" Attribute Details " Language="C#" AutoEventWireup="true" Debug="true" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" CodeBehind="attributesDetails.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.attributesDetails" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MMContent" runat="server">
    <asp:ScriptManager ID="mySManager" EnablePageMethods="true" EnablePartialRendering="true" runat="server" SupportsPartialRendering="true">
    </asp:ScriptManager>
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
                font-weight: 400 !important;
                background: var(--sideNav-bg);
                color: #fff;
                /* opacity: 0.8; */
                padding: 12px;
                border-top-left-radius: 5px !important;
                border-top-right-radius: 5px !important;
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

        .lblFormItem:after, .labelfontbold:after {
            content: "" !important;
        }

        .flex input[type="text"], input[type="number"], textarea {
            width: 72%;
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

        select[disabled="disabled"] {
            width: 90% !important;
        }

        .form-horizontal .checkbox {
            padding-top: 0px !important;
        }

       
        .row {
            margin-left: 0px !important;
            margin-right: 0px !important;
        }

        .gapCheckbox .checkbox {
            margin-top: 0 !important;
        }

        .data-tooltip {
            position: absolute;
            left: -10px;
            background: #464444;
            color: #fff;
            padding: 3px 10px;
            font-size: 11px;
            transform: scale3d(0, 1, 0);
            transition: transform 200ms ease-in;
            /* color: black; */
            border-radius: 3px;
            bottom: -10px;
            left: 0px;
            backface-visibility: hidden;
            z-index: 9999999;
            font-family: var(--default-font);
            font-style: normal !important;
            box-shadow: var(--z2);
        }

        #MM_MST_DependsOnAttribute_ID:hover ~ .data-tooltip {
            transform: scale3d(1, 1, 1);
        }

        #btnCreate:hover ~ .data-tooltip {
            transform: scale3d(1, 1, 1);
        }

        .chackbox-disble {
            width: 150px;
            margin: 14px 0px !important;
        }
    </style>



    <div>

        <div class="pagewidth">
            <gap5></gap5>
            <flex end><button type="button" onclick="BacktoList();" style="float:right;" class="btn btn-primary"><i class="material-icons vl">arrow_back</i>Back to List</button> </flex>
            <div>
                <h4 class="h3Header">Attributes Header</h4>
                <div class="boxp">
                    <input type="hidden" id="hdnCreatedBy" class="hdnCreatedBy" runat="server" value="0" />
                    <input type="hidden" id="hdnCreatedOn" value="2018-01-04" />
                    <input type="hidden" id="hdnUpdatedBy" class="hdnUpdatedBy" runat="server" value="0" />
                    <input type="hidden" id="hdnUpdatedOn" value="2018-01-04" />
                    <input type="hidden" id="AM_MST_Account_ID" class="fieldToGet" />
                    <input type="hidden" id="TM_MST_Tenant_ID" class="fieldToGet" />
                    <input type="hidden" value="0" id="GEN_MST_Industry_ID" />


                    <div class="row">
                        <div style="padding: 15px !important;">
                            <div id="divValidationCycleCountMessages" class="text-danger validitems"></div>
                        </div>
                        <div class="col-md-3">
                            <div class="flex">
                                <input type="text" id="txtAccount" required="" />
                                <label class="lblFormItem">Select Account</label>
                                <span class="requiredlabel"></span>
                            </div>

                        </div>

                        <div class="col-md-3">
                            <div class="flex">
                                <input type="text" id="txtTenant" required="" />
                                <label class="lblFormItem">Select Tenant</label>
                                <span class="requiredlabel"></span>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="flex">
                                <input type="text" id="AttributeName" required="" class="fieldToGetAttr" />
                                <label class="lblFormItem">Attribute Name</label>
                                <span class="requiredlabel"></span>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="flex">
                                <input type="text" id="AttributeCode" class="fieldToGetAttr" required="" />
                                <label class="lblFormItem">Attribute Code</label>
                                <span class="requiredlabel"></span>
                            </div>
                        </div>
                    </div>
                    <br />

                    <div class="row">

                        <div class="col-md-3">
                            <div class="gapCheckbox">
                                <div class="flex__ checkbox">
                                    <input type="checkbox" id="IsLookupDefined" class="i-checks fieldToGetAttr" onclick="hasLookup();" />
                                    <label for="IsLookupDefined" class="lblFormItem">&nbsp;&nbsp;Has Lookup</label>
                                </div>
                                <p></p>
                                <div class="flex__ checkbox">
                                    <input type="checkbox" id="HasDependency" class="i-checks fieldToGetAttr" disabled="disabled" onclick="DisableDependents();" />
                                    <label for="HasDependency" class="lblFormItem" id="unsetP" style="position: relative !important;">&nbsp;&nbsp;Has Dependency</label>
                                </div>

                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="flex">
                                <select id="MM_MST_DependsOnAttribute_ID" required class="fieldToGetAttr" Title="" disabled="disabled"></select>
                                <label id="">Dependency Attribute </label>
                            </div>
                        </div>
                        <div class="col-md-3">

                            <div class="flex">
                                <textarea class="fieldToGetAttr" id="Description" required=""></textarea>
                                <label class="lblFormItem">Description</label>
                            </div>
                        </div>


                        <div class="col-md-3">
                            <div class="checkbox between" flex between >
                                <input type="checkbox" id="IsActive" class="i-checks fieldToGetAttr" checked />
                                <label for="IsActive" class="lblFormItem">&nbsp;&nbsp;Active</label>
                                <input type="hidden" value="0" id="MM_MST_Attribute_ID" class="fieldToGet" />
                                <button type="button" id="btnCreate" class="btn btn-primary" onclick="UpsertData();">Create</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />


            <div class="ibox-content" id="divcyclecountPref">
                <h4 class="h3Header">Industry Mapping</h4>
                <div class="invScrool" style="padding: 10px;">
                    <table class="table-striped dataTables-example" id="tblIndList"></table>
                </div>
            </div>
            <br />
            <br />

            <div class="ibox-content" id="lookupContainer" style="display: none;">
                <h4 class="h3Header">Attribute Lookup</h4>
                <div class="invScrool" style="padding: 10px;">

                    <table class="table-striped dataTables-example" id="tblList"></table>
                </div>
            </div>




            <!-- ========================= Modal Popup For Attribute Lookup ========================================== -->
            <div class="modal inmodal" id="AddLookupToCreate" tabindex="-1" role="dialog" aria-hidden="true">
                <div class="modal-dialog" style="width: 50% !important;">
                    <div class="modal-content animated fadeIn">

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">Attribute Lookup</h4>
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
                                                <input type="text" id="LookupText" required="" />
                                                <label id="">Lookup Text </label>
                                                <span class="errorMsg"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="flex">
                                                <input type="text" id="LookupValue" required="" onkeypress="return isNumberKey(event)" />
                                                <label id="">Lookup Value </label>
                                                <span class="errorMsg"></span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-md-6" id="divLookupFilterValue">
                                            <div class="flex">
                                                <select id="LookupFilterValue" class="EntityfieldToGet"></select>
                                                <label id="filterValue">Lookup Filter Value </label>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="flex">
                                                <input type="text" id="DisplaySeq" required="" onkeypress="return isNumberKey(event)" />
                                                <label id="">Display Seq. </label>
                                                <span class="errorMsg"></span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <div class="checkbox">
                                                <input type="checkbox" id="IsActiveLookup" class="i-checks" checked />
                                                <label for="IsActiveEntity" class="lblFormItem">&nbsp;&nbsp;Active</label>
                                            </div>
                                            <div class=" checkbox" style="display: none !important;">
                                                <input type="checkbox" id="IsDeletedLookup" class="i-checks" />
                                                <label for="IsDeletedEntity" class="lblFormItem">&nbsp;&nbsp;Is Deleted</label>
                                            </div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="hidden" value="0" id="MM_MST_AttributeLookup_ID" class="EntityfieldToGet" />
                            <button type="button" class="btn btn-primary" data-dismiss="modal" style="color: white !important;">Close</button>
                            <button type="button" class="btn btn-primary" id="btnLookupCreate" onclick="return UpsertLookupData();">Create</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- ========================= END Modal Popup For Attribute Lookup ========================================== -->


            <!-- ========================= Modal Popup For Attribute Lookup ========================================== -->
            <div class="modal inmodal" id="AddIndustryToCreate" tabindex="-1" role="dialog" aria-hidden="true">
                <div class="modal-dialog" style="width: 50% !important;">
                    <div class="modal-content animated fadeIn">

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">Industry Mapping</h4>
                        </div>

                        <div class="modal-body">
                            <div id="divValidationCycleCountEntityMessages1" class="text-danger"></div>
                            <p></p>
                            <p></p>
                            <br />
                            <div id="divIndustryDetails" class="form-horizontal">
                                <form role="form">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <div class="flex">
                                                <input type="text" id="txtIndustry" required="" />
                                                <label id="">Industry </label>
                                                <span class="errorMsg"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="flex">
                                                <input type="text" id="UILabelText" required="" />
                                                <label id="">UI Label Text </label>
                                                <span class="errorMsg"></span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <div class="flex">
                                                <select id="GEN_MST_UIControlType_ID" class="EntityfieldToGet"></select>
                                                <label id="">UI Control Type </label>
                                                <span class="errorMsg"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="checkbox">
                                                <input type="checkbox" id="IsMandatory" class="i-checks" />
                                                <label for="IsMandatory" class="lblFormItem">&nbsp;&nbsp;Is Mandatory</label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <div class="checkbox">
                                                <input type="checkbox" id="IsActiveIndustry" class="i-checks" checked />
                                                <label for="IsActiveIndustry" class="lblFormItem">&nbsp;&nbsp;Active</label>
                                            </div>
                                            <div class="checkbox" style="display: none !important;">
                                                <input type="checkbox" id="IsDeletedIndustry" class="i-checks" />
                                                <label for="IsDeletedIndustry" class="lblFormItem">&nbsp;&nbsp;Is Deleted</label>
                                            </div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input type="hidden" value="0" id="MM_CNF_IndustryMaterialAttribute_ID" class="EntityfieldToGet" />

                            <button type="button" class="btn btn-primary" data-dismiss="modal" style="color: white !important;">Close</button>
                            <button type="button" class="btn btn-primary" id="btnIndustryCreate" onclick="return UpsertIndustryData();">Create</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- ========================= END Modal Popup For Attribute Lookup ========================================== -->
            <div class="loading" id="divLoading" style="display: none;"></div>
        </div>

    </div>



    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/CycleCountScripts/jquery-1.11.3.min.js"></script>
    <script src="../mInventory/Scripts/jquery-ui-1.8.24.min.js"></script>
    <script src="../mInventory/CycleCountScripts/bootstrap.min.js"></script>
    <script src="../mInventory/CycleCountScripts/jquery.dataTables.min.js"></script>
    <script src="../mInventory/CycleCountScripts/dataTables.bootstrap.min.js"></script>

    <script src="../Scripts/autoResize.js"></script>
    <script src="../Scripts/CommonWMS.js"></script>
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="../mInbound/Scripts/InventraxAjax.js"></script>
    <script src="../mInventory/CycleCountScripts/jquery.cookie.min.js"></script>
    <link href="../Content/app.css" rel="stylesheet" />



    <script>
        debugger;
        var ItemList = [];
        var AttributeID = 0;
        var Accountid;
        var TotalAttributesList = [];

        $(document).ready(function () {
            debugger;
            //AttributeID = new URL(window.location.href).searchParams.get("parm");
            LoadPage();

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
                    $("#txtTenant").val("");
                    $("#TM_MST_Tenant_ID").val("0")
                },
                minLength: 0
            });


            //debugger;
            var TextFieldName = $("#txtTenant");
            DropdownFunction(TextFieldName);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantsForWHList") %>',
                        data: "{ 'prefix': '" + request.term + "', 'AccountID': '" + $("#AM_MST_Account_ID").val() + "'}",

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

                    $("#TM_MST_Tenant_ID").val(i.item.val);

                },
                minLength: 0
            });

            //debugger;
            var TextFieldName = $("#txtIndustry");
            DropdownFunction(TextFieldName);
            $("#txtIndustry").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadIndustries_Auto") %>',
                        data: "{ 'Prefix': '" + request.term + "'}",

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
                    $("#GEN_MST_Industry_ID").val(i.item.val);
                },
                minLength: 0
            });

        });




        function LoadPage() {
            $("#divLoading").show();
            AttributeID = new URL(window.location.href).searchParams.get("parm");
            var data = "{ Id :  '" + AttributeID + "' }";
            InventraxAjax.AjaxResultExecute("attributesDetails.aspx/GetList", data, 'GetListOnSuccess', 'GetListOnError', null);
        }
        function GetListOnSuccess(data) {
            var dataList = JSON.parse(data.Result);
            ItemList = dataList;
            LoadData();
            AttributeID = new URL(window.location.href).searchParams.get("parm");


            if (AttributeID != 0) {
                EditItem(AttributeID);
            }
            else {
                $('.EntityListCollapse').on('click', function (e) {
                    e.stopPropagation();
                });
                $("#MM_MST_DependsOnAttribute_ID").empty();
                $("#MM_MST_DependsOnAttribute_ID").append($("<option></option>").val(0).html("Please Select"));
                for (var x = 0; x < ItemList.Table.length; x++) {
                    $("#MM_MST_DependsOnAttribute_ID").append($("<option></option>").val(ItemList.Table[x].MM_MST_Attribute_ID).html(ItemList.Table[x].AttributeName));
                }
                //hasLookup();
            }
            $("#divLoading").hide();
        }


        function LoadData() {
            Accountid = <%=this.cp.AccountID %>;
                if (Accountid == 0 || Accountid == null) {
                    Accountid = 0;
                    $("#txtAccount").attr("disabled", false);
                }
                else {
                    Accountid = Accountid;
                    var Account = '<%=this.cp.Account%>';
                $("#txtAccount").val(Account);
                $("#AM_MST_Account_ID").val(Accountid);

                $("#txtAccount").attr("disabled", true);
            }
        }

        function EditItem(AttrID) {
            debugger;
            var data = "{ AttributeID:  " + AttrID + " }";
            InventraxAjax.AjaxResultExecute("attributesDetails.aspx/GetEditData", data, 'GetEditDatatOnSuccess', 'GetListOnError', null);
        }

        function GetEditDatatOnSuccess(data) {
            debugger;
            var item = JSON.parse(data.Result);
            LoadDependenceAttr(item.Table4);
            BuildAttributeFormtoEdit(item.Table);
            LoadDependenceAttr(item.Table4);
            BuildAttributeFormtoEdit(item.Table);
            $("#btnCreate").html('Update');
            LoadLookupData(item.Table1);
            LoadIndustryData(item.Table2);
            if ($('#IsLookupDefined').prop('checked') == true && item.Table2.length > 0) {
                $("#lookupContainer").css("display", "block");
            }
            LoadDropdowns(item.Table3, item.Table5);


        }
        function BuildAttributeFormtoEdit(item) {
            debugger;
            $('.fieldToGetAttr').each(function () {
                var fieldID = $(this).attr('id');
                var paramtype = $(this).attr('type');
                if (paramtype == 'checkbox') {
                    item[0][fieldID] == 1 ? $('#' + fieldID).prop('checked', true) : $('#' + fieldID).prop('checked', false)
                }
                $('#' + fieldID).val(item[0][fieldID]);
            });

            if (item[0].MM_MST_DependsOnAttribute_ID == null) {
                $("#MM_MST_DependsOnAttribute_ID").val("0");
            }

            $("#AM_MST_Account_ID").val(item[0].AM_MST_Account_ID);
            $("#txtAccount").val(item[0].Account);
            $("#TM_MST_Tenant_ID").val(item[0].TM_MST_Tenant_ID);
            $("#txtTenant").val(item[0].Tenant);
            $("#MM_MST_Attribute_ID").val(item[0].MM_MST_Attribute_ID);
            DisableDependents();
            //hasLookup();
        }

        function BacktoList() {
            $("#divLoading").show();
            window.location.href = "attributeMaster.aspx";
        }

        function LoadDependenceAttr(items) {
            debugger;
            var dt = $.grep(items, function (a) { return a.MM_MST_Attribute_ID == $("#MM_MST_Attribute_ID").val() });

            var Obj = differenceInFirstArray(items, dt, 'MM_MST_Attribute_ID');

            $("#MM_MST_DependsOnAttribute_ID").empty();
            $("#MM_MST_DependsOnAttribute_ID").append($("<option></option>").val(0).html("Please Select"));
            for (var x = 0; x < Obj.length; x++) {
                $("#MM_MST_DependsOnAttribute_ID").append($("<option></option>").val(Obj[x].MM_MST_Attribute_ID).html(Obj[x].AttributeName));
            }
        }

        function differenceInFirstArray(array1, array2, compareField) {
            return array1.filter(function (current) {
                return array2.filter(function (current_b) {
                    return current_b[compareField] === current[compareField];
                }).length == 0;
            });
        }

        function GetAttributeFormData() {
            debugger;

            var fieldDataOut = '{';
            var fieldData = '<root><data>';
            $('.fieldToGetAttr').each(function () {
                debugger;
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

                if (param == "MM_MST_DependsOnAttribute_ID" && Actualval == "0") {
                    fieldData += '<MM_MST_DependsOnAttribute_ID></MM_MST_DependsOnAttribute_ID>';
                }
                else {
                    fieldData += '<' + param + '>' + val + '</' + param + '>';
                }

            });

            fieldData += '<AM_MST_Account_ID>' + $("#AM_MST_Account_ID").val() + '</AM_MST_Account_ID>';
            fieldData += '<TM_MST_Tenant_ID>' + $("#TM_MST_Tenant_ID").val() + '</TM_MST_Tenant_ID>';
            fieldData += '<IsDeleted>' + 0 + '</IsDeleted>';
            fieldData = fieldData + '</data></root>';
            fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'UpdatedBy' + '":"' + <%=this.cp.UserID%> + '",' + '"' + String.fromCharCode(64) + 'MM_MST_Attribute_ID' + '":"' + $('#MM_MST_Attribute_ID').val() + '",';
            fieldDataOut = fieldDataOut.substring(0, fieldDataOut.length - 1);
            fieldDataOut += '}';
            return fieldDataOut;
        }

        function UpsertData() {
            debugger;
            if (ValidateAttributeData()) {
                $("#divLoading").show();
                var param = JSON.stringify({ "Sp_Set": "", "JSON": JSON.stringify(GetAttributeFormData()) });
                $.ajax({
                    url: "attributesDetails.aspx/MasterDetailsSet",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    cache: false,
                    data: param,
                    success: function (response) {
                        var result = response;
                        if ($('#btnCreate').text() == "Update") {
                            showStickyToast(true, 'Attributes Details Updated Successfully', false);
                            setTimeout(function () {
                                BacktoList();
                            }, 2000);
                        }
                        else {
                            showStickyToast(true, 'Attributes Details Created Successfully', false);
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


        function ValidateAttributeData() {
            $("#divValidationCycleCountMessages").empty();
            var IsValid = true;

            if ($("#txtAccount").val() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Select Account.', false);
                return false;
            }

            if ($("#txtTenant").val() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Select Tenant.', false);
                return false;
            }

            if ($("#AttributeName").val() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Enter Attribute Name.', false);
                return false;
            }

            if ($("#AttributeCode").val().trim() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Enter Attribute Code.', false);
                return false;
            }


            if ($("#AttributeName").val().trim() != "") {
                var item = ItemList.Table, Count = 0;

                if ($('#MM_MST_Attribute_ID').val() != 0) {

                    item = $.grep(ItemList.Table, function (data) {
                        return data['MM_MST_Attribute_ID'] != $('#MM_MST_Attribute_ID').val();
                    });
                }

                Count = $.grep(item, function (data) {

                    return data['AttributeName'] == $('#AttributeName').val().trim();
                });

                if (Count.length != 0) {
                    IsValid = false;
                    showStickyToast(false, 'Attribute Name already exists.', false);
                    return false;
                }
            }

            if ($("#AttributeCode").val().trim() != "") {
                var item = ItemList.Table, Count = 0;

                if ($('#MM_MST_Attribute_ID').val() != 0) {

                    item = $.grep(ItemList.Table, function (data) {
                        return data['MM_MST_Attribute_ID'] != $('#MM_MST_Attribute_ID').val();
                    });
                }

                Count = $.grep(item, function (data) {

                    return data['AttributeCode'] == $('#AttributeCode').val().trim();
                });

                if (Count.length != 0) {
                    IsValid = false;
                    showStickyToast(false, 'Attribute Code already exists.', false);
                    return false;
                }
            }

            return IsValid;
        }



        function DisableDependents() {
            debugger;

            if ($('#HasDependency').prop('checked') == true) {
                $("#MM_MST_DependsOnAttribute_ID").attr("disabled", false);
                $("#divLookupFilterValue").css("display", "block");

            }
            else {
                $("#MM_MST_DependsOnAttribute_ID").attr("disabled", "disabled");
                $("#divLookupFilterValue").css("display", "none");
                $("#MM_MST_DependsOnAttribute_ID").val("");
            }
        }

        function hasLookup() {
            debugger;
            if (AttributeID != 0) {
                if ($('#IsLookupDefined').prop('checked') == true) {
                    $("#lookupContainer").css("display", "block");
                    $("#MM_MST_DependsOnAttribute_ID").attr("disabled", false);
                    $("#HasDependency").attr("disabled", false);
                    checkORuncheck();
                }
                else {

                    $("#lookupContainer").css("display", "none");
                    $("#MM_MST_DependsOnAttribute_ID").val("0");
                    $("#MM_MST_DependsOnAttribute_ID").attr("disabled", true);
                    $("#HasDependency").attr("disabled", true);
                    $("#HasDependency").prop("checked", false);
                    DisableDependents();
                    checkORuncheck();
                }
            }
            else {
                if ($('#IsLookupDefined').prop('checked') == true) {
                    $("#HasDependency").attr("disabled", false);
                }
                else {
                    $("#MM_MST_DependsOnAttribute_ID").val("0");
                    $("#MM_MST_DependsOnAttribute_ID").attr("disabled", true);
                    $("#HasDependency").attr("disabled", true);
                    $("#HasDependency").prop("checked", false);
                    DisableDependents();
                    checkORuncheck();
                }
            }
        }

        function checkORuncheck() {
            $("#tblList tbody").find("tr").each(function () {
                debugger;
                if ($(this).length > 0) {
                    $("#MM_MST_DependsOnAttribute_ID").attr("disabled", true);
                    $("#HasDependency").attr("disabled", true);
                }
                else {
                    $("#lookupContainer").css("display", "none");
                    $("#MM_MST_DependsOnAttribute_ID").val("0");
                    $("#MM_MST_DependsOnAttribute_ID").attr("disabled", false);
                    $("#HasDependency").attr("disabled", false);
                    $("#HasDependency").prop("checked", false);
                }
            });
        }

        //=================== Lookup ======================//
        var LookupList = null;
        function LoadLookupData(Obj) {
            if (Obj != null && Obj.length > 0) {
                LookupList = Obj;

                $("#tblList").empty();
                $("#tblList").append("<thead><tr><th colspan='7' style='background-color: white'><div style='text-align:right;'><button type='button' class='btn btn-primary' data-toggle='modal' data-target='#AddLookupToCreate' onclick='clearLookupDetails()'>Add <i class='fa fa-plus' aria-hidden='true'></i></button></div></th></tr><tr   class='stripedhed'><th class='text-center'>S.No </th><th class='text-center'>Lookup Text</th><th class='text-center' id='thAttributeName'>" + $("#MM_MST_DependsOnAttribute_ID Option:selected").text() + "</th><th class='text-center'>Display Seq.</th><th class='text-center'>Active</th><th class='text-center'>Action</th></tr></thead><tbody>");
                for (var i = 0; i < LookupList.length; i++) {
                    if (LookupList[i].IsActive == true)
                        LookupList[i].IsActive = 'Yes'
                    else
                        LookupList[i].IsActive = 'No'
                    $("#tblList").append("<tr><td >" + (i + 1) + "</td><td class='text-left'>" + LookupList[i].LookupText + "</td><td class='text-left' id='tdAttributeName" + (i + 1) + "'>" + LookupList[i].LookupFilterValueText + "</td><td>" + LookupList[i].DisplaySeq + "</td><td>" + LookupList[i].IsActive + "</td><td class='text-center' style='text-align:left !important;'> <a style='cursor:pointer;' data-toggle='modal' data-target='#AddLookupToCreate' onclick='EditLookupData(" + LookupList[i].MM_MST_AttributeLookup_ID + ");'><i class='material-icons ss'>mode_edit</i></a>&emsp; <a style='cursor:pointer;' onclick='DeleteLookupItem(" + LookupList[i].MM_MST_AttributeLookup_ID + ");'><i class='material-icons ss'>delete</i></a></td></tr>");

                }
                $("#tblList").append("</tbody>");
                $("#MM_MST_DependsOnAttribute_ID").attr("disabled", true);
                $("#IsLookupDefined").attr("disabled", true);

                $('#IsLookupDefined').parent('.checkbox').addClass('chackbox-disble');
                $("#HasDependency").attr("disabled", true);
                $('#MM_MST_DependsOnAttribute_ID').after('<span class="data-tooltip">Please delete lookup data to change.</span>');
                $('#btnCreate').after('<span class="data-tooltip">Please delete lookup data to change.</span>');
                $("#btnCreate").attr("disabled", true);
                $("#lookupContainer").css("display", "block");
                if ($("#MM_MST_DependsOnAttribute_ID").val() == "0" || $("#MM_MST_DependsOnAttribute_ID").val() == null) {
                    for (var i = 0; i < LookupList.length; i++) {
                        $("#tdAttributeName" + (i + 1)).css("display", "none");
                    }
                    $("#thAttributeNameFooter").css("display", "none");
                    $("#thAttributeName").css("display", "none");

                }
                else {
                    $("#thAttributeNameFooter").css("display", "block");
                    $("#thAttributeName").css("display", "block");
                    for (var i = 0; i < LookupList.length; i++) {
                        $("#tdAttributeName" + (i + 1)).css("display", "table-cell");
                    }
                }

                //SetTableSettings();
            }
            else {
                $("#tblList").empty();
                $("#tblList").append("<thead><tr><th colspan='7'  style='background-color: white'><div style='text-align:right;'><button type='button' class='btn btn-primary' data-toggle='modal' data-target='#AddLookupToCreate' onclick='clearLookupDetails()'>Add <i class='fa fa-plus' aria-hidden='true'></i></button></div></th></tr><tr><th class='text-center'>S.No </th><th class='text-center'>Lookup Text</th><th class='text-center' id='thAttributeNameFooter'>" + $("#MM_MST_DependsOnAttribute_ID Option:selected").text() + "</th><th class='text-center'>Display Seq.</th><th class='text-center'>Active</th><th class='text-center'>Action</th></tr><tr><th colspan='7' class='text-center' style='background-color: white'><p style='text-align: center;'><strong>No Data</strong></p></th></tr></thead><tbody>");
                $("#tblList").append("</tbody>");
                $("#MM_MST_DependsOnAttribute_ID").attr("disabled", false);
                $("#HasDependency").attr("disabled", false);
                $("#btnCreate").attr("disabled", false);
                $("#lookupContainer").css("display", "none");
                $("#thAttributeNameFooter").css("display", "none");
                $("#thAttributeName").css("display", "none");
            }
        }

        function GetAttributeLookupFormData() {
            debugger;
            var fieldDataOut = '{';
            var fieldData = '<root><data>';
            var val = 0;
            if ($("#IsActiveLookup").prop('checked') == true) { val = 1; } else { val = 0; }
            fieldData += '<MM_MST_Attribute_ID>' + $("#MM_MST_Attribute_ID").val() + '</MM_MST_Attribute_ID>';
            fieldData += '<LookupText>' + $("#LookupText").val() + '</LookupText>';
            fieldData += '<LookupValue>' + $("#LookupValue").val() + '</LookupValue>';
            if ($("#LookupFilterValue").val() == "0" || $("#LookupFilterValue").val() == null) {
                fieldData += '<LookupFilterValue></LookupFilterValue>';
            }
            else {
                fieldData += '<LookupFilterValue>' + $("#LookupFilterValue").val() + '</LookupFilterValue>';
            }

            fieldData += '<DisplaySeq>' + $("#DisplaySeq").val() + '</DisplaySeq>';

            fieldData += '<IsActive>' + val + '</IsActive>';
            fieldData += '<IsDeleted>' + 0 + '</IsDeleted>';
            fieldData = fieldData + '</data></root>';
            fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'UpdatedBy' + '":"' + <%=this.cp.UserID%> + '",' + '"' + String.fromCharCode(64) + 'MM_MST_AttributeLookup_ID' + '":"' + $('#MM_MST_AttributeLookup_ID').val() + '",';
            fieldDataOut = fieldDataOut.substring(0, fieldDataOut.length - 1);
            fieldDataOut += '}';
            return fieldDataOut;
        }


        function UpsertLookupData() {
            debugger;
            if (ValidateAttributeLookupData()) {
                $("#divLoading").show();
                var param = JSON.stringify({ "Sp_Set": "", "JSON": JSON.stringify(GetAttributeLookupFormData()) });
                $.ajax({
                    url: "attributesDetails.aspx/MasterDetailsSetLookup",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    cache: false,
                    data: param,
                    success: function (response) {
                        var result = response;
                        if ($('#btnLookupCreate').text() == "Update") {
                            showStickyToast(true, 'Attribute Lookup Details Updated Successfully', false);
                            setTimeout(function () {
                                //location.reload();
                                LoadPage();
                                $('#AddLookupToCreate').modal('hide');
                            }, 2000);
                        }
                        else {
                            showStickyToast(true, 'Attribute Lookup Details Created Successfully', false);
                            setTimeout(function () {
                                //location.reload();
                                $('#AddLookupToCreate').modal('hide');
                                LoadPage();
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
        function ValidateAttributeLookupData() {
            $("#divValidationCycleCountMessages").empty();
            var IsValid = true;

            if ($("#LookupText").val() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Enter Lookup Text.', false);
                return false;
            }

            if ($("#LookupValue").val() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Enter Lookup Value.', false);
                return false;
            }

            if ($("#DisplaySeq").val() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Enter Display Seq.', false);
                return false;
            }
            if (LookupList != null && LookupList.length > 0) {
                if ($("#LookupText").val().trim() != "") {
                    var item = LookupList, Count = 0;

                    if ($('#MM_MST_AttributeLookup_ID').val() != 0) {

                        item = $.grep(LookupList, function (data) {
                            return data['MM_MST_AttributeLookup_ID'] != $('#MM_MST_AttributeLookup_ID').val();
                        });
                    }

                    Count = $.grep(item, function (data) {

                        return data['LookupText'] == $('#LookupText').val().trim();
                    });

                    if (Count.length != 0) {
                        IsValid = false;
                        showStickyToast(false, 'Lookup Text already exists.', false);
                        return false;
                    }
                }
            }
            return IsValid;
        }

        function EditLookupData(id) {
            clearLookupDetails();
            var item = $.grep(LookupList, function (a) { return a.MM_MST_AttributeLookup_ID == id });
            $("#LookupText").val(item[0].LookupText);
            $("#LookupValue").val(item[0].LookupValue);
            $("#LookupFilterValue").val(item[0].LookupFilterValue);
            $("#DisplaySeq").val(item[0].DisplaySeq);
            $("#MM_MST_AttributeLookup_ID").val(item[0].MM_MST_AttributeLookup_ID);
            $("#btnLookupCreate").html('Update');
        }

        function DeleteLookupItem(id) {
            if (confirm("Are you sure do you want to delete ?")) {
                $("#divLoading").show();
                var param = JSON.stringify({ "SP_Del": "", "ID": id });
                $.ajax({
                    url: "attributesDetails.aspx/DeleteItemsByIdLookup",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    cache: false,
                    data: param,
                    success: function (response) {

                        debugger;
                        var dt = JSON.parse(response.d).Table[0].N;
                        if (dt == 1) {
                            showStickyToast(false, "This Lookup is configured in Item Master.", false);
                            $("#divLoading").hide();
                            return;
                        }
                        else {
                            showStickyToast(true, "Deleted Successfully.", false);
                            setTimeout(function () {
                                clearLookupDetails();
                                LookupList = null;
                                LoadPage();
                            }, 2500);
                        }

                        //showStickyToast(true, "Deleted Successfully.", false);
                        //setTimeout(function () {
                        //    //location.reload();
                        //    LoadPage();
                        //}, 2500);
                    },
                    failure: function (errMsg) {

                    }
                });
            }
        }



        function clearLookupDetails() {
            debugger;
            $("#LookupText").val("");
            $("#LookupValue").val("");
            $("#LookupFilterValue").val("0");
            $("#DisplaySeq").val("");
            $("#MM_MST_AttributeLookup_ID").val("0");
            $("#filterValue").text($("#MM_MST_DependsOnAttribute_ID Option:selected").text());
        }
        //=================== Industry =======================//

        function LoadDropdowns(Obj, Obj1) {
            debugger;
            if (Obj != null && Obj.length > 0) {
                if ($("#IsLookupDefined").prop('checked') == true) {
                    $("#GEN_MST_UIControlType_ID").empty();
                    $("#GEN_MST_UIControlType_ID").append($("<option></option>").val(0).html("Please Select"));
                    for (var x = 0; x < Obj.length; x++) {
                        if (Obj[x].UIControlType == "TextBox" || Obj[x].UIControlType == "DatePicker" || Obj[x].UIControlType == "CheckBox" || Obj[x].UIControlType == "RadioButton") {
                        }
                        else {
                            $("#GEN_MST_UIControlType_ID").append($("<option></option>").val(Obj[x].UIControlTypeID).html(Obj[x].UIControlType));
                        }
                    }
                }
                else {
                    $("#GEN_MST_UIControlType_ID").empty();
                    $("#GEN_MST_UIControlType_ID").append($("<option></option>").val(0).html("Please Select"));
                    for (var x = 0; x < Obj.length; x++) {
                        if (Obj[x].UIControlType == "DropdownList" || Obj[x].UIControlType == "CheckBox" || Obj[x].UIControlType == "RadioButton") {
                        } else {
                            $("#GEN_MST_UIControlType_ID").append($("<option></option>").val(Obj[x].UIControlTypeID).html(Obj[x].UIControlType));
                        }
                    }
                }
            }
            if (Obj1 != null && Obj1.length > 0) {
                if ($("#HasDependency").prop('checked') == true) {
                    $("#divLookupFilterValue").css("display", "block");
                    var dt = $.grep(Obj1, function (a) { return a.MM_MST_Attribute_ID == $("#MM_MST_DependsOnAttribute_ID").val() });
                    $("#LookupFilterValue").empty();
                    $("#LookupFilterValue").append($("<option></option>").val(0).html("Please Select"));
                    for (var x = 0; x < dt.length; x++) {
                        $("#LookupFilterValue").append($("<option></option>").val(dt[x].MM_MST_AttributeLookup_ID).html(dt[x].LookupText));
                    }
                }
                else {
                    $("#LookupFilterValue").empty();
                    $("#LookupFilterValue").append($("<option></option>").val(0).html("Please Select"));
                    $("#divLookupFilterValue").css("display", "none");
                }
            }
        }

        var IndutryList = null;
        function LoadIndustryData(Obj) {
            if (Obj != null && Obj.length > 0) {
                IndutryList = Obj;

                $("#tblIndList").empty();
                $("#tblIndList").append("<thead><tr><th colspan='7' style='background-color: white'><div style='text-align:right;'><button type='button' class='btn btn-primary' data-toggle='modal' data-target='#AddIndustryToCreate' onclick='clearindustryDetails();'>Add <i class='fa fa-plus' aria-hidden='true'></i></button></div></th></tr><tr   class='stripedhed'><th class='text-center'>S.No </th><th class='text-center'>Industry Name</th><th class='text-center'>UI Label Text</th><th class='text-center'>UI Control Type</th><th class='text-center'>Is Mandatory</th><th class='text-center'>Active</th><th class='text-center'>Action</th></tr></thead><tbody>");
                for (var i = 0; i < IndutryList.length; i++) {
                    debugger
                    if (IndutryList[i].IsMandatory == 'false' || IndutryList[i].IsMandatory == false) {
                        IndutryList[i].IsMandatory = 'No';
                    }
                    else {
                        IndutryList[i].IsMandatory = 'Yes';
                    }
                    if (IndutryList[i].IsActive == true || IndutryList[i].IsActive=='true') {
                        IndutryList[i].IsActive = 'Yes';
                    }
                    else {
                        IndutryList[i].IsActive = 'No';
                    }
                    $("#tblIndList").append("<tr><td >" + (i + 1) + "</td><td class='text-left'>" + IndutryList[i].IndustryName + "</td><td>" + IndutryList[i].UILabelText + "</td><td>" + IndutryList[i].UIControlType + "</td><td>" + IndutryList[i].IsMandatory + "</td><td>" + IndutryList[i].IsActive + "</td><td class='text-center' style='text-align:left !important;'> <a style='cursor:pointer;' data-toggle='modal' data-target='#AddIndustryToCreate' onclick='EditIndustryData(" + IndutryList[i].MM_CNF_IndustryMaterialAttribute_ID + ");'><i class='material-icons ss'>mode_edit</i></a>&emsp; <a style='cursor:pointer;' onclick='DeleteIndustry(" + IndutryList[i].MM_CNF_IndustryMaterialAttribute_ID + ");'><i class='material-icons ss'>delete</i></a></td></tr>");

                }
                $("#tblIndList").append("</tbody>");
                //SetTableSettings();
            }
            else {
                $("#tblIndList").empty();
                $("#tblIndList").append("<thead><tr><th colspan='7'  style='background-color: white'><div style='text-align:right;'><button type='button' class='btn btn-primary' data-toggle='modal' data-target='#AddIndustryToCreate'>Add <i class='fa fa-plus' aria-hidden='true'></i></button></div></th></tr><tr><th class='text-center'>S.No </th><th class='text-center'>Industry Name</th><th class='text-center'>UI Label Text</th><th class='text-center'>UI Control Type</th><th class='text-center'>Is Mandatory</th><th class='text-center'>Active</th><th class='text-center'>Action</th></tr><tr><th colspan='7' class='text-center' style='background-color: white'><p style='text-align: center;'><strong>No Data</strong></p></th></tr></thead><tbody>");
                $("#tblIndList").append("</tbody>");
            }
        }

        function GetAttributeIndustryFormData() {
            debugger;
            var fieldDataOut = '{';
            var fieldData = '<root><data>';
            var val = 0;
            var ismand = 0;
            if ($("#IsActiveIndustry").prop('checked') == true) { val = 1; } else { val = 0; }
            if ($("#IsMandatory").prop('checked') == true) { ismand = 1; } else { ismand = 0; }

            fieldData += '<MM_MST_Attribute_ID>' + $("#MM_MST_Attribute_ID").val() + '</MM_MST_Attribute_ID>';
            fieldData += '<GEN_MST_Industry_ID>' + $("#GEN_MST_Industry_ID").val() + '</GEN_MST_Industry_ID>';
            fieldData += '<UILabelText>' + $("#UILabelText").val() + '</UILabelText>';
            fieldData += '<GEN_MST_UIControlType_ID>' + $("#GEN_MST_UIControlType_ID").val() + '</GEN_MST_UIControlType_ID>';

            fieldData += '<IsMandatory>' + ismand + '</IsMandatory>';

            fieldData += '<IsActive>' + val + '</IsActive>';
            fieldData += '<IsDeleted>' + 0 + '</IsDeleted>';
            fieldData = fieldData + '</data></root>';
            fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'UpdatedBy' + '":"' + <%=this.cp.UserID%> + '",' + '"' + String.fromCharCode(64) + 'MM_CNF_IndustryMaterialAttribute_ID' + '":"' + $('#MM_CNF_IndustryMaterialAttribute_ID').val() + '",';
            fieldDataOut = fieldDataOut.substring(0, fieldDataOut.length - 1);
            fieldDataOut += '}';
            return fieldDataOut;
        }


        function UpsertIndustryData() {
            debugger;
            if (ValidateIndustryData()) {
                $("#divLoading").show();
                var param = JSON.stringify({ "Sp_Set": "", "JSON": JSON.stringify(GetAttributeIndustryFormData()) });
                $.ajax({
                    url: "attributesDetails.aspx/MasterDetailsSetIndustry",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    cache: false,
                    data: param,
                    success: function (response) {
                        debugger
                        var result = response;
                        if ($('#btnIndustryCreate').text() == "Update") {
                            showStickyToast(true, 'Industry Details Updated Successfully', false);
                            setTimeout(function () {
                                //location.reload();
                                LoadPage();
                                $('#AddIndustryToCreate').modal('hide');
                            }, 2000);
                            //LoadPage();
                        }
                        else {
                            showStickyToast(true, 'Industry Details Created Successfully', false);
                            setTimeout(function () {
                                //location.reload();
                                LoadPage();
                                $('#AddIndustryToCreate').modal('hide');
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
        function ValidateIndustryData() {
            $("#divValidationCycleCountMessages").empty();
            var IsValid = true;

            if ($("#txtIndustry").val() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Select Industry.', false);
                return false;
            }

            if ($("#UILabelText").val() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Enter UI Label Text.', false);
                return false;
            }

            if ($("#GEN_MST_UIControlType_ID").val() == "0") {
                IsValid = false;
                showStickyToast(false, 'Please Select UI Control Type.', false);
                return false;
            }

            if (IndutryList != null && IndutryList.length > 0) {
                if ($("#UILabelText").val().trim() != "") {
                    var item = IndutryList, Count = 0;

                    if ($('#MM_CNF_IndustryMaterialAttribute_ID').val() != 0) {

                        item = $.grep(IndutryList, function (data) {
                            return data['MM_CNF_IndustryMaterialAttribute_ID'] != $('#MM_CNF_IndustryMaterialAttribute_ID').val();
                        });
                    }

                    Count = $.grep(item, function (data) {

                        return data['UILabelText'] == $('#UILabelText').val().trim();
                    });

                    if (Count.length != 0) {
                        IsValid = false;
                        showStickyToast(false, 'UI Label Text already exists.', false);
                        return false;
                    }
                }
            }

            if (IndutryList != null && IndutryList.length > 0) {
                if ($("#UILabelText").val().trim() != "") {
                    var item = IndutryList, Count = 0;

                    if ($('#MM_CNF_IndustryMaterialAttribute_ID').val() != 0) {

                        item = $.grep(IndutryList, function (data) {
                            return data['MM_CNF_IndustryMaterialAttribute_ID'] != $('#MM_CNF_IndustryMaterialAttribute_ID').val();
                        });
                    }

                    Count = $.grep(item, function (data) {

                        return data['UILabelText'] == $('#UILabelText').val().trim();
                    });

                    if (Count.length != 0) {
                        IsValid = false;
                        showStickyToast(false, 'UI Label Text already exists.', false);
                        return false;
                    }
                }
            }

            if (IndutryList != null && IndutryList.length > 0) {
                if ($("#txtIndustry").val().trim() != "") {
                    var item = IndutryList, Count = 0;

                    if ($('#MM_CNF_IndustryMaterialAttribute_ID').val() != 0) {

                        item = $.grep(IndutryList, function (data) {
                            return data['MM_CNF_IndustryMaterialAttribute_ID'] != $('#MM_CNF_IndustryMaterialAttribute_ID').val();
                        });
                    }

                    Count = $.grep(item, function (data) {

                        return data['GEN_MST_Industry_ID'] == $('#GEN_MST_Industry_ID').val().trim();
                    });

                    if (Count.length != 0) {
                        IsValid = false;
                        showStickyToast(false, 'Industry already mapped.', false);
                        return false;
                    }
                }
            }

            return IsValid;
        }

        function EditIndustryData(cnfIndId) {
            debugger;
            clearindustryDetails();
            var editIndData = $.grep(IndutryList, function (a) { return a.MM_CNF_IndustryMaterialAttribute_ID == cnfIndId });
            $("#txtIndustry").val(editIndData[0].IndustryName);
            $("#UILabelText").val(editIndData[0].UILabelText);
            $("#GEN_MST_UIControlType_ID").val(editIndData[0].GEN_MST_UIControlType_ID);
            $("#MM_CNF_IndustryMaterialAttribute_ID").val(editIndData[0].MM_CNF_IndustryMaterialAttribute_ID);
            $("#GEN_MST_Industry_ID").val(editIndData[0].GEN_MST_Industry_ID);

            if (editIndData[0].IsActive == "Yes") {
                $("#IsActiveIndustry").prop('checked', true);
            }
            else { $("#IsActiveIndustry").prop('checked', false); }

            if (editIndData[0].IsMandatory == "Yes") {
                $("#IsMandatory").prop('checked', true);
            }
            else { $("#IsMandatory").prop('checked', false); }
            $("#btnIndustryCreate").html("Update");
        }

        function DeleteIndustry(id) {
            if (confirm("Are you sure do you want to delete ?")) {
                $("#divLoading").show();
                var param = JSON.stringify({ "SP_Del": "", "ID": id });
                $.ajax({
                    url: "attributesDetails.aspx/DeleteItemsByIdIndustry",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    cache: false,
                    data: param,
                    success: function (response) {
                        debugger;
                        var dt = JSON.parse(response.d).Table[0].N;
                        if (dt == 1) {
                            showStickyToast(false, "This Industry is configured.", false);
                            $("#divLoading").hide();
                            return;
                        }
                        else {
                            showStickyToast(true, "Deleted Successfully.", false);
                            setTimeout(function () {
                                clearindustryDetails();
                                IndutryList = null;
                                LoadPage();
                            }, 2500);
                        }

                        //getparameters('success', msg, msgtitle);
                        //showStickyToast(true, "Deleted Successfully.", false);
                        //setTimeout(function () {
                        //    //location.reload();
                        //    LoadPage();
                        //}, 2500);
                    },
                    failure: function (errMsg) {

                    }
                });
            }

        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function clearindustryDetails() {
            $("#txtIndustry").val("");
            $("#GEN_MST_Industry_ID").val("0");
            $("#UILabelText").val("");
            $("#GEN_MST_UIControlType_ID").val("0");
            $("#MM_CNF_IndustryMaterialAttribute_ID").val("0");
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#hdnCreatedBy").val(<%=cp.UserID%>);
                $("#hdnUpdatedBy").val(<%=cp.UserID%>);
        });
    </script>

</asp:Content>
