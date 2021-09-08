<%@ Page Title=" Bill of Material :." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="BillofMaterial.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.BillofMaterial" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="ManfContent" runat="server">
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="ss" SupportsPartialRendering="true"></asp:ScriptManager>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <style>
        img
        {  
            border-style: none;
            
        }
    </style>
    

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

                $("#<%= this.txtEffectedDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
                
                

                var textfieldname = $('#atcMaterialCode');
                DropdownFunction(textfieldname);
                $('#atcMaterialCode').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetMCode") %>',
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

                     $("#hifMaterialCode").val(i.item.val);
                     MSPConfifure();
                 },
                 minLength: 0
                });


                var textfieldname = $('#<%=this.txtMType.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=this.txtMType.ClientID%>').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMTypeData") %>',
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
                         $("#<%=hifMTypeID.ClientID %>").val(i.item.val);

                         document.getElementById('<%=this.hifBOMDetailsID.ClientID%>').value = "";
                         

                         var MTypeID = document.getElementById('<%=this.hifMTypeID.ClientID%>').value;

                         /*
                         if (MTypeID == "8" || MTypeID == "9") {
                            
                             document.getElementById('tdChildRev').style.display = "inline";
                             document.getElementById('tdChildMCode').style.display = "none";
                             document.getElementById('tdMMUoM').style.display = "none";
                             document.getElementById('tdMMRevUoM').style.display = "inline";
                             
                             
                         }
                         else {
                             
                             document.getElementById('tdChildMCode').style.display = "inline";
                             document.getElementById('tdChildRev').style.display = "none";

                             document.getElementById('tdMMUoM').style.display = "inline";
                             document.getElementById('tdMMRevUoM').style.display = "none";
                         }
                         */

                        
                     },
                     minLength: 0
                 });

                try
                {


                    var textfieldname = $('#<%=this.txtChildMMCode.ClientID%>');
                    DropdownFunction(textfieldname);
                    $('#<%=this.txtChildMMCode.ClientID%>').autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMCodeByMTypeWithOEM") %>',
                                data: "{ 'prefix': '" + request.term + "','TenantID':'" +<%=cp.TenantID%> + "','MTypeID':'" + document.getElementById('<%=this.hifMTypeID.ClientID%>').value + "'}",
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
                            $("#<%=hifTvBOMChildPartNo.ClientID %>").val(i.item.val);

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

                } catch (ex) { }


                var textfieldname = $('#<%=this.txtUoM.ClientID%>');
                DropdownFunction(textfieldname);
                $("#<%=this.txtUoM.ClientID%>").autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMWithQty") %>',
                            data: "{ 'MaterialID': '" + document.getElementById('<%=this.hifTvBOMChildPartNo.ClientID%>').value + "'}",
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

                        

                        $("#<%=hifTvBOMUoM.ClientID %>").val(i.item.val);

                        


                    },
                    minLength: 0
                });



                var textfieldname = $('#<%=this.txtParentMMCode.ClientID%>');
                DropdownFunction(textfieldname);
                $("#<%= this.txtParentMMCode.ClientID %>").autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadParentRevMcodes") %>',
                            data: "{ 'prefix': '" + request.term + "','BOMHeaderID':'" + document.getElementById('<%=this.hifBOMHeaderID.ClientID%>').value + "' }",
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

                        $("#<%=hifTvBOMParentPartNo.ClientID %>").val(i.item.val);
                          },
                          minLength: 0
                });


                var textfieldname = $('#<%=this.txtbomrefno.ClientID%>');
                DropdownFunction(textfieldname);
                $("#<%= this.txtbomrefno.ClientID %>").autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRoutingRevMcodes") %>',
                         data: "{ 'prefix': '" + request.term + "' }",
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

                              $("#<%=hidBomRef.ClientID %>").val(i.item.val);
               },
               minLength: 0
                });

                

                var textfieldname = $('#<%=this.txtChildMMCodeRev.ClientID%>');
                DropdownFunction(textfieldname);
                $("#<%= this.txtChildMMCodeRev.ClientID %>").autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRevMcodesByMType") %>',
                            data: "{ 'prefix': '" + request.term + "','MTypeID':'" + document.getElementById('<%=this.hifMTypeID.ClientID%>').value + "' }",
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

                        $("#<%=hifChildRevMMID.ClientID %>").val(i.item.val);
                          },
                          minLength: 0
                });




                try
                {
                    var textfieldname = $('#<%=this.txtRTPartNumber.ClientID%>');
                    DropdownFunction(textfieldname);
                    $("#<%= this.txtRTPartNumber.ClientID %>").autocomplete({
                        source: function (request, response) {

                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRevMcodesByMTypeWithOEM") %>',
                                data: "{ 'prefix': '" + request.term + "','MTypeID':'" + document.getElementById('<%=this.hifMTypeID.ClientID%>').value + "' }",
                                dataType: "json",
                                type: "POST",
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

                            $("#<%=hifRTPartNoID.ClientID %>").val(i.item.val);

                            document.getElementById('<%=this.txtRTUoM.ClientID%>').value = "";
                            document.getElementById('<%=this.txtUoM.ClientID%>').value = "";
                            document.getElementById('<%=this.txtMMRevUoM.ClientID%>').value = "";

                            if (document.getElementById('<%=this.hifMTypeID.ClientID%>').value == "8" || document.getElementById('<%=this.hifMTypeID.ClientID%>').value == "8") {

                            } else {

                            }

                            
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

                } catch (ex) { }



                var textfieldname = $('#<%=this.txtRTUoM.ClientID%>');
                DropdownFunction(textfieldname);
                $("#<%=this.txtRTUoM.ClientID%>").autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMWithQty1") %>',
                            data: "{ 'MaterialID': '" + document.getElementById('<%=this.hifRTPartNoID.ClientID%>').value + "','MTypeID':'" + document.getElementById('<%=this.hifMTypeID.ClientID%>').value + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {

                                if (data.d == "") {
                                    alert("No UoM is configured to this part number");
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



                        $("#<%=hifRTUoMID.ClientID %>").val(i.item.val);

                      


                    },
                    minLength: 0
                });





                
                var textfieldname = $('#<%=this.atcBOMType.ClientID%>');
                DropdownFunction(textfieldname);
                $("#<%= this.atcBOMType.ClientID %>").autocomplete({
                    source: function (request, response) {
                       
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadBoMTypeList") %>',
                       data: "{ 'prefix': '" + request.term + "' }",
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

                   $("#<%=hifbomtype.ClientID %>").val(i.item.val);
               },
               minLength: 0
           });
                var textfieldname = $('#<%=this.atcPROUoM_qty.ClientID%>');
                DropdownFunction(textfieldname);
                $("#<%= this.atcPROUoM_qty.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMMRUoMWithQty") %>',
                            data: "{ 'MMRvID': '" + document.getElementById('<%=this.hidBomRef.ClientID%>').value + "'}",
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
                     $("#<%=hifPROUoM_qty.ClientID %>").val(i.item.val);
                 },
                 minLength: 0
             });


                var textfieldname = $("#atcMaterialCode");
                DropdownFunction(textfieldname);
                $("#atcMaterialCode").autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetMCode") %>',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" +<%=cp.TenantID%> +"'}",
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

                         $("#hifMaterialCode").val(i.item.val);
                },
                minLength: 0
                });

                var textfieldname = $("#atcBOMUoM_Qty");
                DropdownFunction(textfieldname);
                $("#atcBOMUoM_Qty").autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMWithQty") %>',
                            data: "{ 'MaterialID': '" + document.getElementById('hifMaterialCode').value + "'}",
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

                        $("#hifBOMUoM_Qty").val(i.item.val);

                       

                    },
                    minLength: 0
                });

                var textfieldname = $("#<%=this.txtMMRevUoM.ClientID%>");
                DropdownFunction(textfieldname);
                $("#<%=this.txtMMRevUoM.ClientID%>").autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMMRUoMWithQty") %>',
                            data: "{ 'MMRvID': '" + document.getElementById('<%=this.hifChildRevMMID.ClientID%>').value + "'}",
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

                        

                        $("#<%=hifRevUoMID.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });


            });
        }
        fnMCodeAC();
    </script>


    <script type="text/javascript">
       
       

        function CheckIsDelted(checkBox) {
            if (checkBox.checked) {

                alert('Are you sure want to delete?');
                
            }
            else {
                checkBox.checked = false;
            }
        }


        function ClearText(TextBox) {
            if (TextBox.value == "Search Material Code...")
                TextBox.value = "";
        }
        function focuslost(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search Material Code...";
        }

    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#divEditRevisionDispute").dialog({
                autoOpen: false,
                modal: true,
                height: 365,
                width: 550,
                resizable: false,
                draggable: false,
                title: "Revision History",
               
                 
                position: ["center top", 40],
                close: function () {
                    $("#divEditRevisionDispute").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                },
                open: function (event, ui) {
                    $("#divEditRevisionDispute").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });

                    $(this).parent().appendTo("#divEditRevisionDisputeDlgContainer");
                },


            });
        });

        function closeRevisionDialog() {
            //Could cause an infinite loop because of "on close handling"
            $("#divEditRevisionDispute").dialog('close');
        }


        function openRevisionDialog(title) {

            

            $("#divEditRevisionDispute").dialog("option", "title", "Revision History");
            $("#divEditRevisionDispute").dialog('open');
            
            blockRevisionDialog();

        }


        function blockRevisionDialog() {
            //block it to clean out the data
            $("#divEditRevisionDispute").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_inb.gif" />',
                  css: { border: '0px' },
                  fadeIn: 0,
                  fadeOut: 0,
                  overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
              });
            unblockRevisionDialog();
          }


          function unblockRevisionDialog() {
              $("#divEditRevisionDispute").unblock();
          }


  </script>

    <style type="text/css">
        .treeNode
        {
            font-size:14pt;
            width:100%;
            font-weight:bold;
            color:#FF4000;
            font-family:Calibri;
            padding:3px;
            /*text-decoration:line-through;*/
        }
        .rootNode
        {
            font-size:16pt;
            width:100%;
            font-weight:bold;
            color:#ff6a00;
            font-family:Calibri;
            padding:3px;
        }
        .leafNode
        {
            font-size:13pt;
            padding:4px;
           font-family:Calibri;
            font-weight:bold;
            color:#000000;
            
        }
        .hoverNode {

            background-color:#000000;
            color:white;
            font-family:Calibri;
            font-weight:bold;
            border:1px solid black;
            border-radius:5px;
            padding:3px;
        }
        .selectedNode {

            color:#2b44d8;
            font-family:Calibri;
            font-weight:bold;
            padding:3px;
        }

                   .rotate {

   -webkit-transform: rotate(90deg);
      -moz-transform: rotate(90deg);
       -ms-transform: rotate(90deg);
        -o-transform: rotate(90deg);
           transform: rotate(90deg);
   filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=1);

   color: #ff6500;
   font-size: 35px;
   font-family: Calibri;
   text-align:left;
   position: fixed;
   font-weight:bold;
   letter-spacing:2px;
   text-shadow:0px 0px 5px #808080;

}
    </style>


    <script type="text/javascript">
        $(function () {
            $("[id*=tvHierarchyView] input[type=checkbox]").bind("click", function () {
                var table = $(this).closest("table");
                if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                    //Is Parent CheckBox
                    var childDiv = table.next();
                    var isChecked = $(this).is(":checked");
                    $("input[type=checkbox]", childDiv).each(function () {
                        if (isChecked) {
                            $(this).attr("checked", "checked");
                        } else {
                            $(this).removeAttr("checked");
                        }
                    });
                } else {
                    //Is Child CheckBox
                    var parentDIV = $(this).closest("DIV");
                    if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                        $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                    } else {
                        $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    }
                }
            });
        })

</script>

     
    <table width="100%" align="center" cellpadding="3" cellspacing="3" style="padding-left:10px;padding-right:10px;">
       

        <tr>
            <td colspan="3" align="left">
                        <span class="mandatory_field"> Note: </span>
                         <asp:Label ID="lberrormsg" runat="server" CssClass="errorMsg" Text=" * " />
                         Indicates mandatory fields

            </td>
            <td align="right" valign="top" >
                       
                         
                     </td>
        </tr>

        <tr>
            <td class="FormLabels" colspan="4"  align="right" >

                <a style="text-decoration:none;" href="BillofMaterialList.aspx" class="ui-button-small">BOM List<%=MRLWMSC21Common.CommonLogic.btnfaList %>  </a> 

                &nbsp;&nbsp;&nbsp;

                <a style="text-decoration:none;" href="BillofMaterial.aspx" class="ui-button-small">New BOM <%=MRLWMSC21Common.CommonLogic.btnfaNew %></a> 

            </td>
         </tr>
                
        <tr>
            <td colspan="4">
                <asp:Literal ID="lblStatus" runat="server" />

            </td>
        </tr>
        
        <tr>
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar" style="">BOM Header</div>
                
                <div class="ui-Customaccordion">


                    <table width="100%" style="padding-top: 10px; padding-left: 10px;">
        

                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel ChildrenAsTriggers="true" ID="uppoNumber" UpdateMode="Always" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:RequiredFieldValidator ID="rfvbomrefno" runat="server" ValidationGroup="save" ControlToValidate="txtbomrefno" Display="Dynamic" ErrorMessage=" * " />
                                                                    <asp:Literal ID="ltbomrefno" runat="server" Text="BOM Ref. No.:" /><br />
                                                                    <asp:TextBox ID="txtbomrefno" runat="server" Width="230" SkinID="txt_Auto" />
                                                                    <asp:HiddenField ID="hidBomRef" runat="server" />
                                                
                                               
                                               
                                                                </ContentTemplate>

                                                            </asp:UpdatePanel>

                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvtxtRevision" runat="server" ValidationGroup="save" ControlToValidate="txtRevision" Display="Dynamic" ErrorMessage=" * " />
                                     <asp:Literal ID="ltRevision" runat="server" Text="Revision Number:" /><br />
                                     <asp:TextBox ID="txtRevision" runat="server"  CssClass="txt_Blue_Small"  Width="200" />
                                </td>

                                 <td align="left">
                                        <asp:RequiredFieldValidator ID="rfvPROUoM_qty" runat="server" ValidationGroup="save" ControlToValidate="atcPROUoM_qty" Display="Dynamic" ErrorMessage=" * " />
                                        <asp:Literal ID="lclPROUoM_qty" runat="server" Text="UoM/Qty.:" /><br />
                                        <asp:TextBox ID="atcPROUoM_qty" runat="server"  onKeyPress="return checkDec(this,event)"   SkinID="txt_Auto"></asp:TextBox>
                                        <asp:HiddenField ID="hifPROUoM_qty" runat="server" />
                                </td>
           
                                 <td align="left" valign="top">
                                            <asp:RequiredFieldValidator ID="rfvBOMType" runat="server" ValidationGroup="save" ControlToValidate="atcBOMType" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal ID="lclBOMType" runat="server" Text="Bill of Material Type:" /><br />
                                            <asp:TextBox ID="atcBOMType"  runat="server"   SkinID="txt_Auto" />
                                            <asp:HiddenField ID="hifbomtype" runat="server" />
                                        </td>

                            </tr>
        
                            <tr>
            
           

                                <td align="left" colspan="2"  class="FormLabels"  valign="top">
                                            <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ValidationGroup="save" ControlToValidate="txtProductName" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal ID="lclProductName" runat="server" Text="Product Name:" /><br />
                                            <asp:TextBox ID="txtProductName" onKeypress="return checkSpecialChar(event)" runat="server"  Width="450" TextMode="MultiLine" />
                     
                                </td>

                                <td align="left" colspan="2" class="FormLabels" valign="top">
                                            Remarks: <br />
                                             <asp:TextBox ID="txtBOMRevRemarks" onKeypress="return checkSpecialChar(event)" runat="server"  Width="450" TextMode="MultiLine" />
                                </td>


                            </tr>
        
                            <tr>
                                <td class="FormLabels" align="left" colspan="2"> 
                                     Effective Date: &nbsp;&nbsp;&nbsp;

                                     <asp:Label runat="server" ID="lblEffectiveDate" ></asp:Label>
                                </td>
                                <td align="right" colspan="2">
                                    <asp:CheckBox ID="chkIsActive" Text="Active" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkIsDeleted" onclick="CheckIsDelted(this);" Text="Delete" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2" valign="bottom">
                
                 
                                    <asp:UpdatePanel ChildrenAsTriggers="true" ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                                                <ContentTemplate>

                                    <asp:LinkButton runat="server" ID="lnkCopyBoMDetails"  Visible="false" CssClass="ui-btn ui-button-large"  OnClientClick="openRevisionDialog('Add New Revision');">

                                        Clone BOM <span class="space fa fa-copy" ></span>
                                    </asp:LinkButton>


                                                                    </ContentTemplate>
                                        </asp:UpdatePanel>

                                </td>
                                <td colspan="2" align="right">
                 
                                    <br />
                                    
                                    <asp:LinkButton ID="lnkClear"  runat="server"  OnClick="lnkClear_Click" CssClass="ui-btn ui-button-large">
Cancel <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
</asp:LinkButton>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;

                                    <asp:LinkButton ID="lnkUpdate" CausesValidation="true" runat="server"  CssClass="ui-btn ui-button-large" ValidationGroup="save" OnClick="lnkUpdate_Click" />
               
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr> 


                        </table>
                </div>
            </td>
        </tr>




       




       <tr>
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar" style="">BOM Line Items</div>
                
                <div class="ui-Customaccordion">


                    <table width="100%" style="padding-top: 10px; padding-left: 10px;">

                            <tr>
                                <td align="right" colspan="4" style="display:none;">
                                    <asp:LinkButton ID="lnkAddNewLineItem" runat="server" Text="Add BOM Line Item" OnClick="lnkAddNewLineItem_Click" SkinID="lnkButEmpty" />
                                    <asp:ImageButton ID="imgbtngvBoMlineItems" Visible="false" runat="server"  ImageAlign="Right" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvBoMlineItems_Click" ToolTip="Export To Excel" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="4"  class="SubHeading3" valign="top" >

                                    <asp:Label  Text="Total Items " runat="server" ID="lblTotalItems" > </asp:Label>
                                    <asp:Label runat="server" ID="lblLineItemsCount"></asp:Label>

                                    &nbsp;&nbsp;&nbsp;&nbsp;
                
               
                                    <asp:ImageButton ID="imgbtngvBOMList" runat="server"  ImageAlign="AbsMiddle"  ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvBOMList_Click" ToolTip="Export To Excel" />
                                </td>
                            </tr>

                            <tr>

                                <td colspan="2" valign="top">

                                    <br />
                 
                                    <asp:TreeView runat="server" ID="tvHierarchyView" NodeStyle-CssClass="treeNode"
                                                    RootNodeStyle-CssClass="rootNode"
                                                    LeafNodeStyle-CssClass="leafNode"
                                                    HoverNodeStyle-CssClass="hoverNode"
                                                    SelectedNodeStyle-CssClass="selectedNode"
                                
                                                    OnSelectedNodeChanged="tvHierarchyView_SelectedNodeChanged" ShowLines="true" ShowExpandCollapse="true" NodeIndent="35"  >

                                

                                    </asp:TreeView>

                        
                      
               

                                </td>

                                <td colspan="2" align="left" valign="top">

                
                
            


                                            <asp:Panel runat="server" ID="pnlBOMTreeView" DefaultButton="lnkButUpdate">
                                            <div style="width:450px;border:1px solid #808080;border-radius:6px;box-shadow:3px 3px 3px #808080;padding-right:10px;">

                            



                           

                                                <table border="0" cellpadding="6" cellspacing="3" align="center" >
                                                    <tr>
                                                        <td colspan="3" align="left" class="FormLabels">
                                                            <asp:LinkButton runat="server" Text="Add Child Item" Visible="false" ID="lnkAddChildItem" SkinID="lnkButEmpty" OnClick="lnkAddChildItem_Click"></asp:LinkButton>
                                                            <span class="SubHeading3">Item Details: </span>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="3" class="FormLabels">

                                                            <asp:RequiredFieldValidator ID="rfvtxtMType" runat="server" ValidationGroup="updateBOMItems" ControlToValidate="txtMType" Display="Dynamic" ErrorMessage=" * " />
                                                            Material Type:
                                                            <br />
                                                            <asp:TextBox ID="txtMType" runat="server" SkinID="txt_Auto"/> 


                                                        </td>
                                                    </tr>

                                                    <tr>

                                                         <td class="FormLabels" id="tdChildMCode" style="display:none;">
                                                            Part #:
                                                            <br />
                                                            <asp:TextBox ID="txtChildMMCode" runat="server" SkinID="txt_Auto"/> 
                                         

                                                        </td>
                                                        <td class="FormLabels" style="display:none;" id="tdChildRev" >
                                                            Part #:
                                                            <br />
                                                            <asp:TextBox ID="txtChildMMCodeRev"  runat="server" SkinID="txt_Auto"/> 
                                                        </td>
                                    
                                                        <td class="FormLabels">
                                                            Part Number:<br />
                                                            <asp:TextBox runat="server" ID="txtRTPartNumber" SkinID="txt_Auto"></asp:TextBox>
                                                        </td>

                                                        <td class="FormLabels">
                                        

                                                            Parent Part #:
                                                            <br />
                                                            <asp:TextBox ID="txtParentMMCode" runat="server" SkinID="txt_Auto"/> 
                                                        </td>
                                   
                
                                                    </tr>
                                                               
                                                    <tr>
                                                        <td class="FormLabels">
                                                             <asp:RequiredFieldValidator ID="rfvtxtRTUoM" runat="server" ValidationGroup="updateBOMItems" ControlToValidate="txtRTUoM" Display="Dynamic" ErrorMessage=" * " />
                                                            UoM / Qty.: <br />
                                                            <asp:TextBox ID="txtRTUoM" SkinID="txt_Auto" runat="server"></asp:TextBox>
                                                        </td>

                                                        <td class="FormLabels" id="tdMMUoM" style="display:none;" >
                                                            
                                                             UoM / Qty.:
                                                            <br />
                                                            <asp:TextBox ID="txtUoM" runat="server" SkinID="txt_Auto"/> 
                                                        </td>

                                                        <td class="FormLabels" style="display:none;" id="tdMMRevUoM">
                                                            
                                                             UoM / Qty.:
                                                            <br />
                                                            <asp:TextBox ID="txtMMRevUoM" runat="server" SkinID="txt_Auto"/> 
                                                        </td>


                                                        <td class="FormLabels" >
                                                              <asp:RequiredFieldValidator ID="rfvtxtQuantity" runat="server" ValidationGroup="updateBOMItems" ControlToValidate="txtQuantity" Display="Dynamic" ErrorMessage=" * " />
                                                             Quantity:
                                                            <br />
                                                            <asp:TextBox ID="txtQuantity" onKeyPress="return checkDec(this,event)" runat="server" Width="200"/> 
                                                        </td>
                                                    </tr>
            
                                                    <tr>
                                    
                                                        <td class="FormLabels" valign="bottom" align="right" colspan="3">
                                                            <asp:CheckBox runat="server" ID="chkBDDelete" Text="Delete" onclick="CheckIsDelted(this);" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="FormLabels" align="right" colspan="3">
                                                            <br />

                                                            <asp:LinkButton runat="server" ID="lnkButCancel" OnClick="lnkButCancel_Click" CssClass="ui-btn ui-button-large"> Clear <%=MRLWMSC21Common.CommonLogic.btnfaClear %> </asp:LinkButton>

                                                                &nbsp;&nbsp;&nbsp;

                                                            <asp:LinkButton runat="server" ID="lnkButUpdate" ValidationGroup="updateBOMItems" OnClick="lnkButUpdate_Click" CssClass="ui-btn ui-button-large">
Update <%=MRLWMSC21Common.CommonLogic.btnfaUpdate %>
</asp:LinkButton>
                                                            
                                                            <br />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            &nbsp;

                                                        </td>
                                                    </tr>
                                                </table>


                            
    
 
                             
                                            </div>
                                            </asp:Panel>
                   

                                </td>
                            </tr>

                            <tr>
                                <td colspan="4" style="visibility:hidden;">
                                    <asp:Literal ID="ltGridStatus" runat="server" /><br />
                                    <asp:GridView ID="gvBoMlineItems" SkinID="gvLightGreen" runat="server" PageSize="15" AutoGenerateColumns="false" OnRowEditing="gvBoMlineItems_RowEditing" OnRowUpdating="gvBoMlineItems_RowUpdating" OnRowCancelingEdit="gvBoMlineItems_RowCancelingEdit">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Line No."  ItemStyle-Width="60" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:literal id="ltPROLineNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BOMLineNumber").ToString() %>' />
                                                     <asp:Literal ID="ltbomDetailsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "BOMDetailsID").ToString() %>' />
                                                    <asp:Literal ID="ltMMID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "BOMMaterialMasterID").ToString() %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Literal ID="ltbomDetailsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "BOMDetailsID").ToString() %>' />
                                                    <asp:Literal ID="ltBoMLineNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BOMLineNumber").ToString() %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                         
                                            <asp:TemplateField HeaderText="Material Code" ItemStyle-Width="130" >
                                                <ItemTemplate>
                                                    <asp:literal id="ltMaterialCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvMaterialCode" runat="server" ValidationGroup="UpdateGridItems" ControlToValidate="atcMaterialCode" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox ID="atcMaterialCode" runat="server" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                                                    <asp:HiddenField ID="hifMaterialCode" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "BOMMaterialMasterID").ToString() %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                              <asp:TemplateField HeaderText="Material Type" ItemStyle-Width="130" >
                                                <ItemTemplate>
                                                    <asp:literal id="ltMType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MType").ToString() %>' />
                                                </ItemTemplate>
                                                  </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Product Category" ItemStyle-Width="130" >
                                                <ItemTemplate>
                                                    <asp:literal id="ltProductCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProductCategory").ToString() %>' />
                                                </ItemTemplate>
                                                  </asp:TemplateField>

                                             <asp:TemplateField HeaderText=" UoM/Qty." ItemStyle-Width="100" >
                                                <ItemTemplate>
                                                    <asp:literal id="ltbuom_qty" runat="server" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "UoM").ToString(),DataBinder.Eval(Container.DataItem, "UoMQty").ToString())  %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvBOMUoM_Qty" runat="server" ValidationGroup="UpdateGridItems" ControlToValidate="atcBOMUoM_Qty" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox ID="atcBOMUoM_Qty" runat="server" Width="80" ClientIDMode="Static" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "UoM").ToString(),DataBinder.Eval(Container.DataItem, "UoMQty").ToString()) %>' />
                                                    <asp:HiddenField ID="hifBOMUoM_Qty" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "MaterialMaster_BOMUoMID").ToString() %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                       

                                            <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:literal id="ltBOMQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BOMQuantity").ToString() %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvbomQuantity" runat="server" ValidationGroup="UpdateGridItems" ControlToValidate="txtbomQuantity" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox ID="txtbomQuantity" onKeyPress=" return checkDec(event)" Width="80" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BOMQuantity").ToString() %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkisdeleted" runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>

                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="return confirm('Are you sure want to delete?')" Font-Underline="false" OnClick="lnkDelete_Click" Text="Delete" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField CausesValidation="true" ControlStyle-Font-Underline="false" ButtonType="Link" CancelText="Cancel" EditText = "<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton = true UpdateText = "Update" />
                                        </Columns>
                                    </asp:GridView>
                                 
                                </td>
                            </tr>

                        </table>
                </div>
            </td>
        </tr>


       
    </table>


    <asp:HiddenField runat="server" ID="hifBOMHeaderID" />
    <asp:HiddenField runat="server" ID="hifTvBOMChildPartNo" />
    <asp:HiddenField runat="server" ID="hifChildRevMMID" />
    <asp:HiddenField runat="server" ID="hifTvBOMUoM" />
    <asp:HiddenField runat="server" ID="hifRevUoMID" />
    <asp:HiddenField runat="server" ID="hifMTypeID" />
    <asp:HiddenField runat="server" ID="hifTvBOMParentPartNo" />
    <asp:HiddenField runat="server" ID="hifBOMDetailsID" />
    <asp:HiddenField runat="server" ID="hifRTPartNoID" />
    <asp:HiddenField runat="server" ID="hifRTUoMID" />


    <br />
    <br />

    <!-- Routing Revision History  -->

     <div id="divEditRevisionDisputeDlgContainer">  

         <div id="divEditRevisionDispute" style="display:none;">  
         
            <asp:UpdatePanel ID="upnlRevision" runat="server" ChildrenAsTriggers="true" UpdateMode="Always" >
                <ContentTemplate>
                  
                    <br />

                     <div class="ui-dailog-body" style="height:245px;padding-left:10px;padding-right:10px;">

                    <table border="0" cellpadding="3" cellpadding="3" width="100%">
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblRevisionStatus"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ID="rfvtxtNewRevision" runat="server" ValidationGroup="UpdateRev" ControlToValidate="txtNewRevision" Display="Dynamic" ErrorMessage=" * " />
                                Revision: 
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150" ID="txtNewRevision" CssClass="txt_Blue_Small"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ID="rfvtxtEffectedDate" runat="server" ValidationGroup="UpdateRev" ControlToValidate="txtEffectedDate" Display="Dynamic" ErrorMessage=" * " />
                                Effective Date: 
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150" ID="txtEffectedDate" ></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td class="FormLabels">
                                 <asp:RequiredFieldValidator ID="rfvtxtRevRemarks" runat="server" ValidationGroup="UpdateRev" ControlToValidate="txtRevRemarks" Display="Dynamic" ErrorMessage=" * " />
                                Remarks: 
                            </td>
                            <td>
                                  
                                <asp:TextBox ID="txtRevRemarks" onKeypress="return checkSpecialChar(event)" runat="server" Height="105"  Width="350" TextMode="MultiLine"  CssClass="txt_Blue_Small"/>
                            </td>
                        </tr>
                       

                    </table>

                         </div>


                    <div class="ui-dailog-footer" >
                            <div style="padding: 15px 13px 15px 5px;">

                            
                                <asp:LinkButton runat="server" ID="lnkCreateNewRevision"  ValidationGroup="UpdateRev"  OnClick="lnkCreateNewRevision_Click" CssClass="ui-btn ui-button-large">
                                    Create New Revision<%=MRLWMSC21Common.CommonLogic.btnfaSave%></asp:LinkButton>
                            
                                </div>
                            </div>


                </ContentTemplate>
            </asp:UpdatePanel>

         </div>
     </div>

    <!-- Routing Revision History  -->

</asp:Content>
