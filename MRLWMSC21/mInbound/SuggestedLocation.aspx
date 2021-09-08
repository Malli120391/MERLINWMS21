<%@ Page Title=" Suggested Location :." Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="SuggestedLocation.aspx.cs" Inherits="MRLWMSC21.mInbound.SuggestedLocation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IBContent" runat="server">
    <script type="text/javascript" src="../mManufacturingProcess/Scripts/ben_Print.js"></script>
    <style>
        .module_login {
            border-right: 0px solid #1a79cf;
            border-left: 0px solid #1a79cf;
        }

        table th table th{
            border:0px !important;
            box-shadow:unset !important;
        }

        .gvLightBlueNew td div table td{
            border:0px !important;
            box-shadow:unset !important;
        }
    </style>
    <script>
        function printData()
        {
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
    <table width="100%" class="internalData" cellpadding="4" cellspacing="4">

       <tr>

            <td align="left" >
                <%--style="visibility:hidden;" --%>
                <img id="Img1"  runat="server" enableviewstate="false"  src="~/Images/Logo_Header_falcon2.png" border="0" alt="">
            </td>
            <td align="center" >
                <asp:Label runat="server" Text="Suggested Location" ID="lblHeader" CssClass="SubHeading3" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td> 
            <td align="right" valign="bottom" class="right">
                        
                <img width="20" height="20" alt="Print" style="visibility:hidden;" class="NoPrint" src="../Images/blue_menu_icons/printer.png" onclick="javascript:printDiv('tdDDRPrintArea');" border="0" style="cursor:pointer;" />

                <asp:LinkButton runat="server" ID="lnkbackToList" CssClass="btn btn-primary NoPrint" PostBackUrl = "../mInbound/InboundTracking.aspx" >Back to List<%=MRLWMSC21Common.CommonLogic.btnfaList %></asp:LinkButton>
                    &nbsp;&nbsp; &nbsp;&nbsp;<a href="#" style="text-decoration:none;" onclick="javascript:printDiv('tdDDRPrintArea');" class="btn btn-primary NoPrint">Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %></a> 

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
       <%-- <tr>
            <td colspan="1">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblStoreRefNo" runat="server" /><div style="height:15px"></div>
                <asp:Label ID="lblBarStoreRefNo" runat="server" />
                        </td>
                        <td align="right">
                            <table cellpadding="1" cellspacing="1" border="0" width="100%">
                                        <tr><td>Tenant &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: </td> <td><asp:Literal runat="server" ID="ltTenant" /> </td></tr>
                                        <tr><td>Doc. Date &nbsp;&nbsp;&nbsp;&nbsp;: </td> <td><asp:Literal runat="server" ID="ltDocDate" /> </td></tr>
                                        <tr><td>Supplier &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: </td> <td><asp:Literal runat="server" ID="ltSupplier" /> </td></tr>
                                        <tr>
                                            <td colspan="2" class="NoPrint" align="">
                                                <asp:Panel ID="pnlSearchMaterial" runat="server" DefaultButton="lnkSearchMaterial">
                                                    
                                                    <asp:TextBox ID="txtMcode" runat="server"  SkinID="txt_Hidden_Req_Auto"  onfocus="ClearText(this)" PlaceHolder="Search Part# ..."  onblur="javascript:focuslost1(this)" Width="160"    /> 
                                                    &nbsp;
                                                    <asp:LinkButton ID="lnkSearchMaterial" runat="server"   OnClick="lnkSearchMaterial_Click"  CssClass="ui-btn ui-button-large"  > Search <span class="space fa fa-search"></span></asp:LinkButton>

                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                        </td>
                    </tr>
                </table>
                
            </td>
            
        </tr>--%>
        <tr>
            <td colspan="3">
                <asp:GridView ID="gvInvoiceList" runat="server"  SkinID="gvLightBlueNew" AutoGenerateColumns="false"
                    OnRowDataBound="gvInvoiceList_RowDataBound"
                    >
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
						            <th width="35px">Line No.</th><th Width="205px">Part No.</t><th Width="80px"> BUoM / Qty. </th><th Width="70px">PUoM / Qty.</th><th Width="55px">Inv. UoM / Qty.</th>
                                    <th Width="50px">PO Qty.</th><th Width="34px">Inv. Qty.</th><th width="60px">Initiated Qty.</th><th>
                                                                                <table><tr>
                                                                                    <th Width="121px">Location</th><th Width="64px"">Sug. Qty.</th><th Width="67px">Receive</th>
								                                                </tr></table>
                                                                            </th>
                                        
					            </tr></table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:GridView ID="gvLineItems"  runat="server" SkinID="gvLightBlueNew" OnRowDataBound="gvLineItems_RowDataBound"  ShowHeader="false" ShowFooter="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Line No." ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltLineItem" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"LineNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Material Code" ItemStyle-Width="205px">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltMaterialCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"MaterialCode") %>' /><br />
                                                <asp:Literal ID="ltMaterialDesc" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"MDesc") %>' />
                                                <asp:Literal runat="server" ID="ltBarcode" Text='<%# String.Format("<img width=\"200px\" src=\"Code39Handler.ashx?code={0} \"",DataBinder.Eval(Container.DataItem, "MaterialCode")) %>'/>&nbsp;&nbsp;&nbsp;&nbsp;
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <%--<asp:TemplateField HeaderText="Material Desc" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText=" BUoM / Qty. " ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltBase_UOM" runat="server" Text='<%#String.Format("{0} / {1}",DataBinder.Eval(Container.DataItem,"Base_UOM"),DataBinder.Eval(Container.DataItem,"Base_UOM_QTY")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="PUoM / Qty." ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltPO_UOM" runat="server" Text='<%#String.Format("{0} / {1}",DataBinder.Eval(Container.DataItem,"PO_UOM"),DataBinder.Eval(Container.DataItem,"PO_UOM_QTY")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="INV. UOM / Qty." ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltINV_UOM" runat="server" Text='<%#String.Format("{0} / {1}",DataBinder.Eval(Container.DataItem,"INV_UOM"),DataBinder.Eval(Container.DataItem,"INV_UOM_QTY")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        
                                        <asp:TemplateField HeaderText="PO Qty." ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltQTY" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PO_QTY") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Inv. Qty." >
                                            <ItemTemplate>
                                                <asp:Literal ID="ltINV_QTY" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"INV_QTY") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Initiated Qty." ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltinitiatedQTY" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"processedDocQty") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:GridView runat="server" ID="gvSuggestLocation" ShowHeader="false" ShowFooter="false" SkinID="gvLightBlueNew" 
                                                    OnRowDataBound="gvSuggestLocation_RowDataBound"
                                                    >
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Location" ItemStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltLocationCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"LocationCode") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sug. Qty." ItemStyle-Width="40px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltQTY" runat="server" Text='<%#(Eval("QTY").ToString().Equals("0")?"":Eval("QTY")) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pick" ItemStyle-Width="30px">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hplPick" runat="server" CssClass="ButEmpty" Text="Recv."/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
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
