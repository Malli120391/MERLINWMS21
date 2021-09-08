<%@ Page Title="Job Order:." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="ProductionOrder.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.ProductionOrder"  MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1"  ContentPlaceHolderID="ManfContent" runat="server">
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="ss" SupportsPartialRendering="true" ></asp:ScriptManager>
   
    <script type="text/javascript" src="Scripts/jquery.blockUI.js"></script>
    
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
  
    <style>
        img
        {  
            border-style: none;
            
        }
        .HeaderClass {
            color:blue;
            font-size:13px;
            
        }
    </style>

    <script type="text/javascript">

         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
         function EndRequestHandler(sender, args) {
             if (args.get_error() == undefined) {
                 fnMCodeAC();
             }
         }


         function fnMCodeAC() {
             $(document).ready(function () {

                 $(document).ready(function () {

                     $("#<%=this.txtStartDate.ClientID%>").datepicker({
                         dateFormat: "dd/mm/yy",
                         minDate: 0,
                         onSelect: function (selected) {
                             $("#<%=this.txtDueDate.ClientID%>").datepicker("option", "minDate", selected, { dateFormat: "dd/mm/yy" })

                        }

                     });

                     var _minDate = new Date(1990, 1, 1, 0, 0, 0);
                     if (document.getElementById('<%=this.txtStartDate.ClientID%>').value != "") {
                          var date = document.getElementById('<%=this.txtStartDate.ClientID%>').value;
                        _minDate = new Date(date.split('/')[2], parseInt(date.split('/')[1]) - 1, date.split('/')[0], 0, 0, 0, 0);
                    }

                     $("#<%=this.txtDueDate.ClientID%>").datepicker({
                         dateFormat: "dd/mm/yy",
                         minDate: _minDate,
                         onSelect: function (selected) {
                             $("#<%=this.txtStartDate.ClientID%>").datepicker("option", "maxDate", selected, { dateFormat: "dd/mm/yy" })
                        }
                    });

                 });


                 $('.DateBoxCSS_small').datepicker({ dateFormat: 'dd/mm/yy' });


                 var textfieldname = $('#<%=atcSOHeader.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcSOHeader.ClientID%>').autocomplete({
                       source: function (request, response) {
                           $.ajax({
                               url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSONumbersForPROHeader") %>',
                               data: "{ 'ProductionOrderHeaderID': '" + <%=ViewState["HeaderID"]%> + "','KitCode':'" + document.getElementById('<%=this.txtkitCode.ClientID%>').value + "','MaterialMasterRevision':'" + document.getElementById('<%=this.hifMaterialRevision.ClientID%>').value + "','TenantID':'" +<%=cp.TenantID%> +"'}",
                               dataType: "json",
                               type: "POST",
                               contentType: "application/json; charset=utf-8",
                               success: function (data) {
                                   if (data.d == "") {
                                       alert('No Sales Order for Job Order');
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

                           $("#<%=hifSOHeader.ClientID %>").val(i.item.val);
                           try {
                               //CheckAutoGenerateKitCode();
                           }
                           catch (err)
                           { }
                       },
                       minLength: 0
                 });

                 var textfieldname = $('#<%=atcMaterialRevision.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcMaterialRevision.ClientID%>').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMaterialListForProduction") %>',
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

                         $("#<%=hifMaterialRevision.ClientID %>").val(i.item.val);
                            try {
                                document.getElementById('<%=this.atcSOHeader.ClientID%>').value = "";
                                document.getElementById('<%=this.hifSOHeader.ClientID%>').value = "";
                                document.getElementById('<%=this.atcRoutingHeaderVersion.ClientID%>').value = "";
                                document.getElementById('<%=this.hifRoutingHeaderVersion.ClientID%>').value = "";
                                document.getElementById('<%=this.txtkitCode.ClientID%>').value = "";
                                document.getElementById('<%=this.atcProductionUoM.ClientID%>').value = "";
                                document.getElementById('<%=this.hifProductionUoM.ClientID%>').value = "";
                            }
                            catch (err) {
                            }
                      },
                       minLength: 0
                 });

                 var textfieldname = $('#<%=atcRoutingHeaderVersion.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcRoutingHeaderVersion.ClientID%>').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRoutingVersionRefNoList") %>',
                             data: "{ 'prefix': '" + request.term + "','MaterialMasterRevisionID':'" + document.getElementById('<%=this.hifMaterialRevision.ClientID%>').value + "'}",
                             dataType: "json",
                             type: "POST",
                             contentType: "application/json; charset=utf-8",
                             success: function (data) {
                                 if (data.d == "") {
                                     alert('No routing is configured to this material');
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

                          $("#<%=hifRoutingHeaderVersion.ClientID %>").val(i.item.val);
                          
                     },
                     minLength: 0
                 });

                 var textfieldname = $('#<%=atcProductionUoM.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcProductionUoM.ClientID%>').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMForPRO") %>',
                             data: "{ 'RoutingHeaderVersionID': '" + document.getElementById('<%=hifRoutingHeaderVersion.ClientID%>').value + "'}",
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

                          $("#<%=hifProductionUoM.ClientID %>").val(i.item.val);
                      },
                      minLength: 0
                 });

                 var textfieldname = $('#<%=atcShopAcceptanceby.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=this.atcShopAcceptanceby.ClientID%>').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersBasedRole") %>',
                             data: "{ 'Prefix': '" + request.term + "','TenantID':'" +<%=this.cp.TenantID%> +"'}",
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

                          $("#<%=this.hifShopAcceptanceby.ClientID%>").val(i.item.val);
                      },
                      minLength: 0
                 });


                 var textfieldname = $('#<%=atcSupervisorAcceptanceBy.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=this.atcSupervisorAcceptanceBy.ClientID%>').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersBasedRole") %>',
                             data: "{ 'Prefix': '" + request.term + "','TenantID':'" +<%=this.cp.TenantID%> +"'}",
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

                         $("#<%=this.hifSupervisorAcceptanceBy.ClientID%>").val(i.item.val);
                      },
                      minLength: 0
                 });

                 var textfieldname = $('#<%=atcQAAcceptanceBy.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=this.atcQAAcceptanceBy.ClientID%>').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersBasedRole") %>',
                             data: "{ 'Prefix': '" + request.term + "','TenantID':'" +<%=this.cp.TenantID%> +"'}",
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

                         $("#<%=this.hifQAAcceptanceBy.ClientID%>").val(i.item.val);
                      },
                      minLength: 0
                 });


                 var textfieldname = $('#<%=atcProductionType.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=this.atcProductionType.ClientID%>').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadproductionType") %>',
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

                         $("#<%=this.hifproductionType.ClientID%>").val(i.item.val);


                         /*if (document.getElementById("<=this.hifproductionType.ClientID%>").value == "10") {

                             document.getElementById("<=this.imgKitCode.ClientID%>").style.display = "inline";


                         }
                         else {
                             document.getElementById("<=this.imgKitCode.ClientID%>").style.display = "none";
                         }*/

                     },
                     minLength: 0
                 });

                 var textfieldname = $('#atcBOMHeader');
                 DropdownFunction(textfieldname);
                 $('#atcBOMHeader').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadBoMRefNoList") %>',
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

                          $("#hifBOMHeader").val(i.item.val);
                      },
                      minLength: 0
                 });

                 var textfieldname = $('#atcBoMUom_Qty');
                 DropdownFunction(textfieldname);
                 $('#atcBoMUom_Qty').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMForPRODetails") %>',
                             data: "{ 'BoMHeaderID': '" + document.getElementById('hifBOMHeader').value + "'}",
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

                         $("#hifBoMUom_Qty").val(i.item.val);
                      },
                      minLength: 0
                 });
             });

         }
         fnMCodeAC();
         
  </script>

    <script>
        $(document).ready(function () {

            $("#divWorkOrder").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 50,
                minWidth: 500,
                height: 650,
                width: 600,
                resizable: false,
                draggable: false,
                position: ["center top", 40],
                open: function (event, ui) {
                   
                    $("#divWorkOrder").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });

                    $(this).parent().appendTo("#disputeDivWorkOrder");
                },
                close: function () {
                    $("#divWorkOrder").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                }
            });

        });

        function closeDialog() {
            //Could cause an infinite loop because of "on close handling"
            $("#divWorkOrder").dialog('close');
        }

        function openDialog() {
            

            $("#divWorkOrder").dialog("option", "title", "Workstation Configuration");
            $("#divWorkOrder").dialog('open');

            NProgress.start();

            $("#divWorkOrder").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_inb.gif" />',
                 css: { border: '0px' },
                 fadeIn: 0,
                 fadeOut: 0,
                 overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
             });
            unblockDialog();
        }

         function unblockDialog() {
             $("#divWorkOrder").unblock();
             NProgress.done();
         }

    </script>

    <script>
        
        function CheckAutoGenerateKitCode() {
            var SOHeaderID = document.getElementById('<%=this.hifSOHeader.ClientID%>').value;
            if (SOHeaderID == "")
                SOHeaderID = "0";
           
            $.ajax({
                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/CheckAutoKitNoorNot") %>',
                data: "{ 'SOHeaderID': '" + SOHeaderID + "'}",
                 dataType: "json",
                 type: "POST",
                 contentType: "application/json; charset=utf-8",
                 success: function (data) {
                     //alert(data.d);
                     if (data.d == "0") {
                         document.getElementById('<%=this.txtkitCode.ClientID%>').disabled = false;

                         document.getElementById('<%=this.imgKitCode.ClientID%>').style.display = "none";
                     }
                     else {
                         document.getElementById('<%=this.txtkitCode.ClientID%>').disabled = true;
                         document.getElementById('<%=this.imgKitCode.ClientID%>').disabled = false;
                         document.getElementById('<%=this.txtkitCode.ClientID%>').value = "";
                     }
                     
                 }
             });
        }


        function CheckIsDelted(checkBox) {
            if (checkBox.checked) {
                alert('Are you sure want to delete the Job Order?');
            }
        }


        function checkQty(TextBox) {
         
            var Parenttablerow = TextBox.parentNode.parentNode;
             var WorkOrdertable = document.getElementById('tbWorkgroupDetails');
            var trList = WorkOrdertable.getElementsByTagName('tr');
            
            var firstSpaceindex = 0;
           
            for (var posi = 0; posi < trList.length; posi++) {
                var tt = trList[posi].attributes.getNamedItem("class");
               
               if (tt != null) {
                   firstSpaceindex = posi;
                }
                if (trList[posi] == Parenttablerow) {
                    //var tr=$(trList[posi]).closest('td');
                    break;
                  //  posi = 42;
                }
            }
            var sumOfQty = 0.0;
            for (var trIndex = firstSpaceindex + 1; trIndex < trList.length; trIndex++) {
              
                var rr = trList[trIndex].getElementsByTagName('td');
                if (rr.length < 3) {
                    break;
                }
               // alert(rr[0].childNodes[1].checked);
                 if (rr[2].childNodes[0].value != "") {
                    sumOfQty += parseFloat(rr[2].childNodes[0].value);
                 
                }
            }
            var Quantity = document.getElementById("<%=this.lbqty.ClientID%>").innerText;
            var proQty = parseFloat(Quantity);
            if (sumOfQty > proQty)
                alert('Assign Work Center Quantity is More than Production Quantity');
            
        }

        function CheckCheckBox(checkBox) {
            var ChickBoxList = document.getElementsByClassName("ChildCheckBox");

            if (checkBox.id == "HeaderCheckBox") {

                for (index = 0; index < ChickBoxList.length; index++) {

                    ChickBoxList[index].firstChild.checked = checkBox.checked;
                }
            }
            else {
                var CheckedAll = true;
                var NotCheckedAll = true;

                for (index = 0; index < ChickBoxList.length; index++) {

                    if (ChickBoxList[index].firstChild.checked) {
                        NotCheckedAll = false;
                    }
                    else {
                        CheckedAll = false;
                    }
                }

                if (CheckedAll) {
                    document.getElementById("HeaderCheckBox").checked = true;
                }
                else if (NotCheckedAll || !CheckedAll) {
                    document.getElementById("HeaderCheckBox").checked = false;
                }
            }
        }

        
    </script>

    <script type="text/javascript">

         $(document).ready(function () {
             $("#divProdDefcList").dialog({
                 autoOpen: false,
                 modal: true,
                 width: '600',
                 height: '500',
                 resizable: false,
                 draggable: false,
                 position: ["center top", 40],
                 open: function (event, ui) {
                     $("#divProdDefcList").hide().fadeIn(500);
                     $('body').css({ 'overflow': 'hidden' });
                     $('body').width($('body').width());
                     $(document).bind('scroll', function () {
                         window.scrollTo(0, 0);
                     });
                     $(this).parent().appendTo("#divProdDefcListContainer");
                 },
                 close: function () {
                     $("#divProdDefcList").fadeOut(500);
                     $(document).unbind('scroll');
                     $('body').css({ 'overflow': 'visible' });
                 }
             });
         });
        
          function ProdCloseDialog() {

              //Could cause an infinite loop because of "on close handling"
              $("#divProdDefcList").dialog('close');

          }

          function ProdOpenDialog() {
              $("#divProdDefcList").dialog("option", "title", "Material Deficiency List");
              $("#divProdDefcList").dialog('open');

              NProgress.start();

              $("#divProdDefcList").block({
                  message: '<img src="<%=ResolveUrl("~") %>Images/async_inb.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });

             

        }

        function ProdUnblockDialog() {
            $("#divProdDefcList").unblock();
            NProgress.done();
        }
    </script>
    
     <script type="text/javascript">

         $(document).ready(function () {
             $("#divMaterialPending").dialog({
                 autoOpen: false,
                 modal: true,
                 minheight:20,
                 height: 570,
                 minWidth: 780,
                 width: 'auto',
                 resizable: false,
                 draggable: false,
                 position: ["center top", 40],
                 open: function (event, ui) {
                     $("#divMaterialPending").hide().fadeIn(500);
                     $('body').css({ 'overflow': 'hidden' });
                     $('body').width($('body').width());
                     $(document).bind('scroll', function () {
                         window.scrollTo(0, 0);
                     });
                     $(this).parent().appendTo("#divMaterialPendingBOD");
                 },
                 close: function () {
                     $("#divMaterialPending").fadeOut(500);
                 $(document).unbind('scroll');
                 $('body').css({ 'overflow': 'visible' });
             }
             });
         });
       
         function Pen_Mat_CloseDialog() {

             //Could cause an infinite loop because of "on close handling"
             $("#divMaterialPending").dialog('close');

         }

         function Pen_Mat_OpenDialog() {
             $("#divMaterialPending").dialog("option", "title", "Material Pending List");
             $("#divMaterialPending").dialog('open');
             NProgress.start();
             $("#divMaterialPending").block({
                 message: '<img src="<%=ResolveUrl("~") %>Images/async_inb.gif" />',
                  css: { border: '0px' },
                  fadeIn: 0,
                  fadeOut: 0,
                  overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
              });
             

          }

         function Pen_Mat_UnblockDialog() {
             $("#divMaterialPending").unblock();
             NProgress.done();
          }
    </script>

     <div id="disputeDivWorkOrder">
        <div id="divWorkOrder"  >
             <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlParentWorkOrderDialog" runat="server" UpdateMode="Always">
                 <ContentTemplate>

                     
                     <table colspan="2" width="95%" rowspan="2" align="center" width="570px">


                         <tr>
                             <td>
                                 <asp:Literal ID="ltDialogBoxStatus" runat="server"></asp:Literal>
                             </td>
                         </tr>
                         <tr>
                             <td align="Center">
                                 <h2>Workstation  Configuration</h2>
                             </td>
                         </tr>
                         <tr>
                             <td>Job Order Ref. #:   
                                                    <asp:Literal ID="ltprorefno" runat="server"></asp:Literal><br />
                                 <br />
                                 Quantity:   
                                                    <asp:Label ID="lbqty" runat="server"></asp:Label>
                                 <br />
                                 <br />
                                 Routing Ref. No.:
                                                    <asp:Literal ID="ltRoutingno" runat="server"></asp:Literal>
                                 <br />
                                 <br />
                             </td>
                         </tr>

                         <tr>
                             <td>
                                 <div class="ui-dailog-body" style="height:380px; padding: 10px;">
                                     <div id="dvWorkCenterdetails" runat="server"></div>
                                 </div>

                             </td>
                         </tr>
                         


                     </table>
                     
                     
                     <div class="ui-dailog-footer">
                         <div style="padding: 15px 13px 15px 5px;">
                             <asp:LinkButton ID="lnkclose" OnClick="lnkclose_Click" OnClientClick="closeDialog()" runat="server" CssClass="ui-btn ui-button-large">Close<%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkAssignWorkOrder" OnClick="lnkAssignWorkOrder_Click" OnClientClick="showAsynchronus();" CssClass="ui-btn ui-button-large" runat="server">Assign Workstation<%=MRLWMSC21Common.CommonLogic.btnfaRightArrow %></asp:LinkButton>
                         </div>
                     </div>
                 </ContentTemplate>
             </asp:UpdatePanel>
        </div>
    </div>
     

     <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlProductionorder" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table width="80%" align="center" cellpadding="5" cellspacing="3">

        <tr>
            <td>
                &nbsp;
            </td>
          
        </tr>

        
             <tr>
                 <td colspan="1" align="left" class="FormLabels">
                         Note:<asp:Label ID="lberrormsg" runat="server" CssClass="errorMsg" Text=" * " />
                         Indicates mandatory fields

            </td>
            <td class="FormLabels" colspan="2"  align="right" >
                <asp:ImageButton ID="ImgChangeRevision" ImageUrl="~/Images/Clonerevision-20.png" Visible="false" ToolTip="Change New Routing Revision" OnClientClick="return confirm('Are you sure want to Change New Routing Revision?');" OnClick="ImgChangeRevision_Click" runat="server" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="lmbWorkOrderReport" ImageUrl="../Images/Configure PRO 20X20.png" Visible="false" OnClientClick="openDialog()"  runat="server"></asp:ImageButton>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <a target="_blank"  style="text-decoration:none;display:none" href="JobRoutePlan.aspx?poid=<%=ViewState["HeaderID"] %>"><image src="../Images/Print 20X20..png" Title=" Print Job Card"></image></a> 
                <asp:UpdatePanel ChildrenAsTriggers="true" ID="uplWorkstationConfigure" UpdateMode="Always" runat="server">
                          <ContentTemplate>
                                <asp:LinkButton runat="server" CssClass="ui-button-small" Visible="false" ForeColor="#333333" ID="lnkworkstationconfiguration" OnClick="lnkworkstationconfiguration_Click" OnClientClick="openDialog();">
                                    <%--<img src="../Images/Configure PRO 20X20.png" />--%> Workstation  Configuration<%=MRLWMSC21Common.CommonLogic.btnfaGear %>
                                </asp:LinkButton>
                          </ContentTemplate>
                        <%--<Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkworkstationconfiguration" EventName="OnClick" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                
            </td>
         </tr>
          <tr>
              <td class="pagewidth" colspan="3">
                  <div class="ui-SubHeading ui-SubHeadingBar " id="dvMDHeader" style=""><%=ViewState["RoutingType"] %> Job Order Header</div>
                <div class="ui-Customaccordion" id="dvMDBody">
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="internalData">
                         

                    <tr>
                        <td align="left" >
                                    <asp:UpdatePanel ChildrenAsTriggers="true" ID="upproNumber" UpdateMode="Always" runat="server">
                                            <ContentTemplate>
                                                <asp:RequiredFieldValidator ID="rfvMaterialRevision" runat="server" ValidationGroup="save" ControlToValidate="atcMaterialRevision" Display="Dynamic" ErrorMessage=" * " />
                                                <asp:Literal ID="lclMaterialRevision" runat="server" Text="Job Order Ref. #:" /><br />
                                                <asp:TextBox ID="atcMaterialRevision" runat="server" Enabled="true"  SkinID="txt_Auto" />
                                                <asp:HiddenField ID="hifMaterialRevision" runat="server" />
                                                <asp:ImageButton ID="IbutNew" Visible="false" runat="server" OnClick="IbutNew_Click" ImageUrl="~/Images/blue_menu_icons/add_new.png"  ToolTip="Generate New PRO Number"/>
                                            </ContentTemplate>

                                   </asp:UpdatePanel>

                                </td>

                        <td >
                            <asp:RequiredFieldValidator ID="rfvJobRefNo" runat="server" ValidationGroup="save" ControlToValidate="txtJobRefNo" Display="Dynamic" ErrorMessage=" * " />
                            Job Order #:<br />
                            <asp:TextBox ID="txtJobRefNo" runat="server" onKeypress="return checkSpecialChar(event)"></asp:TextBox>
                        </td>
                        <td>
                                     

                                    <asp:RequiredFieldValidator ID="rfvproductionType" runat="server" ValidationGroup="save" ControlToValidate="atcProductionType" Display="Dynamic" ErrorMessage=" * " />
                                    Job Type:<br />
                                    <asp:TextBox ID="atcProductionType" runat="server" SkinID="txt_Auto" onKeypress="return checkSpecialChar(event)" />
                                    <asp:HiddenField ID="hifproductionType" runat="server"  />

                                </td>
                        <td >
                                        <asp:Label id="ltPROStatus" runat="server" CssClass="BigCapsHeading" />

                                    </td>
                    </tr>

                            <tr>
                                
                                <td align="left">


                
                                        <asp:RequiredFieldValidator ID="rfvProductionDate" Enabled="false" runat="server" ValidationGroup="save" ControlToValidate="txtStartDate" Display="Dynamic" ErrorMessage=" * " />
                                        <asp:Literal ID="ltProductionDate" runat="server" Text="Job Order Date:" /><br />
                                        <asp:TextBox ID="txtProductionDate" EnableTheming="false" Enabled="false" CssClass="DateBoxCSS_small" runat="server"  Width="200" />
                                           

                                </td>
                                <td align="left">
                
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ValidationGroup="save" ControlToValidate="txtStartDate" Display="Dynamic" ErrorMessage=" * " />
                                        <asp:Literal ID="ltStartDate" runat="server" Text="Start Date:" /><br />
                                        <asp:TextBox ID="txtStartDate"  runat="server"  Width="200" />
                                           

                                </td>
                                <td align="left" colspan="1">
                                            <asp:RequiredFieldValidator ID="rfvDueDate" runat="server" ValidationGroup="save" ControlToValidate="txtDueDate" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal ID="ltDueDate" runat="server" Text="Due Date:" /><br />
                                            <asp:TextBox ID="txtDueDate"  runat="server"    Width="200" />
                        
                                 </td>
                              
            
                                 <td align="left" >

                                    <asp:UpdatePanel ChildrenAsTriggers="true" ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                         <ContentTemplate>
                                            <asp:RequiredFieldValidator ID="rfvkitCode" runat="server" ValidationGroup="save" ControlToValidate="txtkitCode" Display="Dynamic" ErrorMessage=" * " />
                                            Kit Code:<br />
                                            <asp:TextBox ID="txtkitCode" runat="server" onfocus="check()" Width="200" onKeypress="return checkSpecialChar(event)" />
                                            <asp:ImageButton ID="imgKitCode" runat="server" Visible="false" OnClick="imgKitCode_Click" ImageUrl="~/Images/blue_menu_icons/add_new.png"  ToolTip="Generate New KitCode"/>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>

                            </tr>

                           
                            <tr>
                                  
                                <td align="left" >
                                            <asp:RequiredFieldValidator ID="rfvSOHeader" runat="server" ValidationGroup="save" ControlToValidate="atcSOHeader" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal ID="lclSOHeader" runat="server" Text="Sales Order Ref. #:" /><br />
                                            <asp:TextBox ID="atcSOHeader"  runat="server"    SkinID="txt_Auto" />
                                            <asp:HiddenField ID="hifSOHeader" runat="server" />
                                </td>
                                <td align="left" >
                
                                        <asp:RequiredFieldValidator ID="rfvRoutingHeaderVersion" runat="server" ValidationGroup="save" ControlToValidate="atcRoutingHeaderVersion" Display="Dynamic" ErrorMessage=" * " />
                                        <asp:Literal ID="ltRoutingHeaderVersion" runat="server" Text="Routing Version Ref. #:" /><br />
                                        <asp:TextBox ID="atcRoutingHeaderVersion" runat="server" SkinID="txt_Auto" />
                                        <asp:HiddenField ID="hifRoutingHeaderVersion" runat="server" />
                                </td>
                                <td align="left">
                                            <asp:RequiredFieldValidator ID="rfvProductionUoM" runat="server" ValidationGroup="save" ControlToValidate="atcProductionUoM" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal ID="ltProductionUoM" runat="server" Text="UoM:" /><br />
                                            <asp:TextBox ID="atcProductionUoM"  runat="server"    SkinID="txt_Auto" />
                                            <asp:HiddenField ID="hifProductionUoM" runat="server" />
                                 </td>
                                <td align="left" colspan="1">
                                        <asp:RequiredFieldValidator ID="rfvProductionQuantity" runat="server" ValidationGroup="save" ControlToValidate="txtProductionQuantity" Display="Dynamic" ErrorMessage=" * " />
                                        <asp:Literal  runat="server" ID="ltProductionQuantity" Text="Quantity:"/><br />
                                        <asp:TextBox runat="server" Enabled="false" Text="1.00" ID="txtProductionQuantity" onKeyPress=" return checkDec(event)" onblur="CheckDecimal(this)" Width="200" />
                    
                                </td>
                              

            

                            </tr>

                            <tr>
                                  
                               
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="save" Enabled="false" ControlToValidate="atcProductionType" Display="Dynamic" ErrorMessage=" * " />
                                    Manufacturing Date:<br />
                                    <asp:TextBox ID="txtManufacturingDate" CssClass="DateBoxCSS_small" EnableTheming="false" runat="server"  Width="200"  />
                                </td>
                           
                                <td colspan="1">
                                    <asp:RequiredFieldValidator ID="rfvCoCNumber" runat="server" Enabled="false" ValidationGroup="save" ControlToValidate="txtCoCnumber" Display="Dynamic" ErrorMessage=" * " />
                                   CoC Number:<br />
                                    <asp:TextBox ID="txtCoCnumber"  runat="server"  Width="200" onKeypress="return checkSpecialChar(event)" />
                                </td>
                                <td colspan="1">
                                    CoC Date:<br />
                                    <asp:TextBox ID="txtCoCDate"  runat="server" EnableTheming="false"  CssClass="DateBoxCSS_small" />
                                </td>
                                <td colspan="1">
                                    Contract No.:<br />
                                    <asp:TextBox ID="txtContractNo"  runat="server"   onKeypress="return checkSpecialChar(event)" />
                                </td>
                            </tr>
                            <tr>
                                 <td>
                                     <asp:RequiredFieldValidator ID="rfvShopAcceptanceby" runat="server" Enabled="false" ValidationGroup="save" ControlToValidate="atcShopAcceptanceby" Display="Dynamic" ErrorMessage=" * " />
                                    Shop Acceptance By:<br />
                                    <asp:TextBox ID="atcShopAcceptanceby" runat="server" SkinID="txt_Auto" onKeypress="return checkSpecialChar(event)" />
                                    <asp:HiddenField ID="hifShopAcceptanceby" runat="server" />
                                 </td>
                                 <td>
                                     <asp:RequiredFieldValidator ID="rfvSupervisorAcceptanceBy" Enabled="false" runat="server" ValidationGroup="save" ControlToValidate="atcShopAcceptanceby" Display="Dynamic" ErrorMessage=" * " />
                                    Supervisor Acceptance By:<br />
                                    <asp:TextBox ID="atcSupervisorAcceptanceBy" runat="server" SkinID="txt_Auto" onKeypress="return checkSpecialChar(event)" />
                                    <asp:HiddenField ID="hifSupervisorAcceptanceBy" runat="server" />
                                 </td>
                                 <td>
                                     <asp:RequiredFieldValidator ID="rfvQAAcceptanceBy" runat="server" Enabled="false" ValidationGroup="save" ControlToValidate="atcQAAcceptanceBy" Display="Dynamic" ErrorMessage=" * " />
                                    QA Acceptance By:<br />
                                    <asp:TextBox ID="atcQAAcceptanceBy" runat="server" SkinID="txt_Auto" onKeypress="return checkSpecialChar(event)" />
                                    <asp:HiddenField ID="hifQAAcceptanceBy" runat="server" />
                                 </td>
                             </tr>

                            <tr>
                                <td colspan="2">
                                    Addtional Observations:<br />
                                    <asp:TextBox ID="txtAddtionalObservations" Width="87%" runat="server" TextMode="MultiLine" Height="70" />
                                </td>
                            
                                <td align="left" colspan="2">
                    
                                        <asp:Literal  runat="server" ID="ltRemarks" Text="Remarks:"/><br />
                                        <asp:TextBox runat="server" ID="txtRemarks" onKeypress="return checkSpecialChar(event)" TextMode="MultiLine"  Width="90%" Height="70" />
                    
                                </td>
                            </tr>
        
                            <tr>
              
       
                                <td align="right" colspan="4" style="padding-right:16px">
                                            <asp:CheckBox ID="chkIsActive" Text="Active" Enabled="false" runat="server" Visible="false" />&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="chkIsDeleted" onclick="CheckIsDelted(this);" Text="Delete" runat="server" Visible="false" />&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;
                                </td>
           
                            </tr>

            
                
        
        <tr >
            <td colspan="4" class="SubHeadingBarRed" id="tdJoborderDefc" runat="server">
                This job order has insufficient materials to release
            </td>
        </tr>    
                                                    
        <tr>
                                <td colspan="3">
                                    &nbsp;

                                </td>
                            </tr>
                                                           
        <tr>
            
            <td align="right">
                 <asp:LinkButton ID="lnkMaterialDeficiency" Visible="false" OnClick="lnkMaterialDeficiency_Click" Text="View Material Deficiency" runat="server" SkinID="lnkButEmpty" OnClientClick="ProdOpenDialog()"/>&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td colspan="3" align="right" style="padding-right:36px">
                <asp:Panel ID="pnlUpdate" runat="server" Width="540px" HorizontalAlign="Right">
                    <asp:HiddenField ID="hifStatus" runat="server" />
                        <asp:LinkButton ID="lnkChangeStatus" CssClass="ui-btn ui-button-large" runat="server"  Visible="false" OnClick="lnkChangeStatus_Click"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkReleaseJobOrder" CssClass="ui-btn ui-button-large" runat="server" OnClientClick="Pen_Mat_OpenDialog()" OnClick="lnkReleaseJobOrder_Click">Release Job<%=MRLWMSC21Common.CommonLogic.btnfaSignOut %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkClear" runat="server" CssClass="ui-btn ui-button-large" OnClick="lnkClear_Click" >Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkUpdate" runat="server" OnClientClick="showAsynchronus();" CssClass="ui-btn ui-button-large" OnClick="lnkUpdate_Click" ></asp:LinkButton>
                        <br />
                </asp:Panel>
            </td>
        </tr>

        <tr>
                                <td>
                                    &nbsp;

                                </td>
                            </tr>

        <tr style="display:none;">
             <td class="SubHeadingBar" id="tdlineitems" runat="server" colspan="3" visible="false">
                Job Order LineItems

            </td>
        </tr>

        <tr>
            <td>
                &nbsp;
            </td>
        </tr> 
         
        <tr style="display:none;">
            <td align="right" colspan="3">
                <asp:LinkButton ID="lnkAddNewLineItem" visible="false" runat="server" Text="Add New LineItem" OnClick="lnkAddNewLineItem_Click" SkinID="lnkButEmpty" />
            </td>
        </tr>

        <tr style="display:none;">
            <td colspan="3" align="center" visible="false">
                <asp:Literal ID="ltGridStatus" runat="server" /><br />
                <asp:GridView ID="gvPRODetailsList" SkinID="gvLightBlueNew" runat="server" PageSize="15" OnPageIndexChanging="gvPRODetailsList_PageIndexChanging"  AutoGenerateColumns="false" OnRowEditing="gvPRODetailsList_RowEditing"  OnRowUpdating="gvPRODetailsList_RowUpdating" OnRowCancelingEdit="gvPRODetailsList_RowCancelingEdit" >
                    <Columns>
                        <asp:TemplateField HeaderText="Line No."  HeaderStyle-Width="100" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:literal id="ltPROLineNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PROLineNumber").ToString() %>' />
                                <asp:Literal ID="ltProductionOrderDetailsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ProductionOrderDetailsID").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Literal ID="ltProductionOrderDetailsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ProductionOrderDetailsID").ToString() %>' />
                                <asp:Literal ID="ltPROLineNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PROLineNumber").ToString() %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BOM Ref. #" HeaderStyle-Width="150" ItemStyle-Width="100" >
                            <ItemTemplate>
                                <asp:literal id="ltBOMHeader" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BOMRefNumber").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvBOMHeader" runat="server" ValidationGroup="UpdateGridItems" ControlToValidate="atcBOMHeader" Display="Dynamic" ErrorMessage=" * " />
                                <asp:TextBox ID="atcBOMHeader" runat="server" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "BOMRefNumber").ToString() %>' />
                                <asp:HiddenField ID="hifBOMHeader" ClientIDMode="Static" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "BOMHeaderID").ToString() %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="UoM/Qty" HeaderStyle-Width="150" ItemStyle-Width="100" >
                            <ItemTemplate>
                                <asp:literal id="ltBoMUom_Qty" runat="server" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "UoM").ToString(),DataBinder.Eval(Container.DataItem, "UoMQty").ToString())  %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvBoMUom_Qty" runat="server" ValidationGroup="UpdateGridItems" ControlToValidate="atcBoMUom_Qty" Display="Dynamic" ErrorMessage=" * " />
                                <asp:TextBox ID="atcBoMUom_Qty" runat="server" Width="100" ClientIDMode="Static" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "UoM").ToString(),DataBinder.Eval(Container.DataItem, "UoMQty").ToString())  %>'  />
                                <asp:HiddenField ID="hifBoMUom_Qty" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "MaterialMaster_UoMID").ToString() %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText=" Quantity" ItemStyle-Width="100" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:literal id="ltPROQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PROQuantity").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvPROQuantity" runat="server" ValidationGroup="UpdateGridItems" ControlToValidate="txtPROQuantity" Display="Dynamic" ErrorMessage=" * " />
                                <asp:TextBox ID="txtPROQuantity" Width="100" onKeyPress=" return checkNum(event)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PROQuantity").ToString() %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                                               
                        <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkisdeleted" runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>

                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkIsDeleted" runat="server" OnClientClick="return confirm('Are you sure want to delete?')" Text="Delete" Font-Underline="false" OnClick="lnkIsDeleted_Click" />
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:CommandField CausesValidation="true" ButtonType="Link" ControlStyle-Font-Underline="false" CancelText="Cancel" EditText = "<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton = true UpdateText = "Update" />
                    </Columns>
                </asp:GridView>
                <br /><br />
            </td>
        </tr>
                    </table>
                </div>
              </td>
          </tr>
                              
        
      

       
    </table>
                    </ContentTemplate>
    </asp:UpdatePanel>
                   

     <!-- Production Order Materials Deficiency List Dialog   Developed by Naresh 05/03/2014-->
 
    
         <div id="divProdDefcListContainer">  
         <div id="divProdDefcList" style="display:block;">  
            <asp:UpdatePanel ID="upnlProdDefcList" runat="server" ChildrenAsTriggers="true" UpdateMode="Always" >
                <ContentTemplate>
                    
                    <div style="padding-left:10px;padding-right:10px;" align="center">
                        <br />
                                                                        <asp:Panel ID= "pnlProdDefcList" runat="server" Width="500px"  HorizontalAlign="Center"  >
                                                                                                                                           
                                                                            <asp:GridView Width="100%"  ShowFooter="true"   ID="gvProdDefcList" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"  SkinID="gvLightSteelBlueNew" HorizontalAlign="Left"   OnPageIndexChanging="gvProdDefcList_PageIndexChanging" OnRowDataBound="gvProdDefcList_RowDataBound" >
                                                                                <Columns>
                                                                                   
                                                                                     <asp:TemplateField ItemStyle-Width="150" ItemStyle-HorizontalAlign="left" HeaderText="Part Number"  >
                                                                                        <ItemTemplate>
                                                                                            <asp:Literal runat="server" ID="ltMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>'/>
                                                                                            <br />
                                                                                            <asp:Literal ID="ltOEMPartNo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>' />
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

                                                                            <br />
                                                                            <br />
                                                                        </asp:Panel>
                    </div>
                </ContentTemplate>  
           </asp:UpdatePanel>
       </div>
    </div>


     <!-- Production Order Materials Deficiency List Dialog   -->

      <div id="divMaterialPendingBOD">  
         <div id="divMaterialPending" style="display:block;padding:35px;">  
            <asp:UpdatePanel ID="upnlPendingMaterial" runat="server" ChildrenAsTriggers="true" UpdateMode="Always"   >
                <ContentTemplate>
                    <br />
                    <br />

                    <div class="ui-dailog-body" style="height:412px;padding:10px;">

                    <table>
                        <tr>
                            <td>
                                <asp:Panel ID= "Panel1" runat="server" Width="700px"  HorizontalAlign="Center"  Height="400" ScrollBars="Auto" >
                                                                                                                                           
                                        <asp:GridView Width="100%"  ShowFooter="true"   ID="gvPendingMaterial" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False"  SkinID="gvLightGrayNew" HorizontalAlign="Left"    OnRowDataBound="gvPendingMaterial_RowDataBound" >
                                            <Columns>
                                                                                   
                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="Part Number"  >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>'/>
                                                        <br />
                                                        <asp:Literal ID="ltOEMPartNo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>' />
                                                        <asp:HiddenField runat="server" ID="hidMaterialMaster" Value='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="100" HeaderText="UoM/Qty."  >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltUoMQty" Text='<%# DataBinder.Eval(Container.DataItem, "UoM/Qty") %>'/>
                                                    </ItemTemplate>
                                                                            
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Job Qty."  >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltJobQuantity" Text='<%# DataBinder.Eval(Container.DataItem, "JobQuantity") %>'/>
                                                    </ItemTemplate>
                                                                            
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Released Qty."  >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltReceivedQty" Text='<%# DataBinder.Eval(Container.DataItem, "ReceivedQty") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Pending Qty."  >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltPendingQty" Text='<%# DataBinder.Eval(Container.DataItem, "PendingQty") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                                       
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Available Qty."  >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltAvailableQty" Text='<%# DataBinder.Eval(Container.DataItem, "AvailableQty") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<input type='checkbox' id='HeaderCheckBox' onclick='CheckCheckBox(this)' />  ">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectBox" CssClass="ChildCheckBox" onclick="CheckCheckBox(this);" runat="server" />
                                                        <asp:HiddenField ID="hidPossibleQty" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "PossibleQty") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                       
                                    </asp:Panel>
                            </td>
                        </tr>
                        </table>
                        </div>
                    <div class="ui-dailog-footer">
                        <div style="padding: 15px 13px 15px 5px;">

                            <asp:LinkButton runat="server" ID="lnkClosedailog" OnClientClick="Pen_Mat_CloseDialog();" CssClass="ui-btn ui-button-large" >Close<%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>

                            <%--<asp:Button ID="btUpdatePendingMaterial" CssClass="ui-btn ui-button-large" runat="server" Text="Release Material" OnClientClick="this.disabled = true; this.value = 'Submitting...';" UseSubmitBehavior="false" OnClick="lnkUpdatePendingMaterial_Click" />--%>
                            <asp:LinkButton runat="server" ID="lnkUpdatePendingMaterial" CssClass="ui-btn ui-button-large" OnClick="lnkUpdatePendingMaterial_Click" OnClientClick="showAsynchronus();">Release Material<%=MRLWMSC21Common.CommonLogic.btnfaSignOut %></asp:LinkButton>
                        </div>
                    </div>
                        
                                
                        
                                                                        

                </ContentTemplate>  
           </asp:UpdatePanel>
       </div>
    </div>


</asp:Content>
