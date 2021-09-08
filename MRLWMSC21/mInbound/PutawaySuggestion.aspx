
<%@ Page Title="" Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="PutawaySuggestion.aspx.cs" Inherits="MRLWMSC21.mInbound.PutawaySuggestion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IBContent" runat="server">
    <script type="text/javascript" src="../mManufacturingProcess/Scripts/ben_Print.js"></script>
    <style>
        .module_login {
            border-right: 0px solid #1a79cf;
            border-left: 0px solid #1a79cf;
        }
    </style>
    <script>
        function printData() {
            printDiv(document.getElementsByClassName('internalData'));
            return false;
        }
        function printDiv(divName) {
            var Gcount = '<%=this.gvInvoiceList.Rows.Count%>';
            var time = 100;
            if (Gcount > 100)
            { time = 75 } else
                if (Gcount > 500)
                { time = 50 }
            var panel = document.getElementById("<%=gvInvoiceList.ClientID %>");
            var printWindow = window.open('', '', 'height=400,width=800,scrollbars=1,location=1,status=1,resizable=1');
            printWindow.document.write('<html><head><title>Receive Tally Report</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.write('<LINK href="../PrintStyle.css"  type="text/css" rel="stylesheet" media="print">');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();// printWindow.close();
            }, time * Gcount);
        }
    </script>
    <div class="dashed"></div>
    <div class="pagewidth">
    <table width="100%" class="" cellpadding="4" cellspacing="4">

       <tr>

            <td align="left" >
               
                <img id="Img1"  runat="server" enableviewstate="false"  src="~/Images/Logo_Header_falcon2.png" border="0" alt="">
            </td>
            <td align="center">
                <asp:Label runat="server" Text="Suggested Location" ID="lblHeader" CssClass="SubHeading3" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td> 
            <td align="right" valign="bottom">
                        
                <img width="20" height="20" alt="Print" style="visibility:hidden;" class="NoPrint" src="../Images/blue_menu_icons/printer.png" onclick="javascript:printDiv('tdDDRPrintArea');" border="0" style="cursor:pointer;" />

                <asp:LinkButton runat="server" ID="lnkbackToList" CssClass="ui-button-small NoPrint" PostBackUrl = "../mInbound/InboundTracking.aspx" >Back to List<%=MRLWMSC21Common.CommonLogic.btnfaList %></asp:LinkButton>
                    &nbsp;&nbsp; &nbsp;&nbsp;<a href="#" style="text-decoration:none;" onclick="javascript:printDiv('tdDDRPrintArea');" class="ui-button-small NoPrint">Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %></a> 

            </td>
        </tr>
        <tr>
            <td colspan="3"><hr /></td>
        </tr>

        <tr>
         <td>
                <asp:Label ID="lblStoreRefNo" runat="server" /><div style="height:15px"></div>
                <asp:Label ID="lblBarStoreRefNo" runat="server" />
         </td>
         </tr>
       
        <tr>
            <td colspan="3">
                <asp:GridView ID="gvInvoiceList" runat="server"  SkinID="gvLightBlueNew" AutoGenerateColumns="false" OnRowDataBound="gvInvoiceList_RowDataBound">

                    <Columns>
                        <asp:TemplateField HeaderText="PO No." ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Literal ID="ltPoNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"POCode") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Inv. No." ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Literal ID="ltInvoiceCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"supplierInvCode") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <table width="100%">
                                    <tr>
						                <th width="35px">Line No.</th>
                                        <th Width="205px">Part No.</th>
                                        <th Width="80px">BUoM / Qty. </th>
                                        <th Width="70px">PUoM / Qty.</th>
                                        <th Width="70px">Inv. UoM / Qty.</th>
                                        <th Width="50px">PO Qty.</th>
                                        <th Width="50px">Inv. Qty.</th>
                                        <th width="70px">Initiated Qty.</th>
                                        <th Width="110px">Location</th>
                                        <th Width="60px"">Sug. Qty.</th>
                                        <th Width="60px">Receive</th>   
					            </tr>
                                </table>
                            </HeaderTemplate>
                            
                            
                                <ItemTemplate>
                                <asp:GridView ID="gvLineItems"  runat="server" SkinID="gvLightBlueNew"   ShowHeader="false" ShowFooter="false" > 

                                    <Columns>

                                        <asp:BoundField DataField="Line No." HeaderText="Line No." ItemStyle-Width="30" />
                                                                                                                        	
                                          
                                        <asp:TemplateField HeaderText="Material Code" ItemStyle-Width="50">
											<ItemTemplate>
												
                                                <asp:Literal ID="ltMaterialCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Material Code") %>' /><br />
                                                <asp:Literal ID="ltMdesc" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Mdesc") %>' /><br />
                                                <asp:Literal runat="server" ID="ltBarcode" Text='<%# String.Format("<img width=\"200px\" src=\"Code39Handler.ashx?code={0} \"",DataBinder.Eval(Container.DataItem, "Material Code")) %>'/>&nbsp;&nbsp;&nbsp;&nbsp;

											</ItemTemplate>
										</asp:TemplateField>
                                        
                                        
                                        
                                        <asp:BoundField DataField="BUoM / Qty." HeaderText="BUoM / Qty." ItemStyle-Width="50" />
		                                <asp:BoundField DataField="PUoM / Qty." HeaderText="PUoM / Qty." ItemStyle-Width="50" />
		                                <asp:BoundField DataField="INV. UOM / Qty." HeaderText="INV. UOM / Qty." ItemStyle-Width="50" />
		                                <asp:BoundField DataField="PO Qty." HeaderText="PO Qty." ItemStyle-Width="50" />
		                                <asp:BoundField DataField="Inv. Qty." HeaderText="Inv. Qty." ItemStyle-Width="50" />
		                                <asp:BoundField DataField="Initiated Qty." HeaderText="Initiated Qty." ItemStyle-Width="50" />
		
		                                <asp:BoundField DataField="Location" HeaderText="Location" ItemStyle-Width="50" />
		                                <asp:BoundField DataField="Sug. Qty." HeaderText="Sug. Qty." ItemStyle-Width="50" />
		
		                                <asp:TemplateField HeaderText="Pick" ItemStyle-Width="50">
											<ItemTemplate>
												<asp:HyperLink ID="hplPick" runat="server" CssClass="ButEmpty" Text="Recv."  NavigateUrl='<%#DataBinder.Eval(Container.DataItem,"StockInLink") %>' />
											</ItemTemplate>
										</asp:TemplateField>
                                        										
                                    </Columns>


                                </asp:GridView>
                                </ItemTemplate>
                           
                        </asp:TemplateField>                    
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
        </div>
</asp:Content>
