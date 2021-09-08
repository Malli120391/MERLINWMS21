<%@ Page Title="Issue Order:." Language="C#" MasterPageFile="~/mOrders/OrdersMaster.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="ToolManagement.aspx.cs" Inherits="MRLWMSC21.mOrders.ToolManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OrdersContent" runat="server">
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
     <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="ss" SupportsPartialRendering="true"></asp:ScriptManager>

    <script>
        $(document).ready(function () {
            CustomAccordino($('#dvIOHeader'), $('#dvIOBody'));
            CustomAccordino($('#dvIIDHeader'), $('#dvIIDBody'));
        });
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
                

                $('.DateBoxCSS_small').datepicker({ dateFormat: 'dd/mm/yy' });

                var textfieldname = $('#atcEditMCode');
                DropdownFunction(textfieldname);
                $('#atcEditMCode').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMCodeForToolOrder") %>',
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

                         $("#hifmmid").val(i.item.val);
                         try
                         {
                             document.getElementById('atcTMUoMID').value = "";
                             document.getElementById('hifTMUoMid').value = "";
                             document.getElementById("atcKitPlanner").value = "";
                             document.getElementById('hifKitPlanner').value = "";
                         } catch (err) { }

                     },
                     minLength: 0
                 });
                var textfieldname = $("#<%= this.atcEmployee.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.atcEmployee.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersBasedRole") %>',
                        data: "{ 'Prefix': '" + request.term + "' ,'TenantID': '" + '<%=  cp.TenantID %>' + "'}",
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

                         $("#<%=hifEmployee.ClientID %>").val(i.item.val);
                },
                minLength: 0
                 });
                var textfieldname = $('#atcTMUoMID');
                DropdownFunction(textfieldname);
                $('#atcTMUoMID').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMWithQty") %>',
                               data: "{ 'MaterialID': '" + +document.getElementById("hifmmid").value + "'}",
                               dataType: "json",
                               type: "POST",
                               async: true,
                               contentType: "application/json; charset=utf-8",
                               success: function (data) {
                                   if (data.d == "" || data.d == "/") {
                                       alert('No UoM\'s are configured to this Part Number');

                                       document.getElementById("atcPUoM").value = "";

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

                          $("#hifTMUoMid").val(i.item.val);
                          $("#hidUoMQty").val(i.item.label.split('/')[1]);
                      },
                      minLength: 0
                });
                var textfieldname = $('#atcKitPlanner');
                DropdownFunction(textfieldname);
                $('#atcKitPlanner').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadKitPlanner") %>',
                               data: "{ 'MaterialID': '" + +document.getElementById("hifmmid").value + "','TenantID':'" +<%=cp.TenantID%> +"'}",
                               dataType: "json",
                               type: "POST",
                               async: true,
                               contentType: "application/json; charset=utf-8",
                               success: function (data) {
                                   if (data.d == "") {
                                       alert('No Kit\'s are configured to this Part Number');

                                       document.getElementById("atcKitPlanner").value = "";
                                       return;
                                   }
                                   else {
                                       response($.map(data.d, function (item) {
                                           return {
                                               label: item.split(',')[0],
                                               val: item.split(',')[0]
                                           }
                                       }))
                                   }

                               }
                           });
                       },
                     select: function (e, i) {
                         $("#hifKitPlanner").val(i.item.val);

                     },
                     minLength: 0
                 });

                try {
                    var textfieldname = $('#<%=this.txtSearch.ClientID%>');
                    DropdownFunction(textfieldname);
                    $('#<%=this.txtSearch.ClientID%>').autocomplete({

                        source: function (request, response) {

                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMCodeForSaleOrderWithOEM") %>',
                                  data: "{ 'prefix': '" + request.term + "','TenantID':'" +<%=cp.TenantID%> +"','SOheaderID':'<%=ViewState["HeaderID"]%>'}",
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
        fnMCodeAC();
        </script>
    <script>
        function CheckSUoMQty(TextBox) {
            /*var UoMQty = parseFloat(document.getElementById('hidUoMQty').value) * 100;
            var RequireQty = parseFloat(TextBox.value) * 100;
            if (RequireQty % UoMQty != 0) {
                showStickyToast(true, "Quantity should be multiple of Is UoMQty.");
                TextBox.value = "";
                return;
            }*/
            CheckDecimal(TextBox);
        }


        function CheckIsDelted(checkBox) {
            if (checkBox.checked) {
                alert('Are you sure want to delete the Issue Order?');
            }
        }
        function MSPConfifure() {
            var mmid = 0;
            try {

                mmid = document.getElementById("hifmmid").value;
                if (mmid == '' || mmid == null)
                    mmid = 0;
            } catch (err) {
            }
            //alert(mmid);
            // alert(document.getElementById("hifmmid").value);

            $.ajax({
                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/MaterialConfigurationService") %>',
                data: "{ 'MaterialId': '" + mmid + "','TenantID':'" +<%=cp.TenantID%> +"'}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    //alert(data);
                    configure(name, data.d);
                    //response(data.d);
                }
            });
            //alert('dsdsdsdsdsds');
            $.ajax({
                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadKitPlanner") %>',
                data: "{ 'MaterialID': '" + mmid + "','TenantID':'" +<%=cp.TenantID%> +"'}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d != "") {
                        var kitImage = document.getElementById("imgkit");
                        kitImage.style.display = "block";
                        var ToolTip = 'configured kitIDs to this Part Number\n\n    ' + data.d;
                        kitImage.setAttribute("title", ToolTip);
                        return;
                    }
                    else {
                        var kitImage = document.getElementById("imgkit");
                        //alert(kitImage);
                        kitImage.style.display = "none";
                        // kitImage.style.display = "block";
                        // alert(data.d.split(',')[0]);
                        // var ToolTip = 'configure kitIDs to this Material\n\n kit Material IDs are ' + data.d;
                        // kitImage.setAttribute("title", ToolTip);
                        return;
                        //kitImage.setAttribute("title", "Live Chat is currently ONLINE");
                        //alert('aaaaaa');
                    }
                }
            });

        }
        function configure(textbox, text) {
            //alert(text);
            var paramNames = text.split('|');
            var listOfparames = paramNames[0].split(',');

            try {
                for (var item = 0; item < listOfparames.length; item++) {
                    // alert(listOfparames[item]);

                    document.getElementById(listOfparames[item]).style.display = "none";


                }
                listOfparames = paramNames[1].split(',');

                for (var item = 0; item < listOfparames.length; item++) {


                    document.getElementById(listOfparames[item]).style.display = "block";

                }
            } catch (err) {
            }
        }
        function ClearText(TextBox) {
            if (TextBox.value == "Search Item...") {
                TextBox.value = "";
                TextBox.style.color = "#000000";
            }
        }
        function focuslost(TextBox) {
            if (TextBox.value == "") {
                TextBox.value = "Search Item...";
                TextBox.style.color = "#A4A4A4";
            }
        }
    </script>
    <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlcontent" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="dashed"></div>
<div class="pagewidth">
     <table border="0" cellspacing="2" cellpadding="2" class="ListTable" height="360px"  align="center">


                   <tr class="ListHeaderRow">
                         <td align="left">
                             Note:
                             <asp:Label ID="Label2" runat="server" CssClass="errorMsg" Text=" * " />
                             Indicates mandatory fields 
                         
                         </td>
                         <td align="right" valign="top" >
                         <asp:ImageButton runat="server" ID="Ibutprint" Visible="false"  ImageUrl="~/Images/blue_menu_icons/Printer.gif" CssClass="NoPrint"  />
                         
                        </td>
                   </tr>

                   <tr class="ListHeaderRow">
                       <td class="pagewidth">
                           <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvIOHeader" style="">Issue Order Header </div>
                            <div class="ui-Customaccordion" id="dvIOBody">
                                <table border="0" cellspacing="0" cellpadding="0" width="100%" style="padding-top: 10px; padding-left: 10px;">
                                    <tr>
                        <td>
                            <table width="700px" align="center">

                                    <tr>
                                       <td width="36%">
                                           <asp:UpdatePanel ChildrenAsTriggers="true" ID="upTMOCode" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                        <asp:RequiredFieldValidator ID="rfvTMOCode" runat="server" ValidationGroup="EmployeeData" Enabled="false" CssClass="errorMsg" ControlToValidate="txtTMOCode" Display="Dynamic" ErrorMessage=" * " />
                                                        <asp:Literal ID="ltTMOCode" runat="server" Text="Issue Number:" /><br />
                                                        <asp:Literal ID="ltTMOID" Visible="false" runat="server"></asp:Literal>
                                                        <asp:TextBox ID="txtTMOCode" runat="server" Enabled="false" MaxLength="30" Width="160" />
                                                        <asp:ImageButton runat="server" ID="IbtnNew" OnClick="IbtnNew_Click" ImageUrl="../Images/blue_menu_icons/add_new.png" style="height: 16px" ToolTip="Generate New Issue Number" />
                                                </ContentTemplate>
                                           </asp:UpdatePanel>
                                       </td>
                                       <td colspan="1">
                                           <asp:UpdatePanel ID="upnlToolManagement" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvEmployee" ValidationGroup="EmployeeData" runat="server" CssClass="errorMsg" ControlToValidate="atcEmployee" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:Literal ID="ltEmployee" runat="server" Text="Employee:" /><br />
                                                    <asp:TextBox ID="atcEmployee"  runat="server" SkinID="txt_Auto"  Width="160px"></asp:TextBox>
                                                    <asp:HiddenField ID="hifEmployee" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                       </td>
                                       <td align="right">
                                            <asp:Label id="ltTMOStatus" runat="server" CssClass="BigCapsHeading" />

                                       </td>
                                   </tr>
                    
                                    <tr>
                        
                                        <td align="right" colspan="3" >
                                            <asp:CheckBox ID="chkIsActive" Checked="true" runat="server" Text="Active" Visible="false" />
                                            <asp:CheckBox ID="chkIsDeleted" onclick="CheckIsDelted(this);" runat="server" Text="Delete" Visible="false" />
                                        </td>
                                    </tr>
                    
                                    <tr>
                                        <td align="right" colspan="3">
                                          <asp:UpdatePanel ID="udplControls" ChildrenAsTriggers="true" runat="server" UpdateMode="Always">
                                              <Triggers>
                                                  <asp:PostBackTrigger ControlID="lnkUpdate" />
                                              </Triggers>
                                           <ContentTemplate>
                                                    <br />
                                                    <asp:LinkButton ID="lnkReleaseTool" runat="server" CssClass="ui-btn ui-button-large" OnClick="lnkReleaseTool_Click" >Release Tool<%=MRLWMSC21Common.CommonLogic.btnfaSignOut %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkClose" CssClass="ui-btn ui-button-large" runat="server" OnClientClick="return confirm('Are you sure want to cancel?')"  OnClick="lnkClose_Click" >Cancel <%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkUpdate" CssClass="ui-btn ui-button-large" runat="server" OnClientClick="showAsynchronus();" OnClick="lnkUpdate_Click" >
                                                        
                                                        </asp:LinkButton>
                                               <br /><br />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                        </td>
                                    </tr>
                    
                             </table>
                        </td>
                    </tr>
                                </table>
                            </div>
                       </td>
                   </tr>
                   <tr><td class="accordinoGap"></td></tr>
                   <tr class="ListHeaderRow">
                       <td class="pagewidth" id="tdTMOLineItems" runat="server">
                           <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvIIDHeader" style="">Issue Item Details</div>
                            <div class="ui-Customaccordion" id="dvIIDBody">
                                <table border="0" cellspacing="0" cellpadding="0" width="100%" style="padding-top: 10px; padding-left: 10px;">
                                      <tr>
                        <td align="right" colspan="2" style="padding-bottom:10px;">
                            <asp:UpdatePanel ID="upnlNewItem" ChildrenAsTriggers="true" runat="server" UpdateMode="Always">
                           <ContentTemplate>
                               <asp:Panel ID="Panel1" runat="server">
                                   <asp:TextBox runat="server" ID="txtSearch" Visible="false" Width="150" Text="Search Item..." onblur="javascript:focuslost(this)" onfocus="ClearText(this)" SkinID="txt_Hidden_Req_Auto"/>
                                   &nbsp;&nbsp;
                                   <asp:LinkButton runat="server" ID="lnksearch" Visible="false"  OnClick="lnksearch_Click" CssClass="ui-btn ui-button-large">
                                      Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %>
                                   </asp:LinkButton>&nbsp;&nbsp;
                                  <asp:LinkButton ID="lnkAddNewItem" CssClass="ui-button-small" runat="server" OnClick="lnkAddNewItem_Click" >Add Item<%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                      </asp:LinkButton>
                               </asp:Panel>                            
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                    </tr>
                    
                   <tr>
                        <td colspan="2" >
                            <asp:UpdatePanel ID="upnlToolManagementDetails" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                     <asp:Panel ID="pnlToolManagementDetails" ScrollBars="Horizontal" runat="server" Width="1000px">
                                            <asp:GridView ID="gvToolManagementDetails" PageSize="10" OnPageIndexChanging="gvToolManagementDetails_PageIndexChanging" runat="server" SkinID="gvLightGreenNew" AutoGenerateColumns="false" OnRowUpdating="gvToolManagementDetails_RowUpdating" OnRowEditing="gvToolManagementDetails_RowEditing" OnRowDataBound="gvToolManagementDetails_RowDataBound" OnRowCancelingEdit="gvToolManagementDetails_RowCancelingEdit">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="100" HeaderText="Line No."  >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltlinenumber"  Text='<%#DataBinder.Eval(Container.DataItem, "LineNumber").ToString() %>'/>
                                                            <asp:Literal runat="server" Visible="false" ID="ltSODetailsID" text='<%#DataBinder.Eval(Container.DataItem, "SODetailsID").ToString() %>'/>
                                                        </ItemTemplate>

                                                        <EditItemTemplate>
                                                            <asp:Literal runat="server" ID="ltlinenumberEdit" Text='<%#DataBinder.Eval(Container.DataItem, "LineNumber").ToString() %>'/>
                                                            <asp:Literal runat="server" Visible="false" ID="ltSODetailsIDEdit" text='<%#DataBinder.Eval(Container.DataItem, "SODetailsID").ToString() %>'/>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="100" HeaderText="Part Number"  >
                                                        <ItemTemplate>
                                                            <nobr>
                                                                <asp:Literal runat="server" ID="ltMMID"   Text='<%#DataBinder.Eval(Container.DataItem, "MCode").ToString() %>'/>
                                                            </nobr>
                                                            <br/><asp:Label CssClass="BOMPartNoHead" runat="server" ID="lbOEMPartNumber" Text='<%# DataBinder.Eval(Container.DataItem, "oempartno").ToString() %>'></asp:Label>
                                                        </ItemTemplate>

                                                        <EditItemTemplate >
                                                            <asp:RequiredFieldValidator ID="rfvMCode" runat="server" ValidationGroup="UpdateSODetails" CssClass="errorMsg" ControlToValidate="atcEditMCode" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:TextBox ID="atcEditMCode"  onfocus="javascript:MSPConfifure(this);" runat="server" ClientIDMode="Static" onblur="javascript:MSPConfifure(this);"   Visible="true" SkinID="txt_Auto" Width="130" Text='<%#DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                                                            <asp:HiddenField runat="server" ID="hifmmid" ClientIDMode="Static"  value='<%#DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString() %>'/>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="100" HeaderText=" BUoM/ Qty. "  >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltBuomQtyID"  Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "BUoM").ToString(),DataBinder.Eval(Container.DataItem, "BUoMQty").ToString())!="/"?String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "BUoM").ToString(),DataBinder.Eval(Container.DataItem, "BUoMQty").ToString()):""  %>'/>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>   

                                                    <asp:TemplateField ItemStyle-Width="100" HeaderText=" IsUoM/ Qty. "  >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltTMUoMQtyID"  Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "SUoM").ToString(),DataBinder.Eval(Container.DataItem, "SUoMQty").ToString())  %>'/>
                                                        </ItemTemplate>

                                                        <EditItemTemplate>
                                                            <asp:RequiredFieldValidator ID="rfvTMUoMID" runat="server" ValidationGroup="UpdateSODetails" CssClass="errorMsg" ControlToValidate="atcTMUoMID" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:TextBox runat="server" ID="atcTMUoMID" ClientIDMode="Static" SkinID="txt_Auto" Width="80" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "SUoM").ToString(),DataBinder.Eval(Container.DataItem, "SUoMQty").ToString())=="/"?"":String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "SUoM").ToString(),DataBinder.Eval(Container.DataItem, "SUoMQty").ToString())  %>' />
                                                            <asp:HiddenField runat="server" ID="hifTMUoMid" ClientIDMode="Static"  Value='<%#DataBinder.Eval(Container.DataItem, "MaterialMaster_SUoMID").ToString() %>'/>
                                                            <input id="hidUoMQty" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "SUoMQty").ToString() %>'/>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="100" HeaderText="Kit ID"  >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltkitplannet"  Text='<%#DataBinder.Eval(Container.DataItem, "KitPlannerID").ToString() %>'/>
                                                        </ItemTemplate>

                                                        <EditItemTemplate>
                                                            <asp:Image ImageUrl="../Images/kit.gif" ClientIDMode="Static" Visible="true" ID="imgkit" runat="server"/>
                                                            <asp:TextBox runat="server" ID="atcKitPlanner" ClientIDMode="Static" SkinID="txt_Auto" Width="60" Text='<%#DataBinder.Eval(Container.DataItem, "KitPlannerID").ToString() %>' />
                                                            <asp:HiddenField ID="hifKitPlanner" runat="server" ClientIDMode="Static" Value='<%#DataBinder.Eval(Container.DataItem, "KitPlannerID").ToString() %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="60" HeaderText="Kit Qty."  >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltkitQty"  Text='<%#DataBinder.Eval(Container.DataItem, "KitQty").ToString() %>'/>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="100" HeaderText="Quantity"  >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltQuantity"  Text='<%#DataBinder.Eval(Container.DataItem, "SOQuantity").ToString() %>'/>
                                                        </ItemTemplate>

                                                        <EditItemTemplate>
                                                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ValidationGroup="UpdateSODetails" CssClass="errorMsg" ControlToValidate="txtQuantity" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:TextBox runat="server" ID="txtQuantity" onKeyPress="return checkDec(this,event)" onblur="CheckSUoMQty(this)" Width="80" Text='<%#DataBinder.Eval(Container.DataItem, "SOQuantity").ToString() %>'/>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                  
                                                </Columns>
                                            </asp:GridView>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                           
                        </td>
                    </tr>
                                    </table>
                                </div>
                        </td>
                    </tr> 
                    <tr><td class="accordinoGap"></td></tr>

                 

                  
</table></div>
</ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
