<%@ Page Title=" Workstation Type :." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="WorkGroups.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.WorkGroups" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">

      <asp:ScriptManager ID="spmngrWorkCenter" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>
    
    <script type="text/javascript" src="../Scripts/CommonScripts.js"></script>
    <script type="text/javascript" src="../Scripts/timeentry/jquery.timeentry.js"></script>

    <script type="text/javascript" >
         
        
        $(document).ready(function () {

            $(".MCodePicker").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMCodeData") %>',
                         data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "' }",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (data) {
                             response(data.d)
                         },
                         error: function (response) {
                             
                         },
                         failure: function (response) {
                             
                         }
                     });
                 },
                 minLength: 0
            });

            $('.BoMRefPicker').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadBoMVersionRefNoList") %>',
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

                     $("#<%=hifBoMRevID.ClientID %>").val(i.item.val);

                      },
                      minLength: 0
            });


        });

         </script>



    <table border="0" cellpadding="2" cellspacing="2" align="center" width="95%">

         <tr>
            <td colspan="3">
              &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label CssClass="errorMsg" runat="server" ID="lblStatus"></asp:Label>
            </td>
        </tr>

        <tr>
            <td colspan="3" align="right">
                <asp:HyperLink ID="HyperLink1" NavigateUrl="~/mManufacturingProcess/WorkGroupList.aspx" Font-Underline="false" runat="server" CssClass="ui-button-small" >Workstation Type List<%=MRLWMSC21Common.CommonLogic.btnfaList %></asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td class="FormLabels">
                  <asp:RequiredFieldValidator ID="rfvtxtGroupName" SetFocusOnError="true" ControlToValidate="txtGroupName" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="InitiateWG"/>
                Workstation Type: <br />
                <asp:TextBox runat="server" ID="txtGroupName" Width="200"></asp:TextBox>
            </td>
            <td class="FormLabels">
                <asp:RequiredFieldValidator ID="rfvtxtGroupRefNo" SetFocusOnError="true" ControlToValidate="txtGroupRefNo" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="InitiateWG"/>
                Workstation Type Code: <br />
                <asp:TextBox runat="server" ID="txtGroupRefNo" Width="200"></asp:TextBox>
            </td>
            <td class="FormLabels">
                <asp:RequiredFieldValidator  InitialValue="0" ID="rfvddlCompany" SetFocusOnError="true" ControlToValidate="ddlCompany" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="InitiateWG" />
                Company: <br />
                <asp:DropDownList runat="server" ID="ddlCompany" Width="200"></asp:DropDownList>
            </td>

        </tr>
        <tr>
            <td colspan="3" class="FormLabels">
                Description: <br />
                <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="5" Width="500"></asp:TextBox>
            </td>
        </tr>



           <tr>
            <td colspan="3" align="right">
                <br />
                <asp:LinkButton  runat="server" ID="lnkCancel" OnClick="lnkCancel_Click" CssClass="ui-btn ui-button-large" >Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>
                &nbsp;&nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkWCGroup" OnClick="lnkWCGroup_Click" CssClass="ui-btn ui-button-large" ValidationGroup="InitiateWG"></asp:LinkButton>
            </td>
        </tr>




          <tr>
            <td colspan="3">
                <asp:Label ID="Label1" runat="server" Visible="false" Text="Finished Material Details:" CssClass="SubHeading" ></asp:Label> 
            </td>
        </tr>
        <tr>
            
            <td colspan="3" align="right">
                <br />
                <asp:LinkButton runat="server" ID="lnkAddFProduct" Text="Add New" SkinID="lnkButEmpty" OnClick="lnkAddFProduct_Click" Visible="false"></asp:LinkButton>
                
            </td>
           
           
        </tr>
        <tr>
            <td colspan="3" class="FormLabels">
                <asp:Label runat="server" ID="lblFinshedProductStatus"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" >

                 <asp:Panel runat="server" ID="pnlFinishedProducts"  Visible="false"  HorizontalAlign="Center" >

                                        <asp:GridView ID="gvFinishedProduct" HorizontalAlign="Center" SkinID="gvLightGreen"  runat="server"  CellPadding="0"  AllowPaging="true"
                                            AllowSorting="false"
                                            OnPageIndexChanging="gvFinishedProduct_PageIndexChanging"
                                            OnRowDataBound="gvFinishedProduct_RowDataBound" 
                                            OnRowCommand="gvFinishedProduct_RowCommand"
                                            OnRowUpdating="gvFinishedProduct_RowUpdating" 
                                            OnRowCancelingEdit="gvFinishedProduct_RowCancelingEdit"  
                                            OnRowEditing="gvFinishedProduct_RowEditing"
                                            PagerStyle-HorizontalAlign="Right"
                                            PageSize="10"
                                            CellSpacing="2">
                                            <Columns>

                                                <asp:TemplateField HeaderText="Line No." ItemStyle-Width="100" >
                                                    <ItemTemplate>
                                                       
                                                        <asp:Literal ID="lthidWorkCenterGroupFinishedProductsID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "WorkCenterGroupFinishedProductsID") %>' />
                                                         <asp:Literal ID="ltLineNumber"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber") %>' />
                                                    </ItemTemplate>
                                                       
                                                    <EditItemTemplate>
                                                        <asp:Literal ID="lthidWorkCenterGroupFinishedProductsID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "WorkCenterGroupFinishedProductsID") %>' />
                                                         <asp:Literal ID="ltLineNumber"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber") %>' /> 
                                                    </EditItemTemplate>

                                                </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="BoM Ref.#" ItemStyle-Width="100">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ltBoMRefNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BOMRefNumber") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                          <asp:RequiredFieldValidator   ID="rfvtxtBoMRef" SetFocusOnError="true" ControlToValidate="txtBoMRef" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server"  />
                                                        <asp:Literal ID="ltBoMHederID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BOMHeaderRevisionID") %>' />
                                                        <asp:TextBox ID="txtBoMRef" EnableTheming="false" CssClass="BoMRefPicker"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>'  Width="150" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Finished Material">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ltMCode"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:RequiredFieldValidator ID="rfvtxtMCode" SetFocusOnError="true" ControlToValidate="txtMCode" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                        <asp:TextBox ID="txtMCode" EnableTheming="false" CssClass="MCodePicker"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>'  Width="150" />
                                                        <asp:HiddenField ID="hifDescMCodeID" ClientIDMode="Static" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "FinishedProduct_MaterialMasterID") %>' />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Source Location">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ltSourceLocationName"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SourceLocation") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:RequiredFieldValidator  InitialValue="0" ID="rfvddlLocations1" SetFocusOnError="true" ControlToValidate="ddlSourceLocations" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server"  />
                                                         <asp:Literal ID="ltSrcLocationID"  Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SourceLocationID") %>' />
                                                        <asp:DropDownList ID="ddlSourceLocations" runat="server" Width="150"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                </asp:TemplateField> 

                                                <asp:TemplateField HeaderText="Destination Location">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ltDestLocationName"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DestinationLocation") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:RequiredFieldValidator  InitialValue="0" ID="rfvddlLocations2" SetFocusOnError="true" ControlToValidate="ddlDestinationLocations" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server"  />
                                                         <asp:Literal ID="ltDestLocationID"  Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DestinationLocationID") %>' />
                                                        <asp:DropDownList ID="ddlDestinationLocations" runat="server" Width="150"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                </asp:TemplateField> 
                                                

                                                <asp:TemplateField HeaderText="Failed Product Destination Location">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ltFaildDestLocationName"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FailedProductionDestinationLocation") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:RequiredFieldValidator  InitialValue="0" ID="rfvddlLocations3" SetFocusOnError="true" ControlToValidate="ddlFaildDestinationLocations" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server"  />
                                                         <asp:Literal ID="ltFaildDestLocationID"  Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FailedProductionDestinationLocationID") %>' />
                                                        <asp:DropDownList ID="ddlFaildDestinationLocations" runat="server" Width="150"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField  HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint"  ControlStyle-CssClass ="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                                    <HeaderTemplate>
                                                        <nobr><asp:CheckBox ID="chkIsDeletePOInvItemsAll" onclick="return check_uncheck2(this );"  runat="server" /> Delete</nobr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsDeletePOInvItems" runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>

                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkDeletePOInvItem" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> Delete <img src='../Images/redarrowright.gif' border='0' /></nobr>" OnClick="lnkDeletePOInvItem_Click" OnClientClick="confirmMsg2();" />
                                                    </FooterTemplate>
                                                </asp:TemplateField> 
                                                <asp:BoundField ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" Visible="false" DataField="EditName" ReadOnly="true" ItemStyle-CssClass="NoPrint"  ControlStyle-CssClass ="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" />
                                                <asp:CommandField  ItemStyle-Font-Underline="false"  ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint"  ControlStyle-CssClass ="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint"  ItemStyle-HorizontalAlign="left"  CancelImageUrl="Images/cancel.gif"  EditImageUrl="/Images/edit.gif"  EditText="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif"  ControlStyle-Font-Underline="false" />
                                            </Columns>
                                        </asp:GridView>

                </asp:Panel>


            </td>
           
        </tr>




     


    </table>

    <br />
    <br /><br />
    <br /><br />
    <br /><br />
    <br />

    <asp:HiddenField runat="server" ID="hifBoMRevID" />

</asp:Content>
