<%@ Page Title=" Receiving Tally Report :." Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="ReceiveTallyReport.aspx.cs" Inherits="MRLWMSC21.mInbound.ReceiveTallyReport" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="IBContent" runat="server">
<asp:ScriptManager runat="server" ID="smngrDPN" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>
    <script type="text/javascript" src="../mManufacturingProcess/Scripts/ben_Print.js"></script>
<%--   Commented ON 01-JAN-2019 BY Prasad     <script src="../mMaterialManagement/Scripts/BrowserPrint-1.0.4.js"></script>
    <script src="../mMaterialManagement/Scripts/DevDemo.js"></script>--%>
      <script type="text/javascript">
          $('.BarCodeCell').css("width", "350px");

          Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
          function EndRequestHandler(sender, args) {
              if (args.get_error() == undefined) {
                  fnLoadMCode();
              }
          }

          function fnLoadMCode() {

              $(document).ready(function () {
                  $('.BarCodeCell').css("min-width", "340px");

                  $('.module_login').css({
                      "border-color": "#1a79cf",
                      "border-width": "0px",
                      "border-style": "solid"
                  });

                  var textfieldname = $("#<%= this.txtMcode.ClientID %>");
                  DropdownFunction(textfieldname);
                  $("#<%= this.txtMcode.ClientID %>").autocomplete({
                      source: function (request, response) {
                          $.ajax({
                              url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRTRMCodes") %>',
                        data: "{ 'prefix': '" + request.term + "', 'InboundID': '" + '<%= ViewState["InboundID"] %>' + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "" || data.d == "/") {
                                showStickyToast(false, 'No \'Material \' is available for this Shipment');
                            }
                            else
                            response(data.d)
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                      minLength: 0
                  });

              });

              
        }
        fnLoadMCode();
    </script>

    <script type="text/javascript">

        function printDiv(divName) {
            debugger;
            //$(".PrintListcontainer").print();
            var Gcount = '<%=this.gvPOLineQty.Rows.Count%>';
            var time = 100;
            if (Gcount > 100)
            { time = 75 } else
                if (Gcount > 500)
                { time = 50 }
            var panel = document.getElementById("<%=PrintPanel.ClientID %>");
            //var printWindow = window.open('', '', 'height=400,width=800,scrollbars=1,location=1,status=1,');
            var printWindow = window.open('', '', 'height=400,width=800,scrollbars=1,location=1,status=1,resizable=1');
            printWindow.document.write('<html><head><title>Receive Tally Report</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.write('<LINK href="../PrintStyle.css"  type="text/css" rel="stylesheet" media="print">');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print(); 
                printWindow.close();
 
            }, time*Gcount);


        }
    </script>

    <script language="javascript" type="text/JavaScript">
        function check_uncheck(Val) {
            var ValChecked = Val.checked;
            var ValId = Val.id;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for Header Template's Checkbox
                //As we have not other control other than checkbox we just check following statement
                if (this != null) {

                    if (ValId.indexOf('CheckAll') != -1) {

                        // Check if main checkbox is checked,
                        // then select or deselect datagrid checkboxes
                        if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('chkIsPrint') != -1) {
                            if (ValChecked)
                                frm.elements[i].checked = true;
                            else
                                frm.elements[i].checked = false;
                        }
                    }
                    else if (ValId.indexOf('chkIsPrint') != -1) {
                        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                        if (frm.elements[i].checked == false)
                            frm.elements[1].checked = false;
                    }
                } // if
            } // for
        } // function


        $(document).ready(function () {
            var textfieldname = $("#<%= this.txtMcode.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.txtMcode.ClientID %>").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRTRMCodes") %>',
                              data: "{ 'prefix': '" + request.term + "', 'InboundID': '" + '<%= ViewState["InboundID"] %>' + "' }",
                              dataType: "json",
                              type: "POST",
                              contentType: "application/json; charset=utf-8",
                              success: function (data) {
                                  if (data.d == "" || data.d == "/") {
                                      showStickyToast(false, 'No \' Material \' is available for this Shipment');
                                  }
                                  else
                                  response(data.d)
                              },
                              error: function (response) {
                                  alert(response.responseText);
                              },
                              failure: function (response) {
                                  alert(response.responseText);
                              }
                          });
                      },
                      minLength: 0
                  });

        });


       
    </script>



    <style>
        .module_login {
            border: 0px solid #1a79cf;
        }
        #MainContent_IBContent_gvPOLineQty {
            overflow: hidden;
        }

        a.ButEmpty{
            color:#0e0e0e !important;
        }

        select, input {
            width:100% !important;
        }

        #MainContent_IBContent_pnlSearchMaterial {
            width:100% !important;
        }


        .NoLeftBorder td {
            padding: 9px 10px 9px 0px !important;
                border-bottom: 1px solid var(--paper-grey-300);
        }
        td{
            padding:5px;
        }
    </style>



  <div class="loaderforCurrentStock" style="display:none;">
             <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                 <div style="align-self: center;">
                     <div class="spinner">
                         <div class="bounce1"></div>
                         <div class="bounce2"></div>
                         <div class="bounce3"></div>
                     </div>

                 </div>

             </div>

         </div>


<div class="dashed"></div>

<div class="pagewidth">
    <div id="printArea" class="PrintListcontainer">
    <%-- <LINK href="../PrintStyle.css"  type="text/css" rel="stylesheet" media="print">--%>
        <asp:Panel runat="server" ID="PrintPanel">
            <table border="0" cellpadding="0" cellspacing="0" align="center" width="100%" id="tdDDRPrintArea"  >

                                                                    <!-- Start Pending Area  -->
            <thead>
                <tr>
                  <th align="center">
                      <asp:Label runat="server" Text="Receiving Tally Report" ID="lblHeader" CssClass="SubHeading3" /> 
                  </th> 
                    <th align="right" valign="bottom">
                        
                        <div class="right"><img width="20" height="20" alt="Print" style="visibility:hidden;" class="NoPrint" src="../Images/blue_menu_icons/printer.png" onclick="javascript:printDiv('tdDDRPrintArea');" border="0" style="cursor:pointer;" />

                        <asp:LinkButton runat="server" ID="lnkbackToList" CssClass="btn btn-primary NoPrint" PostBackUrl = "../mInbound/InboundTracking.aspx" ><i class="material-icons">keyboard_backspace</i>Back to List</asp:LinkButton>
                            &nbsp;&nbsp; &nbsp;&nbsp;<a href="#" style="text-decoration:none;" onclick="javascript:printDiv('tdDDRPrintArea');" class="btn btn-primary NoPrint">Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %></a>&nbsp; <%--<img src="../Images/redarrowright.gif"  border="0" />--%>
                        </div>
                    </th>
                </tr>
            </thead>
            
            <tfoot> 
                <tr><td colspan="3"> &nbsp;</td></tr>
                <tr> 
                    <td width="100%" colspan="3"  align="right">&nbsp;</td>
                </tr>
            </tfoot>

                                                                    <!-- End Pending Area  -->

            <tbody>
                <tr>
                    <td colspan="3"><hr /></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table cellpadding="2" cellspacing="2" border="0" width="100%">
                            
                            <tr style="box-shadow: var(--z1)">
                                <td  class="FormLabels" valign="middle" width="35%"  >

                                     <b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Store Ref. #:
                                    
                                    <asp:Literal runat="server" ID="ltbarStoreRefNo" /></b>
                                </td>
                                <td valign="top" class="FormLabels" colspan="2" align="right">
                                    <table cellpadding="7" cellspacing="1" border="0" width="100%" align="right" class="indp__">
                                        <tr>
                                            <td style="width:18%;">
                                                <strong>Store <span class="pull-right">:</span></strong>
                                            </td>
                                            <td>
                                                <div><asp:Literal runat="server" ID="ltbarWarehouse" /></div>
                                            </td>
                                            <td style="width:18%;">
                                                <strong>Tenant <span class="pull-right">:</span></strong>
                                            </td>
                                            <td>
                                                <div><asp:Literal runat="server" ID="ltTenant" /></div>
                                            </td>
                                            <%--<td style="width:50%">
                                                <div class="flex__">
                                                    <h5>Store <span class="pull-right">:</span></h5>
                                                    &nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltbarWarehouse" /></div>
                                                </div>
                                            </td>
                                       
                                            <td>
                                                <div class="flex__">
                                                <h5>Tenant <span class="pull-right">:</span></h5>
                                                &nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltTenant" /></div></div>
                                             </td>--%>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Doc. Date <span class="pull-right">:</span></strong>
                                            </td>
                                            <td>
                                                <div><asp:Literal runat="server" ID="ltDocDate" /></div>
                                            </td>
                                            <td>
                                                <strong>Supplier <span class="pull-right">:</span></strong>
                                            </td>
                                            <td>
                                                <div><asp:Literal runat="server" ID="ltSupplier" /></div>
                                            </td>
                                            <%--<td>
                                                <div class="flex__">
                                                    <h5>Doc. Date <span class="pull-right">:</span></h5>
                                                    &nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltDocDate" /></div>
                                                </div>
                                            </td>
                                      
                                            <td>
                                                <div class="flex__">
                                                    <h5>Supplier <span class="pull-right">:</span></h5>
                                                    &nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltSupplier" /></div>
                                                </div>
                                            </td>--%>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>AWB/BL# <span class="pull-right">:</span></strong>
                                            </td>
                                            <td>
                                                <div><asp:Literal runat="server" ID="ltAWBBLNo" /></div>
                                            </td>
                                            <td>
                                                <strong>No. of Pkgs <span class="pull-right">:</span></strong>
                                            </td>
                                            <td>
                                                <div><asp:Literal runat="server" ID="ltNoofPackages" /></div>
                                            </td>
                                            <%--<td>
                                                <div class="flex__">
                                                    <h5>AWB/BL# <span class="pull-right">:</span></h5>
                                                 &nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltAWBBLNo" /></div></div></td>
                                        
                                            <td>
                                                <div class="flex__">
                                                    <h5>No.of Pkgs <span class="pull-right">:</span></h5>
                                                     &nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltNoofPackages" /></div>
                                                </div>
                                            </td>--%>
                                        </tr>
                                       
                                    </table>
                                </td>
                            </tr>
                       
                            </table>
                        </td>
                    </tr>
                     <tr>
                                <td colspan="3">
                                    <div class="row">
                                        <div class=" col m3 ">
                                            <div class="flex ">
                                                <asp:DropDownList ID="ddlNetworkPrinter" required="" runat="server" />
                                                <label>Label Printer</label>

                                            </div>
                                        </div>
                                        <div class="col m3">
                                            <div class="flex ">
                                                <asp:DropDownList ID="ddlLabelSize" runat="server" required=""></asp:DropDownList>
                                                <label>Label Size</label>
                                            </div>
                                        </div>
                                        <div class=" col m3">
                                            <asp:Panel ID="pnlSearchMaterial" runat="server" DefaultButton="lnkSearchMaterial">
                                                <div class="flex">
                                                    <asp:TextBox ID="txtMcode" runat="server" SkinID="txt_Hidden_Req_Auto" onfocus="ClearText(this)" required="" onblur="javascript:focuslost1(this)" />
                                                    <label>Search Part# ...</label>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <div class="col m3">
                                            <div class="flex col m1 NoPrint">
                                                <asp:LinkButton ID="lnkSearchMaterial" runat="server" OnClick="lnkSearchMaterial_Click" CssClass="btn btn-primary NoPrints" Style="color: white"> Search <span class="space fa fa-search"></span></asp:LinkButton></div>
                                        </div>
                                    </div>
                                           
                                </td>
                            </tr>

                <tr>
                    <td colspan="3">
                        <table border="0">
                            <tr>
                                <td colspan="3">
                                    <asp:Literal  runat="server" ID="ltPOQuantityStatus" />
                                    <asp:GridView Width="100%" EnableViewState="false" ShowFooter="true" CellPadding="2" CellSpacing="2"  GridLines="None" CssClass="table-striped"  ID="gvPOLineQty" runat="server"  AutoGenerateColumns="False" AllowPaging="false" PageSize="25" AllowSorting="True" HorizontalAlign="Left"   OnSorting="gvPOLineQty_Sorting"  OnRowDataBound="gvPOLineQty_RowDataBound" >
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="4%" HeaderText="Line#" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltLineNumber"  Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber","{0:000}") %>'  />
                                                </ItemTemplate>
                                            </asp:TemplateField>  
                                            <asp:BoundField DataField="KitPlannerID" HeaderText="KitPlannerID" SortExpression="KitPlannerID" Visible="false" />
                                            <asp:BoundField DataField="ParentMcode" HeaderText="Parent Part#" SortExpression="ParentMcode" Visible="false" />
                                          
                                            <asp:TemplateField ItemStyle-Width="15%" ItemStyle-CssClass ="" HeaderText="Part # (SKU)"  HeaderStyle-HorizontalAlign="Center" >
                                                <ItemTemplate>
                                                        <asp:Literal runat="server"  ID="lblMCode" Text='<%#DataBinder.Eval(Container.DataItem, "MCode") %>'/>
                                                   <span style="color:darkblue;font-size:7pt" class="ng-binding"> [<asp:Literal runat="server" ID="ltOEMPartNo"  Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>'/>]</span>
                                                    <br />
                                                        <span style="color:#2196F3;font-size:8pt" class="ng-binding">
                                                            <asp:Literal runat="server" ID="ltItemDesc" Text='<%# DataBinder.Eval(Container.DataItem, "MDescription") %>'/>
                                                        </span>
                                                    <%--<asp:Label runat="server" CssClass="BarCodetext" ID="lblMCode" Text='<%#String.Format("{0} / {1}",MRLWMSC21Common.CommonLogic.QueryString("TN"), DataBinder.Eval(Container.DataItem, "MCode")) %>'/>--%>&nbsp;&nbsp;
                                                    
                                                    <%--<asp:Literal runat="server" ID="ltBarCodeMCode" Text='<%# String.Format("<img width=\"330px\" src=\"Code39Handler.ashx?code={0}/{1} \"",DataBinder.Eval(Container.DataItem, "TenantID"),DataBinder.Eval(Container.DataItem, "MCode")) %>'/>&nbsp;&nbsp;&nbsp;&nbsp;<br /><br />--%>
                                                   <div class="img-alignment"> <asp:Literal runat="server" ID="Literal1" Text='<%# String.Format("<img width=\"250px\" src=\"Code39Handler.ashx?code={0} \"",DataBinder.Eval(Container.DataItem, "MCode")) %>'/>&nbsp;&nbsp;&nbsp;&nbsp;<br /><br /></div>
                                                    <asp:Literal runat="server" ID="ltAlternativeCode"  Text='<%# DataBinder.Eval(Container.DataItem, "MCodeAlternative1") %>'/>
                                                    
                                                    
                                                    <asp:Literal runat="server" ID="ltCompanyName" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>'/>
                                                    <asp:Literal runat="server" ID="ltPOQuantityID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "POHeaderID") %>'/>
                                                    <asp:Literal runat="server" ID="ltKitID" Visible="false"  Text='<%# DataBinder.Eval(Container.DataItem, "KitPlannerID") %>'/>
                                                    <asp:Literal runat="server" ID="ltKitChildCount" Visible="false"  Text='<%# DataBinder.Eval(Container.DataItem, "KitChildCount") %>'/>
                                                    <asp:Literal runat="server" ID="ltParentMcode" Visible="false" Text=''/>
                                                </ItemTemplate>
                                            </asp:TemplateField>  

                                              <asp:TemplateField ItemStyle-Width="8.5%" HeaderText="Kit ID" HeaderStyle-HorizontalAlign="Left"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltKitID1" Visible="false"  Text='<%# DataBinder.Eval(Container.DataItem, "KitPlannerID") %>'/>
                                                       <asp:Literal runat="server" ID="ltKitID2"   Text='<%# DataBinder.Eval(Container.DataItem, "KitCode") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>  

                                             <asp:TemplateField ItemStyle-Width="8.5%" HeaderText="PO #/Invoice" HeaderStyle-HorizontalAlign="Left"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltPONumber" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber") %>'/>
                                                    <asp:Literal runat="server" ID="ltInvoiceNumber" Text='<%#string.Format(" / {0}",  DataBinder.Eval(Container.DataItem, "InvoiceNumber") )%>'/>

                                                </ItemTemplate>
                                            </asp:TemplateField>  

                                            <asp:TemplateField ItemStyle-Width="7.5%" HeaderText="Last Rcvd. Location" Visible="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltStockType" Text=' <%# DisplayLocation(DataBinder.Eval(Container.DataItem, "Location").ToString(),"<br/>") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="7.5%" HeaderText="Mfg. Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltmfgdate" Text=' <%# DisplayLocation(DataBinder.Eval(Container.DataItem, "MfgDate").ToString(),"<br/>") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="7.5%" HeaderText="Exp. Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    
                                                    <asp:Literal runat="server" ID="ltexpdate" Text=' <%# DisplayLocation(DataBinder.Eval(Container.DataItem, "ExpDate").ToString(),"<br/>") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                             <asp:TemplateField ItemStyle-Width="7.5%" HeaderText="Batch No." HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"  ItemStyle-CssClass="home" >
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltBatchNo" Text='<%# DisplayLocation(DataBinder.Eval(Container.DataItem, "BatchNo").ToString(),"<br/>") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField ItemStyle-Width="7.5%" HeaderText="Serial No." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSerialNo" Text=' <%# DisplayLocation(DataBinder.Eval(Container.DataItem, "SerialNo").ToString(),"<br/>") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <%-- added by lalitha on 01/03/2019--%>
                                             <asp:TemplateField ItemStyle-Width="7.5%" HeaderText="ProjectRefNo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltprojectRefNo" Text=' <%# DisplayLocation(DataBinder.Eval(Container.DataItem, "ProjectRefNo").ToString(),"<br/>") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- added by lalitha on 01/03/2019--%>
                                             <asp:TemplateField ItemStyle-Width="7.5%" HeaderText="MRP" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltMRP" Text=' <%# DisplayLocation(DataBinder.Eval(Container.DataItem, "MRP").ToString(),"<br/>") %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           
                                           <asp:TemplateField ItemStyle-Width="5%" HeaderText="BUoM/MoP." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltBUoM" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "BUoM").ToString(),DataBinder.Eval(Container.DataItem, "BUoMQty","{0:0.00}").ToString()) %>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>  
                                            <asp:TemplateField ItemStyle-Width="5%" HeaderText="IUoM/MoP." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltPUoM" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "InvUoM").ToString(),DataBinder.Eval(Container.DataItem, "InvUoMQty","{0:0.00}").ToString()) %>'/>
                                                    <asp:Literal ID="ltDynamicDisplayData" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>  

                                             

                                            <asp:TemplateField ItemStyle-Width="7.5%" HeaderText="Qty. Rcvd / Inv. Qty."  HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                     <asp:Literal ID="HyperLink3" Text='<%# String.Format("[{0}/{1}]", DataBinder.Eval(Container.DataItem, "ReceivedQty","{0:0.00}"),DataBinder.Eval(Container.DataItem, "InvoiceQuantity","{0:0.00}")) %>'   runat="server"></asp:Literal>
                                                     <asp:Literal ID="ltInvQuantity" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceQuantity","{0:0.00}") %>' runat="server"></asp:Literal>
                                                     <asp:Literal ID="ltReceivedQuantity" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ReceivedQty","{0:0.00}") %>' runat="server"></asp:Literal>
                                                     
                                                </ItemTemplate>
                                            </asp:TemplateField> 

                                             <asp:TemplateField ItemStyle-Width="5%" HeaderText="Receive"   ItemStyle-CssClass="home NoPrint" ControlStyle-CssClass ="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" >
                                                <ItemTemplate>
                                                  <asp:Literal runat="server" ID="ltQCCaptured" Visible="false" Text=""/>
                                                      <div class="helpWTitle" style="position:relative">  <asp:HyperLink ID="lnkReceiveItem" Text="Receive" ForeColor="#3399ff" CssClass="ButEmpty" NavigateUrl='<%# String.Format("../mInventory/GoodsIn.aspx?ibdno={0}&mmid={1}&lno={2}&PODH={3}&SIID={4}",DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString(),DataBinder.Eval(Container.DataItem, "LineNumber").ToString(),DataBinder.Eval(Container.DataItem, "POHeaderID").ToString(),DataBinder.Eval(Container.DataItem, "SupplierInvoiceID").ToString()) %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                                    <img  src="../Images/redarrowright.gif" /></div>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                          


                                             <asp:TemplateField ItemStyle-Width="5%" ItemStyle-CssClass="NoPrint" HeaderStyle-HorizontalAlign="Center"  ControlStyle-CssClass ="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint"   HeaderText="Print Labels"  ItemStyle-HorizontalAlign="Center" >
                                                 <HeaderTemplate>
                                                     <asp:Label ID="lblPrintLabel" Text="Print Label" runat="server" CssClass="NoPrint" /><br />
                                                     <div class="checkbox hide-next-check"><asp:CheckBox ID="CheckAll" onclick="return check_uncheck (this );" runat="server" CssClass="NoPrint" /><label for=""></label></div><br>
                                                 </HeaderTemplate>
                                                 <ItemTemplate>
                                                     <div class="checkbox hide-next-check"><asp:CheckBox ID="chkIsPrint" runat="server" CssClass="NoPrint"/><label for=""></label></div>
                                                 </ItemTemplate>
                                                 <FooterTemplate>
                                                     <div style="text-align: center;"><asp:LinkButton ID="lnkPrintBarCodeLabel" ClientIDMode="Static" runat="server" Text="<nobar><i class='material-icons vl'>print</i></nobar>" CssClass="ButEmpty" CausesValidation="false" OnClick="lnkPrintBarCodeLabel_Click"   /></div>
                                                     
                                                 </FooterTemplate>
                                             </asp:TemplateField>   
                                        </Columns>
                                        <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;" Mode="NumericFirstLast" PageButtonCount="15" />
                                        <FooterStyle CssClass="gvSilver_footerGrid" /><RowStyle CssClass="gvSilver_DataCellGrid" /><EditRowStyle CssClass="gvSilver_DataCellGridEdit" /><PagerStyle CssClass="gvBlue_pager" /><HeaderStyle CssClass="gvSilver_headerGrid" /><AlternatingRowStyle CssClass="gvSilver_DataCellGridAlt" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>

        </table>
        </asp:Panel>  
    </div>

</div>

     <!-- /navigation -->
    <div class="container" style="width: 500px;display:none;">
        <div id="main">
            <div id="printer_data_loading" style="display: none">
                <span id="loading_message">Loading Printer Details...</span><br />
                <div class="progress" style="width: 100%">
                    <div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%">
                    </div>
                </div>
            </div>
            <!-- /printer_data_loading -->
            <div style="display:none;">
            <div id="printer_details" style="display: none">
                <span id="selected_printer">No data</span>
                <button type="button" class="btn btn-success" onclick="changePrinter()">Change</button>
            </div></div>
            <br />
            <!-- /printer_details -->
            <div class="row" style="display:none;">
                <div class="col m3">
                   
                    <!-- /printer_select -->
                </div>
                <div class="col m3" >
               
                    <div id="print_form" style="display: none" class="flex">
                        
                <input type="text" id="entered_name" />
                        <label>ZPL String</label>                        
                    </div>                    
                    <!-- /print_form -->
                </div>

                <div class="col m4">
                    <button type="button" class="btn btn-lg btn-primary" onclick="sendData();" value="Print">Print Label</button>
                </div>
            </div>



        </div>
        <!-- /main -->
        <div id="error_div" style="width: 500px; display: none">
            <div id="error_message"></div>
            <button type="button" class="btn btn-lg btn-success" onclick="trySetupAgain();">Try Again</button>
        </div>
        <!-- /error_div -->
    </div>
    <!-- /container -->

    <script type="text/javascript">
        $(document).ready(setup_web_print);

        //window.onload = function (e) {
        //    debugger;
        //     setup_web_print();
        //     }
    </script>

    <script>
        //function load(event) 
        //{
        //    event.stopPropagation();
        //}

         function PrintRTR_ZPL(zplString)
         {
             debugger;
             var zplData = "";
             var sta = zplString.includes("XYZ786XYZ");
             if (sta == true) {
                 //zplData = zplString.replace("***XYZ786XYZ***","\r\n");
                 zplData = zplString.split('XYZ786XYZ').join('\r\n')
             }
             else
             {
                 zplData = zplString;
             }

             $(".loaderforCurrentStock").show();

             $(document).ready(setup_web_print);

             //$(window).on('load',setup_web_print);
            //alert(zplString);
            debugger;
            $("#entered_name").val("");
            $("#entered_name").val(zplData);

            setTimeout(function () {
                debugger;
                sendData();
                 $(".loaderforCurrentStock").hide();
                showStickyToast(true, "Successfully Printed");
                return false;
            }, 5000);
           
        }

    </script>

</asp:Content>
