<%@ Page Title=" QC Pending List  :." Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="ReceiptConfirmation.aspx.cs" Inherits="MRLWMSC21.mInbound.ReceiptConfirmation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IBContent" runat="server">
    
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="inboundtracking" SupportsPartialRendering="true"></asp:ScriptManager>
    <script type="text/javascript" src="../Scripts/jQuery2/countdown/jquery.countdown.js"></script>

   <script  type="text/javascript" >
       $(function () {

           var activeIndex = parseInt($('#<%=hidAccordionIndex.ClientID %>').val());

                 $("#accordion").accordion({
                     autoHeight: false, clearStyle: true,
                     active: activeIndex,
                     change: function (event, ui) {
                         var index = $(this).children('h3').index(ui.newHeader);
                         $('#<%=hidAccordionIndex.ClientID %>').val(index);
                 }


             });
                 $("#accordion").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });
             


             });
    </script>


    <style type="text/css">
     /*.ui-autocomplete-loading {
             background: white url('../Images/ui-anim_basic_16x16.gif') right center no-repeat;
         }*/

    </style>

    <script  type="text/javascript">
         function OpenImage(path) {
             window.open(path, 'Naresh', 'height=800,width=900');
         }


         function ClearText(TextBox) {
             if (TextBox.value == "Search Store Ref.# ...")
                 TextBox.value = "";

             TextBox.style.color = "#000000";
         }

         function focuslost1(TextBox) {
             if (TextBox.value == "")
                 TextBox.value = "Search Store Ref.# ...";

             TextBox.style.color = "#A4A4A4";
         }


         function CollapseAll() {
             $("#collapseAll").click(function () {
                 $(".ui-accordion-content").hide()
             });
         }

         function ExpandAll() {
             $("#expandAll").click(function () {
                 $(".ui-accordion-content").show()
             });
         }

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

                 var textfieldname = $('#<%= txtVerStoreRefNo.ClientID %>');
                 DropdownFunction(textfieldname);

                 $('#<%= txtVerStoreRefNo.ClientID %>').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadInwdQCStoreRefNumbers") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == null || data.d == "") {
                                    showStickyToast(true, "No store ref.# is available for \'Inward QC Pending List\'");
                                } else {
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
                    minLength: 0
                });

            });
        }
        fnMCodeAC();
    </script>
    <div class="dashed"></div>
<div class="pagewidth">

     <table border="0" cellspacing="4" cellpadding="0" align="center" >

        <tr>
            <td>
                <br /><br />
            </td>
        </tr>
        <tr>
            <td align="center">

                 <div id="accordion" align="left">
                 
                     
                     <h3> Inward QC Pending List  <span id="lblVerificationRecCout" runat="server" />   </h3>
                     <div>
                         <!-- Verification In Process -->

                         <table border="0" width="100%" cellpadding="0" cellspacing="0" align="center">

                             <tr>
                                 <td align="right" class="FormLabels">
                                     <br />
                                    <asp:Panel runat="server" ID="pnlinwdsearch" DefaultButton="lnkVerifSearch">
                                        
                                        
                                     <asp:TextBox ID="txtVerStoreRefNo" runat="server" Text="Search Store Ref.# ..."  SkinID="txt_Hidden_Req_Auto" onfocus="ClearText(this)"  onblur="javascript:focuslost1(this)"></asp:TextBox>

                                        &nbsp;&nbsp;
                                     <asp:LinkButton runat="server" ID="lnkVerifSearch"  OnClick="lnkVerifSearch_Click" CssClass="ui-btn ui-button-large" >Search <span class="space fa fa-search"></span></asp:LinkButton>

                                        &nbsp;&nbsp;

                                     <asp:ImageButton ID="imgbtngvVIProcess" runat="server"  ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvVIProcess_Click" ToolTip="Export To Excel" />
                                     
                                    </asp:Panel>
                                     <br />
                                 </td>
                             </tr>
                             <tr>
                                 <td class="FormLabels">
                                     
                                     <asp:Label ID="lblVerificationStatus" runat="server"></asp:Label>
                                 </td>
                             </tr>
                             <tr>
                                 <td>

                                     <asp:GridView Width="97%" ShowFooter="false"   ID="gvVIProcess" runat="server" PagerSettings-Position="Bottom" AllowPaging="true" PageSize="10"  SkinID="gvLightGrayNew" HorizontalAlign="Left"  OnRowDataBound="gvVIProcess_RowDataBound" OnSorting="gvVIProcess_Sorting" OnPageIndexChanging="gvVIProcess_PageIndexChanging" >
    								    
    								     <Columns>
                                              
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Store Ref. #"  ItemStyle-CssClass="gvOBDNumber">
                                                    <ItemTemplate>
                                                   
                                                        <asp:Literal runat="server" ID="ltStoreRefNo" Text='<%# GetStoreRefNoWithLink(DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString()) %>'/>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField ItemStyle-Width="150" HeaderText="Shipment Type"  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltShipType" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentType").ToString() %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                                                                
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Doc. Rcvd. Dt."  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltDocRdvDate" Text='<%# DataBinder.Eval(Container.DataItem, "DocReceivedDate","{0: dd/MM/yy}") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Ship. Rcvd. Dt."  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltShipmentRecivedDate" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentReceivedOn","{0: dd/MM/yy}") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField ItemStyle-Width="250" HeaderText="Supplier"  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltSupplier" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                    
                                                    <asp:TemplateField ItemStyle-Width="200" HeaderText="Store(s)"  ItemStyle-CssClass="home">
                                                     <ItemTemplate>
                                                      <asp:Literal runat="server" ID="ltSoreAssociated" Text='<%# MRLWMSC21Common.CommonLogic.GetSearchStoreNamesWithVerificationStatus(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br/>",DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "InboundID").ToString(),cp.TenantID.ToString() ) %>'/>
                                                      <asp:Literal runat="server" ID="ltShipmentLocation" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentLocation") %>'/>
                                                    </ItemTemplate>
                                                    
                                                    </asp:TemplateField>
                                                        
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="Status"  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# MRLWMSC21Common.CommonLogic.GetShipmentStatus((DataBinder.Eval(Container.DataItem, "InBoundStatusID").ToString())) %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                <asp:TemplateField   HeaderText="RTR"  HeaderStyle-Width="200" ItemStyle-Width="150">
                                                     <ItemTemplate>
                                                     <a id="A1"   class="helpWTitle" Title="Receiving Tally Report(RTR) | Receiving Tally Report with barcoded material codes to receive items for putaway."  runat="server" Visible="true" href='<%# String.Format("ReceiveTallyReport.aspx?ibdid={0}&lineitemcount={1}", DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem, "LineCount").ToString()) %>'>
                                                         RTR <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                     </a>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                <asp:TemplateField  ItemStyle-CssClass="home">
                                                     <ItemTemplate>
                                                      <asp:LinkButton ID="lnkEditInbound" runat="server" Visible="false" CssClass="GvLink" PostBackUrl ='<%# Eval("InboundID", "InboundDetails.aspx?ibdid={0}&edittype=3") %>'  Text="<nobr> Update <img src='../Images/redarrowright.gif' border='0' /></nobr>" ToolTip="Receive Shipment for Verification" />

                                                          <asp:HyperLink ID="HyperLink1" Text="<nobr> Change <img src='../Images/redarrowright.gif' border='0' /></nobr>" NavigateUrl='<%#  Eval("InboundID", "InboundDetails.aspx?ibdid={0}&edittype=3")  %>' Font-Underline="false" runat="server" ToolTip="Receive Shipment for Verification"></asp:HyperLink>
                                                    </ItemTemplate>

                                                </asp:TemplateField>                          
                                            </Columns>    
                      
                                    </asp:GridView>


                                 </td>
                             </tr>
                         </table>

                         <!-- Verification In Process -->
                     </div>


                 </div>

            </td>
        </tr>
    </table>



</div>




    <asp:HiddenField ID="hidAccordionIndex" runat="server" Value="0" />

</asp:Content>
