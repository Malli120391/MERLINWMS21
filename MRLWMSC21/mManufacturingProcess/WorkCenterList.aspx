<%@ Page Title=" Workstation List :." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="WorkCenterList.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.WorkCenterList"  EnableEventValidation="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">

      <script  type="text/JavaScript">



          function check_uncheck(Val) {
              var ValChecked = Val.checked;
              var ValId = Val.id;
              var frm = document.forms[0];
              // Loop through all elements
              for (i = 0; i < frm.length; i++) {
                  // Look for Header Template's Checkbox
                  //As we have not other control other than checkbox we just check following statement

                  if (this != null) {

                      if (ValId.indexOf('chkIsDeleteRFItemsAll') != -1) {
                          // Check if main checkbox is checked,
                          // then select or deselect datagrid checkboxes

                          if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('chkIsDeleteRFItem') != -1) {
                              if (ValChecked)
                                  frm.elements[i].checked = true;
                              else
                                  frm.elements[i].checked = false;
                          }
                      }
                      else if (ValId.indexOf('chkIsDeleteRFItem') != -1) {
                          // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                          if (frm.elements[i].checked == false)
                              frm.elements[1].checked = false;
                      }

                  }

              } // for
          } // function

          function confirmMsg() {

              //alert(frm.elements[i].name.indexOf("chkIsDelete"));
              var frm = document.forms[0];
              // loop through all elements
              for (i = 0; i < frm.length; i++) {
                  // Look for our checkboxes only
                  if (frm.elements[i].name.indexOf("lnkDeletePOInvItem") != -1) {
                      // If any are checked then confirm alert, otherwise nothing happens
                      if (frm.elements[i].checked)
                          return confirm('Are you sure you want to delete your selection(s)?')
                  }

              }
          }


          function check_uncheck1(Val) {
              var ValChecked = Val.checked;
              var ValId = Val.id;
              var frm = document.forms[0];
              // Loop through all elements
              for (i = 0; i < frm.length; i++) {
                  // Look for Header Template's Checkbox
                  //As we have not other control other than checkbox we just check following statement

                  if (this != null) {

                      if (ValId.indexOf('chkIsDeleteRFItemsAll1') != -1) {
                          // Check if main checkbox is checked,
                          // then select or deselect datagrid checkboxes

                          if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('chkIsDeleteRFItem1') != -1) {
                              if (ValChecked)
                                  frm.elements[i].checked = true;
                              else
                                  frm.elements[i].checked = false;
                          }
                      }
                      else if (ValId.indexOf('chkIsDeleteRFItem1') != -1) {
                          // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                          if (frm.elements[i].checked == false)
                              frm.elements[1].checked = false;
                      }

                  }

              } // for
          } // function

          function confirmMsg1() {

              //alert(frm.elements[i].name.indexOf("chkIsDelete"));
              var frm = document.forms[0];
              // loop through all elements
              for (i = 0; i < frm.length; i++) {
                  // Look for our checkboxes only
                  if (frm.elements[i].name.indexOf("chkIsDeleteRFItem1") != -1) {
                      // If any are checked then confirm alert, otherwise nothing happens
                      if (frm.elements[i].checked)
                          return confirm('Are you sure you want to delete your selection(s)?')
                  }

              }
          }


          function check_uncheck2(Val) {
              var ValChecked = Val.checked;
              var ValId = Val.id;
              var frm = document.forms[0];
              // Loop through all elements
              for (i = 0; i < frm.length; i++) {
                  // Look for Header Template's Checkbox
                  //As we have not other control other than checkbox we just check following statement

                  if (this != null) {

                      if (ValId.indexOf('chkIsDeletePOInvItemsAll') != -1) {
                          // Check if main checkbox is checked,
                          // then select or deselect datagrid checkboxes

                          if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('chkIsDeletePOInvItems') != -1) {
                              if (ValChecked)
                                  frm.elements[i].checked = true;
                              else
                                  frm.elements[i].checked = false;
                          }
                      }
                      else if (ValId.indexOf('chkIsDeletePOInvItems') != -1) {
                          // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                          if (frm.elements[i].checked == false)
                              frm.elements[1].checked = false;
                      }

                  }

              } // for
          } // function

          function confirmMsg2() {

              //alert(frm.elements[i].name.indexOf("chkIsDelete"));
              var frm = document.forms[0];
              // loop through all elements
              for (i = 0; i < frm.length; i++) {
                  // Look for our checkboxes only
                  if (frm.elements[i].name.indexOf("chkIsDeletePOInvItems") != -1) {
                      // If any are checked then confirm alert, otherwise nothing happens
                      if (frm.elements[i].checked)
                          return confirm('Are you sure you want to delete your selection(s)?')
                  }

              }
          }


          function check_uncheck3(Val) {
              var ValChecked = Val.checked;
              var ValId = Val.id;
              var frm = document.forms[0];
              // Loop through all elements
              for (i = 0; i < frm.length; i++) {
                  // Look for Header Template's Checkbox
                  //As we have not other control other than checkbox we just check following statement

                  if (this != null) {

                      if (ValId.indexOf('CheckAll') != -1) {
                          // Check if main checkbox is checked,
                          // then select or deselect datagrid checkboxes

                          if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('deleteRec') != -1) {
                              if (ValChecked)
                                  frm.elements[i].checked = true;
                              else
                                  frm.elements[i].checked = false;
                          }
                      }
                      else if (ValId.indexOf('deleteRec') != -1) {
                          // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                          if (frm.elements[i].checked == false)
                              frm.elements[1].checked = false;
                      }

                  }

              } // for
          } // function



          function checkDec(evt) {
              evt = (evt) ? evt : window.event
              var charCode = (evt.which) ? evt.which : evt.keyCode

              if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46) {
                  status = "This field accepts numbers only."
                  return false
              }
              status = "";
              return true;
          }


          function ClearText(TextBox) {
              if (TextBox.value == "Search Workstation...")
              TextBox.value = "";

              TextBox.style.color = "#000000";
          }

          function focuslost(TextBox) {
              if (TextBox.value == "")
                  TextBox.value = "Search Workstation...";

              TextBox.style.color = "#A4A4A4";
          }
    </script>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>

      <script type="text/javascript">

          $(document).ready(function () {
              var textfieldname = $('#<%=this.txtSearchWorkCenter.ClientID%>');
              DropdownFunction(textfieldname);
              $('#<%=this.txtSearchWorkCenter.ClientID%>').autocomplete({
                  source: function (request, response) {
                      $.ajax({
                          url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetWorkCenterType") %>',
                          data: "{ 'Prefix': '" + request.term + "'}",
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


    <table border="0" cellpadding="2" cellspacing="2" width="90%" align="center">

        <tr>
            <td align="right">
        <br />        

                
                <asp:Panel runat="server" ID="pnlworkcenter" DefaultButton="lnkSearch">
                    Workstation Type:
                <asp:DropDownList ID="ddlWrkcGroup" runat="server" Width="180"></asp:DropDownList>

                &nbsp;&nbsp;&nbsp;
                
                <asp:TextBox runat="server" ID="txtSearchWorkCenter" Text="Search Workstation..."  Width="180"    SkinID="txt_Hidden_Req_Auto"  onblur="javascript:focuslost(this)" onfocus="ClearText(this)"></asp:TextBox>


                <asp:LinkButton runat="server" ID="lnkSearch" OnClick="lnkSearch_Click" CssClass="ui-btn ui-button-large" >Search<%=MRLWMSC21Common.CommonLogic.btnfaSearch %></asp:LinkButton>
                </asp:Panel>
            </td>

             <td align="right" valign="bottom">

                <asp:ImageButton ID="imgbtngvWCList" runat="server"  ImageAlign="Right" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvWCList_Click" ToolTip="Export To Excel" />
            </td>
        </tr>

        <tr>
            <td colspan="2">

                  <asp:Panel runat="server" ID="pnlWCList" >

                            

                    <asp:Label runat="server" ID="lblRecordCount" CssClass="SubHeading3"/>

                      
                      

                      <asp:Label runat="server" ID="lblStatus"/>
                      
                      
                 
                    <asp:GridView  ID="gvWCList" runat="server" Font-Underline="false"  PagerSettings-Position="TopAndBottom"  AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightSkyBlueNew" HorizontalAlign="Left"   OnPageIndexChanging="gvWCList_PageIndexChanging"  >
                    
                  <Columns>
                          <asp:TemplateField ItemStyle-Width="250" HeaderText="Workstation"  HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                      <asp:Literal runat="server" Visible="false" ID="ltWorkCenterID"  Text='<%# DataBinder.Eval(Container.DataItem, "WorkCenterID") %>'/>
                                    <asp:Literal runat="server" ID="ltWorkCenter"  Text='<%# DataBinder.Eval(Container.DataItem, "WorkCenter") %>'/>
                          </ItemTemplate>
                          </asp:TemplateField>  
                                    
                          <asp:TemplateField ItemStyle-Width="150" HeaderText="Code"   HeaderStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltWorkCenterRefNo" Text='<%# DataBinder.Eval(Container.DataItem, "WorkCenterRefNo") %>'/>
                                     </ItemTemplate>
                          </asp:TemplateField>                     
                          <asp:TemplateField ItemStyle-Width="150" HeaderText="Workstation Type"  HeaderStyle-HorizontalAlign="Center" >
                                      <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltWorkCenterGroup" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "WorkCenterGroup") %>'/>
                                           <asp:HyperLink ID="HyperLink12"  Text='<%# DataBinder.Eval(Container.DataItem, "WorkCenterGroup") %>' NavigateUrl='<%#  String.Format("~/mManufacturingProcess/WorkGroups.aspx?wgid={0}",DataBinder.Eval(Container.DataItem, "WorkCenterGroupID").ToString())  %>'  Font-Underline="false" runat="server" ></asp:HyperLink>
                                      </ItemTemplate>
                          </asp:TemplateField>                      
                         

                        <asp:TemplateField ItemStyle-Width="50"  HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint"  ControlStyle-CssClass ="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                                    <HeaderTemplate>
                                                        <nobr><asp:CheckBox ID="chkIsDeletePOInvItemsAll" onclick="return check_uncheck2(this );"  runat="server" /> Delete</nobr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsDeletePOInvItems" runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>

                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkDeletePOInvItem" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> Delete <img src='../Images/redarrowright.gif' border='0' /></nobr>" OnClick="lnkDeletePOInvItem_Click" OnClientClick="return confirm('Are you sure, you want to delete the selected items?')" />
                                                    </FooterTemplate>
                                                </asp:TemplateField> 
                                             
                           <asp:TemplateField ItemStyle-Width="55" ItemStyle-HorizontalAlign="Center">
                                           <ItemTemplate>
                                               
                                               <asp:HyperLink ID="HyperLink1" Text="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" NavigateUrl='<%#  String.Format("WorkCenters.aspx?wcid={0}",DataBinder.Eval(Container.DataItem, "WorkCenterID").ToString())  %>' Font-Underline="false" runat="server" ></asp:HyperLink>

                                                   
                                           </ItemTemplate>
                           </asp:TemplateField>                         
                                                    
                    </Columns>
                             <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;"  Mode="NumericFirstLast" PageButtonCount="15" />
                           
               </asp:GridView>
                
              

                        </asp:Panel>


            </td>
        </tr>

    </table> 
    
                <br /><br />
                <br /><br />
                <br /><br />
               
</asp:Content>
