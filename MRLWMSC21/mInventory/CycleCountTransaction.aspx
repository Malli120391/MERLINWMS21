<%@ Page Title="Cycle Count Transaction" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="CycleCountTransaction.aspx.cs" Inherits="MRLWMSC21.mInventory.CycleCountTransaction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">

    <script src="CycleCountScripts/jquery-1.9.1.js"></script>
    <script src="CycleCountScripts/jquery-ui.min.js"></script>
    <link href="../Scripts/bootstrap/bootstrap.min.css" rel="stylesheet" />
      <script src="CycleCountScripts/bootstrap.min.js"></script>
    <script src="Scripts/bootstrap3-typeahead.min.js"></script>
        <script src="CycleCountScripts/jquery.dataTables.min.js"></script>
    <script src="CycleCountScripts/dataTables.bootstrap.min.js"></script>
        <script src="CycleCountScripts/bootstrap-datepicker.js"></script>
    <link href="CycleCountScripts/bootstrap-datepicker.css" rel="stylesheet" />
       <script src="CycleCountScripts/jquery.cookie.min.js"></script>

    <script src="../mInbound/Scripts/InventraxAjax.js"></script>
    <script src="../Scripts/CommonWMS.js"></script>
    <script src="Scripts/toast/jquery.toastmessage.js"></script>
    <script src="../mInbound/Scripts/InventraxAjax.js"></script>

    <input type="hidden" value='' id="hdnViewName" />
    <input type="hidden" value='' id="hdnSp_Get" />
    <input type="hidden" value='' id="hdnSp_Set" />
    <input type="hidden" value='' id="hdnJSONMaster" />         
    <input type="hidden" value='0' id="hdnCId" />

    <input type="hidden" value='0' id="hdnCreatedBy" />
    <input type="hidden" value='2018-01-04' id="hdnUpdatedOn" />
    <input type="hidden" value='0' id="hdnUpdatedBy" />

    <style>
         /* Absolute Center Spinner */
        .loading
        {
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
            .loading:before
            {
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
            .loading:not(:required)
            {
                /* hide "loading..." text */
                font: 0/0 a;
                color: transparent;
                text-shadow: none;
                background-color: transparent;
                border: 0;
            }

                .loading:not(:required):after
                {
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

        @-webkit-keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-moz-keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-o-keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        .excessds {
            overflow: auto;
            height: 72px !important;
            margin: 10px 0px;
            width: 100% !important;
                margin-top: 10px !important;
        }

         .requiredlabel:before
        {
            content: "";
            color: red;
        }
        
        .dropdown-item {color:White !important;background-color:#29328b !important;
        }

        @media (mx-width: 800px) {
            .modal-dialog {
                width:80% !important;
            }
        }

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

        .fa-edit, fa-trash {
            cursor: pointer;
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

      

    </style>
      <div class="module_yellow">
            <div class="ModuleHeader" height="35px">
                <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Orders</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Cycle Count Transaction </span></div>
                
            </div>

        </div>

    <div class="loading" id="divLoading" style="display: none;"></div> 
    <div class="container">
       
        <div class="ibox-content">
            <div class="invSrool">
                <div style="float:right; margin-left:10px;"><div class="checkbox"><input type="checkbox" id="chkStatus" onclick="ShowCycleCountData();" /><label for="chkStatus"><%= GetGlobalResourceObject("Resource", "ShowAll")%></label></div></div>
                <table class=" table-striped  dataTables-CycleCountList" id="tblList"></table>
            </div>
        </div>

    </div>

    <!-- ========================= Modal Popup For Cycle Count Details ========================================== -->
    <div class="modal inmodal" id="AddEntityToCreate" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog" style="width:50% !important;">
            <div class="modal-content animated fadeIn">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                   <%-- <h4 class="modal-title">Cycle Count</h4>--%>
                     <h4 class="modal-title"> <%= GetGlobalResourceObject("Resource", "CycleCount")%></h4>
                </div>

                <div class="modal-body">
                    <div id="divValidationCycleCountMessages" class="text-danger" style="color:red !important;"></div>
                    <p></p>
                    <div id="divDetails" class="form-horizontal">
                        <form role="form">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "CycleCount")%> </label> :
                                    <label id="CCM_MST_CycleCount_ID" style="font-weight: normal;"></label>
                                </div>
                                <div class="col-md-6">
                                    <label class="lblFormItem"> <%= GetGlobalResourceObject("Resource", "CycleCountName")%></label> :
                                    <label id="CCM_CNF_AccountCycleCount_ID" style="font-weight: normal;"></label>
                                </div>

                            </div>
                           
                            <div class="form-group">
                                <div class="col-md-6">
                                    <label class="lblFormItem"> <%= GetGlobalResourceObject("Resource", "ValidFrom")%> </label> :
                                    <label id="ValidFrom" style="font-weight: normal;"></label>
                                </div>
                                <div class="col-md-6">
                                    <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "ValidTo")%> </label> :
                                    <label id="ValidThru" style="font-weight: normal;"></label>
                                </div>
                            </div>

                             <div class="form-group">
                                 <div class="col-md-6">
                                <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "CycleCountDuration")%>  </label>
                                 <label id="CycleCountDuration" style="font-weight: normal;"></label>
                                     </div>
                                     <div class="col-md-6">
                                 <%--<label class="lblFormItem">Sequence No. : </label>
                                 <label id="SeqNo" class="labelfontbold"></label>--%>
                                         <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "Frequency")%> </label> :
                                 <label id="CycleTimeInDays" style="font-weight: normal;"></label>
                                         </div>
                            </div>



                            <div class="form-group">
                                <div class="col-md-6">
                                     <div class="flex">                                    
                                    <input type="text" class="form-control DueDate" id="PlannedStart" readonly="true" required="required" />
                                          <label><%= GetGlobalResourceObject("Resource", "PlannedStart")%></label>
                                         <span class="lblFormItem errorMsg"></span>
                                    </div>                                  
                                </div>
                                <div class="col-md-6">
                                    <div class="flex">
                                        <input type="text" class="form-control DueDate" id="PlannedEnd" readonly="true" style="background-color: white;" />
                                        <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "PlannedEnd")%></label>
                                    </div>
                                </div>


                            </div>

                            <div class="form-group">
                                <div class="col-md-6">
                                    <div class="flex">
                                        <textarea class="form-control fieldToGet excessds" id="InitiationRemarks"></textarea>
                                        <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "InitiateRemarks")%></label>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                      <div class="flex">
                                    <textarea class="form-control fieldToGet excessds" id="CompletionRemarks"></textarea>
                                          <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "CompletionRemarks")%></label>
                                          </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-6">
                                    <p></p><div class="checkbox">
                                    <input type="checkbox" id="IsActive" class="i-checks fieldToGet" checked />
                                    <label class="lblFormItem" for="IsActive"><%= GetGlobalResourceObject("Resource", "Active")%></label></div><br />
                                    <p></p>
                                    <div class="sr-only">
                                        <input type="checkbox" id="IsDeleted" class="i-checks fieldToGet" />
                                        <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "IsDeleted")%></label><br />
                                    </div>
                                </div>
                            </div>

                        </form>
                    </div>
                </div>
                <div class="modal-footer">
                    <input type="hidden" value="0" id="CCM_TRN_CycleCount_ID" class="fieldToGet" />
                    <button type="button" class="btn btn-primary" data-dismiss="modal" style="color:#fff !important;"><%= GetGlobalResourceObject("Resource", "Close")%> </button>
                    <button type="button" class="btn btn-primary" id="btnCreate" onclick="return UpsertData();"><%= GetGlobalResourceObject("Resource", "Update")%></button>
                </div>
            </div>
        </div>
    </div>
    <!-- ========================= END Modal Popup For Cycle Count Details ========================================== -->

    <!-- ========================= Modal Popup For Cycle Count Capture Details ========================================== -->
    <div class="modal inmodal" id="AddCaptureToCreate" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modalwidth" style="width:75% !Important;">
            <div class="modal-content animated fadeIn">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only"><%= GetGlobalResourceObject("Resource", "Close")%> </span></button>
                    <h4 class="modal-title">Cycle Count Capture</h4>
                </div>

                <div class="modal-body" style="height: 500px; overflow: auto;">
                    <div id="divCaptureValidationMessages" class="text-danger" style="color:red !important;"></div>
                    <p></p>
                    <div id="divCaptureDetails" class="form-horizontal">
                        <form role="form">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <div class="row">                                       
                                        <div class="col-md-5">
                                            <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "CycleCount")%></label>
                                        </div>
                                        <div class="col-md-1">
                                            :
                                        </div>
                                        <div class="col-md-6">
                                            <label id="CCM_MST_CycleCount_ID_Capture" class="labelfontbold"></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <label class="lblFormItem"> <%= GetGlobalResourceObject("Resource", "CycleCountName")%></label>
                                        </div>
                                        <div class="col-md-1">
                                            :
                                        </div>
                                        <div class="col-md-6">
                                            <label id="CCM_CNF_AccountCycleCount_ID_Capture" class="labelfontbold"></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="form-group">                                
                                
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "SequenceNo")%></label>
                                        </div>
                                        <div class="col-md-1">
                                            :
                                        </div>
                                        <div class="col-md-6">
                                            <label id="SeqNo_Capture" class="labelfontbold"></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 IsBlind" style="display:none;">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "AvailableQuantity")%> </label>
                                        </div>
                                        <div class="col-md-1">
                                            :
                                        </div>
                                        <div class="col-md-6">
                                            <label class="labelfontbold" id="AvailableQuantity"></label>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="form-group">
                                
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "PlannedStart")%> </label>
                                        </div>
                                        <div class="col-md-1">
                                            :
                                        </div>
                                        <div class="col-md-6">
                                            <label class="labelfontbold" id="PlannedStart_Capture"></label>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "PlannedEnd")%> </label>
                                        </div>
                                        <div class="col-md-1">
                                            :
                                        </div>
                                        <div class="col-md-6">
                                            <label class="labelfontbold" id="PlannedEnd_Capture"></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "ActualStart")%></label>
                                        </div>
                                        <div class="col-md-1">
                                            :
                                        </div>
                                        <div class="col-md-6">
                                            <label class="labelfontbold" id="ActualStart_Capture"></label>
                                        </div>
                                    </div>
                                </div>
                                <%--<div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <label class="lblFormItem">Actual End</label>
                                        </div>
                                        <div class="col-md-1">
                                            :
                                        </div>
                                        <div class="col-md-6">
                                            <label class="labelfontbold" id="ActualEnd_Capture"></label>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                            <div class="form-group">
                                <div class="col-md-12">
                                    <div class="invScrool" style="width: 100%; overflow: auto;">
                                        <table class=" table-striped  dataTables-example" id="tblCaptureList"></table>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
                <div class="modal-footer">
                    <input type="hidden" value="0" id="CCM_TRN_CycleCount_ID_Capture" class="fieldToGet" />
                    <input type="hidden" value="0" id="hidtable" class="fieldToGet" />
                    <button type="button" class="btn btn-primary" data-dismiss="modal" style="color:#fff !important;"> <%= GetGlobalResourceObject("Resource", "Close")%></button>
                    <button type="button" class="btn btn-primary" style="display:none !important;" id="btnCapture" onclick="return UpsertCaptureData();"><%= GetGlobalResourceObject("Resource", "Save")%> </button>
                </div>
            </div>
        </div>
    </div>
    <!-- ========================= END Modal Popup For Cycle Count Capture Details ========================================== -->


    <div id="modalConfirmYesNo" class="modal fade" aria-hidden="true" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" 

                class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
                <h4 id="lblTitleConfirmYesNo" class="modal-title"><%= GetGlobalResourceObject("Resource", "Confirmation")%></h4>
            </div>
            <div class="modal-body text-left">
              
                <span style="font-size:larger;"><p id="lblMsgConfirmYesNo"></p></span>
                <br /><br /><br />
                 <div style="color:red;font-size:small;"> <%= GetGlobalResourceObject("Resource", "NOTEReleasingalocationwillreleasethelockonthebinandconfirmtheinventory")%></div><p></p>
                <br /><br />
                 <div>
                   <%= GetGlobalResourceObject("Resource", "Doyouwishtocontinue")%> 
                </div>
            </div>
            <div class="modal-footer">               
                <button id="btnYesConfirmYesNo" 

                type="button" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Yes")%> <i class="fa fa-check" aria-hidden="true"></i></button>
                <button id="btnNoConfirmYesNo" 

                type="button" class="btn btn-primary" style="color:white !important;"><%= GetGlobalResourceObject("Resource", "No")%> <i class="fa fa-remove" aria-hidden="true"></i></button>
            </div>
        </div>
    </div>
</div>
    <br />
    <br />
    <br />

    <br /><p></p>

    <script type="text/javascript">
        var TotalDataList = [];
        var TotalTableList = [];
        var ItemList = null;
        var Isblock = 0;



        var TotalList = [];
        //var countList = [];
        var sno = 0;
        var capsno = 0;
        var countid = 0;
        var cid = 0;
        var countlist = [];
        var foundqty = 0;
        var lostqty = 0;
        var availableqty = 0;
        var capturelength = 0;
        var accountid = 0;
        var Locationdata = [];
        var Isblock = 0;
        var BlockedLocations = "";
        var BlockedLocationIds = "";
        var LocationForPupup = "";
        var duration;
        var MaterialData = [];
        var AccountCycleCountID = 0;
        var TotalCountList = [];

        $(document).ready(function () {

            //$("#PlannedStart").datepicker({
            //    todayBtn: 1,
            //    singleDatePicker: true,
            //    showDropdowns: true,
            //    autoclose: true,
            //    forceParse: false,
            //    format: "dd-M-yyyy",
            //    startDate: "today",
            //}).on('changeDate', function (selected) {
            //    var stDate = new Date(selected.date.valueOf());
            //    var enddt = addDays(stDate, (duration - 1));
            //    var formatdate = formatDate(enddt);
            //    $('#PlannedEnd').val(formatdate);
            //});

        });

        function addDays(theDate, days) {
            return new Date(theDate.getTime() + days * 24 * 60 * 60 * 1000);
        }

        function GetAllCycleCountList() {
            $("#divLoading").show();
            var data = "{'AccountID' : '" + <%=this.cp.AccountID%> +"'}";
            InventraxAjax.AjaxResultExecute("CycleCountTransaction.aspx/GetCycleCountTransactionListByStatus", data, 'GetListOnSuccess', 'GetListOnError', null);
        }
        GetAllCycleCountList();

        function GetAllCycleCountListByStatus() {
            var data = "{'AccountID' : '" + <%=this.cp.AccountID%> +"'}";
            InventraxAjax.AjaxResultExecute("CycleCountTransaction.aspx/GetCycleCountTransactionList", data, 'GetListOnSuccess', 'GetListOnError', null);
        }

        function GetListOnSuccess(data) {

            var dataList = JSON.parse(data.Result);
            ItemList = dataList;
            LoadGrid(dataList.Table);
            SetTableSettings();
            $("#divLoading").hide();
        }


        var dataList = null;
        function LoadGrid(Obj) {
            //debugger;
            if (Obj != null) {
                dataList = Obj;//.Table;

                $("#tblList").empty();
                $("#tblList").append("<thead><tr class='stripedhed'><th class='text-center' style='width:5% !important;'><%= GetGlobalResourceObject("Resource", "SNo")%> </th><th class='text-center' hidden><%= GetGlobalResourceObject("Resource", "CycleCount")%></th><th class='text-center'><%= GetGlobalResourceObject("Resource", "CycleCountName")%></th><th number><%= GetGlobalResourceObject("Resource", "SequenceNo")%></th><th><%= GetGlobalResourceObject("Resource", "PlannedStartDate")%> </th><th class='text-center'><%= GetGlobalResourceObject("Resource", "PlannedEndDate")%></th><th class='text-center'><%= GetGlobalResourceObject("Resource", "InitiatedTimeStamp")%></th><th class='text-center'><%= GetGlobalResourceObject("Resource", "CompletedTimeStamp")%></th><th class='text-center'><%= GetGlobalResourceObject("Resource", "Status")%></th><th class='text-center'><%= GetGlobalResourceObject("Resource", "Action")%></th></tr></thead><tbody>");
                for (var i = 0; i < dataList.length; i++) {
                    var statusname = "";
                    var capsatus = "";
                    var date = new Date();
                    if (dataList[i].StatusName == "Planned") {
                        statusname = "Initiate";

                        var configData = $.grep(ItemList.Table2, function (a) { return a.CCM_CNF_AccountCycleCount_ID == dataList[i].CCM_CNF_AccountCycleCount_ID });

                        if (configData != null && configData.length > 0) {
                            if (date >= new Date(dataList[i].PlannedStartDate)) {
                                button = "<button type='button' id='btnStatus' class='btn btn-primary btn-smss' data-toggle='confirmation' data-popout='true' onclick='UpdateInitiatedtime(" + dataList[i].CCM_TRN_CycleCount_ID + "," + dataList[i].CCM_CNF_AccountCycleCount_ID + ")'>" + statusname + "</button>&emsp;";
                            }
                            else {
                                button = "&emsp;";
                            }
                        }
                        else {
                            button = "&emsp;";
                        }
                        $("#tblList").append("<tr><td class='text-right'>" + (i + 1) + "</td><td class='text-left' hidden>" + dataList[i].CycleCountName + "</td><td class='text-left'>" + dataList[i].AccountCycleCountName + "</td><td number>" + dataList[i].SeqNo + "</td><td class='text-center'>" + getFormattedDate(dataList[i].PlannedStartDate) + "</td><td class='text-center'>" + getFormattedDate(dataList[i].PlannedEndDate) + "</td><td class='text-left'>" + dataList[i].InitiatedDate + "</td><td class='text-left'>" + dataList[i].CompletionDate + "</td><td class='text-left'>" + dataList[i].StatusName + "</td><td class='text-center'>" + button + "<a data-toggle='modal' data-target='#AddEntityToCreate' onclick='EditItem(" + dataList[i].CCM_TRN_CycleCount_ID + ");'><i class='material-icons text-navy'>mode_edit</i></a>&emsp; <a onclick='DeleteEntity(" + dataList[i].CCM_TRN_CycleCount_ID + ");'><i class='fa fa-trash text-navy hidden'></i></a></td></tr>");
                    }
                    else if (dataList[i].StatusName == "Initiated") {
                        statusname = "Complete";
                        //debugger;
                        //var item = GetCaptureItems(dataList[i].CCM_TRN_CycleCount_ID, dataList[i].CCM_CNF_AccountCycleCount_ID);
                        var item = $.grep(ItemList.Table1, function (a) { return a.CCM_TRN_CycleCount_ID == dataList[i].CCM_TRN_CycleCount_ID });
                        //if (item.Table != null && item.Table.length > 0) {
                        if (item != null && item.length > 0) {
                            capsatus = "Captured";
                        }
                        else {
                            capsatus = "Capture";
                        }
                        //capsatus = "Capture";
                        $("#tblList").append("<tr><td class='text-right'>" + (i + 1) + "</td><td class='text-left' hidden>" + dataList[i].CycleCountName + "</td><td class='text-left'>" + dataList[i].AccountCycleCountName + "</td><td number>" + dataList[i].SeqNo + "</td><td class='text-center'>" + getFormattedDate(dataList[i].PlannedStartDate) + "</td><td class='text-center'>" + getFormattedDate(dataList[i].PlannedEndDate) + "</td><td class='text-left'>" + dataList[i].InitiatedDate + "</td><td class='text-left'>" + dataList[i].CompletionDate + "</td><td class='text-left'>" + dataList[i].StatusName + "</td><td class='text-center'><button type='button' id='btnStatus' class='btn btn-primary btn-smss' onclick='UpdateCompltedtime(" + dataList[i].CCM_TRN_CycleCount_ID + ")'>" + statusname + "</button>&emsp;<button type='button' data-toggle='modal' data-target='#AddCaptureToCreate' id='btnCapture' class='btn btn-primary btn-smss' onclick='CaptureItem(" + dataList[i].CCM_TRN_CycleCount_ID + "," + dataList[i].CCM_CNF_AccountCycleCount_ID + ")'>" + capsatus + "</button>&emsp;<a style='cursor:pointer !important;' onclick='GoToLocationData(" + dataList[i].CCM_TRN_CycleCount_ID + ","+ dataList[i].CCM_CNF_AccountCycleCount_ID +")'><i class='material-icons text-navy'>open_in_new</i><em style='left: -115px;' class='sugg-tooltis'>Blocked Locations</em></i></a>&emsp;<a style='cursor:pointer !important;' onclick='GoToReport(" + dataList[i].CCM_TRN_CycleCount_ID + ")'><i class='material-icons'>remove_red_eye</i><em class='sugg-tooltis' style='left: -115px;'>Cycle Count Report</em></a>&emsp;<a><i class='material-icons text-navy hidden'>mode_edit</i></a>&emsp; <a><i class='fa fa-trash text-navy hidden'></i></a></td></tr>");
                    }
                    else {
                        $("#tblList").append("<tr><td class='text-right'>" + (i + 1) + "</td><td class='text-left' hidden>" + dataList[i].CycleCountName + "</td><td class='text-left'>" + dataList[i].AccountCycleCountName + "</td><td number>" + dataList[i].SeqNo + "</td><td class='text-center'>" + getFormattedDate(dataList[i].PlannedStartDate) + "</td><td class='text-center'>" + getFormattedDate(dataList[i].PlannedEndDate) + "</td><td class='text-left'>" + dataList[i].InitiatedDate + "</td><td class='text-left'>" + dataList[i].CompletionDate + "</td><td class='text-left'>" + dataList[i].StatusName + "</td><td class='text-center'><a style='cursor:pointer !important;' onclick='GoToReport(" + dataList[i].CCM_TRN_CycleCount_ID + ")'><i class='material-icons'>remove_red_eye</i><em class='sugg-tooltis'>View</em></a></td></tr>");
                    }
                }
                $("#tblList").append("</tbody>");
                SetTableSettings();
            }
            else {
                $("#tblList").empty();
                $("#tblList").append("<thead><tr><th colspan='10' class='text-right' style='background-color: white'><div class='text-right'><button type='button' class='btn btn-primary' data-toggle='modal' data-target='#AddEntityToCreate' onclick='createMaster();'>Add <i class='fa fa-plus' aria-hidden='true'></i></button></div></th></tr><tr><th class='text-center'>S.No </th><th class='text-center' hidden>Cycle Count</th><th class='text-center'>Account Cycle Count</th><th class='text-center'>Sequence No.</th><th class='text-center'>PSD</th><th class='text-center'>PED</th><th class='text-center'>Initiated Date</th><th class='text-center'>CompletionDate</th><th class='text-center'>Status</th><th class='text-center'>Action</th></tr><tr><th colspan='10' class='text-center' style='background-color: white'><p style='text-align: center'><strong>No Data</strong></p></th></tr></thead><tbody>");
                $("#tblList").append("</tbody>");
            }
        }

  function EditItem(id) {
            // debugger;
            var item = $.grep(ItemList.Table, function (a) { return a.CCM_TRN_CycleCount_ID == id });
            BuildCycleCountFormtoEdit(item);
        }

        function BuildCycleCountFormtoEdit(item) {

            $('.fieldToGet').each(function () {
                var fieldID = $(this).attr('id');
                var paramtype = $(this).attr('type');
                $('#' + fieldID).val(item[0][fieldID]);
                if (paramtype == "checkbox") {
                    $('#' + fieldID).attr(item[0][fieldID] == true ? 'checked' : 'unchecked');
                }
                else {
                    $('#' + fieldID).val(item[0][fieldID]);
                }
            });
            $("#CCM_MST_CycleCount_ID").text(item[0].CycleCountName);
            $("#CCM_CNF_AccountCycleCount_ID").text(item[0].AccountCycleCountName);
            $("#SeqNo").text(item[0].SeqNo);
            var PlannedEnd = getFormattedDate(item[0].PlannedEndDate);
            $('#PlannedEnd').val(PlannedEnd);
            var PlannedStart = getFormattedDate(item[0].PlannedStartDate);
            $('#PlannedStart').val(PlannedStart);

            $("#CycleCountDuration").text(item[0].CycleCountDuration);
            $("#ValidFrom").text(item[0].ValidFrom);
            $("#ValidThru").text(item[0].ValidThru);
            duration = item[0].CycleCountDuration;
            $("#CycleTimeInDays").text(item[0].CycleTimeInDays);

            $("#PlannedStart").datepicker({
                todayBtn: 1,
                singleDatePicker: true,
                showDropdowns: true,
                autoclose: true,
                forceParse: false,
                format: "dd-M-yyyy",
                //startDate: "today",
                startDate: PlannedStart,
                //endDate: item[0].ValidThru,
            }).on('changeDate', function (selected) {
                var stDate = new Date(selected.date.valueOf());
                var enddt = addDays(stDate, (duration - 1));
                var formatdate = formatDate(enddt);
                $('#PlannedEnd').val(formatdate);
                });
            $('#PlannedStart').data('datepicker').setDate(PlannedStart);
        }

        function GetCycleCountFormData() {

            var fieldDataOut = '{';
            var fieldData = '<root><data>';
            $('.fieldToGet').each(function () {
                var param = $(this).attr('id');
                var val = $(this).val().trim();
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

            var PEDDate = $("#PlannedEnd").val();
            if (PEDDate != "") {
                PEDDate = formatDateTime($("#PlannedEnd").val());
            }
            else {
                var date = new Date();
                PEDDate = date;
            }

            fieldData += '<CreatedBy>' + $('#hdnCreatedBy').val() + '</CreatedBy>' + '<UpdatedBy>' + $('#hdnUpdatedBy').val() + '</UpdatedBy>' + '<CreatedOn>' + $('#hdnUpdatedOn').val() + '</CreatedOn>' + '<UpdatedOn>' + $('#hdnUpdatedOn').val() + '</UpdatedOn>' + '<NewCCM_TRN_CycleCount_ID>' + 0 + '</NewCCM_TRN_CycleCount_ID>';
            fieldData += '<PlannedStart>' + formatDateTime($("#PlannedStart").val()) + '</PlannedStart>' + '<PlannedEnd>' + PEDDate + '</PlannedEnd>' + '<CCM_MST_CycleCountStatus_ID>' + 1 + '</CCM_MST_CycleCountStatus_ID>';
            fieldData = fieldData + '</data></root>';
            fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'UpdatedBy' + '":"' + <%=this.cp.UserID%> + '",' + '"' + String.fromCharCode(64) + 'CCM_TRN_CycleCount_ID' + '":"' + $('#CCM_TRN_CycleCount_ID').val() + '",';
            fieldDataOut = fieldDataOut.substring(0, fieldDataOut.length - 1);
            fieldDataOut += '}';
            return fieldDataOut;
        }

        function UpsertData() {

            if (ValidateCycleCountData()) {
                $("#divLoading").show();                
                var param = JSON.stringify({ "Sp_Set": "", "JSON": JSON.stringify(GetCycleCountFormData()) });
                $.ajax({
                    url: "CycleCountTransaction.aspx/MasterDetailsSet",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    cache: false,
                    data: param,
                    success: function (response) {
                        var result = response;
                        if ($('#btnCreate').text() == "Update") {
                            showStickyToast(true, 'Cycle Count Transaction Details Updated Successfully', false);
                            setTimeout(function () {
                                location.reload();
                            }, 2000);
                        }
                        else {
                            showStickyToast(true, 'Cycle Count Transaction Details Created Successfully', false);
                            setTimeout(function () {
                                location.reload();
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

        function GetInitiatedtimeData(id) {

            var date = new Date();
            var fieldData = '{';
            fieldData += '"' + String.fromCharCode(64) + 'CCM_TRN_CycleCount_ID":"' + TotalList[0].CCM_TRN_CycleCount_ID + '",';
            fieldData += '"' + String.fromCharCode(64) + 'CCM_CNF_AccountCycleCount_ID":"' + id + '",';
            fieldData += '"' + String.fromCharCode(64) + 'LoggedInUserID":"' + $("#hdnUpdatedBy").val().trim() + '",';
            fieldData += '"' + String.fromCharCode(64) + 'InitiatedTimestamp":"' + formatDateTime(date) + '",';
            fieldData = fieldData.substring(0, fieldData.length - 1);
            fieldData += '}';
            return fieldData;
        }

        function UpdateInitiatedtime(cyclecountid, AccountCycleCountId) {
            debugger;
            $("#divLoading").show();
            Isblock = 1;
            var item = $.grep(ItemList.Table, function (a) { return a.CCM_TRN_CycleCount_ID == cyclecountid });
            TotalList = item;

            var param = JSON.stringify({ "SP_Name": "", "JSON": JSON.stringify(GetInitiatedtimeData(AccountCycleCountId)) });
            $.ajax({
                url: "CycleCountTransaction.aspx/GetSuccessInfoLocationFilter",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                //async: false,//Commented by M.D.Prasad on 02-Apr-2020
                data: param,
                success: function (response) {
                    //alert();
                    showStickyToast(true, 'Initiated Successfully', false);
                    $("#divLoading").hide();
                    setTimeout(function () {
                        location.reload();
                    }, 1500);
                    ///Locationidsdata = JSON.parse(response.d).Table;

                },
                failure: function (errMsg) {

                }
            });
        }

        function GETAllLocationList(id) {

            var fieldData = '{';
            fieldData += '"' + String.fromCharCode(64) + 'CCM_CNF_AccountCycleCount_ID":"' + id + '",';
            fieldData += '"' + String.fromCharCode(64) + 'LoggedInUserID":"' + $("#hdnUpdatedBy").val().trim() + '",';
            fieldData = fieldData.substring(0, fieldData.length - 1);
            fieldData += '}';
            return fieldData;
        }

        function GetAllAccountCycleCountLocationList(id) {

            var Locationidsdata;
            var param = JSON.stringify({ "SP_Name": "", "JSON": JSON.stringify(GETAllLocationList(id)) });
            $.ajax({
                url: "CycleCountTransaction.aspx/GetSuccessInfoLocationFilter",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                async: false,
                data: param,
                success: function (response) {
                    //alert();

                    Locationidsdata = JSON.parse(response.d).Table;

                },
                failure: function (errMsg) {

                }
            });
            return Locationidsdata;
        }
        var CaptureItems = null;
        function GetCaptureItems(CID, ACID) {
            var fieldData = '{';
            fieldData += '"' + String.fromCharCode(64) + 'CID":"' + CID + '",';
            fieldData += '"' + String.fromCharCode(64) + 'AccountID":"' + <%=this.cp.AccountID%> + '",';
            fieldData += '"' + String.fromCharCode(64) + 'CCM_CNF_AccountCycleCount_ID":"' + ACID + '",';
            fieldData = fieldData.substring(0, fieldData.length - 1);
            fieldData += '}';

            var param = JSON.stringify({ "SP_Name": "", "JSON": JSON.stringify(fieldData) })
            $.ajax({
                url: "CycleCountTransaction.aspx/GetSuccessInfoCapture",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
               // async: false,
                data: param,
                success: function (response) {
                    debugger
                    CaptureItems = JSON.parse(response.d);
                    
                    BuildCaptureFormtoEdit(CaptureItems.Table1, CaptureItems.Table2);
                    LoadCaptureGrid(CaptureItems.Table);
                    //AddNewRow();
                    $("#divLoading").hide();
                    //$('inv-preloader').hide();
                },
                failure: function (errMsg) {
                }
            });
            return CaptureItems;
        }



        function CaptureItem(id, Acntid) {
            //debugger;
            //$('inv-preloader').show();
            $("#divLoading").show();
            availableqty = 0;
            $("#divCaptureValidationMessages").empty();
            countid = id;
            sno = 0;
            GetCaptureItems(id, Acntid);
            //var Captureitem = GetCaptureItems(id, Acntid);
            //BuildCaptureFormtoEdit(Captureitem.Table1, Captureitem.Table2);
            //LoadCaptureGrid(Captureitem.Table);
            //AddNewRow();
        }

        function BuildCaptureFormtoEdit(item,items) {
            //debugger;
            AccountCycleCountID = item[0].CCM_CNF_AccountCycleCount_ID;
            $("#CCM_MST_CycleCount_ID_Capture").text(item[0].CycleCountName);
            $("#CCM_CNF_AccountCycleCount_ID_Capture").text(item[0].AccountCycleCountName);
            $("#SeqNo_Capture").text(item[0].SeqNo);
            $('#PlannedEnd_Capture').text(getFormattedDate(item[0].PlannedEndDate));
            $('#PlannedStart_Capture').text(getFormattedDate(item[0].PlannedStartDate));
            $("#PlannedQuantity").text(item[0].PlannedQuantity);
            var ActualStartDate = formatDateTimeAmPm(item[0].ActualTimeStart);
            $("#ActualStart_Capture").text(item[0].ActualTimeStart);

            if (item[0].IsBlindCycleCount == true) {
                $(".IsBlind").css("display", "none");
            }
            else {
                $(".IsBlind").css("display", "block");
                if (items[0].Quantity != null) {
                    $("#AvailableQuantity").text(items[0].Quantity);
                }
            }
        }
        var srno = 0;
        function LoadCaptureGrid(item) {

            $("#tblCaptureList").empty();
            $("#tblCaptureList").append("<thead><tr><th class='text-center'>S.No </th><th class='text-center'>Location</th><th class='text-center' style='display:none;'>Container</th><th class='text-center'>Material</th><th class='text-center'>SLoc.</th><th class='text-center'>Batch No.</th><th class='text-center'>Serial No.</th><th class='text-center'>Mfg. Date</th><th class='text-center'>Exp. Date</th><th class='text-center'>Project Ref. No.</th><th class='text-center'>MRP</th><th class='text-right' style='text-align:right !important;'>Quantity</th></tr></thead><tbody></tbody>");
            if (item != null && item.length > 0) {
                srno = item.length;
                var data = item;
                for (var k = 0; k < data.length; k++) {
                    $("#tblCaptureList").append("<tr><td class='text-right'>" + (k + 1) + "</td><td class='text-left'>" + data[k].Location + "</td><td class='text-left' style='display:none;'>" + data[k].ContainerCode + "</td><td class='text-left'>" + data[k].Material_Name + "</td><td class='text-left'>" + data[k].Code + "</td><td class='text-left'>" + data[k].BatchNo + "</td><td class='text-right'>" + data[k].SerialNo + "</td><td class='text-right'>" + data[k].MfgDate + "</td><td class='text-right'>" + data[k].ExpDate + "</td><td class='text-right'>" + data[k].ProjectRefNo + "</td><td class='text-right'>" + data[k].MfgDate + "</td><td class='text-right' style='text-align:right !important;'>" + data[k].Quantity + "</td></tr>");
                }
            }
        }

        function AddNewRow() {
            //debugger;
            sno = sno + 1;       //(srno + sno)     
            $("#tblCaptureList").append("<tr><td class='text-right'>" + (srno + sno) + "</td><td class='text-left' style='padding:0;width:12%;'> <input id='LocationID" + sno + "' onkeypress='onkeypressAddNewRow(" + sno + ")' type='text' class='typeahead_3' style='width:100%;border:0px;height:35px;padding-left: 8px;background:none;' placeholder='Enter Location' /><lable id='lblLocationid" + sno + "' style='display:none;'>0</lable></td><td class='text-left' style='padding:0;width:12%;'> <input id='CartonID" + sno + "' onkeypress='onkeypressAddNewRow(" + sno + ")' type='text' class='typeahead_3' style='width:100%;border:0px;height:35px;padding-left: 8px;background:none;' placeholder='Enter Container' /><lable id='lblContainerid" + sno + "' style='display:none;'>0</lable></td><td style='padding:0;width:12%;' class='text-left'><input id='MM_MST_Material_ID" + sno + "' onkeyup='onKeyPressHideControls(" + sno + ")' onkeypress='onkeypressAddNewRow(" + sno + ")' type='text' class='typeahead_3' style='width:100%;border:0px;height:35px;padding-left: 8px;background:none;' placeholder='Enter Material'/><label id='lblMaterialId" + sno + "' style='display:none;'>0</label></td><td class='text-center' style='padding:0;width:12%;'><input class='' id='selBatchNo" + sno + "' type='text'  onkeypress='onkeypressAddNewRow(" + sno + ")' style='width:100%;border:0px;height:35px;padding-left: 8px;background:none;' placeholder=''/></td><td class='text-center' style='padding:0;width:12%;'><input class='' id='txtSerialNo" + sno + "' type='text' onkeypress='onkeypressAddNewRow(" + sno + ")' style='width:100%;border:0px;height:35px;padding-left: 8px;background:none;' placeholder=''/></td><td class='text-center' style='padding:0;width:12%;'><input class='DueDate' id='txtMfgDate" + sno + "' type='text'  onkeypress='onkeypressAddNewRow(" + sno + ")' style='width:100%;border:0px;height:35px;padding-left: 8px;background:none;' placeholder='' readonly='true'/></td><td class='text-center' style='padding:0;width:12%;'><input class='DueDate' id='txtExpDate" + sno + "' type='text' onkeypress='onkeypressAddNewRow(" + sno + ")' style='width:100%;border:0px;height:35px;padding-left: 8px;background:none;' placeholder='' readonly='true'/></td><td class='text-center' style='padding:0;width:12%;'><input class='' id='txtProjectRefNo" + sno + "' type='text' onkeypress='onkeypressAddNewRow(" + sno + ")' style='width:100%;border:0px;height:35px;padding-left: 8px;background:none;' placeholder='' /></td><td class='text-center' style='padding:0;width:12%;'><input class='' id='txtMRP" + sno + "' type='text' onkeypress='onkeypressAddNewRow(" + sno + ")' style='width:100%;border:0px;height:35px;padding-left: 8px;background:none;' placeholder='' /></td><td class='text-center' style='padding:0;width:12%;'><input class='' id='Quantity" + sno + "' type='text' onkeypress='onkeypressAddNewRow(" + sno + ")' style='width:100%;border:0px;height:35px;padding-left: 8px;background:none;width:125px !important;' placeholder='Enter Quantity' oncut='return false;' oncopy='return false;' onpaste='return false;' onkeypress = 'return isNumberKey(event)'/></td></tr>");
            GetLocations(sno);
            GETMaterials(sno);

            $("#txtMfgDate" + sno).datepicker({
                format: "dd-M-yyyy",
                maxDate: new Date(),
                endDate: "today",
                autoclose: true,
                forceParse: false,
            }).on('changeDate', function (selected) {
                //debugger;
                var stDate = new Date(selected.date.valueOf());
                var enddt = addDays(stDate, 1);
                var expDate = formatDate(enddt);
                $("#txtExpDate" + (sno - 1)).datepicker('setStartDate', enddt);
            });;

            $("#txtExpDate" + sno).datepicker({
                format: "dd-M-yyyy",
                maxDate: new Date(),
                autoclose: true,
                forceParse: false,
            }).on('changeDate', function (selected) {
            });
        }

        function onkeypressAddNewRow(idSno) {
            if (sno == idSno)
                AddNewRow();
        }

        function GetBlockLocations(id) {
            //debugger;
            var fieldData = '{';
            fieldData += '"' + String.fromCharCode(64) + 'CCM_CNF_AccountCycleCount_ID":"' + id + '",';
            fieldData += '"' + String.fromCharCode(64) + 'CCM_TRN_CycleCount_ID":"' + cyclecountid + '",';
            fieldData += '"' + String.fromCharCode(64) + 'LoggedInUserID":"' + $("#hdnUpdatedBy").val().trim() + '",';
            fieldData = fieldData.substring(0, fieldData.length - 1);
            fieldData += '}';
            var param = JSON.stringify({ "SP_Name": "", "JSON": JSON.stringify(fieldData) });
            $.ajax({
                url: "CycleCountTransaction.aspx/GetSuccessInfoFilter",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                async: false,
                data: param,
                success: function (response) {
                    //alert();

                    Locationdata = JSON.parse(response.d).Table;

                },
                failure: function (errMsg) {

                }
            });
            return Locationdata;
        }
        function GetLocations(id) {
            var cycletrnlist = $.grep(ItemList.Table, function (a) { return a.CCM_TRN_CycleCount_ID == countid });
            accountid = cycletrnlist[0].CCM_CNF_AccountCycleCount_ID;
            cyclecountid = cycletrnlist[0].CCM_TRN_CycleCount_ID;
            var LocationList = GetAccountCycleCountLocationList();
            if (LocationList != null && LocationList.length > 0) {
                LocationList = LocationList;
            }
            else {
                //ItemList.Table4 = ItemList.Table4;
            }

            $("#LocationID" + id).typeahead({
                source: JSON.parse(JSON.stringify(LocationList)),
                afterSelect: function (data) {
                    $("#lblLocationid" + id).text(data.code);
                    $("#txtMfgDate" + id).val("");
                    $("#txtExpDate" + id).val("");
                    var containerlist = GetAllContainers(data.code);
                    $("#CartonID" + id).typeahead({
                        source: JSON.parse(JSON.stringify(containerlist)),
                        afterSelect: function (cdata) {
                            $("#lblContainerid" + id).text(cdata.code);
                        }
                    });
                }
            });
        }
        function GETLocationList() {
            var fieldData = '{';
            fieldData += '"' + String.fromCharCode(64) + 'CCM_CNF_AccountCycleCount_ID":"' + accountid + '",';
            fieldData += '"' + String.fromCharCode(64) + 'CCM_TRN_CycleCount_ID":"' + cyclecountid + '",';
            fieldData += '"' + String.fromCharCode(64) + 'LoggedInUserID":"' + $("#hdnUpdatedBy").val().trim() + '",';
            fieldData = fieldData.substring(0, fieldData.length - 1);
            fieldData += '}';
            return fieldData;
        }

        var Containers = null;
        function GetAllContainers(LocID) {
            // debugger;
            var fieldData = '';
            fieldData = '{';
            fieldData += '"' + String.fromCharCode(64) + 'LocationID":"' + LocID + '",';
            fieldData = fieldData.substring(0, fieldData.length - 1);
            fieldData += '}';

            var param = JSON.stringify({ "SP_Name": "", "JSON": JSON.stringify(fieldData) })
            $.ajax({
                url: "CycleCountTransaction.aspx/GetSuccessInfoContainers",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                async: false,
                data: param,
                success: function (response) {
                    Containers = JSON.parse(response.d).Table;
                },
                failure: function (errMsg) {
                }
            });
            return Containers;
        }


        function GetAccountCycleCountLocationList() {
            var param = JSON.stringify({ "SP_Name": "", "JSON": JSON.stringify(GETLocationList()) });
            $.ajax({
                url: "CycleCountTransaction.aspx/GetSuccessInfoFilter",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                async: false,
                data: param,
                success: function (response) {
                    //debugger;
                    Locationdata = JSON.parse(response.d).Table;
                    MaterialData = JSON.parse(response.d);
                },
                failure: function (errMsg) {

                }
            });
            return Locationdata;
        }


        function GETMaterials(id) {
            debugger;
            $("#MM_MST_Material_ID" + id).typeahead({
                source: JSON.parse(JSON.stringify(MaterialData.Table1)),
                afterSelect: function (data) {
                    $("#lblMaterialId" + id).text(data.code);
                    $("#txtMfgDate" + id).val("");
                    $("#txtExpDate" + id).val("");
                }
            });
        }

        function arrUnique(arr) {
            var a = [];
            for (var i = 0, l = arr.length; i < l; i++)
                if (a.indexOf(arr[i]) === -1 && arr[i] !== '')
                    a.push(arr[i]);
            return a;
        }


        function onKeyPressHideControls(idSno) {

        }


        var GoodsMovementData = "";
        var GoodsMovementOutData = "";
        function GetCaptureData() {

            BlockedLocationIds = '';
            // debugger;
            var qty = 0;
            var lqty = 0;
            var fqty = 0;
            var avbqty = 0;
            var LostandFoundQty = 0;
            if (TotalCountList != null && TotalCountList.length > 0) {
                var item = $.grep(TotalCountList, function (a) { return a.CCM_TRN_CycleCount_ID == countid });
            }
            else {
                var item = $.grep(ItemList.Table, function (a) { return a.CCM_TRN_CycleCount_ID == countid });
            }

            GoodsMovementOutData = '{';
            GoodsMovementData = '<GoodsMovements>';

            var fieldDataOut = '{';
            var fieldData = '<root>';
            for (i = 1; i <= sno - 1; i++) {

                fieldData += '<data>'
                fieldData += '<CCM_TRN_CycleCountInventory_ID>' + 0 + '</CCM_TRN_CycleCountInventory_ID>';
                fieldData += '<CCM_TRN_CycleCount_ID>' + item[0].CCM_TRN_CycleCount_ID + '</CCM_TRN_CycleCount_ID>';
                fieldData += '<CCM_CNF_AccountCycleCount_ID>' + item[0].CCM_CNF_AccountCycleCount_ID + '</CCM_CNF_AccountCycleCount_ID>';
                fieldData += '<CCM_MST_CycleCount_ID>' + item[0].CCM_MST_CycleCount_ID + '</CCM_MST_CycleCount_ID>';


                fieldData += '<MaterialMasterID>' + $("#lblMaterialId" + i).text() + '</MaterialMasterID>';
                fieldData += '<LocationID>' + $("#lblLocationid" + i).text() + '</LocationID>';
                fieldData += '<ContainerID>' + $("#lblContainerid" + i).text() + '</ContainerID>';

                fieldData += '<Entity_ID>' + 6 + '</Entity_ID>';

                fieldData += '<BatchNo>' + $("#selBatchNo" + i).val() + '</BatchNo>';
                fieldData += '<MfgDate>' + $("#txtMfgDate" + i).val() + '</MfgDate>';
                fieldData += '<ExpDate>' + $("#txtExpDate" + i).val() + '</ExpDate>';
                fieldData += '<SerialNo>' + $("#txtSerialNo" + i).val() + '</SerialNo>';
                fieldData += '<ProjectRefNo>' + $("#txtProjectRefNo" + i).val() + '</ProjectRefNo>';
                fieldData+='<MRP>'+$("#txtMRP" + i).val() +'</MRP>'

                lqty = item[0].LostQuantity;
                fqty = item[0].FoundQuantity;
                avbqty = item[0].AvailableQuantity;

                if (lqty == 0 || lqty == null || lqty == undefined || lqty == "") {
                    lostqty += 0;
                }
                else {
                    lostqty += item[0].LostQuantity;
                }

                if (fqty == 0 || fqty == null || fqty == undefined || fqty == "") {
                    foundqty += 0;
                }
                else {
                    foundqty += item[0].FoundQuantity;
                }

                if (avbqty == 0 || avbqty == null || avbqty == undefined || avbqty == "") {
                    availableqty += 0;
                }
                else {
                    availableqty += item[0].AvailableQuantity;
                }

                var TotalQty = 0;
                var slocInventory = GetLocationsCaptureAVS($("#lblLocationid" + i).text(), $("#lblMaterialId" + i).text(), $("#selBatchNo" + i).val(), $("#txtMfgDate" + i).val(), $("#txtExpDate" + i).val(), $("#txtProjectRefNo" + i).val(), $("#lblContainerid" + i).text());
                for (var q = 0; q < slocInventory.length; q++) {
                    TotalQty += slocInventory[q].TotalQuantity;
                }

                if (TotalQty < parseInt($("#Quantity" + i).val())) {
                    foundqty += parseInt($("#Quantity" + i).val()) - TotalQty;
                    lostqty += 0;
                }
                else if (TotalQty > parseInt($("#Quantity" + i).val())) {
                    lostqty += TotalQty - parseInt($("#Quantity" + i).val());
                    foundqty += 0;
                }
                else {
                    availableqty += parseInt($("#Quantity" + i).val());
                    foundqty += 0;
                    lostqty += 0;
                }
                availableqty += parseInt($("#Quantity" + i).val());

                fieldData += '<Quantity>' + $("#Quantity" + i).val() + '</Quantity>';
                fieldData += '<ActivityBy>' + $("#hdnUpdatedBy").val() + '</ActivityBy>';
                fieldData += '<ActivityTimestamp>' + $('#hdnUpdatedOn').val() + '</ActivityTimestamp>';
                fieldData += '<IsActive>' + 1 + '</IsActive>';
                fieldData += '<IsDeleted>' + 0 + '</IsDeleted>';
                fieldData += '<CreatedBy>' + $('#hdnCreatedBy').val() + '</CreatedBy>' + '<CreatedOn>' + $('#hdnUpdatedOn').val() + '</CreatedOn>';
                fieldData += '<UpdatedBy>' + $('#hdnUpdatedBy').val() + '</UpdatedBy>' + '<UpdatedOn>' + $('#hdnUpdatedOn').val() + '</UpdatedOn>' + '<NewCCM_TRN_CycleCountInventory_ID>' + 0 + '</NewCCM_TRN_CycleCountInventory_ID>';
                fieldData += '</data>';

                LocationForPupup += "<div class='col-md-4'><li style='margin-top: 0.3em;'><span class='text-navy' style='font-size:14px;'>" + $("#LocationID" + i).val() + "</span></li></div>";
                BlockedLocations += $("#LocationID" + i).val() + ",";
                BlockedLocationIds += $("#lblLocationid" + i).text() + ",";

                var lostorfoundqty = 0;
                lostorfoundqty = Math.abs($("#Quantity" + i).val() - TotalQty);
                GoodsMovementData += '<GoodsMovement>';

                GoodsMovementData += '<TransactionDocID>' + item[0].CCM_TRN_CycleCount_ID + '</TransactionDocID>';
                GoodsMovementData += '<MaterialMasterID>' + $("#lblMaterialId" + i).text() + '</MaterialMasterID>';
                GoodsMovementData += '<LocationID>' + $("#lblLocationid" + i).text() + '</LocationID>';
                GoodsMovementData += '<Quantity>' + lostorfoundqty + '</Quantity>';
                GoodsMovementData += '<CreatedBy>' + $('#hdnCreatedBy').val() + '</CreatedBy>';
                GoodsMovementData += '<BatchNo>' + $("#selBatchNo" + i).val() + '</BatchNo>';
                GoodsMovementData += '<MfgDate>' + $("#txtMfgDate" + i).val() + '</MfgDate>';
                GoodsMovementData += '<ExpDate>' + $("#txtExpDate" + i).val() + '</ExpDate>';
                GoodsMovementData += '<SerialNo>' + $("#txtSerialNo" + i).val() + '</SerialNo>';
                GoodsMovementData += '<ProjectRefNo>' + $("#txtProjectRefNo" + i).val() + '</ProjectRefNo>';
                GoodsMovementData += '<PhysicalQuantity>' + $("#Quantity" + i).val() + '</PhysicalQuantity>';
                GoodsMovementData += '<LogicalQuantity>' + TotalQty + '</LogicalQuantity>';
                GoodsMovementData += '<CCM_CNF_AccountCycleCount_ID>' + item[0].CCM_CNF_AccountCycleCount_ID + '</CCM_CNF_AccountCycleCount_ID>';
                GoodsMovementData += '<CCM_MST_CycleCount_ID>' + item[0].CCM_MST_CycleCount_ID + '</CCM_MST_CycleCount_ID>';
                GoodsMovementData += '<ContainerID>' + $("#lblContainerid" + i).text() + '</ContainerID>';

                var OperationFlag = "";
                if (lostqty == 0 && foundqty == 0) {
                    OperationFlag = 0;
                }
                else if (lostqty == 0) {
                    OperationFlag = 4;
                }
                else {
                    OperationFlag = 3;
                }

                GoodsMovementData += '</GoodsMovement>';
            }

            BlockedLocationIds = BlockedLocationIds.substring(0, BlockedLocationIds.length - 1);
            GoodsMovementData = GoodsMovementData + '</GoodsMovements>';
            GoodsMovementOutData += '"' + String.fromCharCode(64) + 'DataXml' + '":"' + GoodsMovementData + '",' + '"' + String.fromCharCode(64) + 'OperationFlag' + '":"' + OperationFlag + '",' + '"' + String.fromCharCode(64) + 'UserLoggedId' + '":"' + $("#hdnUpdatedBy").val() + '"';
            GoodsMovementOutData += '}';


            fieldData = fieldData + '</root>';
            fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'UpdatedBy' + '":"' + <%=this.cp.UserID%> + '",' + '"' + String.fromCharCode(64) + 'CCM_TRN_CycleCountInventory_ID' + '":"' + 0 + '"';
            fieldDataOut += '}';
            return fieldDataOut;

        }


        var AvblStkLocs = null;
        function GetLocationsCaptureAVS(LID, MID, BatNo, MFGDT, EXPDT, PrjctNo, CrtnID) {
            var fieldData = '';
            fieldData = '{';
            fieldData += '"' + String.fromCharCode(64) + 'LocationID":"' + LID + '",';
            fieldData += '"' + String.fromCharCode(64) + 'MaterialMasterID":"' + MID + '",';
            fieldData += '"' + String.fromCharCode(64) + 'BatchNo":"' + BatNo + '",';
            fieldData += '"' + String.fromCharCode(64) + 'MfgDate":"' + MFGDT + '",';
            fieldData += '"' + String.fromCharCode(64) + 'ExpDate":"' + EXPDT + '",';
            fieldData += '"' + String.fromCharCode(64) + 'ProjectRefNo":"' + PrjctNo + '",';
            fieldData += '"' + String.fromCharCode(64) + 'CartonID":"' + CrtnID + '",';

            fieldData = fieldData.substring(0, fieldData.length - 1);
            fieldData += '}';

            var param = JSON.stringify({ "SP_Name": "", "JSON": JSON.stringify(fieldData) })
            $.ajax({
                url: "CycleCountTransaction.aspx/GetSuccessInfoLocations_Capture",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                async: false,
                data: param,
                success: function (response) {
                    AvblStkLocs = JSON.parse(response.d).Table;
                },
                failure: function (errMsg) {
                }
            });
            return AvblStkLocs;
        }


        function UpsertCaptureData() {

            debugger;
            if (validcapturedata()) {
                $("#divLoading").show();
                var param = JSON.stringify({ "Sp_Set": "", "JSON": JSON.stringify(GetCaptureData()) });
                $.ajax({
                    url: "CycleCountTransaction.aspx/MasterDetailsSet_NEW",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    cache: false,
                    data: param,
                    async: false,
                    success: function (response) {
                        // alert();
                       // debugger
                        var result = response;
                        showStickyToast(true, 'Saved Successfully', false);
                        //UpsetLostandFoundQty(countid); //=========== Commented By M.D.Prasad On 20-Apr-2020 For Optimize the Code ===============
                        var $confirm = $("#modalConfirmYesNo");
                        $confirm.modal('show');
                        $("#lblTitleConfirmYesNo").html("<i class='fa fa-exclamation-circle' aria-hidden='true'></i> Confirmation");
                        $("#lblMsgConfirmYesNo").html("Do you want to release the following locations : <br/><p></p><div class='row col-md-12'>" + LocationForPupup + "<br/></div>");

                        $("#btnYesConfirmYesNo").on('click').click(function () {

                            $("#divLoading").show();
                            GetCaptureDataList(countid, AccountCycleCountID);
                            UpsertStorageLocations();
                            $confirm.modal("hide");
                            setTimeout(function () {
                                $("#divLoading").hide();
                                location.reload();
                            }, 1000);
                        });
                        $("#btnNoConfirmYesNo").on('click').click(function () {
                            //
                            //noFn();
                            //debugger;
                            $confirm.modal("hide");
                            $("#divLoading").show();
                            GetCaptureDataList(countid, AccountCycleCountID);
                            $("#divLoading").hide();
                            //location.reload();
                        });
                        setTimeout(function () {
                            $("#divLoading").hide();
                        }, 1500);

                    },
                    failure: function (errMsg) {

                    }
                });

                //=========== Commented By M.D.Prasad On 20-Apr-2020 For Optimize the Code ===============//

                /*var $confirm = $("#modalConfirmYesNo");
                $confirm.modal('show');
                $("#lblTitleConfirmYesNo").html("<i class='fa fa-exclamation-circle' aria-hidden='true'></i> Confirmation");
                $("#lblMsgConfirmYesNo").html("Do you want to release the following locations : <br/><p></p><div class='row col-md-12'>" + LocationForPupup + "<br/></div>");

                $("#btnYesConfirmYesNo").on('click').click(function () {

                    $("#divLoading").show();
                    GetCaptureDataList(countid, AccountCycleCountID);
                    UpsertStorageLocations();                    
                    $confirm.modal("hide");
                    setTimeout(function () {
                        $("#divLoading").hide();
                        location.reload();
                    }, 1000);
                });
                $("#btnNoConfirmYesNo").on('click').click(function () {
                    //
                    //noFn();
                    //debugger;
                    $confirm.modal("hide");
                    $("#divLoading").show();
                    GetCaptureDataList(countid, AccountCycleCountID);
                    $("#divLoading").hide();
                    //location.reload();
                });*/
            }
            else {
                $("#divCaptureValidationMessages").show();
            }
        }

        function GetCaptureDataList(cid, Acid) {
            //debugger;
            var fieldData = '{';
            fieldData += '"' + String.fromCharCode(64) + 'CID":"' + cid + '",';
            fieldData += '"' + String.fromCharCode(64) + 'AccountID":"' + <%=this.cp.AccountID%> + '",';
            fieldData += '"' + String.fromCharCode(64) + 'CCM_CNF_AccountCycleCount_ID":"' + Acid + '",';
            fieldData = fieldData.substring(0, fieldData.length - 1);
            fieldData += '}';

            var param = JSON.stringify({ "SP_Name": "", "JSON": JSON.stringify(fieldData) })
            $.ajax({
                url: "CycleCountTransaction.aspx/GetSuccessInfoCapture",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                async: false,
                data: param,
                success: function (response) {
                    debugger;
                    CaptureList = JSON.parse(response.d).Table;
                    TotalCountList = JSON.parse(response.d).Table1;
                    var CaptureQuantity = JSON.parse(response.d).Table2;
                    srno = CaptureList.length;
                    sno = 0;
                    BlockedLocations = "";
                    LocationForPupup = "";

                    LoadCaptureGrid(CaptureList);
                    var item = $.grep(TotalCountList, function (a) { return a.CCM_TRN_CycleCount_ID == cid });
                    if (item[0].IsBlindCycleCount == true) {
                        $(".IsBlind").css("display", "none");
                    }
                    else {
                        $(".IsBlind").css("display", "block");
                        if (CaptureQuantity[0].Quantity != null) {
                            $("#AvailableQuantity").text("");
                            $("#AvailableQuantity").text(CaptureQuantity[0].Quantity);
                            availableqty = 0;
                            foundqty = 0;
                            lostqty = 0;
                        }
                    }

                    AddNewRow();
                },
                failure: function (errMsg) {
                }
            });
        }

        function UpsertStorageLocations() {
            var param = JSON.stringify({ "Sp_Set": "", "JSON": JSON.stringify(GetBlocked_Location()) })
            $.ajax({
                url: "CycleCountTransaction.aspx/MasterDetailsSetIsBlock",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                async: false,
                data: param,
                success: function (response) {
                },
                failure: function (errMsg) {
                }
            });
        }

        function GetBlocked_Location() {
            Isblock = 0;
            var date = new Date();
            var UTCtime = JSON.stringify(date);
            var fieldData = '{';
            fieldData += '"' + String.fromCharCode(64) + 'LocationIds":"' + BlockedLocationIds + '",';
            fieldData += '"' + String.fromCharCode(64) + 'LoggedInUserID":"' + $("#hdnUpdatedBy").val().trim() + '",';
            fieldData += '"' + String.fromCharCode(64) + 'IsBlockedForCycleCount":"' + Isblock + '",';
            fieldData = fieldData.substring(0, fieldData.length - 1);
            fieldData += '}';
            return fieldData;
        }


        function GetQtyData() {
            var date = new Date();
            var fieldDataOut = '{';
            var fieldData = '<root><data>';
            fieldData += '<CCM_TRN_CycleCount_ID>' + countid + '</CCM_TRN_CycleCount_ID>';
            fieldData += '<AvailableQuantity>' + availableqty + '</AvailableQuantity>';
            fieldData += '<LostQuantity>' + lostqty + '</LostQuantity>';
            fieldData += '<FoundQuantity>' + foundqty + '</FoundQuantity>';
            fieldData += '<UpdatedBy>' + $('#hdnUpdatedBy').val() + '</UpdatedBy>' + '<UpdatedOn>' + $('#hdnUpdatedOn').val() + '</UpdatedOn>' + '<NewCCM_TRN_CycleCount_ID>' + 0 + '</NewCCM_TRN_CycleCount_ID>';
            fieldData = fieldData + '</data></root>';
            fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'UpdatedBy' + '":"' + <%=this.cp.UserID%> + '",' + '"' + String.fromCharCode(64) + 'CCM_TRN_CycleCount_ID' + '":"' + countid + '",';
            fieldDataOut = fieldDataOut.substring(0, fieldDataOut.length - 1);
            fieldDataOut += '}';
            return fieldDataOut;
        }

        function UpsetLostandFoundQty(cpid) {
            debugger
            var param = JSON.stringify({ "Sp_Set": "", "JSON": JSON.stringify(GetQtyData()) });
            $.ajax({
                url: "CycleCountTransaction.aspx/MasterDetailsSetLF",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                data: param,
                async: false,
                success: function (response) {
                    var result = response;
                },
                failure: function (errMsg) {

                }
            });
        }

        function GetCompltedtimeData() {

            var date = new Date();
            var fieldDataOut = '{';
            var fieldData = '<root><data>';
            fieldData += '<IsCompleted>' + 1 + '</IsCompleted>';
            fieldData += '<CompletionTimestamp>' + formatDateTime(date) + '</CompletionTimestamp>';
            fieldData += '<CCM_TRN_CycleCount_ID>' + TotalList[0].CCM_TRN_CycleCount_ID + '</CCM_TRN_CycleCount_ID>';
            fieldData += '<CCM_MST_CycleCountStatus_ID>' + 4 + '</CCM_MST_CycleCountStatus_ID>';
            fieldData += '<UpdatedBy>' + $('#hdnUpdatedBy').val() + '</UpdatedBy>' + '<UpdatedOn>' + $('#hdnUpdatedOn').val() + '</UpdatedOn>' + '<NewCCM_TRN_CycleCount_ID>' + 0 + '</NewCCM_TRN_CycleCount_ID>';
            fieldData = fieldData + '</data></root>';
            fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'CCM_TRN_CycleCount_ID' + '":"' + TotalList[0].CCM_TRN_CycleCount_ID + '",';
            fieldDataOut = fieldDataOut.substring(0, fieldDataOut.length - 1);
            fieldDataOut += '}';
            return fieldDataOut;
        }

        function UpdateCompltedtime(cyclecountid) {
            debugger
            var item = $.grep(ItemList.Table, function (a) { return a.CCM_TRN_CycleCount_ID == cyclecountid });
            TotalList = item;

            $("#divLoading").show();

            var GoodsMovementOutData = "";
            GoodsMovementOutData = "{";
            GoodsMovementOutData += '"' + String.fromCharCode(64) + 'CCM_TRN_CycleCount_ID' + '":"' + cyclecountid + '",' + '"' + String.fromCharCode(64) + 'OperationFlag' + '":"' + 4 + '",' + '"' + String.fromCharCode(64) + 'UserLoggedId' + '":"' + $("#hdnUpdatedBy").val() + '"';
            GoodsMovementOutData += '}';


            var param = JSON.stringify({ "Sp_Set": "", "JSON": JSON.stringify(GoodsMovementOutData) });
            $.ajax({
                url: "CycleCountTransaction.aspx/MasterDetailsSetLFO",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                data: param,
                //async: false,
                success: function (response) {
                    debugger
                    var result = response;
                    showStickyToast(true, 'All locations are released  and Completed Successfully', false);
                    cid = cyclecountid;
                    GetCycleCountList(cyclecountid); //=============== Commented By M.D.Prasad On 20-Apr-2020 For Optimize the Code ================//
                    //UpsertCycleCountTRNRecord(cyclecountid);
                    setTimeout(function () {
                        location.reload();
                    }, 2500);
                },
                failure: function (errMsg) {

                }
            });
            //=============== Commented By M.D.Prasad On 20-Apr-2020 For Optimize the Code ================//
            /*var param = JSON.stringify({ "Sp_Set": "", "JSON": JSON.stringify(GetCompltedtimeData()) });
            $.ajax({
                url: "CycleCountTransaction.aspx/MasterDetailsSetCompletion",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                async: false,
                data: param,
                success: function (response) {
                    var result = response;
                    cid = cyclecountid;

                    GetCycleCountList(cyclecountid);
                    UpsertCycleCountTRNRecord(cyclecountid);
                    setTimeout(function () {
                        location.reload();
                    }, 2500);

                },
                failure: function (errMsg) {

                }
            });*/

        }

        function GETList() {
            var fieldData = '{';
            fieldData += '"' + String.fromCharCode(64) + 'CID":"' + cid + '",';
            fieldData += '"' + String.fromCharCode(64) + 'LoggedInUserID":"' + $("#hdnUpdatedBy").val().trim() + '",';
            fieldData += '"' + String.fromCharCode(64) + 'AccountID":"' + <%=this.cp.AccountID%> + '",';
            fieldData = fieldData.substring(0, fieldData.length - 1);
            fieldData += '}';
            return fieldData;
        }

        function GetCycleCountList(cyclecountid) {
            var param = JSON.stringify({ "SP_Name": "", "JSON": JSON.stringify(GETList()) });
            $.ajax({
                url: "CycleCountTransaction.aspx/GetSuccessInfoCounts",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                async: false,
                data: param,
                success: function (response) {
                    countlist = JSON.parse(response.d).Table;
                },
                failure: function (errMsg) {

                }
            });
        }

        function GETNewCount() {
            var date = new Date();
            var fieldDataOut = '{';
            var fieldData = '<root><data>';
            fieldData += '<CCM_MST_CycleCount_ID>' + TotalList[0].CCM_MST_CycleCount_ID + '</CCM_MST_CycleCount_ID>';
            fieldData += '<CCM_CNF_AccountCycleCount_ID>' + TotalList[0].CCM_CNF_AccountCycleCount_ID + '</CCM_CNF_AccountCycleCount_ID>';
            fieldData += '<CCM_MST_CycleCountStatus_ID>' + 1 + '</CCM_MST_CycleCountStatus_ID>';
            fieldData += '<SeqNo>' + (TotalList[0].SeqNo + 1) + '</SeqNo>';
            fieldData += '<IsActive>' + 1 + '</IsActive>';
            fieldData += '<IsDeleted>' + 0 + '</IsDeleted>';
            fieldData += '<CreatedBy>' + $('#hdnCreatedBy').val() + '</CreatedBy>' + '<CreatedOn>' + $('#hdnUpdatedOn').val() + '</CreatedOn>';
            fieldData += '<UpdatedBy>' + $('#hdnUpdatedBy').val() + '</UpdatedBy>' + '<UpdatedOn>' + $('#hdnUpdatedOn').val() + '</UpdatedOn>' + '<NewCCM_TRN_CycleCount_ID>' + 0 + '</NewCCM_TRN_CycleCount_ID>';
            fieldData = fieldData + '</data></root>';
            fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'UpdatedBy' + '":"' + <%=this.cp.UserID%> + '",' + '"' + String.fromCharCode(64) + 'CCM_TRN_CycleCount_ID' + '":"' + TotalList[0].CCM_TRN_CycleCount_ID + '",';
            fieldDataOut += '"' + String.fromCharCode(64) + 'PSD' + '":"' + TotalList[0].InitiatedDate + '",' + '"' + String.fromCharCode(64) + 'PED' + '":"' + TotalList[0].CompletionDate + '",'
            fieldDataOut = fieldDataOut.substring(0, fieldDataOut.length - 1);
            fieldDataOut += '}';
            return fieldDataOut;
        }

        function UpsertCycleCountTRNRecord(cyclecountid) {

            var item = $.grep(countlist, function (a) { return a.CCM_TRN_CycleCount_ID == cyclecountid });
            TotalList = item;

            var param = JSON.stringify({ "Sp_Set": "", "JSON": JSON.stringify(GETNewCount()) });
            $.ajax({
                url: "CycleCountTransaction.aspx/MasterDetailsSetNewCount",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                cache: false,
                data: param,
                success: function (response) {
                    var result = response;
                },
                failure: function (errMsg) {

                }
            });
        }

        function validcapturedata() {
            debugger;
            $("#divCaptureValidationMessages").empty();
            var IsValid = true;
            if (sno == 1) {
                IsValid = false;
                showStickyToast(false, "Please Enter Input Fields.", false);
                return false;
            }
            else {
                var item = GetCaptureItems(countid, 0);
                debugger
                var rowid = 0;
                if (item.Table != null && item.Table.length > 0) {
                    rowid = item.length;
                }
                else {
                    rowid = rowid;
                }
                for (var i = 1; i <= sno - 1; i++) {
                    if ($("#lblLocationid" + i).text() == "0") {
                        IsValid = false;
                        showStickyToast(false, "Please Enter Location in row " + (rowid + i), false);
                        return false;
                    }

                    if ($("#lblMaterialId" + i).text() == "0") {
                        IsValid = false;
                        showStickyToast(false, "Please Enter Material in row " + (rowid + i), false);
                        return false;
                    }


                    if ($("#Quantity" + i).val() == "") {
                        IsValid = false;
                        showStickyToast(false, "Please Enter Quantity in row " + (rowid + i), false);
                        return false;

                    }
                    if ($("#txtSerialNo" + i).val() != "" && ($("#Quantity" + i).val() != "1")) {
                        IsValid = false;
                        showStickyToast(false, "Qty should be 1 for Serial # ", false);
                        return false;
                    }

                    if (IsValid == false)
                        break;
                }
            }

            return IsValid;
        }

        function CaptureAppendMessage(Message) {
            $("#divCaptureValidationMessages").append("<li style='margin-top: 0.3em;'>" + Message + "</li>");
        }


        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }


        function ValidateCycleCountData() {
            $("#divValidationCycleCountMessages").empty();
            var IsValid = true;

            if ($("#PlannedStart").val().trim() == "") {
                IsValid = false;
                showStickyToast(false, 'Please Enter Planned Start.', false);
                return false;
            }

            var from = $("#PlannedStart").val();
            var to = $("#PlannedEnd").val();

            if (Date.parse(from) > Date.parse(to)) {
                IsValid = false;
                showStickyToast(false, 'Invalid Date Range.', false);
                return false;
            }

            return IsValid;
        }

        function CycleCountDataAppendMessage(Message) {
            $("#divValidationCycleCountMessages").append("<li style='margin-top: 0.3em;'>" + Message + "</li>");
        }



        function ShowCycleCountData() {
            var check = $("#chkStatus").prop('checked');
            if (check == false) {
                $("#divLoading").show();
                var table = $('#tblList').DataTable();
                table.destroy();
                $('#tblList').empty();
                GetAllCycleCountList();
            }
            else {
                $("#divLoading").show();
                var table = $('#tblList').DataTable();
                table.destroy();
                $('#tblList').empty();
                GetAllCycleCountListByStatus();
            }
        }

        function GoToReport(CTID) {
            window.open("CycleCountReport.aspx?parm=" + CTID);
        }

        function GoToLocationData(CTID,ACID) {
            window.open("CCBlockedLocations.aspx?param=" + ACID + "&param1=" + CTID);
        }

        function create() {
            window.open("CycleCountDetails.aspx?parm=0");
        }

        function DeleteParams(id) {
            var date = new Date();
            var UTCtime = JSON.stringify(date);
            var fieldData = '{';
            fieldData += '"' + String.fromCharCode(64) + 'PK":' + id + ',';
            fieldData += '"' + String.fromCharCode(64) + 'LoggedInUserID":' + $("#hdnUpdatedBy").val().trim() + ',';
            fieldData += '"' + String.fromCharCode(64) + 'UTCTimestamp":' + UTCtime + ',';
            fieldData = fieldData.substring(0, fieldData.length - 1);
            fieldData += '}';
            return fieldData;
        }

        var msg = "Deleted Successfully.";
        function DeleteEntity(id) {

            procname = "";
            var msgtitle = " Cycle Count Details";
            DeleteItem(id, msg, msgtitle);
        }


        function DeleteItem(id, msg, msgtitle) {
            $("#divLoading").show();
            var param = JSON.stringify({ "SP_Del": procname, "JSON": DeleteParams(id) });
            $.ajax({
                url: "CycleCountList.aspx/DeleteItemsById",// "@Url.Action("DeleteItemsById", "Master")",
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
            $('.dataTables-CycleCountList').DataTable({
                retrieve: true,
                conditionalPaging: true,
                pageLength: 25,
                dom: '<"html5buttons"B>lTfgitp',
                language: {
                    paginate: {
                        next: '>', // or '?'
                        previous: '<' // or '?' 
                    },
                    "sSearch": "Search :  ",
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

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#hdnCreatedBy").val(<%=cp.UserID%>);
             $("#hdnUpdatedBy").val(<%=cp.UserID%>);
        });
    </script>

</asp:Content>

