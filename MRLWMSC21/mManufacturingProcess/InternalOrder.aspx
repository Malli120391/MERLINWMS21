<%@ Page Title="NC Order:." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="InternalOrder.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.InternalOrder" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="ss" SupportsPartialRendering="true" ></asp:ScriptManager>
   
    <style>
        img
        {  
            border-style: none;
        }
    </style>

    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>

    <script>
         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
         function EndRequestHandler(sender, args) {
             if (args.get_error() == undefined) {
                 fnMCodeAC();
             }
         } 


         function fnMCodeAC() {
             $(document).ready(function () {

                 $(".DateBoxCSS_small").datepicker({ dateFormat: "dd/mm/yy" });

                 MSPConfifure();

                 var textfieldname = $('#<%=atcWorkCenter.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcWorkCenter.ClientID%>').autocomplete({
                     source: function (request, response) {
                         
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWorkCenterForProductionOrder") %>',
                             data: "{ 'prefix': '" + request.term + "','ProductionOrderHeaderID':'"+document.getElementById('<%=this.hifProductionOrderHeader.ClientID%>').value+"'}",
                             dataType: "json",
                             type: "POST",
                             async: true,
                             contentType: "application/json; charset=utf-8",
                             success: function (data) {
                                 if (data.d == "") {
                                     alert('No work station for Job Order');
                                 }
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

                          $("#<%=this.hifWorkCenter.ClientID%>").val(i.item.val);

                     },
                     minLength: 0
                  });


                 var textfieldname = $('#<%=atckitCode.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atckitCode.ClientID%>').autocomplete({
                     source: function (request, response) {
                        
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetKitCodeList") %>',
                             data: "{ 'Prefix': '" + request.term + "'}",
                             dataType: "json",
                             type: "POST",
                             async: true,
                             contentType: "application/json; charset=utf-8",
                             success: function (data) {

                                 response($.map(data.d, function (item) {
                                     return {
                                         label: item.split(',')[0],
                                         val: item.split(',')[0]
                                     }
                                 }))

                             }

                         });
                     },
                     select: function (e, i) {

                         $("#<%=this.hifkitCode.ClientID%>").val(i.item.val);

                     },
                     minLength: 0
                 });

                 var textfieldname = $('#<%=atcProductionOrderHeader.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcProductionOrderHeader.ClientID%>').autocomplete({
                     source: function (request, response) {

                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPRORefNo") %>',
                             data: "{ 'KitCode': '" + document.getElementById('<%=this.hifkitCode.ClientID%>').value + "'}",
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

                         $("#<%=this.hifProductionOrderHeader.ClientID%>").val(i.item.val);

                    },
                    minLength: 0
                 });

                 var textfieldname = $('#<%=atcRoutingoperation.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcRoutingoperation.ClientID%>').autocomplete({
                     source: function (request, response) {
                         
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRoutingDetetailsForProduction") %>',
                             data: "{ 'prefix': '" + request.term + "','ProductionOrderHeaderID':'"+document.getElementById('<%=this.hifProductionOrderHeader.ClientID%>').value+"'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == "") {
                                    alert('No Routing Operations for Workstation');
                                }
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

                         $("#<%=this.hifRouotingOperation.ClientID%>").val(i.item.val);

                    },
                    minLength: 0
                 });

                 var textfieldname = $('#<%=atcRequestedBy.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcRequestedBy.ClientID%>').autocomplete({
                     source: function (request, response) {

                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersBasedRole") %>',
                             data: "{ 'Prefix': '" + request.term + "','TenantID':'" + <%=cp.TenantID%>+ "'}",
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

                         $("#<%=this.hifRequestedBy.ClientID%>").val(i.item.val);

                     },
                     minLength: 0
                 });


                 var textfieldname = $('#<%=atcActivity.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcActivity.ClientID%>').autocomplete({
                     source: function (request, response) {

                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRoutingDetetailsActivitiesForRoutingDetails") %>',
                             data: "{ 'prefix': '" + request.term + "','RoutingDetailsID':'" + document.getElementById('<%=this.hifRouotingOperation.ClientID%>').value + "'}",
                             dataType: "json",
                             type: "POST",
                             async: true,
                             contentType: "application/json; charset=utf-8",
                             success: function (data) {
                                 if (data.d=="") {
                                     alert('No Activities for Routing Operation');
                                 }
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

                        $("#<%=this.hifActivity.ClientID%>").val(i.item.val);

                     },
                     minLength: 0
                });


                 var textfieldname = $('#<%=atcReasonForInternalOrderRequest.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcReasonForInternalOrderRequest.ClientID%>').autocomplete({
                     source: function (request, response) {

                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadReasonForInternalOrderRequest") %>',
                             data: "{ 'prefix': '" + request.term + "','TenantID':'" +<%=cp.TenantID%> +"'}",
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

                         $("#<%=this.hifReasonForInternalOrderRequest.ClientID%>").val(i.item.val);

                     },
                     minLength: 0
                 });

                 try

                 {

                     var textfieldname = $('#atcMaterialMaster');
                     DropdownFunction(textfieldname);
                     $('#atcMaterialMaster').autocomplete({
                         source: function (request, response) {

                             $.ajax({
                                 url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetMCodeForInternalOrderWithOEM") %>',
                                 data: "{ 'prefix': '" + request.term + "','ProductionOrderHeaderID':'" +document.getElementById('<%=this.hifProductionOrderHeader.ClientID%>').value +"'}",
                                 dataType: "json",
                                 type: "POST",
                                 async: true,
                                 contentType: "application/json; charset=utf-8",
                                 success: function (data) {
                                     if (data.d == "") {
                                         alert('No Materials for the Job Order');
                                     }
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

                             $("#hifMaterialMaster").val(i.item.val);
                             try
                             {
                                 MSPConfifure();
                             } catch (err) { }
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

                 } catch (ex) {}




                 var textfieldname = $('#atcMaterialMaster_IOUoMID');
                 DropdownFunction(textfieldname);
                 $('#atcMaterialMaster_IOUoMID').autocomplete({
                     source: function (request, response) {

                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMWithQty") %>',
                             data: "{ 'MaterialID': '" + document.getElementById('hifMaterialMaster').value + "'}",
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

                         $("#hifMaterialMaster_IOUoMID").val(i.item.val);

                     },
                     minLength: 0
                 });

             });
         }
         fnMCodeAC();

         function CheckIsDelted(checkBox) {
             if (checkBox.checked) {
                 alert('Are you sure want to delete Internal Order');
             }
         }
    </script>
        
    <script>


        function ClearText(TextBox) {
            if (TextBox.value == "Search Part Number...")
                TextBox.value = "";
        }
        function focuslost(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search Part Number...";
        }

        function CheckInvNum(textbox) {
            // alert('ssssss');
            $.ajax({
                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadInvoiceNumbers") %>',
                data: "{ 'prefix': '" + '' + "','POHeaderID': '" +<%=ViewState["HeaderID"].ToString()%> + "'}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    //alert(data.d);
                    var numbers = data.d.toString();
                    // alert(numbers);
                    // alert(numbers.split(','))
                    var invNumber = numbers.split(',');
                    //alert(textbox.value);
                    //textbox.value = "";
                    // alert(invNumber.length);
                    var check = false;
                    if (textbox.value != "") {

                        for (n = 0; n < invNumber.length; n = n + 2) {
                            //alert(invNumber[n]);
                            if (invNumber[n] == textbox.value) {
                                check = true;
                                break;
                            }
                        }
                    }
                    else
                        check = true;
                    //alert(check);
                    if (check == false) {
                        textbox.value = "";
                        textbox.focus();
                    }
                }

            });


        }



        function MSPConfifure() {

            var mmid = 0;
            try {
                mmid = document.getElementById("hifMaterialMaster").value;
                if (mmid == '' || mmid == null) {
                    mmid = 0;
                }
            }
            catch (err) {
            }
            //alert(mmid);
            $.ajax({
                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/MaterialConfigurationService") %>',
                data: "{ 'MaterialId': '" + mmid + "','TenantID':'" +<%=cp.TenantID%> +"'}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    //alert(data.d);
                    configure(name, data.d);
                    // response(data.d);
                }
            });
            
        }
        function configure(textbox, text) {

            //alert(text);

            var paramNames = text.split('|');
            var listOfparames = paramNames[0].split(',');


            for (var item = 0; item < listOfparames.length; item++) {
                // alert(listOfparames[item]);
                document.getElementById(listOfparames[item]).style.display = "none";

            }
            listOfparames = paramNames[1].split(',');

            for (var item = 0; item < listOfparames.length; item++) {


                document.getElementById(listOfparames[item]).style.display = "block";

            }


        }
    </script>

    <script type="text/javascript">

         $(document).ready(function () {
             $("#divNCDefcList").dialog({
                 autoOpen: false,
                 modal: true,
                 minHeight: 400,
                 minWidth: 500,
                 height: 400,
                 width: 700,
                 resizable: true,
                 draggable: true,
                 position: ["center top", 40],
                 open: function (event, ui) {
                     $("#divNCDefcList").hide().fadeIn(500);
                     $('body').css({ 'overflow': 'hidden' });
                     $('body').width($('body').width());
                     $(document).bind('scroll', function () {
                         window.scrollTo(0, 0);
                     });
                     $(this).parent().appendTo("#divNCDefcListContainer");
                 },
                 close: function () {

                     $("#divNCDefcList").fadeOut(500);
                 $(document).unbind('scroll');
                 $('body').css({ 'overflow': 'visible' });
             }
             });
         });

         function NCCloseDialog() {

             //Could cause an infinite loop because of "on close handling"
             $("#divNCDefcList").dialog('close');

         }
        
         function NCOpenDialog() {
             $("#divNCDefcList").dialog("option", "title", "Material Deficiency List");
             $("#divNCDefcList").dialog('open');

             $("#divNCDefcList").block({
                 message: '<img src="<%=ResolveUrl("~") %>Images/async_inb.gif" />',
                  css: { border: '0px' },
                  fadeIn: 0,
                  fadeOut: 0,
                  overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
              });
              

          }

          function NCUnblockDialog() {
              $("#divNCDefcList").unblock();
          }
    </script>

    <asp:UpdatePanel ID="upnlDeficiency1" runat="server" UpdateMode="Always">
         <ContentTemplate>

                     <table  align="center" width="900px">

                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2" align="left">
                                         Note:
                                         <asp:Label ID="lberrormsg" runat="server" CssClass="errorMsg" Text=" * " />
                                         Indicates mandatory fields

                            </td>
                             <td align="right" valign="top" >
                                 <asp:ImageButton runat="server" ID="Ibutprint" Visible="false"  ImageUrl="~/Images/blue_menu_icons/Printer.gif" CssClass="NoPrint"  />
                         
                              </td>
                        </tr>
                          
                         <tr>
                             <td colspan="3" class="pagewidth">
                                 <div class="ui-SubHeading ui-SubHeadingBar " id="dvNCHeader" style="">NC Order Header Details</div>

                                    <div class="ui-Customaccordion" id="dvNCBody">
                                        <table border="0" align="center" cellspacing="0" cellpadding="3" width="100%" class="internalData">

                                        <tr>
                                            <td align="left">
                                                <asp:UpdatePanel ChildrenAsTriggers="true" ID="upproNumber" UpdateMode="Always" runat="server">
                                                        <ContentTemplate>
                                                            <asp:RequiredFieldValidator ID="rfvIORefNo" runat="server" ValidationGroup="save" ControlToValidate="txtIORefNo" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:Literal ID="lclIORefNo" runat="server" Text="NC Ref. No.:" /><br />
                                                            <asp:TextBox ID="txtIORefNo" runat="server" Enabled="false"  />
                                                            <asp:ImageButton ID="IbutNew" runat="server" OnClick="IbutNew_Click" ImageUrl="~/Images/blue_menu_icons/add_new.png"  ToolTip="Generate New NC Number"/>
                                                        </ContentTemplate>

                                               </asp:UpdatePanel>

                                            </td>

                                            

                                            <td align="left">
                                                        <asp:RequiredFieldValidator ID="rfvkitCode" runat="server" ValidationGroup="save" ControlToValidate="atcProductionOrderHeader" Display="Dynamic" ErrorMessage=" * " />
                                                        <asp:Literal ID="lclkitCode" runat="server" Text="Kit Code:" /><br />
                                                        <asp:TextBox ID="atckitCode"  runat="server"    SkinID="txt_Auto" />
                                                        <asp:HiddenField ID="hifkitCode" runat="server" />
                                            </td>
                                            <td align="left">
                                                        <asp:RequiredFieldValidator ID="rfvProductionOrderHeader" runat="server" ValidationGroup="save" ControlToValidate="atcProductionOrderHeader" Display="Dynamic" ErrorMessage=" * " />
                                                        <asp:Literal ID="lclProductionOrderHeader" runat="server" Text="Job Order Ref. #:" /><br />
                                                        <asp:TextBox ID="atcProductionOrderHeader"  runat="server"  SkinID="txt_Auto"  />
                                                        <asp:HiddenField ID="hifProductionOrderHeader" runat="server" />
                                            </td>

                                            <td align="left">
                
                                                <asp:Label ID="lbNcStatustext" runat="server"  CssClass="BigCapsHeading" />
                                            </td>

                                        </tr>
       
                                        <tr>
                                            
                                            <td align="left" >
                                                    <asp:RequiredFieldValidator ID="rfvWorkCenter" runat="server" ValidationGroup="save" ControlToValidate="atcWorkCenter" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:Literal  runat="server" ID="ltWorkCenter" Text="Workstation:"/><br />
                                                    <asp:TextBox runat="server" ID="atcWorkCenter" SkinID="txt_Auto" />
                                                    <asp:HiddenField runat="server" ID="hifWorkCenter"/>
                                            </td>
           
                                             <td align="left">
                                                        <asp:RequiredFieldValidator ID="rfvRoutingOperation" runat="server" ValidationGroup="save" ControlToValidate="atcRoutingoperation" Display="Dynamic" ErrorMessage=" * " />
                                                        <asp:Literal ID="ltRoutingOperation" runat="server" Text="Routing Operation:" /><br />
                                                        <asp:TextBox ID="atcRoutingoperation"  runat="server"   SkinID="txt_Auto" />
                                                        <asp:HiddenField ID="hifRouotingOperation" runat="server" />
                                             </td>  
                                            <td align="left" >
                                                        <asp:RequiredFieldValidator ID="rfvActivity" runat="server" ValidationGroup="save" ControlToValidate="atcActivity" Display="Dynamic" ErrorMessage=" * " />
                                                        <asp:Literal ID="ltActivity" runat="server" Text="Activity:" /><br />
                                                        <asp:TextBox ID="atcActivity"  runat="server"  SkinID="txt_Auto"  />
                                                        <asp:HiddenField ID="hifActivity" runat="server" />
                                             </td>
                                              <td align="left" >
                                                        <asp:RequiredFieldValidator ID="rfvReasonForInternalOrderRequest" runat="server" ValidationGroup="save" ControlToValidate="atcReasonForInternalOrderRequest" Display="Dynamic" ErrorMessage=" * " />
                                                        <asp:Literal ID="ltReasonForInternalOrderRequest" runat="server" Text="Reason for Request:" /><br />
                                                        <asp:TextBox ID="atcReasonForInternalOrderRequest"  runat="server"    SkinID="txt_Auto" />
                                                        <asp:HiddenField ID="hifReasonForInternalOrderRequest" runat="server" />
                                             </td>         
                                            
                                        </tr>

                                        <tr> 
                                            <td align="left" colspan="3">
                                                        <asp:RequiredFieldValidator ID="rfvRequestedBy" runat="server" ValidationGroup="save" ControlToValidate="atcRequestedBy" Display="Dynamic" ErrorMessage=" * " />
                                                        <asp:Literal ID="ltRequestedBy" runat="server" Text="Requested By:" /><br />
                                                        <asp:TextBox ID="atcRequestedBy"  runat="server"    SkinID="txt_Auto" />
                                                        <asp:HiddenField ID="hifRequestedBy" runat="server" />
                                             </td>
                                            
                                              <td align="left" colspan="1">
                                                        <asp:CheckBox ID="chkIsActive" Text="Active" runat="server" Visible="false" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:CheckBox ID="chkIsDeleted" onclick="CheckIsDelted(this);" Text="Delete" Visible="false" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     
                                            </td>
                                             
                                         </tr>

                                        <tr>
                                           <td align="left" colspan="2">
                                                    <asp:RequiredFieldValidator ID="rfvDisposition" runat="server" ValidationGroup="save" ControlToValidate="txtRemarks" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:Literal  runat="server" ID="ltRemarks" Text="Remarks:"/><br />
                                                    <asp:TextBox runat="server" onKeypress="return checkSpecialChar(event)" ID="txtRemarks" TextMode="MultiLine"  Width="445" Height="70" />
                    
                                            </td>
                                             <asp:UpdatePanel ID="upnlDeficiency" runat="server" UpdateMode="Always">
                                                     <ContentTemplate>
                                             <td valign="center" align="left">
                
                                                         <asp:LinkButton ID="lnkDeficiency" runat="server" OnClientClick="NCOpenDialog();" OnClick="lnkDeficiency_Click" Text="View Material Deficiency" ForeColor="Red" SkinID="lnkButEmpty" />


                                             </td>
                                            <td colspan="2" align="right" valign="bottom">
                                                 <br />
                                               <!-- <asp:UpdatePanel ID="upnlCreteOutbound" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>-->
                                                        <asp:LinkButton ID="lnkCreateOutbount" Text="Release NC" SkinID="lnkButSave" OnClick="lnkCreateOutbount_Click" runat="server" />
                                                   <!-- </ContentTemplate>
                                                </asp:UpdatePanel>-->
                
                                                <asp:LinkButton ID="lnkClear" runat="server" CssClass="ui-btn ui-button-large" OnClick="lnkClear_Click" >Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkUpdate" runat="server" CssClass="ui-btn ui-button-large"  OnClick="lnkUpdate_Click" ></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               
                                            </td>
                                              </ContentTemplate>
                                                 </asp:UpdatePanel>
                                        </tr>
                
                                        </table>
                                    </div>
                             </td>
                         </tr>
                       
                       <tr>
                           <td class="accordinoGap"></td>
                       </tr>
                         <tr>
                             <td colspan="3" class="pagewidth" id="tdlineitems" runat="server">
                                 <div class="ui-SubHeading ui-SubHeadingBar " id="dvNCOrderMDHeader" style="">NC Order Material Details</div>
                                <div class="ui-Customaccordion" id="dvNCOrderMDBody">
                                    <table border="0" align="center" cellspacing="0" cellpadding="3" width="100%" class="internalData">
                                         <tr>
                            <td align="right" colspan="3" style="padding-right:10px;padding-bottom:10px;" >
                                <asp:LinkButton ID="lnkAddNewLineItem" runat="server" OnClick="lnkAddNewLineItem_Click" CssClass="ui-button-small" >Add Material<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="3" align="center">
                                <!-- <asp:UpdatePanel ID="upnlInternalOrderDetails" runat="server"  UpdateMode="Always">
                                       <ContentTemplate>-->
                                        <asp:Panel  ID="pnlgvInternalOrderDetails" runat="server" ScrollBars="Horizontal" Width="900px"  HorizontalAlign="Center">
                            
                                            <asp:GridView ID="gvIODetailsList" SkinID="gvLightGrayNew" runat="server" PageSize="15" OnPageIndexChanging="gvIODetailsList_PageIndexChanging" AutoGenerateColumns="false" OnRowEditing="gvIODetailsList_RowEditing" OnRowUpdating="gvIODetailsList_RowUpdating" OnRowCancelingEdit="gvIODetailsList_RowCancelingEdit" >
                                                <Columns>
                                    
                                                    <asp:TemplateField HeaderText="Part Number" HeaderStyle-Width="150" ItemStyle-Width="100" >
                                                        <ItemTemplate>
                                                            <asp:literal id="ltMaterialMaster" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                                                             <asp:Literal ID="ltInternalOrderDetailsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "InternalOrderDetailsID").ToString() %>' />

                                                            <span style="color:#1287a1;"> <nobr> <%# DataBinder.Eval(Container.DataItem, "OEMPartNo").ToString() %>  </nobr> </span>

                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                             <asp:Literal ID="ltInternalOrderDetailsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "InternalOrderDetailsID").ToString() %>' />
                                                            <asp:RequiredFieldValidator ID="rfvMaterialMaster" runat="server" ValidationGroup="UpdateGridItems" ControlToValidate="atcMaterialMaster" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:TextBox ID="atcMaterialMaster" runat="server" ClientIDMode="Static" SkinID="txt_Auto" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                                                            <asp:HiddenField ID="hifMaterialMaster" ClientIDMode="Static" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString() %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText=" UoM/Qty." HeaderStyle-Width="150" ItemStyle-Width="100" >
                                                        <ItemTemplate>
                                                            <asp:literal id="ltMaterialMaster_IOUoM" runat="server" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "UoM").ToString(),DataBinder.Eval(Container.DataItem, "UoMQty").ToString())  %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:RequiredFieldValidator ID="rfvBoMUom_Qty" runat="server" ValidationGroup="UpdateGridItems" Enabled="false" ControlToValidate="atcMaterialMaster_IOUoMID" Display="Dynamic" ErrorMessage=" * " />
                                                           <asp:literal id="ltMaterialMaster_IOUoM" runat="server" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "UoM").ToString(),DataBinder.Eval(Container.DataItem, "UoMQty").ToString())  %>' />
                                                             <asp:TextBox ID="atcMaterialMaster_IOUoMID" Width="100" Visible="false" SkinID="txt_Auto" runat="server" ClientIDMode="Static" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "UoM").ToString(),DataBinder.Eval(Container.DataItem, "UoMQty").ToString())  %>'  />
                                                            <asp:HiddenField ID="hifMaterialMaster_IOUoMID" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "MaterialMaster_IOUoMID").ToString() %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText=" Quantity" ItemStyle-Width="100" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:literal id="ltIOQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IOQuantity").ToString() %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:RequiredFieldValidator ID="rfvPROQuantity" runat="server" ValidationGroup="UpdateGridItems" ControlToValidate="txtIOQuantity" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:TextBox ID="txtIOQuantity" Width="100" onKeyPress=" return checkDec(this,event)" onblur="CheckDecimal(this)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IOQuantity").ToString() %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                               
                       
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                  <!--  </ContentTemplate>
                                </asp:UpdatePanel>-->
                                            <br /><br />
                            </td>
                        </tr>

                                    </table>
                                </div>
                             </td>
                         </tr>
                         <tr><td class="accordinoGap"></td></tr>
                        

                    
         
                       

                    </table>

         </ContentTemplate>
    </asp:UpdatePanel>    

     <div id="divNCDefcListContainer">  
         <div id="divNCDefcList" style="display:block;padding:35px;" >  
            <asp:UpdatePanel ID="upnlNCDefcList" runat="server" ChildrenAsTriggers="true" UpdateMode="Always" >
                <ContentTemplate>
                                                                        <asp:Panel ID= "pnlNCDefcList" runat="server" Width="500px"  HorizontalAlign="Center"  >
                                                                           <br /><br />
                                <asp:GridView Width="100%" ShowHeaderWhenEmpty="true"  ShowFooter="true"   ID="gvNCDefcList" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"  SkinID="gvLightSeaBlueNew" HorizontalAlign="Left"   OnPageIndexChanging="gvNCDefcList_PageIndexChanging" OnRowDataBound="gvNCDefcList_RowDataBound" >
                                                                                <Columns>
                                                                                     <asp:TemplateField ItemStyle-Width="150" HeaderText="Part Number"  >
                                                                                        <ItemTemplate>
                                                                                            <asp:Literal runat="server" ID="ltMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>'/>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="Required Qty."  >
                                                                                        <ItemTemplate>
                                                                                            <asp:Literal runat="server" ID="ltRequiredQuantity" Text='<%# DataBinder.Eval(Container.DataItem, "RequiredQuantity") %>'/>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Available Qty."  >
                                                                                        <ItemTemplate>
                                                                                            <asp:Literal runat="server" ID="ltAvialableQty" Text='<%# DataBinder.Eval(Container.DataItem, "AvailableQty") %>'/>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="Deficiency"  >
                                                                                        <ItemTemplate>
                                                                                            <asp:Literal runat="server" ID="ltDeficiency" Text='<%# DataBinder.Eval(Container.DataItem, "Deficiency") %>'/>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                            <br /><br />
                                                                        </asp:Panel>
                </ContentTemplate>  
           </asp:UpdatePanel>
       </div>
    </div>

</asp:Content>
