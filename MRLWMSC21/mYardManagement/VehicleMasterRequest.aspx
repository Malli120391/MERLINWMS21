<%@ Page Title="Vehicle Master Request" Language="C#" MasterPageFile="~/mYardManagement/YardManagement.master" AutoEventWireup="true" CodeBehind="VehicleMasterRequest.aspx.cs" Inherits="MRLWMSC21.mYardManagement.VehicleMasterRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />
    <script src="../Scripts/jquery-ui-1.8.24.js"></script>
    <%--<script src="../Scripts/CommonScripts.js"></script>--%>
    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <link href="../Content/app.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            if (<%=this.cp.AccountID%> == 0 || <%=this.cp.AccountID%> == null) {
                //$('#txtAccount').attr("disabled", false);
            }
            else {
                //debugger;
               <%-- $('#<%= this.txtAccount.ClientID %>').attr("disabled", true);--%>
                $("#<%= this.txtAccount.ClientID %>").css("background-color", "#ebebe4");
              <%--  $("#<%= this.txtAccount.ClientID %>").val("<%=this.cp.Account%>");
                $("#<%= this.Account.ClientID %>").val(<%=this.cp.AccountID%>);--%>
            }
        });
        $(function () {
            var accountfield = $('#<%= this.txtAccount.ClientID %>');
            DropdownFunction(accountfield);
            $("#<%= this.txtAccount.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadAccountForCyccleCount") %>',
                        data: "{ 'prefix': '" + request.term + "', 'AccountID': '" +  <%=this.cp.AccountID%> + "'}",
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    //debugger
                    $("#<%= this.Account.ClientID %>").val(i.item.val);
                    $("#<%= this.txtAccount.ClientID %>").val(i.item.label);

                    var AccountID = $("#<%= this.Account.ClientID %>").val();
                    LoadTenantBasedAcc(AccountID);
                    LoadFreightBasedAcc(AccountID);
                    LoadVehicleTypeBasedAcc(AccountID);
                },
                minLength: 0
            });

            //Load Storage Dimensions UoM - Length
            var suomfield = $('#<%= this.txtSUoM.ClientID %>');
            DropdownFunction(suomfield);
            $("#<%= this.txtSUoM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadStorageUoMLength") %>',
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    //debugger
                    $("#<%= this.SUoM.ClientID %>").val(i.item.val);
                    $("#<%= this.txtSUoM.ClientID %>").val(i.item.label);
                    $("#<%= this.VUoM.ClientID %>").val(i.item.val);
                    $("#<%= this.txtVUoM.ClientID %>").val(i.item.label);
                    $("#<%= this.txtVUoM.ClientID %>").attr("readonly",true);
                },
                minLength: 0
            });

            //Load Vehicle Dimensions UoM - Length
            <%--var vuomfield = $('#<%= this.txtVUoM.ClientID %>');
            DropdownFunction(vuomfield);
            $("#<%= this.txtVUoM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadStorageUoMLength") %>',
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    //debugger
                    $("#<%= this.VUoM.ClientID %>").val(i.item.val);
                    $("#<%= this.txtVUoM.ClientID %>").val(i.item.label);
                },
                minLength: 0
            });--%>

            //Load Weight Dimensions UoM - Weight
            var wuomfield = $('#<%= this.txtWUoM.ClientID %>');
            DropdownFunction(wuomfield);
            $("#<%= this.txtWUoM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadStorageUoMWeight") %>',
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    //debugger
                    $("#<%= this.WUoM.ClientID %>").val(i.item.val);
                    $("#<%= this.txtWUoM.ClientID %>").val(i.item.label);
                },
                minLength: 0
            });

            $("#<%= this.WStorage.ClientID %>, #<%= this.WTare.ClientID %>").blur(function () {
                debugger;
                var tareweight = $("#<%= this.WTare.ClientID %>").val();
                var storageweight = $("#<%= this.WStorage.ClientID %>").val();
                if (tareweight != "" && storageweight != "") {
                    var totalweight = parseInt(storageweight) + parseInt(tareweight);
                    $("#<%= this.WTotal.ClientID %>").val(totalweight);
                    $("#<%= this.WTotal.ClientID %>").attr('readonly', true);
                }
                else {
                    if (storageweight != "") {
                     
                        if (tareweight == "" ) {
                            showStickyToast(false, "Please Enter Tare Weight", false);
                            return false;
                        }
                    }
                    else if (tareweight != "") {

                        if (storageweight == "") {
                            showStickyToast(false, "Please Enter Max. Storage weight", false);
                            return false;
                        }
                    }
                    else {
                        showStickyToast(false, "Please Enter Tare Weight and Max. Storage weight", false);
                        return false;
                    }

                }
            })
        });
      function blockSpecialChar(e) {
            var k = e.keyCode;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8   || (k >= 48 && k <= 57));
        }
        //Load Freight Company Based on Account
        function LoadFreightBasedAcc(id) {
            var freightfield = $('#<%= this.txtFreight.ClientID %>');
            DropdownFunction(freightfield);
            $("#<%= this.txtFreight.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadFreightCompanyBasedonAccount") %>',
                        data: "{ 'prefix': '" + request.term + "', 'AccountID': '" + id + "'}",
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    //debugger;
                    $("#<%= this.Freight.ClientID %>").val(i.item.val);
                    $("#<%= this.txtFreight.ClientID %>").val(i.item.label);
                },
                minLength: 0
            });
        }

           //Load Freight Company Based on Account
        function LoadTenantBasedAcc(id) {
            var freightfield = $('#<%= this.txtTenant.ClientID %>');
            DropdownFunction(freightfield);
            $("#<%= this.txtTenant.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PLByAccount") %>',
                        data: "{ 'prefix': '" + request.term + "', 'AccountID': '" + id + "'}",
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    //debugger;
                    $("#<%= this.tenant.ClientID %>").val(i.item.val);
                    $("#<%= this.txtTenant.ClientID %>").val(i.item.label);
                },
                minLength: 0
            });
        }

        //Load Vehicle Type Dropdown
        function LoadVehicleTypeBasedAcc(id) {
            var vehiclefield = $('#<%= this.txtVehicleType.ClientID %>');
            DropdownFunction(vehiclefield);
            $("#<%= this.txtVehicleType.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadVehicleTypeBasedonAccount") %>',
                        data: "{ 'prefix': '" + request.term + "', 'AccountID': '" + id + "'}",
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    //debugger
                    $("#<%= this.VehicleType.ClientID %>").val(i.item.val);
                    $("#<%= this.txtVehicleType.ClientID %>").val(i.item.label);
                },
                minLength: 0
            });
        }

        function GoToList() {
            window.location.href = "VehicleMasterList.aspx";
        }

        function isNumberKeyEvent(e) {
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        }
            function GoToList() {
            window.location.href = "VehicleMasterList.aspx";
        }
        function Success(vid) {
            showStickyToast(true, "Basic Details Saved Successfully", false);
            setTimeout(function () {
                window.location.replace("VehicleMasterRequest.aspx?vid=" + vid);
            }, 1500);
        }


        function validation() {
            debugger;
           var account = $("#<%= this.Account.ClientID %>").val();
            var accountname = $("#<%= this.txtAccount.ClientID %>").val();
            if (account == 0 || account == null || account == undefined || accountname == "" || accountname == null || accountname == undefined  )
            {
                showStickyToast(false, "Please Select Account", false);
                return false;
            }
            var Tennat= $("#<%= this.tenant.ClientID %>").val();
             var Tenantname = $("#<%= this.txtTenant.ClientID %>").val();
            if (Tennat == 0 || Tennat == null || Tennat == undefined || Tenantname == "" || Tenantname == null || Tenantname == undefined  )
            {
                showStickyToast(false,"Please Select Tenant", false);
                return false;
            }

            var Freight= $("#<%= this.Freight.ClientID %>").val();
            var Freightname= $("#<%= this.txtFreight.ClientID %>").val();
            if (Freight == 0 || Freight == null || Freight == undefined || Freightname == "" || Freightname == null || Freightname == undefined  )
            {
                showStickyToast(false,"Please Select Freight Company", false);
                return false;
            }

           var vehicle=  $("#<%= this.VehicleType.ClientID %>").val();
           var vehicleType = $("#<%= this.txtVehicleType.ClientID %>").val();
            if (vehicle == 0 || vehicle == null || vehicle == undefined || vehicleType == "" || vehicleType == null || vehicleType == undefined  )
            {
                showStickyToast(false,"Please Select Vehicle Type", false);
                return false;
            }
            if ($('#MainContent_MMContent_RegdNumber').val() == "" || $('#MainContent_MMContent_RegdNumber').val() == null || $('#MainContent_MMContent_RegdNumber').val() == undefined)
            {
                showStickyToast(false,"Please Enter Registration Number", false);
                return false;
            }
            if ($('#MainContent_MMContent_OwnerName').val() == "" || $('#MainContent_MMContent_OwnerName').val() == null || $('#MainContent_MMContent_OwnerName').val() == undefined)
            {
                showStickyToast( false,"Please Enter Owner Name", false);
                return false;
            }
             if ($('#MainContent_MMContent_OContactNumber').val() == "" || $('#MainContent_MMContent_OContactNumber').val() == null || $('#MainContent_MMContent_OContactNumber').val() == undefined)
            {
                showStickyToast(false,"Please Enter Owner Contact Number", false);
                return false;
            }

            var Storage = $("#<%= this.SUoM.ClientID %>").val();
            var StorageUom = $("#<%= this.txtSUoM.ClientID %>").val();       
            if (Storage == 0 || Storage == "" || Storage == undefined || StorageUom == "" || StorageUom == null || StorageUom == undefined || $('#MainContent_MMContent_SLength').val() == "" ||  $('#MainContent_MMContent_SWidth').val() == "" ||  $('#MainContent_MMContent_SHeight').val() == "")
            {
                showStickyToast(false,"Please Enter All Mandatory Fields in Storage Dimensions", false);
                return false;
            }

            var vehicle = $("#<%= this.VUoM.ClientID %>").val();
            var vehicleuom = $("#<%= this.txtVUoM.ClientID %>").val();
            if (vehicle == 0 || vehicle == "" || vehicle == undefined || vehicleuom == "" || vehicleuom == null || vehicleuom == undefined || $('#MainContent_MMContent_VLength').val() == "" ||  $('#MainContent_MMContent_VWidth').val() == "" ||  $('#MainContent_MMContent_VHeight').val() == "")
            {
                showStickyToast( false,"Please Enter All Mandatory Fields in Volume Dimensions", false);
                return false;
            }

            var Weight = $("#<%= this.WUoM.ClientID %>").val();                  
            var Weightuom =  $("#<%= this.txtWUoM.ClientID %>").val();
            if (Weight == 0 || Weight == "" || Weight == undefined || Weightuom == "" || Weightuom == null || Weightuom == undefined || $('#MainContent_MMContent_WTare').val() == "" ||  $('#MainContent_MMContent_WStorage').val() == "" )
            {
                showStickyToast(false,"Please  Select the  Weight Dimensions", false);
                return false;
            }
            
        }
    </script>
    <style>
        .accord {
            color: #313030;
            font-weight: 400;
            cursor: pointer;
            font-family: Calibri,Verdana,Geneva,sans-serif !important;
            font-size: 13pt !important;
            display: block;
            width: 100%;
        }

  

        .lege {
            width: inherit;
            padding: 0 10px;
            border-bottom: none;
            font-size: 11pt !important;
        }

        .field {
            border: 1px solid silver !important;
            margin: 0 2px !important;
            padding: .35em .625em .75em !important;
        }

      

        .borderless td, .borderless th {
            border: none !important;
        }

        .borderless tr > td {
            border: none !important;
        }

    
    </style>

    <div class="container">
     <%-- <div class="flex__ end"><button type="button" id="btnList" onclick="GoToList()" class="btn btn-primary pull-right mb0"><i class="material-icons vl">arrow_back</i> Back To List</button></div>--%>
        <%--<div class="gap5"></div>--%>
        <div class="row">
            <div class="">
                <div class="panel-group" id="accordion">
                    <%----------------------------------------- Panel 1 ---------------------------------------------%>
                    <div class="panel panel-default panelborder" id="panel1">
                        <div class="panel-heading accordpanel">
                            <a class="accord collapsed" data-toggle="collapse" data-target="#collapseOne">Basic Vehicle Details</a>
                        </div>
                        <div id="collapseOne" class="panel-collapse collapse in">
                            <div class="panel-body" style="border-top-color: #fac18a !important;">
                                <div class="">
                                    <div class="row">
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="txtAccount" runat="server" class="ui-autocomplete-input" required="">
                                                    <label>Account</label>
                                                    <span class="errorMsg"></span>
                                                    <input type="hidden" class="p1save" id="Account" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                           <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="txtTenant" runat="server" class="ui-autocomplete-input" required="">
                                                    <label>Tenant</label>
                                                    <span class="errorMsg"></span>
                                                    <input type="hidden" class="p1save" id="tenant" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="txtFreight" runat="server" class="ui-autocomplete-input" required="">
                                                    <label>Freight Company</label>
                                                    <span class="errorMsg"></span>
                                                    <input type="hidden" id="Freight" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                     
                                  
                                
                                           <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="txtVehicleType" runat="server" class="ui-autocomplete-input" required="">
                                                    <label>Vehicle Type</label>
                                                    <span class="errorMsg"></span>
                                                    <input type="hidden" id="VehicleType" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        </div>
                                <div class="row">
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <input type="text" class="p1save" id="RegdNumber" runat="server" onkeypress="return blockSpecialChar(event)" maxlength="12" required="">
                                                <label>Vehicle Registration Number</label>
                                                <span class="errorMsg"></span>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <input type="text" class="p1save" id="OwnerName" runat="server" maxlength="30" required="">
                                                <label>Owner Name</label>
                                                <span class="errorMsg"></span>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <input type="text" class="p1save" id="OContactNumber" runat="server" maxlength="10" onkeypress="return isNumberKeyEvent(event)" required="">
                                                <label>Owner Contact Number</label>
                                                <span class="errorMsg"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <%--Storage Dimensions--%>
                                    <div class="col-md-4 p0">
                                        <fieldset class="field flexlimit80">
                                            <legend class="lege">Storage Dimensions</legend>
                                            <table class="">
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <div>
                                                            </div>
                                                            <div class="">
                                                                <input type="text" id="txtSUoM" runat="server" class="ui-autocomplete-input" required="">
                                                                <label>Storage UoM</label>
                                                                <span class="errorMsg"></span>
                                                                <input type="hidden" class="p1save" id="SUoM" runat="server" />
                                                            </div>
                                                        </div>


                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <input type="text" id="SLength" maxlength="4" runat="server" onkeypress="return isNumberKeyEvent(event)" required="">
                                                            <label>Storage Length</label>
                                                            <span class="errorMsg"></span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <div>
                                                                <input type="text" id="SWidth" maxlength="4" runat="server" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                <label>Storage Width</label>
                                                                <span class="errorMsg"></span>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <div>
                                                                <input type="text" id="SHeight" maxlength="4" runat="server" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                <label>Storage Height</label>
                                                                <span class="errorMsg"></span>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>

                                    <%--Vehicle Dimensions--%>
                                    <div class="col-md-4">
                                        <fieldset class="field flexlimit80">
                                            <legend class="lege">Vehicle Dimensions</legend>
                                            <table class="">
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <div>
                                                            </div>
                                                            <div class="">
                                                                <input type="text" id="txtVUoM" runat="server" class="ui-autocomplete-input" required="">
                                                                <label>Vehicle UoM</label>
                                                                <span class="errorMsg"></span>
                                                                <input type="hidden" class="p1save" id="VUoM" runat="server" />
                                                            </div>
                                                        </div>


                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <input type="text" id="VLength" maxlength="4" runat="server" onkeypress="return isNumberKeyEvent(event)" required="">
                                                            <label>Vehicle Length</label>
                                                            <span class="errorMsg"></span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <div>
                                                                <input type="text" id="VWidth" maxlength="4" runat="server" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                <label>Vehicle Width</label>
                                                                <span class="errorMsg"></span>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <div>
                                                                <input type="text" id="VHeight" maxlength="4" runat="server" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                <label>Vehicle Height</label>
                                                                <span class="errorMsg"></span>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>

                                    <%--Weight Dimensions--%>
                                    <div class="col-md-4 p0">
                                        <fieldset class="field flexlimit80">
                                            <legend class="lege">Weight Dimensions</legend>
                                            <table class="">
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <div>
                                                            </div>
                                                            <div class="">
                                                                <input type="text" id="txtWUoM" runat="server" class="ui-autocomplete-input" required="">
                                                                <label>Weight UoM</label>
                                                                <span class="errorMsg"></span>
                                                                <input type="hidden" class="p1save" id="WUoM" runat="server" />
                                                            </div>
                                                        </div>


                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <input type="text" id="WTare" maxlength="4" runat="server" onkeypress="return isNumberKeyEvent(event)" required="">
                                                            <label>Tare Weight</label>
                                                            <span class="errorMsg"></span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <div>
                                                                <input type="text" id="WStorage" maxlength="4" runat="server" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                <label>Max. Storage Weight</label>
                                                                <span class="errorMsg"></span>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <div>
                                                                <input type="text" id="WTotal" maxlength="4" readonly="readonly" runat="server" onkeypress="return isNumberKeyEvent(event)" required="" >
                                                                <label>Max. Total Weight</label>
                                                                <span class="errorMsg"></span>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                </div>
                               
                                <div style="display: flex; justify-content: flex-end">
                                    <div class="" flex>
                                         <div><button type="button" id="btnList" onclick="GoToList()" class="btn btn-primary pull-right mb0"><i class="material-icons vl">arrow_back</i> Back To List</button></div>
                                        <asp:LinkButton ID="lnkBasicVehicleSave" runat="server" CssClass="btn btn-primary" OnClientClick="return validation();" OnClick="lnkBasicVehicleSave_Click">Save <i class='fa fa-floppy-o'></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%----------------------------------------- Panel 1 ---------------------------------------------%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
