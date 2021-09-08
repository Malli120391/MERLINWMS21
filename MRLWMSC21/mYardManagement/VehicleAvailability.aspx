<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VehicleAvailability.aspx.cs" MasterPageFile="~/mYardManagement/YardManagement.master" Inherits="MRLWMSC21.mYardManagement.VehicleAvailability" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />
    <script src="../Scripts/jquery-ui-1.8.24.js"></script>
    <%--<script src="../Scripts/CommonScripts.js"></script>--%>
    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <link href="../Content/app.css" rel="stylesheet" />
        <script src="../Scripts/CommonWMS.js"></script>
    <style>
        .field{
                min-height: 320px;
        }
    </style>
    
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

        .ui-autocomplete-input {
            --md-arrow-width: 1em;
            background: url(../Images/magnifier.svg) calc(100% - var(--md-arrow-offset) - var(--md-select-side-padding)) center no-repeat !important;
            background-size: var(--md-arrow-width) !important;
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

        .flexlimit90 .flex {
            width: 90% !important;
        }

        .flexlimit80 .flex {
            width: 85% !important;
        }

        .borderless td, .borderless th {
            border: none !important;
        }

        .borderless tr > td {
            border: none !important;
        }
    </style>
    <link href="../Content/app.css" rel="stylesheet" />
  
       <table class="tbsty">
        <tbody>
            <tr class="module_yellow">
               <td class="ModuleHeader fixed-width">
                       <%-- <div><a href="../Default.aspx">Home</a> / Master Data / <span class="FormSubHeading">Configuration / Container Configuration</span></div>--%>
                    <div style="margin-left:1em"><a href="../Default.aspx">Home</a> / Administration /<span class="FormSubHeading"> Vehicle Availabity & Downtime</span></div>
                </td>
             </tr>
        </tbody>
    </table>
<%--    <div style="margin-top: 3%;" class="flex__ row">
        <div class="col-md-12">
            <div class="col-md-6 pull-left flex">
                <h6>Note : <span style="color: red !important;"><b>__</b></span> Indicates Mandatory Field</h6>
            </div>
            <div class="col-md-6 pull-right flex" style="visibility:hidden">
                <button type="button" id="btnList" onclick="GoToList()" class="btn btn-primary pull-right mb0 bfix"><i class="material-icons vl">arrow_back</i> Back To List</button>
            </div>
        </div>
    </div>--%>
    <div class="dashed"></div>
    <div class="pagewidth">
           <div class="flex__ end">
                <button type="button" id="btnList" onclick="GoToList()" class="btn btn-primary pull-right mb0 bfix"><i class="material-icons vl">arrow_back</i> Back To List</button>
            </div>
        <br />
        <div class="row">
            <div class="col-md-12">
                <div class="panel-group" id="accordion">
                    <%----------------------------------------- Panel 1 ---------------------------------------------%>
                    <div class="panel panel-default panelborder" id="panel1">
                        <div class="panel-heading accordpanel">
                            <a class="accord collapsed" data-toggle="collapse" data-target="#collapseOne">Vehicle Details</a>
                        </div>
                        <div id="collapseOne" class="panel-collapse collapse in">
                            <div class="panel-body" style="border-top-color: #fac18a !important;">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-6">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="txtVehicle" runat="server" class="ui-autocomplete-input" required="">
                                                    <label>Vehicle #</label>
                                                    <span class="errorMsg"></span>
                                                    <input type="hidden" class="p1save" id="Vehicle" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                   
                                    </div>
                                </div>
                          
                                <div class="row">
                                    <%--Storage Dimensions--%>
                                    <div class="col-md-6">
                                        
                                        <fieldset class="field flexlimit80">
                                            <legend class="lege">Availability</legend>


                                       <%--     <table class="table borderless">
                                             <thead>
                                                 <tr>
                                                 <th>From </th>
                                                     <th>To</th>

                                                 </tr>
                                             </thead>
                                            </table>--%>
                                             <div class="pull-right">
                                                   <button type="button" id="AvailAdd" class="btn btn-primary " data-toggle="modal" data-target="#AvailModal" onclick="myAvailclear()">Add<i class="material-icons" ></i></button>
                                                                    
                                    </div>
                                          <div class="row"></div>
                                              <!-- Modal -->
                                    <div id="AvailModal" class="modal fade">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color:#fff !important;">
                                                    <h4 class="modal-title" style="display: inline !important;">Availability Time</h4>
                                                    <button type="button"  data-dismiss="modal" class="pull-right modalclose" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" id="txtavailfrom" required=""/>
                                                                    <label>Available From</label><span class="errorMsg">*</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="flex">
                                                                <div>
                                                                   <input type="text" id="txtavailto" required=""/>
                                                                    <label>Available To</label><span class="errorMsg">*</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                   
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <input type="hidden" id="YM_TRN_VehicleAvailability_ID" value="0"/>
                                                    <button type="button" class="btn btn-secondary" style="color:#fff !important;" onclick="myAvailclear();">Clear</button>
                                                    <button type="button" class="btn btn-secondary" style="color:#fff !important;" data-dismiss="modal">Close</button>
                                                    <button type="button" class="btn btn-warning" onclick="AvailSave();">Save</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Modal -->
                                             <div class="row" id="AvailabilityvTable">
                                                    <%--Material Replenishment Table Append--%>
                                                </div>
                                        </fieldset>
                                    </div>

                                    <%--Down time--%>
                                    <div class="col-md-6">
                                        <fieldset class="field flexlimit80">
                                            <legend class="lege">Down Time </legend>
                                    <%--        <table class="table borderless">
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <div>
                                                            </div>
                                                            <div class="w280">
                                                                <input type="text" id="txtVUoM" runat="server" class="ui-autocomplete-input" required="">
                                                                <label>Vehicle UoM</label>
                                                                <span class="errorMsg"></span>
                                                                <input type="hidden" class="p1save" id="VUoM" runat="server" />
                                                            </div>
                                                        </div>


                                                    </td>
                                                </tr>
                                            
                                           
                                          
                                            </table>--%>
                                                  <div class="pull-right">
                                                   <button type="button" id="DownAdd" class="btn btn-primary " data-toggle="modal" data-target="#DownModal" onclick="myDownclear()">Add<i class="material-icons">add</i></button>
                                                                    
                                    </div>
                                            <div class="row"></div>
                                                  <!-- Modal -->
                                    <div id="DownModal" class="modal fade">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color:#fff !important;">
                                                    <h4 class="modal-title" style="display: inline !important;">Down Time</h4>
                                                    <button type="button"  data-dismiss="modal" class="pull-right modalclose" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="flex">
                                                                <div>
                                                                     <input type="text" id="txtDownfrom" required=""/>
                                                                   <%-- <input type="text" id="txtDownfrom" />--%>
                                                                    <label>Down From</label><span class="errorMsg">*</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="flex">
                                                                <div>
                                                                   <input type="text" id="txtDownto" required=""/>
                                                                    <label>Down To</label><span class="errorMsg">*</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="flex">
                                                                <div>
                                                                   <input type="text" id="txtDesc" required=""/>
                                                                    <label>Description</label><span class="errorMsg">*</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <input type="hidden" id="TM_TRN_VehicleDowntime_ID" value="0" />
                                                    <button type="button" class="btn btn-secondary" style="color:#fff !important;" onclick="myDownclear();">Clear</button>
                                                    <button type="button" class="btn btn-secondary" style="color:#fff !important;" data-dismiss="modal">Close</button>
                                                    <button type="button" class="btn btn-warning" onclick="DownTimeSave();">Save</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Modal -->
                                             <div class="row" id="DownvTable">
                                                    <%--Material Replenishment Table Append--%>
                                                </div>
                                        </fieldset>
                                    </div>

                                    <%--Weight Dimensions--%>
                                  <%--  <div class="col-md-4">
                                        <fieldset class="field flexlimit80">
                                            <legend class="lege">Weight Dimensions</legend>
                                            <table class="table borderless">
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <div>
                                                            </div>
                                                            <div class="w280">
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
                                                            <input type="text" id="WTare" maxlength="4" runat="server" onkeypress="return isNumberKeyEvent(event)" required="" style="width: 105% !important;">
                                                            <label>Tare Weight</label>
                                                            <span class="errorMsg"></span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="FormLabels">
                                                        <div class="flex">
                                                            <div>
                                                                <input type="text" id="WStorage" maxlength="4" runat="server" onkeypress="return isNumberKeyEvent(event)" required="" style="width: 105% !important;">
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
                                                                <input type="text" id="WTotal" maxlength="4" runat="server" onkeypress="return isNumberKeyEvent(event)" required="" style="width: 105% !important;">
                                                                <label>Max. Total Weight</label>
                                                                <span class="errorMsg"></span>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>--%>
                                </div>
                                <br />
                               <%-- <div style="display: flex; justify-content: flex-end">
                                    <div class="flex">
                                        <asp:LinkButton ID="lnkBasicVehicleSave" runat="server" CssClass="btn btn-primary" OnClick="lnkBasicVehicleSave_Click">Save <i class='fa fa-floppy-o'></i></asp:LinkButton>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                    <%----------------------------------------- Panel 1 ---------------------------------------------%>
                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        $(document).ready(function () {
                $("#txtavailfrom").datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date(),
                    onSelect: function (selected) {
                        var instance = $(this).data("datepicker");
                        var date = $.datepicker.parseDate(instance.settings.dateFormat || $.datepicker._defaults.dateFormat, selected, instance.settings);
                        date.setDate(date.getDate() + 1);
                        $("#txtavailto").datepicker("option", "minDate", date, { dateFormat: "dd-M-yy" })
                    }
                });

                $("#txtavailto").datepicker({
                    dateFormat: "dd-M-yy",
                    //maxDate: new Date()
                });

                //Added by kashyap on 21/08/2017  to reslove the server issue 
                $('#txtavailfrom, #txtavailto').keypress(function () {
                    return false;
                });  
                $('#txtavailfrom').keydown(function () {
                    return false;
                });
                $('#txtavailto').keydown(function () {
                    return false;
            });

             //$("#txtDownfrom").datepicker({
             //       dateFormat: "dd-M-yy",
             //       maxDate: new Date(),
             //       onSelect: function (selected) {
             //           var instance = $(this).data("datepicker");
             //           var date = $.datepicker.parseDate(instance.settings.dateFormat || $.datepicker._defaults.dateFormat, selected, instance.settings);
             //           date.setDate(date.getDate() + 1);
             //           $("#txtDownto").datepicker("option", "minDate", date, { dateFormat: "dd-M-yy" })
             //       }
             //   });

             //   $("#txtDownto").datepicker({
             //       dateFormat: "dd-M-yy",
             //       //maxDate: new Date()
             //   });

             //   //Added by kashyap on 21/08/2017  to reslove the server issue 
             //   $('#txtDownfrom, #txtDownto').keypress(function () {
             //       return false;
             //   });  
             //   $('#txtDownfrom').keydown(function () {
             //       return false;
             //   });
             //   $('#txtDownto').keydown(function () {
             //       return false;
             //   });
            var VehicleID= null;
         <%--   if (<%=this.cp.AccountID%> == 0 || <%=this.cp.AccountID%> == null) {
                $('#txtAccount').attr("disabled", false);
            }
            else {
                //debugger;
                $('#<%= this.txtAccount.ClientID %>').attr("disabled", true);
                $("#<%= this.txtAccount.ClientID %>").css("background-color", "#ebebe4");
                $("#<%= this.txtAccount.ClientID %>").val("<%=this.cp.Account%>");
                $("#<%= this.Account.ClientID %>").val(<%=this.cp.AccountID%>);
            }--%>
        });
        var VehicleID= null;
        $(function () {
            var accountfield = $('#<%= this.txtVehicle.ClientID %>');
            DropdownFunction(accountfield);
            $("#<%= this.txtVehicle.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadYardVehicles") %>',
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
                    $("#<%= this.Vehicle.ClientID %>").val(i.item.val);
                    $("#<%= this.txtVehicle.ClientID %>").val(i.item.label);

                    VehicleID = $("#<%= this.Vehicle.ClientID %>").val();
                    GetAvailabityList();
                    //LoadFreightBasedAcc(AccountID);
                    //LoadVehicleTypeBasedAcc(AccountID);
                },
                minLength: 0
            });

        });
            

        //function GoToList() {
        //    window.location.href = "VehicleMasterList.aspx";
        //}

        function isNumberKeyEvent(e) {
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        }
        var AvailabityList = null;
        function GetAvailabityList() {


            if (VehicleID != null) {
                debugger;
                $.ajax({
                    url: '<%=ResolveUrl("~/mYardManagement/VehicleAvailability.aspx/GetAvailabilityDownlist") %>',
                    //data: "{ 'prefix': '" + request.term + "'}",
                    data: "{'VehicleID' : '" + VehicleID + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        debugger;
                        var dt = JSON.parse(response.d);
                        AvailabityList = dt;
                      //  alert(AvailabityList);
                        LoadGrid(AvailabityList);
                        //GetPrefernces();
                        //BindPreferences(MasterID);
                    },
                    error: function (response) {

                    },
                    failure: function (response) {

                    }
                });
            }
            else {
                LoadGrid(AvailabityList);
            }

        }
         GetAvailabityList();
       
        function LoadGrid(dataList) {
            debugger;
           // var avblContainer = document.getElementById("AvailabilityvTable");
            var AvailabilityvTable = "<table class='table table-striped' style='text-align:center;width: 90%;margin: auto;'><thead><tr><th>Availability From</th><th>Availability To</th><th>Actions</th></tr></thead><tbody>";
            if (dataList != null) {
                if (dataList.Table != null && dataList.Table.length > 0) {
                    for (var i = 0; i < dataList.Table.length; i++) {
                        //debugger;
                        AvailabilityvTable += "<tr><td>" + dataList.Table[i].AvailableFrom + "</td><td>" + dataList.Table[i].AvailableTo + "</td><td ><span style='cursor:pointer !important;' data-toggle='modal' data-target='#AvailModal' onclick=EditAvaildetails(" + dataList.Table[i].YM_TRN_VehicleAvailability_ID + ");><i class='material-icons vl'>edit</i></span><span style='cursor:pointer !important;'  onclick=DeleteAvaildetails(" + dataList.Table[i].YM_TRN_VehicleAvailability_ID + ");><i class='material-icons vl'>delete</i></span></td></tr>";
                    }
                }
            }
            else {
                AvailabilityvTable += "<tr><td colspan='8' style='text-align:center'><b>No Data Found</b></td></tr>";
            }
            AvailabilityvTable += "</tbody></table>";
            $('#AvailabilityvTable').html(AvailabilityvTable);
            //avblContainer.innerHTML = AvailabilityvTable;


            var DownvTable = "<table class='table table-striped' style='text-align:center;width:90%;margin:auto'><thead><tr><th>Downtime From </th><th>DownTime To</th><th>Description</th><th>Actions</th></tr></thead><tbody>";
            if (dataList != null) {
                if (dataList.Table1 != null && dataList.Table1.length > 0) {
                    for (var i = 0; i < dataList.Table1.length; i++) {
                        //debugger;
                        DownvTable += "<tr><td>" + dataList.Table1[i].DowntimeFrom + "</td><td>" + dataList.Table1[i].DowntimeTo + "</td><td>" + dataList.Table1[i].Description + "</td><td ><span style='cursor:pointer !important;' onclick=Editdowndetails(" + dataList.Table1[i].TM_TRN_VehicleDowntime_ID + ");><i class='material-icons vl'>edit</i></span><span style='cursor:pointer !important;'  onclick=DeleteDowntimedetails(" + dataList.Table1[i].TM_TRN_VehicleDowntime_ID + ");><i class='material-icons vl'>delete</i></span></td></tr>";
                    }
                }
            }
            else {
                DownvTable += "<tr><td colspan='8' style='text-align:center'><b>No Data Found</b></td><tr>";
            }
            DownvTable += "</tbody></table>";
            $('#DownvTable').html(DownvTable);

        }
        
            //debugger;
            
  
        function Success(vid) {
            showStickyToast(true, "Basic Details Saved Successfully", false);
            setTimeout(function () {
                window.location.replace("VehicleMasterRequest.aspx?vid=" + vid);
            }, 1500);
        }
        function AvailSave() {         
            debugger;
            if (VehicleID == null) {
                showStickyToast(false, "Please Select the Vehicle", false);
                return false;

            }
            var AvailFrom = $("#txtavailfrom").val();
            var vehicleavailid = $("#YM_TRN_VehicleAvailability_ID").val();
            //var vehicleid = $("#YM_TRN_VehicleAvailability_ID").val();
            //ltMMT_GUoMID = ltMMT_GUoMID == "" ? "0" : txtavailto;
            var Availto = $("#txtavailto").val();
            
            if (AvailFrom == '' || AvailFrom == null) {
                showStickyToast(false, "Please enter Avail From date", false);
                return false;
            }
            if (Availto == '' || Availto == "PS") {
                showStickyToast(false, "Please enter Avail To date", false);
                return false;
            }
             var Availcheck = $.grep(AvailabityList.Table, function (a) { return a.AvailableFrom == AvailFrom && a.AvailableTo == Availto && a.YM_TRN_VehicleAvailability_ID!= vehicleavailid});
                 if (Availcheck.length > 0) {
                     showStickyToast(false, "Record already Exists", false);
                         return false;
                 }
           
            $.ajax({

                       url: '<%=ResolveUrl("~/mYardManagement/VehicleAvailability.aspx/UpsertVehicleAvailability") %>',
                       data: "{'AvailFrom' : '" + AvailFrom + "','AvailTo' : '" + Availto + "','VehicleID': "+VehicleID+",'VehicleAvailbilityID':"+vehicleavailid+"}",
                       dataType: "json",
                       type: "POST",
                       contentType: "application/json; charset=utf-8",
                       success: function (response) {
                             debugger;
                             var save = response.d;
                             if (save == "") {
                                 showStickyToast(true, "Availability Details Saved Successfully", false);
                                  GetAvailabityList();
                                 //GetItemMasterDetails(MaterialMasterID);
                                 //myUoMclear();
                                 $("#AvailModal").modal('hide');
                             }
                             else {
                                 showStickyToast(false, "Error while Updating", false);
                                 $("#AvailModal").modal('hide');
                                 myUoMclear();
                             }
                         }
                     });
                 }
                
         function DownTimeSave() {
             
             debugger;
                 if (VehicleID == null) {
                showStickyToast(false, "Please Select the Vehicle", false);
                return false;

            }
            var DowntimeFrom = $("#txtDownfrom").val();
            var vehicleavailid = $("#TM_TRN_VehicleDowntime_ID").val();
            //var vehicleid = $("#YM_TRN_VehicleAvailability_ID").val();
            //ltMMT_GUoMID = ltMMT_GUoMID == "" ? "0" : txtavailto;
             var Downtimeto = $("#txtDownto").val();
              var Description = $("#txtDesc").val();
            
            if (DowntimeFrom == '' || DowntimeFrom == null) {
                showStickyToast(false, "Please enter Down From date", false);
                return false;
            }
            if (Downtimeto == '' || Downtimeto == null) {
                showStickyToast(false, "Please enter Down To date", false);
                return false;
             }
              if (Description == '') {
                showStickyToast(false, "Please enter description", false);
                return false;
             }
             if (DowntimeFrom != '') {
                 var check = $.grep(AvailabityList.Table1, function (a) { return a.DowntimeFrom == DowntimeFrom && a.DowntimeTo == Downtimeto && a.TM_TRN_VehicleDowntime_ID!= vehicleavailid});
                 if (check.length > 0) {
                     showStickyToast(false, "Record already Exists", false);
                         return false;
                 }
             }
           
              $.ajax({
                        url: '<%=ResolveUrl("~/mYardManagement/VehicleAvailability.aspx/UpsertVehicleDowntime") %>',
                         data: "{'DowntimeFrom' : '" + DowntimeFrom + "','DowntimeTo' : '" + Downtimeto + "','Description': '"+Description+"','VehicleID': "+VehicleID+",'VehicleDownID':"+vehicleavailid+"}",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (response) {
                             debugger;
                             var save = response.d;
                             if (save == "") {
                                 showStickyToast(true, "Down time Details Saved Successfully", false);
                                  GetAvailabityList();
                                 //myUoMclear();
                                 $("#DownModal").modal('hide');
                             }
                             else {
                                 showStickyToast(false, "Error while Updating", false);
                                 $("#DownModal").modal('hide');
                                 myDownclear();
                             }
                         }
                     });
        }

        function myAvailclear() {
           $("#txtavailfrom").val("");
           $("#YM_TRN_VehicleAvailability_ID").val(0);          
           $("#txtavailto").val("");
        }
         
        function myDownclear() {
           $("#txtDownfrom").val("");
           $("#TM_TRN_VehicleDowntime_ID").val(0);          
           $("#txtDownto").val("");
           $("#txtDesc").val("");
        }   
       function EditAvaildetails(AvailabilityID)
        {
           debugger;
           var item = $.grep(AvailabityList.Table, function (a) { return a.YM_TRN_VehicleAvailability_ID == AvailabilityID });
             $("#AvailModal").modal({
                show: 'true'
            });
            $("#txtavailfrom").val(item[0].AvailableFrom);
            $("#YM_TRN_VehicleAvailability_ID").val(AvailabilityID);   
            $("#txtavailto").val(item[0].AvailableTo);

        }

        function DeleteAvaildetails(AvailabilityID) {
            DeleteItem(AvailabilityID, '', '');
        }

        function DeleteDowntimedetails(DowntimeID) {
            debugger;
            DeleteDownItem(DowntimeID, '', '');
        }
        
        function DeleteItem(id, procname, header) {
            if (confirm("Are u sure you want to Delete ?")) {
                var obj = {};
                obj.StrId = id;
                $.ajax({
                    url: "VehicleAvailability.aspx/DeleteVehAvailability",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    data: JSON.stringify(obj),
                    success: function (response) {
                        if (response.d == "success") {
                            showStickyToast(true, 'Deleted Successfully');
                             GetAvailabityList();
                        }
                    }
                });
            }
        }
          function DeleteDownItem(id, procname, header) {
            if (confirm("Are u sure you want to Delete ?")) {
                var obj = {};
                obj.StrId = id;
                debugger;
                $.ajax({
                    url: "VehicleAvailability.aspx/DeleteVehDowntime",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    data: JSON.stringify(obj),
                    success: function (response) {
                        if (response.d == "success") {
                            showStickyToast(true, 'Deleted Successfully');
                             GetAvailabityList();
                        }
                    }
                });
            }
        }
        function Editdowndetails(DowntimeID) {
              var item = $.grep(AvailabityList.Table1, function (a) { return a.TM_TRN_VehicleDowntime_ID == DowntimeID });
             $("#DownModal").modal({
                show: 'true'
            });
            $("#txtDownfrom").val(item[0].DowntimeFrom);
            $("#TM_TRN_VehicleDowntime_ID").val(DowntimeID);   
            $("#txtDownto").val(item[0].DowntimeTo);
            $("#txtDesc").val(item[0].Description);
        }

    </script>
      <link href="Scripts/Datepicker/jquery.datetimepicker.css" rel="stylesheet" />
<%--    <script src="Scripts/Datepicker/jquery.datetimepicker.js"></script>--%>
  <%--  <script src="Scripts/Datepicker/jquery.js"></script>--%>
    <script src="Scripts/Datepicker/jquery.datetimepicker.full.js"></script>

           <script type="text/javascript">
           $(document).ready(function () {
               $('#txtDownfrom').datetimepicker({
                   formatTime: 'H:i',
                   formatDate: 'd.m.y',
                   //defaultDate:'8.12.1986', // it's my birthday
                   defaultDate: '+03-Jan-1970', // it's my birthday
                   defaultTime: '1:00',
                   step:5,
                   timepickerScrollbar: false,
                   onShow: function (ct) {
                       this.setOptions({
                       
                           maxDate: jQuery('#txtDownto').val() ? jQuery('#txtDownto').val() : false,
                          
                         //  maxTime:jQuery('#txtDownto').val() ? jQuery('#txtDownto').val() : false
                       });
                   }
               });
                 $('#txtDownto').datetimepicker({
                   formatTime: 'H:i',
                   formatDate: 'd.m.y',
                   //defaultDate:'8.12.1986', // it's my birthday
                   defaultDate: '+03-Jan-1970', // it's my birthday
                   defaultTime: '1:00',
                   step:5,
                     timepickerScrollbar: false,
                   onShow:function( ct ){
                    this.setOptions({
                        minDate: jQuery('#txtDownfrom').val() ? jQuery('#txtDownfrom').val() : false,
                      //  minTime:jQuery('#txtDownfrom').val() ? jQuery('#txtDownfrom').val() : false
                     })
                 },
               });
           });
        </script>
</asp:Content>
