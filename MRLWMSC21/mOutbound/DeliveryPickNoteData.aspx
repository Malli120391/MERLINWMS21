<%@ Page Title=".: Delivery Note :." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="DeliveryPickNoteData.aspx.cs" Inherits="MRLWMSC21.mOutbound.DeliveryPickNoteData"  MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">

    <asp:ScriptManager runat="server" ID="smngrDPN" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>

    <script type="text/javascript" src="../mManufacturingProcess/Scripts/ben_Print.js"></script>
    <script type="text/javascript">

        $(document).ready(function () { 
            CalculateTotals();            
        });

        function CalculateTotals()
        {
            //debugger;
            var TotalVol = 0;
            $('.lblVolume').each(function () {
                var thisQty = $(this).text().trim() == "" ? "0" : $(this).text().trim();
                TotalVol = eval(TotalVol) + eval(thisQty);
            });
            $('.lblTotalVolume').text(TotalVol.toFixed(3));


            var TotalQty = 0;
            $('.lblQty').each(function () {
                var thisQty = $(this).text().trim() == "" ? "0" : $(this).text().trim();
                TotalQty = eval(TotalQty) + eval(thisQty);
            });
            $('.lblQtyTotal').text(TotalQty.toFixed(2));
        }
       
        function printDiv(divName) { //Delivery Note
            
           
            var Gcount = '<%=this.gvPOLineQty1.Rows.Count%>';
            var time = 200;
            if (Gcount > 100)
            { time = 75 } else
                if (Gcount > 500)
                { time = 50 }
            //$(".PrintListcontainer").print();
            var panel = document.getElementById("PrintPanel");
            var printWindow = window.open('', '', 'location=0, status=0, resizable=1, scrollbars=1, width=800, height=400');
            //printWindow.document.write('<html><head><title>Delivery Pick Note</title>');
            printWindow.document.write('<LINK href="../PrintStyle.css?v=4.0"  type="text/css" rel="stylesheet" media="print">');
            //printWindow.document.write('<style type="text/css"> @page { size: portrait !important; margin-top:80px; margin-bottom:80px;}   @media print{@page {size: portrait}    #tdDDRPrintArea{ position:relative;width:100%;}   div.divFooter {display:block;position: relative;margin-left:50px;} div.divFooterLogo {display:block;position: fixed;bottom: 0px;right:0;}  .tdPickLink { display:none; }  .tblGridInnerTable{font-size:10pt;} .spanFooterNote {display:none;} .NoPrint{display:none;} .page-break{page-break-after:always;}      )</style>'); //This line is commented by kashyap on 17/08/2017 for delivery note print error 
            //printWindow.document.write('<style type="text/css"> @page { size: portrait; margin-top:20px; margin-bottom:20px;}   @media print{@page {size: portrait}.gvSilver{display:table-row-group; position: relative; height: 9.5in;  margin-left: .5in;  margin-right: .5in; margin-top: 0;  margin-bottom: -.25;  z-index: 10;} #tdDDRPrintArea{ display:table-header-group;table-layout:fixed; top:150px;  width: 100%;height:50px;}  div.divFooter {display:table-footer-group;position: relative;margin-left:50px;} div.divFooterLogo {display:block;position: fixed;bottom: 0px;right:0;}  .tdPickLink { display:none; }  .tblGridInnerTable{font-size:10pt;} .spanFooterNote {display:none;} .NoPrint{display:none;} .page-break{page-break-before:always;}      )</style>');//This line is commented by kashyap on 18/08/2017 for delivery note print error 
            printWindow.document.write('<style type="text/css"> @page { size: landscape; margin-top:10px;margin-left:15px;margin-right:15px; }   @media print { .gvSilver{ position: relative; height: 9.5in;  margin-left: .5in;  margin-right: .5in; margin-top: 0;  margin-bottom: -.25;  z-index: 10;} .gvSilver_DataCellGrid{page-break-inside : avoid;} #tdDDRPrintArea{ display:table-header-group; top:100px;  width: 100%;}  div.divFooter {display:block;position:relative;clear:both;} div.divFooterLogo {display:table-footer-group;position: fixed;bottom: 0px;clear:both;right:0;  page-break-inside:avoid;  page-break-before:always; }   .tdPickLink { display:none; } .spanFooterNote {display:none;}.gvSilver .tblGridInnerTable td, .tblGridInnerTable th{border:0px !important;padding:0px !important;}} </style>');
            printWindow.document.write('<script src="../Scripts/jquery-1.8.2.min.js"><\/script><script>   $(document).ready(function(){  $(".tblGridInnerTable").attr("cellPadding","5"); $(".tblGridInnerTable tbody tr td").css("border-right","1px solid #000") });<\/script>');
            //printWindow.document.write('<style type="text/css">   @media print { .page-break  { display: block; page-break-before: always; } }   </style>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');

            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();// printWindow.close();
                printWindow.close();
            }, time * Gcount );
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadMCode();
            }
        }

        function focuslost1(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search Part # ...";

            TextBox.style.color = "#A4A4A4";

        }

        function clear(TextBox) {
            if (TextBox.value == "Search Part # ...")
                TextBox.value = "";
            TextBox.style.color = "#000000";
        }


        function fnLoadMCode() {
            $(document).ready(function () {

                try {
                    var textfieldname = $("#<%= this.txtMCode.ClientID %>");
                    DropdownFunction(textfieldname);
                    $("#<%= this.txtMCode.ClientID %>").autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDPNoteMCodeOEMData") %>',
                                data: "{ 'prefix': '" + request.term + "', 'OutboundID': '" + '<%= ViewState["OutboundID"] %>' + "' }",
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    response($.map(data.d, function (item) {
                                        return {
                                            label: item.split('`')[0],
                                            description: item.split('`')[1] == undefined ? "" : " <font color='#086A87'>" + item.split('`')[1] + "</font>"
                                        }
                                    }))
                                },
                                error: function (response) {

                                },
                                failure: function (response) {

                                }
                            });
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

            });
        }
        fnLoadMCode();
    </script>
        <style type="text/css">
            body {
                overflow: scroll;
            }
            .gvSilver {
                border:none !important;
            }
            .gvSilver td,.gvSilver th {
                    padding:5px !important;
                }
            .gvSilver .tblGridInnerTable td, .tblGridInnerTable th
               
                {
                    border: 0px !important;
                    padding: 0px !important
                }
            .gvSilver .tblGridInnerTable th {
                font-weight:700 !important;
            }
            .gvSilver table {
                table-layout: fixed;
            }
            .gvSilver table td {
                overflow: hidden;
            }
            .BarCodeCell {
                min-width: 310px;
            }
            .lblTotalVolume {
                color: black;
                font-weight: bold;
            }
            .tblPickListDetails, .tblDelNoteDetails{ font-size:10.5pt;}
            .tblGridInnerTable {
                border:0px solid grey;
                page-break-after:always;
            }
            .tblGridInnerTable tr td {
                    border-right:1px solid grey;
              }
            .tblGridInnerTable tr td:last-child {
                    border-right:0px solid grey;
              }
            .tblGridInnerTable tr {               
                border-bottom:1px solid grey;
            }
            .tblGridInnerTable tr:last-child {               
                border-bottom:0px solid grey;
            }

            .alignRight {
                text-align: right !important;
            }
            .alignCenter {
                text-align: center !important;
            }
            .tdDamagedQty {
                text-align: right !important;
            }
            .BarCodetext {
                font-weight:500 !important;
            }
            .divAlign {
                display: flex;
                justify-content: space-between;
            }
   </style>

    <style type="text/css">
        @page {
         
        size:landscape;
        }
        @media screen {
            div.divFooter, div.divFooterLogo, .divRemarks {
                display: none;
            }

            .hiddenGrid {
                display: none;
            }

            .alignRight {
                text-align: right !important;
            }
        }
        @media print {
            div.divFooterLogo {
                display: block;
                position: fixed;
                bottom: 0;
            }

            div.divFooter {
                display: block;
            }

            .hiddenGrid {
                display: block;
            }

            .alignRight {
                text-align: right !important;
            }
            .gvSilver {
                border:none !important;
            }
            .divAlign {
                display:flex;justify-content:flex-end !important;
            }           
        }
       @media print{@page {
                        size: landscape}}
   </style>


    <div class="wrapper1">
    <div class="divUP">
    </div>
</div>

    <div class="wrapper2">
    <div class="divdown" id="PrintPanel">
        <asp:HiddenField ID="hdnDelvStatusId" runat="server" Value="0" />
        <link href="../PrintStyle.css?v=4.0" type="text/css" rel="stylesheet" media="print">

       <%-- <style>
             @page {
        size:portrait;
        }           
        </style>--%>
         <div class="container" style="max-height:unset !important;overflow:unset !important;">
        <table border="0" cellpadding="2" cellspacing="2" align="center" id="tdDDRPrintArea" style="width:100%;">
             
                <tr>
                    <td style="width: 42%;">
                       <%-- <img id="Img2" runat="server" src="../TPL/AccountLogos/25032020_092637_logo.png" width="140" border="0" alt="">--%>
                         <img id="Img1" runat="server" visible="false"  width="150" height="50" border="0" alt="">
                    </td>
                    <td style="width: 58%;" align="left">
                        <div class="divAlign">
                        <asp:Label runat="server" Text="DELIVERY NOTE" ID="lblHeader" style="font-size:14pt;" CssClass="SubHeading3 lblHeading RTRHeader" />
                      
                            <span class="NoPrint"><a href="#" onclick="javascript:printDiv('tdDDRPrintArea');" class="btn btn-primary printDelNote" style="float:right;">Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %></a>&nbsp;
                        <asp:LinkButton runat="server" ID="lnkbackToList" CssClass="btn btn-primary" style="float:right;margin-right:25px;" PostBackUrl="OutboundTracking.aspx">Back to List<%=MRLWMSC21Common.CommonLogic.btnfaList %></asp:LinkButton></span></div>
                    </td>
                </tr>              

                <tr>

                    <td colspan="2" align="right" valign="bottom" class="NoPrint">
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr style="height: 0.2px; color: #000; border-color: #CCC; background-color: #000;" />
                    </td>
                </tr>

                <tr class="DPNoteFSize">
                    <td colspan="2">
                        <table style="width:100%;">
                            <tr>
                            <td style="padding-left:0px;width:50%;vertical-align:top;">
                                <span style="font-size:11.5pt;"><b>Ship To :</b></span><br />
                                <asp:Literal ID="ltCustomerName" runat="server"></asp:Literal> - <asp:Literal ID="ltTelephone" runat="server"></asp:Literal><br />
                                <asp:Literal ID="ltAddress1" runat="server"></asp:Literal> <asp:Literal ID="ltAddress2" runat="server"></asp:Literal><br />
                                <asp:Literal ID="ltFax" runat="server"></asp:Literal><br />
                                <asp:Literal ID="ltContactPerson" runat="server"></asp:Literal>
                            </td>
                            <td style="width:30%;"></td>
                            <td style="width:20%;">
                                <table>
                                    <tr>
                                        <td style="width:10%;vertical-align: top !important; font-weight: bold;">Shipment ID</td>
                                        <td style="width:30%;vertical-align: top !important;"><b>:&nbsp;</b><asp:Literal ID="ltShipmentID" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align:top !important;font-weight:bold;">SO#</td>
                                        <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltSONumber" runat="server"></asp:Literal></td>  
                                    </tr>
                                    <tr>
                                        <td style="vertical-align:top !important;font-weight:bold;">Invoice#</td>
                                        <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltInvNo" runat="server"></asp:Literal></td>  
                                    </tr>
                                     <tr>
                                         <td style="vertical-align: top !important; font-weight: bold;">Vehicle&nbsp;No.</td>
                                         <td style="vertical-align: top !important;"><b>:&nbsp;</b><asp:Literal ID="ltVehicleNo" runat="server"></asp:Literal></td>
                                     </tr>
                                     <tr>
                                         <td style="vertical-align:top !important;font-weight:bold;">Driver&nbsp;Name</td>
                                         <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltDeliveryDate" Visible="false" runat="server"></asp:Literal> <asp:Literal ID="ltDeliveryTime" Visible="false" runat="server"></asp:Literal></td>
                                    </tr>
                                </table>
                            </td>
                            </tr>
                        </table>






                        <%--<br />
                        <asp:Literal ID="ltDelvDocNo" runat="server" />
                        <br />
                    </td>
                    <td align="right" colspan="2">
                        <asp:Literal ID="ltDelvDocDetails" runat="server" /></td>--%>
                        <%--<table style="width:90%;margin-left:0%;">
                            <tr>
                                <td style="width:8%;vertical-align:top !important;font-weight:bold;">Name </td>
                                <td style="width:27%;vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltCustomerName" runat="server"></asp:Literal></td>                               
                                <td style="width:1%;vertical-align:top !important;"></td>
                                <td style="width:8%;vertical-align:top !important;font-weight:bold;">Shipment ID</td>
                                <td style="width:13%;vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltShipmentID" runat="server"></asp:Literal></td>
                            </tr>
                            <tr>
                                <td style="vertical-align:top !important;font-weight:bold;">Address 1 </td>
                                <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltAddress1" runat="server"></asp:Literal></td>                               
                                <td style="vertical-align:top !important;"></td>
                                <td style="vertical-align:top !important;font-weight:bold;">SO Number</td>
                                <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltSONumber" runat="server"></asp:Literal></td>                                
                            </tr>
                            <tr>
                                <td style="vertical-align:top !important;font-weight:bold;">Address 2 </td>
                                <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltAddress2" runat="server"></asp:Literal></td>                               
                                <td style="vertical-align:top !important;"></td>
                                <td style="vertical-align:top !important;font-weight:bold;">Invoice Number</td>
                                <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltInvNo" runat="server"></asp:Literal></td>
                            </tr>
                            <tr>
                                <td style="vertical-align:top !important;font-weight:bold;">Telephone </td>
                                <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltTelephone" runat="server"></asp:Literal></td>                               
                                <td style="vertical-align:top !important;"></td>
                                <td style="vertical-align:top !important;font-weight:bold;">Ref #</td>
                                <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltRefNo" runat="server"></asp:Literal></td>
                                
                            </tr>
                            <tr>
                                <td style="vertical-align:top !important;font-weight:bold;">Fax </td>
                                <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltFax" runat="server"></asp:Literal></td>                               
                                <td style="vertical-align:top !important;"></td>
                                <td style="vertical-align:top !important;font-weight:bold;">Vehicle No.</td>
                                <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltVehicleNo" runat="server"></asp:Literal></td>                                
                            </tr>
                            <tr>
                                <td style="vertical-align:top !important;font-weight:bold;">Contact Person </td>
                                <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltContactPerson" runat="server"></asp:Literal></td>                               
                                <td style="vertical-align:top !important;"></td>
                                <td style="vertical-align:top !important;font-weight:bold;">Delivery Date</td>
                                <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltDeliveryDate" runat="server"></asp:Literal></td>
                            </tr>
                            <tr>
                                <td style="vertical-align:top !important;"></td>
                                <td style="vertical-align:top !important;"></td>                               
                                <td style="vertical-align:top !important;"></td>
                                <td style="vertical-align:top !important;font-weight:bold;">Delivery Time</td>
                                <td style="vertical-align:top !important;"><b>:&nbsp;</b><asp:Literal ID="ltDeliveryTime" runat="server"></asp:Literal></td>
                            </tr>
                        </table>--%>
                </tr>
          <%--  </thead>--%>


          <%--  <tbody>--%>


                <tr>
                    <td colspan="2" align="left">
                        <asp:Label ID="lblStatusMessage" runat="server" CssClass="ErrorMsg" />                        
                    </td>
                </tr>
                <tr>
                    <td class="NoPrint">
                        <asp:Literal runat="server" ID="ltPNCPRecordCount" />
                    </td>
                    <td align="right" class="NoPrint">

                        <div style="display:flex;justify-content:flex-end;">
                            <div style="width:300px;overflow: unset;">
                                <div class="flex" style="overflow: unset;">
                                    <asp:TextBox ID="txtMCode" Text="Search Part # ..." SkinID="txt_Hidden_Req_Auto" runat="server" onfocus="clear(this)" onblur="javascript:focuslost1(this);" />
                                </div>
                            </div>
                            <div>
                                 <asp:LinkButton ID="lnkMCodeSearch" runat="server" CssClass="btn btn-primary" OnClick="lnkMCodeSearch_Click"> Search <span class="space fa fa-search"></span></asp:LinkButton>
                            </div>
                        </div>
                        
                    </td>
                </tr>

            
                <tr class="NoPrint">
                    
                   
                     <td valign="top" align="center" colspan="2">


                        <asp:GridView Width="100%" CssClass="gvSilver" CellPadding="1" CellSpacing="1" ShowFooter="true" HeaderStyle-BorderColor="Gray" HeaderStyle-BorderWidth="1" GridLines="Both" BackColor="#FFFFFF" Font-Size="11pt" ID="gvPOLineQty" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="true" HorizontalAlign="Left" OnRowCreated="gvPOLineQty_RowCreated" OnRowCommand="gvPOLineQty_RowCommand" OnSorting="gvPOLineQty_Sorting" OnPageIndexChanging="gvPOLineQty_PageIndexChanging" OnRowDataBound="gvPOLineQty_RowDataBound">

                            <Columns>
                                <asp:TemplateField HeaderText="Line#" ItemStyle-Width="50" ItemStyle-CssClass="LineNoCell" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltLineNumber" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber","{0:000}") %>' />

                                        <asp:Literal runat="server" ID="hidKitPlannerID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "KitPlannerID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="KitPlannerID" HeaderText="KitPlannerID" SortExpression="KitPlannerID" Visible="false" />
                                <asp:BoundField DataField="ParentMcode" HeaderText="ParentMcode" SortExpression="ParentMcode" Visible="false" />

                                <asp:TemplateField HeaderText="Item Code" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" CssClass="BarCodetext" ID="lblMCode" Text='<%# String.Format("{0}",DataBinder.Eval(Container.DataItem, "MCode")) %>' />
                                        <asp:Literal runat="server" ID="ltOEMPartNo" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>' />     
                                        <br />
                                        <span style="color: #2196F3;font-size: 8Pt;">
                                        <asp:Literal runat="server" ID="ltItemDesc1" Text='<%#Eval("MDescription") %>' />
                                        </span>

                                        <asp:Literal runat="server" ID="ltMMID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID") %>' />
                                        <asp:Literal runat="server" ID="ltOBDTrackingID" Visible="false" />
                                        <asp:Literal runat="server" ID="ltSOHID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SOHeaderID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description" ItemStyle-Width="200" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div style="color: #2196F3;text-align: left; font-size: 10pt;">
                                            <asp:Literal runat="server" ID="ltItemDesc" Text='<%#Eval("MDescription") %>' />
                                            </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                

                                <asp:TemplateField HeaderText="Item Image" ItemStyle-Width="100" Visible="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# ("../") +Eval("Image") %>' Height="60px" Width="80px" />  
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                  <asp:TemplateField ItemStyle-Width="20"   HeaderStyle-Width="20" HeaderText="UoM" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltMMSKU" Text='<%# String.Format("{0}", DataBinder.Eval(Container.DataItem, "SUoM").ToString())%>' />
                                        <asp:Literal runat="server" ID="ltSUoMID" Visible="false" />
                                        <asp:Literal runat="server" ID="ltSUoMQty" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SUoMQty","{0:0.00}") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 

                                <asp:TemplateField ItemStyle-Width="20" HeaderText="Base UoM" Visible="false" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltBMMSKU" Text='<%# String.Format("{0}/{1}", DataBinder.Eval(Container.DataItem, "BUoM").ToString(),DataBinder.Eval(Container.DataItem, "BUoMQty","{0:0.00}").ToString() )%>' />
                                        <asp:Literal runat="server" ID="ltBUoMID" Visible="false" />
                                        <asp:Literal runat="server" ID="ltBUoMQty" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "BUoMQty","{0:0.00}") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Width="20" HeaderText="Min Pic." Visible="false" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltMinMSKU" Text='<%# String.Format("{0}/{1}", DataBinder.Eval(Container.DataItem, "MUoM").ToString(),DataBinder.Eval(Container.DataItem, "MUoMQty","{0:0.00}").ToString() )%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                              

                               

                                <asp:TemplateField ItemStyle-Width="200" ItemStyle-CssClass="home" Visible="false" HeaderStyle-Width="200" HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                    
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltSplitLocation" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SplitLocation") %>' />
                                        <asp:Literal runat="server" ID="ltLocation" Visible="true" />
                                        <asp:Literal runat="server" ID="ltLocationID" Visible="false" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <span class="spanFooterNote" hidden>Note: Strike-off row items cannot be picked due to their parameters difference than the required items in Delv.Doc.</span>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Cond." ItemStyle-Width="60">
                                    <ItemTemplate>                                        
                                            <asp:Literal runat="server" ID="ltStockStatus" Text='<%#Eval("SLOC") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Batch#" ItemStyle-Width="60" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>                                        
                                            <asp:Literal runat="server" ID="ltBatchNo" Text='<%#Eval("BatchNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Serial#" ItemStyle-Width="60" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltSerialNo" Text='<%#Eval("SerialNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Mfg.&nbsp;Date" ItemStyle-Width="60" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-CssClass="alignCenter">
                                    <ItemTemplate>
                                        <div style="text-align: center; font-size: 10pt;">
                                            <asp:Literal runat="server" ID="ltMfgDate" Text='<%#Eval("MfgDate") %>' />
                                            </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Exp.&nbsp;Date" ItemStyle-Width="60" ItemStyle-CssClass="alignCenter" FooterStyle-CssClass="alignCenter" HeaderStyle-CssClass="alignCenter">
                                    <ItemTemplate>
                                        <div style="font-size: 10pt;text-align: center;">
                                            <asp:Literal runat="server" ID="ltExpDate" Text='<%#Eval("ExpDate") %>' />
                                            </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Project&nbsp;Ref.#" ItemStyle-Width="60" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>                                        
                                            <asp:Literal runat="server" ID="ltProjectRefNo" Text='<%#Eval("ProjectRefNo") %>' />                                            
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="MRP" ItemStyle-Width="60" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>                                        
                                            <asp:Literal runat="server" ID="ltMRP" Text='<%#Eval("MRP") %>' />                                           
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField ItemStyle-Width="90" HeaderText="Total Qty." ItemStyle-CssClass="alignRight" FooterStyle-CssClass="alignRight" HeaderStyle-CssClass="alignRight">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="ltQuanity1" CssClass="lblQty1" Text='<%# DataBinder.Eval(Container.DataItem, "SOQuantity","{0:0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <label class="lblQtyTotal" Style="text-align: right;width:100%;color:black;font-weight:700;font-size:12px !important;"></label>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Width="90" HeaderStyle-Width="90" Visible="false" HeaderText="Received Good Qty." ItemStyle-CssClass="alignRight" FooterStyle-CssClass="alignRight" HeaderStyle-CssClass="alignRight">
                                    
                                    <ItemTemplate>
                                        <div style="border:1px solid black;margin:5px;height:20px;background-color:#f2f2f2;"></div>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="90" HeaderStyle-Width="90" Visible="false" HeaderText="Received Dam. Qty." ItemStyle-CssClass="alignRight" FooterStyle-CssClass="alignRight" HeaderStyle-CssClass="alignRight">
                                    
                                    <ItemTemplate>
                                        <div style="border:1px solid black;margin:5px;height:20px;background-color:#f2f2f2;"></div>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>


                                <asp:TemplateField ItemStyle-Width="100" HeaderStyle-Width="100" HeaderText="Total Vol.(m&#179;)" ItemStyle-CssClass="alignRight" FooterStyle-CssClass="alignRight" HeaderStyle-CssClass="alignRight">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Style="" ID="ltVolume" CssClass="lblVolume1" Text='<%# DataBinder.Eval(Container.DataItem, "mVolume","{0:0.000}") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="ltVolume" Style="text-align: right; width:100%;color:black;font-weight:700;font-size:12px !important;" CssClass="lblTotalVolume"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>

                            </Columns>

                            <FooterStyle CssClass="gvSilver_footerGrid" />
                            <RowStyle CssClass="gvSilver_DataCellGrid" />
                            <EditRowStyle CssClass="gvSilver_DataCellGridEdit" />
                            <PagerStyle CssClass="gvSilver_pagerGrid" />
                            <HeaderStyle CssClass="gvSilver_headerGrid" />
                            <AlternatingRowStyle CssClass="gvSilver_DataCellGrid" />
                        </asp:GridView>                                                
                    </td>
                    
                </tr>
            <tr>
                    <td class="hiddenGrid" valign="top" align="center" colspan="2" >
              
                        <asp:GridView Width="100%"  CssClass="gvSilver" CellPadding="1" CellSpacing="1" ShowFooter="true"  GridLines="Both"  Font-Size="11pt" ID="gvPOLineQty1" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="true" HorizontalAlign="Left" OnRowCreated="gvPOLineQty1_RowCreated" OnRowCommand="gvPOLineQty1_RowCommand" OnSorting="gvPOLineQty1_Sorting" OnPageIndexChanging="gvPOLineQty1_PageIndexChanging" OnRowDataBound="gvPOLineQty1_RowDataBound">

                            <Columns>
                                <asp:TemplateField HeaderText="Line#"  ItemStyle-Width="50" ItemStyle-CssClass="LineNoCell" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltLineNumber" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber","{0:000}") %>' />

                                        <asp:Literal runat="server" ID="hidKitPlannerID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "KitPlannerID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="KitPlannerID" HeaderText="KitPlannerID" SortExpression="KitPlannerID" Visible="false" />
                                <asp:BoundField DataField="ParentMcode" HeaderText="ParentMcode" SortExpression="ParentMcode" Visible="false" />

                                <asp:TemplateField HeaderText="Item Code" ItemStyle-Width="300" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                         <div style="text-align: left;">
                                        <asp:Label runat="server" CssClass="BarCodetext" ID="lblMCode" Text='<%# String.Format("{0}",DataBinder.Eval(Container.DataItem, "MCode")) %>' />
                                        <asp:Literal runat="server" ID="ltOEMPartNo" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>' />
                                             <br />
                                             <span style="font-size:7pt;">
                                                 <asp:Literal runat="server" ID="ltItemDesc1" Text='<%#Eval("MDescription") %>' />
                                             </span>
                                       

                                        <asp:Literal runat="server" ID="ltMMID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID") %>' />
                                        <asp:Literal runat="server" ID="ltOBDTrackingID" Visible="false" />
                                        <asp:Literal runat="server" ID="ltSOHID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SOHeaderID") %>' />

                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description" Visible="false" ItemStyle-Width="200" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <div style="text-align: left; font-size: 10pt;">
                                            <asp:Literal runat="server" ID="ltItemDesc" Text='<%#Eval("MDescription") %>' />
                                            </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Item Image" Visible="false" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# ("../") +Eval("Image") %>' Height="60px" Width="80px" />  
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="20" HeaderText="Base UoM" Visible="false" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltBMMSKU" Text='<%# String.Format("{0}/{1}", DataBinder.Eval(Container.DataItem, "BUoM").ToString(),DataBinder.Eval(Container.DataItem, "BUoMQty","{0:0.00}").ToString() )%>' />
                                        <asp:Literal runat="server" ID="ltBUoMID" Visible="false" />
                                        <asp:Literal runat="server" ID="ltBUoMQty" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "BUoMQty","{0:0.00}") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Width="20" HeaderText="Min Pic." Visible="false" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltMinMSKU" Text='<%# String.Format("{0}/{1}", DataBinder.Eval(Container.DataItem, "MUoM").ToString(),DataBinder.Eval(Container.DataItem, "MUoMQty","{0:0.00}").ToString() )%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Width="20" HeaderText="UoM" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltMMSKU" Text='<%# String.Format("{0}", DataBinder.Eval(Container.DataItem, "SUoM").ToString())%>' />
                                        <asp:Literal runat="server" ID="ltSUoMID" Visible="false" />
                                        <asp:Literal runat="server" ID="ltSUoMQty" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SUoMQty","{0:0.00}") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                               

                                <asp:TemplateField ItemStyle-Width="150" Visible="false" ItemStyle-CssClass="home" HeaderStyle-Width="150" HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltSplitLocation" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SplitLocation") %>' />
                                        <asp:Literal runat="server" ID="ltLocation" Visible="true" />
                                        <asp:Literal runat="server" ID="ltLocationID" Visible="false" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <span class="spanFooterNote" hidden>Note: Strike-off row items cannot be picked due to their parameters difference than the required items in Delv.Doc.</span>
                                    </FooterTemplate>
                                </asp:TemplateField>

                               <asp:TemplateField HeaderText="Cond." ItemStyle-Width="60">
                                    <ItemTemplate>                                        
                                            <asp:Literal runat="server" ID="ltStockStatus" Text='<%#Eval("SLOC") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Batch#" ItemStyle-Width="60" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>                                        
                                            <asp:Literal runat="server" ID="ltBatchNo" Text='<%#Eval("BatchNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Serial#" ItemStyle-Width="60" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltSerialNo" Text='<%#Eval("SerialNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Mfg.&nbsp;Date" ItemStyle-Width="60" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div style="text-align: center; font-size: 10pt;">
                                            <asp:Literal runat="server" ID="ltMfgDate" Text='<%#Eval("MfgDate") %>' />
                                            </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Exp.&nbsp;Date" ItemStyle-Width="60" ItemStyle-CssClass="alignCenter" FooterStyle-CssClass="alignCenter" HeaderStyle-CssClass="alignCenter">
                                    <ItemTemplate>
                                        <div style="font-size: 10pt;text-align: center;">
                                            <asp:Literal runat="server" ID="ltExpDate" Text='<%#Eval("ExpDate") %>' />
                                            </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Project&nbsp;Ref.#" ItemStyle-Width="60" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>                                        
                                            <asp:Literal runat="server" ID="ltProjectRefNo" Text='<%#Eval("ProjectRefNo") %>' />                                            
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="MRP" ItemStyle-Width="60" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>                                        
                                            <asp:Literal runat="server" ID="ltMRP" Text='<%#Eval("MRP") %>' />                                           
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField ItemStyle-Width="70" HeaderText="Total&nbsp;Qty." HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <div style="text-align: right;">
                                        <asp:Label runat="server" ID="ltQuanity" CssClass="lblQty" Text='<%# DataBinder.Eval(Container.DataItem, "SOQuantity","{0:0.00}") %>'></asp:Label>
                                            </div>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                         <div style="text-align: right;">
                                        <label class="lblQtyTotal" Style="text-align: right;width:100%;color:black;font-weight:700;font-size:12px !important;"></label>
                                             </div>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField ItemStyle-Width="90" HeaderStyle-Width="90" HeaderText="Recv.&nbsp;Good&nbsp;Qty." ItemStyle-CssClass="alignRight" FooterStyle-CssClass="alignRight" HeaderStyle-CssClass="alignRight">
                                    
                                    <ItemTemplate>
                                        <div style="border:1px solid black;margin:5px;height:20px;"></div>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="90" HeaderStyle-Width="90" HeaderText="Recv.&nbsp;Damaged&nbsp;Qty." ItemStyle-CssClass="alignRight" FooterStyle-CssClass="alignRight" HeaderStyle-CssClass="alignRight">
                                    
                                    <ItemTemplate>
                                        <div style="border:1px solid black;margin:5px;height:20px;"></div>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>


                                <asp:TemplateField ItemStyle-Width="100" HeaderStyle-Width="100" Visible="false" HeaderText="Total&nbsp;Vol.(m&#179;)" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                         <div style="text-align: right;">
                                        <asp:Label runat="server" Style="" ID="ltVolume" CssClass="lblVolume" Text='<%# DataBinder.Eval(Container.DataItem, "mVolume","{0:0.000}") %>' />
                                             </div>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                         <div style="text-align: right;">
                                        <asp:Label runat="server" ID="ltVolume" style="text-align: right;width:100%;color:black;font-weight:700;font-size:12px !important;" CssClass="lblTotalVolume"></asp:Label>
                                             </div>
                                    </FooterTemplate>
                                </asp:TemplateField>

                            </Columns>

                            <FooterStyle CssClass="gvSilver_footerGrid" />
                            <RowStyle CssClass="gvSilver_DataCellGrid BorderRight" />
                            <EditRowStyle CssClass="gvSilver_DataCellGridEdit" />
                            <PagerStyle CssClass="gvSilver_pagerGrid" />
                            <HeaderStyle CssClass="gvSilver_headerGrid" />
                            <AlternatingRowStyle CssClass="gvSilver_DataCellGrid" />
                        </asp:GridView>                                                
                    </td>
                </tr>
            
                <tr>
                    <td colspan="2">                       
                             <span class="divRemarks DPNoteFSize" style="font-size:9pt !important;">Remarks</span>
                            <div class="divRemarks" style="border:1px solid lightgrey;height:30px;width:100%;"></div>
                                
                       
                    </td>
                </tr>               
           <%-- </tbody>--%>
        </table>
    <div class="divFooter DPNoteFSize">
                             <table cellspacing="5">
                                <tr>
                                    <td style="width:500px;"><b>Dispatched By: </b></td>
                                    <td style="width:100px;"><b>Received By:</b></td>
                                </tr>                
                                <tr>
                                    <td>Name :</td>
                                    <td>Name :</td>
                                </tr>
                                 <tr>
                                    <td>Mobile No. :</td>
                                    <td>Mobile No. :</td>
                                </tr>
                                <tr>
                                    <td>Sign :</td>
                                    <td>Sign :</td>
                                </tr>
                                <tr>
                                    <td>Date :</td>
                                    <td>Date :</td>
                                </tr>
                            </table> 
                        </div>
     </div>
       <div class="divFooterLogo" style="display: flex; justify-content: space-between; align-items:center; padding:0px 10px;left:0px;bottom:-5px;">                     
           <span><b>Shipment ID :&nbsp;</b><asp:Literal ID="ltShipmentID1" runat="server"></asp:Literal></span>
           <span><small>Printed On : <%= DateTime.Now.ToString("dd-MMM-yyyy") %>&nbsp;<%= DateTime.Now.ToString("hh:mm tt") %></small></span> 
           <span> www.merlinwms.in</span>
        </div>
        
        </div>
    </div>
</asp:Content>
