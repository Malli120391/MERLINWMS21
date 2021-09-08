<%@ Page Title=" .: Ageing Analysis :." Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="MaterialAgingAnalysisList.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.MaterialAgingAnalysisList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">


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


                

                try {

                    var textfieldname = $('#<%=this.atcEditMCode.ClientID%>')
                     DropdownFunction(textfieldname);
                     $('#<%=this.atcEditMCode.ClientID%>').autocomplete({
                         source: function (request, response) {
                             //  alert('ssss');
                             $.ajax({
                                 url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetActiveStockMCode") %>',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" +<%=cp.TenantID%> +"'}",
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

                        $("#<%=this.hifMaterialCode.ClientID%>").val(i.item.val);
                        
                    },
                    minLength: 0
                }).data("autocomplete")._renderItem = function (ul, item) {
                    // Inside of _renderItem you can use any property that exists on each item that we built
                    // with $.map above */
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + item.label + "" + item.description + "</a>")
                        .appendTo(ul)
                };
            } catch (ex) {

            }



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



            try {

                var textfieldname = $('#<%=this.atcEditMCode.ClientID%>')
                    DropdownFunction(textfieldname);
                    $('#<%=this.atcEditMCode.ClientID%>').autocomplete({
                        source: function (request, response) {
                            //  alert('ssss');
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetActiveStockMCode") %>',
                                 data: "{ 'prefix': '" + request.term + "','TenantID':'" +<%=cp.TenantID%> +"'}",
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

                             $("#<%=this.hifMaterialCode.ClientID%>").val(i.item.val);

                    },
                    minLength: 0
                     }).data("autocomplete")._renderItem = function (ul, item) {
                         // Inside of _renderItem you can use any property that exists on each item that we built
                         // with $.map above */
                         return $("<li></li>")
                             .data("item.autocomplete", item)
                             .append("<a>" + item.label + "" + item.description + "</a>")
                             .appendTo(ul)
                     };
                } catch (ex) {

                }


        });


    </script>

    <script>
        function ClearText(TextBox) {
            if (TextBox.value == "Search Part# ...") {
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
                TextBox.value = "Search Part# ...";
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
      <table border="0" cellpadding="3" cellspacing="3" align="center" width="100%">

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

                                        <asp:TextBox runat="server" ID="atcEditMCode" SkinID="txt_Auto"   Text="Search Part# ..." onblur="javascript:focuslost(this)" onfocus="ClearText(this)" Width="200"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="ddlAge" runat="server" ></asp:DropDownList>


                                        <asp:TextBox runat="server" Visible="false" ID="atcEmployee" SkinID="txt_Hidden_Req_Auto"  Text="Search Employee ..." onblur="javascript:focuslost1(this)" onfocus="ClearText(this)" ></asp:TextBox>

                                        &nbsp;&nbsp;&nbsp;

                                        <asp:LinkButton runat="server" ID="lnkIssueSearch" Text="Get Details" SkinID="lnkButEmpty" OnClick="lnkIssueSearch_Click"></asp:LinkButton>

                                           <asp:HiddenField runat="server" ID="hifEmployee" />
                                           <asp:HiddenField runat="server" ID="hifmmid" />

                                    </td>
                                </tr>

                                <tr>
                                    <td>


                
                                                             <asp:GridView Width="100%"  ShowFooter="false"  GridLines="None" CellPadding="1" CellSpacing="1" ID="gvMAgingList" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightGreen" HorizontalAlign="Left"  OnRowCommand="gvMAgingList_RowCommand" OnRowDataBound="gvMAgingList_RowDataBound" OnSorting="gvMAgingList_Sorting" OnPageIndexChanging="gvMAgingList_PageIndexChanging" >
                                                                    <Columns>
                                                
                                                                        <asp:TemplateField  HeaderText="Part Number"  >
                                                                            <ItemTemplate>
                                                                                <asp:Literal runat="server" ID="ltMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>'/>
                                                                                <br />
                                                                                 <asp:Literal ID="ltOEMPartNo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                         <asp:TemplateField  HeaderText="Part Description"  ItemStyle-CssClass="home">
                                                                            <ItemTemplate>
                                                                                <asp:Literal runat="server" ID="ltMDescription" Text='<%# DataBinder.Eval(Container.DataItem, "MDescription").ToString() %>'/>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        
                                                                        <asp:TemplateField  HeaderText="BUoM / Qty." HeaderStyle-HorizontalAlign="left">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ltBUoMQty" runat="server" Text='<%# string.Format("{0} / {1}",DataBinder.Eval(Container.DataItem, "BUoM"),DataBinder.Eval(Container.DataItem, "BUoMQty")) %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField  HeaderText="MUoM / Qty." HeaderStyle-HorizontalAlign="left">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ltMUoMQty" runat="server" Text='<%# string.Format("{0} / {1}",DataBinder.Eval(Container.DataItem, "MUoM"),DataBinder.Eval(Container.DataItem, "MUoMQty")) %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>



                                                                         <asp:TemplateField  ItemStyle-Width="80"   HeaderText="Avail. Qty." >
                                                                             <ItemTemplate>
                                                                                <asp:Literal ID="ltAvailableQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AvailableQuantity") %>'/>
                                                                             </ItemTemplate>                                   
                                                                         </asp:TemplateField>
                                               
                                                                            <asp:TemplateField ItemStyle-Width="110"  HeaderText="Reorder Point"  ItemStyle-CssClass="home">
                                                                            <ItemTemplate>
                                                                                
                                                                                    <asp:Literal runat="server" ID="ltReorderPoint" Text='<%# DataBinder.Eval(Container.DataItem, "ReorderPoint").ToString() %>'></asp:Literal>
                                                                                
                                                                                
                                                                            </ItemTemplate>
                                                                           </asp:TemplateField>  

                                                                           <asp:TemplateField  ItemStyle-CssClass="home"  HeaderText="Goods-In Date"  >
                                                                             <ItemTemplate>
                                                                                 <asp:Literal runat="server" ID="ltCreatedOn" Text='<%# DataBinder.Eval(Container.DataItem, "CreatedOn","{0: dd/MM/yy}")%>'></asp:Literal>
                                                                            </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField  HeaderText="Age"  ItemStyle-CssClass="home">
                                                                             <ItemTemplate>
                                                                              <asp:Literal runat="server" ID="ltAge" Text='<%# DataBinder.Eval(Container.DataItem, "Age").ToString() %>'/>
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

    <asp:HiddenField ID="hifMaterialCode" runat="server" />










</asp:Content>
