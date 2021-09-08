<%@ Page Title=" .: Falcon Error List :." Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FalconErrorList.aspx.cs" Inherits="MRLWMSC21.FalconErrorList" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
         function confirmMsg() {
             var frm = document.forms[0];
             // loop through all elements
             for (i = 0; i < frm.length; i++) {
                 // Look for our checkboxes only
                 if (frm.elements[i].name.indexOf("deleteError") != -1) {
                     // If any are checked then confirm alert, otherwise nothing happens
                     if (frm.elements[i].checked)
                         return confirm('Are you sure you want to delete the selected facility details data?')
                 }
             }
             return false;
         }

         function check_uncheck(Val) {
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
                         if (ValChecked)
                             frm.elements[i].checked = true;
                         else
                             frm.elements[i].checked = false;
                     }
                     else if (ValId.indexOf('deleteError') != -1) {
                         // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                         if (frm.elements[i].checked == false)
                             frm.elements[1].checked = false;
                     }
                 } // if
             } // for
         }
    </script>

    <script type="text/javascript" language="javascript">
        $(function () {

            $('.DateBoxCSS').datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%=this.txtfromdate.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#<%=this.txttodate.ClientID%>").datepicker("option", "minDate", selected, { dateFormat: "dd/mm/yy" })
                  }
            });
            $("#<%=this.txttodate.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                maxDate: new Date()
            });

        });

    </script>
    <style>
        .lnkButtonDlete {
            text-decoration:none;
        }
    </style>
    <table cellpadding="5" cellspacing="2" class="internalData">

        <tr >
            <td width="200">From Date :<br /><asp:TextBox ID="txtfromdate" runat="server" Width="180"></asp:TextBox></td>
            <td width="200">To Date :<br /><asp:TextBox ID="txttodate" runat="server" Width="180" ></asp:TextBox></td>
            <td width="200"><br /><asp:LinkButton ID="lbtndateSearch" runat="server" CssClass="ui-btn ui-button-large" OnClick="lbtndateSearch_Click"> Search<%=MRLWMSC21Common.CommonLogic.btnfaSearch %></asp:LinkButton></td>
            <td></td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="lblCount" runat="server"/>
                <asp:GridView runat="server" AutoGenerateColumns="false" ShowFooter="true" AllowPaging="false" PageSize="10" PagerSettings-Position="TopAndBottom" OnPageIndexChanging="gvErrorsList_PageIndexChanging" SkinID="gvLightSteelBlueNew" id="gvErrorsList" Width="100%"  OnRowCommand="gvErrorsList_RowCommand" >
                    <Columns>
                        <asp:TemplateField  ItemStyle-Width="3%">
                            <HeaderTemplate>
                                <nobr>Sr. No.</nobr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1%>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField  ItemStyle-Width="10%">
                            <HeaderTemplate>
                                <nobr>User ID / Name</nobr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "UserID") %>
                                <asp:HiddenField ID="hifrowid" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ErrorCount") %>' />
                                </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField  ItemStyle-Width="10%">
                            <HeaderTemplate>
                                <nobr>Requested Page</nobr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "E_PageName").ToString().Split('_')[1].Split('_')[0] %>
                            </ItemTemplate>
                            <ItemStyle Width="3%" />
                        </asp:TemplateField>

                        <asp:TemplateField  ItemStyle-Width="5%">
                            <HeaderTemplate>
                                <nobr>Source File</nobr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "E_Source") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField  ItemStyle-Width="30%">
                            <HeaderTemplate>
                                <nobr>Root Cause</nobr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "E_Reason") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Stack Trace" ItemStyle-Width="20%">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "E_ST") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date Time" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "E_Time","{0:dd/MM/yy hh:mm:tt}") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField  ItemStyle-Width="10%"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <nobr>
                                &nbsp;&nbsp;&nbsp;&nbsp; Delete &nbsp;&nbsp;&nbsp;&nbsp;
                                    </nobr>
                                    <br />
                                <asp:CheckBox ID="CheckAll" Visible="false" onclick="return check_uncheck(this);" runat="server" />
                                    
                            </HeaderTemplate>
                                <ItemTemplate>
                                        <%--<asp:Label ID="RecID" Visible="false" Text='<%# Container.DataItemIndex %>' runat="server" />
                                        <asp:CheckBox ID="deleteError" runat="server"></asp:CheckBox>--%>
                                    <asp:LinkButton runat="server" id="lnkdelete" OnClick="lnkdelete_Click"  OnClientClick="return confirm('Are you sure want to delete?');"  Font-Underline="false">Delete <%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                                </ItemTemplate>
                            <FooterTemplate>
                                
                                    <asp:LinkButton ID="btnDelete" Visible="false" runat="server" CssClass="lnkButtonDlete" OnClick="btnDelete_Click" OnClientClick="return confirmMsg()">Delete<%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                                
                            </FooterTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;"
                                                Mode="NumericFirstLast" PageButtonCount="15" />
                                            <FooterStyle CssClass="gvCommon_footerGrid" />
                                            <RowStyle CssClass="gvCommon_DataCellGrid" />
                                            <EditRowStyle CssClass="gvCommon_DataCellGridEdit" />
                                            <PagerStyle CssClass="gvCommon_pager" />
                                            <HeaderStyle CssClass="gvCommon_headerGrid" />
                                            <AlternatingRowStyle CssClass="gvCommon_DataCellGridAlt" />
                </asp:GridView>
            </td>
        </tr>
        
    </table>
    
</asp:Content>
