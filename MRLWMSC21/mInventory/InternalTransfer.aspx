<%@ Page Title="Internal Transfer:." Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="InternalTransfer.aspx.cs" Inherits="MRLWMSC21.mInventory.InternalTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">
    <asp:ScriptManager runat="server" ID="ssss" SupportsPartialRendering="true" EnablePartialRendering="true"></asp:ScriptManager>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <script>
        $(document).ready(function () {
           
            try
            {
                //$("#<%= this.txtTenant.ClientID %>").focusout(function () {

                   // if ($("#<%= this.txtTenant.ClientID %>").val()=='')
                     //   $("#<%= this.txtTenant.ClientID %>").val('Tenant');
                //});

                var TextFieldName = $("#<%= this.txtTenant.ClientID %>");
                DropdownFunction(TextFieldName);
                $("#<%= this.txtTenant.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            <%--url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>--%>
                            url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                            data: "{ 'prefix': '" + request.term + "','WHID':'" + $("#<%=hifWarehouseId.ClientID %>").val() +"'}",
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
                        $("#<%=hifTenant.ClientID %>").val(i.item.val);
                        $('#<%=atcMaterialMaster.ClientID%>').val("");
                        $("#<%=hifMaterialMaster.ClientID%>").val("");
                      <%--  $("#<%=txtWH.ClientID %>").val("");--%>
                      <%--  $("#<%=hifWarehouseId.ClientID %>").val("");--%>

                    },
                    minLength: 0
                });


                var textfieldname = $('#<%=this.atcMaterialMaster.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=this.atcMaterialMaster.ClientID%>').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantActiveStockMCode") %>',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" + $("#<%=hifTenant.ClientID %>").val() + "'}",//<=cp.TenantID%>
                            dataType: "json",
                            type: "POST",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {

                                response($.map(data.d, function (item) {
                                    return {

                                        label: item.split('~')[0].split('`')[0],
                                        description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                        val: item.split('~')[1]
                                    }
                                }))

                            }

                        });
                    },
                    select: function (e, i) {

                        $("#<%=this.hifMaterialMaster.ClientID%>").val(i.item.val);
                          $("#MainContent_InvContent_gvInternalTransfer > tbody tr").remove();
                        <%--  $("#<%=txtWH.ClientID %>").val("");
                        $("#<%=hifWarehouseId.ClientID %>").val("");--%>

                    },
                    minLength: 0
                }).data("autocomplete")._renderItem = function (ul, item) {
                    // Inside of _renderItem you can use any property that exists on each item that we built
                    // with $.map above */
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + item.label + "" + item.description + "</a>")
                        .appendTo(ul)
                };

            } catch (ex) {

            }

//load warehouses under user

            var TextFieldName = $("#<%= this.txtWH.ClientID %>");
                DropdownFunction(TextFieldName);
                $("#<%= this.txtWH.ClientID %>").autocomplete({
                    source: function (request, response) {
                        if ($("#MainContent_InvContent_hifWarehouseId").val() == "" || $("#MainContent_InvContent_hifWarehouseId").val() == null || $("#MainContent_InvContent_hifWarehouseId").val() == undefined) {
                            $("#<%=hifTenant.ClientID %>").val("");
                            $("#<%= this.txtTenant.ClientID %>").val("");
                            $('#<%=atcMaterialMaster.ClientID%>').val("");
                            $("#<%=hifMaterialMaster.ClientID%>").val("");
                        }
                        debugger;
                        $.ajax({
                          <%--  url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouseData") %>',
                            data: "{ 'prefix': '" + request.term + "', 'TenantID':'"+ $("#<%=hifTenant.ClientID %>").val()+"'}",//<=cp.TenantID%>--%>
                            url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
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
                        debugger;

                        $("#<%=hifWarehouseId.ClientID %>").val(i.item.val);
                        $("#<%=hifTenant.ClientID %>").val("");
                        $("#<%= this.txtTenant.ClientID %>").val("");
                         $('#<%=atcMaterialMaster.ClientID%>').val("");
                        $("#<%=hifMaterialMaster.ClientID%>").val("");
                         //  $("#MainContent_InvContent_gvInternalTransfer > tbody tr").remove();

                    },
                    minLength: 0
                });



            var textfieldname = $('#<%=this.txtLocation.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=this.txtLocation.ClientID%>').autocomplete({

                source: function (request, response) {

                    debugger;
                    <%--document.getElementById("<%=this.txtLocation.ClientID%>").value = "";

                    var Table = document.getElementById('<%=this.gvInternalTransfer.ClientID%>');

                    var tableRows = Table.getElementsByTagName('tr');

                    var Hiddenvalues;
                    var IsDamaged = 'No';
                    var HasDiscrepancy = 'No';
                    var hifIsQuarantine = 0;
                    for (index = 1; index < tableRows.length - 1; index++) {
                        Hiddenvalues = tableRows[index].getElementsByTagName('input');
                        if (Hiddenvalues[2].checked && (Hiddenvalues[0].value == 'Yes' || Hiddenvalues[1].value == 'Yes')) {
                            hifIsQuarantine = 1;
                        }
                    }--%>


                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetAllLocationsUnderWarehouse") %>',
                        //data: "{'Prefix': '" + request.term + "','ProductCategory':'" + document.getElementById('<%=this.hifMaterialCategory.ClientID%>').value + "','InboundID':'0'}",
                        data: "{'Prefix': '" + request.term + "','MMID':'" + document.getElementById('<%=this.hifMaterialMaster.ClientID%>').value + "', 'WHID':'"+ document.getElementById('<%=this.hifWarehouseId.ClientID%>').value+"'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
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
                    $("#<%=this.hiflocationID.ClientID%>").val(i.item.val);
                   
                },
                minLength: 0
            });

            debugger;
            var textfieldname = $('#<%=this.txtCarton.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=this.txtCarton.ClientID%>').autocomplete({

                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadContainer_BinToBin") %>',
                        data: "{'prefix': '" + request.term + "','Location':'" + document.getElementById('<%=this.txtLocation.ClientID%>').value + "','MaterialMasterID':" + document.getElementById('<%=this.hifMaterialMaster.ClientID%>').value+", 'WarehouseId':"+$("#<%=hifWarehouseId.ClientID %>").val()+"}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }

                            }))

                            if (data.d.length == "0") {
                                showStickyToast(false , "No Pallet available")
                            }
                        }

                    });
                },
                minLength: 0,
                select: function (e, i) {
                    <%--$.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/CheckContainerMapping") %>',
                        data: "{'Prefix': '" + i.item.val + "','Carton':'" + i.item.value+ "'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            $('.FormLabels.last').css("display", "" + response.d + "");
                        }
                    })--%>
                }
            });
      
        })
        function ClearText(TextBox) {
            if (TextBox.value == "Part Number...") {
                TextBox.value = "";
                TextBox.style.color = "#000000";
            }
        }
        function focuslost(TextBox) {
            if (TextBox.value == "") {
                TextBox.value = "Part Number...";
                TextBox.style.color = "#A4A4A4";
            }
        }

        function ClearTextTenant(TextBox) {
            if (TextBox.value == "Tenant...")
                TextBox.value = "";

            TextBox.style.color = "#000000";
        }

        function focuslostTenant(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Tenant...";

            TextBox.style.color = "#A4A4A4";
        }


        function CheckReceiveQty(textBox) {
           
            //var TransferQty = parseInt(parseFloat(textBox.value) * 1000);



            //if (document.getElementById('<=this.hifConversionType.ClientID%>').value == '0') {
                
            //    if (TransferQty % 100 != 0) {
            //        showStickyToast(true, "'Quantity' should be multiple of BUoMQty.");
            //        textBox.value = "";
            //        return;
            //    }
            //}
            //else {




            //var ConversionValueInMUoM = parseFloat(document.getElementById('<=this.hifConvesionValueINMuom.ClientID%>').value);
            
            //    var quntityINMUoM = (TransferQty * ConversionValueInMUoM);
                
            //    var ModuloValue = quntityINMUoM % 1000;
            //    if (ModuloValue != 0 && (Math.abs(1000 - ModuloValue)) > 50) {
            //        showStickyToast(true, "'Quantity Received' should be multiple of MUoM Qty. <br/> Suggested Quantity is " + parseInt(Math.ceil(quntityINMUoM / 1000) / ConversionValueInMUoM * 1000) / 1000);
            //        textBox.value = "";
            //        return;
            //    }





            //}
            CheckDecimal(textBox);
        }



    </script>

    <script>
        function RadioCheck(rb) {
            
            document.getElementById("<%=this.txtLocation.ClientID%>").value = "";

            var gv = document.getElementById("<%=gvInternalTransfer.ClientID%>");

            var rbs = gv.getElementsByTagName("input");

           // var row = rb.parentNode.parentNode.parentNode;
            //alert(row);
          //  var coumn = row.getElementsByTagName('td');
            //alert(coumn.length);
          //  var text = coumn[0].innerHTML.toString();
            //alert('llll'+text);


         for (var i = 0; i < rbs.length; i++) {
             // alert(rbs[i]);
             if (rbs[i].type == "radio") {

                 if (rbs[i].checked && rbs[i] != rb) {

                     rbs[i].checked = false;

                     break;

                 }


             }

         }
           
     }
    </script>
    <style>
  
</style>

    <script type="text/javascript">
        function CheckOtherIsCheckedByGVID(spanChk) {
            debugger

            var IsChecked = spanChk.checked;

            if (IsChecked) {

               // spanChk.parentElement.parentElement.style.backgroundColor = '#228b22';

               // spanChk.parentElement.parentElement.style.color = 'white';

            }

            var CurrentRdbID = spanChk.id;

            var Chk = spanChk;

            Parent = document.getElementById("<%=gvInternalTransfer.ClientID%>");

                var items = Parent.getElementsByTagName('input');

                for (i = 0; i < items.length; i++) {

                    if (items[i].id != CurrentRdbID && items[i].type == "radio") {

                        if (items[i].checked) {

                            items[i].checked = false;

                           // items[i].parentElement.parentElement.style.backgroundColor = 'white'

                           // items[i].parentElement.parentElement.style.color = 'black';
                        }

                    }

                }

        }
</script>
    <div class="module_yellow">
            <div class="ModuleHeader" height="35px">
                <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Orders</span> <i class="material-icons">arrow_right</i><span>House Keeping</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Inter Store Transfer </span></div>
                
            </div>

        </div>
    <div class="container">
        <!-- Globalization Tag is added for multilingual  -->
    <table cellpadding="4" cellspacing="2">
       <tr>
           <td>
                <span style="color:red;font-size:12pt; display:none;"> <%= GetGlobalResourceObject("Resource", "TransferQuantityallowsprecisiononly")%></span>
           </td>
       </tr>
        <tr>
            <td valign="bottom">
                <table border="0" cellspacing="5" cellpadding="0" width="100%" runat="server" id="tbMaterialDetails" style="padding-left: 926px;" visible="false">
                    <tr>
                          <td><b> <%= GetGlobalResourceObject("Resource", "Tenant")%> </b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                       
                        :&nbsp;&nbsp;<asp:Label CssClass="AutogeneratedNo" id="ltTenant" runat="server"></asp:Label></td>
                    </tr>

                    <tr>
                        <td><b><%= GetGlobalResourceObject("Resource", "PartNo")%></b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                       
                        :&nbsp;&nbsp;<asp:Label CssClass="AutogeneratedNo" id="ltMcode" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                         <td><b><%= GetGlobalResourceObject("Resource", "BUoMQty")%></b>
                        
                        :&nbsp;&nbsp;<asp:Label CssClass="AutogeneratedNo" id="ltBMUoM" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><b><%= GetGlobalResourceObject("Resource", "MUoMQty")%> </b>
                       
                        :&nbsp;&nbsp;<asp:Label CssClass="AutogeneratedNo" id="ltMMUoM" runat="server"></asp:Label></td>
                    </tr>
                </table>
               <asp:HiddenField ID="hifBUoMQty" runat="server" />
                <asp:HiddenField ID="hifConvesionValueINMuom" runat="server" />
                <asp:HiddenField ID="hifConversionType" Value="0" runat="server" />
            </td>
        </tr>
        
      
    </table>
        <div class="row">
                <asp:Panel ID="pnlTransfer" runat="server" DefaultButton="lnkGet">
                    <div class="row">
                          <div class="col m3  offset-m2" >
                            <div class="flex">
                                <asp:TextBox ID="txtWH" SkinID="txt_Hidden_Req" runat="server" required="" />
                                <label><%= GetGlobalResourceObject("Resource", "Warehouse")%></label>
                                 <span class="errormsg"></span>
                                <asp:HiddenField ID="hifWarehouseId" runat="server"  />
                            </div>
                        </div>
                        <div class="col m3" >
                            <div class="flex">
                                <asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Hidden_Req" required=""></asp:TextBox>
                                <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                                <span class="errormsg"></span>
                                <asp:HiddenField runat="server" ID="hifTenant" Value="0" />
                            </div>
                        </div>

                        <div class="col m3">
                            <div class="flex">
                                <asp:TextBox ID="atcMaterialMaster" SkinID="txt_Hidden_Req" runat="server" required="" />
                                <label><%= GetGlobalResourceObject("Resource", "PartNumber")%></label>
                                 <span class="errormsg"></span>
                                <asp:HiddenField ID="hifMaterialMaster" runat="server" />
                            </div>
                        </div>
                      
                        <div class="col m1">
                            <div class="flex">
                                <asp:LinkButton ID="lnkGet" runat="server" OnClientClick="showAsynchronus();" OnClick="lnkGet_Click" CssClass="btn btn-primary">
                            <%= GetGlobalResourceObject("Resource", "Get")%> <%=MRLWMSC21Common.CommonLogic.btnfaFilter %>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </asp:Panel></div>

        <div class="row">
                <asp:GridView ID="gvInternalTransfer" runat="server" SkinID="gvLightBlueNew" OnPageIndexChanging="gvInternalTransfer_PageIndexChanging" >
                    <Columns>
                         <asp:TemplateField HeaderText="<%$Resources:Resource,WHCode%>">
                            <ItemTemplate >
                                
                                <asp:Literal ID="ltWHCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "WHCode").ToString() %>' />
                                <asp:Literal ID="ltWHId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "WarehouseID").ToString() %>' />
                            </ItemTemplate>

                        </asp:TemplateField>
                       <%-- <asp:TemplateField HeaderText="Location">--%>
                         <asp:TemplateField HeaderText="<%$Resources:Resource,Location%>">
                            <ItemTemplate >
                                
                                <asp:Literal ID="ltLocation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Location").ToString() %>' />
                                <asp:Literal ID="ltGoodsMovementDetailsIDs" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GoodsMovementDetailsIDs").ToString() %>' />
                            </ItemTemplate>

                        </asp:TemplateField>
                       <%--  <asp:TemplateField HeaderText="Container">--%>
                          <asp:TemplateField HeaderText="<%$Resources:Resource,Pallet%>">
                            <ItemTemplate>

                                <asp:Literal ID="ltCartonCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PalletCode").ToString() %>' />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <%--<asp:TemplateField HeaderText="KitID" Visible="false">--%>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,KitID%>" Visible="false">
                            <ItemTemplate>
                                <asp:Literal ID="ltKitPlannerID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "KitPlannerID").ToString() %>' />
                                <asp:HiddenField ID="hifIsDamaged" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "IsDamaged").ToString() %>' />
                                <asp:HiddenField ID="hifHasDiscrepancy" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "HasDiscrepancy").ToString() %>' />
                            </ItemTemplate>

                        </asp:TemplateField>
                       <%-- <asp:TemplateField HeaderText="Is Damg." Visible="false">--%>
                         <asp:TemplateField HeaderText="<%$Resources:Resource,IsDamg%>"  Visible="false">
                            <ItemTemplate>
                                <%--<asp:Image ID="imgIsDamg"  ImageAlign="Middle" runat="server" ImageUrl='<%#Getimage( DataBinder.Eval(Container.DataItem, "IsDamaged").ToString() )%>' />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Has Disc." Visible="false">
                            <ItemTemplate>
                                <%--<asp:Image ID="imgDiscrepancy"  ImageAlign="Middle" runat="server" ImageUrl='<%#Getimage( DataBinder.Eval(Container.DataItem, "HasDiscrepancy").ToString() )%>' />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Is Non Conf." Visible="false">
                            <ItemTemplate>
                                <%--<asp:Image ID="imgIsNonConformity"  ImageAlign="Middle" runat="server" ImageUrl='<%#Getimage( DataBinder.Eval(Container.DataItem, "IsNonConfirmity").ToString() )%>' />--%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="As Is" Visible="false">
                            <ItemTemplate>
                                <%--<asp:Image ID="imgAsIs"  ImageAlign="Middle" runat="server" ImageUrl='<%#Getimage( DataBinder.Eval(Container.DataItem, "AsIs").ToString() )%>' />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Quantity">--%>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,Quantity%>">
                            <ItemTemplate>
                                <asp:Literal ID="ltQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Quantity").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                         <%--<asp:TemplateField HeaderText="Storage Location">--%>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,StorageLocation%>" >
                            <ItemTemplate>
                                <asp:Literal ID="ltStorageLocationID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StorageLocationID").ToString() %>' />
                                <asp:Literal ID="ltCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Code").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Batch No.">--%>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,BatchNo%>" >
                            <ItemTemplate>
                                
                                <asp:Literal ID="ltBatchNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BatchNo").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,MfgDate%>">
                            <ItemTemplate>
                               
                                <asp:Literal ID="ltmfgDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MfgDate","{0:dd-MMM-yyyy}").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Exp. Date">--%>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,ExpDate%>" >
                            <ItemTemplate>
                                
                                <asp:Literal ID="ltExpDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExpDate","{0:dd-MMM-yyyy}").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Serial No.">--%>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,SerialNo%>" >
                            <ItemTemplate>
                               
                                <asp:Literal ID="ltSerialNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SerialNo").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:TemplateField HeaderText="Project Ref. No.">--%>
                         <asp:TemplateField HeaderText="<%$Resources:Resource,ProjectRefNo%>">
                            <ItemTemplate>
                                <asp:Literal ID="ltProjectRefNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProjectRefNo").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="<%$Resources:Resource,MRP%>">
                            <ItemTemplate>
                                <asp:Literal ID="ltMRP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MRP").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <%--<asp:TemplateField HeaderText="Select One">--%>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,SelectOne%>" >
                            <ItemTemplate>
                                <asp:RadioButton ID="rdselect"  runat="server" onclick="javascript:CheckOtherIsCheckedByGVID(this);" />
                                <%--<input name="MyRadioButton" type="radio" value='<%# Eval("GoodsMovementDetailsIDs") %>' />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView></div>

        <div class="row">
                <asp:Panel runat="server" ID="pnlSearch">
                    <div  class=" ">
                        <div class="row">
                          
                            <div class=" col m3 offset-m2">
                                <div class="flex">
                                    <asp:TextBox ID="txtQuantity" runat="server" onKeyPress="return checkDec(this,event)" onblur="CheckReceiveQty(this)"  required="" />
                                 
                                    <label> <%= GetGlobalResourceObject("Resource", "Quantity")%></label>
                                    <span class="errormsg"></span>
                                    <asp:HiddenField ID="hifMaterialCategory" runat="server" />
                                </div>
                            </div>
                            <div class=" col m3">
                                 <div class="flex">
                                    <asp:TextBox ID="txtLocation"  onKeypress="return checkSpecialChar(event)" runat="server" SkinID="txt_Auto"  required="" />
                                 
                                       <label> <%= GetGlobalResourceObject("Resource", "Location")%> </label>
                                     <span class="errormsg"></span>
                                    <asp:HiddenField runat="server" ID="hiflocationID" Value="0" />
                                 </div>
                            </div>
                            <div class=" col m3">
                                <div class="flex">
                                    <asp:TextBox ID="txtCarton" SkinID="txt_Auto" runat="server"  required="" />
                                  
                                    <label> <%= GetGlobalResourceObject("Resource", "Pallet")%> </label>
                                    <span class="errormsg"></span>
                                </div>
                            </div>
                            
                              <div class=" col m3" style="display:none;">
                                  <div class="flex">
                                    <asp:DropDownList ID="ddlStorageLocationID" runat="server" required=""></asp:DropDownList>
                                  
                                       <label><%= GetGlobalResourceObject("Resource", "StorageLocation")%> </label>
                                  </div>
                            </div>

                             
                            <div>
                                <asp:LinkButton ID="lnkGenerateNewContainer" Visible="false"  OnClick="lnkGenerateNewContainer_Click"  runat="server"  CssClass="ui-btn ui-button-small" ToolTip="New Container">New <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
                            </div>

                            <div class="FormLabels col m1">
                                <div class="gap10"></div>
                                  <asp:LinkButton ID="lnkTransfer" runat="server"  OnClientClick="showAsynchronus();" OnClick="lnkTransfer_Click" CssClass="btn btn-primary">
                                <%= GetGlobalResourceObject("Resource", "Transfer")%>  <span class="space fa fa-exchange"></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </asp:Panel></div>

    </div>
</asp:Content>
