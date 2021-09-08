<%@ Page Title="My Account" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="AccountList.aspx.cs" Inherits="MRLWMSC21.TPL.AccountList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">

    <asp:ScriptManager runat="server" ID="spmngrOBDTracking" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>
          <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
      <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    
     <script>
        function ClearText(TextBox) {
            if (TextBox.value == "PO Number...") {
                TextBox.value = "";
                TextBox.style.color = "#000000";
            }
        }
        $(document).ready(function(){
      var textfieldname = $('#<%=this.txtAccount.ClientID%>');
                DropdownFunction(textfieldname);

                $('#<%=this.txtAccount.ClientID%>').autocomplete({

                    source: function (request, response) {
                        debugger;
                         if ($("#<%= this.txtAccount.ClientID %>").val() == '') {
                            $("#<%=hifAccount.ClientID %>").val("0");
                                $("#hiAccountName").val("");
                         }
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadAccountDataFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                debugger;
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

                        $("#<%=hifAccount.ClientID %>").val(i.item.val);
                        $('#hiAccountName').val(i.item.label);
                    },
                    minLength: 0
                });
        }) 

        function ClearText(TextBox) {
            if (TextBox.value == "Search Account...") {
                TextBox.value = "";
                TextBox.style.color = "#000000";
            }
        }
        function focuslost(TextBox) {
            if (TextBox.value == "") {
                TextBox.value = "Search Account...";
                TextBox.style.color = "#A4A4A4";
            }
        }
         function validate() {
             debugger;
           <%-- if ($('#<%=this.txtAccount.ClientID%>').val() != $('#hiAccountName').val()) {--%>

             if ($('#<%=this.txtAccount.ClientID%>').val() == "" || $('#<%=this.txtAccount.ClientID%>').val() == undefined ||$("#<%=hifAccount.ClientID %>").val()==0) {
                 showStickyToast(false, 'Select valid Account');
                 return false;
             }
            else return true;
        }
       
    </script>
    
   <div class="container">
   
      <asp:UpdateProgress ID="uprgAccountList" runat="server" AssociatedUpdatePanelID="upnlAccountList">
        <ProgressTemplate>
            <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">    
            <div style="align-self:center;" >
                    <div class="spinner">
                <div class="bounce1"></div>
                <div class="bounce2"></div>
                <div class="bounce3"></div>
            </div>

            </div>
                                  
            </div>
                                
                                
        </ProgressTemplate>
        </asp:UpdateProgress>
    <asp:UpdatePanel ID="upnlAccountList" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="">
                <div>
                    <!-- Globalization Tag is added for multilingual  -->
                    <div class="row">
                        <div class="">
                            <div class="col m3 s3 offset-m7">
                                <div class="flex">
                                    <asp:Literal ID="ltMsg" runat="server" />
                                    <asp:TextBox ID="txtAccount" SkinID="txt_Hidden_Req_Auto" required="" runat="server" />
                                    <label><%= GetGlobalResourceObject("Resource", "SearchAccount")%> </label>
                                </div>
                            </div>
                            <div class="col m2 s2 p0">
                                    <gap5></gap5>
                                   <flex> <asp:HiddenField runat="server" ID="hifAccount" Value="0" />
                                    <input type="hidden" id="hiAccountName" />
                                    <asp:LinkButton ID="lnkGetData" runat="server" OnClientClick="return validate();" CssClass="btn ui-button-large" OnClick="lnkGetData_Click"> <%= GetGlobalResourceObject("Resource", "Search")%><%=MRLWMSC21Common.CommonLogic.btnfaSearch %></asp:LinkButton>
                                    <asp:LinkButton ID="lnkAddAccount" runat="server" CssClass="btn ui-button-large" PostBackUrl="~/TPL/NewAccount.aspx"><%= GetGlobalResourceObject("Resource", "Add")%><i class="material-icons vl">add</i></asp:LinkButton>
                            </flex>
                            </div>
                        </div>
                    </div>

                    <div class="ListDataRow" style="margin-bottom:20px;">
                        <div>
                            <asp:GridView SkinID="gvLightSkyBlueNew" ID="gvACCList" runat="server" AutoGenerateColumns="false" PagerSettings-Position="Bottom" AllowPaging="true" PageSize="25" AllowSorting="True" HorizontalAlign="Left" OnPageIndexChanging="gvACCList_PageIndexChanging" Width="899px" OnRowDataBound="gvACCList_RowDataBound">
                                <Columns>

                                    <asp:TemplateField  HeaderText="AccountID" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ItAccountID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "AccountID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField  HeaderText="Account" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ItCompanyName" Text='<%# DataBinder.Eval(Container.DataItem, "Account")  %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField  HeaderText="<%$Resources:Resource,Account%>" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div title='<%#Eval("Account") %>'><%#Eval("AccountCode") %>   </div>
                                            <%--  <asp:Literal runat="server" ID="ltAccountCode"  Text='<%# DataBinder.Eval(Container.DataItem, "AccountCode") %>'/>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="<%$Resources:Resource,CompanyName%>" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltLegalName" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyLegalName").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="<%$Resources:Resource,AccountSince%>" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltZoho" Text='<%# DataBinder.Eval(Container.DataItem, "AccountSince").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Zoho Subscription ID" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltZoho" Text='<%# DataBinder.Eval(Container.DataItem, "ZohoAccountId").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField  HeaderText="<%$Resources:Resource,Action%>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%--<a style="text-decoration:none;" id='<%# DataBinder.Eval(Container.DataItem, "AccountID").ToString() %>' href='<%# String.Concat("NewAccount.aspx?accountid=",DataBinder.Eval(Container.DataItem, "AccountID").ToString()) %>' > <i class="material-icons ss">mode_edit</i></a>--%>
                                            <a style="text-decoration: none;" id="btnAccountID" runat="server" href='<%# String.Concat("NewAccount.aspx?accountid=",DataBinder.Eval(Container.DataItem, "AccountID").ToString()) %>'><i class="material-icons ss">mode_edit</i></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    <div align="center">No Data Found</div>
                                </EmptyDataTemplate>
                                <PagerSettings FirstPageText="&amp;lt;&amp;lt;" LastPageText="&amp;gt;&amp;gt;"
                                    Mode="NumericFirstLast" PageButtonCount="15" />
                                
                            </asp:GridView>
                            
                        </div>

                    </div>

                </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>
