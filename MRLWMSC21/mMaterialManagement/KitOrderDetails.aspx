<%@ Page Title="" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="KitOrderDetails.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.KitOrderDetails" %>



<asp:Content ID="Content2" ContentPlaceHolderID="MMContent" runat="server">
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="PODetailsScript" SupportsPartialRendering="true"></asp:ScriptManager>
    <style>
 

        #MainContent_MMContent_LinkButton3 {
            text-decoration: none;
            box-shadow: 0 3px 1px -2px rgba(0,0,0,.2), 0 2px 2px 0 rgba(0,0,0,.14), 0 1px 5px 0 rgba(0,0,0,.12);
            background: var(--sideNav-bg);
            font-weight: normal;
            color: #fff;
            border-radius: 3px;
            margin-bottom: 9px;
            display: inline-block;
    
            z-index: 998;
        }

        .gvLightSkyBlueNew_DataCellGridEdit a{

                display: inline-block;
                font-weight: 400;
                text-align: center;
                white-space: nowrap;
                vertical-align: middle;
                -webkit-user-select: none;
                -moz-user-select: none;
                -ms-user-select: none;
                user-select: none;
                border: 1px solid transparent;
                padding: 0.3rem 0.75rem;
                font-size: 1rem;
                line-height: 1.5;
                border-radius: 0.25rem;
                transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out, border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
                transition: all 0.8s;
                font-size: 12.5px !important;
                background-color: var(--sideNav-bg);
                box-shadow: 0 0 2px rgba(0,0,0,0.12), 0 2px 2px rgba(0,0,0,0.24);
                transition: all .3s;
                border-radius: 2px;
                box-sizing: border-box;
                outline: 0;

                margin-left:5px;
                position: relative;
                overflow: hidden;
                transform: translate3d(0,0,0);
                background-image: url(../Images/banner-bg3e7818e040f1fb3244fc67d5d4b29ab6.png);
                background-size: cover;

                color: #fff !important;
                background-color: var(--sideNav-bg);
                border-color: transparent;
                background: var(--sideNav-bg);
                border: 0px;
                display: inline;
                align-self: center;
        }

        .flex input[type="text"], input[type="number"], textarea {
            width:100% !important;
        }
    </style>
    <script>
        $(document).ready(function () {
           
            fnLoadMCode();
        });
        function checkNum(evt) {

            evt = (evt) ? evt : window.event
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                status = "This field accepts numbers only."
                return false
            }
            status = "";
            return true;
        }
        var UomResult;
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadMCode();
            }
        } 
        function fnLoadMCode() {
            var textfieldname1 = $('.vDynaMcode');
            DropdownFunction(textfieldname1);
            var val =<%= headerid %>;
            debugger;
            $(".vDynaMcode").autocomplete({
                source: function (request, response) {
                    
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadKitMcodes") %>',
                        data: "{ 'prefix': '" + request.term + "','tenantid':'" + $('#<%=this.hidTenantID.ClientID%>').val() + "','kittypeid': '" + $('#<%= ddlkittypes.ClientID %>').val() + "','headerid': '" + val + "'}",
                             dataType: "json",
                             type: "POST",
                             contentType: "application/json; charset=utf-8",
                             success: function (data) {
                                 if (data.d == "" || data.d == "/,") {
                                     response($.map(data.d, function (item) {
                                         return {
                                             label: item.split(',')[0],
                                             val: item.split(',')[1]
                                         }
                                     }))
                                     showStickyToast(false, 'No Kits Availble For Your Search Crieteria', false);
                                     //alert("No Kits Availble For Your Search Crieteria");
                                     return;
                                 }

                                 response($.map(data.d, function (item) {
                                     return {
                                         label: item.split(',')[0],
                                         val: item.split(',')[1]
                                     }
                                 }))
                             },
                             error: function (response) {


                             },
                             failure: function (response) {


                             }
                         });
                     },
                     select: function (e, i) {

                         $("#<%=hifDynaMCodeId.ClientID %>").val(i.item.val);

                     },
                     minLength: 0
            });

            var textfieldname = $('#<%=txtTenant.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtTenant.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantsForWHList") %>',
                        data: "{ 'prefix': '" + request.term + "','AccountID':'" + '<%= cp.AccountID %>'+"'}",//<=cp.TenantID%>
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hidTenantID.ClientID %>").val(i.item.val);
                   

                   
                },
                minLength: 0,

            });

          
        }
    </script>
    
    <div class="dashed"></div>
   
    <div class="pagewidth">
         <div class="flex__ end">
        <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-primary" SkinID="lnkButEmpty" PostBackUrl="~/mMaterialManagement/KitOrdersList.aspx"><i class="material-icons vl">arrow_back</i>Back to List</asp:LinkButton>
    </div>

    <div class="">

        <div class="row">
            <div class="col-md-3">
                <div class="flex">
                    <asp:TextBox ID="txtrefno" runat="server" Enabled="false" required="" />
                    <label>Kit Order Ref. No.</label>
                </div>
            </div>
             <div class="col-md-3">
                <div class="flex">
                     <asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Auto"  required="required"/>
                    <label>Tenant</label>
                </div>
            </div>
            <div class="col-md-3">
                <div class="flex flex____">
                    <asp:DropDownList ID="ddlkittypes" runat="server" required="" />
                    <label>Kit Type.</label>
                </div>
            </div>
            
            <div class="col-md-3">
                <div class="flex">
                    <asp:TextBox ID="txtstatus" runat="server" Text="New" Enabled="false" required="" MaxLength="50" />
                    <label>Status</label>
                </div>
            </div>

        </div>
        <div class="row">  
                <div class="col-md-12">
           
                    <div class="flex__ right">
                        <asp:UpdatePanel ChildrenAsTriggers="true" ID="uppoNumber" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkUpdate" CssClass="btn btn-primary" ValidationGroup="FleetCordValidation" OnClick="lnkUpdate_Click" runat="server">Create <i class="material-icons">add</i>                               
                                </asp:LinkButton>&nbsp;&nbsp;
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <button type="reset" runat="server" id="btnclear" class="btn btn-primary">Clear <i class="material-icons">clear_all</i></button>
                    </div>
                </div>
        </div>   


         </div>
    
        <div class="row">
                      <div class="col-md-1 col-md-offset-11 right">
                                                    <asp:LinkButton runat="server" ID="lnkAddNewKitItems" Font-Underline="false" OnClick="lnkAddNewKitItems_Click" CssClass="btn btn-sm btn-primary">
                                                    Add Kit Item <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                                    </asp:LinkButton>
                                                </div>
                                      
                                            </div>
      
    

        <asp:GridView SkinID="gvLightSkyBlueNew" ID="deatilslist" runat="server" ShowHeaderWhenEmpty="true" 
            AutoGenerateColumns="false" PagerSettings-Position="Bottom" AllowPaging="true" PageSize="10" 
            AllowSorting="True" HorizontalAlign="Left" OnSorting="deatilslist_Sorting" 
            OnPageIndexChanging="deatilslist_PageIndexChanging" 
             OnRowUpdating="deatilslist_RowUpdating"
          OnRowCancelingEdit="deatilslist_RowCancelingEdit"
             OnRowEditing="deatilslist_RowEditing"
            Width="899px">
            <Columns>



               <%-- <asp:TemplateField ItemStyle-Width="140" HeaderText="Tenant" ItemStyle-HorizontalAlign="left">
                    <ItemTemplate>

                        <asp:Literal runat="server" ID="ItRefNo" Text='<%# DataBinder.Eval(Container.DataItem, "tenant")  %>' />
                        <asp:Literal runat="server" ID="Ittenntid" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "TenantID") %>' />
                        
                    </ItemTemplate>
                    <EditItemTemplate>
                        
                    </EditItemTemplate>
                </asp:TemplateField>--%>

                <asp:TemplateField ItemStyle-Width="150" HeaderText="Kit No." ItemStyle-HorizontalAlign="left">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="ltKitType" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                         <asp:Label ID="lbldetailsid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "KitJobOrderDetailsID") %>'></asp:Label>
                    </ItemTemplate>
                     <EditItemTemplate>
                        <div class="gridInput flex">
                          
                            <asp:TextBox ID="txtMcode" runat="server" EnableTheming="false" CssClass="vDynaMcode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' required=""></asp:TextBox>
                            <asp:Label ID="itdeatilsid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "KitJobOrderDetailsID") %>'></asp:Label>
                            <asp:Label ID="lblkitid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "KitPlannerID") %>'></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvtxtPONumber" runat="server" ControlToValidate="txtMcode" Display="Dynamic" ErrorMessage=" * " />

                        </div>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="150" HeaderText="Quantity" ItemStyle-HorizontalAlign="left">
                    <ItemTemplate>
                       <%-- <asp:TextBox runat="server" ID="txtQuantity" Text='<%# DataBinder.Eval(Container.DataItem, "Quantity")  %>' />--%>
                        <asp:Literal runat="server" ID="ltquantity" Text='<%# DataBinder.Eval(Container.DataItem, "Quantity") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <div class="flex">
                            <asp:TextBox ID="txtQuantity" runat="server" onKeyPress="return checkNum(event)" EnableTheming="false"  Text='<%# DataBinder.Eval(Container.DataItem, "Quantity") %>' required=""></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rftxtqty" runat="server" ControlToValidate="txtQuantity" Display="Dynamic" ErrorMessage="  " />
                        </div>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                    <HeaderTemplate>
                        <nobr>Delete</nobr>
                    </HeaderTemplate>
                    <ItemTemplate>
                       <asp:CheckBox ID="chkIsDeletePOInvItems" runat="server" />
                    </ItemTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkDeletePOInvItem" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> <i class='material-icons ss'>delete</i></nobr>" OnClick="lnkDeletePOInvItem_Click" OnClientClick="return confirm('Are you sure want to delete?')" />
                    </FooterTemplate>
                </asp:TemplateField>

               
                    <asp:BoundField ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" Visible="false" DataField="EditName" ReadOnly="true" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" />
                    <asp:CommandField ItemStyle-Font-Underline="false"  ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint"  ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr><i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" ControlStyle-Font-Underline="false" />
                
            </Columns>
           <%-- <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="&amp;gt;&amp;gt;"
                Mode="NumericFirstLast" PageButtonCount="15" />--%>


        </asp:GridView>
        </div>
     <div class="">
                      <div class="right">
                                                    <asp:LinkButton runat="server" ID="btninitiateorder" Font-Underline="false" OnClick="lnkAddNewKitItemsInitiateTransfer_Click" CssClass="btn btn-sm btn-primary">
                                                    Initiate Transfer 
                                                    </asp:LinkButton>
                                                </div>
                                      
                                            </div>
    <asp:HiddenField ID="hifDynaMCodeId" runat="server" />
    <asp:HiddenField ID="hidTenantID" runat="server" />
   <%-- <div style="float:right;padding-top:30px;padding-right:20px;">
         <asp:LinkButton ID="btnsavedetails" CssClass="btn btn-primary" ValidationGroup="FleetCordValidation" OnClick="lnkUpdateDeatils_Click" runat="server">Save Kit Details                                   
                            </asp:LinkButton>
    </div>--%>
   
</asp:Content>
