<%@ Page  Title=" Customer Request :." Language="C#" AutoEventWireup="true" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" CodeBehind="Customer.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.Customer" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MMContent" runat="server">
    <asp:ScriptManager ID="mySManager" EnablePageMethods="true" EnablePartialRendering="true" runat="server" SupportsPartialRendering="true">
    </asp:ScriptManager>
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <style>
        .requiredlabel:before {
            content: " * ";
            color: red;
        }

     

    </style>
    <script lang="javascript" type="text/javascript">

        function validate(value) {
            var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
            if (reg.test(value) == false) {
                showStickyToast(false, 'Please provide a valid email address');
                return false;
            }
        }
    </script>

      <asp:UpdateProgress ID="uprgSearchOutbound" runat="server" AssociatedUpdatePanelID="upnlSearchOutbound">
            <ProgressTemplate>
                <div style="width: 100%; height: 100%; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: #e0ddd8ba;">

                    <div style="align-self: center;">
                        <div class="spinner">
                            <div class="bounce1"></div>
                            <div class="bounce2"></div>
                            <div class="bounce3"></div>
                        </div>

                    </div>

                </div>
                                
                                
            </ProgressTemplate>
        </asp:UpdateProgress>
       <div class="dashed"></div>
    <!-- Globalization Tag is added for multilingual  -->
    <div class="pagewidth">
        <asp:UpdatePanel ID="upnlSearchOutbound" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="width:100%; display:flex; justify-content:flex-end;">
                <%--<asp:LinkButton ID="lnkbacktolist" CssClass="btn btn-sm btn-primary" runat="server" PostBackUrl="~/mMaterialManagement/CustomerList.aspx"><i class="material-icons vl">arrow_back</i>Back to List </asp:LinkButton>&emsp;&emsp;--%>
                     <asp:LinkButton ID="lnkbacktolist" CssClass="btn btn-sm btn-primary" runat="server" PostBackUrl="~/mMaterialManagement/CustomerList.aspx"><i class="material-icons vl">arrow_back</i> <%= GetGlobalResourceObject("Resource", "BacktoList")%> </asp:LinkButton>&emsp;&emsp;
                 </div>
                <div>


<%--                    <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvPOHDHeader" style="font-size: 13pt">Customer Header</div>--%>
                    <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvPOHDHeader" style="font-size: 13pt"> <%= GetGlobalResourceObject("Resource", "CustomerHeader")%></div>
                    <div class="ui-Customaccordion" id="dvPOHDBody">

                        <div class="dashedborder"></div>

                        <div class="container-fluid">
                            <div style="color: red !important; margin-left: 8% !important;" id="divErrors"></div>
                            <gap5></gap5> <gap5></gap5>
                            <div class="row">
                                <div class="col m3 s3">
                                    <div class="flex">
                                        <select id="AccountID" class="fieldToGet" required="">                               
                                        </select>
                                        <span class="errorMsg"></span>
                                       <%-- <label>Account</label>--%>
                                        <label> <%= GetGlobalResourceObject("Resource", "Account")%></label>
                                    </div>
                                </div>

                                <div class="col m3 s3">
                                    <div class="flex">
                                        <select id="TenantID" class="fieldToGet" required="">
                                        </select>
                                        <span class="errorMsg"></span>
                                        <%--<label>Tenant</label>--%>
                                        <label>  <%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                                    </div>
                                </div>

                                <div class="col m3 s3">
                                    <div class="flex">
                                        <input type="text" id="CustomerName" class="fieldToGet" maxlength="50" required="" />
                                        <span class="errorMsg"></span>
                                      <%--  <label>Customer Name </label>--%>
                                         <label> <%= GetGlobalResourceObject("Resource", "CustomerName")%> </label>
                                    </div>
                                </div>

                          
                                <div class="col m3 s3">
                                    <div class="flex">
                                        <input type="text" id="CustomerCode" class=" fieldToGet" maxlength="10" required="" />
                                        <span class="errorMsg"></span>
                                        <%--<label>Customer Code</label>--%>
                                         <label><%= GetGlobalResourceObject("Resource", "CustomerCode")%> </label>

                                    </div>
                                </div>
                                </div>
                           
                            <div class="row">
                                <div class="col m3 s3">
                                    <div class="flex">
                                        <input type="text" id="EmailAddress" class="fieldToGet" maxlength="30" required="" />
                                      <%--  <label>Email</label>--%>
                                         <label><%= GetGlobalResourceObject("Resource", "Email")%> </label>

                                    </div>
                                </div>

                                <div class="col m3 s3">
                                    <div class="flex">
                                        <input type="text" id="Mobile" class="fieldToGet" maxlength="10" onpaste="return false;" onkeypress="return isNumberKey(event)" required="" />
                                       <%-- <label>Mobile</label>--%>
                                        <label> <%= GetGlobalResourceObject("Resource", "Mobile")%></label>

                                    </div>
                                </div>

                          
                                <div class="col m6">
                                    <gap></gap><gap></gap>                                    <div flex end>
                                        <%--<button type="button" class="btn btn-primary" id="btnCustomer" style="font-size: 12pt" onclick="return UpsertCustomer();">Create</button>--%>
                                         <button type="button" class="btn btn-primary" id="btnCustomer" onclick="return UpsertCustomer();"> <%= GetGlobalResourceObject("Resource", "Create")%></button>
                                        <%--<button type="button" class="btn btn-primary" id="btnClear" style="font-size: 12pt" onclick="ClearHeader()">Clear</button>--%>
                                         <button type="button" class="btn btn-primary" id="btnClear"  onclick="ClearHeader()"> <%= GetGlobalResourceObject("Resource", "Clear")%> </button>
                                    </div>

                                    <input type="hidden" value="0" id="CustomerID" class="fieldToGet AddressfieldToGet " />
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                  <%--  <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divCusAddress" style="font-size: 13pt">Customer Address Details</div>--%>
                     <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divCusAddress" style="font-size: 13pt"> <%= GetGlobalResourceObject("Resource", "CustomerAddressDetails")%></div>
                    <div class="ui-Customaccordion" id="dvCusAddressBody">

                            <div>
                                <br />
                                <div id="AddressList" style="display: none;">
                                    <div style="display: flex; justify-content: flex-end;"><button type='button' id='btnCreateAddress' class="btn btn-primary" data-toggle='modal' data-target='#AurthoBrief' onclick='ClearAddressData();'> <%= GetGlobalResourceObject("Resource", "Add")%><i class="material-icons">add</i></button></div>
                                    <table class="table table-striped table-bordered  table-hover dataTables-example-Address " id="tblAddressList" style="display:none"></table>
                                </div>
                            </div>
                        <div class="modal inmodal" id="AurthoBrief" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog" style="width: 50%;">
                                <div class="modal-content animated fadeIn">
                                    <div class="modal-header">
<%--                                        <button type="button" class="close btn btn-primary" style="font-size: 12pt" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only"><%= GetGlobalResourceObject("Resource", "Close")%></span></button>--%>
                                       <%-- <h3 class="modal-title">Customer Address</h3>--%>
                                        <h3 class="modal-title"> <%= GetGlobalResourceObject("Resource", "CustomerAddress")%></h3>
                                    </div>

                                    <div class="modal-body">

                                        <div id="divAddressValidationMessages" style="color: red"></div>
                                        <p></p>
                                        <div id="divAddressDetails" class="form-horizontal">
                                            <form role="form">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <div class="flex">
                                                            <select id="GEN_MST_AddressType_ID" class=" AddressfieldToGet" required="">
                                                            </select>
                                                            <span class="errorMsg"></span>
                                                           <%-- <label>Address Type</label>--%>
                                                             <label> <%= GetGlobalResourceObject("Resource", "AddressType")%></label>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                      
                                                        <div class="flex">
                                                            <textarea id="AddressLine1" class=" AddressfieldToGet" maxlength="95" required="" >  </textarea>
                                                            <span class="errorMsg"></span>
                                                            <%--<label>Address Line 1</label>--%>
                                                            <label> <%= GetGlobalResourceObject("Resource", "AddressLine")%></label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <div class="flex">

                                                            <textarea id="AddressLine2" class=" AddressfieldToGet" maxlength="95" required="" ></textarea>
                                                            <%--<label>Address Line 2</label>--%>
                                                            <label> <%= GetGlobalResourceObject("Resource", "AddressLines")%></label>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="flex">
                                                            <select id="GEN_MST_Country_ID" class=" AddressfieldToGet" onchange="LoadStates()" required="">
                                                            </select>
                                                            <span class="errorMsg"></span>
                                                          <%--  <label>Country</label>--%>
                                                              <label> <%= GetGlobalResourceObject("Resource", "Country")%></label>

                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <div class="flex">

                                                            <select id="GEN_MST_State_ID" class=" AddressfieldToGet" onchange="LoadCities()" required="">
                                                            </select>
                                                            <span class="errorMsg"></span>
                                                           <%-- <label>State</label>--%>
                                                             <label> <%= GetGlobalResourceObject("Resource", "State")%></label>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="flex">

                                                            <select id="GEN_MST_City_ID" class=" AddressfieldToGet" onchange="LoadZipCodes()" required="">
                                                            </select>
                                                            <span class="errorMsg"></span>
                                                          <%--  <label>City</label>--%>
                                                            <label>  <%= GetGlobalResourceObject("Resource", "City")%> </label>

                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <div class="flex">

                                                            <select id="GEN_MST_ZipCode_ID" class=" AddressfieldToGet" required="" style="margin-top:7px;">
                                                            </select>
                                                            <span class="errorMsg"></span>
                                                            <label> <%= GetGlobalResourceObject("Resource", "ZipCode")%> </label>

                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="flex">

                                                            <input type="text" id="DeliveryPoint" class=" AddressfieldToGet" required="" maxlength="5">
                                                            <span class="errorMsg"></span>
                                                           <%-- <label>Delivery Site Code</label>--%>
                                                             <label> <%= GetGlobalResourceObject("Resource", "DeliverySiteCode")%></label>
                                                        </div>
                                                    </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-md-6">
                                                            <%--<div class="flex">
                                                                    <input type="text" id="latitude" class=" AddressfieldToGet" required="" disabled />
                                                                    <label style="margin-bottom:2px;">Latitude</label>
                                                            </div>--%>
                                                     </div>

                                                        <div class="col-md-6">
                                                            <%--<div class="flex">
                                                                    <input type="text" id="longitude" class=" AddressfieldToGet" required="" disabled />
                                                                  <label>Longitude</label>
                                                            </div>--%>
                                                       </div>
                                                    </div>
                                                    
                                                </div>
                                            </form>
                                        </div>



                                    </div>
                                <div class="modal-footer">
                                     <button type="button" id="btncancelad" class="btn btn-primary" onclick="ClearAddressData()"><%= GetGlobalResourceObject("Resource", "Clear")%><i class="material-icons">clear_all</i></button>

                                    <button type="button" class=" btn btn-primary" data-dismiss="modal"><%= GetGlobalResourceObject("Resource", "Close")%> <i class="material-icons">close</i></button>
                                   
                                    <button type="button" id="btnaddress" class="btn btn-primary" onclick="UpsertAddress()"><%= GetGlobalResourceObject("Resource", "Create")%> <i class="material-icons">add</i></button>
                                    <input type="hidden" value="0" id="GEN_MST_Address_ID" class="AddressfieldToGet" />

                                </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <%--<div id="divDeleteCustomer" class="divDeleteCustomer">
                    <div>
                        <span id="spanDeleteCustomer"></span>
                    </div>        
                </div>--%>

               <div id="divDeleteCustomer"></div>
    
            </ContentTemplate>
        </asp:UpdatePanel>
  </div>

     <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB-amPYw4EvJGyYfY16HzhF2lqpw--FcHM&libraries=places"></script>
    <script>

        var CustomerList;

        $(document).ready(function () {

            CustomAccordino($('#dvPOHDHeader'), $('#dvPOHDBody'));
            CustomAccordino($('#divCusAddress'), $('#dvCusAddressBody'));
            CustomerList = null
            GetCustomerList();


        });
        var ItemList = null;
        function GetCustomerList() {
            $.ajax({
                url: "Customer.aspx/GetCustomerList",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                data: {},
                success: function (response) {

                    CustomerList = JSON.parse(response.d);
                    var CusEditId = new URL(window.location.href).searchParams.get("CusId");
                    if (CusEditId != 0) {
                        $("#btnClear").hide();
                    }
                    LoadDropDowns();
                    BindCustomer(CusEditId);
                    LoadAddressDropdowns(CustomerList);
                    var CusAddress = $.grep(CustomerList.Table1, function (a) { return a.CustomerID == CusEditId });
                    ItemList = CusAddress;
                    LoadAddressGrid(CusAddress);
                }
            });
        }


        function BacktoList() {
            window.location.href = "CustomerList.aspx";
        }

        function UpsertCustomer() {
            debugger;
            if (ValidateCustomer()) {

                var obj = {};
                obj.CusId = $("#CustomerID").val();
                obj.Inxml = GetAddressFormData('.fieldToGet');
                $.ajax({
                    url: "Customer.aspx/SetCustomer",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    data: JSON.stringify(obj),
                    success: function (response) {
                        debugger;
                        if (response.d == 1) {
                            var CusEditId = new URL(window.location.href).searchParams.get("CusId");

                            if (CusEditId == "0" || CusEditId == null || CusEditId == undefined) {
                                showStickyToast(true, 'Saved successfully');
                                setTimeout(function () {
                                    window.location.href = "CustomerList.aspx";
                                }, 500);
                            }
                            else {

                                showStickyToast(true, 'Updated successfully');
                            }
                            
                        }
                        
                        else {
                            if (response.d == -111) {
                                showStickyToast(false, "Customer name already exists");
                                return false;
                            }
                            if (response.d == -222) {
                                showStickyToast(false, "Customer code already exists");
                                return false;
                            }
                        }
                        
                    }

                });
            }
        }

        var geocoder = new google.maps.Geocoder();
        var latitude, longitude,DeliveryPoint;

        //function UpsertAddress() {
        //    ////debugger;
        //    if (ValidateAddress()) {
        //       debugger;
        //        //get latitude and longitude of that zip code

        //        var e = document.getElementById("GEN_MST_ZipCode_ID");
        //        var address = e.options[e.selectedIndex].text;
        //        geocoder.geocode({ 'address': address }, function (results, status) {

        //            if (status == google.maps.GeocoderStatus.OK) {

        //                latitude = results[0].geometry.location.lat();
        //                longitude = results[0].geometry.location.lng();
        //                //alert("Latitude: " + latitude + "\nLongitude: " + longitude);
        //                $('#latitude').val(latitude);
        //                $('#longitude').val(longitude);

        //                var obj = {};
        //                obj.AddressId = $("#GEN_MST_Address_ID").val();
        //                //debugger;
        //                obj.Inxml = GetAddressFormData('.AddressfieldToGet');
        //                //obj.Latitude = latitude;
        //                //obj.Longitude = longitude;
        //                obj.DeliveryPoint =$("#DeliveryPoint").val();
        //                $.ajax({
        //                    url: "Customer.aspx/SetAddress",
        //                    dataType: 'json',
        //                    contentType: "application/json",
        //                    type: 'POST',
        //                    data: JSON.stringify(obj),
        //                    success: function (response) {
        //                        if (response.d == "success") {

        //                            GetCustomerList();
        //                            ////debugger;
        //                            //$("#DeliveryPoint").val(ItemList[0].DeliveryPoint);
        //                            setTimeout(function () {
        //                                showStickyToast(true, 'Saved Successfully');
        //                                ClearAddressData();
        //                                $('#AurthoBrief').modal('hide');
        //                            }, 500);
        //                        }
        //                    }

        //                });



        //            } else {
        //                alert("Request failed.")
        //            }
        //        });
        //    }
        //}
         function UpsertAddress() {
            ////debugger;
            if (ValidateAddress()) {
               debugger;
                //get latitude and longitude of that zip code

               

                        
                        //alert("Latitude: " + latitude + "\nLongitude: " + longitude);
                        
                        var obj = {};
                        obj.AddressId = $("#GEN_MST_Address_ID").val();
                        //debugger;
                        obj.Inxml = GetAddressFormData('.AddressfieldToGet');
                        //obj.Latitude = latitude;
                        //obj.Longitude = longitude;
                        obj.DeliveryPoint =$("#DeliveryPoint").val();
                        $.ajax({
                            url: "Customer.aspx/SetAddress",
                            dataType: 'json',
                            contentType: "application/json",
                            type: 'POST',
                            data: JSON.stringify(obj),
                            success: function (response) {
                                if (response.d == "success") {

                                    GetCustomerList();
                                    debugger;
                                    //$("#DeliveryPoint").val(ItemList[0].DeliveryPoint);
                                    setTimeout(function () {
                                        showStickyToast(true, 'Saved Successfully');
                                        ClearAddressData();
                                        $('#AurthoBrief').modal('hide');
                                    }, 500);
                                }
                            }

                        });



                    } else {
                        //alert("Request failed.")
                    }
            
        }
        function CancelAddress() {

        }
        function ValidateAddress() {
            debugger;
            $("#divAddressValidationMessages").empty();
            var IsValid = true;
            //debugger;
            if ($('#GEN_MST_AddressType_ID').val() == "0" || $('#AddressLine1').val().trim() == "" || $('#GEN_MST_City_ID').val() == "0" || $('#GEN_MST_State_ID').val() == "0" || $('#GEN_MST_Country_ID').val() == "0" || $('#GEN_MST_ZipCode_ID').val() == "0") {
                IsValid = false;
                showStickyToast(false, 'Please check all mandatory fields', false);
                return false;
                // AddressAppendMessage("Please select Address Types.");
            }
             if ( $('#DeliveryPoint').val().trim() == "" || $('#DeliveryPoint').val() == null || $('#DeliveryPoint').val() == undefined) {
                IsValid = false;
                showStickyToast(false, 'Please check all mandatory fields', false);
                  return false;
                // AddressAppendMessage("Please select Address Types.");
            }
        
            
            ////debugger;
            if ($('#GEN_MST_AddressType_ID').val() != "0") {
                if ($('#GEN_MST_AddressType_ID').val() == "1") {
                    var AddressRecords = $.grep(CustomerList.Table1, function (a) { return a.GEN_MST_AddressType_ID == $("#GEN_MST_AddressType_ID").val() && a.GEN_MST_Address_ID != $("#GEN_MST_Address_ID").val() && a.CustomerID == $("#CustomerID").val() });
                    if (AddressRecords.length > 0) {
                        IsValid = false;
                        showStickyToast(false, 'Corporate office already Exists', false);
                         return false;
                    }
                }
                }

                if ($('#DeliveryPoint').val().trim() != "" || $('#DeliveryPoint').val() != null || $('#DeliveryPoint').val() != undefined) {
                var deliverypoint = $.grep(CustomerList.Table1, function (a) { return a.DeliveryPoint == $('#DeliveryPoint').val().trim() && a.CustomerID == $("#CustomerID").val() && a.GEN_MST_Address_ID!=$("#GEN_MST_Address_ID").val() });
                if (deliverypoint.length > 0) {
                    IsValid = false;
                    showStickyToast(false, 'Delivery Site Code already Exists', false);
                     return false;
                }
                // AddressAppendMessage("Please select Address Types.");
            }
            //if ($('#AddressLine1').val().trim() == "") {
            //    IsValid = false;
            //    AddressAppendMessage("Please Enter Address Line1.");
            //}
            //if ($('#AddressLine2').val().trim() == "") {
            //    IsValid = false;
            //    AddressAppendMessage("Please Enter Address Line2.");
            //}

            //if ($('#GEN_MST_City_ID').val() == "0") {
            //    IsValid = false;
            //    AddressAppendMessage("Please select city.");
            //}
            //if ($('#GEN_MST_State_ID').val() == "0") {
            //    IsValid = false;
            //    AddressAppendMessage("Please select State.");
            //}
            //if ($('#GEN_MST_Country_ID').val() == "0") {
            //    IsValid = false;
            //    AddressAppendMessage("Please select Country.");
            //}
            //if ($('#GEN_MST_ZipCode_ID').val() == "0") {
            //    IsValid = false;
            //    AddressAppendMessage("Please select Zip.");
            //}

            if ($('#GEN_MST_AddressType_ID').val() != "0" && $('#AddressLine1').val().trim() != "" && $('#AddressLine2').val().trim() != "" && $('#GEN_MST_City_ID').val() != "0"
                && $('#GEN_MST_State_ID').val() != "0" && $('#GEN_MST_Country_ID').val() != "0" && $('#GEN_MST_ZipCode_ID').val() != "0") {

                var AddressRecords = $.grep(CustomerList.Table1, function (a) { return a.CustomerID == $("#CustomerID").val() && a.GEN_MST_Address_ID != $("#GEN_MST_Address_ID").val() });

                var AddressExist = $.grep(AddressRecords, function (a) {
                    return a.CountryMasterID == $("#GEN_MST_Country_ID").val() && a.StateMasterID == $("#GEN_MST_State_ID").val() && a.CityMasterID == $("#GEN_MST_City_ID").val()
                        && a.ZipCodeID == $("#GEN_MST_ZipCode_ID").val() && a.GEN_MST_AddressType_ID == $("#GEN_MST_AddressType_ID").val() && a.AddressLine1 == $("#AddressLine1").val() && a.AddressLine2 == $("#AddressLine2").val()
                });
               
                if (AddressExist.length != 0) {
                    IsValid = false;
                    // AddressAppendMessage("Record Already Exists.");
                    showStickyToast(true, 'Record Already Exists', false);
                     return false;
                }

            }

            return IsValid;
        }

        function AddressAppendMessage(Message) {
            $("#divAddressValidationMessages").append("<li style='margin-top: 0.3em;'>" + Message + "</li>");
        }

        function LoadDropDowns() {
            $("#TenantID").empty();

            $("#TenantID").append($("<option></option>").val(0).html("Please Select"));
            for (var x = 0; x < CustomerList.Table2.length; x++) {
                $("#TenantID").append($("<option></option>").val(CustomerList.Table2[x].TenantID).html(CustomerList.Table2[x].TenantName));
            }

            $("#AccountID").empty();

            $("#AccountID").append($("<option></option>").val(0).html("Please Select"));
            for (var x = 0; x < CustomerList.Table8.length; x++) {
                $("#AccountID").append($("<option></option>").val(CustomerList.Table8[x].AccountID).html(CustomerList.Table8[x].Account));
            }

            if (CustomerList.Table8.length == 1) {
                $("#AccountID").val($("#AccountID option").eq(1).val());
                $("#AccountID").attr('disabled', 'disabled');
            }
            else {
                $("#AccountID").removeAttr('disabled');
            }

        }



        function BindCustomer(CusId) {
            ////debugger;
            if (CusId != 0) {
                var Customer = $.grep(CustomerList.Table, function (a) { return a.CustomerID == CusId });
                BuildFormtoEditAddress(Customer, '.fieldToGet');
                $("#btnCustomer").html("Update <i class='material-icons'>update</i>");
                $("#AddressList").show();

            }

            else {
                ClearFields();
                $("#AddressList").hide();
            }
        }

        function ClearFields() {
            $('#TenantID').val(0);
            $('#CustomerName,#CustomerCode,#EmailAddress,#Mobile').val("");
        }

        function isValidEmailAddress(emailAddress) {

            var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);

            var result = pattern.test(emailAddress);

            return result;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }




        function ValidateCustomer() {
            debugger;
            var tenid = $('#TenantID').val();

            $("#divErrors").empty();
            var IsValid = true;

            if ($('#TenantID').val() == "0" || $('#CustomerName').val().trim() == "" || $('#CustomerCode').val().trim() == "") {                
                showStickyToast(false, 'Please check all mandatory fields', false);
                return false;
            }
                        

            //if ($('#CustomerCode').val().trim() != "" && $('#CustomerCode').val().trim().length != 0) {

            //    //var retval = CheckDuplicate(CustomerList.Table, "CustomerCode", $('#CustomerCode').val().trim(), "CustomerID");
            //    //if (retval == false) {                   
            //    //    showStickyToast(false, 'Customer Code Already Exists.', false);
            //    //    return false;
            //    //}
            //    var PK = "CustomerID";
                
            //    var item = CustomerList.Table, Count = 0;

            //    //if ($('#CCM_CNF_AccountCycleCount_ID').val() != 0) {

            //    //    item = $.grep(ItemList.Table5, function (data) {
            //    //        return data['CCM_CNF_AccountCycleCount_ID'] != $('#CCM_CNF_AccountCycleCount_ID').val();
            //    //    });
            //    //}

            //    if ($('#' + PK).val() != 0) {
            //        item = $.grep(obj, function (data) {
            //            return data[PK] != $('#' + PK).val();
            //        });
            //    }

            //    Count = $.grep(item, function (data) {

            //        return data['CustomerCode'] == $('#CustomerCode').val().trim() && data['TenantID'] == $('#TenantID').val().trim();
            //    });

            //    if (Count.length != 0) {
            //        IsValid = false;
            //        showStickyToast(false, 'Customer Code Already Exists.', false);
            //        return false;
            //    }
            

            //}

            var eMail = $('#EmailAddress').val();
            if (eMail != "") {
                if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(eMail)) {

                }
                else {
                    showStickyToast(false, 'Please enter valid Email', false);
                    return false;
                }
            }
           

            //if ($('#CustomerName').val().trim() != "" && $('#CustomerName').val().trim().length != 0) {

            //    //var retval = CheckDuplicate(CustomerList.Table, "CustomerName", $('#CustomerName').val().trim(), "CustomerID");
            //    //if (retval == false) {                   
            //    //    showStickyToast(false, 'Customer Name Already Exists.', false);
            //    //    return false;
            //    //}

            //     var PK = "CustomerID";
                
            //    var item = CustomerList.Table, Count = 0;
            //    if ($('#' + PK).val() != 0) {
            //        item = $.grep(obj, function (data) {
            //            return data[PK] != $('#' + PK).val();
            //        });
            //    }

            //    Count = $.grep(item, function (data) {

            //        return data['CustomerName'] == $('#CustomerName').val().trim() && data['TenantID'] == $('#TenantID').val().trim();
            //    });

            //    if (Count.length != 0) {
            //        IsValid = false;
            //        showStickyToast(false, 'Customer Name Already Exists.', false);
            //        return false;
            //    }


            //}

            //if ($('#EmailAddress').val().trim() != "")
            //{
            //    var validEmail = isValidEmailAddress($('#EmailAddress').val().trim());
            //    if (validEmail == false)
            //    {
            //        AppendMessage("Please Enter valid Email.");
            //    }
            //}

            return true;
        }

        function ClearHeader() {

            $('#TenantID').val('0');
            $('#CustomerName').val('');
            $('#CustomerCode').val('');
            $('#EmailAddress').val('');
            $('#Mobile').val('');
        }
        function AppendMessage(Message) {
            $("#divErrors").append("<li style='margin-top: 0.3em;'>" + Message + "</li>");
        }

        function GetAddressFormData(classname) {
            var fieldDataOut = '{';
            var fieldData = '<root><data>';
            $(classname).each(function () {
                var param = $(this).attr('id');
                var val = $(this).val().trim();
                var paramtype = $(this).attr('type');
                fieldData += '<' + param + '>' + val + '</' + param + '>';
            });

            fieldData += '<CreatedBy>' + <%=cp.UserID.ToString()%> + '</CreatedBy>';
                    fieldData = fieldData + '</data></root>';

                    return fieldData;
        }



        function EditAddress(Id) {
            //debugger;
            $('#GEN_MST_Address_ID').val(Id);
            $("#divAddressValidationMessages").empty();
            var item = $.grep(CustomerList.Table1, function (a) { return a.GEN_MST_Address_ID == Id });
            $("#btnaddress").html('Update <i class="material-icons">update</i>');
            BuildFormtoEditAddress(item, '.AddressfieldToGet');
            $('#GEN_MST_Country_ID').val(item[0]['CountryMasterID']);
            LoadStates();
            $('#GEN_MST_State_ID').val(item[0]['StateMasterID']);
            LoadCities();
            $('#GEN_MST_City_ID').val(item[0]['CityMasterID']);
            LoadZipCodes();
            $('#GEN_MST_ZipCode_ID').val(item[0]['GEN_MST_ZipCode_ID']);

            $('#DeliveryPoint').val(item[0]['DeliveryPoint']);
            $('#latitude').val(item[0]['Latitude']);
            $('#longitude').val(item[0]['Longitude']);
            $("#AurthoBrief").modal({
                show: 'true'
            });
        }

        function BuildFormtoEditAddress(item, classname) {


            $(classname).each(function () {
                var fieldID = $(this).attr('id');
                var paramtype = $(this).attr('type');
                $('#' + fieldID).val(item[0][fieldID]);
            });
        }

        function DeleteAddress(id) {
           

            //document.getElementById("spanDeleteCustomer").innerHTML = " Do you want to Delete <b><font color='red'>" + 1 + " </b></font> Location(s)?<br> ";
            //$("#divDeleteCustomer").dialog("option", "title", "Confirmation");
            //$("#divDeleteCustomer").dialog("open");
            //$("#divDeleteCustomer").dialog({
            //    buttons: [
            //        {
            //            text: "OK",
            //            "click": function () {
            //                DeleteItem(id, "", "");
            //            }
            //        },
            //        {
            //            text: "Cancel",
            //            "click": function () {                           
            //                $('#divDeleteCustomer').dialog("close");
            //            }
            //        }
            //    ],
            //    focus: function (i, j) {
            //        $('#divDeleteCustomer').siblings('.ui-dialog-buttonpane').find('button.ui-widget').css('position', "initial");
            //    },
            //    create: function (i, j) {
            //        $('#divDeleteCustomer').siblings('.ui-dialog-buttonpane').find('button.ui-widget').css('position', "initial");
            //    },
            //    close: function (i, j) {
            //        $("#divDeleteCustomer").dialog("destroy");
            //    }
            //});

            DeleteItem(id, "", "");
        }

        

        function DeleteItem(id, procname, header) {
            if (confirm("Are you sure do you want to delete ?")) {
                var obj = {};
                obj.StrId = id;
                $.ajax({
                    url: "Customer.aspx/DeleteAddress",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    data: JSON.stringify(obj),
                    success: function (response) {
                        if (response.d == "Exist") {
                            showStickyToast(false, 'Could not delete as this Delivery site mapped to SO');
                            return;
                        }
                        if (response.d == "success") {
                            showStickyToast(true, 'Deleted Successfully');
                            GetCustomerList();
                        }
                    }
                });
            }
        }


        function CheckDuplicate(obj, field, value, PK) {

            var status = true;
            var item = obj, Count = 0;

            if ($('#' + PK).val() != 0) {
                item = $.grep(obj, function (data) {
                    return data[PK] != $('#' + PK).val();
                });
            }
            Count = $.grep(item, function (data) {

                return data[field] == value || data[field].toUpperCase() == value.toUpperCase() || data[field].toLowerCase() == value.toLowerCase();
            });

            if (Count.length != 0) {
                status = false;
            }
            return status;
        }


        function LoadAddressGrid(Obj) {

            $("#tblAddressList").empty();
            $("#tblAddressList").append("<thead><tr><td colspan='9' class='text-right' style='background-color: white'> <div class='text-right'></div></td></tr><tr><th style='text-align:left !important;font-size:12pt;'>S.No </th><th style='text-align:left !important;font-size:12pt;'>Address Type</th><th style='text-align:left !important;font-size:12pt;'>Address</th><th style='text-align:left !important;font-size:12pt;'>City</th><th style='text-align:left !important;font-size:12pt;' class='text-left'>State</th><th class='text-left' style='text-align:left !important;font-size:12pt;'>Country</th><th class='text-left' style='text-align:left !important;font-size:12pt;'>Zip Code</th><th style='text-align:left !important;font-size:12pt;'>Delivery Site</th><th class='text-left' style='text-align:left !important;font-size:12pt;'>Action</th></tr></thead><tbody>");

            if (Obj != null) {
                var dataList1 = Obj;
                if (dataList1.length > 0) {
                    $("#tblAddressList").css("display", "table");
                    //$("#tblAddressList").append("<thead><tr><th colspan='8' class='text-right' style='background-color: white'> <div class='text-right'><button type='button' id='btnCreateAddress'  class='btn btn-primary' data-toggle='modal' data-target='#AddAddressToCreate' onclick='ClearAddressData();'>Add <i class='fa fa-plus' aria-hidden='true'></i></button></div></th></tr><tr><th class='text-center'>S.No </th><th class='text-center'>Address Type</th><th class='text-center'>Address</th><th class='text-center'>City</th><th class='text-center'>State</th><th class='text-center'>Country</th><th class='text-center'>Zip</th><th class='text-center'>Action</th></tr></thead><tbody>");
                    for (var i = 0; i < dataList1.length; i++) {
                        $("#tblAddressList").append("<tr><td>" + (i + 1) + "</td><td class='text-left'>" + dataList1[i].AddressType + "</td><td class='text-left'>" + dataList1[i].AddressLine1 + "</td><td class='text-left'>" + dataList1[i].CityName + "</td><td class='text-left'>" + dataList1[i].StateName + "</td><td class='text-left'>" + dataList1[i].CountryName + "</td><td class='text-left'>" + dataList1[i].ZipCode + "</td><td class='text-left'>" + dataList1[i].DeliveryPoint + "</td><td class='text-center'> <a onclick='EditAddress(" + dataList1[i].GEN_MST_Address_ID + ")'><i class='material-icons ss'>mode_edit</i></a>&emsp; <a onclick='DeleteAddress(" + dataList1[i].GEN_MST_Address_ID + ");'><i class='material-icons ss'>delete</i></a></td></tr>");
                    }
                    $("#tblAddressList").append("</tbody>");
                }
                else {

                    $("#tblAddressList").append("</tbody>");
                }
            }
        }

        function ClearAddressData() {
            $('#GEN_MST_Address_ID').val('0');
            $("#btnaddress").html('Create <i class="material-icons">add</i>');
            $("#divAddressValidationMessages").empty();
            $('#AddressLine2').val('');
            $('#AddressLine1').val('');
            $('#GEN_MST_AddressType_ID').val('0');
            $('#GEN_MST_City_ID').val('0');
            $('#GEN_MST_State_ID').val('0');
            $('#GEN_MST_Country_ID').val('0');
            $('#GEN_MST_ZipCode_ID').val('0');
            $('#latitude').val('');
            $('#longitude').val('');
            $('#DeliveryPoint').val('');
        }

        function LoadAddressDropdowns(obj) {
            var AddressTypeList = obj.Table7;
            var CountryList = obj.Table6;

            $("#GEN_MST_AddressType_ID").empty();
            $("#GEN_MST_City_ID").empty();
            $("#GEN_MST_State_ID").empty();
            $("#GEN_MST_Country_ID").empty();
            $("#GEN_MST_ZipCode_ID").empty();

            $("#GEN_MST_AddressType_ID").append($("<option></option>").val(0).html("Select"));
            for (var x = 0; x < AddressTypeList.length; x++) {
                $("#GEN_MST_AddressType_ID").append($("<option></option>").val(AddressTypeList[x].GEN_MST_AddressType_ID).html(AddressTypeList[x].AddressType));
            }

            $("#GEN_MST_City_ID").append($("<option></option>").val(0).html("Select"));

            $("#GEN_MST_State_ID").append($("<option></option>").val(0).html("Select"));

            $("#GEN_MST_Country_ID").append($("<option></option>").val(0).html("Select"));
            for (var x = 0; x < CountryList.length; x++) {
                $("#GEN_MST_Country_ID").append($("<option></option>").val(CountryList[x].CountryMasterID).html(CountryList[x].CountryName));
            }

            $("#GEN_MST_ZipCode_ID").append($("<option></option>").val(0).html("Select"));
        }


        function LoadStates() {

            $("#GEN_MST_State_ID").empty();
            $("#GEN_MST_City_ID").empty();
            $("#GEN_MST_ZipCode_ID").empty();
            ClearLonglatt();

            if ($("#GEN_MST_Country_ID").val() == 0) {
                $("#GEN_MST_State_ID").append($("<option></option>").val(0).html("Please Select"));
                $("#GEN_MST_City_ID").append($("<option></option>").val(0).html("Please Select"));
                $("#GEN_MST_ZipCode_ID").append($("<option></option>").val(0).html("Please Select"));
            }

            else {
                var StateList = $.grep(CustomerList.Table4, function (a) { return a.CountryMasterID == $("#GEN_MST_Country_ID").val() });

                $("#GEN_MST_State_ID").append($("<option></option>").val(0).html("Please Select"));
                $("#GEN_MST_City_ID").append($("<option></option>").val(0).html("Please Select"));
                $("#GEN_MST_ZipCode_ID").append($("<option></option>").val(0).html("Please Select"));

                for (var x = 0; x < StateList.length; x++) {
                    $("#GEN_MST_State_ID").append($("<option></option>").val(StateList[x].StateMasterID).html(StateList[x].StateName));
                }
            }
        }


        function LoadCities() {


            $("#GEN_MST_City_ID").empty();
            $("#GEN_MST_ZipCode_ID").empty();
            ClearLonglatt();

            if ($("#GEN_MST_State_ID").val() == 0) {
                $("#GEN_MST_City_ID").append($("<option></option>").val(0).html("Please Select"));
                $("#GEN_MST_ZipCode_ID").append($("<option></option>").val(0).html("Please Select"));
            }

            else {


                var CityList = $.grep(CustomerList.Table3, function (a) { return a.StateMasterID == $("#GEN_MST_State_ID").val() });

                $("#GEN_MST_City_ID").append($("<option></option>").val(0).html("Please Select"));
                $("#GEN_MST_ZipCode_ID").append($("<option></option>").val(0).html("Please Select"));

                for (var x = 0; x < CityList.length; x++) {
                    $("#GEN_MST_City_ID").append($("<option></option>").val(CityList[x].CityMasterID).html(CityList[x].CityName));
                }
            }
        }

        function LoadZipCodes() {

            $("#GEN_MST_ZipCode_ID").empty();
            ClearLonglatt();

            if ($("#GEN_MST_City_ID").val() == 0) {
                $("#GEN_MST_ZipCode_ID").append($("<option></option>").val(0).html("Please Select"));
            }

            else {

                var CityList = $.grep(CustomerList.Table5, function (a) { return a.CityMasterID == $("#GEN_MST_City_ID").val() });

                $("#GEN_MST_ZipCode_ID").append($("<option></option>").val(0).html("Please Select"));

                for (var x = 0; x < CityList.length; x++) {
                    $("#GEN_MST_ZipCode_ID").append($("<option></option>").val(CityList[x].ZipCodeID).html(CityList[x].ZipCode));
                }
            }
        }

        function ClearLonglatt() {
            $("#latitude").val('');
            $("#longitude").val('');
            $("#DeliveryPoint").val('');
        }

       
    </script>

</asp:Content>