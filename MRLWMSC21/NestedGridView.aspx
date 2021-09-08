<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NestedGridView.aspx.cs" Inherits="MRLWMSC21.NestedGridView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <script language=javascript type="text/javascript">
        function expandcollapse(obj, row) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);

            if (div.style.display == "none") {
                div.style.display = "block";
                if (row == 'alt') {
                    img.src = "minus.gif";
                }
                else {
                    img.src = "minus.gif";
                }
                img.alt = "Close to view other Customers";
            }
            else {
                div.style.display = "none";
                if (row == 'alt') {
                    img.src = "plus.gif";
                }
                else {
                    img.src = "plus.gif";
                }
                img.alt = "Expand to show Orders";
            }
        }

    </script>

    <br />
    <br />

     <div style="padding:10px;">

         <table border="0" width="100%">
             <tr>
                 <td align="right">
                     <asp:LinkButton runat="server" ID="lnkAddNewLineItem"  OnClick="lnkAddNewLineItem_Click" Text="Add Line Item" SkinID="lnkButEmpty"></asp:LinkButton>
                 </td>
             </tr>

         </table>

        <asp:GridView ID="gvParent" 
            SkinID="gvLightGreen"
            AllowPaging="True" 
            AutoGenerateColumns=false
            ShowFooter=true
            runat="server" 
            GridLines=None
            OnRowDataBound="gvParent_RowDataBound" 
            OnRowCommand = "gvParent_RowCommand" 
            OnRowUpdating = "gvParent_RowUpdating"
            OnRowCancelingEdit="gvParent_RowCancelingEdit"
            OnRowEditing="gvParent_RowEditing"
            OnPageIndexChanging="gvParent_PageIndexChanging"
            >

            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="javascript:expandcollapse('div<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID").ToString() %>', 'one');">
                            <img id='imgdiv<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID").ToString() %>' alt="Click to show/hide Orders for Customer <%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID").ToString() %>"  width="9px" border="0" src="plus.gif"/>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>

               <asp:TemplateField HeaderText="Seq. No."  ItemStyle-Width="60" >
                            <ItemTemplate>
                                <asp:literal id="ltSequenceNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SequenceNumber").ToString() %>' />
                                <asp:Literal ID="ltRoutingDetailsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Literal ID="ltRoutingDetailsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID").ToString() %>' />
                                <asp:Literal ID="ltSequenceNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SequenceNumber").ToString() %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Routing Name" ItemStyle-Width="150" >
                            <ItemTemplate>
                                <asp:literal id="ltName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ValidationGroup="UpdateGridItems" ControlToValidate="txtName" Display="Dynamic" ErrorMessage=" * " />
                                <asp:TextBox ID="txtName" runat="server" MaxLength="100" onKeypress="return checkSpecialChar(event)" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "Name").ToString() %>' />
                                
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="WorkCenter Group" ItemStyle-Width="160" >
                            <ItemTemplate>
                                <asp:literal id="ltWorkCenterGroup" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "WorkCenterGroup").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvWorkCenterGroup" runat="server" ValidationGroup="UpdateGridItems" ControlToValidate="atcWorkCenterGroup" Display="Dynamic" ErrorMessage=" * " />
                                <asp:TextBox ID="atcWorkCenterGroup" runat="server" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "WorkCenterGroup").ToString() %>' />
                                <asp:HiddenField ID="hifWorkCenterGroup" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "WorkCenterGroupID").ToString() %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Number of Cycles" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:literal id="ltNumberofCycles" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NumberofCycles").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvNumberofCycles" runat="server" ValidationGroup="UpdateGridItems" ControlToValidate="txtNumberofCycles" Display="Dynamic" ErrorMessage=" * " />
                                <asp:TextBox ID="txtNumberofCycles" Width="70" onKeyPress=" return checkNum(event)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NumberofCycles").ToString() %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Number of Hours" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:literal id="ltNumberofHours" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NumberofHours").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvNumberofHours" runat="server" ValidationGroup="UpdateGridItems" ControlToValidate="txtNumberofHours" Display="Dynamic" ErrorMessage=" * " />
                                <asp:TextBox ID="txtNumberofHours" Width="70" onKeyPress=" return checkNum(event)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NumberofHours").ToString() %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SerialNo. Req.">
                            <ItemTemplate>
                                <asp:Literal ID="ltIsSerialNumberRequire" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IsSerialNoRequired").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsSerialNumberRequire" runat="server" Checked='<%# Convert.ToBoolean( Convert.ToInt16(DataBinder.Eval(Container.DataItem, "IsSerialNoRequired"))) %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" ItemStyle-Width="180" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:literal id="ltDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                
                                <asp:TextBox ID="txtDescription" Width="180" MaxLength="200" onKeyPress=" return checkSpecialChar(event)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description").ToString() %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:CheckBox ID="chkisdeleted" runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>

                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkIsDeleted" runat="server" Text="Delete" OnClientClick="return confirm('Are you Sure want to delete?')" Font-Underline="false" OnClick="lnkIsDeleted_Click" />
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:CommandField CausesValidation="true" ButtonType="Link" ControlStyle-Font-Underline="false" CancelText="Cancel" EditText = "<nobr> Edit <img src='Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton = true UpdateText = "Update" />

			    <asp:TemplateField>
			        <ItemTemplate>
			            <tr>
                            <td colspan="100%">

                                <div id='div<%#  DataBinder.Eval(Container.DataItem, "RoutingDetailsID").ToString() %>' >



                                    <asp:GridView 
                                        
                                        ID="gvChild" 
                                        AllowPaging="True" 
                                        AllowSorting="true" 
                                        SkinID="gvLightSteelBlue"
                                        Width=100% 
                                        AutoGenerateColumns=false 
                                        runat="server" 
                                        ShowFooter=true
                                        GridLines=None 
                                        OnPageIndexChanging="gvChild_PageIndexChanging" 
                                        OnRowUpdating = "gvChild_RowUpdating"
                                        OnRowCommand = "gvChild_RowCommand" 
                                        OnRowEditing = "gvChild_RowEditing" 
                                        OnRowCancelingEdit = "gvChild_RowCancelingEdit" 
                                        OnRowDataBound = "gvChild_RowDataBound">

                                        <Columns>
                                             
                                       <asp:TemplateField ItemStyle-Width="100" HeaderText="Non Confirmity Type"  HeaderStyle-HorizontalAlign="Center" >
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltNonConfirmityType" Text='<%# DataBinder.Eval(Container.DataItem, "NonConfirmityType") %>'/>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="txtNonConfirmityType" EnableTheming="false" CssClass="NonConfirmityTypes" Text='<%# DataBinder.Eval(Container.DataItem, "NonConfirmityType") %>'/>
                                        </EditItemTemplate>
                                     </asp:TemplateField>
                                                   
                                    <asp:TemplateField ItemStyle-Width="80" HeaderText="Non Confirmity Code" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" >
                                        <ItemTemplate>
                                            <asp:Literal Visible="false"  runat="server" ID="ltHidRoutingDetails_ScrapCodeID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetails_NonConfirmityID") %>'/>
                                            <asp:Literal runat="server" ID="ltScrapCode" Text='<%# DataBinder.Eval(Container.DataItem, "NonConfirmityCode") %>'/>
                                            
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                             <asp:RequiredFieldValidator ID="rfvtxtScrapCode" runat="server" ControlToValidate="txtScrapCode" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal Visible="false"  runat="server" ID="ltHidRoutingDetails_ScrapCodeID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetails_NonConfirmityID") %>'/>
                                            <asp:TextBox ID="txtScrapCode" EnableTheming="false" CssClass="ScrapCodePicker" runat="server"  Width="160" Text='<%# DataBinder.Eval(Container.DataItem, "NonConfirmityCode") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                                                 
                                    <asp:TemplateField ItemStyle-Width="100" HeaderText="Description"  HeaderStyle-HorizontalAlign="Center" >
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltMDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'/>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="txtDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'/>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField  ItemStyle-Width="45" HeaderText="Delete"  ItemStyle-CssClass="txtCenteralign" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                         <ItemTemplate>
                                             <asp:CheckBox ID="chkChildIsDelete" runat="server"  />
                                         </ItemTemplate>
                                        <EditItemTemplate></EditItemTemplate>
                                         <FooterTemplate>
                                              <asp:LinkButton Font-Underline="false" ID="lnkRoutSCRDelete" runat="server" Text="Delete"  OnClick="lnkRoutSCRDelete_Click"  OnClientClick="return confirm('Are you sure you want to delete the selected Items?')"  />
                                              <img border="0" src="Images/redarrowright.gif" alt="delete"/>
                                         </FooterTemplate>
                                    </asp:TemplateField>  
                                    
                                    <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-Font-Underline="false"  ItemStyle-Width="40" ButtonType="Link"  CancelImageUrl="icons/cancel.gif" CancelText="Cancel" EditImageUrl="icons/edit.gif"  EditText="<nobr> Edit <img src='Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton="True" UpdateImageUrl="icons/update.gif" UpdateText="Update" />               

                                        </Columns>
                                   </asp:GridView>




                                </div>



                             </td>
                        </tr>
			        </ItemTemplate>			       
			    </asp:TemplateField>			    
			</Columns>
        </asp:GridView>

        
    </div>


    <br />
    <br />
     <br />
    <br />
     <br />
    <br />
     <br />
    <br />
</asp:Content>
