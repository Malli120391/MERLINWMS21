<%@ Page Title=" .: Delivery Pick Note :." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="DeliveryPickNote.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.DeliveryPickNote" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">
      <script type="text/javascript" src="Scripts/ben_Print.js"></script>
<%--    <LINK href="../PrintFile.css"  type="text/css" rel="stylesheet" media="print">--%>
    <asp:ScriptManager runat="server" ID="smngrDPN" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>

    <script type="text/javascript">
        //$('.BarCodeCell').css("min-width", "350px");

        function printDiv(divName) {
            var Gcount = '<%=this.gvPOLineQty.Rows.Count%>';
            var time = 100;
            if (Gcount > 100)
            { time = 75 } else
                if (Gcount > 500)
                { time = 50 }
            //$(".PrintListcontainer").print();
            var panel = document.getElementById("PrintPanel");
            var printWindow = window.open('', '', 'height=400,width=800,scrollbars=1,location=1,status=1,resizable=1');
            printWindow.document.write('<html><head><title>Delivery Pick Note</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.write('<LINK href="../PrintStyle.css"  type="text/css" rel="stylesheet" media="print">');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();// printWindow.close();
            }, time * Gcount);
            
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

                try
                {
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
                                    if (data.d == "" || data.d == null) {
                                        showStickyToast(true, "No \'Part Number\' is available");
                                    }
                                    else {
                                    response($.map(data.d, function (item) {
                                        return {
                                            label: item.split('`')[0],
                                            description: item.split('`')[1] == undefined ? "" : " <font color='#086A87'>" + item.split('`')[1] + "</font>"
                                        }
                                    }))
                                    }
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
         body    {
            overflow:scroll;

        }
       .gvSilver table {
    table-layout:fixed;
}
       .gvSilver table td {
            overflow: hidden;
        }
        .BarCodeCell {
            min-width:310px;
        }
   </style>
    

       
    <div class="wrapper1">
    <div class="divUP">
    </div>
</div>

    <div class="wrapper2">
    <div class="divdown" id="PrintPanel">

      

        
         
            <table border="0" cellpadding="2" cellspacing="2" align="center" width="98%" id="tdDDRPrintArea" style="margin:20px;">
            <thead> 
                <tr> 
                    
                        <td colspan="3" align="right" valign="bottom" class="NoPrint">
                            <br />
                            <asp:LinkButton runat="server" ID="lnkbackToList" CssClass="ui-button-small" PostBackUrl = "../mOutbound/OutboundTracking.aspx" >Back to List<%=MRLWMSC21Common.CommonLogic.btnfaList %></asp:LinkButton>
                            &nbsp;&nbsp; &nbsp;&nbsp;<a href="#" style="text-decoration:none;" onclick="javascript:printDiv('tdDDRPrintArea');" class="ui-button-small">Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %></a> &nbsp; <%--<img src="../Images/redarrowright.gif"  border="0" />--%>
                        </td>
                </tr>
                <tr>
                    <td  colspan="3">
                        <table  cellpadding="3" cellspacing="3" border="0" width="95%">
                            <tr>
                                <%--style="visibility:hidden;"--%>
                                <td> <img id="Img1"  runat="server"   src="~/Images/RT_Logo_icon.jpg"  width="80"    border="0" alt=""></td>
                                <td align="center"><font  size="6"> Delivery Pick Note &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></td>
                                <td align="right"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr style="height:0.5px;color:#000;border-color:#000;background-color:#000;"/>
                    </td>
                </tr>

                <tr>
                    <td><br /> <asp:Literal ID="ltDelvDocNo" runat="server" /> <br /></td>
                    <td align="right" colspan="2"> <asp:Literal  ID="ltDelvDocDetails" runat="server" /></td>
                </tr>
            </thead> 
            

            <tbody>


                <tr>  
                    <td colspan="3" align="right"> <asp:Label ID="lblStatusMessage" runat="server" CssClass="ErrorMsg" /> <br /><br /></td>
                </tr>
                <tr>
                    <td class="NoPrint" >
                        <asp:Literal runat="server" ID="ltPNCPRecordCount" />
                    </td> 
                    <td align="right" colspan="2" class="NoPrint"> 
                       
                            <asp:TextBox ID="txtMCode"  Text="Search Part # ..." SkinID="txt_Hidden_Req_Auto"  runat="server" onfocus="clear(this)" onblur="javascript:focuslost1(this);"  />&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkMCodeSearch" runat="server"    OnClick="lnkMCodeSearch_Click" CssClass="ui-btn ui-button-large"> Search <%= MRLWMSC21Common.CommonLogic.btnfaSearch %> </asp:LinkButton>
                       
                    </td>
                </tr>
   
   
      <tr>
         <td valign="top" align="center" colspan="3" > 
       
             <asp:Panel runat="server"  >
                 
                            <asp:GridView Width="100%" CssClass="gvSilver" CellPadding="1" CellSpacing="1" ShowFooter="true" GridLines="Both" BackColor="#999999"    ID="gvPOLineQty" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="false"  AllowSorting="true"  HorizontalAlign="Left" OnRowCreated="gvPOLineQty_RowCreated"  OnRowCommand="gvPOLineQty_RowCommand"  OnSorting="gvPOLineQty_Sorting" OnPageIndexChanging="gvPOLineQty_PageIndexChanging" OnRowDataBound="gvPOLineQty_RowDataBound"  >
          
            <Columns>
              <asp:TemplateField HeaderText="Line #"   ItemStyle-CssClass="LineNoCell" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="ltLineNumber" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber","{0:000}") %>'/>
                    
                    <asp:Literal runat="server" ID="hidKitPlannerID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "KitPlannerID") %>'/>
                </ItemTemplate>
             </asp:TemplateField>  
             
             <asp:BoundField DataField="KitPlannerID" HeaderText="KitPlannerID" SortExpression="KitPlannerID" Visible="false" />
             <asp:BoundField DataField="ParentMcode" HeaderText="ParentMcode" SortExpression="ParentMcode" Visible="false" />
             
              <asp:TemplateField HeaderText="Part Number"  ItemStyle-CssClass ="BarCodeCell" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                <asp:Label runat="server" CssClass="BarCodetext" ID="lblMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>'/> -<asp:Literal runat="server" ID="ltItemDesc" /><br /> 
                <asp:Literal runat="server" ID="ltBarCodeMCode" Text='<%# String.Format("<img width=\"300px\" src=\"../mInbound/Code39Handler.ashx?code={0}\"",DataBinder.Eval(Container.DataItem, "MCode")) %>'/>&nbsp;&nbsp;&nbsp;<br /><br />
                    <asp:Literal runat="server" ID="ltOEMPartNo" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>'/> <br />
                
                <br />
               
                    <asp:Literal runat="server" ID="ltMMID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID") %>'/>
                    <asp:Literal runat="server" ID="ltOBDTrackingID" Visible="false" />
                    <asp:Literal runat="server" ID="ltSOHID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SOHeaderID") %>'/>
                    
                    
                </ItemTemplate>
                
              
             </asp:TemplateField>  
             
              <asp:TemplateField ItemStyle-Width="20" HeaderText="Base UoM"   ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="ltBMMSKU" Text='<%# String.Format("{0}/{1}", DataBinder.Eval(Container.DataItem, "BUoM").ToString(),DataBinder.Eval(Container.DataItem, "BUoMQty","{0:0.00}").ToString() )%>'/>
                    <asp:Literal runat="server" ID="ltBUoMID" Visible="false" />
                    <asp:Literal runat="server" ID="ltBUoMQty" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "BUoMQty","{0:0.00}") %>'/>
                </ItemTemplate>
             </asp:TemplateField>  
             
                       
             
              <asp:TemplateField ItemStyle-Width="30" HeaderText="Sales UoM"   ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="ltMMSKU" Text='<%# String.Format("{0}/{1}", DataBinder.Eval(Container.DataItem, "SUoM").ToString(),DataBinder.Eval(Container.DataItem, "SUoMQty","{0:0.00}").ToString() )%>'/>
                    <asp:Literal runat="server" ID="ltSUoMID" Visible="false" />
                    <asp:Literal runat="server" ID="ltSUoMQty" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SUoMQty","{0:0.00}") %>'/>
                </ItemTemplate>
             </asp:TemplateField>  
             
             <asp:TemplateField ItemStyle-Width="20" HeaderText="Delv. Doc. Qty."  ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="ltQuanity" Text='<%# DataBinder.Eval(Container.DataItem, "SOQuantity","{0:0.00}") %>'/>
                </ItemTemplate>
             </asp:TemplateField> 
             
             
               <asp:TemplateField  ItemStyle-Width="990" HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="ltSplitLocation" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SplitLocation") %>'/>
                    <asp:Literal runat="server" ID="ltLocation" Visible="true" />
                    <asp:Literal runat="server" ID="ltLocationID" Visible="false" />
                </ItemTemplate>
                <FooterTemplate>
                Note: Strike-off row items cannot be picked due to their parameters difference than the required items in Delv.Doc.
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

             </asp:Panel>
             
    </td></tr>
  	         	        	
  	      </tbody>  		

        </table>
        
        </div>
        </div>

</asp:Content>
