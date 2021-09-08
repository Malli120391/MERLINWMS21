<%@ Page Title="" Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="OptimalPickPathLibrary.aspx.cs" Inherits="MRLWMSC21.mOutbound.OptimalPickPathLibrary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
    <style type="text/css">
        .OnlyForPrint {
            visibility: hidden;
            display: none;
        }
        .Strikeoff {
            text-decoration:line-through;
        }

        table {
            border:0px;
        }

        .scroll {
       width: 84vw;
    /*transition: all 0.8s;*/
    overflow-x:auto;
}

.scrollDexced {
        width: 92vw !important;
         overflow:auto;
}
    </style>

    <script>
        function printDiv(divName) {
            var Gcount = '<%=this.tblJRP.Rows.Count%>';
            var time = 100;
            if (Gcount > 100)
            { time = 75 } else
                if (Gcount > 500)
                { time = 50 }
            //$(".PrintListcontainer").print();
            var panel = document.getElementById("PrintPanel");
            var printWindow = window.open('', '', 'location=0, status=0, resizable=1, scrollbars=1, width=800, height=400');
            printWindow.document.write('<html><head><title>Delivery Pick Note</title>');
            printWindow.document.write('<LINK href="../PrintStyle.css"  type="text/css" rel="stylesheet" media="print">');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');

            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();// printWindow.close();
            }, time * Gcount);
        }

        function focuslost(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search Part Number...";

            TextBox.style.color = "#A4A4A4";

        }

        function onFocus(TextBox) {
            if (TextBox.value == "Search Part Number...")
                TextBox.value = "";
            TextBox.style.color = "#000000";
        }


        //function fnLoadMCode() {
        $(document).ready(function () {

            try {
                var textfieldname = $("#<%= this.txtMCode.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.txtMCode.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDPNoteMCodeOEMData") %>',
                                data: "{ 'prefix': '" + request.term + "', 'OutboundID': '" + '<%= MRLWMSC21Common.CommonLogic.QueryString("obdid") %>' + "' }",
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
            //}
            //fnLoadMCode();
    </script>

    <div class="dashed"></div>
    <div class="divdown pagewidth" id="PrintPanel">

        <link href="../PrintStyle.css" type="text/css" rel="stylesheet" media="print">
        <div class="">
        <table border="0" cellpadding="2" cellspacing="2" width="1300px" align="center" id="tdDDRPrintArea" runat="server" style="padding: 10px;">
            <thead>
                <tr>

                    <td colspan="3" align="right" valign="bottom" class="NoPrint">
                        <div class="right">
                        <asp:LinkButton runat="server" ID="lnkbackToList" CssClass="btn btn-primary" PostBackUrl="OutboundTracking.aspx">Back to List<%=MRLWMSC21Common.CommonLogic.btnfaList %></asp:LinkButton>
                        &nbsp;&nbsp; &nbsp;&nbsp;<a href="#" onclick="javascript:printDiv('divdown');" class="btn btn-primary">Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %></a> &nbsp; <%--<img src="../Images/redarrowright.gif"  border="0" />--%>
                    </div></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table cellpadding="3" cellspacing="3" border="0" width="90%">
                            <tr>
                                <td>
                                    <img id="Img1" runat="server" src="~/Images/Logo_Header_falcon2.png" width="150" border="0" alt="">
                                </td>
                                <td align="center"><font size="6"> Optimal Pick Path &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></td>
                                <td align="right"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr style="height: 0.5px; color: #000; border-color: #000; background-color: #000;" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3"></td>

                </tr>
                <tr>
                    <td>
                        <br />
                        <asp:Literal ID="ltDelvDocNo" runat="server" />
                        <br />

                    </td>
                    <td align="right" colspan="2">
                        <asp:Literal ID="ltDelvDocDetails" runat="server" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkRestickQty" runat="server" TextAlign="Right" AutoPostBack="true" OnCheckedChanged="chkRestickQty_CheckedChanged" Text="Suggested Pick Note" />
                    </td>
                    <td colspan="2" align="right">
                        <div class="flex__">
                            <div><asp:TextBox ID="txtMCode" runat="server" Text="Search Part Number..."  SkinID="txt_Hidden_Req_Auto" onfocus="onFocus(this)" onblur="javascript:focuslost(this);"></asp:TextBox></div>&nbsp;&nbsp;&nbsp;
                            <div><asp:LinkButton ID="lnkMCodeSearch" runat="server" CssClass="NoPrint btn btn-primary" OnClick="lnkMCodeSearch_Click">Search<span class="space fa fa-search"></span></asp:LinkButton></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table id="tblJRP" runat="server" class="DynamicTable" border="1" width="100%" style="border-collapse: collapse;">
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div class="scroll">
                        <asp:GridView ID="gvOptimalPickPath" runat="server" AutoGenerateColumns="false" PageSize="100" Width="100%"  AllowPaging="true" CssClass="gvSilver" CellPadding="1" CellSpacing="1" ShowFooter="true" GridLines="Both"   
                            OnRowDataBound="gvOptimalPickPath_RowDataBound" >
                             <Columns>
                                 <asp:TemplateField HeaderText="Location" HeaderStyle-Width="80" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Label  ID="ltLocation" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"LocCode" )%>'/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line No." ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:Label ID="ltLineno" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"LineNo" )%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Number" ItemStyle-Width="320">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltMcode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"MaterialCode" )%>'/>
                                        <br />
                                        <asp:Literal ID="ltMaterialDesc" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"MaterialDesc") %>'/><br />
                                        <asp:Literal runat="server" ID="ltBarCodeMaterialCode" Text='<%# String.Format("<img width=\"150px\" src=\"../mInbound/Code39Handler.ashx?code={0}\"",DataBinder.Eval(Container.DataItem, "MaterialCode")) %>'/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Part Desc." ItemStyle-Width="320" Visible="false">
                                    <ItemTemplate>
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 
                                                
                                <asp:TemplateField HeaderText="SUoM/ Qty." ItemStyle-Width="90">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltSuomQty" runat="server" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem,"SOUOM"),DataBinder.Eval(Container.DataItem,"SOUOMQTY"))%>'	/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Del. Doc. Qty." ItemStyle-Width="90"> 
                                    <ItemTemplate>
                                        <asp:Literal ID="ltDelDocQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SOQTY" )%>'/>
                                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Kit ID" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:Label ID="lbKitID" runat="server" Text='<%#(DataBinder.Eval(Container.DataItem,"KitPlannerId").ToString()=="0"?"":DataBinder.Eval(Container.DataItem,"KitPlannerId"))%>'/>
                                        <asp:Literal ID="ltDynamicMsps" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Dam." ItemStyle-Width="60">
                                    <ItemTemplate>
                                        
                                        <asp:Literal ID="ltDamage" runat="server" Text='<%#(DataBinder.Eval(Container.DataItem,"IsDamaged").Equals(true)?"<img src=\"../Images/blue_menu_icons/check_mark.png\">":"")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dis." ItemStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltDisc" runat="server" Text='<%#(DataBinder.Eval(Container.DataItem,"HasDiscrepancy").Equals(true)?"<img src=\"../Images/blue_menu_icons/check_mark.png\">":"")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                 
                                <asp:TemplateField HeaderText="Avl. Qty." ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Label ID="ltAvailbleQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"QTY")%>'/>
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Picked Qty." ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Label ID="ltPickedQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"InitiatedQTY")%>' />

                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Sug. Qty." ItemStyle-Width="80" Visible="false">
                                     <ItemTemplate>
                                         <asp:Label ID="ltSuggestedQTY" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SuggestedQTY")%>'/>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                <asp:TemplateField Visible="false" HeaderText="Pickable Qty." ItemStyle-Width="100">
                                    
                                    <ItemTemplate>
                                        <asp:Label ID="ltPickableQty" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem,"SuggestedQTY")%>'/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField ItemStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlPicklink" runat="server" CssClass="ButEmpty" NavigateUrl='<%#String.Format("../mInventory/PickItem.aspx?soid={0}&mmid={1}&obdid={2}&LineNum={3}&locid={4}&kitid={5}",DataBinder.Eval(Container.DataItem,"SOId"),DataBinder.Eval(Container.DataItem,"MaterialId"),DataBinder.Eval(Container.DataItem,"OutboundId"),DataBinder.Eval(Container.DataItem,"LineNo"),DataBinder.Eval(Container.DataItem,"LocId"),(DataBinder.Eval(Container.DataItem,"KitPlannerId").ToString()=="0"?"null":DataBinder.Eval(Container.DataItem,"KitPlannerId")))%>'  Text="Pick" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 
                             </Columns>
                             <FooterStyle CssClass="gvSilver_footerGrid" />
                                            <RowStyle CssClass="gvSilver_DataCellGrid" />
                                            <EditRowStyle CssClass="gvSilver_DataCellGridEdit" />
                                            <PagerStyle CssClass="gvSilver_pagerGrid" />
                                            <HeaderStyle CssClass="gvSilver_headerGrid" />
                                            <AlternatingRowStyle CssClass="gvSilver_DataCellGrid" />
                        </asp:GridView>
                            </div>
                    </td>
                </tr>
            </thead>
        </table>
            </div>
    </div>


</asp:Content>
