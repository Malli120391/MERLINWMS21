<%@ Page Title="" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="CaptureCycleCountDetails.aspx.cs" Inherits="MRLWMSC21.mInventory.CaptureCycleCountDetails" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="InvContent" runat="server">
  
    <link rel="stylesheet" type="text/css" href="../App_Themes/Inventory/Inventory_Style.css" media="screen" />
    
    <script>
        function ShowStickyToast1(type, message, IsParmenent) {
            var val;
            var time;
            if (type == true)
                val = 'success';
            else
                val = 'error';
            $().toastmessage('showToast', {
                stayTime: 5000,
                text: message,
                sticky: IsParmenent,
                position: 'bottom-right',
                type: val,
                closeText: '',
                close: function () {
                },

            });
        }
        $(document).ready(function () {

            $('.DateBoxCSS_small').datepicker({ dateFormat: 'dd/mm/yy' });
            //Auto Complte For Mcodes
            var textfieldname = $("#<%= this.txtmmID.ClientID %>");
            DropdownFunction(textfieldname);
            $('#<%=this.txtmmID.ClientID%>').autocomplete({

                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetMcodeForCycleCount") %>',
                        data: "{ 'Prefix': '" + request.term + "'}",
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
                select: function (e, i) {
                    $("#<%=hifMCode.ClientID %>").val(i.item.val);
                },

                minLength: 0
            }).data("autocomplete")._renderItem = function (ul, item) {
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + item.label + "" + item.description + "</a>")
                    .appendTo(ul)
            };


            //Auto Complte For CycleCountIDs
            var textfieldname = $("#<%= this.txtccID.ClientID %>");
            DropdownFunction(textfieldname);
            $('#<%=this.txtccID.ClientID%>').autocomplete({

                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetCCID") %>',
                        data: "{ 'MMID': '" + document.getElementById('<%=this.hifMCode.ClientID%>').value + "'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response(data.d)
                        }

                    });
                },

                minLength: 0
            });

            var textfieldname = $("#<%= this.txtlocID.ClientID %>");
            DropdownFunction(textfieldname);
            $('#<%=this.txtlocID.ClientID%>').autocomplete({

                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetLocationListINCC") %>',
                        data: "{'Prefix': '" + request.term + "'}",
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

                    $("#<%=this.hifLocID.ClientID%>").val(i.item.val);

                },

                minLength: 0
            });

        });


            function CheckFOrMcode() {
                var data = document.getElementById('<%=this.hifMCode.ClientID%>').value;
            if (data == "0") {

                document.getElementById('<%=this.txtmmID.ClientID%>').focus();
            }
        }
        function testIsRequired(object) {
            //var element = document.getElementById(object);

            if (object.value == "") {

                ShowStickyToast1(false, "Provide Mandatory Fields", false);
                object.focus();
            }
        }
        function CheckIsrequiredDate(object) {

            alert(object.value);
        }
    </script>

       <div class="dashed"></div>
    <div class="pagewidth">
        <center>
                  <table>
                  <tr>
                      <td>
                            Note:
                         <asp:Label ID="Label2" runat="server" CssClass="errorMsg" Text=" * " />
                         Indicates mandatory fields 
                      </td>
                  </tr>
                  <tr>
                      <td>
                             <center><asp:Panel runat="server" ID="pnlCCCapture" Width="800" Height="220" >
                  <fieldset style="border:1px solid #808080;border-radius:5px;" align="left">
                    <LEGEND><b>CycleCount Details</b></LEGEND>
                        <table align="center" cellspacing="30">
                  
                  <tr>
                    <td colspan="2" align="left" >
                    Material Code
                           <asp:RequiredFieldValidator ID="rftxtxmmID" CssClass="errorMsg"  ControlToValidate="txtmmID" Display="Dynamic"  ErrorMessage=" * " runat="server" ValidationGroup="GetCCData"  />
                 <br /> <asp:TextBox runat="server" ID="txtmmID" SkinID="txt_Auto"></asp:TextBox>
                    <asp:HiddenField ID="hifMCode" runat="server"  Value="0"/>

              </td>
              <td colspan="2" align="left">
               
                CycleCount ID 
                   <asp:RegularExpressionValidator ID="regtxtccID" ControlToValidate="txtccID" runat="server" ErrorMessage="*" ValidationExpression="\d+" ValidationGroup="GetCCData"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rftxtccID"   ControlToValidate="txtccID" Display="Dynamic" ErrorMessage="*" runat="server" ValidationGroup="GetCCData"   />
           <br />
                <asp:TextBox runat="server" ID="txtccID" SkinID="txt_Auto"  CausesValidation="true" onfocus="CheckFOrMcode()">
                  </asp:TextBox>
              </td>
             
          </tr>
                  <tr>
              <td colspan="2" align="left" >
                  Location
                   <asp:RequiredFieldValidator ID="rflocation"   ControlToValidate="txtlocID" Display="Dynamic" ErrorMessage="*" runat="server" ValidationGroup="GetCCData"   />

                 <br /> <asp:TextBox ID="txtlocID" runat="server" SkinID="txt_Auto"></asp:TextBox>
                  <asp:HiddenField ID="hifLocID" runat="server" Value="0" />
              </td>
                   
                     
                      <td>
                           <asp:LinkButton ID="lnkin_GetDetails" runat="server" ValidationGroup="getDetails"  OnClick="lnkin_GetDetails_Click"  CssClass="ui-btn ui-button-large">
 Get Details <%=MRLWMSC21Common.CommonLogic.btnfaFilter %>
</asp:LinkButton>
                          &nbsp;&nbsp;
                           </td>
                                <td>                            
                            <asp:LinkButton ID="lnkin_cancel" runat="server"  OnClick="lnkin_cancel_Click" CssClass="ui-btn ui-button-large">
Cancel <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
</asp:LinkButton>
                      </td>
                            

                        </tr>
                 </table>
                         
                      </fieldset>
                    </asp:Panel></center>
                      </td>
                  </tr>
              
              </table>
              </center>
               <center>
                   <asp:Panel ID="pnlMsps" runat="server" Width="800" Height="600" Visible="false">
                            <fieldset style="border:1px solid #808080;border-radius:5px;" align="left">
                           
                           
                            <table id="tblMSPs" runat="server" cellspacing="20">
                                <tr>
                                    <td>
                                         Count UOM
                                    </td>
                                    <td>
                                            <asp:DropDownList runat="server" ID="ddlUOM"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Count Qty
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCountQty" runat="server" onblur="testIsRequired(this)" OnTextChanged="txtCountQty_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                                                            
                            &nbsp&nbsp&nbsp<asp:CheckBox runat="server" ID="chkIsdam" />Is Damaged
                             &nbsp&nbsp&nbsp &nbsp&nbsp&nbsp
                                <asp:LinkButton ID="lnkSubmitClick" runat="server" OnClick="btnSubmit_Click" CssClass="ui-btn ui-button-large">Capture<%=MRLWMSC21Common.CommonLogic.btnfaSave %></asp:LinkButton>
                    <LEGEND><b>Item Details</b></LEGEND>
                                </fieldset>
                       
                   </asp:Panel>

               </center>
  

    </div>
    
         
                      
                            
           
               
                        
   
</asp:Content>
