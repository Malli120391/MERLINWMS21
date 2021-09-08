<%@ Page Title="Job Order List:." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="ProductionOrderList.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.ProductionOrderList" EnableEventValidation="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="ss" SupportsPartialRendering="true"></asp:ScriptManager>

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

                $(document).ready(function () {
                    var textfieldname = $("#<%= this.txtKitCode.ClientID %>");
                    DropdownFunction(textfieldname);
                    $('#<%=txtKitCode.ClientID%>').autocomplete({
                        source: function (request, response) {

                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetKitCodeList") %>',
                        data: "{ 'Prefix': '" + request.term + "','Type':'1'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[0]
                                }
                            }))

                        }

                    });
                },
                minLength: 0
                     });




                    var textfieldname = $("#<%= this.txtPRORefNoSearch.ClientID %>");
                    DropdownFunction(textfieldname);
                    $("#<%= this.txtPRORefNoSearch.ClientID %>").autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPRONumber") %>',
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
                        minLength: 0
                    });
                });
            })
        }
        fnMCodeAC();

        function ClearText(TextBox,Text) {
            if (TextBox.value == Text) {
                TextBox.value = "";
                TextBox.style.color = "#000000";
            }
        }
        function focuslost(TextBox,Text) {
            if (TextBox.value == "") {
                TextBox.value = Text;
                TextBox.style.color = "#A4A4A4";
            }
        }
    </script>

    <style type="text/css"> 
			.showonhover .hovertext { display: none;}
            .showonhover:hover .hovertext {display: inline;}
            div.viewdescription {color:#999;}
            div.viewdescription:hover {background-color:#999; color: White;}
            .hovertext {position:absolute;z-index:1000;border:2px solid #ffd971;background-color:#9cb70f;padding:5px;width:120px;font-size: 15px;}
        .InternalorderList {
             text-decoration:none;
        }
		</style>

   <%-- <script type="text/javascript">

          $(document).ready(function () {
              $("#divProdDefcList").dialog({
                  autoOpen: false,
                  modal: true,
                  minHeight: 400,
                  minWidth: 500,
                  width: 'auto',
                  resizable: true,
                  draggable: true,
                  open: function (event, ui) { $(this).parent().appendTo("#divProdDefcListContainer"); }
              });
          });
          function ProdBlockDialog() {

              //block it to clean out the data
              $("#divProdDefcList").block({
                  message: '<img src="<%=ResolveUrl("~") %>Images/async.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });
        }
        function ProdCloseDialog() {

            //Could cause an infinite loop because of "on close handling"
            $("#divProdDefcList").dialog('close');

        }

        function ProdOpenPrintDialog() {
            $("#divProdDefcList").dialog({
                autoOpen: false,
            });
        }
        function ProdOpenDialog() {

            $("#divProdDefcList").dialog('open');
            $("#divProdDefcList").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async.gif" />',
                 css: { border: '0px' },
                 fadeIn: 0,
                 fadeOut: 0,
                 overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
             });
             ProdUnblockDialog();

         }

         function ProdUnblockDialog() {
             $("#divProdDefcList").unblock();
         }
    </script>--%>

    <script language="javascript" type="text/javascript">
        function divexpandcollapse(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);
            if (div.style.display == "none") {
                div.style.display = "inline";
                img.src = "../minus.gif";
            } else {
                div.style.display = "none";
                img.src = "../plus.gif";
            }
        }
</script>

   
    <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlPROList" UpdateMode="Always">
        <Triggers>
            <asp:PostBackTrigger ControlID="imgbtngvPROList" />
        </Triggers>
        <ContentTemplate>

   <table align="center" class="ListTable" width="100%" cellpadding="3" cellspacing="3">
         
        <tr class="ListHeaderRow" >
                
            <td class="FormLabels" align="right" colspan="2">
                <asp:Panel ID="pnlJobList" runat="server" DefaultButton="lnkGetData">
                  
                            Status:
                            <asp:DropDownList ID="ddlStatus" runat="server"  Width="150" Height="34"/>
                            &nbsp;&nbsp;
                            Routing Type:
                            <asp:DropDownList ID="ddlRoutingType" runat="server"  Width="150" Height="34"/>
                            &nbsp;&nbsp;
                            <asp:TextBox runat="server" SkinID="txt_Auto" Width="150"  Text="Kit Code..." onblur="javascript:focuslost(this,'Kit Code...')" onfocus="ClearText(this,'Kit Code...')" ID="txtKitCode"></asp:TextBox>
                            &nbsp;&nbsp;
                            <asp:TextBox runat="server" SkinID="txt_Auto"  Width="150" Text="Job Order Ref. #..." onblur="javascript:focuslost(this,'Job Order Ref. #...')" onfocus="ClearText(this,'Job Order Ref. #...')" ID="txtPRORefNoSearch"></asp:TextBox>
                            <asp:LinkButton  ID="lnkGetData"  CssClass="ui-btn ui-button-large"  runat="server"  OnClick="lnkGetData_Click" >Search<%=MRLWMSC21Common.CommonLogic.btnfaSearch %></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            
                </asp:Panel>
            </td>
        </tr>
       
           
          <tr>
              <td align="right">
                  <div id="JOD" style="width:30px;height:10px;background-color:#ef4a4a;"></div>
              </td>
              <td >     
                       <span class="SubHeading"> Indicates job order has insufficient materials to release</span>
                                  
            </td>
              
          </tr>
       <tr>
           <td align="right" colspan="2">
                  <asp:ImageButton ID="imgbtngvPROList" runat="server"   ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvPROList_Click" ToolTip="Export To Excel" />

              </td>
          </tr>

       <tr>
           <td colspan="2">
               <asp:GridView SkinID="gvLightGreenNew" ID="gvPROList" runat="server" AutoGenerateColumns="false" PagerSettings-Position="TopAndBottom" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvPROList_PageIndexChanging" AllowSorting="True" OnRowDataBound="gvPROList_RowDataBound" >
                   <Columns>
                       <asp:TemplateField HeaderText="Job Order Ref. #" ItemStyle-Width="450">
                           <ItemTemplate>
                               <nobr>
                                                <a href="JavaScript:divexpandcollapse('div<%# DataBinder.Eval(Container.DataItem, "ProductionOrderHeaderID").ToString() %>');">
                                                <img id='imgdiv<%# DataBinder.Eval(Container.DataItem, "ProductionOrderHeaderID").ToString() %>'  style="display:<%# Convert.ToInt16(DataBinder.Eval(Container.DataItem, "ProductionOrderTypeID"))!=7 && DataBinder.Eval(Container.DataItem,"InternalOrdersList").ToString()!="" ?"":"none" %>" width="9px" border="0" src="../plus.gif" />
                                                </a>
                            
                                                    <asp:Literal runat="server" ID="ltInternalOrderList"  Text='<%#  DataBinder.Eval(Container.DataItem, "JobOrderRefNo").ToString() %>' />
                                              </nobr>
                           </ItemTemplate>
                       </asp:TemplateField>


                       <asp:TemplateField HeaderText="Routing Type" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                           <ItemTemplate>
                               <asp:Literal runat="server" ID="ltProductionOrderType" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDocumentType") %>' />
                           </ItemTemplate>
                       </asp:TemplateField>


                       <asp:TemplateField HeaderText="Kit Code" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                           <ItemTemplate>
                               <asp:Literal runat="server" ID="ltkitCode" Text='<%# DataBinder.Eval(Container.DataItem, "KitCode") %>' />
                           </ItemTemplate>
                       </asp:TemplateField>

                       <asp:TemplateField HeaderText="SO Ref. #" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                           <ItemTemplate>
                               <asp:Literal runat="server" ID="ltSONumber" Text='<%# DataBinder.Eval(Container.DataItem, "SO Number") %>' />
                           </ItemTemplate>
                       </asp:TemplateField>

                       <asp:TemplateField HeaderText="Routing Ref. #" ItemStyle-Width="220" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                           <ItemTemplate>
                               <nobr>  
                            <asp:Literal runat="server" ID="ltBOMRefNumber" Text='<%#DataBinder.Eval(Container.DataItem, "Routing Ref #") %>'/>
                            </nobr>
                           </ItemTemplate>
                       </asp:TemplateField>


                       <asp:TemplateField HeaderText="Qty." ItemStyle-Width="70" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                           <ItemTemplate>
                               <asp:Literal runat="server" ID="ltProductionQuantity" Text='<%#DataBinder.Eval(Container.DataItem, "Prod Qty") %>' />
                           </ItemTemplate>
                       </asp:TemplateField>
                       <asp:TemplateField HeaderText="Order Date" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                           <ItemTemplate>
                               <asp:Literal runat="server" ID="ltProductionDate" Text='<%# DataBinder.Eval(Container.DataItem, "ProductionOrderDate","{0:dd/MM/yy}") %>' />
                           </ItemTemplate>
                       </asp:TemplateField>


                       <asp:TemplateField HeaderText="Start Date" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                           <ItemTemplate>
                               <asp:Literal runat="server" ID="ltStartDate" Text='<%# DataBinder.Eval(Container.DataItem, "Start Date","{0:dd/MM/yy}") %>' />
                           </ItemTemplate>
                       </asp:TemplateField>

                       <asp:TemplateField HeaderText="Status" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="left">
                           <ItemTemplate>
                               <asp:Image ID="imgStatus" runat="server" ToolTip='<%# DataBinder.Eval(Container.DataItem, "Prod Order Status") %>' ImageUrl='<%# GetStatusImage(  DataBinder.Eval(Container.DataItem, "ProductionOrderStatusID").ToString()) %>' />
                               <asp:Literal runat="server" ID="ltDueDate" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "Prod Order Status") %>' />
                           </ItemTemplate>
                       </asp:TemplateField>

                       <asp:TemplateField HeaderText="" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false">
                           <ItemTemplate>
                               <asp:LinkButton Font-Underline="false" ID="lnkViewMDeficiency" runat="server" Text="View Deficiency" CommandName="EditChildItems" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ProductionOrderHeaderID") %> ' />
                               <img src='../Images/redarrowright.gif' border='0' />
                           </ItemTemplate>
                       </asp:TemplateField>



                       <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100">
                           <ItemTemplate>
                               <nobr>
                            <a style="text-decoration:none;" href='<%# (Convert.ToInt16(DataBinder.Eval(Container.DataItem, "ProductionOrderTypeID"))!=7?String.Concat("ProductionOrder.aspx?proid=",DataBinder.Eval(Container.DataItem, "ProductionOrderHeaderID").ToString()):String.Concat("PhantomJobOrder.aspx?proid=",DataBinder.Eval(Container.DataItem, "ProductionOrderHeaderID").ToString()))  %>'>Edit  <image src="../Images/redarrowright.gif"></image></a>
                              </nobr>
                           </ItemTemplate>
                       </asp:TemplateField>
                       <asp:TemplateField HeaderText="Job Status" Visible="false">
                           <ItemTemplate>
                               <asp:Literal ID="ltStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Prod Order Status") %>' />
                           </ItemTemplate>
                       </asp:TemplateField>
                       <asp:TemplateField HeaderText="">
                           <ItemTemplate>
                               <tr>
                                   <td colspan="100%">
                                       <div style="display: none;" id='div<%#DataBinder.Eval(Container.DataItem, "ProductionOrderHeaderID").ToString()%>'>
                                           <asp:GridView ID="gvInternalOrderList" ShowFooter="false" runat="server" OnRowDataBound="gvInternalOrderList_RowDataBound" AutoGenerateColumns="false" SkinID="gvLightGreenNew">
                                               <Columns>
                                                   <asp:TemplateField HeaderText="NC Ref. No.">
                                                       <ItemTemplate>
                                                           <asp:HyperLink ID="hylInternalOrder" NavigateUrl='<%# String.Concat("InternalOrder.aspx?ioid=",DataBinder.Eval(Container.DataItem, "InternalOrderHeaderID").ToString()) %>' Font-Underline="false" Text='<%# DataBinder.Eval(Container.DataItem, "IORefNo")  %>' Target="_new" runat="server" />
                                                           <image src="../Images/redarrowright.gif"></image>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>

                                                   <asp:TemplateField HeaderText="Status">
                                                       <ItemTemplate>
                                                           <asp:Literal ID="ltStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>' />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Created On">
                                                       <ItemTemplate>
                                                           <asp:Literal ID="ltCreatedOn" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CreatedOn") %>' />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Created By">
                                                       <ItemTemplate>
                                                           <asp:Literal ID="ltCreatedBy" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CreatedBy") %>' />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Reason for Request">
                                                       <ItemTemplate>
                                                           <asp:Literal ID="ltReason" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Reason") %>' />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                               </Columns>

                                           </asp:GridView>
                                       </div>
                                   </td>
                               </tr>
                           </ItemTemplate>
                       </asp:TemplateField>
                   </Columns>
                   <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;" Mode="NumericFirstLast" PageButtonCount="15" />
               </asp:GridView>

           </td>
       </tr> 

   </table> 

        </ContentTemplate>

    </asp:UpdatePanel> 
     <!-- Production Order Materials Deficiency List Dialog   Developed by Naresh 05/03/2014-->
       <%--  <div id="divProdDefcListContainer">  
         <div id="divProdDefcList" style="display:block;padding:35px;" Title="Material Wise Deficiency">  
            <asp:UpdatePanel ID="upnlProdDefcList" runat="server" ChildrenAsTriggers="true" UpdateMode="Always" >
                <ContentTemplate>
                    
                    
                                                                        <asp:Panel ID= "pnlProdDefcList" runat="server" Width="500px"  HorizontalAlign="Center"  >

                                                                            <table border="0" align="center" cellpadding="4" cellspacing="4" >
                                                                                <tr>
                                                                                    <td class="FormLabelsBlue">Prod. Ref. No. :</td>
                                                                                    <td class="FormLabels"><asp:Label runat="server" ID="lblProdRefNo"></asp:Label></td>
                                                                                    <td class="FormLabelsBlue">BOM Ref. No. :</td>
                                                                                    <td class="FormLabels"><asp:Label runat="server" ID="lblBoMRefNo"></asp:Label></td>
                                                                                </tr>
                                                                                
                                                                            </table>

                                                                            <br /><br />
                                                               
                                                                            <asp:GridView Width="100%"  ShowFooter="false"   ID="gvProdDefcList" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"  SkinID="gvLightSeaBlue" HorizontalAlign="Left"   OnPageIndexChanging="gvProdDefcList_PageIndexChanging" OnRowDataBound="gvProdDefcList_RowDataBound" >
                                                                                <Columns>
                                                                                   
                                                                                     <asp:TemplateField ItemStyle-Width="150" HeaderText="MCode"  >
                                                                                        <ItemTemplate>
                                                                                            <asp:Literal runat="server" ID="ltMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>'/>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="Required Qty."  >
                                                                                        <ItemTemplate>
                                                                                            <asp:Literal runat="server" ID="ltRequiredQuantity" Text='<%# DataBinder.Eval(Container.DataItem, "RequiredQuantity") %>'/>
                                                                                        </ItemTemplate>
                                                                            
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="Avialable Qty."  >
                                                                                        <ItemTemplate>
                                                                                            <asp:Literal runat="server" ID="ltAvialableQty" Text='<%# DataBinder.Eval(Container.DataItem, "AvialableQty") %>'/>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="Deficiency"  >
                                                                                        <ItemTemplate>
                                                                                            <asp:Literal runat="server" ID="ltDeficiency" Text='<%# DataBinder.Eval(Container.DataItem, "Deficiency") %>'/>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                       
                                                                                </Columns>
                                                                            </asp:GridView>

                                                                            <br />
                                                                            <br />
                                                                        </asp:Panel>

                </ContentTemplate>  
           </asp:UpdatePanel>
       </div>
    </div>--%>
     <!-- Production Order Materials Deficiency List Dialog   -->

</asp:Content>
