<%@ Page Title="" Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="PhantomJobOrder.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.PhantomJobOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="ss" SupportsPartialRendering="true" ></asp:ScriptManager>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
     <script type="text/javascript">

         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
         function EndRequestHandler(sender, args) {
             if (args.get_error() == undefined) {
                 fnMCodeAC();
             }
         }


         function fnMCodeAC() {
             $(document).ready(function () {

                 $(document).ready(function () {

                     $("#<%=this.txtStartDate.ClientID%>").datepicker({
                         dateFormat: "dd/mm/yy",
                         minDate: 0,
                         onSelect: function (selected) {
                             $("#<%=this.txtDueDate.ClientID%>").datepicker("option", "minDate", selected, { dateFormat: "dd/mm/yy" })

                         }

                     });

                     var _minDate = new Date(1990, 1, 1, 0, 0, 0);
                     if (document.getElementById('<%=this.txtStartDate.ClientID%>').value != "") {
                         var date = document.getElementById('<%=this.txtStartDate.ClientID%>').value;
                         _minDate = new Date(date.split('/')[2], parseInt(date.split('/')[1]) - 1, date.split('/')[0], 0, 0, 0, 0);
                     }

                     $("#<%=this.txtDueDate.ClientID%>").datepicker({
                         dateFormat: "dd/mm/yy",
                         minDate:_minDate,
                         onSelect: function (selected) {
                             $("#<%=this.txtStartDate.ClientID%>").datepicker("option", "maxDate", selected, { dateFormat: "dd/mm/yy" })
                         }

                     });

                 });


                 $('.DateBoxCSS_small').datepicker({ dateFormat: 'dd/mm/yy' });

                 
                 var textfieldname = $('#<%=atcPRORefNo.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcPRORefNo.ClientID%>').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMaterialListForPhantomProduction") %>',
                             data: "{ 'prefix': '" + request.term + "'}",
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

                         $("#<%=hifPRORefNo.ClientID %>").val(i.item.val);
                         try {
                                 document.getElementById('<%=this.atcRoutingHeaderVersion.ClientID%>').value = "";
                                 document.getElementById('<%=this.hifRoutingHeaderVersion.ClientID%>').value = "";
                                 document.getElementById('<%=this.atcProductionUoM.ClientID%>').value = "";
                                 document.getElementById('<%=this.hifProductionUoM.ClientID%>').value = "";
                             }
                         catch (err) {
                         }
                        },
                     minLength: 0
                 });

                 var textfieldname = $('#<%=atcRoutingHeaderVersion.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcRoutingHeaderVersion.ClientID%>').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRoutingVersionRefNoList") %>',
                             data: "{ 'prefix': '" + request.term + "','MaterialMasterRevisionID':'" + document.getElementById('<%=this.hifPRORefNo.ClientID%>').value + "'}",
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

                         $("#<%=hifRoutingHeaderVersion.ClientID %>").val(i.item.val);

                      },
                      minLength: 0
                 });

                 var textfieldname = $('#<%=atcProductionUoM.ClientID%>');
                 DropdownFunction(textfieldname);
                 $('#<%=atcProductionUoM.ClientID%>').autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMForPRO") %>',
                             data: "{ 'RoutingHeaderVersionID': '" + document.getElementById('<%=hifRoutingHeaderVersion.ClientID%>').value + "'}",
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

                         $("#<%=hifProductionUoM.ClientID %>").val(i.item.val);
                      },
                      minLength: 0
                 });

               
                
             });

         }
         fnMCodeAC();

  </script>

    <script type="text/javascript">
        var PhanthomArray=new Array();
        var MaterialCount;
        var HeaderRowCells;

        function CheckTextBoxValue(TextBox) {
            PhanthomArray = new Array();
            var text = TextBox.value;
            if (text == '.' || parseFloat(text) <= 0)
                TextBox.value = "";
            else if(text!='')
                CheckQty(TextBox);
        }

        function CheckQty(TextBox) {
            var DataTable = document.getElementById('gvPanthomMaterial');
            var DataRows = DataTable.getElementsByTagName('tr');
            
            HeaderRowCells = DataRows[0].getElementsByTagName('th');
            var columnCount = HeaderRowCells.length;
            MaterialCount = (columnCount - 4);// / 2;
                        
            var RowCount = DataRows.length;
            var textBox;
            var CheckBox;

            var DataCells;
            //RowCount
            for (RowIndex = 1; RowIndex < RowCount; RowIndex++) {
                
                DataCells = DataRows[RowIndex].getElementsByTagName('td');
                CheckBox = DataCells[MaterialCount  + 2].firstChild;
                textBox = DataCells[MaterialCount + 3].firstChild;
                
                PossibleQty = parseFloat(DataCells[MaterialCount+1].innerText);
                if (CheckBox.checked && textBox.value != "" && PossibleQty < parseFloat(textBox.value)) {
                    //alert('Entered quantity is more than PossibleQty');
                    showStickyToast(true, '\'Require Qty.\' is more than \'Possible Phantom Qty.\'', false);
                    TextBox.value = "";
                }

                if (CheckBox.checked && textBox.value != "") {
                    //alert(CheckRowMaterialQty(DataRows[RowIndex]));
                    if (!CheckRowMaterialQty(DataRows[RowIndex])) {
                        //checkBox.checked = false;
                        //alert(TextBox);
                        TextBox.value = "";
                        break;
                    }
                }
            }
        }

        function CheckRowMaterialQty(Row) {
            var isExist = false;
            var tableCells=Row.getElementsByTagName('td');
            var validSelection = true;
            for (Materialindex = 0; Materialindex < MaterialCount; Materialindex++) {
                try{
                    PhanthomArray.forEach(function (element) {
                    
                        if (element.MaterialMasterID == tableCells[Materialindex + 1].childNodes[4].value && element.BatchNo == tableCells[Materialindex + 1].getElementsByTagName('span')[0].innerText) {

                            if (parseFloat(tableCells[Materialindex+1].childNodes[2].value) * parseFloat(tableCells[MaterialCount + 3].firstChild.value) + element.ReqQty > element.TotalQty) {
                                //alert('BatchNo :' + element.BatchNo + ' of material selected Quantity is more than available Quantity');
                                //alert('Selected quantity of   \''+HeaderRowCells[Materialindex+1].getElementsByTagName('span')[0].innerText+'\'   with BatchNo:  \''+ element.BatchNo + '\'  is more than available quantity');
                                showStickyToast(true, 'Selected quantity of   \'' + HeaderRowCells[Materialindex + 1].getElementsByTagName('span')[0].innerText + '\'   with BatchNo:  \'' + element.BatchNo + '\'  is more than available quantity',false);
                                //return false;
                                throw StopIteration;
                            }
                        }
                    });
                } catch (err) {
                    return false;
                }
            }
            
            for (Materialindex = 0; Materialindex < MaterialCount; Materialindex++) {
                isExist = false;
                try{
                    PhanthomArray.forEach(function (element) {
                        //alert(element.MaterialMasterID +'=='+ tableCells[Materialindex + 1].childNodes[4].value +'&&'+ element.BatchNo +'=='+ tableCells[Materialindex + 1].getElementsByTagName('span')[0].innerText);
                        if (element.MaterialMasterID == tableCells[Materialindex + 1].childNodes[4].value && element.BatchNo == tableCells[Materialindex + 1].getElementsByTagName('span')[0].innerText) {
                            
                            if (parseFloat(tableCells[Materialindex+1].childNodes[2].value) * parseFloat(tableCells[MaterialCount  + 3].firstChild.value) + element.ReqQty <= element.TotalQty) {
                                //alert(parseFloat(tableCells[Materialindex + 1].childNodes[2].value) * parseFloat(tableCells[MaterialCount + 3].firstChild.value) + element.ReqQty);
                                element.ReqQty =parseFloat(tableCells[Materialindex+1].childNodes[2].value) *parseFloat(tableCells[MaterialCount + 3].firstChild.value) + element.ReqQty;
                                //break;
                                isExist = true;
                                throw StopIteration;
                            }
                        }
                    });
                } catch (err) {
                }
                if (!isExist) {
                    PhanthomArray.push({ MaterialMasterID: tableCells[Materialindex + 1].childNodes[4].value, BatchNo: tableCells[Materialindex + 1].getElementsByTagName('span')[0].innerText, TotalQty: parseFloat(tableCells[Materialindex + 1].childNodes[1].value), ReqQty: parseFloat(tableCells[Materialindex + 1].childNodes[2].value) * parseFloat(tableCells[MaterialCount + 3].firstChild.value) });
                }
            }
            return true;
            
        }

        function CheckCheckBox(CheckBox) {
            if (CheckBox.checked && CheckBox.parentNode.nextSibling.firstChild.value!='') {
                //alert(CheckBox.parentNode.parentNode.lastChild);
                //alert(CheckBox.parentNode.nextSibling.firstChild.value);
                CheckQty(CheckBox.parentNode.nextSibling.firstChild);

            }
        }


        function CheckIsDelted(checkBox) {
            if (checkBox.checked) {
                alert('Are you sure want to delete the Phantom Job Order?');
            }
        }
    </script>

     <table align="center" cellpadding="5" cellspacing="3" class="pagewidth">

        <tr>
            <td>
                &nbsp;
            </td>
          
        </tr>
        
        <tr>
                 <td colspan="1" align="left" class="FormLabels">
                         Note:<asp:Label ID="lberrormsg" runat="server" CssClass="errorMsg" Text=" * " />
                         Indicates mandatory fields

            </td>
            
         </tr>
                           
         <tr>
             <td colspan="3">
                 <div class="ui-SubHeading ui-SubHeadingBar " id="dvPOHDHeader" style="">Phantom Job Order Header </div>
                
                        <div class="ui-Customaccordion" id="dvPOHDBody">
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" class="internalData">

            
                            

                            <tr>
                                <td align="left" width="25%">
                                    <asp:UpdatePanel ChildrenAsTriggers="true" ID="upproNumber" UpdateMode="Always" runat="server" >
                                            <ContentTemplate>
                                                <asp:RequiredFieldValidator ID="rfvPRORefNo" runat="server" ValidationGroup="save" ControlToValidate="atcPRORefNo" Display="Dynamic" ErrorMessage=" * " />
                                                <asp:Literal ID="lclPRORefNo" runat="server" Text="Job Order Ref. #:" /><br />
                                                <asp:TextBox ID="atcPRORefNo" runat="server" Enabled="true"  SkinID="txt_Auto" />
                                                <asp:HiddenField ID="hifPRORefNo" runat="server" />
                                                <asp:ImageButton ID="IbutNew" Visible="false" runat="server"  ImageUrl="~/Images/blue_menu_icons/add_new.png" />
                                            </ContentTemplate>

                                   </asp:UpdatePanel>
                                    </td>
                                    <td>
                                    
                                        <asp:RequiredFieldValidator ID="rfvJobRefNo" runat="server" ValidationGroup="save" ControlToValidate="txtJobRefNo" Display="Dynamic" ErrorMessage=" * " />
                                         Job Order #:<br />
                                        <asp:TextBox ID="txtJobRefNo" runat="server" Width="200" onKeypress="return checkSpecialChar(event)"></asp:TextBox>
                                    </td>
                                 <td align="left" >
                                        <asp:RequiredFieldValidator ID="rfvProductionDate" runat="server" ValidationGroup="save" ControlToValidate="txtStartDate" Display="Dynamic" ErrorMessage=" * " />
                                        <asp:Literal ID="ltProductionDate" runat="server" Text="Job Order Date:" /><br />
                                        <asp:TextBox ID="txtProductionDate" EnableTheming="false"  CssClass="DateBoxCSS_small" runat="server"  Width="200" />
                                </td>
                                      
                                 <td rowspan="2" valign="middle" align="center" style="padding-right:75px;">
                                        <asp:Label id="ltPROStatus" runat="server" CssClass="BigCapsHeading" />

                                    </td>

                            </tr>

                            <tr>
                            
                                     <td align="left">

                                    <asp:UpdatePanel ChildrenAsTriggers="true" ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                         <ContentTemplate>
                                            <asp:RequiredFieldValidator ID="rfvkitCode" runat="server" ValidationGroup="save" ControlToValidate="txtkitCode" Display="Dynamic" ErrorMessage=" * " />
                                            Kit Code:<br />
                                            <asp:TextBox ID="txtkitCode" runat="server" Enabled="false" onfocus="check()" Width="200" onKeypress="return checkSpecialChar(event)" />
                                            <asp:ImageButton ID="imgKitCode" OnClick="imgKitCode_Click" runat="server" ImageUrl="~/Images/blue_menu_icons/add_new.png"   Title="Generate New Kit Code"/>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td> 

                                
                                
                            
                                <td align="left">
                
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ValidationGroup="save" ControlToValidate="txtStartDate" Display="Dynamic" ErrorMessage=" * " />
                                        <asp:Literal ID="ltStartDate" runat="server" Text="Start Date:" /><br />
                                        <asp:TextBox ID="txtStartDate"  runat="server"  Width="200" />
                                           

                                </td>
                                 <td align="left" colspan="1">
                                            <asp:RequiredFieldValidator ID="rfvDueDate" runat="server" ValidationGroup="save" ControlToValidate="txtDueDate" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal ID="ltDueDate" runat="server" Text="Due Date:" /><br />
                                            <asp:TextBox ID="txtDueDate"  runat="server"    Width="200" />
                        
                                 </td>
                               
                                

                            </tr>

                            <tr>
                                 <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="save" ControlToValidate="txtManufacturingDate" Display="Dynamic" ErrorMessage=" * " />
                                    Manufacturing Date:<br />
                                    <asp:TextBox ID="txtManufacturingDate" CssClass="DateBoxCSS_small" EnableTheming="false" runat="server"  Width="200"  />
                                </td>
                                <td align="left">
                
                                        <asp:RequiredFieldValidator ID="rfvRoutingHeaderVersion" runat="server" ValidationGroup="save" ControlToValidate="atcRoutingHeaderVersion" Display="Dynamic" ErrorMessage=" * " />
                                        <asp:Literal ID="ltRoutingHeaderVersion" runat="server" Text="Routing Version Ref. #:" /><br />
                                        <asp:TextBox ID="atcRoutingHeaderVersion" runat="server"  SkinID="txt_Auto" />
                                        <asp:HiddenField ID="hifRoutingHeaderVersion" runat="server" />
                                           

                                </td>
                                  <td align="left">
                                            <asp:RequiredFieldValidator ID="rfvProductionUoM" runat="server" ValidationGroup="save" ControlToValidate="atcProductionUoM" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal ID="ltProductionUoM" runat="server" Text="UoM:" /><br />
                                            <asp:TextBox ID="atcProductionUoM"  runat="server"   SkinID="txt_Auto" />
                                            <asp:HiddenField ID="hifProductionUoM" runat="server" />
                                 </td>
                                <td align="left" colspan="1">
                                        <asp:RequiredFieldValidator ID="rfvProductionQuantity" runat="server" ValidationGroup="save" ControlToValidate="txtProductionQuantity" Display="Dynamic" ErrorMessage=" * " />
                                        <asp:Literal  runat="server" ID="ltProductionQuantity" Text="Quantity:"/><br />
                                        <asp:TextBox runat="server" ID="txtProductionQuantity" onKeyPress="return checkDec(this,event)" onblur="CheckDecimal(this)" Width="200" />
                    
                                </td>
                               
                                
                            </tr>
                            

                            <tr>
                                <td align="left" colspan="2">
                    
                                        <asp:Literal  runat="server" ID="ltRemarks" Text="Remarks:"/><br />
                                        <asp:TextBox runat="server" ID="txtRemarks" onKeypress="return checkSpecialChar(event)" TextMode="MultiLine"  Width="87%" Height="70" />
                    
                                </td>

                                <td align="right" colspan="2" style="padding-right:34px">
                                    <br />
                                            <asp:CheckBox ID="chkIsActive" Text="Active" runat="server" Visible="false" />&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="chkIsDeleted" onclick="CheckIsDelted(this);" Text="Delete" Visible="false" runat="server" />
                                    <br /><br />
                                          <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click" CssClass="ui-btn ui-button-large" >Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkUpdate" OnClientClick="showAsynchronus();" runat="server" CssClass="ui-btn ui-button-large" OnClick="lnkUpdate_Click" ></asp:LinkButton>
                                </td>
           
                            </tr>
                                 <tr>
             <td colspan="4">
                 <asp:Label ID="lbMaterialCombination" runat="server" Text="Phantom Material Possibility Matrix" Font-Size="20px" ForeColor="#0066cc"></asp:Label>
                 <asp:Panel ID="pnlPanthomMaterial"  runat="server" Height="400" ScrollBars="Vertical">
                         <asp:GridView ID="gvPanthomMaterial" ClientIDMode="Static" runat="server" SkinID="gvLightGreenNew" >
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <nobr>
                                        <asp:Label ID="lbPartNumber" ForeColor="#0B3B39" runat="server" Text="Part Number" /></nobr><br />
                                        <asp:Label ID="lbBoMRatio" Font-Size="12px" ForeColor="#2E2EFE" runat="server" Text="[BOM Ratio]" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbSideHeadBatchNo" ForeColor="#0B3B39" runat="server" Text="Batch No."></asp:Label><br />
                                        <asp:Label ID="lbSideHeaderRatio" Font-Size="12px" ForeColor="#2E2EFE" runat="server" Text="[Total Qty.]"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                         </asp:GridView>
                 </asp:Panel>
             </td>
         </tr>
         <tr>
             <td align="right" colspan="4" style="padding-top:20px;">
                 
                 <asp:LinkButton ID="lnkCreateOutBound" runat="server" OnClientClick="showAsynchronus();" OnClick="lnkCreateOutBound_Click" CssClass="ui-btn ui-button-large">Create Outbound<%=MRLWMSC21Common.CommonLogic.btnfaSave %></asp:LinkButton>
                 
             </td>
         </tr>
                                <tr>
                                    <td class="accordinoGap"></td>
                                </tr>

                           
                                </table>
                            <//div>
             </td>
         </tr>   
        
      </table>
    
</asp:Content>
