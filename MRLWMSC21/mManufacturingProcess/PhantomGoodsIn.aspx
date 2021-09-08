<%@ Page Title="" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="PhantomGoodsIn.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.PhantomGoodsIn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">

     <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="ss" SupportsPartialRendering="true" ></asp:ScriptManager>
     <script type="text/javascript" src="Scripts/CommonScripts.js"></script>

        <link rel="stylesheet" type="text/css" href="../App_Themes/Inventory/Inventory_Style.css" media="screen" />

      <script type="text/javascript">



          $(document).ready(function () {
            
              var textfieldname = $("#<%= this.txtin_kitcode.ClientID %>");
              DropdownFunction(textfieldname);

              $('#<%=this.txtin_kitcode.ClientID%>').autocomplete({

                  source: function (request, response) {
                        //  alert('www');
                      $.ajax({
                          url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetKitcode") %>',
                          data: "{ 'Prefix': '" + request.term + "'}",
                          dataType: "json",
                          type: "POST",
                          async: true,
                          contentType: "application/json; charset=utf-8",
                          success: function (data) {
                              response($.map(data.d, function (item) {
                                // alert(item.val);
                                  return {
                                      label: item.split(',')[0],
                                      val: item.split(',')[1]
                                      
                                      
                                  }
                              }))
                          }

                      });
                  },
                  select: function (e, i) {
                    //  alert(i.val);

                      $("#<%=hifkitcode.ClientID %>").val(i.item.val);
                  },
                  minLength: 0
              });

              var textfieldname = $("#<%= this.txtin_poitemline.ClientID %>");
              DropdownFunction(textfieldname);

              $('#<%=this.txtin_poitemline.ClientID%>').autocomplete({

                  source: function (request, response) {
                     // alert('jjj');
                      $.ajax({
                          url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetLinenoforjob") %>',
                          data: "{ 'ProductionOrderHeaderID':'"+document.getElementById("<%=hifkitcode.ClientID%>").value+ "'}",
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

                  minLength: 0
              });

          });
             

    </script>
    

<table border="0" cellpadding="4" cellspacing="4" align="center" height="400px" width="600px">
    
    <tr>
            <td colspan="1" style="height: 13px">
                &nbsp;<br />
            </td>
        </tr>
        <tr>
                      <td colspan="1" class="FormLabels" style="height: 13px">
               
                <span class="mandatory_field"> Note: </span>
                <asp:Label ID="lbl" runat="server" Font-Bold="true" ForeColor="Red" Text=" * " />
                Indicates mandatory fields </td>
                    
        </tr>
    
    
     


    <tr>
        <td align="left" colspan="3" valign="top">
                <asp:Panel ID="pnggoodsin" runat="server" >


                    <fieldset style="border:1px solid #808080;border-radius:5px;width:720px;" align="left">
                    <LEGEND><b>Job Order Details</b></LEGEND>
                    
                         <table border="0"  runat="server" id="tbgoodsin" cellpadding="4" cellspacing="4" align="center"  width="100%">
                        
                  <tr >


                       <td align="left">
                            <asp:RequiredFieldValidator ID="rfvkitcode" runat="server" ValidationGroup="getDetails" ControlToValidate="txtin_kitcode" Display="Dynamic" ErrorMessage=" * " />
                            <asp:Literal ID="ltin_kitcode" runat="server" Text="Kit Code " /><br />
                            <asp:TextBox ID="txtin_kitcode"  runat="server"   SkinID="txt_Auto"   />
                            <asp:HiddenField ID="hifkitcode" runat="server" />
                        </td>
                         <td align="left">
                            <asp:RequiredFieldValidator ID="rfvpoitemline" runat="server" ValidationGroup="getDetails" ControlToValidate="txtin_poitemline" Display="Dynamic" ErrorMessage=" * " />
                            <asp:Literal ID="ltin_poitemline" runat="server" Text="Line Number " /><br />
                            <asp:TextBox ID="txtin_poitemline"  runat="server"   SkinID="txt_Auto" />
                                
                        </td>
                           
                              <td align="left" colspan="3">
              
                            <asp:LinkButton ID="lnkin_GetDetails" runat="server"  ValidationGroup="getDetails" OnClick="lnkin_GetDetails_Click" CssClass="ui-btn ui-button-large">
 Get Details <%=MRLWMSC21Common.CommonLogic.btnfaFilter %>
</asp:LinkButton> &nbsp;&nbsp;
                                                       
                            <asp:LinkButton ID="lnkin_cancel" runat="server"  OnClick="lnkin_cancel_Click1"  CssClass="ui-btn ui-button-large">
Clear <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
</asp:LinkButton>
                        </td>
                        
   </tr>

                         </table>
                        
   
                         </fieldset>
                    
                    </asp:Panel>
             
            </td>
    </tr>   





    
    <tr>
        <td align="left" colspan="3">
                <asp:Panel runat="server" ID="pnljoborderList" Visible="false"  >
                             <fieldset style="border:1px solid #808080;border-radius:5px;width:720px;" align="left">
                                    <LEGEND><font size="4.99999"><b>Phantom Kit Items</b></font></LEGEND>
                                 <table border="0" cellpadding="5" runat="server" id="tbjoborder_FormDetails" cellspacing="5" align="center" width="100%">
                              
                                       <tr>
                                        <td colspan="1" align="left" width="35%">
                                            <asp:Panel  ID="pnltabl" runat="server"  ></asp:Panel>
                                           
                                            
                                        </td>
                                        
                                    </tr>
                                     <tr>
                                         <td colspan="3" align="left" >
                                             <table id="tbljoborder" runat="server" border="0" cellspacing="0" cellpadding="0"  width="100%" >
                                                    <tr >
                                                        <td colspan="1" >
                                                              <nobr ><b>
                                                            Part Number</b>
                                                            </nobr>
                                                        </td>
                                                        <td>
                                                            <b>
                                                            Batch No.
                                                                </b>
                                                        </td>
                                                        <td >
                                                            <b>
                                                            Quantity
                                                            </b>
                                                        </td>
                                                        
                                                    </tr>
                                                   <br/>
                                                  
                                                 
                                             </table>
                                         </td>
                                     </tr>
                                     <tr>
                                         <td align="left">
                                             <asp:Label ID="lbldata" runat="server"   />
                                         </td>

                                     </tr>
                                        <tr>
                            <td  class="FormLabels" align="left">
                            
                         </td>
                       </tr>
                                      <tr>
                                        <td colspan="3" align="right">
                                            <asp:LinkButton ID="lnkpo" runat="server"   CssClass="ui-btn ui-button-large" OnClick="lnkpo_Click"  >
                                                Add Phantom line item<%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                            </asp:LinkButton>
                                    
                                        </td>
                                    </tr>
                                     <tr>
                                          <td colspan="3" align="right">
                                              </td>

                                     </tr>
                                      
                                     </table>
                                  </fieldset>
                </asp:Panel>

        </td>
    </tr>
    <tr>
            <td colspan="5">
                &nbsp;
            </td>
        </tr>
        </table>
    
</asp:Content>
