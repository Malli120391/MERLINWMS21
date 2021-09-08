<%@ Page Title="Transfer List" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="FastMovingLocsTransfer.aspx.cs" Inherits="MRLWMSC21.mInventory.FastMovingLocsTransfer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">


    <link href="../Scripts/bootstrap/bootstrap.min.css" rel="stylesheet" />  
<script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/dataTables.bootstrap.min.js"></script>   
   
    <script src="../mInbound/Scripts/InventraxAjax.js"></script>
   <script src="../Scripts/CommonWMS.js"></script> 

    <input type="hidden" value='' id="hdnViewName" />
<input type="hidden" value='' id="hdnSp_Get" />
<input type="hidden" value='' id="hdnSp_Set" />
<input type="hidden" value='' id="hdnJSONMaster" />
<input type="hidden" value='0' id="hdnCId" />

<input type="hidden" value='1' id="hdnCreatedBy" />
<input type="hidden" value='2018-01-04' id="hdnUpdatedOn" />
<input type="hidden" value='1' id="hdnUpdatedBy" />
    <input type="hidden" value='0' id ="hdnWareHouseID" />
 
     <style>
        .row {
            margin-left:0;
            margin-right:0;
        }
        table {
            border-collapse:inherit !important;
        }
        a {
            box-sizing: initial !important;
        }
         .fa-edit, fa-trash {
         cursor:pointer;
         }
          .dataTables_filter, .dataTables_length, .dataTables_info {
         display:inline-block;
         }
        .dataTables_info {
            margin-left:20px;
        }
        .dataTables_filter {
        float:right;
        }

         .pagination > li > a:hover, .pagination > li > span:hover, .pagination > li > a:focus, .pagination > li > span:focus {

    border-color: #6b8c57 !important;
    background-color: #6b8c57 !important;
    color: #fff;
}

         .previous:hover{
            background-color: #fff !important;
            color: #0e0e0e !important;
            border-color: #fff !important;
         }

         .trRow {
         cursor:pointer;}
         .tblChild {
         display:none;}

         select {
            font-size:14px !important;
                width: 99% !important;
         }

         .table-striped .text-right {
                padding: 0 !important;
                border: 0;
                box-shadow: none;
         }

         .table-striped tr td {
             font-size:13px !important;
         }
    </style>
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
    
 <!-- Globalization Tag is added for multilingual  -->
      <div class="module_yellow">
            <div class="ModuleHeader" height="35px">
                <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Orders</span> <i class="material-icons">arrow_right</i><span>House Keeping</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Material Transfer </span></div>
                
            </div>

        </div>
     <div class="loading" id="divLoading" style="display: none;"></div> 
    <div class="container">       
 
        <div class="ibox-content">
            <div class="">
                <div class="row">
                    <div class="col m3 ">
                        <div class="flex">
                            <select id="ddlTransferType" class="ddlTransferType" runat="server" required="" style="width: 100% !important;">
                                <option value="">Select </option>
                            </select>
                            <%--  <label>Transfer Type</label>--%>
                            <label>
                                <%= GetGlobalResourceObject("Resource", "TransferType")%>
                            </label>
                        </div>
                    </div>
                    <div class="col m3 s3">
                        <div class="flex">
                            <select id="ddlStatus" class="ddlStatus" runat="server" required="" style="width: 100% !important;">
                                <option value="">Select</option>
                            </select>
                            <%--  <label>Status</label>--%>
                            <label><%= GetGlobalResourceObject("Resource", "Status")%></label>
                        </div>
                    </div>
                     <div class="col m3 s3">
                        <div class="flex">
                            <input type="text" id="txtWH" skinid="txt_Auto" required="" />
                            <label>Warehouse </label>
                           
                        </div>
                    </div>
                    <div class="col m2">
                        <gap></gap>
                        <button type="button" value="Search" id="btnAdd" class="btn btn-primary right" onclick="createAdd();"><%= GetGlobalResourceObject("Resource", "Add")%><i class='material-icons'>add</i></button>
                        <button type="button" value="Search" id="btnSearch" class="btn btn-primary right" style="margin-left:2px !important" onclick="btnSearch_Click();"><%= GetGlobalResourceObject("Resource", "Search")%><i class="material-icons vl">search</i></button>
                    </div>
                </div>
                <div class="table-striped dataTables-example" id="divList"></div>


            </div>
        </div>

    </div>


    <script type="text/javascript">
       
        $(document).ready(function () {
           // $("#tblList").dataTable().fnDestroy();
            btnSearch_Click();


           
             var TextFieldName = $("#txtWH");
                DropdownFunction(TextFieldName);
                $("#txtWH").autocomplete({
                    source: function (request, response) {
                        debugger;     
                        if ($("#txtWH").val() == "" || $("#txtWH").val() == null) {

                            $("#hdnWareHouseID").val(0);
                        }

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouseBasedonUser") %>',
                            data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>
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

                        $("#hdnWareHouseID").val(i.item.val);
                        $("#txtWH").val(i.item.label);

                    },
                    minLength: 0
                });


        });
        //$(document).ready(function () {
        //    $('#tblList').dataTable();
        //});
        function btnSearch_Click() {
            
            BuildGrid();
        }

        function BuildGrid() {
            
            var data = "{TransferTypeId:'" + $('.ddlTransferType').val() + "', StatusId:  '" + $('.ddlStatus').val() + "' , WarehouseID : '"+$("#hdnWareHouseID").val()+"' }";
            InventraxAjax.AjaxResultExecute("FastMovingLocsTransfer.aspx/GetList", data, 'GetListOnSuccess', 'GetListOnError', null);
        }

        function GetListOnSuccess(data) {
            debugger;
            var dataList = data.Result;
            LoadGrid(dataList);
        }
        function createAdd() {
            debugger;
            window.location.href = "../mInventory/InternaltransferRequest.aspx";
        }


      
        var dataList = null;
        function LoadGrid(Obj) {

            $("#divList").empty();
            var s = "";
            s+="<table class='table-striped dataTables-example' id='tblList'><thead><tr hidden><th colspan='8' class='text-right' style='background-color: white'><div class='text-right hidden'><button type='button' class='btn btn-primary' data-toggle='modal' onclick='create();'><%= GetGlobalResourceObject("Resource", "Add")%><i class='fa fa-plus' aria-hidden='true'></i></button></div></th></tr>  <tr class='stripedhed'><th class='text-center'><%= GetGlobalResourceObject("Resource", "SNo")%> </th><th class='text-center'><%= GetGlobalResourceObject("Resource", "Tenant")%></th><th class='text-center'><%= GetGlobalResourceObject("Resource", "Warehouse")%></th><th class='text-center'><%= GetGlobalResourceObject("Resource", "TransRefNo")%></th><th class='text-center'><%= GetGlobalResourceObject("Resource", "TransferType")%> </th><th class='text-center'><%= GetGlobalResourceObject("Resource", "CreatedBy")%></th><th class='text-center'><%= GetGlobalResourceObject("Resource", "CreatedOn")%></th><th class='text-center'><%= GetGlobalResourceObject("Resource", "Status")%></th><th class='text-center'><%= GetGlobalResourceObject("Resource", "Action")%></th></tr></thead><tbody>";

            if (Obj != null && Obj.length > 0) {
                dataList = Obj;

               
                for (var i = 0; i < dataList.length; i++) {
                    var status = dataList[i].FulfillStatus;
                    var DelButton = "";
                    if (dataList[i].FulfillStatusID == 1)
                    {
                        status = "<button class='btn btn-primary btn-xs' style='cursor: pointer;' onclick='return btnInitiate_Click(" + dataList[i].TransferRequestID + ");' type='button'><%= GetGlobalResourceObject("Resource", "Initiate")%></button>";
                        DelButton = "<a onclick='DeleteOrder(" + dataList[i].TransferRequestID + ");'><i class='material-icons ss'>delete</i></a>";
                    }
                    else if (dataList[i].FulfillStatusID == 2) {
                        status = "<button class='btn btn-primary btn-xs' style='cursor: pointer;' onclick='return btnInProcess_Click(" + dataList[i].TransferRequestID + ");' type='button'><%= GetGlobalResourceObject("Resource", "InProcess")%> </button>";
                       // status = dataList[i].FulfillStatus + "&emsp;<button class='btn btn-primary btn-xs' onclick='return btnPick_Click(" + dataList[i].TransferRequestID + ");' type='button'>Pick</button>";
                    }


                    s +="<tr class='trRow'><td class='text-right' style='vertical-align: middle;'>" + (i + 1) + "</td><td class='text-left'>" + dataList[i].TenantName + "</td> <td class='text-left'>" + dataList[i].WareHouse + "</td><td class='text-left'>" + dataList[i].TransferRequestNumber + "</td><td class='text-left'>" + dataList[i].TransferType + "</td><td class='text-center'>" + dataList[i].CreatedBy + "</td><td class='text-center'>" + dataList[i].CreatedOn + "</td><td class='text-left'>" + status + "</td> <td class='text-left'> <a style'text- decoration: none;' href='../mInventory/InternalTransferRequest.aspx?Id=" + dataList[i].TransferRequestID + "' ><i class='material-icons>mode_edit</i></a></td><td class='text-center hidden'> <a onclick='EditItem(" + dataList[i].TransferRequestID + ");'><i class='material-icons ss'>mode_edit</i></a>&emsp; " + DelButton + "</td></tr>";
                    var Details = dataList[i].Details;

                   // $("#tblList").append("<tr class='tblChild'><td>&nbsp;</td><td colspan='6'>" + GetChildTable(Details) + "</td></tr>");
                }
                s +="</tbody></table>";
               

                //LoadHandlers();
            }
            else {               
              //  s +="<thead><tr><td colspan='8' class='text-center' style='background-color: white; text-align:center;'>No Orders</strong></td></tr></thead><tbody>";
                s +="</tbody></table>";
            }


            $("#divList").html(s);
            debugger;
            if (dataList.length > 25) {
                SetTableSettings();
            }
            else {
                SetTableSettingsNoPageing();
            }
           

        }

        function GetChildTable(obj)
        {
            var s = "<table class='table table-bordered' style='width:100%;'><tr><th><%= GetGlobalResourceObject("Resource", "SNo")%></th><th><%= GetGlobalResourceObject("Resource", "MCode")%></th><th><%= GetGlobalResourceObject("Resource", "Quantity")%></th><th><%= GetGlobalResourceObject("Resource", "BatchNo")%></th><th><%= GetGlobalResourceObject("Resource", "FromLocation")%></th><th><%= GetGlobalResourceObject("Resource", "ToLocation")%></th></tr>";
            for (var j = 0; j < obj.length; j++) {
                s += "<tr><td style='vertical-align: middle;'>" + (j + 1) + "</td><td>" + obj[j].MCode + "</td><td class='text-right'>" + obj[j].Quantity + "</td><td>" + obj[j].BatchNo + "</td><td>" + obj[j].FromLocation + "</td><td>" + obj[j].ToLocation + "</td></tr>";
            }
            s += '</table>';
            return s;
        }


       
        function btnInitiate_Click(TransferRequestID) {
            debugger;
            var param = "{TransferRequestID:'" + TransferRequestID + "'}";
              $("#divLoading").show();
            InventraxAjax.AjaxResultExecute('FastMovingLocsTransfer.aspx/InitiateToInProcess', param, 'InitiateToInProcessOnSuccess', null, null);
             $("#divLoading").hide();
            return false;
           
        }
        function btnInProcess_Click(TransferRequestID) {

            var param = "{TransferRequestID:'" + TransferRequestID + "'}";
            InventraxAjax.AjaxResultExecute('FastMovingLocsTransfer.aspx/InprocessToCloss', param, 'InprocessToClose', null, null);

            return false;
        }
        function InitiateToInProcessOnSuccess(data) {
            if (data.Result > 0) {
                showStickyToast(true, "Order Initiated Successfully", false);
                btnSearch_Click();

            }
            else if (data.Result == -999) {
                showStickyToast(false, "Order contains no items", false);
            }
            else if (data.Result == -1) {

                //showStickyToast(false, "Stock is not available  for given items", false);
                showStickyToast(false, "There are no items added in transfer details", false);
            }
            else {
                showStickyToast(true, "Order Initiated Successfully", false);
                btnSearch_Click();
            }
        }

        function InprocessToClose(data) {
            if (data.Result == 1) {
                showStickyToast(true, "Order sucessfully closed", false);
                btnSearch_Click();
            }
            else if (data.Result == -1) {
                showStickyToast(false, "This order is not yet transferred.", false);
            }
        }

        function btnPick_Click(TransferRequestID)
        {
            window.open("../mOutbound/GroupDeliveryPickNote.aspx?TRANSREQID=" + TransferRequestID);
        }

       
        //function LoadHandlers() {
        //    $('.trRow').click(function () {
        //        var stat = $(this).next().css('display');
        //        if (stat == 'none') {
        //            $(this).next().css({ 'display': 'table-row', 'transition': '2s linear' });
        //        }
        //        else {
        //            $(this).next().css({ 'transition': '2s linear', 'display': 'none' });
        //        }
        //    });
        //}

        function DeleteOrder(id)
        {
            if (window.confirm("Do you want to Delete?")) {
                var param = "{TransferRequestID:'" + id + "'}";
                InventraxAjax.AjaxResultExecute('FastMovingLocsTransfer.aspx/DeleteOrder', param, 'DeleteOrderOnSuccess', null, null);
            }

        }

        function DeleteOrderOnSuccess(data)
        {
            if (data.Result > 0) {
                showStickyToast(true, "Order Deleted Successfully", false);
                btnSearch_Click();
            }
            else {
                showStickyToast(false, "Order Deletion failed", false);
            }
        }

        function EditItem(id) {
            // window.location.href = "@Url.Action("Get","Master")?pId=" + $('#hdnCId').val() + "&parm=" + id + "&ischild=1";
            window.open("InternaltransferRequest.aspx?Id=" + id);
        }

        function create() {
            //window.location.href = "@Url.Action("Get","Master")?pId=" + $('#hdnCId').val() + "&parm=0&ischild=1";
            window.open("InternaltransferRequest.aspx?Id=0");
        }
        function SetTableSettings() {
            $('#tblList').DataTable({
                pageLength: 25,
                dom: '<"html5buttons"B>lTfgitp',
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

        function SetTableSettingsNoPageing() {
            $('#tblList').DataTable({
                "paging": false,
              //  pageLength: 25,
                dom: '<"html5buttons"B>lTfgitp',
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



</asp:Content>
