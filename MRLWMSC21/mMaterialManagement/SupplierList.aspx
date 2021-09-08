<%@ Page Title="Suppliers" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="SupplierList.aspx.cs" Inherits="MRLWMSC21.mInventory.SupplierList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">

    <asp:ScriptManager runat="server" ID="spmngrOBDTracking" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>
     <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
     <link rel="stylesheet" type="text/css" href="../App_Themes/MasterData/Master_Style.css" media="screen" />
    
  
    <style>
        img {
            border-style: none;
        }

        .srcbtn {
            position: relative;
            top: 3px;
            right: 5px;
        }

        .custom-file-input {
            color: transparent;
        }

        .txt_Blue_Small::-webkit-file-upload-button {
            visibility: hidden;
        }

        input[type="file"] {
            margin-bottom: 0px !important;
        }

        .txt_Blue_Small::before {
            content: 'Choose excel';
            color: #fff;
            display: inline-block;
            background: var(--sideNav-bg);
            border: 1px solid var(--sideNav-bg);
            /* border-radius: 3px; */
            padding: 3px 6px;
            outline: none;
            white-space: nowrap;
            -webkit-user-select: none;
            cursor: pointer;
            /* text-shadow: 1px 1px #fff; */
            font-weight: 500;
            font-size: 9pt;
            line-height: 18px;
            position: relative;
            top: -1px;
        }

        .ctxt_Blue_Small:hover::before {
            border-color: black;
        }

        .txt_Blue_Small:active {
            outline: 0;
        }

            .txt_Blue_Small:active::before {
                background: -webkit-linear-gradient(top, #e3e3e3, #f9f9f9);
            }

        [type="file"] {
            width: 100% !important;
            border: 1px solid #999;
            padding: 5px;
            margin-bottom: 5px;
        }

        label {
        }

        input[type="file"] {
            border: 1px solid var(--sideNav-bg) !important;
            padding: 0px !important;
            /* margin-bottom: 5px; */
            width: 100% !important;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            var tenentname = '';
            //debugger;     
            var TextFieldName = $("#<%= this.txtAccount.ClientID %>");
            DropdownFunction(TextFieldName);
            $("#<%= this.txtAccount.ClientID %>").autocomplete({
                source: function (request, response) {
                    debugger;
                    if ($("#<%= this.txtAccount.ClientID %>").val() == '') {
                        $("#<%=hifAccount.ClientID %>").val("0");
                         $("#<%=hiaccountname.ClientID %>").val("");
                    }
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadAccountDataFor3PL") %>',
                        data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>
                        dataType: "json",
                        type: "POST",
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
                    //debugger;
                    $("#<%=hifAccount.ClientID %>").val(i.item.val);
                    $("#<%=hiaccountname.ClientID %>").val(i.item.label);
                    //getTenant();
                },
                minLength: 0
            });
            var textfieldname = $('#<%=txtTenant.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtTenant.ClientID %>").autocomplete({

                source: function (request, response) {

                    debugger;
                     if ($("#<%= this.txtTenant.ClientID %>").val() == '') {
                        $("#<%=hifTenant.ClientID %>").val("0");
                         $("#<%=hiTenantName.ClientID %>").val("");
                    }
                    $.ajax({

                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantsForWHList") %>',
                        data: "{ 'prefix': '" + request.term + "','AccountID':" + $('#<%=this.hifAccount.ClientID%>').val() + "}",//<=cp.TenantID%>
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
                    $("#<%=hifTenant.ClientID %>").val(i.item.val);
                    $("#<%=txtSupplier.ClientID %>").val("");
                    
                     $("#<%=hiTenantName.ClientID %>").val(i.item.label);

                    var TenantID = $("#<%=hifTenant.ClientID %>").val();
                },
                minLength: 0,

            });
            var textfieldnames = $('#<%=txtSupplier.ClientID%>');
            DropdownFunction(textfieldnames);
            $("#<%= this.txtSupplier.ClientID %>").autocomplete({
                source: function (request, response) {
                      if ($("#<%= this.txtSupplier.ClientID %>").val() == '') {                        
                         $("#hifSupplier").val("");
                    }
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSupplierDataFor3PL") %>',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + $('#<%=this.hifTenant.ClientID%>').val() + "','Type':'Supplier'}",//<=cp.TenantID%>
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
                   $("#hifSupplier").val(i.item.label);
                },
                minLength: 0,

            });

        });

        function checkFileExtension(elem) {
            //debugger;
            var filePath = elem.value;

            if (filePath.indexOf('.') == -1)
                return false;

            var validExtensions = new Array();
            var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

            validExtensions[0] = 'xls';
            validExtensions[1] = 'xlsx';


            for (var i = 0; i < validExtensions.length; i++) {
                if (ext == validExtensions[i])
                    return true;
            }

            elem.value = "";
            alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
            return false;
        }


        function getTenant() {
            var TextFieldName = $("#<%= this.txtTenant.ClientID %>");
            var AccID = $("#<%=hifAccount.ClientID %>").val();
            DropdownFunction(TextFieldName);
            $("#<%= this.txtTenant.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantsForWHList") %>',
                        data: "{ 'prefix': '" + request.term + "','AccountID': '" + $("#<%=hifAccount.ClientID %>").val() + "'}",//<=cp.TenantID%>
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

                    $("#<%=hifTenant.ClientID %>").val(i.item.val);
                    //getSupplier();
                },
                minLength: 0
            });
        }






     //function getSupplier() {
     <%--    var textfieldname = $('#<%=this.txtSupplier.ClientID%>');
         DropdownFunction(textfieldname);
         $('#<%=this.txtSupplier.ClientID%>').autocomplete({

             source: function (request, response) {

                 $.ajax({
                     url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSupplierDataFor3PL") %>',
                     data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifTenant.ClientID %>').value + "','Type':'Supplier'}",//<=cp.TenantID%>
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
             minLength: 0
         });--%>

        <%-- var textfieldname = $('#<%=this.txtSupplierCode.ClientID%>');
         DropdownFunction(textfieldname);
         $('#<%=this.txtSupplierCode.ClientID%>').autocomplete({

             source: function (request, response) {

                 $.ajax({
                     url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSupplierCode") %>',
                     data: "{ 'SupplierCode': '" + request.term + "','TenantID':'" +<%= cp.TenantID %> +"'}",
                     dataType: "json",
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     success: function (data) {

                         response(data.d)
                     }
                 });
             },
             minLength: 0
         }); --%>    
     //}
    </script>

    <script>
        function validate() {
            if ($("#<%= this.txtAccount.ClientID %>").val() != $("#<%=hiaccountname.ClientID %>").val()) {
                showStickyToast(false, 'Select valid Account');
                return false;
            }
            else if ($("#<%= this.txtTenant.ClientID %>").val()!=$("#<%=hiTenantName.ClientID %>").val()) {
                showStickyToast(false, 'Select valid Tenant');
                return false;
            }
            else if ($('#<%=txtSupplier.ClientID%>').val() != $("#hifSupplier").val()) {  
                 showStickyToast(false, 'Select valid Supplier');
                return false;
            }
            else return true;
        }

        function ClearText(TextBox, text) {

            if (TextBox.value == text) {
                TextBox.value = "";
                TextBox.style.color = "#000000";
            }
        }
        function focuslost(TextBox, text) {
            if (TextBox.value == "") {
                TextBox.value = text;
                TextBox.style.color = "#A4A4A4";
            }
        }

    </script>


    <%--    <asp:PlaceHolder id="dontCare" runat="server">

   </asp:PlaceHolder>

<br />--%>
    <%-- <asp:UpdateProgress ID="uprgSupplierList" runat="server" AssociatedUpdatePanelID="upnlSupplierList">
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
        </asp:UpdateProgress>--%>
    <%--<asp:UpdatePanel ID="upnlSupplierList" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
        <ContentTemplate>--%>

    
    <div class="container">
        <div align="center" class="">

            <div class="row">
                <div class="">
                    <asp:Panel ID="pnlSupplierList" runat="server" DefaultButton="lnkGetData">
                        <div class="row">
                            <div>
                                <asp:TextBox ID="txtSupplierCode" Visible="false" onblur="javascript:focuslost(this,'Supplier Code...')" onfocus="ClearText(this,'Supplier Code...')" Text="Supplier Code..." runat="server" />
                            </div>
                            <div class="col m3 s4">
                                <div class="flex">
                                    <!-- Globalization Tag is added for multilingual  -->
                                    <div>
                                        <asp:TextBox ID="txtAccount" SkinID="txt_Req" runat="server" required="" />
                                        <label><%= GetGlobalResourceObject("Resource", "Account")%></label>
                                        <asp:HiddenField runat="server" ID="hifAccount" Value="0" />
                                        <asp:HiddenField runat="server" ID="hiaccountname" Value="0" />
                                    </div>
                                </div>
                            </div>
                            <div class="col m3 s4">
                                <div class="flex">
                                    <div>
                                    </div>
                                    <div>
                                        <asp:TextBox ID="txtTenant" SkinID="txt_Auto" runat="server" required="" />
                                        <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                                        <asp:HiddenField runat="server" ID="hifTenant" Value="0" />
                                        <asp:HiddenField runat="server" ID="hiTenantName"  />
                                    </div>
                                </div>
                            </div>
                            <div class="col m3 s4">
                                <div class="flex">
                                    <div>
                                        <asp:TextBox ID="txtSupplier" SkinID="txt_Auto" runat="server" required="" />
                                        <label><%= GetGlobalResourceObject("Resource", "SupplierName")%></label>
                                         <input type="hidden" id="hifSupplier" />
                                    </div>
                                </div>
                            </div>
                            <div class="col m3 s12" >
                                    <gap5></gap5>
                                  <flex>  <asp:LinkButton ID="lnkGetData" CssClass="btn btn-primary" runat="server" OnClick="lnkGetData_Click">
                              <%= GetGlobalResourceObject("Resource", "Search")%>  <i class="material-icons vl">search</i>
                                    </asp:LinkButton>
                               
                                    <asp:LinkButton ID="lnkAdd" runat="server" CssClass="btn btn-primary" PostBackUrl="~/mMaterialManagement/SupplierRequest.aspx">  <%= GetGlobalResourceObject("Resource", "Add")%><i class="material-icons vl">add</i></asp:LinkButton>
                               
                                    <%-- <asp:ImageButton ID="btnLiveStockEE" runat="server" ImageAlign="AbsMiddle"  ImageUrl="~/Images/excel_icon.jpg"  OnClick="btnLiveStockEE_Click" ToolTip="Export To Excel" />--%>
                                    <asp:LinkButton ID="btnLiveStockE" runat="server" CssClass="btn btn-primary" OnClick="btnLiveStockEE_Click"><%= GetGlobalResourceObject("Resource", "Export")%> <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                                </flex>
                            </div>

                        </div>
                    </asp:Panel>
                </div>


            </div>

            <div class="row">
                <div class="col m3 offset-m6">
                    <asp:FileUpload runat="server" ID="FUSupImportExcel" CssClass="custom-file-input" onchange="return checkFileExtension(this);" />
                </div>
                <div class="col m3">
                    <flex><asp:LinkButton  runat="server" ID="lnkflupldImportExcel" CssClass="btn btn-primary" OnClick="lnkflupldImportExcel_Click"> <%= GetGlobalResourceObject("Resource", "Import")%><i class="space fa fa-file-excel-o "></i></asp:LinkButton>
                    <a id="btnSample" style="width:139px !important" class="btn btn-primary" href="SampleTemplateForMaterial/SupplierFile.xlsx" runat="server"><%= GetGlobalResourceObject("Resource", "SampleTemplate")%><i class="material-icons">file_download</i></a></flex>
                </div>
            </div>
          

            <div class="row">
                <div colspan="4">
                    <asp:GridView SkinID="gvLightOrangeNew" ID="gvManList" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" PagerSettings-Position="Bottom" AllowPaging="true" PageSize="25"
                        OnPageIndexChanging="gvManList_PageIndexChanging" AllowSorting="True" OnRowDeleting="gvManList_RowDeleting" OnRowDataBound="gvManList_RowDataBound">

                        <Columns>
                            <asp:TemplateField ItemStyle-Width="270" HeaderText="<%$Resources:Resource,Account%>">
                                <ItemTemplate>
                                    <%-- <div title='<%#Eval("Account") %>'> <%#Eval("AccountCode") %>   </div>--%>
                                    <asp:Label ID="lblaccountn" runat="server" Text='<%# Bind("AccountCode") %>' ToolTip='<%# Bind("Account") %>'></asp:Label>
                                    <%-- <asp:Literal runat="server" ID="ltAccount" Text='<%# DataBinder.Eval(Container.DataItem, "Account") %>'/>--%>
                                    <asp:Literal runat="server" ID="ltAccountID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "AccountID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- <asp:BoundField DataField="TenantCode" SortExpression='<%= GetGlobalResourceObject("Resource", "Tenant")%>' HeaderText='Tenant'
                            ItemStyle-Width="20%">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>--%>

                            <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,Tenant%>">
                                <ItemTemplate>
                                    <%--  <div title='<%#Eval("TenantName") %>'> <%#Eval("TenantCode") %>   </div>--%>
                                    <asp:Label ID="lbltenantn" runat="server" Text='<%# Bind("TenantCode") %>' ToolTip='<%# Bind("TenantName") %>'></asp:Label>
                                    <%-- <asp:Literal runat="server" ID="ltTenantName"  Text='<%# DataBinder.Eval(Container.DataItem, "TenantName").ToString() %>'/>--%>
                                </ItemTemplate>
                            </asp:TemplateField>

                                   
                           <%-- Supplier Name added by lalitha on 23/02/2019--%>
                              <asp:TemplateField ItemStyle-Width="270" HeaderText="<%$Resources:Resource,SupplierName%>">
                                <ItemTemplate>
                                  
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("SupplierName") %>' ToolTip='<%# Bind("SupplierName") %>'></asp:Label>
                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                             
                           <%-- Supplier Name added by lalitha on 23/02/2019--%>

                            <asp:TemplateField ItemStyle-Width="270" HeaderText="<%$Resources:Resource,SupplierCode%>">
                                <ItemTemplate>
                                    <%-- <div title='<%#Eval("SupplierName") %>'> <%#Eval("SupplierCode") %>   </div>--%>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("SupplierCode") %>' ToolTip='<%# Bind("SupplierCode") %>'></asp:Label>
                                    <%-- <asp:Literal runat="server" ID="ltSupplier" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>'/>--%>
                                    <asp:Literal runat="server" ID="ltSupplierID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                           

                          <%--  <asp:TemplateField ItemStyle-Width="130" HeaderText="Supplier Code" Visible="false">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltSupplierCode" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierCode").ToString() %>' />
                                </ItemTemplate>
                            </asp:TemplateField>--%>




                            <asp:TemplateField HeaderText="Approved" ItemStyle-CssClass="txtCenteralign" Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkIsApproved" runat="server" Checked='<%#  GetBool(DataBinder.Eval(Container.DataItem, "IsApproved").ToString())%>' />
                                </ItemTemplate>

                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkUpdateIsApproved" CssClass="ui-button-large ui-btn" runat="server" Text="<i class='material-icons vl'>update</i>Update" Font-Underline="false" OnClick="lnkUpdateIsApproved_Click" OnClientClick="return confirmMsg()" />
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Active" Visible="false" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="txtCenteralign" ItemStyle-CssClass="txtCenteralign">
                                <ItemTemplate>
                                    <div class="checkbox chkIsActive">
                                        <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%#GetBool(DataBinder.Eval(Container.DataItem, "IsActive").ToString())%>' />
                                        <label for="chkIsActive"></label>
                                        <%-- <asp:Literal runat="server" for="chkIsActive"></asp:Literal>--%>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkUpdateIsIsActive" CssClass="ui-button-large ui-btn" runat="server" Text="<i class='material-icons vl'>update</i>Update" Font-Underline="false" OnClick="lnkUpdateIsIsActive_Click" OnClientClick="return confirmMsg()" />
                                </FooterTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField ItemStyle-Width="80" ItemStyle-CssClass="txtCenteralign" HeaderText="<%$Resources:Resource,Edit%>" HeaderStyle-CssClass="txtCenteralign">
                                <ItemTemplate>
                                    <a style="text-decoration: none;" href='<%# String.Concat("SupplierRequest.aspx?mfgid=",DataBinder.Eval(Container.DataItem, "SupplierID").ToString()) %>'><i class="material-icons ss">mode_edit</i></a>

                                </ItemTemplate>

                            </asp:TemplateField>





                        </Columns>
                        <EmptyDataTemplate>

                            <div align="center">No Data Found</div>

                        </EmptyDataTemplate>

                    </asp:GridView>
                    <br />
                </div>
            </div>

        </div>
    </div>

    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <style>
        .txtCenteralign {
            text-align: center !important;
        }
    </style>
    <%--<asp:PlaceHolder id="dontCare" runat="server">


   </asp:PlaceHolder>

<br />--%>


    <%-- <table align="center" class="ListTable" width="90%">
                    
         <tr class="ListHeaderRow">
                     
            <td class="FormLabels" align="right">

                <asp:Panel runat="server" DefaultButton="lnkGetData" ID="pnlSupplierList">
                <asp:TextBox id="txtSupplierCode" Visible="false" onblur="javascript:focuslost(this,'Supplier Code...')" onfocus="ClearText(this,'Supplier Code...')" Text="Supplier Code..." runat ="server" Width ="100" />
                &nbsp;&nbsp;&nbsp;

                    <asp:TextBox ID="txtTenant" SkinID="txt_Req" runat="server" onblur="javascript:focuslost(this,'Tenant...')" onfocus="ClearText(this,'Tenant...')" Text="Tenant..."  Width="200" />
                    <asp:HiddenField runat="server" ID="hifTenant" Value="0"/>
                &nbsp;&nbsp;

                <asp:TextBox ID="txtSupplier" SkinID="txt_Req" runat="server" onblur="javascript:focuslost(this,'Supplier Name...')" onfocus="ClearText(this,'Supplier Name...')" Text="Supplier Name..."  Width="200" />
                    &nbsp;&nbsp;
                <asp:LinkButton  ID="lnkGetData" CssClass="btn btn-primary" runat="server" OnClick="lnkGetData_Click" >
                    Search <span class="space fa fa-search"></span>
                </asp:LinkButton>

                &nbsp;&nbsp;
                    <asp:ImageButton ID="btnLiveStockEE" runat="server" ImageAlign="AbsMiddle"  ImageUrl="~/Images/excel_icon.jpg" Width="20" OnClick="btnLiveStockEE_Click" ToolTip="Export To Excel" /><br />
                </asp:Panel>           
           </td>

       </tr>
                    
        <tr class="ListDataRow">
                            
			<td >
    			<asp:Label ID="lblGroupLabelStatus"  runat="server" CssClass="ErrorMsg" />	 						
    			<asp:literal id="lblstatus"  runat="server" /><br />

                <asp:GridView SkinID="gvLightOrangeNew"    ID="gvManList" runat="server"  PagerSettings-Position="TopAndBottom"  AllowPaging="true" PageSize="50" AllowSorting="True"  HorizontalAlign="Left"   
                OnSorting="gvManList_Sorting" OnPageIndexChanging="gvManList_PageIndexChanging" OnRowDataBound="gvManList_RowDataBound" >
                    <Columns>

                        <asp:TemplateField ItemStyle-Width="270" HeaderText="Supplier Name"   >
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltSupplier" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>'/>
                                <asp:Literal runat="server" ID="ltSupplierID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierID") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>

                            <asp:TemplateField ItemStyle-Width="130" HeaderText="Supplier Code"  >
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltSupplierCode" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierCode").ToString() %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="200" HeaderText="Tenant"  >
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltTenantName"  Text='<%# DataBinder.Eval(Container.DataItem, "TenantName").ToString() %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                                                
                            <asp:TemplateField ItemStyle-Width="120" HeaderText="Requested By"  >
                            <ItemTemplate>
                                <asp:Literal ID="lthidRequestedBy" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RequestedBy") %>' />
                                <asp:Literal runat="server" ID="ltRequestedBy" Text='<%# DataBinder.Eval(Container.DataItem, "RequestedByName").ToString() %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>

                            <asp:TemplateField  HeaderText="Approved"  ItemStyle-CssClass="txtCenteralign">
                            <ItemTemplate>
                            <asp:CheckBox ID="chkIsApproved" runat="server" Checked='<%#  GetBool(DataBinder.Eval(Container.DataItem, "IsApproved").ToString())%>' />
                            </ItemTemplate>
                                                   
                            <FooterTemplate>
                            <asp:LinkButton ID="lnkUpdateIsApproved" runat="server" Text="Update" Font-Underline="false" OnClick="lnkUpdateIsApproved_Click" OnClientClick="return confirmMsg()"  />
                            </FooterTemplate>
                            </asp:TemplateField>  
                                                    
                            <asp:TemplateField HeaderText ="Active" ItemStyle-CssClass ="txtCenteralign">
                            <ItemTemplate>
                            <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%#  GetBool(DataBinder.Eval(Container.DataItem, "IsActive").ToString())%>' />
                            </ItemTemplate>
                                <FooterTemplate>
                            <asp:LinkButton ID="lnkUpdateIsIsActive" runat="server" Text="Update" Font-Underline="false" OnClick="lnkUpdateIsIsActive_Click" OnClientClick="return confirmMsg()"  />
                            </FooterTemplate>                                                   
                            </asp:TemplateField>
                                                   
                                                   
                        <asp:TemplateField ItemStyle-Width="80">
                            <ItemTemplate>
                                <a style="text-decoration:none;" href='<%# String.Concat("SupplierRequest.aspx?mfgid=",DataBinder.Eval(Container.DataItem, "SupplierID").ToString()) %>'>Edit  <image src="../Images/redarrowright.gif"></image></a>
                                                        
                            </ItemTemplate>
                                                     
                        </asp:TemplateField>
                                                   
                                               
                                                   
                                                  
                                                
                    </Columns>
                    <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;"
                        Mode="NumericFirstLast" PageButtonCount="15" />
                </asp:GridView>

                                		  
			</td>
							
		</tr>
          
				
                </table>--%>
</asp:Content>
