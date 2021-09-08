<%@ Page Title=" Attributes List " Language="C#" AutoEventWireup="true" Debug="true"  MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" CodeBehind="attributeMaster.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.attributeMaster" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MMContent" runat="server">
    <asp:ScriptManager ID="mySManager" EnablePageMethods="true" EnablePartialRendering="true" runat="server" SupportsPartialRendering="true">
    </asp:ScriptManager>
<link href="../Scripts/bootstrap/bootstrap.min.css" rel="stylesheet" /> 
    <script src="../mInventory/CycleCountScripts/jquery.dataTables.min.js"></script>  
    <script src="../mInventory/CycleCountScripts/dataTables.bootstrap.min.js"></script>
   
    <script src="../mInbound/Scripts/InventraxAjax.js"></script>
   <script src="../Scripts/CommonWMS.js"></script> 
    <style>
         /* Absolute Center Spinner */
         .dataTables_length, #tblList_filter, .dataTables_info{
             display:none;
         }
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

        .table-striped  thead tr:not(.stripedhed) th{
                border: 0px !important;
        }
    </style>


    <script>
        var TotalDataList = [];
        var TotalTableList = [];
        $(document).ready(function () {

            var data = "{ Id:  '0' }";
            InventraxAjax.AjaxResultExecute("attributeMaster.aspx/GetList", data, 'GetListOnSuccess', 'GetListOnError', null);

        });

        function GetListOnSuccess(data) {
            debugger;
            var dataList = JSON.parse(data.Result);
            LoadGrid(dataList);
        }


        var dataList = null;
        function LoadGrid(Obj) {

            if (Obj != null) {
                dataList = Obj.Table;

                $("#tblList").empty();
                $("#tblList").append("<thead><tr><th colspan='6' style='background-color: white'><div style='text-align:right;'><button type='button' class='btn btn-primary' data-toggle='modal' onclick='create();'>Add <i class='material-icons'>add</i></button></div></th></tr><tr   class='stripedhed'><th class='text-center'>S.No </th><th class='text-center'>Account</th><th class='text-center'>Tenant</th><th class='text-center'>Attribute Name</th><th class='text-center'>Attribute Code</th><th class='text-center'>Action</th></tr></thead><tbody>");
                for (var i = 0; i < dataList.length; i++) {
                    let list = `<tr><td >${i + 1}</td><td class='text-left'>${dataList[i].Account}</td><td>${dataList[i].Tenant}</td><td>${dataList[i].AttributeName}</td><td>${dataList[i].AttributeCode}</td><td class='text-center' style='text-align:left !important;'> <a style='cursor:pointer;' onclick='EditItem(${dataList[i].MM_MST_Attribute_ID});'><i class='material-icons ss'>mode_edit</i></a>&emsp; <a style='cursor:pointer;' onclick='DeleteItem(${dataList[i].MM_MST_Attribute_ID});'><i class='material-icons ss'>delete</i></a></td></tr>`;
                    $("#tblList").append(list);

                }
                $("#tblList").append("</tbody>");
                SetTableSettings();
            }
            else {
                $("#tblList").empty();
                $("#tblList").append("<thead><tr><th colspan='6'  style='background-color: white'><div ><button type='button' class='btn btn-primary' data-toggle='modal' onclick='createMaster();'>Add <i class='material-icons'>add</i></button></div></th></tr><tr><th class='text-center'>S.No </th><th class='text-center'>Tenant</th><th class='text-center'>Cycle Count</th><th class='text-center'>Valid From</th><th class='text-center'>valid Thru</th><th class='text-center'>Frequency</th><th class='text-center'>Active</th><th class='text-center'>Action</th></tr><tr><th colspan='9' class='text-center' style='background-color: white'><p style='text-align: center;'><strong>No Data</strong></p></th></tr></thead><tbody>");
                $("#tblList").append("</tbody>");
            }
        }

        function EditItem(id) {
            window.location.href = "attributesDetails.aspx?parm=" + id;
        }
        function create() {
            window.location.href = "attributesDetails.aspx?parm=0";
        }

        function DeleteItem(id) {
            if (confirm("Are you sure do you want to delete ?")) {
                $("#divLoading").show();
                var param = JSON.stringify({ "SP_Del": "", "ID": id });
                $.ajax({
                    url: "attributeMaster.aspx/DeleteItemsById",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    cache: false,
                    data: param,
                    success: function (response) {
                        var dt = JSON.parse(response.d).Table[0].N;
                        if (dt == 1) {
                            showStickyToast(false, "This Attribute is configured in Item Master.", false);
                            $("#divLoading").hide();
                            return;
                        }
                        else {
                            showStickyToast(true, "Deleted Successfully.", false);
                            setTimeout(function () {
                                location.reload();
                            }, 2500);
                        }
                        //getparameters('success', msg, msgtitle);


                    },
                    failure: function (errMsg) {

                    }
                });
            }
            else {
                return false;
            }

        }

        function SetTableSettings() {
            $('.dataTables-example').DataTable({
                pageLength: 25,
                dom: '<"html5buttons"B>lTfgitp',
                language: {
                    paginate: {
                        next: '>', // or '→'
                        previous: '<' // or '←' 
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
    <div class="module_yellow">
            <div class="ModuleHeader">
               <div> <a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Administration</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Attributes </span></div>
                
           </div>
            
        </div>
    <div>

        <div class="" container>

            <div class="ibox-content">
                <div class="">
                    <table class="table-striped  dataTables-example" id="tblList"></table>

                </div>
            </div>
          
        </div>

    </div>

    <div class="loading" id="divLoading" style="display: none;"></div>
  


</asp:Content>
