<%@ Page Title="Goods IN:." Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master"  AutoEventWireup="true" CodeBehind="StockIn.aspx.cs" Inherits="MRLWMSC21.mInventory.StockIn" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="InvContent" runat="server">

    <style>

        fieldset {border: 1px solid var(--sideNav-bg) !important;
        }

        .flex input[type="text"], input[type="number"], textarea {
            width: 70%;
        }
     </style>
    <asp:ScriptManager runat="server" ID="ssss" SupportsPartialRendering="true" EnablePartialRendering="true"></asp:ScriptManager>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Inventory/Inventory_Style.css" media="screen" />
    <script type="text/javascript" src="Scripts/jquery.blockUI.js"></script>


     <script type="text/javascript">

         $(document).ready(function () {
             //$("span").css("chkremark", "none");
             $("#divPrintDlg").dialog({
                 autoOpen: false,
                 modal: true,
                 minHeight: 50,
                 minWidth: 200,
                 height: 'auto',
                 width: 'auto',
                 overflow: 'auto',
                 resizable: false,
                 draggable: false,
                 position: {
                     my: "center middle",
                     at: "center middle",
                     of: window
                 },
                 open: function (event, ui) { $(this).parent().appendTo("#divPrintDlgContainer"); }
             });
         });

         function closePrintDialog() {
             //Could cause an infinite loop because of "on close handling"
             $("#divPrintDlg").dialog('close');
         }
         function openPrintDialog() {
             $("#divPrintDlg").dialog("option", "title", "Print Item Details");
             $("#divPrintDlg").dialog('open');
         }





    </script>

    <script type="text/javascript">
        
        function CheckIsDamaged() {
            var isdamaged = document.getElementById("<%=this.chkIsDamaged.ClientID%>");
           // alert(isdamaged);
            var checked = isdamaged.checked;
            //alert(checked);
            if (checked) {
                document.getElementById("<%=this.chkHasDiscrepancy.ClientID%>").checked = true;
                    //alert('dddddd');
                }

            }



        $(document).ready(function () {
            
            

            $('.DateBoxCSS_small').datepicker({ dateFormat: 'dd/mm/yy' });

            
            


            var textfieldname = $("#<%= this.atcPOnumber.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcPOnumber.ClientID %>").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPONumber") %>',
                        data: "{ 'StoreRefNo': '" + document.getElementById("<%=this.txtin_Storefno.ClientID%>").value + "'}",
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

                     $("#<%=hifPONumber.ClientID %>").val(i.item.val);
                       },
                       minLength: 0
            });


            var textfieldname = $("#<%= this.txtin_Storefno.ClientID %>");
            DropdownFunction(textfieldname);
            $('#<%=this.txtin_Storefno.ClientID%>').autocomplete({

                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetStorerefNoForGoodsInTenant") %>',
                        data: "{ 'Prefix': '" + request.term + "','TenantID':'" + $("#<%=hifTenant.ClientID %>").val() + "'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response(data.d)
                           
                        }

                    });
                },

                minLength: 0
            });

            var textfieldname = $("#<%= this.txtin_poitemline.ClientID %>");
            DropdownFunction(textfieldname);
            $('#<%=this.txtin_poitemline.ClientID%>').autocomplete({

                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetPOLineNumbers") %>',
                        data: "{ 'StoreRefNo': '" + document.getElementById('<%=this.txtin_Storefno.ClientID%>').value + "','MCode':'"+document.getElementById('<%=this.txtin_MaterialCode.ClientID%>').value+"'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response(data.d)
                        }

                    });
                },

                minLength: 0
            });


            var textfieldname = $("#<%= this.txtContainerCode.ClientID %>");
            DropdownFunction(textfieldname);
            $('#<%=this.txtContainerCode.ClientID%>').autocomplete({

                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetContainers_StockIn") %>',
                        data: "{'Prefix': '" + request.term + "', 'LocationID':'" + $("#<%=hifputwayloc.ClientID %>").val() + "', 'StoreRefNum':'" + document.getElementById("<%=this.txtin_Storefno.ClientID%>").value + "'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //if (data.d == "" || data.d == "/") {
                            //    showStickyToast(false, 'No Containers');
                            //}
                            //else response(data.d)
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

                   $("#<%=hifContainercode.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });



            var textfieldname = $("#<%= this.txtPutaway.ClientID %>");
            DropdownFunction(textfieldname);
            $('#<%=this.txtPutaway.ClientID%>').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetLocations") %>',
                        data: "{'Prefix': '" + request.term + "','ProductCategory':'0','InboundID':'<%=MRLWMSC21Common.CommonLogic.QueryString("ibdno")%>'}",
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
                    $("#<%=hifputwayloc.ClientID %>").val(i.item.val);
                },

                minLength: 0
            });


            var textfieldname = $(".LocPutaway");
            DropdownFunction(textfieldname);
            $('.LocPutaway').autocomplete({               
                source: function (request, response) {                    
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetLocations") %>',
                             data: "{'Prefix': '" + request.term + "','ProductCategory':'0','InboundID':'<%=MRLWMSC21Common.CommonLogic.QueryString("ibdno")%>'}",
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
                         $("#hifLocationID").val(i.item.val);
                     },

                     minLength: 0
            });

//// container Code

            var textfieldname = $("#txtCartonCode");
            DropdownFunction(textfieldname);
            $("#txtCartonCode").autocomplete({                
                source: function (request, response) {
                    
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetContainers") %>',
                        data: "{'Prefix': '" + request.term + "'}",
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
                    $("#hifCartonID").val(i.item.val);
                },

                minLength: 0
            });

        });
    </script>

    <script type="text/javascript">

         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
         function EndRequestHandler(sender, args) {
             if (args.get_error() == undefined) {
                 fnMCodeAC();
             }
         }


         function fnMCodeAC() {
             $(document).ready(function () {

                 $('.DateBoxCSS_small').datepicker({ dateFormat: 'dd/mm/yy' });

                 var TextFieldName = $("#<%= this.txtTenant.ClientID %>");
                 DropdownFunction(TextFieldName);
                 $("#<%= this.txtTenant.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
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

                    $("#<%=hifTenant.ClientID %>").val(i.item.val);
                    $("#<%=txtin_Storefno.ClientID %>").val('');
                    },
                    minLength: 0
                 });

                 try {
                     var textfieldname = $("#<%= this.txtin_MaterialCode.ClientID %>");
                     DropdownFunction(textfieldname);
                     $('#<%=this.txtin_MaterialCode.ClientID%>').autocomplete({

                     source: function (request, response) {

                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/getMCodeforstorerefno") %>',
                                 data: "{ 'prefix': '" + request.term + "','StorerefNo':'" + document.getElementById("<%=txtin_Storefno.ClientID%>").value + "'}",
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

                         minLength: 0
                     }).data("autocomplete")._renderItem = function (ul, item) {
                         return $("<li></li>")
                             .data("item.autocomplete", item)
                             .append("<a>" + item.label + "" + item.description + "</a>")
                             .appendTo(ul)
                     };
                 }catch(err){
                 }


                 var textfieldname = $("#<%= this.atcPOnumber.ClientID %>");
                 DropdownFunction(textfieldname);
                 $("#<%= this.atcPOnumber.ClientID %>").autocomplete({
                     source: function (request, response) {
                         
                         
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPONumber") %>',
                        data: "{ 'StoreRefNo': '" + document.getElementById("<%=txtin_Storefno.ClientID%>").value + "'}",
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

                           $("#<%=hifPONumber.ClientID %>").val(i.item.val);
                },
                minLength: 0
                 });

                 var textfieldname = $("#<%= this.atcQCNonconformityLocation.ClientID %>");
                 DropdownFunction(textfieldname);
                 $("#<%= this.atcQCNonconformityLocation.ClientID %>").autocomplete({
                     source: function (request, response) {
                         
                         var nonConformity = document.getElementById('chkqcIsnonconformity').checked ? "1" : "0";
                         var asis = document.getElementById('<%=this.chkasis.ClientID%>').checked ? "1" : "0";

                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetNonConformityLocationsFor3PL") %>',
                             data: "{ 'Prefix': '" + request.term + "','IsNonConformity':'" + nonConformity + "','AsIs':'" + asis + "','InboundID':'<%=MRLWMSC21Common.CommonLogic.QueryString("ibdno").ToString()%>'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "") {
                                alert('No location found');
                                document.getElementById('<%= this.atcQCNonconformityLocation.ClientID %>').value="";
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

                         $("#<%=hifQCNonConformityLocation.ClientID %>").val(i.item.val);

                         //alert($("#<%=hifQCNonConformityLocation.ClientID %>").val());
                         //SetfocustoLink();
                },
                minLength: 0
                 });

                 var textfieldname = $("#<%= this.atcInvoiceNumber.ClientID %>");
                 DropdownFunction(textfieldname);
                 $("#<%= this.atcInvoiceNumber.ClientID %>").autocomplete({
                     source: function (request, response) {

                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetinvoiceListForGoodsIN") %>',
                             data: "{ 'StoreNo': '" + document.getElementById('<%=this.txtin_Storefno.ClientID%>').value + "','MCode':'" + document.getElementById('<%=this.txtin_MaterialCode.ClientID%>').value + "','LineNo':'" + document.getElementById('<%=this.txtin_poitemline.ClientID%>').value + "','POHeaderID':'"+document.getElementById('<%=this.hifPONumber.ClientID%>').value+"'}",
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

                         $("#<%=hifInvoiceNumber.ClientID %>").val(i.item.val);
                         $("#<%= this.atcInvoiceNumber.ClientID %>").val(i.item.label);
                         document.getElementById('<%=this.lnkin_GetDetails.ClientID%>').click();
                     },
                     minLength: 0
                 });

               });


            }
            fnMCodeAC();
  </script>

    <style type="text/css">
        .horizentalalign {
            width:90%;
            overflow-x: scroll 
        }
        input
        {
           border:1px solid #333333;
           font-family:Calibri,Verdana,Geneva,sans-serif;
           position:relative;
           color: #000000; 
           font-size:11pt;
        }
        .noclose .ui-dialog-titlebar-close
        {
            display:none;
        }
       
</style>
  
    <script>
            $(document).ready(function () {
                $("span.chkremark").hide();
            var flag;
            if('<%=this.ViewState["autoOpen"].ToString()%>'!='false')
                flag=true;
            else
                flag=false;
            $("#divQCParameters").dialog({
                //bgiframe: true,
                autoOpen: flag,
                modal: true,
                height: 650,
                width: 650,
                resizable: false,
                draggable: false,
                title:'Quality Check Parameter Capturing',
                 closeOnEscape: false,
                 //beforeclose: function (event, ui) { return false; },
                 dialogClass: "noclose",
                 open: function (event, ui) {
                     $(this).parent().appendTo("#disputeDivQCParameters");
                     $("#divQCParameters").hide().fadeIn(500);
                     $('body').css({ 'overflow': 'hidden' });
                     $('body').width($('body').width());
                     $(document).bind('scroll', function () {
                         window.scrollTo(0, 0);
                     });
                 },

                  
                position: ["center top", 40],
            close: function () {

                $("#divQCParameters").fadeOut(500);
                $(document).unbind('scroll');
                $('body').css({ 'overflow': 'visible' });
                    
            },

            



             });
         });

         function closeQCParametersDialog() {
             //Could cause an infinite loop because of "on close handling"
             $("#divQCParameters").dialog('close');
         }

         function openQCParametersDialog() {
             // alert("from");
             // BuildDialogBox(GoodsMovementID)
             $("#divQCParameters").dialog("option", "title", "Quality Check Parameter Capturing");
             $("#divQCParameters").dialog('open');
             //return false;

             NProgress.start();

             blockQCParametersDialog();
         }

         function blockQCParametersDialog() {

           
             $("#divQCParameters").block({
                 message: '<img src="<%=ResolveUrl("~") %>Images/async_inv.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });

        }
         function unblockQCParametersDialog() {
             $("#divQCParameters").unblock();

             NProgress.done();

         }


    </script>

    <script>
        

        function checknonConfirm(Textbox)
        {
           
            var IsNonConfirmed = false;
            if (Textbox.value != '') {
                $.ajax({
                    url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTolarenceforQc") %>',
                    data: "{ 'MaterialMasterID':'<%=MRLWMSC21Common.CommonLogic.QueryString("mmid").ToString()%>'}",
                     dataType: "json",
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     success: function (data) {
                        
                         var TotalData = data.d.toString();
                         var parameterList = TotalData.split(',');
                        
                         for (var position = 0; position < parameterList.length; position++) {
                             var ParameterDetails = parameterList[position];
                             var parameterDetailsList = ParameterDetails.split('|');
                             
                             if (IdentifyTolerent(parameterDetailsList[0], parameterDetailsList[1], parameterDetailsList[2])) {
                                 IsNonConfirmed = true;
                             }
                         }
                         debugger;
                         document.getElementById('chkqcIsnonconformity').checked = IsNonConfirmed;
                         var chkAsIs = document.getElementById('<%=this.chkasis.ClientID%>');
                         var hifOldLocation = document.getElementById('<%=this.hifCheckLocation.ClientID%>');
                         if (IsNonConfirmed && hifOldLocation.value != "Q1" && !chkAsIs.checked) {
                             document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled = "";
                             document.getElementById('tbAsIsDetails').style.display = "block";
                             
                         }
                         else {
                             if (hifOldLocation.value != "Q1") {
                                 document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled = "disabled";
                                 document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').value = "";
                             }
                             else {
                                 document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled = "";
                                 document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').value = "";
                                 document.getElementById('spnNCLocation').innerText = "Normal Location:";
                             }
                             
                             if (document.getElementById('<%=this.hifIsDamaged.ClientID%>').value == "No") {
                                 document.getElementById('<%=this.chkasis.ClientID%>').checked = false;
                                 document.getElementById('<%=this.txtGMDQCRemarks.ClientID%>').value = "";
                                 document.getElementById('tbAsIsDetails').style.display = "none";
                             }
                         }


                         if (document.getElementById('chkqcIsnonconformity').checked) {
                             document.getElementById('chkqcIsnonconformity').disabled = "disabled";
                         }
                     }

                 });
            }
        }
        function IdentifyTolerent(minvalue, maxvalue, FieldName) {
            var minTolerent = parseFloat(minvalue);
            var maxTolerent = parseFloat(maxvalue);
            var txtMinValue;
            var txtMaxValue;
            var MinValue;
            var MaxValue;
            var Textbox = document.getElementById('txtqc' + FieldName);
            
            if (document.getElementById('chk' + FieldName).checked) {
                txtMinValue = document.getElementById('txtMin' + FieldName);
                
                txtMaxValue = document.getElementById('txtMax' + FieldName);
                
                if (txtMinValue.value != "" && txtMinValue.value != "MinValue" && txtMaxValue.value != "" && txtMaxValue.value != "MaxValue") {
                    MinValue = parseFloat(txtMinValue.value);
                    MaxValue = parseFloat(txtMaxValue.value);
                    
                    if (minTolerent > MinValue || maxTolerent < MinValue || minTolerent > MaxValue || maxTolerent < MaxValue) {
                        return true;
                    }
                }
                else {
                    return false;
                }
            }
            else {
                if (Textbox.value == "") {
                    return false;
                }
                var value = parseFloat(Textbox.value);
                if (minTolerent > value || maxTolerent < value)
                    return true;
            }
                       
            return false;
        }

        function CheckLocation(Button) {
            
           
            if (!document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled && (document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').value == "" || document.getElementById('<%=this.hifQCNonConformityLocation.ClientID%>').value == "")) {
                showStickyToast(false, "Select location for item");
                return false;
            }
            showAsynchronus();
            
            Button.attr("onclick", "return false");
            
            return true;
        }

        function CheckNonconformity(checkBox) {
            if (checkBox.checked) {

                if (!document.getElementById('<%=this.chkasis.ClientID%>').checked) {
                    document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled = "";
                  
                 
                }
                else {
                    document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled = "disabled";
                    //$("#chkremark").show();
                    
                    $("#chkremark").toggle();
                }
                document.getElementById('tbAsIsDetails').style.display = "block";
                document.getElementById('spnNCLocation').innerText = "Non-Conformity Location:";
            }
            else {
                if (document.getElementById('<%=this.hifCheckLocation.ClientID%>').value != 'Q1') {
                    document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled = "disabled";
                    document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').value = "";
                    $("#chkremark").toggle();
                }
                else {
                    document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled = "";
                    document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').value = "";
                    
                }

                document.getElementById('spnNCLocation').innerText = "Normal Location:";
                //alert(document.getElementById('<%=this.hifIsDamaged.ClientID%>').value);
                if (document.getElementById('<%=this.hifIsDamaged.ClientID%>').value == "No") {
                    document.getElementById('<%=this.chkasis.ClientID%>').checked = false;
                    document.getElementById('<%=this.txtGMDQCRemarks.ClientID%>').value = "";
                    document.getElementById('tbAsIsDetails').style.display = "none";
                }
            }
        }

        function SetfocustoLink() {
            showAsynchronus();
            var btnID = '<%=lnkin_GetDetails.ClientID %>';
            var params = 'CaptureQC';
            __doPostBack(btnID, params);
            //document.getElementById('<%=this.lnkin_GetDetails.ClientID%>').click();
        }

        function EnableQCQuantity(checkBox)
        {
            if (checkBox.checked) {
                document.getElementById('<%=this.txtQcQuantity.ClientID%>').disabled = "";
            }
            else {
                document.getElementById('<%=this.txtQcQuantity.ClientID%>').value = "";
                document.getElementById('<%=this.txtQcQuantity.ClientID%>').disabled = "disabled";
            }
        }

        function CheckMandataryFields() {
           //var  document.getElementsByClassName('QCParentdiv');
           // var MandataryFieldCount=
        }

        function chkAsis(CheckBox)
        {
            
            var asis = document.getElementById('<%=this.chkasis.ClientID%>').checked ? "1" : "0";
            if (asis == 1) {
              //  $("span.chkremark").show();
               // $("#chkremark").show();
            }
            if (CheckBox.checked && document.getElementById('<%=this.hifCheckLocation.ClientID%>').value == 'Q1') {
                document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled = "";
                document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').value = "";
                //  $("#chkremark").css("display", "block");
               
                
                document.getElementById('spnNCLocation').innerText = "Normal Location:";
            }
            else if (!CheckBox.checked && document.getElementById('chkqcIsnonconformity').checked) {
                document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled = "";
                document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').value = "";
                document.getElementById('spnNCLocation').innerText = "Non-Conformity Location:";
                $('#chkremark').toggle();
            }
            else {
                alert("hi");
                $('#chkremark').toggle();
                document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled = "disabled";
                document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').value = "";
                document.getElementById('spnNCLocation').innerText = "Non-Conformity Location:";
               
            }
           
        }

    </script>

    <script>
              
        function CheckReceiveQty() {
            

            //if (document.getElementById('<=this.hifMeasurementType.ClientID%>').value == "0") {

            //    var textBox = document.getElementById('<=this.txtreceiveqty.ClientID%>');

            //    var ReceivedQty = parseFloat(document.getElementById('<=this.hifConversion.ClientID%>').value) * parseFloat(textBox.value) * 100;

            //    MUoMConversionvalue = parseFloat(document.getElementById('<=this.hifconversionInMUoM.ClientID%>').value) * 100;
                

            //    if (ReceivedQty < MUoMConversionvalue) {
            //        showStickyToast(true, "'Quantity Received' should be greater than or equal to MUoM Qty.");
            //        textBox.value = "";
            //        return;
            //    }
            //    if ((ReceivedQty % 100) != 0) {
            //        showStickyToast(true, "'Quantity Received' should be multiple of BUoMQty.");
            //        textBox.value = "";
            //        return;
            //    }
            //}
            //else {


                var textBox = document.getElementById('<%=this.txtreceiveqty.ClientID%>');

                //var ConversionvalueInMUoM = parseFloat(document.getElementById('<=this.hifconversionInMUoM.ClientID%>').value);
                //var Quantity = parseFloat(textBox.value);
                //var ReceivedQtyInMUoM = parseInt(ConversionvalueInMUoM * Quantity * 1000);
                ////alert(ConversionvalueInMUoM);
                /////MUoMConversionvalue = parseFloat(document.getElementById('<=this.hifconversionInMUoM.ClientID%>').value) * 100;
                //var ModuloValue = ReceivedQtyInMUoM % 1000;
                ////alert((Math.abs(100 - ModuloValue)) );
                
                //if (ModuloValue!=0 && (Math.abs(1000 - ModuloValue)) > 100) {
                //    showStickyToast(true, "'Quantity Received' should be multiple of MUoM Qty. <br/> Suggested Quantity is " + parseInt(Math.ceil(ReceivedQtyInMUoM / 1000) / ConversionvalueInMUoM * 1000) / 1000);
                //    textBox.value = "";
                //    return;
                //}


                //if ((ReceivedQty % 100) != 0) {
                //    showStickyToast(true, "'Quantity Received' should be multiple of BUoMQty.");
                //    textBox.value = "";
                //    return;
                //}



            //}
            
            CheckDecimal(textBox);
        }

        function ClearText(TextBox,checkValue)
        {
            if (TextBox.value == checkValue) {
                TextBox.value = "";
            }
            
        }

        function CheckClearText(TextBox,checkValue,ParentDiv)
        {
            //alert('dfgdfgdf');
            //alert(TextBox.value.length);
            if (TextBox.value.length == 0) {
                TextBox.value = checkValue;
            }
            else {
               
                checknonConfirm(TextBox);
            }
        }

        function CheckDecimalTextBoxes(CheckBox,ParentDiv)
        {           
            if (CheckBox.checked) {
                document.getElementById(ParentDiv).getElementsByTagName('div')[0].style.display = 'block';
                document.getElementById(ParentDiv).getElementsByTagName('input')[3].style.display = "none";
                try
                {
                    document.getElementById(ParentDiv).getElementsByTagName('span')[1].controltovalidate="";
                } catch (err) {
                }
            }
            else {
                document.getElementById(ParentDiv).getElementsByTagName('div')[0].style.display = 'none';
                document.getElementById(ParentDiv).getElementsByTagName('input')[3].style.display = "block";
                try{
                    document.getElementById(ParentDiv).getElementsByTagName('span')[1].controltovalidate = "";
                } catch (err) {
                }
            }
        }

        function ShowDefaultTextBoxes() {
            
            $.ajax({
                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTolarenceforQc") %>',
                data: "{ 'MaterialMasterID':'<%=MRLWMSC21Common.CommonLogic.QueryString("mmid")%>'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    // alert(data.d);
                    var TotalData = data.d.toString();
                    var parameterList = TotalData.split(',');

                    var ParentDiv;
                    try { 
                        for (index = 0; index < parameterList.length; index++) {

                            ParentDiv = document.getElementById('div' + parameterList[index].split('|')[2]);
                            if (document.getElementById('chk' + parameterList[index].split('|')[2]).checked) {
                                ParentDiv.getElementsByTagName('input')[3].style.display = "none";

                            }
                            else {
                                ParentDiv.getElementsByTagName('div')[0].style.display = "none";
                            }
                        }
                    } catch (err) { }

                    //if ($("#divQCParameters").dialog("isOpen") === false)
                    //    openQCParametersDialog();
                }

            });

            if (document.getElementById('<%=this.hifIsDamaged.ClientID%>').value == "Yes" || document.getElementById('chkqcIsnonconformity').checked) {
                document.getElementById('tbAsIsDetails').style.display = "block";
            }
            else {
                document.getElementById('tbAsIsDetails').style.display = "none";
                document.getElementById('<%=this.chkasis.ClientID%>').checked = false;
                document.getElementById('<%=this.txtGMDQCRemarks.ClientID%>').value = "";
            }
            if (document.getElementById('chkqcIsnonconformity').checked && !document.getElementById('<%=this.chkasis.ClientID%>').checked) {
                document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled = "";
            }
            else {
                document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').disabled = "disabled";
                document.getElementById('<%=this.atcQCNonconformityLocation.ClientID%>').value = "";
            }

        }

        function SelectAllQcValues(HeaderCheckBox) {
            var CheckBoxList = document.getElementsByClassName('DeleteQCList');
            for (index = 0; index < CheckBoxList.length; index++) {
                CheckBoxList[index].firstChild.checked = HeaderCheckBox.checked;
            }
           
        }

        function RadioCheck(rb) {

            var gv = document.getElementById("<%=this.gvQCUpdatedList.ClientID%>");

             var rbs = gv.getElementsByTagName("input");

             var row = rb.parentNode.parentNode.parentNode;
             //alert(row);
             var coumn = row.getElementsByTagName('td');
             //alert(coumn.length);
             var text = coumn[0].innerHTML.toString();
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
    
     <script type="text/javascript" language="javascript">

         $(document).ready(function () {
             $('#<%=chkHasDiscrepancy.ClientID%>').change(function () {
                 var IsQuarantine = 0;
                 var Damchecked = document.getElementById("<%=this.chkIsDamaged.ClientID%>").checked;
                 var Dischecked = document.getElementById("<%=this.chkHasDiscrepancy.ClientID%>").checked;

                

                 if (Damchecked || Dischecked) 
                     $("#<%=hifIsQuarantine.ClientID %>").val('1');

                 else if (!Dischecked && !Damchecked) 
                     $("#<%=hifIsQuarantine.ClientID %>").val('0');

             });

             $('#<%=chkIsDamaged.ClientID%>').change(function () {
                 var IsQuarantine = 0;
                 var Damchecked = document.getElementById("<%=this.chkIsDamaged.ClientID%>").checked;
                 var Dischecked = document.getElementById("<%=this.chkHasDiscrepancy.ClientID%>").checked;

           
                  if (Damchecked || Dischecked) 
                      $("#<%=hifIsQuarantine.ClientID %>").val('1');
                 
                  else if (!Dischecked && !Damchecked) 
                      $("#<%=hifIsQuarantine.ClientID %>").val('0');
                   
              });
         });

    </script>

         <script type="text/javascript">
           
                $(document).ready(function () {
                    $("#<%= this.txtMfgDate.ClientID%>").datepicker({ dateFormat: 'dd/mm/yy' });
                    $("#<%= this.txtExpDate.ClientID%>").datepicker({ dateFormat: 'dd/mm/yy' });
                });
            

    </script>
    <div class="dashed"></div>
    <div class="pagewidth">
    <div id="disputeDivQCParameters">
    <div id="divQCParameters"  >
        <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlQCParameters" runat="server" UpdateMode="Always">
                <ContentTemplate>
        
                    <div class="ui-dailog-body" style="height:545px;padding-left:10px;padding-right:10px;">

                    <table width="100%">
         
        <tr><td ><asp:Label ID="lblDailogStatus" runat="server"  CssClass="errorMsg"/> </td>   </tr>
           
        <tr>
            <td style="background-color:#E0F8F7">
                <asp:HiddenField ID="hifGoodsMovementID" runat="server" />
                <asp:HiddenField ID="hifIsDamaged" runat="server" />
                <asp:HiddenField ID="hifCheckLocation" runat="server" />
                <table runat="server" id="tbGoodsMovementDetails" width="45%">

                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" >
                    <tr id="tbAsIsDetails">
                        <td valign="top" width="50%">
                            <asp:CheckBox ID="chkasis" onClick="chkAsis(this);" runat="server" Text="As Is" />
                        </td>
                        <td>
                            <asp:CustomValidator ID="rfvAsIsDetails" runat="server" OnServerValidate="CValidator_ServerValidate" Display="Dynamic" Text="*" ForeColor="Red"></asp:CustomValidator>
                                
                            <span id="chkremark" style="display:none;color:red">*</span>Remarks for As Is:<br />
                            <asp:TextBox ID="txtGMDQCRemarks" runat="server" TextMode="MultiLine" Width="200" Height="60"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlPosition" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <table id="tbQcParameters"  runat="server" border="0" width="100%">

                                <tr >
                                    <td align="center" colspan="2">
                                          
                                                    <asp:Panel ID="pnlQCUpdateList" runat="server" Height="200" Width="60%" ScrollBars="Vertical">
                                                            <asp:GridView ID="gvQCUpdatedList" runat="server" AutoGenerateColumns="false" SkinID="gvLightGreenNew">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate >
                                                                            <asp:RadioButton onclick="RadioCheck(this);" ID="chkSelectRow" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="QC Serial No.">
                                                                           
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="ltSerialNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GoodsMovementDetailsID").ToString()!=""? String.Format("{0}-{1}",DataBinder.Eval(Container.DataItem, "GoodsMovementDetailsID").ToString(),DataBinder.Eval(Container.DataItem, "QCSerialNo").ToString()):"Pending Quantity"  %>'/>
                                                                            <asp:HiddenField ID="hifGoodsMovementDetailsID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "GoodsMovementDetailsID").ToString() %>' />
                                                                            <asp:HiddenField ID="hifSerialNumber" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "QCSerialNo").ToString() %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="QC Quantity">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="ltQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Quantity").ToString() %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            Select All
                                                                            <asp:CheckBox ClientIDMode="Static" ID="chkHeader" runat="server" onclick="SelectAllQcValues(this)"  />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" CssClass="DeleteQCList" Visible='<%# DataBinder.Eval(Container.DataItem, "GoodsMovementDetailsID").ToString()!=""?true:false %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:LinkButton ID="lnkDelete"  runat="server" ForeColor="Blue" OnClientClick="return confirm('Are you sure want to delete the selected items?')" Text="Delete" OnClick="lnkDelete_Click"  Font-Underline="false" ></asp:LinkButton>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                       
                                                                </Columns>
                                                            </asp:GridView>
                                                    </asp:Panel>
                                        
                                               
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                           
                                        <asp:Label ID="lbCompletedQty" class="QCSerialNo" runat="server" Text="QC Completed Qty.:"></asp:Label>&nbsp;&nbsp;
                                        <asp:Literal ID="ltCompletedQty" runat="server" ></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbTotalQty" class="QCSerialNo" runat="server" Text="QC Total Qty.:" ></asp:Label>&nbsp;&nbsp;
                                        <asp:Literal ID="ltTotalQty" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><br /><br />
                                        <Label class="QCSerialNo">QC Ref. No.</Label>&nbsp;:&nbsp;&nbsp;
                                        <asp:Literal ID="ltQCSerialNo" runat="server" />
                                        <asp:HiddenField ID="hifSelectedQty" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td><br />
                                        Bulk QC Quantity:
                                        <asp:TextBox ID="txtQcQuantity" runat="server" Width="60" onKeypress="return checkDec(this,event)" onblur="CheckDecimal(this)" Enabled="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                          
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="left" id="tdNCLocation">
                <span id="spnNCLocation">Non-Conformity Location:</span>
                <br />
                <asp:TextBox ID="atcQCNonconformityLocation" SkinID="txt_Auto" Width="120" Enabled="false" runat="server" />
                <asp:HiddenField ID="hifQCNonConformityLocation" runat="server" />
            </td>
        </tr>
           <tr>
               <td>
                   &nbsp;
               </td>
           </tr>
    </table>

                        </div>
                    <br /><br />


                    <div class="ui-dailog-footer" >
                        <div style="padding: 15px 13px 15px 5px;">

                             
                            <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click" OnClientClick="closeQCParametersDialog();" CssClass="btn btn-primary">
                            Close <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                            </asp:LinkButton>

                <asp:LinkButton ID="lnkUpdateQcValues" OnClientClick="return CheckLocation(this);" OnClick="lnkUpdateQcValues_Click" runat="server"  CssClass="btn btn-primary">
                Capture QC Parameters <%=MRLWMSC21Common.CommonLogic.btnfaSave %>
                </asp:LinkButton>
                            
                            </div>
                        </div>



                    </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    
    <table border="0" cellpadding="4" cellspacing="4" align="center" class="pagewidth">
       
        <tr height="15px">
            <td colspan="3">
                &nbsp;<br />
            </td>
        </tr>

<%--        <tr height="15px">
                     <td colspan="3" align="Left" >
                         Note:
                         <asp:Label ID="Label2" runat="server" CssClass="errorMsg" Text=" * " />
                         Indicates mandatory fields 
                     </td>
                    
        </tr>--%>


         <tr>
            <td colspan="3">


                  <fieldset style="border:1px solid #808080;border-radius:5px;width:800px; margin:auto;" align="left">
                    <LEGEND><b>Inbound Details</b></LEGEND>
              

                <asp:Panel ID="pnggoodsin" runat="server" >
                        
                         
                             <table border="0"  runat="server" id="tbgoodsin" cellpadding="4" cellspacing="4" align="center"  width="790px">
                                   
                                    <tr>
                                        <td>
                                            <div class="flex">
<%--                                            <label><asp:RequiredFieldValidator ID="rfvTenant" runat="server" ValidationGroup="getDetails" ControlToValidate="txtTenant" Display="Dynamic"/>
                                            <span style="color:red;margin-left:-0.3em">*</span><asp:Literal ID="ltTenant" runat="server" Text="Tenant:&lt;br /&gt;" /></label>--%>
                                            <div><asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Auto"  required=""></asp:TextBox>
                                            <label><asp:RequiredFieldValidator ID="rfvTenant" runat="server" ValidationGroup="getDetails" ControlToValidate="txtTenant" Display="Dynamic"/>
                                           <asp:Literal ID="ltTenant" runat="server" Text="Tenant:&lt;br /&gt;" /></label> <span class="errorMsg">*</span>
                                            <asp:HiddenField runat="server" ID="hifTenant"/></div></div>
                                        </td>

                                         <td colspan="2" align="left">
                                             <div class="flex">
<%--                                             <label><asp:RequiredFieldValidator ID="rfvstorefno" runat="server" ValidationGroup="getDetails" ControlToValidate="txtin_Storefno" Display="Dynamic"/>
                                             <span style="color:red;margin-left:-0.3em">*</span><asp:Literal ID="ltin_storefno" runat="server" Text="Store Ref. No." /></label>--%>
                                             <div><asp:TextBox ID="txtin_Storefno" onKeypress="return checkSpecialChar(event)" SkinID="txt_Auto" runat="server"  required=""/>
                                             <label><asp:RequiredFieldValidator ID="rfvstorefno" runat="server" ValidationGroup="getDetails" ControlToValidate="txtin_Storefno" Display="Dynamic"/>
                                             <asp:Literal ID="ltin_storefno" runat="server" Text="Store Ref. No." /></label><span class="errorMsg">*</span>
                                             </div></div>
                                         </td>
                                        
                        
                                     </tr>

                                    <tr>
                                        <td align="left">
                                            <div class="flex">
<%--                                            <label><asp:RequiredFieldValidator ID="rfvMaterialCode" runat="server" ValidationGroup="getDetails" ControlToValidate="txtin_MaterialCode" Display="Dynamic"/>
                                            <span style="color:red;margin-left:-0.3em">*</span><asp:Literal ID="ltin_MaterialCode" runat="server" Text="Part Number" /></label>--%>
                                            <div><asp:TextBox ID="txtin_MaterialCode" runat="server" onKeypress="return checkSpecialChar(event)" EnableTheming="false" CssClass="txt_small_Auto"  required=""/>
                                                                                            <label><asp:RequiredFieldValidator ID="rfvMaterialCode" runat="server" ValidationGroup="getDetails" ControlToValidate="txtin_MaterialCode" Display="Dynamic"/>
                                            <asp:Literal ID="ltin_MaterialCode" runat="server" Text="Part Number" /></label><span class="errorMsg">*</span>
                                            </div></div>
                                        </td>
                                        <td align="left">
                                            <div class="flex">
                                                <%--<label>
                                                <asp:RequiredFieldValidator ID="rfvpoitemline" runat="server" ValidationGroup="getDetails" ControlToValidate="txtin_poitemline" Display="Dynamic"/>
                                                <span style="color:red;margin-left:-0.3em">*</span><asp:Literal ID="ltin_poitemline" runat="server" Text=" PO Line Item # " /></label>--%>
                                                <div><asp:TextBox ID="txtin_poitemline" SkinID="txt_Auto" onKeyPress="return checkNum(event)" runat="server" required=""/>
                                                    <label>
                                                <asp:RequiredFieldValidator ID="rfvpoitemline" runat="server" ValidationGroup="getDetails" ControlToValidate="txtin_poitemline" Display="Dynamic"/>
                                                <asp:Literal ID="ltin_poitemline" runat="server" Text=" PO Line Item # " /></label><span class="errorMsg">*</span>
                                                </div>
                                            </div>
                                        </td>
                                        </tr>
                                 <tr>
                                        <td align="center" colspan="2">
                                            <div class="flex__ flrx__excess">
                                            <asp:LinkButton ID="lnkin_GetDetails" runat="server" ValidationGroup="getDetails"  OnClick="lnkin_GetDetails_Click" CssClass="btn btn-primary">
                                             Get Details <%=MRLWMSC21Common.CommonLogic.btnfaFilter %>
                                            </asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkin_cancel" runat="server" OnClick="lnkin_cancel_Click" CssClass="btn btn-primary">
                                            Clear <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                            </asp:LinkButton>
                                            </div>
                                        </td>
                                 </tr>
                    
                            </table>

                        
                
                        

                </asp:Panel>
                      </fieldset>
           
            </td>
        </tr>

      
        
         <tr>
            <td colspan="3">

                  <asp:Panel runat="server" ID="pnlPOQuantityList" Visible="false"  >

                 <fieldset style="border:1px solid #808080;border-radius:5px;width:798px; margin:auto;" align="left">
                    <LEGEND><b>Item Details</b></LEGEND>
                
              
                                      
                                        <table style="width:100%"><tr>
                                     <td align="left">

                                     
                                     <asp:Literal ID="ltPONumber" runat="server" Visible="false" Text="PO Number:<br/>" />
                                     <asp:TextBox ID="atcPOnumber"  Visible="false" SkinID="txt_Auto" runat="server" Width="160" />
                                     <asp:Literal ID="ltInvoiceNumber" runat="server" Visible="false" Text="<br />Invoice Number:<br/>" />
                                     <asp:TextBox ID="atcInvoiceNumber" Visible="false" SkinID="txt_Auto" runat="server" Width="160"/>
                                     <asp:HiddenField ID="hifPONumber" runat="server"  Value="0"/>
                                     <asp:HiddenField ID="hifInvoiceNumber" runat="server" Value="0" />
                                         </td>
                                        </tr>
                                 </table>

                                      <table border="0" cellpadding="3" runat="server" id="tbInBound_FormDetails" cellspacing="3" align="center" style="width:100%">
                                          <tr id="trSerialText" runat="server" visible="false">
                                              <td colspan="3">
                                                  <span class="errorMsg" >Material with 'Serial No.' capture can only be received in BUoM  </span>
                                              </td>
                                          </tr>
                                          <tr>
                                        <td colspan="1" align="left" width="35%">
                                            <asp:Label ID="ltDescription" runat="server" Text="Description:" CssClass="FormLabelsBlue"  /><br />
                                            <asp:Literal ID="ltDescriptionvalue" runat="server" />
                                            
                                        </td>
                                         <td align="left" width="25%">
                                             <asp:Label ID="ltInvUoMQty" runat="server" Text="Inv. UoM/Qty.:" CssClass="FormLabelsBlue" /><br />
                                             <asp:Literal ID="ltInvUoMQtyValue" runat="server" />
                                             
                                        </td>
                                         <td align="left">
                                            <asp:Label ID="ltInvqty" runat="server" Text="Inv. Qty.:" CssClass="FormLabelsBlue" /><br />
                                            <asp:Literal ID="ltInvqtyvalue" runat="server" />
                                             <asp:HiddenField ID="hifInvUoMQty" runat="server" />
                                        </td>
                                    </tr>
                                  
                                          <tr>
                                        <td align="left" width="35%">
                                             <asp:Label ID="ltBaseUoMQty" runat="server" Text="BUoM/Qty.:" CssClass="FormLabelsBlue" /><br />
                                             <asp:Literal ID="ltBaseUoMQtyValue" runat="server" />
                                            <asp:HiddenField ID="hifConversion" runat="server" />
                                             <asp:HiddenField ID="hifBUoMQty" runat="server" />
                                            <asp:HiddenField ID="hifMeasurementType" runat="server" />

                                        </td>
                                               <td align="left" width="30%">
                                             <asp:Label ID="lbMUoMQty" runat="server" Text="MUoM/Qty.:" CssClass="FormLabelsBlue" /><br />
                                             <asp:Literal ID="ltMUoMQty" runat="server" />
                                            

                                        </td>
                                        <td  align="left">
                                             <asp:Label ID="ltPOUoMQty" runat="server" Text="PO UoM/Qty.:" CssClass="FormLabelsBlue" /><br />
                                             <asp:Literal ID="ltPOUoMQtyValue" runat="server" />
                                            <asp:HiddenField ID="hifUoMQty" runat="server" />
                                        </td>
                                       
                                    </tr>
                                  
                                          <tr> 
                                        <td colspan="1" align="left" class="FormLabels" width="35%">
                                            <asp:Label ID="ltreceiveqtyperuom" runat="server" Text="Conversion Factor to BUoM:" CssClass="FormLabelsBlue" /><br />
                                            <asp:Literal ID="ltreceiveqtyperuomValue" runat="server"  />
                                            <%--<asp:HiddenField ID="hifconversionInMUoM" runat="server" />--%>
                                        </td>
                                        <td colspan="1" align="left" class="FormLabels" width="30%">
                                            <asp:Label ID="ltTotalQuantity" runat="server" Text="Total Receiving Qty. in BUoM:" CssClass="FormLabelsBlue" /><br />
                                            <asp:Literal ID="ltTotalQuantityvalue" runat="server"  />
                                        </td>
                                        <td colspan="1" align="left" class="FormLabels" width="30%">
                                            <asp:Label ID="ltkitPlanner" CssClass="FormLabelsBlue"  runat="server" Text="KitID:" /><br />
                                            <asp:Literal ID="ltKitplannerValue" runat="server" Text="KitID" />
                                        </td>
                                        
                                    </tr>
                                  
                                          <tr>  
                                        <td colspan="1" align="left" width="35%">
                                             <span id="Span1" runat="server" style="color:red">*</span>
                                            <%--<asp:RequiredFieldValidator ID="rfvreceiveqty" runat="server" ValidationGroup="receiveQuantity" ControlToValidate="txtreceiveqty" Display="Dynamic" ErrorMessage=" * " />--%>
                                            <asp:Literal ID="ltreceiveqty" runat="server" Text="Quantity Received:" /><br />
                                            <asp:TextBox ID="txtreceiveqty" onKeyPress="return checkDec(this,event)" onblur="CheckReceiveQty()" runat="server" Width="160" />
                                        </td>
                                                        <td  align="left" colspan="2" class="FormLabels" width="30%">
                                    
                                            <asp:CheckBox ID="chkIsDamaged" runat="server" onclick="CheckIsDamaged()" Text="Is Damaged" />&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="chkHasDiscrepancy" runat="server" Text="Has Discrepancy" />&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="chkPositiveRecall" runat="server" Text="Is Positive Recall" />
                                            <asp:HiddenField runat="server" ID="hifIsQuarantine" Value="0" />
                                        </td>
                                    
                                    </tr>
                                          <tr>
                                              <td>
                                                  <span id="SpMfgDate" runat="server" style="color:red">*</span>
                                                  <asp:Literal ID="ltMfgDate" runat="server" Text="Mfg. Date:" ></asp:Literal><br />
                                                  <asp:TextBox ID="txtMfgDate" runat="server" ></asp:TextBox>
                                              </td>
                                             
                                              <td>
                                                  <span id="SpExpdate" runat="server" style="color:red">*</span>
                                                  <asp:Literal ID="ltExpDate" runat="server" Text="Exp. Date:" ></asp:Literal><br />
                                                  <asp:TextBox ID="txtExpDate" runat="server"></asp:TextBox>
                                              </td>
                                              <td>
                                                  <span id="SpSerialNo" runat="server" style="color:red">*</span>
                                                  <asp:Literal ID="ltSerialNo" runat="server" Text="Serial No. :"></asp:Literal><br />
                                                  <asp:TextBox ID="txtSerialNo" runat="server"></asp:TextBox>
                                              </td>
                                          </tr>

                                          <tr>
                                              <td>
                                                  <span id="SpBatchNo" runat="server" style="color:red">*</span>
                                                  <asp:Literal ID="ltBatchNo" runat="server" Text="Batch No. :" ></asp:Literal><br />
                                                  <asp:TextBox ID="txtBatchNo" runat="server"></asp:TextBox>
                                              </td>
                                              <td>
                                                  <span id="SpProjRefNo" runat="server" style="color:red">*</span>
                                                  <asp:Literal ID="ltProjectRefNo" runat="server" Text="Project Ref. No. :"></asp:Literal><br />
                                                  <asp:TextBox ID="txtProjectRefNo" runat="server" ></asp:TextBox>
                                              </td>
                                          </tr>
                                  
                                          <tr>
                                         <td colspan="3" align="left" >
                                             <table id="tbMaterialStorageparameter" runat="server" border="0" cellspacing="0" cellpadding="0"  width="100%" >
                                         
                                             </table>
                                         </td>
                                     </tr>
                                          <tr>
                                              <td align="left" colspan="3">
                                                   <span runat="server" style="color: red">*</span>
                                             <asp:RequiredFieldValidator ID="rfStorageLocation" runat="server" ValidationGroup="receiveQuantity" ControlToValidate="ddlStorageLocationID" Display="Dynamic"  />
                                            <asp:Literal ID="Literal1" runat="server" Text="Storage Location:"  /><br />
                                            <asp:DropDownList ID="ddlStorageLocationID" runat="server" Width="160px"  ></asp:DropDownList>
                                                  </td>
                                          </tr>
                                  
                                          <tr>
                                              <td align="left">
                                                  <%--<asp:CustomValidator ID="rfvPutaway" runat="server" ValidationGroup="receiveQuantity" ControlToValidate="txtPutaway" OnServerValidate="rfvCarton_ServerValidate"  ErrorMessage=" * " />--%>
                                                   <span runat="server" style="color: red">*</span>
                                                  <asp:Literal ID="ltPutaway" runat="server" Text="Location Putaway:"  /><br />
                                                  <asp:TextBox ID="txtPutaway"   runat="server" onKeypress="return checkSpecialChar(event)" SkinID="txt_Auto" Width="140" />
                                                  <asp:HiddenField runat="server" ID="hifputwayloc" />
                                              </td>
                                              <td align="left" colspan="3">
                                                  <span runat="server" style="color: red">*</span>
                                                  <%--<asp:RequiredFieldValidator ID="rfvContainerCode" runat="server" ValidationGroup="receiveQuantity" ControlToValidate="txtContainerCode" Display="Dynamic" ErrorMessage=" * " />--%>
                                                  <asp:Literal ID="ltContainerCode" runat="server" Text="Container Code:" /><br />
                                                  <asp:TextBox ID="txtContainerCode" runat="server" onKeypress="return checkSpecialChar(event)" SkinID="txt_Auto" Width="140" />
                                                  <asp:HiddenField runat="server" ID="hifContainercode" />
                                                  <asp:LinkButton ID="lnkGenerateNewContainer" runat="server" OnClick="lnkGenerateNewContainer_Click" CssClass="ui-btn ui-button-small" ToolTip="New Container" Visible="false">New <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkPrint" runat="server" CssClass="ui-btn ui-button-small" ToolTip="Print Barcode">Print <span class="fa fa-print"></span></asp:LinkButton>
                                              </td>

                                          </tr>
                                  
                                          <tr>
                                        <td align="left" colspan="3">
                                            <asp:Literal ID="ltRemarks" runat="server" Text="Remarks:" /><br />
                                            <asp:TextBox ID="txtRemarks" runat="server" Width="46%" TextMode="MultiLine" Rows="3"/>
                                        </td>
                                    </tr>
                                  
                                          <tr>
                                        <td colspan="3" align="right" width="80%">
                                            <br />
                                            
                                            <asp:CheckBox runat="server" ID="chkIsPrintRequired" Text="Is Print Required" Checked="true"/>

                                            <asp:DropDownList ID="ddlNetworkPrinter" runat="server" CssClass="NoPrint" /> &nbsp;&nbsp;&nbsp;

                                            
                                            <asp:DropDownList ID="ddlLabelSize" runat="server" CssClass="NoPrint"></asp:DropDownList>


                                            <asp:LinkButton ID="lnkReceive" runat="server"  Text="" OnClientClick="showAsynchronus(); CheckReceiveQty();" OnClick="inkReceive_Click" ValidationGroup="receiveQuantity" CssClass="btn btn-primary">
Receive <%=MRLWMSC21Common.CommonLogic.btnfaSave %>
</asp:LinkButton>
                                            
                                            <br />
                                    
                                        </td>
                                    </tr>

                                         


                                    <tr>
                                        <td align="left" colspan="3">
                                            <asp:Panel ID="pnlGoodsIndata" runat="server" Width="790" ScrollBars="Auto">

                                            
                                             <asp:UpdatePanel ChildrenAsTriggers="true"  ID="upnlReceivedStock" runat="server" UpdateMode="Always">
                                                 <ContentTemplate>
                                                            <asp:GridView ID="gvPOQuantityList"  SkinID="gvLightGreenNew" PageSize="20" OnRowCommand="gvPOQuantityList_RowCommand" OnPageIndexChanging="gvPOQuantityList_PageIndexChanging" Width="100%"  runat="server" AutoGenerateColumns="false" OnRowCancelingEdit="gvPOQuantityList_RowCancelingEdit" OnRowDataBound="gvPOQuantityList_RowDataBound" OnRowEditing="gvPOQuantityList_RowEditing" OnRowUpdating="gvPOQuantityList_RowUpdating"  OnSelectedIndexChanging="gvPOQuantityList_SelectedIndexChanging">
                                                                <Columns>
                                   
                                                                    <asp:TemplateField HeaderStyle-Width="80" HeaderText="Container">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="ltGoodsMovementDetailsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "GoodsMovementDetailsID").ToString() %>' />
                                                                            <asp:Literal ID="ltLocation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CartonCode").ToString() %>' />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ValidationGroup="gridrequirequantity" ControlToValidate="txtLocation" Display="Dynamic" ErrorMessage=" * " />
                                                                            <asp:Literal ID="ltGoodsMovementDetailsID_Edit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "GoodsMovementDetailsID").ToString() %>' />
                                                                            <asp:TextBox ID="txtLocation" Width="70" runat="server" onKeypress="return checkSpecialChar(event)" Text='<%# DataBinder.Eval(Container.DataItem, "CartonCode").ToString() %>' />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField  HeaderStyle-Width="80" HeaderText="Location">
                                                                        <ItemTemplate>
                                                                             <asp:Literal ID="ltloc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Location").ToString() %>' />
                                                                        </ItemTemplate>
                                                                       
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-Width="40" HeaderText="Is Dam.">
                                                                        <ItemTemplate >
                                                                            <%--<asp:Image ID="imgDamage"  ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "IsDamaged").ToString()) %>' />--%>
                                                                            <asp:Literal runat="server" ID="ltImgDamage" Text='<%#Getimage(DataBinder.Eval(Container.DataItem, "IsDamaged").ToString()) %>'>' ></asp:Literal>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:CheckBox ID="chkDamaged"  Width="30" runat="server" Checked='<%# GetBool(DataBinder.Eval(Container.DataItem, "IsDamaged").ToString()) %>' />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-Width="40" HeaderText="Has Disc.">
                                                                        <ItemTemplate >
                                                                            <%--<asp:Image ID="imgDiscrepancy"  ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "HasDiscrepancy").ToString()) %>' />--%>
                                                                            <asp:Literal runat="server" ID="ltImgDiscrepancy" Text='<%#Getimage(DataBinder.Eval(Container.DataItem, "HasDiscrepancy").ToString()) %>'>' ></asp:Literal>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:CheckBox ID="chkDiscrepancy"  Width="30" runat="server" Checked='<%# GetBool(DataBinder.Eval(Container.DataItem, "HasDiscrepancy").ToString()) %>' />
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderStyle-Width="40" HeaderText="QC Non Conf.">
                                                                        <ItemTemplate >
                                                                            <%--<asp:Image ID="imgisNonconformity"  ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "IsNonConfirmity").ToString()) %>' />--%>
                                                                            <asp:Literal runat="server" ID="ltImgNonConformity" Text='<%#Getimage(DataBinder.Eval(Container.DataItem, "IsNonConfirmity").ToString()) %>'>' ></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                     <asp:TemplateField HeaderStyle-Width="40" HeaderText="Pve Recall">
                                                                        <ItemTemplate >
                                                                            <%--<asp:Image ID="imgIsPositiveRecall"  ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "IsPositiveRecall").ToString()) %>' />--%>
                                                                            <asp:Literal runat="server" ID="ltImgpositiveRecall" Text='<%#Getimage(DataBinder.Eval(Container.DataItem, "IsPositiveRecall").ToString()) %>'>' ></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-Width="40" HeaderText="As Is">
                                                                        <ItemTemplate >
                                                                            <%--<asp:Image ID="imgAsIs"  ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "AsIs").ToString()) %>' />--%>
                                                                            <asp:Literal runat="server" ID="ltImgAsIs" Text='<%#Getimage(DataBinder.Eval(Container.DataItem, "AsIs").ToString()) %>'>' ></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderStyle-Width="80" HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate >
                                                                            <asp:Literal ID="ltQuantity"  runat="server"  Text='<%# DataBinder.Eval(Container.DataItem, "DocQty") %>' />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ValidationGroup="gridrequirequantity" ControlToValidate="txtQuantity" Display="Dynamic" ErrorMessage=" * " />
                                                                            <asp:TextBox ID="txtQuantity" Width="70" onKeyPress="return checkDec(this,event)" onblur="CheckDecimal(this)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocQty").ToString() %>' />
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate >
                                                                            <asp:Literal ID="ltQuantityCount" runat="server" />
                                                                        </FooterTemplate>

                                        
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                     </ContentTemplate>
                                                 </asp:UpdatePanel>
                                             </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                                                     
                      </fieldset>
                       <fieldset  style="border:1px solid #808080;border-radius:5px;width:800px; margin: auto; display:none;" align="left">
                        <LEGEND><b>Container Location Mapping</b></LEGEND>
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <asp:LinkButton ID="lnkMapContainertoLocation" runat="server" OnClick="lnkMapContainertoLocation_Click" Visible="false" CssClass="ui-btn ui-button-small">Add Location<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlMapping" runat="server" UpdateMode="Always">
                                        <ContentTemplate>

                                        
                                    <asp:GridView ID="gvLocationMapping"  Visible="false" runat="server" SkinID="gvLightGreenNew" Width="50%" PageSize="20" AutoGenerateColumns="false"
                                        OnRowDataBound="gvLocationMapping_RowDataBound"
                                        OnRowEditing="gvLocationMapping_RowEditing"
                                        OnRowUpdating="gvLocationMapping_RowUpdating"
                                        OnRowCancelingEdit="gvLocationMapping_RowCancelingEdit"
                                        >
                                        <Columns>
                                            <asp:TemplateField HeaderText="Container Code" ItemStyle-Width="400">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltCartonCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CartonCode")%>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvCartoon" runat="server"  ControlToValidate="txtCartonCode" ErrorMessage="*" ValidationGroup="VgMapping" Display="Dynamic" />
                                                    <asp:TextBox ID="txtCartonCode" CssClass="LocCarton txt_small_Auto" ClientIDMode="Static"  runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CartonCode")%>' />
                                                    <asp:HiddenField ID="hifPreviousCartonID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"CartonID")%>' />
                                                    <asp:HiddenField ID="hifCartonID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"CartonID")%>' />
                                                    
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Location" ItemStyle-Width="300">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltLocation" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Location")%>' />
                                                </ItemTemplate>
                                                <EditItemTemplate >
                                                     <asp:RequiredFieldValidator ID="rfvLocation" runat="server"  ControlToValidate="txtLocation" ErrorMessage="*" ValidationGroup="VgMapping" Display="Dynamic" />
                                                    <asp:TextBox ID="txtLocation" CssClass="LocPutaway txt_small_Auto" EnableTheming="false"  runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Location")%>' />
                                                   <asp:HiddenField ID="hifPreviousLocationID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"LocationID")%>' />
                                                    <asp:HiddenField ID="hifLocationID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"LocationID")%>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ControlStyle-Font-Underline="false" CausesValidation="true" EditText="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton="true" ItemStyle-Width="200" />
                                        </Columns>
                                    </asp:GridView>
                                            </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </asp:Panel>

                    

            </td>
        </tr>

      
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
     
    </table>



     <!-- Input Dialog for Print Quantity -->
    
    <div id="divPrintDlgContainer">

        <div id="divPrintDlg" style="display: none;">

            <asp:UpdatePanel ID="upnlPrintDialog" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                <ContentTemplate>

                    <table border="0" align="center" cellpadding="5" cellspacing="5">
                        <tr>
                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ID="rfvtxtPrintQty" runat="server" Enabled="false" ControlToValidate="txtPrintQty" Display="Dynamic" ErrorMessage=" * " />
                                <asp:Label runat="server" ID="lblPrintQty" Text="Please enter quantity :" CssClass="FormLabelsBlue"></asp:Label>
                                &nbsp;&nbsp;&nbsp;

                                <asp:TextBox runat="server" ID="txtPrintQty" Text="1" Width="50" onKeyPress="return checkNum(event)"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td align="right">

                                <asp:LinkButton runat="server" ID="lnkPrintSubmit" OnClick="lnkPrintSubmit_Click" SkinID="lnkButEmpty" Text="Ok"></asp:LinkButton>

                            </td>

                        </tr>

                    </table>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    
    <!-- Input Dialog for Print Quantity -->
    </div>



    </asp:Content>
