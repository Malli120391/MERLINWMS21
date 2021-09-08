<%@ Page Title=".: RCR :." Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="ReceiveConfirmationReport.aspx.cs" Inherits="MRLWMSC21.mInbound.ReceiveConfirmationReport" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="IBContent" runat="server">
    <asp:ScriptManager runat="server" ID="smngrDPN" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>
    <script type="text/javascript" src="../mManufacturingProcess/Scripts/ben_Print.js"></script>

    <script type="text/javascript">
        $('.BarCodeCell').css("width", "350px");
        function ClearText(TextBox) {
            if (TextBox.value == "Search Part# ...")
                TextBox.value = "";

            TextBox.style.color = "#000000";
        }

        function focuslost1(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search Part# ...";

            TextBox.style.color = "#A4A4A4";
        }

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

        $(document).ready(function () {            
            CalculateTotals();
        });

        function CalculateTotals() {
            var TotalRcvdQty = 0;
            $('.lblRcvdQty').each(function () {
                var thisQty = $(this).text().trim() == "" ? "0" : $(this).text().trim();
                TotalRcvdQty = eval(TotalRcvdQty) + eval(thisQty);
            });
            $('.lblRcvdQtyTotal').text(TotalRcvdQty.toFixed(2));

            var TotalExpQty = 0;
            $('.lblExpQty').each(function () {
                var thisQty = $(this).text().trim() == "" ? "0" : $(this).text().trim();
                TotalExpQty = eval(TotalExpQty) + eval(thisQty);
            });
            $('.lblExpQtyTotal').text((TotalExpQty).toFixed(2));

            var TotalRcptQty = 0;
            $('.lblRcptQty').each(function () {
                var thisQty = $(this).text().trim() == "" ? "0" : $(this).text().trim();
                TotalRcptQty = eval(TotalRcptQty) + eval(thisQty);
            });
            $('.lblRcptQtyTotal').text(TotalRcptQty.toFixed(2));

            var TotalDmgdQty = 0;
            $('.lblDmgdQty').each(function () {
                var thisQty = $(this).text().trim() == "" ? "0" : $(this).text().trim();
                TotalDmgdQty = eval(TotalDmgdQty) + eval(thisQty);
            });
            $('.lblDmgdQtyTotal').text(TotalDmgdQty.toFixed(2));

            var TotalExsQty = 0;
            $('.lblExsQty').each(function () {
                var thisQty = $(this).text().trim() == "" ? "0" : $(this).text().trim();
                TotalExsQty = eval(TotalExsQty) + eval(thisQty);
            });
            $('.lblExsQtyTotal').text(TotalExsQty.toFixed(2));

            var TotalShtQty = 0;
            $('.lblShtQty').each(function () {
                var thisQty = $(this).text().trim() == "" ? "0" : $(this).text().trim();
                TotalShtQty = eval(TotalShtQty) + eval(thisQty);
            });
            $('.lblShtQtyTotal').text(TotalShtQty.toFixed(2));

            var TotalVolume = 0;
            $('.lblVolume').each(function () {
                var thisQty = $(this).text().trim() == "" ? "0" : $(this).text().trim();
                TotalVolume = eval(TotalVolume) + eval(thisQty);
            });
            $('.lblVolumeTotal').text(TotalVolume.toFixed(3));


        }

        

        function printDiv(divName) { // RCR

            var Gcount = '<%=this.gvPOLineQty.Rows.Count%>';
            var time = 100;
            if (Gcount > 100) {
                time = 75
            }
            else if (Gcount > 500) {
                time = 50
            }
            var panel = document.getElementById("<%=PrintPanel.ClientID %>");
             var printWindow = window.open('', '', 'location=0, status=0, resizable=1, scrollbars=1, width=800, height=400');
            //printWindow.document.write('<html><head><title>Delivery Pick Note</title>'); //margin-top:80px; margin-bottom:50px;margin-left:5px;margin-right:5px;
             printWindow.document.write('<LINK href="../PrintStyle.css"  type="text/css" rel="stylesheet" media="print">');
             printWindow.document.write('<style type="text/css"> @page { size: landscape; margin:0;}  @media print { @page{ size: landscape;}  #tdDDRPrintArea{ }   div.divFooter {display:block;position: relative;margin-left:75px;} div.divFooterLogo {display:block;position: fixed;bottom: 0px;right:0px;} .tdPickLink { display:none; } .tblRCR {font-size:10pt;} .gvSilver_footerGrid { text-align:center; color:black;  font-weight:bold;} .removeRightBorder { border-right:0; } .removeLeftBorder {border-left:0;}   }</style>');
             //printWindow.document.write('<style type="text/css"> @page { size: portrait; margin-top:80px; margin-bottom:80px;}  @media print {    @page{ size: portrait;}    #tdDDRPrintArea{ position:relative; display:table;table-layout:fixed; width: 100%;height:auto;}   div.divFooter {display:block;position: relative;margin-left:50px;} div.divFooterLogo {display:block;position: fixed;bottom: 0px;right:0;}  .tdPickLink { display:none; } .spanFooterNote {display:none;}     )</style>');
             printWindow.document.write('<script src="../Scripts/jquery-1.8.2.min.js"><\/script><script>   $(document).ready(function(){  });<\/script>');
             printWindow.document.write('</head><body >');
             printWindow.document.write(panel.innerHTML);
             printWindow.document.write('</body></html>');
             printWindow.document.close();
             setTimeout(function () {
                 printWindow.print();// printWindow.close();
                 printWindow.close();
             }, time * Gcount);
         }
         //var p = $(".divFooter"); var position = p.position(); var top = position.top; var pages=' + Gcount / 15 + '; if(pages<1){pages=1;} $(".divFooter").css("top", pages.toFixed(0)*100); 
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

        .cssBoxText {
            border: 1px solid black;
            width: 70px;
            height: 30px;
        }
        .BarCodeCell {
            min-width:270px !important;
        }
        /*@page { size: portrait !important; }*/
        .tblRCR, .tblRTS {
            /*font-size:10.5pt;*/
        }
        .gvSilver_footerGrid {
            text-align:center;
            color:black;
            font-weight:bold;
        }
        .removeRightBorder {
            border-right:0;
        }
        .removeLeftBorder {
            border-left:0;
        }
        .lblHeading {
            font-size:14pt;
        }
    </style>

     <style type="text/css">
    @media screen {
        div.divFooter, div.divFooterLogo {
            display: none;
        }
         .tblTotals, .divRemarks {
         display:none;
         }
    }
    @media print {
        div.divFooter, div.divFooterLogo {
            display:block;
            position: fixed;
            bottom: 0;
            right:0;
        }   
        
    }
   </style>
    <asp:HiddenField ID="hdnStatus" runat="server" Value="0" />
    <div id="printArea" class="PrintListcontainer container">
        <%-- <LINK href="../PrintStyle.css"  type="text/css" rel="stylesheet" media="print">--%>
        <asp:Panel runat="server" ID="PrintPanel">
            <table border="0" cellpadding="0" cellspacing="0" align="center" id="tdDDRPrintArea">

                <!-- Start Pending Area  -->
                <thead>
                    <tr>
                        <td style="width:40%;">&nbsp;</td>
                        <td style="width:60%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left">
                            <%--style="visibility:hidden;" --%>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <img id="Img1" runat="server" enableviewstate="false" src="~/Images/inventrax.jpg" border="0" alt="" width="180" height="50">
                        </td>

                        <td align="left">
                            <asp:Label runat="server" Text="Receipt Confirmation Report" ID="lblHeader" CssClass="SubHeading3 lblHeading" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      
                        </td>

                    </tr>
                    <tr>
                        <td align="right" style="text-align:right;" valign="bottom" colspan="2">

                            <img width="20" height="20" alt="Print" style="visibility: hidden;" class="NoPrint" src="../Images/blue_menu_icons/printer.png" onclick="javascript:printDiv('tdDDRPrintArea');" border="0" style="cursor: pointer;" />

                            <asp:LinkButton runat="server" ID="lnkbackToList" CssClass="btn btn-primary NoPrint" PostBackUrl="../mInbound/InboundTracking.aspx">Back to List<%=MRLWMSC21Common.CommonLogic.btnfaList %></asp:LinkButton>
                            &nbsp;&nbsp; &nbsp;&nbsp;<a href="#" style="text-decoration: none;" onclick="javascript:printDiv('tdDDRPrintArea');" class="btn btn-primary NoPrint printRCR">Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %></a>&emsp;&emsp;

                        </td>
                    </tr>
                </thead>

               

                <!-- End Pending Area  -->

                <tbody>
                    <tr>
                        <td colspan="2">
                            <hr style="height: 0.5px; color: #000; border-color: #000; background-color: #000;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table cellpadding="2" cellspacing="2" border="0" width="100%" style="font-size:10.5pt;">

                                <tr>
                                    <td valign="top" width="60%">

                                        <b>&emsp;Receipt ID : 
                                    
                                    <asp:Literal runat="server" ID="ltbarStoreRefNo" /></b>
                                        <br />
                                        <label>&emsp;<b>Doc. Date : </b></label>
                                        <asp:Label runat="server" ID="ltDocDate" style="font-size:10pt;" ></asp:Label>
                                    </td>
                                    <td valign="top" class="FormLabels" colspan="2" align="right">
                                        <table style="display: none;" cellpadding="0" cellspacing="0" border="0" width="100%" align="right">
                                            <tr>
                                                <td width="150">Customer &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: </td>
                                                <td>
                                                    <asp:Literal runat="server" ID="ltTenant" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="110">Warehouse &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: </td>
                                                <td>
                                                    <asp:Literal runat="server" ID="ltbarWarehouse" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Supplier &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: </td>
                                                <td>
                                                    <asp:Literal runat="server" ID="ltSupplier" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>AWB/BL# &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: </td>
                                                <td>
                                                    <asp:Literal runat="server" ID="ltAWBBLNo" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <nobr>No.of Pkgs &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</nobr>
                                                </td>
                                                <td>
                                                    <asp:Literal runat="server" ID="ltNoofPackages" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NoPrint">
                                                    <asp:Label ID="lblNetworkPrinter" runat="server" Text="Label Printer"></asp:Label>
                                                    <nobr>&nbsp;&nbsp;&nbsp;&nbsp;:</nobr>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlNetworkPrinter" runat="server" CssClass="NoPrint" /></td>
                                            </tr>
                                            <tr>
                                                <td class="NoPrint">
                                                    <asp:Label ID="lblLabelSize" runat="server" CssClass="NoPrint" Text="Label Size" />
                                                    <nobr>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</nobr>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rdlLabelSize" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" CssClass="NoPrint">
                                                        <asp:ListItem Text="Small (2 x 1 inch)" Value="Big" Selected="True"> </asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td colspan="2" class="NoPrint">
                                                    <asp:Panel ID="pnlSearchMaterial" runat="server" Visible="false" DefaultButton="lnkSearchMaterial">

                                                        <asp:TextBox ID="txtMcode" runat="server" SkinID="txt_Hidden_Req_Auto" onfocus="ClearText(this)" PlaceHolder="Search Part# ..." onblur="javascript:focuslost1(this)" Width="160" />
                                                        &nbsp;
                                                    <asp:LinkButton ID="lnkSearchMaterial" runat="server" OnClick="lnkSearchMaterial_Click" CssClass="ui-btn ui-button-large"> Search <span class="space fa fa-search"></span></asp:LinkButton>

                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>                                      

                                        <table class="tblRCR" style="width:100%;">     
                                            <tr>
                                                <td valign="top"><b>Customer Name</b></td>
                                                <td>:
                                                    <asp:Literal runat="server" ID="LtCustomerName" Text="&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top"><b>Address</b></td>
                                                <td>:
                                                    <asp:Literal runat="server" ID="LtAddress" />
                                                </td>
                                            </tr>    
                                            <tr style="display:none;">
                                                <td >Supplier Details </td>
                                                <td>:
                                                    <asp:Literal ID="LtSupplierRCR" runat="server" />
                                                </td>
                                            </tr>                                                                         
                                            <tr>
                                                <td><b>Shipment Type</b> </td>
                                                <td>:
                                                    <asp:Literal ID="LtShipmentType" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top"><b>Truck No./Trailer ID. </b></td>
                                                <td>:
                                                    <asp:Literal runat="server" ID="LtTruckOrTrailerIDRCR" />
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>                       
                                    <td valign="top" align="center" colspan="2">
                                        <asp:Literal runat="server" ID="ltPOQuantityStatus" /><br />
                                        <asp:GridView Width="100%" EnableViewState="false" ShowFooter="true" CellPadding="2" CellSpacing="0" GridLines="Both" CssClass="NoLeftBorder" ID="gvPOLineQty" runat="server" AutoGenerateColumns="False" AllowPaging="false" PageSize="25" AllowSorting="True" HorizontalAlign="Left" OnSorting="gvPOLineQty_Sorting" OnRowDataBound="gvPOLineQty_RowDataBound" FooterStyle-HorizontalAlign="Center">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="130px" HeaderText="Item Code" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="home_">
                                                    <ItemTemplate>                                                        
                                                         <asp:Label runat="server" CssClass="BarCodetext" ID="lblMCode" Text='<%#DataBinder.Eval(Container.DataItem, "MCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate></FooterTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:BoundField DataField="KitPlannerID" HeaderText="KitPlannerID" SortExpression="KitPlannerID" Visible="false" />
                                                <asp:BoundField DataField="ParentMcode" HeaderText="Parent Part#" SortExpression="ParentMcode" Visible="false" />
                                                
                                                <asp:TemplateField ItemStyle-Width="100px" ItemStyle-CssClass="BarCodeCell" HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>                                                       
                                                        <div style="font-size: 10pt;">
                                                            <asp:Literal runat="server" ID="ltItemDesc" Text='<%# DataBinder.Eval(Container.DataItem, "MDescription") %>' />
                                                        </div>                                                       
                                                    </ItemTemplate>
                                                    <FooterTemplate>Total</FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false" ItemStyle-Width="80" HeaderText="PO # / Invoice No." HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home_">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltPONumber" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber") %>' />
                                                        <asp:Literal runat="server" ID="ltInvoiceNumber" Text='<%#string.Format(" / {0}",  DataBinder.Eval(Container.DataItem, "InvoiceNumber") )%>' />

                                                    </ItemTemplate>
                                                    <FooterTemplate></FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false" ItemStyle-Width="160" HeaderText="Last Rcvd. Location" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="home_">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltStockType" Text=' <%# DisplayLocation(DataBinder.Eval(Container.DataItem, "Location").ToString(),"<br/>") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate></FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="UoM" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="home_">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltBUoM" Text='<%# String.Format("{0}",DataBinder.Eval(Container.DataItem, "InvUoM").ToString()) %>' />
                                                   
                                                    </ItemTemplate>
                                                   
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-Width="80" HeaderText="Expected Qty." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="home_ removeRightBorder" HeaderStyle-CssClass="removeRightBorder" FooterStyle-CssClass="removeRightBorder">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" CssClass="lblExpQty" ID="ltPUoM" Text='<%# String.Format("{0}",DataBinder.Eval(Container.DataItem, "POQuantity").ToString()) %>' />
                                                        <%-- <asp:Literal ID="ltDynamicDisplayData" runat="server" />--%>
                                                    </ItemTemplate>  
                                                     <FooterTemplate>
                                                        <asp:Label ID="lblExpQtyTotal" runat="server"  class="lblExpQtyTotal"></asp:Label>
                                                    </FooterTemplate>                           
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-Width="0" HeaderStyle-Width="0" ItemStyle-CssClass="home_ removeLeftBorder" HeaderStyle-CssClass="removeLeftBorder" FooterStyle-CssClass="removeLeftBorder">
                                                     <ItemTemplate>                                                       
                                                         <asp:Literal ID="ltDynamicDisplayData" runat="server" />
                                                    </ItemTemplate>  
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-Width="70" HeaderText="Rcvd. Qty." HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="home_" ItemStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="HyperLink3" CssClass="lblRcvdQty" Text='<%# String.Format("{0}", DataBinder.Eval(Container.DataItem, "ReceivedQty","{0:0.00}")) %>' runat="server"></asp:Label>
                                                        <asp:Literal ID="ltInvQuantity" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceQuantity","{0:0.00}") %>' runat="server"></asp:Literal>
                                                        <asp:Literal ID="ltReceivedQuantity" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ReceivedQty","{0:0.00}") %>' runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                   <FooterTemplate>
                                                       <asp:Label ID="Label30"  runat="server" class="lblRcvdQtyTotal"></asp:Label>
                                                   </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField ItemStyle-Width="70" HeaderText="Good Qty." HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="home_" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>                                                       
                                                            <asp:Label ID="txtReceiptQty" CssClass="lblRcptQty" runat="server" Text='<%#Eval("GoodQty") %>'></asp:Label>                                                   
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="Label31"  runat="server" class="lblRcptQtyTotal"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="Dmgd. Qty." HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="home_" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>                                                        
                                                            <asp:Label ID="txtDamagedQty" CssClass="lblDmgdQty" runat="server" Text='<%#Eval("DamagedQty") %>'  ></asp:Label>                                                      
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="Label32"  runat="server" CssClass="lblDmgdQtyTotal" ></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="Excess Qty." HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="home_ tfExcessQty" HeaderStyle-CssClass="tfExcessQty"  Visible="true" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtExcessQty" runat="server" CssClass="lblExsQty" Text='<%#Eval("ExcessQty") %>'></asp:Label>                                                       
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="Label33"  runat="server" CssClass="lblExsQtyTotal"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="Short Qty." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="home_ tfShortQty" HeaderStyle-CssClass="tfShortQty"  Visible="true">
                                                    <ItemTemplate>
                                                       <asp:Label ID="txtShortQty" runat="server" CssClass="lblShtQty" Text='<%#Eval("DiscrepancyQty")%>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                         <asp:Label ID="Label2"  runat="server" class="lblShtQtyTotal"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>                                               

                                                <asp:TemplateField ItemStyle-Width="100" HeaderText="Volume (m&#179;)" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="home_" ItemStyle-HorizontalAlign="Center" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtVolume" runat="server" CssClass="lblVolume"  Text='<%# DataBinder.Eval(Container.DataItem, "MVolume","{0:0.000}") %>' />                                                        
                                                    </ItemTemplate>
                                                     <FooterTemplate>
                                                         <asp:Label ID="Label1"  runat="server" class="lblVolumeTotal"></asp:Label>
                                                     </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField ItemStyle-Width="80" HeaderText="Receive" Visible="false" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltQCCaptured" Visible="false" Text="" />
                                                        <%--<asp:HyperLink ID="lnkReceiveItem" Text="Receive" CssClass="ButEmpty" NavigateUrl='<%# String.Format("../mInventory/StockIn.aspx?ibdno={0}&mmid={1}&lno={2}&poid={3}&invid={4}",DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString(),DataBinder.Eval(Container.DataItem, "LineNumber").ToString(),DataBinder.Eval(Container.DataItem, "POHeaderID").ToString(),DataBinder.Eval(Container.DataItem, "SupplierInvoiceID").ToString()) %>' Font-Underline="false" runat="server"></asp:HyperLink>--%>
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-CssClass="NoPrint" HeaderStyle-HorizontalAlign="Center" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" HeaderText="Print Labels" ItemStyle-HorizontalAlign="center" ItemStyle-Width="50">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblPrintLabel" Text="Print Label" runat="server" CssClass="NoPrint" /><br />
                                                        <asp:CheckBox ID="CheckAll" onclick="return check_uncheck (this );" runat="server" CssClass="NoPrint" /><br>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsPrint" runat="server" CssClass="NoPrint" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkPrintBarCodeLabel" runat="server" Text="Print" CssClass="ButEmpty" CausesValidation="false" OnClick="lnkPrintBarCodeLabel_Click" />

                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;" Mode="NumericFirstLast" PageButtonCount="15" />
                                            <FooterStyle CssClass="gvSilver_footerGrid_RCR" />
                                            <RowStyle CssClass="gvSilver_DataCellGrid_RCR" />
                                            <EditRowStyle CssClass="gvSilver_DataCellGridEdit" />
                                            <PagerStyle CssClass="gvBlue_pager" />
                                            <HeaderStyle CssClass="gvSilver_headerGrid" />
                                            <AlternatingRowStyle CssClass="gvSilver_DataCellGridAlt gvSilver_DataCellGridAlt_RCR" />
                                        </asp:GridView>
                                    </td>
                               
                    </tr>
                </tbody>
            </table>           
           
            <br /><br /><br />
            <div class="divFooter">
                <table style="width:100%;font-size:10pt;">
                    <tr style="display:none;">
                        <td style="width:30%;">Remarks :</td>
                        <td style="width:30%;"></td>
                        <td style="width:30%;"></td>
                    </tr>                    
                    <tr style="display:none;">
                        <td colspan="3">
                            <div style="border:1px solid grey;width:85%;height:50px;margin:10px;"></div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">DATE:</td>
                    </tr>
                    <tr>
                        <td ><br />SIGNATURE :</td>
                        <td >SIGNATURE :</td>
                        <td >SIGNATURE :</td>
                    </tr>
                    <tr>
                        <td ><br />WAREHOUSE MANAGER</td>
                        <td >OPERATION SUPERVISOR</td>
                        <td >WMS SUPERVISOR</td>
                    </tr>
                </table>
            </div>           
            <div class="divFooterLogo">               
            Powered By : www.inventrax.com
            </div>
        </asp:Panel>

    </div>
</asp:Content>
