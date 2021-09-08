<%@ Page Title=" .: Tool Issues List :." Language="C#" MasterPageFile="~/mOrders/OrdersMaster.master" AutoEventWireup="true" CodeBehind="ToolIssueList.aspx.cs" Inherits="MRLWMSC21.mOrders.ToolIssueList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OrdersContent" runat="server">

      <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
     <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="ss" SupportsPartialRendering="true"></asp:ScriptManager>

    
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnMCodeAC();
            }
        }


        function fnMCodeAC() {
            $(document).ready(function () {

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



                var textfieldname = $('#<%= this.atcEditMCode.ClientID %>');
                DropdownFunction(textfieldname);
                $('#<%= this.atcEditMCode.ClientID %>').autocomplete({
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

                    $("#<%=hifmmid.ClientID %>").val(i.item.val);

                    },
                    minLength: 0
            });


            });
        }

    </script>

    <script type="text/javascript">

        $(document).ready(function () {

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


            var textfieldname = $('#<%= this.atcEditMCode.ClientID %>');
            DropdownFunction(textfieldname);
            $('#<%= this.atcEditMCode.ClientID %>').autocomplete({
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

                        $("#<%=hifmmid.ClientID %>").val(i.item.val);
                       
                    },
                    minLength: 0
                });



         });


    </script>

    <script>
        function ClearText(TextBox) {
            if (TextBox.value == "Search Tool ...") {
                TextBox.value = "";
                TextBox.style.color = "#000000";
            }
            if (TextBox.value == "Search Employee ...") {
                TextBox.value = "";
                TextBox.style.color = "#000000";
            }

        }
        function focuslost(TextBox) {
            if (TextBox.value == "") {
                TextBox.value = "Search Tool ...";
                TextBox.style.color = "#A4A4A4";
            }
           
        }
        function focuslost1(TextBox) {
            if (TextBox.value == "") {
                TextBox.value = "Search Employee ...";
                TextBox.style.color = "#A4A4A4";
            }

        }
    </script>
       <div class="dashed"></div>
    <div class="pagewidth">
      <table border="0" cellpadding="3" cellspacing="3" align="center" width="100%" style="padding:10px;">

          <tr>
              <td>
                     <asp:UpdatePanel runat="server" ID="upnlToolIssues" ChildrenAsTriggers="true" UpdateMode="Always">

                        <ContentTemplate>

                                                         <table border="0" cellpadding="3" cellspacing="3" align="center" width="100%" style="padding:10px;">

                                <tr>
                                    <td>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="FormLabels" align="right">

                                        <asp:TextBox runat="server" ID="atcEditMCode" SkinID="txt_Hidden_Req_Auto"   Text="Search Tool ..." onblur="javascript:focuslost(this)" onfocus="ClearText(this)"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox runat="server" ID="atcEmployee" SkinID="txt_Hidden_Req_Auto"  Text="Search Employee ..." onblur="javascript:focuslost1(this)" onfocus="ClearText(this)" ></asp:TextBox>

                                        &nbsp;&nbsp;&nbsp;

                                        <asp:LinkButton runat="server" ID="lnkIssueSearch" CssClass="ui-btn ui-button-large" OnClick="lnkIssueSearch_Click">Get Details<%=MRLWMSC21Common.CommonLogic.btnfaFilter %></asp:LinkButton>

                                           <asp:HiddenField runat="server" ID="hifEmployee" />
                                           <asp:HiddenField runat="server" ID="hifmmid" />

                                    </td>
                                </tr>

                                <tr>
                                    <td>


                
                                                             <asp:GridView Width="100%"  ShowFooter="false"  GridLines="None" CellPadding="1" CellSpacing="1" ID="gvToolIssueList" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightGrayNew" HorizontalAlign="Left" OnPageIndexChanging="gvToolIssueList_PageIndexChanging" >
                                                                    <Columns>
                                                
                                                                        <asp:TemplateField  HeaderText="Issue Ref. No."  >
                                                                            <ItemTemplate>
                                                                                <asp:Literal runat="server" ID="ltSONumber" Text='<%# DataBinder.Eval(Container.DataItem, "SONumber") %>'/>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                         <asp:TemplateField  HeaderText="Employee"  ItemStyle-CssClass="home">
                                                                            <ItemTemplate>
                                                                                <asp:Literal runat="server" ID="ltEmployee" Text='<%# DataBinder.Eval(Container.DataItem, "Employee").ToString() %>'/>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                         <asp:TemplateField  ItemStyle-Width="150"   HeaderText="Tool" >
                                                                             <ItemTemplate>
                                                                                <asp:Literal ID="ltMCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>'/>
                                                                             </ItemTemplate>                                   
                                                                         </asp:TemplateField>
                                               
                                                                            <asp:TemplateField  HeaderText="Serial No."  ItemStyle-CssClass="home">
                                                                            <ItemTemplate>
                                                                                <asp:Literal runat="server" ID="ltSerialNo" Text='<%# DataBinder.Eval(Container.DataItem, "SerialNo").ToString() %>'></asp:Literal>
                                                                            </ItemTemplate>
                                                                           </asp:TemplateField>  
                                                                            <asp:TemplateField  HeaderText="Batch No."  ItemStyle-CssClass="home">
                                                                             <ItemTemplate>
                                                                              <asp:Literal runat="server" ID="ltBatchNo" Text='<%# DataBinder.Eval(Container.DataItem, "BatchNo").ToString() %>'/>
                                                                            </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                             <asp:TemplateField  ItemStyle-CssClass="home"  HeaderText="Calibration Due Date"  >
                                                                             <ItemTemplate>
                                                                                 <asp:Literal runat="server" ID="ltCalibrationDueDate" Text='<%# DataBinder.Eval(Container.DataItem, "CalibrationDueDate","{0: dd/MM/yy}")%>'></asp:Literal>
                                                                            </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                    </Columns>
                                                            </asp:GridView>



                                    </td>
                                </tr>


                            </table>


                        </ContentTemplate>

                    </asp:UpdatePanel>

              </td>
          </tr>


          </table>

 </div>



    <br /><br /><br /><br /><br /><br /><br /><br /><br />
</asp:Content>
