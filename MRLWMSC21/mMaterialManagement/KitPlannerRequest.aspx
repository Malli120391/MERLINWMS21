<%@ Page Title=" Kit Planner Request :." Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="KitPlannerRequest.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.KitPlannerRequest" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MMContent" runat="server">
    <script src="../mInbound/Scripts/InventraxAjax.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    
    <script type="text/javascript">


        var UomResult;
        $(document).ready(function () {

            LoadKitDetailsList();
            var val =<%= Headerid %>;
            if (val == "0") {
                $('#lnkAddSupplier').css('display', 'none');
                $('#divKitItems').val('');
            }
            else {
                $('#lnkAddSupplier').css('display', 'block');
            }
            var textfieldname = $('#<%=txtKitTenant.ClientID%>');
             DropdownFunction(textfieldname);
             $('#<%=txtKitTenant.ClientID%>').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
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
                    $("#<%=hifTenant.ClientID %>").val(i.item.val);

                },
                minLength: 0
            });





            var textfieldname = $('#<%=txtPartNo.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=txtPartNo.ClientID%>').autocomplete({
                source: function (request, response) {
                    $("#<%=txtHeight.ClientID %>").val("");
                    $("#<%=txtWidth.ClientID %>").val("");
                     $("#<%=txtLength.ClientID %>").val("");
                    $("#<%=txtWeight.ClientID %>").val("");
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetParentMCodeKitFor3PL") %>',
                        //data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + document.getElementById('hifTenant').value + "'}",
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + $('#<%=this.hifTenant.ClientID%>').val() + "','KitTypeID':'" + $('#<%=this.ddlKitType.ClientID%>').val() +"'}",//<=cp.TenantID%>
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1],
                                    Height: item.split(',')[2],
                                    Width: item.split(',')[3],
                                    Length: item.split(',')[4],
                                    Weight: item.split(',')[5],
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

                    $("#<%=txtHeight.ClientID %>").val("");
                    $("#<%=txtWidth.ClientID %>").val("");
                    $("#<%=txtLength.ClientID %>").val("");
                    $("#<%=txtWeight.ClientID %>").val("");

                    $("#<%=hifMMID.ClientID %>").val(i.item.val);

                    $("#<%=txtHeight.ClientID %>").val(i.item.Height);
                    $("#<%=txtWidth.ClientID %>").val(i.item.Width);
                    $("#<%=txtLength.ClientID %>").val(i.item.Length);
                    $("#<%=txtWeight.ClientID %>").val(i.item.Weight);

                },
                minLength: 0
            });



            var kitpartno = $('#<%=txtkitpartno.ClientID%>');
            DropdownFunction(kitpartno);
            debugger;
            $('#<%=txtkitpartno.ClientID%>').autocomplete({              
                source: function (request, response) {
                    debugger;
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetParentMCodeKitFor3PL1") %>',
                        //data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + document.getElementById('hifTenant').value + "'}",
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + $('#<%=this.hifTenant.ClientID%>').val() + "','kitheaderid': '" + val + "','SupplierID': '" + $("#<%=hifsupplier.ClientID %>").val() + "'}",//<=cp.TenantID%>
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
                    $("#<%=hikitpartnoID.ClientID %>").val(i.item.val);
                    $("#<%=hifPUoM_Qty.ClientID %>").val("0");
                    $("#<%=hidUoMQty.ClientID %>").val("0");
                    $("#<%=txtuom.ClientID %>").val("");
                  <%--  $("#<%=hifsupplier.ClientID %>").val("0");
                    $("#<%=txtSupplier.ClientID %>").val("");--%>
                },
                minLength: 0
            });



            $("#lnkAddSupplier").click(function () {
                debugger;
                myKitclear();
            });


            var supplier = $('#<%=txtSupplier.ClientID%>');
            DropdownFunction(supplier);
            debugger;
            $('#<%=txtSupplier.ClientID%>').autocomplete({
                source: function (request, response) {
                    if ($("#<%=hifMMID.ClientID %>").val() == "0" || $("#<%=hifMMID.ClientID %>").val() == "") {
                        $("#<%=hifsupplier.ClientID %>").val("0");
                        return false;
                    }
                    debugger;
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetsuppliersForMcode") %>',
                        //data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + document.getElementById('hifTenant').value + "'}",
                        data: "{ 'prefix': '" + request.term + "','MMid':'" + $("#<%=hifMMID.ClientID %>").val() + "'}",//<=cp.TenantID%>
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
                    $("#<%=hifsupplier.ClientID %>").val(i.item.val);
                   
                },
                minLength: 0
            });
          



           
           
            var uomdata = $('#<%=txtuom.ClientID%>');
            DropdownFunction(uomdata);
            $('#<%=txtuom.ClientID%>').autocomplete({
                source: function (request, response) {
                    if ($('#<%=txtkitpartno.ClientID%>').val() == "") {
                        showStickyToast(false, 'Select Kit part number');
                        return false;
                    }
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMWithQty") %>',
                        data: "{ 'MaterialID': '" + $('#<%=this.hikitpartnoID.ClientID%>').val() + "'}",
                                dataType: "json",
                                type: "POST",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "" || data.d == "/") {
                                        showStickyToast(false, 'No UoM\'s are configured to this Part Number');
                                        document.getElementById("atcPUoM").value = "";
                                        return;
                                    }
                                    else {
                                        response($.map(data.d, function (item) {
                                            return {
                                                label: item.split(',')[0],
                                                val: item.split(',')[1]
                                            }
                                        }))
                                    }
                                }
                            });
                        },
                        select: function (e, i) {
                            $("#<%=hifPUoM_Qty.ClientID %>").val(i.item.val);
                            $("#<%=hidUoMQty.ClientID %>").val(i.item.val);
                          
                        },
                        minLength: 0
                    });
       
        
        });


        function LoadKitDetailsList()
        {
            debugger;
          
            var val =<%= Headerid %>;
            if (val != "0") {
                var data = "{}";
                InventraxAjax.AjaxResultExecute("KitPlannerRequest.aspx/GetKitDetailsList", data, 'GetItemDetailsOnSuccess', 'GetItemDetailsOnError', null);
            }
            
           
        }

        var obj = null;
        function GetItemDetailsOnSuccess(data) {
            $('#divKitItems').empty();
            obj = JSON.parse(data.Result);
            
            var s = '<table class="table table-striped"><tr><th width150>S. No.</th><th width150>Child Part No.</th><th width150>Supplier</th><th number width150>Quantity</th><th width150>UoM</th><th>Action</th></tr>';
            for (var i = 0; i < obj.length; i++)
            {
                s += '<tr><td width150>' + (i + 1) + '</td><td width150>' + obj[i].MCode + '</td><td width150>' + obj[i].SupplierName + '</td><td number width150>' + obj[i].ChildKitQuantity + '</td><td width150>' + obj[i].ChildKUoM + '</td><td><span onclick="btnEdit_Click(' + obj[i].KitPlannerDetailID + ')"><i class="material-icons">edit</i></span>&nbsp;&nbsp;&nbsp;<span onclick="btnDelete_Click(' + obj[i].KitPlannerDetailID +')"><i class="material-icons">delete</i></span></td></tr>';
            }
            s += '</table>';
            $('#divKitItems').html(s);

        }

        function GetItemDetailsOnError() {

        }

        function btnEdit_Click(id)
        {
            debugger;
            $("#SupModal").modal({
                show: 'true'
            });
            var item = $.grep(obj, function (a) { return a.KitPlannerDetailID == id });
            console.log(item);
            $("#<%=hifPUoM_Qty.ClientID %>").val(item[0].MaterialMaster_UoMID);
            $("#<%=hidUoMQty.ClientID %>").val(item[0].MaterialMaster_UoMID);
            $("#<%=hikitpartnoID.ClientID %>").val(item[0].ChildMMID);
            $("#<%=txtkitpartno.ClientID %>").val(item[0].MCode);
            $("#txtQuantity").val(item[0].ChildKitQuantity);
            $("#<%=txtuom.ClientID %>").val(item[0].ChildKUoM);
            $("#<%=kitDetailsid.ClientID %>").val(item[0].KitPlannerDetailID);
         
            //alert($("#<%=kitDetailsid.ClientID %>").val());


            return false;
        }
        function btnDelete_Click(id) {
            if (confirm('Are you sure to delete ?')) {

                var data = "{ 'kpid':'" + id + "'}";
                InventraxAjax.AjaxResultExecute("KitPlannerRequest.aspx/DeleteChildItems", data, 'GetItemDeleteOnSuccess', 'GetDeleteOnError', null);
            }
            
        }

        function btnSubmitC_Click()
        {
            var kpid = $('.hdnkpid').val();
            var DetailsID = $('#hdnDetailID').val();

            var data = "{ 'kpid':'" + kpid + "','DetailID':'" + DetailsID + "','MatID':'" + $('#hdnMCodeC').val() + "','Qty':'" + $('#txtQtyC').val() + "' }";
            InventraxAjax.AjaxResultExecute("KitPlannerRequest.aspx/SetKitDetails", data, 'SetItemDetailsOnSuccess', 'SetItemDetailsOnError', null);  

        }
        function GetItemDeleteOnSuccess(data) {
            debugger;
            if (data.Result == -3) {
                showStickyToast(false, "Unable to delete, Kit already mapped to Inward Order", true);
                return false;
            }
            else if (data.Result == -4) {
                showStickyToast(false, "Unable to delete, Kit already mapped to Outward Order", true);
                return false;
            }
            if (data.Result == 1) {
                showStickyToast(true, "Successfully Deleted", false);
            }
            else {
                showStickyToast(false, "Error While Deleting", false);
            }
            LoadKitDetailsList();
        }
        function GetDeleteOnError() {
        }

        function SetItemDetailsOnSuccess(data)
        {
            $('#hdnDetailID').val(0);
            $('#hdnMCodeC').val(0);
            $('#txtMCodeC').val('');
            $('#txtQtyC').val(0);
            $('#btnSubmitC').text('Submit');
        }

        function SetItemDetailsOnError()
        {
        }
        function AddKits() {
            debugger;
            var kit = document.getElementById("txtkitpartno");
            if (kit == "") {
                showStickyToast(false, "Please select Child Part No.", false);
                return false;
            }
            else if ($("#<%=hikitpartnoID.ClientID %>").val() == "" || $("#<%=hikitpartnoID.ClientID %>").val() == "0") {
                showStickyToast(false, "Please select Child Part No.", false);
                return false;
            }
            var quantity = $('#txtQuantity').val();
           
           
            if (quantity == "0" || quantity== "") {
                showStickyToast(false, "Please Enter valid Qty.", false);
                return false;
            }
            if ($("#<%=hifPUoM_Qty.ClientID %>").val() == "0" || $("#<%=hidUoMQty.ClientID %>").val() == "0" || $("#<%=txtuom.ClientID %>").val()=="") {
                showStickyToast(false, "Please Enter UOM", false);
                return false;
            }
          
            $.ajax({
                url: '<%=ResolveUrl("~/mMaterialManagement/KitPlannerRequest.aspx/UpsertKitData") %>',
                data: "{'KitPartNo' : '" + $("#<%=hikitpartnoID.ClientID %>").val() + "','Quantity' : '" + quantity + "','PUOM' : '" + $("#<%=hifPUoM_Qty.ClientID %>").val() + "','Duom' : '" + $("#<%=hidUoMQty.ClientID %>").val() + "','detailsid' : '" + $("#<%=kitDetailsid.ClientID %>").val() + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            if (response.d == -3) {
                                showStickyToast(false, "Unable to edit/add, Kit already mapped to Inward Order", true);
                                return false;
                            }
                            else if (response.d == -4) {
                                showStickyToast(false, "Unable to edit/add, Kit already mapped to Outward Order", true);
                                return false;
                            }
                            else if (response.d < 0) {
                                showStickyToast(false, "Error while adding", true);
                                return false;

                            }
                            else {
                                showStickyToast(true, "Kit Details Saved Successfully", false);
                                LoadKitDetailsList();
                            }
                            
                            //LoadSupplierAttach();
                            myKitclear();
                            $("#SupModal").modal('hide');
                            //location.reload();
                            //document.getElementById("panel4").focus();
                        }
              });

          
               <%-- if (validateeditsup.length == 0) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertSupDetailsInfo") %>',
                         data: "{'ltMMT_SupID' : '" + ltMMT_SupID + "','MaterialMasterID' : '" + MaterialMasterID + "','vSUPID' : '" + vSUPID + "','TenantID' : '" + Tenant + "','vUnitCost' : '" + vUnitCost + "','vDeliveryTime' : '" + vDeliveryTime + "','CurrencyID' : '" + CurrencyID + "','txtSupplierPartNumber' : '" + txtSupplierPartNumber + "','vInitialOrderQty' : '" + vInitialOrderQty + "','CreatedBy' : '" + <%=this.cp.UserID%> + "'}",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (response) {
                             showStickyToast(true, "Supplier Details Saved Successfully", false);
                             GetItemMasterDetails(MaterialMasterID);
                             //LoadSupplierAttach();
                             mySupclear();
                             $("#SupModal").modal('hide');
                             //location.reload();
                             //document.getElementById("panel4").focus();
                         }
                     });

                 }--%>
                 

        }
        function myKitclear() {
            $("#<%=hifPUoM_Qty.ClientID %>").val("0");
            $("#<%=hidUoMQty.ClientID %>").val("0");
          $("#<%=hikitpartnoID.ClientID %>").val("0");
            $("#<%=txtkitpartno.ClientID %>").val("");
            $("#txtQuantity").val("");
            $("#<%=txtuom.ClientID %>").val("");
            $("#<%=kitDetailsid.ClientID %>").val("0");

        }
      
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }
        

    </script>


    <style>
        .flex input[type="text"], input[type="number"], textarea{
            width: 100% !important;
        }

        select {
             width: 100% !important;
        }

        select[disabled="disabled"]{
              width: 100% !important;
        }

        input[disabled="disabled"]{
              width: 90% !important;
        }

       
    </style>


   <div class="container">
            
      <div class=" " flex end>
                         <asp:LinkButton ID="LinkButton3"  runat="server" CssClass="btn btn-primary "  SkinID="lnkButEmpty"   PostBackUrl="~/mMaterialManagement/KitPlannerList.aspx" style="padding-left:0px !important; padding: 0.375rem 0.75rem;" ><i class="material-icons vl">arrow_back</i>Back to List</asp:LinkButton>
                                    </div>
   
    <input type="hidden" id="hdnkpid" runat="server" value="0" class="hdnkpid" />
    <div id="divKit" class="">
        <div class="row">
            <div class="col m3 s3">
                <div class="flex">
                    <asp:TextBox ID="txtKitCode" runat="server" required="" Enabled="false"></asp:TextBox>
                    <label>Kit Code</label>
                </div>
            </div>
            <div class="col m3 s3">
                <div class="flex">
                    <asp:TextBox ID="txtKitTenant" runat="server" required=""></asp:TextBox>
                    <span class="errorMsg"></span>
                    <label>Tenant</label>
                    <asp:HiddenField ID="hifTenant" runat="server" Value="0" />
                </div>
            </div>
            <div class="col m3 s3">
                <div class="flex">
                    <asp:DropDownList ID="ddlKitType" OnSelectedIndexChanged="ddlKitType_SelectedIndexChanged" AutoPostBack="true" runat="server" required=""></asp:DropDownList>
                    <span class="errorMsg"></span>
                    <label>Kit Type</label>

                </div>
            </div>
            <div class="col m3 s3">
                <div class="flex">
                    <asp:TextBox ID="txtPartNo" runat="server" required="" ></asp:TextBox>
                       
                    <span class="errorMsg"></span>
                    <label>Part No.</label>
                    <asp:HiddenField ID="hifMMID" runat="server" Value="0" />
                </div>
            </div>
        </div>
        <div class="row">

            <div class="col m3 s3">
                <div class="flex">
                    <asp:TextBox ID="txtSupplier" runat="server" required="" ></asp:TextBox>
                       
                  <%--  <span class="errorMsg"></span>--%>
                    <label>Supplier</label>
                    <asp:HiddenField ID="hifsupplier" runat="server" Value="0" />
                </div>
            </div>
            <div class="col m3 s3" style="display:none;">
                <div class="flex">
                   <%-- <asp:TextBox ID="txtSupplier" runat="server" required=""></asp:TextBox>
                    <span class="errorMsg"></span>
                    <label>Supplier</label>
                    <asp:HiddenField ID="hidSupplier" runat="server" Value="0" />--%>
                     <div class="flex">
                    <asp:TextBox ID="txtLength" onkeypress="return isNumber(event)" runat="server" required=""></asp:TextBox>
                    <label>Length</label>
                </div>
                </div>
            </div>
           

        </div>
        <div class="row" class="col m3 s3" style="display:none;">
             <div class="col m3 s3">
                
                <div class="flex">
                   <%-- <asp:TextBox ID="txtLength" runat="server" required=""></asp:TextBox>
                    <label>Length</label>--%>
                       <asp:TextBox ID="txtWidth" onkeypress="return isNumber(event)" runat="server" required=""></asp:TextBox>
                    <label>Width</label>
                    
                </div>
               <%-- <div class="flex__" style="justify-content: space-between">                    
                    <div>
                        <asp:Literal ID="ltUoM" runat="server"></asp:Literal>
                        <label id="phuom" runat="server">UoM:EA</label>
                    </div>
                    <div class="right">
                        <asp:Literal ID="ltQty" runat="server"></asp:Literal>
                        <label id="phqty" runat="server">Qty:1.00</label>
                    </div>
                </div>--%>
            </div>

            <div class="col m3 s3" >
               <div class="flex">
                    <asp:TextBox ID="txtHeight" onkeypress="return isNumber(event)" runat="server" required=""></asp:TextBox>
                    <label>Height</label>
                </div> 
            </div>
            <div class="col m3 s3">
                 <div class="flex">
                       <asp:TextBox ID="txtWeight" onkeypress="return isNumber(event)" runat="server" required=""></asp:TextBox>
                    <label>Weight</label>                 
                </div>
            </div>
            <div class="col m3 s3">

               
            </div>
        </div>

        <div class="row">

           <%-- <div class="col m3 s3">
                <div class="flex">
                    <asp:TextBox ID="txtWeight" runat="server" required=""></asp:TextBox>
                    <label>Weight</label>
                </div>
            </div>--%>
            <div class="col m1 offset-m11 s1 offset-s11">
                <br class="hide-sm" />
                <div class="flex right">
                    <asp:LinkButton ID="lnkCreatekit" runat="server" CssClass="btn btn-primary" OnClick="lnkCreatekit_Click">Create <i class='fa fa-floppy-o'></i></asp:LinkButton>

                </div>

            </div>

        </div>
    </div>

  
    <%--<div class="row" >
         <div id="divContainer" style="border:1px solid #E7E7E7; padding:10px;">
        <div id="divinner">
            <input type="hidden" id="hdnDetailID" class="hdnDetailID" value="0" />
            <div class="row">
                <div class="col m4 s4">
                    MCode:<br />
                    <input type="text" id="txtMCodeC" class="form-control" />
                    <input type="hidden" id="hdnMCodeC" value="0" />
                </div>                 
                 <div class="col m4 s4">
                    Quantity:<br />
                    <input type="text" id="txtQtyC" class="form-control" />
                </div>
                 <div class="col m4 s4">
                   <br />
                   <button id="btnSubmitC" onclick="btnSubmitC_Click();" class="btn btn-primary">Submit</button>
                </div>
            </div>
            <div class="row">
                
            </div>
        </div>
    </div>
        

           

    </div>--%>
      

    <div class="panel-body">
                                    <div class="pull-right">
                                        <button type="button" id="lnkAddSupplier"  class="btn btn-primary " data-toggle="modal" data-target="#SupModal">Add Child Items <i class="fa fa-plus" aria-hidden="true"></i></button>
                                    </div>
                                    <!-- Modal -->
                                    <div id="SupModal" class="modal fade">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color:#fff !important;">
                                                    <h4 class="modal-title" style="display: inline !important;">Add Child Items</h4>
                                                    <button type="button"  data-dismiss="modal" class="pull-right modalclose" onclick="myKitclear();" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                <div class="modal-body" id="mySupForm">
                                                    <div class="row">
                                                        <div class="col m4 s4">
                                                            <div class="flex">
                                                                <div>
                                                                     <asp:TextBox ID="txtkitpartno" runat="server" required=""></asp:TextBox>
                                                                    <label>Child Part No.</label>
                                                                    <span class="errorMsg">*</span>
                                                                    <asp:HiddenField ID="hikitpartnoID" runat="server" Value="0" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col m4 s4">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" id="txtQuantity" onkeypress="return isNumber(event)" required="" onKeyPress="return checkNum(event)">
                                                                    <label>Quantity</label>
                                                                    <span class="errorMsg">*</span> 
                                                                </div>
                                                            </div>
                                                        </div>
                                                         <div class="col m4 s4">
                                                            <div class="flex">
                                                                <div>
                                                                     <asp:TextBox runat="server"  ID="txtuom"  required=""></asp:TextBox>
                                                                    <label>UOM</label>
                                                                    <span class="errorMsg">*</span>
                                                                    <asp:HiddenField ID="hifPUoM_Qty"   runat="server" Value="0" />
                                                                      <asp:HiddenField ID="hidUoMQty" runat="server" Value="0" />
                                                                    <asp:HiddenField ID="kitDetailsid" runat="server" Value="0" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    
                                                </div>
                                                <div class="modal-footer">
                                                    <input type="hidden" id="MMT_SUPPLIER_ID" />
                                                   
                                                    <button type="button" class="btn btn-primary"  onclick="myKitclear();">Clear</button>
                                                    <button type="button" class="btn btn-primary"  data-dismiss="modal">Close</button>
                                                    <button type="button" class="btn btn-primary" onclick="AddKits();">Save</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Modal -->
                                    <div class="row" style="margin-top: 8% !important; margin: auto !important;" id="SupDetailsTable">
                                        <%--Supplier Details Table Append--%>
                                    </div>
                                </div>
      <div id="divKitItems">

        </div>   

   </div>



</asp:Content>
