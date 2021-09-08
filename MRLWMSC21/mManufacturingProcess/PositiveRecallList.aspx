<%@ Page Title="" Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="PositiveRecallList.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.PositiveRecallList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">

     <asp:ScriptManager runat="server" ID="ssss" SupportsPartialRendering="true" EnablePartialRendering="true"></asp:ScriptManager> 
    
    <script type="text/javascript" src="Scripts/jquery.blockUI.js"></script>
    <script>
        
        $(document).ready(function () {
            $("#divPositiveReCall").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 50,
                minWidth: 300,
                height: 260,
                width: 400,
                resizable: false,
                draggable: false,
                position: {
                    my: "center middle",
                    at: "center middle",
                    of: window
                },
                open: function (event, ui) { $(this).parent().appendTo("#disputeDivPositiveReCall"); }
            });
        });
       
        function closePositiveReCallDialog() {
            //Could cause an infinite loop because of "on close handling"
            $("#divPositiveReCall").dialog('close');
        }

        function openPositiveReCallDialog() {
            // alert("from");
            // BuildDialogBox(GoodsMovementID)
            $("#divPositiveReCall").dialog("option", "title", "Positive ReCall Dailog Box");
            $("#divPositiveReCall").dialog('open');
           // document.getElementById('lbProRefNo').innerText = ProRefNo;
           // document.getElementById('lbWorkCenter').innerHTML = WorkCenter;
           // document.getElementById('hifReCallID').value = RecallID;
            
        }

        function openPositiveReCallDialogAndBlock(title) {

            openPositiveReCallDialog(title);
            //block it to clean out the data
            $("#divPositiveReCall").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });

            //unblockDialog();
        }
        function unblockPositiveReCallDialog() {
            $("#divPositiveReCall").unblock();
        }
    </script>

       <div id="disputeDivPositiveReCall">
     <div id="divPositiveReCall"  >
         <asp:UpdatePanel  ID="upnlReCallClose" runat="server">
                    <ContentTemplate>
                                <table  Width="350">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbStatus" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Pro Ref. No. <asp:Label ID="lbProRefNo" runat="server" /> <br /><br />WorkCenter:<asp:Label ID="lbWorkCenter"  runat="server" /><asp:HiddenField ID="hifReCallID" ClientIDMode="Static" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td Height="130">
                                            Description:<br/>
                                            <asp:TextBox ID="txtResonForClose" runat="server" TextMode="MultiLine" Width="230" Height="100" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <br />
                                            <asp:LinkButton ID="lnkCloseRecall" SkinID="lnkButSave"  OnClick="lnkCloseRecall_Click" runat="server" Text="Close ReCall" />

                                        </td>
                                    </tr>
                                </table>
       
                    </ContentTemplate>
             </asp:UpdatePanel>
         </div>
        </div>




   
    <table align="center" Height="500px">
        <tr valign="top">
            <td valign="top">
                <asp:UpdatePanel  ID="upnlGridView" runat="server" >
                        <ContentTemplate>
                            <br />
                            <br />
                             <asp:GridView ID="gvPositiveRecall" OnRowCommand="gvPositiveRecall_RowCommand" OnRowDataBound="gvPositiveRecall_RowDataBound" OnPageIndexChanging="gvPositiveRecall_PageIndexChanging" runat="server" AutoGenerateColumns="false" SkinID="gvLightGrayNew" >
                                 <Columns>
                                        <asp:TemplateField HeaderText="PRO Ref. No.">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltPRORefNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PRORefNo").ToString() %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ON WorkCenter">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltONWorkCenter" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ONWorkCenter").ToString() %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recall On Date">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltPositiveRecallOnDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PositiveRecallOnDate","{0:dd/MM/yyyy}").ToString() %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason For Positive Recall">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltRemarks" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Remarks").ToString() %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recall Created By">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltuserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "userName").ToString() %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Close Recall" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lnkClose" CommandArgument='<%# String.Format("{0},{1},{2}",DataBinder.Eval(Container.DataItem, "PRORefNo").ToString(),DataBinder.Eval(Container.DataItem, "ONWorkCenter").ToString(),DataBinder.Eval(Container.DataItem, "PositiveRecallID").ToString())  %>' ImageUrl="~/Images/cancel.png" runat="server" Text="bvhg"  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                             </asp:GridView>

                        </ContentTemplate>
                 </asp:UpdatePanel> 
            </td>
        </tr>
    </table>
                         

</asp:Content>
